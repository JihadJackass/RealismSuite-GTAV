using System;
using System.Collections.Generic;
using System.Drawing;
using CayoPericoHeistInSP;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using SinglePlayerHeists.Freeform;
using SinglePlayerHeists.HitNRun;
using SinglePlayerHeists.UtilsMisc;
using SmartAIGuards;

namespace SinglePlayerHeists
{
	public class Class1 : Script
	{
		public enum Nodetype
		{
			AnyRoad,
			Road,
			Offroad,
			Water
		}

		public class ButtonData
		{
			public int ButtonID { get; set; }

			public string DisplayName { get; set; }

			public string KeyboardKey { get; set; }

			public string ControllerKey { get; set; }

			public ButtonData()
			{
			}

			public ButtonData(int bid, string dn, string kk, string ck)
			{
				ButtonID = bid;
				DisplayName = dn;
				KeyboardKey = kk;
				ControllerKey = ck;
			}
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

		public enum ConeColor
		{
			White = 1,
			Black = 2,
			LightGray = 3,
			DarkGray = 5,
			Red = 6,
			LightRed = 7,
			DarkRed = 8,
			Blue = 9,
			LightBlue = 10,
			DarkBlue = 11,
			Yellow = 12,
			LightYellow = 13,
			DarkYellow = 14,
			Orange = 15,
			LightOrange = 16,
			DarkOrange = 17,
			Green = 18,
			LightGreen = 19,
			DarkGreen = 20,
			Purple = 21,
			LightPurple = 22,
			DarkPurple = 23,
			Pink = 24
		}

		public enum HeistStage
		{
			None,
			Stage1,
			Stage2,
			Stage3,
			Stage4,
			Stage5,
			Stage6,
			Stage7,
			Stage8,
			Stage9,
			Stage10,
			Stage11,
			Stage12
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

		public enum LootType
		{
			Trolly_Cash,
			Trolly_Gold,
			Trolly_Diamonds,
			Trolly_Coke,
			CP_Cash,
			CP_Gold,
			CP_Safe,
			GlassCut_Cabinet,
			SafetyDepositBoxes,
			LootBox_Plates,
			LootBox_HDD,
			WoodCrate,
			Server_HDD,
			TreasureChest,
			JewelStoreCabinet,
			Painting
		}

		public class WoodenContainer
		{
			public enum Loot
			{
				Gold
			}

			public enum ObjectType
			{
				Ped,
				Prop,
				Vehicle
			}

			public MissionOjects SilverCrate = new MissionOjects(new List<ObjectPlacementMap>
			{
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_shelve_crt_01a"), 675111, 1778979720u, new Vector3(663.6606f, -2267.20557f, 2.52456641f), new Vector3(1.19358049E-07f, 2.95124742E-08f, -95.56211f), 287767, 0, new Vector3(0f, 0f, 0.8399995f), new Vector3(0f, 0f, 0f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_office_swag_silver2"), 601128, 1105678628u, new Vector3(663.748047f, -2266.4104f, 2.54456639f), new Vector3(-2.95124742E-08f, 1.19358049E-07f, 174.437866f), 287767, 0, new Vector3(-0.7999996f, 0.0100003555f, 0.8599995f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_office_swag_silver2"), 4118, 1105678628u, new Vector3(663.594f, -2267.993f, 2.54456663f), new Vector3(-2.95124742E-08f, 1.19358049E-07f, 174.437866f), 287767, 0, new Vector3(0.7899996f, 0.0100000016f, 0.859999537f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_office_swag_silver2"), 239729, 1105678628u, new Vector3(663.632751f, -2267.59473f, 2.54456663f), new Vector3(2.95125187E-08f, -1.19358049E-07f, -5.562142f), 287767, 0, new Vector3(0.389999956f, 0.0100000016f, 0.859999537f), new Vector3(0f, 0f, 89.9999847f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_office_swag_silver2"), 242294, 1105678628u, new Vector3(663.670532f, -2267.20654f, 2.54456663f), new Vector3(-2.95124742E-08f, 1.19358049E-07f, 174.437866f), 287767, 0, new Vector3(0f, 0.0100000016f, 0.859999537f), new Vector3(0f, 0f, -90.00001f)),
				new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "ex_office_swag_silver2"), 625163, 1105678628u, new Vector3(663.7093f, -2266.80835f, 2.54456663f), new Vector3(2.95125187E-08f, -1.19358049E-07f, -5.562142f), 287767, 0, new Vector3(-0.399999946f, 0.0100000016f, 0.859999537f), new Vector3(0f, 0f, 89.9999847f))
			});

			public List<ObjectPlacementMap> Props = new List<ObjectPlacementMap>();

			public int RobLootStage { get; set; }

			public Prop CrowBar { get; set; }

			public int SyncScene { get; set; }

			public int SyncScene2 { get; set; }

			public Prop LootProp1 { get; set; }

			public Prop BagProp { get; set; }

			public Prop LootProp2 { get; set; }

			public Prop LootBox { get; set; }

			public bool HasRetrievedSupplies { get; set; }

			public Blip Radius { get; set; }

			public Blip DeliveryPoint { get; set; }

			public Vector3 Location { get; set; }

			public Prop MainProp { get; set; }

			public Loot LootType { get; set; }

			public WoodenContainer()
			{
			}

			public Model RequestModel(int Name)
			{
				Model result = new Model(Name);
				result.Request(50);
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

			public WoodenContainer(Prop Lootbox, Loot Type)
			{
				RobLootStage = 10;
				LootType = Type;
				LootBox = Lootbox;
				Location = Lootbox.Position;
			}
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

			public KeypadClass(Vector3 L, float R, int T)
			{
				SpawnedKeypad = false;
				KeypadRot = R;
				KeypadPos = L;
				Type = T;
			}
		}

		public enum UnionDPStage
		{
			ApproachOrScout,
			InVault,
			BlowingGrate,
			RetrievingC4,
			RetrievingBugstarGear,
			RetrievingStockade,
			BlowingWall,
			InUnionDepositoryLobby,
			InVaultAfterLobby,
			EnteringAsBugstars,
			EnteringAsGrouppe6,
			EnteringAsBugstarsWalkingToElv,
			EnteringAsGrouppe6WalkingToElv,
			DrivingCutter,
			UsedCutter,
			EscapingCops,
			WalkingToElevator,
			WalkingToVault,
			EnteringViaHeli
		}

		public enum ApproachStyle
		{
			NoneScouting,
			FrontEnterance,
			EnteringAsBugstars,
			EnteringAsGrouppe6,
			EnteringViaCutterOrBomb
		}

		public enum ApproachToolTip
		{
			NoneScouting,
			FrontEnterance,
			EnteringAsBugstars,
			EnteringAsGrouppe6,
			EnteringAsGrouppe6ORBugstars,
			EnteringViaCutter,
			NearCutterTooltip,
			ExitingViaHeli,
			ExitingViaC4,
			ExitingViaGrateAndHeli,
			NearGruppe6Clothes,
			EnteringHeli,
			HeliRide,
			PayForGetAwayCar,
			SearchPartyIncoming
		}

		public enum ObjectType
		{
			Ped,
			Prop,
			Vehicle
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

			public ObjectPlacementMap(string M, Vector3 P, Vector3 R)
			{
				Hash = Function.Call<int>(GTA.Native.Hash.GET_HASH_KEY, M);
				Position = P;
				Rotation = R;
			}
		}

		public class Vector4
		{
			public float X { get; set; }

			public float Y { get; set; }

			public float Z { get; set; }

			public float H { get; set; }

			public Vector4(float x, float y, float z, float h)
			{
				X = x;
				Y = y;
				Z = z;
				H = h;
			}
		}

		public class Door
		{
			public string Model { get; set; }

			public Vector3 Location { get; set; }

			public Door()
			{
			}

			public Door(string M, Vector3 P)
			{
				Model = M;
				Location = P;
			}
		}

		public class Activity
		{
			public List<Prop> Props = new List<Prop>();

			public bool CreatedCampFire;

			public Prop Camp { get; set; }

			public Vector3 Location { get; set; }

			public Main.HeistID Id { get; set; }

			public string Name { get; set; }

			public int BlipId { get; set; }

			public float Heading { get; set; }

			public Ped HeliPilot { get; set; }

			public Vehicle Heli { get; set; }

			public bool CreatedHeli { get; set; }

			public Blip Blip { get; set; }

			public bool NotReady { get; set; }

			public Activity()
			{
			}

			public Activity(Vector3 L, Main.HeistID id, string N, int BID)
			{
				Location = L;
				Id = id;
				Name = N;
				BlipId = BID;
			}

			public Activity(Vector3 L, Main.HeistID id, string N, int BID, bool unk)
			{
				Location = L;
				Id = id;
				Name = N;
				BlipId = BID;
				NotReady = unk;
			}

			public Activity(Vector4 L, Main.HeistID id, string N, int BID)
			{
				Location = new Vector3(L.X, L.Y, L.Z);
				Heading = L.H;
				Id = id;
				Name = N;
				BlipId = BID;
			}
		}

		public enum HeistIn
		{
			None,
			Fleeca,
			Jewel,
			Paleto,
			PacStandard,
			BikerClubhouse,
			LaFuetaBlanca,
			UnionDP
		}

		public static int RunBikerClubhouseHeist = 0;

		public static int RunFleeca = 0;

		public static int CurrentFleecaBank = -1;

		public static int RunJewelryStore = 0;

		public static int RunLaFauenteBlanca = 0;

		public static int RunPacificStandard = 0;

		public static int RunPaleto = 0;

		public static int RunUnionDepositoryHeist = 0;

		public static int CheckDoorsTick;

		public static int Key_ActionYes_Keyboard = 246;

		public static int Key_ActionNo_Keyboard = 249;

		public static int Key_ActionYes_Controller = 23;

		public static int Key_ActionNo_Controller = 45;

		public static int PriorityLoot = 1;

		public static int timer;

		public static Color ActivityMarkerColor = Color.Goldenrod;

		public static VehicleSeat DefaultSeat = VehicleSeat.Passenger;

		public static int HeistPathfindVariation = 1;

		public static bool PlayingAnim;

		public static int Key_HeistGrabLoot = 51;

		public string SelectedButtonIndex;

		public List<ButtonData> ButtonIndex = new List<ButtonData>
		{
			new ButtonData(0, "INPUT_NEXT_CAMERA", "V", "BACK"),
			new ButtonData(1, "INPUT_LOOK_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(2, "INPUT_LOOK_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(3, "INPUT_LOOK_UP_ONLY", "(NONE)", "RIGHT STICK"),
			new ButtonData(4, "INPUT_LOOK_DOWN_ONLY", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(5, "INPUT_LOOK_LEFT_ONLY", "(NONE)", "RIGHT STICK"),
			new ButtonData(6, "INPUT_LOOK_RIGHT_ONLY", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(7, "INPUT_CINEMATIC_SLOWMO", "(NONE)", "R3"),
			new ButtonData(8, "INPUT_SCRIPTED_FLY_UD", "S", "LEFT STICK"),
			new ButtonData(9, "INPUT_SCRIPTED_FLY_LR", "D", "LEFT STICK"),
			new ButtonData(10, "INPUT_SCRIPTED_FLY_ZUP", "PAGEUP", "LT"),
			new ButtonData(11, "INPUT_SCRIPTED_FLY_ZDOWN", "PAGEDOWN", "RT"),
			new ButtonData(12, "INPUT_WEAPON_WHEEL_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(13, "INPUT_WEAPON_WHEEL_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(14, "INPUT_WEAPON_WHEEL_NEXT", "SCROLLWHEEL DOWN", "DPAD RIGHT"),
			new ButtonData(15, "INPUT_WEAPON_WHEEL_PREV", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(16, "INPUT_SELECT_NEXT_WEAPON", "SCROLLWHEEL", "DOWN (NONE)"),
			new ButtonData(17, "INPUT_SELECT_PREV_WEAPON", "SCROLLWHEEL", "UP (NONE)"),
			new ButtonData(18, "INPUT_SKIP_CUTSCENE", "ENTER / LEFT MOUSE BUTTON/SPACEBAR", "A"),
			new ButtonData(19, "INPUT_CHARACTER_WHEEL", "LEFT ALT", "DPAD DOWN"),
			new ButtonData(20, "INPUT_MULTIPLAYER_INFO", "Z", "DPAD DOWN"),
			new ButtonData(21, "INPUT_SPRINT", "LEFT", "SHIFT A"),
			new ButtonData(22, "INPUT_JUMP", "SPACEBAR", "X"),
			new ButtonData(23, "INPUT_ENTER", "F", "Y"),
			new ButtonData(24, "INPUT_ATTACK", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(25, "INPUT_AIM", "RIGHT MOUSE", "BUTTON LT"),
			new ButtonData(26, "INPUT_LOOK_BEHIND", "C", "R3"),
			new ButtonData(27, "INPUT_PHONE", "ARROW UP / SCROLLWHEEL BUTTON(PRESS)DPAD", "UP"),
			new ButtonData(28, "INPUT_SPECIAL_ABILITY", "(NONE)", "L3"),
			new ButtonData(29, "INPUT_SPECIAL_ABILITY_SECONDARY", "B", "R3"),
			new ButtonData(30, "INPUT_MOVE_LR", "D", "LEFT STICK"),
			new ButtonData(31, "INPUT_MOVE_UD", "S", "LEFT STICK"),
			new ButtonData(32, "INPUT_MOVE_UP_ONLY", "W", "LEFT STICK"),
			new ButtonData(33, "INPUT_MOVE_DOWN_ONLY", "S", "LEFT STICK"),
			new ButtonData(34, "INPUT_MOVE_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(35, "INPUT_MOVE_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(36, "INPUT_DUCK", "LEFT", "CTRL L3"),
			new ButtonData(37, "INPUT_SELECT_WEAPON", "TAB", "LB"),
			new ButtonData(38, "INPUT_PICKUP", "E", "LB"),
			new ButtonData(39, "INPUT_SNIPER_ZOOM", "[LEFT", "STICK"),
			new ButtonData(40, "INPUT_SNIPER_ZOOM_IN_ONLY]", "LEFT", "STICK"),
			new ButtonData(41, "INPUT_SNIPER_ZOOM_OUT_ONLY", "[LEFT", "STICK"),
			new ButtonData(42, "INPUT_SNIPER_ZOOM_IN_SECONDARY]", "DPAD", "UP"),
			new ButtonData(43, "INPUT_SNIPER_ZOOM_OUT_SECONDARY", "[DPAD", "DOWN"),
			new ButtonData(44, "INPUT_COVER", "Q", "RB"),
			new ButtonData(45, "INPUT_RELOAD", "R", "B"),
			new ButtonData(46, "INPUT_TALK", "E", "DPAD RIGHT"),
			new ButtonData(47, "INPUT_DETONATE", "G", "DPAD LEFT"),
			new ButtonData(48, "INPUT_HUD_SPECIAL", "Z", "DPAD DOWN"),
			new ButtonData(49, "INPUT_ARREST", "F", "Y"),
			new ButtonData(50, "INPUT_ACCURATE_AIM", "SCROLLWHEEL", "DOWN R3"),
			new ButtonData(51, "INPUT_CONTEXT", "E", "DPAD RIGHT"),
			new ButtonData(52, "INPUT_CONTEXT_SECONDARY", "Q", "DPAD LEFT"),
			new ButtonData(53, "INPUT_WEAPON_SPECIAL", "(NONE)", "Y"),
			new ButtonData(54, "INPUT_WEAPON_SPECIAL_TWO", "E", "DPAD RIGHT"),
			new ButtonData(55, "INPUT_DIVE", "SPACEBAR", "RB"),
			new ButtonData(56, "INPUT_DROP_WEAPON", "F9", "Y"),
			new ButtonData(57, "INPUT_DROP_AMMO", "F10", "B"),
			new ButtonData(58, "INPUT_THROW_GRENADE", "G", "DPAD LEFT"),
			new ButtonData(59, "INPUT_VEH_MOVE_LR", "D", "LEFT STICK"),
			new ButtonData(60, "INPUT_VEH_MOVE_UD", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(61, "INPUT_VEH_MOVE_UP_ONLY", "LEFT SHIFT", "LEFT STICK"),
			new ButtonData(62, "INPUT_VEH_MOVE_DOWN_ONLY", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(63, "INPUT_VEH_MOVE_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(64, "INPUT_VEH_MOVE_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(65, "INPUT_VEH_SPECIAL", "(NONE)", "(NONE)"),
			new ButtonData(66, "INPUT_VEH_GUN_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(67, "INPUT_VEH_GUN_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(68, "INPUT_VEH_AIM", "RIGHT MOUSE", "BUTTON LB"),
			new ButtonData(69, "INPUT_VEH_ATTACK", "LEFT MOUSE", "BUTTON RB"),
			new ButtonData(70, "INPUT_VEH_ATTACK2", "RIGHT MOUSE", "BUTTON A"),
			new ButtonData(71, "INPUT_VEH_ACCELERATE", "W", "RT"),
			new ButtonData(72, "INPUT_VEH_BRAKE", "S", "LT"),
			new ButtonData(73, "INPUT_VEH_DUCK", "X", "A"),
			new ButtonData(74, "INPUT_VEH_HEADLIGHT", "H", "DPAD RIGHT"),
			new ButtonData(75, "INPUT_VEH_EXIT", "F", "Y"),
			new ButtonData(76, "INPUT_VEH_HANDBRAKE", "SPACEBAR", "RB"),
			new ButtonData(77, "INPUT_VEH_HOTWIRE_LEFT", "W", "LT"),
			new ButtonData(78, "INPUT_VEH_HOTWIRE_RIGHT", "S", "RT"),
			new ButtonData(79, "INPUT_VEH_LOOK_BEHIND", "C", "R3"),
			new ButtonData(80, "INPUT_VEH_CIN_CAM", "R", "B"),
			new ButtonData(81, "INPUT_VEH_NEXT_RADIO", ".", "(NONE)"),
			new ButtonData(82, "INPUT_VEH_PREV_RADIO", ",", "(NONE)"),
			new ButtonData(83, "INPUT_VEH_NEXT_RADIO_TRACK", "=", "(NONE)"),
			new ButtonData(84, "INPUT_VEH_PREV_RADIO_TRACK", "-", "(NONE)"),
			new ButtonData(85, "INPUT_VEH_RADIO_WHEEL", "Q", "DPAD LEFT"),
			new ButtonData(86, "INPUT_VEH_HORN", "E", "L3"),
			new ButtonData(87, "INPUT_VEH_FLY_THROTTLE_UP", "W", "RT"),
			new ButtonData(88, "INPUT_VEH_FLY_THROTTLE_DOWN", "S", "LT"),
			new ButtonData(89, "INPUT_VEH_FLY_YAW_LEFT", "A", "LB"),
			new ButtonData(90, "INPUT_VEH_FLY_YAW_RIGHT", "D", "RB"),
			new ButtonData(91, "INPUT_VEH_PASSENGER_AIM", "RIGHT MOUSE", "BUTTON LT"),
			new ButtonData(92, "INPUT_VEH_PASSENGER_ATTACK", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(93, "INPUT_VEH_SPECIAL_ABILITY_FRANKLIN", "(NONE)", "R3"),
			new ButtonData(94, "INPUT_VEH_STUNT_UD", "(NONE)", "(NONE)"),
			new ButtonData(95, "INPUT_VEH_CINEMATIC_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(96, "INPUT_VEH_CINEMATIC_UP_ONLY", "NUMPAD- /", "SCROLLWHEEL UP(NONE)"),
			new ButtonData(97, "INPUT_VEH_CINEMATIC_DOWN_ONLY", "NUMPAD+ /", "SCROLLWHEEL DOWN(NONE)"),
			new ButtonData(98, "INPUT_VEH_CINEMATIC_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(99, "INPUT_VEH_SELECT_NEXT_WEAPON", "SCROLLWHEEL", "UP X"),
			new ButtonData(100, "INPUT_VEH_SELECT_PREV_WEAPON", "[", "(NONE)"),
			new ButtonData(101, "INPUT_VEH_ROOF", "H", "DPAD RIGHT"),
			new ButtonData(102, "INPUT_VEH_JUMP", "SPACEBAR", "RB"),
			new ButtonData(103, "INPUT_VEH_GRAPPLING_HOOK", "E", "DPAD RIGHT"),
			new ButtonData(104, "INPUT_VEH_SHUFFLE", "H", "DPAD RIGHT"),
			new ButtonData(105, "INPUT_VEH_DROP_PROJECTILE", "X", "A"),
			new ButtonData(106, "INPUT_VEH_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(107, "INPUT_VEH_FLY_ROLL_LR", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(108, "INPUT_VEH_FLY_ROLL_LEFT_ONLY", "NUMPAD 4", "LEFT STICK"),
			new ButtonData(109, "INPUT_VEH_FLY_ROLL_RIGHT_ONLY", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(110, "INPUT_VEH_FLY_PITCH_UD", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(111, "INPUT_VEH_FLY_PITCH_UP_ONLY", "NUMPAD 8", "LEFT STICK"),
			new ButtonData(112, "INPUT_VEH_FLY_PITCH_DOWN_ONLY", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(113, "INPUT_VEH_FLY_UNDERCARRIAGE", "G", "L3"),
			new ButtonData(114, "INPUT_VEH_FLY_ATTACK", "RIGHT MOUSE", "BUTTON A"),
			new ButtonData(115, "INPUT_VEH_FLY_SELECT_NEXT_WEAPON", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(116, "INPUT_VEH_FLY_SELECT_PREV_WEAPON", "[", "(NONE)"),
			new ButtonData(117, "INPUT_VEH_FLY_SELECT_TARGET_LEFT", "NUMPAD", "7 LB"),
			new ButtonData(118, "INPUT_VEH_FLY_SELECT_TARGET_RIGHT", "NUMPAD", "9 RB"),
			new ButtonData(119, "INPUT_VEH_FLY_VERTICAL_FLIGHT_MODE", "E", "DPAD RIGHT"),
			new ButtonData(120, "INPUT_VEH_FLY_DUCK", "X", "A"),
			new ButtonData(121, "INPUT_VEH_FLY_ATTACK_CAMERA", "INSERT", "R3"),
			new ButtonData(122, "INPUT_VEH_FLY_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(123, "INPUT_VEH_SUB_TURN_LR", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(124, "INPUT_VEH_SUB_TURN_LEFT_ONLY", "NUMPAD 4", "LEFT STICK"),
			new ButtonData(125, "INPUT_VEH_SUB_TURN_RIGHT_ONLY", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(126, "INPUT_VEH_SUB_PITCH_UD", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(127, "INPUT_VEH_SUB_PITCH_UP_ONLY", "NUMPAD 8", "LEFT STICK"),
			new ButtonData(128, "INPUT_VEH_SUB_PITCH_DOWN_ONLY", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(129, "INPUT_VEH_SUB_THROTTLE_UP", "W", "RT"),
			new ButtonData(130, "INPUT_VEH_SUB_THROTTLE_DOWN", "S", "LT"),
			new ButtonData(131, "INPUT_VEH_SUB_ASCEND", "LEFT", "SHIFT X"),
			new ButtonData(132, "INPUT_VEH_SUB_DESCEND", "LEFT", "CTRL A"),
			new ButtonData(133, "INPUT_VEH_SUB_TURN_HARD_LEFT", "A", "LB"),
			new ButtonData(134, "INPUT_VEH_SUB_TURN_HARD_RIGHT", "D", "RB"),
			new ButtonData(135, "INPUT_VEH_SUB_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(136, "INPUT_VEH_PUSHBIKE_PEDAL", "W", "A"),
			new ButtonData(137, "INPUT_VEH_PUSHBIKE_SPRINT", "CAPSLOCK", "A"),
			new ButtonData(138, "INPUT_VEH_PUSHBIKE_FRONT_BRAKE", "Q", "LT"),
			new ButtonData(139, "INPUT_VEH_PUSHBIKE_REAR_BRAKE", "S", "RT"),
			new ButtonData(140, "INPUT_MELEE_ATTACK_LIGHT", "R", "B"),
			new ButtonData(141, "INPUT_MELEE_ATTACK_HEAVY", "Q", "A"),
			new ButtonData(142, "INPUT_MELEE_ATTACK_ALTERNATE", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(143, "INPUT_MELEE_BLOCK", "SPACEBAR", "X"),
			new ButtonData(144, "INPUT_PARACHUTE_DEPLOY", "F / LEFT MOUSE BUTTON", "Y"),
			new ButtonData(145, "INPUT_PARACHUTE_DETACH", "F", "Y"),
			new ButtonData(146, "INPUT_PARACHUTE_TURN_LR", "D", "LEFT STICK"),
			new ButtonData(147, "INPUT_PARACHUTE_TURN_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(148, "INPUT_PARACHUTE_TURN_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(149, "INPUT_PARACHUTE_PITCH_UD", "S", "LEFT STICK"),
			new ButtonData(150, "INPUT_PARACHUTE_PITCH_UP_ONLY", "W", "LEFT STICK"),
			new ButtonData(151, "INPUT_PARACHUTE_PITCH_DOWN_ONLY", "S", "LEFT STICK"),
			new ButtonData(152, "INPUT_PARACHUTE_BRAKE_LEFT", "Q", "LB"),
			new ButtonData(153, "INPUT_PARACHUTE_BRAKE_RIGHT", "E", "RB"),
			new ButtonData(154, "INPUT_PARACHUTE_SMOKE", "X", "A"),
			new ButtonData(155, "INPUT_PARACHUTE_PRECISION_LANDING", "LEFT", "SHIFT(NONE)"),
			new ButtonData(156, "INPUT_MAP", "(NONE)", "(NONE)"),
			new ButtonData(157, "INPUT_SELECT_WEAPON_UNARMED", "1", "(NONE)"),
			new ButtonData(158, "INPUT_SELECT_WEAPON_MELEE", "2", "(NONE)"),
			new ButtonData(159, "INPUT_SELECT_WEAPON_HANDGUN", "6", "(NONE)"),
			new ButtonData(160, "INPUT_SELECT_WEAPON_SHOTGUN", "3", "(NONE)"),
			new ButtonData(161, "INPUT_SELECT_WEAPON_SMG", "7", "(NONE)"),
			new ButtonData(162, "INPUT_SELECT_WEAPON_AUTO_RIFLE", "8", "(NONE)"),
			new ButtonData(163, "INPUT_SELECT_WEAPON_SNIPER", "9", "(NONE)"),
			new ButtonData(164, "INPUT_SELECT_WEAPON_HEAVY", "4", "(NONE)"),
			new ButtonData(165, "INPUT_SELECT_WEAPON_SPECIAL", "5", "(NONE)"),
			new ButtonData(166, "INPUT_SELECT_CHARACTER_MICHAEL", "F5", "(NONE)"),
			new ButtonData(167, "INPUT_SELECT_CHARACTER_FRANKLIN", "F6", "(NONE)"),
			new ButtonData(168, "INPUT_SELECT_CHARACTER_TREVOR", "F7", "(NONE)"),
			new ButtonData(169, "INPUT_SELECT_CHARACTER_MULTIPLAYER", "F8(CONSOLE)", "(NONE)"),
			new ButtonData(170, "INPUT_SAVE_REPLAY_CLIP", "F3", "B"),
			new ButtonData(171, "INPUT_SPECIAL_ABILITY_PC", "CAPSLOCK", "(NONE)"),
			new ButtonData(172, "INPUT_CELLPHONE_UP", "ARROW UP", "DPAD UP"),
			new ButtonData(173, "INPUT_CELLPHONE_DOWN", "ARROW DOWN", "DPAD DOWN"),
			new ButtonData(174, "INPUT_CELLPHONE_LEFT", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(175, "INPUT_CELLPHONE_RIGHT", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(176, "INPUT_CELLPHONE_SELECT", "ENTER / LEFT MOUSE BUTTON", "A"),
			new ButtonData(177, "INPUT_CELLPHONE_CANCEL", "BACKSPACE / ESC / RIGHTMOUSEBUTTON", "B"),
			new ButtonData(178, "INPUT_CELLPHONE_OPTION", "DELETE", "Y"),
			new ButtonData(179, "INPUT_CELLPHONE_EXTRA_OPTION", "SPACEBAR", "X"),
			new ButtonData(180, "INPUT_CELLPHONE_SCROLL_FORWARD", "SCROLLWHEEL", "DOWN(NONE)"),
			new ButtonData(181, "INPUT_CELLPHONE_SCROLL_BACKWARD", "SCROLLWHEEL", "UP(NONE)"),
			new ButtonData(182, "INPUT_CELLPHONE_CAMERA_FOCUS_LOCK", "L", "RT"),
			new ButtonData(183, "INPUT_CELLPHONE_CAMERA_GRID", "G", "RB"),
			new ButtonData(184, "INPUT_CELLPHONE_CAMERA_SELFIE", "E", "R3"),
			new ButtonData(185, "INPUT_CELLPHONE_CAMERA_DOF", "F", "LB"),
			new ButtonData(186, "INPUT_CELLPHONE_CAMERA_EXPRESSION", "X", "L3"),
			new ButtonData(187, "INPUT_FRONTEND_DOWN", "ARROW DOWN", "DPAD DOWN"),
			new ButtonData(188, "INPUT_FRONTEND_UP", "ARROW UP", "DPAD UP"),
			new ButtonData(189, "INPUT_FRONTEND_LEFT", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(190, "INPUT_FRONTEND_RIGHT", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(191, "INPUT_FRONTEND_RDOWN", "ENTER", "A"),
			new ButtonData(192, "INPUT_FRONTEND_RUP", "TAB", "Y"),
			new ButtonData(193, "INPUT_FRONTEND_RLEFT", "(NONE)", "X"),
			new ButtonData(194, "INPUT_FRONTEND_RRIGHT", "BACKSPACE", "B"),
			new ButtonData(195, "INPUT_FRONTEND_AXIS_X", "D", "LEFT STICK"),
			new ButtonData(196, "INPUT_FRONTEND_AXIS_Y", "S", "LEFT STICK"),
			new ButtonData(197, "INPUT_FRONTEND_RIGHT_AXIS_X]", "RIGHT", "STICK"),
			new ButtonData(198, "INPUT_FRONTEND_RIGHT_AXIS_Y", "SCROLLWHEEL DOWN", "RIGHT STICK"),
			new ButtonData(199, "INPUT_FRONTEND_PAUSE", "P", "START"),
			new ButtonData(200, "INPUT_FRONTEND_PAUSE_ALTERNATE", "ESC", "(NONE)"),
			new ButtonData(201, "INPUT_FRONTEND_ACCEPT", "ENTER / NUMPAD ENTER", "A"),
			new ButtonData(202, "INPUT_FRONTEND_CANCEL", "BACKSPACE /", "ESC B"),
			new ButtonData(203, "INPUT_FRONTEND_X", "SPACEBAR", "X"),
			new ButtonData(204, "INPUT_FRONTEND_Y", "TAB", "Y"),
			new ButtonData(205, "INPUT_FRONTEND_LB", "Q", "LB"),
			new ButtonData(206, "INPUT_FRONTEND_RB", "E", "RB"),
			new ButtonData(207, "INPUT_FRONTEND_LT", "PAGE", "DOWN LT"),
			new ButtonData(208, "INPUT_FRONTEND_RT", "PAGE", "UP RT"),
			new ButtonData(209, "INPUT_FRONTEND_LS", "LEFT", "SHIFT L3"),
			new ButtonData(210, "INPUT_FRONTEND_RS", "LEFT", "CONTROL R3"),
			new ButtonData(211, "INPUT_FRONTEND_LEADERBOARD", "TAB", "RB"),
			new ButtonData(212, "INPUT_FRONTEND_SOCIAL_CLUB", "HOME", "BACK"),
			new ButtonData(213, "INPUT_FRONTEND_SOCIAL_CLUB_SECONDARY", "HOME", "RB"),
			new ButtonData(214, "INPUT_FRONTEND_DELETE", "DELETE", "X"),
			new ButtonData(215, "INPUT_FRONTEND_ENDSCREEN_ACCEPT", "ENTER", "A"),
			new ButtonData(216, "INPUT_FRONTEND_ENDSCREEN_EXPAND", "SPACEBAR", "X"),
			new ButtonData(217, "INPUT_FRONTEND_SELECT", "CAPSLOCK", "BACK"),
			new ButtonData(218, "INPUT_SCRIPT_LEFT_AXIS_X", "D", "LEFT STICK"),
			new ButtonData(219, "INPUT_SCRIPT_LEFT_AXIS_Y", "S", "LEFT STICK"),
			new ButtonData(220, "INPUT_SCRIPT_RIGHT_AXIS_X", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(221, "INPUT_SCRIPT_RIGHT_AXIS_Y", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(222, "INPUT_SCRIPT_RUP", "RIGHT MOUSE", "BUTTON Y"),
			new ButtonData(223, "INPUT_SCRIPT_RDOWN", "LEFT MOUSE", "BUTTON A"),
			new ButtonData(224, "INPUT_SCRIPT_RLEFT", "LEFT", "CTRL X"),
			new ButtonData(225, "INPUT_SCRIPT_RRIGHT", "RIGHT MOUSE", "BUTTON B"),
			new ButtonData(226, "INPUT_SCRIPT_LB", "(NONE)", "LB"),
			new ButtonData(227, "INPUT_SCRIPT_RB", "(NONE)", "LT"),
			new ButtonData(228, "INPUT_SCRIPT_LT", "(NONE)", "LB"),
			new ButtonData(229, "INPUT_SCRIPT_RT", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(230, "INPUT_SCRIPT_LS", "(NONE)", "L3"),
			new ButtonData(231, "INPUT_SCRIPT_RS", "(NONE)", "R3"),
			new ButtonData(232, "INPUT_SCRIPT_PAD_UP", "W", "DPAD UP"),
			new ButtonData(233, "INPUT_SCRIPT_PAD_DOWN", "S", "DPAD DOWN"),
			new ButtonData(234, "INPUT_SCRIPT_PAD_LEFT", "A", "DPAD LEFT"),
			new ButtonData(235, "INPUT_SCRIPT_PAD_RIGHT", "D", "DPAD RIGHT"),
			new ButtonData(236, "INPUT_SCRIPT_SELECT", "V", "BACK"),
			new ButtonData(237, "INPUT_CURSOR_ACCEPT", "LEFT MOUSE", "BUTTON (NONE)"),
			new ButtonData(238, "INPUT_CURSOR_CANCEL", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(239, "INPUT_CURSOR_X", "(NONE)", "(NONE)"),
			new ButtonData(240, "INPUT_CURSOR_Y", "(NONE)", "(NONE)"),
			new ButtonData(241, "INPUT_CURSOR_SCROLL_UP", "SCROLLWHEEL", "UP (NONE)"),
			new ButtonData(242, "INPUT_CURSOR_SCROLL_DOWN", "SCROLLWHEEL", "DOWN (NONE)"),
			new ButtonData(243, "INPUT_ENTER_CHEAT_CODE", "~ /", "` (NONE)"),
			new ButtonData(244, "INPUT_INTERACTION_MENU", "M", "BACK"),
			new ButtonData(245, "INPUT_MP_TEXT_CHAT_ALL", "T", "(NONE)"),
			new ButtonData(246, "INPUT_MP_TEXT_CHAT_TEAM", "Y", "(NONE)"),
			new ButtonData(247, "INPUT_MP_TEXT_CHAT_FRIENDS", "(NONE)", "(NONE)"),
			new ButtonData(248, "INPUT_MP_TEXT_CHAT_CREW", "(NONE)", "(NONE)"),
			new ButtonData(249, "INPUT_PUSH_TO_TALK", "N", "(NONE)"),
			new ButtonData(250, "INPUT_CREATOR_LS", "R", "L3"),
			new ButtonData(251, "INPUT_CREATOR_RS", "F", "R3"),
			new ButtonData(252, "INPUT_CREATOR_LT", "X", "LT"),
			new ButtonData(253, "INPUT_CREATOR_RT", "C", "RT"),
			new ButtonData(254, "INPUT_CREATOR_MENU_TOGGLE", "LEFT", "SHIFT (NONE)"),
			new ButtonData(255, "INPUT_CREATOR_ACCEPT", "SPACEBAR", "A"),
			new ButtonData(256, "INPUT_CREATOR_DELETE", "DELETE", "X"),
			new ButtonData(257, "INPUT_ATTACK2", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(258, "INPUT_RAPPEL_JUMP", "(NONE)", "A"),
			new ButtonData(259, "INPUT_RAPPEL_LONG_JUMP", "(NONE)", "X"),
			new ButtonData(260, "INPUT_RAPPEL_SMASH_WINDOW", "(NONE)", "RT"),
			new ButtonData(261, "INPUT_PREV_WEAPON", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(262, "INPUT_NEXT_WEAPON", "SCROLLWHEEL DOWN", "DPAD RIGHT"),
			new ButtonData(263, "INPUT_MELEE_ATTACK1", "R", "B"),
			new ButtonData(264, "INPUT_MELEE_ATTACK2", "Q", "A"),
			new ButtonData(265, "INPUT_WHISTLE", "(NONE)", "(NONE)"),
			new ButtonData(266, "INPUT_MOVE_LEFT", "D", "LEFT STICK"),
			new ButtonData(267, "INPUT_MOVE_RIGHT", "D", "LEFT STICK"),
			new ButtonData(268, "INPUT_MOVE_UP", "S", "LEFT STICK"),
			new ButtonData(269, "INPUT_MOVE_DOWN", "S", "LEFT STICK"),
			new ButtonData(270, "INPUT_LOOK_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(271, "INPUT_LOOK_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(272, "INPUT_LOOK_UP", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(273, "INPUT_LOOK_DOWN", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(274, "INPUT_SNIPER_ZOOM_IN", "[RIGHT", "STICK"),
			new ButtonData(275, "INPUT_SNIPER_ZOOM_OUT", "[RIGHT", "STICK"),
			new ButtonData(276, "INPUT_SNIPER_ZOOM_IN_ALTERNATE", "[LEFT", "STICK"),
			new ButtonData(277, "INPUT_SNIPER_ZOOM_OUT_ALTERNATE", "[LEFT", "STICK"),
			new ButtonData(278, "INPUT_VEH_MOVE_LEFT", "D", "LEFT STICK"),
			new ButtonData(279, "INPUT_VEH_MOVE_RIGHT", "D", "LEFT STICK"),
			new ButtonData(280, "INPUT_VEH_MOVE_UP", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(281, "INPUT_VEH_MOVE_DOWN", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(282, "INPUT_VEH_GUN_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(283, "INPUT_VEH_GUN_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(284, "INPUT_VEH_GUN_UP", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(285, "INPUT_VEH_GUN_DOWN", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(286, "INPUT_VEH_LOOK_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(287, "INPUT_VEH_LOOK_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(288, "INPUT_REPLAY_START_STOP_RECORDING", "F1", "A"),
			new ButtonData(289, "INPUT_REPLAY_START_STOP_RECORDING_SECONDARY", "F2", "X"),
			new ButtonData(290, "INPUT_SCALED_LOOK_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(291, "INPUT_SCALED_LOOK_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(292, "INPUT_SCALED_LOOK_UP_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(293, "INPUT_SCALED_LOOK_DOWN_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(294, "INPUT_SCALED_LOOK_LEFT_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(295, "INPUT_SCALED_LOOK_RIGHT_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(296, "INPUT_REPLAY_MARKER_DELETE", "DELETE", "X"),
			new ButtonData(297, "INPUT_REPLAY_CLIP_DELETE", "DELETE", "Y"),
			new ButtonData(298, "INPUT_REPLAY_PAUSE", "SPACEBAR", "A"),
			new ButtonData(299, "INPUT_REPLAY_REWIND", "ARROW", "DOWN LB"),
			new ButtonData(300, "INPUT_REPLAY_FFWD", "ARROW", "UP RB"),
			new ButtonData(301, "INPUT_REPLAY_NEWMARKER", "M", "A"),
			new ButtonData(302, "INPUT_REPLAY_RECORD", "S", "(NONE)"),
			new ButtonData(303, "INPUT_REPLAY_SCREENSHOT", "U", "DPAD UP"),
			new ButtonData(304, "INPUT_REPLAY_HIDEHUD", "H", "R3"),
			new ButtonData(307, "INPUT_REPLAY_ADVANCE", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(308, "INPUT_REPLAY_BACK", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(309, "INPUT_REPLAY_TOOLS", "T", "DPAD DOWN"),
			new ButtonData(310, "INPUT_REPLAY_RESTART", "R", "BACK"),
			new ButtonData(311, "INPUT_REPLAY_SHOWHOTKEY", "K", "DPAD DOWN"),
			new ButtonData(312, "INPUT_REPLAY_CYCLEMARKERLEFT", "[DPAD", "LEFT"),
			new ButtonData(313, "INPUT_REPLAY_CYCLEMARKERRIGHT]", "DPAD", "RIGHT"),
			new ButtonData(314, "INPUT_REPLAY_FOVINCREASE", "NUMPAD", "+ RB"),
			new ButtonData(315, "INPUT_REPLAY_FOVDECREASE", "NUMPAD", "- LB"),
			new ButtonData(316, "INPUT_REPLAY_CAMERAUP", "PAGE", "UP (NONE)"),
			new ButtonData(317, "INPUT_REPLAY_CAMERADOWN", "PAGE", "DOWN (NONE)"),
			new ButtonData(318, "INPUT_REPLAY_SAVE", "F5", "START"),
			new ButtonData(319, "INPUT_REPLAY_TOGGLETIME", "C", "(NONE)"),
			new ButtonData(320, "INPUT_REPLAY_TOGGLETIPS", "V", "(NONE)"),
			new ButtonData(321, "INPUT_REPLAY_PREVIEW", "SPACEBAR", "(NONE)"),
			new ButtonData(322, "INPUT_REPLAY_TOGGLE_TIMELINE", "ESC", "(NONE)"),
			new ButtonData(323, "INPUT_REPLAY_TIMELINE_PICKUP_CLIP", "X", "(NONE)"),
			new ButtonData(324, "INPUT_REPLAY_TIMELINE_DUPLICATE_CLIP", "C", "(NONE)"),
			new ButtonData(325, "INPUT_REPLAY_TIMELINE_PLACE_CLIP", "V", "(NONE)"),
			new ButtonData(326, "INPUT_REPLAY_CTRL", "LEFT CTRL", "(NONE)"),
			new ButtonData(327, "INPUT_REPLAY_TIMELINE_SAVE", "F5", "CTRL (NONE)"),
			new ButtonData(328, "INPUT_REPLAY_PREVIEW_AUDIO", "SPACEBAR", "RT"),
			new ButtonData(329, "INPUT_VEH_DRIVE_LOOK", "LEFT MOUSE", "BUTTON (NONE)"),
			new ButtonData(330, "INPUT_VEH_DRIVE_LOOK2", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(331, "INPUT_VEH_FLY_ATTACK2", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(332, "INPUT_RADIO_WHEEL_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(333, "INPUT_RADIO_WHEEL_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(334, "INPUT_VEH_SLOWMO_UD", "SCROLLWHEEL DOWN", "LEFT STICK"),
			new ButtonData(335, "INPUT_VEH_SLOWMO_UP_ONLY", "SCROLLWHEEL UP", "LEFT STICK"),
			new ButtonData(336, "INPUT_VEH_SLOWMO_DOWN_ONLY", "SCROLLWHEEL DOWN", "LEFT STICK"),
			new ButtonData(337, "INPUT_VEH_HYDRAULICS_CONTROL_TOGGLE", "X", "A"),
			new ButtonData(338, "INPUT_VEH_HYDRAULICS_CONTROL_LEFT", "A", "LEFT STICK"),
			new ButtonData(339, "INPUT_VEH_HYDRAULICS_CONTROL_RIGHT", "D", "LEFT STICK"),
			new ButtonData(340, "INPUT_VEH_HYDRAULICS_CONTROL_UP", "LEFT SHIFT", "LEFT STICK"),
			new ButtonData(341, "INPUT_VEH_HYDRAULICS_CONTROL_DOWN", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(342, "INPUT_VEH_HYDRAULICS_CONTROL_UD", "D", "LEFT STICK"),
			new ButtonData(343, "INPUT_VEH_HYDRAULICS_CONTROL_LR", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(344, "INPUT_SWITCH_VISOR", "F11", "DPAD RIGHT"),
			new ButtonData(345, "INPUT_VEH_MELEE_HOLD", "X", "A"),
			new ButtonData(346, "INPUT_VEH_MELEE_LEFT", "LEFT MOUSE", "BUTTON LB"),
			new ButtonData(347, "INPUT_VEH_MELEE_RIGHT", "RIGHT MOUSE", "BUTTON RB"),
			new ButtonData(348, "INPUT_MAP_POI", "SCROLLWHEEL BUTTON", "(PRESS) Y"),
			new ButtonData(349, "INPUT_REPLAY_SNAPMATIC_PHOTO", "TAB", "X"),
			new ButtonData(350, "INPUT_VEH_CAR_JUMP", "E", "L3"),
			new ButtonData(351, "INPUT_VEH_ROCKET_BOOST", "E", "L3"),
			new ButtonData(352, "INPUT_VEH_FLY_BOOST", "LEFT", "SHIFT L3"),
			new ButtonData(353, "INPUT_VEH_PARACHUTE", "SPACEBAR", "A"),
			new ButtonData(354, "INPUT_VEH_BIKE_WINGS", "X", "A"),
			new ButtonData(355, "INPUT_VEH_FLY_BOMB_BAY", "E", "DPAD RIGHT"),
			new ButtonData(356, "INPUT_VEH_FLY_COUNTER", "E", "DPAD RIGHT"),
			new ButtonData(357, "INPUT_VEH_TRANSFORM", "X", "A")
		};

		public static bool PlayingBikerClubhouseHeist;

		public static int Scene1;

		public bool ActiveHack;

		private int hackingScalwform;

		private int uLocal_70;

		private int Hacking_Index = -1;

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

		private List<dynamic> HackingProps = new List<object>
		{
			"hei_prop_hei_securitypanel", "hei_prop_hei_keypad_01", "hei_prop_hei_keypad_02", "hei_prop_hei_keypad_03", "prop_ld_keypad_01", "prop_ld_keypad_01b", "prop_ld_keypad_01b_lod", "hei_prop_hei_cs_keyboard", "prop_cs_keyboard_01", "prop_keyboard_01a",
			"prop_keyboard_01b", "prop_laptop_jimmy", "prop_laptop_lester", "prop_laptop_lester2", "p_amb_lap_top_02", "p_cs_laptop_02", "p_laptop_02_s", "prop_laptop_01a"
		};

		public string DictToUse;

		public static Prop FleecaKeypad;

		public static Prop Laptop;

		public static bool PlayingThermitedoor4;

		public static bool ThermiteDoor4;

		public static string DictToUse2;

		public static Prop _thermiteProp;

		public static bool ThirmiteDoor1;

		public static bool ThirmiteDoor2;

		public static bool ThirmiteDoor3;

		public static Vector3 ThirmiteDoor2Pos = new Vector3(252.9815f, 220.7f, 101.8f);

		public static Vector3 ThirmiteDoor3Pos = new Vector3(261.659f, 215.6567f, 101.8f);

		public static bool PlayeringThirmiteDoor1;

		public static bool PlayeringThirmiteDoor2;

		public static bool PlayeringThirmiteDoor3;

		public static Blip DoorAblip;

		public static Blip DoorBblip;

		public static Blip DoorCblip;

		public static Blip DoorDblip;

		public static bool CreatedBlips;

		public static bool CanNoLongerOverideCommands;

		public static List<Ped> Tellers = new List<Ped>();

		public static Ped Manager;

		public static bool LockedDoors;

		public static bool CivsAreFreakingOut;

		public static bool PacificStandardCivsAndTellerInCower;

		public static int PTFX1;

		public static int PTFX2;

		public static Prop HackCard;

		public static Prop BagProp;

		public static Ped teller;

		public static int Bank_Location;

		public static bool StartDrill;

		public static bool DrillDone;

		public static Prop JammmerProp;

		public static Vector3 FleecaInsurgentSpawn = new Vector3(-1731.9f, 2838f, 33f);

		public static Blip InsurgentBLip;

		public static Vehicle Insurgent;

		public static Vector3 FleecaKurumaSpawn = new Vector3(-13.62394f, -1067.724f, 38f);

		public static Blip KurumaBlip;

		public static Vehicle Kuruma;

		public static string VehicleChoosen;

		public static bool NotifyEnd;

		public static bool GetCameras;

		public static bool OpenedDoor;

		public static int tellerwalkStage = 0;

		public static int TellerScene;

		public static Vehicle GetawayCar;

		public static Vector3 PatrolCarDeletePos = new Vector3(769f, -719f, 28f);

		public static int CopTime;

		public static int TimeTillPaletoVaultOpens;

		public static Vector3 FleecaJammerSpawm = new Vector3(455f, -987f, 25f);

		public static Vector3 FleecaJammerPos = new Vector3(254.3401f, 225.6263f, 100.5f);

		public static int CashCount;

		public static Vector3 MoneyPosition = new Vector3(312.319f, -288.876f, 53.5f);

		public static Vector3 VaultPosition = new Vector3(312.0821f, -283.3166f, 54f);

		public static float OpenAmt = 0.25f;

		public static float CloseAmt = -0.25f;

		public static int OpenValue = -110;

		public static int CloseValue = -179;

		public static Vector3 PeletoBankMarker = new Vector3(-128f, 6443.71f, 29.8f);

		public static int TargetingTime;

		public static Vector3 Offset;

		public static bool DoorALocked;

		public static bool DoorBLocked;

		public static bool FoundKeyCard;

		public static bool DoorCLocked;

		public static bool DoorDLocked;

		public static bool GotKeyCard;

		public static bool HackKeyPadDone;

		public static bool HackedPacificStandardSecurityPad;

		public static Vector3 HackPacficStandardSecurityPadPosition = new Vector3(253.33f, 228.3f, 100.68f);

		public static bool TriggerCops;

		public static bool GotVaultDoor;

		public static Ped CutsenePed1;

		public static Ped CutsenePed2;

		public static Ped CutsenePed3;

		public static Ped CutsenePed4;

		public static bool PlayingCutscene;

		public static Prop jammerPartA;

		public static Prop jammerPartB;

		public static float HTime;

		public static Prop VaultDoor;

		public static bool AIWaitForPlayer;

		public static float DoorOpenAmt = 0f;

		public static List<Prop> SecurityCam = new List<Prop>();

		public static int CamerasLeft;

		public static bool ShowCCTVBlipAliveDebug;

		public static bool ShowCCTVBlipDeadDebug;

		public static bool ShowPacificStandardVaultDebug;

		public static bool ShowTimeronShiftX;

		public static Vector3 DoorApos = new Vector3(233.12f, 215.95f, 105f);

		public static Vector3 DoorBPos = new Vector3(259.755f, 204.6733f, 105f);

		public static Vector3 DoorCPos = new Vector3(257.027f, 220.4003f, 105f);

		public static Vector3 DoorDPos = new Vector3(261.93f, 221.79f, 105f);

		public static int CurrentLootTarget;

		public static List<Vector3> Crate_loc = new List<Vector3>();

		public static List<float> Crate_Rot = new List<float>();

		public static int Completedfleeca;

		public static int CompletedPacificStandard;

		public static int CompletedPaleto;

		public static int CompletedJewel;

		public float SpeedChecktimer;

		public Vector3 Paleto = new Vector3(-116.69f, 6435.1f, 30.5f);

		public Vector3 PacStandard = new Vector3(223.06f, 207.39f, 104.5f);

		public Vector3 Jewelstore = new Vector3(-638.093f, -242.64f, 37.11f);

		private List<Vector3> HeistsLocations = new List<Vector3>
		{
			new Vector3(317.18f, -269.97f, 52.9f),
			new Vector3(152.41f, -1032.45f, 30f),
			new Vector3(-345.69f, -33.2f, 48f),
			new Vector3(-1217.544f, -321.25f, 38f),
			new Vector3(-2970.65f, 482.4f, 15.45f),
			new Vector3(1175.379f, 2693.28f, 38f),
			new Vector3(-116.69f, 6435.1f, 30.5f),
			new Vector3(223.06f, 207.39f, 104.5f),
			new Vector3(-638.093f, -242.64f, 37.11f),
			new Vector3(1309.312f, 1109.406f, 104.5f)
		};

		public static bool TestCase = false;

		public static Prop Phone;

		public static Prop USB;

		public static Prop Crate;

		public static float nextTick;

		public static List<Prop> PaintingHolder = new List<Prop>();

		public static List<Prop> Paintings = new List<Prop>();

		public static List<int> PaintingsFinished = new List<int>();

		public static List<string> PaintingIDs = new List<string> { "ch_prop_vault_painting_01j", "ch_prop_vault_painting_01i", "ch_prop_vault_painting_01h", "ch_prop_vault_painting_01g", "ch_prop_vault_painting_01f", "ch_prop_vault_painting_01e", "ch_prop_vault_painting_01d", "ch_prop_vault_painting_01c", "ch_prop_vault_painting_01b", "ch_prop_vault_painting_01a" };

		public static List<int> LootId = new List<int>();

		public static Vector3 LastRobberyPosition;

		public static bool InLaFuetaBlancaHeist;

		public static bool InitialAnimDone = false;

		public static bool CreatedCrates;

		public static int MaxPay = 100000000;

		public static int Sound;

		public static float Zone;

		public static int FailSound;

		public static Prop Drill;

		public static bool Isdrilliing;

		public static int DrillFX;

		public static float Speed;

		public static float Temp;

		public static float Pos;

		public static int scale;

		public static bool QuitHacking;

		public static bool isHacking;

		public static bool HackDone;

		public static bool Hackedkeypad;

		public static bool RequestEndCutscene;

		public static Ped Lester;

		public static Vector3 LesterSpawn = new Vector3(2681.348f, -360.3722f, -55.1f);

		public static int BreachTimer;

		public static int PinSound;

		public static bool triggerLockA;

		public static bool triggerLockB;

		public static bool triggerLockC;

		public static bool triggerLockD;

		public static int HackScene1;

		public static int JuggerNautTimer;

		public static int SwatTimer;

		public static Vector3 PeletoJammerPos = new Vector3(-104.086f, 6471.71f, 30.5f);

		public static bool JewelStoreRobberyOn;

		public static bool RobberA_BlockPermanentEvents;

		public static bool RobberA_AlwaysKeepTask;

		public static bool RobberB_BlockPermanentEvents;

		public static bool RobberB_AlwaysKeepTask;

		public static bool RobberC_BlockPermanentEvents;

		public static bool RobberC_AlwaysKeepTask;

		public static List<Vector3> JewelryPos = new List<Vector3>();

		public static List<float> JewelMarker = new List<float>();

		public static List<float> JewelsAmount = new List<float>();

		public static List<float> MaxAmount = new List<float>();

		public static List<int> CabinetType = new List<int>();

		public static bool TriggerUINotify;

		public static bool InHeist;

		public static Prop SmokeOrigin;

		public static List<Prop> Cameras = new List<Prop>();

		public static bool NotifiedPlayerStage3;

		public static Ped JewelHost;

		public static List<Ped> Civ = new List<Ped>();

		public static List<Vector3> CivPos = new List<Vector3>();

		public static List<Ped> Peds = new List<Ped>();

		public static List<Vehicle> Vehicles = new List<Vehicle>();

		public static List<Ped> Guards = new List<Ped>();

		public static ConeColor AiConeColor = ConeColor.DarkBlue;

		public static List<Ped> Guards2 = new List<Ped>();

		public static List<Ped> Civs = new List<Ped>();

		public static bool placedJammer;

		public static bool DefendingCops;

		public static bool VaultOpen;

		public static bool MissionOn;

		public static float HeistTime;

		public static bool HeadingtoArcade;

		public static bool escapingcops;

		public static int FleecaHeistStage;

		public static bool TriggeredJewelStoreAlarm;

		public static int JewelStoreJobCopsArriveIn;

		public static int LocalTake;

		public static HeistStage JewelStoreStage;

		public string MethodA_OR_MethodB = "A";

		public static List<Prop> JewelsCases = new List<Prop>();

		public static List<bool> RobbedJewelsCases = new List<bool>();

		public static int RobbedJewelsLeft;

		public static List<Blip> RobbedJewelsCasesBlips = new List<Blip>();

		public static List<Blip> CameraBlips = new List<Blip>();

		public static bool RobberAInitialAnimDone = false;

		public static bool RobberBInitialAnimDone = false;

		public static bool RobberCInitialAnimDone = false;

		public static bool RobberANearLoot = false;

		public static bool RobberBNearLoot = false;

		public static bool RobberCNearLoot = false;

		public static Vector3 FleecaBankMarker = new Vector3(-634.214f, -239.491f, 37f);

		public static bool OverideTasks;

		public static Blip Bank;

		public static int RobberAJewelType;

		public static List<Prop> Jewels = new List<Prop>();

		public static int soundid;

		public static int PlayerJewelSmashType;

		public static Prop RobberANearsetCabinet;

		public static Prop RobberANearsetGlass;

		public static Prop NearestGlass;

		public static int JewelType;

		public static Prop NearestCabinet;

		public static bool isRobbingCase;

		public static bool GotJewels;

		public static int Fleeca_HeistMinPay = 400000;

		public static int Fleeca_HeistMaxPay = 2000000;

		public static int Paleto_HeistMinPay = 400000;

		public static int Paleto_HeistMaxPay = 2000000;

		public static int JewelStore_HeistMinPay = 400000;

		public static int JewelStore_HeistMaxPay = 2000000;

		public static int PacificStandard_HeistMinPay = 400000;

		public static int PacificStandard_HeistMaxPay = 2000000;

		public static int BikerClubhouse_HeistMinPay = 400000;

		public static int BikerClubhouse_HeistMaxPay = 2000000;

		public static int LaFuenteBlanca_HeistMinPay = 400000;

		public static int LaFuenteBlanca_HeistMaxPay = 2000000;

		public static int UnionDepository_HeistMinPay = 400000;

		public static int UnionDepository_HeistMaxPay = 2000000;

		public static int HeistMinPay = 400000;

		public static int HeistMaxPay = 2000000;

		public bool TellPlayerMax;

		public int Difficulty = 4;

		public bool Fail;

		public bool Pass;

		public string m;

		public int p;

		public string r;

		public float t;

		public int H;

		public int c;

		public int cp;

		public float Timer;

		public static int RobberAJewelSmashType;

		public static int RobberBJewelSmashType;

		public static int RobberCJewelSmashType;

		public static int RobberBJewelType;

		public static int RobberCJewelType;

		public static Prop RobberBNearsetCabinet;

		public static Prop RobberCNearsetCabinet;

		public static Prop RobberBNearsetGlass;

		public static Prop RobberCNearsetGlass;

		public static bool EnableErrorDebugging = false;

		public Vector3 FleecaCash_Location;

		public float FleecaCash_Heading;

		public Vector3 FleecaTrolly_Location;

		public Vector3 FleecaTrolly_Rotation;

		public static string PlayerCurrentSDDict;

		public static int PlayerCurrentLockbox;

		public static int PlayerCurrentLockboxStage;

		public static Prop PlayerCurrentLockBoxProp;

		public static string PlayerCurrentLockBoxType;

		public static Prop PlayerCurrentLootBagProp;

		public static string RobberACurrentSDDict;

		public static int RobberACurrentLockbox;

		public static int RobberACurrentLockboxStage;

		public static Prop RobberACurrentLockBoxProp;

		public static string RobberACurrentLockBoxType;

		public static Prop RobberACurrentLootBagProp;

		public static string RobberBCurrentSDDict;

		public static int RobberBCurrentLockbox;

		public static int RobberBCurrentLockboxStage;

		public static Prop RobberBCurrentLockBoxProp;

		public static string RobberBCurrentLockBoxType;

		public static Prop RobberBCurrentLootBagProp;

		public static string RobberCCurrentSDDict;

		public static int RobberCCurrentLockbox;

		public static int RobberCCurrentLockboxStage;

		public static Prop RobberCCurrentLockBoxProp;

		public static string RobberCCurrentLockBoxType;

		public static Prop RobberCCurrentLootBagProp;

		public static bool RobberBFirstAnim;

		public static bool RobberCFirstAnim;

		public static bool PlayerGotJewels;

		public static List<SecCabinetProps> SecurityCabinetProps = new List<SecCabinetProps>();

		public static bool PlayerPlayingFinalAnim;

		public static bool RobberAPlayingFinalAnim;

		public static bool PlayerStartedCuttingAnim;

		public static bool PlayerFirstAnim;

		public static bool RobberAFirstAnim;

		public static Prop PlayerRobbingProp;

		public static Prop RobberARobbingProp;

		public static Prop RobberBRobbingProp;

		public static Prop RobberCRobbingProp;

		public static bool PlayerWaitingForLastAnim;

		public static bool RobberAWaitingForLastAnim;

		public static bool RobberBWaitingForLastAnim;

		public static bool RobberCWaitingForLastAnim;

		public static bool PlayerFinishedLastAnim;

		public static bool RobberAFinishedLastAnim;

		public static bool RobberBFinishedLastAnim;

		public static bool RobberCFinishedLastAnim;

		public static int RobberARobTimer;

		public static int RobberBRobTimer;

		public static int RobberCRobTimer;

		public static int CuttingCabinet;

		public static int CuttingScene1;

		public static Vector3 CuttingVector3;

		public static float Cuttingheading;

		public static Prop CuttingGlassPlate;

		public static int CabinetPropsSelectedIndex;

		public static int CuttingTime;

		public static int CuttingPTFX;

		public static float PowerOutput;

		public static int RequiredPowerOutput;

		public static int TimerPowerOutput;

		public static bool PlayerFinishedCutting;

		public static bool RobberAFinishedCutting;

		public static bool RobberBFinishedCutting;

		public static bool RobberCFinishedCutting;

		public static int CuttingSound;

		public static bool PlayingPTFX;

		public static int RobberAtimer = 0;

		public static int RobberBtimer = 0;

		public static int RobberCtimer = 0;

		public static float RobberACurrentLootBoxRotation;

		public static float PlayerCurrentLootBoxRotation;

		public static float PlayerGrabLootDelayStop;

		public static float RobberAGrabLootDelayStop;

		public static bool PlayingCashGrabIdle;

		public static float CashGrabLastSavePlace;

		public static int IdleScene;

		public static Prop PlayerLastLootProp;

		public static Prop RobberALastLootProp;

		public static bool PlayedFirstAnim;

		public static Ped RobberATarget;

		public static Ped RobberBTarget;

		public static Ped RobberCTarget;

		public static Prop RobberA_bagProp;

		public static Prop RobberB_bagProp;

		public static Prop RobberC_bagProp;

		public static float RobberBCurrentLootBoxRotation;

		public static float RobberCCurrentLootBoxRotation;

		public static Prop RobberA_HoldingLootProp;

		public static Prop RobberB_HoldingLootProp;

		public static Prop RobberC_HoldingLootProp;

		public static int RobberA_CashGrabTimer;

		public static int RobberB_CashGrabTimer;

		public static int RobberC_CashGrabTimer;

		public static Vector3 RobberA_RobberMarker = new Vector3(0f, 0f, 0f);

		public static Vector3 RobberB_RobberMarker = new Vector3(0f, 0f, 0f);

		public static Vector3 RobberC_RobberMarker = new Vector3(0f, 0f, 0f);

		public Prop RobberACurrentCabinet;

		public Prop RobberBCurrentCabinet;

		public Prop RobberCCurrentCabinet;

		public static int RobberASyncScene;

		public static int RobberBSyncScene;

		public static int RobberCSyncScene;

		public static bool RobberARobbing;

		public static bool RobberBRobbing;

		public static bool RobberCRobbing;

		public static LootType RobberACurrentType;

		public static LootType RobberBCurrentType;

		public static LootType RobberCCurrentType;

		public static int RobberACurrentValualabe;

		public static int RobberBCurrentValualabe;

		public static int RobberCCurrentValualabe;

		public static int RobberACuttingStage;

		public static int RobberBCuttingStage;

		public static int RobberCCuttingStage;

		public static bool RobberAMovingToLoot;

		public static bool RobberBMovingToLoot;

		public static bool RobberCMovingToLoot;

		public static int RobberAMoveTimer;

		public static int RobberBMoveTimer;

		public static int RobberCMoveTimer;

		public static Prop RobberACurrentPaiting;

		public static Prop RobberBCurrentPaiting;

		public static Prop RobberCCurrentPaiting;

		public static Prop RobberACurrentPaitingHolder;

		public static Prop RobberBCurrentPaitingHolder;

		public static Prop RobberCCurrentPaitingHolder;

		public static Prop PlayerCurrentPaitingHolder;

		public static int RobberACurrentID;

		public static int RobberBCurrentID;

		public static int RobberCCurrentID;

		public static Ped RobberA;

		public static Ped RobberB;

		public static Ped RobberC;

		public static bool RobberASetRob = false;

		public static bool RobberBSetRob = false;

		public static bool RobberCSetRob = false;

		public static WoodenContainer CurrentWoodenContainer;

		public static List<MissionOjects> SpawnedSilverCrates = new List<MissionOjects>();

		public static int iParam0f_1;

		public static int Local_26709f_13;

		public static int Local_26709f_8 = -1;

		public static int SelectedNum = 0;

		public static float InputedNumber1 = 0f;

		public static float InputedNumber2 = 0f;

		public static float InputedNumber3 = 0f;

		public static bool ShowingSafeScaleform;

		public static bool ReadyForCode;

		public static int SafeScene1;

		public static Prop Safe;

		public static bool PlayingIdle;

		public static bool GotCorrectSafeCode;

		public static bool GotIncorrectSafeCode;

		public static Prop SafeValuables;

		public static bool PlayingSafeFinalAnim;

		public static bool FinishedSafeAnimations;

		public static int CodeNumber1;

		public static int CodeNumber2;

		public static int CodeNumber3;

		public static int iLocal_26708;

		public static int Local_26709f_9;

		public static int Heist_Average_Pay;

		public static Prop bagProp;

		public static Prop _cashGrabTray1;

		public static Prop MoneyProp;

		public static int RobTimer;

		public static LootType CurrentType;

		public static bool SetRob = false;

		public static Prop PlayerCurrentPaiting;

		public static int PlayerCurrentID;

		public static int CuttingStage;

		public static Prop CurrentPaintingHolder;

		public static int CurrentPaitingID;

		public static Prop CurrentPainting;

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

		public static Vector3 FleecaBankHeistStartLocation;

		public static List<Vector3> FleecaBanks = new List<Vector3>
		{
			new Vector3(317.18f, -269.97f, 52.9f),
			new Vector3(152.41f, -1032.45f, 30f),
			new Vector3(-345.69f, -33.2f, 48f),
			new Vector3(-1217.544f, -321.25f, 38f),
			new Vector3(-2970.65f, 482.4f, 15.45f),
			new Vector3(1175.379f, 2693.28f, 38f)
		};

		public static Vector3 FleecaVaultHackLocation;

		public static float FleecaVaultHackHeading;

		public static List<Prop> Props = new List<Prop>();

		public static bool PaletoCopsIncoming;

		public static int KilledCops;

		public static int BankHeistResponceType;

		public static float RobberALastCombatTime;

		public static int Waittime;

		public static bool ExitPacStandard;

		public static bool PlayingPacificStandard;

		public static bool PlayingFleeca;

		public static bool PlayingPaleto;

		public static bool PlayingUnion;

		public static List<Blip> HeistBlipsFleeca = new List<Blip>();

		public static Blip PacStandardBlip;

		public static Blip PeletoBlip;

		public static int BankHeistStage;

		public static bool HackingFleecaKeypad;

		public static bool hackEnterAnimDone;

		public static bool CreatedHeistMarkers;

		public static bool EscapingHouseRobbery;

		public static bool InHouseRobbery;

		public static List<KeypadClass> MartinMadrazoKeyPads = new List<KeypadClass>
		{
			new KeypadClass(new Vector3(1393.91f, 1140.1f, 114.8f), -88.82f, 2)
		};

		public static List<KeypadClass> UnionKeyPads = new List<KeypadClass>
		{
			new KeypadClass(new Vector3(-7.34f, -686.36f, 16.38f), 70.34f, 2)
		};

		public static bool hackedUnionVaultDoor;

		public static bool hackedMadrazoFrontDoor;

		public static bool HackingKeypad = false;

		public static Prop LastKeypad;

		public static Vector3 MartinMadrazoHouseEnterance = new Vector3(1395.191f, 1141.743f, 113.63f);

		public static Vector3 MartinMadrazoHouseExit = new Vector3(1397.055f, 1141.794f, 113.33f);

		public static bool AllBombsPlaced;

		public static bool PlayerNearCargoShipA;

		public static bool PlayerNearCargoShipB;

		public static bool HasBeenOnCargoShip;

		public static int Followtimer;

		public static int WeaponsDelay;

		public static bool ParkedAtParkingSpot;

		public static bool UnionDepositoryGuardsHatePlayer = false;

		public static bool AlertedToBrokenDisguise;

		public static bool PaidforStorageOfGetawayCar;

		public static bool ChoosenStoredVehicleSpawn;

		public static bool AwawitingChoiceofSpawn;

		public static int GetawayVehicleSpawnChoice;

		public static SaveCar SavedGetawayVehicle;

		public static bool SpawnedGetawayVehicle;

		public static Vector3 LastGetawayCarPosition;

		public static Vector3 LastGetawayCarRotation;

		public static bool GetawayVehicleDetected;

		public static Ped GetawayCarDeliveryPed;

		public static bool SpawnedSearchParty;

		public static int SearchPartyDetectDelay;

		public static bool SearchPartyArrivedAtUnionDepository;

		public static bool SearchPartyDestoryed;

		public static List<Vector3> AllTokens = new List<Vector3>();

		public static List<float> TokenMarker = new List<float>();

		public static List<float> TokenAmount = new List<float>();

		public static List<float> TokenSize = new List<float>();

		public static bool GotCameras;

		public static bool CreateProps;

		public static float TotalCost;

		public static bool CreateSecondaryCops;

		public static List<Vehicle> Veh = new List<Vehicle>();

		public static bool TriggerVault;

		public static bool VaultUnlocked;

		public static Prop VaultD;

		public static Vehicle Van1;

		public static Vehicle Van2;

		public static Vehicle Van3;

		public static List<Prop> Spikes = new List<Prop>();

		public static bool CreateUnionDepositoryBlocker;

		public static Prop P;

		public static Prop P2;

		public static bool DeletedElv2;

		public static bool DeletedElv1;

		public static List<Prop> GoldTrayProps = new List<Prop>();

		public static List<string> UnionDP_MPMaleGruppe6Gear = new List<string> { "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Defender,0,0,0,0,0,0,11,0,10,0,0,0,51,0,0,0,153,0,7,1,76,0,319,1,142,0,8,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Enactor,0,0,0,0,0,0,1,0,10,0,0,0,51,0,0,0,153,0,7,1,71,0,316,2,126,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Imposer,0,0,0,0,0,0,1,0,10,0,0,0,51,0,0,0,153,0,7,1,71,0,317,1,126,0,5,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Rigid,0,0,0,0,0,0,1,0,10,0,0,0,51,0,0,0,153,0,7,1,71,0,316,1,144,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Shatterproof,0,0,0,0,0,0,11,0,10,0,0,0,51,0,0,0,153,0,7,1,76,0,318,1,126,0,5,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Gruppe Sechs Gear , The Unbreakable,0,0,0,0,0,0,11,0,10,0,0,0,51,0,0,0,153,0,7,1,76,0,319,2,144,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

		public static List<string> UnionDP_MPFemaleGruppe6Gear = new List<string> { "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Defender,21,0,0,0,0,0,9,0,6,0,0,0,52,0,0,0,189,0,6,1,85,0,330,1,141,0,11,1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Enactor,21,0,0,0,0,0,3,0,6,0,0,0,52,0,0,0,189,0,6,1,80,0,327,2,125,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Imposer,21,0,0,0,0,0,3,0,6,0,0,0,52,0,0,0,189,0,6,1,80,0,328,1,125,0,11,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Rigid,21,0,0,0,0,0,3,0,6,0,0,0,52,0,0,0,189,0,6,1,80,0,327,1,143,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Shatterproof,21,0,0,0,0,0,9,0,6,0,0,0,52,0,0,0,189,0,6,1,85,0,329,1,125,0,11,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Gruppe Sechs Gear , The Unbreakable,21,0,0,0,0,0,9,0,6,0,0,0,52,0,0,0,189,0,6,1,85,0,330,2,143,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

		public static List<string> UnionDP_MPMaleBugstarGear = new List<string> { "!Male , Diamond Casino Heist , Bugstars Uniforms , The Bagger,0,0,0,0,0,0,64,0,39,0,0,0,63,4,0,0,15,0,0,0,67,0,66,0,140,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Compressor,0,0,175,1,0,0,92,1,38,0,0,0,48,0,0,0,15,0,0,0,66,0,65,0,140,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Debugger,0,0,0,0,0,0,4,0,38,0,0,0,52,0,0,0,15,0,0,0,66,0,65,0,139,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Depositor,0,0,0,0,0,0,4,0,39,0,0,0,56,1,0,0,15,0,0,0,67,0,66,0,139,0,16,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Eradicator,0,0,175,1,0,0,88,0,39,0,0,0,56,1,0,0,15,0,0,0,67,0,66,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Male , Diamond Casino Heist , Bugstars Uniforms , The Sprayer,0,0,175,1,0,0,88,0,38,0,0,0,54,0,0,0,15,0,0,0,66,0,65,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

		public static List<string> UnionDP_MPFemaleBugstarGear = new List<string> { "!Female , Diamond Casino Heist , Bugstars Uniforms , The Bagger,21,0,0,0,0,0,75,0,39,0,0,0,64,0,0,0,2,0,0,0,76,0,60,0,139,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Compressor,21,0,175,0,0,0,101,1,38,0,0,0,49,0,0,0,2,0,0,0,75,0,59,0,139,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Debugger,21,0,0,0,0,0,3,0,38,0,0,0,59,0,0,0,2,0,0,0,75,0,59,0,138,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Depositor,21,0,0,0,0,0,3,0,39,0,0,0,59,1,0,0,2,0,0,0,76,0,60,0,138,0,20,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Eradicator,21,0,175,0,0,0,101,0,39,0,0,0,59,1,0,0,2,0,0,0,76,0,60,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1", "!Female , Diamond Casino Heist , Bugstars Uniforms , The Sprayer,21,0,175,0,0,0,101,0,38,0,0,0,55,0,0,0,2,0,0,0,75,0,59,0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1" };

		public static bool IsDressedUpAsBugstarsOrGroupe6;

		public static bool OverrideDisguiseIfAimingGun;

		public static bool OverrideDisguiseIfNotUnarmed;

		public static ApproachToolTip CurrentUnionApproachToottip;

		public static ApproachStyle CurrentUnionApproach;

		public static UnionDPStage CurrentUnionDPstage;

		public static List<Blip> IntelMarkers = new List<Blip>();

		public static List<Blip> EnteranceBlipMarkers = new List<Blip>();

		public static List<Blip> IntelMarkersRadiusArea = new List<Blip>();

		public static List<Blip> EnteranceBlipMarkersRadiusArea = new List<Blip>();

		public static Prop CutterKeys;

		public static Vehicle CutterVeh;

		public static Vector3 Gruppe6Stockade_LastPosition;

		public static Vector3 Gruppe6Stockade_LastRotation;

		public static Vehicle StockadeA;

		public static Vehicle StockadeB;

		public static Vehicle StockadeC;

		public static Vehicle BugstarsVan;

		public static Vehicle Gruppe6Stockade;

		public static Vector3 BugstarsVan_LastPosition;

		public static Vector3 BugstarsVan_LastRotation;

		public static Vehicle MaverickHeli;

		public static Ped MaverickPilot;

		public static Prop BugstarClothes;

		public static Prop Gruppe6Clothes;

		public static Vector3 UnionFrontEnt = new Vector3(4.8268f, -706.5121f, 45.0731f);

		public static bool GotBugstarVan;

		public static bool GotBugstarClothes;

		public static bool GotGruppe6Van;

		public static bool GotGruppe6Clothes;

		public static bool GotHiredHeli;

		public static int EnteringHeliTaskOveride;

		public static bool GotCutterKeys;

		public static bool BlownUpGrate;

		public static bool GotC4Crate;

		public static bool WaringBugstarsOutfit;

		public static bool WaringGruppe6Outfit;

		public static bool BrokenDisguise;

		public static List<bool> CheckCrateLoc = new List<bool>();

		public static List<Blip> CheckCrateLocBlip = new List<Blip>();

		public static Blip Missionblip;

		public static Blip Missionblip2;

		public static Blip VehSpawn1blip;

		public static Blip VehSpawn2blip;

		public static Blip HighwayEnteranceblip;

		public static bool HasBeenInUnionVaultFloor;

		public static bool UnionRappelingFromHeli;

		public static Random UnionDP_Random = new Random();

		public static Vector3 GamePlayCameraPos;

		public static Vector3 GamePlayCameraRot;

		public static bool PlayingBombPlacing;

		public static Camera PlaceBombCam;

		public static Tuple<Vector3, Vector3> BombVaultPlantLoc = new Tuple<Vector3, Vector3>(new Vector3(6.17f, -659.25f, 5.13f), new Vector3(0f, 0f, 344.07f));

		public static Tuple<Vector3, Vector3> CutterLoc = new Tuple<Vector3, Vector3>(new Vector3(-2.13f, -625.113f, 14.75f), new Vector3(0f, 0f, 241.13f));

		public static Tuple<Vector3, Vector3> MaverickLoc = new Tuple<Vector3, Vector3>(new Vector3(-1262.219f, -2294.15f, 12.94f), new Vector3(0f, 0f, 67.9f));

		public static Tuple<Vector3, Vector3> BugstarVanLoc = new Tuple<Vector3, Vector3>(new Vector3(149.87f, -3105.002f, 5.711f), new Vector3(0f, 0f, 0f));

		public static Tuple<Vector3, Vector3> BugstarClothesLoc = new Tuple<Vector3, Vector3>(new Vector3(120.83f, -3102.73f, 5.95f), new Vector3(-10.3f, 13f, 92.23f));

		public static Tuple<Vector3, Vector3> Gruppe6ClothesLoc = new Tuple<Vector3, Vector3>(new Vector3(-112.76f, -10.87f, 69.94f), new Vector3(0f, 0f, 136f));

		public static Tuple<Vector3, Vector3> Gruppe6VanLoc = new Tuple<Vector3, Vector3>(new Vector3(872.79f, -1350.214f, 25.91f), new Vector3(0f, 0f, 90f));

		public static Tuple<Vector3, Vector3> CutterKeysLoc = new Tuple<Vector3, Vector3>(new Vector3(-92.16f, -1032.582f, 28.08f), new Vector3(0f, 0f, 108.29f));

		public static string CutterKeysModel = "prop_cuff_keys_01";

		public static string BugstarsClothesBagModel = "ch_p_m_bag_var03_arm_s";

		public static string Gruppe6ClothesBagModel = "ch_prop_ch_duffbag_gruppe_01a";

		public static Prop iLocal_910;

		public static Prop iLocal_911;

		public static Prop TunnelHoleBlocker;

		public static Prop TunnelHoleBlocker2;

		public static bool MovingPastTunnelPathindBlocker;

		public static Ped RobberAClone;

		public static Prop MovingPastTunnelPathindProp;

		public static int NextCall;

		public static List<int> UnionPTFX = new List<int>();

		public static int BombExplodeDelay = 0;

		public static bool WaitingForBombExplosion;

		public static Tuple<Vector3, Vector3> VaultBombPlaceLoc = new Tuple<Vector3, Vector3>(new Vector3(6.17f, -659.25f, 15.13f), new Vector3(0f, 0f, 344f));

		public static Prop BombProp;

		public static Prop BombBagProp;

		public static bool VaultBombUsed;

		public static string VehRecordingsLocal_761 = "finheistb";

		public static string CutterAnimDictsLocal_772 = "vehicle_cutter";

		public static int DrillingTimeOut;

		public static bool CutterDrillingVault;

		public static bool CutterDrilledOpenVault;

		public static Vector3 UnionTopFloorElvToVaultPos = new Vector3(147.02f, -637.65f, 205.04f);

		public static Vector3 UnionTopFloorElvToEnterancePos = new Vector3(116.07f, -639.41f, 205.04f);

		public static Vector3 UnionElevatorVaultFloor = new Vector3(0.12f, -705.65f, 15.13f);

		public static Vector3 UnionElevatorUnderGroundFloor = new Vector3(10.41f, -670.89f, 33.44f);

		public static List<ObjectPlacementMap> UnionDPTopFloor = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap("apa_mp_h_acc_artwalll_03", new Vector3(124.620079f, -622.52f, 207.139038f), new Vector3(0f, 0f, -109.942924f)),
			new ObjectPlacementMap("ex_mp_h_din_table_11", new Vector3(120.720192f, -614.207f, 205.0468f), new Vector3(0f, 0f, 35.74183f)),
			new ObjectPlacementMap("ex_mp_h_din_table_11", new Vector3(127.640121f, -610.6733f, 205.046783f), new Vector3(0f, 0f, 11.31656f)),
			new ObjectPlacementMap("ex_mp_h_din_table_11", new Vector3(136.212662f, -610.050049f, 205.046738f), new Vector3(0f, 0f, -8.691252f)),
			new ObjectPlacementMap("ex_mp_h_din_table_11", new Vector3(133.158234f, -619.270935f, 205.046814f), new Vector3(0f, 0f, 159.370209f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(132.884338f, -620.3857f, 205.055176f), new Vector3(0f, 0f, 168.5494f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(118.18412f, -613.7403f, 205.054947f), new Vector3(0f, 0f, 69.8640442f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(117.816338f, -615.4297f, 205.0549f), new Vector3(0f, 0f, 97.40709f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(120.796188f, -616.039246f, 205.055161f), new Vector3(0f, 0f, -151.9002f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(122.003273f, -615.2464f, 205.055176f), new Vector3(0f, 0f, -111.797714f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(120.446655f, -612.4014f, 205.055054f), new Vector3(0f, 0f, 12.5801859f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(123.073662f, -612.781433f, 205.055008f), new Vector3(0f, 0f, -55.41166f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(128.208267f, -608.661438f, 205.055023f), new Vector3(0f, 0f, -14.6160717f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(126.604179f, -608.8673f, 205.055038f), new Vector3(0f, 0f, 24.9665947f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(124.8762f, -609.8108f, 205.054871f), new Vector3(0f, 0f, 66.3681259f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(125.168465f, -611.9968f, 205.055038f), new Vector3(0f, 0f, 112.27346f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(129.728485f, -611.940857f, 205.055161f), new Vector3(0f, 0f, -144.623856f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(127.129623f, -612.4722f, 205.055145f), new Vector3(0f, 0f, 143.919342f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(139.579056f, -609.275146f, 205.0548f), new Vector3(0f, 0f, -69.7023239f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(139.043655f, -611.174133f, 205.055f), new Vector3(0f, 0f, -117.6862f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(137.394821f, -612.075f, 205.05513f), new Vector3(0f, 0f, -166.276489f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(133.271637f, -611.0672f, 205.055084f), new Vector3(0f, 0f, 114.557976f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(133.426086f, -609.2592f, 205.054916f), new Vector3(0f, 0f, 67.09327f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(137.545227f, -608.4391f, 205.054947f), new Vector3(0f, 0f, -46.80435f)),
			new ObjectPlacementMap("ex_prop_monitor_01_ex", new Vector3(132.956177f, -619.378235f, 205.850525f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap("prop_cabinet_02b", new Vector3(122.692223f, -610.2728f, 206.080078f), new Vector3(0f, 0f, -56.6812172f)),
			new ObjectPlacementMap("prop_cabinet_02b", new Vector3(116.887177f, -615.4681f, 205.953979f), new Vector3(0f, 0f, 138.113159f)),
			new ObjectPlacementMap("prop_cabinet_02b", new Vector3(116.345985f, -615.050049f, 205.953934f), new Vector3(0f, 0f, 137.679825f)),
			new ObjectPlacementMap("xm_prop_base_staff_desk_01", new Vector3(143.672287f, -611.3526f, 205.046478f), new Vector3(0f, 0f, -110.987778f)),
			new ObjectPlacementMap("xm_prop_base_staff_desk_01", new Vector3(140.92189f, -624.0121f, 205.046692f), new Vector3(0f, 0f, 158.1217f)),
			new ObjectPlacementMap("xm_prop_base_staff_desk_02", new Vector3(137.338562f, -622.5976f, 205.0467f), new Vector3(0f, 0f, 159.7664f)),
			new ObjectPlacementMap("xm_prop_base_staff_desk_01", new Vector3(145.0366f, -612.135437f, 205.046524f), new Vector3(0f, 0f, 69.8291855f)),
			new ObjectPlacementMap("xm_prop_base_staff_desk_01", new Vector3(147.646851f, -611.584351f, 205.04631f), new Vector3(0f, 0f, -20.3357372f)),
			new ObjectPlacementMap("apa_p_mp_door_apart_door", new Vector3(149.634949f, -629.1431f, 205.196625f), new Vector3(0f, 0f, 69.74277f)),
			new ObjectPlacementMap("apa_mp_h_acc_artwalll_02", new Vector3(147.5563f, -634.6756f, 205.218872f), new Vector3(0f, 0f, 70.04016f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(141.407928f, -622.8216f, 205.055069f), new Vector3(0f, 0f, -8.591413f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(137.812714f, -621.6061f, 205.0551f), new Vector3(0f, 0f, -25.7409477f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(142.550964f, -611.0155f, 205.054825f), new Vector3(0f, 0f, 85.31469f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(146.065186f, -612.6427f, 205.0549f), new Vector3(0f, 0f, -119.125694f)),
			new ObjectPlacementMap("v_corp_offchair", new Vector3(147.306366f, -612.6432f, 205.05481f), new Vector3(0f, 0f, 157.464371f))
		};

		public static List<Prop> UnionDPTopFloorProps = new List<Prop>();

		public static List<ObjectPlacementMap> UnionDP_C4CrateLocs = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-230.165527f, -2375.57544f, 16.6101513f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-233.067734f, -2363.22339f, 16.6101513f), new Vector3(0f, 0f, -90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-220.389755f, -2357.45874f, 20.6106319f), new Vector3(0f, 0f, -90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-220.31f, -2372.0564f, 24.6125736f), new Vector3(0f, 0f, -90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-252.3921f, -2365.54858f, 8.596741f), new Vector3(0f, 0f, 90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-187.9996f, -2369.19336f, 8.596741f), new Vector3(0f, 0f, -90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-161.935226f, -2378.954f, 11.2491188f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-110.111153f, -2360.48926f, 13.069706f), new Vector3(0f, 0f, -90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-78.481f, -2366.09375f, 13.5746765f), new Vector3(0f, 0f, -90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-117.3421f, -2378.894f, 8.596741f), new Vector3(0f, 0f, -180f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-128.343872f, -2368.88013f, 8.596741f), new Vector3(0f, 0f, -90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-145.544159f, -2352.81616f, 11.2491169f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-156.159058f, -2363.61572f, 19.9130859f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-172.195938f, -2368.22852f, 19.9130859f), new Vector3(0f, 0f, -180f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-183.904922f, -2352.37769f, 8.596741f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-212.919785f, -2367.40161f, 8.596741f), new Vector3(0f, 0f, 90f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-214.575211f, -2366.47852f, 28.9627323f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap("ch_prop_crate_01a", new Vector3(-201.471436f, -2366.23657f, 19.90091f), new Vector3(0f, 0f, 0f))
		};

		public static Prop UnionDP_C4Crate;

		public static Vector3 UnionDepositoryVaultDoorPos = new Vector3(-3.63722f, -687.673f, 15.1f);

		public static Vector3 UnionDepositoryMainMarker = new Vector3(-13.7145f, -687.805f, 32f);

		public static bool PlayerInVault;

		public static List<Vector3> Nodes = new List<Vector3>();

		public static int SavePos;

		public static int BombsPlaced;

		public static bool StartedPlacingBombs;

		public static List<Blip> BombPlacementAreaBlip = new List<Blip>();

		public static int PlaceBombScene;

		public static List<Prop> GrateBombs = new List<Prop>();

		public static bool PlacingBombOnGrate;

		public static bool CreatedSafeAreaMarker;

		public static int CurrentHeliWaypoint;

		public static List<Vector3> HelicopterRoute = new List<Vector3>
		{
			new Vector3(-423.45f, -849.13f, 105f),
			new Vector3(-101.71f, -327.46f, 112f),
			new Vector3(96.24f, -403.83f, 128.89f),
			new Vector3(25.05681f, -634.4557f, 75f)
		};

		public static Vector3 HeliWaypoint;

		public static int HeliTaskTimer;

		public static bool StartedHeliRide;

		public static bool RobberACanLoot;

		public static bool RobberBCanLoot;

		public static bool RobberCCanLoot;

		public bool LastRobberACanLootSet;

		public static bool LastRobberBCanLootSet;

		public static bool LastRobberCCanLootSet;

		public static bool hasMovedPastBlocker;

		public static bool RobberANearVault;

		public static bool RobberBNearVault;

		public static bool RobberCNearVault;

		public List<Vector4> ReinforcmentPositions = new List<Vector4>
		{
			new Vector4(30.55113f, -664.4372f, 31.62864f, 339.5314f),
			new Vector4(21.07305f, -660.8932f, 31.62762f, 1.87974f),
			new Vector4(-13.11575f, -664.3311f, 32.44286f, 214.2148f),
			new Vector4(-27.15172f, -665.5052f, 32.44286f, 190.2212f),
			new Vector4(-41.33039f, -679.9819f, 32.47562f, 291.2764f),
			new Vector4(6.669189f, -695.2044f, 32.33809f, 73.7316f),
			new Vector4(7.375961f, -708.3442f, 32.48063f, 81.55267f),
			new Vector4(-9.806067f, -659.8499f, 33.48027f, 200.4622f),
			new Vector4(11.54412f, -659.2112f, 33.44882f, 133.8207f),
			new Vector4(-0.04649079f, -684.8476f, 16.13063f, 348.6949f),
			new Vector4(-5.244945f, -682.8839f, 16.13063f, 344.297f),
			new Vector4(-14.55426f, -694.449f, 16.13099f, 256.3901f),
			new Vector4(4.076643f, -708.2537f, 16.13101f, 3.19203f),
			new Vector4(8.298239f, -709.7027f, 16.13102f, 347.3044f),
			new Vector4(120.0991f, -618.9365f, 206.0469f, 218.789f),
			new Vector4(130.8158f, -610.8727f, 206.0467f, 100.732f),
			new Vector4(143.4073f, -613.0829f, 206.0468f, 87.3849f),
			new Vector4(155.9223f, -623.1694f, 206.0469f, 63.35915f),
			new Vector4(146.7045f, -642.4041f, 206.0468f, 344.3156f)
		};

		public bool RobberAWaitingAtCargoShip;

		public bool RobberBWaitingAtCargoShip;

		public bool RobberCWaitingAtCargoShip;

		public List<Vector4> DockWorkerGuard = new List<Vector4>
		{
			new Vector4(-127.4357f, -2354.393f, 9.319211f, 81.22004f),
			new Vector4(-163.5304f, -2353.586f, 9.319211f, 87.37349f),
			new Vector4(-209.425f, -2354.284f, 9.319211f, 86.37349f),
			new Vector4(-251.2381f, -2364.895f, 9.319211f, 201.8735f),
			new Vector4(-209.8157f, -2377.835f, 9.319211f, 273.8735f),
			new Vector4(-183.1492f, -2367.315f, 9.319211f, 201.3735f),
			new Vector4(-163.79f, -2377.746f, 9.319211f, 276.8735f),
			new Vector4(-128.237f, -2378.104f, 9.319211f, 204.8734f),
			new Vector4(-99.26895f, -2367.264f, 9.319211f, 19.37341f)
		};

		public List<int> DocksWorkerHashes = new List<int>();

		public int iLocal_1709;

		public List<Door> OpenDoors = new List<Door>
		{
			new Door("prop_gate_docks_ld", new Vector3(10.64414f, -2542.213f, 5.047173f)),
			new Door("prop_gate_docks_ld", new Vector3(19.40451f, -2529.702f, 5.047173f)),
			new Door("prop_gate_docks_ld", new Vector3(-202.6151f, -2515.309f, 5.047173f)),
			new Door("prop_gate_docks_ld", new Vector3(-187.3406f, -2515.309f, 5.047173f))
		};

		public List<Vector4> CargoShipGuardSpawns = new List<Vector4>
		{
			new Vector4(1232.419f, -3034.392f, 9.365917f, 0.4587774f),
			new Vector4(1253.093f, -3003.274f, 9.319256f, 0.8750737f),
			new Vector4(1228.546f, -2986.273f, 9.319257f, 358.0279f),
			new Vector4(1228.519f, -2923.55f, 9.319252f, 20.82217f),
			new Vector4(1239.253f, -2985.87f, 9.319263f, 91.12614f),
			new Vector4(1251.666f, -2942.952f, 9.331647f, 88.73854f),
			new Vector4(1252.585f, -2923.882f, 9.319253f, 28.53413f),
			new Vector4(1252.741f, -2923.594f, 9.319262f, 87.15132f),
			new Vector4(1229.336f, -3010.24f, 9.319263f, 269.6374f)
		};

		public static List<KeypadClass> BikerClubhouseKeyPads = new List<KeypadClass>
		{
			new KeypadClass(new Vector3(1113.719f, -3156.489f, -37.37517f), 272.2959f, 3)
		};

		public bool hackedBikerClubhouseGrateDoor;

		public static List<ObjectPlacementMap> BikerClubhousePlacements = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap("prop_ss1_mpint_garage", new Vector3(1100.95f, -3160.146f, -37.02f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap("prop_ss1_mpint_garage", new Vector3(1100.95f, -3160.151f, -38.64f), new Vector3(0f, 0f, 180f)),
			new ObjectPlacementMap("apa_prop_ss1_mpint_garage2", new Vector3(1114.016f, -3158.113f, -37.97829f), new Vector3(0f, 0f, 270.4391f)),
			new ObjectPlacementMap("prop_ld_vault_door", new Vector3(1118.185f, -3147.918f, -38.0763f), new Vector3(0f, 0f, 0.1662736f)),
			new ObjectPlacementMap("bkr_prop_bkr_cashpile_01", new Vector3(1113.142f, -3145.25f, -37.22839f), new Vector3(0f, 0f, 270.8312f)),
			new ObjectPlacementMap("prop_cash_trolly", new Vector3(1111.103f, -3144.65f, -38f), new Vector3(0f, 0f, 89.87756f)),
			new ObjectPlacementMap("xs_prop_arena_wall_02a_sf", new Vector3(1104.895f, -3161.51f, -38.6186f), new Vector3(0f, 0f, 1.209162f)),
			new ObjectPlacementMap("hei_prop_hei_cash_trolly_03", new Vector3(1119.484f, -3146.485f, -38f), new Vector3(0f, 0f, 270.6733f)),
			new ObjectPlacementMap("xs_prop_arena_wall_02a_sf", new Vector3(1105.23f, -3161.432f, -38.6186f), new Vector3(0f, 0f, 91.23122f)),
			new ObjectPlacementMap("bkr_prop_weed_table_01b", new Vector3(1116.65f, -3148.11f, -38.06f), new Vector3(0f, 0f, 178f)),
			new ObjectPlacementMap("bkr_prop_weed_table_01b", new Vector3(1114.16f, -3148.049f, -38.06f), new Vector3(0f, 0f, 178f)),
			new ObjectPlacementMap("h4_prop_h4_cash_stack_01a", new Vector3(1116.677f, -3145.234f, -37.2281f), new Vector3(0f, 0f, 91.36672f)),
			new ObjectPlacementMap("ch_prop_ch_cash_trolly_01c", new Vector3(1119.468f, -3145.165f, -38f), new Vector3(0f, 0f, 270.6733f)),
			new ObjectPlacementMap("h4_prop_h4_weed_stack_01a", new Vector3(1114.855f, -3145.625f, -37.23292f), new Vector3(0f, 0f, 0.4635244f)),
			new ObjectPlacementMap("ch_prop_ch_cash_trolly_01c", new Vector3(1111.023f, -3146.026f, -38f), new Vector3(0f, 0f, 91.35081f)),
			new ObjectPlacementMap("ch_prop_ch_cash_trolly_01c", new Vector3(1118.08f, -3142.843f, -38f), new Vector3(0f, 0f, 1.011606f)),
			new ObjectPlacementMap("ch_prop_ch_cash_trolly_01c", new Vector3(1116.43f, -3142.89f, -38f), new Vector3(0f, 0f, 1.011606f)),
			new ObjectPlacementMap("bkr_prop_meth_table01a", new Vector3(1104.36865f, -3153.77661f, -38.5186f), new Vector3(0f, 0f, -89.59686f)),
			new ObjectPlacementMap("bkr_prop_weed_table_01a", new Vector3(1104.37622f, -3156.08716f, -38.5186f), new Vector3(0f, 0f, -89.59686f)),
			new ObjectPlacementMap("bkr_prop_coke_table01a", new Vector3(1104.39246f, -3158.398f, -38.5186f), new Vector3(0f, 0f, -89.59686f)),
			new ObjectPlacementMap("bkr_prop_coke_table01a", new Vector3(1105.55017f, -3144.204f, -38.5186f), new Vector3(0f, 0f, -89.59686f)),
			new ObjectPlacementMap("bkr_prop_weed_table_01b", new Vector3(1105.559f, -3146.59424f, -38.5186f), new Vector3(0f, 0f, -89.59686f)),
			new ObjectPlacementMap("bkr_prop_coke_tablepowder", new Vector3(1105.67444f, -3146.57129f, -37.6812325f), new Vector3(0f, 0f, -89.59686f))
		};

		public static Prop BikerClubhouseGrateDoor;

		public static Prop BikerClubhouseVaultDoor;

		public static List<Prop> BikeClubhouseHeistProps = new List<Prop>();

		public static Vector3 BikerclubhouseEnterance = new Vector3(186.64f, 2786.45f, 44.9f);

		public static Vector3 BikerclubhouseExit = new Vector3(1100.655f, -3159.314f, -38.5f);

		public static bool BikerClubhouseVaultOpen;

		public static bool BikerClubhouseVaultIsOpening;

		public static float BikerClubhouseVaultOpenedAmt;

		public static List<Vector4> BikerClubhouseEnemySpawns = new List<Vector4>
		{
			new Vector4(1101.347f, -3141.798f, -37.51859f, 183.01f),
			new Vector4(1105.552f, -3153.442f, -37.51859f, 288.2606f),
			new Vector4(1114.398f, -3154.982f, -37.06276f, 269.5791f),
			new Vector4(1124.683f, -3145.061f, -37.06276f, 119.0132f),
			new Vector4(1122.077f, -3161.936f, -36.87049f, 5.998611f),
			new Vector4(1097.854f, -3153.097f, -37.51863f, 285.9077f),
			new Vector4(185.6666f, 2789.359f, 45.53331f, 286.3654f),
			new Vector4(183.4445f, 2805.349f, 45.59919f, 353.594f),
			new Vector4(195.4021f, 2806.725f, 45.61104f, 14.56703f),
			new Vector4(214.5182f, 2801.358f, 45.65519f, 226.4392f),
			new Vector4(214.2983f, 2800.109f, 45.65519f, 247.1329f),
			new Vector4(214.4345f, 2781.635f, 45.65527f, 7.474914f),
			new Vector4(220.155f, 2776.178f, 45.65527f, 225.0522f),
			new Vector4(222.0085f, 2775.427f, 45.65527f, 126.5365f),
			new Vector4(213.706f, 2768.422f, 45.65527f, 197.6476f),
			new Vector4(165.789f, 2775.274f, 45.70288f, 71.28694f),
			new Vector4(169.8947f, 2771.877f, 45.70288f, 182.2822f),
			new Vector4(170.8924f, 2771.239f, 45.68875f, 158.2258f),
			new Vector4(198.5529f, 2792.26f, 45.65518f, 170.6843f),
			new Vector4(197.1446f, 2791.662f, 45.65518f, 254.7305f),
			new Vector4(199.2199f, 2791.147f, 45.65518f, 100.8582f),
			new Vector4(199.2999f, 2776.29f, 45.65526f, 188.1952f)
		};

		public static List<Vector4> BikerClubhouseVanSpawns = new List<Vector4>
		{
			new Vector4(196.588f, 2794.712f, 45.47298f, 291.5604f),
			new Vector4(194.4052f, 2787.158f, 45.47102f, 355.1047f),
			new Vector4(176.7635f, 2772.954f, 45.51102f, 304.3328f)
		};

		public static List<Vector4> BikerClubhouseBikeSpawns = new List<Vector4>
		{
			new Vector4(209.0909f, 2789.943f, 45.12632f, 319.9031f),
			new Vector4(207.3349f, 2789.931f, 45.12876f, 346.8014f),
			new Vector4(210.9535f, 2791.45f, 45.12679f, 49.02586f),
			new Vector4(191.2634f, 2793.392f, 45.12432f, 35.3793f),
			new Vector4(189.5293f, 2791.123f, 45.11097f, 18.74611f),
			new Vector4(181.6992f, 2793.577f, 45.11097f, 229.2461f),
			new Vector4(216.404f, 2806.114f, 45.11097f, 109.2461f),
			new Vector4(217.7297f, 2804.687f, 45.11097f, 139.2461f),
			new Vector4(220.0459f, 2779.819f, 45.11097f, 199.7461f),
			new Vector4(217.8632f, 2779.783f, 45.11097f, 212.2461f)
		};

		public static bool ConvStarted;

		public static Ped ConvPed;

		public static List<Activity> Activities = new List<Activity>
		{
			new Activity(new Vector3(317.18f, -269.97f, 52.9f), Main.HeistID.Fleeca, "Fleeca Bank Heist", 272),
			new Activity(new Vector3(152.41f, -1032.34f, 28.33f), Main.HeistID.Fleeca, "Fleeca Bank Heist", 272),
			new Activity(new Vector3(-345.69f, -33.2f, 46.53f), Main.HeistID.Fleeca, "Fleeca Bank Heist", 272),
			new Activity(new Vector3(-1217.544f, -321.25f, 36.67f), Main.HeistID.Fleeca, "Fleeca Bank Heist", 272),
			new Activity(new Vector3(-2970.65f, 482.4f, 14.4f), Main.HeistID.Fleeca, "Fleeca Bank Heist", 272),
			new Activity(new Vector3(1175.379f, 2693.28f, 36.91f), Main.HeistID.Fleeca, "Fleeca Bank Heist", 272),
			new Activity(new Vector3(223.06f, 207.39f, 104.5f), Main.HeistID.PacificStandard, "Pacific Standard Heist", 272),
			new Activity(new Vector3(-128f, 6443.71f, 29.8f), Main.HeistID.Paleto, "Paleto Bank Heist", 272),
			new Activity(new Vector3(-673.6f, -227.4f, 36.09f), Main.HeistID.JewelStore, "Jewel Store Heist", 272),
			new Activity(new Vector3(-755.691f, -2819.78f, 12.94f), Main.HeistID.LaoubPrincessCargoshipHeist, "La'oub Princess Cargoship Heist", 272),
			new Activity(new Vector3(1309.312f, 1109.406f, 104.5f), Main.HeistID.LaFuenteBlanca, "La Fuente Blanca Heist", 272),
			new Activity(new Vector3(116.79f, -442.26f, 40.12f), Main.HeistID.UnionDepository, "Union Depository Heist", 272),
			new Activity(new Vector3(291.87f, 2652.5f, 43.65f), Main.HeistID.BikerClubhouse, "Biker Clubhouse Heist", 272),
			new Activity(new Vector3(1295.66f, -3337.12f, 4.9f), Main.HeistID.TheLuxigntonRaid, "The Luxington Raid Heist", 272),
			new Activity(new Vector3(-1760.03f, 83.03f, 67.6f), Main.HeistID.TheBillionairesParty, "The Billionaires Party Heist", 272),
			new Activity(new Vector3(-992.7f, 6256.53f, 1.6f), Main.HeistID.TheCartelYachtHeist, "The Cartel Yacht Heist", 272),
			new Activity(new Vector3(1785.84f, 3045.28f, 61.9f), Main.HeistID.TheFederalReserveTrainHeist, "The Federal Reserve Train Heist", 272),
			new Activity(new Vector3(-1341.6f, -859.43f, 15.7f), Main.HeistID.TheMazeBankSavingsBankHeist, "The Maze Bank Savings Bank Heist", 272),
			new Activity(new Vector3(-150.97f, -875.12f, 28.5f), Main.HeistID.ThePrinthouseHeist, "The Printhouse Heist", 272),
			new Activity(new Vector3(396.52f, -872.41f, 28.24f), Main.HeistID.TheDiamondJewelStoreHeist, "The Diamond Jewel Store Heist", 272, false),
			new Activity(new Vector3(-1308.53f, -369.83f, 35.64f), Main.HeistID.TheBettaBankHeist, "The Betta Bank Heist", 272, false),
			new Activity(new Vector3(-623.06f, -966.17f, 20.3f), Main.HeistID.TheGefangnisArtHeist, "The Gefangnis Art Heist", 272, false),
			new Activity(new Vector3(816.64f, -47.45f, 79.56f), Main.HeistID.TheDiamondCasino, "The Diamond Casino Heist", 272, false),
			new Activity(new Vector3(1986.89f, 2349.98f, 89.39f), Main.HeistID.ThePrisonBreak, "The Prison Break", 272, false),
			new Activity(new Vector3(2901.012f, 4432.351f, 47.19f), Main.HeistID.TheHumaineLabs, "The Humaine Labs Raid", 272, false)
		};

		public static bool nearActivity = false;

		public static Vector3 CurrentActivity;

		public static int ChecknearActivity;

		public static HeistIn CurrentHeist;

		public static int Heist_Blip_Type;

		public static bool Disable_HeistIcon_Blips;

		public static bool Disable_Bodyguard_AiIntergration;

		public static bool Disable_PlayerCompanion_AiIntergration;

		public static bool Disable_Single_Player_Pause;

		public static bool Initialized;

		public static void SetNoLongerInHeist()
		{
			SmartGuards.RemoveAIGuards();
			nearActivity = false;
			PlayingFleeca = false;
			PlayingPaleto = false;
			PlayingPacificStandard = false;
			JewelStoreRobberyOn = false;
			PlayingUnion = false;
			InLaFuetaBlancaHeist = false;
			PlayingBikerClubhouseHeist = false;
			SmartGuards.ClearGenericGuards();
			CurrentHeist = HeistIn.None;
			foreach (Activity activity in Activities)
			{
				if (activity.Blip != null)
				{
					activity.Blip.Alpha = 255;
					if (Disable_HeistIcon_Blips)
					{
						activity.Blip.Alpha = 0;
					}
				}
			}
			CargoShipHeist.CargoshipHeistOn = false;
			TheBillionaresParty.TheBiliionaresPartyHeistOn = false;
			TheFederalReserveTrainHeist.TheFederalReserveTrainHeistOn = false;
			TheLexingtonRaid.TheLuxingtonRaidHeistOn = false;
			YachtHeist.TheCartelYachtHeistOn = false;
			CayoPericoHeist.MissionOn = false;
			HumainLabsOnline.MissionOn = false;
			TheDiamondHeist.MissionOn = false;
			ThePrisonBreak.InPrisonBreakHeist = false;
			The247Job._247RobberyOn = false;
			TheBetaBankHeist.TheBetaBankHeistOn = false;
			TheDiamondJewelStore.TheDiamondJewelStoreHeistOn = false;
			TheGefangnisArtHeist.TheGefangnisArtHeistOn = false;
			TheMazebankSavingsBankHeistcs.TheMazeBankSavngsBankHeistOn = false;
			ThePrinthouseHeist.ThePrinthouseHeistOn = false;
			TheSilthouseJob.SilthouseRobberyOn = false;
			ManageRobberyAnims.PlayingAnim = false;
			ManageRobberyAnims.DeleteLoot();
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public static void TextNotification(string avatar, string author, string title, string message)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CONFIRM_BEEP", "HUD_MINI_GAME_SOUNDSET");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, new InputArgument[1] { "STRING" });
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { message });
			int num = Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, new InputArgument[6] { avatar, avatar, true, 0, title, author });
		}

		public static void TextNotification(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 0, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public static Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
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

		public static void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public static void EnterBestSeat()
		{
			VehicleSeat seat = DefaultSeat;
			if (!(Game.Player.Character.CurrentVehicle != null))
			{
				return;
			}
			Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
			if (currentVehicle.Model == VehicleHash.Menacer || currentVehicle.Model == VehicleHash.Caracara)
			{
				seat = VehicleSeat.ExtraSeat1;
			}
			if (currentVehicle.Model == VehicleHash.Insurgent || currentVehicle.Model == VehicleHash.Insurgent3)
			{
				seat = VehicleSeat.ExtraSeat5;
			}
			if (currentVehicle.Model == VehicleHash.Technical || currentVehicle.Model == VehicleHash.Technical2 || currentVehicle.Model == VehicleHash.Technical3)
			{
				seat = VehicleSeat.LeftRear;
			}
			if (currentVehicle.Model == VehicleHash.Barrage)
			{
				if (currentVehicle.IsSeatFree(VehicleSeat.LeftRear))
				{
					seat = VehicleSeat.LeftRear;
				}
				if (!currentVehicle.IsSeatFree(VehicleSeat.LeftRear))
				{
					seat = (currentVehicle.IsSeatFree(VehicleSeat.RightRear) ? VehicleSeat.RightRear : VehicleSeat.Passenger);
				}
			}
			if (currentVehicle.Model == RequestModel("PATROLBOAT"))
			{
				if (currentVehicle.IsSeatFree(VehicleSeat.LeftRear))
				{
					seat = VehicleSeat.LeftRear;
				}
				if (!currentVehicle.IsSeatFree(VehicleSeat.LeftRear))
				{
					seat = (currentVehicle.IsSeatFree(VehicleSeat.LeftRear) ? VehicleSeat.LeftRear : VehicleSeat.Passenger);
				}
			}
			if (currentVehicle.Model == RequestModel("DINGHY5"))
			{
				if (currentVehicle.IsSeatFree(VehicleSeat.ExtraSeat1))
				{
					seat = VehicleSeat.ExtraSeat1;
				}
				if (!currentVehicle.IsSeatFree(VehicleSeat.ExtraSeat1))
				{
					seat = (currentVehicle.IsSeatFree(VehicleSeat.ExtraSeat1) ? VehicleSeat.ExtraSeat1 : VehicleSeat.Passenger);
				}
			}
			if (!(Game.Player.Character.CurrentVehicle != null))
			{
				return;
			}
			if (RobberA != null)
			{
				if (Game.Player.Character.CurrentVehicle.IsSeatFree(seat))
				{
					RobberA.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, seat, -1, 30f);
				}
				else
				{
					RobberA.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, -1, 30f);
				}
			}
			if (RobberC != null)
			{
				if (Game.Player.Character.CurrentVehicle.IsSeatFree(seat))
				{
					RobberC.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, seat, -1, 30f);
				}
				else
				{
					RobberC.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, -1, 30f);
				}
			}
			if (RobberB != null)
			{
				if (Game.Player.Character.CurrentVehicle.IsSeatFree(seat))
				{
					RobberB.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, seat, -1, 30f);
				}
				else
				{
					RobberB.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, -1, 30f);
				}
			}
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
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public new static void Wait(int Time)
		{
			Script.Wait(Time);
		}

		public string ReturnButtonString(int ID)
		{
			SelectedButtonIndex = "Error Invald Key ID";
			if (ID < 358)
			{
				foreach (ButtonData item in ButtonIndex)
				{
					if (item.ButtonID == ID)
					{
						SelectedButtonIndex = item.DisplayName;
						string selectedButtonIndex = SelectedButtonIndex;
						SelectedButtonIndex = "~" + selectedButtonIndex + "~";
					}
				}
			}
			else
			{
				SelectedButtonIndex = "Error Invald Key ID :" + ID;
			}
			return SelectedButtonIndex;
		}

		public int ReturnButtonID(int ID)
		{
			return ID;
		}

		public void StartHouseRobbery(int InteriorType)
		{
			ManageRobberyAnims.CurrentPay = 0;
			GTA.UI.Screen.FadeOut(500);
			Wait(500);
			foreach (Prop prop22 in Props)
			{
				if (prop22 != null)
				{
					prop22.Delete();
				}
			}
			ManageRobberyAnims.DeleteLoot();
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
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
			if (LootId.Count > 0)
			{
				LootId.Clear();
			}
			ManageRobberyAnims.DeleteLoot();
			if (Crate_loc.Count > 0)
			{
				Crate_loc.Clear();
			}
			if (Crate_Rot.Count > 0)
			{
				Crate_Rot.Clear();
			}
			if (PaintingsFinished.Count > 0)
			{
				PaintingsFinished.Clear();
			}
			Random random = new Random();
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
			Game.Player.WantedLevel = 0;
			if (InteriorType == 1)
			{
				int num = 400;
				num = random.Next(0, 350);
				Crate_loc.Add(new Vector3(-765.76f, 326.2966f, 221.85f));
				Crate_Rot.Add(179f);
				LootId.Add(0);
				Crate_loc.Add(new Vector3(-759.9371f, 318.52f, 221.75f));
				Crate_Rot.Add(-92f);
				LootId.Add(0);
				for (int i = 0; i < Crate_loc.Count; i++)
				{
					Vector3 vector = Crate_loc[i];
					int num2 = random.Next(0, 400);
					if (num2 < 100)
					{
						Prop o = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_cash_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						ManageRobberyAnims.AddPropAsLoot(o);
					}
					if (num2 >= 100 && num2 < 200)
					{
						Prop o2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_weed_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						ManageRobberyAnims.AddPropAsLoot(o2);
					}
					if (num2 >= 200 && num2 < 300)
					{
						Prop o3 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_coke_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						ManageRobberyAnims.AddPropAsLoot(o3);
					}
					if (num2 >= 300 && num2 < 400)
					{
						Prop o4 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_gold_stack_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						ManageRobberyAnims.AddPropAsLoot(o4);
					}
					Wait(1);
				}
				Crate_loc.Add(new Vector3(-766.95f, 319.26f, 220.85f));
				Crate_Rot.Add(-133f);
				LootId.Add(1);
				Crate_loc.Add(new Vector3(-777.76f, 323.34f, 222.25f));
				Crate_Rot.Add(38f);
				LootId.Add(1);
				Crate_loc.Add(new Vector3(-775.16f, 329.7166f, 222.25f));
				Crate_Rot.Add(0f);
				LootId.Add(1);
				for (int j = 0; j < Crate_loc.Count; j++)
				{
					Vector3 vector2 = Crate_loc[j];
					if (LootId[j] != 1)
					{
						continue;
					}
					if (num < 100)
					{
						int num3 = random.Next(0, 300);
						if (num3 < 100)
						{
							Prop prop = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01a"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop);
						}
						if (num3 >= 100 && num3 < 200)
						{
							Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01b"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop2);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop2);
						}
						if (num3 >= 200)
						{
							Prop prop3 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01c"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop3);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop3);
						}
					}
					if (num >= 100 && num < 200)
					{
						int num4 = random.Next(0, 300);
						if (num4 < 100)
						{
							Prop prop4 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01a"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							prop4.IsPositionFrozen = true;
							ManageRobberyAnims.AddPropAsLoot(prop4);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop4);
						}
						if (num4 >= 100 && num4 < 200)
						{
							Prop prop5 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01b"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							prop5.IsPositionFrozen = true;
							ManageRobberyAnims.AddPropAsLoot(prop5);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop5);
						}
						if (num4 >= 200)
						{
							Prop prop6 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01c"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							prop6.IsPositionFrozen = true;
							ManageRobberyAnims.AddPropAsLoot(prop6);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop6);
						}
					}
					if (num >= 200 && num < 300)
					{
						int num5 = random.Next(0, 300);
						if (num5 < 100)
						{
							Prop prop7 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01a"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							prop7.IsPositionFrozen = true;
							ManageRobberyAnims.AddPropAsLoot(prop7);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop7);
						}
						if (num5 >= 100 && num5 < 200)
						{
							Prop prop8 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01b"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							prop8.IsPositionFrozen = true;
							ManageRobberyAnims.AddPropAsLoot(prop8);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop8);
						}
						if (num5 >= 200)
						{
							Prop prop9 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01c"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop9);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop9);
						}
					}
					if (num > 300)
					{
						int num6 = random.Next(0, 300);
						Prop prop10 = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_adv_case_01a"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
						prop10.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop10);
						Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop10);
					}
					num = random.Next(0, 350);
				}
				CreatedCrates = true;
				Prop prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-751.83f, 329.79f, 222.6f), new Vector3(0f, 90f, 180f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-764.1f, 313.87f, 217.86f), new Vector3(0f, 90f, 90f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-769.51f, 313.89f, 217.86f), new Vector3(0f, 90f, 90f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-781.72f, 313.88f, 224.07f), new Vector3(0f, 90f, 90f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-764.19f, 313.81f, 222.66f), new Vector3(0f, 90f, 90f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-769.25f, 314.07f, 222.66f), new Vector3(0f, 90f, 90f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-760.76f, 313.86f, 222.66f), new Vector3(0f, 90f, 90f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-751.95f, 316.5f, 222.66f), new Vector3(0f, 90f, 180f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				prop11 = GlobalHelper.CreateProp(RequestModel("hei_heist_acc_rugwooll_03"), new Vector3(-751.81f, 321.43f, 222.66f), new Vector3(0f, 90f, 180f), false, false);
				prop11.IsPositionFrozen = true;
				Props.Add(prop11);
				Game.Player.Character.Position = new Vector3(-781.74f, 323.39f, 222.5f);
			}
			if (InteriorType == 3)
			{
				int num7 = 400;
				num7 = random.Next(0, 350);
				Crate_loc.Add(new Vector3(1402.402f, 1133.939f, 114.33f));
				Crate_Rot.Add(266f);
				LootId.Add(1);
				Crate_loc.Add(new Vector3(1400.061f, 1141.461f, 114.33f));
				Crate_Rot.Add(-90f);
				LootId.Add(1);
				Crate_loc.Add(new Vector3(1395.35f, 1148.704f, 114.33f));
				Crate_Rot.Add(90f);
				LootId.Add(1);
				Crate_loc.Add(new Vector3(1399.303f, 1165.555f, 114.33f));
				Crate_Rot.Add(0f);
				LootId.Add(1);
				Crate_loc.Add(new Vector3(1408.351f, 1145.566f, 114.33f));
				Crate_Rot.Add(268f);
				LootId.Add(1);
				Crate_loc.Add(new Vector3(1400.499f, 1129.013f, 114.33f));
				Crate_Rot.Add(180f);
				LootId.Add(1);
				for (int k = 0; k < Crate_loc.Count; k++)
				{
					Vector3 vector3 = Crate_loc[k];
					if (LootId[k] != 1)
					{
						continue;
					}
					if (num7 < 100)
					{
						int num8 = random.Next(0, 300);
						if (num8 < 100)
						{
							Prop prop12 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01a"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop12);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop12);
						}
						if (num8 >= 100 && num8 < 200)
						{
							Prop prop13 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01b"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop13);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop13);
						}
						if (num8 >= 200)
						{
							Prop prop14 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01c"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop14);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop14);
						}
					}
					if (num7 >= 100 && num7 < 200)
					{
						int num9 = random.Next(0, 300);
						if (num9 < 100)
						{
							Prop prop15 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01a"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop15);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop15);
						}
						if (num9 >= 100 && num9 < 200)
						{
							Prop prop16 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01b"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop16);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop16);
						}
						if (num9 >= 200)
						{
							Prop prop17 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01c"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop17);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop17);
						}
					}
					if (num7 >= 200 && num7 < 300)
					{
						int num10 = random.Next(0, 300);
						if (num10 < 100)
						{
							Prop prop18 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01a"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop18);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop18);
						}
						if (num10 >= 100 && num10 < 200)
						{
							Prop prop19 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01b"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop19);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop19);
						}
						if (num10 >= 200)
						{
							Prop prop20 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01c"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
							ManageRobberyAnims.AddPropAsLoot(prop20);
							Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop20);
						}
					}
					if (num7 > 300)
					{
						int num11 = random.Next(0, 300);
						Prop prop21 = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_adv_case_01a"), Crate_loc[k], new Vector3(0f, 0f, Crate_Rot[k] - 180f), true, false);
						ManageRobberyAnims.AddPropAsLoot(prop21);
						Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop21);
					}
					num7 = random.Next(0, 350);
				}
				CreatedCrates = true;
				Game.Player.Character.Position = MartinMadrazoHouseExit;
			}
			Wait(500);
			GTA.UI.Screen.FadeIn(500);
			InHouseRobbery = true;
			if (RobberA != null)
			{
				RobberA.Position = Game.Player.Character.Position;
			}
			if (RobberB != null)
			{
				RobberB.Position = Game.Player.Character.Position;
			}
			if (RobberC != null)
			{
				RobberC.Position = Game.Player.Character.Position;
			}
		}

		public void EndHouseRobbery()
		{
			GTA.UI.Screen.FadeOut(500);
			Wait(500);
			foreach (Prop prop in Props)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			ManageRobberyAnims.DeleteLoot();
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
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
			if (LootId.Count > 0)
			{
				LootId.Clear();
			}
			if (Crate_loc.Count > 0)
			{
				Crate_loc.Clear();
			}
			if (Crate_Rot.Count > 0)
			{
				Crate_Rot.Clear();
			}
			if (PaintingsFinished.Count > 0)
			{
				PaintingsFinished.Clear();
			}
			Random random = new Random();
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Game.Player.WantedLevel = 4;
			Game.Player.Character.Position = LastRobberyPosition;
			if (RobberA != null)
			{
				RobberA.Position = Game.Player.Character.Position;
			}
			if (RobberB != null)
			{
				RobberB.Position = Game.Player.Character.Position;
			}
			if (RobberC != null)
			{
				RobberC.Position = Game.Player.Character.Position;
			}
			Wait(500);
			GTA.UI.Screen.FadeIn(500);
			InHouseRobbery = false;
			EscapingHouseRobbery = true;
		}

		public static Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Wait(50);
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
					Wait(50);
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
					Wait(50);
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
					Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
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

		public void TEXTURE(string Textdict, string Textname, PointF Pos, SizeF Size, float Rot, Color Colour)
		{
			ScaledTexture scaledTexture = new ScaledTexture(Textdict, Textname);
			scaledTexture.Dictionary = Textdict;
			scaledTexture.Texture = Textname;
			scaledTexture.Position = Pos;
			scaledTexture.Size = Size;
			scaledTexture.Color = Color.Black;
			scaledTexture.Draw();
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
			hackingScalwform = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "Hacking_PC");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, hackingScalwform))
			{
				GTA.UI.Screen.ShowSubtitle("LOADING HACKING_PC SCALEFORM");
				hackingScalwform = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "Hacking_PC");
				Wait(1);
			}
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_CURSOR_VISIBILITY");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_0", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_0"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_1", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_1"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_2", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_2"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_3", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_3"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_4", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_4"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_5", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_5"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@keypad@");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@keypad@"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel@hacking");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel@hacking"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@ornate_bank@hack");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@ornate_bank@hack"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@keypad@");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@keypad@"))
			{
				Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel@hacking");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel@hacking"))
			{
				Wait(10);
			}
			Hacking_Index = 0;
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
			Hacking_Index = -1;
			Game.Player.Character.IsPositionFrozen = false;
			Wait(1000);
			Game.Player.Character.Task.Wait(0);
			Game.Player.Character.IsPositionFrozen = false;
		}

		public static string ReturnPedhash()
		{
			string result = "a_m_y_stbla_01";
			Random random = new Random();
			int num = random.Next(0, 200);
			if (num < 25)
			{
				result = "a_m_y_stbla_01";
			}
			if (num >= 25 && num < 50)
			{
				result = "a_m_y_stbla_02";
			}
			if (num >= 50 && num < 75)
			{
				result = "a_f_y_tourist_01";
			}
			if (num >= 75 && num < 100)
			{
				result = "a_f_y_tourist_02";
			}
			if (num >= 100 && num < 125)
			{
				result = "a_m_m_stlat_02";
			}
			if (num >= 125 && num < 150)
			{
				result = "a_m_y_genstreet_01";
			}
			if (num >= 150 && num < 175)
			{
				result = "a_f_y_business_03";
			}
			if (num >= 175 && num < 200)
			{
				result = "a_f_y_business_02";
			}
			return result;
		}

		public void LoadAudioDict(string dict)
		{
			Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, dict, 0, -1);
		}

		public static float increaseCurrentPay(int min, int max)
		{
			float num = 0f;
			Random random = new Random();
			if (min < max)
			{
				num = random.Next(min, max);
				ManageRobberyAnims.CurrentPay += (int)num;
			}
			if (min > max)
			{
				num = random.Next(max, min);
				ManageRobberyAnims.CurrentPay += (int)num;
			}
			return num;
		}

		public void MissionPassed(string MissionName, int Payout, int CrewCut, int CrewAlive, float Time, int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			float num4 = Time / 10.5f;
			MissionName = "The Fleeca Heist";
			num = 18500;
			num2 = 9900;
			num3 = 14500;
			string textname = "bronzemedal";
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num5 = Convert.ToInt32(absoluteX / 2f);
			TEXT("mission passed", new Point(num5, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(num5, 230), 1.25f, Color.White, GTA.UI.Font.HouseScript);
			TEXT("Player Cut : $" + Payout.ToString("N"), new Point(num5, 330), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TEXT("Crew Cut $" + CrewCut.ToString("N"), new Point(num5, 400), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			switch (CrewAlive)
			{
			case 3:
				textname = "silvermedal";
				break;
			case 4:
				textname = "goldmedal";
				break;
			}
			TEXTURE("mpmissionend", textname, new Point(num5 - 180, 470), new Size(46, 46), 0f, Color.White);
			TEXT("Crew Alive : " + CrewAlive + "/4", new Point(num5, 471), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			if (Time <= (float)num)
			{
				textname = "bronzemedal";
			}
			if (Time <= (float)num3)
			{
				textname = "silvermedal";
			}
			if (Time <= (float)num2)
			{
				textname = "goldmedal";
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds(num4);
			TEXTURE("mpmissionend", textname, new Point(num5 - 180, 551), new Size(46, 46), 0f, Color.White);
			TEXT("Time : " + timeSpan.ToString("hh':'mm':'ss"), new Point(num5, 550), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			float num6 = (float)num / 10.5f;
			float num7 = (float)num2 / 10.5f;
			float num8 = (float)num3 / 10.5f;
			textname = "goldmedal";
			TimeSpan timeSpan2 = TimeSpan.FromSeconds((float)num2 / 10.5f);
			TEXTURE("mpmissionend", textname, new Point(num5 - 300, 619), new Size(46, 46), 0f, Color.White);
			TEXT(timeSpan2.ToString("hh':'mm':'ss") ?? "", new Point(num5 - 200, 620), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TimeSpan timeSpan3 = TimeSpan.FromSeconds((float)num3 / 10.5f);
			textname = "silvermedal";
			TEXTURE("mpmissionend", textname, new Point(num5 - 100, 619), new Size(46, 46), 0f, Color.White);
			TEXT(timeSpan3.ToString("hh':'mm':'ss") ?? "", new Point(num5, 620), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TimeSpan timeSpan4 = TimeSpan.FromSeconds((float)num / 10.5f);
			textname = "bronzemedal";
			TEXTURE("mpmissionend", textname, new Point(num5 + 100, 619), new Size(46, 46), 0f, Color.White);
			TEXT(timeSpan4.ToString("hh':'mm':'ss") ?? "", new Point(num5 + 200, 620), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void MissionFailed(string Reason, string MissionName, int CrewCut, int CrewAlive, float Time, int Heist)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			float num4 = Time / 10.5f;
			MissionName = "The Fleeca Heist";
			num = 18500;
			num2 = 9900;
			num3 = 14500;
			Pass = false;
			Fail = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			Notification.Show("Width " + Convert.ToInt32(absoluteX));
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.White);
			TEXT("mission failed", new Point(x, 100), 2.5f, Color.FromArgb(255, 148, 27, 46), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(x, 230), 1.25f, Color.White, GTA.UI.Font.HouseScript);
			TEXT("Reason : " + Reason, new Point(x, 310), 0.5f, Color.White, GTA.UI.Font.ChaletComprimeCologne);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public void WAIT(int Delay)
		{
			Wait(Delay);
		}

		public void SetFail(string MissionName, int Payout, int CrewCut, string Reason, int CrewAlive, float Time, int Heist)
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Bed", "WastedSounds");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300f;
			m = MissionName;
			p = Payout;
			t = Time;
			c = CrewAlive;
			r = Reason;
			H = Heist;
			cp = CrewCut;
			Fail = true;
		}

		public void SetPass(string MissionName, int Payout, int CrewCut, string Reason, int CrewAlive, float Time, int Heist)
		{
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300f;
			m = MissionName;
			p = Payout;
			t = Time;
			c = CrewAlive;
			r = Reason;
			H = Heist;
			cp = CrewCut;
			Pass = true;
			for (int i = 0; i < 300; i++)
			{
				if (Fail)
				{
					MissionFailed(r, m, cp, c, t, H);
				}
				if (Pass)
				{
					MissionPassed(m, p, cp, c, t, H);
				}
				WAIT(1);
			}
		}

		public void StealJewels()
		{
			if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-624f, -232f, 38f), "<", 50f))
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-624f, -232f, 38f), "<", 30f))
				{
					bool flag = true;
					if (Cameras.Count == 4)
					{
						for (int i = 0; i < Cameras.Count; i++)
						{
							Prop prop = Cameras[i];
							if (prop != null)
							{
								if (prop.Health < 999)
								{
									CameraBlips[i].Alpha = 0;
								}
								if (prop.Health > 999)
								{
									flag = false;
									break;
								}
								int health = prop.Health;
								int num17 = 995;
							}
						}
						if (!flag)
						{
							GTA.UI.Screen.ShowSubtitle("Destroy the ~r~Cameras~w~");
						}
					}
				}
				bool gotJewel = GotJewels;
			}
			if (JewelsCases.Count == 20)
			{
				if (RobberA != null && RobberACurrentCabinet != null)
				{
					if (RobberACurrentCabinet.Model == RequestModel("des_jewel_cab_start") && GetDistance_VDIST2(RobberA.Position, RobberA_RobberMarker, "<", 1.25f) && !RobberARobbing)
					{
						Random random = new Random();
						int num = random.Next(0, 100);
						if (num < 50)
						{
							RobberAJewelSmashType = 1;
							RobberAMovingToLoot = false;
							RobberAJewelType = 1;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberA.Position = RobberACurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation = RobberACurrentCabinet.Rotation;
							RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberANearsetCabinet = RobberACurrentCabinet;
							RobberARobbing = true;
						}
						if (num >= 50)
						{
							RobberAJewelSmashType = 2;
							RobberAMovingToLoot = false;
							RobberAJewelType = 1;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberA.Position = RobberACurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation2 = RobberACurrentCabinet.Rotation;
							RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation2.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation2.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberANearsetCabinet = RobberACurrentCabinet;
							RobberARobbing = true;
						}
					}
					if (RobberACurrentCabinet.Model == RequestModel("des_jewel_cab2_start") && GetDistance_VDIST2(RobberA.Position, RobberA_RobberMarker, "<", 1.25f) && !RobberARobbing)
					{
						Random random2 = new Random();
						int num2 = random2.Next(0, 100);
						if (num2 < 50)
						{
							RobberAJewelSmashType = 1;
							RobberAMovingToLoot = false;
							RobberAJewelType = 2;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberA.Position = RobberACurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation3 = RobberACurrentCabinet.Rotation;
							RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation3.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation3.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberANearsetCabinet = RobberACurrentCabinet;
							RobberARobbing = true;
						}
						if (num2 >= 50)
						{
							RobberAJewelSmashType = 2;
							RobberAMovingToLoot = false;
							RobberAJewelType = 2;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberA.Position = RobberACurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation4 = RobberACurrentCabinet.Rotation;
							RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation4.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation4.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberANearsetCabinet = RobberACurrentCabinet;
							RobberARobbing = true;
						}
					}
					if (RobberACurrentCabinet.Model == RequestModel("des_jewel_cab3_start") && GetDistance_VDIST2(RobberA.Position, RobberA_RobberMarker, "<", 1.25f) && !RobberARobbing)
					{
						Random random3 = new Random();
						int num3 = random3.Next(0, 100);
						if (num3 < 50)
						{
							RobberAJewelSmashType = 1;
							RobberAMovingToLoot = false;
							RobberAJewelType = 3;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberA.Position = RobberACurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation5 = RobberACurrentCabinet.Rotation;
							RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation5.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation5.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberANearsetCabinet = RobberACurrentCabinet;
							RobberARobbing = true;
						}
						if (num3 >= 50)
						{
							RobberAJewelSmashType = 2;
							RobberAMovingToLoot = false;
							RobberAJewelType = 3;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberA.Position = RobberACurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation6 = RobberACurrentCabinet.Rotation;
							RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation6.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, rotation6.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict("missheist_jewel"), "smash_case_tray_ba", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberANearsetCabinet = RobberACurrentCabinet;
							RobberARobbing = true;
						}
					}
					if (RobberACurrentCabinet.Model == RequestModel("des_jewel_cab4_start") && GetDistance_VDIST2(RobberA.Position, RobberA_RobberMarker, "<", 1.25f) && !RobberARobbing)
					{
						Random random4 = new Random();
						int num4 = random4.Next(0, 100);
						if (num4 < 50)
						{
							RobberAJewelSmashType = 1;
							RobberAMovingToLoot = false;
							RobberAJewelType = 4;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberA.Position = RobberACurrentCabinet.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
							Vector3 vector = new Vector3(RobberACurrentCabinet.Rotation.X, RobberACurrentCabinet.Rotation.Y, RobberACurrentCabinet.Rotation.Z + 90f);
							RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, vector.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, vector.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberANearsetCabinet = RobberACurrentCabinet;
							RobberARobbing = true;
						}
						if (num4 >= 50)
						{
							RobberAJewelSmashType = 2;
							RobberAMovingToLoot = false;
							RobberAJewelType = 4;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberA.Position = RobberACurrentCabinet.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
							Vector3 vector2 = new Vector3(RobberACurrentCabinet.Rotation.X, RobberACurrentCabinet.Rotation.Y, RobberACurrentCabinet.Rotation.Z + 90f);
							RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, vector2.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberA.Position.X, RobberA.Position.Y, RobberA.Position.Z - 1.1f, 0.0, 0.0, vector2.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberANearsetCabinet = RobberACurrentCabinet;
							RobberARobbing = true;
						}
					}
				}
				if (RobberB != null && RobberBCurrentCabinet != null)
				{
					if (RobberBCurrentCabinet.Model == RequestModel("des_jewel_cab_start") && GetDistance_VDIST2(RobberB.Position, RobberB_RobberMarker, "<", 1.25f) && !RobberBRobbing)
					{
						Random random5 = new Random();
						int num5 = random5.Next(0, 100);
						if (num5 < 50)
						{
							RobberBJewelSmashType = 1;
							RobberBMovingToLoot = false;
							RobberBJewelType = 1;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberB.Position = RobberBCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation7 = RobberBCurrentCabinet.Rotation;
							RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation7.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation7.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberBNearsetCabinet = RobberBCurrentCabinet;
							RobberBRobbing = true;
						}
						if (num5 >= 50)
						{
							RobberBJewelSmashType = 2;
							RobberBMovingToLoot = false;
							RobberBJewelType = 1;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberB.Position = RobberBCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation8 = RobberBCurrentCabinet.Rotation;
							RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation8.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation8.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberBNearsetCabinet = RobberBCurrentCabinet;
							RobberBRobbing = true;
						}
					}
					if (RobberBCurrentCabinet.Model == RequestModel("des_jewel_cab2_start") && GetDistance_VDIST2(RobberB.Position, RobberB_RobberMarker, "<", 1.25f) && !RobberBRobbing)
					{
						Random random6 = new Random();
						int num6 = random6.Next(0, 100);
						if (num6 < 50)
						{
							RobberBJewelSmashType = 1;
							RobberBMovingToLoot = false;
							RobberBJewelType = 2;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberB.Position = RobberBCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation9 = RobberBCurrentCabinet.Rotation;
							RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation9.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation9.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberBNearsetCabinet = RobberBCurrentCabinet;
							RobberBRobbing = true;
						}
						if (num6 >= 50)
						{
							RobberBJewelSmashType = 2;
							RobberBMovingToLoot = false;
							RobberBJewelType = 2;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberB.Position = RobberBCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation10 = RobberBCurrentCabinet.Rotation;
							RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation10.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation10.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberBNearsetCabinet = RobberBCurrentCabinet;
							RobberBRobbing = true;
						}
					}
					if (RobberBCurrentCabinet.Model == RequestModel("des_jewel_cab3_start") && GetDistance_VDIST2(RobberB.Position, RobberB_RobberMarker, "<", 1.25f) && !RobberBRobbing)
					{
						Random random7 = new Random();
						int num7 = random7.Next(0, 100);
						if (num7 < 50)
						{
							RobberBJewelSmashType = 1;
							RobberBMovingToLoot = false;
							RobberBJewelType = 3;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberB.Position = RobberBCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation11 = RobberBCurrentCabinet.Rotation;
							RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation11.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation11.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberBNearsetCabinet = RobberBCurrentCabinet;
							RobberBRobbing = true;
						}
						if (num7 >= 50)
						{
							RobberBJewelSmashType = 2;
							RobberBMovingToLoot = false;
							RobberBJewelType = 3;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberB.Position = RobberBCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation12 = RobberBCurrentCabinet.Rotation;
							RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation12.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, rotation12.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_ba", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberBNearsetCabinet = RobberBCurrentCabinet;
							RobberBRobbing = true;
						}
					}
					if (RobberBCurrentCabinet.Model == RequestModel("des_jewel_cab4_start") && GetDistance_VDIST2(RobberB.Position, RobberB_RobberMarker, "<", 1.25f) && !RobberBRobbing)
					{
						Random random8 = new Random();
						int num8 = random8.Next(0, 100);
						if (num8 < 50)
						{
							RobberBJewelSmashType = 1;
							RobberBMovingToLoot = false;
							RobberBJewelType = 4;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberB.Position = RobberBCurrentCabinet.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
							Vector3 vector3 = new Vector3(RobberBCurrentCabinet.Rotation.X, RobberBCurrentCabinet.Rotation.Y, RobberBCurrentCabinet.Rotation.Z + 90f);
							RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, vector3.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, vector3.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberBNearsetCabinet = RobberBCurrentCabinet;
							RobberBRobbing = true;
						}
						if (num8 >= 50)
						{
							RobberBJewelSmashType = 2;
							RobberBMovingToLoot = false;
							RobberBJewelType = 4;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberB.Position = RobberBCurrentCabinet.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
							Vector3 vector4 = new Vector3(RobberBCurrentCabinet.Rotation.X, RobberBCurrentCabinet.Rotation.Y, RobberBCurrentCabinet.Rotation.Z + 90f);
							RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, vector4.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberB.Position.X, RobberB.Position.Y, RobberB.Position.Z - 1.1f, 0.0, 0.0, vector4.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberBNearsetCabinet = RobberBCurrentCabinet;
							RobberBRobbing = true;
						}
					}
				}
				if (RobberC != null && RobberCCurrentCabinet != null)
				{
					if (RobberCCurrentCabinet.Model == RequestModel("des_jewel_cab_start") && GetDistance_VDIST2(RobberC.Position, RobberC_RobberMarker, "<", 1.25f) && !RobberCRobbing)
					{
						Random random9 = new Random();
						int num9 = random9.Next(0, 100);
						if (num9 < 50)
						{
							RobberCJewelSmashType = 1;
							RobberCMovingToLoot = false;
							RobberCJewelType = 1;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberC.Position = RobberCCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation13 = RobberCCurrentCabinet.Rotation;
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation13.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation13.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberCNearsetCabinet = RobberCCurrentCabinet;
							RobberCRobbing = true;
						}
						if (num9 >= 50)
						{
							RobberCJewelSmashType = 2;
							RobberCMovingToLoot = false;
							RobberCJewelType = 1;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberC.Position = RobberCCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation14 = RobberCCurrentCabinet.Rotation;
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation14.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation14.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberCNearsetCabinet = RobberCCurrentCabinet;
							RobberCRobbing = true;
						}
					}
					if (RobberCCurrentCabinet.Model == RequestModel("des_jewel_cab2_start") && GetDistance_VDIST2(RobberC.Position, RobberC_RobberMarker, "<", 1.25f) && !RobberCRobbing)
					{
						Random random10 = new Random();
						int num10 = random10.Next(0, 100);
						if (num10 < 50)
						{
							RobberCJewelSmashType = 1;
							RobberCMovingToLoot = false;
							RobberCJewelType = 2;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberC.Position = RobberCCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation15 = RobberCCurrentCabinet.Rotation;
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation15.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation15.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberCNearsetCabinet = RobberCCurrentCabinet;
							RobberCRobbing = true;
						}
						if (num10 >= 50)
						{
							RobberCJewelSmashType = 2;
							RobberCMovingToLoot = false;
							RobberCJewelType = 2;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberC.Position = RobberCCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation16 = RobberCCurrentCabinet.Rotation;
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation16.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation16.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberCNearsetCabinet = RobberCCurrentCabinet;
							RobberCRobbing = true;
						}
					}
					if (RobberCCurrentCabinet.Model == RequestModel("des_jewel_cab3_start") && GetDistance_VDIST2(RobberC.Position, RobberC_RobberMarker, "<", 1.25f) && !RobberCRobbing)
					{
						Random random11 = new Random();
						int num11 = random11.Next(0, 100);
						if (num11 < 50)
						{
							RobberCJewelSmashType = 1;
							RobberCMovingToLoot = false;
							RobberCJewelType = 3;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberC.Position = RobberCCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation17 = RobberCCurrentCabinet.Rotation;
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation17.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation17.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberCNearsetCabinet = RobberCCurrentCabinet;
							RobberCRobbing = true;
						}
						if (num11 >= 50)
						{
							RobberCJewelSmashType = 2;
							RobberCMovingToLoot = false;
							RobberCJewelType = 3;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberC.Position = RobberCCurrentCabinet.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
							Vector3 rotation18 = RobberCCurrentCabinet.Rotation;
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation18.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, rotation18.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_ba", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberCNearsetCabinet = RobberCCurrentCabinet;
							RobberCRobbing = true;
						}
					}
					if (RobberCCurrentCabinet.Model == RequestModel("des_jewel_cab4_start") && GetDistance_VDIST2(RobberC.Position, RobberC_RobberMarker, "<", 1.25f) && !RobberCRobbing)
					{
						Random random12 = new Random();
						int num12 = random12.Next(0, 100);
						if (num12 < 50)
						{
							RobberCJewelSmashType = 1;
							RobberCMovingToLoot = false;
							RobberCJewelType = 4;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberC.Position = RobberCCurrentCabinet.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
							Vector3 vector5 = new Vector3(RobberCCurrentCabinet.Rotation.X, RobberCCurrentCabinet.Rotation.Y, RobberCCurrentCabinet.Rotation.Z + 90f);
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, vector5.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, vector5.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberCNearsetCabinet = RobberCCurrentCabinet;
							RobberCRobbing = true;
						}
						if (num12 >= 50)
						{
							RobberCJewelSmashType = 2;
							RobberCMovingToLoot = false;
							RobberCJewelType = 4;
							LoadDict("missheist_jewel");
							LoadDict("missheist_jewel");
							RobberC.Position = RobberCCurrentCabinet.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
							Vector3 vector6 = new Vector3(RobberCCurrentCabinet.Rotation.X, RobberCCurrentCabinet.Rotation.Y, RobberCCurrentCabinet.Rotation.Z + 90f);
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, vector6.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberC.Position.X, RobberC.Position.Y, RobberC.Position.Z - 1.1f, 0.0, 0.0, vector6.Z, true);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
							RobberCNearsetCabinet = RobberCCurrentCabinet;
							RobberCRobbing = true;
						}
					}
				}
			}
			if (JewelsCases.Count != 20)
			{
				return;
			}
			for (int j = 0; j < JewelsCases.Count; j++)
			{
				Prop prop2 = JewelsCases[j];
				if (prop2.Model == RequestModel("des_jewel_cab_start") && !RobbedJewelsCases[j])
				{
					Vector3 offsetPosition = prop2.GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
					Vector3 offsetPosition2 = prop2.GetOffsetPosition(new Vector3(0f, -0.9f, 0.5f));
					Vector3 offsetPosition3 = prop2.GetOffsetPosition(new Vector3(0f, 0f, -1.5f));
					if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition2, "<", 0.75f))
					{
						DisplayHelpTextThisFrame("Press " + ReturnButtonString(Key_HeistGrabLoot) + " to Steal Jewelery");
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition2, "<", 0.75f))
					{
						World.DrawMarker(MarkerType.Cylinder, offsetPosition, Vector3.Zero, Vector3.Zero, new Vector3(0.4f, 0.4f, 0.4f), Color.Red);
						if (Game.IsControlJustPressed((Control)ReturnButtonID(Key_HeistGrabLoot)) && !isRobbingCase)
						{
							Random random13 = new Random();
							int num13 = random13.Next(0, 100);
							if (num13 < 50)
							{
								PlayerJewelSmashType = 1;
								isRobbingCase = true;
								JewelType = 1;
								LoadDict("missheist_jewel");
								LoadDict("missheist_jewel");
								Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
								Vector3 rotation19 = prop2.Rotation;
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation19.Z, 2);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation19.Z, true);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
								NearestCabinet = prop2;
								RobbedJewelsCases[j] = true;
								RobbedJewelsCasesBlips[j].Alpha = 0;
								break;
							}
							if (num13 >= 50)
							{
								PlayerJewelSmashType = 2;
								isRobbingCase = true;
								JewelType = 1;
								LoadDict("missheist_jewel");
								LoadDict("missheist_jewel");
								Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
								Vector3 rotation20 = prop2.Rotation;
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation20.Z, 2);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation20.Z, true);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
								NearestCabinet = prop2;
								RobbedJewelsCases[j] = true;
								RobbedJewelsCasesBlips[j].Alpha = 0;
								break;
							}
						}
					}
				}
				if (prop2.Model == RequestModel("des_jewel_cab2_start") && !RobbedJewelsCases[j])
				{
					Vector3 offsetPosition4 = prop2.GetOffsetPosition(new Vector3(0f, -0.9f, 0.5f));
					Vector3 offsetPosition5 = prop2.GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
					Vector3 offsetPosition6 = prop2.GetOffsetPosition(new Vector3(0f, 0f, -1.5f));
					if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition4, "<", 0.75f))
					{
						DisplayHelpTextThisFrame("Press " + ReturnButtonString(Key_HeistGrabLoot) + " to Steal Jewelery");
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition4, "<", 0.75f))
					{
						World.DrawMarker(MarkerType.Cylinder, offsetPosition5, Vector3.Zero, Vector3.Zero, new Vector3(0.4f, 0.4f, 0.4f), Color.Red);
						if (Game.IsControlJustPressed((Control)ReturnButtonID(Key_HeistGrabLoot)) && !isRobbingCase)
						{
							Random random14 = new Random();
							int num14 = random14.Next(0, 100);
							if (num14 < 50)
							{
								PlayerJewelSmashType = 1;
								isRobbingCase = true;
								JewelType = 2;
								LoadDict("missheist_jewel");
								LoadDict("missheist_jewel");
								Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
								Vector3 rotation21 = prop2.Rotation;
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation21.Z, 2);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation21.Z, true);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
								NearestCabinet = prop2;
								RobbedJewelsCases[j] = true;
								RobbedJewelsCasesBlips[j].Alpha = 0;
								break;
							}
							if (num14 >= 50)
							{
								PlayerJewelSmashType = 2;
								isRobbingCase = true;
								JewelType = 2;
								LoadDict("missheist_jewel");
								LoadDict("missheist_jewel");
								Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
								Vector3 rotation22 = prop2.Rotation;
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation22.Z, 2);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation22.Z, true);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
								NearestCabinet = prop2;
								RobbedJewelsCases[j] = true;
								RobbedJewelsCasesBlips[j].Alpha = 0;
								break;
							}
						}
					}
				}
				if (prop2.Model == RequestModel("des_jewel_cab3_start") && !RobbedJewelsCases[j])
				{
					Vector3 offsetPosition7 = prop2.GetOffsetPosition(new Vector3(0f, -0.9f, 0.5f));
					Vector3 offsetPosition8 = prop2.GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
					Vector3 offsetPosition9 = prop2.GetOffsetPosition(new Vector3(0f, 0f, -1.5f));
					if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition7, "<", 0.75f))
					{
						DisplayHelpTextThisFrame("Press " + ReturnButtonString(Key_HeistGrabLoot) + " to Steal Jewelery");
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition7, "<", 0.75f))
					{
						World.DrawMarker(MarkerType.Cylinder, offsetPosition8, Vector3.Zero, Vector3.Zero, new Vector3(0.4f, 0.4f, 0.4f), Color.Red);
						if (Game.IsControlJustPressed((Control)ReturnButtonID(Key_HeistGrabLoot)) && !isRobbingCase)
						{
							Random random15 = new Random();
							int num15 = random15.Next(0, 100);
							if (num15 < 50)
							{
								PlayerJewelSmashType = 1;
								isRobbingCase = true;
								JewelType = 3;
								LoadDict("missheist_jewel");
								LoadDict("missheist_jewel");
								Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
								Vector3 rotation23 = prop2.Rotation;
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation23.Z, 2);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation23.Z, true);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
								NearestCabinet = prop2;
								RobbedJewelsCases[j] = true;
								RobbedJewelsCasesBlips[j].Alpha = 0;
								break;
							}
							if (num15 >= 50)
							{
								PlayerJewelSmashType = 1;
								isRobbingCase = true;
								JewelType = 3;
								LoadDict("missheist_jewel");
								LoadDict("missheist_jewel");
								Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(0f, -1.2f, -0.5f));
								Vector3 rotation24 = prop2.Rotation;
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation24.Z, 2);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, rotation24.Z, true);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
								NearestCabinet = prop2;
								RobbedJewelsCases[j] = true;
								RobbedJewelsCasesBlips[j].Alpha = 0;
								break;
							}
						}
					}
				}
				if (!(prop2.Model == RequestModel("des_jewel_cab4_start")) || RobbedJewelsCases[j])
				{
					continue;
				}
				Vector3 offsetPosition10 = prop2.GetOffsetPosition(new Vector3(0.9f, 0f, 0.5f));
				Vector3 offsetPosition11 = prop2.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
				Vector3 offsetPosition12 = prop2.GetOffsetPosition(new Vector3(0f, 0f, -1.5f));
				if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition10, "<", 0.75f))
				{
					DisplayHelpTextThisFrame("Press " + ReturnButtonString(Key_HeistGrabLoot) + " to Steal Jewelery");
				}
				if (!GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition10, "<", 0.75f))
				{
					continue;
				}
				World.DrawMarker(MarkerType.Cylinder, offsetPosition11, Vector3.Zero, Vector3.Zero, new Vector3(0.4f, 0.4f, 0.4f), Color.Red);
				if (Game.IsControlJustPressed((Control)ReturnButtonID(Key_HeistGrabLoot)) && !isRobbingCase)
				{
					Random random16 = new Random();
					int num16 = random16.Next(0, 100);
					if (num16 < 50)
					{
						PlayerJewelSmashType = 1;
						isRobbingCase = true;
						JewelType = 4;
						LoadDict("missheist_jewel");
						LoadDict("missheist_jewel");
						Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
						Vector3 vector7 = new Vector3(prop2.Rotation.X, prop2.Rotation.Y, prop2.Rotation.Z + 90f);
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, vector7.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, vector7.Z, true);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missheist_jewel"), "smash_case_tray_a", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "smash_case_tray_a", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
						NearestCabinet = prop2;
						RobbedJewelsCases[j] = true;
						RobbedJewelsCasesBlips[j].Alpha = 0;
						break;
					}
					if (num16 >= 50)
					{
						PlayerJewelSmashType = 2;
						isRobbingCase = true;
						JewelType = 4;
						LoadDict("missheist_jewel");
						LoadDict("missheist_jewel");
						Game.Player.Character.Position = prop2.GetOffsetPosition(new Vector3(0.9f, 0f, -0.5f));
						Vector3 vector8 = new Vector3(prop2.Rotation.X, prop2.Rotation.Y, prop2.Rotation.Z + 90f);
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, vector8.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z - 1.1f, 0.0, 0.0, vector8.Z, true);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("missheist_jewel"), "smash_case_tray_b", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "smash_case_tray_b", LoadDict("missheist_jewel"), 16f, -16f, 0, 1148846080);
						NearestCabinet = prop2;
						RobbedJewelsCases[j] = true;
						RobbedJewelsCasesBlips[j].Alpha = 0;
						break;
					}
				}
			}
		}

		public void CheckJewels()
		{
			try
			{
				if (JewelStoreStage == HeistStage.Stage1 && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-624f, -232f, 38f), ">", 30f))
				{
					GTA.UI.Screen.ShowSubtitle("Approach the ~y~Jewel Store~w~");
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-624f, -232f, 38f), "<", 30f))
				{
					bool flag = true;
					if (Cameras.Count == 4)
					{
						for (int i = 0; i < Cameras.Count; i++)
						{
							Prop prop = Cameras[i];
							if (prop != null)
							{
								if (prop.Health < 1000)
								{
									CameraBlips[i].Alpha = 0;
								}
								if (prop.Health > 1000)
								{
									flag = false;
									break;
								}
							}
						}
						if (!flag)
						{
							GTA.UI.Screen.ShowSubtitle("Destroy the ~r~Cameras~w~");
						}
					}
				}
				if (JewelsCases.Count != 20)
				{
					return;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1045050216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1048353216)
				{
					if (PlayerJewelSmashType == 1)
					{
						Vector3 offsetPosition = NearestCabinet.GetOffsetPosition(new Vector3(0f, 0f, -1.5f));
						int num = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass");
						int num2 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
						NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 5f, num, 0, 0, 0);
						if (GetDistance_VDIST2(NearestCabinet.Position, NearestGlass.Position, "<", 2f))
						{
							NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 5f, num, 0, 0, 0);
							NearestGlass.Opacity = 0;
						}
						if (GetDistance_VDIST2(NearestCabinet.Position, NearestGlass.Position, ">", 2f))
						{
							num2 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
							NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 5f, num2, 0, 0, 0);
							NearestGlass.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, "scr_jewel_cab_smash", NearestGlass, 0f, 0f, 0.5f, 0f, 0f, NearestGlass.Heading - 90f, 1.15f, false, false, false);
						LoadAudioDict("SCRIPT/JWL_HEIST_RAID_GLASS_SMASH");
						LoadAudioDict("SCRIPT/JEWEL_HEIST_SOUNDS");
						LoadAudioDict("JEWEL_HEIST_SOUNDS");
						soundid = GlobalHelper.CreateSoundID();
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, "JEWEL_HEIST_SOUNDS", false, 10, false);
						if (JewelType == 1)
						{
							int num3 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start");
							Prop prop2 = (NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, 2f, num3, 0, 0, 0));
							prop2.Opacity = 0;
							Prop prop3 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab_end"), new Vector3(prop2.Position.X, prop2.Position.Y, prop2.Position.Z - 0.6f), new Vector3(prop2.Rotation.X, prop2.Rotation.Y, prop2.Rotation.Z), false, false);
							prop3.IsPositionFrozen = true;
							Jewels.Add(prop3);
						}
						if (JewelType == 2)
						{
							int num4 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start");
							Prop prop4 = (NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, 2f, num4, 0, 0, 0));
							prop4.Opacity = 0;
							Prop prop5 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab2_end"), new Vector3(prop4.Position.X, prop4.Position.Y, prop4.Position.Z - 0.6f), new Vector3(prop4.Rotation.X, prop4.Rotation.Y, prop4.Rotation.Z), false, false);
							prop5.IsPositionFrozen = true;
							Jewels.Add(prop5);
						}
						if (JewelType == 3)
						{
							int num5 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start");
							Prop prop6 = (NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, 2f, num5, 0, 0, 0));
							prop6.Opacity = 0;
							Prop prop7 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab3_end"), new Vector3(prop6.Position.X, prop6.Position.Y, prop6.Position.Z - 0.6f), new Vector3(prop6.Rotation.X, prop6.Rotation.Y, prop6.Rotation.Z), false, false);
							prop7.IsPositionFrozen = true;
							Jewels.Add(prop7);
						}
						if (JewelType == 4)
						{
							int num6 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start");
							Prop prop8 = (NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, 2f, num6, 0, 0, 0));
							prop8.Opacity = 0;
							Prop prop9 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab3_end"), new Vector3(prop8.Position.X, prop8.Position.Y, prop8.Position.Z - 0.6f), new Vector3(prop8.Rotation.X, prop8.Rotation.Y, prop8.Rotation.Z + 90f), false, false);
							prop9.IsPositionFrozen = true;
							Jewels.Add(prop9);
						}
					}
					if (PlayerJewelSmashType == 2)
					{
						Vector3 offsetPosition2 = NearestCabinet.GetOffsetPosition(new Vector3(0f, 0f, -1.5f));
						int num7 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass");
						int num8 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
						NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 5f, num7, 0, 0, 0);
						if (GetDistance_VDIST2(NearestCabinet.Position, NearestGlass.Position, "<", 2f))
						{
							NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 5f, num7, 0, 0, 0);
							NearestGlass.Opacity = 0;
						}
						if (GetDistance_VDIST2(NearestCabinet.Position, NearestGlass.Position, ">", 2f))
						{
							num8 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
							NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 5f, num8, 0, 0, 0);
							NearestGlass.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, "scr_jewel_cab_smash", NearestGlass, 0f, 0f, 0.5f, 0f, 0f, NearestGlass.Heading - 90f, 1.15f, false, false, false);
						LoadAudioDict("SCRIPT/JWL_HEIST_RAID_GLASS_SMASH");
						LoadAudioDict("SCRIPT/JEWEL_HEIST_SOUNDS");
						LoadAudioDict("JEWEL_HEIST_SOUNDS");
						soundid = GlobalHelper.CreateSoundID();
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, "JEWEL_HEIST_SOUNDS", false, 10, false);
						if (JewelType == 1)
						{
							int num9 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start");
							Prop prop10 = (NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, 2f, num9, 0, 0, 0));
							prop10.Opacity = 0;
							Prop prop11 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop10.Position.X, prop10.Position.Y, prop10.Position.Z - 0.6f), new Vector3(prop10.Rotation.X, prop10.Rotation.Y, prop10.Rotation.Z - 90f), false, false);
							prop11.IsPositionFrozen = true;
							Jewels.Add(prop11);
						}
						if (JewelType == 2)
						{
							int num10 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start");
							Prop prop12 = (NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, 2f, num10, 0, 0, 0));
							prop12.Opacity = 0;
							Prop prop13 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop12.Position.X, prop12.Position.Y, prop12.Position.Z - 0.6f), new Vector3(prop12.Rotation.X, prop12.Rotation.Y, prop12.Rotation.Z - 90f), false, false);
							prop13.IsPositionFrozen = true;
							Jewels.Add(prop13);
						}
						if (JewelType == 3)
						{
							int num11 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start");
							Prop prop14 = (NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, 2f, num11, 0, 0, 0));
							prop14.Opacity = 0;
							Prop prop15 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop14.Position.X, prop14.Position.Y, prop14.Position.Z - 0.6f), new Vector3(prop14.Rotation.X, prop14.Rotation.Y, prop14.Rotation.Z), false, false);
							prop15.IsPositionFrozen = true;
							Jewels.Add(prop15);
						}
						if (JewelType == 4)
						{
							int num12 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start");
							Prop prop16 = (NearestGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, NearestCabinet.Position.X, NearestCabinet.Position.Y, NearestCabinet.Position.Z, 2f, num12, 0, 0, 0));
							prop16.Opacity = 0;
							Prop prop17 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop16.Position.X, prop16.Position.Y, prop16.Position.Z - 0.6f), new Vector3(prop16.Rotation.X, prop16.Rotation.Y, prop16.Rotation.Z), false, false);
							prop17.IsPositionFrozen = true;
							Jewels.Add(prop17);
						}
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
				{
					ManageRobberyAnims.CurrentPay += (int)increaseCurrentPay(HeistMinPay / 15, HeistMaxPay / 15);
					Notification.Show("Pay " + ManageRobberyAnims.CurrentPay);
					Function.Call(Hash.STOP_SOUND, soundid);
					Function.Call(Hash.RELEASE_SOUND_ID, soundid);
					Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
					Audio.PlaySoundFrontend("LOCAL_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
					Game.Player.Character.Task.ClearAll();
					isRobbingCase = false;
				}
				if (RobberA != null && RobberARobbing)
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1045050216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) <= 1048353216)
					{
						if (RobberAJewelSmashType == 1)
						{
							Vector3 offsetPosition3 = RobberANearsetCabinet.GetOffsetPosition(new Vector3(0f, 0f, -1.5f));
							int num13 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass");
							int num14 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
							RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 5f, num13, 0, 0, 0);
							if (GetDistance_VDIST2(RobberANearsetCabinet.Position, RobberANearsetGlass.Position, "<", 2f))
							{
								RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 5f, num13, 0, 0, 0);
								RobberANearsetGlass.Opacity = 0;
							}
							if (GetDistance_VDIST2(RobberANearsetCabinet.Position, RobberANearsetGlass.Position, ">", 2f))
							{
								num14 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
								RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 5f, num14, 0, 0, 0);
								RobberANearsetGlass.Opacity = 0;
							}
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, "scr_jewel_cab_smash", RobberANearsetGlass, 0f, 0f, 0.5f, 0f, 0f, RobberANearsetGlass.Heading - 90f, 1.15f, false, false, false);
							LoadAudioDict("SCRIPT/JWL_HEIST_RAID_GLASS_SMASH");
							LoadAudioDict("SCRIPT/JEWEL_HEIST_SOUNDS");
							LoadAudioDict("JEWEL_HEIST_SOUNDS");
							soundid = GlobalHelper.CreateSoundID();
							Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, "JEWEL_HEIST_SOUNDS", false, 10, false);
							if (RobberAJewelType == 1)
							{
								int num15 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start");
								Prop prop18 = (RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, 2f, num15, 0, 0, 0));
								prop18.Opacity = 0;
								Prop prop19 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab_end"), new Vector3(prop18.Position.X, prop18.Position.Y, prop18.Position.Z - 0.6f), new Vector3(prop18.Rotation.X, prop18.Rotation.Y, prop18.Rotation.Z), false, false);
								prop19.IsPositionFrozen = true;
								Jewels.Add(prop19);
							}
							if (RobberAJewelType == 2)
							{
								int num16 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start");
								Prop prop20 = (RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, 2f, num16, 0, 0, 0));
								prop20.Opacity = 0;
								Prop prop21 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab2_end"), new Vector3(prop20.Position.X, prop20.Position.Y, prop20.Position.Z - 0.6f), new Vector3(prop20.Rotation.X, prop20.Rotation.Y, prop20.Rotation.Z), false, false);
								prop21.IsPositionFrozen = true;
								Jewels.Add(prop21);
							}
							if (RobberAJewelType == 3)
							{
								int num17 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start");
								Prop prop22 = (RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, 2f, num17, 0, 0, 0));
								prop22.Opacity = 0;
								Prop prop23 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab3_end"), new Vector3(prop22.Position.X, prop22.Position.Y, prop22.Position.Z - 0.6f), new Vector3(prop22.Rotation.X, prop22.Rotation.Y, prop22.Rotation.Z), false, false);
								prop23.IsPositionFrozen = true;
								Jewels.Add(prop23);
							}
							if (RobberAJewelType == 4)
							{
								int num18 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start");
								Prop prop24 = (RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, 2f, num18, 0, 0, 0));
								prop24.Opacity = 0;
								Prop prop25 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab3_end"), new Vector3(prop24.Position.X, prop24.Position.Y, prop24.Position.Z - 0.6f), new Vector3(prop24.Rotation.X, prop24.Rotation.Y, prop24.Rotation.Z + 90f), false, false);
								prop25.IsPositionFrozen = true;
								Jewels.Add(prop25);
							}
						}
						if (RobberAJewelSmashType == 2)
						{
							Vector3 offsetPosition4 = RobberANearsetCabinet.GetOffsetPosition(new Vector3(0f, 0f, -1.5f));
							int num19 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass");
							int num20 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
							RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 5f, num19, 0, 0, 0);
							if (GetDistance_VDIST2(RobberANearsetCabinet.Position, RobberANearsetGlass.Position, "<", 2f))
							{
								RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 5f, num19, 0, 0, 0);
								RobberANearsetGlass.Opacity = 0;
							}
							if (GetDistance_VDIST2(RobberANearsetCabinet.Position, RobberANearsetGlass.Position, ">", 2f))
							{
								num20 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
								RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 5f, num20, 0, 0, 0);
								RobberANearsetGlass.Opacity = 0;
							}
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, "scr_jewel_cab_smash", RobberANearsetGlass, 0f, 0f, 0.5f, 0f, 0f, RobberANearsetGlass.Heading - 90f, 1.15f, false, false, false);
							LoadAudioDict("SCRIPT/JWL_HEIST_RAID_GLASS_SMASH");
							LoadAudioDict("SCRIPT/JEWEL_HEIST_SOUNDS");
							LoadAudioDict("JEWEL_HEIST_SOUNDS");
							soundid = GlobalHelper.CreateSoundID();
							Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, "JEWEL_HEIST_SOUNDS", false, 10, false);
							if (RobberAJewelType == 1)
							{
								int num21 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start");
								Prop prop26 = (RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, 2f, num21, 0, 0, 0));
								prop26.Opacity = 0;
								Prop prop27 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop26.Position.X, prop26.Position.Y, prop26.Position.Z - 0.6f), new Vector3(prop26.Rotation.X, prop26.Rotation.Y, prop26.Rotation.Z - 90f), false, false);
								prop27.IsPositionFrozen = true;
								Jewels.Add(prop27);
							}
							if (RobberAJewelType == 2)
							{
								int num22 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start");
								Prop prop28 = (RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, 2f, num22, 0, 0, 0));
								prop28.Opacity = 0;
								Prop prop29 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop28.Position.X, prop28.Position.Y, prop28.Position.Z - 0.6f), new Vector3(prop28.Rotation.X, prop28.Rotation.Y, prop28.Rotation.Z - 90f), false, false);
								prop29.IsPositionFrozen = true;
								Jewels.Add(prop29);
							}
							if (RobberAJewelType == 3)
							{
								int num23 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start");
								Prop prop30 = (RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, 2f, num23, 0, 0, 0));
								prop30.Opacity = 0;
								Prop prop31 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop30.Position.X, prop30.Position.Y, prop30.Position.Z - 0.6f), new Vector3(prop30.Rotation.X, prop30.Rotation.Y, prop30.Rotation.Z), false, false);
								prop31.IsPositionFrozen = true;
								Jewels.Add(prop31);
							}
							if (RobberAJewelType == 4)
							{
								int num24 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start");
								Prop prop32 = (RobberANearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberANearsetCabinet.Position.X, RobberANearsetCabinet.Position.Y, RobberANearsetCabinet.Position.Z, 2f, num24, 0, 0, 0));
								prop32.Opacity = 0;
								Prop prop33 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop32.Position.X, prop32.Position.Y, prop32.Position.Z - 0.6f), new Vector3(prop32.Rotation.X, prop32.Rotation.Y, prop32.Rotation.Z), false, false);
								prop33.IsPositionFrozen = true;
								Jewels.Add(prop33);
							}
						}
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1 && !RobbedJewelsCases[RobberACurrentValualabe])
					{
						RobbedJewelsCases[RobberACurrentValualabe] = true;
						RobbedJewelsCasesBlips[RobberACurrentValualabe].Alpha = 0;
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353200)
					{
						ManageRobberyAnims.CurrentPay += (int)increaseCurrentPay(HeistMinPay / 15, HeistMaxPay / 15);
						Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
						Audio.PlaySoundFrontend("LOCAL_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
						RobberA.Task.ClearAll();
						Random random = new Random();
						for (int j = 0; j < JewelsCases.Count; j++)
						{
							int num25 = random.Next(0, JewelsCases.Count);
							if (RobbedJewelsCasesBlips[num25].Alpha == 255 && !RobbedJewelsCases[num25])
							{
								RobberAMovingToLoot = false;
								if (CabinetType[num25] == 1)
								{
									RobberACurrentCabinet = JewelsCases[num25];
									RobberA_RobberMarker = JewelsCases[num25].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
								}
								if (CabinetType[num25] == 2)
								{
									RobberACurrentCabinet = JewelsCases[num25];
									RobberA_RobberMarker = JewelsCases[num25].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
								}
								if (CabinetType[num25] == 3)
								{
									RobberACurrentCabinet = JewelsCases[num25];
									RobberA_RobberMarker = JewelsCases[num25].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
								}
								if (CabinetType[num25] == 4)
								{
									RobberACurrentCabinet = JewelsCases[num25];
									RobberA_RobberMarker = JewelsCases[num25].GetOffsetPosition(new Vector3(-0.9f, 0f, -0.5f));
								}
								RobberACurrentValualabe = num25;
								Notification.Show("RobberA Moving To Pos");
								if (HeistPathfindVariation == 0)
								{
									Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberA, RobberA_RobberMarker.X, RobberA_RobberMarker.Y, RobberA_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
								}
								if (HeistPathfindVariation == 1)
								{
									Vector3 robberA_RobberMarker = RobberA_RobberMarker;
									Vector3 robberA_RobberMarker2 = RobberA_RobberMarker;
									Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberA, robberA_RobberMarker.X, robberA_RobberMarker.Y, robberA_RobberMarker.Z, 30f, 20000, 0.1f, 512, 0);
								}
								break;
							}
						}
						bool flag2 = false;
						for (int k = 0; k < JewelsCases.Count; k++)
						{
							if (!RobbedJewelsCases[k])
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							RobberARobbing = false;
						}
						if (!flag2)
						{
							RobberARobbing = true;
							RobberA.Task.RunTo(new Vector3(-634f, -240.6f, 38f), false, 9999);
							RobberA_RobberMarker = new Vector3(0f, 0f, 0f);
						}
					}
				}
				if (RobberB != null && RobberBRobbing)
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1045050216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) <= 1048353216)
					{
						if (RobberBJewelSmashType == 1)
						{
							Vector3 positionOffset = RobberBNearsetCabinet.GetPositionOffset(new Vector3(0f, 0f, -1.5f));
							int num26 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass");
							int num27 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
							RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset.X, positionOffset.Y, positionOffset.Z, 5f, num26, 0, 0, 0);
							if (World.GetDistance(RobberBNearsetCabinet.Position, RobberBNearsetGlass.Position) < 2f)
							{
								RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset.X, positionOffset.Y, positionOffset.Z, 5f, num26, 0, 0, 0);
								RobberBNearsetGlass.Opacity = 0;
							}
							if (World.GetDistance(RobberBNearsetCabinet.Position, RobberBNearsetGlass.Position) > 2f)
							{
								num27 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
								RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset.X, positionOffset.Y, positionOffset.Z, 5f, num27, 0, 0, 0);
								RobberBNearsetGlass.Opacity = 0;
							}
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, "scr_jewel_cab_smash", RobberBNearsetGlass, 0f, 0f, 0.5f, 0f, 0f, RobberBNearsetGlass.Heading - 90f, 1.15f, false, false, false);
							LoadAudioDict("SCRIPT/JWL_HEIST_RAID_GLASS_SMASH");
							LoadAudioDict("SCRIPT/JEWEL_HEIST_SOUNDS");
							LoadAudioDict("JEWEL_HEIST_SOUNDS");
							soundid = GlobalHelper.CreateSoundID();
							Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, "JEWEL_HEIST_SOUNDS", false, 10, false);
							if (RobberBJewelType == 1)
							{
								int num28 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start");
								Prop prop34 = (RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, 2f, num28, 0, 0, 0));
								prop34.Opacity = 0;
								Prop prop35 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab_end"), new Vector3(prop34.Position.X, prop34.Position.Y, prop34.Position.Z - 0.6f), new Vector3(prop34.Rotation.X, prop34.Rotation.Y, prop34.Rotation.Z), false, false);
								prop35.IsPositionFrozen = true;
								Jewels.Add(prop35);
							}
							if (RobberBJewelType == 2)
							{
								int num29 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start");
								Prop prop36 = (RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, 2f, num29, 0, 0, 0));
								prop36.Opacity = 0;
								Prop prop37 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab2_end"), new Vector3(prop36.Position.X, prop36.Position.Y, prop36.Position.Z - 0.6f), new Vector3(prop36.Rotation.X, prop36.Rotation.Y, prop36.Rotation.Z), false, false);
								prop37.IsPositionFrozen = true;
								Jewels.Add(prop37);
							}
							if (RobberBJewelType == 3)
							{
								int num30 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start");
								Prop prop38 = (RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, 2f, num30, 0, 0, 0));
								prop38.Opacity = 0;
								Prop prop39 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab3_end"), new Vector3(prop38.Position.X, prop38.Position.Y, prop38.Position.Z - 0.6f), new Vector3(prop38.Rotation.X, prop38.Rotation.Y, prop38.Rotation.Z), false, false);
								prop39.IsPositionFrozen = true;
								Jewels.Add(prop39);
							}
							if (RobberBJewelType == 4)
							{
								int num31 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start");
								Prop prop40 = (RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, 2f, num31, 0, 0, 0));
								prop40.Opacity = 0;
								Prop prop41 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab3_end"), new Vector3(prop40.Position.X, prop40.Position.Y, prop40.Position.Z - 0.6f), new Vector3(prop40.Rotation.X, prop40.Rotation.Y, prop40.Rotation.Z + 90f), false, false);
								prop41.IsPositionFrozen = true;
								Jewels.Add(prop41);
							}
						}
						if (RobberBJewelSmashType == 2)
						{
							Vector3 positionOffset2 = RobberBNearsetCabinet.GetPositionOffset(new Vector3(0f, 0f, -1.5f));
							int num32 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass");
							int num33 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
							RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset2.X, positionOffset2.Y, positionOffset2.Z, 5f, num32, 0, 0, 0);
							if (World.GetDistance(RobberBNearsetCabinet.Position, RobberBNearsetGlass.Position) < 2f)
							{
								RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset2.X, positionOffset2.Y, positionOffset2.Z, 5f, num32, 0, 0, 0);
								RobberBNearsetGlass.Opacity = 0;
							}
							if (World.GetDistance(RobberBNearsetCabinet.Position, RobberBNearsetGlass.Position) > 2f)
							{
								num33 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
								RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset2.X, positionOffset2.Y, positionOffset2.Z, 5f, num33, 0, 0, 0);
								RobberBNearsetGlass.Opacity = 0;
							}
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, "scr_jewel_cab_smash", RobberBNearsetGlass, 0f, 0f, 0.5f, 0f, 0f, RobberBNearsetGlass.Heading - 90f, 1.15f, false, false, false);
							LoadAudioDict("SCRIPT/JWL_HEIST_RAID_GLASS_SMASH");
							LoadAudioDict("SCRIPT/JEWEL_HEIST_SOUNDS");
							LoadAudioDict("JEWEL_HEIST_SOUNDS");
							soundid = GlobalHelper.CreateSoundID();
							Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, "JEWEL_HEIST_SOUNDS", false, 10, false);
							if (RobberBJewelType == 1)
							{
								int num34 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start");
								Prop prop42 = (RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, 2f, num34, 0, 0, 0));
								prop42.Opacity = 0;
								Prop prop43 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop42.Position.X, prop42.Position.Y, prop42.Position.Z - 0.6f), new Vector3(prop42.Rotation.X, prop42.Rotation.Y, prop42.Rotation.Z - 90f), false, false);
								prop43.IsPositionFrozen = true;
								Jewels.Add(prop43);
							}
							if (RobberBJewelType == 2)
							{
								int num35 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start");
								Prop prop44 = (RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, 2f, num35, 0, 0, 0));
								prop44.Opacity = 0;
								Prop prop45 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop44.Position.X, prop44.Position.Y, prop44.Position.Z - 0.6f), new Vector3(prop44.Rotation.X, prop44.Rotation.Y, prop44.Rotation.Z - 90f), false, false);
								prop45.IsPositionFrozen = true;
								Jewels.Add(prop45);
							}
							if (RobberBJewelType == 3)
							{
								int num36 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start");
								Prop prop46 = (RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, 2f, num36, 0, 0, 0));
								prop46.Opacity = 0;
								Prop prop47 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop46.Position.X, prop46.Position.Y, prop46.Position.Z - 0.6f), new Vector3(prop46.Rotation.X, prop46.Rotation.Y, prop46.Rotation.Z), false, false);
								prop47.IsPositionFrozen = true;
								Jewels.Add(prop47);
							}
							if (RobberBJewelType == 4)
							{
								int num37 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start");
								Prop prop48 = (RobberBNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberBNearsetCabinet.Position.X, RobberBNearsetCabinet.Position.Y, RobberBNearsetCabinet.Position.Z, 2f, num37, 0, 0, 0));
								prop48.Opacity = 0;
								Prop prop49 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop48.Position.X, prop48.Position.Y, prop48.Position.Z - 0.6f), new Vector3(prop48.Rotation.X, prop48.Rotation.Y, prop48.Rotation.Z), false, false);
								prop49.IsPositionFrozen = true;
								Jewels.Add(prop49);
							}
						}
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1 && !RobbedJewelsCases[RobberBCurrentValualabe])
					{
						RobbedJewelsCases[RobberBCurrentValualabe] = true;
						RobbedJewelsCasesBlips[RobberBCurrentValualabe].Alpha = 0;
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353200)
					{
						ManageRobberyAnims.CurrentPay += (int)increaseCurrentPay(HeistMinPay / 15, HeistMaxPay / 15);
						RobberB.Task.ClearAll();
						Random random2 = new Random();
						for (int l = 0; l < JewelsCases.Count; l++)
						{
							int num38 = random2.Next(0, JewelsCases.Count);
							if (RobbedJewelsCasesBlips[num38].Alpha == 255 && !RobbedJewelsCases[num38])
							{
								RobberBMovingToLoot = false;
								if (CabinetType[num38] == 1)
								{
									RobberBCurrentCabinet = JewelsCases[num38];
									RobberB_RobberMarker = JewelsCases[num38].GetPositionOffset(new Vector3(0f, -0.9f, -0.5f));
								}
								if (CabinetType[num38] == 2)
								{
									RobberBCurrentCabinet = JewelsCases[num38];
									RobberB_RobberMarker = JewelsCases[num38].GetPositionOffset(new Vector3(0f, -0.9f, -0.5f));
								}
								if (CabinetType[num38] == 3)
								{
									RobberBCurrentCabinet = JewelsCases[num38];
									RobberB_RobberMarker = JewelsCases[num38].GetPositionOffset(new Vector3(0f, -0.9f, -0.5f));
								}
								if (CabinetType[num38] == 4)
								{
									RobberBCurrentCabinet = JewelsCases[num38];
									RobberB_RobberMarker = JewelsCases[num38].GetPositionOffset(new Vector3(-0.9f, 0f, -0.5f));
								}
								RobberBCurrentValualabe = num38;
								Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberB, RobberB_RobberMarker.X, RobberB_RobberMarker.Y, RobberB_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
								break;
							}
						}
						bool flag3 = false;
						for (int m = 0; m < JewelsCases.Count; m++)
						{
							if (!RobbedJewelsCases[m])
							{
								flag3 = true;
								break;
							}
						}
						if (flag3)
						{
							RobberBRobbing = false;
						}
						if (!flag3)
						{
							RobberBRobbing = true;
							RobberB.Task.RunTo(new Vector3(-634f, -240.6f, 38f), false, 9999);
							RobberB_RobberMarker = new Vector3(0f, 0f, 0f);
						}
					}
				}
				if (!(RobberC != null) || !RobberCRobbing)
				{
					return;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1045050216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) <= 1048353216)
				{
					if (RobberCJewelSmashType == 1)
					{
						Vector3 positionOffset3 = RobberCNearsetCabinet.GetPositionOffset(new Vector3(0f, 0f, -1.5f));
						int num39 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass");
						int num40 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
						RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset3.X, positionOffset3.Y, positionOffset3.Z, 5f, num39, 0, 0, 0);
						if (World.GetDistance(RobberCNearsetCabinet.Position, RobberCNearsetGlass.Position) < 2f)
						{
							RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset3.X, positionOffset3.Y, positionOffset3.Z, 5f, num39, 0, 0, 0);
							RobberCNearsetGlass.Opacity = 0;
						}
						if (World.GetDistance(RobberCNearsetCabinet.Position, RobberCNearsetGlass.Position) > 2f)
						{
							num40 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
							RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset3.X, positionOffset3.Y, positionOffset3.Z, 5f, num40, 0, 0, 0);
							RobberCNearsetGlass.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, "scr_jewel_cab_smash", RobberCNearsetGlass, 0f, 0f, 0.5f, 0f, 0f, RobberCNearsetGlass.Heading - 90f, 1.15f, false, false, false);
						LoadAudioDict("SCRIPT/JWL_HEIST_RAID_GLASS_SMASH");
						LoadAudioDict("SCRIPT/JEWEL_HEIST_SOUNDS");
						LoadAudioDict("JEWEL_HEIST_SOUNDS");
						soundid = GlobalHelper.CreateSoundID();
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, "JEWEL_HEIST_SOUNDS", false, 10, false);
						if (RobberCJewelType == 1)
						{
							int num41 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start");
							Prop prop50 = (RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, 2f, num41, 0, 0, 0));
							prop50.Opacity = 0;
							Prop prop51 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab_end"), new Vector3(prop50.Position.X, prop50.Position.Y, prop50.Position.Z - 0.6f), new Vector3(prop50.Rotation.X, prop50.Rotation.Y, prop50.Rotation.Z), false, false);
							prop51.IsPositionFrozen = true;
							Jewels.Add(prop51);
						}
						if (RobberCJewelType == 2)
						{
							int num42 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start");
							Prop prop52 = (RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, 2f, num42, 0, 0, 0));
							prop52.Opacity = 0;
							Prop prop53 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab2_end"), new Vector3(prop52.Position.X, prop52.Position.Y, prop52.Position.Z - 0.6f), new Vector3(prop52.Rotation.X, prop52.Rotation.Y, prop52.Rotation.Z), false, false);
							prop53.IsPositionFrozen = true;
							Jewels.Add(prop53);
						}
						if (RobberCJewelType == 3)
						{
							int num43 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start");
							Prop prop54 = (RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, 2f, num43, 0, 0, 0));
							prop54.Opacity = 0;
							Prop prop55 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab3_end"), new Vector3(prop54.Position.X, prop54.Position.Y, prop54.Position.Z - 0.6f), new Vector3(prop54.Rotation.X, prop54.Rotation.Y, prop54.Rotation.Z), false, false);
							prop55.IsPositionFrozen = true;
							Jewels.Add(prop55);
						}
						if (RobberCJewelType == 4)
						{
							int num44 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start");
							Prop prop56 = (RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, 2f, num44, 0, 0, 0));
							prop56.Opacity = 0;
							Prop prop57 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab3_end"), new Vector3(prop56.Position.X, prop56.Position.Y, prop56.Position.Z - 0.6f), new Vector3(prop56.Rotation.X, prop56.Rotation.Y, prop56.Rotation.Z + 90f), false, false);
							prop57.IsPositionFrozen = true;
							Jewels.Add(prop57);
						}
					}
					if (RobberCJewelSmashType == 2)
					{
						Vector3 positionOffset4 = RobberCNearsetCabinet.GetPositionOffset(new Vector3(0f, 0f, -1.5f));
						int num45 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass");
						int num46 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
						RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset4.X, positionOffset4.Y, positionOffset4.Z, 5f, num45, 0, 0, 0);
						if (World.GetDistance(RobberCNearsetCabinet.Position, RobberCNearsetGlass.Position) < 2f)
						{
							RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset4.X, positionOffset4.Y, positionOffset4.Z, 5f, num45, 0, 0, 0);
							RobberCNearsetGlass.Opacity = 0;
						}
						if (World.GetDistance(RobberCNearsetCabinet.Position, RobberCNearsetGlass.Position) > 2f)
						{
							num46 = Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass");
							RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, positionOffset4.X, positionOffset4.Y, positionOffset4.Z, 5f, num46, 0, 0, 0);
							RobberCNearsetGlass.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						Function.Call(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, "scr_jewel_cab_smash", RobberCNearsetGlass, 0f, 0f, 0.5f, 0f, 0f, RobberCNearsetGlass.Heading - 90f, 1.15f, false, false, false);
						LoadAudioDict("SCRIPT/JWL_HEIST_RAID_GLASS_SMASH");
						LoadAudioDict("SCRIPT/JEWEL_HEIST_SOUNDS");
						LoadAudioDict("JEWEL_HEIST_SOUNDS");
						soundid = GlobalHelper.CreateSoundID();
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, "JEWEL_HEIST_SOUNDS", false, 10, false);
						if (RobberCJewelType == 1)
						{
							int num47 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start");
							Prop prop58 = (RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, 2f, num47, 0, 0, 0));
							prop58.Opacity = 0;
							Prop prop59 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop58.Position.X, prop58.Position.Y, prop58.Position.Z - 0.6f), new Vector3(prop58.Rotation.X, prop58.Rotation.Y, prop58.Rotation.Z - 90f), false, false);
							prop59.IsPositionFrozen = true;
							Jewels.Add(prop59);
						}
						if (RobberCJewelType == 2)
						{
							int num48 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start");
							Prop prop60 = (RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, 2f, num48, 0, 0, 0));
							prop60.Opacity = 0;
							Prop prop61 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop60.Position.X, prop60.Position.Y, prop60.Position.Z - 0.6f), new Vector3(prop60.Rotation.X, prop60.Rotation.Y, prop60.Rotation.Z - 90f), false, false);
							prop61.IsPositionFrozen = true;
							Jewels.Add(prop61);
						}
						if (RobberCJewelType == 3)
						{
							int num49 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start");
							Prop prop62 = (RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, 2f, num49, 0, 0, 0));
							prop62.Opacity = 0;
							Prop prop63 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop62.Position.X, prop62.Position.Y, prop62.Position.Z - 0.6f), new Vector3(prop62.Rotation.X, prop62.Rotation.Y, prop62.Rotation.Z), false, false);
							prop63.IsPositionFrozen = true;
							Jewels.Add(prop63);
						}
						if (RobberCJewelType == 4)
						{
							int num50 = Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start");
							Prop prop64 = (RobberCNearsetGlass = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, RobberCNearsetCabinet.Position.X, RobberCNearsetCabinet.Position.Y, RobberCNearsetCabinet.Position.Z, 2f, num50, 0, 0, 0));
							prop64.Opacity = 0;
							Prop prop65 = GlobalHelper.CreateProp(RequestModel("des_jewel_cab4_end"), new Vector3(prop64.Position.X, prop64.Position.Y, prop64.Position.Z - 0.6f), new Vector3(prop64.Rotation.X, prop64.Rotation.Y, prop64.Rotation.Z), false, false);
							prop65.IsPositionFrozen = true;
							Jewels.Add(prop65);
						}
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1 && !RobbedJewelsCases[RobberCCurrentValualabe])
				{
					RobbedJewelsCases[RobberCCurrentValualabe] = true;
					RobbedJewelsCasesBlips[RobberCCurrentValualabe].Alpha = 0;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) < 1065353200)
				{
					return;
				}
				ManageRobberyAnims.CurrentPay += (int)increaseCurrentPay(HeistMinPay / 15, HeistMaxPay / 15);
				RobberC.Task.ClearAll();
				Random random3 = new Random();
				for (int n = 0; n < JewelsCases.Count; n++)
				{
					int num51 = random3.Next(0, JewelsCases.Count);
					if (RobbedJewelsCasesBlips[num51].Alpha == 255 && !RobbedJewelsCases[num51])
					{
						RobberCMovingToLoot = false;
						if (CabinetType[num51] == 1)
						{
							RobberCCurrentCabinet = JewelsCases[num51];
							RobberC_RobberMarker = JewelsCases[num51].GetPositionOffset(new Vector3(0f, -0.9f, -0.5f));
						}
						if (CabinetType[num51] == 2)
						{
							RobberCCurrentCabinet = JewelsCases[num51];
							RobberC_RobberMarker = JewelsCases[num51].GetPositionOffset(new Vector3(0f, -0.9f, -0.5f));
						}
						if (CabinetType[num51] == 3)
						{
							RobberCCurrentCabinet = JewelsCases[num51];
							RobberC_RobberMarker = JewelsCases[num51].GetPositionOffset(new Vector3(0f, -0.9f, -0.5f));
						}
						if (CabinetType[num51] == 4)
						{
							RobberCCurrentCabinet = JewelsCases[num51];
							RobberC_RobberMarker = JewelsCases[num51].GetPositionOffset(new Vector3(-0.9f, 0f, -0.5f));
						}
						RobberCCurrentValualabe = num51;
						Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberC, RobberC_RobberMarker.X, RobberC_RobberMarker.Y, RobberC_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
						break;
					}
				}
				bool flag4 = false;
				for (int num52 = 0; num52 < JewelsCases.Count; num52++)
				{
					if (!RobbedJewelsCases[num52])
					{
						flag4 = true;
						break;
					}
				}
				if (flag4)
				{
					RobberCRobbing = false;
				}
				if (!flag4)
				{
					RobberCRobbing = true;
					RobberC.Task.RunTo(new Vector3(-634f, -240.6f, 38f), false, 9999);
					RobberC_RobberMarker = new Vector3(0f, 0f, 0f);
				}
			}
			catch
			{
				if (EnableErrorDebugging)
				{
					Notification.Show("~r~ Error Caught~w~ On Line 30763, contact ~g~ HKH191~");
				}
			}
		}

		public static void ShowCopsTimeUntill_ShowTake(string UntillWhat, int Waittime, int Take)
		{
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.95f, 0.21f, 0.08f, 255, 255, 255, 130);
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.95f, 0.21f, 0.08f, 255, 255, 255, 130);
			drawSprite2("timerbars", "all_black_bg", 0.89f, 0.95f, 0.21f, 0.08f, 255, 255, 255, 130);
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(Waittime);
			drawText(UntillWhat + " " + timeSpan.Minutes + ":" + timeSpan.Seconds + "." + timeSpan.Milliseconds, 0.89f, 0.92f, 0.3f, 255, 255, 255);
			drawText("Take ~g~" + Take.ToString("N"), 0.89f, 0.961f, 0.3f, 255, 255, 255);
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

		private static float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		private static float progresswidth(float X, float percent)
		{
			return X - 0.08f * percent;
		}

		private static float progressxcoord(float Xcoord, float percent)
		{
			float num = 0.04f * percent;
			return Xcoord + num;
		}

		private static float progressxcoord(float X, float Xcoord, float percent)
		{
			float num = X - 0.04f * percent;
			return Xcoord + num;
		}

		public static void CalculatePay(Ped ped, LootType Type)
		{
			if (Type == LootType.Trolly_Cash)
			{
				Random random = new Random();
				float num = increaseCurrentPay(Heist_Average_Pay / 425, Heist_Average_Pay / 325);
				ManageRobberyAnims.CurrentPay += (int)num;
				bool flag = ped == Game.Player.Character;
			}
			if (Type == LootType.Trolly_Gold)
			{
				Random random2 = new Random();
				float num2 = increaseCurrentPay(Heist_Average_Pay / 425, Heist_Average_Pay / 400);
				ManageRobberyAnims.CurrentPay += (int)num2;
				bool flag2 = ped == Game.Player.Character;
			}
			if (Type == LootType.Trolly_Diamonds)
			{
				Random random3 = new Random();
				float num3 = increaseCurrentPay(Heist_Average_Pay / 420, Heist_Average_Pay / 480);
				ManageRobberyAnims.CurrentPay += (int)num3;
				if (ped == Game.Player.Character)
				{
					Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
					Audio.PlaySoundFrontend("LOCAL_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
				}
			}
		}

		public void AddCratesPaleto()
		{
			LocalTake = 0;
			if (MoneyProp != null)
			{
				MoneyProp.Delete();
			}
			if (_cashGrabTray1 != null)
			{
				_cashGrabTray1.Delete();
			}
			if (bagProp != null)
			{
				bagProp.Delete();
			}
			ManageRobberyAnims.DeleteLoot();
			Crate_loc.Clear();
			Crate_Rot.Clear();
			Wait(10);
			Crate_loc.Add(new Vector3(-104.35f, 6478.826f, 31.62f));
			Crate_Rot.Add(45f);
			for (int i = 0; i < Crate_loc.Count; i++)
			{
				Crate_loc[i] = new Vector3(Crate_loc[i].X, Crate_loc[i].Y, Crate_loc[i].Z - 1f);
			}
			Wait(10);
			Random random = new Random();
			for (int j = 0; j < Crate_loc.Count; j++)
			{
				Vector3 vector = Crate_loc[j];
				Prop prop = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_cash_trolly_01"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
				ManageRobberyAnims.AddPropAsLoot(prop);
				Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop);
			}
			CreatedCrates = true;
		}

		public void AddCratesFleeca()
		{
			ManageRobberyAnims.DeleteLoot();
			if (MoneyProp != null)
			{
				MoneyProp.Delete();
			}
			if (_cashGrabTray1 != null)
			{
				_cashGrabTray1.Delete();
			}
			if (bagProp != null)
			{
				bagProp.Delete();
			}
			Crate_loc.Clear();
			Crate_Rot.Clear();
			Wait(10);
			Bank_Location = GetNearestFleecaBankIndex();
			if (Bank_Location == 0)
			{
				FleecaCash_Location = new Vector3(313.0498f, -286.8682f, 53.1439f);
				FleecaCash_Heading = 175f;
				FleecaTrolly_Location = new Vector3(311.2888f, -288.1999f, 53.616f);
				FleecaTrolly_Rotation = new Vector3(0f, 0f, -28.0066f);
			}
			if (Bank_Location == 1)
			{
				FleecaCash_Location = new Vector3(148.4134f, -1049.139f, 29.1966f);
				FleecaCash_Heading = 52.78f;
				FleecaTrolly_Location = new Vector3(147.1844f, -1049.924f, 28.8193f);
				FleecaTrolly_Rotation = new Vector3(0f, 0f, -20.4033f);
			}
			if (Bank_Location == 2)
			{
				FleecaCash_Location = new Vector3(-351.9788f, -57.687f, 48.0156f);
				FleecaCash_Heading = 185f;
				FleecaTrolly_Location = new Vector3(-353.8292f, -59.0439f, 48.4878f);
				FleecaTrolly_Rotation = new Vector3(0f, 0f, -34.6244f);
			}
			if (Bank_Location == 3)
			{
				FleecaCash_Location = new Vector3(-1207.791f, -336.6265f, 36.7601f);
				FleecaCash_Heading = 205f;
				FleecaTrolly_Location = new Vector3(-1208.079f, -339.1376f, 37.2323f);
				FleecaTrolly_Rotation = new Vector3(0f, 0f, 17.0116f);
			}
			if (Bank_Location == 4)
			{
				FleecaCash_Location = new Vector3(-2954.871f, 484.173f, 14.6762f);
				FleecaCash_Heading = 275f;
				FleecaTrolly_Location = new Vector3(-2952.945f, 482.7372f, 15.1483f);
				FleecaTrolly_Rotation = new Vector3(0f, 0f, 76.6865f);
			}
			if (Bank_Location == 5)
			{
				FleecaCash_Location = new Vector3(1173.538f, 2715.18f, 37.9166f);
				FleecaCash_Heading = 52.78f;
				FleecaTrolly_Location = new Vector3(1174.826f, 2716.426f, 37.5393f);
				FleecaTrolly_Rotation = new Vector3(0f, 0f, 162.2452f);
			}
			Crate_loc.Add(new Vector3(FleecaTrolly_Location.X, FleecaTrolly_Location.Y, FleecaTrolly_Location.Z));
			Crate_Rot.Add(FleecaTrolly_Rotation.Z - 180f);
			Random random = new Random();
			for (int i = 0; i < Crate_loc.Count; i++)
			{
				Vector3 vector = Crate_loc[i];
				int num = random.Next(0, 100);
				if (num > 50)
				{
					Prop prop = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_cash_trolly_01"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
					ManageRobberyAnims.AddPropAsLoot(prop);
					Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop);
				}
				else
				{
					Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01b"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
					ManageRobberyAnims.AddPropAsLoot(prop2);
					Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop2);
				}
			}
			CreatedCrates = true;
		}

		public void AddCratesPacStandard()
		{
			ManageRobberyAnims.DeleteLoot();
			if (MoneyProp != null)
			{
				MoneyProp.Delete();
			}
			if (_cashGrabTray1 != null)
			{
				_cashGrabTray1.Delete();
			}
			if (bagProp != null)
			{
				bagProp.Delete();
			}
			Crate_loc.Clear();
			Crate_Rot.Clear();
			Wait(10);
			Crate_loc.Add(new Vector3(254f, 216.4f, 100.6f));
			Crate_Rot.Add(159f);
			Crate_loc.Add(new Vector3(255.5f, 215.8f, 100.6f));
			Crate_Rot.Add(160f);
			Crate_loc.Add(new Vector3(257.1f, 214.8f, 100.6f));
			Crate_Rot.Add(160f);
			Crate_loc.Add(new Vector3(258.7f, 214.2f, 100.6f));
			Crate_Rot.Add(160f);
			Crate_loc.Add(new Vector3(259.7f, 213.9f, 100.6f));
			Crate_Rot.Add(160f);
			Crate_loc.Add(new Vector3(262.2f, 213f, 100.6f));
			Crate_Rot.Add(160f);
			Crate_loc.Add(new Vector3(263.5f, 212.4f, 100.6f));
			Crate_Rot.Add(160f);
			Crate_loc.Add(new Vector3(265.3f, 212.4f, 100.6f));
			Crate_Rot.Add(-110f);
			Crate_loc.Add(new Vector3(264.3f, 216.1f, 100.6f));
			Crate_Rot.Add(-22f);
			Crate_loc.Add(new Vector3(263.4f, 216.4f, 100.6f));
			Crate_Rot.Add(-22f);
			Crate_loc.Add(new Vector3(260.9f, 217.2f, 100.6f));
			Crate_Rot.Add(-22f);
			Crate_loc.Add(new Vector3(259.7f, 217.7f, 100.6f));
			Crate_Rot.Add(-22f);
			Crate_loc.Add(new Vector3(258.2f, 218.4f, 100.6f));
			Crate_Rot.Add(-22f);
			Crate_loc.Add(new Vector3(256.7f, 218.4f, 100.6f));
			Crate_Rot.Add(-22f);
			Crate_loc.Add(new Vector3(254.9f, 219f, 100.6f));
			Crate_Rot.Add(-22f);
			Crate_loc.Add(new Vector3(249f, 218.4f, 100.6f));
			Crate_Rot.Add(70f);
			Crate_loc.Add(new Vector3(249.5f, 219.7f, 100.6f));
			Crate_Rot.Add(70f);
			Crate_loc.Add(new Vector3(249.9f, 220.9f, 100.6f));
			Crate_Rot.Add(70f);
			int i = 0;
			Random random = new Random();
			for (; i < 10; i++)
			{
				int index = random.Next(0, Crate_loc.Count);
				Crate_loc.RemoveAt(index);
				Crate_Rot.RemoveAt(index);
			}
			Wait(10);
			for (int j = 0; j < Crate_loc.Count; j++)
			{
				Vector3 vector = Crate_loc[j];
				int num = random.Next(0, 100);
				if (num > 80)
				{
					Prop prop = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_cash_trolly_01"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
					ManageRobberyAnims.AddPropAsLoot(prop);
					Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop);
				}
				else
				{
					Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01b"), Crate_loc[j], new Vector3(0f, 0f, Crate_Rot[j] - 180f), true, false);
					ManageRobberyAnims.AddPropAsLoot(prop2);
					Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop2);
				}
			}
			CreatedCrates = true;
		}

		public void DrillScreen(float z)
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scale, 255, 255, 255, 255);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
			Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "More Power");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Less Power");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void DrillScreenStart()
		{
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
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "More Power");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Less Power");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
			scaleform.Dispose();
			scaleform.Dispose();
		}

		public static void AddCash(Ped Ped, int min, int max)
		{
			if (!PlayingCashGrabIdle)
			{
				Random random = new Random();
				float num = increaseCurrentPay(min, max);
				ManageRobberyAnims.CurrentPay += (int)num;
				bool flag = Ped == Game.Player.Character;
			}
		}

		private static float progressxcoord(float percent)
		{
			float num = 0.04f * percent;
			return 0.9f + num;
		}

		public static void ShowSafeButtons()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
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

		public static void BEGIN_SCALEFORM_MOVIE_METHOD(int scaleform, string methodName)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, scaleform, methodName);
		}

		public static void END_SCALEFORM_MOVIE_METHOD()
		{
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void func_4450(int iParam0)
		{
			BEGIN_SCALEFORM_MOVIE_METHOD(Local_26709f_9, "SET_STATE");
			Add_param_int(iParam0);
			END_SCALEFORM_MOVIE_METHOD();
		}

		public static void func_4436(int iParam0)
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

		public static void func_4443()
		{
			BEGIN_SCALEFORM_MOVIE_METHOD(Local_26709f_9, "SET_CURSOR_POSITION");
			Add_param_int(SelectedNum);
			END_SCALEFORM_MOVIE_METHOD();
		}

		public static void func_4437(int iParam0)
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

		public static int func_4438()
		{
			Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
			{
				return 1;
			}
			return 0;
		}

		public static int func_4439(bool bParam0, int iParam1)
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

		public static void func_4453()
		{
			Random random = new Random();
			CodeNumber1 = random.Next(1, 99);
			CodeNumber2 = random.Next(1, 99);
			CodeNumber3 = random.Next(1, 99);
		}

		public static void func_4454(int iParam0)
		{
			iLocal_26708 = iParam0;
		}

		public static void func_4448(int iParam0, int iParam1)
		{
			BEGIN_SCALEFORM_MOVIE_METHOD(Local_26709f_9, "SET_VALUE");
			Add_param_int(iParam0);
			Add_param_int(iParam1);
			END_SCALEFORM_MOVIE_METHOD();
		}

		public static void Add_param_int(int V)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, V);
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

		public float CalculateMaxPay(int min, int max)
		{
			float result = 0f;
			Random random = new Random();
			if (min < max)
			{
				result = random.Next(min, max);
			}
			if (min > max)
			{
				result = random.Next(max, min);
			}
			return result;
		}

		public static void GetBank(int Loc)
		{
			if (Loc == 0)
			{
				FleecaVaultHackLocation = new Vector3(311.5506f, -284.5029f, 54.56907f);
				FleecaVaultHackHeading = 251.1724f;
				FleecaBankHeistStartLocation = new Vector3(317.18f, -269.97f, 52.9f);
				PatrolCarDeletePos = new Vector3(769f, -719f, 28f);
				FleecaBankMarker = new Vector3(318.565f, -267f, 53.8f);
				FleecaJammerPos = new Vector3(311.815f, -283.3f, 54.5f);
				MoneyPosition = new Vector3(312.319f, -288.876f, 53.6f);
				VaultPosition = new Vector3(312.0821f, -283.3166f, 53.8f);
				OpenAmt = 0.25f;
				CloseAmt = -0.25f;
				OpenValue = -110;
				CloseValue = -179;
			}
			if (Loc == 1)
			{
				FleecaVaultHackLocation = new Vector3(147.2658f, -1046.237f, 29.77327f);
				FleecaVaultHackHeading = 247.1955f;
				FleecaBankHeistStartLocation = new Vector3(152.41f, -1032.45f, 30f);
				PatrolCarDeletePos = new Vector3(-184.724f, -892.129f, 29f);
				FleecaBankMarker = new Vector3(152.491f, -1029.7f, 29f);
				FleecaJammerPos = new Vector3(147.264f, -1044.92f, 28.5f);
				MoneyPosition = new Vector3(148.433f, -1050.31f, 28.5f);
				VaultPosition = new Vector3(147.94f, -1044.718f, 30f);
				OpenAmt = 0.25f;
				CloseAmt = -0.25f;
				OpenValue = -110;
				CloseValue = -179;
			}
			if (Loc == 2)
			{
				FleecaVaultHackLocation = new Vector3(-353.498f, -55.41371f, 49.44156f);
				FleecaVaultHackHeading = 251.2823f;
				FleecaBankHeistStartLocation = new Vector3(-345.69f, -33.2f, 48f);
				PatrolCarDeletePos = new Vector3(-78.0138f, -108.686f, 57.85f);
				FleecaBankMarker = new Vector3(-346.961f, -36.3279f, 47.511f);
				FleecaJammerPos = new Vector3(-353.807f, -54.0838f, 48.5f);
				MoneyPosition = new Vector3(-352.1539f, -59.54951f, 48.5f);
				VaultPosition = new Vector3(-352.71f, -53.74f, 49f);
				OpenAmt = 0.25f;
				CloseAmt = -0.25f;
				OpenValue = -110;
				CloseValue = -179;
			}
			if (Loc == 3)
			{
				FleecaVaultHackLocation = new Vector3(-1210.415f, -336.423f, 38.18623f);
				FleecaVaultHackHeading = 292.7944f;
				FleecaBankHeistStartLocation = new Vector3(-1217.544f, -321.25f, 38f);
				PatrolCarDeletePos = new Vector3(-1649.95f, -572.925f, 33f);
				FleecaBankMarker = new Vector3(-1215.88f, -324.573f, 37f);
				FleecaJammerPos = new Vector3(-1211.78f, -335.735f, 36.5f);
				MoneyPosition = new Vector3(-1206.57f, -338.154f, 36.5f);
				VaultPosition = new Vector3(-1211.002f, -334.832f, 37.7f);
				OpenAmt = 0.25f;
				CloseAmt = -0.25f;
				OpenValue = -63;
				CloseValue = -150;
			}
			if (Loc == 4)
			{
				FleecaVaultHackLocation = new Vector3(-2956.497f, 482.1348f, 16.10213f);
				FleecaVaultHackHeading = 354.319f;
				FleecaBankHeistStartLocation = new Vector3(-2970.65f, 482.4f, 15.45f);
				PatrolCarDeletePos = new Vector3(-3114.99f, 1097.3f, 20f);
				FleecaBankMarker = new Vector3(-2972.87f, 483.904f, 15.2675f);
				FleecaJammerPos = new Vector3(-2957.92f, 481.654f, 14.5f);
				MoneyPosition = new Vector3(-2953.04f, 484.34f, 14.5f);
				VaultPosition = new Vector3(-2958.377f, 482.2488f, 16f);
				OpenAmt = 0.25f;
				CloseAmt = -0.25f;
				OpenValue = -2;
				CloseValue = -90;
			}
			if (Loc == 5)
			{
				FleecaVaultHackLocation = new Vector3(1175.757f, 2712.793f, 38.49307f);
				FleecaVaultHackHeading = 97.71587f;
				FleecaBankHeistStartLocation = new Vector3(1175.379f, 2693.28f, 38f);
				PatrolCarDeletePos = new Vector3(769f, -719f, 28f);
				FleecaBankMarker = new Vector3(1173.65f, 2697.52f, 37f);
				FleecaJammerPos = new Vector3(1176.68f, 2711.68f, 37.2f);
				MoneyPosition = new Vector3(1173.03f, 2716f, 37.2f);
				VaultPosition = new Vector3(1175.467f, 2710.887f, 38f);
				OpenAmt = 0.25f;
				CloseAmt = -0.25f;
				OpenValue = 90;
				CloseValue = 1;
			}
		}

		public void GetFleecaBankStart(int Loc)
		{
			if (Loc == 0)
			{
				FleecaBankHeistStartLocation = new Vector3(317.18f, -269.97f, 52.9f);
				FleecaVaultHackLocation = new Vector3(311.5506f, -284.5029f, 54.56907f);
				FleecaVaultHackHeading = 251.1724f;
			}
			if (Loc == 1)
			{
				FleecaBankHeistStartLocation = new Vector3(152.41f, -1032.45f, 30f);
				FleecaVaultHackLocation = new Vector3(147.2658f, -1046.237f, 29.77327f);
				FleecaVaultHackHeading = 247.1955f;
			}
			if (Loc == 2)
			{
				FleecaBankHeistStartLocation = new Vector3(-345.69f, -33.2f, 48f);
				FleecaVaultHackLocation = new Vector3(-353.498f, -55.41371f, 49.44156f);
				FleecaVaultHackHeading = 251.2823f;
			}
			if (Loc == 3)
			{
				FleecaBankHeistStartLocation = new Vector3(-1217.544f, -321.25f, 38f);
				FleecaVaultHackLocation = new Vector3(-1210.415f, -336.423f, 38.18623f);
				FleecaVaultHackHeading = 292.7944f;
			}
			if (Loc == 4)
			{
				FleecaBankHeistStartLocation = new Vector3(-2970.65f, 482.4f, 15.45f);
				FleecaVaultHackLocation = new Vector3(-2956.497f, 482.1348f, 16.10213f);
				FleecaVaultHackHeading = 354.319f;
			}
			if (Loc == 5)
			{
				FleecaBankHeistStartLocation = new Vector3(1175.379f, 2693.28f, 38f);
				FleecaVaultHackLocation = new Vector3(1175.757f, 2712.793f, 38.49307f);
				FleecaVaultHackHeading = 97.71587f;
			}
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public static int GetNearestFleecaBankIndex()
		{
			float radius = 9999f;
			Vector3 position = Game.Player.Character.Position;
			int result = -1;
			for (int i = 0; i < FleecaBanks.Count; i++)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaBanks[i], "<", radius))
				{
					result = i;
					radius = GetDistance(Game.Player.Character.Position, FleecaBanks[i]);
				}
			}
			return result;
		}

		public static void StartJewelryStore()
		{
			isRobbingCase = false;
			TriggeredJewelStoreAlarm = false;
			ManageRobberyAnims.CurrentPay = 0;
			Function.Call(Hash.REMOVE_IPL, "v_refit");
			Function.Call(Hash.REMOVE_IPL, "jewel2fake");
			Function.Call(Hash.REQUEST_IPL, "refit_unload");
			Function.Call(Hash.REMOVE_IPL, "post_hiest_unload");
			Function.Call(Hash.REQUEST_IPL, "post_hiest_unload");
			HeistMinPay = JewelStore_HeistMinPay;
			HeistMaxPay = JewelStore_HeistMaxPay;
			Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 1.25f);
			foreach (Prop prop in Props)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			ManageRobberyAnims.DeleteLoot();
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
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
			if (LootId.Count > 0)
			{
				LootId.Clear();
			}
			if (Crate_loc.Count > 0)
			{
				Crate_loc.Clear();
			}
			if (Crate_Rot.Count > 0)
			{
				Crate_Rot.Clear();
			}
			if (PaintingsFinished.Count > 0)
			{
				PaintingsFinished.Clear();
			}
			LocalTake = 0;
			RobberARobbing = false;
			RobberACuttingStage = 0;
			RobberAInitialAnimDone = false;
			RobberA_RobberMarker = new Vector3(0f, 0f, 0f);
			RobberBRobbing = false;
			RobberBCuttingStage = 0;
			RobberBInitialAnimDone = false;
			RobberB_RobberMarker = new Vector3(0f, 0f, 0f);
			RobberCRobbing = false;
			RobberCCuttingStage = 0;
			RobberCInitialAnimDone = false;
			RobberC_RobberMarker = new Vector3(0f, 0f, 0f);
			HeistTime = 0f;
			if (SmokeOrigin != null)
			{
				SmokeOrigin.Delete();
			}
			foreach (Ped item2 in Civ)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 6, 0, 1);
			foreach (Ped guard in Guards)
			{
				if (guard != null)
				{
					guard.Delete();
				}
			}
			foreach (Ped civ in Civs)
			{
				if (civ != null)
				{
					civ.Delete();
				}
			}
			if (JewelHost != null)
			{
				JewelHost.Delete();
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			Ped ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(-629.677f, -240.2413f, 37f), 125f);
			ped.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(-616.4749f, -235.6701f, 37f), 80f);
			ped.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(-629.6724f, -228.7469f, 37f), -147f);
			ped.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
			Guards.Add(ped);
			foreach (Ped guard2 in Guards)
			{
				SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(guard2, SmartGuards.EnemyGroup.Security));
			}
			ped = GlobalHelper.CreatePed(PedHash.JewelassCutscene, new Vector3(-623.108f, -229.971f, 37f), 120f);
			ped.IsPersistent = false;
			JewelHost = ped;
			ped = GlobalHelper.CreatePed(RequestModel(ReturnPedhash()), new Vector3(-618.8979f, -231.7412f, 37.0445f), -90f);
			ped.IsPersistent = false;
			Civ.Add(ped);
			ped = GlobalHelper.CreatePed(RequestModel(ReturnPedhash()), new Vector3(-625.2069f, -229.4436f, 37.0397f), -90f);
			ped.IsPersistent = false;
			Civ.Add(ped);
			ped = GlobalHelper.CreatePed(RequestModel(ReturnPedhash()), new Vector3(-618.8979f, -231.7412f, 37.0445f).Around(5f), -90f);
			ped.IsPersistent = false;
			Civ.Add(ped);
			ped = GlobalHelper.CreatePed(RequestModel(ReturnPedhash()), new Vector3(-625.2069f, -229.4436f, 37.0397f).Around(5f), -90f);
			ped.IsPersistent = false;
			Civ.Add(ped);
			ped = GlobalHelper.CreatePed(RequestModel(ReturnPedhash()), new Vector3(-623.2971f, -230.1614f, 37.0257f), -90f);
			ped.IsPersistent = false;
			Civ.Add(ped);
			Random random = new Random();
			if (JewelryPos.Count > 0)
			{
				JewelryPos.Clear();
				JewelMarker.Clear();
				JewelsAmount.Clear();
				MaxAmount.Clear();
			}
			JewelryPos.Add(new Vector3(-626.769f, -235.702f, 37.2f));
			JewelryPos.Add(new Vector3(-625.706f, -234.791f, 37.2f));
			JewelryPos.Add(new Vector3(-627.191f, -232.79f, 37.2f));
			JewelryPos.Add(new Vector3(-628.123f, -233.766f, 37.2f));
			JewelryPos.Add(new Vector3(-625.021f, -231.36f, 37.2f));
			JewelryPos.Add(new Vector3(-620.39f, -234.155f, 37.2f));
			JewelryPos.Add(new Vector3(-619.542f, -233.33f, 37.2f));
			JewelryPos.Add(new Vector3(-619.357f, -230.289f, 37.2f));
			JewelryPos.Add(new Vector3(-617.668f, -230.468f, 37.2f));
			JewelryPos.Add(new Vector3(-618.44f, -229.531f, 37.2f));
			JewelryPos.Add(new Vector3(-619.814f, -227.669f, 37.2f));
			JewelryPos.Add(new Vector3(-620.629f, -226.903f, 37.2f));
			JewelryPos.Add(new Vector3(-623.643f, -227.289f, 37.2f));
			JewelryPos.Add(new Vector3(-624.627f, -228.187f, 37.2f));
			foreach (Blip robbedJewelsCasesBlip in RobbedJewelsCasesBlips)
			{
				if (robbedJewelsCasesBlip != null)
				{
					robbedJewelsCasesBlip.Delete();
				}
			}
			foreach (Prop jewel in Jewels)
			{
				if (jewel != null)
				{
					jewel.Delete();
				}
			}
			ManageRobberyAnims.CurrentPay = 0;
			JewelStoreStage = HeistStage.Stage1;
			MissionOn = true;
			GotJewels = false;
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -624f, -232f, 38f);
			if (num != 0)
			{
				Function.Call(Hash.REFRESH_INTERIOR, num);
			}
			Game.Player.Character.IsPositionFrozen = false;
			JewelStoreRobberyOn = true;
			InHeist = true;
			CurrentHeist = HeistIn.Jewel;
		}

		public static void StartFleeca(int bank)
		{
			FleecaHeistStage = 0;
			ManageRobberyAnims.CurrentPay = 0;
			HeistMinPay = Fleeca_HeistMinPay;
			HeistMaxPay = Fleeca_HeistMaxPay;
			ManageRobberyAnims.Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 1.25f);
			foreach (Prop prop in Props)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			ManageRobberyAnims.DeleteLoot();
			ManageRobberyAnims.Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 1.25f);
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
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
			if (LootId.Count > 0)
			{
				LootId.Clear();
			}
			if (Crate_loc.Count > 0)
			{
				Crate_loc.Clear();
			}
			if (Crate_Rot.Count > 0)
			{
				Crate_Rot.Clear();
			}
			if (PaintingsFinished.Count > 0)
			{
				PaintingsFinished.Clear();
			}
			BankHeistResponceType = 0;
			KilledCops = 0;
			VaultOpen = false;
			DefendingCops = false;
			escapingcops = false;
			HeadingtoArcade = false;
			PlayingThermitedoor4 = false;
			ThermiteDoor4 = false;
			ThirmiteDoor1 = false;
			ThirmiteDoor2 = false;
			ThirmiteDoor3 = false;
			PlayeringThirmiteDoor1 = false;
			PlayeringThirmiteDoor2 = false;
			PlayeringThirmiteDoor3 = false;
			CreatedBlips = false;
			LockedDoors = false;
			StartDrill = false;
			DrillDone = false;
			CivsAreFreakingOut = false;
			NotifyEnd = false;
			GetCameras = false;
			OpenedDoor = false;
			DoorALocked = false;
			DoorBLocked = false;
			FoundKeyCard = false;
			DoorCLocked = false;
			DoorDLocked = false;
			GotKeyCard = false;
			HackKeyPadDone = false;
			TriggerCops = false;
			GotVaultDoor = false;
			PlayingCutscene = false;
			CreatedCrates = false;
			InitialAnimDone = false;
			Isdrilliing = false;
			QuitHacking = false;
			isHacking = false;
			HackDone = false;
			Hackedkeypad = false;
			RequestEndCutscene = false;
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			SetRob = false;
			BankHeistStage = 0;
			if (Laptop != null)
			{
				Laptop.Delete();
			}
			if (HackCard != null)
			{
				HackCard.Delete();
			}
			Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
			Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
			if (BagProp != null)
			{
				BagProp.Delete();
			}
			if (teller != null)
			{
				teller.Delete();
			}
			foreach (Blip cameraBlip in CameraBlips)
			{
				if (cameraBlip != null)
				{
					cameraBlip.Delete();
				}
			}
			if (bank == -1)
			{
				Bank_Location = GetNearestFleecaBankIndex();
			}
			else
			{
				if (bank < 100)
				{
					Bank_Location = 0;
				}
				if (bank >= 100 && bank < 200)
				{
					Bank_Location = 1;
				}
				if (bank >= 200 && bank < 300)
				{
					Bank_Location = 2;
				}
				if (bank >= 300 && bank < 400)
				{
					Bank_Location = 3;
				}
				if (bank >= 400 && bank < 500)
				{
					Bank_Location = 4;
				}
				if (bank >= 500 && bank <= 600)
				{
					Bank_Location = 5;
				}
			}
			GetBank(Bank_Location);
			StartDrill = false;
			DrillDone = false;
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 6, 0, 1);
			ManageRobberyAnims.CurrentPay = 0;
			HeistTime = 0f;
			TriggerUINotify = true;
			if (JammmerProp != null)
			{
				JammmerProp.Delete();
			}
			if (Insurgent != null)
			{
				Insurgent.Delete();
			}
			if (Kuruma != null)
			{
				Kuruma.Delete();
			}
			if (InsurgentBLip != null)
			{
				InsurgentBLip.Delete();
			}
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			Bank = GlobalHelper.CreateBlip(FleecaBankMarker);
			Bank.Sprite = BlipSprite.DollarBill;
			Bank.Name = "Fleeca Bank #" + Bank_Location;
			Bank.ShowRoute = true;
			Bank.IsShortRange = true;
			placedJammer = true;
			GetCameras = false;
			NotifyEnd = false;
			Game.Player.Character.IsPositionFrozen = false;
			Game.Player.CanControlCharacter = true;
			Random random = new Random();
			Waittime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 25000, 60000);
			TriggerUINotify = true;
			MissionOn = true;
			PlayingFleeca = true;
			CurrentHeist = HeistIn.Fleeca;
		}

		public static void StartPaleto()
		{
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.DeleteLoot();
			HeistMinPay = Paleto_HeistMinPay;
			HeistMaxPay = Paleto_HeistMaxPay;
			ManageRobberyAnims.Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 1.45f);
			foreach (Prop prop in Props)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
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
			if (LootId.Count > 0)
			{
				LootId.Clear();
			}
			if (Crate_loc.Count > 0)
			{
				Crate_loc.Clear();
			}
			if (Crate_Rot.Count > 0)
			{
				Crate_Rot.Clear();
			}
			if (PaintingsFinished.Count > 0)
			{
				PaintingsFinished.Clear();
			}
			PaletoCopsIncoming = false;
			BankHeistResponceType = 0;
			KilledCops = 0;
			VaultOpen = false;
			DefendingCops = false;
			escapingcops = false;
			HeadingtoArcade = false;
			PlayingThermitedoor4 = false;
			ThermiteDoor4 = false;
			ThirmiteDoor1 = false;
			ThirmiteDoor2 = false;
			ThirmiteDoor3 = false;
			PlayeringThirmiteDoor1 = false;
			PlayeringThirmiteDoor2 = false;
			PlayeringThirmiteDoor3 = false;
			CreatedBlips = false;
			LockedDoors = false;
			StartDrill = false;
			DrillDone = false;
			CivsAreFreakingOut = false;
			NotifyEnd = false;
			GetCameras = false;
			OpenedDoor = false;
			DoorALocked = false;
			DoorBLocked = false;
			FoundKeyCard = false;
			DoorCLocked = false;
			DoorDLocked = false;
			GotKeyCard = false;
			HackKeyPadDone = false;
			TriggerCops = false;
			GotVaultDoor = false;
			PlayingCutscene = false;
			CreatedCrates = false;
			InitialAnimDone = false;
			Isdrilliing = false;
			QuitHacking = false;
			isHacking = false;
			HackDone = false;
			Hackedkeypad = false;
			RequestEndCutscene = false;
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			SetRob = false;
			BankHeistStage = 0;
			foreach (Blip cameraBlip in CameraBlips)
			{
				if (cameraBlip != null)
				{
					cameraBlip.Delete();
				}
			}
			HeistTime = 0f;
			ManageRobberyAnims.CurrentPay = 0;
			TriggerUINotify = true;
			if (Bank != null)
			{
				Bank.Delete();
			}
			if (GetawayCar != null)
			{
				GetawayCar.Delete();
			}
			foreach (Ped guard in Guards)
			{
				if (guard != null)
				{
					guard.Delete();
				}
			}
			foreach (Ped teller in Tellers)
			{
				if (teller != null)
				{
					teller.Delete();
				}
			}
			foreach (Ped civ in Civs)
			{
				if (civ != null)
				{
					civ.Delete();
				}
			}
			Ped ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(-118.009f, 6469.7f, 30f), -135f);
			ped.Weapons.Give(WeaponHash.SMG, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(-102.544f, 6470.59f, 30f), 123f);
			ped.Weapons.Give(WeaponHash.SMG, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(-99.5375f, 6461.291f, 30f), 42f);
			ped.Weapons.Give(WeaponHash.SMG, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(-110.8344f, 6458.824f, 30f), 132f);
			ped.Weapons.Give(WeaponHash.SMG, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Business01AFY, new Vector3(-111.0587f, 6470.092f, 30f), 128f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Business01AMY, new Vector3(-112.1814f, 6471.075f, 30f), 128f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Business01AMM, new Vector3(-110.1623f, 6468.925f, 30f), 128f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(-107.435f, 5465.91f, 30f));
			ped.Heading = -20f;
			ped.Task.WanderAround(ped.Position, 5f);
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(-107.435f, 5465.91f, 30f));
			ped.Heading = -20f;
			ped.Task.WanderAround(ped.Position, 5f);
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(-112.066f, 6465.93f, 30f));
			ped.Heading = -20f;
			ped.Task.WanderAround(ped.Position, 5f);
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(-112.066f, 6465.93f, 30f));
			ped.Heading = -20f;
			ped.Task.WanderAround(ped.Position, 5f);
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(-112.066f, 6465.93f, 30f));
			ped.Heading = -20f;
			Civs.Add(ped);
			foreach (Ped guard2 in Guards)
			{
				if (guard2 != null)
				{
					SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(guard2, SmartGuards.EnemyGroup.Security));
				}
			}
			ThirmiteDoor1 = false;
			PlayeringThirmiteDoor1 = false;
			Bank = GlobalHelper.CreateBlip(PeletoBankMarker);
			Bank.Sprite = BlipSprite.DollarBill;
			Bank.Name = "Paleto Bank";
			Bank.ShowRoute = true;
			Bank.IsShortRange = true;
			placedJammer = true;
			GetCameras = false;
			NotifyEnd = false;
			ThirmiteDoor1 = false;
			PlayeringThirmiteDoor1 = false;
			Game.Player.Character.IsPositionFrozen = false;
			Game.Player.CanControlCharacter = true;
			Random random = new Random();
			Waittime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 25000, 60000);
			TriggerUINotify = true;
			MissionOn = true;
			PlayingPaleto = true;
			CurrentHeist = HeistIn.Paleto;
			Notification.Show("~y~ Lester ~w~ Ok head up to Paleto Bay, and get inside the Vault, and do what you do best!");
		}

		public Prop GetClosestTellerChair(Vector3 Pos)
		{
			return Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Pos.X, Pos.Y, Pos.Z, 2f, Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_bk_chair3"), 0, 0, 0);
		}

		public static void StartPacificStandard()
		{
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.DeleteLoot();
			ExitPacStandard = false;
			HackedPacificStandardSecurityPad = false;
			PacificStandardCivsAndTellerInCower = false;
			HeistMinPay = PacificStandard_HeistMinPay;
			HeistMaxPay = PacificStandard_HeistMaxPay;
			ManageRobberyAnims.Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 1.25f);
			foreach (Prop prop in Props)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
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
			if (LootId.Count > 0)
			{
				LootId.Clear();
			}
			if (Crate_loc.Count > 0)
			{
				Crate_loc.Clear();
			}
			if (Crate_Rot.Count > 0)
			{
				Crate_Rot.Clear();
			}
			if (PaintingsFinished.Count > 0)
			{
				PaintingsFinished.Clear();
			}
			BankHeistResponceType = 0;
			KilledCops = 0;
			VaultOpen = false;
			DefendingCops = false;
			escapingcops = false;
			HeadingtoArcade = false;
			PlayingThermitedoor4 = false;
			ThermiteDoor4 = false;
			ThirmiteDoor1 = false;
			ThirmiteDoor2 = false;
			ThirmiteDoor3 = false;
			PlayeringThirmiteDoor1 = false;
			PlayeringThirmiteDoor2 = false;
			PlayeringThirmiteDoor3 = false;
			CreatedBlips = false;
			LockedDoors = false;
			StartDrill = false;
			DrillDone = false;
			CivsAreFreakingOut = false;
			NotifyEnd = false;
			GetCameras = false;
			OpenedDoor = false;
			DoorALocked = false;
			DoorBLocked = false;
			FoundKeyCard = false;
			DoorCLocked = false;
			DoorDLocked = false;
			GotKeyCard = false;
			HackKeyPadDone = false;
			TriggerCops = false;
			GotVaultDoor = false;
			PlayingCutscene = false;
			CreatedCrates = false;
			InitialAnimDone = false;
			Isdrilliing = false;
			QuitHacking = false;
			isHacking = false;
			HackDone = false;
			Hackedkeypad = false;
			RequestEndCutscene = false;
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			SetRob = false;
			BankHeistStage = 0;
			PlayingThermitedoor4 = false;
			ThermiteDoor4 = false;
			foreach (Blip cameraBlip in CameraBlips)
			{
				if (cameraBlip != null)
				{
					cameraBlip.Delete();
				}
			}
			LocalTake = 0;
			ThirmiteDoor1 = false;
			ThirmiteDoor2 = false;
			ThirmiteDoor3 = false;
			PlayeringThirmiteDoor1 = false;
			PlayeringThirmiteDoor2 = false;
			PlayeringThirmiteDoor3 = false;
			CanNoLongerOverideCommands = false;
			RobberARobbing = false;
			RobberACuttingStage = 0;
			RobberAInitialAnimDone = false;
			RobberA_RobberMarker = new Vector3(0f, 0f, 0f);
			RobberBRobbing = false;
			RobberBCuttingStage = 0;
			RobberBInitialAnimDone = false;
			RobberB_RobberMarker = new Vector3(0f, 0f, 0f);
			RobberCRobbing = false;
			RobberCCuttingStage = 0;
			RobberCInitialAnimDone = false;
			RobberC_RobberMarker = new Vector3(0f, 0f, 0f);
			HeistTime = 0f;
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 6, 0, 1);
			ManageRobberyAnims.CurrentPay = 0;
			if (DoorAblip != null)
			{
				DoorAblip.Delete();
			}
			if (DoorBblip != null)
			{
				DoorBblip.Delete();
			}
			if (DoorCblip != null)
			{
				DoorCblip.Delete();
			}
			if (DoorDblip != null)
			{
				DoorDblip.Delete();
			}
			foreach (Ped guard in Guards)
			{
				if (guard != null)
				{
					guard.Delete();
				}
			}
			Guards.Clear();
			foreach (Ped teller in Tellers)
			{
				if (teller != null)
				{
					teller.Delete();
				}
			}
			foreach (Ped civ in Civs)
			{
				if (civ != null)
				{
					civ.Delete();
				}
			}
			Ped ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(238.0773f, 218.1296f, 110f), -60f);
			ped.Weapons.Give(WeaponHash.SMG, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(246.4415f, 214.1346f, 105f), -20f);
			ped.Weapons.Give(WeaponHash.SMG, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(260.1138f, 217.36f, 105f), 77f);
			ped.Weapons.Give(WeaponHash.SMG, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Security01SMM, new Vector3(233.8793f, 213.4102f, 105f), -55f);
			ped.Weapons.Give(WeaponHash.SMG, 9999, true, true);
			Guards.Add(ped);
			foreach (Ped guard2 in Guards)
			{
				SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(guard2, SmartGuards.EnemyGroup.Security));
			}
			ped = GlobalHelper.CreatePed(PedHash.Business01AFY, new Vector3(254.0542f, 222.7347f, 105.5315f), 156f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Business01AMM, new Vector3(252.2567f, 223.3889f, 105.5315f), 156f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Business02AFY, new Vector3(248.9468f, 224.5936f, 105.5315f), 156f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Business03AFY, new Vector3(247.1492f, 225.2479f, 105.5315f), 156f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Business03AFY, new Vector3(243.8184f, 226.4602f, 105.5315f), 156f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Business03AFY, new Vector3(242.0206f, 227.1146f, 105.5315f), 156f);
			Tellers.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(253.5543f, 220.6448f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(253.3319f, 220.0604f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(251.6954f, 221.3695f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(248.3533f, 222.6015f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(248.3533f, 222.1f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(248.3533f, 221.6f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(246.6219f, 223.2135f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(246.6219f, 223.2135f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(246.6219f, 222.1f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(243.1322f, 224.2072f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(243.1322f, 223.1866f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(241.4443f, 225.1371f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(241.4443f, 224.1371f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(241.4443f, 223.1371f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(241.4443f, 222.1371f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			ped = GlobalHelper.CreateRandomPed(new Vector3(241.4443f, 221.1371f, 105f));
			ped.Heading = -20f;
			Civs.Add(ped);
			if (DoorAblip != null)
			{
				DoorAblip.Delete();
			}
			if (DoorBblip != null)
			{
				DoorBblip.Delete();
			}
			if (DoorCblip != null)
			{
				DoorCblip.Delete();
			}
			if (DoorDblip != null)
			{
				DoorDblip.Delete();
			}
			CreatedBlips = false;
			LockedDoors = false;
			CivsAreFreakingOut = false;
			TargetingTime = 0;
			if (Manager != null)
			{
				Manager.Delete();
			}
			TriggerUINotify = true;
			if (Insurgent != null)
			{
				Insurgent.Delete();
			}
			if (Kuruma != null)
			{
				Kuruma.Delete();
			}
			if (InsurgentBLip != null)
			{
				InsurgentBLip.Delete();
			}
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			LockedDoors = false;
			tellerwalkStage = 0;
			Offset = new Vector3(258.3f, 206.53f, 110.283f);
			Manager = GlobalHelper.CreatePed(PedHash.Business01AMM, new Vector3(249.233f, 211.4478f, 110f), -14f);
			Manager.BlockPermanentEvents = true;
			DoorALocked = false;
			DoorBLocked = false;
			FoundKeyCard = false;
			DoorCLocked = true;
			DoorDLocked = true;
			GotKeyCard = false;
			Bank = GlobalHelper.CreateBlip(new Vector3(223.06f, 207.39f, 104.5f));
			Bank.Sprite = BlipSprite.DollarBill;
			Bank.Name = "Pacific_Standard Bank #2";
			Bank.ShowRoute = true;
			Bank.IsShortRange = true;
			placedJammer = true;
			HackKeyPadDone = true;
			Game.Player.Character.IsPositionFrozen = false;
			Game.Player.CanControlCharacter = true;
			Random random = new Random();
			Waittime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 25000, 60000);
			TriggerUINotify = true;
			MissionOn = true;
			NotifyEnd = false;
			TriggerCops = false;
			foreach (Ped guard3 in Guards)
			{
				RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "COP");
				Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, guard3, relationshipGroup);
				Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, guard3, 0, 0);
				Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, guard3, 1);
			}
			PlayingPacificStandard = true;
			CurrentHeist = HeistIn.PacStandard;
		}

		public static void EndHouseRobbery2()
		{
			ManageRobberyAnims.DeleteLoot();
			foreach (Prop prop in Props)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Prop painting in Paintings)
			{
				if (painting != null)
				{
					painting.Delete();
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
			if (LootId.Count > 0)
			{
				LootId.Clear();
			}
			if (Crate_loc.Count > 0)
			{
				Crate_loc.Clear();
			}
			if (Crate_Rot.Count > 0)
			{
				Crate_Rot.Clear();
			}
			if (PaintingsFinished.Count > 0)
			{
				PaintingsFinished.Clear();
			}
			Random random = new Random();
			InHouseRobbery = false;
			EscapingHouseRobbery = false;
		}

		public static void StartInLaFuetaBlancaHeist()
		{
			ManageRobberyAnims.CurrentPay = 0;
			HeistMinPay = LaFuenteBlanca_HeistMinPay;
			HeistMaxPay = LaFuenteBlanca_HeistMaxPay;
			ManageRobberyAnims.Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 1.25f);
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			BankHeistStage = 0;
			hackedMadrazoFrontDoor = false;
			foreach (KeypadClass martinMadrazoKeyPad in MartinMadrazoKeyPads)
			{
				if (martinMadrazoKeyPad.Keypad != null)
				{
					martinMadrazoKeyPad.Keypad.Delete();
				}
				martinMadrazoKeyPad.SpawnedKeypad = false;
			}
			EndHouseRobbery2();
			foreach (Blip cameraBlip in CameraBlips)
			{
				cameraBlip.Delete();
			}
			foreach (Prop item2 in SecurityCam)
			{
				item2.Delete();
			}
			CameraBlips.Clear();
			SecurityCam.Clear();
			Prop item = GlobalHelper.CreateProp(RequestModel("prop_cctv_cam_06a"), new Vector3(1390.27f, 1143.27f, 116.85f), new Vector3(0f, 0f, -29.09f), false, false);
			SecurityCam.Add(item);
			item = GlobalHelper.CreateProp(RequestModel("prop_cctv_cam_06a"), new Vector3(1390.27f, 1151.52f, 116.85f), new Vector3(0f, 0f, -109.09f), false, false);
			SecurityCam.Add(item);
			item = GlobalHelper.CreateProp(RequestModel("prop_cctv_cam_06a"), new Vector3(1390.34f, 1136.35f, 116.65f), new Vector3(0f, 0f, 163.79f), false, false);
			SecurityCam.Add(item);
			item = GlobalHelper.CreateProp(RequestModel("prop_cctv_cam_06a"), new Vector3(1408.85f, 1128.12f, 116.64f), new Vector3(0f, 0f, 36.36f), false, false);
			SecurityCam.Add(item);
			item = GlobalHelper.CreateProp(RequestModel("prop_cctv_cam_06a"), new Vector3(1409.78f, 1151.48f, 116.84f), new Vector3(0f, 0f, 156.12f), false, false);
			SecurityCam.Add(item);
			item = GlobalHelper.CreateProp(RequestModel("prop_cctv_cam_06a"), new Vector3(1408.72f, 1158.63f, 116.23f), new Vector3(0f, 0f, 48.16f), false, false);
			SecurityCam.Add(item);
			item = GlobalHelper.CreateProp(RequestModel("prop_cctv_cam_06a"), new Vector3(1390.35f, 1128.08f, 116.73f), new Vector3(0f, 0f, -27.94f), false, false);
			SecurityCam.Add(item);
			item = GlobalHelper.CreateProp(RequestModel("prop_cctv_cam_06a"), new Vector3(1390.44f, 1166.64f, 116.59f), new Vector3(0f, 0f, -94.24f), false, false);
			SecurityCam.Add(item);
			foreach (Blip cameraBlip2 in CameraBlips)
			{
				cameraBlip2.Delete();
			}
			foreach (Prop item3 in SecurityCam)
			{
				item3.Position = new Vector3(item3.Position.X, item3.Position.Y, item3.Position.Z - 0.4f);
				item3.AddBlip();
				item3.AttachedBlip.Sprite = BlipSprite.Standard;
				item3.AttachedBlip.Color = BlipColor.Red2;
				item3.AttachedBlip.Name = "Camera";
				item3.AttachedBlip.Scale = 0.5f;
				CameraBlips.Add(item3.AttachedBlip);
			}
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			CamerasLeft = SecurityCam.Count + 1;
			KurumaBlip = GlobalHelper.CreateBlip(MartinMadrazoHouseEnterance);
			KurumaBlip.Alpha = 0;
			LastRobberyPosition = Game.Player.Character.Position;
			InLaFuetaBlancaHeist = true;
			CurrentHeist = HeistIn.LaFuetaBlanca;
		}

		public static void StartUnionDepositoryHeist()
		{
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.DeleteLoot();
			OverrideDisguiseIfAimingGun = false;
			OverrideDisguiseIfNotUnarmed = false;
			IsDressedUpAsBugstarsOrGroupe6 = false;
			HeistMinPay = UnionDepository_HeistMinPay;
			HeistMaxPay = UnionDepository_HeistMaxPay;
			Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 1.05f);
			AllBombsPlaced = false;
			Function.Call(Hash.REMOVE_IPL, "FINBANK");
			Function.Call(Hash.REQUEST_IPL, "FINBANK");
			Function.Call(Hash.REMOVE_IPL, "sunkcargoship");
			Function.Call(Hash.REMOVE_IPL, "dt1_03_gr_closed");
			Function.Call(Hash.REQUEST_IPL, "dt1_03_gr_closed");
			Function.Call(Hash.REMOVE_IPL, "cargoship");
			Function.Call(Hash.REQUEST_IPL, "cargoship");
			DeleteUnionNewAssets();
			foreach (Blip intelMarker in IntelMarkers)
			{
				if (intelMarker != null)
				{
					intelMarker.Delete();
				}
			}
			CurrentUnionApproachToottip = ApproachToolTip.NoneScouting;
			CurrentUnionApproach = ApproachStyle.NoneScouting;
			CurrentUnionDPstage = UnionDPStage.ApproachOrScout;
			GotCameras = false;
			foreach (Blip cameraBlip in CameraBlips)
			{
				if (cameraBlip != null)
				{
					cameraBlip.Delete();
				}
			}
			HeistTime = 0f;
			LocalTake = 0;
			CanNoLongerOverideCommands = false;
			if (MoneyProp != null)
			{
				MoneyProp.Delete();
			}
			if (_cashGrabTray1 != null)
			{
				_cashGrabTray1.Delete();
			}
			if (bagProp != null)
			{
				bagProp.Delete();
			}
			foreach (Prop unionDPTopFloorProp in UnionDPTopFloorProps)
			{
				if (unionDPTopFloorProp != null)
				{
					unionDPTopFloorProp.Delete();
				}
			}
			if (UnionDP_C4Crate != null)
			{
				UnionDP_C4Crate.Delete();
			}
			foreach (ObjectPlacementMap item in UnionDPTopFloor)
			{
				Prop prop = GlobalHelper.CreateProp(RequestModel(item.Hash), item.Position, item.Rotation, false, false);
				prop.IsPositionFrozen = true;
				UnionDPTopFloorProps.Add(prop);
			}
			Random random = new Random();
			ObjectPlacementMap objectPlacementMap = UnionDP_C4CrateLocs[random.Next(0, UnionDP_C4CrateLocs.Count)];
			UnionDP_C4Crate = GlobalHelper.CreateProp(RequestModel("prop_drop_crate_01"), objectPlacementMap.Position, objectPlacementMap.Rotation, false, false);
			UnionDP_C4Crate.IsPositionFrozen = true;
			UnionDP_C4Crate.LodDistance = 3000;
			foreach (ObjectPlacementMap unionDP_C4CrateLoc in UnionDP_C4CrateLocs)
			{
				Blip blip = GlobalHelper.CreateBlip(unionDP_C4CrateLoc.Position);
				blip.Sprite = BlipSprite.SpecialCargo;
				blip.Color = BlipColor.Green;
				blip.IsShortRange = true;
				blip.Alpha = 0;
				blip.Scale = 0.5f;
				CheckCrateLoc.Add(false);
				CheckCrateLocBlip.Add(blip);
			}
			CutterVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Cutter), CutterLoc.Item1, CutterLoc.Item2.Z);
			CutterVeh.LockStatus = VehicleLockStatus.CannotEnter;
			CutterVeh.AddBlip();
			CutterVeh.AttachedBlip.Color = BlipColor.Blue;
			CutterVeh.AttachedBlip.Name = "Cutter for Union Depository";
			CutterVeh.AttachedBlip.IsShortRange = true;
			CutterVeh.IsPersistent = true;
			BugstarsVan = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Burrito2), BugstarVanLoc.Item1, BugstarVanLoc.Item2.Z);
			BugstarsVan.AddBlip();
			BugstarsVan.AttachedBlip.Color = BlipColor.Blue;
			BugstarsVan.AttachedBlip.Name = "Bugstars Van for Union Depository";
			BugstarsVan.AttachedBlip.IsShortRange = true;
			BugstarsVan.IsPersistent = true;
			MaverickHeli = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Maverick), MaverickLoc.Item1, MaverickLoc.Item2.Z);
			MaverickHeli.IsInvincible = true;
			MaverickHeli.LockStatus = VehicleLockStatus.CannotEnter;
			MaverickHeli.AddBlip();
			MaverickHeli.AttachedBlip.Color = BlipColor.Blue;
			MaverickHeli.AttachedBlip.Name = "Maverick for Union Depository";
			MaverickHeli.AttachedBlip.IsShortRange = true;
			MaverickHeli.IsPersistent = true;
			Gruppe6Stockade = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), Gruppe6VanLoc.Item1, Gruppe6VanLoc.Item2.Z);
			Gruppe6Stockade.AddBlip();
			Gruppe6Stockade.AttachedBlip.Color = BlipColor.Blue;
			Gruppe6Stockade.AttachedBlip.Name = "Stockade for Union Depository";
			Gruppe6Stockade.AttachedBlip.IsShortRange = true;
			Gruppe6Stockade.IsPersistent = true;
			MaverickPilot = GlobalHelper.CreatePed(RequestModel(PedHash.Pilot02SMM), MaverickHeli.GetOffsetPosition(new Vector3(1f, 1f, 0f)));
			MaverickPilot.BlockPermanentEvents = true;
			CutterKeys = GlobalHelper.CreateProp(RequestModel(CutterKeysModel), CutterKeysLoc.Item1, CutterKeysLoc.Item2, false, false);
			CutterKeys.IsPositionFrozen = true;
			Gruppe6Clothes = GlobalHelper.CreateProp(RequestModel(Gruppe6ClothesBagModel), Gruppe6ClothesLoc.Item1, Gruppe6ClothesLoc.Item2, false, false);
			Gruppe6Clothes.IsPositionFrozen = true;
			BugstarClothes = GlobalHelper.CreateProp(RequestModel(BugstarsClothesBagModel), BugstarClothesLoc.Item1, BugstarClothesLoc.Item2, false, false);
			BugstarClothes.IsPositionFrozen = true;
			GetawayCarDeliveryPed = GlobalHelper.CreatePed(RequestModel("mp_g_m_pros_01"), new Vector3(600.06f, -507.78f, 24.59f), 76f);
			GetawayCarDeliveryPed.BlockPermanentEvents = true;
			GetawayCarDeliveryPed.AddBlip();
			GetawayCarDeliveryPed.AttachedBlip.Color = BlipColor.Yellow;
			GetawayCarDeliveryPed.AttachedBlip.Name = "Broker";
			GetawayCarDeliveryPed.AttachedBlip.IsShortRange = true;
			GetawayCarDeliveryPed.IsPersistent = true;
			Blip blip2 = GlobalHelper.CreateBlip(new Vector3(22.1f, -638.29f, 30.18f));
			blip2.Sprite = BlipSprite.TheBureauRaid;
			blip2.Name = "Grate Enterance";
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			blip2.Color = BlipColor.Yellow2;
			EnteranceBlipMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(new Vector3(22.1f, -638.29f, 30.18f), 20f);
			blip2.Name = "Grate Enterance";
			blip2.Color = BlipColor.Yellow2;
			EnteranceBlipMarkersRadiusArea.Add(blip2);
			blip2.Alpha = 140;
			blip2 = GlobalHelper.CreateBlip(new Vector3(25.86f, -662.72f, 30.18f));
			blip2.Sprite = BlipSprite.TheBureauRaid;
			blip2.Color = BlipColor.Yellow2;
			blip2.Name = "Back Enterance";
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			EnteranceBlipMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(new Vector3(25.86f, -662.72f, 30.18f), 20f);
			blip2.Name = "Back Enterance";
			blip2.Color = BlipColor.Yellow2;
			EnteranceBlipMarkersRadiusArea.Add(blip2);
			blip2.Alpha = 140;
			blip2 = GlobalHelper.CreateBlip(new Vector3(4.9f, -700.15f, 45.96f));
			blip2.Sprite = BlipSprite.TheBureauRaid;
			blip2.Color = BlipColor.Yellow2;
			blip2.Name = "Front Enterance";
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			EnteranceBlipMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(new Vector3(4.9f, -700.15f, 45.96f), 20f);
			blip2.Name = "Front Enterance";
			blip2.Color = BlipColor.Yellow2;
			EnteranceBlipMarkersRadiusArea.Add(blip2);
			blip2.Alpha = 140;
			blip2 = GlobalHelper.CreateBlip(new Vector3(-86.5f, -677.06f, 35.25f));
			blip2.Sprite = BlipSprite.TheBureauRaid;
			blip2.Color = BlipColor.Yellow2;
			blip2.Name = "Side Enterance";
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			EnteranceBlipMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(new Vector3(-86.5f, -677.06f, 35.25f), 20f);
			blip2.Name = "Side Enterance";
			blip2.Color = BlipColor.Yellow2;
			blip2.Alpha = 140;
			EnteranceBlipMarkersRadiusArea.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(new Vector3(134.32f, -3138.63f, 18.39f));
			blip2.Sprite = BlipSprite.HeistSetup;
			blip2.Color = BlipColor.Green;
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			blip2.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(new Vector3(134.32f, -3138.63f, 18.39f), 100f);
			blip2.Name = "Possible Intel/Asset";
			blip2.Color = BlipColor.Green;
			blip2.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(new Vector3(-198.56f, -2391.212f, 6f));
			blip2.Sprite = BlipSprite.HeistSetup;
			blip2.Color = BlipColor.Green;
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			blip2.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(new Vector3(-198.56f, -2391.212f, 6f), 100f);
			blip2.Name = "Possible Intel/Asset";
			blip2.Color = BlipColor.Green;
			blip2.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(CutterKeysLoc.Item1);
			blip2.Sprite = BlipSprite.HeistSetup;
			blip2.Color = BlipColor.Green;
			blip2.Name = "Possible Intel/Asset";
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			IntelMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(CutterKeysLoc.Item1, 100f);
			blip2.Name = "Possible Intel/Asset";
			blip2.Color = BlipColor.Green;
			blip2.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(MaverickLoc.Item1);
			blip2.Sprite = BlipSprite.HeistSetup;
			blip2.Color = BlipColor.Green;
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			blip2.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(MaverickLoc.Item1, 100f);
			blip2.Name = "Possible Intel/Asset";
			blip2.Color = BlipColor.Green;
			blip2.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(Gruppe6ClothesLoc.Item1);
			blip2.Sprite = BlipSprite.HeistSetup;
			blip2.Color = BlipColor.Green;
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			blip2.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(Gruppe6ClothesLoc.Item1, 100f);
			blip2.Name = "Possible Intel/Asset";
			blip2.Color = BlipColor.Green;
			blip2.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(Gruppe6VanLoc.Item1);
			blip2.Sprite = BlipSprite.HeistSetup;
			blip2.Color = BlipColor.Green;
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			blip2.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(Gruppe6VanLoc.Item1, 100f);
			blip2.Name = "Possible Intel/Asset";
			blip2.Color = BlipColor.Green;
			blip2.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(GetawayCarDeliveryPed.Position);
			blip2.Sprite = BlipSprite.HeistSetup;
			blip2.Color = BlipColor.Green;
			blip2.IsShortRange = true;
			blip2.Scale = 0.5f;
			blip2.Name = "Possible Intel/Asset";
			IntelMarkers.Add(blip2);
			blip2 = GlobalHelper.CreateBlip(GetawayCarDeliveryPed.Position, 100f);
			blip2.Name = "Possible Intel/Asset";
			blip2.Color = BlipColor.Green;
			blip2.Alpha = 140;
			IntelMarkersRadiusArea.Add(blip2);
			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 6, 0, 1);
			Prop[] nearbyProps = World.GetNearbyProps(new Vector3(-1.880968f, -687.0798f, 16f), 1f);
			if (nearbyProps.Length != 0)
			{
				Prop[] array = nearbyProps;
				foreach (Prop prop2 in array)
				{
					if (prop2 != null)
					{
						VaultD = prop2;
					}
				}
			}
			TriggerVault = false;
			VaultUnlocked = false;
			ManageRobberyAnims.CurrentPay = 0;
			CreateProps = false;
			foreach (Ped guard in Guards)
			{
				if (guard != null)
				{
					guard.Delete();
				}
			}
			foreach (Ped teller in Tellers)
			{
				if (teller != null)
				{
					teller.Delete();
				}
			}
			foreach (Ped civ in Civs)
			{
				if (civ != null)
				{
					civ.Delete();
				}
			}
			Ped ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(2.732f, -659.698f, 33f), -60f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(-10.1808f, -657.667f, 33f), -60f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(11.6837f, -665.624f, 33f), 100f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(8.20965f, -665.808f, 33f), -60f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(0.0146f, -694.5891f, 16f), 61f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(-4.277f, -700.8828f, 16f), -25f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(-9.84f, -691.506f, 16f), -102f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(122.58f, -625.54f, 206.04f), 84.6f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(130.02f, -618.56f, 206.04f), 76.37f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(145.78f, -620.99f, 206.04f), 88.58f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(142.67f, -613.44f, 206.04f), 158.28f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			ped = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(137.03f, -620.43f, 206.04f), -18.49f);
			ped.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
			Guards.Add(ped);
			foreach (Ped guard2 in Guards)
			{
				if (guard2 != null && guard2.IsAlive)
				{
					SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(guard2, SmartGuards.EnemyGroup.Security));
				}
			}
			if (DoorAblip != null)
			{
				DoorAblip.Delete();
			}
			CreatedBlips = false;
			LockedDoors = false;
			CivsAreFreakingOut = false;
			if (Manager != null)
			{
				Manager.Delete();
			}
			TriggerUINotify = true;
			if (Insurgent != null)
			{
				Insurgent.Delete();
			}
			if (Kuruma != null)
			{
				Kuruma.Delete();
			}
			if (InsurgentBLip != null)
			{
				InsurgentBLip.Delete();
			}
			if (KurumaBlip != null)
			{
				KurumaBlip.Delete();
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			LockedDoors = false;
			Bank = GlobalHelper.CreateBlip(new Vector3(-13.7145f, -687.805f, 32f));
			Bank.Sprite = BlipSprite.Gold;
			Bank.Name = "Union Depository";
			Bank.Color = BlipColor.Yellow;
			placedJammer = true;
			Waittime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 25000, 60000);
			TriggerUINotify = true;
			MissionOn = true;
			CurrentHeist = HeistIn.UnionDP;
			PlayingUnion = true;
		}

		public void AddCrates_UnionDP()
		{
			ManageRobberyAnims.DeleteLoot();
			SearchPartyDetectDelay = 0;
			SearchPartyDestoryed = false;
			SearchPartyArrivedAtUnionDepository = false;
			SpawnedSearchParty = false;
			GetawayVehicleDetected = false;
			SpawnedGetawayVehicle = false;
			GetawayVehicleSpawnChoice = 0;
			AwawitingChoiceofSpawn = false;
			ChoosenStoredVehicleSpawn = false;
			PaidforStorageOfGetawayCar = false;
			AlertedToBrokenDisguise = false;
			ParkedAtParkingSpot = false;
			WeaponsDelay = 0;
			UnionDepositoryGuardsHatePlayer = false;
			Nodes.Add(new Vector3(9.71f, -666.312f, 16.07951f));
			Nodes.Add(new Vector3(-3.728158f, -670.8178f, 16.02941f));
			Nodes.Add(new Vector3(1.499823f, -673.1439f, 15.83023f));
			Nodes.Add(new Vector3(-0.9653265f, -678.5943f, 16.02196f));
			Nodes.Add(new Vector3(-6.56278f, -676.4349f, 16.19211f));
			Nodes.Add(new Vector3(5.633241f, -680.9384f, 16.45669f));
			Nodes.Add(new Vector3(5.843213f, -674.8979f, 15.88421f));
			Nodes.Add(new Vector3(-1.14f, -664.58f, 16.28261f));
			Nodes.Add(new Vector3(3.209539f, -667.5511f, 16.451f));
			Nodes.Add(new Vector3(-2.196378f, -681.987f, 16.12918f));
			Nodes.Add(new Vector3(3.330956f, -684.9877f, 16.53733f));
			Nodes.Add(new Vector3(-8.494119f, -680.0273f, 15.873f));
			if (MoneyProp != null)
			{
				MoneyProp.Delete();
			}
			if (_cashGrabTray1 != null)
			{
				_cashGrabTray1.Delete();
			}
			if (bagProp != null)
			{
				bagProp.Delete();
			}
			Crate_loc.Clear();
			Crate_Rot.Clear();
			Script.Wait(10);
			Crate_loc.Add(new Vector3(-0.4237852f, -657.1611f, 15.14073f));
			Crate_Rot.Add(337.4898f);
			Crate_loc.Add(new Vector3(-4.109085f, -664.4586f, 15.13073f));
			Crate_Rot.Add(70.81672f);
			Crate_loc.Add(new Vector3(-3.595407f, -662.9464f, 15.13073f));
			Crate_Rot.Add(70.86247f);
			Crate_loc.Add(new Vector3(-5.574486f, -668.8438f, 15.13073f));
			Crate_Rot.Add(71.80892f);
			Crate_loc.Add(new Vector3(2.354294f, -658.1786f, 15.13073f));
			Crate_Rot.Add(339.9172f);
			Crate_loc.Add(new Vector3(10.06568f, -660.9405f, 15.13073f));
			Crate_Rot.Add(340.3188f);
			Crate_loc.Add(new Vector3(7.402965f, -679.4984f, 15.13073f));
			Crate_Rot.Add(250.4167f);
			Crate_loc.Add(new Vector3(-8.328865f, -676.4971f, 15.13073f));
			Crate_Rot.Add(69.59763f);
			Crate_loc.Add(new Vector3(-7.321983f, -673.6937f, 15.13073f));
			Crate_Rot.Add(70.1881f);
			Crate_loc.Add(new Vector3(-6.542074f, -671.6068f, 15.13073f));
			Crate_Rot.Add(68.78564f);
			Crate_loc.Add(new Vector3(6.391189f, -682.3236f, 15.13073f));
			Crate_Rot.Add(250.2987f);
			Crate_loc.Add(new Vector3(-2.694389f, -660.3176f, 15.13073f));
			Crate_Rot.Add(69.51572f);
			Crate_loc.Add(new Vector3(-1.725182f, -657.8183f, 15.13073f));
			Crate_Rot.Add(70.20048f);
			Crate_loc.Add(new Vector3(13.47014f, -663.3132f, 15.13073f));
			Crate_Rot.Add(249.4218f);
			Crate_loc.Add(new Vector3(12.55181f, -665.8472f, 15.13073f));
			Crate_Rot.Add(249.7296f);
			Crate_loc.Add(new Vector3(12.80382f, -661.9373f, 15.13073f));
			Crate_Rot.Add(339.6811f);
			Crate_loc.Add(new Vector3(11.60648f, -668.3718f, 15.13073f));
			Crate_Rot.Add(249.9899f);
			Crate_loc.Add(new Vector3(10.9206f, -670.2504f, 15.13073f));
			Crate_Rot.Add(249.9903f);
			Crate_loc.Add(new Vector3(10.25773f, -672.038f, 15.13073f));
			Crate_Rot.Add(249.5653f);
			Crate_loc.Add(new Vector3(9.316103f, -674.2436f, 15.13073f));
			Crate_Rot.Add(249.873f);
			Crate_loc.Add(new Vector3(-4.733149f, -666.3451f, 15.13073f));
			Crate_Rot.Add(71.85616f);
			Crate_loc.Add(new Vector3(8.28974f, -677.057f, 15.13073f));
			Crate_Rot.Add(250.2511f);
			Crate_loc.Add(new Vector3(9.274776f, -675.8141f, 15.13062f));
			Crate_Rot.Add(249.4009f);
			Crate_loc.Add(new Vector3(-8.401758f, -675.3078f, 15.13062f));
			Crate_Rot.Add(70.8887f);
			Crate_loc.Add(new Vector3(-6.485773f, -670.0386f, 15.13062f));
			Crate_Rot.Add(70.69914f);
			Crate_loc.Add(new Vector3(11.60987f, -660.951f, 15.13062f));
			Crate_Rot.Add(339.4231f);
			Crate_loc.Add(new Vector3(13.45739f, -664.7424f, 15.13062f));
			Crate_Rot.Add(249.9676f);
			Crate_loc.Add(new Vector3(-2.668543f, -658.858f, 15.13062f));
			Crate_Rot.Add(70.06036f);
			Crate_loc.Add(new Vector3(1.115294f, -657.2122f, 15.13062f));
			Crate_Rot.Add(340.9109f);
			Crate_loc.Add(new Vector3(7.349669f, -681.0522f, 15.13062f));
			Crate_Rot.Add(250.2999f);
			Crate_loc.Add(new Vector3(3.913167f, -675.3004f, 15.6054f));
			Crate_Rot.Add(70.22723f);
			Crate_loc.Add(new Vector3(3.009157f, -678.0027f, 15.6054f));
			Crate_Rot.Add(70.46213f);
			Crate_loc.Add(new Vector3(4.961871f, -672.3828f, 15.6054f));
			Crate_Rot.Add(70.22723f);
			Crate_loc.Add(new Vector3(4.365735f, -676.4425f, 15.6054f));
			Crate_Rot.Add(159.6357f);
			Crate_loc.Add(new Vector3(4.083532f, -677.4326f, 15.6054f));
			Crate_Rot.Add(341.9503f);
			Crate_loc.Add(new Vector3(2.005867f, -680.8299f, 15.6054f));
			Crate_Rot.Add(70.46213f);
			Crate_loc.Add(new Vector3(-3.478009f, -679.1136f, 15.6054f));
			Crate_Rot.Add(70.79115f);
			Crate_loc.Add(new Vector3(-3.358976f, -675.8115f, 15.6054f));
			Crate_Rot.Add(249.4685f);
			Crate_loc.Add(new Vector3(-1.679995f, -674.7862f, 15.6054f));
			Crate_Rot.Add(70.34198f);
			Crate_loc.Add(new Vector3(-1.310673f, -670.2929f, 15.6054f));
			Crate_Rot.Add(249.0432f);
			Crate_loc.Add(new Vector3(-2.370085f, -673.0969f, 15.6054f));
			Crate_Rot.Add(249.1141f);
			Crate_loc.Add(new Vector3(-4.329615f, -678.5329f, 15.6054f));
			Crate_Rot.Add(249.8231f);
			Crate_loc.Add(new Vector3(-8.443607f, -678.8035f, 15.6054f));
			Crate_Rot.Add(340.5592f);
			Crate_loc.Add(new Vector3(-5.424603f, -679.8647f, 15.6054f));
			Crate_Rot.Add(340.5828f);
			Crate_loc.Add(new Vector3(6.791682f, -666.1232f, 15.6054f));
			Crate_Rot.Add(339.0942f);
			Crate_loc.Add(new Vector3(0.2945576f, -663.3441f, 15.6054f));
			Crate_Rot.Add(339.8972f);
			Crate_loc.Add(new Vector3(1.709696f, -662.0422f, 15.6054f));
			Crate_Rot.Add(250.1813f);
			Crate_loc.Add(new Vector3(0.5964375f, -662.4326f, 15.6054f));
			Crate_Rot.Add(159.0001f);
			Crate_loc.Add(new Vector3(8.155861f, -664.2801f, 15.6054f));
			Crate_Rot.Add(69.73208f);
			Crate_loc.Add(new Vector3(4.195254f, -672.0841f, 15.6054f));
			Crate_Rot.Add(251.0057f);
			Crate_loc.Add(new Vector3(-1.263842f, -668.5455f, 15.6054f));
			Crate_Rot.Add(159.7069f);
			Crate_loc.Add(new Vector3(6.09597f, -671.2809f, 15.6054f));
			Crate_Rot.Add(160.5565f);
			Crate_loc.Add(new Vector3(5.450879f, -685.5939f, 15.6054f));
			Crate_Rot.Add(249.7075f);
			Crate_loc.Add(new Vector3(-8.420816f, -682.7526f, 15.6054f));
			Crate_Rot.Add(161.0767f);
			Crate_loc.Add(new Vector3(2.727097f, -663.6865f, 15.6054f));
			Crate_Rot.Add(70.81835f);
			Script.Wait(10);
			Random random = new Random();
			for (int i = 0; i < Crate_loc.Count; i++)
			{
				Vector3 vector = Crate_loc[i];
				if (i >= 0 && i <= 21)
				{
					int num = random.Next(1, 9);
					if (num == 1)
					{
						int num2 = random.Next(0, 100);
						if (num2 < 30)
						{
							Prop prop = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop.Position = Crate_loc[i];
							prop.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop2.Position = prop.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
							prop.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop2);
						}
						else
						{
							Prop prop3 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop3.Position = Crate_loc[i];
							prop3.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop4 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop4.PositionNoOffset = Crate_loc[i];
							prop4.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop4.Position = prop3.Position;
							prop3.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop4);
						}
					}
					if (num == 2)
					{
						int num3 = random.Next(0, 100);
						if (num3 < 30)
						{
							Prop prop5 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop5.Position = Crate_loc[i];
							prop5.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop6 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01b"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop6.PositionNoOffset = Crate_loc[i];
							prop6.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop6.Position = prop5.Position;
							prop5.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop6);
						}
						else
						{
							Prop prop7 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop7.Position = Crate_loc[i];
							prop7.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop8 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop8.PositionNoOffset = Crate_loc[i];
							prop8.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop8.Position = prop7.Position;
							prop7.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop8);
						}
					}
					if (num == 3)
					{
						int num4 = random.Next(0, 100);
						if (num4 < 30)
						{
							Prop prop9 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop9.Position = Crate_loc[i];
							prop9.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop10 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01c"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop10.PositionNoOffset = Crate_loc[i];
							prop10.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop10.Position = prop9.GetOffsetPosition(new Vector3(-0.4f, 0f, 0f));
							prop9.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop10);
						}
						else
						{
							Prop prop11 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop11.Position = Crate_loc[i];
							prop11.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop12 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop12.PositionNoOffset = Crate_loc[i];
							prop12.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop12.Position = prop11.Position;
							prop11.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop12);
						}
					}
					if (num == 4)
					{
						int num5 = random.Next(0, 100);
						if (num5 < 30)
						{
							Prop prop13 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop13.Position = Crate_loc[i];
							prop13.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop14 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01b"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop14.PositionNoOffset = Crate_loc[i];
							prop14.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop14.Position = prop13.Position;
							prop13.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop14);
						}
						else
						{
							Prop prop15 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop15.Position = Crate_loc[i];
							prop15.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop16 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop16.PositionNoOffset = Crate_loc[i];
							prop16.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop16.Position = prop15.Position;
							prop15.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop16);
						}
					}
					if (num == 5)
					{
						int num6 = random.Next(0, 100);
						if (num6 < 30)
						{
							Prop prop17 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop17.Position = Crate_loc[i];
							prop17.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop18 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01e"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop18.PositionNoOffset = Crate_loc[i];
							prop18.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop18.Position = prop17.Position;
							prop17.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop18);
						}
						else
						{
							Prop prop19 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop19.Position = Crate_loc[i];
							prop19.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop20 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop20.PositionNoOffset = Crate_loc[i];
							prop20.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop20.Position = prop19.Position;
							prop19.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop20);
						}
					}
					if (num == 6)
					{
						int num7 = random.Next(0, 100);
						if (num7 < 30)
						{
							Prop prop21 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop21.Position = Crate_loc[i];
							prop21.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop22 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01f"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop22.PositionNoOffset = Crate_loc[i];
							prop22.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop22.Position = prop21.GetOffsetPosition(new Vector3(-0.4f, 0f, 0f));
							prop21.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop22);
						}
						else
						{
							Prop prop23 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop23.Position = Crate_loc[i];
							prop23.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop24 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop24.PositionNoOffset = Crate_loc[i];
							prop24.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop24.Position = prop23.Position;
							prop23.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop24);
						}
					}
					if (num == 7)
					{
						int num8 = random.Next(0, 100);
						if (num8 < 30)
						{
							Prop prop25 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop25.Position = Crate_loc[i];
							prop25.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop26 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01g"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop26.PositionNoOffset = Crate_loc[i];
							prop26.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop26.Position = prop25.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
							prop25.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop26);
						}
						else
						{
							Prop prop27 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop27.Position = Crate_loc[i];
							prop27.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop28 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop28.PositionNoOffset = Crate_loc[i];
							prop28.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop28.Position = prop27.Position;
							prop27.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop28);
						}
					}
					if (num == 8)
					{
						int num9 = random.Next(0, 100);
						if (num9 < 30)
						{
							Prop prop29 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop29.Position = Crate_loc[i];
							prop29.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop30 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01h"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop30.PositionNoOffset = Crate_loc[i];
							prop30.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop30.Position = prop29.Position;
							prop29.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop30);
						}
						else
						{
							Prop prop31 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop31.Position = Crate_loc[i];
							prop31.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop32 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop32.PositionNoOffset = Crate_loc[i];
							prop32.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop32.Position = prop31.Position;
							prop31.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop32);
						}
					}
					if (num == 9)
					{
						int num10 = random.Next(0, 100);
						if (num10 < 30)
						{
							Prop prop33 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop33.Position = Crate_loc[i];
							prop33.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop34 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01i"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop34.PositionNoOffset = Crate_loc[i];
							prop34.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop34.Position = prop33.Position;
							prop33.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop34);
						}
						else
						{
							Prop prop35 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), new Vector3(3.68f, -667.7f, 16f), new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							Script.Wait(10);
							prop35.Position = Crate_loc[i];
							prop35.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							Prop prop36 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01j"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
							prop36.PositionNoOffset = Crate_loc[i];
							prop36.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
							prop36.Position = prop35.Position;
							prop35.IsVisible = false;
							ManageRobberyAnims.AddPropAsLoot(prop36);
						}
					}
					Script.Wait(1);
				}
				if (i < 30)
				{
					continue;
				}
				int num11 = random.Next(0, 100);
				if (num11 < 30)
				{
					int num12 = random.Next(0, 600);
					if (num12 < 100)
					{
						Prop prop37 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01a"), new Vector3(Crate_loc[i].X, Crate_loc[i].Y, Crate_loc[i].Z - 0.5f), new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						ManageRobberyAnims.AddPropAsLoot(prop37);
						Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop37);
					}
					if (num12 >= 100 && num12 < 200)
					{
						Prop prop38 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01a"), new Vector3(Crate_loc[i].X, Crate_loc[i].Y, Crate_loc[i].Z - 0.5f), new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						ManageRobberyAnims.AddPropAsLoot(prop38);
						Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop38);
					}
					if (num12 >= 200 && num12 < 300)
					{
						Prop prop39 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01c"), new Vector3(Crate_loc[i].X, Crate_loc[i].Y, Crate_loc[i].Z - 0.5f), new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						ManageRobberyAnims.AddPropAsLoot(prop39);
						Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop39);
					}
					if (num12 >= 300 && num12 < 400)
					{
						Prop prop40 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01b"), new Vector3(Crate_loc[i].X, Crate_loc[i].Y, Crate_loc[i].Z - 0.5f), new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						ManageRobberyAnims.AddPropAsLoot(prop40);
						Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop40);
					}
					if (num12 >= 400 && num12 < 500)
					{
						Prop prop41 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01b"), new Vector3(Crate_loc[i].X, Crate_loc[i].Y, Crate_loc[i].Z - 0.5f), new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						ManageRobberyAnims.AddPropAsLoot(prop41);
						Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop41);
					}
					if (num12 >= 500 && num12 <= 600)
					{
						Prop prop42 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01c"), new Vector3(Crate_loc[i].X, Crate_loc[i].Y, Crate_loc[i].Z - 0.5f), new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						ManageRobberyAnims.AddPropAsLoot(prop42);
						Function.Call(Hash.PLACE_OBJECT_ON_GROUND_PROPERLY, prop42);
					}
				}
			}
			CreatedCrates = true;
		}

		public static void DeleteUnionNewAssets()
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
			foreach (Ped teller in Tellers)
			{
				if (teller != null)
				{
					teller.Delete();
				}
			}
			SpawnedSearchParty = false;
			if (StockadeB != null)
			{
				StockadeB.Delete();
			}
			PaidforStorageOfGetawayCar = false;
			if (GetawayCarDeliveryPed != null)
			{
				GetawayCarDeliveryPed.Delete();
			}
			if (VehSpawn1blip != null)
			{
				VehSpawn1blip.Delete();
			}
			if (VehSpawn2blip != null)
			{
				VehSpawn2blip.Delete();
			}
			if (HighwayEnteranceblip != null)
			{
				HighwayEnteranceblip.Delete();
			}
			RobberACanLoot = true;
			RobberBCanLoot = true;
			RobberCCanLoot = true;
			CurrentHeliWaypoint = 0;
			StartedHeliRide = false;
			UnionRappelingFromHeli = false;
			PlacingBombOnGrate = false;
			CreatedSafeAreaMarker = false;
			BombsPlaced = 0;
			StartedPlacingBombs = false;
			HasBeenInUnionVaultFloor = false;
			hackedUnionVaultDoor = false;
			if (Missionblip != null)
			{
				Missionblip.Delete();
			}
			if (Missionblip2 != null)
			{
				Missionblip2.Delete();
			}
			BrokenDisguise = false;
			WaringGruppe6Outfit = false;
			WaringBugstarsOutfit = false;
			if (Gruppe6Clothes != null)
			{
				Gruppe6Clothes.Delete();
			}
			CheckCrateLoc.Clear();
			foreach (Blip item2 in CheckCrateLocBlip)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			foreach (Prop grateBomb in GrateBombs)
			{
				if (grateBomb != null)
				{
					grateBomb.Delete();
				}
			}
			foreach (Prop unionDPTopFloorProp in UnionDPTopFloorProps)
			{
				if (unionDPTopFloorProp != null)
				{
					unionDPTopFloorProp.Delete();
				}
			}
			if (UnionDP_C4Crate != null)
			{
				UnionDP_C4Crate.Delete();
			}
			BlownUpGrate = false;
			GotCutterKeys = false;
			GotBugstarClothes = false;
			GotGruppe6Clothes = false;
			GotC4Crate = false;
			GotHiredHeli = false;
			CutterDrilledOpenVault = false;
			VaultBombUsed = false;
			if (MaverickHeli != null)
			{
				MaverickHeli.Delete();
			}
			if (MaverickPilot != null)
			{
				MaverickPilot.Delete();
			}
			if (BombProp != null)
			{
				BombProp.Delete();
			}
			if (BombBagProp != null)
			{
				BombBagProp.Delete();
			}
			foreach (int item3 in UnionPTFX)
			{
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, item3, false);
			}
			if (MovingPastTunnelPathindProp != null)
			{
				MovingPastTunnelPathindProp.Delete();
			}
			if (RobberAClone != null)
			{
				RobberAClone.Delete();
			}
			if (PlaceBombCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				PlaceBombCam.IsActive = false;
				PlaceBombCam.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
			}
			foreach (Blip item4 in BombPlacementAreaBlip)
			{
				if (item4 != null)
				{
					item4.Delete();
				}
			}
			foreach (Blip enteranceBlipMarker in EnteranceBlipMarkers)
			{
				if (enteranceBlipMarker != null)
				{
					enteranceBlipMarker.Delete();
				}
			}
			foreach (Blip item5 in IntelMarkersRadiusArea)
			{
				if (item5 != null)
				{
					item5.Delete();
				}
			}
			foreach (Blip item6 in EnteranceBlipMarkersRadiusArea)
			{
				if (item6 != null)
				{
					item6.Delete();
				}
			}
			if (CutterKeys != null)
			{
				CutterKeys.Delete();
			}
			if (CutterVeh != null)
			{
				CutterVeh.Delete();
			}
			if (BugstarsVan != null)
			{
				BugstarsVan.Delete();
			}
			if (Gruppe6Stockade != null)
			{
				Gruppe6Stockade.Delete();
			}
			if (StockadeA != null)
			{
				StockadeA.Delete();
			}
			if (StockadeB != null)
			{
				StockadeB.Delete();
			}
			if (StockadeC != null)
			{
				StockadeC.Delete();
			}
			if (TunnelHoleBlocker != null)
			{
				TunnelHoleBlocker.Delete();
			}
			if (TunnelHoleBlocker2 != null)
			{
				TunnelHoleBlocker2.Delete();
			}
			if (BugstarClothes != null)
			{
				BugstarClothes.Delete();
			}
			foreach (Ped item7 in Guards2)
			{
				if (item7 != null)
				{
					item7.Delete();
				}
			}
		}

		public static Vector3 DirectionToRotation(Vector3 dir, float RotationY)
		{
			Vector3 normalized = dir.Normalized;
			Vector3 normalized2 = new Vector3(y: new Vector3(normalized.X, normalized.Y, 0f).Length(), x: normalized.Z, z: 0f).Normalized;
			return new Vector3((float)(Math.Atan2(normalized2.X, normalized2.Y) * 57.295780181884766), RotationY, (float)(Math.Atan2(normalized.X, normalized.Y) * -57.295780181884766));
		}

		public static Vector3 GetWallPos()
		{
			Vector3 position = Game.Player.Character.Position;
			Vector3 dir = Game.Player.Character.Position - Game.Player.Character.GetOffsetPosition(new Vector3(0f, -0.5f, 0f));
			Vector3 vector = DirectionToRotation(dir, Game.Player.Character.Heading);
			float num = vector.Z * (MathF.PI / 180f);
			float num2 = vector.X * (MathF.PI / 180f);
			float num3 = (float)Math.Abs(Math.Cos(num2));
			Vector3 vector2 = new Vector3((float)Math.Sin(num) * num3 * -1f, (float)Math.Cos(num) * num3, (float)Math.Sin(num2));
			RaycastResult raycastResult = World.Raycast(position, position + vector2 * 25000f, IntersectFlags.LosToEntity);
			Vector3 result = default(Vector3);
			if (raycastResult.DidHit)
			{
				return raycastResult.HitPosition;
			}
			return result;
		}

		public void CreateCamTests()
		{
			if (Game.IsControlPressed(Control.Sprint) && Game.IsControlJustPressed(Control.Cover) && PlaceBombCam != null)
			{
				PlaceBombCam.IsActive = false;
				PlaceBombCam.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				Notification.Show("Destroy Cam");
			}
			if (Game.IsControlPressed(Control.Sprint) && Game.IsControlJustPressed(Control.Context))
			{
				GamePlayCameraPos = GameplayCamera.Position;
				GamePlayCameraRot = GameplayCamera.Rotation;
				PlaceBombCam = GlobalHelper.CreateCamera(GamePlayCameraPos, GamePlayCameraRot, GameplayCamera.FieldOfView);
				PlaceBombCam.IsActive = true;
				PlaceBombCam.FarClip = 2000f;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				World.RenderingCamera = PlaceBombCam;
				Notification.Show("Create Cam");
			}
		}

		public void ApplyUnionDepositoryOutfits()
		{
			if (WaringBugstarsOutfit)
			{
				if (RobberA != null)
				{
					if (RobberA.Model == PedHash.FreemodeFemale01)
					{
						Random random = new Random();
					}
					if (RobberA.Model == PedHash.FreemodeMale01)
					{
						Random random2 = new Random();
					}
				}
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					Ped character = Game.Player.Character;
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 15, 0, false);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 4, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 1, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 1, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 8, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					Ped character2 = Game.Player.Character;
					Function.Call(Hash.SET_PED_PROP_INDEX, character2, 0, 6, 0, false);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 3, 12, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 4, 11, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 5, 6, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 6, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 8, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 10, 4, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 11, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 12, 0, 0, 1);
				}
			}
			if (!WaringGruppe6Outfit)
			{
				return;
			}
			if (RobberA != null)
			{
				if (RobberA.Model == PedHash.FreemodeFemale01)
				{
					Random random3 = new Random();
				}
				if (RobberA.Model == PedHash.FreemodeMale01)
				{
					Random random4 = new Random();
				}
			}
			if (Game.Player.Character.Model == PedHash.Franklin)
			{
				Ped character3 = Game.Player.Character;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 3, 8, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 4, 8, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 6, 8, 0, 1);
			}
			if (Game.Player.Character.Model == PedHash.Trevor)
			{
				Ped character4 = Game.Player.Character;
				Function.Call(Hash.SET_PED_PROP_INDEX, character4, 0, 6, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 3, 8, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 4, 14, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 12, 0, 0, 1);
			}
			if (Game.Player.Character.Model == PedHash.Michael)
			{
				Ped character5 = Game.Player.Character;
				Function.Call(Hash.SET_PED_PROP_INDEX, character5, 0, 6, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 3, 12, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 4, 11, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 5, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 6, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 10, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 12, 0, 0, 1);
			}
		}

		public void PlaceBombOnWall()
		{
			Vector3 wallPos = GetWallPos();
			if (Game.IsControlJustPressed(Control.VehicleDuck))
			{
				WaitingForBombExplosion = false;
				LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
				if (BombBagProp != null)
				{
					BombBagProp.Delete();
				}
				BombProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_ld_bomb_01a"), wallPos, new Vector3(0f, 0f, Game.Player.Character.Heading), false, false);
				BombBagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.6f, 2f)), false, false);
				BombProp.Position = BombProp.GetOffsetPosition(new Vector3(0f, 0.1f, 0f));
				Vector3 offsetPosition = BombProp.GetOffsetPosition(new Vector3(0f, -0.1f, -0.25f));
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z + 0.75f, 0, 0, Game.Player.Character.Heading, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BombProp, Scene1, "enter_bomb", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BombBagProp, Scene1, "enter_bag", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig11_bomb_plant@male@", "enter", 0.0, 0.0, 0, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Game.Player.CanControlCharacter = true;
				GamePlayCameraPos = GameplayCamera.Position;
				GamePlayCameraRot = GameplayCamera.Rotation;
				PlaceBombCam = GlobalHelper.CreateCamera(GamePlayCameraPos, GamePlayCameraRot, GameplayCamera.FieldOfView);
				PlaceBombCam.IsActive = true;
				PlaceBombCam.FarClip = 2000f;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				World.RenderingCamera = PlaceBombCam;
				PlayingBombPlacing = true;
				BombExplodeDelay = Game.GameTime + 10000;
			}
		}

		public void PlaceBombOnWall(ObjectPlacementMap N)
		{
			if (Game.IsControlJustPressed(Control.VehicleDuck))
			{
				Game.Player.Character.Position = new Vector3(N.Position.X, N.Position.Y, N.Position.Z - 1f);
				Game.Player.Character.Rotation = N.Rotation;
				Script.Wait(1000);
				Vector3 wallPos = GetWallPos();
				WaitingForBombExplosion = false;
				LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
				if (BombBagProp != null)
				{
					BombBagProp.Delete();
				}
				BombProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_ld_bomb_01a"), wallPos, new Vector3(0f, 0f, Game.Player.Character.Heading), false, false);
				BombBagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.6f, 2f)), false, false);
				BombProp.Position = BombProp.GetOffsetPosition(new Vector3(0f, 0.1f, 0f));
				Vector3 offsetPosition = BombProp.GetOffsetPosition(new Vector3(0f, -0.1f, -0.25f));
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z + 0.75f, 0, 0, Game.Player.Character.Heading, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BombProp, Scene1, "enter_bomb", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BombBagProp, Scene1, "enter_bag", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig11_bomb_plant@male@", "enter", 0.0, 0.0, 0, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Game.Player.CanControlCharacter = true;
				GamePlayCameraPos = GameplayCamera.Position;
				GamePlayCameraRot = GameplayCamera.Rotation;
				PlaceBombCam = GlobalHelper.CreateCamera(GamePlayCameraPos, GamePlayCameraRot, GameplayCamera.FieldOfView);
				PlaceBombCam.IsActive = true;
				PlaceBombCam.FarClip = 2000f;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				World.RenderingCamera = PlaceBombCam;
				PlayingBombPlacing = true;
				BombExplodeDelay = Game.GameTime + 10000;
			}
		}

		private void PlayRoute(int TASK, Ped AI)
		{
			TaskSequence taskSequence = new TaskSequence();
			Random random = new Random();
			AI.Task.PerformSequence(taskSequence);
			taskSequence.Close();
		}

		public static void DeleteBikerClubhouseAssets()
		{
			ManageRobberyAnims.DeleteLoot();
			foreach (KeypadClass bikerClubhouseKeyPad in BikerClubhouseKeyPads)
			{
				if (bikerClubhouseKeyPad.Keypad != null)
				{
					bikerClubhouseKeyPad.Keypad.Delete();
				}
				bikerClubhouseKeyPad.SpawnedKeypad = false;
			}
			foreach (Ped ped in Peds)
			{
				if (ped != null)
				{
					ped.Delete();
				}
			}
			Peds.Clear();
			if (Missionblip != null)
			{
				Missionblip.Delete();
			}
			foreach (Prop camera in Cameras)
			{
				if (camera != null)
				{
					camera.Delete();
				}
			}
			Cameras.Clear();
			foreach (Prop prop in Props)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			Props.Clear();
			foreach (Vehicle vehicle in Vehicles)
			{
				if (vehicle != null)
				{
					vehicle.Delete();
				}
			}
			Vehicles.Clear();
			foreach (Vehicle vehicle2 in Vehicles)
			{
				if (vehicle2 != null)
				{
					vehicle2.Delete();
				}
			}
			if (Vehicles.Count > 0)
			{
				Vehicles.Clear();
			}
			foreach (Ped ped2 in Peds)
			{
				if (ped2 != null)
				{
					ped2.Delete();
				}
			}
			if (Peds.Count > 0)
			{
				Peds.Clear();
			}
			if (Missionblip != null)
			{
				Missionblip.Delete();
			}
			foreach (Prop item in PaintingHolder)
			{
				if (item != null)
				{
					item.Delete();
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
			foreach (Prop bikeClubhouseHeistProp in BikeClubhouseHeistProps)
			{
				if (bikeClubhouseHeistProp != null)
				{
					bikeClubhouseHeistProp.Delete();
				}
			}
			if (Missionblip != null)
			{
				Missionblip.Delete();
			}
		}

		public static void StartBikerClubhouseHeist()
		{
			ManageRobberyAnims.CurrentPay = 0;
			HeistMinPay = BikerClubhouse_HeistMinPay;
			HeistMaxPay = BikerClubhouse_HeistMaxPay;
			ManageRobberyAnims.Heist_Average_Pay = (int)((float)(HeistMinPay + HeistMaxPay) / 2f * 1.25f);
			BikerClubhouseVaultOpenedAmt = 0f;
			PlayeringThirmiteDoor1 = false;
			DefendingCops = false;
			ThirmiteDoor1 = false;
			BikerClubhouseVaultOpen = false;
			Prop[] nearbyProps = World.GetNearbyProps(new Vector3(1115.152f, -3145.152f, -37f), 18f);
			Prop[] array = nearbyProps;
			foreach (Prop prop in array)
			{
				if (prop != null)
				{
					prop.Delete();
				}
			}
			RobberACanLoot = true;
			RobberBCanLoot = true;
			RobberCCanLoot = true;
			DeleteBikerClubhouseAssets();
			AddCrates_ClubhouseHeist();
			PlayingBikerClubhouseHeist = true;
		}

		public static void AddCrates_ClubhouseHeist()
		{
			BankHeistStage = 0;
			if (Missionblip != null)
			{
				Missionblip.Delete();
			}
			Missionblip = GlobalHelper.CreateBlip(BikerclubhouseEnterance);
			Missionblip.Sprite = BlipSprite.Standard;
			Missionblip.Color = BlipColor.Yellow;
			foreach (Vector4 bikerClubhouseEnemySpawn in BikerClubhouseEnemySpawns)
			{
				List<object> list = new List<object>();
				list.Add(PedHash.Lost01GFY);
				list.Add(PedHash.Lost01GMY);
				list.Add(PedHash.Lost02GMY);
				list.Add(PedHash.Lost03GMY);
				list.Add("mp_g_m_pros_01");
				Random random = new Random();
				Vector3 vector = new Vector3(bikerClubhouseEnemySpawn.X, bikerClubhouseEnemySpawn.Y, bikerClubhouseEnemySpawn.Z);
				Ped ped = GlobalHelper.CreatePed(Class1.RequestModel((dynamic)list[random.Next(0, list.Count)]), vector, bikerClubhouseEnemySpawn.H);
				SmartGuards.GuardGroup[14].GroupList.Add(new SmartGuards.SmartAiV2(ped, SmartGuards.EnemyGroup.Lost_MC));
			}
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel("GBURRITO"), new Vector3(1099.538f, -3145.34f, -38.51f), 180f);
			Vehicles.Add(vehicle);
			vehicle.LockStatus = VehicleLockStatus.CannotEnter;
			vehicle = GlobalHelper.CreateVehicle(RequestModel("GBURRITO"), new Vector3(1103.405f, -3145.52f, -38.51f), 180f);
			Vehicles.Add(vehicle);
			vehicle.LockStatus = VehicleLockStatus.CannotEnter;
			vehicle = GlobalHelper.CreateVehicle(RequestModel("GBURRITO"), new Vector3(1107.87f, -3157.95f, -38.51f), 346f);
			Vehicles.Add(vehicle);
			vehicle.LockStatus = VehicleLockStatus.CannotEnter;
			foreach (Vector4 bikerClubhouseVanSpawn in BikerClubhouseVanSpawns)
			{
				vehicle = GlobalHelper.CreateVehicle(position: new Vector3(bikerClubhouseVanSpawn.X, bikerClubhouseVanSpawn.Y, bikerClubhouseVanSpawn.Z), model: RequestModel("GBURRITO"), heading: bikerClubhouseVanSpawn.H);
				Vehicles.Add(vehicle);
				vehicle.LockStatus = VehicleLockStatus.CannotEnter;
			}
			foreach (Vector4 bikerClubhouseBikeSpawn in BikerClubhouseBikeSpawns)
			{
				List<object> list2 = new List<object>();
				list2.Add(VehicleHash.Hexer);
				list2.Add(VehicleHash.Daemon);
				list2.Add(VehicleHash.Wolfsbane);
				list2.Add(VehicleHash.RatBike);
				list2.Add(VehicleHash.Gargoyle);
				list2.Add(VehicleHash.ZombieA);
				list2.Add(VehicleHash.ZombieB);
				list2.Add(VehicleHash.Avarus);
				Random random2 = new Random();
				Vector3 vector2 = new Vector3(bikerClubhouseBikeSpawn.X, bikerClubhouseBikeSpawn.Y, bikerClubhouseBikeSpawn.Z);
				vehicle = GlobalHelper.CreateVehicle(Class1.RequestModel((dynamic)list2[random2.Next(0, list2.Count)]), vector2, bikerClubhouseBikeSpawn.H);
				Vehicles.Add(vehicle);
				vehicle.LockStatus = VehicleLockStatus.CannotEnter;
			}
			foreach (ObjectPlacementMap bikerClubhousePlacement in BikerClubhousePlacements)
			{
				Random random3 = new Random();
				Prop prop = GlobalHelper.CreateProp(RequestModel(bikerClubhousePlacement.Hash), bikerClubhousePlacement.Position, bikerClubhousePlacement.Rotation, false, false);
				BikeClubhouseHeistProps.Add(prop);
				prop.IsPositionFrozen = true;
				if (prop.Model == RequestModel("apa_prop_ss1_mpint_garage2"))
				{
					BikerClubhouseGrateDoor = prop;
				}
				if (prop.Model == RequestModel("prop_ld_vault_door"))
				{
					BikerClubhouseVaultDoor = prop;
				}
				if (prop.Model == RequestModel("bkr_prop_weed_table_01b"))
				{
					int num = random3.Next(0, 300);
					if (num < 100)
					{
						Prop o = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_cash_stack_01a"), prop.GetOffsetPosition(new Vector3(0f, 0f, 0.85f)), new Vector3(0f, 0f, prop.Heading), false, false);
						ManageRobberyAnims.AddPropAsLoot(o);
					}
					if (num >= 100 && num < 200)
					{
						Prop o2 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_weed_stack_01a"), prop.GetOffsetPosition(new Vector3(0f, 0f, 0.85f)), new Vector3(0f, 0f, prop.Heading), false, false);
						ManageRobberyAnims.AddPropAsLoot(o2);
					}
					if (num >= 200 && num < 300)
					{
						Prop o3 = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_coke_stack_01a"), prop.GetOffsetPosition(new Vector3(0f, 0f, 0.85f)), new Vector3(0f, 0f, prop.Heading), false, false);
						ManageRobberyAnims.AddPropAsLoot(o3);
					}
					Script.Wait(1);
				}
				if (prop.Model == RequestModel("ch_prop_ch_cash_trolly_01c"))
				{
					Prop prop2 = prop;
					prop2.Rotation = new Vector3(prop2.Rotation.X, prop2.Rotation.Y, prop2.Rotation.Z - 180f);
					ManageRobberyAnims.AddPropAsLoot(prop2);
					Script.Wait(1);
				}
				if (prop.Model == RequestModel("h4_prop_h4_weed_stack_01a"))
				{
					Prop o4 = prop;
					ManageRobberyAnims.AddPropAsLoot(o4);
					Script.Wait(1);
				}
				if (prop.Model == RequestModel("h4_prop_h4_cash_stack_01a"))
				{
					Prop o5 = prop;
					ManageRobberyAnims.AddPropAsLoot(o5);
					Script.Wait(1);
				}
			}
			CurrentHeist = HeistIn.BikerClubhouse;
		}

		public Class1()
		{
			base.Tick += OnTick;
			SetupActivities();
		}

		public static void SetupActivities()
		{
			foreach (Activity activity in Activities)
			{
				if (activity.Blip != null)
				{
					activity.Blip.Delete();
					activity.Blip = null;
				}
				if (!(activity.Blip == null))
				{
					continue;
				}
				if (activity.NotReady)
				{
					if (!Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
					{
						break;
					}
					activity.NotReady = false;
				}
				activity.Blip = GlobalHelper.CreateBlip(new Vector3(activity.Location.X, activity.Location.Y, activity.Location.Z));
				if (Heist_Blip_Type == 0)
				{
					activity.Blip.Sprite = BlipSprite.CashPickup;
					activity.Blip.Scale = 1.5f;
				}
				if (Heist_Blip_Type == 1)
				{
					activity.Blip.Sprite = BlipSprite.Heist;
					activity.Blip.Scale = 0.8f;
				}
				if (Heist_Blip_Type == 2)
				{
					activity.Blip.Sprite = BlipSprite.DollarBill;
					activity.Blip.Scale = 0.8f;
				}
				if (Heist_Blip_Type == 3)
				{
					activity.Blip.Sprite = BlipSprite.Equipment;
					activity.Blip.Scale = 0.8f;
				}
				activity.Blip.Name = "Single Player Heists : " + activity.Name;
				activity.Blip.IsShortRange = true;
				Color goldenrod = Color.Goldenrod;
				Function.Call(Hash.SET_BLIP_COLOUR, activity.Blip.Handle, (goldenrod.R << 24) + (goldenrod.G << 16) + (goldenrod.B << 8) + 255);
				if (activity.NotReady || Main.ArcadeOwned == 0)
				{
					activity.Blip.Alpha = 0;
				}
				if (Disable_HeistIcon_Blips)
				{
					activity.Blip.Alpha = 0;
				}
			}
		}

		public void RunScaleformFunction(int Handle, string name, params object[] args)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, name);
			foreach (object obj in args)
			{
				if (obj.GetType() == typeof(int))
				{
					Function.Call<int>(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)obj);
				}
				else if (obj.GetType() == typeof(float))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)obj);
				}
				else if (obj.GetType() == typeof(double))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)(double)obj);
				}
				else if (obj.GetType() == typeof(bool))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)obj);
				}
				else if (obj.GetType() == typeof(string))
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (string)obj);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
				else if (obj.GetType() == typeof(char))
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ((char)obj).ToString());
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public void DeleteKeypads()
		{
			foreach (KeypadClass unionKeyPad in UnionKeyPads)
			{
				if (unionKeyPad != null && unionKeyPad.Keypad != null && unionKeyPad.Keypad.AttachedBlip != null)
				{
					unionKeyPad.Keypad.AttachedBlip.Delete();
				}
			}
		}

		public unsafe void OnTick(object sender, EventArgs e)
		{
			if (ConvStarted && !Function.Call<bool>(Hash.IS_SCRIPTED_CONVERSATION_ONGOING))
			{
				ConvStarted = false;
				if (ConvPed != null)
				{
					ConvPed.Delete();
				}
			}
			if (RunBikerClubhouseHeist == 1)
			{
				StartBikerClubhouseHeist();
				RunBikerClubhouseHeist = 0;
			}
			if (RunFleeca == 1)
			{
				CurrentFleecaBank = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 600);
				StartFleeca(CurrentFleecaBank);
				RunFleeca = 0;
			}
			if (RunLaFauenteBlanca == 1)
			{
				StartInLaFuetaBlancaHeist();
				RunLaFauenteBlanca = 0;
			}
			if (RunJewelryStore == 1)
			{
				StartJewelryStore();
				RunJewelryStore = 0;
			}
			if (RunPacificStandard == 1)
			{
				StartPacificStandard();
				RunPacificStandard = 0;
			}
			if (RunPaleto == 1)
			{
				StartPaleto();
				RunPaleto = 0;
			}
			if (RunUnionDepositoryHeist == 1)
			{
				StartUnionDepositoryHeist();
				RunUnionDepositoryHeist = 0;
			}
			if (hackingScalwform != 0)
			{
				FingerprintHack.GUIOn = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				if (MoveTimer < Game.GameTime)
				{
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 32) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 172))
					{
						MoveTimer = Game.GameTime + 200;
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 8);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 33) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 173))
					{
						MoveTimer = Game.GameTime + 200;
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 9);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174))
					{
						MoveTimer = Game.GameTime + 200;
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 10);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175))
					{
						MoveTimer = Game.GameTime + 200;
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 11);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
				}
				if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237) || Game.IsControlJustPressed(Control.FrontendAccept))
				{
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT_SELECT");
					uLocal_70 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE, hackingScalwform);
				}
				else if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237) || Game.IsControlJustPressed(Control.FrontendAccept))
				{
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT_SELECT");
					uLocal_70 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE, hackingScalwform);
				}
				if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, uLocal_70))
				{
					int num = (FeedBackNumber = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, uLocal_70));
				}
				switch (Hacking_Index)
				{
				case 0:
					Anim_Timer = Game.GameTime + 3000;
					Hacking_Index = 10;
					break;
				case 10:
					if (Anim_Timer < Game.GameTime)
					{
						Hacking_Index = 50;
						DrawHackScaleform = true;
					}
					break;
				case 50:
				{
					if (Function.Call<bool>(Hash.GET_IS_WIDESCREEN))
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "ADD_PROGRAM", 7f, 4f, -1f, -1f, -1f, "MyComputer", 0, 0, 0, 0);
					}
					else
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "ADD_PROGRAM", 8f, 4f, -1f, -1f, -1f, "MyComputer", 0, 0, 0, 0);
					}
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_BACKGROUND", 4f, -1082130432, -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					string text = "GR_PWD_RA";
					Random random = new Random();
					int num2 = random.Next(0, 14);
					if (num2 == 0)
					{
						text = "GR_PWD_RA";
					}
					if (num2 == 1)
					{
						text = "GR_PWD_PA";
					}
					if (num2 == 2)
					{
						text = "GR_PWD_PE";
					}
					if (num2 == 3)
					{
						text = "GR_PWD_GE";
					}
					if (num2 == 4)
					{
						text = "GR_PWD_CH";
					}
					if (num2 == 5)
					{
						text = "GR_PWD_FI";
					}
					if (num2 == 6)
					{
						text = "GR_PWD_PR";
					}
					if (num2 == 7)
					{
						text = "GR_PWD_UN";
					}
					if (num2 == 8)
					{
						text = "GR_PWD_DE";
					}
					if (num2 == 9)
					{
						text = "GR_PWD_FO";
					}
					if (num2 == 10)
					{
						text = "GR_PWD_AR";
					}
					if (num2 == 11)
					{
						text = "GR_PWD_TO";
					}
					if (num2 == 12)
					{
						text = "GR_PWD_IN";
					}
					if (num2 == 13)
					{
						text = "GR_PWD_PRI";
					}
					if (num2 == 14)
					{
						text = "GR_PWD_LA";
					}
					RunScaleformFunction(hackingScalwform, "SET_ROULETTE_WORD", text);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_LABELS");
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_1");
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_2");
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_3");
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_4");
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_5");
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_6");
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_SPEED", 10f, -1082130432, -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 0f, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 30f, 50f), -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 1f, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 30f, 50f), -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 2f, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 30f, 50f), -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 3f, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 30f, 50f), -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 4f, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 30f, 50f), -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 5f, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 30f, 50f), -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 6f, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 30f, 50f), -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 7f, Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 30f, 50f), -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 2f, -1082130432, -1082130432, -1082130432, -1082130432);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, 59f, 0f, -1082130432, -1082130432);
					if (PlayBoth)
					{
						Hacking_Index = 100;
					}
					if (!PlayBoth)
					{
						Hacking_Index = 500;
					}
					break;
				}
				case 100:
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 32) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 172))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 8);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 33) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 173))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 9);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 10);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 11);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (FeedBackNumber == 82)
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, 60f, 0f, -1082130432, -1082130432);
						MyHackingTimer = new VariableTimer(60000);
						MyHackingTimer.Start();
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
						IPSound = GlobalHelper.CreateSoundID();
						Function.Call(Hash.PLAY_SOUND_FRONTEND, IPSound, "HACKING_COUNTDOWN_IP_FIND", 0, true);
						FeedBackNumber = -1;
						Hacking_Index = 200;
					}
					if (FeedBackNumber == 83 && !TRYDL)
					{
						ErrorTimer = Game.GameTime + 500;
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 1f, -1f, -1f, -1f, -1f, "TRYHACK", 0, 0, 0, 0);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
						TRYDL = true;
						FeedBackNumber = -1;
					}
					if (FeedBackNumber == 80 && !TRYDL)
					{
						ErrorTimer = Game.GameTime + 500;
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 1f, -1f, -1f, -1f, -1f, "ACCESSD", 0, 0, 0, 0);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
						TRYDL = true;
						FeedBackNumber = -1;
					}
					if (ErrorTimer < Game.GameTime && TRYDL)
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 0f, -1f, -1f, -1f, -1f, "ACCESSD", 0, 0, 0, 0);
						TRYDL = false;
					}
					if (FeedBackNumber == 4)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
						FeedBackNumber = -1;
					}
					if (FeedBackNumber == 81)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
						FeedBackNumber = -1;
					}
					break;
				case 200:
				{
					if (FeedBackNumber == 85 && !IpTry)
					{
						IpTryTimer = Game.GameTime + 500;
						if (Hacking_Lives > 0f)
						{
							Hacking_Lives -= 1f;
						}
						if (Hacking_Lives > 0f)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
						}
						if (Hacking_Lives == 0f)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_IP_OUTCOME", 1f, -1f, -1f, -1f, -1f, "LOSTIP", 0, 0, 0, 0);
							IpTryFailedTimer = Game.GameTime + 1000;
							Hacking_Index = 300;
							if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
							{
								Function.Call(Hash.STOP_SOUND, IPSound);
								IPSound = -1;
							}
						}
						IpTry = true;
						FeedBackNumber = -1;
					}
					if (FeedBackNumber == 84)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_IP_OUTCOME", 1f, -1f, -1f, -1f, -1f, "WINIP", 0, 0, 0, 0);
						if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
						{
							Function.Call(Hash.STOP_SOUND, IPSound);
							IPSound = -1;
						}
						Hacking_Index = 400;
					}
					if (IpTryTimer < Game.GameTime && IpTry)
					{
						IpTry = false;
					}
					if (MoveSoundTimer < Game.GameTime)
					{
						MoveSoundTimer = Game.GameTime + 200;
						if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 32) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 172)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
						}
						if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 33) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 173)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
						}
						if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
						}
						if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
						}
					}
					MyHackingTimer.Update(1f);
					TimeSpan timeSpan2 = TimeSpan.FromMilliseconds(MyHackingTimer.Counter);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, (float)timeSpan2.Seconds, (float)timeSpan2.Milliseconds, -1082130432, -1082130432);
					if ((float)timeSpan2.Seconds < 1f && (float)timeSpan2.Milliseconds < 1f)
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_IP_OUTCOME", 1f, -1f, -1f, -1f, -1f, "LOSTIP", 0, 0, 0, 0);
						IpTryFailedTimer = Game.GameTime + 1000;
						Hacking_Index = 300;
						if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
						{
							Function.Call(Hash.STOP_SOUND, IPSound);
							IPSound = -1;
						}
					}
					break;
				}
				case 300:
				{
					List<object> list = new List<object>();
					List<object> list4 = list;
					foreach (Control value in Enum.GetValues(typeof(Control)))
					{
						if (Game.IsControlJustPressed(value))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 2f, -1082130432, -1082130432, -1082130432, -1082130432);
							Hacking_Index = 100;
							Hacking_Lives = 3f;
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
						}
					}
					break;
				}
				case 400:
				{
					List<object> list = new List<object>();
					List<object> list5 = list;
					foreach (Control value2 in Enum.GetValues(typeof(Control)))
					{
						if (Game.IsControlJustPressed(value2))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 2f, -1082130432, -1082130432, -1082130432, -1082130432);
							Hacking_Index = 500;
							Hacking_Lives = 3f;
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
						}
					}
					break;
				}
				case 500:
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 32) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 172))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 8);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 33) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 173))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 9);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 10);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 11);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					}
					if (FeedBackNumber == 83)
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 1f, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, 60f, 0f, -1082130432, -1082130432);
						MyHackingTimer = new VariableTimer(60000);
						MyHackingTimer.Start();
						IPSound = GlobalHelper.CreateSoundID();
						Function.Call(Hash.PLAY_SOUND_FRONTEND, IPSound, "HACKING_COUNTDOWN_IP_FIND", 0, true);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
						FeedBackNumber = -1;
						Hacking_Index = 600;
						Hacking_Lives = 3f;
					}
					if (FeedBackNumber == 82 && !TRYDL)
					{
						ErrorTimer = Game.GameTime + 500;
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 1f, -1f, -1f, -1f, -1f, "TRYBRUTE", 0, 0, 0, 0);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
						TRYDL = true;
						FeedBackNumber = -1;
					}
					if (FeedBackNumber == 80 && !TRYDL)
					{
						ErrorTimer = Game.GameTime + 500;
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 1f, -1f, -1f, -1f, -1f, "ACCESSD", 0, 0, 0, 0);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
						TRYDL = true;
						FeedBackNumber = -1;
					}
					if (ErrorTimer < Game.GameTime && TRYDL)
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 0f, -1f, -1f, -1f, -1f, "ACCESSD", 0, 0, 0, 0);
						TRYDL = false;
					}
					if (FeedBackNumber == 4)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
						FeedBackNumber = -1;
					}
					if (FeedBackNumber == 81)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
						FeedBackNumber = -1;
					}
					break;
				case 600:
				{
					if (FeedBackNumber == 87 && !IpTry)
					{
						if (Hacking_Lives > 0f)
						{
							Hacking_Lives -= 1f;
						}
						if (Hacking_Lives > 0f)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "RESET_ROULETTE");
						}
						if (Hacking_Lives == 0f)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_ROULETTE_OUTCOME", 1f, -1f, -1f, -1f, -1f, "LOSEBRUTE", 0, 0, 0, 0);
							IpTryFailedTimer = Game.GameTime + 1000;
							Hacking_Index = 700;
							if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
							{
								Function.Call(Hash.STOP_SOUND, IPSound);
								IPSound = -1;
							}
						}
						FeedBackNumber = -1;
					}
					if (FeedBackNumber == 86)
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_ROULETTE_OUTCOME", 1f, -1f, -1f, -1f, -1f, "WINBRUTE", 0, 0, 0, 0);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_GR_Steal_Railguns_Sounds", true);
						Hacking_Index = 800;
						FeedBackNumber = -1;
					}
					if (FeedBackNumber == 92)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
						FeedBackNumber = -1;
					}
					if (MoveSoundTimer < Game.GameTime)
					{
						MoveSoundTimer = Game.GameTime + 200;
						if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
						}
						if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
						}
					}
					MyHackingTimer.Update(1f);
					TimeSpan timeSpan = TimeSpan.FromMilliseconds(MyHackingTimer.Counter);
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, (float)timeSpan.Seconds, (float)timeSpan.Milliseconds, -1082130432, -1082130432);
					if ((float)timeSpan.Seconds < 1f && (float)timeSpan.Milliseconds < 1f)
					{
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_ROULETTE_OUTCOME", 1f, -1f, -1f, -1f, -1f, "LOSEBRUTE", 0, 0, 0, 0);
						IpTryFailedTimer = Game.GameTime + 1000;
						Hacking_Index = 700;
						if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
						{
							Function.Call(Hash.STOP_SOUND, IPSound);
							IPSound = -1;
						}
					}
					break;
				}
				case 700:
				{
					List<object> list = new List<object>();
					List<object> list3 = list;
					foreach (Control value3 in Enum.GetValues(typeof(Control)))
					{
						if (Game.IsControlJustPressed(value3))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 1f, -1082130432, -1082130432, -1082130432, -1082130432);
							Hacking_Index = 600;
							Hacking_Lives = 3f;
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
						}
					}
					break;
				}
				case 800:
				{
					List<object> list = new List<object>();
					List<object> list2 = list;
					foreach (Control value4 in Enum.GetValues(typeof(Control)))
					{
						if (Game.IsControlJustPressed(value4))
						{
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "unlocked_bleep", Game.Player.Character, "HACKING_DOOR_UNLOCK_SOUNDS", 0, 0);
							HackingSuccess = true;
							ActiveHack = true;
							HackDone = true;
							UnloadLoadehacker();
						}
					}
					break;
				}
				}
				if (DrawHackScaleform)
				{
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
					Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, hackingScalwform, 255, 255, 255, 255, 0);
					if (Hacking_Index == 100)
					{
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
						DisplayHelpTextThisFrame("to start the hack, select 'My Computer', 'External Device', HackConnect.exe\nPress ~INPUT_CONTEXT~ to Exit", 1);
					}
					if (Hacking_Index == 200)
					{
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
						DisplayHelpTextThisFrame("Select the correct IP from the changing list of numbers, ~INPUT_MOVE_UP_ONLY~ ~INPUT_MOVE_LEFT_ONLY~ ~INPUT_MOVE_DOWN_ONLY~ ~INPUT_MOVE_RIGHT_ONLY~ Move IP selection\nPress ~INPUT_CONTEXT~ to Exit", 1);
					}
					if (Hacking_Index == 400)
					{
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
						DisplayHelpTextThisFrame("to start/continue the hack, select 'My Computer', 'External Device', Bruteforce.exe\nPress ~INPUT_CONTEXT~ to Exit", 1);
					}
					if (Hacking_Index == 500)
					{
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
						DisplayHelpTextThisFrame("to start/continue the hack, select 'My Computer', 'External Device', Bruteforce.exe\nPress ~INPUT_CONTEXT~ to Exit", 1);
					}
					if (Hacking_Index == 600)
					{
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
						DisplayHelpTextThisFrame("Correctly freeze each column to reveal the password, use ~INPUT_MOVE_LEFT_ONLY~ ~INPUT_MOVE_RIGHT_ONLY~ to to change columns and press ~INPUT_FRONTEND_ACCEPT~ to select current colum.", 1);
					}
				}
			}
			if (Game.GameTime >= ChecknearActivity)
			{
				ChecknearActivity = Game.GameTime + 2000;
				foreach (Activity activity in Activities)
				{
					Vector3 location = activity.Location;
					if (GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, ">", 10f) && GetDistance_VDIST2(CurrentActivity, Game.Player.Character.Position, ">", 10f))
					{
						nearActivity = false;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, activity.Location, "<", 60f))
					{
						CurrentActivity = activity.Location;
						nearActivity = true;
					}
				}
			}
			if (The247Job._247RobberyOn || TheMazebankSavingsBankHeistcs.TheMazeBankSavngsBankHeistOn || ThePrinthouseHeist.ThePrinthouseHeistOn || TheBetaBankHeist.TheBetaBankHeistOn || TheDiamondJewelStore.TheDiamondJewelStoreHeistOn || TheGefangnisArtHeist.TheGefangnisArtHeistOn || (Main.ArcadeOwned == 1 && !nearActivity && !PlayingFleeca && !PlayingPaleto && !PlayingPacificStandard && !JewelStoreRobberyOn && !PlayingUnion && !InLaFuetaBlancaHeist && !PlayingBikerClubhouseHeist))
			{
				return;
			}
			if (SPH_Debug.ShowRunningClassDebug)
			{
				drawText("Class1 Running ", 0.8f, 0.06f, 0.5f, 255, 255, 255);
			}
			if (Main.ArcadeOwned == 1 && InHouseRobbery)
			{
				if (!PlayingUnion)
				{
					RobberACanLoot = true;
					RobberBCanLoot = true;
					RobberCCanLoot = true;
				}
				if (RobberA != null)
				{
					RobberA.BlockPermanentEvents = true;
				}
				if (RobberB != null)
				{
					RobberB.BlockPermanentEvents = true;
				}
				if (RobberC != null)
				{
					RobberC.BlockPermanentEvents = true;
				}
				float num169 = (float)Game.GameTime;
				float nextTick2 = nextTick;
				if (!PlayingUnion)
				{
					UI.RenderQue = 0f;
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
					UI.DrawPayment("TAKE", LocalTake);
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-781.74f, 323.39f, 222.5f), "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(-781.74f, 323.39f, 222.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), ActivityMarkerColor);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-781.74f, 323.39f, 222.5f), "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Exit House Robbery");
						if (Game.IsControlJustPressed(Control.Context))
						{
							foreach (Ped ped24 in Peds)
							{
								if (ped24 != null)
								{
									ped24.Delete();
								}
							}
							foreach (Vehicle vehicle3 in Vehicles)
							{
								if (vehicle3 != null)
								{
									vehicle3.Delete();
								}
							}
							EndHouseRobbery();
							if (!InLaFuetaBlancaHeist)
							{
								Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Police), new Vector3(0f, 0f, 0f));
								vehicle.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.Cop01SMY));
								Function.Call(Hash.SET_PED_AS_COP, vehicle.GetPedOnSeat(VehicleSeat.Driver));
								vehicle.GetPedOnSeat(VehicleSeat.Driver).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								vehicle.CreatePedOnSeat(VehicleSeat.Passenger, RequestModel(PedHash.Cop01SFY));
								Function.Call(Hash.SET_PED_AS_COP, vehicle.GetPedOnSeat(VehicleSeat.Passenger));
								vehicle.GetPedOnSeat(VehicleSeat.Passenger).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								vehicle.Position = GenerateSpawnPos(Game.Player.Character.Position, Nodetype.AnyRoad, false);
								vehicle.IsPersistent = false;
								vehicle.GetPedOnSeat(VehicleSeat.Driver).IsPersistent = false;
								vehicle.GetPedOnSeat(VehicleSeat.Passenger).IsPersistent = false;
								Peds.Add(vehicle.GetPedOnSeat(VehicleSeat.Driver));
								Peds.Add(vehicle.GetPedOnSeat(VehicleSeat.Passenger));
								Vehicles.Add(vehicle);
								Vehicle vehicle2 = vehicle;
								vehicle = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Police), new Vector3(0f, 0f, 0f));
								vehicle.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.Cop01SMY));
								Function.Call(Hash.SET_PED_AS_COP, vehicle.GetPedOnSeat(VehicleSeat.Driver));
								vehicle.GetPedOnSeat(VehicleSeat.Driver).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								vehicle.CreatePedOnSeat(VehicleSeat.Passenger, RequestModel(PedHash.Cop01SFY));
								Function.Call(Hash.SET_PED_AS_COP, vehicle.GetPedOnSeat(VehicleSeat.Passenger));
								vehicle.GetPedOnSeat(VehicleSeat.Passenger).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								vehicle.Position = vehicle2.GetOffsetPosition(new Vector3(5f, 0f, 0f));
								vehicle.IsPersistent = false;
								vehicle.GetPedOnSeat(VehicleSeat.Driver).IsPersistent = false;
								vehicle.GetPedOnSeat(VehicleSeat.Passenger).IsPersistent = false;
								Peds.Add(vehicle.GetPedOnSeat(VehicleSeat.Driver));
								Peds.Add(vehicle.GetPedOnSeat(VehicleSeat.Passenger));
								Vehicles.Add(vehicle);
								vehicle = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Police), new Vector3(0f, 0f, 0f));
								vehicle.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.Cop01SMY));
								Function.Call(Hash.SET_PED_AS_COP, vehicle.GetPedOnSeat(VehicleSeat.Driver));
								vehicle.GetPedOnSeat(VehicleSeat.Driver).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								vehicle.CreatePedOnSeat(VehicleSeat.Passenger, RequestModel(PedHash.Cop01SFY));
								Function.Call(Hash.SET_PED_AS_COP, vehicle.GetPedOnSeat(VehicleSeat.Passenger));
								vehicle.GetPedOnSeat(VehicleSeat.Passenger).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								vehicle.Position = vehicle2.GetOffsetPosition(new Vector3(-5f, 0f, 0f));
								vehicle.IsPersistent = false;
								vehicle.GetPedOnSeat(VehicleSeat.Driver).IsPersistent = false;
								vehicle.GetPedOnSeat(VehicleSeat.Passenger).IsPersistent = false;
								Peds.Add(vehicle.GetPedOnSeat(VehicleSeat.Driver));
								Peds.Add(vehicle.GetPedOnSeat(VehicleSeat.Passenger));
								Vehicles.Add(vehicle);
								Function.Call(Hash.REPORT_CRIME, Game.Player, 27, Function.Call<int>(Hash.GET_WANTED_LEVEL_THRESHOLD, 4));
								foreach (Ped ped25 in Peds)
								{
									if (ped25 != null)
									{
										ped25.Health = 300;
									}
								}
							}
						}
					}
				}
			}
			if (Main.ArcadeOwned != 1)
			{
				return;
			}
			switch (CurrentHeist)
			{
			case HeistIn.BikerClubhouse:
				UI.RenderQue = 0f;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Lost_MC;
				if (!PlayingBikerClubhouseHeist)
				{
					break;
				}
				if (BankHeistStage == 0)
				{
					GTA.UI.Screen.ShowSubtitle("Make your way to the ~y~Lost MC Clubhouse");
					if (GetDistance_VDIST2(Game.Player.Character.Position, BikerclubhouseEnterance, "<", 20f))
					{
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						BankHeistStage = 1;
					}
				}
				if (BankHeistStage == 1)
				{
					if (Game.Player.WantedLevel > 0)
					{
						GTA.UI.Screen.ShowSubtitle("Lose the ~r~Cops");
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Make your way into the ~y~Lost MC Clubhouse");
					}
				}
				if (BankHeistStage == 2)
				{
					GTA.UI.Screen.ShowSubtitle("Hack the ~b~Security Panel~w~ to get access to ~y~Lounge Area");
				}
				if (BankHeistStage == 3)
				{
					GTA.UI.Screen.ShowSubtitle("Place ~r~Thermite~w~ on the ~y~Vault");
				}
				if (BankHeistStage == 4)
				{
					if (ManageRobberyAnims.CurrentPay >= 100000)
					{
						GTA.UI.Screen.ShowSubtitle("Loot the ~g~Vault~w~ or leave the ~y~Lost MC Clubhouse~w~");
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Loot the ~g~Vault~w~");
					}
					if (ManageRobberyAnims.ValuableLootLeft <= 0)
					{
						BankHeistStage = 5;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, BikerclubhouseExit, "<", 10f))
					{
						BankHeistStage = 5;
					}
				}
				if (BankHeistStage == 5)
				{
					GTA.UI.Screen.ShowSubtitle("Leave the ~y~Lost MC Clubhouse~w~");
				}
				if (BankHeistStage == 6)
				{
					if (Game.Player.WantedLevel == 0)
					{
						GTA.UI.Screen.ShowSubtitle("Leave the Area.");
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Escape the ~r~Lost MC & Cops~w~");
					}
				}
				if (ManageRobberyAnims.CurrentPay >= 100000 && Game.Player.WantedLevel == 0 && BankHeistStage == 6 && GetDistance_VDIST2(Game.Player.Character.Position, BikerclubhouseEnterance, ">", 100f))
				{
					PlayingBikerClubhouseHeist = false;
					DeleteBikerClubhouseAssets();
					float num23 = ManageRobberyAnims.CurrentPay;
					float num24 = (num23 -= TotalCost);
					float totalCost = TotalCost;
					Script.Wait(10);
					totalCost = num24 / 10f;
					InHouseRobbery = false;
					Main.TotalCrewEarned += totalCost;
					Main.TotalPlayerEarned += num24;
					XP.CompleteHeist_BikerClubhouse++;
					Main.SaveCuts();
					SetNoLongerInHeist();
					MissionScreen.SetPass("Biker Clubhouse Heist", (int)num24, (int)totalCost, "The Player has died", 2, HTime, 5);
					int num25 = Game.GameTime + 30000;
					while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num25)
					{
						Script.Wait(0);
					}
					Script.Wait(2000);
					XP.AddXP_Percent(XP.CompleteHeist_XPPercent_BikerClubhouse);
					HeistTime = 0f;
					PlayingUnion = false;
					DeleteUnionNewAssets();
					Random random3 = new Random();
					ManagePlayerFunds.AddFunds((int)num23);
					HeadingtoArcade = false;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
					Notification.Show("~b~ Heist Complete ~w~");
					Notification.Show("~b~ Player Cut ~w~ : $" + num24.ToString("N"));
					Notification.Show("~b~ RobberA Cut ~w~ : $" + TotalCost.ToString("N"));
					MissionOn = false;
				}
				if (BankHeistStage == 5 && ManageRobberyAnims.CurrentPay >= 100000 && Game.Player.WantedLevel != 0 && GetDistance_VDIST2(Game.Player.Character.Position, BikerclubhouseEnterance, "<", 100f))
				{
					List<Vehicle> list6 = new List<Vehicle>();
					Vehicle item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Daemon), GenerateSpawnPos(Game.Player.Character.Position.Around(100f), Nodetype.AnyRoad, false), 180f);
					list6.Add(item);
					Vehicles.Add(item);
					item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Daemon), GenerateSpawnPos(Game.Player.Character.Position.Around(100f), Nodetype.AnyRoad, false), 180f);
					list6.Add(item);
					Vehicles.Add(item);
					item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Daemon), GenerateSpawnPos(Game.Player.Character.Position.Around(100f), Nodetype.AnyRoad, false), 346f);
					list6.Add(item);
					Vehicles.Add(item);
					foreach (Vehicle item7 in list6)
					{
						List<object> list7 = new List<object>();
						list7.Add(PedHash.Lost01GFY);
						list7.Add(PedHash.Lost01GMY);
						list7.Add(PedHash.Lost02GMY);
						list7.Add(PedHash.Lost03GMY);
						list7.Add("mp_g_m_pros_01");
						Random random4 = new Random();
						Vector3 position2 = item7.Position;
						Ped ped2 = GlobalHelper.CreatePed(RequestModel((dynamic)list7[random4.Next(0, list7.Count)]), position2, 0);
						ped2.SetIntoVehicle(item7, VehicleSeat.Driver);
						Peds.Add(ped2);
						SmartGuards.GuardGroup[14].GroupList.Add(new SmartGuards.SmartAiV2(ped2, SmartGuards.EnemyGroup.Lost_MC));
						int passengerCapacity = item7.PassengerCapacity;
						for (int num26 = 0; num26 < 1; num26++)
						{
							position2 = item7.Position;
							ped2 = GlobalHelper.CreatePed(RequestModel((dynamic)list7[random4.Next(0, list7.Count)]), position2, 0f);
							ped2.SetIntoVehicle(item7, (VehicleSeat)num26);
							Peds.Add(ped2);
							SmartGuards.GuardGroup[14].GroupList.Add(new SmartGuards.SmartAiV2(ped2, SmartGuards.EnemyGroup.Lost_MC));
						}
					}
					foreach (Ped ped26 in Peds)
					{
						if (ped26.IsAlive)
						{
							ped26.Task.FightAgainst(Game.Player.Character);
						}
					}
					BankHeistStage = 6;
				}
				foreach (Ped ped27 in Peds)
				{
					if (!ped27.IsAlive)
					{
						if (ped27.AttachedBlip != null)
						{
							ped27.AttachedBlip.Alpha = 0;
						}
						continue;
					}
					if ((Game.Player.IsTargeting(ped27) || GetDistance_VDIST2(Game.Player.Character.Position, ped27.Position, "<", 20f)) && !ped27.IsInCombatAgainst(Game.Player.Character))
					{
						ped27.Task.FightAgainst(Game.Player.Character);
						foreach (Ped ped28 in Peds)
						{
							if (GetDistance_VDIST2(ped28.Position, ped27.Position, "<", 10f))
							{
								ped28.Task.FightAgainst(Game.Player.Character);
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, ped27.Position, "<", 80f))
					{
						if (ped27.AttachedBlip != null)
						{
							ped27.AttachedBlip.Scale = 0.5f;
							ped27.AttachedBlip.Alpha = 255;
						}
					}
					else if (ped27.AttachedBlip != null)
					{
						ped27.AttachedBlip.Alpha = 0;
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, BikerclubhouseEnterance, "<", 100f))
				{
					if (ManageRobberyAnims.CurrentPay <= 100000 && Game.Player.WantedLevel == 0)
					{
						World.DrawMarker(MarkerType.Cylinder, BikerclubhouseEnterance, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), ActivityMarkerColor);
						if (GetDistance_VDIST2(Game.Player.Character.Position, BikerclubhouseEnterance, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Enter Clubhouse");
							if (Game.IsControlJustPressed(Control.Context))
							{
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(500);
								BankHeistStage = 2;
								Game.Player.Character.Position = BikerclubhouseExit;
								Game.Player.Character.Heading = 2f;
								Script.Wait(500);
								GTA.UI.Screen.FadeIn(500);
								if (RobberA != null)
								{
									RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									RobberA.Heading = 342f;
								}
								if (RobberB != null)
								{
									RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									RobberB.Heading = 342f;
								}
								if (RobberC != null)
								{
									RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									RobberC.Heading = 342f;
								}
							}
						}
					}
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, BikerclubhouseExit, "<", 100f))
				{
					World.DrawMarker(MarkerType.Cylinder, BikerclubhouseExit, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), ActivityMarkerColor);
					if (GetDistance_VDIST2(Game.Player.Character.Position, BikerclubhouseExit, "<", 2f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Exit Biker Clubhouse");
						if (Game.IsControlJustPressed(Control.Context))
						{
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(500);
							Game.Player.Character.Position = BikerclubhouseEnterance;
							Game.Player.Character.Heading = 301f;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
							if (RobberA != null)
							{
								RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
								RobberA.Heading = 342f;
							}
							if (RobberB != null)
							{
								RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
								RobberB.Heading = 342f;
							}
							if (RobberC != null)
							{
								RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
								RobberC.Heading = 342f;
							}
							if (ManageRobberyAnims.CurrentPay >= 100000 && Game.Player.WantedLevel == 0)
							{
								BankHeistStage = 6;
								Game.Player.WantedLevel = 4;
								List<Vehicle> list8 = new List<Vehicle>();
								Vehicle item2 = GlobalHelper.CreateVehicle(RequestModel("GBURRITO"), GenerateSpawnPos(Game.Player.Character.Position.Around(100f), Nodetype.AnyRoad, false), 180f);
								list8.Add(item2);
								Vehicles.Add(item2);
								item2 = GlobalHelper.CreateVehicle(RequestModel("GBURRITO"), GenerateSpawnPos(Game.Player.Character.Position.Around(100f), Nodetype.AnyRoad, false), 180f);
								list8.Add(item2);
								Vehicles.Add(item2);
								item2 = GlobalHelper.CreateVehicle(RequestModel("GBURRITO"), GenerateSpawnPos(Game.Player.Character.Position.Around(100f), Nodetype.AnyRoad, false), 346f);
								list8.Add(item2);
								Vehicles.Add(item2);
								foreach (Vehicle item8 in list8)
								{
									List<object> list9 = new List<object>();
									list9.Add(PedHash.Lost01GFY);
									list9.Add(PedHash.Lost01GMY);
									list9.Add(PedHash.Lost02GMY);
									list9.Add(PedHash.Lost03GMY);
									list9.Add("mp_g_m_pros_01");
									Random random5 = new Random();
									Vector3 position3 = item8.Position;
									Ped ped3 = GlobalHelper.CreatePed(RequestModel((dynamic)list9[random5.Next(0, list9.Count)]), position3, 0);
									ped3.SetIntoVehicle(item8, VehicleSeat.Driver);
									Peds.Add(ped3);
									SmartGuards.GuardGroup[14].GroupList.Add(new SmartGuards.SmartAiV2(ped3, SmartGuards.EnemyGroup.Lost_MC));
									int passengerCapacity2 = item8.PassengerCapacity;
									for (int num27 = 0; num27 < 3; num27++)
									{
										position3 = item8.Position;
										ped3 = GlobalHelper.CreatePed(RequestModel((dynamic)list9[random5.Next(0, list9.Count)]), position3, 0f);
										SmartGuards.GuardGroup[14].GroupList.Add(new SmartGuards.SmartAiV2(ped3, SmartGuards.EnemyGroup.Lost_MC));
									}
								}
								foreach (Ped ped29 in Peds)
								{
									if (ped29.IsAlive)
									{
										ped29.Task.FightAgainst(Game.Player.Character);
									}
								}
								BankHeistStage = 6;
							}
						}
					}
				}
				if (!ThirmiteDoor1)
				{
					Vector3 offsetPosition4 = BikerClubhouseVaultDoor.GetOffsetPosition(new Vector3(1f, -1f, -1f));
					if (Game.Player.Character.Position.DistanceTo(offsetPosition4) < 10f && !PlayeringThirmiteDoor1)
					{
						World.DrawMarker(MarkerType.Cylinder, offsetPosition4, Vector3.Zero, Vector3.Zero, new Vector3(0.8f, 0.8f, 0.8f), ActivityMarkerColor);
					}
					if (Game.Player.Character.Position.DistanceTo(offsetPosition4) < 2f)
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
							Vector3 offsetPosition5 = BikerClubhouseVaultDoor.GetOffsetPosition(new Vector3(1.08f, -0.2f, 0f));
							BagProp = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), offsetPosition4, new Vector3(1f, 0f, 0f), false, false);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition5.X, offsetPosition5.Y, offsetPosition5.Z, 0.0, 0.0, 358f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, Scene1, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, BagProp);
							_thermiteProp = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, _thermiteProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Function.Call(Hash.PLAY_ENTITY_ANIM, _thermiteProp, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
						}
					}
					if (PlayeringThirmiteDoor1)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1056353216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1057353216 && !DefendingCops)
						{
							BikerClubhouseVaultOpen = true;
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
							PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", _thermiteProp, 0f, 0f, 0f, 1f, 0, 0, 0);
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
							PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", _thermiteProp, 0f, 0f, 0f, 10f, 0, 0, 0);
							Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
							DefendingCops = true;
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 && !ThirmiteDoor1)
						{
							placedJammer = false;
							ThirmiteDoor1 = true;
							Game.Player.Character.Task.ClearAll();
							if (BagProp != null)
							{
								BagProp.Delete();
							}
							if (_thermiteProp != null)
							{
								_thermiteProp.Detach();
								_thermiteProp.IsPositionFrozen = true;
							}
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
							BikerClubhouseVaultIsOpening = true;
						}
					}
				}
				if (ThirmiteDoor1)
				{
					Waittime = 0;
					if (BankHeistStage == 3)
					{
						BankHeistStage = 4;
					}
					PlayeringThirmiteDoor1 = false;
					Waittime = 0;
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", _thermiteProp, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
					Prop[] nearbyProps9 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate_pris");
					Prop[] array13 = nearbyProps9;
					foreach (Prop prop14 in array13)
					{
						if (prop14 != null)
						{
							prop14.IsPositionFrozen = false;
						}
					}
					if (_thermiteProp != null)
					{
						_thermiteProp.Delete();
					}
					Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, Scene1);
				}
				if (BikerClubhouseVaultIsOpening && BikerClubhouseVaultOpenedAmt <= 90f)
				{
					BikerClubhouseVaultDoor.Rotation = new Vector3(BikerClubhouseVaultDoor.Rotation.X, BikerClubhouseVaultDoor.Rotation.Y, BikerClubhouseVaultDoor.Rotation.Z - 0.15f);
					BikerClubhouseVaultOpenedAmt += 0.15f;
				}
				if (hackedBikerClubhouseGrateDoor && BikerClubhouseGrateDoor.Position.Z < -28f)
				{
					BikerClubhouseGrateDoor.Position = new Vector3(BikerClubhouseGrateDoor.Position.X, BikerClubhouseGrateDoor.Position.Y, BikerClubhouseGrateDoor.Position.Z + 0.01f);
				}
				if (BikerClubhouseVaultOpen)
				{
					InHouseRobbery = true;
				}
				else
				{
					InHouseRobbery = false;
				}
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
				UI.DrawPayment("TAKE", LocalTake);
				{
					foreach (KeypadClass bikerClubhouseKeyPad in BikerClubhouseKeyPads)
					{
						if (!bikerClubhouseKeyPad.SpawnedKeypad && !bikerClubhouseKeyPad.SpawnedKeypad)
						{
							bikerClubhouseKeyPad.SpawnedKeypad = true;
							bikerClubhouseKeyPad.Keypad = GlobalHelper.CreateProp(RequestModel("ch_prop_fingerprint_scanner_01c"), bikerClubhouseKeyPad.KeypadPos, new Vector3(0f, 0f, bikerClubhouseKeyPad.KeypadRot), false, false);
							bikerClubhouseKeyPad.Keypad.IsPositionFrozen = true;
							bikerClubhouseKeyPad.Keypad.AddBlip();
							bikerClubhouseKeyPad.Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
							bikerClubhouseKeyPad.Keypad.AttachedBlip.Scale = 0.75f;
							bikerClubhouseKeyPad.Keypad.AttachedBlip.IsShortRange = true;
							Wait(1);
							bikerClubhouseKeyPad.Keypad.Position = bikerClubhouseKeyPad.Keypad.GetOffsetPosition(new Vector3(0f, 0.055f, 0f));
							Random random6 = new Random();
						}
						if (!bikerClubhouseKeyPad.SpawnedKeypad)
						{
							continue;
						}
						float num29 = bikerClubhouseKeyPad.KeypadPos.Z - 3f;
						float num30 = bikerClubhouseKeyPad.KeypadPos.Z + 3f;
						if (GetDistance_VDIST2(Game.Player.Character.Position, bikerClubhouseKeyPad.KeypadPos, ">", 30f))
						{
							bikerClubhouseKeyPad.Keypad.AttachedBlip.Alpha = 0;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, bikerClubhouseKeyPad.KeypadPos, "<", 30f))
						{
							if (Game.Player.Character.Position.Z > num29 && Game.Player.Character.Position.Z < num30)
							{
								if (!BrokenDisguise || (!WaringBugstarsOutfit && !WaringGruppe6Outfit))
								{
									bikerClubhouseKeyPad.Keypad.AttachedBlip.Alpha = 255;
								}
							}
							else
							{
								bikerClubhouseKeyPad.Keypad.AttachedBlip.Alpha = 0;
							}
						}
						if (!HackingKeypad)
						{
							if ((BrokenDisguise && (WaringBugstarsOutfit || WaringGruppe6Outfit || hackedUnionVaultDoor)) || !GetDistance_VDIST2(Game.Player.Character.Position, bikerClubhouseKeyPad.KeypadPos, "<", 10f))
							{
								continue;
							}
							Vector3 offsetPosition6 = bikerClubhouseKeyPad.Keypad.GetOffsetPosition(new Vector3(0f, -1f, 0f));
							if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition6, "<", 2f))
							{
								if (!HackingKeypad)
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Hack Keypad");
								}
								if (Game.IsControlJustPressed(Control.Context) && !HackingKeypad)
								{
									LastKeypad = bikerClubhouseKeyPad.Keypad;
									if (Phone != null)
									{
										Phone.Delete();
									}
									if (USB != null)
									{
										USB.Delete();
									}
									Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), bikerClubhouseKeyPad.KeypadPos, false, false);
									USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), bikerClubhouseKeyPad.KeypadPos, false, false);
									USB.IsVisible = false;
									Phone.IsVisible = false;
									USB.IsVisible = true;
									Phone.IsVisible = true;
									int index4 = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
									LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, bikerClubhouseKeyPad.Keypad.Position.X, bikerClubhouseKeyPad.Keypad.Position.Y, bikerClubhouseKeyPad.Keypad.Position.Z, bikerClubhouseKeyPad.Keypad.Rotation.X, bikerClubhouseKeyPad.Keypad.Rotation.Y, bikerClubhouseKeyPad.Keypad.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 0.0, 0.0, 0, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "action_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "action_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									if (Main.DisableHacks)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1393.91f, 1140.1f, 114.8f), "<", 2f))
										{
											hackedMadrazoFrontDoor = true;
										}
										if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 10f))
										{
											hackedUnionVaultDoor = true;
											CurrentUnionDPstage = UnionDPStage.InVault;
										}
										if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1113.719f, -3156.489f, -36.77517f), "<", 10f))
										{
											BankHeistStage = 3;
											hackedBikerClubhouseGrateDoor = true;
										}
										Script.Wait(4000);
										bikerClubhouseKeyPad.AwaitingHackGUI = false;
										LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
										Script.Wait(4000);
										Game.Player.Character.Task.ClearAllImmediately();
										if (Phone != null)
										{
											Phone.Delete();
										}
										if (USB != null)
										{
											USB.Delete();
										}
									}
									else
									{
										bikerClubhouseKeyPad.AwaitingHackGUI = true;
									}
								}
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200 && bikerClubhouseKeyPad.AwaitingHackGUI)
							{
								Scene1 = -1;
								if (bikerClubhouseKeyPad.Type == 1)
								{
									FingerprintHack.RunKeypadHack();
								}
								if (bikerClubhouseKeyPad.Type == 2)
								{
									FingerprintHack.RunFingerPrintHack();
								}
								if (bikerClubhouseKeyPad.Type == 3)
								{
									FingerprintHack.RunCopyPrintHack();
								}
								bikerClubhouseKeyPad.AwaitingHackGUI = false;
								HackingKeypad = true;
							}
						}
						else
						{
							if (!HackingKeypad)
							{
								continue;
							}
							if (Game.IsControlJustPressed(Control.Context) && HackingKeypad)
							{
								bikerClubhouseKeyPad.AwaitingHackGUI = false;
								LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							}
							if (!GetDistance_VDIST2(Game.Player.Character.Position, bikerClubhouseKeyPad.KeypadPos, "<", 5f))
							{
								continue;
							}
							if (FingerprintHack.HackFinishedID == 2)
							{
								bikerClubhouseKeyPad.AwaitingHackGUI = false;
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
								bikerClubhouseKeyPad.Keypad.AttachedBlip.Alpha = 0;
								bikerClubhouseKeyPad.AwaitingHackGUI = false;
								FingerprintHack.HackFinishedID = 0;
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1393.91f, 1140.1f, 114.8f), "<", 2f))
								{
									hackedMadrazoFrontDoor = true;
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 10f))
								{
									hackedUnionVaultDoor = true;
									CurrentUnionDPstage = UnionDPStage.InVault;
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1113.719f, -3156.489f, -36.77517f), "<", 10f))
								{
									BankHeistStage = 3;
									hackedBikerClubhouseGrateDoor = true;
								}
								LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "success_exit", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "fail_react_quick_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "fail_react_quick_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200)
							{
								HackingKeypad = false;
								bikerClubhouseKeyPad.AwaitingHackGUI = false;
								if (Phone != null)
								{
									Phone.Delete();
								}
								if (USB != null)
								{
									USB.Delete();
								}
								Scene1 = -1;
								hackedUnionVaultDoor = false;
								hackedMadrazoFrontDoor = false;
								Game.Player.Character.Task.ClearAll();
							}
						}
					}
					break;
				}
			case HeistIn.UnionDP:
			{
				UI.RenderQue = 0f;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
				if (!PlayingUnion)
				{
					break;
				}
				if (UnionDP_C4Crate != null)
				{
					GetDistance_VDIST2(Game.Player.Character.Position, UnionDP_C4Crate.Position, "<", 100f);
				}
				if (Game.GameTime >= CheckDoorsTick)
				{
					CheckDoorsTick = Game.GameTime + 2500;
					foreach (Door openDoor in OpenDoors)
					{
						if (openDoor.Location.DistanceTo(Game.Player.Character.Position) < 15f)
						{
							Vector3 position4 = Game.Player.Character.Position;
							int num31 = Function.Call<int>(Hash.GET_HASH_KEY, openDoor.Model);
							Prop prop15 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position4.X, position4.Y, position4.Z, 2f, num31, 0, 0, 0);
							if (prop15 != null)
							{
								prop15.IsPositionFrozen = true;
							}
						}
					}
				}
				PlayerNearCargoShipA = Function.Call<bool>(Hash.IS_ENTITY_IN_AREA, Game.Player.Character, -264.536f, -2386.203f, 3.808094f, -66.45306f, -2346.207f, 76.36792f, 1, 1, 0);
				if (PlayerNearCargoShipA)
				{
					HasBeenOnCargoShip = true;
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "v_fakeboatpo1sh1"), -165.34f, -2350.03f, 0, 0);
				}
				else if (PlayerNearCargoShipB)
				{
					HasBeenOnCargoShip = true;
					Function.Call(Hash.SET_RADAR_AS_INTERIOR_THIS_FRAME, Function.Call<int>(Hash.GET_HASH_KEY, "V_FakeBoatPO1SH2A"), 1240f, -2970f, 0, 0);
				}
				else if (!PlayerNearCargoShipA && !PlayerNearCargoShipB && HasBeenOnCargoShip)
				{
					Function.Call(Hash.SET_RADAR_AS_EXTERIOR_THIS_FRAME);
					HasBeenOnCargoShip = false;
				}
				if (RobberA != null)
				{
					if (PlayerNearCargoShipA || PlayerNearCargoShipB)
					{
						if (!RobberAWaitingAtCargoShip)
						{
							TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "RobberA Waiting", "While you are on the cargo ship, your RobberA will wait on the Dock for your return.");
						}
						RobberAWaitingAtCargoShip = true;
						RobberA.Task.Wait(1);
					}
					else
					{
						RobberAWaitingAtCargoShip = false;
					}
				}
				if (RobberB != null)
				{
					if (PlayerNearCargoShipA || PlayerNearCargoShipB)
					{
						if (!RobberBWaitingAtCargoShip)
						{
							TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "RobberB Waiting", "While you are on the cargo ship, your RobberB will wait on the Dock for your return.");
						}
						RobberBWaitingAtCargoShip = true;
						RobberB.Task.Wait(1);
					}
					else
					{
						RobberBWaitingAtCargoShip = false;
					}
				}
				if (RobberB != null)
				{
					if (PlayerNearCargoShipA || PlayerNearCargoShipB)
					{
						if (!RobberCWaitingAtCargoShip)
						{
							TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "RobberC Waiting", "While you are on the cargo ship, your RobberC will wait on the Dock for your return.");
						}
						RobberCWaitingAtCargoShip = true;
						RobberC.Task.Wait(1);
					}
					else
					{
						RobberCWaitingAtCargoShip = false;
					}
				}
				if (SpawnedSearchParty)
				{
					CurrentUnionApproachToottip = ApproachToolTip.SearchPartyIncoming;
					if (!SearchPartyArrivedAtUnionDepository && !SearchPartyDestoryed && Game.GameTime > SearchPartyDetectDelay && StockadeB != null && StockadeA != null && StockadeC != null)
					{
						if (!StockadeB.IsAlive && !StockadeC.IsAlive && !StockadeA.IsAlive)
						{
							SpawnedSearchParty = false;
							CurrentUnionApproachToottip = ApproachToolTip.NoneScouting;
							TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Search Party Destoryed", "Well Done. the ~r~Search Party~w~ has been destoryed,the Union Depository has been weakened");
							StockadeA.AttachedBlip.Alpha = 0;
							StockadeB.AttachedBlip.Alpha = 0;
							StockadeC.AttachedBlip.Alpha = 0;
							SearchPartyDestoryed = true;
						}
						else if (StockadeB.IsAlive || StockadeA.IsAlive || StockadeC.IsAlive)
						{
							if (GetDistance_VDIST2(StockadeB.Position, new Vector3(55.77f, -622.93f, 30.64f), "<", 20f))
							{
								SpawnedSearchParty = false;
								CurrentUnionApproachToottip = ApproachToolTip.NoneScouting;
								StockadeB.AttachedBlip.Alpha = 0;
								TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Reforcements Arrived", "~r~Search Party~w~ Arrived at Union Depository, Reforcements have been deployed");
								foreach (Ped guard in Guards)
								{
									guard.AttachedBlip.Delete();
								}
								foreach (Vector4 reinforcmentPosition in ReinforcmentPositions)
								{
									Ped ped4 = GlobalHelper.CreatePed(position: new Vector3(reinforcmentPosition.X, reinforcmentPosition.Y, reinforcmentPosition.Z), model: PedHash.Security01SMM, heading: reinforcmentPosition.H);
									ped4.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
									Guards.Add(ped4);
									SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped4, SmartGuards.EnemyGroup.Security));
								}
								BrokenDisguise = true;
								SearchPartyArrivedAtUnionDepository = true;
							}
							if (GetDistance_VDIST2(StockadeA.Position, new Vector3(55.77f, -622.93f, 30.64f), "<", 20f))
							{
								SpawnedSearchParty = false;
								CurrentUnionApproachToottip = ApproachToolTip.NoneScouting;
								StockadeA.AttachedBlip.Alpha = 0;
								TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Reforcements Arrived", "~r~Search Party~w~ Arrived at Union Depository, Reforcements have been deployed");
								foreach (Vector4 reinforcmentPosition2 in ReinforcmentPositions)
								{
									Ped ped5 = GlobalHelper.CreatePed(position: new Vector3(reinforcmentPosition2.X, reinforcmentPosition2.Y, reinforcmentPosition2.Z), model: PedHash.Security01SMM, heading: reinforcmentPosition2.H);
									ped5.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
									Guards.Add(ped5);
									SmartGuards.GuardGroup[14].GroupList.Add(new SmartGuards.SmartAiV2(ped5, SmartGuards.EnemyGroup.Lost_MC));
								}
								BrokenDisguise = true;
								SearchPartyArrivedAtUnionDepository = true;
							}
							if (GetDistance_VDIST2(StockadeC.Position, new Vector3(55.77f, -622.93f, 30.64f), "<", 20f))
							{
								SpawnedSearchParty = false;
								CurrentUnionApproachToottip = ApproachToolTip.NoneScouting;
								StockadeC.AttachedBlip.Alpha = 0;
								TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Reforcements Arrived", "~r~Search Party~w~ Arrived at Union Depository, Reforcements have been deployed");
								foreach (Ped guard2 in Guards)
								{
									guard2.AttachedBlip.Delete();
								}
								foreach (Vector4 reinforcmentPosition3 in ReinforcmentPositions)
								{
									Ped ped6 = GlobalHelper.CreatePed(position: new Vector3(reinforcmentPosition3.X, reinforcmentPosition3.Y, reinforcmentPosition3.Z), model: PedHash.Security01SMM, heading: reinforcmentPosition3.H);
									ped6.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
									Guards.Add(ped6);
									SmartGuards.GuardGroup[14].GroupList.Add(new SmartGuards.SmartAiV2(ped6, SmartGuards.EnemyGroup.Lost_MC));
								}
								BrokenDisguise = true;
								SearchPartyArrivedAtUnionDepository = true;
							}
						}
					}
				}
				if (BrokenDisguise && !AlertedToBrokenDisguise)
				{
					TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Disguise Broken", "Your disguise has been broken guards now know who you are");
					AlertedToBrokenDisguise = true;
				}
				if (HasBeenInUnionVaultFloor)
				{
					Vector3 vector2 = new Vector3(UnionElevatorVaultFloor.X, UnionElevatorVaultFloor.Y, World.GetGroundHeight(new Vector3(UnionElevatorVaultFloor.X, UnionElevatorVaultFloor.Y, UnionElevatorVaultFloor.Z + 1f)) + 0.25f);
					if (GetDistance_VDIST2(Game.Player.Character.Position, vector2, "<=", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, vector2, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), ActivityMarkerColor);
						if (GetDistance_VDIST2(Game.Player.Character.Position, vector2, "<=", 2.5f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Exit Union Depository");
							if (Game.IsControlJustPressed(Control.Context))
							{
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(500);
								Game.Player.Character.Position = UnionElevatorUnderGroundFloor;
								if (ManageRobberyAnims.CurrentPay >= 10000)
								{
									CurrentUnionDPstage = UnionDPStage.EscapingCops;
								}
								Script.Wait(500);
								GTA.UI.Screen.FadeIn(500);
								if (RobberA != null)
								{
									RobberA.Position = Game.Player.Character.Position.Around(2f);
								}
								if (RobberB != null)
								{
									RobberB.Position = Game.Player.Character.Position.Around(2f);
								}
								if (RobberC != null)
								{
									RobberC.Position = Game.Player.Character.Position.Around(2f);
								}
							}
						}
					}
					vector2 = new Vector3(UnionElevatorUnderGroundFloor.X, UnionElevatorUnderGroundFloor.Y, World.GetGroundHeight(new Vector3(UnionElevatorUnderGroundFloor.X, UnionElevatorUnderGroundFloor.Y, UnionElevatorUnderGroundFloor.Z + 1f)) + 0.25f);
					if (GetDistance_VDIST2(Game.Player.Character.Position, vector2, "<=", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, vector2, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), ActivityMarkerColor);
						if (GetDistance_VDIST2(Game.Player.Character.Position, vector2, "<=", 2.5f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Enter Union Depository");
							if (Game.IsControlJustPressed(Control.Context))
							{
								GTA.UI.Screen.FadeOut(500);
								Script.Wait(500);
								Game.Player.Character.Position = UnionElevatorVaultFloor;
								Script.Wait(500);
								GTA.UI.Screen.FadeIn(500);
								if (RobberA != null)
								{
									RobberA.Position = Game.Player.Character.Position.Around(2f);
								}
								if (RobberB != null)
								{
									RobberB.Position = Game.Player.Character.Position.Around(2f);
								}
								if (RobberC != null)
								{
									RobberC.Position = Game.Player.Character.Position.Around(2f);
								}
							}
						}
					}
				}
				if (CutterDrillingVault && Game.GameTime >= DrillingTimeOut)
				{
					Notification.Show("Cutter Stop");
					CutterDrillingVault = false;
					Vehicle cutterVeh = CutterVeh;
					if (Function.Call<bool>(Hash.IS_PLAYBACK_GOING_ON_FOR_VEHICLE, cutterVeh))
					{
						Function.Call(Hash.STOP_PLAYBACK_RECORDED_VEHICLE, cutterVeh);
					}
					Function.Call(Hash.PLAY_ENTITY_ANIM, cutterVeh, "cutter_spindown", CutterAnimDictsLocal_772, 8f, false, true, false, 0f, 0);
					Function.Call(Hash.SET_ENTITY_HEADING, cutterVeh, 164.9295f);
					Function.Call(Hash.SET_ENTITY_COORDS, cutterVeh, 8.1528f, -652.3942f, 15.0973f, true, false, false, true);
					Function.Call(Hash.SET_ENTITY_VELOCITY, cutterVeh, 0f, 0f, 0f);
					if (RobberA != null)
					{
						RobberA.Position = new Vector3(20.64f, -648.29f, 16.08f);
					}
					if (RobberB != null)
					{
						RobberB.Position = new Vector3(20.64f, -648.29f, 16.08f);
					}
					if (RobberC != null)
					{
						RobberC.Position = new Vector3(20.64f, -648.29f, 16.08f);
					}
					if (Missionblip != null)
					{
						Missionblip.Delete();
					}
					Missionblip = GlobalHelper.CreateBlip(new Vector3(24.64f, -638.45f, 16f));
					Missionblip.Sprite = BlipSprite.Standard;
					Missionblip.Color = BlipColor.Yellow;
					CurrentUnionDPstage = UnionDPStage.UsedCutter;
				}
				bool flag17 = CutterVeh != null;
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(9.23f, -649.7f, 15f), "<=", 7.5f))
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Use Cutter");
					if (Game.IsControlJustPressed(Control.Context))
					{
						bool flag18 = CutterVeh == null;
						if (CutterVeh != null && Game.Player.Character.CurrentVehicle != null && Game.Player.Character.CurrentVehicle == CutterVeh)
						{
							Vehicle cutterVeh2 = CutterVeh;
							for (int num32 = 0; num32 < 5000; num32++)
							{
								if (!Function.Call<bool>(Hash.HAS_VEHICLE_RECORDING_BEEN_LOADED, 551, VehRecordingsLocal_761))
								{
									Function.Call(Hash.REQUEST_VEHICLE_RECORDING, 700, VehRecordingsLocal_761);
									Function.Call(Hash.REQUEST_VEHICLE_RECORDING, 701, VehRecordingsLocal_761);
									Function.Call(Hash.REQUEST_VEHICLE_RECORDING, 702, VehRecordingsLocal_761);
									Function.Call(Hash.REQUEST_VEHICLE_RECORDING, 551, VehRecordingsLocal_761);
								}
								Function.Call<bool>(Hash.HAS_VEHICLE_RECORDING_BEEN_LOADED, 551, VehRecordingsLocal_761);
								int num170 = 4999;
							}
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 4);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 4);
							Script.Wait(100);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 5);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 5);
							while (!Function.Call<bool>(Hash.HAS_VEHICLE_RECORDING_BEEN_LOADED, 551, VehRecordingsLocal_761))
							{
								Script.Yield();
								Function.Call(Hash.REQUEST_VEHICLE_RECORDING, 551, VehRecordingsLocal_761);
							}
							while (!Function.Call<bool>(Hash.LOAD_STREAM, "DRILL_WALL", "BIG_SCORE_3B_SOUNDS"))
							{
								Script.Yield();
								Function.Call<bool>(Hash.LOAD_STREAM, "DRILL_WALL", "BIG_SCORE_3B_SOUNDS");
							}
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 4);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 4);
							Script.Wait(100);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 5);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 5);
							Function.Call<bool>(Hash.LOAD_STREAM, "DRILL_WALL", "BIG_SCORE_3B_SOUNDS");
							if (Function.Call<bool>(Hash.HAS_VEHICLE_RECORDING_BEEN_LOADED, 551, VehRecordingsLocal_761) && Function.Call<int>(Hash.GET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910) == 5 && Function.Call<bool>(Hash.LOAD_STREAM, "DRILL_WALL", "BIG_SCORE_3B_SOUNDS"))
							{
								Function.Call(Hash.START_PLAYBACK_RECORDED_VEHICLE, cutterVeh2, 551, VehRecordingsLocal_761, false);
								CutterDrilledOpenVault = true;
								Function.Call(Hash.FORCE_PLAYBACK_RECORDED_VEHICLE_UPDATE, cutterVeh2, false);
								Function.Call(Hash.SET_VEHICLE_ACTIVE_DURING_PLAYBACK, cutterVeh2, true);
								Function.Call(Hash.RESET_VEHICLE_WHEELS, cutterVeh2, false);
								float num33 = 0.55f;
								Function.Call(Hash.SET_PLAYBACK_SPEED, cutterVeh2, num33);
								Function.Call(Hash.PLAY_STREAM_FRONTEND);
								if (!Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, "BS_2B_VAULT_RAYFIRE"))
								{
									Function.Call(Hash.START_AUDIO_SCENE, "BS_2B_VAULT_RAYFIRE");
								}
								Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 6);
								Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 6);
								Function.Call(Hash.TRIGGER_MUSIC_EVENT, "FH2B_DRILL_START");
								Function.Call(Hash.SET_CONTROL_SHAKE, 2, 200, 200);
								Function.Call(Hash.SETTIMERA, 0);
								Function.Call(Hash.SETTIMERB, 0);
								DrillingTimeOut = Game.GameTime + 10000;
								CutterDrillingVault = true;
							}
						}
					}
				}
				iLocal_910 = Function.Call<Prop>(Hash.GET_RAYFIRE_MAP_OBJECT, 7.25f, -656.98f, 17.14f, 50f, "des_finale_tunnel");
				iLocal_911 = Function.Call<Prop>(Hash.GET_RAYFIRE_MAP_OBJECT, 7.25f, -656.98f, 17.14f, 50f, "des_finale_vault");
				if (Function.Call<float>(Hash.GET_RAYFIRE_MAP_OBJECT_ANIM_PHASE, iLocal_910) >= 0.58f)
				{
					Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 12);
					Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 12);
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(7.25f, -656.98f, 17.14f), "<=", 10f))
				{
					if (Game.IsControlPressed(Control.Sprint) && Game.IsControlJustPressed(Control.Cover))
					{
						TunnelHoleBlocker = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 7f, -657.46f, 13f, 5f, 1086547934, 0, 0, 0);
						TunnelHoleBlocker2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 7f, -657.46f, 13f, 5f, 1086547934, 0, 0, 0);
						if (TunnelHoleBlocker != null)
						{
							if (TunnelHoleBlocker.Exists())
							{
								TunnelHoleBlocker.Delete();
								TunnelHoleBlocker = null;
								Notification.Show("Delete Hole Blocker");
							}
							else if (!TunnelHoleBlocker.Exists())
							{
								TunnelHoleBlocker = GlobalHelper.CreateProp(RequestModel(1086547934), new Vector3(7f, -657.46f, 13f), new Vector3(0f, 0f, 0f), false, false);
								TunnelHoleBlocker.IsPositionFrozen = true;
								Notification.Show("Create Hole Blocker (2)");
							}
						}
						else if (TunnelHoleBlocker == null)
						{
							TunnelHoleBlocker = GlobalHelper.CreateProp(RequestModel(1086547934), new Vector3(7f, -657.46f, 13f), new Vector3(0f, 0f, 0f), false, false);
							TunnelHoleBlocker.IsPositionFrozen = true;
							Notification.Show("Create Hole Blocker (1)");
						}
					}
					bool flag19 = iLocal_910 != null;
				}
				if (RobberA != null)
				{
					if (GetDistance_VDIST2(RobberA.Position, new Vector3(-3.5f, -687.15f, 16f), "<", 5f))
					{
						if (!RobberANearVault)
						{
							LastRobberACanLootSet = RobberACanLoot;
							RobberACanLoot = false;
							RobberANearVault = true;
						}
					}
					else if (RobberANearVault)
					{
						RobberACanLoot = LastRobberACanLootSet;
						RobberANearVault = false;
					}
					if (PlayerInVault && GetDistance_VDIST2(Game.Player.Character.Position, RobberA.Position, "<", 5f) && Game.IsControlJustPressed(Control.VehicleNextRadioTrack))
					{
						RobberA.Task.ClearAll();
						RobberACanLoot = !RobberACanLoot;
						Notification.Show("RobberA Looting " + RobberACanLoot);
					}
				}
				if (RobberB != null)
				{
					if (GetDistance_VDIST2(RobberB.Position, new Vector3(-3.5f, -687.15f, 16f), "<", 5f))
					{
						if (!RobberBNearVault)
						{
							LastRobberBCanLootSet = RobberBCanLoot;
							RobberBCanLoot = false;
							RobberBNearVault = true;
						}
					}
					else if (RobberBNearVault)
					{
						RobberBCanLoot = LastRobberBCanLootSet;
						RobberBNearVault = false;
					}
					if (PlayerInVault && GetDistance_VDIST2(Game.Player.Character.Position, RobberB.Position, "<", 5f) && Game.IsControlJustPressed(Control.VehicleNextRadioTrack))
					{
						RobberB.Task.ClearAll();
						RobberBCanLoot = !RobberBCanLoot;
						Notification.Show("RobberB Looting " + RobberBCanLoot);
					}
				}
				if (RobberC != null)
				{
					if (GetDistance_VDIST2(RobberC.Position, new Vector3(-3.5f, -687.15f, 16f), "<", 5f))
					{
						if (!RobberCNearVault)
						{
							LastRobberCCanLootSet = RobberCCanLoot;
							RobberCCanLoot = false;
							RobberCNearVault = true;
						}
					}
					else if (RobberCNearVault)
					{
						RobberCCanLoot = LastRobberCCanLootSet;
						RobberCNearVault = false;
					}
					if (PlayerInVault && GetDistance_VDIST2(Game.Player.Character.Position, RobberC.Position, "<", 5f) && Game.IsControlJustPressed(Control.VehicleNextRadioTrack))
					{
						RobberC.Task.ClearAll();
						RobberCCanLoot = !RobberCCanLoot;
						Notification.Show("RobberC Looting " + RobberCCanLoot);
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlaceBombScene) >= 1050737829 && PlacingBombOnGrate)
				{
					Vector3 position8 = Game.Player.Character.Bones[Bone.IKRightHand].Position;
					Prop prop16 = GlobalHelper.CreateProp(position: new Vector3(position8.X, position8.Y, position8.Z - 0.1f), model: RequestModel("h4_prop_h4_ld_bomb_01a"), rotation: new Vector3(-90f, 0f, Game.Player.Character.Heading), dynamic: false, placeOnGround: true);
					prop16.IsPositionFrozen = true;
					GrateBombs.Add(prop16);
					PlacingBombOnGrate = false;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlaceBombScene) >= 1065353200)
				{
					PlacingBombOnGrate = false;
					PlaceBombScene = -1;
					Game.Player.Character.Task.ClearAll();
				}
				foreach (KeypadClass unionKeyPad in UnionKeyPads)
				{
					if (!unionKeyPad.SpawnedKeypad && !unionKeyPad.SpawnedKeypad)
					{
						unionKeyPad.SpawnedKeypad = true;
						unionKeyPad.Keypad = GlobalHelper.CreateProp(RequestModel("ch_prop_fingerprint_scanner_01c"), unionKeyPad.KeypadPos, new Vector3(0f, 0f, unionKeyPad.KeypadRot), false, false);
						unionKeyPad.Keypad.IsPositionFrozen = true;
						unionKeyPad.Keypad.AddBlip();
						unionKeyPad.Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
						unionKeyPad.Keypad.AttachedBlip.Scale = 0.75f;
						unionKeyPad.Keypad.AttachedBlip.IsShortRange = true;
						Wait(1);
						Random random7 = new Random();
						int num34 = random7.Next(0, 100);
						if (num34 <= 50)
						{
							unionKeyPad.Type = 1;
						}
						else
						{
							unionKeyPad.Type = 2;
						}
					}
					if (!unionKeyPad.SpawnedKeypad)
					{
						continue;
					}
					float num35 = unionKeyPad.KeypadPos.Z - 3f;
					float num36 = unionKeyPad.KeypadPos.Z + 3f;
					if (GetDistance_VDIST2(Game.Player.Character.Position, unionKeyPad.KeypadPos, ">", 30f))
					{
						unionKeyPad.Keypad.AttachedBlip.Alpha = 0;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, unionKeyPad.KeypadPos, "<", 30f))
					{
						if (Game.Player.Character.Position.Z > num35 && Game.Player.Character.Position.Z < num36)
						{
							if (!BrokenDisguise || (!WaringBugstarsOutfit && !WaringGruppe6Outfit))
							{
								unionKeyPad.Keypad.AttachedBlip.Alpha = 255;
							}
						}
						else
						{
							unionKeyPad.Keypad.AttachedBlip.Alpha = 0;
						}
					}
					if (!HackingKeypad)
					{
						if ((BrokenDisguise && (WaringBugstarsOutfit || WaringGruppe6Outfit || hackedUnionVaultDoor)) || !GetDistance_VDIST2(Game.Player.Character.Position, unionKeyPad.KeypadPos, "<", 10f))
						{
							continue;
						}
						Vector3 offsetPosition7 = unionKeyPad.Keypad.GetOffsetPosition(new Vector3(0f, -1f, 0f));
						if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition7, "<", 2f))
						{
							if (!HackingKeypad)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Hack Keypad");
							}
							if (Game.IsControlJustPressed(Control.Context) && !HackingKeypad)
							{
								LastKeypad = unionKeyPad.Keypad;
								if (Phone != null)
								{
									Phone.Delete();
								}
								if (USB != null)
								{
									USB.Delete();
								}
								Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), unionKeyPad.KeypadPos, false, false);
								USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), unionKeyPad.KeypadPos, false, false);
								USB.IsVisible = false;
								Phone.IsVisible = false;
								USB.IsVisible = true;
								Phone.IsVisible = true;
								int index5 = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
								LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, unionKeyPad.Keypad.Position.X, unionKeyPad.Keypad.Position.Y, unionKeyPad.Keypad.Position.Z, unionKeyPad.Keypad.Rotation.X, unionKeyPad.Keypad.Rotation.Y, unionKeyPad.Keypad.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 0.0, 0.0, 0, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "action_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "action_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								if (Main.DisableHacks)
								{
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1393.91f, 1140.1f, 114.8f), "<", 2f))
									{
										hackedMadrazoFrontDoor = true;
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 10f))
									{
										hackedUnionVaultDoor = true;
										CurrentUnionDPstage = UnionDPStage.InVault;
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1113.719f, -3156.489f, -36.77517f), "<", 10f))
									{
										BankHeistStage = 3;
										hackedBikerClubhouseGrateDoor = true;
									}
									Script.Wait(4000);
									unionKeyPad.AwaitingHackGUI = false;
									LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Script.Wait(4000);
									Game.Player.Character.Task.ClearAllImmediately();
									if (Phone != null)
									{
										Phone.Delete();
									}
									if (USB != null)
									{
										USB.Delete();
									}
								}
								else
								{
									unionKeyPad.AwaitingHackGUI = true;
								}
							}
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200 && unionKeyPad.AwaitingHackGUI)
						{
							Scene1 = -1;
							if (unionKeyPad.Type == 1)
							{
								FingerprintHack.RunKeypadHack();
							}
							if (unionKeyPad.Type == 2)
							{
								FingerprintHack.RunFingerPrintHack();
							}
							unionKeyPad.AwaitingHackGUI = false;
							HackingKeypad = true;
						}
					}
					else
					{
						if (!HackingKeypad)
						{
							continue;
						}
						if (Game.IsControlJustPressed(Control.Context) && HackingKeypad)
						{
							unionKeyPad.AwaitingHackGUI = false;
							LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
						}
						if (!GetDistance_VDIST2(Game.Player.Character.Position, unionKeyPad.KeypadPos, "<", 5f))
						{
							continue;
						}
						if (FingerprintHack.HackFinishedID == 2)
						{
							unionKeyPad.AwaitingHackGUI = false;
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
							unionKeyPad.Keypad.AttachedBlip.Alpha = 0;
							unionKeyPad.AwaitingHackGUI = false;
							FingerprintHack.HackFinishedID = 0;
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1393.91f, 1140.1f, 114.8f), "<", 2f))
							{
								hackedMadrazoFrontDoor = true;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 10f))
							{
								hackedUnionVaultDoor = true;
								CurrentUnionDPstage = UnionDPStage.InVault;
							}
							LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "success_exit", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "fail_react_quick_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "fail_react_quick_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200)
						{
							HackingKeypad = false;
							unionKeyPad.AwaitingHackGUI = false;
							if (Phone != null)
							{
								Phone.Delete();
							}
							if (USB != null)
							{
								USB.Delete();
							}
							Scene1 = -1;
							hackedUnionVaultDoor = false;
							hackedMadrazoFrontDoor = false;
							Game.Player.Character.Task.ClearAll();
						}
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, UnionElevatorVaultFloor, "<", 5f) || CutterDrilledOpenVault)
				{
					HasBeenInUnionVaultFloor = true;
				}
				if (!CutterDrilledOpenVault && !VaultBombUsed)
				{
					if (GotC4Crate && GetDistance_VDIST2(Game.Player.Character.Position, VaultBombPlaceLoc.Item1, "<=", 100f) && !WaitingForBombExplosion && !PlayingBombPlacing)
					{
						World.DrawMarker(MarkerType.Cylinder, VaultBombPlaceLoc.Item1, Vector3.Zero, Vector3.Zero, new Vector3(0.35f, 0.35f, 0.25f), ActivityMarkerColor);
						if (GetDistance_VDIST2(Game.Player.Character.Position, VaultBombPlaceLoc.Item1, "<=", 2.5f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Place bomb on wall");
							if (Game.IsControlJustPressed(Control.Context))
							{
								Vector3 wallPos = GetWallPos();
								Game.Player.Character.Position = VaultBombPlaceLoc.Item1;
								Game.Player.Character.Rotation = VaultBombPlaceLoc.Item2;
								WaitingForBombExplosion = false;
								LoadDict("anim@scripted@heist@ig11_bomb_plant@male@");
								if (BombProp != null)
								{
									BombProp.Delete();
								}
								if (BombBagProp != null)
								{
									BombBagProp.Delete();
								}
								BombProp = GlobalHelper.CreateProp(RequestModel("h4_prop_h4_ld_bomb_01a"), wallPos, new Vector3(0f, 0f, Game.Player.Character.Heading), false, false);
								BombBagProp = GlobalHelper.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.6f, 2f)), false, false);
								BombProp.Position = BombProp.GetOffsetPosition(new Vector3(0f, 0.1f, 0f));
								Vector3 offsetPosition8 = BombProp.GetOffsetPosition(new Vector3(0f, -0.1f, -0.25f));
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition8.X, offsetPosition8.Y, offsetPosition8.Z + 0.75f, 0, 0, Game.Player.Character.Heading, 2);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BombProp, Scene1, "enter_bomb", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BombBagProp, Scene1, "enter_bag", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig11_bomb_plant@male@", "enter", 0.0, 0.0, 0, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig11_bomb_plant@male@"), 1f, -1f, 0, 1148846080);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
								Game.Player.CanControlCharacter = true;
								GamePlayCameraPos = GameplayCamera.Position;
								GamePlayCameraRot = GameplayCamera.Rotation;
								PlaceBombCam = GlobalHelper.CreateCamera(GamePlayCameraPos, GamePlayCameraRot, GameplayCamera.FieldOfView);
								PlaceBombCam.IsActive = true;
								PlaceBombCam.FarClip = 2000f;
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
								World.RenderingCamera = PlaceBombCam;
								PlayingBombPlacing = true;
								BombExplodeDelay = Game.GameTime + 10000;
							}
						}
					}
					if (WaitingForBombExplosion)
					{
						TimeSpan timeSpan3 = TimeSpan.FromMilliseconds(BombExplodeDelay - Game.GameTime);
						Function.Call(Hash.SET_DRAW_ORIGIN, BombProp.Position.X, BombProp.Position.Y, BombProp.Position.Z, 0);
						Vector2 vector3 = World3DToScreen2d(BombProp.Position);
						if (timeSpan3.Seconds > 9)
						{
							drawText(timeSpan3.Seconds.ToString(), vector3.X, vector3.Y, 0.35f, 255, 255, 255);
						}
						else
						{
							drawText(timeSpan3.Seconds.ToString(), vector3.X, vector3.Y, 0.35f, 255, 255, 255);
						}
						Function.Call(Hash.CLEAR_DRAW_ORIGIN);
						if (Game.GameTime >= BombExplodeDelay)
						{
							foreach (int item9 in UnionPTFX)
							{
								Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, item9, false);
							}
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
							int item3 = GlobalHelper.CreatePTFX_LoopedAtCoord("fire_wrecked_train", 9.59f, -656.05f, 16.07f, 0f, 0f, 0f, 3f, true, true, true, true);
							UnionPTFX.Add(item3);
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
							int item4 = GlobalHelper.CreatePTFX_LoopedAtCoord("fire_wrecked_train", 5.0, -656.04998779296875, 16.07, 0.0, 0.0, 0.0, 3.0, true, true, true, true);
							UnionPTFX.Add(item4);
							WaitingForBombExplosion = false;
							World.AddExplosion(BombProp.Position, ExplosionType.TankShell, 3f, 3f);
							if (BombProp != null)
							{
								BombProp.Delete();
							}
							foreach (int item10 in UnionPTFX)
							{
								Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, item10, false);
							}
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 9);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 9);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 4);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 4);
							Script.Wait(100);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 5);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 5);
							Script.Wait(100);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_910, 6);
							Function.Call(Hash.SET_STATE_OF_RAYFIRE_MAP_OBJECT, iLocal_911, 6);
							Function.Call(Hash.TRIGGER_MUSIC_EVENT, "FH2B_DRILL_START");
							VaultBombUsed = true;
							BrokenDisguise = true;
						}
					}
					if (PlayingBombPlacing)
					{
						Vector3 position10 = Game.Player.Character.Position;
						PlaceBombCam.Position = GamePlayCameraPos;
						PlaceBombCam.Rotation = GamePlayCameraRot;
						Function.Call(Hash.SET_FOCUS_POS_AND_VEL, position10.X, position10.Y, position10.Z, 0f, 0f, 0f);
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200)
						{
							if (BombBagProp != null)
							{
								BombBagProp.Delete();
							}
							if (PlaceBombCam != null)
							{
								PlaceBombCam.IsActive = false;
								PlaceBombCam.Delete();
								Game.Player.Character.IsPositionFrozen = false;
								Game.Player.CanControlCharacter = true;
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
							}
							WaitingForBombExplosion = true;
							Game.Player.Character.Task.ClearAll();
							PlayingBombPlacing = false;
							Function.Call(Hash.CLEAR_FOCUS);
						}
					}
				}
				Vector3 position11 = Game.Player.Character.Position;
				Prop prop17 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position11.X, position11.Y, position11.Z, 20f, -2145849767, 0, 0, 0);
				if (prop17 != null)
				{
					if (RobberA != null)
					{
						RobberA.SetNoCollision(prop17, true);
						RobberA.SetNoCollision(prop17, false);
						Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, prop17, RobberA, false);
					}
					Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, prop17, Game.Player.Character, false);
				}
				if (Game.Player.Character.CurrentVehicle == null && !RobberARobbing && RobberACuttingStage == 0 && !RobberAMovingToLoot)
				{
					if (RobberA != null && (GetDistance_VDIST2(RobberA.Position, new Vector3(6.7f, -657.96f, 16f), "<=", 2.5f) || GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(6.7f, -657.96f, 16f), "<=", 2.5f)) && !MovingPastTunnelPathindBlocker)
					{
						if (MovingPastTunnelPathindProp != null)
						{
							MovingPastTunnelPathindProp.Delete();
						}
						if (RobberAClone != null)
						{
							RobberAClone.Delete();
						}
						Vector3 position12 = new Vector3(RobberA.Position.X, RobberA.Position.Y, World.GetGroundHeight(RobberA.Position));
						RobberAClone = Function.Call<Ped>(Hash.CLONE_PED, RobberA, false, false, true);
						RobberAClone.Position = position12;
						RobberAClone.CanBeTargetted = false;
						RobberAClone.BlockPermanentEvents = true;
						RobberAClone.CanRagdoll = false;
						RobberAClone.IsFireProof = true;
						RobberA.IsPositionFrozen = true;
						RobberA.IsVisible = false;
						MovingPastTunnelPathindBlocker = true;
						MovingPastTunnelPathindProp = GlobalHelper.CreateProp(RequestModel("prop_apple_box_01"), position12, RobberAClone.Rotation, false, false);
						MovingPastTunnelPathindProp.IsPositionFrozen = true;
						MovingPastTunnelPathindProp.IsVisible = false;
					}
					if (MovingPastTunnelPathindBlocker)
					{
						if (GetDistance_VDIST2(RobberAClone.Position, Game.Player.Character.Position, ">=", 1.5f))
						{
							if (NextCall <= Game.GameTime)
							{
								Vector3 dir = MovingPastTunnelPathindProp.Position - Game.Player.Character.Position;
								RobberAClone.Rotation = new Vector3(0f, 0f, DirectionToRotation(dir, RobberAClone.Heading).Z - 180f);
								MovingPastTunnelPathindProp.Rotation = RobberAClone.Rotation;
								Vector3 offsetPosition9 = MovingPastTunnelPathindProp.GetOffsetPosition(new Vector3(0f, 0.25f, 1f));
								MovingPastTunnelPathindProp.Position = new Vector3(offsetPosition9.X, offsetPosition9.Y, World.GetGroundHeight(offsetPosition9));
								RobberAClone.Position = MovingPastTunnelPathindProp.Position;
								NextCall = Game.GameTime + 100;
								if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, RobberAClone, "move_action@mp_female@unarmed@core", "walk", 3))
								{
									RobberAClone.Task.PlayAnimation("move_action@mp_female@unarmed@core", "walk", 8f, 1000, AnimationFlags.Loop);
								}
							}
						}
						else
						{
							Notification.Show("near Player ");
							if (RobberA != null)
							{
								RobberA.IsPositionFrozen = false;
								RobberAClone.IsVisible = true;
								Vector3 position13 = new Vector3(RobberAClone.Position.X, RobberAClone.Position.Y, World.GetGroundHeight(RobberAClone.Position));
								RobberA.Position = position13;
								RobberA.Rotation = RobberAClone.Rotation;
								RobberA.IsVisible = true;
							}
							if (MovingPastTunnelPathindProp != null)
							{
								MovingPastTunnelPathindProp.Delete();
							}
							if (RobberAClone != null)
							{
								RobberAClone.Delete();
							}
							MovingPastTunnelPathindBlocker = false;
						}
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(3.21f, -667.61f, 16.13f), "<=", 35f))
				{
					Prop[] nearbyProps10 = World.GetNearbyProps(new Vector3(3.21f, -667.61f, 16.13f), 30f);
					Prop[] array14 = nearbyProps10;
					foreach (Prop prop18 in array14)
					{
						if (prop18 != null)
						{
							if (prop18.Model == -1685825437 || prop18.Model == -2022916910 || prop18.Model == 929864185 || prop18.Model == 307713837 || prop18.Model == -1326042488 || prop18.Model == -1685625437 || prop18.Model == -2108598162 || prop18.Model == -1474092373 || prop18.Model == RequestModel("v_corp_cashtrolley_2") || prop18.Model == RequestModel("v_corp_banktrolley"))
							{
								prop18.Delete();
							}
							if (prop18.Model == RequestModel("v_ilev_fingate") || prop18.Model == RequestModel("prop_gold_vault_gate_01"))
							{
								prop18.IsPositionFrozen = false;
							}
						}
					}
					Prop[] nearbyProps11 = World.GetNearbyProps(new Vector3(7.75f, -663.47f, 16.3f), 3f);
					Prop[] array15 = nearbyProps11;
					foreach (Prop prop19 in array15)
					{
						if (prop19.Model == -275220570)
						{
							prop19.Rotation = new Vector3(0f, 0f, 70f);
						}
					}
					nearbyProps11 = World.GetNearbyProps(new Vector3(3.022f, -660.12f, 16.3f), 3f);
					Prop[] array16 = nearbyProps11;
					foreach (Prop prop20 in array16)
					{
						if (prop20.Model == -275220570)
						{
							prop20.Rotation = new Vector3(0f, 0f, 250f);
						}
					}
				}
				for (int num40 = 0; num40 < ManageRobberyAnims.Crate_Valuables.Count; num40++)
				{
					if (!PlayerInVault)
					{
						ManageRobberyAnims.Crate_Valuables[num40].LootBlip.Alpha = 0;
					}
					if (ManageRobberyAnims.Crate_Valuables[num40].Loot.Model == 15989799)
					{
						ManageRobberyAnims.Crate_Valuables[num40].LootBlip.Alpha = 0;
					}
				}
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
				if (!AwawitingChoiceofSpawn)
				{
					UI.DrawPayment("TAKE", LocalTake);
				}
				if (AwawitingChoiceofSpawn)
				{
					if (GetawayCarDeliveryPed != null)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, GetawayCarDeliveryPed.Position, ">", 5f))
						{
							UI.DrawPayment("TAKE", LocalTake);
						}
					}
					else
					{
						UI.DrawPayment("TAKE", LocalTake);
					}
				}
				PlayerInVault = GetDistance(Game.Player.Character.Position, new Vector3(3.21f, -667.61f, 16.13f)) < 20f;
				if (PlayerInVault)
				{
					if (Game.Player.Character.Position.Z > 19f)
					{
						PlayerInVault = false;
					}
					if (MovingPastTunnelPathindBlocker)
					{
						hasMovedPastBlocker = true;
						LastRobberACanLootSet = false;
						RobberACanLoot = false;
						LastRobberBCanLootSet = false;
						RobberBCanLoot = false;
						LastRobberCCanLootSet = false;
						RobberCCanLoot = false;
					}
					if (!MovingPastTunnelPathindBlocker && hasMovedPastBlocker)
					{
						RobberACanLoot = LastRobberACanLootSet;
						hasMovedPastBlocker = false;
						RobberBCanLoot = LastRobberBCanLootSet;
						hasMovedPastBlocker = false;
						RobberCCanLoot = LastRobberCCanLootSet;
						hasMovedPastBlocker = false;
					}
					InHouseRobbery = true;
				}
				bool playerInVault = PlayerInVault;
				if (!RobberAWaitingAtCargoShip)
				{
					bool flag = false;
					Vector3 position14 = Game.Player.Character.Position;
					int num41 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position14.X, position14.Y, position14.Z);
					if (num41 == 134913)
					{
						flag = true;
					}
					if (num41 == 59906)
					{
						flag = true;
					}
					if (RobberA != null && HKHModHelperNew.Class1.PlayerCompanion_AllowHeistMovement && (HeistPathfindVariation == 0 || (HeistPathfindVariation == 1 && RobberALastCombatTime <= 2.5f)) && GetDistance_VDIST2(Game.Player.Character.Position, RobberA.Position, ">=", 2f) && Game.Player.Character.CurrentVehicle == null && !RobberA.IsInCombat && !RobberARobbing && !RobberAMovingToLoot)
					{
						if (Followtimer == 0)
						{
							Vector3 offsetPosition10 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
							if (HeistPathfindVariation == 0 || flag)
							{
								if (GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, ">", 2f) && GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, "<", 5f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberA, offsetPosition10.X, offsetPosition10.Y, offsetPosition10.Z, 1.5f, -1, 0f, 0f);
								}
								else if (GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, ">=", 5f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberA, offsetPosition10.X, offsetPosition10.Y, offsetPosition10.Z, 15f, -1, 0f, 0f);
								}
								else if (GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, "<=", 2f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberA, offsetPosition10.X, offsetPosition10.Y, offsetPosition10.Z, 1f, -1, 0f, 0f);
								}
							}
							if (HeistPathfindVariation == 1 && !flag && GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, ">=", 2f))
							{
								Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: RobberA, argument1: offsetPosition10.X, argument2: offsetPosition10.Y, argument3: offsetPosition10.Z, argument4: 30f, argument5: 20000, argument6: 1.5f, argument7: 512);
							}
						}
						if (Followtimer != 24 && !RobberA.IsInCombat)
						{
							Followtimer++;
						}
						if (Followtimer >= 24 && !RobberA.IsInCombat)
						{
							Followtimer = 0;
						}
					}
				}
				if (!RobberBWaitingAtCargoShip)
				{
					bool flag2 = false;
					Vector3 position15 = Game.Player.Character.Position;
					int num42 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position15.X, position15.Y, position15.Z);
					if (num42 == 134913)
					{
						flag2 = true;
					}
					if (num42 == 59906)
					{
						flag2 = true;
					}
					if (RobberB != null && HKHModHelperNew.Class1.PlayerCompanion_AllowHeistMovement && (HeistPathfindVariation == 0 || HeistPathfindVariation == 1) && GetDistance_VDIST2(Game.Player.Character.Position, RobberB.Position, ">=", 2f) && Game.Player.Character.CurrentVehicle == null && !RobberB.IsInCombat && !RobberBRobbing && !RobberBMovingToLoot)
					{
						if (Followtimer == 0)
						{
							Vector3 offsetPosition11 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
							if (HeistPathfindVariation == 0 || flag2)
							{
								if (GetDistance_VDIST2(RobberB.Position, Game.Player.Character.Position, ">", 2f) && GetDistance_VDIST2(RobberB.Position, Game.Player.Character.Position, "<", 5f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberB, offsetPosition11.X, offsetPosition11.Y, offsetPosition11.Z, 1.5f, -1, 0f, 0f);
								}
								else if (GetDistance_VDIST2(RobberB.Position, Game.Player.Character.Position, ">=", 5f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberB, offsetPosition11.X, offsetPosition11.Y, offsetPosition11.Z, 15f, -1, 0f, 0f);
								}
								else if (GetDistance_VDIST2(RobberB.Position, Game.Player.Character.Position, "<=", 2f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberB, offsetPosition11.X, offsetPosition11.Y, offsetPosition11.Z, 1f, -1, 0f, 0f);
								}
							}
							if (HeistPathfindVariation == 1 && !flag2 && GetDistance_VDIST2(RobberB.Position, Game.Player.Character.Position, ">=", 2f))
							{
								Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: RobberB, argument1: offsetPosition11.X, argument2: offsetPosition11.Y, argument3: offsetPosition11.Z, argument4: 30f, argument5: 20000, argument6: 1.5f, argument7: 512);
							}
						}
						if (Followtimer != 24 && !RobberB.IsInCombat)
						{
							Followtimer++;
						}
						if (Followtimer >= 24 && !RobberB.IsInCombat)
						{
							Followtimer = 0;
						}
					}
				}
				if (!RobberCWaitingAtCargoShip)
				{
					bool flag3 = false;
					Vector3 position16 = Game.Player.Character.Position;
					int num43 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position16.X, position16.Y, position16.Z);
					if (num43 == 134913)
					{
						flag3 = true;
					}
					if (num43 == 59906)
					{
						flag3 = true;
					}
					if (RobberC != null && HKHModHelperNew.Class1.PlayerCompanion_AllowHeistMovement && (HeistPathfindVariation == 0 || HeistPathfindVariation == 1) && GetDistance_VDIST2(Game.Player.Character.Position, RobberC.Position, ">=", 2f) && Game.Player.Character.CurrentVehicle == null && !RobberC.IsInCombat && !RobberCRobbing && !RobberCMovingToLoot)
					{
						if (Followtimer == 0)
						{
							Vector3 offsetPosition12 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
							if (HeistPathfindVariation == 0 || flag3)
							{
								if (GetDistance_VDIST2(RobberC.Position, Game.Player.Character.Position, ">", 2f) && GetDistance_VDIST2(RobberC.Position, Game.Player.Character.Position, "<", 5f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberC, offsetPosition12.X, offsetPosition12.Y, offsetPosition12.Z, 1.5f, -1, 0f, 0f);
								}
								else if (GetDistance_VDIST2(RobberC.Position, Game.Player.Character.Position, ">=", 5f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberC, offsetPosition12.X, offsetPosition12.Y, offsetPosition12.Z, 15f, -1, 0f, 0f);
								}
								else if (GetDistance_VDIST2(RobberC.Position, Game.Player.Character.Position, "<=", 2f))
								{
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberC, offsetPosition12.X, offsetPosition12.Y, offsetPosition12.Z, 1f, -1, 0f, 0f);
								}
							}
							if (HeistPathfindVariation == 1 && !flag3 && GetDistance_VDIST2(RobberC.Position, Game.Player.Character.Position, ">=", 2f))
							{
								Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: RobberC, argument1: offsetPosition12.X, argument2: offsetPosition12.Y, argument3: offsetPosition12.Z, argument4: 30f, argument5: 20000, argument6: 1.5f, argument7: 512);
							}
						}
						if (Followtimer != 24 && !RobberC.IsInCombat)
						{
							Followtimer++;
						}
						if (Followtimer >= 24 && !RobberC.IsInCombat)
						{
							Followtimer = 0;
						}
					}
				}
				if (UnionRappelingFromHeli)
				{
					if (RobberA != null)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(21.38f, -638.49f, 16.08f), "<=", 10f) && Game.Player.Character.HeightAboveGround < 1f && RobberA.HeightAboveGround < 1f)
						{
							Notification.Show("in Site");
							RobberA.IsInvincible = false;
							if (MaverickPilot != null)
							{
								MaverickPilot.Delete();
							}
							if (MaverickHeli != null)
							{
								MaverickHeli.Delete();
							}
							MaverickHeli = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Maverick), MaverickLoc.Item1, MaverickLoc.Item2.Z);
							MaverickHeli.IsInvincible = true;
							MaverickHeli.LockStatus = VehicleLockStatus.CannotEnter;
							MaverickHeli.AddBlip();
							MaverickHeli.AttachedBlip.Color = BlipColor.Blue;
							MaverickHeli.AttachedBlip.Name = "Maverick for Union Depository";
							MaverickHeli.AttachedBlip.IsShortRange = true;
							MaverickHeli.IsPersistent = true;
							MaverickPilot = GlobalHelper.CreatePed(RequestModel(PedHash.Pilot02SMM), MaverickHeli.GetOffsetPosition(new Vector3(1f, 1f, 0f)));
							MaverickPilot.BlockPermanentEvents = true;
							CurrentUnionDPstage = UnionDPStage.ApproachOrScout;
							UnionRappelingFromHeli = false;
							GotHiredHeli = false;
							foreach (Blip intelMarker in IntelMarkers)
							{
								if (GetDistance_VDIST2(MaverickHeli.Position, intelMarker.Position, "<=", 30f))
								{
									intelMarker.Alpha = 255;
								}
							}
							foreach (Blip item11 in IntelMarkersRadiusArea)
							{
								if (GetDistance_VDIST2(MaverickHeli.Position, item11.Position, "<=", 30f))
								{
									item11.Alpha = 144;
								}
							}
						}
					}
					else if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(21.38f, -638.49f, 16.08f), "<=", 10f) && Game.Player.Character.HeightAboveGround < 1f)
					{
						if (MaverickPilot != null)
						{
							MaverickPilot.Delete();
						}
						if (MaverickHeli != null)
						{
							MaverickHeli.Delete();
						}
						MaverickHeli = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Maverick), MaverickLoc.Item1, MaverickLoc.Item2.Z);
						MaverickHeli.IsInvincible = true;
						MaverickHeli.LockStatus = VehicleLockStatus.CannotEnter;
						MaverickHeli.AddBlip();
						MaverickHeli.AttachedBlip.Color = BlipColor.Blue;
						MaverickHeli.AttachedBlip.Name = "Maverick for Union Depository";
						MaverickHeli.AttachedBlip.IsShortRange = true;
						MaverickHeli.IsPersistent = true;
						MaverickPilot = GlobalHelper.CreatePed(RequestModel(PedHash.Pilot02SMM), MaverickHeli.GetOffsetPosition(new Vector3(1f, 1f, 0f)));
						MaverickPilot.BlockPermanentEvents = true;
						CurrentUnionDPstage = UnionDPStage.ApproachOrScout;
						UnionRappelingFromHeli = false;
						GotHiredHeli = false;
						foreach (Blip intelMarker2 in IntelMarkers)
						{
							if (GetDistance_VDIST2(MaverickHeli.Position, intelMarker2.Position, "<=", 30f))
							{
								intelMarker2.Alpha = 255;
							}
						}
						foreach (Blip item12 in IntelMarkersRadiusArea)
						{
							if (GetDistance_VDIST2(MaverickHeli.Position, item12.Position, "<=", 30f))
							{
								item12.Alpha = 144;
							}
						}
					}
				}
				if (!SpawnedGetawayVehicle)
				{
					if (HasBeenInUnionVaultFloor && ChoosenStoredVehicleSpawn && !SpawnedGetawayVehicle)
					{
						if (GetawayVehicleSpawnChoice == 1 && SavedGetawayVehicle != null)
						{
							GetawayCar = GlobalHelper.CreateVehicle(RequestModel(SavedGetawayVehicle.SpawnHash), new Vector3(48.6f, -651.81f, 30.6f), 72f);
							SavedGetawayVehicle.Load(GetawayCar);
							GetawayCar.AddBlip();
							GetawayCar.AttachedBlip.Color = BlipColor.Blue;
							GetawayCar.AttachedBlip.Name = "GetawayCar for Union Depository";
							GetawayCar.AttachedBlip.IsShortRange = true;
							GetawayCar.IsPersistent = true;
							LastGetawayCarPosition = GetawayCar.Position;
							LastGetawayCarRotation = GetawayCar.Rotation;
							SpawnedGetawayVehicle = true;
						}
						if (GetawayVehicleSpawnChoice == 2 && SavedGetawayVehicle != null)
						{
							GetawayCar = GlobalHelper.CreateVehicle(RequestModel(SavedGetawayVehicle.SpawnHash), new Vector3(-22.71f, -587.44f, 16.94f), 245f);
							SavedGetawayVehicle.Load(GetawayCar);
							GetawayCar.AddBlip();
							GetawayCar.AttachedBlip.Color = BlipColor.Blue;
							GetawayCar.AttachedBlip.Name = "GetawayCar for Union Depository";
							GetawayCar.AttachedBlip.IsShortRange = true;
							GetawayCar.IsPersistent = true;
							LastGetawayCarPosition = GetawayCar.Position;
							LastGetawayCarRotation = GetawayCar.Rotation;
							SpawnedGetawayVehicle = true;
						}
					}
				}
				else if (GetawayCar != null && !GetawayCar.Exists())
				{
					GetawayCar = GlobalHelper.CreateVehicle(RequestModel(SavedGetawayVehicle.SpawnHash), LastGetawayCarPosition, LastGetawayCarRotation.Z);
					SavedGetawayVehicle.Load(GetawayCar);
					GetawayCar.AddBlip();
					GetawayCar.AttachedBlip.Color = BlipColor.Blue;
					GetawayCar.AttachedBlip.Name = "GetawayCar for Union Depository";
					GetawayCar.AttachedBlip.IsShortRange = true;
					GetawayCar.IsPersistent = true;
					LastGetawayCarPosition = GetawayCar.Position;
					LastGetawayCarRotation = GetawayCar.Rotation;
					SpawnedGetawayVehicle = true;
				}
				if (GetawayCarDeliveryPed != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, GetawayCarDeliveryPed.Position, ">=", 20f))
					{
						if (VehSpawn1blip != null)
						{
							VehSpawn1blip.Alpha = 0;
						}
						if (VehSpawn2blip != null)
						{
							VehSpawn2blip.Alpha = 0;
						}
						if (HighwayEnteranceblip != null)
						{
							HighwayEnteranceblip.Alpha = 0;
						}
						GetawayCarDeliveryPed.AttachedBlip.Alpha = 0;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, GetawayCarDeliveryPed.Position, "<=", 100f))
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, GetawayCarDeliveryPed.Position, ">=", 20f))
						{
							if (VehSpawn1blip != null)
							{
								VehSpawn1blip.Alpha = 0;
							}
							if (VehSpawn2blip != null)
							{
								VehSpawn2blip.Alpha = 0;
							}
							if (HighwayEnteranceblip != null)
							{
								HighwayEnteranceblip.Alpha = 0;
							}
							GetawayCarDeliveryPed.AttachedBlip.Alpha = 0;
						}
						else
						{
							if (VehSpawn1blip != null)
							{
								VehSpawn1blip.Alpha = 255;
							}
							if (VehSpawn2blip != null)
							{
								VehSpawn2blip.Alpha = 255;
							}
							if (HighwayEnteranceblip != null)
							{
								HighwayEnteranceblip.Alpha = 255;
							}
							Scaleform scaleform = new Scaleform("PLAYER_NAME_07");
							scaleform.CallFunction("SET_PLAYER_NAME", "Park ~d~Vehicle~w~ here For\n ~y~Union Depository Heist Getaway Vehicle\n~w~\\/");
							scaleform.Render3D(new Vector3(599.89f, -502.38f, 27f), new Vector3(0f, 0f, 78f), new Vector3(6f, 3f, 2f));
							GetawayCarDeliveryPed.AttachedBlip.Alpha = 255;
						}
						World.DrawMarker(MarkerType.Cylinder, new Vector3(599.89f, -502.38f, 23.72f), Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), Color.Yellow);
						Vehicle closestVehicle = World.GetClosestVehicle(new Vector3(599.89f, -502.38f, 23.72f), 10f);
						CurrentUnionApproachToottip = ApproachToolTip.PayForGetAwayCar;
						if (closestVehicle != null)
						{
							GetawayVehicleDetected = true;
							if (GetDistance_VDIST2(Game.Player.Character.Position, GetawayCarDeliveryPed.Position, "<=", 2.5f) && !PaidforStorageOfGetawayCar && !PaidforStorageOfGetawayCar)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Store Vehicle $500,000 ");
								if (Game.IsControlJustPressed(Control.Context))
								{
									if (ManagePlayerFunds.AreFundsHigher(500000))
									{
										Script.Wait(100);
										foreach (Blip intelMarker3 in IntelMarkers)
										{
											if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker3.Position, "<=", 30f))
											{
												intelMarker3.Alpha = 0;
											}
										}
										foreach (Blip item13 in IntelMarkersRadiusArea)
										{
											if (GetDistance_VDIST2(Game.Player.Character.Position, item13.Position, "<=", 30f))
											{
												item13.Alpha = 0;
											}
										}
										AwawitingChoiceofSpawn = true;
										PaidforStorageOfGetawayCar = true;
										ManagePlayerFunds.RemoveFunds(500000);
										if (VehSpawn1blip != null)
										{
											VehSpawn1blip.Delete();
										}
										if (VehSpawn2blip != null)
										{
											VehSpawn2blip.Delete();
										}
										if (HighwayEnteranceblip != null)
										{
											HighwayEnteranceblip.Delete();
										}
										HighwayEnteranceblip = GlobalHelper.CreateBlip(new Vector3(-64.44f, -539.26f, 30f));
										HighwayEnteranceblip.Sprite = BlipSprite.Standard;
										HighwayEnteranceblip.Color = BlipColor.Yellow;
										HighwayEnteranceblip.IsShortRange = true;
										HighwayEnteranceblip.Name = "Underground Highway Enterance";
										VehSpawn1blip = GlobalHelper.CreateBlip(new Vector3(48.6f, -651.81f, 30.6f));
										VehSpawn1blip.Sprite = BlipSprite.Standard;
										VehSpawn1blip.Color = BlipColor.Blue;
										VehSpawn1blip.IsShortRange = true;
										VehSpawn1blip.Name = "Option 1 : Car Park";
										VehSpawn2blip = GlobalHelper.CreateBlip(new Vector3(-22.71f, -587.44f, 16.94f));
										VehSpawn2blip.Sprite = BlipSprite.Standard;
										VehSpawn2blip.Color = BlipColor.Blue;
										VehSpawn2blip.IsShortRange = true;
										VehSpawn2blip.Name = "Option 2 : Underground Area";
									}
									else
									{
										Notification.Show("Not enough money to store a personal vehicle!");
									}
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, GetawayCarDeliveryPed.Position, "<=", 2.5f) && PaidforStorageOfGetawayCar && AwawitingChoiceofSpawn && !ChoosenStoredVehicleSpawn)
							{
								if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
								{
									Scaleform scaleform2 = new Scaleform("instructional_buttons");
									scaleform2.CallFunction("CLEAR_ALL");
									scaleform2.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
									scaleform2.CallFunction("CREATE_CONTAINER");
									scaleform2.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, Key_ActionYes_Keyboard, 0), "Option 1 (Car Park)");
									scaleform2.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, Key_ActionNo_Keyboard, 0), "Option 2 (Underground)");
									scaleform2.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
									scaleform2.Render2D();
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Option 1, ~INPUT_VEH_DUCK~ Option 2");
									if (Game.IsControlJustPressed((Control)Key_ActionYes_Keyboard))
									{
										if (VehSpawn1blip != null)
										{
											VehSpawn1blip.Delete();
										}
										if (VehSpawn2blip != null)
										{
											VehSpawn2blip.Delete();
										}
										if (HighwayEnteranceblip != null)
										{
											HighwayEnteranceblip.Delete();
										}
										TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Getaway Car Delivery", "Your Getaway car will be delivered to the Union Depository Car park on entering the Vault");
										AwawitingChoiceofSpawn = false;
										GetawayVehicleSpawnChoice = 1;
										SavedGetawayVehicle = new SaveCar();
										SavedGetawayVehicle.LoadIniFile("scripts//HKH_SinglePlayerHeists//Heists//UnionDepositoryGetawayCar.ini");
										SavedGetawayVehicle.SaveCurrentCar(closestVehicle.DisplayName, closestVehicle);
										closestVehicle.LockStatus = VehicleLockStatus.CannotEnter;
										ChoosenStoredVehicleSpawn = true;
									}
									if (Game.IsControlJustPressed((Control)Key_ActionNo_Keyboard))
									{
										if (VehSpawn1blip != null)
										{
											VehSpawn1blip.Delete();
										}
										if (VehSpawn2blip != null)
										{
											VehSpawn2blip.Delete();
										}
										if (HighwayEnteranceblip != null)
										{
											HighwayEnteranceblip.Delete();
										}
										TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Getaway Car Delivery", "Your Getaway car will be delivered to the underground area on entering the Vault");
										AwawitingChoiceofSpawn = false;
										GetawayVehicleSpawnChoice = 2;
										SavedGetawayVehicle = new SaveCar();
										SavedGetawayVehicle.LoadIniFile("scripts//HKH_SinglePlayerHeists//Heists//UnionDepositoryGetawayCar.ini");
										SavedGetawayVehicle.SaveCurrentCar(closestVehicle.DisplayName, closestVehicle);
										closestVehicle.LockStatus = VehicleLockStatus.CannotEnter;
										ChoosenStoredVehicleSpawn = true;
									}
								}
								if (Game.LastInputMethod == InputMethod.GamePad)
								{
									Scaleform scaleform3 = new Scaleform("instructional_buttons");
									scaleform3.CallFunction("CLEAR_ALL");
									scaleform3.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
									scaleform3.CallFunction("CREATE_CONTAINER");
									scaleform3.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, Key_ActionYes_Controller, 0), "Option 1 (Car Park)");
									scaleform3.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, Key_ActionNo_Controller, 0), "Option 2 (Underground)");
									scaleform3.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
									scaleform3.Render2D();
									if (Game.IsControlJustPressed((Control)Key_ActionYes_Controller))
									{
										if (VehSpawn1blip != null)
										{
											VehSpawn1blip.Delete();
										}
										if (VehSpawn2blip != null)
										{
											VehSpawn2blip.Delete();
										}
										if (HighwayEnteranceblip != null)
										{
											HighwayEnteranceblip.Delete();
										}
										TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Getaway Car Delivery", "Your Getaway car will be delivered to the Union Depository Car park on entering the Vault");
										AwawitingChoiceofSpawn = false;
										GetawayVehicleSpawnChoice = 1;
										SavedGetawayVehicle = new SaveCar();
										SavedGetawayVehicle.LoadIniFile("scripts//HKH_SinglePlayerHeists//Heists//UnionDepositoryGetawayCar.ini");
										SavedGetawayVehicle.SaveCurrentCar(closestVehicle.DisplayName, closestVehicle);
										closestVehicle.LockStatus = VehicleLockStatus.CannotEnter;
										ChoosenStoredVehicleSpawn = true;
									}
									if (Game.IsControlJustPressed((Control)Key_ActionNo_Controller))
									{
										if (VehSpawn1blip != null)
										{
											VehSpawn1blip.Delete();
										}
										if (VehSpawn2blip != null)
										{
											VehSpawn2blip.Delete();
										}
										if (HighwayEnteranceblip != null)
										{
											HighwayEnteranceblip.Delete();
										}
										TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Getaway Car Delivery", "Your Getaway car will be delivered to the underground area on entering the Vault");
										AwawitingChoiceofSpawn = false;
										GetawayVehicleSpawnChoice = 2;
										SavedGetawayVehicle = new SaveCar();
										SavedGetawayVehicle.LoadIniFile("scripts//HKH_SinglePlayerHeists//Heists//UnionDepositoryGetawayCar.ini");
										SavedGetawayVehicle.SaveCurrentCar(closestVehicle.DisplayName, closestVehicle);
										closestVehicle.LockStatus = VehicleLockStatus.CannotEnter;
										ChoosenStoredVehicleSpawn = true;
									}
								}
							}
						}
						if (closestVehicle == null)
						{
							GetawayVehicleDetected = false;
						}
					}
				}
				if (MaverickPilot != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, MaverickPilot.Position, "<=", 2.5f) && !GotHiredHeli)
					{
						if (!BlownUpGrate)
						{
							DisplayHelpTextThisFrame("~BLIP_INFO_ICON~ Maverick Enter Union Depository Locked");
						}
						else if (!GotHiredHeli)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Hire Pilot $750,000 ");
							if (Game.IsControlJustPressed(Control.Context))
							{
								if (ManagePlayerFunds.AreFundsHigher(750000))
								{
									GotHiredHeli = true;
									foreach (Blip intelMarker4 in IntelMarkers)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker4.Position, "<=", 30f))
										{
											intelMarker4.Alpha = 0;
										}
									}
									foreach (Blip item14 in IntelMarkersRadiusArea)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, item14.Position, "<=", 30f))
										{
											item14.Alpha = 0;
										}
									}
									GotHiredHeli = true;
									ManagePlayerFunds.RemoveFunds(750000);
									MaverickPilot.Task.ClearAllImmediately();
									MaverickHeli.LockStatus = VehicleLockStatus.None;
									MaverickPilot.Task.EnterVehicle(MaverickHeli, VehicleSeat.Driver);
									if (RobberA != null)
									{
										RobberA.Task.EnterVehicle(MaverickHeli, VehicleSeat.RightRear);
									}
									Game.Player.Character.Task.EnterVehicle(MaverickHeli, VehicleSeat.LeftRear);
								}
								else
								{
									Notification.Show("Not enough money to hire pilot!");
								}
							}
						}
					}
					if (GotHiredHeli)
					{
						if (RobberA != null)
						{
							if (MaverickPilot.CurrentVehicle == MaverickHeli && RobberA.CurrentVehicle == MaverickHeli && Game.Player.Character.CurrentVehicle == MaverickHeli)
							{
								CurrentUnionApproachToottip = ApproachToolTip.HeliRide;
								if (!StartedHeliRide)
								{
									EnteringHeliTaskOveride = 0;
									HeliWaypoint = HelicopterRoute[0];
									StartedHeliRide = true;
									Function.Call(Hash.TASK_HELI_MISSION, MaverickPilot, MaverickPilot.CurrentVehicle, 0, 0, HeliWaypoint.X, HeliWaypoint.Y, HeliWaypoint.Z, 4, 50.0, 10.0, MaverickPilot.CurrentVehicle.Heading, 10, 10, 5f, 0);
								}
								if (StartedHeliRide && GetDistance_VDIST2(MaverickHeli.Position, new Vector3(25.05681f, -634.4557f, 75f), "<", 5f))
								{
									MaverickHeli.Rotation = new Vector3(0f, 0f, MaverickHeli.Heading);
									if (!UnionRappelingFromHeli)
									{
										RobberA.IsInvincible = true;
										RobberA.SetIntoVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.RightRear);
										Game.Player.Character.SetIntoVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.LeftRear);
										UnionRappelingFromHeli = true;
										Function.Call(Hash.TASK_RAPPEL_FROM_HELI, Game.Player.Character, 1092616192);
										Function.Call(Hash.TASK_RAPPEL_FROM_HELI, RobberA, 1092616192);
									}
								}
							}
							else if (GetDistance_VDIST2(MaverickHeli.Position, Game.Player.Character.Position, "<", 100f))
							{
								if (EnteringHeliTaskOveride != 24)
								{
									EnteringHeliTaskOveride++;
								}
								else
								{
									CurrentUnionApproachToottip = ApproachToolTip.EnteringHeli;
									if (MaverickPilot.VehicleTryingToEnter != MaverickHeli && MaverickPilot.CurrentVehicle != MaverickHeli)
									{
										MaverickPilot.Task.EnterVehicle(MaverickHeli, VehicleSeat.Driver);
									}
									if (RobberA.VehicleTryingToEnter != MaverickHeli && RobberA.CurrentVehicle != MaverickHeli)
									{
										RobberA.Task.EnterVehicle(MaverickHeli, VehicleSeat.RightRear);
									}
									if (Game.Player.Character.VehicleTryingToEnter != MaverickHeli && Game.Player.Character.CurrentVehicle != MaverickHeli)
									{
										Game.Player.Character.Task.EnterVehicle(MaverickHeli, VehicleSeat.LeftRear);
									}
									EnteringHeliTaskOveride = 0;
								}
							}
						}
						else if (MaverickPilot.CurrentVehicle == MaverickHeli && Game.Player.Character.CurrentVehicle == MaverickHeli)
						{
							CurrentUnionApproachToottip = ApproachToolTip.HeliRide;
							if (!StartedHeliRide)
							{
								EnteringHeliTaskOveride = 0;
								HeliWaypoint = HelicopterRoute[0];
								StartedHeliRide = true;
								Function.Call(Hash.TASK_HELI_MISSION, MaverickPilot, MaverickPilot.CurrentVehicle, 0, 0, HeliWaypoint.X, HeliWaypoint.Y, HeliWaypoint.Z, 4, 50.0, 10.0, MaverickPilot.CurrentVehicle.Heading, 10, 10, 5f, 0);
							}
							if (StartedHeliRide && GetDistance_VDIST2(MaverickHeli.Position, new Vector3(25.05681f, -634.4557f, 75f), "<", 5f))
							{
								MaverickHeli.Rotation = new Vector3(0f, 0f, MaverickHeli.Heading);
								if (!UnionRappelingFromHeli)
								{
									Game.Player.Character.SetIntoVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.LeftRear);
									UnionRappelingFromHeli = true;
									Function.Call(Hash.TASK_RAPPEL_FROM_HELI, Game.Player.Character, 1092616192);
								}
							}
						}
						else if (GetDistance_VDIST2(MaverickHeli.Position, Game.Player.Character.Position, "<", 100f))
						{
							if (EnteringHeliTaskOveride != 24)
							{
								EnteringHeliTaskOveride++;
							}
							else
							{
								CurrentUnionApproachToottip = ApproachToolTip.EnteringHeli;
								if (MaverickPilot.VehicleTryingToEnter != MaverickHeli && MaverickPilot.CurrentVehicle != MaverickHeli)
								{
									MaverickPilot.Task.EnterVehicle(MaverickHeli, VehicleSeat.Driver);
								}
								if (Game.Player.Character.VehicleTryingToEnter != MaverickHeli && Game.Player.Character.CurrentVehicle != MaverickHeli)
								{
									Game.Player.Character.Task.EnterVehicle(MaverickHeli, VehicleSeat.LeftRear);
								}
								EnteringHeliTaskOveride = 0;
							}
						}
					}
				}
				if (CutterKeys != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, CutterKeys.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, CutterKeys.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, CutterKeys.Position, "<=", 2.5f) && !GotCutterKeys)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Cutter Keys");
						if (Game.IsControlJustPressed(Control.Context))
						{
							TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Intel Found", "You have found the Cutter's keys");
							foreach (Blip intelMarker5 in IntelMarkers)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker5.Position, "<=", 30f))
								{
									intelMarker5.Alpha = 0;
								}
							}
							foreach (Blip item15 in IntelMarkersRadiusArea)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, item15.Position, "<=", 30f))
								{
									item15.Alpha = 0;
								}
							}
							GotCutterKeys = true;
						}
					}
				}
				if (BugstarClothes != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, BugstarClothes.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, BugstarClothes.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, BugstarClothes.Position, "<=", 2.5f) && !GotBugstarClothes)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Bugstar Clothes");
						if (Game.IsControlJustPressed(Control.Context))
						{
							WaringGruppe6Outfit = false;
							WaringBugstarsOutfit = true;
							ApplyUnionDepositoryOutfits();
							TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Intel Found", "You have found Bugstar Clothes");
							foreach (Blip intelMarker6 in IntelMarkers)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker6.Position, "<=", 30f))
								{
									intelMarker6.Alpha = 0;
								}
							}
							foreach (Blip item16 in IntelMarkersRadiusArea)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, item16.Position, "<=", 30f))
								{
									item16.Alpha = 0;
								}
							}
							GotBugstarClothes = true;
						}
					}
				}
				if (Gruppe6Clothes != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Clothes.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, Gruppe6Clothes.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Clothes.Position, "<=", 2.5f) && !GotGruppe6Clothes)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Gruppe6 Clothes");
						if (Game.IsControlJustPressed(Control.Context))
						{
							WaringGruppe6Outfit = true;
							WaringBugstarsOutfit = false;
							ApplyUnionDepositoryOutfits();
							TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Intel Found", "You have found Gruppe6 Clothes");
							foreach (Blip intelMarker7 in IntelMarkers)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker7.Position, "<=", 30f))
								{
									intelMarker7.Alpha = 0;
								}
							}
							foreach (Blip item17 in IntelMarkersRadiusArea)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, item17.Position, "<=", 30f))
								{
									item17.Alpha = 0;
								}
							}
							GotGruppe6Clothes = true;
						}
					}
				}
				if (UnionDP_C4Crate != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDP_C4Crate.Position, "<", 30f))
					{
						World.DrawMarker(MarkerType.Chevron1, UnionDP_C4Crate.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
					}
					UnionDP_C4Crate.IsPositionFrozen = true;
					if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDP_C4Crate.Position, "<=", 2.5f) && !GotC4Crate)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup C4");
						if (Game.IsControlJustPressed(Control.Context))
						{
							TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Intel Found", "You have found C4");
							foreach (Blip intelMarker8 in IntelMarkers)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker8.Position, "<=", 150f))
								{
									intelMarker8.Alpha = 0;
								}
							}
							foreach (Blip item18 in IntelMarkersRadiusArea)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, item18.Position, "<=", 150f))
								{
									item18.Alpha = 0;
								}
							}
							foreach (Blip item19 in CheckCrateLocBlip)
							{
								item19.Alpha = 0;
							}
							GotC4Crate = true;
						}
					}
				}
				if (MaverickHeli != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, MaverickHeli.Position, "<", 30f))
					{
						MaverickHeli.AttachedBlip.Alpha = 255;
					}
					else
					{
						MaverickHeli.AttachedBlip.Alpha = 0;
					}
					if (MaverickHeli != null)
					{
						MaverickHeli.IsVisible = true;
						MaverickHeli.Opacity = 255;
					}
					if (MaverickPilot != null)
					{
						if (!GotHiredHeli)
						{
							MaverickPilot.Task.TurnTo(Game.Player.Character);
						}
						MaverickPilot.IsVisible = true;
						MaverickPilot.Opacity = 255;
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, MaverickLoc.Item1, ">", 10f) && GetDistance_VDIST2(Game.Player.Character.Position, MaverickLoc.Item1, "<", 30f))
					{
						if (!MaverickHeli.Exists())
						{
							MaverickHeli = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Maverick), MaverickLoc.Item1, MaverickLoc.Item2.Z);
							MaverickHeli.IsInvincible = true;
							MaverickHeli.LockStatus = VehicleLockStatus.CannotEnter;
							MaverickHeli.AddBlip();
							MaverickHeli.AttachedBlip.Color = BlipColor.Blue;
							MaverickHeli.AttachedBlip.Name = "Maverick for Union Depository";
							MaverickHeli.AttachedBlip.IsShortRange = true;
						}
						if (!GotHiredHeli)
						{
							MaverickHeli.Position = MaverickLoc.Item1;
							MaverickHeli.Rotation = MaverickLoc.Item2;
						}
					}
				}
				if (Gruppe6Stockade != null)
				{
					Gruppe6Stockade_LastPosition = Gruppe6Stockade.Position;
					Gruppe6Stockade_LastRotation = Gruppe6Stockade.Rotation;
					if (GotGruppe6Clothes)
					{
						Gruppe6Clothes.IsCollisionEnabled = false;
						Gruppe6Clothes.SetNoCollision(Gruppe6Stockade, true);
						Gruppe6Stockade.SetNoCollision(Gruppe6Clothes, true);
						Gruppe6Clothes.Position = Gruppe6Stockade.GetOffsetPosition(new Vector3(0f, -3.2f, 0.5f));
						Gruppe6Clothes.Rotation = Gruppe6Stockade.Rotation;
						if (!WaringGruppe6Outfit)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Clothes.Position, "<", 30f))
							{
								World.DrawMarker(MarkerType.Chevron1, Gruppe6Clothes.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Clothes.Position, "<=", 2.5f) && GotGruppe6Clothes)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Gruppe6 Clothes");
								if (Game.IsControlJustPressed(Control.Context))
								{
									WaringGruppe6Outfit = true;
									WaringBugstarsOutfit = false;
									ApplyUnionDepositoryOutfits();
									TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Waring Outfit", "You have found Gruppe6 Clothes");
									foreach (Blip intelMarker9 in IntelMarkers)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker9.Position, "<=", 30f))
										{
											intelMarker9.Alpha = 0;
										}
									}
									foreach (Blip item20 in IntelMarkersRadiusArea)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, item20.Position, "<=", 30f))
										{
											item20.Alpha = 0;
										}
									}
									GotGruppe6Clothes = true;
								}
							}
						}
					}
					if (Gruppe6Stockade.Driver == Game.Player.Character)
					{
						if (!GotGruppe6Van)
						{
							foreach (Blip intelMarker10 in IntelMarkers)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker10.Position, "<=", 30f))
								{
									intelMarker10.Alpha = 0;
								}
							}
							foreach (Blip item21 in IntelMarkersRadiusArea)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, item21.Position, "<=", 30f))
								{
									item21.Alpha = 0;
								}
							}
						}
						OverrideDisguiseIfAimingGun = true;
						IsDressedUpAsBugstarsOrGroupe6 = true;
						GTA.UI.Screen.ShowSubtitle("Drive the Stockade to the ~y~Union Depository~w~");
						GotGruppe6Van = true;
						foreach (Blip intelMarker11 in IntelMarkers)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker11.Position, "<=", 30f))
							{
								intelMarker11.Alpha = 0;
							}
						}
						foreach (Blip item22 in IntelMarkersRadiusArea)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, item22.Position, "<=", 30f))
							{
								item22.Alpha = 0;
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Stockade.Position, "<", 30f))
					{
						if (!GotGruppe6Clothes)
						{
							Scaleform scaleform4 = new Scaleform("PLAYER_NAME_01");
							scaleform4.CallFunction("SET_PLAYER_NAME", "Stockade Requires ~g~Gruppe6 Clothes~w~");
							scaleform4.Render3D(Gruppe6Stockade.GetOffsetPosition(new Vector3(0f, 6f, 0f)), new Vector3(0f, 0f, Gruppe6Stockade.Heading), new Vector3(6f, 3f, 2f));
						}
						Gruppe6Stockade.AttachedBlip.Alpha = 255;
					}
					else
					{
						Gruppe6Stockade.AttachedBlip.Alpha = 0;
					}
					if (!GotGruppe6Clothes && GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Stockade.Position, "<", 30f))
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Stockade.Position, "<", 5f))
						{
							DisplayHelpTextThisFrame("~BLIP_INFO_ICON~ Acquire Gruppe6 Clothes to use Stockade");
						}
						Gruppe6Stockade.LockStatus = VehicleLockStatus.CannotEnter;
					}
				}
				if (BugstarsVan != null)
				{
					if (BugstarsVan.Driver == Game.Player.Character)
					{
						IntelMarkers[0].Alpha = 0;
						IntelMarkersRadiusArea[0].Alpha = 0;
						OverrideDisguiseIfNotUnarmed = true;
						IsDressedUpAsBugstarsOrGroupe6 = true;
						GTA.UI.Screen.ShowSubtitle("Drive the ~b~Bugstars Van~w~ to the ~y~Union Depository~w~ as Bugstars, and enter through the ~y~Side or Back~w~ Enterance ");
					}
					BugstarsVan_LastPosition = BugstarsVan.Position;
					BugstarsVan_LastRotation = BugstarsVan.Rotation;
					if (GotBugstarClothes)
					{
						BugstarClothes.IsCollisionEnabled = false;
						BugstarClothes.SetNoCollision(BugstarsVan, true);
						BugstarsVan.SetNoCollision(BugstarClothes, true);
						BugstarClothes.Position = BugstarsVan.GetOffsetPosition(new Vector3(0f, -2.5f, -0.25f));
						BugstarClothes.Rotation = BugstarsVan.Rotation;
						if (!WaringBugstarsOutfit)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, BugstarClothes.Position, "<", 30f))
							{
								World.DrawMarker(MarkerType.Chevron1, BugstarClothes.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), Vector3.Zero, new Vector3(0f, 180f, 0f), new Vector3(0.65f, 0.65f, 0.65f), Color.FromArgb(0, 255, 0), true, true);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, BugstarClothes.Position, "<=", 2.5f) && GotBugstarClothes)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Pickup Bugstar Clothes");
								if (Game.IsControlJustPressed(Control.Context))
								{
									WaringGruppe6Outfit = false;
									WaringBugstarsOutfit = true;
									ApplyUnionDepositoryOutfits();
									TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "Waring Outfit", "You have found Bugstar Clothes");
									foreach (Blip intelMarker12 in IntelMarkers)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker12.Position, "<=", 30f))
										{
											intelMarker12.Alpha = 0;
										}
									}
									foreach (Blip item23 in IntelMarkersRadiusArea)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, item23.Position, "<=", 30f))
										{
											item23.Alpha = 0;
										}
									}
									GotBugstarClothes = true;
								}
							}
						}
					}
					if (BugstarsVan.Driver == Game.Player.Character)
					{
						if (!GotBugstarVan)
						{
							foreach (Blip intelMarker13 in IntelMarkers)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker13.Position, "<=", 30f))
								{
									intelMarker13.Alpha = 0;
								}
							}
							foreach (Blip item24 in IntelMarkersRadiusArea)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, item24.Position, "<=", 30f))
								{
									item24.Alpha = 0;
								}
							}
						}
						GotBugstarVan = true;
						foreach (Blip intelMarker14 in IntelMarkers)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, intelMarker14.Position, "<=", 30f))
							{
								intelMarker14.Alpha = 0;
							}
						}
						foreach (Blip item25 in IntelMarkersRadiusArea)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, item25.Position, "<=", 30f))
							{
								item25.Alpha = 0;
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, BugstarsVan.Position, "<", 30f))
					{
						if (!GotBugstarClothes)
						{
							Scaleform scaleform5 = new Scaleform("PLAYER_NAME_02");
							scaleform5.CallFunction("SET_PLAYER_NAME", "Bugstar Van Requires ~g~Bugstar Clothes~w~");
							scaleform5.Render3D(BugstarsVan.GetOffsetPosition(new Vector3(0f, 3f, 0f)), new Vector3(0f, 0f, BugstarsVan.Heading - 180f), new Vector3(6f, 3f, 2f));
						}
						BugstarsVan.AttachedBlip.Alpha = 255;
					}
					else
					{
						BugstarsVan.AttachedBlip.Alpha = 0;
					}
					if (!GotBugstarClothes && GetDistance_VDIST2(Game.Player.Character.Position, BugstarsVan.Position, "<", 30f))
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, BugstarsVan.Position, "<", 5f))
						{
							DisplayHelpTextThisFrame("~BLIP_INFO_ICON~ Acquire Bugstar Clothes to use Van");
						}
						BugstarsVan.LockStatus = VehicleLockStatus.CannotEnter;
					}
				}
				if (CutterVeh != null)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, CutterVeh.Position, "<", 30f))
					{
						if (!GotCutterKeys)
						{
							CutterVeh.LockStatus = VehicleLockStatus.CannotEnter;
							Scaleform scaleform6 = new Scaleform("PLAYER_NAME_03");
							scaleform6.CallFunction("SET_PLAYER_NAME", "Cutter Requires ~g~Cutter Keys~w~");
							scaleform6.Render3D(CutterVeh.GetOffsetPosition(new Vector3(0f, 7f, 0f)), new Vector3(0f, 0f, -60f), new Vector3(6f, 3f, 2f));
						}
						else
						{
							CutterVeh.LockStatus = VehicleLockStatus.Unlocked;
						}
						CutterVeh.AttachedBlip.Alpha = 255;
					}
					else
					{
						CutterVeh.AttachedBlip.Alpha = 0;
					}
					if (!GotCutterKeys && GetDistance_VDIST2(Game.Player.Character.Position, CutterVeh.Position, "<", 30f))
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, CutterVeh.Position, "<", 5f))
						{
							DisplayHelpTextThisFrame("~BLIP_INFO_ICON~ Acquire Cutter Keys to use Cutter");
						}
						CutterVeh.LockStatus = VehicleLockStatus.CannotEnter;
					}
				}
				if (!GotC4Crate)
				{
					for (int num44 = 0; num44 < CheckCrateLoc.Count; num44++)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, CheckCrateLocBlip[num44].Position, "<", 100f) && !CheckCrateLoc[num44])
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, CheckCrateLocBlip[num44].Position, "<", 5f) && !CheckCrateLoc[num44])
							{
								CheckCrateLocBlip[num44].Alpha = 0;
								CheckCrateLoc[num44] = true;
							}
							CheckCrateLocBlip[num44].Scale = 0.5f;
							CheckCrateLocBlip[num44].Alpha = 255;
						}
						else
						{
							CheckCrateLocBlip[num44].Scale = 0.5f;
							CheckCrateLocBlip[num44].Alpha = 0;
						}
					}
				}
				else
				{
					for (int num45 = 0; num45 < CheckCrateLoc.Count; num45++)
					{
						CheckCrateLocBlip[num45].Scale = 0.5f;
						CheckCrateLocBlip[num45].Alpha = 0;
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(41.26f, -618.03f, 31.62f), "<", 100f))
				{
					Prop[] nearbyProps12 = World.GetNearbyProps(new Vector3(41.26f, -618.03f, 31.62f), 10f);
					Prop[] array17 = nearbyProps12;
					foreach (Prop prop21 in array17)
					{
						if (prop21 != null && GetDistance_VDIST2(Game.Player.Character.Position, prop21.Position, ">", 3.5f))
						{
							prop21.Delete();
						}
					}
				}
				if ((!WaringBugstarsOutfit && !WaringGruppe6Outfit) || BrokenDisguise)
				{
					World.DrawMarker(MarkerType.Cylinder, UnionFrontEnt, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), ActivityMarkerColor);
					if (GetDistance_VDIST2(Game.Player.Character.Position, UnionFrontEnt, "<=", 2.5f) && !GotCutterKeys)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Enter Through Front Enterance");
						if (Game.IsControlJustPressed(Control.Context))
						{
							foreach (Blip intelMarker15 in IntelMarkers)
							{
								if (intelMarker15 != null)
								{
									intelMarker15.Alpha = 0;
								}
							}
							foreach (Blip item26 in IntelMarkersRadiusArea)
							{
								if (item26 != null)
								{
									item26.Alpha = 0;
								}
							}
							foreach (Blip enteranceBlipMarker in EnteranceBlipMarkers)
							{
								if (enteranceBlipMarker != null)
								{
									enteranceBlipMarker.Alpha = 0;
								}
							}
							foreach (Blip item27 in EnteranceBlipMarkersRadiusArea)
							{
								if (item27 != null)
								{
									item27.Alpha = 0;
								}
							}
							GTA.UI.Screen.FadeOut(500);
							Script.Wait(500);
							CurrentUnionDPstage = UnionDPStage.InUnionDepositoryLobby;
							Game.Player.Character.Position = new Vector3(115.44f, -639.28f, 206.04f);
							if (Missionblip != null)
							{
								Missionblip.Delete();
							}
							Missionblip = GlobalHelper.CreateBlip(new Vector3(147.08f, -637.59f, 206.4f));
							Missionblip.Sprite = BlipSprite.Standard;
							Missionblip.Color = BlipColor.Yellow;
							CurrentUnionDPstage = UnionDPStage.InUnionDepositoryLobby;
							BrokenDisguise = true;
							Script.Wait(500);
							GTA.UI.Screen.FadeIn(500);
							if (RobberA != null)
							{
								RobberA.Position = Game.Player.Character.Position.Around(2f);
							}
							if (RobberB != null)
							{
								RobberB.Position = Game.Player.Character.Position.Around(2f);
							}
							if (RobberC != null)
							{
								RobberC.Position = Game.Player.Character.Position.Around(2f);
							}
						}
					}
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, IntelMarkers[0].Position, "<", 100f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.EnteringAsBugstars;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, IntelMarkers[1].Position, "<", 250f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.ExitingViaC4;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, IntelMarkers[2].Position, "<", 100f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.EnteringViaCutter;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, IntelMarkers[3].Position, "<", 100f))
				{
					if (!GotHiredHeli)
					{
						CurrentUnionApproachToottip = ApproachToolTip.ExitingViaHeli;
					}
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, IntelMarkers[4].Position, "<", 100f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.NearGruppe6Clothes;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, IntelMarkers[5].Position, "<", 100f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.NearGruppe6Clothes;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, IntelMarkers[6].Position, "<", 100f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.PayForGetAwayCar;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, CutterVeh.Position, "<", 25f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.NearCutterTooltip;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, EnteranceBlipMarkers[0].Position, "<", 20f) || AllBombsPlaced)
				{
					CurrentUnionApproachToottip = ApproachToolTip.ExitingViaGrateAndHeli;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, EnteranceBlipMarkers[1].Position, "<", 15f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.EnteringAsGrouppe6ORBugstars;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, EnteranceBlipMarkers[2].Position, "<", 60f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.FrontEnterance;
				}
				else if (GetDistance_VDIST2(Game.Player.Character.Position, EnteranceBlipMarkers[3].Position, "<", 15f))
				{
					CurrentUnionApproachToottip = ApproachToolTip.EnteringAsGrouppe6ORBugstars;
				}
				else
				{
					CurrentUnionApproachToottip = ApproachToolTip.NoneScouting;
				}
				if (CurrentUnionDPstage == UnionDPStage.InUnionDepositoryLobby)
				{
					GTA.UI.Screen.ShowSubtitle("fight past the ~r~guards~w~ and make your way to the ~y~Elevator~w~ to enter the Vault");
					if (Missionblip != null)
					{
						Vector3 vector4 = new Vector3(Missionblip.Position.X, Missionblip.Position.Y, World.GetGroundHeight(Missionblip.Position) + 0.25f);
						if (GetDistance_VDIST2(Game.Player.Character.Position, vector4, "<=", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, vector4, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), ActivityMarkerColor);
							if (GetDistance_VDIST2(Game.Player.Character.Position, vector4, "<=", 2.5f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Enter Elevator");
								if (Game.IsControlJustPressed(Control.Context))
								{
									GTA.UI.Screen.FadeOut(500);
									Script.Wait(500);
									CurrentUnionDPstage = UnionDPStage.InUnionDepositoryLobby;
									Game.Player.Character.Position = UnionElevatorVaultFloor;
									if (Missionblip != null)
									{
										Missionblip.Delete();
									}
									BrokenDisguise = true;
									Script.Wait(500);
									GTA.UI.Screen.FadeIn(500);
									CurrentUnionDPstage = UnionDPStage.InVaultAfterLobby;
								}
							}
						}
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.InVaultAfterLobby)
				{
					Prop[] nearbyProps13 = World.GetNearbyProps(new Vector3(-1.880968f, -687.0798f, 16f), 10f);
					if (nearbyProps13.Length != 0)
					{
						Prop[] array18 = nearbyProps13;
						foreach (Prop prop22 in array18)
						{
							if (prop22.Model == -1932297301 && prop22 != null)
							{
								prop22.Rotation = new Vector3(0f, 0f, 160f);
							}
						}
					}
					GTA.UI.Screen.ShowSubtitle("Hack the ~y~Keypad~w~ to open the vault");
					if (Missionblip != null)
					{
						GetDistance_VDIST2(Game.Player.Character.Position, Missionblip.Position, "<", 5f);
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.WalkingToVault)
				{
					GTA.UI.Screen.ShowSubtitle("Make your way to the ~g~Vault~w~");
					if (Missionblip != null && GetDistance_VDIST2(Game.Player.Character.Position, Missionblip.Position, "<", 5f))
					{
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						CurrentUnionDPstage = UnionDPStage.InVault;
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.WalkingToElevator)
				{
					GTA.UI.Screen.ShowSubtitle("Make your way to the ~y~Elevator~w~ to enter the Vault");
					if (Missionblip != null)
					{
						Vector3 vector5 = new Vector3(Missionblip.Position.X, Missionblip.Position.Y, World.GetGroundHeight(Missionblip.Position) + 0.25f);
						if (GetDistance_VDIST2(Game.Player.Character.Position, vector5, "<=", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, vector5, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), ActivityMarkerColor);
							if (GetDistance_VDIST2(Game.Player.Character.Position, vector5, "<=", 2.5f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Enter Elevator");
								if (Game.IsControlJustPressed(Control.Context))
								{
									if (Missionblip != null)
									{
										Missionblip.Delete();
									}
									Missionblip = GlobalHelper.CreateBlip(UnionDepositoryVaultDoorPos);
									Missionblip.Sprite = BlipSprite.Standard;
									Missionblip.Color = BlipColor.Yellow;
									CurrentUnionDPstage = UnionDPStage.WalkingToVault;
									GTA.UI.Screen.FadeOut(500);
									Script.Wait(500);
									Game.Player.Character.Position = UnionElevatorVaultFloor;
									Script.Wait(500);
									GTA.UI.Screen.FadeIn(500);
								}
							}
						}
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.EnteringAsGrouppe6WalkingToElv)
				{
					IsDressedUpAsBugstarsOrGroupe6 = true;
					OverrideDisguiseIfAimingGun = true;
					GTA.UI.Screen.ShowSubtitle("Make your way to the ~y~Elevator~w~ to enter the Vault");
					if (Missionblip != null && GetDistance_VDIST2(Game.Player.Character.Position, Missionblip.Position, "<", 5f))
					{
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						Missionblip = GlobalHelper.CreateBlip(UnionElevatorUnderGroundFloor);
						Missionblip.Sprite = BlipSprite.Standard;
						Missionblip.Color = BlipColor.Yellow;
						CurrentUnionDPstage = UnionDPStage.WalkingToElevator;
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.EnteringAsBugstarsWalkingToElv)
				{
					IsDressedUpAsBugstarsOrGroupe6 = true;
					OverrideDisguiseIfNotUnarmed = true;
					GTA.UI.Screen.ShowSubtitle("Make your way to the ~y~Elevator~w~ to enter the Vault");
					if (Missionblip != null && GetDistance_VDIST2(Game.Player.Character.Position, Missionblip.Position, "<", 5f))
					{
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						Missionblip = GlobalHelper.CreateBlip(UnionElevatorUnderGroundFloor);
						Missionblip.Sprite = BlipSprite.Standard;
						Missionblip.Color = BlipColor.Yellow;
						CurrentUnionDPstage = UnionDPStage.WalkingToElevator;
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.EnteringAsBugstars)
				{
					IsDressedUpAsBugstarsOrGroupe6 = true;
					OverrideDisguiseIfNotUnarmed = true;
					GTA.UI.Screen.ShowSubtitle("Park the ~b~Bugstars Van~w~ in the designated ~y~Spot~w~");
					if (Missionblip != null && GetDistance_VDIST2(BugstarsVan.Position, Missionblip.Position, "<", 5f))
					{
						ParkedAtParkingSpot = true;
						WeaponsDelay = Game.GameTime + 60000;
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						Missionblip = GlobalHelper.CreateBlip(UnionElevatorUnderGroundFloor);
						Missionblip.Sprite = BlipSprite.Standard;
						Missionblip.Color = BlipColor.Yellow;
						CurrentUnionDPstage = UnionDPStage.EnteringAsBugstarsWalkingToElv;
						TextNotification("CHAR_DEFAULT", "~g~ Unknown", "~g~Intel~w~", "While dressed as Bugstars, Player cannot use weapons, using weapons will break disguise");
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.EnteringAsGrouppe6)
				{
					IsDressedUpAsBugstarsOrGroupe6 = true;
					OverrideDisguiseIfAimingGun = true;
					GTA.UI.Screen.ShowSubtitle("Park the ~b~Stockade~w~ in the designated ~y~Spot~w~");
					if (Missionblip != null && GetDistance_VDIST2(Gruppe6Stockade.Position, Missionblip.Position, "<", 5f))
					{
						ParkedAtParkingSpot = true;
						WeaponsDelay = Game.GameTime + 60000;
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						Missionblip = GlobalHelper.CreateBlip(new Vector3(10.41f, -670.89f, 33.44f));
						Missionblip.Sprite = BlipSprite.Standard;
						Missionblip.Color = BlipColor.Yellow;
						CurrentUnionDPstage = UnionDPStage.EnteringAsGrouppe6WalkingToElv;
						TextNotification("CHAR_DEFAULT", "~g~ Unknown", "~g~Intel~w~", "While dressed as Gruppe6, Aiming at guards will break disguise");
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.DrivingCutter)
				{
					GTA.UI.Screen.ShowSubtitle("Drill through the ~y~wall~w~ into the bank vault");
				}
				if (CurrentUnionDPstage == UnionDPStage.UsedCutter)
				{
					GTA.UI.Screen.ShowSubtitle("Park the cutter out of the way by the ~y~wall.~w~");
					if (Missionblip != null && GetDistance_VDIST2(CutterVeh.Position, Missionblip.Position, "<", 5f))
					{
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						CurrentUnionDPstage = UnionDPStage.InVault;
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.InVault)
				{
					HasBeenInUnionVaultFloor = true;
					if ((!WaringBugstarsOutfit && !WaringGruppe6Outfit) || BrokenDisguise)
					{
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					}
					GTA.UI.Screen.ShowSubtitle("Loot the ~g~Vault~w~\n or Leave the ~y~Vault~w~ when ready");
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(3.21f, -667.61f, 16.13f), ">", 40f))
					{
						CurrentUnionDPstage = UnionDPStage.EscapingCops;
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.EscapingCops)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
					if ((WaringBugstarsOutfit || WaringGruppe6Outfit) && !BrokenDisguise)
					{
						GTA.UI.Screen.ShowSubtitle("Leave the ~y~Union Depository~w~ and leave the area");
					}
					else
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(3.21f, -667.61f, 16.13f), "<", 40f) || GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(4.9f, -658.2f, 32f), "<", 40f))
						{
							Game.Player.WantedLevel = 5;
						}
						GTA.UI.Screen.ShowSubtitle("Leave the ~y~Union Depository~w~ and escape the ~r~cops~w~");
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.ApproachOrScout && CurrentUnionApproachToottip == ApproachToolTip.NoneScouting)
				{
					if (BugstarsVan != null && Gruppe6Stockade != null)
					{
						if (BugstarsVan.Driver != Game.Player.Character && Gruppe6Stockade.Driver != Game.Player.Character && !SpawnedSearchParty)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Approach the ~y~Union Depository~w~ or Scout for ~g~Assets/Intel~w~");
						}
						if (SpawnedSearchParty && !SearchPartyArrivedAtUnionDepository && !SearchPartyDestoryed)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Destroy the ~r~Gruppe6 Search Party Stockades~w~ before it reaches the ~y~Union Depository~w~ to stop ~d~Reinforcements");
						}
					}
					if (BugstarsVan != null && (GetDistance_VDIST2(BugstarsVan.Position, new Vector3(9.66f, -683.09f, 31.94f), "<", 20f) || GetDistance_VDIST2(BugstarsVan.Position, new Vector3(-44.92f, -690.45f, 31.94f), "<", 20f)))
					{
						foreach (Blip intelMarker16 in IntelMarkers)
						{
							if (intelMarker16 != null)
							{
								intelMarker16.Alpha = 0;
							}
						}
						foreach (Blip item28 in IntelMarkersRadiusArea)
						{
							if (item28 != null)
							{
								item28.Alpha = 0;
							}
						}
						foreach (Blip enteranceBlipMarker2 in EnteranceBlipMarkers)
						{
							if (enteranceBlipMarker2 != null)
							{
								enteranceBlipMarker2.Alpha = 0;
							}
						}
						foreach (Blip item29 in EnteranceBlipMarkersRadiusArea)
						{
							if (item29 != null)
							{
								item29.Alpha = 0;
							}
						}
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						Missionblip = GlobalHelper.CreateBlip(new Vector3(-34.36f, -673.3f, 31.94f));
						Missionblip.Sprite = BlipSprite.Standard;
						Missionblip.Color = BlipColor.Yellow;
						CurrentUnionDPstage = UnionDPStage.EnteringAsBugstars;
					}
					if (Gruppe6Stockade != null && (GetDistance_VDIST2(Gruppe6Stockade.Position, new Vector3(9.66f, -683.09f, 31.94f), "<", 20f) || GetDistance_VDIST2(Gruppe6Stockade.Position, new Vector3(-44.92f, -690.45f, 31.94f), "<", 20f)))
					{
						foreach (Blip intelMarker17 in IntelMarkers)
						{
							if (intelMarker17 != null)
							{
								intelMarker17.Alpha = 0;
							}
						}
						foreach (Blip item30 in IntelMarkersRadiusArea)
						{
							if (item30 != null)
							{
								item30.Alpha = 0;
							}
						}
						foreach (Blip enteranceBlipMarker3 in EnteranceBlipMarkers)
						{
							if (enteranceBlipMarker3 != null)
							{
								enteranceBlipMarker3.Alpha = 0;
							}
						}
						foreach (Blip item31 in EnteranceBlipMarkersRadiusArea)
						{
							if (item31 != null)
							{
								item31.Alpha = 0;
							}
						}
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						Missionblip = GlobalHelper.CreateBlip(new Vector3(-6.12f, -671.4f, 31.94f));
						Missionblip.Sprite = BlipSprite.Standard;
						Missionblip.Color = BlipColor.Yellow;
						CurrentUnionDPstage = UnionDPStage.EnteringAsGrouppe6;
					}
				}
				if (CurrentUnionDPstage == UnionDPStage.ApproachOrScout)
				{
					if (CutterVeh != null && Game.Player.Character.CurrentVehicle == CutterVeh)
					{
						foreach (Blip intelMarker18 in IntelMarkers)
						{
							if (intelMarker18 != null)
							{
								intelMarker18.Alpha = 0;
							}
						}
						foreach (Blip item32 in IntelMarkersRadiusArea)
						{
							if (item32 != null)
							{
								item32.Alpha = 0;
							}
						}
						foreach (Blip enteranceBlipMarker4 in EnteranceBlipMarkers)
						{
							if (enteranceBlipMarker4 != null)
							{
								enteranceBlipMarker4.Alpha = 0;
							}
						}
						foreach (Blip item33 in EnteranceBlipMarkersRadiusArea)
						{
							if (item33 != null)
							{
								item33.Alpha = 0;
							}
						}
						if (Missionblip != null)
						{
							Missionblip.Delete();
						}
						Missionblip = GlobalHelper.CreateBlip(new Vector3(8.84f, -650.44f, 15f));
						Missionblip.Sprite = BlipSprite.Standard;
						Missionblip.Color = BlipColor.Yellow;
						CurrentUnionDPstage = UnionDPStage.DrivingCutter;
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.FrontEnterance)
					{
						if (Game.Player.Character.CurrentVehicle != null)
						{
							if (Game.Player.Character.CurrentVehicle != BugstarsVan || Game.Player.Character.CurrentVehicle != Gruppe6Stockade)
							{
								GTA.UI.Screen.ShowSubtitle("Take out the ~r~guards~w~ and make your way to the ~g~Elevator~w~");
							}
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Take out the ~r~guards~w~ and make your way to the ~g~Elevator~w~");
						}
					}
				}
				if (Game.Player.Character.CurrentVehicle == MaverickHeli)
				{
					CurrentUnionApproachToottip = ApproachToolTip.HeliRide;
				}
				if (CurrentUnionDPstage == UnionDPStage.ApproachOrScout && CurrentUnionApproachToottip != 0)
				{
					if (CurrentUnionApproachToottip == ApproachToolTip.PayForGetAwayCar)
					{
						if (!PaidforStorageOfGetawayCar && GetawayCarDeliveryPed != null)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, GetawayCarDeliveryPed.Position, "<", 20f))
							{
								if (!GetawayVehicleDetected)
								{
									GTA.UI.Screen.ShowSubtitle("You can pay to have a ~b~Personal Vehicle~w~ Stored near the Union Depository, ~d~Park a vehicle near the yellow marker to save it as Personal Vehicle~w~");
								}
								else
								{
									GTA.UI.Screen.ShowSubtitle("You can pay to have a ~b~Personal Vehicle~w~ Stored near the Union Depository, Talk to the ~y~Broker~w~ about storing your vehicle");
								}
							}
							else
							{
								GTA.UI.Screen.ShowSubtitle("Find the ~y~Broker~w~ to have a ~b~Personal Vehicle~w~ Stored near the Union Depository,");
							}
						}
						if (PaidforStorageOfGetawayCar && AwawitingChoiceofSpawn && !ChoosenStoredVehicleSpawn)
						{
							GTA.UI.Screen.ShowSubtitle("Choose An Option, ~d~1.~w~ Car Park. ~d~2.~w~ Underground area, Near Cutter/Highway Enterance.\n ~p~Open your Map to see where spawns are.~w~");
						}
						if (ChoosenStoredVehicleSpawn)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Approach the ~y~Union Depository~w~ or Scout for more ~g~Assets/Intel~w~");
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.SearchPartyIncoming)
					{
						GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Destroy the ~r~Gruppe6 Search Party Stockade~w~ before it reaches the ~y~Union Depository~w~ to stop ~d~Reinforcements");
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.HeliRide)
					{
						if (HeliTaskTimer != 48)
						{
							HeliTaskTimer++;
						}
						else if (RobberA != null)
						{
							if (MaverickPilot.CurrentVehicle != null && MaverickPilot.CurrentVehicle == MaverickHeli && RobberA.CurrentVehicle == MaverickHeli && Game.Player.Character.CurrentVehicle == MaverickHeli)
							{
								if (CurrentHeliWaypoint <= 2)
								{
									Function.Call(Hash.TASK_HELI_MISSION, MaverickPilot, MaverickPilot.CurrentVehicle, 0, 0, HeliWaypoint.X, HeliWaypoint.Y, HeliWaypoint.Z, 4, 50.0, 10.0, (HeliWaypoint - MaverickPilot.CurrentVehicle.Position).ToHeading(), 10, 10, 5f, 0);
								}
								else
								{
									Function.Call(Hash.TASK_HELI_MISSION, MaverickPilot, MaverickPilot.CurrentVehicle, 0, 0, HeliWaypoint.X, HeliWaypoint.Y, HeliWaypoint.Z, 4, 10.0, 10.0, (HeliWaypoint - MaverickPilot.CurrentVehicle.Position).ToHeading(), 10, 10, 5f, 0);
								}
							}
						}
						else if (MaverickPilot.CurrentVehicle != null && MaverickPilot.CurrentVehicle == MaverickHeli && Game.Player.Character.CurrentVehicle == MaverickHeli)
						{
							if (CurrentHeliWaypoint <= 2)
							{
								Function.Call(Hash.TASK_HELI_MISSION, MaverickPilot, MaverickPilot.CurrentVehicle, 0, 0, HeliWaypoint.X, HeliWaypoint.Y, HeliWaypoint.Z, 4, 50.0, 10.0, (HeliWaypoint - MaverickPilot.CurrentVehicle.Position).ToHeading(), 10, 10, 5f, 0);
							}
							else
							{
								Function.Call(Hash.TASK_HELI_MISSION, MaverickPilot, MaverickPilot.CurrentVehicle, 0, 0, HeliWaypoint.X, HeliWaypoint.Y, HeliWaypoint.Z, 4, 10.0, 10.0, (HeliWaypoint - MaverickPilot.CurrentVehicle.Position).ToHeading(), 10, 10, 5f, 0);
							}
						}
						if (MaverickHeli != null && GetDistance_VDIST2(MaverickHeli.Position, HelicopterRoute[CurrentHeliWaypoint], "<", 75f) && CurrentHeliWaypoint <= 2)
						{
							CurrentHeliWaypoint++;
							HeliWaypoint = HelicopterRoute[CurrentHeliWaypoint];
							if (CurrentHeliWaypoint <= 2)
							{
								Function.Call(Hash.TASK_HELI_MISSION, MaverickPilot, MaverickPilot.CurrentVehicle, 0, 0, HeliWaypoint.X, HeliWaypoint.Y, HeliWaypoint.Z, 4, 50.0, 10.0, MaverickPilot.CurrentVehicle.Heading, 10, 10, 5f, 0);
							}
							else
							{
								Function.Call(Hash.TASK_HELI_MISSION, MaverickPilot, MaverickPilot.CurrentVehicle, 0, 0, HeliWaypoint.X, HeliWaypoint.Y, HeliWaypoint.Z, 4, 10.0, 10.0, MaverickPilot.CurrentVehicle.Heading, 10, 10, 5f, 0);
							}
						}
						if (Game.Player.Character.CurrentVehicle != MaverickHeli)
						{
							GTA.UI.Screen.ShowSubtitle("Wait for the ~b~Maverick~w~ to reach the ~y~Construction Site~w~");
						}
						else if (RobberA != null)
						{
							if (RobberA.CurrentVehicle != MaverickHeli)
							{
								GTA.UI.Screen.ShowSubtitle("Wait for you ~y~RobberA~w~ to enter the ~b~Maverick~w~");
							}
							else
							{
								GTA.UI.Screen.ShowSubtitle("Wait for the ~b~Maverick~w~ to reach the ~y~Construction Site~w~");
							}
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Wait for the ~b~Maverick~w~ to reach the ~y~Construction Site~w~");
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.EnteringHeli)
					{
						if (Game.Player.Character.CurrentVehicle != MaverickHeli)
						{
							GTA.UI.Screen.ShowSubtitle("Enter the ~b~Maverick~w~");
						}
						else if (RobberA != null)
						{
							if (RobberA.CurrentVehicle != MaverickHeli)
							{
								GTA.UI.Screen.ShowSubtitle("Wait for you ~y~RobberA~w~ to enter the ~b~Maverick~w~");
							}
							else
							{
								GTA.UI.Screen.ShowSubtitle("Wait for the ~b~Maverick~w~ to reach the ~y~Construction Site~w~");
							}
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Wait for the ~b~Maverick~w~ to reach the ~y~Construction Site~w~");
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.EnteringAsBugstars && !GotBugstarVan)
					{
						if (!GotBugstarClothes)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ as Bugstars by finding ~g~Bugstar Clothes and a Bugstar Van~w~");
						}
						if (GotBugstarClothes)
						{
							if (WaringBugstarsOutfit)
							{
								if (Game.Player.Character.CurrentVehicle != BugstarsVan)
								{
									BugstarsVan.LockStatus = VehicleLockStatus.Unlocked;
									GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Enter the ~b~Bugstar Van~w~ to approach the ~y~Union Depository~w~ as Bugstars ");
								}
								else
								{
									OverrideDisguiseIfNotUnarmed = true;
									IsDressedUpAsBugstarsOrGroupe6 = true;
									GTA.UI.Screen.ShowSubtitle("Drive the ~b~Bugstar Van~w~ to the ~y~Union Depository~w~ as Bugstars, and enter through the ~y~Side or Back~w~ Enterance ");
								}
							}
							else
							{
								BugstarsVan.LockStatus = VehicleLockStatus.CannotEnter;
								GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Equip ~g~Bugstar Gear~w~ from the back of the  ~b~Bugstars Van~w~ to approach the ~y~Union Depository~w~ as Bugstars ");
							}
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.EnteringAsGrouppe6)
					{
						if (!GotGruppe6Clothes)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ as Gruppe6 by finding ~g~Gruppe6 Clothes and a Gruppe6 Van~w~");
						}
						if (GotGruppe6Clothes)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Stockade.Position, "<", 30f))
							{
								if (Game.Player.Character.CurrentVehicle != Gruppe6Stockade)
								{
									GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Enter the ~b~Stockade~w~ to approach the ~y~Union Depository~w~ as Gruppe6 ");
								}
								else
								{
									OverrideDisguiseIfAimingGun = true;
									IsDressedUpAsBugstarsOrGroupe6 = true;
									GTA.UI.Screen.ShowSubtitle("Drive the ~b~Stockade~w~ to the ~y~Union Depository~w~ as Gruppe6, and enter through the ~y~Side or Back~w~ Enterance ");
								}
							}
							else if (WaringGruppe6Outfit)
							{
								Gruppe6Stockade.LockStatus = VehicleLockStatus.Unlocked;
								GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Locate ~b~Stockade~w~ to approach the ~y~Union Depository~w~ as Gruppe6 ");
							}
							else
							{
								Gruppe6Stockade.LockStatus = VehicleLockStatus.CannotEnter;
								GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Equip ~g~Gruppe 6 Gear~w~ from the back of the  ~b~Stockade~w~ to approach the ~y~Union Depository~w~ as Gruppe6 ");
							}
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.EnteringAsGrouppe6ORBugstars)
					{
						if (Game.Player.Character.CurrentVehicle != null)
						{
							if (Game.Player.Character.CurrentVehicle != BugstarsVan || Game.Player.Character.CurrentVehicle != Gruppe6Stockade)
							{
								GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ here as ~g~Bugstars/Gruppe6~w~");
							}
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ here as ~g~Bugstars/Gruppe6~w~");
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.EnteringViaCutter)
					{
						if (!GotCutterKeys)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ using a ~b~Cutter~w~, by finding the ~g~Cutter's keys~w~ located somewhere in this constructions site");
						}
						if (GotCutterKeys)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, CutterVeh.Position, "<", 30f))
							{
								GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Enter the ~b~Cutter~w~ to drill out a hole in the ~y~Union Depository's~w~ Vault ");
							}
							else
							{
								GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Locate ~b~Cutter~w~ to approach the ~y~Union Depository~w~ with the Cutter ");
							}
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.ExitingViaGrateAndHeli)
					{
						if (!GotC4Crate)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ here by blowing the Grate with ~g~C4~w~ before hand and using a ~b~Maverick~w~ ");
						}
						if (!StartedPlacingBombs && !BlownUpGrate)
						{
							StartedPlacingBombs = true;
							List<Vector3> list10 = new List<Vector3>();
							list10.Add(new Vector3(23.08f, -626.28f, 30.3f));
							list10.Add(new Vector3(31.61f, -629.04f, 30.3f));
							list10.Add(new Vector3(19.73f, -639.6f, 30.3f));
							list10.Add(new Vector3(26.69f, -642.51f, 30.3f));
							foreach (Vector3 item34 in list10)
							{
								Blip blip = GlobalHelper.CreateBlip(item34);
								blip.Color = BlipColor.Yellow;
								blip.Name = "Place Bomb";
								blip.IsShortRange = true;
								BombPlacementAreaBlip.Add(blip);
							}
						}
						if (!BlownUpGrate)
						{
							if (BombsPlaced != 4)
							{
								GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Place a ~g~C4~w~ on each marked ~y~Location~w~ to blow open the grate");
							}
							if (BombsPlaced >= 4)
							{
								if (!BlownUpGrate)
								{
									GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ move to the marked ~y~Location~w~ to blow open the grate");
									if (Missionblip2 != null && GetDistance_VDIST2(Game.Player.Character.Position, Missionblip2.Position, "<", 10f))
									{
										if (Missionblip2 != null)
										{
											Missionblip2.Delete();
										}
										AllBombsPlaced = false;
										foreach (Prop grateBomb in GrateBombs)
										{
											Script.Wait(1000);
											World.AddExplosion(grateBomb.Position, ExplosionType.Tanker, 15f, 1f);
											grateBomb.Opacity = 0;
										}
										Function.Call(Hash.REMOVE_IPL, "dt1_03_gr_closed");
										BlownUpGrate = true;
										if (!SpawnedSearchParty)
										{
											foreach (Ped teller in Tellers)
											{
												if (teller != null)
												{
													teller.Delete();
												}
											}
											if (StockadeB != null)
											{
												StockadeB.Delete();
											}
											if (StockadeA != null)
											{
												StockadeA.Delete();
											}
											if (StockadeC != null)
											{
												StockadeC.Delete();
											}
											StockadeB = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), new Vector3(1144.92f, -832.76f, 53.5f), 91f);
											StockadeB.IsPersistent = true;
											StockadeB.AddBlip();
											StockadeB.AttachedBlip.Sprite = BlipSprite.Truck;
											StockadeB.AttachedBlip.Color = BlipColor.Red;
											StockadeB.AttachedBlip.Name = "Gruppe6 Search Party";
											StockadeB.AttachedBlip.IsShortRange = true;
											Ped ped7 = GlobalHelper.CreatePed(RequestModel(PedHash.Armoured02SMM), StockadeB.Position);
											Tellers.Add(ped7);
											ped7.SetIntoVehicle(StockadeB, VehicleSeat.Driver);
											ped7 = GlobalHelper.CreatePed(RequestModel(PedHash.Armoured02SMM), StockadeB.Position);
											Tellers.Add(ped7);
											ped7.SetIntoVehicle(StockadeB, VehicleSeat.Passenger);
											StockadeB.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeB, new Vector3(55.77f, -622.93f, 30.64f), 20f, 100f);
											StockadeA = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), StockadeB.GetOffsetPosition(new Vector3(0f, -6f, 0f)), 91f);
											StockadeA.IsPersistent = true;
											StockadeA.AddBlip();
											StockadeA.AttachedBlip.Sprite = BlipSprite.Truck;
											StockadeA.AttachedBlip.Color = BlipColor.Red;
											StockadeA.AttachedBlip.Name = "Gruppe6 Search Party";
											StockadeA.AttachedBlip.IsShortRange = true;
											ped7 = GlobalHelper.CreatePed(RequestModel(PedHash.Armoured02SMM), StockadeA.Position);
											Tellers.Add(ped7);
											ped7.SetIntoVehicle(StockadeA, VehicleSeat.Driver);
											ped7 = GlobalHelper.CreatePed(RequestModel(PedHash.Armoured02SMM), StockadeA.Position);
											Tellers.Add(ped7);
											ped7.SetIntoVehicle(StockadeA, VehicleSeat.Passenger);
											StockadeA.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeA, new Vector3(55.77f, -622.93f, 30.64f), 20f, 100f);
											StockadeC = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), StockadeB.GetOffsetPosition(new Vector3(0f, 6f, 0f)), 91f);
											StockadeC.IsPersistent = true;
											StockadeC.AddBlip();
											StockadeC.AttachedBlip.Sprite = BlipSprite.Truck;
											StockadeC.AttachedBlip.Color = BlipColor.Red;
											StockadeC.AttachedBlip.Name = "Gruppe6 Search Party";
											StockadeC.AttachedBlip.IsShortRange = true;
											ped7 = GlobalHelper.CreatePed(RequestModel(PedHash.Armoured02SMM), StockadeC.Position);
											Tellers.Add(ped7);
											ped7.SetIntoVehicle(StockadeC, VehicleSeat.Driver);
											ped7 = GlobalHelper.CreatePed(RequestModel(PedHash.Armoured02SMM), StockadeC.Position);
											Tellers.Add(ped7);
											ped7.SetIntoVehicle(StockadeC, VehicleSeat.Passenger);
											StockadeC.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeC, new Vector3(55.77f, -622.93f, 30.64f), 20f, 100f);
											TextNotification("CHAR_HUMANDEFAULT", "~y~Union Depository Heist~w~", "~r~Search Party Dispatched", "Gruppe6 have dispached a search party to investigate the explosions");
											SpawnedSearchParty = true;
											SearchPartyDetectDelay = Game.GameTime + 1000;
										}
									}
								}
								else
								{
									GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can now enter the ~y~Union Depository~w~ here by using a ~b~Maverick~w~ ");
								}
							}
							if (BombsPlaced >= 4 && !CreatedSafeAreaMarker)
							{
								AllBombsPlaced = true;
								CreatedSafeAreaMarker = true;
								if (Missionblip2 != null)
								{
									Missionblip2.Delete();
								}
								Missionblip2 = GlobalHelper.CreateBlip(new Vector3(42.3f, -595.9f, 31.6f));
								Missionblip2.Sprite = BlipSprite.Standard;
								Missionblip2.Color = BlipColor.Yellow;
							}
							for (int num48 = 0; num48 < BombPlacementAreaBlip.Count; num48++)
							{
								if (GetDistance_VDIST2(BombPlacementAreaBlip[num48].Position, Game.Player.Character.Position, "<", 2.5f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To place bomb");
									if (Game.IsControlJustPressed(Control.Context))
									{
										BombPlacementAreaBlip[num48].Alpha = 0;
										BombsPlaced++;
										LoadDict("weapons@projectile@sticky_bomb");
										LoadDict("weapons@projectile@sticky_bomb");
										PlaceBombScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, 0, 0, Game.Player.Character.Rotation.Z, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, PlaceBombScene, "weapons@projectile@sticky_bomb", "plant_floor", 0.0, 0.0, 0, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, PlaceBombScene, "plant_floor", LoadDict("weapons@projectile@sticky_bomb"), 1000f, 0f, 0, 1000f);
										PlacingBombOnGrate = true;
									}
								}
							}
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.FrontEnterance && BugstarsVan != null && Gruppe6Stockade != null && BugstarsVan.Driver != Game.Player.Character && Gruppe6Stockade.Driver != Game.Player.Character)
					{
						GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ at the Front Enterance with no additional Assets");
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.ExitingViaHeli && !GotHiredHeli)
					{
						if (!BlownUpGrate)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can Enter the ~y~Union Depository~w~ with a ~g~Maverick~w~ when the Grate has been destroyed");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Hire the ~g~Maverick~w~ and its pilot to Enter the ~y~Union Depository~w~ by Rappelling");
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.NearCutterTooltip)
					{
						if (!GotCutterKeys)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ with this ~b~Cutter~w~ by finding its ~g~Keys~w~");
						}
						else if (GetDistance_VDIST2(Game.Player.Character.Position, CutterVeh.Position, "<", 30f))
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Enter the ~b~Cutter~w~ to drill out a hole in the ~y~Union Depository's~w~ Vault ");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Locate ~b~Cutter~w~ to approach the ~y~Union Depository~w~ with the Cutter ");
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.NearGruppe6Clothes)
					{
						if (!GotGruppe6Clothes)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can enter the ~y~Union Depository~w~ as Gruppe6 by finding ~g~Gruppe6 Clothes and a Gruppe6 Van~w~");
						}
						if (GotGruppe6Clothes)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, Gruppe6Stockade.Position, "<", 30f))
							{
								if (Game.Player.Character.CurrentVehicle != Gruppe6Stockade)
								{
									GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Enter the ~b~Stockade~w~ to approach the ~y~Union Depository~w~ as Gruppe6 ");
								}
								else
								{
									OverrideDisguiseIfAimingGun = true;
									IsDressedUpAsBugstarsOrGroupe6 = true;
									GTA.UI.Screen.ShowSubtitle("Drive the ~b~Stockade~w~ to the ~y~Union Depository~w~ as Gruppe6, and enter through the ~y~Side or Back~w~ Enterance ");
								}
							}
							else
							{
								GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Locate ~b~Stockade~w~ to approach the ~y~Union Depository~w~ as Gruppe6 ");
							}
						}
					}
					if (CurrentUnionApproachToottip == ApproachToolTip.ExitingViaC4)
					{
						if (!GotC4Crate)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ Find the ~g~C4 Crate~w~ on the cargo ship to exit the ~y~Union Depository~w~ via the back wall of the Vault or to use a ~b~Maverick~w~ to enter via the Grate");
						}
						if (GotC4Crate)
						{
							GTA.UI.Screen.ShowSubtitle("~b~TIP:~w~ You can use the ~g~C4~w~ to blow up the ~y~Union Depository's~w~ outer wall inside the vault or blow up the Grate to allow a Rappeling from a ~b~Maverick~w~");
						}
					}
				}
				if (Game.Player.IsAlive)
				{
					foreach (Ped guard3 in Guards)
					{
						if (!(guard3 != null))
						{
							continue;
						}
						if (guard3.IsAlive)
						{
							GetDistance_VDIST2(Game.Player.Character.Position, guard3.Position, "<=", 30f);
							if (GetDistance_VDIST2(Game.Player.Character.Position, guard3.Position, "<=", 30f))
							{
								if (BrokenDisguise && GetDistance_VDIST2(Game.Player.Character.Position, guard3.Position, "<=", 10f) && guard3.IsInCombatAgainst(Game.Player.Character))
								{
									guard3.Task.FightAgainst(Game.Player.Character);
								}
								float num49 = guard3.Position.Z - 5f;
								float num50 = guard3.Position.Z + 5f;
								float z5 = Game.Player.Character.Position.Z;
								if (z5 > num49 && z5 < num50)
								{
									if (guard3.AttachedBlip != null)
									{
										guard3.AttachedBlip.Scale = 0.5f;
										guard3.AttachedBlip.Alpha = 255;
									}
								}
								else if (guard3.AttachedBlip != null)
								{
									guard3.AttachedBlip.Alpha = 0;
								}
							}
							else if (guard3.AttachedBlip != null)
							{
								guard3.AttachedBlip.Alpha = 0;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, guard3.Position, "<=", 30f) && Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, guard3, Game.Player.Character, 17))
							{
								foreach (Ped guard4 in Guards)
								{
									if (!(guard4 != null) || !GetDistance_VDIST2(guard3.Position, guard4.Position, "<=", 10f))
									{
										continue;
									}
									if ((!WaringBugstarsOutfit && !WaringGruppe6Outfit) || BrokenDisguise)
									{
										RelationshipGroup relationshipGroup2 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
										Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, guard4, relationshipGroup2);
										Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, guard4, 0, 0);
										Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, guard4, 1);
										guard4.Task.FightAgainst(Game.Player.Character);
									}
									else if (WaringBugstarsOutfit)
									{
										if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && ParkedAtParkingSpot && Game.GameTime > WeaponsDelay)
										{
											BrokenDisguise = true;
										}
									}
									else if (WaringGruppe6Outfit && Game.Player.IsTargeting(guard4) && ParkedAtParkingSpot && Game.GameTime > WeaponsDelay)
									{
										BrokenDisguise = true;
									}
								}
							}
							if (Game.Player.IsTargeting(guard3))
							{
								if (WaringBugstarsOutfit || WaringGruppe6Outfit)
								{
									BrokenDisguise = true;
								}
								foreach (Ped guard5 in Guards)
								{
									if (guard5 != null)
									{
										RelationshipGroup relationshipGroup3 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
										Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, guard5, relationshipGroup3);
										Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, guard5, 0, 0);
										Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, guard5, 1);
										guard5.Task.FightAgainst(Game.Player.Character);
									}
								}
							}
							if (!Game.Player.Character.IsShooting)
							{
								continue;
							}
							if (WaringBugstarsOutfit || WaringGruppe6Outfit)
							{
								BrokenDisguise = true;
							}
							foreach (Ped guard6 in Guards)
							{
								if (guard6 != null)
								{
									RelationshipGroup relationshipGroup4 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
									Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, guard6, relationshipGroup4);
									Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, guard6, 0, 0);
									Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, guard6, 1);
									guard6.Task.FightAgainst(Game.Player.Character);
								}
							}
						}
						else if (guard3.AttachedBlip != null)
						{
							guard3.AttachedBlip.Alpha = 0;
						}
					}
					if (Game.Player.WantedLevel >= 2)
					{
						if (P != null)
						{
							Prop prop23 = P;
							if (prop23.Position.Z <= 40f)
							{
								prop23.Position = new Vector3(prop23.Position.X, prop23.Position.Y, prop23.Position.Z + 0.02f);
							}
						}
						if (P2 != null)
						{
							Prop p = P2;
							if (p.Position.Z <= 40f)
							{
								p.Position = new Vector3(p.Position.X, p.Position.Y, p.Position.Z + 0.02f);
							}
						}
					}
					if (Bank != null && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<=", 300f) && !CreateProps)
					{
						CreateProps = true;
						foreach (Prop goldTrayProp in GoldTrayProps)
						{
							if (goldTrayProp != null)
							{
								goldTrayProp.Delete();
							}
						}
						if (P != null)
						{
							P.Delete();
						}
						if (P2 != null)
						{
							P2.Delete();
						}
						P = GlobalHelper.CreateProp("prop_sec_gate_01d", new Vector3(-73.0274f, -682f, 32.5f), new Vector3(0f, 0f, -110f), false, false);
						P2 = GlobalHelper.CreateProp("prop_sec_gate_01d", new Vector3(24.867f, -664.5428f, 30.5f), new Vector3(0f, 0f, 160f), false, false);
						P.IsPositionFrozen = true;
						P2.IsPositionFrozen = true;
						AddCrates_UnionDP();
						if (Van1 != null)
						{
							Van1.Delete();
						}
						if (Van2 != null)
						{
							Van2.Delete();
						}
						if (Van3 != null)
						{
							Van3.Delete();
						}
						Van1 = GlobalHelper.CreateVehicle(VehicleHash.Stockade, new Vector3(-2.535118f, -707.8532f, 31f), 154f);
						Van2 = GlobalHelper.CreateVehicle(VehicleHash.Stockade, new Vector3(-34.50718f, -699.7496f, 31f), 158f);
						Van3 = GlobalHelper.CreateVehicle(VehicleHash.Stockade, new Vector3(-19.07012f, -703.184f, 31f), 155f);
						Van1.CreatePedOnSeat(VehicleSeat.LeftRear, RequestModel(PedHash.Armoured01SMM));
						Van1.GetPedOnSeat(VehicleSeat.LeftRear).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
						Guards.Add(Van1.GetPedOnSeat(VehicleSeat.LeftRear));
						Van1.CreatePedOnSeat(VehicleSeat.RightRear, RequestModel(PedHash.Armoured01SMM));
						Van1.GetPedOnSeat(VehicleSeat.RightRear).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
						Guards.Add(Van1.GetPedOnSeat(VehicleSeat.RightRear));
						Van2.CreatePedOnSeat(VehicleSeat.LeftRear, RequestModel(PedHash.Armoured01SMM));
						Van2.GetPedOnSeat(VehicleSeat.LeftRear).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
						Guards.Add(Van2.GetPedOnSeat(VehicleSeat.LeftRear));
						Van2.CreatePedOnSeat(VehicleSeat.RightRear, RequestModel(PedHash.Armoured01SMM));
						Van2.GetPedOnSeat(VehicleSeat.RightRear).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
						Guards.Add(Van2.GetPedOnSeat(VehicleSeat.RightRear));
						Van3.CreatePedOnSeat(VehicleSeat.LeftRear, RequestModel(PedHash.Armoured01SMM));
						Van3.GetPedOnSeat(VehicleSeat.LeftRear).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
						Guards.Add(Van3.GetPedOnSeat(VehicleSeat.LeftRear));
						Van3.CreatePedOnSeat(VehicleSeat.RightRear, RequestModel(PedHash.Armoured01SMM));
						Van3.GetPedOnSeat(VehicleSeat.RightRear).Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
						Guards.Add(Van3.GetPedOnSeat(VehicleSeat.RightRear));
						DeletedElv2 = false;
						DeletedElv1 = false;
						foreach (Ped guard7 in Guards)
						{
							if (guard7 != null)
							{
								SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(guard7, SmartGuards.EnemyGroup.Security));
							}
						}
					}
					if (BugstarsVan != null && Gruppe6Stockade != null)
					{
						if (!BugstarsVan.Exists())
						{
							BugstarsVan = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Burrito2), BugstarsVan_LastPosition, BugstarsVan_LastRotation.Z);
							BugstarsVan.AddBlip();
							BugstarsVan.AttachedBlip.Color = BlipColor.Blue;
							BugstarsVan.AttachedBlip.Name = "Bugstars Van for Union Depository";
							BugstarsVan.AttachedBlip.IsShortRange = true;
							BugstarsVan.IsPersistent = true;
						}
						if (!Gruppe6Stockade.Exists())
						{
							Gruppe6Stockade = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), Gruppe6Stockade_LastPosition, Gruppe6Stockade_LastRotation.Z);
							Gruppe6Stockade.AddBlip();
							Gruppe6Stockade.AttachedBlip.Color = BlipColor.Blue;
							Gruppe6Stockade.AttachedBlip.Name = "Stockade for Union Depository";
							Gruppe6Stockade.AttachedBlip.IsShortRange = true;
							Gruppe6Stockade.IsPersistent = true;
						}
						if (BugstarsVan != null && P != null && P2 != null)
						{
							if ((GetDistance_VDIST2(BugstarsVan.Position, P.Position, "<", 15f) || HasBeenInUnionVaultFloor) && P != null)
							{
								Prop prop24 = P;
								if (GetDistance_VDIST2(Game.Player.Character.Position, prop24.Position, ">", 12f) && prop24.Position.Z >= 34.5f)
								{
									prop24.Position = new Vector3(prop24.Position.X, prop24.Position.Y, prop24.Position.Z - 0.01f);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, prop24.Position, "<", 12f) && prop24.Position.Z <= 40f)
								{
									prop24.Position = new Vector3(prop24.Position.X, prop24.Position.Y, prop24.Position.Z + 0.01f);
								}
							}
							if (GetDistance_VDIST2(Gruppe6Stockade.Position, P2.Position, "<", 15f) && P2 != null)
							{
								Prop p2 = P2;
								if (GetDistance_VDIST2(Game.Player.Character.Position, p2.Position, ">", 12f) && p2.Position.Z >= 32.5f)
								{
									p2.Position = new Vector3(p2.Position.X, p2.Position.Y, p2.Position.Z - 0.01f);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, p2.Position, "<", 12f) && p2.Position.Z <= 40f)
								{
									p2.Position = new Vector3(p2.Position.X, p2.Position.Y, p2.Position.Z + 0.01f);
								}
							}
						}
						if (Gruppe6Stockade != null && P != null && P2 != null)
						{
							if ((GetDistance_VDIST2(Gruppe6Stockade.Position, P.Position, "<", 15f) || HasBeenInUnionVaultFloor) && P != null)
							{
								Prop prop25 = P;
								if (GetDistance_VDIST2(Game.Player.Character.Position, prop25.Position, ">", 12f) && prop25.Position.Z >= 34.5f)
								{
									prop25.Position = new Vector3(prop25.Position.X, prop25.Position.Y, prop25.Position.Z - 0.01f);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, prop25.Position, "<", 12f) && prop25.Position.Z <= 40f)
								{
									prop25.Position = new Vector3(prop25.Position.X, prop25.Position.Y, prop25.Position.Z + 0.01f);
								}
							}
							if (GetDistance_VDIST2(Gruppe6Stockade.Position, P2.Position, "<", 15f) && P2 != null)
							{
								Prop p3 = P2;
								if (GetDistance_VDIST2(Game.Player.Character.Position, p3.Position, ">", 12f) && p3.Position.Z >= 32.5f)
								{
									p3.Position = new Vector3(p3.Position.X, p3.Position.Y, p3.Position.Z - 0.01f);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, p3.Position, "<", 12f) && p3.Position.Z <= 40f)
								{
									p3.Position = new Vector3(p3.Position.X, p3.Position.Y, p3.Position.Z + 0.01f);
								}
							}
						}
					}
					if (BugstarsVan != null && Gruppe6Stockade != null)
					{
						if (!BugstarsVan.Exists())
						{
							BugstarsVan = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Burrito2), BugstarsVan_LastPosition, BugstarsVan_LastRotation.Z);
							BugstarsVan.AddBlip();
							BugstarsVan.AttachedBlip.Color = BlipColor.Blue;
							BugstarsVan.AttachedBlip.Name = "Bugstars Van for Union Depository";
							BugstarsVan.AttachedBlip.IsShortRange = true;
							BugstarsVan.IsPersistent = true;
						}
						if (!Gruppe6Stockade.Exists())
						{
							Gruppe6Stockade = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), Gruppe6Stockade_LastPosition, Gruppe6Stockade_LastRotation.Z);
							Gruppe6Stockade.AddBlip();
							Gruppe6Stockade.AttachedBlip.Color = BlipColor.Blue;
							Gruppe6Stockade.AttachedBlip.Name = "Stockade for Union Depository";
							Gruppe6Stockade.AttachedBlip.IsShortRange = true;
							Gruppe6Stockade.IsPersistent = true;
						}
					}
					if (placedJammer)
					{
						InHeist = true;
						if (!DeletedElv1 && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(10.2052f, -668.2f, 33f), "<", 10f))
						{
							DeletedElv1 = true;
							Prop[] nearbyProps14 = World.GetNearbyProps(new Vector3(10.2052f, -668.2f, 33f), 4f);
							if (nearbyProps14.Length != 0)
							{
								Prop[] array19 = nearbyProps14;
								foreach (Prop prop26 in array19)
								{
									if (prop26 != null)
									{
										prop26.Delete();
									}
								}
							}
						}
						if (!DeletedElv2 && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(0.7295f, -703.8784f, 16f), "<", 10f))
						{
							DeletedElv2 = true;
							Prop[] nearbyProps15 = World.GetNearbyProps(new Vector3(0.7295f, -703.8784f, 16f), 4f);
							if (nearbyProps15.Length != 0)
							{
								Prop[] array20 = nearbyProps15;
								foreach (Prop prop27 in array20)
								{
									if (prop27 != null)
									{
										prop27.Delete();
									}
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 30f))
						{
							Prop[] nearbyProps16 = World.GetNearbyProps(new Vector3(-1.880968f, -687.0798f, 16f), 1f);
							if (nearbyProps16.Length != 0)
							{
								Prop[] array21 = nearbyProps16;
								foreach (Prop prop28 in array21)
								{
									if (prop28 != null)
									{
										prop28.Rotation = new Vector3(0f, 0f, 160f);
									}
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 6f))
						{
							if (!BrokenDisguise || (!WaringBugstarsOutfit && !WaringGruppe6Outfit && !CutterDrilledOpenVault))
							{
								Prop[] nearbyProps17 = World.GetNearbyProps(new Vector3(-1.880968f, -687.0798f, 16f), 1f);
								if (nearbyProps17.Length != 0)
								{
									Prop[] array22 = nearbyProps17;
									foreach (Prop prop29 in array22)
									{
										if (prop29 != null)
										{
											VaultD = prop29;
										}
									}
								}
								placedJammer = false;
								if (hackedUnionVaultDoor)
								{
									Waittime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 25000, 60000);
									Script.Wait(200);
									VaultUnlocked = false;
									TriggerVault = true;
									Random random8 = new Random();
								}
							}
							else
							{
								VaultUnlocked = false;
								TriggerVault = true;
							}
						}
					}
					if (VaultOpen)
					{
						Game.Player.WantedLevel = 5;
						if (ManageRobberyAnims.CurrentPay < MaxPay)
						{
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
							UI.DrawPayment("TAKE", LocalTake);
							if (SwatTimer != 120)
							{
								SwatTimer++;
							}
							if (SwatTimer == 120)
							{
								SwatTimer = 0;
								Random random9 = new Random();
								int num55 = random9.Next(1, 100);
								int num56 = random9.Next(1, 100);
								if (num55 < 20)
								{
									for (int num57 = 0; num57 < 3; num57++)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 400f) && (!BrokenDisguise || (!WaringBugstarsOutfit && !WaringGruppe6Outfit)))
										{
											Ped ped8 = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(-14.5376f, -694.226f, 16f), 24f);
											RelationshipGroup relationshipGroup5 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
											Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped8, relationshipGroup5);
											Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped8, 0, 0);
											Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped8, 1);
											ped8.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
											ped8.Task.FightAgainst(Game.Player.Character);
											SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped8, SmartGuards.EnemyGroup.Security));
											Guards.Add(ped8);
											Notification.Show("~y~ Lester ~w~ : Grouppe 6 have burst through the doors in an attempt to stop you");
										}
									}
								}
								if (num56 < 20 && num55 > 20)
								{
									for (int num58 = 0; num58 < 3; num58++)
									{
										if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 300f) && (!BrokenDisguise || (!WaringBugstarsOutfit && !WaringGruppe6Outfit)))
										{
											Ped ped9 = GlobalHelper.CreatePed(PedHash.Armoured02SMM, new Vector3(6.26812f, -708.677f, 16f), 24f);
											RelationshipGroup relationshipGroup6 = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER");
											Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped9, relationshipGroup6);
											Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped9, 0, 0);
											Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped9, 1);
											ped9.Weapons.Give(WeaponHash.APPistol, 9999, true, true);
											ped9.Task.FightAgainst(Game.Player.Character);
											SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped9, SmartGuards.EnemyGroup.Security));
											Guards.Add(ped9);
											Notification.Show("~y~ Lester ~w~ : Grouppe 6 have burst through the doors in an attempt to stop you");
										}
									}
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryMainMarker, ">", 300f))
						{
							TriggerVault = false;
							DefendingCops = false;
							VaultOpen = false;
							escapingcops = true;
							Bank.ShowRoute = false;
						}
						else if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryMainMarker, "<", 300f) && Game.GameTime >= Waittime)
						{
							Game.Player.WantedLevel = 5;
						}
					}
					if (escapingcops)
					{
						if (!CreateSecondaryCops && (!BrokenDisguise || (!WaringBugstarsOutfit && !WaringGruppe6Outfit)))
						{
							CreateSecondaryCops = true;
							foreach (Prop spike in Spikes)
							{
								if (spike != null)
								{
									spike.Delete();
								}
							}
							foreach (Vehicle item35 in Veh)
							{
								if (item35 != null)
								{
									item35.Delete();
								}
							}
							foreach (Ped ped30 in Peds)
							{
								if (ped30 != null)
								{
									ped30.Delete();
								}
							}
							Vehicle item5 = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(-64.5519f, -539.9363f, 31f), 90f);
							Veh.Add(item5);
							Ped item6 = GlobalHelper.CreatePed(PedHash.Cop01SFY, new Vector3(-58.829f, -539.9288f, 31f), -71f);
							Peds.Add(item6);
							item5 = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(-1619.488f, -707.8462f, 10f), 66f);
							Veh.Add(item5);
							item6 = GlobalHelper.CreatePed(PedHash.Cop01SFY, new Vector3(-1623.639f, -706.4884f, 11f), 47f);
							Peds.Add(item6);
							item5 = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(414.8151f, -542.3892f, 28f), -37f);
							Veh.Add(item5);
							item6 = GlobalHelper.CreatePed(PedHash.Cop01SFY, new Vector3(412.0677f, -546.7636f, 28f), 160.2405f);
							Peds.Add(item6);
							item5 = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(927.0449f, -841.4202f, 43f), 24f);
							Veh.Add(item5);
							item6 = GlobalHelper.CreatePed(PedHash.Cop01SFY, new Vector3(924.7476f, -841.9084f, 43f), 88f);
							Peds.Add(item6);
							item5 = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(1628.882f, -960.575f, 62f), -146f);
							Veh.Add(item5);
							item6 = GlobalHelper.CreatePed(PedHash.Cop01SFY, new Vector3(1626.012f, -953.5488f, 62f), 117f);
							Peds.Add(item6);
							item5 = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(-2190.728f, -332.1729f, 12f), 111f);
							Veh.Add(item5);
							item6 = GlobalHelper.CreatePed(PedHash.Cop01SFY, new Vector3(-2189.011f, -334.2067f, 13f), -164f);
							Peds.Add(item6);
							for (int num59 = 0; num59 < 51; num59++)
							{
								Random random10 = new Random();
								int num60 = random10.Next(250, 3000);
								item5 = GlobalHelper.CreateVehicle(VehicleHash.Police4, UnionDepositoryMainMarker.Around(num60));
								item5.PlaceOnNextStreet();
								Veh.Add(item5);
								item6 = GlobalHelper.CreatePed(PedHash.Cop01SFY, item5.Position, -164f);
								item6.SetIntoVehicle(item5, VehicleSeat.Driver);
								item6.Task.DriveTo(item5, item5.Position.Around(500f), 30f, 80f);
								Peds.Add(item6);
							}
							foreach (Vehicle item36 in Veh)
							{
								bool flag20 = item36 != null;
							}
							foreach (Ped ped31 in Peds)
							{
								if (ped31 != null)
								{
									ped31.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
									ped31.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryMainMarker, "<", 300f))
						{
							Ped[] nearbyPeds2 = World.GetNearbyPeds(Game.Player.Character.Position, 2000f);
							if (nearbyPeds2.Length != 0)
							{
								Ped[] array23 = nearbyPeds2;
								foreach (Ped ped10 in array23)
								{
									if (ped10 != null)
									{
										ped10.Opacity = 255;
									}
								}
							}
							Game.Player.WantedLevel = 5;
						}
						if (Game.Player.WantedLevel == 0)
						{
							escapingcops = false;
							HeadingtoArcade = true;
						}
					}
					if (HasBeenInUnionVaultFloor && ManageRobberyAnims.CurrentPay >= 100000 && InHeist)
					{
						Bank.ShowRoute = false;
						if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryMainMarker, ">", 200f) && Game.Player.WantedLevel == 0)
						{
							PlayingCutscene = false;
							HeadingtoArcade = false;
							float num62 = ManageRobberyAnims.CurrentPay;
							float num63 = (num62 -= TotalCost);
							float totalCost2 = TotalCost;
							Script.Wait(10);
							totalCost2 = num63 / 10f;
							InHouseRobbery = false;
							Main.TotalCrewEarned += totalCost2;
							Main.TotalPlayerEarned += num63;
							XP.CompleteHeist_UnionDepository++;
							Main.SaveCuts();
							PlayingUnion = false;
							SetNoLongerInHeist();
							MissionScreen.SetPass("The Union Depository", (int)num63, (int)totalCost2, "The Player has died", 2, HTime, 5);
							int num64 = Game.GameTime + 30000;
							while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num64)
							{
								Script.Wait(0);
							}
							Script.Wait(2000);
							XP.AddXP_Percent(XP.CompleteHeist_XPPercent_UnionDepository);
							ManageRobberyAnims.CurrentPay = 0;
							HeistTime = 0f;
							PlayingUnion = false;
							DeleteUnionNewAssets();
							GTA.UI.Screen.FadeOut(300);
							Script.Wait(300);
							foreach (Prop spike2 in Spikes)
							{
								if (spike2 != null)
								{
									spike2.Delete();
								}
							}
							foreach (Vehicle item37 in Veh)
							{
								if (item37 != null)
								{
									item37.Delete();
								}
							}
							foreach (Ped ped32 in Peds)
							{
								if (ped32 != null)
								{
									ped32.Delete();
								}
							}
							if (Bank != null)
							{
								Bank.Delete();
							}
							if (GetawayCar != null)
							{
								GetawayCar.MarkAsNoLongerNeeded();
							}
							if (Insurgent != null)
							{
								Insurgent.Delete();
							}
							if (Kuruma != null)
							{
								Kuruma.Delete();
							}
							if (InsurgentBLip != null)
							{
								InsurgentBLip.Delete();
							}
							if (KurumaBlip != null)
							{
								KurumaBlip.Delete();
							}
							if (GetawayCar != null)
							{
								GetawayCar.Delete();
							}
							foreach (Ped guard8 in Guards)
							{
								if (guard8 != null)
								{
									guard8.Delete();
								}
							}
							Script.Wait(300);
							GTA.UI.Screen.FadeIn(300);
							foreach (Ped ped33 in Peds)
							{
								if (ped33 != null)
								{
									ped33.Delete();
								}
							}
							if (Bank != null)
							{
								Bank.Delete();
							}
							PlayingUnion = false;
							DeleteUnionNewAssets();
							Random random11 = new Random();
							TriggerVault = false;
							DefendingCops = false;
							VaultOpen = false;
							escapingcops = true;
							Bank.ShowRoute = false;
							TriggerVault = false;
							DefendingCops = false;
							VaultOpen = false;
							VaultUnlocked = false;
							InHeist = false;
							ManagePlayerFunds.AddFunds((int)num63);
							HeadingtoArcade = false;
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Notification.Show("~b~ Heist Complete ~w~");
							Notification.Show("~b~ Player Cut ~w~ : $" + num63.ToString("N"));
							Notification.Show("~b~ RobberA Cut ~w~ : $" + TotalCost.ToString("N"));
							MissionOn = false;
							ManageRobberyAnims.CurrentPay = 0;
							LocalTake = 0;
							if (CreateUnionDepositoryBlocker)
							{
								if (P != null)
								{
									P.Delete();
								}
								if (P2 != null)
								{
									P2.Delete();
								}
								P = GlobalHelper.CreateProp(RequestModel("prop_sec_gate_01d"), new Vector3(-73.0274f, -682f, 32.5f), new Vector3(0f, 0f, -110f), true, false);
								P2 = GlobalHelper.CreateProp(RequestModel("prop_sec_gate_01d"), new Vector3(24.867f, -664.5428f, 30.5f), new Vector3(0f, 0f, 160f), true, false);
								P.IsPositionFrozen = true;
								P2.IsPositionFrozen = true;
							}
						}
					}
				}
				if (Game.Player.IsAlive || !InHeist || !InHeist)
				{
					break;
				}
				if (CreateUnionDepositoryBlocker)
				{
					if (P != null)
					{
						P.Delete();
					}
					if (P2 != null)
					{
						P2.Delete();
					}
					P = GlobalHelper.CreateProp(RequestModel("prop_sec_gate_01d"), new Vector3(-73.0274f, -682f, 32.5f), new Vector3(0f, 0f, -110f), true, false);
					P2 = GlobalHelper.CreateProp(RequestModel("prop_sec_gate_01d"), new Vector3(24.867f, -664.5428f, 30.5f), new Vector3(0f, 0f, 160f), true, false);
					P.IsPositionFrozen = true;
					P2.IsPositionFrozen = true;
				}
				PlayingUnion = false;
				DeleteUnionNewAssets();
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
				HeadingtoArcade = false;
				InHeist = false;
				GTA.UI.Screen.FadeOut(300);
				Script.Wait(300);
				if (Bank != null)
				{
					Bank.Delete();
				}
				foreach (Prop spike3 in Spikes)
				{
					if (spike3 != null)
					{
						spike3.Delete();
					}
				}
				foreach (Vehicle item38 in Veh)
				{
					if (item38 != null)
					{
						item38.Delete();
					}
				}
				foreach (Ped ped34 in Peds)
				{
					if (ped34 != null)
					{
						ped34.Delete();
					}
				}
				if (GetawayCar != null)
				{
					GetawayCar.Delete();
				}
				if (Insurgent != null)
				{
					Insurgent.Delete();
				}
				if (Kuruma != null)
				{
					Kuruma.Delete();
				}
				if (InsurgentBLip != null)
				{
					InsurgentBLip.Delete();
				}
				if (KurumaBlip != null)
				{
					KurumaBlip.Delete();
				}
				foreach (Ped ped35 in Peds)
				{
					if (ped35 != null)
					{
						ped35.Delete();
					}
				}
				foreach (Ped guard9 in Guards)
				{
					if (guard9 != null)
					{
						guard9.Delete();
					}
				}
				Script.Wait(300);
				GTA.UI.Screen.FadeIn(300);
				TriggerVault = false;
				DefendingCops = false;
				VaultOpen = false;
				escapingcops = true;
				Bank.ShowRoute = false;
				TriggerVault = false;
				DefendingCops = false;
				VaultOpen = false;
				VaultUnlocked = false;
				InHeist = false;
				HeadingtoArcade = false;
				Script.Wait(20000);
				Notification.Show("~b~ The Union Depository Heist was failed!~w~");
				MissionScreen.SetFail("The Union Depository Heist", 0, 0, "you Died!", 0, HeistTime, 5);
				SetNoLongerInHeist();
				DeleteUnionNewAssets();
				MissionOn = false;
				break;
			}
			case HeistIn.PacStandard:
			{
				UI.RenderQue = 0f;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
				if (!PlayingPacificStandard)
				{
					break;
				}
				if (!PlayingUnion)
				{
					RobberACanLoot = true;
					RobberBCanLoot = true;
					RobberCCanLoot = true;
				}
				if (RobberA != null)
				{
					RobberA.BlockPermanentEvents = true;
				}
				if (RobberB != null)
				{
					RobberB.BlockPermanentEvents = true;
				}
				if (RobberC != null)
				{
					RobberC.BlockPermanentEvents = true;
				}
				float num171 = (float)Game.GameTime;
				float nextTick3 = nextTick;
				if (BankHeistStage == 0)
				{
					foreach (Prop camera in Cameras)
					{
						if (camera.AttachedBlip != null)
						{
							camera.AttachedBlip.Alpha = 0;
						}
					}
					GTA.UI.Screen.ShowSubtitle("Enter the ~y~Pacific Standard Bank~w~");
				}
				if (BankHeistStage == 1010)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					Game.Player.WantedLevel = 0;
					GTA.UI.Screen.ShowSubtitle("Take out the ~r~Guards~w~");
					bool flag4 = true;
					foreach (Ped guard10 in Guards)
					{
						if (guard10 != null && guard10.IsAlive)
						{
							flag4 = false;
						}
					}
					if (flag4)
					{
						ManageCellphoneCall.DialogueLines.Clear();
						ManageCellphoneCall.DialogueLines.Add("OBHF_AEAA");
						ManageCellphoneCall.DialogueLines.Add("OBHF_AEAB");
						ManageCellphoneCall.DialogueLines.Add("OBHF_AEAC");
						ManageCellphoneCall.DialogueLines.Add("OBHF_AFAB");
						ManageCellphoneCall.Speaker = "Lester";
						ManageCellphoneCall.StartVoiceOver();
						BankHeistStage = 1;
					}
				}
				if (BankHeistStage == 1)
				{
					Game.Player.WantedLevel = 0;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					if (GetDistance_VDIST2(Game.Player.Character.Position, Manager.Position, "<", 5f))
					{
						if (Manager.IsAlive)
						{
							GTA.UI.Screen.ShowSubtitle("Target the ~b~Manager~w~ to get him open the door");
						}
						if (!Manager.IsAlive)
						{
							GTA.UI.Screen.ShowSubtitle("Use ~o~Thermite~w~ To Open ~b~Door D~w~");
						}
					}
					else
					{
						if (!Manager.IsAlive)
						{
							GTA.UI.Screen.ShowSubtitle("Use ~o~Thermite~w~ To Open ~b~Door D~w~");
						}
						if (Manager.IsAlive)
						{
							GTA.UI.Screen.ShowSubtitle("Find the ~y~Manager~w~ To Open ~b~Door D~w~");
						}
					}
				}
				if (BankHeistStage == 2)
				{
					Game.Player.WantedLevel = 0;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					if (Manager != null)
					{
						if (Manager.IsAlive)
						{
							GTA.UI.Screen.ShowSubtitle("Follow the ~y~Manager~w~ till he opens ~b~Door D~w~ ");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Use ~o~Thermite~w~ To Open ~b~Door D~w");
						}
					}
				}
				if (BankHeistStage == 3)
				{
					Game.Player.WantedLevel = 0;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					GTA.UI.Screen.ShowSubtitle("Hack the ~y~Security Terminal~w~ to open ~b~Vault~w~");
				}
				if (BankHeistStage == 4)
				{
					Game.Player.WantedLevel = 0;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					GTA.UI.Screen.ShowSubtitle("Wait for the ~y~Vault~w~ to open");
				}
				if (BankHeistStage == 5)
				{
					Game.Player.WantedLevel = 0;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					ManageRobberyAnims.CreatedCrates = true;
					GTA.UI.Screen.ShowSubtitle("Grab as much ~g~Loot~w~ as you can carry\nor Leave the ~y~Vault");
				}
				if (BankHeistStage == 6)
				{
					Game.Player.WantedLevel = 0;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					GTA.UI.Screen.ShowSubtitle("Leave the ~y~Vault");
				}
				if (BankHeistStage == 7)
				{
					Game.Player.WantedLevel = 0;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
					GTA.UI.Screen.ShowSubtitle("Leave the ~y~Bank~w~ and escape the ~r~Cops~w~");
				}
				if (BankHeistStage == 8)
				{
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(253f, 217.4f, 106.2f), "<", 200f))
					{
						Game.Player.WantedLevel = 5;
					}
					GTA.UI.Screen.ShowSubtitle("Escape the ~r~Cops");
				}
				Ped[] nearbyPeds3 = World.GetNearbyPeds(DoorApos, 250f);
				Ped[] array24 = nearbyPeds3;
				foreach (Ped ped11 in array24)
				{
					if (!(ped11.Model != PedHash.Security01SMM) || !(ped11.CurrentVehicle == null))
					{
						continue;
					}
					if (ped11.Killer == Game.Player.Character && ped11.Opacity == 255)
					{
						KilledCops++;
						ped11.Opacity = 254;
					}
					if (KilledCops >= 2)
					{
						BankHeistResponceType = 1;
					}
					if (KilledCops >= 5)
					{
						BankHeistResponceType = 2;
					}
					RelationshipGroup relationshipGroup7 = Function.Call<int>(Hash.GET_HASH_KEY, "COP");
					if (!(ped11.RelationshipGroup == relationshipGroup7))
					{
						continue;
					}
					if (LocalTake == 0)
					{
						if (BankHeistResponceType == 0)
						{
							if (Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped11, Game.Player.Character, 19))
							{
								ped11.Task.AimAt(Game.Player.Character.Position, 1);
							}
							else
							{
								ped11.Task.AimAt(Game.Player.Character.Position, 1);
							}
						}
						if (BankHeistResponceType == 1 && Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped11, Game.Player.Character, 19))
						{
							ped11.Task.ShootAt(Game.Player.Character.Position);
						}
						if (BankHeistResponceType != 2)
						{
						}
						continue;
					}
					if (BankHeistResponceType == 0)
					{
						if (Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped11, Game.Player.Character, 19))
						{
							ped11.Task.ShootAt(Game.Player.Character.Position);
						}
						else
						{
							ped11.Task.AimAt(Game.Player.Character.Position, 1);
						}
					}
					if (BankHeistResponceType == 1 && Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped11, Game.Player.Character, 19))
					{
						ped11.Task.ShootAt(Game.Player.Character.Position);
					}
					int bankHeistResponceType = BankHeistResponceType;
					int num172 = 2;
				}
				if (MissionOn)
				{
					if (!ThirmiteDoor1 && Game.Player.Character.Position.DistanceTo(new Vector3(256.7718f, 220.1058f, 106.2863f)) < 1f)
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Plant Thermal Charge");
						if (Game.IsControlJustPressed(Control.Context) && !PlayeringThirmiteDoor1)
						{
							PlayeringThirmiteDoor1 = true;
							Ped character3 = Game.Player.Character;
							if (Game.Player.Character.Model == PedHash.Trevor)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 9, 0, 0, 1);
							}
							if (Game.Player.Character.Model == PedHash.Michael)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 9, 0, 0, 1);
							}
							if (Game.Player.Character.Model == PedHash.Franklin)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 9, 0, 0, 1);
							}
							DictToUse = "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@";
							DictToUse2 = "anim@heists@ornate_bank@thermal_charge";
							LoadDict(DictToUse);
							LoadDict(DictToUse);
							LoadDict(DictToUse2);
							LoadDict(DictToUse2);
							Function.Call(Hash.CREATE_MODEL_SWAP, 257.4089f, 220.2059f, 106.45f, 5, "hei_v_ilev_fh_heistdoor1", "hei_v_ilev_bk_gate_molten", 0);
							if (_thermiteProp != null)
							{
								_thermiteProp.Delete();
							}
							if (BagProp != null)
							{
								BagProp.Delete();
							}
							BagProp = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), new Vector3(266.6945f, 215.1253f, 110.2829f), new Vector3(1f, 0f, 0f), false, false);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 257.4089f, 220.2059f, 106.45f, 0.0, 0.0, 342.7213f, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, Scene1, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
							Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, BagProp);
							_thermiteProp = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, _thermiteProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
							Function.Call(Hash.PLAY_ENTITY_ANIM, _thermiteProp, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
						}
						if (PlayeringThirmiteDoor1)
						{
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1056353216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1057353216)
							{
								Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
								Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
								PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", _thermiteProp, 0f, 0f, 0f, 1f, 0, 0, 0);
								Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
								Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
								PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", _thermiteProp, 0f, 0f, 0f, 10f, 0, 0, 0);
								Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
							{
								PlayeringThirmiteDoor1 = false;
								ThirmiteDoor1 = true;
								Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
								Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
								Prop[] nearbyProps18 = World.GetNearbyProps(Game.Player.Character.Position, 2f, RequestModel("hei_v_ilev_bk_gate_pris"));
								Prop[] nearbyProps19 = World.GetNearbyProps(Game.Player.Character.Position, 3f, RequestModel(222270721));
								Prop[] array25 = nearbyProps19;
								foreach (Prop prop30 in array25)
								{
									prop30.IsPositionFrozen = false;
								}
								Prop[] nearbyProps20 = World.GetNearbyProps(new Vector3(261.672f, 222.23f, 106.2836f), 3f, RequestModel(746855201));
								Prop[] array26 = nearbyProps20;
								foreach (Prop prop31 in array26)
								{
									prop31.IsPositionFrozen = true;
								}
								Prop[] nearbyProps21 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_v_ilev_bk_safegate_pris");
								Prop[] array27 = nearbyProps21;
								foreach (Prop prop32 in array27)
								{
									prop32.IsPositionFrozen = false;
								}
								Prop[] nearbyProps22 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_v_ilev_bk_safegate_molten");
								Prop[] array28 = nearbyProps22;
								foreach (Prop prop33 in array28)
								{
									prop33.IsPositionFrozen = false;
								}
								Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", _thermiteProp, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
								Game.Player.Character.Task.ClearAll();
								Prop[] nearbyProps23 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate_pris");
								Prop[] array29 = nearbyProps23;
								foreach (Prop prop34 in array29)
								{
									if (prop34 != null)
									{
										prop34.IsPositionFrozen = false;
									}
								}
								if (BagProp != null)
								{
									BagProp.Delete();
								}
								if (_thermiteProp != null)
								{
									_thermiteProp.Delete();
								}
								Ped character4 = Game.Player.Character;
								if (Game.Player.Character.Model == PedHash.Trevor)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 1, 0, 1);
								}
								if (Game.Player.Character.Model == PedHash.Michael)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 2, 0, 1);
								}
								if (Game.Player.Character.Model == PedHash.Franklin)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 6, 0, 1);
								}
								Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, Scene1);
							}
						}
					}
					if (!ThirmiteDoor2)
					{
						if (Game.Player.Character.Position.DistanceTo(new Vector3(253.1041f, 221.6255f, 101.6f)) < 1f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Plant Thermal Charge");
							if (Game.IsControlJustPressed(Control.Context) && !PlayeringThirmiteDoor2)
							{
								PlayeringThirmiteDoor2 = true;
								Ped character5 = Game.Player.Character;
								if (Game.Player.Character.Model == PedHash.Trevor)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 9, 0, 0, 1);
								}
								if (Game.Player.Character.Model == PedHash.Michael)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 9, 0, 0, 1);
								}
								if (Game.Player.Character.Model == PedHash.Franklin)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 9, 0, 0, 1);
								}
								DictToUse = "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@";
								DictToUse2 = "anim@heists@ornate_bank@thermal_charge";
								LoadDict(DictToUse);
								LoadDict(DictToUse);
								LoadDict(DictToUse2);
								LoadDict(DictToUse2);
								Function.Call(Hash.CREATE_MODEL_SWAP, 253.1041f, 221.6255f, 101.6f, 3, "hei_v_ilev_bk_safegate_pris", "hei_v_ilev_bk_safegate_molten", 0);
								if (_thermiteProp != null)
								{
									_thermiteProp.Delete();
								}
								if (BagProp != null)
								{
									BagProp.Delete();
								}
								BagProp = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), new Vector3(253.1041f, 221.6255f, 101.6f), new Vector3(1f, 0f, 0f), false, false);
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 252.9815f, 220.7f, 101.8f, 0.0, 0.0, 175f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, Scene1, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, BagProp);
								_thermiteProp = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, _thermiteProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
								Function.Call(Hash.PLAY_ENTITY_ANIM, _thermiteProp, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
							}
							if (PlayeringThirmiteDoor2)
							{
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1056353216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1057353216)
								{
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
									PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", _thermiteProp, 0f, 0f, 0f, 1f, 0, 0, 0);
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
									PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", _thermiteProp, 0f, 0f, 0f, 10f, 0, 0, 0);
									Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
								}
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
								{
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
									PlayeringThirmiteDoor2 = false;
									ThirmiteDoor2 = true;
									Prop[] nearbyProps24 = World.GetNearbyProps(Game.Player.Character.Position, 2f, RequestModel("hei_v_ilev_bk_safegate_pris"));
									Prop[] nearbyProps25 = World.GetNearbyProps(Game.Player.Character.Position, 3f, RequestModel(222270721));
									Prop[] array30 = nearbyProps25;
									foreach (Prop prop35 in array30)
									{
										prop35.IsPositionFrozen = false;
									}
									Prop[] nearbyProps26 = World.GetNearbyProps(new Vector3(261.672f, 222.23f, 106.2836f), 3f, RequestModel(746855201));
									Prop[] array31 = nearbyProps26;
									foreach (Prop prop36 in array31)
									{
										prop36.IsPositionFrozen = true;
									}
									Prop[] nearbyProps27 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_v_ilev_bk_safegate_pris");
									Prop[] array32 = nearbyProps27;
									foreach (Prop prop37 in array32)
									{
										prop37.IsPositionFrozen = false;
									}
									Prop[] nearbyProps28 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_v_ilev_bk_safegate_molten");
									Prop[] array33 = nearbyProps28;
									foreach (Prop prop38 in array33)
									{
										prop38.IsPositionFrozen = false;
									}
									Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", _thermiteProp, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
									Game.Player.Character.Task.ClearAll();
									Prop[] nearbyProps29 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate_pris");
									Prop[] array34 = nearbyProps29;
									foreach (Prop prop39 in array34)
									{
										if (prop39 != null)
										{
											prop39.IsPositionFrozen = false;
										}
									}
									if (BagProp != null)
									{
										BagProp.Delete();
									}
									if (_thermiteProp != null)
									{
										_thermiteProp.Delete();
									}
									Ped character6 = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character6, 9, 1, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character6, 9, 2, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character6, 9, 6, 0, 1);
									}
									Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, Scene1);
								}
							}
						}
						Prop[] nearbyProps30 = World.GetNearbyProps(ThirmiteDoor2Pos, 3f);
						if (nearbyProps30.Length != 0)
						{
							Prop[] array35 = nearbyProps30;
							foreach (Prop prop40 in array35)
							{
								if (prop40 != null && (prop40.Model == RequestModel("hei_v_ilev_bk_safegate_pris") || prop40.Model == RequestModel("hei_v_ilev_bk_safegate_molten")) && Game.Player.Character.Position.DistanceTo(prop40.Position) < 3f)
								{
									prop40.Heading = 160f;
								}
							}
						}
					}
					if (!ThirmiteDoor3)
					{
						if (Game.Player.Character.Position.DistanceTo(new Vector3(260.8127f, 215.8801f, 101.6f)) < 1f)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Plant Thermal Charge");
							if (Game.IsControlJustPressed(Control.Context) && !PlayeringThirmiteDoor3)
							{
								PlayeringThirmiteDoor3 = true;
								Ped character7 = Game.Player.Character;
								if (Game.Player.Character.Model == PedHash.Trevor)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character7, 9, 0, 0, 1);
								}
								if (Game.Player.Character.Model == PedHash.Michael)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character7, 9, 0, 0, 1);
								}
								if (Game.Player.Character.Model == PedHash.Franklin)
								{
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character7, 9, 0, 0, 1);
								}
								ThirmiteDoor3 = false;
								DictToUse = "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@";
								DictToUse2 = "anim@heists@ornate_bank@thermal_charge";
								LoadDict(DictToUse);
								LoadDict(DictToUse);
								LoadDict(DictToUse2);
								LoadDict(DictToUse2);
								Function.Call(Hash.CREATE_MODEL_SWAP, 260.9127f, 215.9801f, 101.6f, 3, "hei_v_ilev_bk_safegate_pris", "hei_v_ilev_bk_safegate_molten", 0);
								if (_thermiteProp != null)
								{
									_thermiteProp.Delete();
								}
								if (BagProp != null)
								{
									BagProp.Delete();
								}
								BagProp = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), new Vector3(260.8127f, 215.8801f, 101.6f), new Vector3(1f, 0f, 0f), false, false);
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 261.659f, 215.6067f, 101.8f, 0.0, 0.0, -110f, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, Scene1, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
								Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, BagProp);
								_thermiteProp = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, _thermiteProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
								Function.Call(Hash.PLAY_ENTITY_ANIM, _thermiteProp, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
							}
							if (PlayeringThirmiteDoor3)
							{
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1056353216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1057353216)
								{
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
									PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", _thermiteProp, 0f, 0f, 0f, 1f, 0, 0, 0);
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
									PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", _thermiteProp, 0f, 0f, 0f, 10f, 0, 0, 0);
									Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
								}
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
								{
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
									PlayeringThirmiteDoor3 = false;
									ThirmiteDoor3 = true;
									Prop[] nearbyProps31 = World.GetNearbyProps(Game.Player.Character.Position, 2f, RequestModel("hei_v_ilev_bk_gate_pris"));
									Prop[] nearbyProps32 = World.GetNearbyProps(Game.Player.Character.Position, 3f, RequestModel(222270721));
									Prop[] array36 = nearbyProps32;
									foreach (Prop prop41 in array36)
									{
										prop41.IsPositionFrozen = false;
									}
									Prop[] nearbyProps33 = World.GetNearbyProps(new Vector3(261.672f, 222.23f, 106.2836f), 3f, RequestModel(746855201));
									Prop[] array37 = nearbyProps33;
									foreach (Prop prop42 in array37)
									{
										prop42.IsPositionFrozen = true;
									}
									Prop[] nearbyProps34 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_v_ilev_bk_safegate_pris");
									Prop[] array38 = nearbyProps34;
									foreach (Prop prop43 in array38)
									{
										prop43.IsPositionFrozen = false;
									}
									Prop[] nearbyProps35 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_v_ilev_bk_safegate_molten");
									Prop[] array39 = nearbyProps35;
									foreach (Prop prop44 in array39)
									{
										prop44.IsPositionFrozen = false;
									}
									Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", _thermiteProp, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
									Game.Player.Character.Task.ClearAll();
									Prop[] nearbyProps36 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate_pris");
									Prop[] array40 = nearbyProps36;
									foreach (Prop prop45 in array40)
									{
										if (prop45 != null)
										{
											prop45.IsPositionFrozen = false;
										}
									}
									if (BagProp != null)
									{
										BagProp.Delete();
									}
									if (_thermiteProp != null)
									{
										_thermiteProp.Delete();
									}
									Ped character8 = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character8, 9, 1, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character8, 9, 2, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character8, 9, 6, 0, 1);
									}
									Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, Scene1);
								}
							}
						}
						Prop[] nearbyProps37 = World.GetNearbyProps(ThirmiteDoor3Pos, 3f);
						if (nearbyProps37.Length != 0)
						{
							Prop[] array41 = nearbyProps37;
							foreach (Prop prop46 in array41)
							{
								if (prop46 != null && (prop46.Model == RequestModel("hei_v_ilev_bk_safegate_pris") || prop46.Model == RequestModel("hei_v_ilev_bk_safegate_molten")) && Game.Player.Character.Position.DistanceTo(prop46.Position) < 3f)
								{
									prop46.Heading = 249f;
								}
							}
						}
					}
				}
				if (!GotVaultDoor)
				{
					VaultDoor = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 255, 224f, 101, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_vaultdoor"), 0, 0, 0);
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(255f, 224f, 101f), "<", 30f))
					{
						Prop[] nearbyProps38 = World.GetNearbyProps(new Vector3(255.2283f, 223.976f, 102.3932f), 1f);
						if (nearbyProps38.Length != 0)
						{
							Prop[] array42 = nearbyProps38;
							foreach (Prop prop47 in array42)
							{
								if (prop47 != null)
								{
									GotVaultDoor = true;
									VaultDoor = prop47;
								}
							}
						}
					}
				}
				if (GotVaultDoor)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(255f, 224f, 101f), ">", 50f))
					{
						GotVaultDoor = false;
					}
					bool flag21 = VaultDoor != null;
				}
				if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(255f, 224f, 101f), "<", 30f) && VaultDoor != null && VaultDoor != null)
				{
					if (VaultOpen && ShowPacificStandardVaultDebug)
					{
						World.DrawMarker(MarkerType.Sphere, VaultDoor.GetOffsetPosition(new Vector3(0f, -0.001f, 0f)), Vector3.Zero, Vector3.Zero, new Vector3(0.025f, 0.025f, 0.025f), Color.Lime);
					}
					if (!VaultOpen && ShowPacificStandardVaultDebug)
					{
						World.DrawMarker(MarkerType.Sphere, VaultDoor.GetOffsetPosition(new Vector3(0f, -0.001f, 0f)), Vector3.Zero, Vector3.Zero, new Vector3(0.025f, 0.025f, 0.025f), Color.Red);
					}
					if (VaultOpen)
					{
						Vector3 position17 = Game.Player.Character.Position;
						if (position17.Z > 99f && position17.Z < 102f)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, VaultDoor.Position, "<", 8f))
							{
								Prop vaultDoor = VaultDoor;
								if (vaultDoor.Rotation.Z >= -15f)
								{
									vaultDoor.Rotation = new Vector3(vaultDoor.Rotation.X, vaultDoor.Rotation.Y, vaultDoor.Rotation.Z - 0.5f);
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, VaultDoor.Position, ">", 8f))
							{
								bool flag5 = true;
								if (RobberA != null && GetDistance_VDIST2(RobberA.Position, VaultDoor.Position, "<", 8f))
								{
									flag5 = false;
									break;
								}
								if (flag5)
								{
									Prop vaultDoor2 = VaultDoor;
									if (vaultDoor2.Rotation.Z <= 160f)
									{
										vaultDoor2.Rotation = new Vector3(vaultDoor2.Rotation.X, vaultDoor2.Rotation.Y, vaultDoor2.Rotation.Z + 0.5f);
									}
								}
							}
						}
						if (RobberA != null)
						{
							Vector3 position18 = RobberA.Position;
							if (position18.Z > 99f && position18.Z < 102f)
							{
								if (GetDistance_VDIST2(RobberA.Position, VaultDoor.Position, "<", 8f))
								{
									Prop vaultDoor3 = VaultDoor;
									if (vaultDoor3.Rotation.Z >= -15f)
									{
										vaultDoor3.Rotation = new Vector3(vaultDoor3.Rotation.X, vaultDoor3.Rotation.Y, vaultDoor3.Rotation.Z - 0.5f);
									}
								}
								if (GetDistance_VDIST2(RobberA.Position, VaultDoor.Position, ">", 8f) && GetDistance_VDIST2(Game.Player.Character.Position, VaultDoor.Position, ">", 8f))
								{
									bool flag6 = true;
									if (RobberA != null && GetDistance_VDIST2(RobberA.Position, VaultDoor.Position, "<", 8f))
									{
										flag6 = false;
										break;
									}
									if (flag6)
									{
										Prop vaultDoor4 = VaultDoor;
										if (vaultDoor4.Rotation.Z <= 160f)
										{
											vaultDoor4.Rotation = new Vector3(vaultDoor4.Rotation.X, vaultDoor4.Rotation.Y, vaultDoor4.Rotation.Z + 0.5f);
										}
									}
								}
							}
						}
					}
				}
				if (Game.Player.IsAlive)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(253f, 217.4f, 106.2f), "<", 250f) && Game.Player.Character.IsShooting && !PacificStandardCivsAndTellerInCower)
					{
						PacificStandardCivsAndTellerInCower = true;
						foreach (Ped teller2 in Tellers)
						{
							if (teller2 != null)
							{
								teller2.BlockPermanentEvents = true;
								teller2.Task.Cower(-1);
								teller2.AlwaysKeepTask = true;
							}
						}
						foreach (Ped civ in Civs)
						{
							if (civ != null)
							{
								civ.BlockPermanentEvents = true;
								civ.Task.Cower(-1);
								civ.AlwaysKeepTask = true;
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, DoorBPos, "<", 30f))
					{
						World.DrawMarker(MarkerType.Cylinder, DoorBPos, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), Color.Red);
					}
					if (Manager != null)
					{
						if (!Manager.IsAlive && TellerScene != 20)
						{
							if (tellerwalkStage != 25)
							{
								Notification.Show("~y~ Lester~w~ Shit!, you werent supposed to kill the manager!, youll need to use Thermite on the door");
								tellerwalkStage = 25;
							}
							if (tellerwalkStage == 25 && !ThermiteDoor4 && Game.Player.Character.Position.DistanceTo(new Vector3(261.426f, 221.9963f, 106.2833f)) < 2f)
							{
								if (!PlayingThermitedoor4)
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Plant Thermal Charge");
								}
								if (Game.IsControlJustPressed(Control.Context) && !PlayingThermitedoor4)
								{
									Ped character9 = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character9, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character9, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character9, 9, 0, 0, 1);
									}
									PlayingThermitedoor4 = true;
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
									BagProp = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), new Vector3(266.6945f, 215.1253f, 110.2829f), new Vector3(1f, 0f, 0f), false, false);
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, 261.7837f, 221.3933f, 106.4831f, 0.0, 0.0, 252.6861f, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, Scene1, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
									Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, BagProp);
									_thermiteProp = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, _thermiteProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
									Function.Call(Hash.PLAY_ENTITY_ANIM, _thermiteProp, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
								}
								if (PlayingThermitedoor4)
								{
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1056353216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1057353216)
									{
										Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
										Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
										PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", _thermiteProp, 0f, 0f, 0f, 1f, 0, 0, 0);
										Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
										Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
										PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", _thermiteProp, 0f, 0f, 0f, 10f, 0, 0, 0);
										Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
									{
										HackKeyPadDone = false;
										DoorDLocked = false;
										Notification.Show("~y~ Lester ~w~ : Good, Gate D has been Unlocked");
										GotKeyCard = true;
										ManageCellphoneCall.DialogueLines.Clear();
										ManageCellphoneCall.DialogueLines.Add("OBHF_AGAA");
										ManageCellphoneCall.DialogueLines.Add("OBHF_AGAB");
										ManageCellphoneCall.Speaker = "Lester";
										ManageCellphoneCall.StartVoiceOver();
										BankHeistStage = 3;
										CivsAreFreakingOut = true;
										Prop[] nearbyProps39 = World.GetNearbyProps(DoorDPos, 5f, "hei_v_ilev_bk_gate2_pris");
										Prop[] array43 = nearbyProps39;
										foreach (Prop prop48 in array43)
										{
											if (prop48 != null)
											{
												prop48.IsPositionFrozen = false;
											}
										}
										ThermiteDoor4 = true;
										PlayingThermitedoor4 = false;
										Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
										Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
										Prop[] nearbyProps40 = World.GetNearbyProps(new Vector3(261.672f, 222.23f, 106.2836f), 3f, RequestModel(746855201));
										Prop[] array44 = nearbyProps40;
										foreach (Prop prop49 in array44)
										{
											prop49.IsPositionFrozen = false;
										}
										Prop[] nearbyProps41 = World.GetNearbyProps(new Vector3(251.734f, 220.635f, 101.6834f), 3f, RequestModel("hei_v_ilev_bk_safegate_pris"));
										Prop[] array45 = nearbyProps41;
										foreach (Prop prop50 in array45)
										{
											prop50.IsPositionFrozen = true;
										}
										Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", _thermiteProp, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
										Game.Player.Character.Task.ClearAll();
										if (BagProp != null)
										{
											BagProp.Delete();
										}
										if (_thermiteProp != null)
										{
											_thermiteProp.Delete();
										}
										Ped character10 = Game.Player.Character;
										if (Game.Player.Character.Model == PedHash.Trevor)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character10, 9, 1, 0, 1);
										}
										if (Game.Player.Character.Model == PedHash.Michael)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character10, 9, 2, 0, 1);
										}
										if (Game.Player.Character.Model == PedHash.Franklin)
										{
											Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character10, 9, 6, 0, 1);
										}
										Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, Scene1);
									}
								}
							}
						}
						if (tellerwalkStage == 8 && HackCard != null)
						{
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, TellerScene) > 1045353216)
							{
								HackCard.Opacity = 255;
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, TellerScene) > 1058353216)
							{
								TellerScene = 20;
								Manager.Task.ClearAll();
								Manager.Task.PlayAnimation("busted", "idle_2_hands_up_2h", -1f, -1f, -1, AnimationFlags.StayInEndFrame | AnimationFlags.Secondary, -1f);
								HackCard.Opacity = 255;
								HackKeyPadDone = false;
								DoorDLocked = false;
								Notification.Show("~y~ Lester ~w~ : Good, Gate D has been Unlocked");
								GotKeyCard = true;
								BankHeistStage = 3;
								CivsAreFreakingOut = true;
								Prop[] nearbyProps42 = World.GetNearbyProps(DoorDPos, 5f, "hei_v_ilev_bk_gate2_pris");
								Prop[] array46 = nearbyProps42;
								foreach (Prop prop51 in array46)
								{
									if (prop51 != null)
									{
										prop51.IsPositionFrozen = false;
									}
								}
							}
						}
						if (GetDistance_VDIST2(Manager.Position, Offset, "<", 1.5f))
						{
							if (tellerwalkStage == 0)
							{
								Offset = new Vector3(262.4095f, 217.4107f, 110.2f);
								tellerwalkStage = 1;
								Function.Call(Hash.TASK_PLAY_ANIM, Manager, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Manager, Offset.X, Offset.Y, Offset.Y, 1f, -1, Manager.Heading + 100f, -0.1f);
							}
							else if (tellerwalkStage == 1)
							{
								Offset = new Vector3(262.2271f, 217.25f, 110.28f);
								tellerwalkStage = 2;
								Function.Call(Hash.TASK_PLAY_ANIM, Manager, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Manager, Offset.X, Offset.Y, Offset.Y, 1f, -1, Manager.Heading + 100f, -0.1f);
							}
							else if (tellerwalkStage == 2)
							{
								Offset = new Vector3(265.2271f, 216.45f, 110.28f);
								tellerwalkStage = 3;
								Function.Call(Hash.TASK_PLAY_ANIM, Manager, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Manager, Offset.X, Offset.Y, Offset.Y, 1f, -1, Manager.Heading + 100f, -0.1f);
							}
							else if (tellerwalkStage == 3)
							{
								Offset = new Vector3(268.1614f, 223.2361f, 110.2f);
								tellerwalkStage = 4;
								Function.Call(Hash.TASK_PLAY_ANIM, Manager, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Manager, Offset.X, Offset.Y, Offset.Y, 1f, -1, Manager.Heading + 100f, -0.1f);
							}
							else if (tellerwalkStage == 4)
							{
								Offset = new Vector3(260.1968f, 226.2677f, 107.0833f);
								tellerwalkStage = 5;
								Function.Call(Hash.TASK_PLAY_ANIM, Manager, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Manager, Offset.X, Offset.Y, Offset.Y, 1f, -1, Manager.Heading + 100f, -0.1f);
							}
							else if (tellerwalkStage == 5)
							{
								Offset = new Vector3(259.268f, 223.6162f, 106.28f);
								tellerwalkStage = 6;
								Function.Call(Hash.TASK_PLAY_ANIM, Manager, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Manager, Offset.X, Offset.Y, Offset.Y, 1f, -1, Manager.Heading + 100f, -0.1f);
							}
							else if (tellerwalkStage == 6)
							{
								Offset = new Vector3(261.744f, 223.284f, 106.2841f);
								tellerwalkStage = 7;
								Function.Call(Hash.TASK_PLAY_ANIM, Manager, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Manager, Offset.X, Offset.Y, Offset.Y, 1f, -1, Manager.Heading + 100f, -0.1f);
							}
							if (tellerwalkStage == 7)
							{
								Manager.Task.ClearAnimation("ped", "handsup");
								Manager.Task.ClearAll();
								Manager.Task.ClearAllImmediately();
								DictToUse = "anim@heists@ornate_bank@hack";
								LoadDict(DictToUse);
								LoadDict(DictToUse);
								if (HackCard != null)
								{
									HackCard.Delete();
								}
								if (BagProp != null)
								{
									BagProp.Delete();
								}
								HackCard = GlobalHelper.CreateProp(RequestModel("hei_prop_hei_id_bank"), Game.Player.Character.Position, false, false);
								HackCard.Opacity = 0;
								Manager.Position = new Vector3(261.544f, 223.35f, 106.28f);
								Manager.Heading = -109f;
								Vector3 offsetPosition13 = Manager.GetOffsetPosition(new Vector3(0.06f, 0.8f, -0.64f));
								TellerScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition13.X, offsetPosition13.Y, offsetPosition13.Z, 0.0, 0.0, Manager.Heading, 2);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, TellerScene, offsetPosition13.X, offsetPosition13.Y, offsetPosition13.Z, 0.0, 0.0, Manager.Heading, true);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Manager, TellerScene, LoadDict(DictToUse), "hack_enter", 1000.0, -8.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Manager, TellerScene, "hack_enter", LoadDict(DictToUse), 1000.0, -8f, -8f, Manager.Heading, 1148846080, 0);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, HackCard, TellerScene, LoadDict(DictToUse), "hack_enter_card", 1000.0, -8.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HackCard, TellerScene, "hack_enter_card", LoadDict(DictToUse), 1000.0, -8f, -8f, Manager.Heading, 1148846080, 0);
								tellerwalkStage = 8;
							}
						}
						if (!GotKeyCard && GetDistance_VDIST2(Game.Player.Character.Position, Manager.Position, "<", 5f) && Game.Player.IsTargeting(Manager))
						{
							BankHeistStage = 2;
							if (tellerwalkStage == 0)
							{
								tellerwalkStage = 1;
							}
							if (tellerwalkStage != 7)
							{
								Function.Call(Hash.TASK_PLAY_ANIM, Manager, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Manager, Offset.X, Offset.Y, Offset.Y, 1f, -1, Manager.Heading + 100f, -0.1f);
							}
							DoorCPos = new Vector3(257.027f, 220.4003f, 105f);
							DoorDPos = new Vector3(261.93f, 221.79f, 105f);
							Prop[] nearbyProps43 = World.GetNearbyProps(new Vector3(265.88f, 218.1715f, 110f), 5f, 1956494919);
							Prop[] array47 = nearbyProps43;
							foreach (Prop prop52 in array47)
							{
								if (prop52 != null)
								{
									prop52.IsPositionFrozen = false;
								}
							}
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, DoorBPos, "<", 2f))
					{
						if (!DoorBLocked)
						{
							if (Game.IsControlJustPressed(Control.Context))
							{
								DoorBLocked = true;
								Prop[] nearbyProps44 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_prop_hei_bankdoor_new");
								Prop[] array48 = nearbyProps44;
								foreach (Prop prop53 in array48)
								{
									if (prop53 != null)
									{
										prop53.IsPositionFrozen = true;
									}
								}
							}
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Lock Door B");
						}
						else if (DoorBLocked)
						{
							if (Game.IsControlJustPressed(Control.Context))
							{
								DoorBLocked = false;
								Prop[] nearbyProps45 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_prop_hei_bankdoor_new");
								Prop[] array49 = nearbyProps45;
								foreach (Prop prop54 in array49)
								{
									if (prop54 != null)
									{
										prop54.IsPositionFrozen = false;
									}
								}
							}
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Unlock Door B");
						}
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, DoorApos, "<", 30f))
					{
						World.DrawMarker(MarkerType.Cylinder, DoorApos, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), Color.Red);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, DoorApos, "<", 2f))
					{
						if (!DoorALocked)
						{
							if (Game.IsControlJustPressed(Control.Context))
							{
								DoorALocked = true;
								Prop[] nearbyProps46 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_prop_hei_bankdoor_new");
								Prop[] array50 = nearbyProps46;
								foreach (Prop prop55 in array50)
								{
									if (prop55 != null)
									{
										prop55.IsPositionFrozen = true;
									}
								}
							}
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Lock Door A");
						}
						else if (DoorALocked)
						{
							if (Game.IsControlJustPressed(Control.Context))
							{
								DoorALocked = false;
								Prop[] nearbyProps47 = World.GetNearbyProps(Game.Player.Character.Position, 5f, "hei_prop_hei_bankdoor_new");
								Prop[] array51 = nearbyProps47;
								foreach (Prop prop56 in array51)
								{
									if (prop56 != null)
									{
										prop56.IsPositionFrozen = false;
									}
								}
							}
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Unlock Door A");
						}
					}
					if (InHeist)
					{
						HeistTime += 0.35f;
					}
					if (CamerasLeft > 0 && SecurityCam.Count > 0)
					{
						foreach (Prop item39 in SecurityCam)
						{
							if (!(item39 != null))
							{
								continue;
							}
							if (item39.AttachedBlip == null)
							{
								item39.AddBlip();
								if (item39.AttachedBlip != null)
								{
									item39.AttachedBlip.Sprite = BlipSprite.Standard;
									item39.AttachedBlip.Color = BlipColor.Red2;
									item39.AttachedBlip.Name = "Camera";
									item39.AttachedBlip.Scale = 0.5f;
								}
							}
							if (!GetDistance_VDIST2(Game.Player.Character.Position, item39.Position, "<", 100f))
							{
								continue;
							}
							if (item39.Health > 999 && ShowCCTVBlipAliveDebug)
							{
								World.DrawMarker(MarkerType.Sphere, item39.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f), Color.White);
							}
							if (item39.Health <= 999)
							{
								if (item39.Opacity == 255)
								{
									if (!PacificStandardCivsAndTellerInCower)
									{
										PacificStandardCivsAndTellerInCower = true;
										foreach (Ped teller3 in Tellers)
										{
											if (teller3 != null)
											{
												teller3.BlockPermanentEvents = true;
												teller3.Task.Cower(-1);
												teller3.AlwaysKeepTask = true;
											}
										}
										foreach (Ped civ2 in Civs)
										{
											if (civ2 != null)
											{
												civ2.BlockPermanentEvents = true;
												civ2.Task.Cower(-1);
												civ2.AlwaysKeepTask = true;
											}
										}
									}
									item39.Opacity = 254;
									item39.AttachedBlip.Alpha = 0;
									CamerasLeft--;
									Notification.Show("~y~ Lester ~w~ Cameras Left : " + CamerasLeft);
								}
								if (ShowCCTVBlipDeadDebug)
								{
									World.DrawMarker(MarkerType.Sphere, item39.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f), Color.Red);
								}
							}
							if (CamerasLeft != 0)
							{
								continue;
							}
							if (!PacificStandardCivsAndTellerInCower)
							{
								PacificStandardCivsAndTellerInCower = true;
								foreach (Ped teller4 in Tellers)
								{
									if (teller4 != null)
									{
										teller4.BlockPermanentEvents = true;
										teller4.Task.Cower(-1);
										teller4.AlwaysKeepTask = true;
									}
								}
								foreach (Ped civ3 in Civs)
								{
									if (civ3 != null)
									{
										civ3.BlockPermanentEvents = true;
										civ3.Task.Cower(-1);
										civ3.AlwaysKeepTask = true;
									}
								}
							}
							Notification.Show("~y~ Lester ~w~ All Cameras Destroyed!");
						}
					}
					if (CamerasLeft != 0)
					{
						HeadingtoArcade = false;
					}
					if (NotifyEnd && CamerasLeft == 0)
					{
						HeadingtoArcade = true;
					}
					if (!NotifyEnd && TriggerCops)
					{
						if (Difficulty == 0)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 1)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 2)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 3)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 4)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 5)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
							Game.Player.WantedLevel = 0;
						}
					}
					if (TriggerCops)
					{
						if (Difficulty == 0)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 1)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 2);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 2)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 3)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 3);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 4)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
							Game.Player.WantedLevel = 0;
						}
						if (Difficulty == 5)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
							Game.Player.WantedLevel = 0;
						}
					}
					if (placedJammer)
					{
						if (BreachTimer != 450)
						{
							BreachTimer++;
						}
						if (BreachTimer == 450)
						{
							BreachTimer = 0;
							Random random12 = new Random();
							int num93 = random12.Next(1, 100);
							int num94 = random12.Next(1, 100);
							if (DoorALocked && num94 < 50 && num93 <= 5)
							{
								DoorALocked = false;
								World.AddExplosion(DoorApos, ExplosionType.ProxMine, 10f, 3f);
								Notification.Show("~y~ Lester ~w~ : The Cops have breached Door A!");
							}
							if (DoorBLocked && num94 >= 50 && num93 <= 5)
							{
								World.AddExplosion(DoorBPos, ExplosionType.ProxMine, 10f, 3f);
								Notification.Show("~y~ Lester ~w~ : The Cops have breached Door B!");
							}
						}
						InHeist = true;
						Prop[] nearbyProps48 = World.GetNearbyProps(DoorCPos, 1f);
						Prop[] nearbyProps49 = World.GetNearbyProps(DoorDPos, 1f);
						Prop[] array52 = nearbyProps48;
						foreach (Prop prop57 in array52)
						{
							if (prop57 != null && DoorCLocked)
							{
								prop57.Rotation = new Vector3(prop57.Rotation.X, prop57.Rotation.Y, -20f);
							}
						}
						Prop[] array53 = nearbyProps49;
						foreach (Prop prop58 in array53)
						{
							if (prop58 != null && DoorDLocked)
							{
								prop58.Rotation = new Vector3(prop58.Rotation.X, prop58.Rotation.Y, -110f);
							}
						}
						if (!DoorDLocked)
						{
							Prop[] nearbyProps50 = World.GetNearbyProps(DoorDPos, 5f, "hei_v_ilev_bk_gate2_pris");
							Prop[] nearbyProps51 = World.GetNearbyProps(DoorDPos, 5f, "hei_v_ilev_bk_gate_pris");
							Prop[] array54 = nearbyProps51;
							foreach (Prop prop59 in array54)
							{
								if (prop59 != null)
								{
									prop59.IsPositionFrozen = false;
								}
							}
							Prop[] array55 = nearbyProps50;
							foreach (Prop prop60 in array55)
							{
								if (prop60 != null)
								{
									prop60.IsPositionFrozen = false;
								}
							}
						}
						if (!LockedDoors && (GetDistance_VDIST2(Game.Player.Character.Position, DoorBPos, "<", 2f) || GetDistance_VDIST2(Game.Player.Character.Position, DoorApos, "<", 2f)))
						{
							LockedDoors = true;
							Prop[] nearbyProps52 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate2_pris");
							Prop[] nearbyProps53 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate_pris");
							Prop[] nearbyProps54 = World.GetNearbyProps(DoorDPos, 5f, "hei_v_ilev_bk_gate2_pris");
							Prop[] nearbyProps55 = World.GetNearbyProps(DoorDPos, 5f, "hei_v_ilev_bk_gate_pris");
							Prop[] array56 = nearbyProps53;
							foreach (Prop prop61 in array56)
							{
								if (prop61 != null)
								{
									prop61.IsPositionFrozen = true;
								}
							}
							Prop[] array57 = nearbyProps52;
							foreach (Prop prop62 in array57)
							{
								if (prop62 != null)
								{
									prop62.IsPositionFrozen = true;
								}
							}
							Prop[] array58 = nearbyProps55;
							foreach (Prop prop63 in array58)
							{
								if (prop63 != null)
								{
									prop63.IsPositionFrozen = true;
								}
							}
							Prop[] array59 = nearbyProps54;
							foreach (Prop prop64 in array59)
							{
								if (prop64 != null)
								{
									prop64.IsPositionFrozen = true;
								}
							}
						}
						if (CivsAreFreakingOut && !PacificStandardCivsAndTellerInCower)
						{
							PacificStandardCivsAndTellerInCower = true;
							foreach (Ped teller5 in Tellers)
							{
								if (teller5 != null)
								{
									teller5.BlockPermanentEvents = true;
									teller5.Task.Cower(-1);
									teller5.AlwaysKeepTask = true;
								}
							}
							foreach (Ped civ4 in Civs)
							{
								if (civ4 != null)
								{
									civ4.BlockPermanentEvents = true;
									civ4.Task.Cower(-1);
									civ4.AlwaysKeepTask = true;
								}
							}
						}
						if ((DoorALocked || DoorBLocked) && CreatedBlips && Game.Player.Character.IsShooting)
						{
							CivsAreFreakingOut = true;
						}
						if ((GetDistance_VDIST2(Game.Player.Character.Position, FleecaBankMarker, "<", 10f) || GetDistance_VDIST2(Game.Player.Character.Position, DoorApos, "<", 10f) || GetDistance_VDIST2(Game.Player.Character.Position, DoorBPos, "<", 10f)) && BankHeistStage == 0)
						{
							foreach (Prop camera2 in Cameras)
							{
								if (camera2.AttachedBlip != null)
								{
									camera2.AttachedBlip.Alpha = 255;
								}
							}
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("OBHF_ADAA");
							ManageCellphoneCall.Speaker = "Lester";
							ManageCellphoneCall.StartVoiceOver();
							BankHeistStage = 1010;
						}
						if ((GetDistance_VDIST2(Game.Player.Character.Position, FleecaBankMarker, "<", 40f) || GetDistance_VDIST2(Game.Player.Character.Position, DoorApos, "<", 40f) || GetDistance_VDIST2(Game.Player.Character.Position, DoorBPos, "<", 40f)) && !CreatedBlips)
						{
							CreatedBlips = true;
							int num103 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 253f, 217.4f, 106.2f);
							if (num103 != 0)
							{
								Function.Call(Hash.REFRESH_INTERIOR, num103);
								Notification.Show("Refreshed Ornate Bank");
								Wait(1000);
								SecurityCam.Clear();
								Prop[] nearbyProps56 = World.GetNearbyProps(new Vector3(253f, 217.4f, 106.2f), 200f, RequestModel("hei_prop_bank_cctv_01"));
								if (nearbyProps56.Length != 0)
								{
									Prop[] array60 = nearbyProps56;
									foreach (Prop prop65 in array60)
									{
										if (prop65 != null)
										{
											prop65.Opacity = 255;
											SecurityCam.Add(prop65);
											prop65.AddBlip();
											prop65.AttachedBlip.Sprite = BlipSprite.Standard;
											prop65.AttachedBlip.Color = BlipColor.Red2;
											prop65.AttachedBlip.Name = "Camera";
											prop65.AttachedBlip.Scale = 0.5f;
											prop65.AttachedBlip.Rotation = (int)prop65.Rotation.Z;
											CameraBlips.Add(prop65.AttachedBlip);
										}
									}
								}
								CamerasLeft = SecurityCam.Count - 1;
							}
							else
							{
								Notification.Show("No Interior Found");
							}
							Notification.Show("~y~ Lester ~w~ : Lock the main doors, and control the crowd inside, grab the keycard from the manager to open the gate protecting the Vault!");
							Notification.Show("~y~ Lester ~w~ : You Also need to destroy all Cameras in the Bank before you can leave!,~g~ Cameras Found " + (SecurityCam.Count - 1));
							DoorAblip = GlobalHelper.CreateBlip(DoorApos);
							DoorAblip.Sprite = BlipSprite.TargetA;
							DoorAblip.Name = "Door A";
							DoorAblip.Scale = 0.5f;
							DoorBblip = GlobalHelper.CreateBlip(DoorBPos);
							DoorBblip.Sprite = BlipSprite.TargetB;
							DoorBblip.Name = "Door B";
							DoorBblip.Scale = 0.5f;
							DoorDblip = GlobalHelper.CreateBlip(DoorDPos);
							DoorDblip.Sprite = BlipSprite.TargetD;
							DoorDblip.Name = "Door D";
							DoorDblip.Scale = 0.5f;
							DoorCLocked = true;
							DoorDLocked = true;
							Prop[] nearbyProps57 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate2_pris");
							Prop[] nearbyProps58 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate_pris");
							Prop[] nearbyProps59 = World.GetNearbyProps(DoorDPos, 5f, "hei_v_ilev_bk_gate2_pris");
							Prop[] nearbyProps60 = World.GetNearbyProps(DoorDPos, 5f, "hei_v_ilev_bk_gate_pris");
							Prop[] array61 = nearbyProps58;
							foreach (Prop prop66 in array61)
							{
								if (prop66 != null)
								{
									prop66.IsPositionFrozen = true;
								}
							}
							Prop[] array62 = nearbyProps57;
							foreach (Prop prop67 in array62)
							{
								if (prop67 != null)
								{
									prop67.IsPositionFrozen = true;
								}
							}
							Prop[] array63 = nearbyProps60;
							foreach (Prop prop68 in array63)
							{
								if (prop68 != null)
								{
									prop68.IsPositionFrozen = true;
								}
							}
							Prop[] array64 = nearbyProps59;
							foreach (Prop prop69 in array64)
							{
								if (prop69 != null)
								{
									prop69.IsPositionFrozen = true;
								}
							}
						}
						if (InsurgentBLip != null && Insurgent != null)
						{
							InsurgentBLip.Position = Insurgent.Position;
						}
						if (KurumaBlip != null && Kuruma != null)
						{
							KurumaBlip.Position = Kuruma.Position;
						}
					}
					if (!HackedPacificStandardSecurityPad)
					{
						Prop prop70 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 252.8626f, 228.5469f, 102.0883f, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "v_corp_bk_secpanel"), 0, 0, 0);
						if (prop70 != null)
						{
							GetDistance_VDIST2(Game.Player.Character.Position, HackPacficStandardSecurityPadPosition, "<", 30f);
							if (GetDistance_VDIST2(Game.Player.Character.Position, HackPacficStandardSecurityPadPosition, "<", 3f) && !FingerprintHack.BruteForceHackOn && !isHacking)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Hack the Security Pad");
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, HackPacficStandardSecurityPadPosition, "<", 10f))
							{
								if (!isHacking && Game.IsControlJustPressed(Control.Context))
								{
									Ped character11 = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character11, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character11, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character11, 9, 0, 0, 1);
									}
									Hackedkeypad = false;
									DictToUse = "anim@heists@ornate_bank@hack";
									LoadDict(DictToUse);
									LoadDict(DictToUse);
									if (HackCard != null)
									{
										HackCard.Delete();
									}
									if (BagProp != null)
									{
										BagProp.Delete();
									}
									if (Laptop != null)
									{
										Laptop.Delete();
									}
									HackCard = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_card_hack_02"), Game.Player.Character.Position, false, false);
									BagProp = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
									Laptop = GlobalHelper.CreateProp(RequestModel("hei_prop_hst_laptop"), Game.Player.Character.Position, false, false);
									BagProp.IsCollisionEnabled = false;
									BagProp.IsInvincible = true;
									Game.Player.Character.Position = new Vector3(253.7687f, 228.1796f, 100.6833f);
									Game.Player.Character.Heading = 68.69633f;
									Vector3 offsetPosition14 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.4f));
									HackScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition14.X, offsetPosition14.Y, offsetPosition14.Z, 0.0, 0.0, Game.Player.Character.Heading, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, HackScene1, offsetPosition14.X, offsetPosition14.Y, offsetPosition14.Z, 0.0, 0.0, Game.Player.Character.Heading, true);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, HackScene1, LoadDict(DictToUse), "hack_enter", 1000.0, -8.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, HackScene1, "hack_enter", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, HackCard, HackScene1, LoadDict(DictToUse), "hack_enter_card", 1000.0, -8.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HackCard, HackScene1, "hack_enter_card", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BagProp, HackScene1, LoadDict(DictToUse), "hack_enter_bag", 1000.0, -8.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, HackScene1, "hack_enter_bag", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Laptop, HackScene1, LoadDict(DictToUse), "hack_enter_laptop", 1000.0, -8.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Laptop, HackScene1, "hack_enter_laptop", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
									Wait(500);
									isHacking = true;
									QuitHacking = false;
									Hackedkeypad = false;
									HackDone = false;
								}
								if (QuitHacking && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, HackScene1) >= 1065353216)
								{
									QuitHacking = false;
									if (HackCard != null)
									{
										HackCard.Delete();
									}
									if (BagProp != null)
									{
										BagProp.Delete();
									}
									Ped character12 = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character12, 9, 1, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character12, 9, 2, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character12, 9, 6, 0, 1);
									}
									if (Laptop != null)
									{
										Laptop.Delete();
									}
									Game.Player.Character.Task.ClearAll();
									Game.Player.CanControlCharacter = true;
									Hackedkeypad = false;
									isHacking = false;
									if (HackDone)
									{
										HackedPacificStandardSecurityPad = true;
									}
								}
								if (isHacking)
								{
									if (!QuitHacking)
									{
										if (FingerprintHack.HackFinishedID == 1)
										{
											ManageCellphoneCall.DialogueLines.Clear();
											ManageCellphoneCall.DialogueLines.Add("OBHF_AHAA");
											ManageCellphoneCall.Speaker = "Lester";
											ManageCellphoneCall.StartVoiceOver();
											HackedPacificStandardSecurityPad = true;
											FingerprintHack.HackFinishedID = 0;
											Random random13 = new Random();
											Waittime = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 25000, 60000);
											TriggerCops = true;
											QuitHacking = true;
											DisplayHelpTextThisFrame("~y~ Lester ~w~ : ok the jammer is decoding the safes security, defend the cops");
											DefendingCops = true;
											DictToUse = "anim@heists@ornate_bank@hack";
											LoadDict(DictToUse);
											LoadDict(DictToUse);
											Game.Player.Character.Position = new Vector3(253.7687f, 228.1796f, 100.6833f);
											Game.Player.Character.Heading = 68.69633f;
											Vector3 offsetPosition15 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.4f));
											HackScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition15.X, offsetPosition15.Y, offsetPosition15.Z, 0.0, 0.0, Game.Player.Character.Heading, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, HackScene1, offsetPosition15.X, offsetPosition15.Y, offsetPosition15.Z, 0.0, 0.0, Game.Player.Character.Heading, true);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, HackScene1, LoadDict(DictToUse), "hack_exit", 1000.0, -8.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, HackScene1, "hack_exit", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, HackCard, HackScene1, LoadDict(DictToUse), "hack_exit_card", 1000.0, -8.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HackCard, HackScene1, "hack_exit_card", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BagProp, HackScene1, LoadDict(DictToUse), "hack_exit_bag", 1000.0, -8.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, HackScene1, "hack_exit_bag", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Laptop, HackScene1, LoadDict(DictToUse), "hack_exit_laptop", 1000.0, -8.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Laptop, HackScene1, "hack_exit_laptop", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
										}
										if (Game.IsControlJustPressed(Control.Context))
										{
											hackingScalwform = 0;
											isHacking = false;
											QuitHacking = true;
											HackingSuccess = false;
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
											int num109 = hackingScalwform;
											Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num109);
											hackingScalwform = 0;
											Hacking_Index = -1;
											ActiveHack = false;
											DictToUse = "anim@heists@ornate_bank@hack";
											LoadDict(DictToUse);
											LoadDict(DictToUse);
											Game.Player.Character.Position = new Vector3(253.7687f, 228.1796f, 100.6833f);
											Game.Player.Character.Heading = 68.69633f;
											Vector3 offsetPosition16 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.4f));
											HackScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition16.X, offsetPosition16.Y, offsetPosition16.Z, 0.0, 0.0, Game.Player.Character.Heading, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, HackScene1, offsetPosition16.X, offsetPosition16.Y, offsetPosition16.Z, 0.0, 0.0, Game.Player.Character.Heading, true);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, HackScene1, LoadDict(DictToUse), "hack_exit", 1000.0, -8.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, HackScene1, "hack_exit", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, HackCard, HackScene1, LoadDict(DictToUse), "hack_exit_card", 1000.0, -8.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HackCard, HackScene1, "hack_exit_card", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BagProp, HackScene1, LoadDict(DictToUse), "hack_exit_bag", 1000.0, -8.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, HackScene1, "hack_exit_bag", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Laptop, HackScene1, LoadDict(DictToUse), "hack_exit_laptop", 1000.0, -8.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Laptop, HackScene1, "hack_exit_laptop", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
										}
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, HackScene1) >= 1065353216)
									{
										DictToUse = "anim@heists@ornate_bank@hack";
										LoadDict(DictToUse);
										LoadDict(DictToUse);
										Game.Player.Character.Position = new Vector3(253.7687f, 228.1796f, 100.6833f);
										Game.Player.Character.Heading = 68.69633f;
										Vector3 offsetPosition17 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.4f));
										HackScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition17.X, offsetPosition17.Y, offsetPosition17.Z, 0.0, 0.0, Game.Player.Character.Heading, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, HackScene1, offsetPosition17.X, offsetPosition17.Y, offsetPosition17.Z, 0.0, 0.0, Game.Player.Character.Heading, true);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, HackScene1, LoadDict(DictToUse), "hack_loop", 1000.0, -8.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, HackScene1, "hack_loop", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, HackCard, HackScene1, LoadDict(DictToUse), "hack_loop_card", 1000.0, -8.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, HackCard, HackScene1, "hack_loop_card", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BagProp, HackScene1, LoadDict(DictToUse), "hack_loop_bag", 1000.0, -8.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, HackScene1, "hack_loop_bag", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Laptop, HackScene1, LoadDict(DictToUse), "hack_loop_laptop", 1000.0, -8.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Laptop, HackScene1, "hack_loop_laptop", LoadDict(DictToUse), 1000.0, -8f, -8f, Game.Player.Character.Heading, 1148846080, 0);
										if (!Hackedkeypad)
										{
											Hackedkeypad = true;
											Game.Player.Character.IsPositionFrozen = true;
											Notification.Show("~y~ You can cancel The hack at any time by pressing ~INPUT_CONTEXT~");
											HackingSuccess = false;
											DisplayHelpTextThisFrame("~y~ Lester ~w~ : Select ~b~ My Computer ~w~ > ~b~ External Device ~w~ > ~y~ Hackconnect.exe ~w~ to Start the Hack");
											FingerprintHack.RunBruteforceHack_Both();
											ActiveHack = true;
										}
									}
								}
							}
						}
					}
					if (DefendingCops)
					{
						if (HackCard != null)
						{
							HackCard.Delete();
						}
						if (BagProp != null)
						{
							BagProp.Delete();
						}
						if (Laptop != null)
						{
							Laptop.Delete();
						}
						Game.Player.Character.IsPositionFrozen = false;
						BankHeistStage = 4;
						if (BankHeistStage <= 6 && SwatTimer <= Game.GameTime)
						{
							SwatTimer = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 35000, 90000);
							Random random14 = new Random();
							int num110 = random14.Next(1, 100);
							int num111 = random14.Next(1, 100);
							if (num110 < 20 && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 200f))
							{
								Ped ped12 = GlobalHelper.CreatePed(PedHash.Swat01SMY, DoorApos.Around(10f), 24f);
								if (Difficulty == 0)
								{
									ped12.Health = 200;
									ped12.Weapons.Give(WeaponHash.Pistol, 9999, true, true);
								}
								if (Difficulty == 1)
								{
									ped12.Health = 300;
									ped12.Weapons.Give(WeaponHash.SMG, 9999, true, true);
								}
								if (Difficulty == 2)
								{
									ped12.Armor = 100;
									ped12.Health = 350;
									ped12.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								}
								if (Difficulty == 3)
								{
									ped12.Armor = 200;
									ped12.Health = 400;
									ped12.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
								}
								if (Difficulty == 4)
								{
									ped12.Armor = 300;
									ped12.Health = 500;
									ped12.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
								}
								if (Difficulty == 5)
								{
									ped12.Health = 700;
									ped12.Armor = 400;
									ped12.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
								}
								ped12.Task.FightAgainst(Game.Player.Character);
								if (!PacificStandardCivsAndTellerInCower)
								{
									PacificStandardCivsAndTellerInCower = true;
									foreach (Ped teller6 in Tellers)
									{
										if (teller6 != null)
										{
											teller6.BlockPermanentEvents = true;
											teller6.Task.Cower(-1);
											teller6.AlwaysKeepTask = true;
										}
									}
									foreach (Ped civ5 in Civs)
									{
										if (civ5 != null)
										{
											civ5.BlockPermanentEvents = true;
											civ5.Task.Cower(-1);
											civ5.AlwaysKeepTask = true;
										}
									}
								}
								Guards.Add(ped12);
								SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped12, SmartGuards.EnemyGroup.Security));
								Notification.Show("~y~ Lester ~w~ : Swat have been dropped through the roof to free hostages");
							}
							if (num111 < 20 && num110 > 20 && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 200f))
							{
								Ped ped13 = GlobalHelper.CreatePed(PedHash.Swat01SMY, DoorBPos.Around(10f), 24f);
								ped13.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								ped13.Task.FightAgainst(Game.Player.Character);
								if (!PacificStandardCivsAndTellerInCower)
								{
									PacificStandardCivsAndTellerInCower = true;
									foreach (Ped teller7 in Tellers)
									{
										if (teller7 != null)
										{
											teller7.BlockPermanentEvents = true;
											teller7.Task.Cower(-1);
											teller7.AlwaysKeepTask = true;
										}
									}
									foreach (Ped civ6 in Civs)
									{
										if (civ6 != null)
										{
											civ6.BlockPermanentEvents = true;
											civ6.Task.Cower(-1);
											civ6.AlwaysKeepTask = true;
										}
									}
								}
								Guards.Add(ped13);
								SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped13, SmartGuards.EnemyGroup.Security));
								Notification.Show("~y~ Lester ~w~ : Swat have been dropped through the roof to free hostiges");
							}
						}
						if (VaultDoor != null)
						{
							VaultDoor.Rotation = new Vector3(0f, 0f, 160f);
						}
						UI.DrawTime("VAULT", Waittime - Game.GameTime);
						if (Game.GameTime >= Waittime && Waittime != 0)
						{
							Waittime = 0;
							Prop[] nearbyProps61 = World.GetNearbyProps(FleecaJammerPos, 10f);
							Prop[] array65 = nearbyProps61;
							foreach (Prop prop71 in array65)
							{
								if (prop71 != null)
								{
									prop71.IsPositionFrozen = false;
								}
							}
							AddCratesPacStandard();
							DefendingCops = false;
							VaultOpen = true;
							ManageCellphoneCall.DialogueLines.Clear();
							ManageCellphoneCall.DialogueLines.Add("OBHF_AIAA");
							ManageCellphoneCall.Speaker = "Lester";
							ManageCellphoneCall.StartVoiceOver();
							Notification.Show("~y~ Lester ~w~ : Ok head to the vault to grab the money!");
							BankHeistStage = 5;
							OverideTasks = true;
							Random random15 = new Random();
						}
					}
					if (VaultOpen)
					{
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
						UI.DrawPayment("TAKE", LocalTake);
						if (SwatTimer != 420)
						{
							SwatTimer++;
						}
						if (SwatTimer == 420)
						{
							SwatTimer = 0;
							Random random16 = new Random();
							int num113 = random16.Next(1, 100);
							int num114 = random16.Next(1, 100);
							if (num113 < 20 && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 200f))
							{
								Ped ped14 = GlobalHelper.CreatePed(PedHash.Swat01SMY, DoorApos.Around(10f), 24f);
								ped14.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								ped14.Task.FightAgainst(Game.Player.Character);
								if (!PacificStandardCivsAndTellerInCower)
								{
									PacificStandardCivsAndTellerInCower = true;
									foreach (Ped teller8 in Tellers)
									{
										if (teller8 != null)
										{
											teller8.BlockPermanentEvents = true;
											teller8.Task.Cower(-1);
											teller8.AlwaysKeepTask = true;
										}
									}
									foreach (Ped civ7 in Civs)
									{
										if (civ7 != null)
										{
											civ7.BlockPermanentEvents = true;
											civ7.Task.Cower(-1);
											civ7.AlwaysKeepTask = true;
										}
									}
								}
								Guards.Add(ped14);
								SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped14, SmartGuards.EnemyGroup.Security));
								Notification.Show("~y~ Lester ~w~ : Swat have been dropped through the roof to free hostiges");
							}
							if (num114 < 20 && num113 > 20 && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 200f))
							{
								Ped ped15 = GlobalHelper.CreatePed(PedHash.Swat01SMY, DoorBPos.Around(10f), 24f);
								ped15.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
								ped15.Task.FightAgainst(Game.Player.Character);
								if (!PacificStandardCivsAndTellerInCower)
								{
									PacificStandardCivsAndTellerInCower = true;
									foreach (Ped teller9 in Tellers)
									{
										if (teller9 != null)
										{
											teller9.BlockPermanentEvents = true;
											teller9.Task.Cower(-1);
											teller9.AlwaysKeepTask = true;
										}
									}
									foreach (Ped civ8 in Civs)
									{
										if (civ8 != null)
										{
											civ8.BlockPermanentEvents = true;
											civ8.Task.Cower(-1);
											civ8.AlwaysKeepTask = true;
										}
									}
								}
								Guards.Add(ped15);
								SmartGuards.GuardGroup[13].GroupList.Add(new SmartGuards.SmartAiV2(ped15, SmartGuards.EnemyGroup.Security));
								Notification.Show("~y~ Lester ~w~ : Swat have been dropped through the roof to free hostiges");
							}
						}
						OverideTasks = false;
						escapingcops = true;
					}
					if (escapingcops)
					{
						if (ManageRobberyAnims.CurrentPay >= 1000)
						{
							if (ManageRobberyAnims.ValuableLootLeft <= 0 && BankHeistStage == 5)
							{
								BankHeistStage = 6;
							}
							if (ManageRobberyAnims.ValuableLootLeft > 0)
							{
								if (BankHeistStage <= 6)
								{
									ManageCellphoneCall.DialogueLines.Clear();
									ManageCellphoneCall.DialogueLines.Add("OBHF_AKAA");
									ManageCellphoneCall.Speaker = "Lester";
									ManageCellphoneCall.StartVoiceOver();
									BankHeistStage = 5;
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, DoorDPos, "<", 5f))
								{
									foreach (ManageRobberyAnims.Lootable crate_Valuable in ManageRobberyAnims.Crate_Valuables)
									{
										if (crate_Valuable != null && crate_Valuable.LootBlip != null)
										{
											crate_Valuable.LootBlip.Alpha = 0;
										}
									}
									BankHeistStage = 7;
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, DoorDPos, "<", 5f) && BankHeistStage == 6)
							{
								foreach (ManageRobberyAnims.Lootable crate_Valuable2 in ManageRobberyAnims.Crate_Valuables)
								{
									if (crate_Valuable2 != null && crate_Valuable2.LootBlip != null)
									{
										crate_Valuable2.LootBlip.Alpha = 0;
									}
								}
								BankHeistStage = 7;
							}
							Vector3 position19 = Game.Player.Character.Position;
							if (position19.Z < 105f && BankHeistStage == 7 && GetDistance_VDIST2(Game.Player.Character.Position, FleecaJammerPos, "<", 10f) && ManageRobberyAnims.ValuableLootLeft > 0)
							{
								ManageCellphoneCall.DialogueLines.Clear();
								ManageCellphoneCall.DialogueLines.Add("OBHF_AKAA");
								ManageCellphoneCall.Speaker = "Lester";
								ManageCellphoneCall.StartVoiceOver();
								BankHeistStage = 5;
							}
							if (position19.Z >= 105f && BankHeistStage == 7)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, DoorBPos, "<", 10f))
								{
									BankHeistStage = 8;
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, DoorApos, "<", 10f))
								{
									BankHeistStage = 8;
								}
							}
						}
						if (ManageRobberyAnims.Crate_Valuables.Count > 0)
						{
							bool flag7 = true;
							foreach (ManageRobberyAnims.Lootable crate_Valuable3 in ManageRobberyAnims.Crate_Valuables)
							{
								if (crate_Valuable3 != null && crate_Valuable3.LootBlip != null && crate_Valuable3.LootBlip.Alpha != 0)
								{
									flag7 = false;
								}
							}
							if (flag7 && !NotifyEnd)
							{
								NotifyEnd = true;
								HeadingtoArcade = true;
								escapingcops = false;
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaBankMarker, "<", 200f) && (!DoorBLocked || !DoorALocked))
						{
							if (JuggerNautTimer != 80)
							{
								JuggerNautTimer++;
							}
							if (JuggerNautTimer == 80)
							{
								JuggerNautTimer = 0;
								Random random17 = new Random();
								int num115 = random17.Next(1, 100);
								int num116 = random17.Next(1, 100);
								if (num115 < 20)
								{
									int num173 = 33;
									if (num116 > 33)
									{
										int num174 = 66;
									}
									if (num116 > 66)
									{
										int num175 = 100;
									}
								}
							}
						}
						if (DoorBLocked)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, DoorBPos, "<", 30f) && DoorBLocked)
							{
								World.DrawMarker(MarkerType.Cylinder, DoorBPos, Vector3.Zero, Vector3.Zero, new Vector3(1f, 1f, 1f), Color.Red);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, DoorBPos, "<", 2f))
							{
								if (Game.IsControlJustPressed(Control.Context))
								{
									DoorBLocked = false;
									Prop[] nearbyProps62 = World.GetNearbyProps(new Vector3(259.2f, 203.6f, 105f), 5f, "hei_prop_hei_bankdoor_new");
									Prop[] array66 = nearbyProps62;
									foreach (Prop prop72 in array66)
									{
										if (prop72 != null)
										{
											prop72.IsPositionFrozen = false;
										}
									}
								}
								DisplayHelpTextThisFrame("Door B has been unlocked");
								TriggerCops = false;
								if (TriggerCops)
								{
									Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								}
							}
						}
						if (InsurgentBLip != null && Insurgent != null)
						{
							InsurgentBLip.Position = Insurgent.Position;
						}
						if (KurumaBlip != null && Kuruma != null)
						{
							KurumaBlip.Position = Kuruma.Position;
						}
						if (Game.Player.WantedLevel == 0)
						{
							escapingcops = false;
							HeadingtoArcade = true;
						}
					}
					if (HeadingtoArcade)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(231f, 209f, 105f), "<", 250f))
						{
							TriggerCops = false;
						}
						if (NotifyEnd && CamerasLeft == 0)
						{
							CamerasLeft = -1;
						}
						Bank.ShowRoute = false;
						if (InsurgentBLip != null && Insurgent != null)
						{
							InsurgentBLip.Position = Insurgent.Position;
						}
						if (KurumaBlip != null && Kuruma != null)
						{
							KurumaBlip.Position = Kuruma.Position;
						}
						if (Game.Player.WantedLevel == 0 && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, ">", 200f))
						{
							PlayingCutscene = false;
							Prop[] nearbyProps63 = World.GetNearbyProps(new Vector3(259.2f, 203.6f, 105f), 5f, "hei_prop_hei_bankdoor_new");
							Prop[] array67 = nearbyProps63;
							foreach (Prop prop73 in array67)
							{
								if (prop73 != null)
								{
									prop73.IsPositionFrozen = false;
								}
							}
							nearbyProps63 = World.GetNearbyProps(new Vector3(232.6f, 215.5f, 106f), 5f, "hei_prop_hei_bankdoor_new");
							Prop[] array68 = nearbyProps63;
							foreach (Prop prop74 in array68)
							{
								if (prop74 != null)
								{
									prop74.IsPositionFrozen = false;
								}
							}
							if (Lester != null)
							{
								Lester.Delete();
							}
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
							int crewAlive2 = 1;
							if (RobberA != null && RobberA.IsAlive)
							{
								crewAlive2++;
							}
							float num120 = ManageRobberyAnims.CurrentPay;
							float num121 = ManageRobberyAnims.CurrentPay;
							if (Bank != null)
							{
								Bank.Delete();
							}
							Wait(10);
							Main.TotalCrewEarned += 0f;
							Main.TotalPlayerEarned += num121;
							XP.CompleteHeist_PacStandard++;
							Main.SaveCuts();
							SetNoLongerInHeist();
							MissionScreen.SetPassNoAi("The Pacific Standard Bank", (int)num121, 0, "The Player has died", crewAlive2, HTime, 3);
							int num122 = Game.GameTime + 30000;
							while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num122)
							{
								Script.Wait(0);
							}
							Script.Wait(2000);
							XP.AddXP_Percent(XP.CompleteHeist_XPPercent_PacStandard);
							ManageRobberyAnims.CurrentPay = 0;
							HeistTime = 0f;
							GTA.UI.Screen.FadeOut(300);
							Wait(300);
							if (Bank != null)
							{
								Bank.Delete();
							}
							if (DoorAblip != null)
							{
								DoorAblip.Delete();
							}
							if (DoorBblip != null)
							{
								DoorBblip.Delete();
							}
							if (DoorCblip != null)
							{
								DoorCblip.Delete();
							}
							if (DoorDblip != null)
							{
								DoorDblip.Delete();
							}
							foreach (Ped guard11 in Guards)
							{
								if (guard11 != null)
								{
									guard11.Delete();
								}
							}
							foreach (Ped teller10 in Tellers)
							{
								if (teller10 != null)
								{
									teller10.Delete();
								}
							}
							foreach (Ped civ9 in Civs)
							{
								if (civ9 != null)
								{
									civ9.Delete();
								}
							}
							LockedDoors = false;
							CivsAreFreakingOut = false;
							TargetingTime = 0;
							if (Manager != null)
							{
								Manager.Delete();
							}
							if (Insurgent != null)
							{
								Insurgent.Delete();
							}
							if (Kuruma != null)
							{
								Kuruma.Delete();
							}
							if (InsurgentBLip != null)
							{
								InsurgentBLip.Delete();
							}
							if (KurumaBlip != null)
							{
								KurumaBlip.Delete();
							}
							Wait(300);
							GTA.UI.Screen.FadeIn(300);
							DisplayHelpTextThisFrame("~y~ Lester ~w~ : Pacific Standard Job Complete");
							Random random18 = new Random();
							InHeist = false;
							ManagePlayerFunds.AddFunds((int)num121);
							HeadingtoArcade = false;
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Notification.Show("~b~ Heist Complete ~w~");
							PlayingPacificStandard = false;
							MissionOn = false;
							CompletedPacificStandard = 1;
							SmartGuards.RemoveAIGuards();
						}
					}
				}
				if (Game.Player.IsAlive || !InHeist)
				{
					break;
				}
				PlayingPacificStandard = false;
				Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
				Prop[] nearbyProps64 = World.GetNearbyProps(new Vector3(259.2f, 203.6f, 105f), 5f, "hei_prop_hei_bankdoor_new");
				Prop[] array69 = nearbyProps64;
				foreach (Prop prop75 in array69)
				{
					if (prop75 != null)
					{
						prop75.IsPositionFrozen = false;
					}
				}
				nearbyProps64 = World.GetNearbyProps(new Vector3(232.6f, 215.5f, 106f), 5f, "hei_prop_hei_bankdoor_new");
				Prop[] array70 = nearbyProps64;
				foreach (Prop prop76 in array70)
				{
					if (prop76 != null)
					{
						prop76.IsPositionFrozen = false;
					}
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
				InHeist = false;
				GTA.UI.Screen.FadeOut(300);
				Wait(300);
				if (Bank != null)
				{
					Bank.Delete();
				}
				if (DoorAblip != null)
				{
					DoorAblip.Delete();
				}
				if (DoorBblip != null)
				{
					DoorBblip.Delete();
				}
				if (DoorCblip != null)
				{
					DoorCblip.Delete();
				}
				if (DoorDblip != null)
				{
					DoorDblip.Delete();
				}
				foreach (Ped guard12 in Guards)
				{
					if (guard12 != null)
					{
						guard12.Delete();
					}
				}
				foreach (Ped teller11 in Tellers)
				{
					if (teller11 != null)
					{
						teller11.Delete();
					}
				}
				foreach (Ped civ10 in Civs)
				{
					if (civ10 != null)
					{
						civ10.Delete();
					}
				}
				LockedDoors = false;
				CivsAreFreakingOut = false;
				TargetingTime = 0;
				if (Manager != null)
				{
					Manager.Delete();
				}
				if (Insurgent != null)
				{
					Insurgent.Delete();
				}
				if (Kuruma != null)
				{
					Kuruma.Delete();
				}
				if (InsurgentBLip != null)
				{
					InsurgentBLip.Delete();
				}
				if (KurumaBlip != null)
				{
					KurumaBlip.Delete();
				}
				Wait(300);
				GTA.UI.Screen.FadeIn(300);
				placedJammer = false;
				DefendingCops = false;
				HeadingtoArcade = false;
				escapingcops = false;
				VaultOpen = false;
				HeistTime = 0f;
				DisplayHelpTextThisFrame("~y~ Lester ~w~ : Pacific Standard Job Failed");
				Wait(20000);
				Notification.Show("~b~ The Pacific Standard Heist was failed!~w~");
				MissionScreen.SetFail("The Pacific Standard Heist", 0, 0, "you Died!", 0, HeistTime, 3);
				SetNoLongerInHeist();
				MissionOn = false;
				break;
			}
			case HeistIn.Paleto:
			{
				UI.RenderQue = 0f;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
				if (!PlayingPaleto)
				{
					break;
				}
				Prop prop77 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -111.48f, 6463.94f, 31.98499f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bank4door02"), 0, 0, 0);
				if (prop77 != null)
				{
					prop77.IsPositionFrozen = false;
				}
				Prop prop78 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -109.65f, 6462.11f, 31.98499f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_bank4door01"), 0, 0, 0);
				if (prop78 != null)
				{
					prop78.IsPositionFrozen = false;
				}
				Ped[] nearbyPeds4 = World.GetNearbyPeds(new Vector3(-105.32f, 6467.293f, 31.6f), 60f);
				Ped[] array71 = nearbyPeds4;
				foreach (Ped ped16 in array71)
				{
					if (!(ped16.Model != PedHash.Security01SMM) || !(ped16.CurrentVehicle == null))
					{
						continue;
					}
					if (ped16.Killer == Game.Player.Character && ped16.Opacity == 255)
					{
						KilledCops++;
						ped16.Opacity = 254;
					}
					if (KilledCops >= 2)
					{
						BankHeistResponceType = 1;
					}
					if (KilledCops >= 5)
					{
						BankHeistResponceType = 2;
					}
					RelationshipGroup relationshipGroup8 = Function.Call<int>(Hash.GET_HASH_KEY, "COP");
					if (!(ped16.RelationshipGroup == relationshipGroup8))
					{
						continue;
					}
					if (LocalTake == 0)
					{
						if (BankHeistResponceType == 0)
						{
							if (Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped16, Game.Player.Character, 19))
							{
								ped16.Task.AimAt(Game.Player.Character.Position, 1);
							}
							else
							{
								ped16.Task.AimAt(Game.Player.Character.Position, 1);
							}
						}
						if (BankHeistResponceType == 1 && Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped16, Game.Player.Character, 19))
						{
							ped16.Task.ShootAt(Game.Player.Character.Position);
						}
						if (BankHeistResponceType != 2)
						{
						}
						continue;
					}
					if (BankHeistResponceType == 0)
					{
						if (Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped16, Game.Player.Character, 19))
						{
							ped16.Task.ShootAt(Game.Player.Character.Position);
						}
						else
						{
							ped16.Task.AimAt(Game.Player.Character.Position, 1);
						}
					}
					if (BankHeistResponceType == 1 && Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped16, Game.Player.Character, 19))
					{
						ped16.Task.ShootAt(Game.Player.Character.Position);
					}
					int bankHeistResponceType2 = BankHeistResponceType;
					int num176 = 2;
				}
				if (Game.Player.IsAlive)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-104.6049f, 6473.44434f, 31.73f), "<", 100f))
					{
						int copTime = CopTime;
						int num177 = 0;
						if (CopTime > 0 && PlayeringThirmiteDoor1)
						{
							if (ManageRobberyAnims.CurrentPay <= 0)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
								Game.Player.WantedLevel = 0;
							}
							Game.MaxWantedLevel = 0;
						}
						if (Game.Player.Character.IsShooting && !PacificStandardCivsAndTellerInCower)
						{
							PacificStandardCivsAndTellerInCower = true;
							foreach (Ped teller12 in Tellers)
							{
								if (teller12 != null)
								{
									teller12.BlockPermanentEvents = true;
									teller12.Task.Cower(-1);
									teller12.AlwaysKeepTask = true;
								}
							}
							foreach (Ped civ11 in Civs)
							{
								if (civ11 != null)
								{
									civ11.BlockPermanentEvents = true;
									civ11.Task.Cower(-1);
									civ11.AlwaysKeepTask = true;
								}
							}
						}
					}
					if (InHeist)
					{
						HeistTime += 0.35f;
					}
					if (NotifyEnd && CamerasLeft <= 1)
					{
						HeadingtoArcade = true;
					}
					if (CamerasLeft > 0 && SecurityCam.Count > 0)
					{
						foreach (Prop item40 in SecurityCam)
						{
							if (!(item40 != null))
							{
								continue;
							}
							if (item40.AttachedBlip == null)
							{
								item40.AddBlip();
								item40.AttachedBlip.Sprite = BlipSprite.Standard;
								item40.AttachedBlip.Color = BlipColor.Red2;
								item40.AttachedBlip.Name = "Camera";
								item40.AttachedBlip.Scale = 0.5f;
							}
							if (!GetDistance_VDIST2(Game.Player.Character.Position, item40.Position, "<", 100f))
							{
								continue;
							}
							if (item40.Health > 999 && ShowCCTVBlipAliveDebug)
							{
								World.DrawMarker(MarkerType.Sphere, item40.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f), Color.White);
							}
							if (item40.Health <= 999)
							{
								if (item40.Opacity == 255)
								{
									item40.Opacity = 254;
									item40.AttachedBlip.Alpha = 0;
									CamerasLeft--;
									Notification.Show("~y~ Lester ~w~ Cameras Left : " + (CamerasLeft - 1));
								}
								if (ShowCCTVBlipDeadDebug)
								{
									World.DrawMarker(MarkerType.Sphere, item40.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f), Color.Red);
								}
							}
							if (CamerasLeft <= 1 && CamerasLeft != -100)
							{
								CamerasLeft = -100;
								HeadingtoArcade = true;
								Notification.Show("~y~ Lester ~w~ All Cameras Destroyed!");
							}
						}
					}
					if (CamerasLeft > 2)
					{
						HeadingtoArcade = false;
					}
					if (!GetCameras && GetDistance_VDIST2(Game.Player.Character.Position, PeletoJammerPos, "<", 30f))
					{
						GetCameras = true;
						Vector3 peletoJammerPos = PeletoJammerPos;
						int num126 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, peletoJammerPos.X, peletoJammerPos.Y, peletoJammerPos.Z);
						if (num126 != 0)
						{
							Function.Call(Hash.REFRESH_INTERIOR, num126);
							Notification.Show("Refreshed Bank");
							Wait(1000);
							if (Class1.teller != null)
							{
								Class1.teller.Delete();
							}
							OpenedDoor = false;
							tellerwalkStage = 0;
							DoorOpenAmt = 0f;
							tellerwalkStage = 0;
							Class1.teller = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AMY), new Vector3(-110.785f, 6470.347f, 31.6f), 125f);
							Wait(1000);
							Offset = Class1.teller.GetOffsetPosition(new Vector3(-6f, -1f, 0f));
							Class1.teller.BlockPermanentEvents = true;
							Class1.teller.Task.StandStill(2000);
							Class1.teller.AlwaysKeepTask = true;
							Class1.teller.Weapons.Give(WeaponHash.Pistol, 20, true, true);
							Class1.teller.Weapons.Select(WeaponHash.Pistol);
							SecurityCam.Clear();
							Prop[] nearbyProps65 = World.GetNearbyProps(new Vector3(peletoJammerPos.X, peletoJammerPos.Y, peletoJammerPos.Z), 200f, RequestModel("prop_cctv_cam_07a"));
							if (nearbyProps65.Length != 0)
							{
								Prop[] array72 = nearbyProps65;
								foreach (Prop prop79 in array72)
								{
									if (prop79 != null)
									{
										prop79.Opacity = 255;
										SecurityCam.Add(prop79);
									}
								}
							}
							nearbyProps65 = World.GetNearbyProps(new Vector3(peletoJammerPos.X, peletoJammerPos.Y, peletoJammerPos.Z), 200f, RequestModel("prop_cctv_cam_06a"));
							if (nearbyProps65.Length != 0)
							{
								Prop[] array73 = nearbyProps65;
								foreach (Prop prop80 in array73)
								{
									if (prop80 != null)
									{
										prop80.Opacity = 255;
										SecurityCam.Add(prop80);
										prop80.AddBlip();
										prop80.AttachedBlip.Sprite = BlipSprite.Standard;
										prop80.AttachedBlip.Color = BlipColor.Red2;
										prop80.AttachedBlip.Name = "Camera";
										prop80.AttachedBlip.Scale = 0.5f;
										CameraBlips.Add(prop80.AttachedBlip);
									}
								}
							}
							nearbyProps65 = World.GetNearbyProps(new Vector3(peletoJammerPos.X, peletoJammerPos.Y, peletoJammerPos.Z), 200f, RequestModel("v_ilev_gb_teldr"));
							if (nearbyProps65.Length != 0)
							{
								Prop[] array74 = nearbyProps65;
								foreach (Prop prop81 in array74)
								{
									if (prop81 != null)
									{
										prop81.IsPositionFrozen = false;
									}
								}
							}
						}
						CamerasLeft = SecurityCam.Count - 1;
						Notification.Show("~y~ Lester ~W~ :  Multiple ~r~Cameras~w~ have been Found, You need to destroy each one before escaping");
					}
					if (placedJammer)
					{
						if (Game.Player.Character.Position.DistanceTo(PeletoJammerPos) < 20f)
						{
							GTA.UI.Screen.ShowSubtitle("Enter the bank and place the ~y~Thermal Charge~w~ on the Vault Door");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Make your way to the ~y~Paleto Bay Bank~w~");
						}
						Prop[] nearbyProps66 = World.GetNearbyProps(new Vector3(-104.6049f, 6473.44434f, 31.73f), 1f);
						if (nearbyProps66.Length != 0)
						{
							Prop[] array75 = nearbyProps66;
							foreach (Prop prop82 in array75)
							{
								if (prop82 != null)
								{
									prop82.Rotation = new Vector3(0f, 0f, 45f);
								}
							}
						}
						Prop[] nearbyProps67 = World.GetNearbyProps(Game.Player.Character.Position, 1f);
						if (!ThirmiteDoor1)
						{
							if (Game.Player.Character.Position.DistanceTo(PeletoJammerPos) < 10f && !PlayeringThirmiteDoor1)
							{
								World.DrawMarker(MarkerType.Cylinder, PeletoJammerPos, Vector3.Zero, Vector3.Zero, new Vector3(0.8f, 0.8f, 0.8f), Color.Goldenrod);
							}
							if (Game.Player.Character.Position.DistanceTo(PeletoJammerPos) < 2f)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Plant Thermal Charge");
								if (Game.IsControlJustPressed(Control.Context) && !PlayeringThirmiteDoor1)
								{
									PlayeringThirmiteDoor1 = true;
									Ped character13 = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character13, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character13, 9, 0, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character13, 9, 0, 0, 1);
									}
									DictToUse = "anim_heist@hs3f@ig13_thermal_charge@thermal_charge@male@";
									DictToUse2 = "anim@heists@ornate_bank@thermal_charge";
									LoadDict(DictToUse);
									LoadDict(DictToUse);
									LoadDict(DictToUse2);
									LoadDict(DictToUse2);
									Function.Call(Hash.CREATE_MODEL_SWAP, 257.4089f, 220.2059f, 106.45f, 5, "hei_v_ilev_fh_heistdoor1", "hei_v_ilev_bk_gate_molten", 0);
									if (_thermiteProp != null)
									{
										_thermiteProp.Delete();
									}
									if (BagProp != null)
									{
										BagProp.Delete();
									}
									BagProp = GlobalHelper.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), new Vector3(-105.03f, 6471.854f, 31.6f), new Vector3(1f, 0f, 0f), false, false);
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, -105.45f, 6472.604f, 31.6f, 0.0, 0.0, 35f, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@heists@ornate_bank@thermal_charge"), "thermal_charge", 1f, -1f, 3341, 16, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BagProp, Scene1, "bag_thermal_charge", LoadDict("anim@heists@ornate_bank@thermal_charge"), 1f, -1f, 0, 1148846080);
									Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, BagProp);
									_thermiteProp = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_thermite"), Game.Player.Character.Position, false, false);
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, _thermiteProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, -90f, 0f, 0f, 0, 0, 0, 0, 2, 1);
									Function.Call(Hash.PLAY_ENTITY_ANIM, _thermiteProp, "bomb_thermal_charge", "anim@heists@ornate_bank@thermal_charge", 1000.0, 1, 1, 1, 0, 0);
								}
							}
							if (PlayeringThirmiteDoor1)
							{
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1056353216 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1057353216 && !DefendingCops)
								{
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ornate_heist");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ornate_heist");
									PTFX1 = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_heist_ornate_thermal_burn", _thermiteProp, 0f, 0f, 0f, 1f, 0, 0, 0);
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
									PTFX2 = GlobalHelper.CreatePTFX_LoopedOnEntity("proj_flare_fuse_fp", _thermiteProp, 0f, 0f, 0f, 10f, 0, 0, 0);
									Function.Call(Hash.CLEAR_ALL_HELP_MESSAGES);
									TimeTillPaletoVaultOpens = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 15000, 120000);
									TimeSpan timeSpan4 = TimeSpan.FromMilliseconds(TimeTillPaletoVaultOpens - Game.GameTime);
									if (timeSpan4.Minutes == 0)
									{
										Notification.Show("~y~Lester~w~ : Vault opens in " + timeSpan4.Seconds + " Seconds");
									}
									else
									{
										Notification.Show("~y~Lester~w~ : Vault opens in " + timeSpan4.Minutes + " Minutes & " + timeSpan4.Seconds + " Seconds");
									}
									Random random19 = new Random();
									DefendingCops = true;
								}
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 && !ThirmiteDoor1)
								{
									placedJammer = false;
									ThirmiteDoor1 = true;
									Game.Player.Character.Task.ClearAll();
									if (BagProp != null)
									{
										BagProp.Delete();
									}
									if (_thermiteProp != null)
									{
										_thermiteProp.Detach();
										_thermiteProp.IsPositionFrozen = true;
									}
									Ped character14 = Game.Player.Character;
									if (Game.Player.Character.Model == PedHash.Trevor)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character14, 9, 1, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Michael)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character14, 9, 2, 0, 1);
									}
									if (Game.Player.Character.Model == PedHash.Franklin)
									{
										Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character14, 9, 6, 0, 1);
									}
								}
							}
						}
						InHeist = true;
					}
					if (ThirmiteDoor1)
					{
						if (ManageRobberyAnims.CurrentPay <= 0)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
							Game.Player.WantedLevel = 0;
						}
						if (Game.GameTime >= Waittime && Waittime != 0)
						{
							Waittime = 0;
							PlayeringThirmiteDoor1 = false;
							Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX2, false);
							Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PTFX1, false);
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Gate_Lock_Break", _thermiteProp, "DLC_HEISTS_ORNATE_BANK_FINALE_SOUNDS", 0, 0);
							Prop[] nearbyProps68 = World.GetNearbyProps(DoorCPos, 5f, "hei_v_ilev_bk_gate_pris");
							Prop[] array76 = nearbyProps68;
							foreach (Prop prop83 in array76)
							{
								if (prop83 != null)
								{
									prop83.IsPositionFrozen = false;
								}
							}
							if (_thermiteProp != null)
							{
								_thermiteProp.Delete();
							}
							Function.Call<bool>(Hash.IS_SYNCHRONIZED_SCENE_RUNNING, Scene1);
						}
					}
					if (!placedJammer && !VaultOpen && !CutterDrilledOpenVault)
					{
						GTA.UI.Screen.ShowSubtitle("Hold off the ~r~Cops~w~ and Wait for the ~y~Vault Door~w~ to open");
						Vector3 offset = Offset;
						if (Class1.teller != null)
						{
							if (GetDistance_VDIST2(Class1.teller.Position, Offset, "<", 1.5f))
							{
								if (tellerwalkStage == 0)
								{
									tellerwalkStage = 1;
									Offset = Class1.teller.GetOffsetPosition(new Vector3(-2f, 0f, 0f));
									Function.Call(Hash.TASK_PLAY_ANIM, Class1.teller, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Class1.teller, Offset.X, Offset.Y, Offset.Y, 1f, -1, Class1.teller.Heading + 100f, -0.1f);
								}
								else if (tellerwalkStage == 1)
								{
									tellerwalkStage = 2;
									Offset = Class1.teller.GetOffsetPosition(new Vector3(4f, 0f, 0f));
									Function.Call(Hash.TASK_PLAY_ANIM, Class1.teller, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
									Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Class1.teller, Offset.X, Offset.Y, Offset.Y, 1f, -1, Class1.teller.Heading + 100f, -0.1f);
								}
								else if (tellerwalkStage == 2)
								{
									tellerwalkStage = 3;
									Class1.teller.Task.PlayAnimation("busted", "idle_2_hands_up_2h", -1f, -1f, -1, AnimationFlags.StayInEndFrame | AnimationFlags.Secondary, -1f);
									Class1.teller.Weapons.Drop();
								}
							}
							if (OpenedDoor)
							{
								Prop[] nearbyProps69 = World.GetNearbyProps(Class1.teller.Position, 10f);
								Prop[] array77 = nearbyProps69;
								foreach (Prop prop84 in array77)
								{
									if (prop84.Model == RequestModel(-1184592117) && DoorOpenAmt < 90f)
									{
										DoorOpenAmt += 0.5f;
										prop84.Heading += 0.5f;
									}
								}
							}
							if (Game.Player.Character.IsShooting)
							{
								Class1.teller.Task.Cower(99999);
							}
							if (Game.Player.IsTargeting(Class1.teller) && !OpenedDoor)
							{
								OpenedDoor = true;
								Vector3 position20 = Class1.teller.Position;
								CopTime += 35;
								Function.Call(Hash.TASK_PLAY_ANIM, Class1.teller, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Class1.teller, Offset.X, Offset.Y, Offset.Y, 1f, -1, Class1.teller.Heading + 100f, -0.1f);
							}
						}
					}
					if (DefendingCops && !VaultOpen)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, Paleto, "<", 100f) && CopTime <= 0 && ManageRobberyAnims.CurrentPay <= 0)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
							Game.Player.WantedLevel = 0;
						}
						Prop[] nearbyProps70 = World.GetNearbyProps(Game.Player.Character.Position, 1f);
						Prop[] nearbyProps71 = World.GetNearbyProps(new Vector3(-104.6049f, 6473.44434f, 31.73f), 1f);
						if (nearbyProps71.Length != 0)
						{
							Prop[] array78 = nearbyProps71;
							foreach (Prop prop85 in array78)
							{
								if (prop85 != null)
								{
									prop85.Rotation = new Vector3(0f, 0f, 45f);
								}
							}
						}
						UI.DrawTime("VAULT", TimeTillPaletoVaultOpens - Game.GameTime);
						UI.DrawPayment("TAKE", LocalTake);
						if (Game.GameTime >= TimeTillPaletoVaultOpens && DefendingCops && !PaletoCopsIncoming)
						{
							PaletoCopsIncoming = true;
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							if (_thermiteProp != null)
							{
								_thermiteProp.Delete();
							}
							Game.MaxWantedLevel = 5;
							CopTime = -1;
							AddCratesPaleto();
							DefendingCops = false;
							VaultOpen = true;
							DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok head to the vault to grab the money!");
							if (ManageRobberyAnims.CurrentPay <= 0)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
								Game.Player.WantedLevel = 0;
							}
						}
					}
					if (VaultOpen)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, Paleto, "<", 100f) && CopTime <= 0 && ManageRobberyAnims.CurrentPay <= 0)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
							Game.Player.WantedLevel = 0;
						}
						if (!PaletoCopsIncoming)
						{
							PaletoCopsIncoming = true;
							if (CopTime <= 0)
							{
								PaletoCopsIncoming = true;
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								Game.MaxWantedLevel = 5;
								CopTime = -1;
								Notification.Show("~y~ Lester ~w~ : Alarm has been triggered!, cops are on their way!");
								for (int num134 = 0; num134 < 22; num134++)
								{
									int difficulty = Difficulty;
									if (Difficulty == 1 && num134 < 4)
									{
										Ped ped17 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(-169.803f, 6427.34f, 31f).Around(10f), 24f);
										ped17.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
										ped17.Task.RunTo(Bank.Position);
										ped17.Position = World.GetSafeCoordForPed(new Vector3(-169.803f, 6427.34f, 31f));
										Peds.Add(ped17);
									}
									if (Difficulty == 2 && num134 < 9)
									{
										Ped ped18 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(-169.803f, 6427.34f, 31f).Around(10f), 24f);
										ped18.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
										ped18.Task.RunTo(Bank.Position);
										ped18.Position = World.GetSafeCoordForPed(new Vector3(-169.803f, 6427.34f, 31f));
										Peds.Add(ped18);
									}
									if (Difficulty == 3 && num134 < 12)
									{
										Ped ped19 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(-169.803f, 6427.34f, 31f).Around(10f), 24f);
										ped19.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
										ped19.Task.RunTo(Bank.Position);
										ped19.Position = World.GetSafeCoordForPed(new Vector3(-169.803f, 6427.34f, 31f));
										Peds.Add(ped19);
									}
									if (Difficulty == 4 && num134 < 16)
									{
										Ped ped20 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(-169.803f, 6427.34f, 31f).Around(10f), 24f);
										ped20.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
										ped20.Task.RunTo(Bank.Position);
										ped20.Position = World.GetSafeCoordForPed(new Vector3(-169.803f, 6427.34f, 31f));
										Peds.Add(ped20);
									}
									if (Difficulty == 5 && num134 < 22)
									{
										Ped ped21 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(-169.803f, 6427.34f, 31f).Around(10f), 24f);
										ped21.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
										ped21.Task.RunTo(Bank.Position);
										ped21.Position = World.GetSafeCoordForPed(new Vector3(-169.803f, 6427.34f, 31f));
										Peds.Add(ped21);
									}
								}
							}
						}
						if (ManageRobberyAnims.CurrentPay <= 0)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
							Game.Player.WantedLevel = 0;
						}
						GTA.UI.Screen.ShowSubtitle("Grab the ~g~Loot~w~ and escape");
						Prop[] nearbyProps72 = World.GetNearbyProps(Game.Player.Character.Position, 1f);
						Prop prop86 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -104.6049f, 6473.44434f, 31.73f, 15f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_cbankvaulgate01"), 0, 0, 0);
						if (prop86 != null && prop86.Rotation.Z <= 160f)
						{
							float z6 = prop86.Rotation.Z;
							prop86.Rotation = new Vector3(0f, 0f, z6 += 0.25f);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-104.6049f, 6473.44434f, 31.73f), "<", 5f))
						{
							Prop[] nearbyProps73 = World.GetNearbyProps(new Vector3(-104.6049f, 6473.44434f, 31.73f), 1f);
							if (nearbyProps73.Length != 0)
							{
								Prop[] array79 = nearbyProps73;
								foreach (Prop prop87 in array79)
								{
									if (prop87 != null && prop87.Rotation.Z <= 160f)
									{
										if (prop87.Model == RequestModel("v_ilev_cbankvaulgate01"))
										{
											prop87.IsVisible = false;
										}
										float z7 = prop87.Rotation.Z;
										prop87.Rotation = new Vector3(0f, 0f, z7 += 0.25f);
									}
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-104.6049f, 6473.44434f, 31.73f), ">", 5f))
						{
							Prop[] nearbyProps74 = World.GetNearbyProps(new Vector3(-104.6049f, 6473.44434f, 31.73f), 1f);
							if (nearbyProps74.Length != 0)
							{
								Prop[] array80 = nearbyProps74;
								foreach (Prop prop88 in array80)
								{
									if (prop88 != null && prop88.Rotation.Z >= 45f)
									{
										float z8 = prop88.Rotation.Z;
										prop88.Rotation = new Vector3(0f, 0f, z8 -= 0.25f);
									}
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-103.666f, 6477.78f, 30.5f), "<", 30f))
						{
							if (ManageRobberyAnims.CurrentPay <= 0)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
								Game.Player.WantedLevel = 0;
							}
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
							UI.DrawPayment("TAKE", LocalTake);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-103.666f, 6477.78f, 30.5f), "<", 5f))
						{
							GTA.UI.Screen.ShowSubtitle("Grab the ~g~ Cash~w~ and escape the ~r~ Cops");
							if (ManageRobberyAnims.CurrentPay <= 0)
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
								Game.Player.WantedLevel = 0;
							}
							escapingcops = true;
							Bank.ShowRoute = false;
						}
					}
					if (escapingcops)
					{
						if ((float)ManageRobberyAnims.CurrentPay >= (float)Heist_Average_Pay * 0.5f && !ManageRobberyAnims.PlayingAnim)
						{
							Game.MaxWantedLevel = 5;
						}
						foreach (ManageRobberyAnims.Lootable crate_Valuable4 in ManageRobberyAnims.Crate_Valuables)
						{
							if (crate_Valuable4 != null && crate_Valuable4.LootBlip != null)
							{
								crate_Valuable4.LootBlip.Alpha = 0;
							}
						}
						GetDistance_VDIST2(Game.Player.Character.Position, Paleto, "<", 100f);
						if (!PaletoCopsIncoming)
						{
							PaletoCopsIncoming = true;
							if (CopTime <= 0)
							{
								PaletoCopsIncoming = true;
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								Game.MaxWantedLevel = 5;
								CopTime = -1;
								Notification.Show("~y~ Lester ~w~ : Alarm has been triggered!, cops are on their way!");
							}
						}
						if (LocalTake > 0)
						{
							if (!ManageRobberyAnims.PlayingAnim && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 200f))
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								Game.Player.WantedLevel = 5;
								Game.MaxWantedLevel = 5;
							}
							GTA.UI.Screen.ShowSubtitle("Escape the ~r~Cops");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Grab the ~g~ Cash~w~ and escape the ~r~ Cops");
						}
						if (LocalTake > 0 && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 200f) && !ManageRobberyAnims.PlayingAnim)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Game.Player.WantedLevel = 5;
							Game.MaxWantedLevel = 5;
						}
						if (Game.Player.WantedLevel == 0)
						{
							escapingcops = false;
							HeadingtoArcade = true;
						}
					}
					if (HeadingtoArcade)
					{
						if (timer != 30)
						{
							timer++;
						}
						if (timer == 30)
						{
							timer = 0;
							if (CopTime >= 0)
							{
								CopTime--;
							}
						}
						Bank.ShowRoute = false;
						if (Game.Player.WantedLevel == 0 && GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, ">", 200f))
						{
							if (Bank != null)
							{
								Bank.Delete();
							}
							PlayingCutscene = false;
							Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
							int crewAlive3 = 1;
							if (RobberA != null && RobberA.IsAlive)
							{
								crewAlive3++;
							}
							float num137 = ManageRobberyAnims.CurrentPay;
							float num138 = ManageRobberyAnims.CurrentPay;
							if (Bank != null)
							{
								Bank.Delete();
							}
							Wait(10);
							Main.TotalCrewEarned += 0f;
							Main.TotalPlayerEarned += num138;
							XP.CompleteHeist_PaletoBank++;
							Main.SaveCuts();
							SetNoLongerInHeist();
							MissionScreen.SetPassNoAi("The Paleto Bank Job", (int)num138, 0, "The Player has died", crewAlive3, HTime, 2);
							int num139 = Game.GameTime + 30000;
							while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num139)
							{
								Script.Wait(0);
							}
							Script.Wait(2000);
							XP.AddXP_Percent(XP.CompleteHeist_XPPercent_PaletoBank);
							HeistTime = 0f;
							ManageRobberyAnims.CurrentPay = 0;
							GTA.UI.Screen.FadeOut(300);
							Wait(300);
							foreach (Ped guard13 in Guards)
							{
								if (guard13 != null)
								{
									guard13.Delete();
								}
							}
							foreach (Ped teller13 in Tellers)
							{
								if (teller13 != null)
								{
									teller13.Delete();
								}
							}
							foreach (Ped civ12 in Civs)
							{
								if (civ12 != null)
								{
									civ12.Delete();
								}
							}
							if (Insurgent != null)
							{
								Insurgent.Delete();
							}
							if (Kuruma != null)
							{
								Kuruma.Delete();
							}
							if (InsurgentBLip != null)
							{
								InsurgentBLip.Delete();
							}
							if (KurumaBlip != null)
							{
								KurumaBlip.Delete();
							}
							if (GetawayCar != null)
							{
								GetawayCar.Delete();
							}
							Wait(300);
							GTA.UI.Screen.FadeIn(300);
							if (Bank != null)
							{
								Bank.Delete();
							}
							DisplayHelpTextThisFrame("~y~ Lester ~w~ : Paleto Heist Complete");
							Random random20 = new Random();
							InHeist = false;
							ManagePlayerFunds.AddFunds((int)num138);
							HeadingtoArcade = false;
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							Notification.Show("~b~ Heist Complete ~w~");
							PlayingPaleto = false;
							MissionOn = false;
							CompletedPaleto = 1;
						}
					}
				}
				if (Game.Player.IsAlive || !InHeist || !InHeist)
				{
					break;
				}
				PlayingPaleto = false;
				if (Bank != null)
				{
					Bank.Delete();
				}
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
				InHeist = false;
				GTA.UI.Screen.FadeOut(300);
				Wait(300);
				foreach (Ped guard14 in Guards)
				{
					if (guard14 != null)
					{
						guard14.Delete();
					}
				}
				foreach (Ped teller14 in Tellers)
				{
					if (teller14 != null)
					{
						teller14.Delete();
					}
				}
				foreach (Ped civ13 in Civs)
				{
					if (civ13 != null)
					{
						civ13.Delete();
					}
				}
				if (GetawayCar != null)
				{
					GetawayCar.Delete();
				}
				if (Insurgent != null)
				{
					Insurgent.Delete();
				}
				if (Kuruma != null)
				{
					Kuruma.Delete();
				}
				if (InsurgentBLip != null)
				{
					InsurgentBLip.Delete();
				}
				if (KurumaBlip != null)
				{
					KurumaBlip.Delete();
				}
				Wait(300);
				GTA.UI.Screen.FadeIn(300);
				placedJammer = false;
				DefendingCops = false;
				HeadingtoArcade = false;
				escapingcops = false;
				VaultOpen = false;
				HeistTime = 0f;
				DisplayHelpTextThisFrame("~y~ Lester ~w~ : Paleto Heist Failed");
				Wait(20000);
				Notification.Show("~b~ The Paleto Bank Heist was failed!~w~");
				MissionScreen.SetFail("The Paleto Bank Heist", 0, 0, "you Died!", 0, HeistTime, 2);
				SetNoLongerInHeist();
				MissionOn = false;
				break;
			}
			case HeistIn.Jewel:
				UI.RenderQue = 0f;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
				try
				{
					if (!JewelStoreRobberyOn)
					{
						break;
					}
					if (Function.Call<bool>(Hash.IS_IPL_ACTIVE, "bh1_16_refurb"))
					{
						Function.Call(Hash.REMOVE_IPL, "bh1_16_refurb");
					}
					Prop prop89 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -630.9785f, -239.3216f, 37.08013f, 3f, Function.Call<int>(Hash.GET_HASH_KEY, "p_jewel_door_l"), 0, 0, 0);
					if (prop89 != null)
					{
						prop89.IsPositionFrozen = false;
					}
					Prop prop90 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, -630.4265f, -238.4376f, 38.20653f, 3f, Function.Call<int>(Hash.GET_HASH_KEY, "p_jewel_door_r1"), 0, 0, 0);
					if (prop90 != null)
					{
						prop90.IsPositionFrozen = false;
					}
					if (Game.Player.IsAlive)
					{
						if (JewelStoreStage != HeistStage.Stage2 && !TriggeredJewelStoreAlarm && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-631.14f, -237.4f, 38f), "<", 10f))
						{
							TriggeredJewelStoreAlarm = true;
							JewelStoreJobCopsArriveIn = Game.GameTime + Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 85000, 100000);
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
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-631.14f, -237.4f, 38f), "<", 40f) && TriggeredJewelStoreAlarm)
						{
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
							if (Game.GameTime < JewelStoreJobCopsArriveIn)
							{
								UI.DrawTime("COPS ARRIVE", JewelStoreJobCopsArriveIn - Game.GameTime);
								int num140 = JewelStoreJobCopsArriveIn - Game.GameTime;
								if (num140 >= 79900 && num140 <= 80100 && !ConvStarted)
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
								if (num140 >= 69900 && num140 <= 70100 && !ConvStarted)
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
								if (num140 >= 59900 && num140 <= 60100 && !ConvStarted)
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
								if (num140 >= 49900 && num140 <= 50100 && !ConvStarted)
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
								if (num140 >= 39900 && num140 <= 40100 && !ConvStarted)
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
								if (num140 >= 29900 && num140 <= 30100 && !ConvStarted)
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
								if (num140 >= 19900 && num140 <= 20100 && !ConvStarted)
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
								if (num140 >= 9975 && num140 <= 10025)
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
						}
						else
						{
							JewelStoreJobCopsArriveIn = 0;
						}
						CheckJewels();
						if (MethodA_OR_MethodB.Equals("A"))
						{
							if (Game.Player.Character.IsShooting)
							{
								if (JewelHost != null)
								{
									JewelHost.Task.Cower(99999);
								}
								foreach (Ped item41 in Civ)
								{
									if (item41 != null)
									{
										item41.Task.Cower(99999);
									}
								}
							}
							foreach (Ped item42 in Civ)
							{
								bool flag22 = item42 != null;
							}
						}
						if (InHeist)
						{
							HeistTime += 0.35f;
						}
						if (JewelStoreJobCopsArriveIn < Game.GameTime && Difficulty < 3)
						{
							Game.Player.WantedLevel = 0;
						}
						if (InHeist)
						{
							foreach (Ped civ14 in Civs)
							{
								if (!(civ14 != null))
								{
									continue;
								}
								if (Game.Player.IsTargeting(civ14))
								{
									foreach (Ped civ15 in Civs)
									{
										if (civ15 != null)
										{
											Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, civ15, 1);
											civ15.Task.Cower(9999);
										}
									}
								}
								if (!Game.Player.Character.IsShooting)
								{
									continue;
								}
								foreach (Ped civ16 in Civs)
								{
									if (civ16 != null)
									{
										Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, civ16, 1);
										civ16.Task.Cower(9999);
									}
								}
							}
						}
						if (JewelStoreStage == HeistStage.Stage1)
						{
							Game.Player.WantedLevel = 0;
							InHeist = true;
							if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaBankMarker, "<", 3f))
							{
								Game.Player.WantedLevel = 0;
								Random random21 = new Random();
								Notification.Show("Load up as much as you can and then escape the cops");
								foreach (Blip cameraBlip in CameraBlips)
								{
									if (cameraBlip != null)
									{
										cameraBlip.Delete();
									}
								}
								if (CameraBlips.Count > 0)
								{
									CameraBlips.Clear();
								}
								if (Cameras.Count > 0)
								{
									Cameras.Clear();
								}
								Prop[] nearbyProps75 = World.GetNearbyProps(new Vector3(-624f, -232f, 38f), 60f, RequestModel(168901740));
								if (nearbyProps75.Length != 0)
								{
									Prop[] array81 = nearbyProps75;
									foreach (Prop prop91 in array81)
									{
										if (prop91 != null)
										{
											Cameras.Add(prop91);
											Blip blip2 = GlobalHelper.CreateBlip(prop91.Position);
											blip2.Sprite = BlipSprite.Standard;
											blip2.Color = BlipColor.Red2;
											blip2.Name = "Camera";
											blip2.Scale = 0.5f;
											CameraBlips.Add(blip2);
										}
									}
								}
								TellPlayerMax = false;
								if (RobbedJewelsCasesBlips.Count > 0)
								{
									foreach (Blip robbedJewelsCasesBlip in RobbedJewelsCasesBlips)
									{
										if (robbedJewelsCasesBlip != null)
										{
											robbedJewelsCasesBlip.Delete();
										}
									}
								}
								if (CabinetType.Count > 0)
								{
									CabinetType.Clear();
								}
								WAIT(2000);
								JewelsCases.Clear();
								RobbedJewelsCases.Clear();
								Prop[] nearbyProps76 = World.GetNearbyProps(new Vector3(-624f, -232f, 38f), 100f);
								Prop[] array82 = nearbyProps76;
								foreach (Prop prop92 in array82)
								{
									if (prop92.Model == RequestModel("des_jewel_cab_start"))
									{
										CabinetType.Add(1);
										JewelsCases.Add(prop92);
										RobbedJewelsCases.Add(false);
										Blip blip3 = GlobalHelper.CreateBlip(prop92.Position);
										blip3.Sprite = BlipSprite.Standard;
										blip3.Color = BlipColor.Green2;
										blip3.Name = "Jewelery";
										blip3.Scale = 0.5f;
										RobbedJewelsCasesBlips.Add(blip3);
										RobbedJewelsLeft++;
									}
									if (prop92.Model == RequestModel("des_jewel_cab2_start"))
									{
										CabinetType.Add(2);
										JewelsCases.Add(prop92);
										RobbedJewelsCases.Add(false);
										Blip blip4 = GlobalHelper.CreateBlip(prop92.Position);
										blip4.Sprite = BlipSprite.Standard;
										blip4.Color = BlipColor.Green2;
										blip4.Name = "Jewelery";
										blip4.Scale = 0.5f;
										RobbedJewelsCasesBlips.Add(blip4);
										RobbedJewelsLeft++;
									}
									if (prop92.Model == RequestModel("des_jewel_cab3_start"))
									{
										CabinetType.Add(3);
										JewelsCases.Add(prop92);
										RobbedJewelsCases.Add(false);
										Blip blip5 = GlobalHelper.CreateBlip(prop92.Position);
										blip5.Sprite = BlipSprite.Standard;
										blip5.Color = BlipColor.Green2;
										blip5.Name = "Jewelery";
										blip5.Scale = 0.5f;
										RobbedJewelsCasesBlips.Add(blip5);
										RobbedJewelsLeft++;
									}
									if (prop92.Model == RequestModel("des_jewel_cab4_start"))
									{
										CabinetType.Add(4);
										JewelsCases.Add(prop92);
										RobbedJewelsCases.Add(false);
										Blip blip6 = GlobalHelper.CreateBlip(prop92.Position);
										blip6.Sprite = BlipSprite.Standard;
										blip6.Color = BlipColor.Green2;
										blip6.Name = "Jewelery";
										blip6.Scale = 0.5f;
										RobbedJewelsCasesBlips.Add(blip6);
										RobbedJewelsLeft++;
									}
								}
								if (RobbedJewelsCases.Count > 0)
								{
									GotJewels = true;
								}
								if (RobbedJewelsCases.Count == 20)
								{
									VaultOpen = true;
								}
								JewelStoreStage = HeistStage.Stage2;
								Random random22 = new Random();
							}
						}
						if (JewelStoreStage == HeistStage.Stage2)
						{
							Game.Player.WantedLevel = 0;
							if (Game.GameTime >= JewelStoreJobCopsArriveIn && JewelStoreJobCopsArriveIn != 0)
							{
								bool flag23 = Bank != null;
								JewelStoreStage = HeistStage.Stage3;
								Notification.Show("The Alarm has been Tripped!, the cops are on their way!");
								if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaBankMarker, "<", 200f))
								{
									if (Difficulty == 0)
									{
										Game.Player.WantedLevel = 2;
									}
									if (Difficulty == 1)
									{
										Game.Player.WantedLevel = 3;
									}
									if (Difficulty == 2)
									{
										Game.Player.WantedLevel = 3;
									}
									if (Difficulty == 3)
									{
										Game.Player.WantedLevel = 4;
									}
									if (Difficulty == 4)
									{
										Game.Player.WantedLevel = 4;
									}
									if (Difficulty == 5)
									{
										Game.Player.WantedLevel = 5;
									}
								}
								DefendingCops = false;
								foreach (Ped ped36 in Peds)
								{
									if (ped36 != null)
									{
										ped36.Delete();
									}
								}
								foreach (Vehicle vehicle4 in Vehicles)
								{
									if (vehicle4 != null)
									{
										vehicle4.Delete();
									}
								}
								for (int num143 = 0; num143 < 5; num143++)
								{
									Ped ped22 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(-584f, -326f, 34f).Around(5f), 24f);
									ped22.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
									ped22.Task.RunTo(Bank.Position);
									Peds.Add(ped22);
								}
								for (int num144 = 0; num144 < 5; num144++)
								{
									Ped ped23 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(-719.826f, -224.831f, 37.1958f).Around(5f), 24f);
									ped23.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
									ped23.Task.RunTo(Bank.Position);
									Peds.Add(ped23);
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaBankMarker, ">", 200f))
							{
								JewelStoreStage = HeistStage.Stage4;
								Bank.ShowRoute = false;
							}
						}
						if (JewelStoreStage == HeistStage.Stage2 || JewelStoreStage == HeistStage.Stage3)
						{
							if (Game.GameTime >= JewelStoreJobCopsArriveIn)
							{
								UI.DrawPayment("TAKE", LocalTake);
							}
							GTA.UI.Screen.ShowSubtitle("Loot the ~g~Store~w~ or escape the ~r~Cops");
							bool flag8 = true;
							if (RobbedJewelsCases.Count > 0)
							{
								RobbedJewelsLeft = 20;
								for (int num145 = 0; num145 < JewelsCases.Count; num145++)
								{
									if (RobbedJewelsCases[num145])
									{
										RobbedJewelsLeft--;
									}
								}
								for (int num146 = 0; num146 < JewelsCases.Count; num146++)
								{
									if (!RobbedJewelsCases[num146])
									{
										flag8 = false;
										break;
									}
								}
								if (flag8)
								{
									if (!TellPlayerMax)
									{
										foreach (Blip robbedJewelsCasesBlip2 in RobbedJewelsCasesBlips)
										{
											if (robbedJewelsCasesBlip2 != null)
											{
												robbedJewelsCasesBlip2.Alpha = 0;
											}
										}
										Notification.Show("you have got all you can carry!, leave the store Escape");
										TellPlayerMax = true;
									}
									int wantedLevel = Game.Player.WantedLevel;
									if (Game.Player.WantedLevel >= 1)
									{
										foreach (Blip robbedJewelsCasesBlip3 in RobbedJewelsCasesBlips)
										{
											if (robbedJewelsCasesBlip3 != null)
											{
												robbedJewelsCasesBlip3.Alpha = 0;
											}
										}
										GTA.UI.Screen.ShowSubtitle("Escape  the ~r~ Cops~w~");
									}
								}
								if (!OverideTasks)
								{
									if (RobberA != null && RobberA != null && RobberAMovingToLoot && RobberARobbing)
									{
										RobberA.BlockPermanentEvents = true;
										RobberA_BlockPermanentEvents = true;
										RobberA.AlwaysKeepTask = true;
										RobberA_AlwaysKeepTask = true;
									}
									if (RobberB != null && RobberB != null && RobberBMovingToLoot && RobberBRobbing)
									{
										RobberB.BlockPermanentEvents = true;
										RobberB_BlockPermanentEvents = true;
										RobberB.AlwaysKeepTask = true;
										RobberB_AlwaysKeepTask = true;
									}
									if (RobberC != null && RobberC != null && RobberCMovingToLoot && RobberCRobbing)
									{
										RobberC.BlockPermanentEvents = true;
										RobberC_BlockPermanentEvents = true;
										RobberC.AlwaysKeepTask = true;
										RobberC_AlwaysKeepTask = true;
									}
								}
								if (!OverideTasks)
								{
									bool flag9 = true;
									foreach (Ped guard15 in Guards)
									{
										if (!guard15.IsAlive)
										{
											flag9 = false;
											break;
										}
									}
									if (!flag9)
									{
										if (RobberA != null)
										{
											RobberA.Task.ClearAll();
										}
										if (RobberB != null)
										{
											RobberB.Task.ClearAll();
										}
										if (RobberC != null)
										{
											RobberC.Task.ClearAll();
										}
										RobberAMovingToLoot = false;
										RobberBMovingToLoot = false;
										RobberCMovingToLoot = false;
										OverideTasks = true;
									}
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-624f, -232f, 38f), "<", 40f))
								{
									if (JewelsCases.Count > 0)
									{
										for (int num147 = 0; num147 < JewelsCases.Count; num147++)
										{
											if (!(JewelsCases[num147] != null))
											{
												continue;
											}
											Random random23 = new Random();
											Vector3 offsetPosition18 = JewelsCases[num147].GetOffsetPosition(new Vector3(0f, 1f, -0.3f));
											if (RobberA != null)
											{
												if (RobberARobbing && RobberA.IsStopped && GetDistance_VDIST2(RobberA.Position, RobberA_RobberMarker, ">", 1.35f))
												{
													bool flag10 = false;
													for (int num148 = 0; num148 < JewelsCases.Count; num148++)
													{
														if (!RobbedJewelsCases[num148])
														{
															flag10 = true;
															break;
														}
													}
													if (flag10)
													{
														RobberARobbing = false;
														int num149 = random23.Next(0, JewelsCases.Count);
														if (RobbedJewelsCasesBlips[num149].Alpha == 255 && !RobbedJewelsCases[num149])
														{
															RobberAMovingToLoot = false;
															if (CabinetType[num149] == 1)
															{
																RobberACurrentCabinet = JewelsCases[num149];
																RobberA_RobberMarker = JewelsCases[num149].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
															}
															if (CabinetType[num149] == 2)
															{
																RobberACurrentCabinet = JewelsCases[num149];
																RobberA_RobberMarker = JewelsCases[num149].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
															}
															if (CabinetType[num149] == 3)
															{
																RobberACurrentCabinet = JewelsCases[num149];
																RobberA_RobberMarker = JewelsCases[num149].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
															}
															if (CabinetType[num149] == 4)
															{
																RobberACurrentCabinet = JewelsCases[num149];
																RobberA_RobberMarker = JewelsCases[num149].GetOffsetPosition(new Vector3(-0.9f, 0f, -0.5f));
															}
															RobbedJewelsCasesBlips[num149].Alpha = 0;
															RobberACurrentValualabe = num149;
															Notification.Show("RobberA Moving To Pos");
															if (HeistPathfindVariation == 0)
															{
																Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberA, RobberA_RobberMarker.X, RobberA_RobberMarker.Y, RobberA_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
															}
															if (HeistPathfindVariation == 1)
															{
																Vector3 robberA_RobberMarker = RobberA_RobberMarker;
																Vector3 robberA_RobberMarker2 = RobberA_RobberMarker;
																Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberA, robberA_RobberMarker.X, robberA_RobberMarker.Y, robberA_RobberMarker.Z, 30f, 20000, 0.1f, 512, 0);
															}
															break;
														}
													}
												}
												if (!RobberARobbing)
												{
													if (RobberAMoveTimer != 550)
													{
														if (RobberA.IsRunning)
														{
															RobberAMovingToLoot = true;
															RobberA.Opacity = 255;
														}
														RobberAMoveTimer++;
													}
													if (RobberAMoveTimer == 550)
													{
														RobberAMoveTimer = 0;
														if (!RobberARobbing && RobberA.IsStopped)
														{
															bool flag11 = false;
															for (int num150 = 0; num150 < JewelsCases.Count; num150++)
															{
																if (!RobbedJewelsCases[num150])
																{
																	flag11 = true;
																	break;
																}
															}
															if (flag11)
															{
																int num151 = random23.Next(0, JewelsCases.Count);
																if (RobbedJewelsCasesBlips[num151].Alpha == 255 && !RobbedJewelsCases[num151])
																{
																	RobberAMovingToLoot = false;
																	if (CabinetType[num151] == 1)
																	{
																		RobberACurrentCabinet = JewelsCases[num151];
																		RobberA_RobberMarker = JewelsCases[num151].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
																	}
																	if (CabinetType[num151] == 2)
																	{
																		RobberACurrentCabinet = JewelsCases[num151];
																		RobberA_RobberMarker = JewelsCases[num151].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
																	}
																	if (CabinetType[num151] == 3)
																	{
																		RobberACurrentCabinet = JewelsCases[num151];
																		RobberA_RobberMarker = JewelsCases[num151].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
																	}
																	if (CabinetType[num151] == 4)
																	{
																		RobberACurrentCabinet = JewelsCases[num151];
																		RobberA_RobberMarker = JewelsCases[num151].GetOffsetPosition(new Vector3(-0.9f, 0f, -0.5f));
																	}
																	RobberACurrentValualabe = num151;
																	Notification.Show("RobberA Moving To Pos");
																	if (HeistPathfindVariation == 0)
																	{
																		Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberA, RobberA_RobberMarker.X, RobberA_RobberMarker.Y, RobberA_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
																	}
																	if (HeistPathfindVariation == 1)
																	{
																		Vector3 robberA_RobberMarker3 = RobberA_RobberMarker;
																		Vector3 robberA_RobberMarker4 = RobberA_RobberMarker;
																		Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberA, robberA_RobberMarker3.X, robberA_RobberMarker3.Y, robberA_RobberMarker3.Z, 30f, 20000, 0.1f, 512, 0);
																	}
																	break;
																}
															}
														}
													}
												}
											}
											if (RobberB != null)
											{
												if (RobberBRobbing && RobberB.IsStopped && GetDistance_VDIST2(RobberB.Position, RobberB_RobberMarker, ">", 1.35f))
												{
													bool flag12 = false;
													for (int num152 = 0; num152 < JewelsCases.Count; num152++)
													{
														if (!RobbedJewelsCases[num152])
														{
															flag12 = true;
															break;
														}
													}
													if (flag12)
													{
														RobberBRobbing = false;
														int num153 = random23.Next(0, JewelsCases.Count);
														if (RobbedJewelsCasesBlips[num153].Alpha == 255 && !RobbedJewelsCases[num153])
														{
															RobberBMovingToLoot = false;
															if (CabinetType[num153] == 1)
															{
																RobberBCurrentCabinet = JewelsCases[num153];
																RobberB_RobberMarker = JewelsCases[num153].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
															}
															if (CabinetType[num153] == 2)
															{
																RobberBCurrentCabinet = JewelsCases[num153];
																RobberB_RobberMarker = JewelsCases[num153].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
															}
															if (CabinetType[num153] == 3)
															{
																RobberBCurrentCabinet = JewelsCases[num153];
																RobberB_RobberMarker = JewelsCases[num153].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
															}
															if (CabinetType[num153] == 4)
															{
																RobberBCurrentCabinet = JewelsCases[num153];
																RobberB_RobberMarker = JewelsCases[num153].GetOffsetPosition(new Vector3(-0.9f, 0f, -0.5f));
															}
															RobbedJewelsCasesBlips[num153].Alpha = 0;
															RobberBCurrentValualabe = num153;
															Notification.Show("RobberB Moving To Pos");
															if (HeistPathfindVariation == 0)
															{
																Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberB, RobberB_RobberMarker.X, RobberB_RobberMarker.Y, RobberB_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
															}
															if (HeistPathfindVariation == 1)
															{
																Vector3 robberB_RobberMarker = RobberB_RobberMarker;
																Vector3 robberB_RobberMarker2 = RobberB_RobberMarker;
																Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberB, robberB_RobberMarker.X, robberB_RobberMarker.Y, robberB_RobberMarker.Z, 30f, 20000, 0.1f, 512, 0);
															}
															break;
														}
													}
												}
												if (!RobberBRobbing)
												{
													if (RobberBMoveTimer != 550)
													{
														if (RobberB.IsRunning)
														{
															RobberBMovingToLoot = true;
															RobberB.Opacity = 255;
														}
														RobberBMoveTimer++;
													}
													if (RobberBMoveTimer == 550)
													{
														RobberBMoveTimer = 0;
														if (!RobberBRobbing && RobberB.IsStopped)
														{
															bool flag13 = false;
															for (int num154 = 0; num154 < JewelsCases.Count; num154++)
															{
																if (!RobbedJewelsCases[num154])
																{
																	flag13 = true;
																	break;
																}
															}
															if (flag13)
															{
																int num155 = random23.Next(0, JewelsCases.Count);
																if (RobbedJewelsCasesBlips[num155].Alpha == 255 && !RobbedJewelsCases[num155])
																{
																	RobberBMovingToLoot = false;
																	if (CabinetType[num155] == 1)
																	{
																		RobberBCurrentCabinet = JewelsCases[num155];
																		RobberB_RobberMarker = JewelsCases[num155].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
																	}
																	if (CabinetType[num155] == 2)
																	{
																		RobberBCurrentCabinet = JewelsCases[num155];
																		RobberB_RobberMarker = JewelsCases[num155].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
																	}
																	if (CabinetType[num155] == 3)
																	{
																		RobberBCurrentCabinet = JewelsCases[num155];
																		RobberB_RobberMarker = JewelsCases[num155].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
																	}
																	if (CabinetType[num155] == 4)
																	{
																		RobberBCurrentCabinet = JewelsCases[num155];
																		RobberB_RobberMarker = JewelsCases[num155].GetOffsetPosition(new Vector3(-0.9f, 0f, -0.5f));
																	}
																	RobberBCurrentValualabe = num155;
																	Notification.Show("RobberB Moving To Pos");
																	if (HeistPathfindVariation == 0)
																	{
																		Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberB, RobberB_RobberMarker.X, RobberB_RobberMarker.Y, RobberB_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
																	}
																	if (HeistPathfindVariation == 1)
																	{
																		Vector3 robberB_RobberMarker3 = RobberB_RobberMarker;
																		Vector3 robberB_RobberMarker4 = RobberB_RobberMarker;
																		Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberB, robberB_RobberMarker3.X, robberB_RobberMarker3.Y, robberB_RobberMarker3.Z, 30f, 20000, 0.1f, 512, 0);
																	}
																	break;
																}
															}
														}
													}
												}
											}
											if (!(RobberC != null))
											{
												continue;
											}
											if (RobberCRobbing && RobberC.IsStopped && GetDistance_VDIST2(RobberC.Position, RobberC_RobberMarker, ">", 1.35f))
											{
												bool flag14 = false;
												for (int num156 = 0; num156 < JewelsCases.Count; num156++)
												{
													if (!RobbedJewelsCases[num156])
													{
														flag14 = true;
														break;
													}
												}
												if (flag14)
												{
													RobberCRobbing = false;
													int num157 = random23.Next(0, JewelsCases.Count);
													if (RobbedJewelsCasesBlips[num157].Alpha == 255 && !RobbedJewelsCases[num157])
													{
														RobberCMovingToLoot = false;
														if (CabinetType[num157] == 1)
														{
															RobberCCurrentCabinet = JewelsCases[num157];
															RobberC_RobberMarker = JewelsCases[num157].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
														}
														if (CabinetType[num157] == 2)
														{
															RobberCCurrentCabinet = JewelsCases[num157];
															RobberC_RobberMarker = JewelsCases[num157].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
														}
														if (CabinetType[num157] == 3)
														{
															RobberCCurrentCabinet = JewelsCases[num157];
															RobberC_RobberMarker = JewelsCases[num157].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
														}
														if (CabinetType[num157] == 4)
														{
															RobberCCurrentCabinet = JewelsCases[num157];
															RobberC_RobberMarker = JewelsCases[num157].GetOffsetPosition(new Vector3(-0.9f, 0f, -0.5f));
														}
														RobbedJewelsCasesBlips[num157].Alpha = 0;
														RobberCCurrentValualabe = num157;
														Notification.Show("RobberC Moving To Pos");
														if (HeistPathfindVariation == 0)
														{
															Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberC, RobberC_RobberMarker.X, RobberC_RobberMarker.Y, RobberC_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
														}
														if (HeistPathfindVariation == 1)
														{
															Vector3 robberC_RobberMarker = RobberC_RobberMarker;
															Vector3 robberC_RobberMarker2 = RobberC_RobberMarker;
															Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberC, robberC_RobberMarker.X, robberC_RobberMarker.Y, robberC_RobberMarker.Z, 30f, 20000, 0.1f, 512, 0);
														}
														break;
													}
												}
											}
											if (RobberCRobbing)
											{
												continue;
											}
											if (RobberCMoveTimer != 550)
											{
												if (RobberC.IsRunning)
												{
													RobberCMovingToLoot = true;
													RobberC.Opacity = 255;
												}
												RobberCMoveTimer++;
											}
											if (RobberCMoveTimer != 550)
											{
												continue;
											}
											RobberCMoveTimer = 0;
											if (RobberCRobbing || !RobberC.IsStopped)
											{
												continue;
											}
											bool flag15 = false;
											for (int num158 = 0; num158 < JewelsCases.Count; num158++)
											{
												if (!RobbedJewelsCases[num158])
												{
													flag15 = true;
													break;
												}
											}
											if (!flag15)
											{
												continue;
											}
											int num159 = random23.Next(0, JewelsCases.Count);
											if (RobbedJewelsCasesBlips[num159].Alpha == 255 && !RobbedJewelsCases[num159])
											{
												RobberCMovingToLoot = false;
												if (CabinetType[num159] == 1)
												{
													RobberCCurrentCabinet = JewelsCases[num159];
													RobberC_RobberMarker = JewelsCases[num159].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
												}
												if (CabinetType[num159] == 2)
												{
													RobberCCurrentCabinet = JewelsCases[num159];
													RobberC_RobberMarker = JewelsCases[num159].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
												}
												if (CabinetType[num159] == 3)
												{
													RobberCCurrentCabinet = JewelsCases[num159];
													RobberC_RobberMarker = JewelsCases[num159].GetOffsetPosition(new Vector3(0f, -0.9f, -0.5f));
												}
												if (CabinetType[num159] == 4)
												{
													RobberCCurrentCabinet = JewelsCases[num159];
													RobberC_RobberMarker = JewelsCases[num159].GetOffsetPosition(new Vector3(-0.9f, 0f, -0.5f));
												}
												RobberCCurrentValualabe = num159;
												Notification.Show("RobberC Moving To Pos");
												if (HeistPathfindVariation == 0)
												{
													Function.Call(Hash.TASK_GO_TO_COORD_ANY_MEANS, RobberC, RobberC_RobberMarker.X, RobberC_RobberMarker.Y, RobberC_RobberMarker.Z, 10f, 0, false, 786603, 3212836864u);
												}
												if (HeistPathfindVariation == 1)
												{
													Vector3 robberC_RobberMarker3 = RobberC_RobberMarker;
													Vector3 robberC_RobberMarker4 = RobberC_RobberMarker;
													Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberC, robberC_RobberMarker3.X, robberC_RobberMarker3.Y, robberC_RobberMarker3.Z, 30f, 20000, 0.1f, 512, 0);
												}
												break;
											}
										}
									}
									StealJewels();
								}
								if (!flag8)
								{
									if (RobberA != null && !RobberARobbing && RobberACuttingStage == 0)
									{
										RobberACuttingStage = 7;
										RobberA.Task.RunTo(new Vector3(-634f, -240.6f, 38f), false, 9999);
									}
									if (RobberB != null && !RobberBRobbing && RobberBCuttingStage == 0)
									{
										RobberBCuttingStage = 7;
										RobberB.Task.RunTo(new Vector3(-634f, -240.6f, 38f), false, 9999);
									}
									if (RobberC != null && !RobberCRobbing && RobberCCuttingStage == 0)
									{
										RobberCCuttingStage = 7;
										RobberC.Task.RunTo(new Vector3(-634f, -240.6f, 38f), false, 9999);
									}
									if (!RobberARobbing && RobberACuttingStage == 0 && !RobberBRobbing && RobberBCuttingStage == 0 && !RobberCRobbing && RobberCCuttingStage == 0)
									{
										OverideTasks = false;
									}
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaBankMarker, ">", 40f))
							{
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								JewelStoreStage = HeistStage.Stage4;
								Bank.ShowRoute = false;
							}
							else if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaBankMarker, "<", 40f) && Game.GameTime >= JewelStoreJobCopsArriveIn && JewelStoreJobCopsArriveIn != 0 && (DefendingCops || escapingcops))
							{
								Game.Player.WantedLevel = 4;
							}
						}
						if (EnableErrorDebugging)
						{
							Notification.Show("~r~ Error Caught~w~ On Line 61713, contact ~g~ HKH191~");
						}
						bool flag16 = true;
						if (Cameras.Count == 4)
						{
							for (int num160 = 0; num160 < Cameras.Count; num160++)
							{
								Prop prop93 = Cameras[num160];
								if (prop93 != null)
								{
									if (prop93.Health < 999)
									{
										CameraBlips[num160].Alpha = 0;
									}
									if (prop93.Health > 999)
									{
										flag16 = false;
										break;
									}
									int health = prop93.Health;
									int num178 = 995;
								}
							}
							if (flag16)
							{
								HeadingtoArcade = true;
							}
							if (!flag16)
							{
								HeadingtoArcade = false;
							}
						}
						if (JewelStoreStage == HeistStage.Stage4 && Game.Player.WantedLevel == 0)
						{
							foreach (Blip robbedJewelsCasesBlip4 in RobbedJewelsCasesBlips)
							{
								if (robbedJewelsCasesBlip4 != null)
								{
									robbedJewelsCasesBlip4.Alpha = 0;
								}
							}
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							JewelStoreStage = HeistStage.Stage5;
						}
						if (EnableErrorDebugging)
						{
							Notification.Show("~r~ Error Caught~w~ On Line 61773, contact ~g~ HKH191~");
						}
						if (JewelStoreStage == HeistStage.Stage5 && InHeist)
						{
							UI.DrawPayment("TAKE", LocalTake);
							GTA.UI.Screen.ShowSubtitle("Leave The Area");
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-632.3f, -238.2f, 37f), ">", 200f))
							{
								GTA.UI.Screen.ShowSubtitle("Leave The Area");
								if (Bank != null)
								{
									Bank.ShowRoute = false;
								}
								if (Game.Player.WantedLevel == 0)
								{
									Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
									JewelStoreStage = HeistStage.None;
									Random random24 = new Random();
									int num161 = random24.Next(0, 3);
									string text2 = "mph_fin_cel_bar";
									float heistTime = HeistTime;
									JewelStoreRobberyOn = false;
									if (SmokeOrigin != null)
									{
										SmokeOrigin.Delete();
									}
									foreach (Ped item43 in Civ)
									{
										if (item43 != null)
										{
											item43.Delete();
										}
									}
									Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
									int crewAlive4 = 1;
									if (RobberA != null && RobberA.IsAlive)
									{
										crewAlive4++;
									}
									float num162 = ManageRobberyAnims.CurrentPay;
									float num163 = (num162 -= (float)ManageRobberyAnims.CurrentPay * 0.25f);
									float num164 = 0f;
									Main.TotalCrewEarned += num164;
									Main.TotalPlayerEarned += num163;
									XP.CompleteHeist_JewelStore++;
									Main.SaveCuts();
									SetNoLongerInHeist();
									MissionScreen.SetPassNoAi("The Jewel Store Job", (int)num163, (int)num164, "The Player has died", crewAlive4, heistTime, 4);
									int num165 = Game.GameTime + 30000;
									while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num165)
									{
										Script.Wait(0);
									}
									Script.Wait(2000);
									ManagePlayerFunds.AddFunds((int)num163);
									ManageRobberyAnims.CurrentPay = 0;
									XP.AddXP_Percent(XP.CompleteHeist_XPPercent_JewelStore);
									HeistTime = 0f;
									foreach (Blip cameraBlip2 in CameraBlips)
									{
										if (cameraBlip2 != null)
										{
											cameraBlip2.Delete();
										}
									}
									foreach (Blip robbedJewelsCasesBlip5 in RobbedJewelsCasesBlips)
									{
										if (robbedJewelsCasesBlip5 != null)
										{
											robbedJewelsCasesBlip5.Delete();
										}
									}
									foreach (Ped guard16 in Guards)
									{
										if (guard16 != null)
										{
											guard16.Delete();
										}
									}
									foreach (Ped civ17 in Civs)
									{
										if (civ17 != null)
										{
											civ17.Delete();
										}
									}
									foreach (Ped ped37 in Peds)
									{
										if (ped37 != null)
										{
											ped37.Delete();
										}
									}
									Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
									Random random25 = new Random();
									Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
									InHeist = false;
								}
								CompletedJewel = 1;
								MissionOn = false;
							}
						}
						if ((JewelStoreStage == HeistStage.Stage3 || JewelStoreStage == HeistStage.Stage4) && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-624f, -232f, 38f), ">", 40f) && Game.Player.WantedLevel == 0)
						{
							foreach (Blip robbedJewelsCasesBlip6 in RobbedJewelsCasesBlips)
							{
								if (robbedJewelsCasesBlip6 != null)
								{
									robbedJewelsCasesBlip6.Alpha = 0;
								}
							}
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
							JewelStoreStage = HeistStage.Stage5;
						}
					}
					if (Game.Player.IsAlive || !InHeist || !InHeist)
					{
						break;
					}
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
					JewelStoreStage = HeistStage.None;
					JewelStoreRobberyOn = false;
					if (SmokeOrigin != null)
					{
						SmokeOrigin.Delete();
					}
					foreach (Ped item44 in Civ)
					{
						if (item44 != null)
						{
							item44.Delete();
						}
					}
					foreach (Ped ped38 in Peds)
					{
						if (ped38 != null)
						{
							ped38.Delete();
						}
					}
					foreach (Vehicle vehicle5 in Vehicles)
					{
						if (vehicle5 != null)
						{
							vehicle5.Delete();
						}
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					InHeist = false;
					GTA.UI.Screen.FadeOut(300);
					WAIT(300);
					foreach (Ped ped39 in Peds)
					{
						if (ped39 != null)
						{
							ped39.Delete();
						}
					}
					foreach (Vehicle vehicle6 in Vehicles)
					{
						if (vehicle6 != null)
						{
							vehicle6.Delete();
						}
					}
					foreach (Ped guard17 in Guards)
					{
						if (guard17 != null)
						{
							guard17.Delete();
						}
					}
					foreach (Ped civ18 in Civs)
					{
						if (civ18 != null)
						{
							civ18.Delete();
						}
					}
					WAIT(300);
					GTA.UI.Screen.FadeIn(300);
					placedJammer = false;
					DefendingCops = false;
					HeadingtoArcade = false;
					escapingcops = false;
					VaultOpen = false;
					HeistTime = 0f;
					WAIT(20000);
					Notification.Show("~b~ The Jewel Store Heist was failed!~w~");
					SetFail("The Jewel Store Heist", 0, 0, "you Died!", 0, HeistTime, 4);
					isRobbingCase = false;
					SetNoLongerInHeist();
					foreach (Blip cameraBlip3 in CameraBlips)
					{
						if (cameraBlip3 != null)
						{
							cameraBlip3.Delete();
						}
					}
					foreach (Blip robbedJewelsCasesBlip7 in RobbedJewelsCasesBlips)
					{
						if (robbedJewelsCasesBlip7 != null)
						{
							robbedJewelsCasesBlip7.Delete();
						}
					}
					MissionOn = false;
					break;
				}
				catch
				{
					GTA.UI.Screen.ShowSubtitle("Error on Jewel Store Heist");
					break;
				}
			case HeistIn.LaFuetaBlanca:
			{
				UI.RenderQue = 0f;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
				if (!InLaFuetaBlancaHeist)
				{
					break;
				}
				if (BankHeistStage == 0)
				{
					if (CamerasLeft > 1)
					{
						KurumaBlip.Alpha = 0;
						if (GetDistance_VDIST2(Game.Player.Character.Position, MartinMadrazoHouseEnterance, "<", 100f))
						{
							GTA.UI.Screen.ShowSubtitle("Destroy Martin Madrazo's ~r~Security Cameras~w~");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Make your way to ~y~Martin Madrazo's House~w~");
						}
						if (SecurityCam.Count > 0)
						{
							foreach (Prop item45 in SecurityCam)
							{
								if (!(item45 != null))
								{
									continue;
								}
								if (item45.AttachedBlip == null)
								{
									item45.AddBlip();
									item45.AttachedBlip.Sprite = BlipSprite.Standard;
									item45.AttachedBlip.Color = BlipColor.Red2;
									item45.AttachedBlip.Name = "Camera";
									item45.AttachedBlip.Scale = 0.5f;
								}
								if (!GetDistance_VDIST2(Game.Player.Character.Position, item45.Position, "<", 100f))
								{
									continue;
								}
								if (item45.Health > 999)
								{
									if (ShowCCTVBlipAliveDebug)
									{
										World.DrawMarker(MarkerType.Sphere, item45.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f), Color.White);
									}
								}
								else
								{
									if (item45.Opacity == 255)
									{
										item45.Opacity = 254;
										item45.AttachedBlip.Alpha = 0;
										CamerasLeft--;
										Notification.Show("~y~ Lester ~w~ Cameras Left : " + (CamerasLeft - 1));
									}
									if (ShowCCTVBlipDeadDebug)
									{
										World.DrawMarker(MarkerType.Sphere, item45.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f), Color.Red);
									}
								}
								if (CamerasLeft == 0)
								{
									Notification.Show("~y~ Lester ~w~ All Cameras Destroyed!");
								}
							}
						}
					}
					else if (!hackedMadrazoFrontDoor)
					{
						KurumaBlip.Alpha = 0;
						GTA.UI.Screen.ShowSubtitle("Hack the Front door's ~y~Keypad~w~");
						foreach (KeypadClass martinMadrazoKeyPad in MartinMadrazoKeyPads)
						{
							if (!martinMadrazoKeyPad.SpawnedKeypad && !martinMadrazoKeyPad.SpawnedKeypad)
							{
								martinMadrazoKeyPad.SpawnedKeypad = true;
								martinMadrazoKeyPad.Keypad = GlobalHelper.CreateProp(RequestModel("ch_prop_fingerprint_scanner_01c"), martinMadrazoKeyPad.KeypadPos, new Vector3(0f, 0f, martinMadrazoKeyPad.KeypadRot), false, false);
								martinMadrazoKeyPad.Keypad.IsPositionFrozen = true;
								martinMadrazoKeyPad.Keypad.AddBlip();
								martinMadrazoKeyPad.Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
								martinMadrazoKeyPad.Keypad.AttachedBlip.Scale = 0.75f;
								martinMadrazoKeyPad.Keypad.AttachedBlip.IsShortRange = true;
								Wait(1);
								martinMadrazoKeyPad.Keypad.Position = martinMadrazoKeyPad.Keypad.GetOffsetPosition(new Vector3(0f, 0.05f, 0f));
							}
							if (!martinMadrazoKeyPad.SpawnedKeypad)
							{
								continue;
							}
							float num166 = martinMadrazoKeyPad.KeypadPos.Z - 3f;
							float num167 = martinMadrazoKeyPad.KeypadPos.Z + 3f;
							if (GetDistance_VDIST2(Game.Player.Character.Position, martinMadrazoKeyPad.KeypadPos, ">", 30f))
							{
								martinMadrazoKeyPad.Keypad.AttachedBlip.Alpha = 0;
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, martinMadrazoKeyPad.KeypadPos, "<", 30f))
							{
								if (Game.Player.Character.Position.Z > num166 && Game.Player.Character.Position.Z < num167)
								{
									martinMadrazoKeyPad.Keypad.AttachedBlip.Alpha = 255;
								}
								else
								{
									martinMadrazoKeyPad.Keypad.AttachedBlip.Alpha = 0;
								}
							}
							if (!HackingKeypad)
							{
								if (!GetDistance_VDIST2(Game.Player.Character.Position, martinMadrazoKeyPad.KeypadPos, "<", 10f))
								{
									continue;
								}
								Vector3 offsetPosition19 = martinMadrazoKeyPad.Keypad.GetOffsetPosition(new Vector3(0f, -1f, 0f));
								if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition19, "<", 2f))
								{
									if (!HackingKeypad)
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Hack Keypad");
									}
									if (Game.IsControlJustPressed(Control.Context) && !HackingKeypad)
									{
										LastKeypad = martinMadrazoKeyPad.Keypad;
										if (Phone != null)
										{
											Phone.Delete();
										}
										if (USB != null)
										{
											USB.Delete();
										}
										Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), martinMadrazoKeyPad.KeypadPos, false, false);
										USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), martinMadrazoKeyPad.KeypadPos, false, false);
										USB.IsVisible = false;
										Phone.IsVisible = false;
										USB.IsVisible = true;
										Phone.IsVisible = true;
										int index6 = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
										LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, martinMadrazoKeyPad.Keypad.Position.X, martinMadrazoKeyPad.Keypad.Position.Y, martinMadrazoKeyPad.Keypad.Position.Z, martinMadrazoKeyPad.Keypad.Rotation.X, martinMadrazoKeyPad.Keypad.Rotation.Y, martinMadrazoKeyPad.Keypad.Rotation.Z, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 0.0, 0.0, 0, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "action_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "action_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
										if (Main.DisableHacks)
										{
											if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1393.91f, 1140.1f, 114.8f), "<", 2f))
											{
												hackedMadrazoFrontDoor = true;
											}
											if (GetDistance_VDIST2(Game.Player.Character.Position, UnionDepositoryVaultDoorPos, "<", 10f))
											{
												hackedUnionVaultDoor = true;
												CurrentUnionDPstage = UnionDPStage.InVault;
											}
											if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1113.719f, -3156.489f, -36.77517f), "<", 10f))
											{
												BankHeistStage = 3;
												hackedBikerClubhouseGrateDoor = true;
											}
											Script.Wait(4000);
											martinMadrazoKeyPad.AwaitingHackGUI = false;
											LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
											Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
											Script.Wait(4000);
											Game.Player.Character.Task.ClearAllImmediately();
											if (Phone != null)
											{
												Phone.Delete();
											}
											if (USB != null)
											{
												USB.Delete();
											}
										}
										else
										{
											martinMadrazoKeyPad.AwaitingHackGUI = true;
										}
									}
								}
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200 && martinMadrazoKeyPad.AwaitingHackGUI)
								{
									Scene1 = -1;
									if (martinMadrazoKeyPad.Type == 1)
									{
										FingerprintHack.RunKeypadHack();
									}
									if (martinMadrazoKeyPad.Type == 2)
									{
										FingerprintHack.RunFingerPrintHack();
									}
									martinMadrazoKeyPad.AwaitingHackGUI = false;
									HackingKeypad = true;
								}
							}
							else
							{
								if (!HackingKeypad)
								{
									continue;
								}
								if (Game.IsControlJustPressed(Control.Context) && HackingKeypad)
								{
									martinMadrazoKeyPad.AwaitingHackGUI = false;
									LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								}
								if (!GetDistance_VDIST2(Game.Player.Character.Position, martinMadrazoKeyPad.KeypadPos, "<", 5f))
								{
									continue;
								}
								if (FingerprintHack.HackFinishedID == 2)
								{
									martinMadrazoKeyPad.AwaitingHackGUI = false;
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
									martinMadrazoKeyPad.Keypad.AttachedBlip.Alpha = 0;
									martinMadrazoKeyPad.AwaitingHackGUI = false;
									FingerprintHack.HackFinishedID = 0;
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1393.91f, 1140.1f, 114.8f), "<", 2f))
									{
										hackedMadrazoFrontDoor = true;
									}
									LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "success_exit", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "fail_react_quick_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "fail_react_quick_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								}
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200)
								{
									HackingKeypad = false;
									martinMadrazoKeyPad.AwaitingHackGUI = false;
									if (Phone != null)
									{
										Phone.Delete();
									}
									if (USB != null)
									{
										USB.Delete();
									}
									Scene1 = -1;
									hackedMadrazoFrontDoor = false;
									Game.Player.Character.Task.ClearAll();
								}
							}
						}
					}
					if (hackedMadrazoFrontDoor)
					{
						if (Phone != null)
						{
							Phone.Delete();
						}
						if (USB != null)
						{
							USB.Delete();
						}
						GTA.UI.Screen.ShowSubtitle("Enter Martin Madrazo's House");
						KurumaBlip.Alpha = 255;
						if (GetDistance_VDIST2(Game.Player.Character.Position, MartinMadrazoHouseEnterance, "<", 60f))
						{
							World.DrawMarker(MarkerType.Cylinder, MartinMadrazoHouseEnterance, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), ActivityMarkerColor);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, MartinMadrazoHouseEnterance, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Enter House");
							if (Game.IsControlJustPressed(Control.Context))
							{
								LastRobberyPosition = MartinMadrazoHouseEnterance;
								BankHeistStage = 1;
								StartHouseRobbery(3);
							}
						}
					}
				}
				if (BankHeistStage == 1)
				{
					KurumaBlip.Alpha = 0;
					GTA.UI.Screen.ShowSubtitle("Loot the ~g~Valuables~w~ or leave the ~y~House.~w~");
					if (GetDistance_VDIST2(Game.Player.Character.Position, MartinMadrazoHouseExit, "<", 60f))
					{
						World.DrawMarker(MarkerType.Cylinder, MartinMadrazoHouseExit, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.4f), ActivityMarkerColor);
					}
					if (GetDistance_VDIST2(Game.Player.Character.Position, MartinMadrazoHouseExit, "<", 1.45f))
					{
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ To Exit House");
						if (Game.IsControlJustPressed(Control.Context))
						{
							BankHeistStage = 2;
							EndHouseRobbery();
						}
					}
				}
				if (BankHeistStage != 2)
				{
					break;
				}
				KurumaBlip.Alpha = 0;
				GTA.UI.Screen.ShowSubtitle("Escape the ~r~Cops");
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
				UI.DrawPayment("TAKE", LocalTake);
				if (Game.Player.WantedLevel != 0)
				{
					break;
				}
				if (KurumaBlip != null)
				{
					KurumaBlip.Delete();
				}
				BankHeistStage = 0;
				hackedMadrazoFrontDoor = false;
				foreach (KeypadClass martinMadrazoKeyPad2 in MartinMadrazoKeyPads)
				{
					if (martinMadrazoKeyPad2.Keypad != null)
					{
						martinMadrazoKeyPad2.Keypad.Delete();
					}
					martinMadrazoKeyPad2.SpawnedKeypad = false;
				}
				hackedMadrazoFrontDoor = false;
				foreach (KeypadClass unionKeyPad2 in UnionKeyPads)
				{
					if (unionKeyPad2.Keypad != null)
					{
						unionKeyPad2.Keypad.Delete();
					}
					unionKeyPad2.SpawnedKeypad = false;
				}
				EndHouseRobbery2();
				foreach (Blip cameraBlip4 in CameraBlips)
				{
					cameraBlip4.Delete();
				}
				foreach (Prop item46 in SecurityCam)
				{
					item46.Delete();
				}
				CameraBlips.Clear();
				SecurityCam.Clear();
				BankHeistStage = 0;
				EscapingHouseRobbery = false;
				ManagePlayerFunds.AddFunds(ManageRobberyAnims.CurrentPay);
				MissionScreen missionScreen2 = new MissionScreen();
				if (Bank != null)
				{
					Bank.Delete();
				}
				Wait(10);
				Main.TotalCrewEarned += 0f;
				Main.TotalPlayerEarned += ManageRobberyAnims.CurrentPay;
				XP.CompleteHeist_LaFuentaBlanca++;
				Main.SaveCuts();
				SetNoLongerInHeist();
				MissionScreen.SetPassNoAi("La Fuente Blanca Heist", ManageRobberyAnims.CurrentPay, 0, "The Player has died", 2, HTime, -1);
				int num168 = Game.GameTime + 30000;
				while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num168)
				{
					Script.Wait(0);
				}
				Script.Wait(2000);
				ManageRobberyAnims.CurrentPay = 0;
				XP.AddXP_Percent(XP.CompleteHeist_XPPercent_LaFuentaBlanca);
				ManageRobberyAnims.CurrentPay = 0;
				InLaFuetaBlancaHeist = false;
				break;
			}
			case HeistIn.Fleeca:
			{
				UI.RenderQue = 0f;
				SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Security;
				if (!PlayingFleeca)
				{
					break;
				}
				Ped[] nearbyPeds = World.GetNearbyPeds(Bank.Position, 60f);
				Ped[] array = nearbyPeds;
				foreach (Ped ped in array)
				{
					if (!(ped.Model != PedHash.Security01SMM) || !(ped.CurrentVehicle == null))
					{
						continue;
					}
					if (ped.Killer == Game.Player.Character && ped.Opacity == 255)
					{
						KilledCops++;
						ped.Opacity = 254;
					}
					if (KilledCops >= 2)
					{
						BankHeistResponceType = 1;
					}
					if (KilledCops >= 5)
					{
						BankHeistResponceType = 2;
					}
					RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "COP");
					if (!(ped.RelationshipGroup == relationshipGroup))
					{
						continue;
					}
					if (LocalTake == 0)
					{
						if (BankHeistResponceType == 0)
						{
							if (Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped, Game.Player.Character, 19))
							{
								ped.Task.AimAt(Game.Player.Character.Position, 1);
							}
							else
							{
								ped.Task.AimAt(Game.Player.Character.Position, 1);
							}
						}
						if (BankHeistResponceType == 1 && Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped, Game.Player.Character, 19))
						{
							ped.Task.ShootAt(Game.Player.Character.Position);
						}
						if (BankHeistResponceType != 2)
						{
						}
						continue;
					}
					if (BankHeistResponceType == 0)
					{
						if (Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped, Game.Player.Character, 19))
						{
							ped.Task.ShootAt(Game.Player.Character.Position);
						}
						else
						{
							ped.Task.AimAt(Game.Player.Character.Position, 1);
						}
					}
					if (BankHeistResponceType == 1 && Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, ped, Game.Player.Character, 19))
					{
						ped.Task.ShootAt(Game.Player.Character.Position);
					}
					int bankHeistResponceType3 = BankHeistResponceType;
					int num179 = 2;
				}
				if (Game.Player.IsAlive)
				{
					Vector3 offset2 = Offset;
					if (Class1.teller != null)
					{
						if (GetDistance_VDIST2(Class1.teller.Position, Game.Player.Character.Position, "<", 10f) && tellerwalkStage < 2)
						{
							if (Class1.teller.IsAlive)
							{
								GTA.UI.Screen.ShowSubtitle("Aim at the ~b~ teller~w~ to intimidate him");
							}
							if (!Class1.teller.IsAlive && !escapingcops)
							{
								Game.Player.WantedLevel = 3;
							}
						}
						if (GetDistance_VDIST2(Class1.teller.Position, Offset, "<", 1.5f))
						{
							if (tellerwalkStage == 0)
							{
								tellerwalkStage = 1;
								Offset = Class1.teller.GetOffsetPosition(new Vector3(4f, 0f, 0f));
								Function.Call(Hash.TASK_PLAY_ANIM, Class1.teller, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
								Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Class1.teller, Offset.X, Offset.Y, Offset.Y, 1f, -1, Class1.teller.Heading + 100f, -0.1f);
							}
							else if (tellerwalkStage == 1)
							{
								tellerwalkStage = 2;
								Class1.teller.Task.PlayAnimation("busted", "idle_2_hands_up_2h", -1f, -1f, -1, AnimationFlags.StayInEndFrame | AnimationFlags.Secondary, -1f);
								Class1.teller.Weapons.Drop();
							}
						}
						if (OpenedDoor)
						{
							Prop[] nearbyProps = World.GetNearbyProps(Class1.teller.Position, 10f);
							Prop[] array2 = nearbyProps;
							foreach (Prop prop in array2)
							{
								if (prop.Model == RequestModel(-131754413) && DoorOpenAmt < 90f)
								{
									DoorOpenAmt += 0.5f;
									prop.Heading += 0.5f;
								}
							}
						}
						if (Game.Player.IsTargeting(Class1.teller) && !OpenedDoor)
						{
							OpenedDoor = true;
							Vector3 position = Class1.teller.Position;
							Function.Call(Hash.TASK_PLAY_ANIM, Class1.teller, "ped", "handsup", 4f, 0f, -1, 49, 0f, 0, 0, 0);
							Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Class1.teller, Offset.X, Offset.Y, Offset.Y, 1f, -1, Class1.teller.Heading + 100f, -0.1f);
						}
					}
					if (Bank != null)
					{
						Bank.ShowRoute = true;
					}
					if (InHeist)
					{
						HeistTime += 0.35f;
						if (!HeadingtoArcade && DefendingCops)
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
						}
					}
					if (NotifyEnd && CamerasLeft == 0)
					{
						HeadingtoArcade = true;
					}
					if (CamerasLeft > 1 && SecurityCam.Count > 0)
					{
						foreach (Prop item47 in SecurityCam)
						{
							if (!(item47 != null) || !GetDistance_VDIST2(Game.Player.Character.Position, item47.Position, "<", 100f) || !(item47 != null))
							{
								continue;
							}
							if (item47.AttachedBlip == null)
							{
								item47.AddBlip();
								item47.AttachedBlip.Sprite = BlipSprite.Standard;
								item47.AttachedBlip.Color = BlipColor.Red2;
								item47.AttachedBlip.Name = "Camera";
								item47.AttachedBlip.Scale = 0.5f;
							}
							if (item47.Health > 999)
							{
								if (item47 != null && ShowCCTVBlipAliveDebug)
								{
									World.DrawMarker(MarkerType.Sphere, item47.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f), Color.White);
								}
							}
							else if (item47 != null)
							{
								if (item47.Opacity == 255)
								{
									item47.Opacity = 254;
									if (item47.AttachedBlip != null)
									{
										item47.AttachedBlip.Alpha = 0;
									}
									CamerasLeft--;
									Notification.Show("~y~ Lester ~w~ Cameras Left : " + (CamerasLeft - 1));
								}
								if (ShowCCTVBlipDeadDebug)
								{
									World.DrawMarker(MarkerType.Sphere, item47.Position, Vector3.Zero, Vector3.Zero, new Vector3(0.1f, 0.1f, 0.1f), Color.Red);
								}
							}
							if (CamerasLeft == 0)
							{
								Notification.Show("~y~ Lester ~w~ All Cameras Destroyed!");
							}
						}
					}
					if (CamerasLeft <= 0)
					{
						HeadingtoArcade = false;
					}
					if (!GetCameras && GetDistance_VDIST2(Game.Player.Character.Position, FleecaJammerPos, "<", 30f))
					{
						if (Class1.teller != null)
						{
							Class1.teller.Delete();
						}
						OpenedDoor = false;
						tellerwalkStage = 0;
						DoorOpenAmt = 0f;
						if (Bank_Location == 0)
						{
							int num3 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 313.4f, -280.85f, 54);
							if (num3 != 0)
							{
								Function.Call(Hash.REFRESH_INTERIOR, num3);
							}
							tellerwalkStage = 0;
							Class1.teller = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AMY), new Vector3(313.4f, -280.85f, 54f), -24f);
							Wait(1000);
							Offset = Class1.teller.GetOffsetPosition(new Vector3(-5f, -1f, 0f));
							Class1.teller.BlockPermanentEvents = true;
							Class1.teller.Task.StandStill(2000);
							Class1.teller.AlwaysKeepTask = true;
							Class1.teller.Weapons.Give(WeaponHash.Pistol, 20, true, true);
							Class1.teller.Weapons.Select(WeaponHash.Pistol);
						}
						if (Bank_Location == 1)
						{
							int num4 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 149f, -1042f, 29f);
							if (num4 != 0)
							{
								Function.Call(Hash.REFRESH_INTERIOR, num4);
							}
							tellerwalkStage = 0;
							Class1.teller = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AMY), new Vector3(149f, -1042f, 29f), -21f);
							Wait(1000);
							Offset = Class1.teller.GetOffsetPosition(new Vector3(-5f, -1f, 0f));
							Class1.teller.BlockPermanentEvents = true;
							Class1.teller.Task.StandStill(2000);
							Class1.teller.AlwaysKeepTask = true;
							Class1.teller.Weapons.Give(WeaponHash.Pistol, 20, true, true);
							Class1.teller.Weapons.Select(WeaponHash.Pistol);
						}
						if (Bank_Location == 2)
						{
							int num5 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -351.48f, -51.73f, 49.05f);
							if (num5 != 0)
							{
								Function.Call(Hash.REFRESH_INTERIOR, num5);
							}
							tellerwalkStage = 0;
							Class1.teller = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AMY), new Vector3(-351.7042f, -52.06f, 49.05f), -22f);
							Wait(1000);
							Offset = Class1.teller.GetOffsetPosition(new Vector3(-5f, -1f, 0f));
							Class1.teller.BlockPermanentEvents = true;
							Class1.teller.Task.StandStill(2000);
							Class1.teller.AlwaysKeepTask = true;
							Class1.teller.Weapons.Give(WeaponHash.Pistol, 20, true, true);
							Class1.teller.Weapons.Select(WeaponHash.Pistol);
						}
						if (Bank_Location == 3)
						{
							int num6 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -1211.73f, -332.7145f, 37.7f);
							if (num6 != 0)
							{
								Function.Call(Hash.REFRESH_INTERIOR, num6);
							}
							tellerwalkStage = 0;
							Class1.teller = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AMY), new Vector3(-1211.73f, -332.7145f, 37.7f), 20f);
							Wait(1000);
							Offset = Class1.teller.GetOffsetPosition(new Vector3(-5f, -1f, 0f));
							Class1.teller.BlockPermanentEvents = true;
							Class1.teller.Task.StandStill(2000);
							Class1.teller.AlwaysKeepTask = true;
							Class1.teller.Weapons.Give(WeaponHash.Pistol, 20, true, true);
							Class1.teller.Weapons.Select(WeaponHash.Pistol);
						}
						if (Bank_Location == 4)
						{
							int num7 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, -2960.78f, 483.1551f, 15.69f);
							if (num7 != 0)
							{
								Function.Call(Hash.REFRESH_INTERIOR, num7);
							}
							tellerwalkStage = 0;
							Class1.teller = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AMY), new Vector3(-2960.78f, 483.1551f, 15.69f), 85f);
							Wait(1000);
							Offset = Class1.teller.GetOffsetPosition(new Vector3(-5f, -1f, 0f));
							Class1.teller.BlockPermanentEvents = true;
							Class1.teller.Task.StandStill(2000);
							Class1.teller.AlwaysKeepTask = true;
							Class1.teller.Weapons.Give(WeaponHash.Pistol, 20, true, true);
							Class1.teller.Weapons.Select(WeaponHash.Pistol);
						}
						if (Bank_Location == 5)
						{
							int num8 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1175.025f, 2708.765f, 38.08f);
							if (num8 != 0)
							{
								Function.Call(Hash.REFRESH_INTERIOR, num8);
							}
							tellerwalkStage = 0;
							Class1.teller = GlobalHelper.CreatePed(RequestModel(PedHash.Business01AMY), new Vector3(1175.025f, 2708.765f, 38.08f), 174f);
							Wait(1000);
							Offset = Class1.teller.GetOffsetPosition(new Vector3(-5f, -1f, 0f));
							Class1.teller.BlockPermanentEvents = true;
							Class1.teller.Task.StandStill(2000);
							Class1.teller.AlwaysKeepTask = true;
							Class1.teller.Weapons.Give(WeaponHash.Pistol, 20, true, true);
							Class1.teller.Weapons.Select(WeaponHash.Pistol);
						}
						GetCameras = true;
						Vector3 vector = new Vector3(1175.373f, 2708.62f, 37.96f);
						if (Bank_Location == 0)
						{
							vector = new Vector3(315.0822f, -281.4176f, 54.17f);
						}
						if (Bank_Location == 1)
						{
							vector = new Vector3(149.606f, -1042.537f, 29.363f);
						}
						if (Bank_Location == 2)
						{
							vector = new Vector3(-351.6926f, -51.71f, 49.03f);
						}
						if (Bank_Location == 3)
						{
							vector = new Vector3(-1211.621f, -332.74f, 38f);
						}
						if (Bank_Location == 4)
						{
							vector = new Vector3(-2960.623f, 482.2177f, 15.75f);
						}
						if (Bank_Location == 5)
						{
							vector = new Vector3(1175.373f, 2708.62f, 37.96f);
						}
						int num9 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, vector.X, vector.Y, vector.Z);
						if (num9 != 0)
						{
							Function.Call(Hash.REFRESH_INTERIOR, num9);
							Notification.Show("Refreshed Bank");
							Wait(1000);
							foreach (Blip cameraBlip5 in CameraBlips)
							{
								if (cameraBlip5 != null)
								{
									cameraBlip5.Delete();
								}
							}
							CameraBlips.Clear();
							SecurityCam.Clear();
							if (Bank_Location == 4)
							{
								Prop[] nearbyProps2 = World.GetNearbyProps(new Vector3(vector.X, vector.Y, vector.Z), 40f);
								if (nearbyProps2.Length != 0)
								{
									Prop[] array3 = nearbyProps2;
									foreach (Prop prop2 in array3)
									{
										if (prop2 != null)
										{
											if (prop2.Model.GetHashCode() == -1007354661)
											{
												prop2.Opacity = 255;
												SecurityCam.Add(prop2);
											}
											if (prop2.Model.GetHashCode() == -1842407088)
											{
												prop2.Opacity = 255;
												SecurityCam.Add(prop2);
												prop2.AddBlip();
												prop2.AttachedBlip.Sprite = BlipSprite.Standard;
												prop2.AttachedBlip.Color = BlipColor.Red2;
												prop2.AttachedBlip.Name = "Camera";
												prop2.AttachedBlip.Scale = 0.5f;
												CameraBlips.Add(prop2.AttachedBlip);
											}
										}
									}
								}
								nearbyProps2 = World.GetNearbyProps(new Vector3(vector.X, vector.Y, vector.Z), 40f, RequestModel("v_ilev_gb_teldr"));
								if (nearbyProps2.Length != 0)
								{
									Prop[] array4 = nearbyProps2;
									foreach (Prop prop3 in array4)
									{
										if (prop3 != null)
										{
											prop3.IsPositionFrozen = false;
										}
									}
								}
							}
							if (Bank_Location == 5)
							{
								Prop[] nearbyProps3 = World.GetNearbyProps(new Vector3(vector.X, vector.Y, vector.Z), 10f, RequestModel("prop_cctv_cam_07a"));
								if (nearbyProps3.Length != 0)
								{
									Prop[] array5 = nearbyProps3;
									foreach (Prop prop4 in array5)
									{
										if (prop4 != null && GetDistance_VDIST2(prop4.Position, new Vector3(1169.849f, 2711.392f, 39.9f), ">", 1.5f))
										{
											prop4.Opacity = 255;
											SecurityCam.Add(prop4);
											prop4.AddBlip();
											prop4.AttachedBlip.Sprite = BlipSprite.Standard;
											prop4.AttachedBlip.Color = BlipColor.Red2;
											prop4.AttachedBlip.Name = "Camera";
											prop4.AttachedBlip.Scale = 0.5f;
											CameraBlips.Add(prop4.AttachedBlip);
										}
									}
								}
								nearbyProps3 = World.GetNearbyProps(new Vector3(vector.X, vector.Y, vector.Z), 10f, RequestModel("prop_cctv_cam_06a"));
								if (nearbyProps3.Length != 0)
								{
									Prop[] array6 = nearbyProps3;
									foreach (Prop prop5 in array6)
									{
										if (prop5 != null && GetDistance_VDIST2(prop5.Position, new Vector3(1169.849f, 2711.392f, 39.9f), ">", 1.5f))
										{
											prop5.Opacity = 255;
											SecurityCam.Add(prop5);
											prop5.AddBlip();
											prop5.AttachedBlip.Sprite = BlipSprite.Standard;
											prop5.AttachedBlip.Color = BlipColor.Red2;
											prop5.AttachedBlip.Name = "Camera";
											prop5.AttachedBlip.Scale = 0.5f;
											CameraBlips.Add(prop5.AttachedBlip);
										}
									}
								}
								nearbyProps3 = World.GetNearbyProps(new Vector3(vector.X, vector.Y, vector.Z), 30f, RequestModel("v_ilev_gb_teldr"));
								if (nearbyProps3.Length != 0)
								{
									Prop[] array7 = nearbyProps3;
									foreach (Prop prop6 in array7)
									{
										if (prop6 != null)
										{
											prop6.IsPositionFrozen = false;
										}
									}
								}
							}
							if (Bank_Location != 4 && Bank_Location != 5)
							{
								Prop[] nearbyProps4 = World.GetNearbyProps(new Vector3(vector.X, vector.Y, vector.Z), 40f, RequestModel("prop_cctv_cam_07a"));
								if (nearbyProps4.Length != 0)
								{
									Prop[] array8 = nearbyProps4;
									foreach (Prop prop7 in array8)
									{
										if (prop7 != null)
										{
											prop7.Opacity = 255;
											SecurityCam.Add(prop7);
											prop7.AddBlip();
											prop7.AttachedBlip.Sprite = BlipSprite.Standard;
											prop7.AttachedBlip.Color = BlipColor.Red2;
											prop7.AttachedBlip.Name = "Camera";
											prop7.AttachedBlip.Scale = 0.5f;
											CameraBlips.Add(prop7.AttachedBlip);
										}
									}
								}
								nearbyProps4 = World.GetNearbyProps(new Vector3(vector.X, vector.Y, vector.Z), 40f, RequestModel("prop_cctv_cam_06a"));
								if (nearbyProps4.Length != 0)
								{
									Prop[] array9 = nearbyProps4;
									foreach (Prop prop8 in array9)
									{
										if (prop8 != null)
										{
											prop8.Opacity = 255;
											SecurityCam.Add(prop8);
											prop8.AddBlip();
											prop8.AttachedBlip.Sprite = BlipSprite.Standard;
											prop8.AttachedBlip.Color = BlipColor.Red2;
											prop8.AttachedBlip.Name = "Camera";
											prop8.AttachedBlip.Scale = 0.5f;
											CameraBlips.Add(prop8.AttachedBlip);
										}
									}
								}
								nearbyProps4 = World.GetNearbyProps(new Vector3(vector.X, vector.Y, vector.Z), 40f, RequestModel("v_ilev_gb_teldr"));
								if (nearbyProps4.Length != 0)
								{
									Prop[] array10 = nearbyProps4;
									foreach (Prop prop9 in array10)
									{
										if (prop9 != null)
										{
											prop9.IsPositionFrozen = false;
										}
									}
								}
							}
							if (FleecaKeypad != null)
							{
								FleecaKeypad.Delete();
							}
							FleecaKeypad = GlobalHelper.CreateProp(RequestModel("ba_prop_battle_secpanel"), FleecaVaultHackLocation, new Vector3(0f, 0f, FleecaVaultHackHeading), false, false);
							FleecaKeypad.IsPositionFrozen = true;
							FleecaKeypad.Opacity = 0;
							FleecaKeypad.AddBlip();
							FleecaKeypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
							FleecaKeypad.AttachedBlip.Scale = 0.75f;
							FleecaKeypad.AttachedBlip.IsShortRange = true;
							FleecaKeypad.AttachedBlip.Color = BlipColor.Blue;
						}
						CamerasLeft = SecurityCam.Count + 1;
					}
					if (FleecaHeistStage == 0)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, VaultPosition, "<", 10f))
						{
							GTA.UI.Screen.ShowSubtitle("Hack the ~b~keypad~w~ to open the ~y~Vault Door~w~");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Make your way to the marked ~y~Fleeca Bank~w~");
						}
						Prop[] nearbyProps5 = World.GetNearbyProps(Game.Player.Character.Position, 1f);
						if (GetDistance_VDIST2(Game.Player.Character.Position, VaultPosition, "<", 3f))
						{
							Prop[] nearbyProps6 = World.GetNearbyProps(VaultPosition, 1f);
							if (nearbyProps6.Length != 0)
							{
								Prop[] array11 = nearbyProps6;
								foreach (Prop prop10 in array11)
								{
									if (prop10 != null && prop10.Rotation.Z >= (float)CloseValue)
									{
										float z = prop10.Rotation.Z;
										prop10.Rotation = new Vector3(0f, 0f, OpenValue);
									}
								}
							}
						}
						InHeist = true;
						if (InsurgentBLip != null && Insurgent != null)
						{
							InsurgentBLip.Position = Insurgent.Position;
						}
						if (KurumaBlip != null && Kuruma != null)
						{
							KurumaBlip.Position = Kuruma.Position;
						}
						if (FleecaKeypad != null && GetDistance_VDIST2(Game.Player.Character.Position, FleecaKeypad.Position, ">", 50f))
						{
							if (Phone != null)
							{
								Phone.Delete();
							}
							if (USB != null)
							{
								USB.Delete();
							}
							FleecaKeypad.Delete();
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaVaultHackLocation, "<", 25f))
						{
							bool flag24 = FleecaKeypad == null;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FleecaVaultHackLocation, "<", 2f))
						{
							if (!HackingFleecaKeypad)
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Hack Keypad");
							}
							if (Game.IsControlJustPressed(Control.Context))
							{
								if (!HackingFleecaKeypad)
								{
									Waittime = 0;
									hackEnterAnimDone = false;
									if (Phone != null)
									{
										Phone.Delete();
									}
									if (USB != null)
									{
										USB.Delete();
									}
									Vector3 offsetPosition = FleecaKeypad.GetOffsetPosition(new Vector3(0f, 0.05f, -0.15f));
									Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), offsetPosition, false, false);
									USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), offsetPosition, false, false);
									USB.IsVisible = false;
									Phone.IsVisible = false;
									USB.IsVisible = true;
									Phone.IsVisible = true;
									int index = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
									LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0, 0, FleecaVaultHackHeading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 0.0, 0.0, 0, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "action_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "action_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									if (Main.DisableHacks)
									{
										Script.Wait(4000);
										LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0, 0, FleecaVaultHackHeading, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
										Script.Wait(4000);
										Game.Player.Character.Task.ClearAllImmediately();
										HackingFleecaKeypad = false;
										DefendingCops = true;
										Waittime = Game.GameTime + (int)(60000f * Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0.25f, 1.5f));
										if (Phone != null)
										{
											Phone.Delete();
										}
										if (USB != null)
										{
											USB.Delete();
										}
										FleecaHeistStage = 1;
									}
									else
									{
										HackingFleecaKeypad = true;
										hackEnterAnimDone = false;
									}
								}
								else
								{
									hackEnterAnimDone = true;
									if (Phone != null)
									{
										Phone.Delete();
									}
									if (USB != null)
									{
										USB.Delete();
									}
									Vector3 offsetPosition2 = FleecaKeypad.GetOffsetPosition(new Vector3(0f, 0.05f, -0.15f));
									Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), offsetPosition2, false, false);
									USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), offsetPosition2, false, false);
									USB.IsVisible = false;
									Phone.IsVisible = false;
									USB.IsVisible = true;
									Phone.IsVisible = true;
									int index2 = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
									LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0, 0, FleecaVaultHackHeading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									HackingFleecaKeypad = false;
								}
							}
							if (HackingFleecaKeypad && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 && !hackEnterAnimDone)
							{
								FingerprintHack.RunCircuitHack();
								hackEnterAnimDone = true;
							}
							if (Game.IsControlJustPressed(Control.VehicleDuck))
							{
								FingerprintHack.HackFinishedID = 1;
							}
							if (FingerprintHack.HackFinishedID == 1)
							{
								FleecaKeypad.AttachedBlip.Alpha = 0;
								FingerprintHack.HackFinishedID = 0;
								hackEnterAnimDone = true;
								if (Phone != null)
								{
									Phone.Delete();
								}
								if (USB != null)
								{
									USB.Delete();
								}
								Vector3 offsetPosition3 = FleecaKeypad.GetOffsetPosition(new Vector3(0f, 0.05f, -0.15f));
								Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), offsetPosition3, false, false);
								USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), offsetPosition3, false, false);
								USB.IsVisible = false;
								Phone.IsVisible = false;
								USB.IsVisible = true;
								Phone.IsVisible = true;
								int index3 = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
								LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0, 0, FleecaVaultHackHeading, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								HackingFleecaKeypad = false;
								DefendingCops = true;
								Waittime = Game.GameTime + (int)(60000f * Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 0.25f, 1.5f));
								FleecaHeistStage = 1;
								placedJammer = false;
							}
							if (!HackingFleecaKeypad && hackEnterAnimDone && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
							{
								if (Phone != null)
								{
									Phone.Delete();
								}
								if (USB != null)
								{
									USB.Delete();
								}
								hackEnterAnimDone = false;
								Game.Player.Character.Task.ClearAll();
							}
						}
					}
					if (FleecaHeistStage == 1)
					{
						if (!HackingFleecaKeypad && hackEnterAnimDone && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							if (Phone != null)
							{
								Phone.Delete();
							}
							if (USB != null)
							{
								USB.Delete();
							}
							hackEnterAnimDone = false;
							Game.Player.Character.Task.ClearAll();
						}
						GTA.UI.Screen.ShowSubtitle("Wait for the ~y~Vault~w~ to Open");
						Prop[] nearbyProps7 = World.GetNearbyProps(Game.Player.Character.Position, 1f);
						if (GetDistance_VDIST2(Game.Player.Character.Position, VaultPosition, "<", 3f))
						{
							Prop[] nearbyProps8 = World.GetNearbyProps(VaultPosition, 1f);
							if (nearbyProps8.Length != 0)
							{
								Prop[] array12 = nearbyProps8;
								foreach (Prop prop11 in array12)
								{
									if (prop11 != null && prop11.Rotation.Z >= (float)CloseValue)
									{
										float z2 = prop11.Rotation.Z;
										prop11.Rotation = new Vector3(0f, 0f, OpenValue);
									}
								}
							}
						}
						UI.DrawTime("VAULT", Waittime - Game.GameTime);
						UI.DrawPayment("TAKE", LocalTake);
						Game.Player.WantedLevel = 0;
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
						if (Game.GameTime >= Waittime && Waittime != 0)
						{
							AddCratesFleeca();
							if (JammmerProp != null)
							{
								JammmerProp.Delete();
							}
							if (jammerPartA != null)
							{
								jammerPartA.Delete();
							}
							if (jammerPartB != null)
							{
								jammerPartB.Delete();
							}
							DefendingCops = false;
							VaultOpen = true;
							Notification.Show("~y~ Lester ~w~ : Get in the Vault and Loot the valuables");
							FleecaHeistStage = 2;
						}
					}
					if (FleecaHeistStage == 2)
					{
						if (ManageRobberyAnims.CurrentPay <= 200000)
						{
							Game.Player.WantedLevel = 0;
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 4);
						}
						if (ManageRobberyAnims.ValuableLootLeft <= 0 && !ManageRobberyAnims.PlayingAnim && ManageRobberyAnims.CurrentPay > 1000)
						{
							FleecaHeistStage = 3;
						}
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
						UI.DrawPayment("TAKE", LocalTake);
						if (ManageRobberyAnims.ValuableLootLeft > 0)
						{
							if ((float)ManageRobberyAnims.CurrentPay <= ManageRobberyAnims.Heist_Average_Pay / 2f)
							{
								GTA.UI.Screen.ShowSubtitle("Loot the ~g~Valuables~w~");
							}
							else
							{
								GTA.UI.Screen.ShowSubtitle("Leave the area.");
							}
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Leave the area.");
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, VaultPosition, "<", 20f))
						{
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 5);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, VaultPosition, ">", 20f) && !escapingcops)
						{
							FleecaHeistStage = 3;
							Game.Player.WantedLevel = 5;
							escapingcops = true;
						}
						Prop prop12 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, VaultPosition.X, VaultPosition.Y, VaultPosition.Z, 15f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_gb_vauldr"), 0, 0, 0);
						Prop prop13 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, VaultPosition.X, VaultPosition.Y, VaultPosition.Z, 15f, Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_heist_sec_door"), 0, 0, 0);
						if (prop12 != null && prop12.Rotation.Z >= (float)CloseValue)
						{
							prop12.IsPositionFrozen = false;
							float z3 = prop12.Rotation.Z;
							float num17 = (prop12.Heading = z3 + CloseAmt);
						}
						if (prop13 != null)
						{
							if (prop12 != null && prop12.Exists())
							{
								prop13.Opacity = 0;
							}
							if (prop13.Rotation.Z >= (float)CloseValue)
							{
								prop13.IsPositionFrozen = false;
								float z4 = prop13.Rotation.Z;
								float num19 = (prop13.Heading = z4 + CloseAmt);
							}
						}
						if (ManageRobberyAnims.ValuableLootLeft <= 0)
						{
							escapingcops = true;
						}
					}
					if (FleecaHeistStage == 3)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, VaultPosition, "<", 20f) && ManageRobberyAnims.ValuableLootLeft == 0 && !ManageRobberyAnims.PlayingAnim)
						{
							Game.Player.WantedLevel = 4;
							Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
						}
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
						UI.DrawPayment("TAKE", LocalTake);
						if (Game.Player.WantedLevel != 0)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, VaultPosition, "<", 20f))
							{
								Game.Player.WantedLevel = 4;
							}
							GTA.UI.Screen.ShowSubtitle("Escape The ~r~Cops~w~");
						}
						if (InsurgentBLip != null && Insurgent != null)
						{
							InsurgentBLip.Position = Insurgent.Position;
						}
						if (KurumaBlip != null && Kuruma != null)
						{
							KurumaBlip.Position = Kuruma.Position;
						}
						if (Game.Player.WantedLevel == 0)
						{
							FleecaHeistStage = 4;
							escapingcops = false;
							HeadingtoArcade = true;
						}
					}
					if (FleecaHeistStage == 4)
					{
						Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
						Bank.ShowRoute = false;
						if (InsurgentBLip != null && Insurgent != null)
						{
							InsurgentBLip.Position = Insurgent.Position;
						}
						if (KurumaBlip != null && Kuruma != null)
						{
							KurumaBlip.Position = Kuruma.Position;
						}
						if (Game.Player.WantedLevel == 0)
						{
							GTA.UI.Screen.ShowSubtitle("Leave the area.");
							if (GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, ">", 200f))
							{
								FleecaHeistStage = -1;
								ManageRobberyAnims.DeleteLoot();
								if (MoneyProp != null)
								{
									MoneyProp.Delete();
								}
								if (_cashGrabTray1 != null)
								{
									_cashGrabTray1.Delete();
								}
								if (bagProp != null)
								{
									bagProp.Delete();
								}
								if (jammerPartA != null)
								{
									jammerPartA.Delete();
								}
								if (jammerPartB != null)
								{
									jammerPartB.Delete();
								}
								if (Class1.teller != null)
								{
									Class1.teller.Delete();
								}
								PlayingCutscene = false;
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
								int crewAlive = 1;
								if (RobberA != null && RobberA.IsAlive)
								{
									crewAlive++;
								}
								float num20 = ManageRobberyAnims.CurrentPay;
								float num21 = ManageRobberyAnims.CurrentPay;
								MissionScreen missionScreen = new MissionScreen();
								if (Bank != null)
								{
									Bank.Delete();
								}
								Wait(10);
								Main.TotalCrewEarned += 0f;
								Main.TotalPlayerEarned += num21;
								XP.CompleteHeist_Fleeca++;
								Main.SaveCuts();
								SetNoLongerInHeist();
								MissionScreen.SetPassNoAi("The Fleeca Bank Heist", (int)num21, 0, "The Player has died", crewAlive, HTime, 1);
								int num22 = Game.GameTime + 30000;
								while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num22)
								{
									Script.Wait(0);
								}
								Script.Wait(2000);
								ManageRobberyAnims.CurrentPay = 0;
								XP.AddXP_Percent(XP.CompleteHeist_XPPercent_Fleeca);
								if (FleecaKeypad != null)
								{
									FleecaKeypad.Delete();
								}
								HeistTime = 0f;
								GTA.UI.Screen.FadeOut(300);
								Wait(300);
								if (JammmerProp != null)
								{
									JammmerProp.Delete();
								}
								if (Insurgent != null)
								{
									Insurgent.Delete();
								}
								if (Kuruma != null)
								{
									Kuruma.Delete();
								}
								if (InsurgentBLip != null)
								{
									InsurgentBLip.Delete();
								}
								if (KurumaBlip != null)
								{
									KurumaBlip.Delete();
								}
								Wait(300);
								GTA.UI.Screen.FadeIn(300);
								DisplayHelpTextThisFrame("~y~ Lester ~w~ : Fleeca Heist #" + Bank_Location + " Complete");
								Random random2 = new Random();
								InHeist = false;
								ManagePlayerFunds.AddFunds((int)num21);
								HeadingtoArcade = false;
								Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
								Notification.Show("~b~ Heist Complete ~w~");
								PlayingFleeca = false;
								MissionOn = false;
								RequestEndCutscene = false;
								Completedfleeca = 1;
							}
						}
					}
				}
				if (!Game.Player.IsAlive && InHeist)
				{
					ManageRobberyAnims.DeleteLoot();
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					if (_cashGrabTray1 != null)
					{
						_cashGrabTray1.Delete();
					}
					if (bagProp != null)
					{
						bagProp.Delete();
					}
					PlayingFleeca = false;
					Function.Call(Hash.SET_FAKE_WANTED_LEVEL, 0);
					if (jammerPartA != null)
					{
						jammerPartA.Delete();
					}
					if (jammerPartB != null)
					{
						jammerPartB.Delete();
					}
					if (Class1.teller != null)
					{
						Class1.teller.Delete();
					}
					Function.Call(Hash.STOP_SOUND, Sound);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
					InHeist = false;
					GTA.UI.Screen.FadeOut(300);
					Wait(300);
					if (JammmerProp != null)
					{
						JammmerProp.Delete();
					}
					if (Insurgent != null)
					{
						Insurgent.Delete();
					}
					if (Kuruma != null)
					{
						Kuruma.Delete();
					}
					if (InsurgentBLip != null)
					{
						InsurgentBLip.Delete();
					}
					if (KurumaBlip != null)
					{
						KurumaBlip.Delete();
					}
					Wait(300);
					GTA.UI.Screen.FadeIn(300);
					placedJammer = false;
					DefendingCops = false;
					HeadingtoArcade = false;
					escapingcops = false;
					VaultOpen = false;
					HeistTime = 0f;
					DisplayHelpTextThisFrame("~y~ Lester ~w~ : Fleeca Heist #" + Bank_Location + " Failed");
					Wait(20000);
					Notification.Show("~b~ The Fleeca Heist was failed!~w~");
					MissionScreen.SetFail("The Fleeca Heist", 0, 0, "you Died!", 0, HeistTime, 1);
					if (FleecaKeypad != null)
					{
						FleecaKeypad.Delete();
					}
					SetNoLongerInHeist();
					MissionOn = false;
				}
				break;
			}
			}
		}
	}
}
