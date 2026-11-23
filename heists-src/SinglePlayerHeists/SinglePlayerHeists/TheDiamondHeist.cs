using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using HKHModHelperNew;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;
using SinglePlayerHeists.UtilsMisc;
using SmartAIGuards;

namespace SinglePlayerHeists
{
	public class TheDiamondHeist : Script
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

		public class PedTaskLoop : Script
		{
			public List<Vector3> Loop { get; set; }

			public int AtPoint { get; set; }

			public PedTaskLoop()
			{
			}

			public PedTaskLoop(List<Vector3> L, int Point)
			{
				Loop = L;
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

		public enum FloorOn
		{
			LoadingBay,
			Offices,
			Vault,
			Hotel
		}

		public static bool LesterToldPlayerAboutContractCut;

		public static bool PoliceAlearted;

		public List<ObjectPlacementMap> SwatEntties = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 483379, 3089277354u, new Vector3(880.948242f, -89.14629f, 78.77174f), new Vector3(-0.261856049f, 7.83236027f, 55.73889f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 625186, 3089277354u, new Vector3(815.582f, -45.43746f, 80.20473f), new Vector3(-0.483169258f, -1.69841826f, 39.9340973f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 410402, 3089277354u, new Vector3(795.425049f, -32.9233437f, 80.1744461f), new Vector3(0.8309335f, -2.34679532f, 69.47742f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 591393, 3089277354u, new Vector3(1012.52515f, 197.158264f, 80.51064f), new Vector3(0.6369935f, -0.0708917454f, 83.143074f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 351264, 3089277354u, new Vector3(1005.3678f, 199.8899f, 80.53464f), new Vector3(-1.1961602f, -0.0890139937f, 54.3681221f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 27430, 3089277354u, new Vector3(940.261658f, -170.218414f, 74.08131f), new Vector3(1.46862173f, 0.8064466f, 77.56677f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 247087, 3089277354u, new Vector3(933.404053f, -167.426254f, 74.0742645f), new Vector3(-2.46319532f, -0.397335172f, 50.41641f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 308288, 3089277354u, new Vector3(1028.6626f, -39.6484528f, 74.85745f), new Vector3(3.04831147f, -3.81112361f, 34.3888626f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 665925, 3089277354u, new Vector3(1032.04712f, -45.65819f, 74.8984146f), new Vector3(MathF.PI * -221f / 252f, -2.51142979f, 13.5870218f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 725270, 3089277354u, new Vector3(933.504944f, 93.212616f, 78.72199f), new Vector3(-1.76575482f, -0.622951269f, 119.105011f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 375830, 3089277354u, new Vector3(925.7959f, 81.84633f, 78.46813f), new Vector3(0.851414561f, -0.01732818f, 149.84021f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 734536, 3089277354u, new Vector3(900.0117f, 11.0259047f, 78.48012f), new Vector3(0.183496788f, -1.63666153f, -103.5292f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 586537, 3089277354u, new Vector3(887.17334f, 19.0007687f, 78.5072861f), new Vector3(-0.762648463f, -1.74343431f, -149.2389f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 699448, 2374966032u, new Vector3(888.975f, 19.9037914f, 78.8908539f), new Vector3(-0f, -0f, -59.65969f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 536624, 2374966032u, new Vector3(890.578552f, 16.76058f, 78.89162f), new Vector3(-0f, -0f, -67.18411f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 420126, 2374966032u, new Vector3(895.174255f, 12.9098673f, 78.875885f), new Vector3(-0f, -0f, -12.5732536f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 667416, 2374966032u, new Vector3(903.1762f, 12.0004444f, 78.98438f), new Vector3(1.19258261f, -0.06538349f, -16.7151871f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 202038, 2374966032u, new Vector3(798.8562f, -31.25118f, 80.58908f), new Vector3(-0f, -0f, -57.6136131f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 19259, 2374966032u, new Vector3(799.8613f, -34.1763763f, 80.4749f), new Vector3(-0f, -0f, -59.9678421f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 321874, 2374966032u, new Vector3(813.0071f, -42.1112061f, 80.4399643f), new Vector3(0f, 0f, 13.6193151f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 489007, 2374966032u, new Vector3(815.192566f, -42.0626335f, 80.5885544f), new Vector3(-0f, -0f, -27.3233242f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 67098, 2374966032u, new Vector3(877.4348f, -83.036644f, 78.75968f), new Vector3(-0f, -0f, -41.6537781f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 287512, 2374966032u, new Vector3(879.677246f, -85.38761f, 78.7700958f), new Vector3(-0f, -0f, -25.73858f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 95021, 2374966032u, new Vector3(884.2808f, -88.79674f, 78.83487f), new Vector3(-0f, -0f, -28.39996f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 137797, 2374966032u, new Vector3(1026.33911f, -40.7985573f, 74.95394f), new Vector3(0f, -0f, 109.229843f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 597826, 2374966032u, new Vector3(1029.91467f, -46.9181442f, 74.9546051f), new Vector3(0f, -0f, 124.531349f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 579618, 2374966032u, new Vector3(1028.8667f, -43.7261467f, 74.984f), new Vector3(0f, -0f, 114.134445f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 201792, 2374966032u, new Vector3(1029.37549f, -46.241f, 74.95439f), new Vector3(0f, -0f, 109.837669f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 136234, 2374966032u, new Vector3(1027.06213f, -41.348175f, 74.955f), new Vector3(0f, -0f, 114.03228f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 59420, 2374966032u, new Vector3(932.0406f, -164.03653f, 74.34022f), new Vector3(0f, 0f, 3.32630372f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 283192, 2374966032u, new Vector3(934.907f, -166.416245f, 74.46873f), new Vector3(8.540027E-07f, -5.86568035E-07f, -29.5317116f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 651092, 2374966032u, new Vector3(937.973938f, -167.174316f, 74.53588f), new Vector3(-0f, -0f, -21.9547482f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 383534, 2374966032u, new Vector3(944.582f, -168.960831f, 74.37092f), new Vector3(-0f, -0f, -28.76141f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 217161, 2374966032u, new Vector3(944.9187f, -171.079926f, 74.48035f), new Vector3(-0f, -0f, -69.29527f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Maverick"), 208199, 353883353u, new Vector3(901.5301f, 38.53149f, 111.71283f), new Vector3(0.150559515f, -0.0001355437f, -116.793304f), 134, 0, 112, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 462116, 2374966032u, new Vector3(902.5198f, 40.24696f, 111.309662f), new Vector3(-0f, -0f, -57.3198738f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 585775, 2374966032u, new Vector3(904.2954f, 39.79166f, 111.312256f), new Vector3(-0f, -0f, -68.98936f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 71721, 2374966032u, new Vector3(905.1403f, 35.92512f, 111.310608f), new Vector3(0f, -0f, -128.666367f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 394297, 2374966032u, new Vector3(902.423f, 35.7741623f, 111.311676f), new Vector3(0f, -0f, -166.437775f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 725027, 2374966032u, new Vector3(942.964539f, 64.8689957f, 113.534966f), new Vector3(-0f, -0f, -68.52647f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 464937, 2374966032u, new Vector3(934.0893f, 47.61857f, 113.56411f), new Vector3(0f, -0f, -143.912521f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 679735, 2374966032u, new Vector3(909.3337f, 26.6534882f, 111.332268f), new Vector3(0f, -0f, 174.422989f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 328771, 2374966032u, new Vector3(909.376831f, 7.20565653f, 111.26223f), new Vector3(0f, -0f, -98.2095642f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 116023, 2374966032u, new Vector3(921.309f, 22.1564064f, 113.41983f), new Vector3(-0f, -0f, -78.35037f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 745807, 3089277354u, new Vector3(867.2043f, -122.184311f, 78.97419f), new Vector3(-0.9239138f, 1.99857342f, 67.84784f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 486718, 3089277354u, new Vector3(859.6316f, -117.651f, 79.04728f), new Vector3(-2.40293479f, -0.59062624f, 44.02532f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 216907, 3089277354u, new Vector3(1079.44116f, -97.7075f, 81.8039246f), new Vector3(-1.283347f, -1.69010758f, -27.41785f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 79144, 3089277354u, new Vector3(1088.58289f, -90.97413f, 81.71827f), new Vector3(0.6905425f, -4.114062f, -71.3167343f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 722751, 3089277354u, new Vector3(1099.76868f, -86.95342f, 81.83314f), new Vector3(0.3370165f, -0.7617897f, -61.71644f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 538172, 2374966032u, new Vector3(1075.39111f, -99.23945f, 82.18568f), new Vector3(0f, 0f, 28.3713322f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 240194, 2374966032u, new Vector3(1078.819f, -94.00639f, 81.979454f), new Vector3(0f, 0f, 31.2864113f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 215108, 2374966032u, new Vector3(1081.1615f, -94.0487061f, 82.03343f), new Vector3(0f, 0f, 31.2864628f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 737083, 2374966032u, new Vector3(1084.10559f, -90.84415f, 81.94015f), new Vector3(0f, 0f, 33.692f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 763231, 2374966032u, new Vector3(1090.40564f, -88.32306f, 81.98141f), new Vector3(0f, 0f, 19.1562862f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 281168, 2374966032u, new Vector3(1095.50732f, -87.67726f, 82.12139f), new Vector3(0f, 0f, 65.0146f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 135235, 2374966032u, new Vector3(1098.83179f, -84.5375443f, 82.0939f), new Vector3(0f, 0f, 10.96728f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 150116, 2374966032u, new Vector3(1103.55493f, -84.3137054f, 82.18586f), new Vector3(0f, 0f, 1.90922749f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "RCV"), 647469, 2601952180u, new Vector3(1032.219f, -200.528015f, 70.06665f), new Vector3(-1.00296891f, 0.234347269f, -31.3989754f), 75, 12, 6, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "RCV"), 400655, 2601952180u, new Vector3(1008.08331f, -209.269058f, 70.05668f), new Vector3(-0.7682939f, -0.9871561f, -115.033112f), 144, 12, 6, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 261675, 3089277354u, new Vector3(1025.31458f, -207.8942f, 69.88804f), new Vector3(0.5450281f, 0.537901461f, -61.1170769f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Police Riot"), 190802, 3089277354u, new Vector3(1018.19745f, -212.013214f, 69.79376f), new Vector3(-1.45748317f, 1.44182992f, -89.67496f), 132, 0, 0, -1, -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 732749, 2374966032u, new Vector3(1025.67749f, -205.092621f, 70.260376f), new Vector3(0f, 0f, 36.2646141f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 776806, 2374966032u, new Vector3(1019.2821f, -209.5394f, 70.18508f), new Vector3(0f, 0f, 12.1073132f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 598836, 2374966032u, new Vector3(1015.04932f, -210.050613f, 70.2253f), new Vector3(0f, 0f, 8.319679f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 395073, 2374966032u, new Vector3(1011.43719f, -208.495987f, 70.2051f), new Vector3(-0f, -0f, -23.77092f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 338214, 2374966032u, new Vector3(1003.424f, -205.7397f, 70.3892441f), new Vector3(0f, 0f, 19.73299f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 738876, 2374966032u, new Vector3(1028.11572f, -202.9455f, 70.26018f), new Vector3(0f, 0f, 57.14646f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 261210, 2374966032u, new Vector3(2797f / MathF.E, -201.994553f, 70.2540741f), new Vector3(0f, -0f, 63.442318f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "SWAT"), 26215, 2374966032u, new Vector3(1032.71582f, -195.437729f, 70.15151f), new Vector3(0f, 0f, 35.1152077f), 2725352035u, 200, 100, "false", "None", "None", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> BugStarVents = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_wall_vent_03"), 547892, 1161501201u, new Vector3(2476.62134f, -285.558258f, -69.1074448f), new Vector3(0f, -0f, -178.093628f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_wall_vent_03"), 28725, 1161501201u, new Vector3(2506.2583f, -279.246979f, -68.95074f), new Vector3(0f, 0f, -89.85578f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_wall_vent_03"), 613191, 1161501201u, new Vector3(2537.18213f, -272.930969f, -69.17092f), new Vector3(0f, -0f, -90.77644f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_wall_vent_03"), 102458, 1161501201u, new Vector3(2476.23242f, -273.2306f, -68.92544f), new Vector3(0f, 0f, 89.5339661f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_wall_vent_03"), 737051, 1161501201u, new Vector3(2489.31f, -260.6586f, -69.65765f), new Vector3(0f, -0f, -179.82515f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_wall_vent_03"), 333878, 1161501201u, new Vector3(2465.164f, -274.788177f, -68.94242f), new Vector3(0f, -0f, 94.59796f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static List<ObjectPlacementMap> Groupe6Cams = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 504579, 289451089u, new Vector3(2465.97021f, -283.843964f, -68.31688f), new Vector3(0f, -0f, 139.53244f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 511076, 289451089u, new Vector3(2491.99951f, -285.0168f, -68.0269f), new Vector3(0f, -0f, -132.076965f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 337695, 289451089u, new Vector3(2501.43921f, -273.6608f, -68.3269043f), new Vector3(0f, 0f, 30.1673737f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 163454, 289451089u, new Vector3(2492.78613f, -290.181519f, -68.8363953f), new Vector3(0f, -0f, -133.552765f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "p_cctv_s"), 149071, 289451089u, new Vector3(2474.0813f, -284.9948f, -68.0169f), new Vector3(0f, -0f, -130.531021f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static bool CompletedBugstarTasks;

		public static int BugstarTaskStage;

		public static bool CompletedGroupe6Tasks;

		public static int Groupe6TaskStage;

		public static int RunHeist = 0;

		public static Vector3 GetawayCarSpawn = new Vector3(2701.902f, -361.279f, -55.8f);

		public static float GetawayCarHeading = 90f;

		public float TotalCrewEarned;

		public float TotalPlayerEarned;

		public bool EnableRobberyMarkers;

		public Vector3 StartHeistMarker = new Vector3(2712.729f, -369.3823f, -55f);

		public Vector3 RobberA_StartMarker = new Vector3(2700.421f, -363.3955f, -55.5f);

		public Vector3 RobberB_StartMarker = new Vector3(2702.088f, -363.4372f, -55.5f);

		public Vector3 RobberC_StartMarker = new Vector3(2703.941f, -363.576f, -55.5f);

		public float _GoldPickupPay;

		public float _DiamondsPickupPay;

		public float _CashPickupPay;

		public float _PaintingsPickupPay;

		public float _MiscPickupPay;

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

		public static string[] StringSplit;

		public static string pay;

		public static int HeistMinPay;

		public static int HeistMaxPay;

		public bool SpawnSeatedAI;

		public float GrabbingSpeed;

		public bool WaitingforClick;

		public static bool ClicktoContinueGrabbing;

		public bool RequestEndCutscene;

		public bool AIisHoldingPostion;

		public static Keys ResetAiPosition_ForDebug = Keys.MediaStop;

		public static Keys GetinVehicleKey = Keys.Decimal;

		public static Keys HoldPositionKey = Keys.OemBackslash;

		public float HTime;

		public Ped CutsenePed1;

		public Ped CutsenePed2;

		public Ped CutsenePed3;

		public Ped CutsenePed4;

		public bool PlayingCutscene;

		public List<Prop> Props = new List<Prop>();

		public float screenaspectratio;

		public static NativeMenu Heists;

		private ObjectPool modLemonUIObjectPool2;

		private NativeMenu mainMenu2;

		private NativeMenu Heists2;

		public static bool CameraON;

		public static Camera HeistStartCam;

		public static PedHash RobberASkin;

		public static PedHash RobberBSkin;

		public static PedHash RobberCSkin;

		public static bool MissionOn;

		public static Blip Bank;

		public Vector3 AppartmentPos = new Vector3(718f, -981f, 22f);

		public Vector3 FleecaBankMarker = new Vector3(-13.7145f, -687.805f, 32f);

		public Vector3 FleecaJammerSpawm = new Vector3(455f, -987f, 25f);

		public Vector3 FleecaJammerPos = new Vector3(-3.63722f, -687.673f, 15.1f);

		public Vector3 FleecaInsurgentSpawn = new Vector3(-1731.9f, 2838f, 33f);

		public Blip InsurgentBLip;

		public Vehicle Insurgent;

		public Vector3 FleecaKurumaSpawn = new Vector3(-13.62394f, -1067.724f, 38f);

		public Blip KurumaBlip;

		public Vehicle Kuruma;

		public Vector3 FleecaVaultDoor;

		public Vehicle Riot1;

		public Vehicle Riot2;

		public Vehicle Riot3;

		public Vehicle Riot4;

		public int VaultDoorTimer;

		public bool triggerPatrol;

		public Vehicle PatrolCar;

		public Vehicle PatrolCar1;

		public Vehicle PatrolCar2;

		public Ped TargetPed;

		public Vector3 PatrolCarDeletePos = new Vector3(515.366f, 83.88f, 95f);

		public bool PatrolCarEnitiated;

		public bool headingbacktoyourAppartment;

		public bool SecutingJammer;

		public static bool returningtofactroy;

		public bool SecuringVehicle;

		public bool GotVehicle;

		public bool placedJammer;

		public string VehicleChoosen;

		public bool WaitingForVaultdoortoOpen;

		public int Waittime;

		public bool VaultOpen;

		public bool DefendingCops;

		public int timer;

		public bool escapingcops;

		public bool HeadingtoyourAppartment;

		public static bool TriggerUINotify;

		public static Ped RobberA;

		public static int RobberAskill;

		public static Ped RobberB;

		public static int RobberBskill;

		public static Ped RobberC;

		public static int RobberCskill;

		public static int Hackerskill;

		public static float TotalCost;

		public static float HackerCost = 3500000f;

		public static float TeamAI1Cost = 0f;

		public static float TeamAI2Cost = 0f;

		public static float TeamAI3Cost = 0f;

		public static float TeamAI4Cost = 0f;

		public Vector3 DoorApos = new Vector3(233.12f, 215.95f, 105f);

		public Vector3 DoorBPos = new Vector3(259.755f, 204.6733f, 105f);

		public Vector3 DoorCPos = new Vector3(257.027f, 220.4003f, 105f);

		public Vector3 DoorDPos = new Vector3(261.93f, 221.79f, 105f);

		public bool DoorALocked;

		public bool DoorBLocked;

		public bool FoundKeyCard;

		public bool DoorCLocked;

		public bool DoorDLocked;

		public bool GotKeyCard;

		public Ped Manager;

		public bool LockedDoors;

		public bool CivsAreFreakingOut;

		public int TargetingTime;

		public Blip DoorAblip;

		public Blip DoorBblip;

		public Blip DoorCblip;

		public Blip DoorDblip;

		public bool CreatedBlips;

		public Vector3 DoorLoc;

		public Vector3 VaultLoc;

		public List<Ped> Tellers = new List<Ped>();

		public List<Ped> Civs = new List<Ped>();

		public List<Ped> Guards = new List<Ped>();

		public bool InHeist;

		public Prop VaultDoor;

		public static int MaxPay;

		public int CurrentPay;

		public static List<Ped> Peds = new List<Ped>();

		public static List<Vehicle> Vehicles = new List<Vehicle>();

		public bool AtPosA;

		public bool AtPosB;

		public bool AtPosC;

		public SaveCar SaveCar = new SaveCar();

		public bool CreateEnemy;

		public int JuggerNautTimer;

		public int SwatTimer;

		public int BreachTimer;

		public float HeistTime;

		public int HeistTimer;

		public Prop Vent;

		public Vector3 Ent_FrontUp = new Vector3(-7.56f, -742.096f, 44f);

		public Vector3 Ent_FrontDown = new Vector3(0.9095f, -741.01f, 31f);

		public Vector3 Ent_Side = new Vector3(-88.82f, -661.114f, 35f);

		public Vector3 Ent_Back = new Vector3(53.8223f, -642.084f, 31f);

		public int FirstStage;

		public Prop P;

		public Prop P2;

		public int ENT_Used;

		public bool GotPlans;

		public static bool GotC4_1;

		public static bool GotC4_2;

		public static Prop Crate_1;

		public static Prop Crate_2;

		public static List<Vehicle> Veh = new List<Vehicle>();

		public Vehicle ExBankPedVehicle;

		public static bool LookingForVehicle;

		public static Blip DeliveryPoint;

		public static Vector3 DeliveryLoc = new Vector3(692.4743f, -1004.749f, 21f);

		public Vehicle GetawayCar;

		public bool C4_1Detonated;

		public bool C4_2Detonated;

		public Vector3 Gen1 = new Vector3(64.6155f, -770.726f, 17f);

		public Vector3 Gen2 = new Vector3(50.6583f, -776.297f, 17f);

		public Vector3 CameraPos1 = new Vector3(72.588f, -781.2409f, 21f);

		public float CameraRot1 = 39f;

		public Vector3 CameraPos2 = new Vector3(1052.889f, -287.9034f, 55f);

		public float CameraRot2 = 64f;

		public Vector3 ExplosivePos2 = new Vector3(1029.871f, -264.4223f, 50f);

		public Vehicle PlayerVehicle;

		public Prop C4_1;

		public Prop C4_2;

		public Blip C4_1Blip;

		public Blip C4_2Blip;

		public Camera Cam;

		public bool Detonated;

		public Prop VaultD;

		public bool TriggerVault;

		public bool VaultUnlocked;

		public bool DeletedElv2;

		public bool DeletedElv1;

		public Prop CasinoVaultDoor;

		private ObjectPool modLemonUIObjectPool;

		public static NativeMenu mainMenu;

		public static int HeistMissson = 13;

		public static int HeistStage;

		public bool HasntFailed;

		private static ScriptSettings Config;

		public bool GotPL;

		public bool TriggerInfo;

		public bool MissionDone;

		public bool sitting;

		public static List<Vehicle> AircraftCarrierVehicles2 = new List<Vehicle>();

		public static List<Vehicle> AircraftCarrierVehicles = new List<Vehicle>();

		public static List<Ped> AircarftCarrierPeds = new List<Ped>();

		public static List<Ped> AircarftCarrierPeds2 = new List<Ped>();

		public static List<Vehicle> ReinforcementsVehicles = new List<Vehicle>();

		public static List<Ped> ReinforcementsPeds = new List<Ped>();

		public List<Vehicle> AttackAircraft = new List<Vehicle>();

		public List<Ped> AttackPed = new List<Ped>();

		public List<Ped> PlaneTargetPositions = new List<Ped>();

		public List<Vehicle> SwatVehicles = new List<Vehicle>();

		public List<Ped> SwatPeds = new List<Ped>();

		public bool AirConvoyOn;

		public bool ForcedCargotoland;

		public bool ForcedCargotoland2;

		public Vehicle CargoPlane;

		public Vehicle AvengerA;

		public Vehicle AvengerB;

		public Vehicle AvengerC;

		public Vehicle AvengerD;

		public Vehicle AvengerE;

		public Vehicle AvengerF;

		public Vehicle AvengerG;

		public Vehicle AvengerH;

		public Vehicle AvengerI;

		public Vehicle Hydra;

		public bool GotHydra;

		public bool DestroyedHydras;

		public bool TriggeredAttack;

		public Blip HangerB;

		public bool GotGetawayVeh;

		public Vehicle ChoosenVeh;

		public List<Vector3> RobPositions = new List<Vector3>();

		public bool TriggerNotifyNear;

		public bool TriggerNotifyVehicle;

		public bool TrackingBlip;

		public bool GotMaxPay;

		public bool TriggeredWantedLevelEscape;

		public bool TriggeredWantedLevelReinforcement;

		public bool FreezeGetawayCar;

		public Vector3 LastPos;

		public List<Ped> Peds2 = new List<Ped>();

		public List<Vector3> Spawns = new List<Vector3>();

		public Vehicle Prizecar;

		public Ped ped;

		public List<Vector3> PokerTables = new List<Vector3>();

		public List<float> PokerTablesRot = new List<float>();

		public List<Vector3> BlackJackTables = new List<Vector3>();

		public List<float> BlackJackRot = new List<float>();

		public List<Vector3> SlotMachines = new List<Vector3>();

		public List<float> SlotMachinesRot = new List<float>();

		public List<Vector3> RouletteTables = new List<Vector3>();

		public List<float> RouletteTablesRot = new List<float>();

		public List<Prop> Tables = new List<Prop>();

		public List<Prop> Tables2 = new List<Prop>();

		public Prop WheelProp;

		public Prop WheelProp2;

		public Prop BasePlate;

		public Prop BasePlate2;

		public Vector3 ThraxPos = new Vector3(1099.792f, 219.5638f, -49.1f);

		public Prop WheelWinMarker;

		public List<Prop> WheelMarkers = new List<Prop>();

		public Vector3 LuckyWheel = new Vector3(1111.045f, 228.6406f, -50.5f);

		public Vector3 MarkerExit = new Vector3(1089.677f, 205.8681f, -50f);

		public Vector3 MarkerEnter = new Vector3(935.9782f, 47.018f, 80.2f);

		public Ped Vendor1;

		public Ped Vendor2;

		public List<Ped> MainPed = new List<Ped>();

		public List<Ped> SMPed = new List<Ped>();

		public bool UsingCasino;

		public static int EnteranceUsed;

		public static int ExitUsed;

		public static bool HackedWasteDisposalDoor;

		public bool VaultClosed;

		public bool VaultDone;

		public bool EscapingCasino;

		public static int LocAt;

		public static Ped Lester;

		public static Vehicle StockadeA;

		public static Vehicle StockadeB;

		public static Vehicle StockadeC;

		public static Vector3 LesterSpawn = new Vector3(2681.348f, -360.3722f, -55.1f);

		public static bool SecuredAsset_C4;

		public static bool SecuredAsset_Stockade;

		public static bool SecuredAsset_Vip;

		public int Hacktime;

		public int HackProg;

		public static bool isDriving;

		public int DriveStage;

		public Vehicle FollowCar;

		public Ped FollowPed;

		public static Ped MeetPed;

		public bool EnteredNormalCasino;

		public bool EnteredOffice;

		public bool MetalDetectorOn;

		public static bool IsInterior;

		public bool BypassMetalDetector;

		public Prop BombSewer;

		public Prop BombVault_A;

		public Prop BombVault_B;

		public Prop BombVault_C;

		public Prop BombVault_D;

		public bool PlacedBomb;

		public Vehicle SecondaryGetawayVeh;

		public bool FreezeGetawayCar2;

		public bool IsPlanting;

		public bool AlarmsGoingOff;

		public int AlarmTimer;

		public bool GotCash;

		public bool NotifyOutVault;

		public bool NotifyOutVault2;

		public static List<Vector3> Crate_loc = new List<Vector3>();

		public static List<Prop> PaintingHolder = new List<Prop>();

		public static List<float> Crate_Rot = new List<float>();

		public int Scene1;

		public Prop bagProp;

		public Prop _cashGrabTray1;

		public Prop MoneyProp;

		public bool PlayingAnim;

		public bool CreatedCrates;

		public int CurrentType;

		public float CashCount;

		public int RobTimer;

		public bool InNormalCasino;

		public bool OverideAlarm;

		public int ReportTimer;

		public static bool TriggerFullResponce;

		public bool ReportedtoGuardA;

		public bool ReportedtoGuardB;

		public ManageRobberyAnims.Lootable ExtraValuable;

		public bool RobbedExtraValuable;

		public bool ExtraVaultOpen;

		public int ExtraValuableType;

		public ManageRobberyAnims.Lootable ExtraValuable2;

		public bool RobbedExtraValuable2;

		public bool ExtraVaultOpen2 = true;

		public int ExtraValuableType2;

		public static bool DebugTimeOn;

		public static bool ShowTimeronShiftX;

		public int MetalDetectorTimer;

		public bool hackedDataPad;

		public bool NotifyHack;

		public Prop HackConsole;

		public static bool AIinCasinoHeistCanWander;

		public static int AIinCasinoWanderChance;

		public static int AIinCasinoWanderRadius;

		public List<Vector3> AI_Pos = new List<Vector3>();

		public List<Vector3> AI_Rot = new List<Vector3>();

		public List<int> AI_Type = new List<int>();

		public bool Active;

		public int Amt;

		public int iLocal_8077;

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

		public List<Prop> SmokeEmitters = new List<Prop>();

		public bool PlantedBZGas;

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

		public FiringPattern RobberAFP = FiringPattern.BurstFire;

		public FiringPattern RobberBFP = FiringPattern.BurstFire;

		public FiringPattern RobberCFP = FiringPattern.BurstFire;

		public int RobberAtimer;

		public int RobberBtimer;

		public int RobberCtimer;

		public string OutfitReq = "No Requirement";

		public int RobberA_C;

		public int RobberB_C;

		public int RobberC_C;

		public string ID_C;

		public float BaseCrewCost = 765000f;

		public bool SetupAi;

		public bool TriggeredReinforcments;

		public bool SpawnedReinforcements;

		public bool SetupReinforcments;

		public bool SetSewerBomb;

		public bool DetonatedSewerBomb;

		public static Vector3 PointA = new Vector3(108.4f, -1355.4f, 30f);

		public static Vector3 PointB = new Vector3(791.5f, -1388.8f, 27f);

		public static Vector3 PointC = new Vector3(504.6f, -333.5f, 44f);

		public List<CasinoPed> CasinoAI = new List<CasinoPed>();

		public int Difficulty;

		public int PriorityLoot = 1;

		public static List<string> PaintingIDs = new List<string> { "ch_prop_vault_painting_01j", "ch_prop_vault_painting_01i", "ch_prop_vault_painting_01h", "ch_prop_vault_painting_01g", "ch_prop_vault_painting_01f", "ch_prop_vault_painting_01e", "ch_prop_vault_painting_01d", "ch_prop_vault_painting_01c", "ch_prop_vault_painting_01b", "ch_prop_vault_painting_01a" };

		public int CuttingStage;

		public int VaultType = 1;

		public List<int> CurrentSDBType = new List<int>();

		public List<int> CurrentSDB = new List<int>();

		public List<string> CurrentSDBDict = new List<string>();

		public bool SetupPaintingsOrLockVault;

		public float nextTick;

		public Prop CurrentPaintingHolder;

		public int CurrentPaitingID;

		public Prop CurrentPainting;

		public bool CanNoLongerOverideCommands;

		public int LightNode;

		public int LocalTake;

		public Vector3 MetalDetector1 = new Vector3(2545.732f, -276.79f, -57.82f);

		public Vector3 MetalDetector2 = new Vector3(2525.57f, -267.8022f, -57.79f);

		public Vector3 MetalDetector3 = new Vector3(2542.522f, -265.155f, -57.68f);

		public static int OverrideStart;

		public bool SetRob;

		public bool InitialAnimDone;

		public bool PlayingCashGrabIdle;

		public float CashGrabLastSavePlace;

		public int IdleScene;

		public Prop PlayerLastLootProp;

		public bool PlayerGotJewels;

		public List<SecCabinetProps> SecurityCabinetProps = new List<SecCabinetProps>();

		public bool PlayerPlayingFinalAnim;

		public bool PlayerStartedCuttingAnim;

		public bool PlayerFirstAnim;

		public static bool RobberAFirstAnim;

		public bool RobberBFirstAnim;

		public bool RobberCFirstAnim;

		public Prop PlayerRobbingProp;

		public static Prop RobberARobbingProp;

		public Prop RobberBRobbingProp;

		public Prop RobberCRobbingProp;

		public bool PlayerWaitingForLastAnim;

		public static bool RobberAWaitingForLastAnim;

		public bool RobberBWaitingForLastAnim;

		public bool RobberCWaitingForLastAnim;

		public bool PlayerFinishedLastAnim;

		public static bool RobberAFinishedLastAnim;

		public bool RobberBFinishedLastAnim;

		public bool RobberCFinishedLastAnim;

		public static int RobberARobTimer;

		public int RobberBRobTimer;

		public int RobberCRobTimer;

		public int CuttingCabinet;

		public int CuttingScene1;

		public Vector3 CuttingVector3;

		public float Cuttingheading;

		public Prop CuttingGlassPlate;

		public int CabinetPropsSelectedIndex;

		public int CuttingTime;

		public int CuttingPTFX;

		public float PowerOutput;

		public int RequiredPowerOutput;

		public int TimerPowerOutput;

		public bool PlayerFinishedCutting;

		public static bool RobberAFinishedCutting;

		public bool RobberBFinishedCutting;

		public bool RobberCFinishedCutting;

		public int CuttingSound;

		public bool PlayingPTFX;

		public static bool RobberANearLoot;

		public Vector3 V1 = new Vector3(2500.4f, -238.5f, -70.7f);

		public bool DrillComplete;

		public bool VaultOpenedByDrill;

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

		public bool AllowMix;

		public bool firstbounce;

		public int newPos;

		public List<bool> BackupPosSpawned = new List<bool>();

		public List<Vector3> BackupPos = new List<Vector3>
		{
			new Vector3(2477.812f, -276.1245f, -70.6942f),
			new Vector3(2477.106f, -282.1636f, -70.6942f),
			new Vector3(2482.556f, -282.6871f, -70.6942f),
			new Vector3(2490.97f, -283.2016f, -70.6942f),
			new Vector3(2487.452f, -286.9735f, -70.6942f),
			new Vector3(2486.826f, -289.4441f, -70.6942f),
			new Vector3(2491.098f, -276.5151f, -70.6942f),
			new Vector3(2491.203f, -270.2704f, -70.6942f),
			new Vector3(2491.109f, -264.1815f, -70.6942f),
			new Vector3(2486.512f, -263.038f, -70.6942f),
			new Vector3(2487.134f, -268.3417f, -70.69427f),
			new Vector3(2481.719f, -268.9995f, -70.69427f),
			new Vector3(2479.42f, -266.1651f, -70.66609f),
			new Vector3(2486.96f, -253.5711f, -70.69421f),
			new Vector3(2492.331f, -252.8109f, -70.69421f),
			new Vector3(2492.159f, -258.8415f, -70.69421f),
			new Vector3(2481.333f, -274.0419f, -70.69422f),
			new Vector3(2483.68f, -275.0776f, -70.69422f),
			new Vector3(2486.596f, -273.9734f, -70.69422f),
			new Vector3(2472.303f, -263.576f, -70.69416f),
			new Vector3(2471.057f, -266.94f, -70.69416f),
			new Vector3(2468.32f, -269.119f, -70.69389f),
			new Vector3(2472.311f, -270.101f, -70.69416f),
			new Vector3(2504.135f, -272.9939f, -70.69875f),
			new Vector3(2502.262f, -278.9861f, -70.70031f),
			new Vector3(2503.994f, -285.5575f, -70.69814f),
			new Vector3(2521.884f, -281.9326f, -70.71973f),
			new Vector3(2521.927f, -277.4792f, -70.71924f),
			new Vector3(2536.459f, -278.9669f, -70.69413f),
			new Vector3(2514.831f, -278.2312f, -70.7229f),
			new Vector3(2507.935f, -278.5391f, -67.72305f),
			new Vector3(2519.598f, -283.2425f, -64.72285f),
			new Vector3(2528.32f, -283.279f, -64.72285f),
			new Vector3(2537.08f, -283.3883f, -64.72285f),
			new Vector3(2538.747f, -275.008f, -64.72285f),
			new Vector3(2530.105f, -274.8861f, -64.72285f),
			new Vector3(2520.571f, -274.9277f, -64.72285f),
			new Vector3(2506.228f, -291.7865f, -58.72299f),
			new Vector3(2515.225f, -291.4375f, -58.72299f),
			new Vector3(2526.057f, -291.4065f, -58.72299f),
			new Vector3(2532.216f, -291.3967f, -58.72299f),
			new Vector3(2541.441f, -283.3645f, -58.72299f),
			new Vector3(2541.278f, -277.9255f, -58.72299f),
			new Vector3(2541.333f, -271.0313f, -58.72299f),
			new Vector3(2539.736f, -262.8907f, -58.72299f),
			new Vector3(2535.522f, -268.6498f, -58.72299f),
			new Vector3(2528.785f, -268.9984f, -58.72299f),
			new Vector3(2516.166f, -269.121f, -58.72299f),
			new Vector3(2504.775f, -269.9748f, -58.72299f),
			new Vector3(2504.647f, -273.1197f, -58.72299f),
			new Vector3(2497.481f, -281.0116f, -58.70791f),
			new Vector3(2490.184f, -279.7581f, -58.7079f),
			new Vector3(2490.381f, -271.3556f, -59.12818f),
			new Vector3(2498.216f, -266.0304f, -59.10852f),
			new Vector3(2500.849f, -255.3115f, -59.69021f),
			new Vector3(2515.047f, -256.2528f, -59.70851f),
			new Vector3(2514.934f, -242.6098f, -60.32192f),
			new Vector3(2517.867f, -230.8718f, -60.71269f),
			new Vector3(2529.938f, -238.624f, -60.70994f),
			new Vector3(2526.584f, -262.6163f, -58.72836f),
			new Vector3(2526.836f, -281.77f, -58.72296f),
			new Vector3(2532.105f, -281.6215f, -58.72296f),
			new Vector3(2504.251f, -286.3752f, -58.72301f),
			new Vector3(2503.491f, -279.029f, -58.7231f),
			new Vector3(2497.802f, -273.659f, -58.7079f),
			new Vector3(2508.801f, -262.2183f, -58.71273f),
			new Vector3(2517.617f, -258.2362f, -55.12315f),
			new Vector3(2513.972f, -258.13f, -55.12315f),
			new Vector3(2508.846f, -252.9727f, -55.12315f),
			new Vector3(2509.08f, -241.2641f, -55.12315f),
			new Vector3(2498.047f, -238.1323f, -55.12384f),
			new Vector3(2490.102f, -247.4868f, -55.12384f),
			new Vector3(2499.01f, -229.6317f, -60.32352f),
			new Vector3(2509.396f, -229.1905f, -60.31847f),
			new Vector3(2527.416f, -227.368f, -61.1085f),
			new Vector3(2512.244f, -239.6007f, -60.3186f),
			new Vector3(2498.92f, -229.9899f, -55.12383f),
			new Vector3(2488.921f, -257.522f, -39.12291f),
			new Vector3(2488.212f, -289.1223f, -39.12292f),
			new Vector3(2522.907f, -289.443f, -39.12292f),
			new Vector3(2512.312f, -257.2605f, -39.12292f),
			new Vector3(2504.504f, -257.6024f, -39.12316f),
			new Vector3(2523.541f, -234.2265f, -39.12291f),
			new Vector3(2504.565f, -233.244f, -39.1229f)
		};

		public static bool DisplayOrdersMenu;

		public HKHModHelperNew.MissionScreen Menus;

		public int MenuSelectedIndexY;

		public int MenuSelectedIndexX;

		public bool IsDisplayingOrderMenu;

		public bool TriggerBusCutscene;

		public bool PedATryingtoEnterVehicle;

		public bool PedBTryingtoEnterVehicle;

		public bool PedCTryingtoEnterVehicle;

		public bool PedDTryingtoEnterVehicle;

		public bool PedAGuardCurrentPos;

		public bool PedBGuardCurrentPos;

		public bool PedCGuardCurrentPos;

		public bool TriggerLockCutscene;

		public string PlayerCurrentSDDict;

		public int PlayerCurrentLockbox;

		public int PlayerCurrentLockboxStage;

		public Prop PlayerCurrentLockBoxProp;

		public string PlayerCurrentLockBoxType;

		public Prop PlayerCurrentLootBagProp;

		public float PlayerCurrentLootBoxRotation;

		public float PlayerGrabLootDelayStop;

		public string RobberACurrentSDDict;

		public int RobberACurrentLockbox;

		public int RobberACurrentLockboxStage;

		public Prop RobberACurrentLockBoxProp;

		public string RobberACurrentLockBoxType;

		public Prop RobberACurrentLootBagProp;

		public float RobberACurrentLootBoxRotation;

		public string RobberBCurrentSDDict;

		public int RobberBCurrentLockbox;

		public int RobberBCurrentLockboxStage;

		public Prop RobberBCurrentLockBoxProp;

		public string RobberBCurrentLockBoxType;

		public Prop RobberBCurrentLootBagProp;

		public float RobberBCurrentLootBoxRotation;

		public string RobberCCurrentSDDict;

		public int RobberCCurrentLockbox;

		public int RobberCCurrentLockboxStage;

		public Prop RobberCCurrentLockBoxProp;

		public string RobberCCurrentLockBoxType;

		public Prop RobberCCurrentLootBagProp;

		public float RobberCCurrentLootBoxRotation;

		public static bool RobberAInitialAnimDone = false;

		public bool RobberBInitialAnimDone;

		public bool RobberCInitialAnimDone;

		public static bool RobberASetRob = false;

		public bool RobberBSetRob;

		public bool RobberCSetRob;

		public static Ped RobberATarget;

		public Ped RobberBTarget;

		public Ped RobberCTarget;

		public static Prop RobberA_bagProp;

		public Prop RobberB_bagProp;

		public Prop RobberC_bagProp;

		public static Prop RobberA_HoldingLootProp;

		public Prop RobberB_HoldingLootProp;

		public Prop RobberC_HoldingLootProp;

		public static int RobberA_CashGrabTimer;

		public int RobberB_CashGrabTimer;

		public int RobberC_CashGrabTimer;

		public static Vector3 RobberA_RobberMarker = new Vector3(0f, 0f, 0f);

		public Vector3 RobberB_RobberMarker = new Vector3(0f, 0f, 0f);

		public Vector3 RobberC_RobberMarker = new Vector3(0f, 0f, 0f);

		public static int RobberASyncScene;

		public static int RobberBSyncScene;

		public static int RobberCSyncScene;

		public static bool RobberARobbing;

		public static bool RobberBRobbing;

		public static bool RobberCRobbing;

		public static int RobberACurrentType;

		public static int RobberBCurrentType;

		public static int RobberCCurrentType;

		public static int RobberACurrentValualabe;

		public static int RobberBCurrentValualabe;

		public static int RobberCCurrentValualabe;

		public static int RobberACuttingStage;

		public static int RobberBCuttingStage;

		public static int RobberCCuttingStage;

		public static bool OverideTasks;

		public static bool RobberAMovingToLoot;

		public static bool RobberBMovingToLoot;

		public static bool RobberCMovingToLoot;

		public static int RobberAMoveTimer;

		public static int RobberBMoveTimer;

		public static int RobberCMoveTimer;

		public static Prop RobberACurrentPaiting;

		public static Prop RobberBCurrentPaiting;

		public static Prop RobberCCurrentPaiting;

		public static Prop PlayerCurrentPaiting;

		public static Prop RobberACurrentPaitingHolder;

		public Prop RobberBCurrentPaitingHolder;

		public Prop RobberCCurrentPaitingHolder;

		public Prop PlayerCurrentPaitingHolder;

		public static int RobberACurrentID;

		public int RobberBCurrentID;

		public int RobberCCurrentID;

		public int PlayerCurrentID;

		public bool Surrendered;

		public PedTaskLoop L = new PedTaskLoop();

		public List<Ped> WanderingGuards = new List<Ped>();

		public List<PedTaskLoop> WanderingGuardsPoints = new List<PedTaskLoop>();

		public bool Firsttime;

		public bool ChangedGuardsHearing;

		public static string BypassEventText = "null";

		public static Vector3 BypassEventLoc = new Vector3(0f, 0f, 0f);

		public static Blip purchaseBlip2;

		public static Blip purchaseBlip;

		public bool CreatedMarker;

		public static Color MarkerColor;

		public List<KeypadClass> KeyPads = new List<KeypadClass>
		{
			new KeypadClass(new Vector3(994.16f, 75.1f, 81.3f), 147f, 1),
			new KeypadClass(new Vector3(2516.78f, -259.7819f, -38.9f), -178.68f, 2),
			new KeypadClass(new Vector3(2506.57f, -271.38f, -58.5f), -90f, 2),
			new KeypadClass(new Vector3(2519.638f, -266.036f, -58.5f), 267.51f, 2),
			new KeypadClass(new Vector3(2520.592f, -277.48f, -58.48f), 90.64f, 2),
			new KeypadClass(new Vector3(2519.19f, -281.8895f, -64.6f), 359.38f, 2),
			new KeypadClass(new Vector3(2485.912f, -255.3f, -70.5f), 87.85f, 2)
		};

		public Prop USB;

		public Prop Phone;

		public static bool HackingKeypad = false;

		public static bool UnlockedThreeFloorBypass = false;

		public static bool UnlockedVaultToOffice = false;

		public static bool UnlockedStaffDoor = false;

		public static bool UnlockedVaultSecDoorA;

		public static bool UnlockedVaultSecDoorB;

		public static bool UnlockedVaultSecDoorC;

		public static bool UnlockedVaultSecDoorD;

		public Prop LastKeypad;

		public Vector3 FloorVaultElevator = new Vector3(2484.844f, -253.88f, -71.68f);

		public Vector3 FloorVaultOfficeElevator = new Vector3(2517.577f, -263.65f, -59.711f);

		public Vector3 FloorOfficeElevator = new Vector3(2518.613f, -279.58f, -59.72f);

		public Vector3 FloorLoadingBayElevator = new Vector3(2518.613f, -279.58f, -65.72f);

		public Vector3 FloorHotelElevator = new Vector3(2518.613f, -261.733f, -40.12f);

		public Vector3 StaffDoor = new Vector3(2506.57f, -271.38f, -58.5f);

		public Vector3 SideDoorEnterance = new Vector3(977.33f, 16.56f, 79.99f);

		public Vector3 SideDoorExit = new Vector3(2549.289f, -269.67f, -58.723f);

		public Prop MDBox1;

		public Prop MDBox2;

		public Prop MDBox3;

		public static bool ShutdownMetalDetector1;

		public static bool ShutdownMetalDetector2;

		public static bool ShutdownMetalDetector3;

		public bool CreatedSD;

		public List<Vector3> Crate_locFake = new List<Vector3>();

		public List<float> Crate_RotFake = new List<float>();

		public static Vector3 VaultPos = new Vector3(2521.121f, -238.44f, -70.5f);

		public static int PercentNeedTake = (int)((float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 45f);

		public bool UnlockedVaultEnterance;

		public Vector3 StaffDoubleDoor = new Vector3(2506.3f, -269.7f, -60f);

		public Vector3 StaffDoor2 = new Vector3(2503.609f, -276.52f, -58.72f);

		public Vector3 VaultTunnelEnteranceDoor = new Vector3(2464.635f, -279.01f, -70.5f);

		public static bool DisableModPart_DiamondCasino;

		public static FloorOn CurrentFloor;

		public static bool CreatedLoot;

		public void GetTimeInVault()
		{
			if (Hackerskill == 0)
			{
				Waittime = 305;
			}
			if (Hackerskill == 1)
			{
				Waittime = 440;
			}
			if (Hackerskill == 2)
			{
				Waittime = 570;
			}
			if (Hackerskill == 3)
			{
				Waittime = 600;
			}
			if (Hackerskill == 4)
			{
				Waittime = 650;
			}
			if (Hackerskill == 5)
			{
				Waittime = 715;
			}
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			float num = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			num = (float)Math.Sqrt(num);
			return num;
		}

		public Vector3 CheckAppartmentPos()
		{
			Main.GetArcadeLoc();
			return Main.GetArcadeLoc();
		}

		public Model SpawnProp(string Prop)
		{
			Model result = new Model(Prop);
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
			model.Request(10000);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(50);
				}
				Prop item = GlobalHelper.CreateProp(model, Pos, Rot, true, false);
				Props.Add(item);
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

		public void SpawnPropBombSewer(string Prop, Vector3 Pos, Vector3 Rot)
		{
			Model model = new Model(Prop);
			model.Request(10000);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(50);
				}
				Prop prop = GlobalHelper.CreateProp(model, Pos, Rot, true, false);
				Props.Add(prop);
				BombSewer = prop;
				Prop[] nearbyProps = World.GetNearbyProps(Pos, 60f, model);
				Prop[] array = nearbyProps;
				foreach (Prop prop2 in array)
				{
					prop2.SetNoCollision(prop2, false);
					prop2.LodDistance = 3000;
					prop2.IsInvincible = true;
					prop2.IsPositionFrozen = true;
					Props.Add(prop2);
				}
				Script.Wait(1);
			}
			model.MarkAsNoLongerNeeded();
		}

		public void SpawnPropBombVault(string Prop, Vector3 Pos, Vector3 Rot)
		{
			Model model = new Model(Prop);
			model.Request(250);
			if (model.IsInCdImage && model.IsValid)
			{
				while (!model.IsLoaded)
				{
					Script.Wait(50);
				}
				Prop prop = GlobalHelper.CreateProp(model, Pos, Rot, true, false);
				Props.Add(prop);
				if (BombVault_A == null)
				{
					BombVault_A = prop;
					return;
				}
				if (BombVault_B == null)
				{
					BombVault_B = prop;
					return;
				}
				if (BombVault_C == null)
				{
					BombVault_C = prop;
					return;
				}
				if (BombVault_D == null)
				{
					BombVault_D = prop;
					return;
				}
				Prop[] nearbyProps = World.GetNearbyProps(Pos, 60f, model);
				Prop[] array = nearbyProps;
				foreach (Prop prop2 in array)
				{
					prop2.SetNoCollision(prop2, false);
					prop2.LodDistance = 3000;
					prop2.IsInvincible = true;
					prop2.IsPositionFrozen = true;
					Props.Add(prop2);
				}
				Script.Wait(1);
			}
			model.MarkAsNoLongerNeeded();
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

		public void ManageDoor(Vector3 Pos, string Model, bool Lock)
		{
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, Pos.X, Pos.Y, Pos.Z, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, Model), 0, 0, 0);
			if (prop != null)
			{
				prop.IsPositionFrozen = Lock;
			}
		}

		public TheDiamondHeist()
		{
			LoadIniFile("scripts//HKH_SinglePlayerHeists//Main.ini");
			base.Tick += OnTick;
			base.KeyDown += OnKeyDown;
			base.Aborted += OnShutdown;
			Setup();
		}

		public void Setup()
		{
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu("Diamond Casino Heist", "Select an Option");
			Heists = mainMenu;
			modLemonUIObjectPool.Add(mainMenu);
			modLemonUIObjectPool2 = new ObjectPool();
			mainMenu2 = new NativeMenu("Diamond Casino Heist", "Select an Option");
			modLemonUIObjectPool2.Add(mainMenu2);
			Heists2 = new NativeMenu("", "Choose Enterance/Exit");
			modLemonUIObjectPool2.Add(Heists2);
			mainMenu2.AddSubMenu(Heists2);
			SetupHeists(true);
			GetLocation();
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

		private void DisplayHelpTextThisFrame(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text + " 1" };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
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

		public static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
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

		public void SetFollow(Vehicle Plane, Ped P)
		{
			Vector3 position = P.Position;
			Function.Call(Hash.TASK_PLANE_MISSION, Plane.GetPedOnSeat(VehicleSeat.Driver).Handle, Plane.Handle, P, position.X, position.Y, position.Z, 7, 0.0, -1.0, 0.0, 1000, 150);
		}

		public void SetActiveTarget2(Vehicle Plane, Ped P)
		{
			Vector3 position = P.Position;
			Function.Call(Hash.TASK_PLANE_MISSION, Plane.GetPedOnSeat(VehicleSeat.Driver).Handle, Plane.Handle, P, position.X, position.Y, position.Z, 6, 0.0, -1.0, 0.0, 1000, 150);
		}

		public void SetActiveTarget(Vehicle Plane, Ped P)
		{
			Vector3 position = P.Position;
			Function.Call(Hash.TASK_PLANE_MISSION, Plane.GetPedOnSeat(VehicleSeat.Driver).Handle, Plane.Handle, P, position.X, position.Y, position.Z, 6, 0.0, -1.0, 0, 1000, 150);
		}

		public void FightAgainst(Ped Target, Ped Enemy)
		{
			Enemy.Task.ClearAll();
			Enemy.Task.FightAgainst(Target);
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
			scaledTexture.Color = Color.Black;
			scaledTexture.Draw();
		}

		public void DisplayReq()
		{
			string text = "bronzemedal";
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(4, 10), new Size(200, 200), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT(" !   Outfit Requirement   !", new Point(1700, 30), 0.76f, Color.FromArgb(255, 255, 255, 255), GTA.UI.Font.Pricedown);
			TEXT(OutfitReq, new Point(1700, 90), 0.76f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.Render2D();
		}

		public Model RequestPed(PedHash Name)
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

		public void PlayAnimation(Ped ped)
		{
			Random random = new Random();
			int num = random.Next(1, 44);
			ped.Task.ClearAll();
			ped.Task.ClearAllImmediately();
			if (num == 1)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationfemale@banging_tunes_right", "banging_tunes", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 1)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@air_synth", "air_synth", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 2)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@blow_kiss", "blow_kiss", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 3)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@bro_love", "bro_love", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 4)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@cats_cradle", "cats_cradle", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 5)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@chicken_taunt", "chicken_taunt", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 6)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@chin_brush", "chin_brush", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 7)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@cry_baby", "cry_baby", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 8)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@dj", "dj", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 9)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@dock", "dock", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 10)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@face_palm", "face_palm", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 11)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@find_the_fish", "find_the_fish", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 12)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@finger", "finger", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 13)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@finger_kiss", "finger_kiss", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 14)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@freakout", "freakout", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 15)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@heart_pumping", "heart_pumping", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 16)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@jazz_hands", "jazz_hands", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 17)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@karate_chops", "karate_chops", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 18)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@knuckle_crunch", "knuckle_crunch", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 19)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@mind_blown", "mind_blown", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 20)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@no_way", "no_way", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 21)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@nose_pick", "nose_pick", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 22)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@oh_snap", "oh_snap", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 23)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@peace", "peace", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 24)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@photography", "photography", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 25)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@raining_cash", "raining_cash", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 26)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@raise_the_roof", "raise_the_roof", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 27)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@rock", "rock", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 28)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@salsa_roll", "salsa_roll", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 29)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@salute", "salute", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 30)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@shadow_boxing", "shadow_boxing", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 31)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@shush", "shush", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 32)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@slow_clap", "slow_clap", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 33)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@smoke_flick", "smoke_flick", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 34)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@spray_champagne", "spray_champagne", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 35)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@stinker", "stinker", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 36)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@surrender", "surrender", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 37)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@the_woogie", "the_woogie", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 38)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@thumb_on_ears", "thumb_on_ears", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 39)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@thumbs_up", "thumbs_up", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 40)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@uncle_disco", "uncle_disco", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 41)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@v_sign", "v_sign", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 42)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@wank", "wank", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 43)
			{
				ped.Task.PlayAnimation("anim@mp_player_intcelebrationmale@wave", "wave", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
			if (num == 44)
			{
				ped.Task.PlayAnimation("aanim@mp_player_intcelebrationmale@you_loco", "you_loco", 1f, 1f, -1, AnimationFlags.None, 0f);
			}
		}

		public void Setoutfit(Ped p, int i)
		{
			try
			{
				if (p.Model == PedHash.MovAlien01)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (!(p.Model == PedHash.FreemodeMale01))
				{
					return;
				}
				Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 0, 0, 0);
				Function.Call(Hash.SET_PED_PROP_INDEX, p, -1, 0, 0, 17);
				bool flag = false;
				string iD_C = ID_C;
				if (i == 9)
				{
					if (iD_C.Equals("Outfit Default"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Purple"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 3, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Pink"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 7, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Orange"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 5, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (!flag)
					{
						if (!flag && !iD_C.Equals("Outfit Default"))
						{
							Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
						}
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 125, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 10)
				{
					if (iD_C.Equals("Outfit Default"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 89, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 55, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 54, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Black"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 89, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 55, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 54, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 11 && iD_C.Equals("Outfit Default"))
				{
					if (!flag && !iD_C.Equals("Outfit Default"))
					{
						Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
					}
					flag = true;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Random random = new Random();
					int num = random.Next(1, 100);
					if (num <= 25)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 38, 0, 1);
					}
					if (num > 25 && num <= 50)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 112, 0, 1);
					}
					if (num > 50 && num <= 75)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 46, 0, 1);
					}
					if (num > 75)
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 104, 25, 1);
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 125, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 68, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (i == 12)
				{
					if (iD_C.Equals("Blue"))
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 3, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Green"))
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Red"))
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 5, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 5, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 5, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 5, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Orange"))
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Purple"))
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 3, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 3, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Pink"))
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("White"))
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Black") || iD_C.Equals("Outfit Default"))
					{
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 91, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 84, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 97, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 186, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 13)
				{
					if (iD_C.Equals("Black"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("White"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 7, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 7, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 12, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Orange"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 13, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Purple"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 14, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Pink"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 15, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 11, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Red"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 14, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Outfit Default") || !flag)
					{
						if (!flag && !iD_C.Equals("Outfit Default"))
						{
							Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
						}
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 275, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 14)
				{
					if (iD_C.Equals("Black"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 5, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("White"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 7, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 12, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Orange"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 13, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Purple"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 14, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 11, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Pink"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 15, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Red"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Outfit Default") || !flag)
					{
						if (!flag && !iD_C.Equals("Outfit Default"))
						{
							Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
						}
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 142, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 19, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 107, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 84, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 276, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 15)
				{
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 134, 0, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 147, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 167, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 113, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 90, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 286, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 1, 0, 135, 0);
					}
					if (iD_C.Equals("Outfit Default") || !flag)
					{
						if (!flag && !iD_C.Equals("Outfit Default"))
						{
							Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
						}
						flag = true;
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 134, 0, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 147, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 167, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 113, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 90, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 286, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 1, 0, 135, 0);
					}
				}
				if (i == 16)
				{
					if (iD_C.Equals("Outfit Default"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 0, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 4, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Purple"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 6, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 3, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Pink"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 7, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Orange"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 5, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 2, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (!flag)
					{
						if (!flag && !iD_C.Equals("Outfit Default"))
						{
							Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
						}
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 115, 0, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 17, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 34, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 69, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 128, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 17)
				{
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 8, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Purple"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 10, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Pink"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 11, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 11, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 11, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 11, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 11, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Orange"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 9, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (!flag)
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 0, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Outfit Default") || !flag)
					{
						if (!flag && !iD_C.Equals("Outfit Default"))
						{
							Notification.Show("~y~Warning~w~ this Outfit did not have the specified color, setting to default, color chosen : " + iD_C);
						}
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 133, 0, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 166, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 110, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 88, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 2, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 283, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 18)
				{
					if (iD_C.Equals("Outfit Default"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 0, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 1, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 1, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("White"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 9, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 7, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 7, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 7, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Blue"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 3, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 3, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 3, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 3, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Black"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 10, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 10, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (!flag)
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 91, 0, 17);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 77, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 55, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 130, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 178, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 19)
				{
					if (iD_C.Equals("Outfit Default"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Blue"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 1, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 1, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Outfit Default") || !flag)
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 20)
				{
					if (iD_C.Equals("Outfit Default"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Blue"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 6, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 6, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 6, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("White"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 1, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 1, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Purple"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 4, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 4, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Pink"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 1, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 1, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Red"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 8, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 8, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Outfit Default") || !flag)
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 139, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 164, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 108, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 33, 0, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 277, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
				if (i == 21)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 0, 0, 0);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
				}
				if (i == 25)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 0, 0, 0);
					if (iD_C.Equals("Outfit Default"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 8, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 8, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Purple"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 9, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 1, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 9, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 9, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Green"))
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 8, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 8, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
					if (iD_C.Equals("Outfit Default") || !flag)
					{
						flag = true;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 1, 134, 8, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 3, 3, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 4, 106, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 6, 152, 8, 0);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 8, 15, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 10, 83, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 11, 274, 8, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, p, 12, 0, 0, 1);
					}
				}
			}
			catch
			{
			}
		}

		private void DisplayHelpTextThisFrameNoSound(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text + " 1" };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public void SetOutift(Ped ped, int Type)
		{
			if (Type == 39)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 40)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 41)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 42)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 43)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
			}
			if (Type == 44)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
			}
			if (Type == 45)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
			}
			if (Type == 52)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 53)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 54)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 55)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 56)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 57)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 58)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 59)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 60)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 61)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 62)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 63)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 64)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 65)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 7, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 66)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 67)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 68)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 69)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 70)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 71)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 72)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 73)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 322, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 38, 14, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 154, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 24, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 44, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 52, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 27, 0, true);
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 1, 19, 7, true);
			}
			if (Type == 74)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 16, 6, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 17, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 7, 3, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 12, 6, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 38, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 3, 4, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 75)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 8, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 23, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 3, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 24, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_HEAD_BLEND_DATA, ped, 26, 44, 0, 0, 0, 0, 0f, 0f, 0f, false);
				Function.Call(Hash.SET_PED_HEAD_OVERLAY, ped, 4, 45, 1f);
				Function.Call(Hash.SET_PED_HEAD_OVERLAY_TINT, ped, 4, 0, 20, 64);
				Function.Call(Hash.SET_PED_HAIR_TINT, ped, 44, 0);
			}
			if (Type == 76)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 27, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 69, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 33, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 3, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 72, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 77)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 5, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 11, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 14, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 78)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 6, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 12, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 11, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 6, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 79)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 6, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 8, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 14, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 80)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 15, 0, false);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 1, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 8, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 81)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 26, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 82)
			{
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Function.Call(Hash.SET_PED_PROP_INDEX, ped, 0, 24, 0, 0);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped, 1429513766);
			}
			if (Type == 83)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 25, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 12, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 6, 1, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 84)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 29, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 25, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 13, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 18, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
			if (Type == 85)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 31, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 24, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 9, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 11, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 12, 0, 0, 1);
			}
		}

		private void SetupHeists()
		{
			HackerCost = 90000f;
			TeamAI1Cost = 0f;
			TeamAI2Cost = 0f;
			TeamAI3Cost = 0f;
			TeamAI4Cost = 0f;
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
			List<object> list = new List<object>();
			list.Add("Robber (Default)");
			list.Add("Franklin Clinton");
			list.Add("Micheal De Santa");
			list.Add("Trevor Phillips");
			list.Add("Lamar Davis");
			list.Add("Clown");
			list.Add("Zombie");
			list.Add("Alien");
			list.Add("Juggernaut v2");
			list.Add("Doomsday Armour");
			list.Add("Heist Gear");
			list.Add("Heist Gear 2");
			list.Add("Juggernaut (Cosmetic)");
			list.Add("Arena War 1");
			list.Add("Arena War 2");
			list.Add("Space Monkey");
			list.Add("Commando");
			list.Add("Space Suit");
			list.Add("Tron");
			list.Add("Scifi Suit");
			list.Add("Alien 2");
			list.Add("Pogo ");
			list.Add("R. Space Range");
			list.Add("Mime ");
			list.Add("Mariachi ");
			list.Add("UBO ");
			list.Add("Sasquatch");
			list.Add("Dave Norton");
			list.Add("Steve Haines");
			list.Add("Devon Weston");
			list.Add("Paige");
			list.Add("Agent 14");
			list.Add("Lester");
			list.Add("Denise Clinton");
			list.Add("Nervous Ron");
			list.Add("Chef");
			list.Add("Eddie Toh");
			list.Add("Fabien");
			list.Add("Clay Simmons");
			list.Add("Norm Richards");
			list.Add("Patrick McReary");
			list.Add("Daryl Johns");
			list.Add("Gustavo Mota");
			list.Add("Hugo Welsh");
			list.Add("Karl Abolaji");
			list.Add("Chef (Heist)");
			list.Add("Lazlow Jones");
			list.Add("Amanda De Santa");
			list.Add("Tracy De Santa ");
			list.Add("Jimmy De Santa ");
			list.Add("Stretch");
			list.Add("Johnny Klebitz");
			list.Add("Norm Richards ");
			list.Add("Patrick McReary ");
			list.Add("Daryl Johns ");
			list.Add("Gustavo Mota");
			list.Add("Hugo Welsh ");
			list.Add("Karl Abolaji ");
			list.Add("Chef ");
			list.Add("Norm Richards");
			list.Add("Patrick McReary ");
			list.Add("Daryl Johns ");
			list.Add("Gustavo Mota ");
			list.Add("Hugo Welsh ");
			list.Add("Karl Abolaji");
			list.Add("Chef ");
			list.Add("Norm Richards ");
			list.Add("Patrick McReary ");
			list.Add("Daryl Johns");
			list.Add("Gustavo Mota ");
			list.Add("Hugo Welsh");
			list.Add("Karl Abolaji ");
			list.Add("Chef ");
			list.Add("Cruelmaster MC");
			list.Add("Micheal Myers");
			list.Add("Joker");
			list.Add("Jack the Ripper");
			list.Add("Michael (Grouppe6)");
			list.Add("Michael (Bugstars)");
			list.Add("Trevor (Grouppe6)");
			list.Add("Franklin (Bugstars)");
			list.Add("Michael (Balistic Gear)");
			list.Add("Trevor (Balistic Gear)");
			list.Add("Franklin (Tuxido)");
			list.Add("Michael (Tuxido)");
			list.Add("Trevor (Tuxido)");
			List<dynamic> AIskin = new List<object>();
			AIskin.Add(PedHash.Robber01SMY);
			AIskin.Add(PedHash.Franklin);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Trevor);
			AIskin.Add(PedHash.LamarDavis);
			AIskin.Add(PedHash.Clown01SMY);
			AIskin.Add(PedHash.Zombie01);
			AIskin.Add(PedHash.MovAlien01);
			AIskin.Add(PedHash.Juggernaut01M);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.Pogo01);
			AIskin.Add(PedHash.RsRanger01AMO);
			AIskin.Add(PedHash.MimeSMY);
			AIskin.Add(PedHash.Mariachi01SMM);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.Orleans);
			AIskin.Add(PedHash.DaveNorton);
			AIskin.Add(PedHash.SteveHains);
			AIskin.Add(PedHash.Devin);
			AIskin.Add(PedHash.Paige);
			AIskin.Add(PedHash.Agent14);
			AIskin.Add(PedHash.LesterCrest);
			AIskin.Add(PedHash.Denise);
			AIskin.Add(PedHash.NervousRon);
			AIskin.Add(PedHash.Chef);
			AIskin.Add(PedHash.EdToh);
			AIskin.Add(PedHash.Fabien);
			AIskin.Add(PedHash.Clay);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.Lazlow);
			AIskin.Add(PedHash.AmandaTownley);
			AIskin.Add(PedHash.TracyDisanto);
			AIskin.Add(PedHash.JimmyDisanto);
			AIskin.Add(PedHash.Stretch);
			AIskin.Add(PedHash.JohnnyKlebitzCutscene);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.PestContGunman);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.FreemodeMale01);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Trevor);
			AIskin.Add(PedHash.Franklin);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Trevor);
			AIskin.Add(PedHash.Franklin);
			AIskin.Add(PedHash.Michael);
			AIskin.Add(PedHash.Trevor);
			List<object> list2 = new List<object>();
			list2.Add("No AI");
			list2.Add("AI");
			List<object> list3 = new List<object>();
			list3.Add("Poor");
			list3.Add("Average");
			list3.Add("Moderate");
			list3.Add("Semi-Pro");
			list3.Add("Pro");
			list3.Add("Unbeatable");
			List<object> list4 = new List<object>();
			list4.Add("Poor");
			list4.Add("Average");
			list4.Add("Moderate");
			list4.Add("Semi-Pro");
			list4.Add("Pro");
			list4.Add("Unbeatable");
			list4.Add("Juggernaut");
			List<dynamic> DF = new List<object>();
			DF.Add("Very Easy");
			DF.Add("Easy");
			DF.Add("Normal");
			DF.Add("Hard");
			DF.Add("Very Hard");
			DF.Add("Extreme");
			List<object> list5 = new List<object>();
			list5.Add(false);
			list5.Add(true);
			List<dynamic> FP = new List<object>();
			FiringPattern[] array = (FiringPattern[])Enum.GetValues(typeof(FiringPattern));
			foreach (FiringPattern firingPattern in array)
			{
				FP.Add(firingPattern);
			}
			List<NativeListItem> list6 = new List<NativeListItem>();
			NativeListItem<dynamic> H = new NativeListItem<object>("Hacker Lvl : ", list3.ToArray());
			mainMenu.Add(H);
			list6.Add(H);
			NativeItem item2 = new NativeItem("-----------------------------------------------------------");
			mainMenu.Add(item2);
			NativeListItem<dynamic> C = new NativeListItem<object>("Color : ", Colours.ToArray());
			mainMenu.Add(C);
			list6.Add(C);
			C.Description = "Use this Colour Whenever possible For AI or use Default";
			NativeItem item3 = new NativeItem("-----------------------------------------------------------");
			mainMenu.Add(item3);
			NativeMenu nativeMenu = new NativeMenu("", "Team Mate 1 Options");
			modLemonUIObjectPool.Add(nativeMenu);
			Heists.AddSubMenu(nativeMenu);
			NativeMenu nativeMenu2 = new NativeMenu("", "Team Mate 2 Options");
			modLemonUIObjectPool.Add(nativeMenu2);
			Heists.AddSubMenu(nativeMenu2);
			NativeMenu nativeMenu3 = new NativeMenu("", "Team Mate 3 Options");
			modLemonUIObjectPool.Add(nativeMenu3);
			Heists.AddSubMenu(nativeMenu3);
			NativeMenu nativeMenu4 = new NativeMenu("", "View Crew Cuts");
			modLemonUIObjectPool.Add(nativeMenu4);
			Heists.AddSubMenu(nativeMenu4);
			NativeListItem<dynamic> T1 = new NativeListItem<object>("Team Mate 1  : ", list2.ToArray());
			nativeMenu.Add(T1);
			list6.Add(T1);
			T1.SelectedIndex = 1;
			NativeListItem<dynamic> T1S = new NativeListItem<object>("Team Mate 1  Skill : ", list4.ToArray());
			nativeMenu.Add(T1S);
			list6.Add(T1S);
			NativeListItem<dynamic> T1Skin = new NativeListItem<object>("Team Mate 1 Skin : ", list.ToArray());
			nativeMenu.Add(T1Skin);
			list6.Add(T1Skin);
			T1Skin.Enabled = false;
			NativeListItem<dynamic> T1FP = new NativeListItem<object>("Team Mate 1 Fire Mode : ", FP.ToArray());
			nativeMenu.Add(T1FP);
			list6.Add(T1FP);
			NativeListItem<dynamic> T2 = new NativeListItem<object>("Team Mate 2  : ", list2.ToArray());
			nativeMenu2.Add(T2);
			list6.Add(T2);
			NativeListItem<dynamic> T2S = new NativeListItem<object>("Team Mate 2  Skill : ", list4.ToArray());
			nativeMenu2.Add(T2S);
			list6.Add(T2S);
			NativeListItem<dynamic> T2Skin = new NativeListItem<object>("Team Mate 2 Skin : ", list.ToArray());
			nativeMenu2.Add(T2Skin);
			list6.Add(T2Skin);
			NativeListItem<dynamic> T2FP = new NativeListItem<object>("Team Mate 2 Fire Mode : ", FP.ToArray());
			nativeMenu2.Add(T2FP);
			list6.Add(T2FP);
			NativeListItem<dynamic> T3 = new NativeListItem<object>("Team Mate 3  : ", list2.ToArray());
			nativeMenu3.Add(T3);
			list6.Add(T3);
			NativeListItem<dynamic> T3S = new NativeListItem<object>("Team Mate 3  Skill : ", list4.ToArray());
			nativeMenu3.Add(T3S);
			list6.Add(T3S);
			NativeListItem<dynamic> T3Skin = new NativeListItem<object>("Team Mate 3 Skin : ", list.ToArray());
			nativeMenu3.Add(T3Skin);
			list6.Add(T3Skin);
			NativeListItem<dynamic> T3FP = new NativeListItem<object>("Team Mate 3 Fire Mode : ", FP.ToArray());
			nativeMenu3.Add(T3FP);
			list6.Add(T3FP);
			NativeItem nativeItem = new NativeItem("~y~Start Heist");
			mainMenu.Add(nativeItem);
			NativeListItem<object> nativeListItem = new NativeListItem<object>("AI Weapons Silenced : ", list5.ToArray());
			mainMenu.Add(nativeListItem);
			nativeListItem.Description = "When True, Teammate Weapons will always be Silenced, and not Unsilenced when player is using a Unsilenced Weapon";
			NativeListItem<dynamic> SETDiff = new NativeListItem<object>("Difficulty : ", DF.ToArray());
			mainMenu.Add(SETDiff);
			SETDiff.Description = "Sets the Difficulty of The Ai in the Heist";
			SETDiff.SelectedIndex = 2;
			NativeListItem<dynamic> PriorityLootO = new NativeListItem<object>("Prioritize Closer Loot : ", list5.ToArray());
			Heists.Add(PriorityLootO);
			PriorityLootO.Description = "When True, AI Will Concentrate on Loot that closer to them rather than random loot, saving time";
			PriorityLootO.ItemChanged += delegate
			{
				PriorityLoot = PriorityLootO.SelectedIndex;
				Notification.Show("Priority Loot Set to ~b~" + IntToBool(PriorityLoot));
			};
			NativeItem item4 = new NativeItem("-----------------------------------------------------------");
			mainMenu.Add(item4);
			NativeItem CrewCut = new NativeItem("Crew Cut : $~g~0,00");
			nativeMenu4.Add(CrewCut);
			NativeItem AI1Cyt = new NativeItem("AI 1 Cut : $~g~0,00~w~/~b~0%");
			nativeMenu4.Add(AI1Cyt);
			NativeItem AI2Cyt = new NativeItem("AI 2 Cut : $~g~0,00~w~/~b~0%");
			nativeMenu4.Add(AI2Cyt);
			NativeItem AI3Cyt = new NativeItem("AI 3 Cut : $~g~0,00~w~/~b~0%");
			nativeMenu4.Add(AI3Cyt);
			NativeItem HCyt = new NativeItem("Hacker Cut: $~g~0,00~w~/~b~0%");
			nativeMenu4.Add(HCyt);
			SETDiff.ItemChanged += delegate
			{
				Difficulty = SETDiff.SelectedIndex;
				float num2 = ((float)Difficulty - 2f) * 5f;
				num2 += 10f;
				SETDiff.Description = "Sets the Difficulty of The Ai in the Heist, Extra Pay : ~" + num2 + "%";
				Notification.Show("Difficulty has been set to ~b~" + DF[SETDiff.SelectedIndex] + ", extra pay : " + num2 + "%");
			};
			foreach (NativeListItem<object> item5 in list6)
			{
				item5.ItemChanged += delegate
				{
					ID_C = Colours[C.SelectedIndex];
					Difficulty = SETDiff.SelectedIndex;
					float num = ((float)Difficulty - 2f) * 5f;
					num += 10f;
					SETDiff.Description = "Sets the Difficulty of The Ai in the Heist";
					Notification.Show("Difficulty has been set to ~b~" + DF[SETDiff.SelectedIndex] + ", extra pay : " + num + "%");
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					if (T1.SelectedIndex == 1)
					{
						if (RobberA != null)
						{
							RobberA.Delete();
						}
						RobberA = GlobalHelper.CreatePed(AIskin[T1Skin.SelectedIndex], new Vector3(400.3f, -960.1f, -100f), -88);
						RobberA.IsPositionFrozen = true;
						Setoutfit(RobberA, T1Skin.SelectedIndex);
						PlayAnimation(RobberA);
						if (T1Skin.SelectedIndex <= 26)
						{
							OutfitReq = "No Requirement";
							RobberASkin = AIskin[T1Skin.SelectedIndex];
							RobberA_C = T1Skin.SelectedIndex;
							if (T1Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberA, 21);
								RobberA_C = 21;
							}
						}
						if (T1Skin.SelectedIndex > 26)
						{
							OutfitReq = "No Requirement";
							RobberASkin = AIskin[T1Skin.SelectedIndex];
							RobberA_C = T1Skin.SelectedIndex;
							SetOutift(RobberA, RobberA_C);
						}
						if (T1Skin.SelectedIndex > 37)
						{
							OutfitReq = "No Requirement";
							RobberASkin = AIskin[T1Skin.SelectedIndex];
							RobberA_C = T1Skin.SelectedIndex;
							if (T1Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberA, 21);
								RobberA_C = 21;
							}
						}
					}
					else
					{
						if (RobberA != null)
						{
							RobberA.Delete();
						}
						RobberA_C = 0;
					}
					if (T2.SelectedIndex == 1)
					{
						if (RobberB != null)
						{
							RobberB.Delete();
						}
						RobberB = GlobalHelper.CreatePed(AIskin[T2Skin.SelectedIndex], new Vector3(400.3f, -957.5f, -100f), -88);
						RobberB.IsPositionFrozen = true;
						Setoutfit(RobberB, T2Skin.SelectedIndex);
						PlayAnimation(RobberB);
						if (T2Skin.SelectedIndex <= 26)
						{
							OutfitReq = "No Requirement";
							RobberBSkin = AIskin[T2Skin.SelectedIndex];
							RobberB_C = T2Skin.SelectedIndex;
							if (T2Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberB, 21);
								RobberB_C = 21;
							}
						}
						if (T2Skin.SelectedIndex > 26)
						{
							OutfitReq = "No Requirement";
							RobberBSkin = AIskin[T2Skin.SelectedIndex];
							RobberB_C = T2Skin.SelectedIndex;
							SetOutift(RobberB, RobberB_C);
						}
						if (T2Skin.SelectedIndex > 27)
						{
							OutfitReq = "No Requirement";
							RobberBSkin = AIskin[T2Skin.SelectedIndex];
							RobberB_C = T2Skin.SelectedIndex;
							if (T2Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberB, 21);
								RobberB_C = 21;
							}
						}
					}
					else
					{
						if (RobberB != null)
						{
							RobberB.Delete();
						}
						RobberB_C = 0;
					}
					if (T3.SelectedIndex == 1)
					{
						if (RobberC != null)
						{
							RobberC.Delete();
						}
						RobberC = GlobalHelper.CreatePed(AIskin[T3Skin.SelectedIndex], new Vector3(400.3f, -956.5f, -100f), -88);
						RobberC.IsPositionFrozen = true;
						Setoutfit(RobberC, T3Skin.SelectedIndex);
						PlayAnimation(RobberC);
						if (T3Skin.SelectedIndex <= 26)
						{
							OutfitReq = "No Requirement";
							RobberCSkin = AIskin[T3Skin.SelectedIndex];
							RobberC_C = T3Skin.SelectedIndex;
							if (T3Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberC, 21);
								RobberC_C = 21;
							}
						}
						if (T3Skin.SelectedIndex > 26)
						{
							OutfitReq = "No Requirement";
							RobberCSkin = AIskin[T3Skin.SelectedIndex];
							RobberC_C = T3Skin.SelectedIndex;
							SetOutift(RobberC, RobberC_C);
						}
						if (T3Skin.SelectedIndex > 27)
						{
							OutfitReq = "No Requirement";
							RobberCSkin = AIskin[T3Skin.SelectedIndex];
							RobberC_C = T3Skin.SelectedIndex;
							if (T3Skin.SelectedIndex == 6)
							{
								Setoutfit(RobberC, 21);
								RobberC_C = 21;
							}
						}
					}
					else
					{
						if (RobberC != null)
						{
							RobberC.Delete();
						}
						RobberC_C = 0;
					}
					if (T3.SelectedIndex == 1)
					{
						if (T3S.SelectedIndex != 0)
						{
							TeamAI3Cost = (float)MaxPay * (0.03f * (float)(T3S.SelectedIndex + 1));
						}
						else
						{
							TeamAI3Cost = 0f;
						}
						TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
						CrewCut.Title = "CrewCut : $" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
					}
					if (T2.SelectedIndex == 1)
					{
						if (T2S.SelectedIndex != 0)
						{
							TeamAI2Cost = (float)MaxPay * (0.03f * (float)(T2S.SelectedIndex + 1));
						}
						else
						{
							TeamAI2Cost = 0f;
						}
						TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
						CrewCut.Title = "CrewCut : $" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
					}
					if (T1.SelectedIndex == 1)
					{
						if (T1S.SelectedIndex != 0)
						{
							TeamAI1Cost = (float)MaxPay * (0.03f * (float)(T1S.SelectedIndex + 1));
						}
						else
						{
							TeamAI1Cost = 0f;
						}
						TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
						CrewCut.Title = "CrewCut : $" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
					}
					if (H.SelectedIndex != 0)
					{
						HackerCost = (float)MaxPay * (0.015f * (float)H.SelectedIndex);
					}
					else
					{
						HackerCost = (float)MaxPay * 0.015f;
					}
					TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
					CrewCut.Title = "CrewCut : $" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
					CrewCut.Title = "Crew Cut: $~g~" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
					AI1Cyt.Title = "AI 1 Cut : $~g~" + TeamAI1Cost.ToString("N") + "~w~/~b~" + 0.03f * (float)(T1S.SelectedIndex + 1) * 100f + "%";
					AI2Cyt.Title = "AI 2 Cut : $~g~" + TeamAI2Cost.ToString("N") + "~w~/~b~" + 0.03f * (float)(T2S.SelectedIndex + 1) * 100f + "%";
					AI3Cyt.Title = "AI 3 Cut : $~g~" + TeamAI3Cost.ToString("N") + "~w~/~b~" + 0.03f * (float)(T3S.SelectedIndex + 1) * 100f + "%";
					HCyt.Title = "Hacker Cut: $~g~" + HackerCost.ToString("N") + "~w~/~b~" + 0.015f * (float)(H.SelectedIndex + 1) * 100f + "%";
				};
			}
			nativeItem.Activated += delegate
			{
				RobberAFP = FP[T1FP.SelectedIndex];
				RobberBFP = FP[T2FP.SelectedIndex];
				RobberCFP = FP[T3FP.SelectedIndex];
				SetupAi = true;
				ID_C = Colours[C.SelectedIndex];
				if (RobberA != null)
				{
					RobberA.Delete();
				}
				if (RobberB != null)
				{
					RobberB.Delete();
				}
				if (RobberC != null)
				{
					RobberC.Delete();
				}
				if (HeistStartCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					HeistStartCam.IsActive = false;
					HeistStartCam.Delete();
					Game.Player.Character.IsPositionFrozen = false;
					Game.Player.CanControlCharacter = true;
				}
				if (T3.SelectedIndex == 1)
				{
					if (T3.SelectedIndex == 1)
					{
						if (T3S.SelectedIndex != 0)
						{
							TeamAI3Cost = BaseCrewCost + BaseCrewCost * (float)T3S.SelectedIndex * (float)T3S.SelectedIndex / 10f;
						}
						else
						{
							TeamAI3Cost = BaseCrewCost;
						}
					}
				}
				else
				{
					TeamAI3Cost = 0f;
				}
				if (T2.SelectedIndex == 1)
				{
					if (T2.SelectedIndex == 1)
					{
						if (T2S.SelectedIndex != 0)
						{
							TeamAI2Cost = BaseCrewCost + BaseCrewCost * (float)T2S.SelectedIndex * (float)T2S.SelectedIndex / 10f;
						}
						else
						{
							TeamAI2Cost = BaseCrewCost;
						}
					}
				}
				else
				{
					TeamAI2Cost = 0f;
				}
				if (T1.SelectedIndex == 1)
				{
					if (T1.SelectedIndex == 1)
					{
						if (T1S.SelectedIndex != 0)
						{
							TeamAI1Cost = BaseCrewCost + BaseCrewCost * (float)T1S.SelectedIndex * (float)T1S.SelectedIndex / 10f;
						}
						else
						{
							TeamAI1Cost = BaseCrewCost;
						}
					}
				}
				else
				{
					TeamAI1Cost = 0f;
				}
				if (H.SelectedIndex != 0)
				{
					HackerCost = 90000 + 90000 * H.SelectedIndex * H.SelectedIndex / 10;
				}
				else
				{
					HackerCost = 90000f;
				}
				TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
				CrewCut.Title = "CrewCut : $" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
				if (T1.SelectedIndex != 0)
				{
					RobberAskill = T1S.SelectedIndex + 1;
				}
				else
				{
					RobberAskill = 0;
				}
				if (T2.SelectedIndex != 0)
				{
					RobberBskill = T2S.SelectedIndex + 1;
				}
				else
				{
					RobberBskill = 0;
				}
				if (T3.SelectedIndex != 0)
				{
					RobberCskill = T3S.SelectedIndex + 1;
				}
				else
				{
					RobberCskill = 0;
				}
				if (H.SelectedIndex != 6)
				{
					Hackerskill = H.SelectedIndex;
				}
				if (H.SelectedIndex == 6)
				{
					Hackerskill = 5;
				}
				if (RobberC != null)
				{
					RobberC.Delete();
				}
				if (RobberB != null)
				{
					RobberB.Delete();
				}
				if (RobberA != null)
				{
					RobberA.Delete();
				}
				CameraON = false;
				if (HeistStartCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					HeistStartCam.IsActive = false;
					HeistStartCam.Delete();
				}
				if (!SetupAi)
				{
					if (RobberC != null)
					{
						Notification.Show("DELETE");
						RobberC.Delete();
					}
					if (RobberB != null)
					{
						RobberB.Delete();
					}
					if (RobberA != null)
					{
						RobberA.Delete();
					}
				}
				Game.Player.Character.Position = StartHeistMarker;
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
				modLemonUIObjectPool.HideAll();
				Script.Wait(50);
				EndofHiest();
				Script.Wait(50);
				SetupAi = false;
			};
		}

		private void SetupHeists(bool unk)
		{
			HackerCost = 90000f;
			TeamAI1Cost = 0f;
			TeamAI2Cost = 0f;
			TeamAI3Cost = 0f;
			TeamAI4Cost = 0f;
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
			List<object> list = new List<object>();
			list.Add("Robber (Default)");
			list.Add("Franklin Clinton");
			list.Add("Micheal De Santa");
			list.Add("Trevor Phillips");
			list.Add("Lamar Davis");
			list.Add("Clown");
			list.Add("Zombie");
			list.Add("Alien");
			list.Add("Juggernaut v2");
			list.Add("Doomsday Armour");
			list.Add("Heist Gear");
			list.Add("Heist Gear 2");
			list.Add("Juggernaut (Cosmetic)");
			list.Add("Arena War 1");
			list.Add("Arena War 2");
			list.Add("Space Monkey");
			list.Add("Commando");
			list.Add("Space Suit");
			list.Add("Tron");
			list.Add("Scifi Suit");
			list.Add("Alien 2");
			list.Add("Pogo ");
			list.Add("R. Space Range");
			list.Add("Mime ");
			list.Add("Mariachi ");
			list.Add("UBO ");
			list.Add("Sasquatch");
			list.Add("Dave Norton");
			list.Add("Steve Haines");
			list.Add("Devon Weston");
			list.Add("Paige");
			list.Add("Agent 14");
			list.Add("Lester");
			list.Add("Denise Clinton");
			list.Add("Nervous Ron");
			list.Add("Chef");
			list.Add("Eddie Toh");
			list.Add("Fabien");
			list.Add("Clay Simmons");
			list.Add("Norm Richards");
			list.Add("Patrick McReary");
			list.Add("Daryl Johns");
			list.Add("Gustavo Mota");
			list.Add("Hugo Welsh");
			list.Add("Karl Abolaji");
			list.Add("Chef (Heist)");
			list.Add("Lazlow Jones");
			list.Add("Amanda De Santa");
			list.Add("Tracy De Santa ");
			list.Add("Jimmy De Santa ");
			list.Add("Stretch");
			list.Add("Johnny Klebitz");
			list.Add("Norm Richards ");
			list.Add("Patrick McReary ");
			list.Add("Daryl Johns ");
			list.Add("Gustavo Mota");
			list.Add("Hugo Welsh ");
			list.Add("Karl Abolaji ");
			list.Add("Chef ");
			list.Add("Norm Richards");
			list.Add("Patrick McReary ");
			list.Add("Daryl Johns ");
			list.Add("Gustavo Mota ");
			list.Add("Hugo Welsh ");
			list.Add("Karl Abolaji");
			list.Add("Chef ");
			list.Add("Norm Richards ");
			list.Add("Patrick McReary ");
			list.Add("Daryl Johns");
			list.Add("Gustavo Mota ");
			list.Add("Hugo Welsh");
			list.Add("Karl Abolaji ");
			list.Add("Chef ");
			list.Add("Cruelmaster MC");
			list.Add("Micheal Myers");
			list.Add("Joker");
			list.Add("Jack the Ripper");
			list.Add("Michael (Grouppe6)");
			list.Add("Michael (Bugstars)");
			list.Add("Trevor (Grouppe6)");
			list.Add("Franklin (Bugstars)");
			list.Add("Michael (Balistic Gear)");
			list.Add("Trevor (Balistic Gear)");
			list.Add("Franklin (Tuxido)");
			list.Add("Michael (Tuxido)");
			list.Add("Trevor (Tuxido)");
			List<object> list2 = new List<object>();
			list2.Add(PedHash.Robber01SMY);
			list2.Add(PedHash.Franklin);
			list2.Add(PedHash.Michael);
			list2.Add(PedHash.Trevor);
			list2.Add(PedHash.LamarDavis);
			list2.Add(PedHash.Clown01SMY);
			list2.Add(PedHash.Zombie01);
			list2.Add(PedHash.MovAlien01);
			list2.Add(PedHash.Juggernaut01M);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.Pogo01);
			list2.Add(PedHash.RsRanger01AMO);
			list2.Add(PedHash.MimeSMY);
			list2.Add(PedHash.Mariachi01SMM);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.Orleans);
			list2.Add(PedHash.DaveNorton);
			list2.Add(PedHash.SteveHains);
			list2.Add(PedHash.Devin);
			list2.Add(PedHash.Paige);
			list2.Add(PedHash.Agent14);
			list2.Add(PedHash.LesterCrest);
			list2.Add(PedHash.Denise);
			list2.Add(PedHash.NervousRon);
			list2.Add(PedHash.Chef);
			list2.Add(PedHash.EdToh);
			list2.Add(PedHash.Fabien);
			list2.Add(PedHash.Clay);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.Lazlow);
			list2.Add(PedHash.AmandaTownley);
			list2.Add(PedHash.TracyDisanto);
			list2.Add(PedHash.JimmyDisanto);
			list2.Add(PedHash.Stretch);
			list2.Add(PedHash.JohnnyKlebitzCutscene);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.PestContGunman);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.FreemodeMale01);
			list2.Add(PedHash.Michael);
			list2.Add(PedHash.Michael);
			list2.Add(PedHash.Trevor);
			list2.Add(PedHash.Franklin);
			list2.Add(PedHash.Michael);
			list2.Add(PedHash.Trevor);
			list2.Add(PedHash.Franklin);
			list2.Add(PedHash.Michael);
			list2.Add(PedHash.Trevor);
			List<object> list3 = new List<object>();
			list3.Add("No AI");
			list3.Add("AI");
			List<object> list4 = new List<object>();
			list4.Add("Poor");
			list4.Add("Average");
			list4.Add("Moderate");
			list4.Add("Semi-Pro");
			list4.Add("Pro");
			list4.Add("Unbeatable");
			List<object> list5 = new List<object>();
			list5.Add("Poor");
			list5.Add("Average");
			list5.Add("Moderate");
			list5.Add("Semi-Pro");
			list5.Add("Pro");
			list5.Add("Unbeatable");
			list5.Add("Juggernaut");
			List<dynamic> DF = new List<object>();
			DF.Add("Very Easy");
			DF.Add("Easy");
			DF.Add("Normal");
			DF.Add("Hard");
			DF.Add("Very Hard");
			DF.Add("Extreme");
			List<object> list6 = new List<object>();
			list6.Add(false);
			list6.Add(true);
			List<object> list7 = new List<object>();
			FiringPattern[] array = (FiringPattern[])Enum.GetValues(typeof(FiringPattern));
			foreach (FiringPattern firingPattern in array)
			{
				list7.Add(firingPattern);
			}
			List<NativeListItem> list8 = new List<NativeListItem>();
			NativeListItem<dynamic> H = new NativeListItem<object>("Hacker Lvl : ", list4.ToArray());
			mainMenu.Add(H);
			list8.Add(H);
			NativeItem item2 = new NativeItem("-----------------------------------------------------------");
			mainMenu.Add(item2);
			NativeListItem<dynamic> C = new NativeListItem<object>("Color : ", Colours.ToArray());
			mainMenu.Add(C);
			list8.Add(C);
			C.Description = "Use this Colour Whenever possible For AI or use Default";
			NativeItem item3 = new NativeItem("-----------------------------------------------------------");
			mainMenu.Add(item3);
			NativeListItem<object> nativeListItem = new NativeListItem<object>("AI Weapons Silenced : ", list6.ToArray());
			mainMenu.Add(nativeListItem);
			nativeListItem.Description = "When True, Teammate Weapons will always be Silenced, and not Unsilenced when player is using a Unsilenced Weapon";
			NativeListItem<dynamic> SETDiff = new NativeListItem<object>("Difficulty : ", DF.ToArray());
			mainMenu.Add(SETDiff);
			SETDiff.Description = "Sets the Difficulty of The Ai in the Heist";
			SETDiff.SelectedIndex = 2;
			NativeListItem<dynamic> PriorityLootO = new NativeListItem<object>("Prioritize Closer Loot : ", list6.ToArray());
			Heists.Add(PriorityLootO);
			PriorityLootO.Description = "When True, AI Will Concentrate on Loot that closer to them rather than random loot, saving time";
			PriorityLootO.ItemChanged += delegate
			{
				PriorityLoot = PriorityLootO.SelectedIndex;
				Notification.Show("Priority Loot Set to ~b~" + IntToBool(PriorityLoot));
			};
			NativeItem CrewCut = new NativeItem("Crew Cut : $~g~0,00");
			mainMenu.Add(CrewCut);
			NativeItem nativeItem = new NativeItem("~y~Start Heist");
			mainMenu.Add(nativeItem);
			SETDiff.ItemChanged += delegate
			{
				Difficulty = SETDiff.SelectedIndex;
				float num2 = ((float)Difficulty - 2f) * 5f;
				num2 += 10f;
				SETDiff.Description = "Sets the Difficulty of The Ai in the Heist, Extra Pay : ~" + num2 + "%";
				Notification.Show("Difficulty has been set to ~b~" + DF[SETDiff.SelectedIndex] + ", extra pay : " + num2 + "%");
			};
			foreach (NativeListItem<object> item4 in list8)
			{
				item4.ItemChanged += delegate
				{
					ID_C = Colours[C.SelectedIndex];
					Difficulty = SETDiff.SelectedIndex;
					float num = ((float)Difficulty - 2f) * 5f;
					num += 10f;
					SETDiff.Description = "Sets the Difficulty of The Ai in the Heist";
					Notification.Show("Difficulty has been set to ~b~" + DF[SETDiff.SelectedIndex] + ", extra pay : " + num + "%");
					if (RobberA != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberA);
					}
					if (RobberB != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberB);
					}
					if (RobberC != null)
					{
						Script.Wait(5);
						PlayAnimation(RobberC);
					}
					if (H.SelectedIndex != 0)
					{
						HackerCost = (float)MaxPay * (0.015f * (float)H.SelectedIndex);
					}
					else
					{
						HackerCost = (float)MaxPay * 0.015f;
					}
					TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
					CrewCut.Title = "CrewCut : $" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
					CrewCut.Title = "Crew Cut: $~g~" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
				};
			}
			nativeItem.Activated += delegate
			{
				SetupAi = true;
				ID_C = Colours[C.SelectedIndex];
				if (RobberA != null)
				{
					RobberA.Delete();
				}
				if (RobberB != null)
				{
					RobberB.Delete();
				}
				if (RobberC != null)
				{
					RobberC.Delete();
				}
				if (HeistStartCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					HeistStartCam.IsActive = false;
					HeistStartCam.Delete();
					Game.Player.Character.IsPositionFrozen = false;
					Game.Player.CanControlCharacter = true;
				}
				if (H.SelectedIndex != 0)
				{
					HackerCost = 90000 + 90000 * H.SelectedIndex * H.SelectedIndex / 10;
				}
				else
				{
					HackerCost = 90000f;
				}
				TotalCost = HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost;
				CrewCut.Title = "CrewCut : $" + (HackerCost + TeamAI1Cost + TeamAI2Cost + TeamAI3Cost + TeamAI4Cost).ToString("N");
				if (H.SelectedIndex != 6)
				{
					Hackerskill = H.SelectedIndex;
				}
				if (H.SelectedIndex == 6)
				{
					Hackerskill = 5;
				}
				if (RobberC != null)
				{
					RobberC.Delete();
				}
				if (RobberB != null)
				{
					RobberB.Delete();
				}
				if (RobberA != null)
				{
					RobberA.Delete();
				}
				CameraON = false;
				if (HeistStartCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					HeistStartCam.IsActive = false;
					HeistStartCam.Delete();
				}
				if (!SetupAi)
				{
					if (RobberC != null)
					{
						Notification.Show("DELETE");
						RobberC.Delete();
					}
					if (RobberB != null)
					{
						RobberB.Delete();
					}
					if (RobberA != null)
					{
						RobberA.Delete();
					}
				}
				Game.Player.Character.Position = StartHeistMarker;
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
				modLemonUIObjectPool.HideAll();
				Script.Wait(50);
				EndofHiest();
				Script.Wait(50);
				SetupAi = false;
			};
		}

		public void PlayDealerAnim(Ped ped)
		{
			Random random = new Random();
			float num = random.Next(1, 27);
			ped.BlockPermanentEvents = true;
			if (num < 2f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@threecardpoker@dealer", "deck_shuffle", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 2f && num < 5f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@dealer_female", "dealer_idle", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 5f && num < 7f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@blackjack@dealer_female", "dealer_focus_player_01_idle", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 7f && num < 10f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_01", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 10f && num < 12f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_02", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 12f && num < 14f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_03", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 14f && num < 17f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_04", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 17f && num < 20f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_05", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 20f && num < 23f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_06", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 23f && num < 25f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_07", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			if (num >= 25f && num < 28f)
			{
				ped.Task.PlayAnimation("anim_casino_b@amb@casino@games@shared@dealer@", "female_idle_var_08", random.Next(64, 320), 1f, -1, AnimationFlags.Loop, -1f);
			}
			ped.AlwaysKeepTask = true;
		}

		public void SetPedOutfit(Ped ped)
		{
			Random random = new Random();
			int num = random.Next(1, 9);
			if (num == 1)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num2 = random.Next(0, 5);
				int num3 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num2, num3, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 1, 1);
				int num4 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num4, 0, 1);
				int num5 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, num5, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num6 = random.Next(0, 1);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, num6, 1);
			}
			if (num == 2)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num7 = random.Next(0, 5);
				int num8 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num7, num8, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				int num9 = random.Next(0, 2);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num9, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 1, 0, 1);
			}
			if (num == 3)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num10 = random.Next(0, 5);
				int num11 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num10, num11, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				int num12 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num12, 0, 1);
				int num13 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, num13, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num14 = random.Next(0, 1);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, num14, 1);
			}
			if (num == 4)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num15 = random.Next(0, 5);
				int num16 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num15, num16, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				int num17 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num17, 0, 1);
				int num18 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, num18, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num19 = random.Next(0, 1);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, num19, 1);
			}
			if (num == 5)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num20 = random.Next(0, 5);
				int num21 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num20, num21, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				int num22 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num22, 0, 1);
				int num23 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, num23, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num24 = random.Next(0, 1);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, num24, 1);
			}
			if (num == 6)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num25 = random.Next(0, 5);
				int num26 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num25, num26, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 1, 1);
				int num27 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num27, 0, 1);
				int num28 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, num28, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num29 = random.Next(0, 1);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 1, 1);
			}
			if (num == 7)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num30 = random.Next(0, 5);
				int num31 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num30, num31, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 1, 1);
				int num32 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num32, 0, 1);
				int num33 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num34 = random.Next(0, 1);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 1, 1);
			}
			if (num == 8)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num35 = random.Next(0, 5);
				int num36 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num35, num36, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				int num37 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num37, 0, 1);
				int num38 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, num38, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num39 = random.Next(0, 1);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 1, 1);
			}
			if (num == 9)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 0, 4, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 1, 0, 0, 1);
				int num40 = random.Next(0, 5);
				int num41 = random.Next(0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 2, num40, num41, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 3, 1, 2, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 4, 1, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 6, 0, 0, 1);
				int num42 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 7, num42, 0, 1);
				int num43 = random.Next(0, 2);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 8, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 9, 2, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 10, 0, 0, 1);
				int num44 = random.Next(0, 1);
				Script.Wait(1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped, 11, 0, 1, 1);
			}
		}

		public void SetupPedCostume(Ped p)
		{
			Random random = new Random();
			float num = random.Next(0, 275);
			if (num < 200f)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 0, 0, false);
			}
			if (num > 200f)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 1, 0, false);
			}
			if (num > 225f)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 2, 0, false);
			}
			if (num > 250f)
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, p, 0, 3, 0, false);
			}
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

		public void EndofHiest()
		{
			PoliceAlearted = false;
			LesterToldPlayerAboutContractCut = false;
			foreach (ObjectPlacementMap swatEntty in SwatEntties)
			{
				if (swatEntty.Entity != null)
				{
					swatEntty.Entity.Delete();
				}
			}
			PercentNeedTake = (int)((float)(HeistMinPay + HeistMaxPay) / 1.5f / 100f * 45f);
			foreach (KeypadClass keyPad in KeyPads)
			{
				if (keyPad.Keypad != null)
				{
					keyPad.Keypad.Delete();
				}
				keyPad.SpawnedKeypad = false;
			}
			UnlockedVaultEnterance = false;
			UnlockedStaffDoor = false;
			UnlockedThreeFloorBypass = false;
			UnlockedVaultToOffice = false;
			HackedWasteDisposalDoor = false;
			LocalTake = 0;
			CanNoLongerOverideCommands = false;
			RobberARobbing = false;
			RobberBRobbing = false;
			RobberCRobbing = false;
			RobberACuttingStage = 0;
			RobberBCuttingStage = 0;
			RobberCCuttingStage = 0;
			RobberAInitialAnimDone = false;
			RobberBInitialAnimDone = false;
			RobberCInitialAnimDone = false;
			RobberA_RobberMarker = new Vector3(0f, 0f, 0f);
			RobberB_RobberMarker = new Vector3(0f, 0f, 0f);
			RobberC_RobberMarker = new Vector3(0f, 0f, 0f);
			HeistTime = 0f;
			Notification.Show("~g~ HKH191 ~w~ : Loading Heist, Please be Patient");
			SetupHeiststuff();
			if (RobberC != null)
			{
				RobberC.Delete();
			}
			if (RobberB != null)
			{
				RobberB.Delete();
			}
			Game.Player.Character.Opacity = 0;
			Vector3 position = Game.Player.Character.Position;
			Game.Player.Character.Position = new Vector3(152f, 322f, 612f);
			Script.Wait(1);
			Game.Player.Character.Position = position;
			Game.Player.Character.Opacity = 255;
			if (HeistStartCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				HeistStartCam.IsActive = false;
				HeistStartCam.Delete();
			}
			Game.Player.Character.IsPositionFrozen = false;
			Game.Player.CanControlCharacter = true;
			if (RobberA != null && RobberAskill != 0)
			{
				if (RobberAskill != 7)
				{
					RobberA = GlobalHelper.CreatePed(RequestPed(RobberASkin), new Vector3(712.6678f, -977.357f, 24f), 168f);
					Setoutfit(RobberA, RobberA_C);
					SetOutift(RobberA, RobberA_C);
				}
				if (RobberAskill == 7)
				{
					RobberA = GlobalHelper.CreatePed(PedHash.FreemodeMale01, new Vector3(712.6678f, -977.357f, 24f), 168f);
					Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
					Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
					Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
					Ped robberA = RobberA;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 1, 91, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 4, 84, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 8, 97, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 11, 186, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberA, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_USING_ACTION_MODE, robberA, true, -1, 0);
					Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, robberA, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
					Function.Call(Hash.SET_PED_STRAFE_CLIPSET, robberA, "MOVE_STRAFE_BALLISTIC");
					Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, robberA, 1429513766);
				}
				if (RobberAskill == 1)
				{
					RobberA.Health = 300;
					RobberA.Armor = 500;
					RobberA.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
					RobberA.CanSufferCriticalHits = true;
					RobberA.CanRagdoll = true;
				}
				if (RobberAskill == 2)
				{
					RobberA.Health = 500;
					RobberA.Armor = 700;
					RobberA.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
					RobberA.CanSufferCriticalHits = true;
					RobberA.CanRagdoll = true;
				}
				if (RobberAskill == 3)
				{
					RobberA.Health = 600;
					RobberA.Armor = 800;
					RobberA.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
					RobberA.CanSufferCriticalHits = true;
					RobberA.CanRagdoll = true;
				}
				if (RobberAskill == 4)
				{
					RobberA.Health = 800;
					RobberA.Armor = 800;
					RobberA.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
					RobberA.CanRagdoll = false;
					RobberA.CanSufferCriticalHits = true;
				}
				if (RobberAskill == 5)
				{
					RobberA.Health = 1100;
					RobberA.Armor = 1000;
					RobberA.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
					RobberA.CanRagdoll = false;
					RobberA.CanSufferCriticalHits = true;
				}
				if (RobberAskill == 6)
				{
					RobberA.Health = 1300;
					RobberA.Armor = 1400;
					RobberA.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
					RobberA.CanRagdoll = false;
					RobberA.CanSufferCriticalHits = false;
				}
				if (RobberAskill == 7)
				{
					RobberA.Health = 1600;
					RobberA.Armor = 1800;
					RobberA.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
					RobberA.CanRagdoll = false;
					RobberA.CanSufferCriticalHits = false;
				}
			}
			if (RobberB != null && RobberBskill != 0)
			{
				if (RobberBskill != 7)
				{
					RobberB = GlobalHelper.CreatePed(RequestPed(RobberBSkin), new Vector3(711.889f, -977.887f, 24f), 168f);
					Setoutfit(RobberB, RobberB_C);
					SetOutift(RobberB, RobberB_C);
				}
				if (RobberBskill == 7)
				{
					RobberB = GlobalHelper.CreatePed(PedHash.FreemodeMale01, new Vector3(711.889f, -977.887f, 24f), 168f);
					Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
					Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
					Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
					Ped robberB = RobberB;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 1, 91, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 4, 84, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 8, 97, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 11, 186, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberB, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_USING_ACTION_MODE, robberB, true, -1, 0);
					Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, robberB, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
					Function.Call(Hash.SET_PED_STRAFE_CLIPSET, robberB, "MOVE_STRAFE_BALLISTIC");
					Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, robberB, 1429513766);
				}
				if (RobberBskill == 1)
				{
					RobberB.Health = 300;
					RobberB.Armor = 500;
					RobberB.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
					RobberB.CanRagdoll = true;
					RobberB.CanSufferCriticalHits = true;
				}
				if (RobberBskill == 2)
				{
					RobberB.Health = 500;
					RobberB.Armor = 700;
					RobberB.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
					RobberB.CanRagdoll = true;
					RobberB.CanSufferCriticalHits = true;
				}
				if (RobberBskill == 3)
				{
					RobberB.Health = 600;
					RobberB.Armor = 800;
					RobberB.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
					RobberB.CanRagdoll = true;
					RobberB.CanSufferCriticalHits = true;
				}
				if (RobberBskill == 4)
				{
					RobberB.Health = 800;
					RobberB.Armor = 800;
					RobberB.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
					RobberB.CanRagdoll = false;
					RobberB.CanSufferCriticalHits = true;
				}
				if (RobberBskill == 5)
				{
					RobberB.Health = 1100;
					RobberB.Armor = 1000;
					RobberB.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
					RobberB.CanRagdoll = false;
					RobberB.CanSufferCriticalHits = true;
				}
				if (RobberBskill == 6)
				{
					RobberB.Health = 1300;
					RobberB.Armor = 1400;
					RobberB.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
					RobberB.CanRagdoll = false;
					RobberB.CanSufferCriticalHits = false;
				}
				if (RobberBskill == 7)
				{
					RobberB.Health = 1600;
					RobberB.Armor = 1800;
					RobberB.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
					RobberB.CanRagdoll = false;
					RobberB.CanSufferCriticalHits = false;
				}
			}
			if (RobberC != null && RobberCskill != 0)
			{
				if (RobberCskill != 7)
				{
					RobberC = GlobalHelper.CreatePed(RequestPed(RobberCSkin), new Vector3(709.845f, -977.909f, 24f), 168f);
					Setoutfit(RobberC, RobberC_C);
					SetOutift(RobberC, RobberC_C);
				}
				if (RobberCskill == 7)
				{
					RobberC = GlobalHelper.CreatePed(PedHash.FreemodeMale01, new Vector3(709.845f, -977.909f, 24f), 168f);
					Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
					Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
					Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
					Ped robberC = RobberC;
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 1, 91, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 3, 46, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 4, 84, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 8, 97, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 11, 186, 10, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, robberC, 12, 0, 0, 1);
					Function.Call(Hash.SET_PED_USING_ACTION_MODE, robberC, true, -1, 0);
					Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, robberC, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
					Function.Call(Hash.SET_PED_STRAFE_CLIPSET, robberC, "MOVE_STRAFE_BALLISTIC");
					Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, robberC, 1429513766);
				}
				if (RobberCskill == 1)
				{
					RobberC.Health = 300;
					RobberC.Armor = 500;
					RobberC.Weapons.Give(WeaponHash.AssaultRifle, 9999, true, true);
					RobberC.CanRagdoll = true;
					RobberC.CanSufferCriticalHits = true;
				}
				if (RobberBskill == 2)
				{
					RobberC.Health = 500;
					RobberC.Armor = 700;
					RobberC.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
					RobberC.CanRagdoll = true;
					RobberC.CanSufferCriticalHits = true;
				}
				if (RobberCskill == 3)
				{
					RobberC.Health = 600;
					RobberC.Armor = 800;
					RobberC.Weapons.Give(WeaponHash.SpecialCarbine, 9999, true, true);
					RobberC.CanRagdoll = true;
					RobberC.CanSufferCriticalHits = true;
				}
				if (RobberCskill == 4)
				{
					RobberC.Health = 800;
					RobberC.Armor = 800;
					RobberC.Weapons.Give(WeaponHash.CarbineRifleMk2, 9999, true, true);
					RobberC.CanRagdoll = false;
					RobberC.CanSufferCriticalHits = true;
				}
				if (RobberCskill == 5)
				{
					RobberC.Health = 1100;
					RobberC.Armor = 1000;
					RobberC.Weapons.Give(WeaponHash.CombatMG, 9999, true, true);
					RobberC.CanRagdoll = false;
					RobberC.CanSufferCriticalHits = false;
				}
				if (RobberCskill == 6)
				{
					RobberC.Health = 1300;
					RobberC.Armor = 1400;
					RobberC.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
					RobberC.CanRagdoll = false;
					RobberC.CanSufferCriticalHits = false;
				}
				if (RobberCskill == 7)
				{
					RobberC.Health = 1600;
					RobberC.Armor = 1800;
					RobberC.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
					RobberC.CanRagdoll = false;
					RobberC.CanSufferCriticalHits = false;
				}
			}
			if (RobberA != null && RobberA.Exists())
			{
				RobberA.Position = new Vector3(709.845f, -977.909f, 24f);
				Ped robberA2 = RobberA;
				PedGroup pedGroup = Game.Player.Character.PedGroup;
				Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberA2, pedGroup);
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberA2, 100);
				if (robberA2.AttachedBlip != null)
				{
					robberA2.AttachedBlip.Delete();
				}
				robberA2.AddBlip();
				robberA2.AttachedBlip.Sprite = BlipSprite.Enemy;
				robberA2.AttachedBlip.Color = BlipColor.Blue;
				robberA2.AttachedBlip.Name = "Heist Partner";
				RobberA.CanWrithe = false;
				Function.Call(Hash.SET_PED_ALERTNESS, RobberA, 3);
				Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberA, true);
			}
			if (RobberB != null && RobberB.Exists())
			{
				RobberB.Position = new Vector3(711.889f, -977.887f, 24f);
				Ped robberB2 = RobberB;
				PedGroup pedGroup2 = Game.Player.Character.PedGroup;
				Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberB2, pedGroup2);
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberB2, 100);
				if (robberB2.AttachedBlip != null)
				{
					robberB2.AttachedBlip.Delete();
				}
				robberB2.AddBlip();
				robberB2.AttachedBlip.Sprite = BlipSprite.Enemy;
				robberB2.AttachedBlip.Color = BlipColor.Blue;
				robberB2.AttachedBlip.Name = "Heist Partner";
				RobberB.CanWrithe = false;
				Function.Call(Hash.SET_PED_ALERTNESS, RobberB, 3);
				Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberB, true);
			}
			if (RobberC != null && RobberC.Exists())
			{
				RobberC.Position = new Vector3(709.845f, -977.909f, 24f);
				Ped robberC2 = RobberC;
				PedGroup pedGroup3 = Game.Player.Character.PedGroup;
				Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberC2, pedGroup3);
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberC2, 100);
				if (robberC2.AttachedBlip != null)
				{
					robberC2.AttachedBlip.Delete();
				}
				robberC2.AddBlip();
				robberC2.AttachedBlip.Sprite = BlipSprite.Enemy;
				robberC2.AttachedBlip.Color = BlipColor.Blue;
				robberC2.AttachedBlip.Name = "Heist Partner";
				RobberC.CanWrithe = false;
				Function.Call(Hash.SET_PED_ALERTNESS, RobberC, 3);
				Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberC, true);
			}
			if (RobberA != null && RobberA.Exists())
			{
				RobberA.FiringPattern = RobberAFP;
				RobberA.Weapons.Give(WeaponHash.Pistol, 9999, false, true);
				RobberA.Weapons.Give(WeaponHash.MicroSMG, 9999, false, true);
				RobberA.Weapons.Give(WeaponHash.Parachute, 9999, false, true);
				RobberA.DrownsInWater = false;
				RobberA.DiesOnLowHealth = false;
				RobberA.CanFlyThroughWindscreen = false;
				RobberA.CanSufferCriticalHits = false;
				RobberA.CanWrithe = false;
				RobberA.DrownsInSinkingVehicle = false;
				RobberA.DiesInstantlyInWater = false;
				RobberA.CanSwitchWeapons = true;
				RobberA.DrivingSpeed = 200f;
			}
			if (RobberB != null && RobberB.Exists())
			{
				RobberB.FiringPattern = RobberBFP;
				RobberB.Weapons.Give(WeaponHash.Pistol, 9999, false, true);
				RobberB.Weapons.Give(WeaponHash.MicroSMG, 9999, false, true);
				RobberB.Weapons.Give(WeaponHash.Parachute, 9999, false, true);
				RobberB.DrownsInWater = false;
				RobberB.DiesOnLowHealth = false;
				RobberB.CanFlyThroughWindscreen = false;
				RobberB.CanSufferCriticalHits = false;
				RobberB.CanWrithe = false;
				RobberB.DrownsInSinkingVehicle = false;
				RobberB.DiesInstantlyInWater = false;
				RobberB.CanSwitchWeapons = true;
				RobberB.DrivingSpeed = 200f;
			}
			if (RobberC != null && RobberC.Exists())
			{
				RobberC.FiringPattern = RobberCFP;
				RobberC.Weapons.Give(WeaponHash.Pistol, 9999, false, true);
				RobberC.Weapons.Give(WeaponHash.MicroSMG, 9999, false, true);
				RobberC.Weapons.Give(WeaponHash.Parachute, 9999, false, true);
				RobberC.DrownsInWater = false;
				RobberC.DiesOnLowHealth = false;
				RobberC.CanFlyThroughWindscreen = false;
				RobberC.CanSufferCriticalHits = false;
				RobberC.CanWrithe = false;
				RobberC.DrownsInSinkingVehicle = false;
				RobberC.DiesInstantlyInWater = false;
				RobberC.CanSwitchWeapons = true;
				RobberC.DrivingSpeed = 200f;
			}
			hackedDataPad = false;
			FreezeGetawayCar = false;
			FreezeGetawayCar2 = false;
			TriggerNotifyNear = false;
			PlacedBomb = false;
			EscapingCasino = false;
			VaultClosed = true;
			VaultDone = false;
			MetalDetectorOn = true;
			MissionOn = true;
			IsInterior = false;
			BypassMetalDetector = false;
			AlarmsGoingOff = false;
			GotCash = false;
			TriggeredWantedLevelEscape = false;
			IsPlanting = false;
			NotifyOutVault = false;
			CurrentPay = 0;
			ManageRobberyAnims.CurrentPay = 0;
			ManageRobberyAnims.Heist_Average_Pay = (float)(HeistMinPay + HeistMaxPay) / 2f;
			AlarmsGoingOff = false;
			HeadingtoyourAppartment = false;
			SetSewerBomb = false;
			DetonatedSewerBomb = false;
			EnteredNormalCasino = false;
			EnteredOffice = false;
			TriggeredAttack = false;
			NotifyOutVault2 = false;
			InNormalCasino = false;
			OverideAlarm = false;
			AlarmTimer = 0;
			TriggerFullResponce = false;
			ReportedtoGuardA = false;
			ReportedtoGuardB = false;
			NotifyHack = false;
			ExtraVaultOpen = false;
			ExtraValuableType = 0;
			RobbedExtraValuable = false;
			ExtraValuableType2 = 0;
			RobbedExtraValuable2 = false;
			ReportTimer = 0;
			Random random = new Random();
			if (Bank != null)
			{
				Bank.Delete();
			}
			if (GetawayCar != null)
			{
				GetawayCar.Delete();
			}
			try
			{
				if (EnteranceUsed == 0 || EnteranceUsed == 2 || EnteranceUsed == 6 || EnteranceUsed == 7)
				{
					SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars//DiamondHeist_Land.ini");
					VehicleChoosen = SaveCar.VehicleName;
					GetawayCar = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), GetawayCarSpawn, GetawayCarHeading);
					if (GetawayCar.AttachedBlip != null)
					{
						GetawayCar.AttachedBlip.Delete();
					}
					GetawayCar.AddBlip();
					GetawayCar.AttachedBlip.Sprite = BlipSprite.GetawayCar;
					GetawayCar.AttachedBlip.Color = BlipColor.Yellow;
					GetawayCar.AttachedBlip.Name = "Stockade";
					GetawayCar.IsPositionFrozen = false;
					GetawayCar.Repair();
					GetawayCar.IsPositionFrozen = false;
					GetawayCar.IsEngineRunning = true;
					GetawayCar.IsDriveable = true;
					GetawayCar.IsInvincible = true;
					GetawayCar.IsPositionFrozen = true;
					Notification.Show("~y~ Lester ~w~ Get in your getaway car and make your way up to the Casino and I'll breif you on the way");
					Notification.Show("~y~ Lester ~w~ if guards see you they will attempt to arrest you, so be carefull");
				}
				if (EnteranceUsed == 1)
				{
					SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars//DiamondHeist_Special.ini");
					VehicleChoosen = SaveCar.VehicleName;
					GetawayCar = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), GetawayCarSpawn, GetawayCarHeading);
					if (GetawayCar.AttachedBlip != null)
					{
						GetawayCar.AttachedBlip.Delete();
					}
					GetawayCar.AddBlip();
					GetawayCar.AttachedBlip.Sprite = BlipSprite.GetawayCar;
					GetawayCar.AttachedBlip.Color = BlipColor.Yellow;
					GetawayCar.AttachedBlip.Name = "Stockade";
					GetawayCar.IsPositionFrozen = false;
					GetawayCar.Repair();
					GetawayCar.IsPositionFrozen = false;
					GetawayCar.IsEngineRunning = true;
					GetawayCar.IsDriveable = true;
					GetawayCar.IsInvincible = true;
					GetawayCar.IsPositionFrozen = true;
					Notification.Show("~y~ Lester ~w~ Get in the Stockade, and make your way to the Casino Desgised as Groupe6");
					Notification.Show("~y~ Lester ~w~ Ive gotten you a pass to the Loading Bay, Vault Area and Main Office, if a guard sees you on a higher floor, they will arrest you");
				}
				if (EnteranceUsed == 3 || EnteranceUsed == 4)
				{
					SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars//DiamondHeist_Air.ini");
					VehicleChoosen = SaveCar.VehicleName;
					GetawayCar = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(-980.8368f, -2995.735f, 13f), 58f);
					if (GetawayCar.AttachedBlip != null)
					{
						GetawayCar.AttachedBlip.Delete();
					}
					GetawayCar.AddBlip();
					GetawayCar.AttachedBlip.Sprite = BlipSprite.Helicopter;
					GetawayCar.AttachedBlip.Color = BlipColor.Yellow;
					GetawayCar.AttachedBlip.Name = "Getaway Heli";
					GetawayCar.IsPositionFrozen = false;
					GetawayCar.Repair();
					GetawayCar.IsPositionFrozen = false;
					GetawayCar.IsEngineRunning = true;
					GetawayCar.IsDriveable = true;
					GetawayCar.IsInvincible = true;
					GetawayCar.IsPositionFrozen = true;
					Notification.Show("~y~ Lester ~w~ head to LSIA to retrieve your Getaway Heli and make your way to the Casino rooftop after");
					Notification.Show("~y~ Lester ~w~ Your access is though the Hotel, make your way down to the vault, loot the vault, then get out");
				}
				if (EnteranceUsed == 5)
				{
					SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars//DiamondHeist_Special.ini");
					VehicleChoosen = SaveCar.VehicleName;
					GetawayCar = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Burrito2), GetawayCarSpawn, GetawayCarHeading);
					if (GetawayCar.AttachedBlip != null)
					{
						GetawayCar.AttachedBlip.Delete();
					}
					GetawayCar.AddBlip();
					GetawayCar.AttachedBlip.Sprite = BlipSprite.GetawayCar;
					GetawayCar.AttachedBlip.Color = BlipColor.Yellow;
					GetawayCar.AttachedBlip.Name = "Bugstars Van";
					GetawayCar.IsPositionFrozen = false;
					GetawayCar.Repair();
					GetawayCar.IsPositionFrozen = false;
					GetawayCar.IsEngineRunning = true;
					GetawayCar.IsDriveable = true;
					GetawayCar.IsInvincible = true;
					GetawayCar.IsPositionFrozen = true;
					Notification.Show("~y~ Lester ~w~ Get in the Bugstars Van, and make your way to the Casino Desgised as Bugstars Exterminator");
					Notification.Show("~y~ Lester ~w~ Ive gotten you a pass to the Loading Bay & Vault Area, if a guard sees you on a higher floor, they will arrest you");
				}
			}
			catch
			{
				Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
			}
			if (EnteranceUsed == 0)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(new Vector3(925.7f, 45.8f, 80.3f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Main)";
			}
			if (EnteranceUsed == 1)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(new Vector3(932.7f, -4.23f, 78.7f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Loading Bay)";
			}
			if (EnteranceUsed == 2)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(new Vector3(992.7f, -141.7f, 34f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Sewer)";
			}
			if (EnteranceUsed == 3)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(new Vector3(953.7f, 3.98f, 110f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Roof)";
			}
			if (EnteranceUsed == 4)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(new Vector3(959.6f, 32.2f, 119f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Roof)";
			}
			if (EnteranceUsed == 5)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(new Vector3(932.7f, -4.23f, 78.7f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Loading Bay)";
			}
			if (EnteranceUsed == 6)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(new Vector3(925.7f, 45.8f, 80.3f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Main)";
			}
			if (EnteranceUsed == 7)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(new Vector3(993.13f, 76.7f, 80f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Waste Disposal)";
			}
			if (EnteranceUsed == 8)
			{
				if (Bank != null)
				{
					Bank.Delete();
				}
				Bank = GlobalHelper.CreateBlip(SideDoorEnterance);
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Color = BlipColor.Yellow;
				Bank.Name = "Diamond Casino Enterance (Offices Side Door)";
			}
			if (EnteranceUsed == 1)
			{
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					Ped character = Game.Player.Character;
					Function.Call(Hash.SET_PED_PROP_INDEX, character, 0, 5, 0, false);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 3, 11, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 4, 14, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 6, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 8, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 11, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character, 12, 0, 0, 1);
				}
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
					Ped character2 = Game.Player.Character;
					Function.Call(Hash.SET_PED_PROP_INDEX, character2, 0, 6, 0, false);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 3, 8, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 4, 14, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 5, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 6, 10, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 8, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 10, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 11, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character2, 12, 0, 0, 1);
				}
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					Notification.Show("Franklin Has No Groupe6 Outfit");
				}
				Game.Player.Character.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
			}
			if (EnteranceUsed == 5 && ExitUsed == 4)
			{
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					Ped character3 = Game.Player.Character;
					Function.Call(Hash.SET_PED_PROP_INDEX, character3, 0, 6, 0, false);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 1, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 3, 12, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 4, 11, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 5, 6, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 6, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 8, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 10, 4, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 11, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character3, 12, 0, 0, 1);
				}
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
					Notification.Show("Trevpr Has No Bugstar Outfit Outfit");
				}
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					Ped character4 = Game.Player.Character;
					Function.Call(Hash.SET_PED_PROP_INDEX, character4, 0, 15, 0, false);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 0, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 1, 4, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 2, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 3, 1, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 4, 1, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 5, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 6, 8, 1, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 7, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 8, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 10, 2, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 11, 0, 0, 1);
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 12, 0, 0, 1);
				}
				Game.Player.Character.Weapons.Give(WeaponHash.CeramicPistol, 9999, true, true);
			}
			Ped character5 = Game.Player.Character;
			if (Game.Player.Character.Model == PedHash.Trevor)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 9, 1, 0, 1);
			}
			if (Game.Player.Character.Model == PedHash.Michael)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 9, 2, 0, 1);
			}
			if (Game.Player.Character.Model == PedHash.Franklin)
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character5, 9, 6, 0, 1);
			}
			if (RobberA != null)
			{
				RobberA.Position = RobberA_StartMarker;
			}
			if (RobberB != null)
			{
				RobberB.Position = RobberB_StartMarker;
			}
			if (RobberC != null)
			{
				RobberC.Position = RobberC_StartMarker;
			}
			HackedWasteDisposalDoor = false;
			if (MDBox1 != null)
			{
				MDBox1.Delete();
			}
			if (MDBox2 != null)
			{
				MDBox2.Delete();
			}
			if (MDBox3 != null)
			{
				MDBox3.Delete();
			}
			MDBox1 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_fuse_box_01a"), new Vector3(2545.732f, -276.79f, -57.82f), new Vector3(0f, 0f, -178.76f), false, false);
			MDBox1.IsPositionFrozen = true;
			MDBox2 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_fuse_box_01a"), new Vector3(2525.57f, -267.8022f, -57.79f), new Vector3(0f, 0f, -179.07f), false, false);
			MDBox2.IsPositionFrozen = true;
			MDBox3 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_fuse_box_01a"), new Vector3(2542.522f, -265.155f, -57.68f), new Vector3(0f, 0f, 89.72f), false, false);
			MDBox3.IsPositionFrozen = true;
		}

		public static void LoadIniFile(string iniName)
		{
			try
			{
				Config = ScriptSettings.Load(iniName);
				HeistStage = Config.GetValue("TheDiamondHiest", "HeistStage", HeistStage);
				EnteranceUsed = Config.GetValue("TheDiamondHiest", "EnteranceUsed", EnteranceUsed);
				ExitUsed = Config.GetValue("TheDiamondHiest", "ExitUsed", ExitUsed);
				HeistMinPay = Config.GetValue("TheDiamondHiest", "HeistMinPay", HeistMinPay);
				HeistMaxPay = Config.GetValue("TheDiamondHiest", "HeistMaxPay", HeistMaxPay);
			}
			catch (Exception)
			{
				Notification.Show("~r~Error~w~: Heist.ini Failed To Load.");
			}
		}

		private void GetLocation()
		{
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
			NativeListItem<dynamic> L = new NativeListItem<object>("Enterance : ", list.ToArray());
			Heists2.Add(L);
			NativeListItem<dynamic> E = new NativeListItem<object>("Exit : ", list2.ToArray());
			Heists2.Add(E);
			NativeItem item2 = new NativeItem("-----------------------------------------------------------");
			Heists2.Add(item2);
			NativeItem nativeItem = new NativeItem("Set Enterance & Exit");
			Heists2.Add(nativeItem);
			nativeItem.Description = "Enterance & Exit determines what Gear is needed for the Heist";
			nativeItem.Activated += delegate
			{
				if (E.SelectedIndex == 4 && L.SelectedIndex != 5)
				{
					Notification.Show("~y~ Lester ~w~ : if Choosing Bugstars for Exit, Enterance and Exit have to be Bugstars!");
				}
				if (E.SelectedIndex == 1 && L.SelectedIndex != 2)
				{
					Notification.Show("~y~ Lester ~w~ : if Choosing Sewer for Exit, Enterance and Exit have to be Sewer!");
				}
				if (E.SelectedIndex == 1 && L.SelectedIndex == 2)
				{
					LocAt = 7;
					EnteranceUsed = L.SelectedIndex;
					Config.SetValue("TheDiamondHiest", "EnteranceUsed", EnteranceUsed);
					ExitUsed = E.SelectedIndex;
					Config.SetValue("TheDiamondHiest", "ExitUsed", ExitUsed);
					Config.Save();
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
						Game.Player.Character.Position = CheckAppartmentPos();
						modLemonUIObjectPool2.HideAll();
					}
				}
				if (E.SelectedIndex == 4 && L.SelectedIndex == 5)
				{
					LocAt = 7;
					EnteranceUsed = L.SelectedIndex;
					Config.SetValue("TheDiamondHiest", "EnteranceUsed", EnteranceUsed);
					ExitUsed = E.SelectedIndex;
					Config.SetValue("TheDiamondHiest", "ExitUsed", ExitUsed);
					Config.Save();
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
						Game.Player.Character.Position = CheckAppartmentPos();
						modLemonUIObjectPool2.HideAll();
					}
				}
				if (E.SelectedIndex != 1)
				{
					LocAt = 7;
					EnteranceUsed = L.SelectedIndex;
					Config.SetValue("TheDiamondHiest", "EnteranceUsed", EnteranceUsed);
					ExitUsed = E.SelectedIndex;
					Config.SetValue("TheDiamondHiest", "ExitUsed", ExitUsed);
					Config.Save();
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
						Game.Player.Character.Position = CheckAppartmentPos();
						modLemonUIObjectPool2.HideAll();
					}
				}
				Notification.Show("~y~ Lester ~w~ : Enterance and Exit have been set");
			};
			L.ItemChanged += delegate
			{
				if (L.SelectedIndex == 0)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(910.9f, 43.3f, 82.5f), new Vector3(0f, 0f, -80f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (L.SelectedIndex == 1)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(918.4f, -37.6f, 82.5f), new Vector3(0f, 0f, -28f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (L.SelectedIndex == 2)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(1039f, -285.2f, 52.3f), new Vector3(0f, 0f, 18f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (L.SelectedIndex == 3)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(961.4f, -17.2f, 113.4f), new Vector3(0f, 0f, 30f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (L.SelectedIndex == 4)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(949.9f, 30.6f, 121.2f), new Vector3(0f, 0f, -85f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (L.SelectedIndex == 5)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(918.4f, -37.6f, 82.5f), new Vector3(0f, 0f, -28f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (L.SelectedIndex == 1)
				{
					Notification.Show("~y~ Lester ~w~ Choosing this Enterance requires a Stockade to be stolen in Setup 2");
				}
				if (L.SelectedIndex == 3)
				{
					Notification.Show("~y~ Lester ~w~ Choosing this Enterance requires a VIP pass to be stolen in Setup 2");
					Notification.Show("~y~ Lester ~w~ Choosing this Enterance requires a Helicopter to be stolen in Setup 3");
				}
				if (L.SelectedIndex == 4)
				{
					Notification.Show("~y~ Lester ~w~ Choosing this Enterance requires a VIP pass to be stolen in Setup 2");
					Notification.Show("~y~ Lester ~w~ Choosing this Enterance requires a Helicopter to be stolen in Setup 3");
				}
				if (L.SelectedIndex == 5)
				{
					Notification.Show("~y~ Lester ~w~ Choosing this Enterance requires a Bugstars Van to be stolen in Setup 3");
				}
			};
			E.ItemChanged += delegate
			{
				if (E.SelectedIndex == 0)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(910.9f, 43.3f, 82.5f), new Vector3(0f, 0f, -80f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (E.SelectedIndex == 1)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(1039f, -285.2f, 52.3f), new Vector3(0f, 0f, 18f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (E.SelectedIndex == 2)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(961.4f, -17.2f, 113.4f), new Vector3(0f, 0f, 30f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (E.SelectedIndex == 3)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(949.9f, 30.6f, 121.2f), new Vector3(0f, 0f, -85f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (L.SelectedIndex == 4)
				{
					if (HeistStartCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						HeistStartCam.IsActive = false;
						HeistStartCam.Delete();
						Game.Player.Character.IsPositionFrozen = false;
						Game.Player.CanControlCharacter = true;
					}
					HeistStartCam = GlobalHelper.CreateCamera(new Vector3(918.4f, -37.6f, 82.5f), new Vector3(0f, 0f, -28f), 60f);
					HeistStartCam.IsActive = true;
					HeistStartCam.FarClip = 2000f;
					Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
					Game.Player.Character.IsPositionFrozen = true;
					Game.Player.CanControlCharacter = false;
					World.RenderingCamera = HeistStartCam;
				}
				if (E.SelectedIndex == 1)
				{
					Notification.Show("~y~ Lester ~w~ Choosing this Exit requires Enterace to be the same");
				}
				if (E.SelectedIndex == 2)
				{
					Notification.Show("~y~ Lester ~w~ Choosing this Exit requires a Helicopter to be stolen in Setup 3");
				}
				if (E.SelectedIndex == 3)
				{
					Notification.Show("~y~ Lester ~w~ Choosing this Exit requires a Helicopter to be stolen in Setup 3");
				}
				if (E.SelectedIndex == 4)
				{
					Notification.Show("~y~ Lester ~w~ Choosing this Exit requires a Bugstars Van to be stolen in Setup 3");
				}
			};
		}

		public void StartDiamondCasino()
		{
			SetuptHeist();
		}

		public static void SetuptHeist()
		{
			Notification.Show("~b~Starting/Continuing Heist : The Diamond Casino Heist ~w~");
			if (HeistStage == 1)
			{
				foreach (Prop item in PaintingHolder)
				{
					if (item != null)
					{
						item.Delete();
					}
				}
				foreach (Ped ped in Peds)
				{
					if (ped != null)
					{
						ped.Delete();
					}
				}
				foreach (Vehicle vehicle in Vehicles)
				{
					if (vehicle != null)
					{
						vehicle.Delete();
					}
				}
				if (Bank != null)
				{
					Bank.Delete();
				}
				if (Lester != null)
				{
					Lester.Delete();
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
				Lester = GlobalHelper.CreatePed(PedHash.LesterCrest, LesterSpawn);
				Ped lester = Lester;
				PedGroup pedGroup = Game.Player.Character.PedGroup;
				Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, lester, pedGroup);
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, lester, 100);
				Bank = GlobalHelper.CreateBlip(new Vector3(921.5f, 49.3f, 79f));
				Bank.Sprite = BlipSprite.Jewel;
				Bank.Scale = 1.25f;
				Bank.Name = "Possible Enterance/Exit";
				Bank.ShowRoute = true;
				LocAt = 1;
				MissionOn = true;
				Notification.Show("~y~ Lester ~w~ :  Go take a look at the Casino so we know what were dealing with");
			}
			if (HeistStage == 2)
			{
				foreach (Prop item2 in PaintingHolder)
				{
					if (item2 != null)
					{
						item2.Delete();
					}
				}
				if (Bank != null)
				{
					Bank.Delete();
				}
				if (Lester != null)
				{
					Lester.Delete();
				}
				if (MeetPed != null)
				{
					MeetPed.Delete();
				}
				foreach (Ped aircarftCarrierPed in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed != null)
					{
						aircarftCarrierPed.Delete();
					}
				}
				foreach (Vehicle aircraftCarrierVehicle in AircraftCarrierVehicles)
				{
					if (aircraftCarrierVehicle != null)
					{
						aircraftCarrierVehicle.Delete();
					}
				}
				DiamdondCasinoSmartGuards.RemoveAIGuards();
				foreach (Ped aircarftCarrierPed2 in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed2 != null)
					{
						aircarftCarrierPed2.Delete();
					}
				}
				foreach (Vehicle aircraftCarrierVehicle2 in AircraftCarrierVehicles)
				{
					if (aircraftCarrierVehicle2 != null)
					{
						aircraftCarrierVehicle2.Delete();
					}
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
				foreach (Ped ped2 in Peds)
				{
					if (ped2 != null)
					{
						ped2.Delete();
					}
				}
				foreach (Vehicle vehicle2 in Vehicles)
				{
					if (vehicle2 != null)
					{
						vehicle2.Delete();
					}
				}
				if (Crate_1 != null)
				{
					Crate_1.Delete();
				}
				if (Crate_2 != null)
				{
					Crate_2.Delete();
				}
				foreach (Vehicle item3 in Veh)
				{
					if (item3 != null)
					{
						item3.Delete();
					}
				}
				GotC4_1 = false;
				GotC4_2 = false;
				TriggerUINotify = false;
				GotC4_1 = false;
				GotC4_2 = false;
				returningtofactroy = false;
				isDriving = false;
				SecuredAsset_C4 = false;
				SecuredAsset_Stockade = false;
				SecuredAsset_Vip = false;
				Crate_1 = GlobalHelper.CreateProp("p_secret_weapon_02", new Vector3(461.5811f, -3257.675f, 5.1f), true, false);
				Crate_1.IsPositionFrozen = true;
				if (Crate_1.AttachedBlip != null)
				{
					Crate_1.AttachedBlip.Delete();
				}
				Crate_1.AddBlip();
				Crate_1.AttachedBlip.Sprite = BlipSprite.SatchelCharge;
				Crate_1.AttachedBlip.Color = BlipColor.Yellow;
				Crate_1.AttachedBlip.Name = "C4 Crate";
				Crate_2 = GlobalHelper.CreateProp("p_secret_weapon_02", new Vector3(461.5811f, -3245.603f, 5.1f), true, false);
				Crate_2.IsPositionFrozen = true;
				if (Crate_2.AttachedBlip != null)
				{
					Crate_2.AttachedBlip.Delete();
				}
				Crate_2.AddBlip();
				Crate_2.AttachedBlip.Sprite = BlipSprite.SatchelCharge;
				Crate_2.AttachedBlip.Color = BlipColor.Yellow;
				Crate_2.AttachedBlip.Name = "C4 Crate";
				Crate_1.AttachedBlip.ShowRoute = true;
				Crate_2.AttachedBlip.ShowRoute = true;
				Notification.Show("~y~ Lester ~w~ : Unfortunatly i have run into a sort of barrier, the Vault, it uses Electro magnetic technology, meaning that a Hacker, or Thirmite will not work on the Vault, at least not alone");
				Script.Wait(1000);
				SecuredAsset_C4 = false;
				SecuredAsset_Stockade = false;
				SecuredAsset_Vip = false;
				LoadIniFile("scripts//HKH_SinglePlayerHeists//Main.ini");
				if (EnteranceUsed == 1)
				{
					DiamdondCasinoSmartGuards.RemoveAIGuards();
					foreach (Ped aircarftCarrierPed3 in AircarftCarrierPeds)
					{
						if (aircarftCarrierPed3 != null)
						{
							aircarftCarrierPed3.Delete();
						}
					}
					foreach (Vehicle aircraftCarrierVehicle3 in AircraftCarrierVehicles)
					{
						if (aircraftCarrierVehicle3 != null)
						{
							aircraftCarrierVehicle3.Delete();
						}
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
					Notification.Show("~y~ Lester ~w~ : You will also need to Steal one of the 3, Stockades patroling around LS");
					StockadeA = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), new Vector3(791.5f, -1388.8f, 27.3f), -176f);
					StockadeB = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), StockadeA.GetOffsetPosition(new Vector3(0f, -20f, 0f)), -176f);
					StockadeC = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), StockadeA.GetOffsetPosition(new Vector3(0f, -10f, 0f)), -176f);
					AircraftCarrierVehicles.Add(StockadeA);
					AircraftCarrierVehicles.Add(StockadeB);
					AircraftCarrierVehicles.Add(StockadeC);
					foreach (Vehicle aircraftCarrierVehicle4 in AircraftCarrierVehicles)
					{
						if (aircraftCarrierVehicle4 != null)
						{
							aircraftCarrierVehicle4.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.Armoured01SMM));
							AircarftCarrierPeds.Add(aircraftCarrierVehicle4.GetPedOnSeat(VehicleSeat.Driver));
							aircraftCarrierVehicle4.CreatePedOnSeat(VehicleSeat.LeftRear, RequestModel(PedHash.Armoured01SMM));
							AircarftCarrierPeds.Add(aircraftCarrierVehicle4.GetPedOnSeat(VehicleSeat.LeftRear));
							aircraftCarrierVehicle4.CreatePedOnSeat(VehicleSeat.Passenger, RequestModel(PedHash.Armoured01SMM));
							AircarftCarrierPeds.Add(aircraftCarrierVehicle4.GetPedOnSeat(VehicleSeat.Passenger));
							aircraftCarrierVehicle4.CreatePedOnSeat(VehicleSeat.RightRear, RequestModel(PedHash.Armoured01SMM));
							AircarftCarrierPeds.Add(aircraftCarrierVehicle4.GetPedOnSeat(VehicleSeat.RightRear));
							aircraftCarrierVehicle4.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(aircraftCarrierVehicle4, PointA, 60f, 80f);
							if (aircraftCarrierVehicle4.AttachedBlip != null)
							{
								aircraftCarrierVehicle4.AttachedBlip.Delete();
							}
							aircraftCarrierVehicle4.AddBlip();
							aircraftCarrierVehicle4.AttachedBlip.Sprite = BlipSprite.ArmoredTruck;
							aircraftCarrierVehicle4.AttachedBlip.Color = BlipColor.Yellow;
							aircraftCarrierVehicle4.AttachedBlip.Name = "Steal A Stockade";
							aircraftCarrierVehicle4.AttachedBlip.IsShortRange = true;
						}
					}
					foreach (Ped aircarftCarrierPed4 in AircarftCarrierPeds)
					{
						if (aircarftCarrierPed4 != null)
						{
							aircarftCarrierPed4.IsPersistent = true;
							aircarftCarrierPed4.Weapons.Give(WeaponHash.CarbineRifle, 9999, false, true);
							aircarftCarrierPed4.Health = 200;
							aircarftCarrierPed4.Armor = 200;
							RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "SECURITY_GUARD");
							Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, aircarftCarrierPed4, relationshipGroup);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, aircarftCarrierPed4, 0, 0);
							aircarftCarrierPed4.RelationshipGroup = relationshipGroup;
						}
					}
				}
				if (EnteranceUsed == 3 || EnteranceUsed == 4)
				{
					Notification.Show("~y~ Lester ~w~ :  Because you are using a Rooftop enternace we will need a electronic VIP pass");
					Notification.Show("~y~ Lester ~w~ :  head up to the Casino and see if you can aquire one there");
					if (Bank != null)
					{
						Bank.Delete();
					}
					HeistMissson = 1;
					Bank = GlobalHelper.CreateBlip(new Vector3(962.9f, 189.9f, 80f));
					Bank.Sprite = BlipSprite.Firewall;
					Bank.Scale = 1.25f;
					Bank.Name = "Search Point";
					Bank.ShowRoute = true;
					MissionOn = true;
				}
				if (EnteranceUsed == 5)
				{
					int exitUsed = ExitUsed;
					int num = 4;
				}
				MissionOn = true;
			}
			if (HeistStage == 3)
			{
				foreach (Prop item4 in PaintingHolder)
				{
					if (item4 != null)
					{
						item4.Delete();
					}
				}
				if (EnteranceUsed != 3 && EnteranceUsed != 4 && ExitUsed != 2 && ExitUsed != 3 && EnteranceUsed != 5)
				{
					if (DeliveryPoint != null)
					{
						DeliveryPoint.Delete();
					}
					DeliveryPoint = GlobalHelper.CreateBlip(DeliveryLoc);
					DeliveryPoint.Sprite = BlipSprite.CriminalCarstealLost;
					DeliveryPoint.Color = BlipColor.Yellow;
					DeliveryPoint.Name = "Heist Getaway Car Delivery Point";
					DeliveryPoint.ShowRoute = true;
					Notification.Show("~y~ Lester ~w~ : Ok we are all set for the Heist, but we need a getaway car, you will need to provide a car");
					Script.Wait(2000);
					Notification.Show("~y~ Lester ~w~ : You can use any car, even a bike, but it all depends on how many gun men you will be taking");
					Script.Wait(2000);
					Notification.Show("~y~ Lester ~w~ : Deliver the vehicle to the Marked location on your map");
					LookingForVehicle = true;
					MissionOn = true;
				}
				if (EnteranceUsed == 3 || EnteranceUsed == 4 || ExitUsed == 2 || (ExitUsed == 3 && ExitUsed != 4 && EnteranceUsed != 5))
				{
					if (DeliveryPoint != null)
					{
						DeliveryPoint.Delete();
					}
					DeliveryPoint = GlobalHelper.CreateBlip(new Vector3(-987.7f, -2992.6f, 14f));
					DeliveryPoint.Sprite = BlipSprite.Helicopter;
					DeliveryPoint.Color = BlipColor.Yellow;
					DeliveryPoint.Name = "Heist Getaway Heli Delivery Point";
					DeliveryPoint.ShowRoute = true;
					Notification.Show("~y~ Lester ~w~ : Ok we are all set for the Heist, but we need a getaway car, because you choose Rooftop Enterance or Exit");
					Script.Wait(2000);
					Notification.Show("~y~ Lester ~w~ : You will need to provide a Helicopter, you can use any Helicopter can be weaponized or not, up to you");
					Script.Wait(2000);
					Notification.Show("~y~ Lester ~w~ : Deliver the vehicle to the Marked location on your map");
					LookingForVehicle = true;
					MissionOn = true;
				}
				if (EnteranceUsed == 5 && ExitUsed == 4)
				{
					if (DeliveryPoint != null)
					{
						DeliveryPoint.Delete();
					}
					DeliveryPoint = GlobalHelper.CreateBlip(DeliveryLoc);
					DeliveryPoint.Sprite = BlipSprite.CriminalCarstealLost;
					DeliveryPoint.Color = BlipColor.Yellow;
					DeliveryPoint.Name = "Heist Getaway Car Delivery Point";
					DeliveryPoint.ShowRoute = true;
					Notification.Show("~y~ Lester ~w~ : Ok we are all set for the Heist, but we need a getaway car, you will need to provide a Bugstars Van");
					Script.Wait(2000);
					Notification.Show("~y~ Lester ~w~ : Deliver the vehicle to the Marked location on your map");
					LookingForVehicle = true;
					MissionOn = true;
				}
			}
			if (HeistStage != 4)
			{
				return;
			}
			foreach (Prop item5 in PaintingHolder)
			{
				if (item5 != null)
				{
					item5.Delete();
				}
			}
			Notification.Show("~b~Setup 4 ~w~");
			MaxPay = (int)CalculateMaxPay(HeistMinPay, HeistMaxPay);
			Game.Player.Character.Position = new Vector3(405.9228f, -954.1149f, -99.6627f);
			Script.Wait(1);
			Game.Player.Character.Position = new Vector3(400.4847f, -958.757f, -100f);
			Game.Player.Character.Rotation = new Vector3(0f, 0f, -73f);
			mainMenu.Visible = !mainMenu.Visible;
			CameraON = true;
			HeistStartCam = GlobalHelper.CreateCamera(new Vector3(404.3f, -958.6f, -99f), new Vector3(0f, 0f, 85f), 60f);
			HeistStartCam.IsActive = true;
			HeistStartCam.FarClip = 2000f;
			Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
			Game.Player.Character.IsPositionFrozen = true;
			Game.Player.CanControlCharacter = false;
			World.RenderingCamera = HeistStartCam;
		}

		public static float CalculateMaxPay(int min, int max)
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

		public void SetupSwat()
		{
			foreach (Ped swatPed in SwatPeds)
			{
				if (swatPed != null)
				{
					swatPed.Delete();
				}
			}
			foreach (Vehicle swatVehicle in SwatVehicles)
			{
				if (swatVehicle != null)
				{
					swatVehicle.Delete();
				}
			}
			Vehicle item = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(899.4f, 8f, 78.5f), -34f);
			SwatVehicles.Add(item);
			item = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(927.2f, 68.5f, 79.3f), -111f);
			SwatVehicles.Add(item);
			item = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(874.9f, 19.9f, 78.4f), -43f);
			SwatVehicles.Add(item);
			item = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(953.2f, -27.9f, 78.4f), -32f);
			SwatVehicles.Add(item);
			item = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(902.2f, 15.3f, 78.6f), 152f);
			SwatVehicles.Add(item);
			item = GlobalHelper.CreateVehicle(VehicleHash.Riot, new Vector3(880.2f, -86f, 79.4f), -135f);
			SwatVehicles.Add(item);
			Ped item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(898.9f, 10.6f, 78.8f), 58.9f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(897.2f, 8.24f, 78.8f), 58.9f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(902.6f, 8.79f, 78.8f), -121f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(901.3f, 6.9f, 78.8f), -122f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(901.35f, 6.9f, 78.8f), -122f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(900f, 4.9f, 78.8f), -122f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(899.2f, 3.5f, 78.8f), -122f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(952.5f, -25.7f, 78.7f), 58f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(951.4f, -27.3f, 78.7f), 58f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(950.4f, -29f, 78.8f), 58f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(953f, -24.4f, 78.8f), 58f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(927.6f, 66.2f, 79.9f), 168f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(926f, 66.7f, 79.9f), 160f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(924.5f, 67.2f, 80f), 163f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(880.2f, -85.115f, 78.8f), -30f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(881.5f, -85.9f, 78.8f), -33f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(879.7f, -84.8f, 78.7f), -29f);
			SwatPeds.Add(item2);
			item2 = GlobalHelper.CreatePed(PedHash.Swat01SMY, new Vector3(878.1f, -84f, 78.7f), -29f);
			SwatPeds.Add(item2);
			if (SwatPeds.Count > 0)
			{
				foreach (Ped swatPed2 in SwatPeds)
				{
					DiamdondCasinoSmartGuards.SpawnGroupsUse.Add(new SmartGuards.SmartAiV2(swatPed2, SmartGuards.EnemyGroup.Swat));
				}
			}
			SmartGuards.ActiveGroups.Clear();
			SmartGuards.ActiveGroups.Add(SmartGuards.EnemyGroup.Diamond_Casino_Security);
			SmartGuards.ActiveGroups.Add(SmartGuards.EnemyGroup.Swat);
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.None;
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
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
				Script.Wait(1);
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
				Script.Wait(1);
			}
			Model model = new Model("hei_prop_heist_drill");
			model.Request(10000);
			Prop prop = GlobalHelper.CreateProp(RequestModel("hei_prop_heist_drill"), Game.Player.Character.Position, false, false);
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
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, 0f);
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
				GTA.UI.Screen.ShowSubtitle("Loading Scaleform 'DRILLING'");
				scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
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
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, 0f);
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
				scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
				GTA.UI.Screen.ShowSubtitle("Loading Scaleform 'DRILLING'");
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
			Notification.Show("Casino Thermal Drill 11");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(0);
			}
			Notification.Show("Casino Thermal Drill 12");
			if (Drill != null)
			{
				Drill.Delete();
			}
			Notification.Show("Casino Thermal Drill 13");
			Drill = GlobalHelper.CreateProp(RequestModel("ch_prop_laserdrill_01a"), new Vector3(0f, 0f, 0f), false, false);
			Drill.IsPositionFrozen = true;
			Drill.IsCollisionEnabled = false;
			Drill.IsInvincible = true;
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, Drill, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 0, 2, 1);
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Notification.Show("Casino Thermal Drill 14");
			Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, 0f);
			firsttime = true;
			Notification.Show("Casino Thermal Drill 15");
		}

		public void ShowClick()
		{
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int num = Convert.ToInt32(absoluteX / 2f);
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 24, 0), "Grab");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void SetupHeiststuff()
		{
			SmartGuards.CurrentActiveGroup = SmartGuards.EnemyGroup.Diamond_Casino_Security;
			Surrendered = false;
			Function.Call(Hash.REMOVE_IPL, "vw_int_placement_vw_interior_0_dlc_casino_main_milo_");
			Function.Call(Hash.REMOVE_IPL, "vw_int_placement_vw_interior_2_dlc_casino_garage_milo_");
			Function.Call(Hash.REMOVE_IPL, "vw_int_placement_vw_interior_4_dlc_casino_carpark_milo_");
			Function.Call(Hash.REMOVE_IPL, "vw_casino_carpark");
			Function.Call(Hash.REMOVE_IPL, "vw_casino_main");
			Function.Call(Hash.REMOVE_IPL, "vw_casino_garage");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_0_dlc_casino_main_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_2_dlc_casino_garage_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_int_placement_vw_interior_4_dlc_casino_carpark_milo_");
			Function.Call(Hash.REQUEST_IPL, "vw_casino_carpark");
			Function.Call(Hash.REQUEST_IPL, "vw_casino_main");
			Function.Call(Hash.REQUEST_IPL, "vw_casino_garage");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_9_dlc_casino_shaft_milo_");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_8_dlc_tunnel_milo_");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_6_dlc_casino_vault_milo_");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_5_dlc_casino_loading_milo_");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_4_dlc_casino_hotel_milo_");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_3_dlc_casino_back_milo_");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_2_dlc_plan_milo_");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_1_dlc_arcade_milo_");
			Function.Call(Hash.REMOVE_IPL, "ch_int_placement_ch_interior_0_dlc_casino_heist_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_9_dlc_casino_shaft_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_8_dlc_tunnel_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_6_dlc_casino_vault_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_5_dlc_casino_loading_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_4_dlc_casino_hotel_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_3_dlc_casino_back_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_2_dlc_plan_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_1_dlc_arcade_milo_");
			Function.Call(Hash.REQUEST_IPL, "ch_int_placement_ch_interior_0_dlc_casino_heist_milo_");
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Vault_Door");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "set_vault_dressing");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_cash_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_cash_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_gold_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_gold_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_art_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_art_02");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_diamonds_01");
			Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_diamonds_02");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_Vault_Door");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_cash_01");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_cash_02");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_gold_01");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_gold_02");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_art_01");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_art_02");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_diamonds_01");
			Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num, "Set_vault_diamonds_02");
			if (VaultDoor != null)
			{
				VaultDoor.Delete();
			}
			if (HackConsole != null)
			{
				HackConsole.Delete();
			}
			foreach (Prop prop8 in Props)
			{
				if (prop8 != null)
				{
					prop8.Delete();
				}
			}
			Props.Clear();
			HackConsole = GlobalHelper.CreateProp(RequestModel("ch_prop_fingerprint_scanner_01e"), new Vector3(2464.9f, -276.05f, -70.6f), new Vector3(0f, 0f, 70f), false, false);
			HackConsole.IsPositionFrozen = true;
			if (Vent != null)
			{
				Vent.Delete();
			}
			Prop prop = (Vent = GlobalHelper.CreateProp(RequestModel("prop_roofvent_07a"), new Vector3(2493.1f, -252.6f, -72f), new Vector3(0f, 0f, -90f), true, false));
			Vent.IsPositionFrozen = true;
			foreach (Prop smokeEmitter in SmokeEmitters)
			{
				if (smokeEmitter != null)
				{
					smokeEmitter.Delete();
				}
			}
			foreach (Prop item2 in PaintingHolder)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			foreach (Ped aircarftCarrierPed in AircarftCarrierPeds)
			{
				if (aircarftCarrierPed != null)
				{
					aircarftCarrierPed.Delete();
				}
			}
			foreach (Vehicle aircraftCarrierVehicle in AircraftCarrierVehicles)
			{
				if (aircraftCarrierVehicle != null)
				{
					aircraftCarrierVehicle.Delete();
				}
			}
			SmokeEmitters.Clear();
			ManageRobberyAnims.DeleteLoot();
			AircarftCarrierPeds.Clear();
			AircraftCarrierVehicles.Clear();
			if (WheelProp != null)
			{
				WheelProp.Delete();
			}
			if (WheelProp2 != null)
			{
				WheelProp2.Delete();
			}
			if (BasePlate != null)
			{
				BasePlate.Delete();
			}
			foreach (Ped item3 in MainPed)
			{
				if (item3 != null)
				{
					item3.Delete();
				}
			}
			foreach (Prop table in Tables)
			{
				if (table != null)
				{
					table.Delete();
				}
			}
			foreach (Prop item4 in Tables2)
			{
				if (item4 != null)
				{
					item4.Delete();
				}
			}
			foreach (Ped item5 in SMPed)
			{
				if (item5 != null)
				{
					item5.Delete();
				}
			}
			foreach (Prop prop9 in Props)
			{
				if (prop9 != null)
				{
					prop9.Delete();
				}
			}
			BackupPosSpawned.Clear();
			foreach (Vector3 backupPo in BackupPos)
			{
				BackupPosSpawned.Add(false);
			}
			MainPed.Clear();
			Tables.Clear();
			Tables2.Clear();
			SMPed.Clear();
			Props.Clear();
			if (Prizecar != null)
			{
				Prizecar.Delete();
			}
			AI_Pos.Clear();
			AI_Type.Clear();
			AI_Rot.Clear();
			Script.Wait(1000);
			SpawnProp("ch_prop_ch_metal_detector_01a", new Vector3(2525.6f, -270f, -59.8f), new Vector3(0f, 0f, -90f));
			SpawnProp("ch_prop_ch_metal_detector_01a", new Vector3(2525.6f, -268.4f, -59.8f), new Vector3(0f, 0f, -90f));
			SpawnProp("ch_prop_baggage_scanner_01a", new Vector3(2525.6f, -267.3f, -59.8f), new Vector3(0f, 0f, -90f));
			SpawnProp("ch_prop_ch_sec_cabinet_04a", new Vector3(2525.6f, -271.6f, -59.8f), new Vector3(0f, 0f, -90f));
			SpawnProp("ch_prop_ch_sec_cabinet_04a", new Vector3(2525.9f, -271.6f, -59.8f), new Vector3(0f, 0f, 90f));
			ShutdownMetalDetector1 = false;
			ShutdownMetalDetector2 = false;
			ShutdownMetalDetector3 = false;
			if (MDBox1 != null)
			{
				MDBox1.Delete();
			}
			if (MDBox2 != null)
			{
				MDBox2.Delete();
			}
			if (MDBox3 != null)
			{
				MDBox3.Delete();
			}
			MDBox1 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_fuse_box_01a"), new Vector3(2545.732f, -276.79f, -57.62f), new Vector3(0f, 0f, -178.76f), false, false);
			MDBox1.IsPositionFrozen = true;
			MDBox2 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_fuse_box_01a"), new Vector3(2525.57f, -267.8022f, -57.59f), new Vector3(0f, 0f, -179.07f), false, false);
			MDBox2.IsPositionFrozen = true;
			MDBox3 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_fuse_box_01a"), new Vector3(2542.522f, -265.155f, -57.48f), new Vector3(0f, 0f, 89.72f), false, false);
			MDBox3.IsPositionFrozen = true;
			PlantedBZGas = false;
			Vector3 position = Game.Player.Character.Position;
			Vehicle item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), new Vector3(2524.9f, -271f, -65f), 180f);
			AircraftCarrierVehicles.Add(item);
			item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), new Vector3(2533.4f, -288.1f, -65.1f), 0f);
			AircraftCarrierVehicles.Add(item);
			item = GlobalHelper.CreateVehicle(RequestModel(VehicleHash.Stockade), new Vector3(2542.5f, -288.5f, -65.1f), 0f);
			AircraftCarrierVehicles.Add(item);
			VaultDoor = GlobalHelper.CreateProp(RequestModel("prop_com_gar_door_01"), new Vector3(2506.7f, -269.5f, -58f), new Vector3(0f, 0f, 90f), true, false);
			VaultDoor.IsVisible = false;
			VaultDoor.IsPositionFrozen = true;
			PlayingCutscene = false;
			CreatedCrates = false;
			CreatedCrates = false;
			ManageRobberyAnims.DeleteLoot();
			Script.Wait(10);
			GetLootLocations_DCH();
			Script.Wait(10);
			SetupPaintingsOrLockVault = false;
			CurrentSDBType.Clear();
			CurrentSDB.Clear();
			CurrentSDBDict.Clear();
			CuttingStage = 0;
			Random random = new Random();
			int num2 = random.Next(0, 1000);
			AllowMix = false;
			SetupPaintingsOrLockVault = false;
			PlaceLoot();
			Notification.Show("~y~ Lester ~w~ Valuables is a mix of ~o~Cash~w~, ~o~Gold~w~, ~o~Diamonds~w~, ~o~Paintings~w~ & ~o~Safe Depoist Boxes~w~  ");
			VaultType = 1;
			AllowMix = true;
			SetupPaintingsOrLockVault = true;
			CreatedCrates = true;
			if (EnteranceUsed == 0)
			{
				SlotMachines.Clear();
				SlotMachinesRot.Clear();
				SlotMachines.Add(new Vector3(1105.959f, 228.7872f, -49.8f));
				SlotMachinesRot.Add(35f);
				SlotMachines.Add(new Vector3(1106.344f, 230.4661f, -49.8f));
				SlotMachinesRot.Add(108f);
				SlotMachines.Add(new Vector3(1105.047f, 231.4999f, -49.8f));
				SlotMachinesRot.Add(178f);
				SlotMachines.Add(new Vector3(1103.892f, 230.5018f, -49.8f));
				SlotMachinesRot.Add(-107f);
				SlotMachines.Add(new Vector3(1101.564f, 230.6477f, -49.8f));
				SlotMachinesRot.Add(63f);
				SlotMachines.Add(new Vector3(1100.822f, 230.105f, -49.8f));
				SlotMachinesRot.Add(45f);
				SlotMachines.Add(new Vector3(1101.661f, 231.5672f, -49.8f));
				SlotMachinesRot.Add(73f);
				SlotMachines.Add(new Vector3(1101.94f, 232.4414f, -49.8f));
				SlotMachinesRot.Add(87f);
				SlotMachines.Add(new Vector3(1101.775f, 233.3168f, -49.8f));
				SlotMachinesRot.Add(101f);
				SlotMachines.Add(new Vector3(1107.15f, 234.9417f, -49.8f));
				SlotMachinesRot.Add(-111f);
				SlotMachines.Add(new Vector3(1107.558f, 233.311f, -49.8f));
				SlotMachinesRot.Add(-34f);
				SlotMachines.Add(new Vector3(1109.176f, 233.5511f, -49.8f));
				SlotMachinesRot.Add(32f);
				SlotMachines.Add(new Vector3(1109.793f, 234.9457f, -49.8f));
				SlotMachinesRot.Add(109f);
				SlotMachines.Add(new Vector3(1108.582f, 239.1074f, -49.8f));
				SlotMachinesRot.Add(-48f);
				SlotMachines.Add(new Vector3(1109.164f, 238.4004f, -49.8f));
				SlotMachinesRot.Add(-34f);
				SlotMachines.Add(new Vector3(1110.082f, 238.1253f, -49.8f));
				SlotMachinesRot.Add(-20f);
				SlotMachines.Add(new Vector3(1110.956f, 238.1476f, -49.8f));
				SlotMachinesRot.Add(-3f);
				SlotMachines.Add(new Vector3(1111.801f, 238.1584f, -49.8f));
				SlotMachinesRot.Add(25f);
				SlotMachines.Add(new Vector3(1112.645f, 238.5698f, -49.8f));
				SlotMachinesRot.Add(32f);
				SlotMachines.Add(new Vector3(1113.419f, 238.9962f, -49.8f));
				SlotMachinesRot.Add(46f);
				SlotMachines.Add(new Vector3(1115.422f, 234.7188f, -49.8f));
				SlotMachinesRot.Add(109f);
				SlotMachines.Add(new Vector3(1114.047f, 235.7146f, -49.8f));
				SlotMachinesRot.Add(-170f);
				SlotMachines.Add(new Vector3(1112.701f, 234.7498f, -49.8f));
				SlotMachinesRot.Add(-107f);
				SlotMachines.Add(new Vector3(1113.335f, 233.1384f, -49.8f));
				SlotMachinesRot.Add(-37f);
				SlotMachines.Add(new Vector3(1114.964f, 233.0724f, -49.8f));
				SlotMachinesRot.Add(25f);
				SlotMachines.Add(new Vector3(1116.269f, 228.2968f, -49.8f));
				SlotMachinesRot.Add(-39f);
				SlotMachines.Add(new Vector3(1117.877f, 2278.35742f, -49.8f));
				SlotMachinesRot.Add(44f);
				SlotMachines.Add(new Vector3(1118.452f, 229.9547f, -49.8f));
				SlotMachinesRot.Add(103f);
				SlotMachines.Add(new Vector3(1117.169f, 230.8778f, -49.8f));
				SlotMachinesRot.Add(-175f);
				SlotMachines.Add(new Vector3(1117.169f, 230.8778f, -49.8f));
				SlotMachinesRot.Add(-175f);
				SlotMachines.Add(new Vector3(1129.584f, 250.9299f, -51.2f));
				SlotMachinesRot.Add(-173f);
				SlotMachines.Add(new Vector3(1130.55f, 251.0233f, -51.2f));
				SlotMachinesRot.Add(165f);
				SlotMachines.Add(new Vector3(1131.325f, 250.5148f, -51.2f));
				SlotMachinesRot.Add(154f);
				SlotMachines.Add(new Vector3(1132.006f, 249.9666f, -51.2f));
				SlotMachinesRot.Add(133f);
				SlotMachines.Add(new Vector3(1132.541f, 249.3008f, -51.2f));
				SlotMachinesRot.Add(119f);
				SlotMachines.Add(new Vector3(1132.924f, 248.4754f, -51.2f));
				SlotMachinesRot.Add(105f);
				SlotMachines.Add(new Vector3(1133.166f, 247.5615f, -51.2f));
				SlotMachinesRot.Add(84f);
				SlotMachines.Add(new Vector3(1137.762f, 251.4668f, -51.2f));
				SlotMachinesRot.Add(-44f);
				SlotMachines.Add(new Vector3(1139.381f, 251.0917f, -51.2f));
				SlotMachinesRot.Add(25f);
				SlotMachines.Add(new Vector3(1140.171f, 252.5691f, -51.2f));
				SlotMachinesRot.Add(102f);
				SlotMachines.Add(new Vector3(1138.972f, 253.6587f, -51.2f));
				SlotMachinesRot.Add(175f);
				SlotMachines.Add(new Vector3(1135.809f, 256.7634f, -51.2f));
				SlotMachinesRot.Add(98f);
				SlotMachines.Add(new Vector3(1134.669f, 257.789f, -51.2f));
				SlotMachinesRot.Add(168f);
				SlotMachines.Add(new Vector3(1133.367f, 257.0611f, -51.2f));
				SlotMachinesRot.Add(-117f);
				try
				{
					WheelProp = GlobalHelper.CreateProp(SpawnProp("vw_prop_vw_luckywheel_02a"), new Vector3(LuckyWheel.X, LuckyWheel.Y + 1.17f, LuckyWheel.Z + 0.12f), true, false);
					WheelProp.IsPositionFrozen = true;
					WheelProp.Rotation = new Vector3(0f, -10f, 0f);
					WheelProp2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_vw_luckywheel_02a"), new Vector3(1110.771f, 223.5515f, -50f), true, false);
					WheelProp2.Rotation = new Vector3(0f, -90f, 0f);
					WheelProp2.IsPositionFrozen = true;
					WheelProp2.Rotation = new Vector3(0f, -10f, 0f);
					WheelProp2.IsVisible = false;
					WheelProp2.IsCollisionEnabled = false;
					Vector3 thraxPos = ThraxPos;
					BasePlate = GlobalHelper.CreateProp(position: new Vector3(thraxPos.X, thraxPos.Y, thraxPos.Z - 5.1f), model: SpawnProp("xs_prop_arena_flipper_small_01a_sf"), dynamic: true, placeOnGround: false);
					BasePlate.Rotation = new Vector3(0f, 0f, 0f);
					BasePlate.IsPositionFrozen = true;
					BasePlate.IsVisible = false;
					BasePlate.IsCollisionEnabled = true;
				}
				catch (Exception)
				{
					Notification.Show("~r~Error 3~");
				}
				try
				{
					WheelWinMarker = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0.18f, -0.3f, 1.15f)), true, false);
					Prop prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(0f, -0.3f, 1.15f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(0.4f, -0.3f, 1.1f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(0.7f, -0.3f, 0.9f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(0.95f, -0.3f, 0.6f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(1.1f, -0.3f, 0.3f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(1.15f, -0.3f, 0f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(1.1f, -0.3f, -0.3f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(0.95f, -0.3f, -0.6f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(0.7f, -0.3f, -0.9f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(0.35f, -0.3f, -1.1f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(0f, -0.3f, -1.15f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-0.35f, -0.3f, -1.1f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-0.7f, -0.3f, 0.9f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-0.95f, -0.3f, 0.6f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-1.1f, -0.3f, 0.3f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-1.15f, -0.3f, 0f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-1.1f, -0.3f, -0.3f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp("vw_prop_roulette_marker", WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-0.95f, -0.3f, -0.6f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-0.7f, -0.3f, -0.9f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-0.4f, -0.3f, -1.1f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					prop2 = GlobalHelper.CreateProp(SpawnProp("vw_prop_roulette_marker"), WheelProp.GetOffsetPosition(new Vector3(0f, 0f, 0f)), true, false);
					prop2.AttachTo(WheelProp, new Vector3(-0.4f, -0.3f, 1.1f), new Vector3(90f, 0f, 0f));
					WheelMarkers.Add(prop2);
					foreach (Prop wheelMarker in WheelMarkers)
					{
						if (wheelMarker != null)
						{
							wheelMarker.IsVisible = false;
						}
					}
				}
				catch (Exception)
				{
					Notification.Show("~r~Prize Wheel Markers failed to spawn~");
				}
				try
				{
					Vendor1 = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(967.0132f, 29.35624f, 115f), 56f);
					Vendor2 = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(969.15f, 33.19205f, 115f), 56f);
					SetPedOutfit(Vendor1);
					PlayDealerAnim(Vendor1);
					SetPedOutfit(Vendor2);
					PlayDealerAnim(Vendor2);
					MainPed.Add(Vendor1);
					MainPed.Add(Vendor2);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1118.256f, 219.4535f, -49.5f), 103f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					Vector3 position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1101.027f, 195.2306f, -49.5f), -48f);
					SetPedOutfit(this.ped);
					position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("ig_tomcasino"), new Vector3(1087.711f, 221.1435f, -49.5f), 177f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1145.633f, 261.8369f, -49.5f), -136f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1151.171f, 267.3671f, -49.5f), -136f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1149.443f, 269.1333f, -49.5f), 39f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1144.041f, 263.6831f, -49.5f), 48f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1149.349f, 252.3492f, -49.5f), 137f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1143.853f, 246.481f, -49.5f), -38f);
					this.ped.IsPositionFrozen = true;
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1110.818f, 209.2807f, -49.5f), 34f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					this.ped.Task.PlayAnimation("anim@amb@casino@mini@drinking@bar@drink_v2@heels@idle_a", "idle_a_bartender", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1113.457f, 208.9917f, -49.5f), -63f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					this.ped.Task.PlayAnimation("anim@amb@casino@mini@drinking@bar@drink_v2@heels@idle_a", "idle_a_bartender", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1112.64f, 206.597f, -49.5f), -178f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					this.ped.Task.PlayAnimation("anim@amb@casino@mini@drinking@bar@drink_v2@heels@idle_a", "idle_a_bartender", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1148.878f, 264.5572f, -49.5f), 44f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1147.896f, 246.8535f, -49.5f), -51f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1134.328f, 267.3924f, -49.5f), 134f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(958.8818f, 71.53243f, 111f), 154f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					this.ped.Task.PlayAnimation("anim@amb@casino@mini@drinking@bar@drink_v2@heels@idle_a", "idle_a_bartender", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1128.806f, 261.6401f, -50f), -42f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel(PedHash.TaoCheng), new Vector3(1113.545f, 249.2349f, -46f), -179f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					this.ped.Task.PlayAnimation("move_drunk_m", "idle", 1900f, 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel(PedHash.TaosTranslator), new Vector3(1113.671f, 247.5057f, -46f), -32f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					this.ped.Task.StartScenario("WORLD_HUMAN_MOBILE_FILM_SHOCKING", this.ped.Position.Z);
					SetPedOutfit(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1131.212f, 268.1662f, -50f), 111f);
					MainPed.Add(this.ped);
					position3 = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = new Vector3(position3.X, position3.Y, -52f);
					PlayDealerAnim(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("s_f_y_casino_01"), new Vector3(1128.704f, 211f, -49f), -43f);
					SetPedOutfit(this.ped);
					MainPed.Add(this.ped);
					SetPedOutfit(this.ped);
					this.ped = GlobalHelper.CreatePed("s_f_y_casino_01", new Vector3(945.3591f, 13.48792f, 115f), 52f);
					this.ped.Task.PlayAnimation("anim@amb@casino@mini@drinking@bar@drink_v2@heels@idle_a", "idle_a_bartender", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreatePed(RequestModel("csb_agatha"), new Vector3(1114.801f, 253.6271f, -46.4f), -90f);
					this.ped.AlwaysKeepTask = true;
					MainPed.Add(this.ped);
					this.ped.Task.PlayAnimation("anim@amb@office@pa@female@", "pa_base", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.131f, 278.7511f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1134.131f, 278.7511f, -52.1f);
					this.ped.IsPositionFrozen = true;
					this.ped.Rotation = new Vector3(0f, 0f, -90f);
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.PlayAnimation("missfam5_blackout", "vomit", 128f, 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1135.075f, 280.4876f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1134.134f, 280.4636f, -52.1f);
					this.ped.IsPositionFrozen = true;
					this.ped.Rotation = new Vector3(0f, 0f, 90f);
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.PlayAnimation("switch@trevor@on_toilet", "trev_on_toilet_loop", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1134.012f, 282.1808f, -52.1f);
					this.ped.IsPositionFrozen = true;
					this.ped.Rotation = new Vector3(0f, 0f, 90f);
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.PlayAnimation("switch@trevor@puking_into_fountain", "trev_fountain_puke_loop", 128f, 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1133.018f, 276.7346f, -52f);
					this.ped.IsPositionFrozen = true;
					this.ped.Rotation = new Vector3(0f, 0f, -90f);
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.PlayAnimation("switch@michael@wash_face", "loop_michael", 128f, 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50.5f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1105.535f, 217.0142f, -49.9f);
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.Rotation = new Vector3(0f, 0f, 62f);
					this.ped.Task.StartScenario("WORLD_HUMAN_PAPARAZZI", this.ped.Position.Z);
					MainPed.Add(this.ped);
					this.ped.Rotation = new Vector3(0f, 0f, 62f);
					this.ped.Task.StartScenario("WORLD_HUMAN_TOURIST_MOBILE", this.ped.Position.Z);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50.5f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1104.666f, 215.5959f, -49.9f);
					this.ped.IsPositionFrozen = true;
					this.ped.Heading = 38f;
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.StartScenario("WORLD_HUMAN_PAPARAZZI", this.ped.Position.Z);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -49.8f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1106.087f, 218.7728f, -49.9f);
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.Rotation = new Vector3(0f, 0f, 82f);
					this.ped.Task.StartScenario("WORLD_HUMAN_PAPARAZZI", this.ped.Position.Z);
					MainPed.Add(this.ped);
					this.ped.Heading = 82f;
					this.ped.Task.StartScenario("WORLD_HUMAN_TOURIST_MOBILE", this.ped.Position.Z);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1102.207f, 200.4515f, -50.4f);
					this.ped.IsPositionFrozen = true;
					this.ped.Heading = 136f;
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.StartScenario("WORLD_HUMAN_TOURIST_MOBILE", this.ped.Position.Z);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1103.912f, 197.4887f, -50.4f);
					this.ped.IsPositionFrozen = true;
					this.ped.Heading = -81f;
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.StartScenario("WORLD_HUMAN_AA_COFFEE", this.ped.Position.Z);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1096.842f, 208.2964f, -50f);
					this.ped.IsPositionFrozen = true;
					this.ped.Heading = 75f;
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.StartScenario("WORLD_HUMAN_AA_COFFEE", this.ped.Position.Z);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1094.77f, 208.3848f, -50f);
					this.ped.IsPositionFrozen = true;
					this.ped.Rotation = new Vector3(0f, 0f, -90f);
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.StartScenario("WORLD_HUMAN_HANG_OUT_STREET", this.ped.Position.Z);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1096.422f, 209.5439f, -50f);
					this.ped.IsPositionFrozen = true;
					this.ped.Heading = 146f;
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.StartScenario("WORLD_HUMAN_HANG_OUT_STREET", this.ped.Position.Z);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1134.012f, 282.1808f, -50f));
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1106.37f, 212.0176f, -50.3f);
					this.ped.IsPositionFrozen = true;
					this.ped.Rotation = new Vector3(0f, 0f, 146f);
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.PlayAnimation("amb@world_human_sunbathe@female@back@base", "base", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreatePed(PedHash.Paramedic01SMM, new Vector3(1134.012f, 282.1808f, -51f), 1f);
					this.ped.Position = MarkerEnter;
					Script.Wait(1);
					this.ped.Position = new Vector3(1106.548f, 211.8759f, -50.3f);
					this.ped.IsPositionFrozen = true;
					this.ped.Rotation = new Vector3(0f, 0f, 122f);
					this.ped.AlwaysKeepTask = true;
					this.ped.Task.PlayAnimation("missheistfbi3b_ig8_2", "cpr_loop_paramedic", 8f, 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					foreach (Ped item6 in MainPed)
					{
						if (item6 != null)
						{
							Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, item6, 0, 0);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item6, 17, 1);
							Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item6, 1);
							Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, item6, 0, 0);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item6, 17, 1);
							item6.RelationshipGroup = 1;
							item6.AlwaysKeepTask = true;
						}
					}
					Random random2 = new Random();
					Vector3 markerEnter = MarkerEnter;
					Vector3 vector = new Vector3(0f, 0f, 0f);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1111.493f, 211.5376f, -51.5f));
					this.ped.Heading = -174f;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1110.145f, 210.9409f, -51.5f));
					this.ped.Heading = -147f;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1109.086f, 209.9584f, -51.5f));
					this.ped.Heading = -122f;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1108.59f, 206.8573f, -51.5f));
					this.ped.Heading = -73f;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1109.433f, 205.567f, -51.5f));
					this.ped.Heading = -38f;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1110.84f, 204.7336f, -51.5f));
					this.ped.Heading = -28f;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1112.326f, 204.5824f, -51.5f));
					this.ped.Heading = -3f;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1114.798f, 206.2514f, -51.5f));
					this.ped.Heading = 45f;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
					this.ped = GlobalHelper.CreateRandomPed(new Vector3(1115.477f, 20.6416f, -51.5f));
					this.ped.Heading = 45f;
					this.ped.IsPositionFrozen = true;
					this.ped.AlwaysKeepTask = true;
					this.ped.IsPersistent = true;
					vector = this.ped.Position;
					Script.Wait(1);
					this.ped.Position = position3;
					Script.Wait(1);
					this.ped.Position = vector;
					Script.Wait(1);
					this.ped.Task.PlayAnimation("anim@special_peds@casino@bar@ped_male@sit_withdrink@01a@base", "base", random2.Next(162, 1000), 1f, -1, AnimationFlags.Loop, -1f);
					MainPed.Add(this.ped);
				}
				catch
				{
				}
				foreach (Ped item7 in MainPed)
				{
					if (item7 != null)
					{
						Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, item7, 0, 0);
						Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item7, 17, 1);
						Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item7, 1);
						Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, item7, 0, 0);
						Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item7, 17, 1);
						item7.RelationshipGroup = 1;
						item7.AlwaysKeepTask = true;
					}
				}
				try
				{
					try
					{
						Prop[] nearbyProps = World.GetNearbyProps(MarkerExit, 3000f, RequestModel("vw_prop_casino_3cardpoker_01"));
						Prop[] array = nearbyProps;
						foreach (Prop prop3 in array)
						{
							if (prop3 != null)
							{
								Tables.Add(prop3);
							}
						}
						nearbyProps = World.GetNearbyProps(MarkerExit, 3000f, RequestModel("vw_prop_casino_3cardpoker_01b"));
						Prop[] array2 = nearbyProps;
						foreach (Prop prop4 in array2)
						{
							if (prop4 != null)
							{
								Tables.Add(prop4);
							}
						}
						nearbyProps = World.GetNearbyProps(MarkerExit, 3000f, RequestModel("vw_prop_casino_blckjack_01"));
						Prop[] array3 = nearbyProps;
						foreach (Prop prop5 in array3)
						{
							if (prop5 != null)
							{
								Tables.Add(prop5);
							}
						}
						nearbyProps = World.GetNearbyProps(MarkerExit, 3000f, RequestModel("vw_prop_casino_blckjack_01b"));
						Prop[] array4 = nearbyProps;
						foreach (Prop prop6 in array4)
						{
							if (prop6 != null)
							{
								Tables.Add(prop6);
							}
						}
					}
					catch
					{
						Notification.Show("~r~ Error 10");
					}
					Prop prop7 = GlobalHelper.CreateProp(SpawnProp("vw_prop_casino_blckjack_01b"), new Vector3(968.2f, 33.75655f, 115f), true, false);
					prop7.Rotation = new Vector3(0f, 0f, -124f);
					Tables.Add(prop7);
					prop7 = GlobalHelper.CreateProp(SpawnProp("vw_prop_casino_3cardpoker_01b"), new Vector3(966.2f, 29.93874f, 115f), true, false);
					prop7.Rotation = new Vector3(0f, 0f, -124f);
					Tables.Add(prop7);
					prop7 = GlobalHelper.CreateProp(SpawnProp("vw_prop_casino_roulette_01"), new Vector3(1147.584f, 265.6707f, -52.8f), true, false);
					prop7.Rotation = new Vector3(0f, 0f, 133f);
					Tables2.Add(prop7);
					prop7 = GlobalHelper.CreateProp(SpawnProp("vw_prop_casino_roulette_01b"), new Vector3(1148.889f, 248.0665f, -52f), true, false);
					prop7.Rotation = new Vector3(0f, 0f, 43f);
					Tables2.Add(prop7);
					prop7 = GlobalHelper.CreateProp(SpawnProp("vw_prop_casino_roulette_01b"), new Vector3(1129.883f, 267.433f, -52f), true, false);
					prop7.Rotation = new Vector3(0f, 0f, -159f);
					Tables2.Add(prop7);
				}
				catch (Exception)
				{
					Notification.Show("~r~ Error 5");
				}
				try
				{
					Ped ped = null;
					for (int m = 0; m < SlotMachines.Count; m++)
					{
						Random random3 = new Random();
						int num3 = random3.Next(0, 100);
						int num4 = random3.Next(1, 6);
						if (num3 <= 100)
						{
							ped = GlobalHelper.CreateRandomPed(SlotMachines[m]);
							ped.Heading = SlotMachinesRot[m];
							ped.IsPositionFrozen = true;
							ped.AlwaysKeepTask = true;
							ped.IsPersistent = true;
							SMPed.Add(ped);
							if (num4 == 1)
							{
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_a", 16f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (num4 == 2)
							{
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_b", 16f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (num4 == 3)
							{
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_c", 16f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (num4 == 4)
							{
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_d", 16f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (num4 == 5)
							{
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_e", 16f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (num4 == 6)
							{
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_f", 16f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (num4 > 6)
							{
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_b", 16f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (num4 < 0)
							{
								ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_e", 16f, 1f, -1, AnimationFlags.Loop, -1f);
							}
						}
					}
					for (int n = 0; n < SlotMachines.Count; n++)
					{
						if (SMPed[n] != null)
						{
							Vector3 position4 = SMPed[n].Position;
							position4 = new Vector3(position4.X, position4.Y, position4.Z - 2.3f);
							SMPed[n].Position = MarkerEnter;
							Vector3 position5 = SMPed[n].Position;
							SMPed[n].Position = position4;
							Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, SMPed[n], 0, 0);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, SMPed[n], 17, 1);
							Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, SMPed[n], 1);
							Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, SMPed[n], 0, 0);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, SMPed[n], 17, 1);
							SMPed[n].RelationshipGroup = 1;
							SMPed[n].AlwaysKeepTask = true;
						}
					}
					foreach (Ped item8 in SMPed)
					{
						if (item8 != null)
						{
							Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, item8, 0, 0);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item8, 17, 1);
							Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, item8, 1);
							Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, item8, 0, 0);
							Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, item8, 17, 1);
							item8.RelationshipGroup = 1;
							item8.AlwaysKeepTask = true;
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					Prizecar = GlobalHelper.CreateVehicle(RequestModel("thrax"), ThraxPos, 9.5f);
					Prizecar.IsPersistent = true;
				}
				catch (Exception)
				{
					Notification.Show("~r~Error 4~");
				}
			}
			int enteranceUsed = EnteranceUsed;
			int num5 = 6;
			Random random4 = new Random();
			CompletedBugstarTasks = false;
			CompletedGroupe6Tasks = false;
			Groupe6TaskStage = 0;
			BugstarTaskStage = 0;
			foreach (ObjectPlacementMap bugStarVent in BugStarVents)
			{
				if (bugStarVent.Entity != null)
				{
					bugStarVent.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap groupe6Cam in Groupe6Cams)
			{
				if (groupe6Cam.Entity != null)
				{
					groupe6Cam.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap bugStarVent2 in BugStarVents)
			{
				if (bugStarVent2.Entity == null)
				{
					bugStarVent2.CreateObjatCoords();
				}
			}
			foreach (ObjectPlacementMap groupe6Cam2 in Groupe6Cams)
			{
				if (groupe6Cam2.Entity == null)
				{
					groupe6Cam2.CreateObjatCoords();
				}
			}
		}

		public int BoolToInt(bool B)
		{
			int result = 0;
			if (B)
			{
				result = 1;
			}
			if (!B)
			{
				result = 0;
			}
			return result;
		}

		public bool IntToBool(int B)
		{
			bool result = false;
			if (B == 0)
			{
				result = false;
			}
			if (B == 1)
			{
				result = true;
			}
			return result;
		}

		public void SetEntityinCustscenes()
		{
			SetPedOutfitCutscene("MP_1", Game.Player.Character);
			Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, Game.Player.Character, "MP_1", 0, 0, 64);
			if (RobberA != null)
			{
				SetPedOutfitCutscene("MP_2", RobberA);
				Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, RobberA, "MP_2", 0, 0, 64);
			}
			else
			{
				CutsenePed2 = GlobalHelper.CreatePed(RequestPed(PedHash.FreemodeMale01), new Vector3(711.889f, -977.887f, 24f), 168f);
				SetPedOutfitCutscene("MP_2", CutsenePed2);
				Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed2, "MP_2", 0, 0, 64);
				CutsenePed2.IsVisible = false;
			}
			if (RobberB != null)
			{
				if (RobberBskill != 0)
				{
					if (RobberBskill != 7)
					{
						CutsenePed3 = GlobalHelper.CreatePed(RequestPed(RobberBSkin), new Vector3(711.889f, -977.887f, 24f), 168f);
						Setoutfit(CutsenePed3, RobberB_C);
						SetOutift(CutsenePed3, RobberB_C);
					}
					if (RobberBskill == 7)
					{
						CutsenePed3 = GlobalHelper.CreatePed(PedHash.FreemodeMale01, new Vector3(711.889f, -977.887f, 24f), 168f);
						Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
						Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
						Ped cutsenePed = CutsenePed3;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 1, 91, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 4, 84, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 8, 97, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 11, 186, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed, 12, 0, 0, 1);
						Function.Call(Hash.SET_PED_USING_ACTION_MODE, cutsenePed, true, -1, 0);
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, cutsenePed, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
						Function.Call(Hash.SET_PED_STRAFE_CLIPSET, cutsenePed, "MOVE_STRAFE_BALLISTIC");
						Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, cutsenePed, 1429513766);
					}
				}
				SetPedOutfitCutscene("MP_3", CutsenePed3);
				Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed3, "MP_3", 0, 0, 64);
				RobberB.IsVisible = false;
			}
			else
			{
				CutsenePed3 = GlobalHelper.CreatePed(RequestPed(PedHash.FreemodeMale01), new Vector3(711.889f, -977.887f, 24f), 168f);
				SetPedOutfitCutscene("MP_3", CutsenePed3);
				Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed3, "MP_3", 0, 0, 64);
				CutsenePed3.IsVisible = false;
			}
			if (RobberC != null)
			{
				if (RobberCskill != 0)
				{
					if (RobberCskill != 7)
					{
						CutsenePed4 = GlobalHelper.CreatePed(RequestPed(RobberCSkin), new Vector3(709.845f, -977.909f, 24f), 168f);
						Setoutfit(CutsenePed4, RobberC_C);
						SetOutift(CutsenePed4, RobberC_C);
					}
					if (RobberCskill == 7)
					{
						CutsenePed4 = GlobalHelper.CreatePed(PedHash.FreemodeMale01, new Vector3(709.845f, -977.909f, 24f), 168f);
						Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
						Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
						Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
						Ped cutsenePed2 = CutsenePed4;
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 0, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 1, 91, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 2, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 3, 46, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 4, 84, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 5, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 6, 10, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 7, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 8, 97, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 9, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 10, 0, 0, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 11, 186, 10, 1);
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, cutsenePed2, 12, 0, 0, 1);
						Function.Call(Hash.SET_PED_USING_ACTION_MODE, cutsenePed2, true, -1, 0);
						Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, cutsenePed2, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
						Function.Call(Hash.SET_PED_STRAFE_CLIPSET, cutsenePed2, "MOVE_STRAFE_BALLISTIC");
						Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, cutsenePed2, 1429513766);
					}
				}
				SetPedOutfitCutscene("MP_4", CutsenePed4);
				Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed4, "MP_4", 0, 0, 64);
				RobberC.IsVisible = false;
			}
			else
			{
				CutsenePed4 = GlobalHelper.CreatePed(RequestPed(PedHash.FreemodeMale01), new Vector3(711.889f, -977.887f, 24f), 168f);
				SetPedOutfitCutscene("MP_4", CutsenePed4);
				Function.Call(Hash.REGISTER_ENTITY_FOR_CUTSCENE, CutsenePed4, "MP_4", 0, 0, 64);
				CutsenePed4.IsVisible = false;
			}
		}

		private Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public static void PlaceLoot()
		{
			Random random = new Random();
			int num = 400;
			num = random.Next(0, 1000);
			for (int i = 0; i < Crate_loc.Count; i++)
			{
				Vector3 vector = Crate_loc[i];
				if (num < 100)
				{
					int num2 = random.Next(0, 300);
					if (num2 < 100)
					{
						Prop prop = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop);
					}
					if (num2 >= 100 && num2 < 200)
					{
						Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01b"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop2.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop2);
					}
					if (num2 >= 200)
					{
						Prop prop3 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01c"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop3.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop3);
					}
				}
				if (num >= 100 && num < 200)
				{
					int num3 = random.Next(0, 300);
					if (num3 < 100)
					{
						Prop prop4 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop4.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop4);
					}
					if (num3 >= 100 && num3 < 200)
					{
						Prop prop5 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01b"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop5.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop5);
					}
					if (num3 >= 200)
					{
						Prop prop6 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01c"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop6.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop6);
					}
				}
				if (num >= 200 && num < 300)
				{
					int num4 = random.Next(0, 300);
					if (num4 < 100)
					{
						Prop prop7 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop7.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop7);
					}
					if (num4 >= 100 && num4 < 200)
					{
						Prop prop8 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01b"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop8.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop8);
					}
					if (num4 >= 200)
					{
						Prop prop9 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01c"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i] - 180f), true, false);
						prop9.IsPositionFrozen = true;
						ManageRobberyAnims.AddPropAsLoot(prop9);
					}
				}
				if (num >= 300 && num < 500)
				{
					int num5 = random.Next(1, 9);
					if (num5 == 1)
					{
						Prop prop10 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop11 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop11.Position = prop10.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
						ManageRobberyAnims.AddPropAsLoot(prop11);
						prop10.IsVisible = false;
						PaintingHolder.Add(prop10);
					}
					if (num5 == 2)
					{
						Prop prop12 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop13 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01b"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop13.PositionNoOffset = Crate_loc[i];
						prop13.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
						ManageRobberyAnims.AddPropAsLoot(prop13);
						prop12.IsVisible = false;
						PaintingHolder.Add(prop12);
					}
					if (num5 == 3)
					{
						Prop prop14 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop15 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01c"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop15.PositionNoOffset = Crate_loc[i];
						prop15.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
						prop15.Position = prop14.GetOffsetPosition(new Vector3(-0.4f, 0f, 0f));
						ManageRobberyAnims.AddPropAsLoot(prop15);
						prop14.IsVisible = false;
						PaintingHolder.Add(prop14);
					}
					if (num5 == 4)
					{
						Prop prop16 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop17 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01d"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop17.PositionNoOffset = Crate_loc[i];
						prop17.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
						prop17.Position = prop16.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
						prop17.Position = prop16.Position;
						ManageRobberyAnims.AddPropAsLoot(prop17);
						prop16.IsVisible = false;
						PaintingHolder.Add(prop16);
					}
					if (num5 == 5)
					{
						Prop prop18 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop19 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01e"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop19.PositionNoOffset = Crate_loc[i];
						prop19.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
						prop19.Position = prop18.Position;
						ManageRobberyAnims.AddPropAsLoot(prop19);
						prop18.IsVisible = false;
						PaintingHolder.Add(prop18);
					}
					if (num5 == 6)
					{
						Prop prop20 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop21 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01f"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop21.PositionNoOffset = Crate_loc[i];
						prop21.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
						prop21.Position = prop20.GetOffsetPosition(new Vector3(-0.4f, 0f, 0f));
						ManageRobberyAnims.AddPropAsLoot(prop21);
						prop20.IsVisible = false;
						PaintingHolder.Add(prop20);
					}
					if (num5 == 7)
					{
						Prop prop22 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop23 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01g"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop23.PositionNoOffset = Crate_loc[i];
						prop23.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
						prop23.Position = prop22.GetOffsetPosition(new Vector3(0.4f, 0f, 0f));
						ManageRobberyAnims.AddPropAsLoot(prop23);
						prop22.IsVisible = false;
						PaintingHolder.Add(prop22);
					}
					if (num5 == 8)
					{
						Prop prop24 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop25 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01h"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop25.PositionNoOffset = Crate_loc[i];
						prop25.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
						prop25.Position = prop24.Position;
						ManageRobberyAnims.AddPropAsLoot(prop25);
						prop24.IsVisible = false;
						PaintingHolder.Add(prop24);
					}
					if (num5 == 9)
					{
						Prop prop26 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop prop27 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_01i"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						prop27.PositionNoOffset = Crate_loc[i];
						prop27.Rotation = new Vector3(0f, 0f, Crate_Rot[i]);
						prop27.Position = prop26.Position;
						ManageRobberyAnims.AddPropAsLoot(prop27);
						prop26.IsVisible = false;
						PaintingHolder.Add(prop26);
					}
					Script.Wait(1);
				}
				if (num >= 500 && num < 1000)
				{
					string name = PaintingIDs[random.Next(0, PaintingIDs.Count)];
					int num6 = random.Next(0, 300);
					if (num6 < 180)
					{
						Prop prop28 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_02a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop o = GlobalHelper.CreateProp(RequestModel(name), prop28.GetOffsetPosition(new Vector3(0f, 0.5f, 1.1f)), prop28.Rotation, false, false);
						ManageRobberyAnims.AddPropAsLoot(o);
						PaintingHolder.Add(prop28);
					}
					if (num6 >= 180)
					{
						Prop prop29 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_sec_cabinet_04a"), Crate_loc[i], new Vector3(0f, 0f, Crate_Rot[i]), false, false);
						Prop o2 = GlobalHelper.CreateProp(RequestModel(name), prop29.GetOffsetPosition(new Vector3(0f, 0.5f, 1.1f)), prop29.Rotation, false, false);
						ManageRobberyAnims.AddPropAsLoot(o2);
						PaintingHolder.Add(prop29);
					}
					Script.Wait(1);
				}
				num = random.Next(0, 1000);
			}
			foreach (ManageRobberyAnims.Lootable crate_Valuable in ManageRobberyAnims.Crate_Valuables)
			{
				if (crate_Valuable != null && crate_Valuable.LootBlip != null)
				{
					crate_Valuable.LootBlip.Alpha = 0;
				}
			}
		}

		public void GetLootLocations_DCH()
		{
			Crate_loc.Clear();
			Crate_Rot.Clear();
			ManageRobberyAnims.Crate_loc.Clear();
			ManageRobberyAnims.Crate_Rot.Clear();
			Crate_loc.Add(new Vector3(2541.419f, -239.8748f, -71.73713f));
			Crate_Rot.Add(267.3391f);
			Crate_loc.Add(new Vector3(2541.068f, -242.5081f, -71.73713f));
			Crate_Rot.Add(261.4025f);
			Crate_loc.Add(new Vector3(2514.469f, -219.1778f, -71.73713f));
			Crate_Rot.Add(18.19468f);
			Crate_loc.Add(new Vector3(2527.617f, -257.903f, -71.73721f));
			Crate_Rot.Add(197.789f);
			Crate_loc.Add(new Vector3(2522.388f, -258.9382f, -71.73721f));
			Crate_Rot.Add(185.1488f);
			Crate_loc.Add(new Vector3(2525.041f, -258.5603f, -71.73721f));
			Crate_Rot.Add(190.5733f);
			Crate_loc.Add(new Vector3(2519.677f, -218.155f, -71.73713f));
			Crate_Rot.Add(3.147372f);
			Crate_loc.Add(new Vector3(2541.071f, -234.5617f, -71.73718f));
			Crate_Rot.Add(282.7439f);
			Crate_loc.Add(new Vector3(2540.435f, -231.9486f, -71.73718f));
			Crate_Rot.Add(287.8618f);
			Crate_loc.Add(new Vector3(2541.439f, -237.1867f, -71.73713f));
			Crate_Rot.Add(274.197f);
			Crate_loc.Add(new Vector3(2517.08f, -218.5034f, -71.73713f));
			Crate_Rot.Add(11.5411f);
			Crate_loc.Add(new Vector3(2517.06f, -258.5537f, -71.73721f));
			Crate_Rot.Add(168.8723f);
			Crate_loc.Add(new Vector3(2519.723f, -258.9276f, -71.73721f));
			Crate_Rot.Add(177.5228f);
			Crate_loc.Add(new Vector3(2514.492f, -257.8667f, -71.73721f));
			Crate_Rot.Add(161.2967f);
			Crate_loc.Add(new Vector3(2540.426f, -245.0993f, -71.73713f));
			Crate_Rot.Add(254.7494f);
			Crate_loc.Add(new Vector3(2527.614f, -219.2013f, -71.73721f));
			Crate_Rot.Add(343.1869f);
			Crate_loc.Add(new Vector3(2525.017f, -218.5114f, -71.73721f));
			Crate_Rot.Add(349.5333f);
			Crate_loc.Add(new Vector3(2522.389f, -218.1367f, -71.73713f));
			Crate_Rot.Add(356.8009f);
		}

		public void OpenCasinoDoors()
		{
			ManageDoor(new Vector3(2519.938f, -251.0496f, -71.74234f), "ch_prop_ch_vault_slide_door_lrg", false);
			ManageDoor(new Vector3(2519.962f, -226.0072f, -71.742f), "ch_prop_ch_vault_slide_door_lrg", false);
			ManageDoor(new Vector3(2533.56f, -239.6175f, -71.742f), "ch_prop_ch_vault_slide_door_lrg", false);
			if (VaultClosed)
			{
				if (EnteranceUsed != 5 && EnteranceUsed != 1)
				{
					ManageDoor(new Vector3(2504.97f, -240.3102f, -70.1788f), "ch_prop_ch_vaultdoor01x", true);
				}
				else
				{
					ManageDoor(new Vector3(2504.97f, -240.3102f, -70.1788f), "ch_prop_ch_vaultdoor01x", false);
				}
			}
			else
			{
				ManageDoor(new Vector3(2504.97f, -240.3102f, -70.1788f), "ch_prop_ch_vaultdoor01x", true);
			}
			if (ManageRobberyAnims.CurrentPay >= 200)
			{
				ManageDoor(new Vector3(2504.97f, -240.3102f, -70.1788f), "ch_prop_ch_vaultdoor01x", false);
			}
			ManageDoor(new Vector3(2492.28f, -237.4592f, -71.73859f), "ch_prop_ch_tunnel_door_01_l", false);
			ManageDoor(new Vector3(2492.28f, -239.544f, -71.73859f), "ch_prop_ch_tunnel_door_01_r", false);
		}

		public void VaultLooting()
		{
			if (CreatedCrates)
			{
				GetDistance_VDIST2(Game.Player.Character.Position, VaultPos, "<", 30f);
			}
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

		public void OnTick(object sender, EventArgs e)
		{
			if (SPH_Debug.CasinoDrillDebug && Game.IsControlJustPressed(GTA.Control.ThrowGrenade))
			{
				if (!DrillComplete)
				{
					DrillComplete = true;
					Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
					Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
					StartCasinoLaser();
					Notification.Show("You can Cancel the Drilling by pressing G");
				}
				else if (DrillComplete)
				{
					DrillComplete = false;
					Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
					Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
					DestoryDrill();
				}
			}
			if (!CreatedLoot)
			{
				CreatedLoot = true;
				PlaceLoot();
			}
			if (modLemonUIObjectPool != null)
			{
				bool areAnyVisible = modLemonUIObjectPool.AreAnyVisible;
			}
			if (modLemonUIObjectPool2 != null)
			{
				bool areAnyVisible2 = modLemonUIObjectPool2.AreAnyVisible;
			}
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			if (modLemonUIObjectPool2 != null)
			{
				modLemonUIObjectPool2.Process();
			}
			if (RunHeist == 1)
			{
				Main.modLemonUIObjectPool.HideAll();
				StartDiamondCasino();
				RunHeist = 0;
			}
			if (!MissionOn && !CameraON)
			{
				return;
			}
			if (MissionOn)
			{
				VaultLooting();
			}
			OpenCasinoDoors();
			bool cameraON = CameraON;
			if (CameraON && (RobberA != null || RobberB != null || RobberC != null))
			{
				DisplayReq();
			}
			if (!modLemonUIObjectPool.AreAnyVisible && CameraON)
			{
				CameraON = false;
				if (HeistStartCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					HeistStartCam.IsActive = false;
					HeistStartCam.Delete();
				}
				if (!SetupAi)
				{
					if (RobberC != null)
					{
						RobberC.Delete();
					}
					if (RobberB != null)
					{
						RobberB.Delete();
					}
				}
				Game.Player.Character.Position = StartHeistMarker;
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
			}
			if (Active)
			{
				DrawFIBHack("MORGUE_LAPTOP");
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, iLocal_8077))
				{
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iLocal_8077, "SET_PROGRESS_PERCENT");
					int amt = Amt;
					int num42 = 100;
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, amt);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, iLocal_8077, 255, 255, 255, 255, 0);
				}
			}
			if (active)
			{
				FingerprintHack.GUIOn = true;
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
				switch (indexx)
				{
				case 0:
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "hackingNG", 0);
					break;
				case 10:
					if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "hackingNG"))
					{
						indexx = 20;
					}
					break;
				case 20:
					activatescale("HACKING_PC");
					Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleformloading, 255, 255, 255, 255, 0);
					drawSprite2("hackingNG", "DHMain", 0.5f, 0.5f, 0.731f, 1.306f, 255, 255, 255, 255);
					if (currentbar == 0)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.35f, colOneY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.35f, colOneY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 1)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.39f, colTwoY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.39f, colTwoY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 2)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.43f, colThreeY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.43f, colThreeY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 3)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.47f, colFourY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.47f, colFourY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 4)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.51f, colFiveY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.51f, colFiveY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 5)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.55f, colSixY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.55f, colSixY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 6)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.59f, colSevenY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.59f, colSevenY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 7)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.63f, colEightY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.63f, colEightY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 2, 201) && firstbounce)
					{
						if (currentbar == 0)
						{
							if (check(colOneY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn1 = true;
								colOneY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 1;
							}
							if (check(colOneY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
							}
						}
						if (currentbar == 1 && Game.GameTime > GameTimeReference1)
						{
							if (check(colTwoY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn2 = true;
								colTwoY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 2;
							}
							if (check(colTwoY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 0;
								collumn1 = false;
							}
						}
						if (currentbar == 2 && Game.GameTime > GameTimeReference1)
						{
							if (check(colThreeY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn3 = true;
								colThreeY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 3;
							}
							if (check(colThreeY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 1;
								collumn2 = false;
							}
						}
						if (currentbar == 3 && Game.GameTime > GameTimeReference1)
						{
							if (check(colFourY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn4 = true;
								colFourY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 4;
							}
							if (check(colFourY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 2;
								collumn3 = false;
							}
						}
						if (currentbar == 4 && Game.GameTime > GameTimeReference1)
						{
							if (check(colFiveY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn5 = true;
								colFiveY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 5;
							}
							if (check(colFiveY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 3;
								collumn4 = false;
							}
						}
						if (currentbar == 5 && Game.GameTime > GameTimeReference1)
						{
							if (check(colSixY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn6 = true;
								colSixY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 6;
							}
							if (check(colSixY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 4;
								collumn5 = false;
							}
						}
						if (currentbar == 6 && Game.GameTime > GameTimeReference1)
						{
							if (check(colSevenY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn7 = true;
								colSevenY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 7;
							}
							if (check(colSevenY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 5;
								collumn6 = false;
							}
						}
						if (currentbar == 7 && Game.GameTime > GameTimeReference1)
						{
							if (check(colEightY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn8 = true;
								colEightY = 0.575f;
							}
							if (check(colEightY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 6;
								collumn7 = false;
							}
						}
					}
					if (collumn1 && collumn2 && collumn3 && collumn4 && collumn5 && collumn6 && collumn7 && collumn8)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
						active = false;
						hackedDataPad = true;
						colOneY = 0.4f;
						colTwoY = 0.4f;
						colThreeY = 0.4f;
						colFourY = 0.4f;
						colFiveY = 0.74f;
						colSixY = 0.74f;
						colSevenY = 0.74f;
						colEightY = 0.74f;
						collumn1 = false;
						collumn2 = false;
						collumn3 = false;
						collumn4 = false;
						collumn5 = false;
						collumn6 = false;
						collumn7 = false;
						collumn8 = false;
						skip1 = false;
						skip2 = false;
						skip3 = false;
						loadingin = true;
						firstbounce = false;
						up = false;
						up1 = false;
						up2 = false;
						up3 = false;
						up4 = true;
						up5 = true;
						up6 = true;
						up7 = true;
						currentbar = 0;
					}
					if (up && !collumn1)
					{
						if (colOneY > 0.4f)
						{
							colOneY -= 0.003f;
						}
						if (colOneY <= 0.4f)
						{
							up = false;
						}
					}
					if (!up && !collumn1)
					{
						if (colOneY < 0.74f)
						{
							colOneY += 0.003f;
						}
						if (colOneY >= 0.74f)
						{
							up = true;
						}
					}
					if (loadingin)
					{
						if (colOneY < 0.52f && colOneY > 0.47f && !up)
						{
							skip1 = true;
						}
						if (colOneY < 0.64f && colOneY > 0.59f && !up)
						{
							skip2 = true;
						}
						if ((double)colOneY < 0.75 && colOneY > 0.7f && !up)
						{
							skip3 = true;
							firstbounce = true;
							loadingin = false;
						}
					}
					if (skip1)
					{
						if (up1 && !collumn2)
						{
							if (colTwoY > 0.4f)
							{
								colTwoY -= 0.003f;
							}
							if (colTwoY <= 0.4f)
							{
								up1 = false;
							}
						}
						if (!up1 && !collumn2)
						{
							if (colTwoY < 0.74f)
							{
								colTwoY += 0.003f;
							}
							if (colTwoY >= 0.74f)
							{
								up1 = true;
							}
						}
					}
					if (skip2)
					{
						if (up2 && !collumn3)
						{
							if (colThreeY > 0.4f)
							{
								colThreeY -= 0.003f;
							}
							if (colThreeY <= 0.4f)
							{
								up2 = false;
							}
						}
						if (!up2 && !collumn3)
						{
							if (colThreeY < 0.74f)
							{
								colThreeY += 0.003f;
							}
							if (colThreeY >= 0.74f)
							{
								up2 = true;
							}
						}
					}
					if (skip3)
					{
						if (up3 && !collumn4)
						{
							if (colFourY > 0.4f)
							{
								colFourY -= 0.003f;
							}
							if (colFourY <= 0.4f)
							{
								up3 = false;
							}
						}
						if (!up3 && !collumn4)
						{
							if (colFourY < 0.74f)
							{
								colFourY += 0.003f;
							}
							if (colFourY >= 0.74f)
							{
								up3 = true;
							}
						}
					}
					if (skip3)
					{
						if (up4 && !collumn5)
						{
							if (colFiveY > 0.4f)
							{
								colFiveY -= 0.003f;
							}
							if (colFiveY <= 0.4f)
							{
								up4 = false;
							}
						}
						if (!up4 && !collumn5)
						{
							if (colFiveY < 0.74f)
							{
								colFiveY += 0.003f;
							}
							if (colFiveY >= 0.74f)
							{
								up4 = true;
							}
						}
					}
					if (skip2)
					{
						if (up5 && !collumn6)
						{
							if (colSixY > 0.4f)
							{
								colSixY -= 0.003f;
							}
							if (colSixY <= 0.4f)
							{
								up5 = false;
							}
						}
						if (!up5 && !collumn6)
						{
							if (colSixY < 0.74f)
							{
								colSixY += 0.003f;
							}
							if (colSixY >= 0.74f)
							{
								up5 = true;
							}
						}
					}
					if (skip1)
					{
						if (up6 && !collumn7)
						{
							if (colSevenY > 0.4f)
							{
								colSevenY -= 0.003f;
							}
							if (colSevenY <= 0.4f)
							{
								up6 = false;
							}
						}
						if (!up6 && !collumn7)
						{
							if (colSevenY < 0.74f)
							{
								colSevenY += 0.003f;
							}
							if (colSevenY >= 0.74f)
							{
								up6 = true;
							}
						}
					}
					if (up7 && !collumn8)
					{
						if (colEightY > 0.4f)
						{
							colEightY -= 0.003f;
						}
						if (colEightY <= 0.4f)
						{
							up7 = false;
						}
					}
					if (!up7 && !collumn8)
					{
						if (colEightY < 0.74f)
						{
							colEightY += 0.003f;
						}
						if (colEightY >= 0.74f)
						{
							up7 = true;
						}
					}
					break;
				}
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				DisplayHelpTextThisFrame("Press ~INPUT_FRONTEND_ACCEPT~ when the highlighted block is passing through the red line. Align all blocks to successfuly pass the hack.\nPress ~INPUT_CONTEXT~ to Exit", 1);
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
				if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237) || Game.IsControlJustPressed(GTA.Control.FrontendAccept))
				{
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT_SELECT");
					uLocal_70 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE, hackingScalwform);
				}
				else if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237) || Game.IsControlJustPressed(GTA.Control.FrontendAccept))
				{
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT_SELECT");
					uLocal_70 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE, hackingScalwform);
				}
				if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, uLocal_70))
				{
					int num = (FeedBackNumber = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, uLocal_70));
				}
				switch (Hacking_SelectedIndex)
				{
				case 0:
					Anim_Timer = Game.GameTime + 3000;
					Hacking_SelectedIndex = 10;
					break;
				case 10:
					if (Anim_Timer < Game.GameTime)
					{
						Hacking_SelectedIndex = 50;
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
					Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_STRING, hackingScalwform, "SET_ROULETTE_WORD", text, 0, 0, 0, 0);
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
						Hacking_SelectedIndex = 100;
					}
					if (!PlayBoth)
					{
						Hacking_SelectedIndex = 500;
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
						Hacking_SelectedIndex = 200;
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
							Hacking_SelectedIndex = 300;
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
						Hacking_SelectedIndex = 400;
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
						Hacking_SelectedIndex = 300;
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
					foreach (GTA.Control value in Enum.GetValues(typeof(GTA.Control)))
					{
						if (Game.IsControlJustPressed(value))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 2f, -1082130432, -1082130432, -1082130432, -1082130432);
							Hacking_SelectedIndex = 100;
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
					foreach (GTA.Control value2 in Enum.GetValues(typeof(GTA.Control)))
					{
						if (Game.IsControlJustPressed(value2))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 2f, -1082130432, -1082130432, -1082130432, -1082130432);
							Hacking_SelectedIndex = 500;
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
						Hacking_SelectedIndex = 600;
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
							Hacking_SelectedIndex = 700;
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
						Hacking_SelectedIndex = 800;
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
						Hacking_SelectedIndex = 700;
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
					foreach (GTA.Control value3 in Enum.GetValues(typeof(GTA.Control)))
					{
						if (Game.IsControlJustPressed(value3))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 1f, -1082130432, -1082130432, -1082130432, -1082130432);
							Hacking_SelectedIndex = 600;
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
					foreach (GTA.Control value4 in Enum.GetValues(typeof(GTA.Control)))
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
					Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, hackingScalwform, 255, 255, 255, 255, 0);
				}
			}
			if (Drill != null && Isdrilliing)
			{
				if (Game.IsControlPressed(GTA.Control.MoveDownOnly) || Game.IsControlPressed(GTA.Control.MoveUpOnly))
				{
					if (activeCasLaser)
					{
						if (!PlayDrillFXSound)
						{
							Function.Call(Hash.STOP_SOUND, Sound);
							Function.Call(Hash.STOP_SOUND, FailSound);
							Function.Call(Hash.RELEASE_SOUND_ID, Sound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, Sound, "laser_power_down", Drill, "dlc_ch_heist_finale_laser_drill_sounds", true, 20);
							PlayDrillFXSound = true;
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "fm_mission_controler");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "fm_mission_controler");
							DrillFX = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_drill_debris", Drill, 0f, -0.55f, 0.01f, 90f, 90f, 90f, 0.8f, 0, 0, 0);
							Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, DrillFX, "power", 0.7f, 0);
						}
						if (PlayDrillFXSound && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, Sound))
						{
							Function.Call(Hash.STOP_SOUND, Sound);
							Function.Call(Hash.STOP_SOUND, FailSound);
							Function.Call(Hash.RELEASE_SOUND_ID, Sound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
							PlayDrillFXSound = false;
							if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
							{
								Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
							}
						}
					}
					else
					{
						if (!PlayDrillFXSound)
						{
							Function.Call(Hash.STOP_SOUND, Sound);
							Function.Call(Hash.STOP_SOUND, FailSound);
							Function.Call(Hash.RELEASE_SOUND_ID, Sound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, Sound, "Drill", Drill, "DLC_HEIST_FLEECA_SOUNDSET", true, 20);
							PlayDrillFXSound = true;
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "fm_mission_controler");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "fm_mission_controler");
							DrillFX = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_drill_debris", Drill, 0f, -0.55f, 0.01f, 90f, 90f, 90f, 0.8f, 0, 0, 0);
							Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, DrillFX, "power", 0.7f, 0);
						}
						if (PlayDrillFXSound && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, Sound))
						{
							Function.Call(Hash.STOP_SOUND, Sound);
							Function.Call(Hash.STOP_SOUND, FailSound);
							Function.Call(Hash.RELEASE_SOUND_ID, Sound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
							PlayDrillFXSound = false;
						}
					}
				}
				if (!UseCasinoDrill && Temp >= 3f && (Game.IsControlPressed(GTA.Control.MoveDownOnly) || Game.IsControlPressed(GTA.Control.MoveUpOnly)))
				{
					Pos = 0f;
					Temp = 0f;
					Speed = 0f;
					DrillScreen(Zone);
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1))
					{
						Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
						Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
						Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, -1, AnimationFlags.Loop, -1f);
					}
				}
				if (UseCasinoDrill && temperature >= 1f && (Game.IsControlPressed(GTA.Control.MoveDownOnly) || Game.IsControlPressed(GTA.Control.MoveUpOnly)) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1))
				{
					Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
					Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, -1, AnimationFlags.Loop, -1f);
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
				}
				if (!UseCasinoDrill && !Game.IsControlPressed(GTA.Control.MoveDownOnly) && !Game.IsControlPressed(GTA.Control.MoveUpOnly) && !UseCasinoDrill)
				{
					Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_idle", 1f, 1f, -1, AnimationFlags.Loop, -1f);
					if (Speed >= 0f)
					{
						Speed -= 0.1f;
					}
					if (Temp >= 0f)
					{
						Temp -= 0.1f;
					}
					if (Temp <= 0f)
					{
						Temp = 0f;
					}
					DrillScreen(Zone);
				}
				if (!UseCasinoDrill)
				{
					if (triggerLockA && triggerLockB && triggerLockC && triggerLockD)
					{
						Function.Call(Hash.STOP_SOUND, Sound);
						Function.Call(Hash.STOP_SOUND, FailSound);
						Function.Call(Hash.RELEASE_SOUND_ID, Sound);
						Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
						DrillingComplete = true;
						Isdrilliing = false;
					}
					if (Temp <= 3f)
					{
						if (Game.IsControlPressed(GTA.Control.MoveDownOnly))
						{
							Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							Random random2 = new Random();
							Zone += random2.Next(1, 3);
							Temp += 0.015f;
							if (Temp < 1.8f)
							{
								Speed += 0.1f;
								if (Hackerskill == 0)
								{
									Pos += 0.0005f;
								}
								if (Hackerskill == 1)
								{
									Pos += 0.001f;
								}
								if (Hackerskill == 2)
								{
									Pos += 0.002f;
								}
								if (Hackerskill == 3)
								{
									Pos += 0.003f;
								}
								if (Hackerskill == 4)
								{
									Pos += 0.004f;
								}
								if (Hackerskill == 5)
								{
									Pos += 0.005f;
								}
							}
							else if (Temp >= 1.8f)
							{
								Speed += 0.05f;
								if (Hackerskill == 0)
								{
									Pos += 0.0002f;
								}
								if (Hackerskill == 1)
								{
									Pos += 0.0003f;
								}
								if (Hackerskill == 2)
								{
									Pos += 0.0005f;
								}
								if (Hackerskill == 3)
								{
									Pos += 0.0006f;
								}
								if (Hackerskill == 4)
								{
									Pos += 0.0007f;
								}
								if (Hackerskill == 5)
								{
									Pos += 0.0008f;
								}
							}
							DrillScreen(Zone);
							if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
							{
								Function.Call(Hash.STOP_SOUND, PinSound);
								Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
							}
							if (Pos >= 0.326f && !triggerLockA)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
								triggerLockA = true;
							}
							if (Pos >= 0.476f && !triggerLockB)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
								triggerLockB = true;
							}
							if (Pos >= 0.625f && !triggerLockC)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
								triggerLockC = true;
							}
							if (Pos >= 0.776f && !triggerLockD)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
								triggerLockD = true;
							}
							DrillScreen(Zone);
						}
						if (Game.IsControlPressed(GTA.Control.MoveUpOnly))
						{
							Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							Random random3 = new Random();
							Zone -= random3.Next(1, 3);
							Temp += 0.075f;
							if (Temp < 1.8f)
							{
								Speed += 0.05f;
								if (Hackerskill == 0)
								{
									Pos += 0.002f;
								}
								if (Hackerskill == 1)
								{
									Pos += 0.003f;
								}
								if (Hackerskill == 2)
								{
									Pos += 0.004f;
								}
								if (Hackerskill == 3)
								{
									Pos += 0.005f;
								}
								if (Hackerskill == 4)
								{
									Pos += 0.006f;
								}
								if (Hackerskill == 5)
								{
									Pos += 0.008f;
								}
							}
							else if (Temp >= 1.8f)
							{
								Speed += 0.01f;
								if (Hackerskill == 0)
								{
									Pos += 0.0005f;
								}
								if (Hackerskill == 1)
								{
									Pos += 0.0006f;
								}
								if (Hackerskill == 2)
								{
									Pos += 0.0007f;
								}
								if (Hackerskill == 3)
								{
									Pos += 0.0008f;
								}
								if (Hackerskill == 4)
								{
									Pos += 0.0009f;
								}
								if (Hackerskill == 5)
								{
									Pos += 0.001f;
								}
							}
							DrillScreen(Zone);
							if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
							{
								Function.Call(Hash.STOP_SOUND, PinSound);
								Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
							}
							if (Pos >= 0.326f && !triggerLockA)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
								triggerLockA = true;
							}
							if (Pos >= 0.476f && !triggerLockB)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
								triggerLockB = true;
							}
							if (Pos >= 0.625f && !triggerLockC)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
								triggerLockC = true;
							}
							if (Pos >= 0.776f && !triggerLockD)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
								triggerLockD = true;
							}
							DrillScreen(Zone);
						}
					}
				}
				if (UseCasinoDrill)
				{
					if (activeCasLaser && activatescaleCas("VAULT_LASER") == 1)
					{
						if (temperature <= 1.2f && Drill != null && Isdrilliing && (Game.IsControlPressed(GTA.Control.MoveUpOnly) || Game.IsControlPressed(GTA.Control.MoveDownOnly)) && temperature >= 1f)
						{
							Function.Call(Hash.STOP_SOUND, Sound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
							Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
							if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1))
							{
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							}
						}
						if (temperature <= 1f)
						{
							if (Game.IsControlPressed(GTA.Control.MoveUpOnly))
							{
								drillspeed += 0.25f;
								DrillPos += 0.025f;
								temperature += 0.225f;
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (Game.IsControlPressed(GTA.Control.MoveDownOnly))
							{
								drillspeed += 0.01f;
								DrillPos += 0.0001f;
								temperature += 0.125f;
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							}
						}
					}
					if (activeCasDrill && activatescaleCas("VAULT_DRILL") == 1)
					{
						if (temperature <= 1.2f && Drill != null && Isdrilliing && (Game.IsControlPressed(GTA.Control.MoveUpOnly) || Game.IsControlPressed(GTA.Control.MoveDownOnly)) && temperature >= 1f)
						{
							Function.Call(Hash.STOP_SOUND, Sound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
							Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
							if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1))
							{
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							}
						}
						if (temperature <= 1f)
						{
							if (Game.IsControlPressed(GTA.Control.MoveUpOnly))
							{
								Isdrilliing = true;
								drillspeed += 0.25f;
								DrillPos += 0.01f;
								temperature += 0.075f;
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							}
							if (Game.IsControlPressed(GTA.Control.MoveDownOnly))
							{
								Isdrilliing = true;
								drillspeed += 0.1f;
								DrillPos += 0.001f;
								temperature += 0.0025f;
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							}
						}
					}
				}
				if (UseCasinoDrill)
				{
					if (activeCasDrill && activatescaleCas("VAULT_DRILL") == 1 && temperature <= 100f)
					{
						if (!Game.IsControlPressed(GTA.Control.MoveUpOnly) && !Game.IsControlPressed(GTA.Control.MoveDownOnly))
						{
							Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_idle", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							DrillPos += 0f;
							drillspeed = 0f;
							if (temperature > 0f)
							{
								temperature -= 0.025f;
							}
							if (temperature < 0f)
							{
								temperature = 0f;
							}
						}
						float num3 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
						num3 = 0f - num3 + 0.9f;
						if (DrillPos > 0.242f && !pin_one)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_one = true;
							if (NumOfDiscs == 1f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 0.37f && !pin_two)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_two = true;
							if (NumOfDiscs == 2f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 0.54f && !pin_three)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_three = true;
							if (NumOfDiscs == 3f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 0.65f && !pin_four)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_four = true;
							if (NumOfDiscs == 4f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 0.85f && !pin_five)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_five = true;
							if (NumOfDiscs == 5f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 1f && !pin_six)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_six = true;
							if (NumOfDiscs == 6f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
					}
					if (activeCasLaser && activatescaleCas("VAULT_LASER") == 1 && temperature <= 100f)
					{
						if (!Game.IsControlPressed(GTA.Control.MoveUpOnly) && !Game.IsControlPressed(GTA.Control.MoveDownOnly))
						{
							Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_idle", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							DrillPos += 0f;
							drillspeed = 0f;
							if (temperature > 0f)
							{
								temperature -= 0.025f;
							}
							if (temperature < 0f)
							{
								temperature = 0f;
							}
						}
						float num4 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
						num4 = 0f - num4 + 0.9f;
						if (DrillPos > 0.242f && !pin_one)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_one = true;
							if (NumOfDiscs == 1f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 0.37f && !pin_two)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_two = true;
							if (NumOfDiscs == 2f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 0.54f && !pin_three)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_three = true;
							if (NumOfDiscs == 3f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 0.65f && !pin_four)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_four = true;
							if (NumOfDiscs == 4f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 0.85f && !pin_five)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_five = true;
							if (NumOfDiscs == 5f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
						if (DrillPos > 1f && !pin_six)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_six = true;
							if (NumOfDiscs == 6f)
							{
								Function.Call(Hash.STOP_SOUND, Sound);
								Function.Call(Hash.STOP_SOUND, FailSound);
								Function.Call(Hash.RELEASE_SOUND_ID, Sound);
								Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
					}
				}
				if (UseCasinoDrill)
				{
					if (activeCasLaser && activatescaleCas("VAULT_LASER") == 1)
					{
						runCasinoLaser(scaleformloading);
						if (!pin_one)
						{
							Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_LASER_2", 1);
						}
						if (pin_two)
						{
							Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_LASER_3", 1);
						}
						if (pin_three)
						{
							Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_LASER_4", 1);
						}
						if (pin_four)
						{
							Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_LASER_1", 1);
						}
					}
					if (activeCasDrill && activatescaleCas("VAULT_DRILL") == 1)
					{
						runCasinoDrill(scaleformloading);
						if (!pin_one)
						{
							Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_DRILL_4", 1);
						}
						if (pin_two)
						{
							Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_DRILL_3", 1);
						}
						if (pin_three)
						{
							Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_DRILL_2", 1);
						}
						if (pin_four)
						{
							Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "MC_DRILL_1", 1);
						}
					}
				}
				if (!UseCasinoDrill)
				{
					DrillScreen(Zone);
				}
			}
			if (!MissionOn)
			{
				return;
			}
			UI.RenderQue = 0f;
			switch (HeistStage)
			{
			case 4:
			{
				if (Game.Player.IsAlive)
				{
					foreach (KeypadClass keyPad in KeyPads)
					{
						if (!keyPad.SpawnedKeypad && !keyPad.SpawnedKeypad)
						{
							keyPad.SpawnedKeypad = true;
							keyPad.Keypad = GlobalHelper.CreateProp(RequestModel("ch_prop_fingerprint_scanner_01c"), keyPad.KeypadPos, new Vector3(0f, 0f, keyPad.KeypadRot), false, false);
							keyPad.Keypad.IsPositionFrozen = true;
							keyPad.Keypad.AddBlip();
							keyPad.Keypad.AttachedBlip.Sprite = BlipSprite.Keypad2;
							keyPad.Keypad.AttachedBlip.Scale = 0.75f;
							keyPad.Keypad.AttachedBlip.IsShortRange = true;
							Script.Wait(1);
							if (GetDistance_VDIST2(keyPad.Keypad.Position, new Vector3(2519.19f, -281.8895f, -64.6f), "<", 5f))
							{
								keyPad.Keypad.Position = keyPad.Keypad.GetOffsetPosition(new Vector3(0f, -0.007f, 0f));
								Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, keyPad.Keypad, 277761, 554788825);
							}
							if (GetDistance_VDIST2(keyPad.Keypad.Position, new Vector3(2516.78f, -259.7819f, -38.9f), "<", 5f))
							{
								keyPad.Keypad.Position = keyPad.Keypad.GetOffsetPosition(new Vector3(0f, 0.002f, 0f));
							}
						}
						if (!keyPad.SpawnedKeypad)
						{
							continue;
						}
						float num5 = keyPad.KeypadPos.Z - 3f;
						float num6 = keyPad.KeypadPos.Z + 3f;
						if (GetDistance_VDIST2(Game.Player.Character.Position, keyPad.KeypadPos, ">", 30f))
						{
							keyPad.Keypad.AttachedBlip.Alpha = 0;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, keyPad.KeypadPos, "<", 30f))
						{
							if (Game.Player.Character.Position.Z > num5 && Game.Player.Character.Position.Z < num6)
							{
								keyPad.Keypad.AttachedBlip.Alpha = 255;
							}
							else
							{
								keyPad.Keypad.AttachedBlip.Alpha = 0;
							}
						}
						if (!HackingKeypad)
						{
							if (!GetDistance_VDIST2(Game.Player.Character.Position, keyPad.KeypadPos, "<", 10f))
							{
								continue;
							}
							Vector3 offsetPosition = keyPad.Keypad.GetOffsetPosition(new Vector3(0f, -1f, 0f));
							if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, "<", 2f))
							{
								DisplayHelpTextThisFrame("~INPUT_CONTEXT~ Hack Keypad");
								if (Game.IsControlJustPressed(GTA.Control.Context) && !HackingKeypad)
								{
									LastKeypad = keyPad.Keypad;
									if (Phone != null)
									{
										Phone.Delete();
									}
									if (USB != null)
									{
										USB.Delete();
									}
									Phone = GlobalHelper.CreateProp(RequestModel("prop_phone_ing"), keyPad.KeypadPos, false, false);
									USB = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_usb_drive01x"), keyPad.KeypadPos, false, false);
									USB.IsVisible = false;
									Phone.IsVisible = false;
									USB.IsVisible = true;
									Phone.IsVisible = true;
									int index = Game.Player.Character.Bones[Bone.PHLeftHand].Index;
									LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, keyPad.Keypad.Position.X, keyPad.Keypad.Position.Y, keyPad.Keypad.Position.Z, keyPad.Keypad.Rotation.X, keyPad.Keypad.Rotation.Y, keyPad.Keypad.Rotation.Z, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "action_var_01", 0.0, 0.0, 0, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "action_var_01_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "action_var_01_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
									keyPad.AwaitingHackGUI = true;
								}
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353200 && keyPad.AwaitingHackGUI)
							{
								Scene1 = -1;
								if (keyPad.Type == 1)
								{
									FingerprintHack.RunKeypadHack();
								}
								if (keyPad.Type == 2)
								{
									FingerprintHack.RunFingerPrintHack();
								}
								keyPad.AwaitingHackGUI = false;
								HackingKeypad = true;
							}
						}
						else
						{
							if (!HackingKeypad)
							{
								continue;
							}
							if (Game.IsControlJustPressed(GTA.Control.Context) && HackingKeypad)
							{
								keyPad.AwaitingHackGUI = false;
								LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@");
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, LastKeypad.Position.X, LastKeypad.Position.Y, LastKeypad.Position.Z, LastKeypad.Rotation.X, LastKeypad.Rotation.Y, LastKeypad.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim_heist@hs3f@ig1_hack_keypad@male@", "exit", 0.0, 0.0, 0, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Phone, Scene1, "exit_prop_phone_ing", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, USB, Scene1, "exit_ch_prop_ch_usb_drive01x", LoadDict("anim_heist@hs3f@ig1_hack_keypad@male@"), 1000f, 0f, 0, 1000f);
							}
							if (!GetDistance_VDIST2(Game.Player.Character.Position, keyPad.KeypadPos, "<", 5f))
							{
								continue;
							}
							if (FingerprintHack.HackFinishedID == 2)
							{
								keyPad.AwaitingHackGUI = false;
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
								keyPad.Keypad.AttachedBlip.Alpha = 0;
								keyPad.AwaitingHackGUI = false;
								FingerprintHack.HackFinishedID = 0;
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(994.1423f, 75.17f, 81.3f), "<", 2f))
								{
									HackedWasteDisposalDoor = true;
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, FloorVaultElevator, "<", 5f) || GetDistance_VDIST2(Game.Player.Character.Position, FloorVaultOfficeElevator, "<", 5f))
								{
									UnlockedVaultToOffice = true;
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, FloorOfficeElevator, "<", 5f) || GetDistance_VDIST2(Game.Player.Character.Position, FloorHotelElevator, "<", 5f) || GetDistance_VDIST2(Game.Player.Character.Position, FloorLoadingBayElevator, "<", 5f))
								{
									UnlockedThreeFloorBypass = true;
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, StaffDoor, "<", 5f))
								{
									UnlockedStaffDoor = true;
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
								keyPad.AwaitingHackGUI = false;
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
					if (IsInterior && PoliceAlearted && !TriggeredWantedLevelEscape)
					{
						if (AlarmsGoingOff || PoliceAlearted)
						{
							if (!OverideAlarm && !Game.Player.Character.IsInCombat)
							{
								OverideAlarm = true;
								Function.Call(Hash.REMOVE_CUTSCENE);
								int num7 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs3f_all_esc");
								Function.Call(Hash.START_CUTSCENE, num7);
								Script.Wait(12000);
								SetupSwat();
							}
							if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
							{
								foreach (ObjectPlacementMap swatEntty in SwatEntties)
								{
									swatEntty.CreateObjatCoords();
									if (swatEntty.Entity != null && swatEntty.Entity.Model.IsPed)
									{
										DiamdondCasinoSmartGuards.SpawnGroupsUse.Add(new SmartGuards.SmartAiV2((Ped)swatEntty.Entity, SmartGuards.EnemyGroup.Diamond_Casino_Security));
									}
								}
								TriggerFullResponce = true;
								Notification.Show("~y~Lester~w~ Police Alerted Diamond Casino Security to a possible intruder, Police have setup roadblocks and swat teams around the casino");
								SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
								TriggeredWantedLevelEscape = true;
							}
						}
						AlarmTimer = 0;
					}
					CurrentPay = ManageRobberyAnims.CurrentPay;
					Vector3 position = Game.Player.Character.Position;
					if (position.Z > -72f && position.Z < -66.5f)
					{
						CurrentFloor = FloorOn.Vault;
					}
					if (position.Z > -66.5f && position.Z < -60.5f)
					{
						CurrentFloor = FloorOn.LoadingBay;
					}
					if (position.Z > -60.5f && position.Z < -45.5f)
					{
						CurrentFloor = FloorOn.Offices;
					}
					if (position.Z > -45.5f && position.Z < -20f)
					{
						CurrentFloor = FloorOn.Hotel;
					}
					ManageDoor(new Vector3(2503.288f, -276.8731f, -58.57f), "ch_prop_ch_service_door_02a", false);
					if (UnlockedVaultEnterance)
					{
						ManageDoor(new Vector3(2464.183f, -280.2885f, -71.6943f), "ch_prop_ch_tunnel_door_01_r", false);
						ManageDoor(new Vector3(2464.183f, -278.2036f, -71.6943f), "ch_prop_ch_tunnel_door_01_l", false);
					}
					if (UnlockedVaultToOffice)
					{
						ManageDoor(new Vector3(2485.624f, -252.9468f, -71.68795f), "v_ilev_garageliftdoor", false);
						ManageDoor(new Vector3(2485.624f, -254.4468f, -71.68795f), "v_ilev_garageliftdoor", false);
						ManageDoor(new Vector3(2516.783f, -265.0122f, -59.72355f), "v_ilev_garageliftdoor", false);
						ManageDoor(new Vector3(2518.283f, -265.0122f, -59.72355f), "v_ilev_garageliftdoor", false);
						if (GetDistance_VDIST2(Game.Player.Character.Position, FloorVaultElevator, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, FloorVaultElevator, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.Goldenrod);
							if (GetDistance_VDIST2(Game.Player.Character.Position, FloorVaultElevator, "<", 2f))
							{
								DisplayHelpTextThisFrame("~INPUT_CONTEXT~ Take Elevator Up to Offices");
								if (Game.IsControlJustPressed(GTA.Control.Context) && GetDistance_VDIST2(Game.Player.Character.Position, FloorVaultElevator, "<", 2f))
								{
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									Script.Wait(1);
									Game.Player.Character.Position = FloorVaultOfficeElevator;
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FloorVaultOfficeElevator, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, FloorVaultOfficeElevator, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.Goldenrod);
							if (GetDistance_VDIST2(Game.Player.Character.Position, FloorVaultOfficeElevator, "<", 2f))
							{
								DisplayHelpTextThisFrame("~INPUT_CONTEXT~ Take Elevator Down to Vault");
								if (Game.IsControlJustPressed(GTA.Control.Context) && GetDistance_VDIST2(Game.Player.Character.Position, FloorVaultOfficeElevator, "<", 2f))
								{
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									Script.Wait(1);
									Game.Player.Character.Position = FloorVaultElevator;
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
								}
							}
						}
					}
					else if (UnlockedThreeFloorBypass)
					{
						ManageDoor(new Vector3(2520.204f, -280.1398f, -59.72355f), "v_ilev_garageliftdoor", false);
						ManageDoor(new Vector3(2520.204f, -278.6398f, -59.72355f), "v_ilev_garageliftdoor", false);
						ManageDoor(new Vector3(2520.25f, -280.0848f, -65.72307f), "ch_prop_ch_bay_elev_door", false);
						ManageDoor(new Vector3(2520.25f, -278.0948f, -65.72307f), "ch_prop_ch_bay_elev_door", false);
						ManageDoor(new Vector3(2519.433f, -260.6313f, -40.12687f), "v_ilev_garageliftdoor", false);
						ManageDoor(new Vector3(2517.933f, -260.6313f, -40.12687f), "v_ilev_garageliftdoor", false);
						if (GetDistance_VDIST2(Game.Player.Character.Position, FloorHotelElevator, "<", 10f))
						{
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FloorOfficeElevator, "<", 10f))
						{
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FloorLoadingBayElevator, "<", 10f))
						{
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FloorLoadingBayElevator, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, FloorLoadingBayElevator, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.Goldenrod);
							if (GetDistance_VDIST2(Game.Player.Character.Position, FloorLoadingBayElevator, "<", 2f))
							{
								DisplayHelpTextThisFrame("~INPUT_FRONTEND_UP~ Take Elevator Up to Offices");
								if (Game.IsControlJustPressed(GTA.Control.FrontendUp) && GetDistance_VDIST2(Game.Player.Character.Position, FloorLoadingBayElevator, "<", 2f))
								{
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									Script.Wait(1);
									Game.Player.Character.Position = FloorOfficeElevator;
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FloorOfficeElevator, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, FloorOfficeElevator, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.Goldenrod);
							if (GetDistance_VDIST2(Game.Player.Character.Position, FloorOfficeElevator, "<", 2f))
							{
								DisplayHelpTextThisFrame("~INPUT_FRONTEND_UP~ Take Elevator Up to Hotel\n~INPUT_FRONTEND_DOWN~ Take Elevator Down to Loading Bay");
								if (Game.IsControlJustPressed(GTA.Control.FrontendUp) && GetDistance_VDIST2(Game.Player.Character.Position, FloorOfficeElevator, "<", 2f))
								{
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									Script.Wait(1);
									Game.Player.Character.Position = FloorHotelElevator;
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
								}
								if (Game.IsControlJustPressed(GTA.Control.FrontendDown) && GetDistance_VDIST2(Game.Player.Character.Position, FloorOfficeElevator, "<", 2f))
								{
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									Script.Wait(1);
									Game.Player.Character.Position = FloorLoadingBayElevator;
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
								}
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, FloorHotelElevator, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, FloorHotelElevator, Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.Goldenrod);
							if (GetDistance_VDIST2(Game.Player.Character.Position, FloorHotelElevator, "<", 2f))
							{
								DisplayHelpTextThisFrame("~INPUT_FRONTEND_DOWN~ Take Elevator Down to Offices");
								if (Game.IsControlJustPressed(GTA.Control.FrontendDown) && GetDistance_VDIST2(Game.Player.Character.Position, FloorHotelElevator, "<", 2f))
								{
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									Script.Wait(1);
									Game.Player.Character.Position = FloorOfficeElevator;
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
									}
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
								}
							}
						}
					}
					else
					{
						ManageDoor(new Vector3(2520.204f, -280.1398f, -59.72355f), "v_ilev_garageliftdoor", true);
						ManageDoor(new Vector3(2520.204f, -278.6398f, -59.72355f), "v_ilev_garageliftdoor", true);
						ManageDoor(new Vector3(2520.25f, -280.0848f, -65.72307f), "ch_prop_ch_bay_elev_door", true);
						ManageDoor(new Vector3(2520.25f, -278.0948f, -65.72307f), "ch_prop_ch_bay_elev_door", true);
						ManageDoor(new Vector3(2519.433f, -260.6313f, -40.12687f), "v_ilev_garageliftdoor", true);
						ManageDoor(new Vector3(2517.933f, -260.6313f, -40.12687f), "v_ilev_garageliftdoor", true);
					}
					if (IsInterior)
					{
						if (!LesterToldPlayerAboutContractCut)
						{
							Notification.Show("~y~Lester~w~ the Contractor has asked as to take at least ~g~$" + PercentNeedTake.ToString("N") + "~w~ He also wishes that you NOT alert the cops, or his cut will increase dramatically");
							LesterToldPlayerAboutContractCut = true;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, VaultPos, "<", 30f))
						{
							if (ManageRobberyAnims.CurrentPay >= PercentNeedTake && !VaultClosed)
							{
								VaultOpen = true;
								GTA.UI.Screen.ShowSubtitle("Keep Looting more ~g~Valuables~w~ or Leave when ready");
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
							else if (!VaultClosed)
							{
								GTA.UI.Screen.ShowSubtitle("Loot at least ~g~$" + PercentNeedTake.ToString("N") + "~w~ From the ~y~Diamond Casino's Vault");
								if (IsInterior)
								{
									if (!OverideAlarm)
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
										UI.DrawTime("COPS ARRIVE", Waittime - Game.GameTime);
										UI.DrawPayment("TAKE", LocalTake);
									}
									else
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
									}
								}
								else
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
								}
							}
							else if (EnteranceUsed != 1 && EnteranceUsed != 4 && GetDistance_VDIST2(Game.Player.Character.Position, VaultPos, "<", 5f))
							{
								GTA.UI.Screen.ShowSubtitle("Use the ~o~Thermal Drill~w~, ~b~Normal Drill~w~ or ~r~C4~w~ to Open the Vault");
							}
						}
						else if (ManageRobberyAnims.CurrentPay >= PercentNeedTake && !VaultClosed)
						{
							GTA.UI.Screen.ShowSubtitle("Leave the ~y~Diamond Casino~w~");
						}
						else if (EnteranceUsed == 1)
						{
							if (!CompletedGroupe6Tasks)
							{
								ManageDoor(new Vector3(2464.183f, -280.2885f, -71.6943f), "ch_prop_ch_tunnel_door_01_r", true);
								ManageDoor(new Vector3(2464.183f, -278.2036f, -71.6943f), "ch_prop_ch_tunnel_door_01_l", true);
								if (Groupe6TaskStage == 0)
								{
									GTA.UI.Screen.ShowSubtitle("Make you way down to the ~y~Vault~w~ and talk to the ~d~Security Guard~w~ at the Security Desk");
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2489.3f, -276.5f, -71.6f), "<", 2f) && World.GetClosestPed(new Vector3(2488.01f, -275.4f, -71.5f), 3f) != null)
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to  talk to the Security Guard~w~");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											Groupe6TaskStage = 1;
											Notification.Show("~r~Security Guard~w~ Weve been getting some strange issues with out security System lately can you check them out?");
											foreach (ObjectPlacementMap groupe6Cam in Groupe6Cams)
											{
												if (groupe6Cam.Entity != null)
												{
													groupe6Cam.Entity.AddBlip();
													groupe6Cam.Entity.AttachedBlip.Color = BlipColor.Blue;
													groupe6Cam.Entity.AttachedBlip.Name = "Investigate Camera";
												}
											}
										}
									}
								}
								if (Groupe6TaskStage == 1)
								{
									GTA.UI.Screen.ShowSubtitle("Investigate the ~b~Cameras~w~ around the ~y~Vault Enterance~w~ and talk to the ~d~Security Guard~w~ when done.");
									foreach (ObjectPlacementMap groupe6Cam2 in Groupe6Cams)
									{
										if (!(groupe6Cam2.Entity != null) || !GetDistance_VDIST2(Game.Player.Character.Position, groupe6Cam2.Entity.Position, "<", 3f))
										{
											continue;
										}
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Investigate Camera");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											Game.Player.Character.Task.PlayAnimation("anim@amb@business@bgen@bgen_inspecting@", "inspecting_high_idle_02_inspector", 1f, 1f, -1, AnimationFlags.None, 1f);
											if (groupe6Cam2.Entity.AttachedBlip != null)
											{
												groupe6Cam2.Entity.AttachedBlip.Alpha = 0;
											}
										}
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2489.3f, -276.5f, -71.6f), "<", 2f) && World.GetClosestPed(new Vector3(2488.01f, -275.4f, -71.5f), 3f) != null)
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to talk to the Security Guard~w~");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											bool flag = true;
											foreach (ObjectPlacementMap groupe6Cam3 in Groupe6Cams)
											{
												if (groupe6Cam3.Entity != null && groupe6Cam3.Entity.AttachedBlip != null && groupe6Cam3.Entity.AttachedBlip.Alpha == 255)
												{
													flag = false;
												}
											}
											if (flag)
											{
												Groupe6TaskStage = 2;
												CompletedGroupe6Tasks = true;
												Notification.Show("~r~Security Guard~w~ All clear? good, head over to the vault, ive given you access to get though the security doors");
											}
											else
											{
												Notification.Show("~r~Security Guard~w~ Come back when you have cleared the room!");
											}
										}
									}
								}
								if (Groupe6TaskStage == 2)
								{
									GTA.UI.Screen.ShowSubtitle("Go to the ~y~Vault~w~");
								}
							}
							else
							{
								ManageDoor(new Vector3(2464.183f, -280.2885f, -71.6943f), "ch_prop_ch_tunnel_door_01_r", false);
								ManageDoor(new Vector3(2464.183f, -278.2036f, -71.6943f), "ch_prop_ch_tunnel_door_01_l", false);
								GTA.UI.Screen.ShowSubtitle("Go to the ~y~Vault~w~");
							}
						}
						else if (EnteranceUsed == 5)
						{
							if (!CompletedBugstarTasks)
							{
								ManageDoor(new Vector3(2464.183f, -280.2885f, -71.6943f), "ch_prop_ch_tunnel_door_01_r", true);
								ManageDoor(new Vector3(2464.183f, -278.2036f, -71.6943f), "ch_prop_ch_tunnel_door_01_l", true);
								if (BugstarTaskStage == 0)
								{
									GTA.UI.Screen.ShowSubtitle("Make you way down to the ~y~Vault~w~ and talk to the ~d~Security Guard~w~ at the Security Desk");
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2489.3f, -276.5f, -71.6f), "<", 2f) && World.GetClosestPed(new Vector3(2488.01f, -275.4f, -71.5f), 3f) != null)
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to  talk to the Security Guard~w~");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											BugstarTaskStage = 1;
											Notification.Show("~r~Security Guard~w~ Weve been getting some strange smells from the vents around here, take a look why dont you?");
											foreach (ObjectPlacementMap bugStarVent in BugStarVents)
											{
												if (bugStarVent.Entity != null)
												{
													bugStarVent.Entity.AddBlip();
													bugStarVent.Entity.AttachedBlip.Color = BlipColor.Blue;
													bugStarVent.Entity.AttachedBlip.Name = "Investigate Vent";
												}
											}
										}
									}
								}
								if (BugstarTaskStage == 1)
								{
									GTA.UI.Screen.ShowSubtitle("Investigate the ~b~Vents~w~ around the ~y~Vault Enterance~w~ and talk to the ~d~Security Guard~w~ when done.");
									foreach (ObjectPlacementMap bugStarVent2 in BugStarVents)
									{
										if (!(bugStarVent2.Entity != null) || !GetDistance_VDIST2(Game.Player.Character.Position, bugStarVent2.Entity.Position, "<", 2f))
										{
											continue;
										}
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Investigate Vent");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											Game.Player.Character.Task.PlayAnimation("anim@amb@business@bgen@bgen_inspecting@", "inspecting_high_idle_02_inspector", 1f, 1f, -1, AnimationFlags.None, 1f);
											if (bugStarVent2.Entity.AttachedBlip != null)
											{
												bugStarVent2.Entity.AttachedBlip.Alpha = 0;
											}
										}
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2489.3f, -276.5f, -71.6f), "<", 2f) && World.GetClosestPed(new Vector3(2488.01f, -275.4f, -71.5f), 3f) != null)
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to talk to the Security Guard~w~");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											bool flag2 = true;
											foreach (ObjectPlacementMap bugStarVent3 in BugStarVents)
											{
												if (bugStarVent3.Entity != null && bugStarVent3.Entity.AttachedBlip != null && bugStarVent3.Entity.AttachedBlip.Alpha == 255)
												{
													flag2 = false;
												}
											}
											if (flag2)
											{
												BugstarTaskStage = 2;
												CompletedBugstarTasks = true;
												Notification.Show("~r~Security Guard~w~ All clear? good, head over to the vault, ive given you access to get though the security doors");
											}
											else
											{
												Notification.Show("~r~Security Guard~w~ Come back when you have cleared the room!");
											}
										}
									}
								}
								if (BugstarTaskStage == 2)
								{
									GTA.UI.Screen.ShowSubtitle("Go to the ~y~Vault~w~");
								}
							}
							else
							{
								ManageDoor(new Vector3(2464.183f, -280.2885f, -71.6943f), "ch_prop_ch_tunnel_door_01_r", false);
								ManageDoor(new Vector3(2464.183f, -278.2036f, -71.6943f), "ch_prop_ch_tunnel_door_01_l", false);
								GTA.UI.Screen.ShowSubtitle("Go to the ~y~Vault~w~");
							}
						}
						else if (ManageRobberyAnims.CurrentPay <= PercentNeedTake)
						{
							if (!PoliceAlearted)
							{
								GTA.UI.Screen.ShowSubtitle("Sneak your way down to the ~y~Vault~w~");
							}
							else
							{
								GTA.UI.Screen.ShowSubtitle("Fight your past all the ~r~Guards~w~ way down to the ~y~Vault~w~");
							}
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Leave the ~y~Diamond Casino~w~");
						}
					}
					else if (VaultOpen || ManageRobberyAnims.CurrentPay >= 1000)
					{
						if (Game.Player.WantedLevel >= 1)
						{
							GTA.UI.Screen.ShowSubtitle("Lose the ~r~Cops");
						}
						else
						{
							GTA.UI.Screen.ShowSubtitle("Return to the ~y~Arcade");
						}
					}
					else if (Game.Player.WantedLevel >= 1)
					{
						GTA.UI.Screen.ShowSubtitle("Lose the ~r~Cops");
					}
					else
					{
						if (EnteranceUsed == 1)
						{
							GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Diamond Casino's Loading Bay ~w~ in the ~b~Stockade~w~");
						}
						if (EnteranceUsed == 2)
						{
							GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Sewer Tunnel Enterance~w~ to access the ~y~Diamond Casino");
						}
						if (EnteranceUsed == 3)
						{
							GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Diamond Casino's Roof VIP Access");
						}
						if (EnteranceUsed == 4)
						{
							GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Diamond Casino's Roof VIP Access");
						}
						if (EnteranceUsed == 5 && ExitUsed == 4)
						{
							GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Diamond Casino's Loading Bay ~w~ in the ~b~Bugstars Van~w~");
						}
						if (EnteranceUsed == 6)
						{
							GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Diamond Casino Main Enterance");
						}
						if (EnteranceUsed == 7)
						{
							GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Diamond Casino's Waste Disposal Enterance");
						}
						if (EnteranceUsed == 8)
						{
							GTA.UI.Screen.ShowSubtitle("Make your way over to the ~y~Diamond Casino's Office Side Door");
						}
					}
					if (Surrendered)
					{
						MissionOn = false;
						Surrendered = false;
						Script.Wait(1000);
						foreach (Prop prop40 in Props)
						{
							if (prop40 != null)
							{
								prop40.Delete();
							}
						}
						foreach (Prop smokeEmitter in SmokeEmitters)
						{
							if (smokeEmitter != null)
							{
								smokeEmitter.Delete();
							}
						}
						if (HackConsole != null)
						{
							HackConsole.Delete();
						}
						hackedDataPad = false;
						if (VaultDoor != null)
						{
							VaultDoor.Delete();
						}
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
						if (SecondaryGetawayVeh != null)
						{
							SecondaryGetawayVeh.Delete();
						}
						if (DeliveryPoint != null)
						{
							DeliveryPoint.Delete();
						}
						ManageRobberyAnims.DeleteLoot();
						if (Bank != null)
						{
							Bank.Delete();
						}
						if (GetawayCar != null)
						{
							GetawayCar.Delete();
						}
						foreach (Prop prop41 in Props)
						{
							if (prop41 != null)
							{
								prop41.Delete();
							}
						}
						if (RobberC != null)
						{
							RobberC.Delete();
						}
						if (RobberB != null)
						{
							RobberB.Delete();
						}
						foreach (Ped aircarftCarrierPed in AircarftCarrierPeds)
						{
							if (aircarftCarrierPed != null)
							{
								aircarftCarrierPed.Delete();
							}
						}
						foreach (Vehicle aircraftCarrierVehicle in AircraftCarrierVehicles)
						{
							if (aircraftCarrierVehicle != null)
							{
								aircraftCarrierVehicle.Delete();
							}
						}
						if (WheelProp != null)
						{
							WheelProp.Delete();
						}
						if (WheelProp2 != null)
						{
							WheelProp2.Delete();
						}
						if (BasePlate != null)
						{
							BasePlate.Delete();
						}
						foreach (Ped item in MainPed)
						{
							if (item != null)
							{
								item.Delete();
							}
						}
						foreach (Prop table in Tables)
						{
							if (table != null)
							{
								table.Delete();
							}
						}
						foreach (Prop item2 in Tables2)
						{
							if (item2 != null)
							{
								item2.Delete();
							}
						}
						if (Prizecar != null)
						{
							Prizecar.Delete();
						}
						foreach (Ped item3 in SMPed)
						{
							if (item3 != null)
							{
								item3.Delete();
							}
						}
						FreezeGetawayCar = false;
						FreezeGetawayCar2 = false;
						TriggerNotifyNear = false;
						PlacedBomb = false;
						EscapingCasino = false;
						VaultClosed = true;
						VaultDone = false;
						MetalDetectorOn = true;
						IsInterior = false;
						BypassMetalDetector = false;
						NotifyOutVault2 = false;
						AlarmsGoingOff = false;
						GotCash = false;
						TriggeredWantedLevelEscape = false;
						IsPlanting = false;
						NotifyOutVault = false;
						CurrentPay = 0;
						ManageRobberyAnims.CurrentPay = 0;
						OverideAlarm = false;
						AlarmsGoingOff = false;
						HeadingtoyourAppartment = false;
						SetSewerBomb = false;
						DetonatedSewerBomb = false;
						EnteredNormalCasino = false;
						EnteredOffice = false;
						TriggeredAttack = false;
						AlarmTimer = 0;
						InNormalCasino = false;
						Game.Player.Character.Opacity = 0;
						Vector3 position2 = Game.Player.Character.Position;
						Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
						Script.Wait(1);
						Game.Player.Character.Position = position2;
						Game.Player.Character.Opacity = 255;
						MissionOn = false;
						MissionScreen.SetFail("", 0, 0, "Player was Caught", 0, HeistTime, HeistMissson);
						PoliceAlearted = false;
						LesterToldPlayerAboutContractCut = false;
						foreach (ObjectPlacementMap swatEntty2 in SwatEntties)
						{
							if (swatEntty2.Entity != null)
							{
								swatEntty2.Entity.Delete();
							}
						}
						foreach (ObjectPlacementMap bugStarVent4 in BugStarVents)
						{
							if (bugStarVent4.Entity != null)
							{
								bugStarVent4.Entity.Delete();
							}
						}
						foreach (ObjectPlacementMap groupe6Cam4 in Groupe6Cams)
						{
							if (groupe6Cam4.Entity != null)
							{
								groupe6Cam4.Entity.Delete();
							}
						}
						SmartGuards.ClearGenericGuards();
						ManageRobberyAnims.Crate_loc.Clear();
						ManageRobberyAnims.Crate_Rot.Clear();
						MissionOn = false;
						Game.Player.Character.Position = CheckAppartmentPos();
					}
					if (PlayingCutscene)
					{
						if (CutsenePed2 != null && RobberA == null)
						{
							CutsenePed2.IsVisible = false;
						}
						if (CutsenePed3 != null && RobberB == null)
						{
							CutsenePed3.IsVisible = false;
						}
						if (CutsenePed4 != null && RobberC == null)
						{
							CutsenePed4.IsVisible = false;
						}
					}
					if (EnteranceUsed == 6)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(935.65f, 46.81f, 80f), "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, new Vector3(935.65f, 46.81f, 80f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(935.65f, 46.81f, 80f), "<", 2f))
						{
							Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs3f_dir_ent");
							if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene)
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
								if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
								{
									Ped[] nearbyPeds = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
									if (nearbyPeds.Length != 0)
									{
										Ped[] array = nearbyPeds;
										foreach (Ped ped in array)
										{
											if (ped != null)
											{
												ped.IsVisible = false;
											}
										}
									}
									int num8 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_Vault_Door");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "set_vault_dressing");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_vault_cash_01");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_vault_cash_02");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_vault_gold_01");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_vault_gold_02");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_vault_art_01");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_vault_art_02");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_vault_diamonds_01");
									Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num8, "Set_vault_diamonds_02");
									Function.Call(Hash.REFRESH_INTERIOR, num8);
									SetEntityinCustscenes();
									Function.Call(Hash.START_CUTSCENE, 0);
									Script.Wait(100);
									PlayingCutscene = true;
									GetPedOutfitCutscene("MP_1", Game.Player.Character);
									if (RobberA != null)
									{
										GetPedOutfitCutscene("MP_2", RobberA);
									}
									if (RobberB != null)
									{
										GetPedOutfitCutscene("MP_3", RobberB);
									}
									if (RobberC != null)
									{
										GetPedOutfitCutscene("MP_4", RobberC);
									}
									TriggerFullResponce = true;
									Notification.Show("~y~ Lester ~w~ : guards have been alerted, they are now on high alert for any intruders ");
								}
							}
						}
						if (CutsenePed2 != null && RobberA == null)
						{
							CutsenePed2.IsVisible = false;
						}
						if (CutsenePed3 != null && RobberB == null)
						{
							CutsenePed3.IsVisible = false;
						}
						if (CutsenePed4 != null && RobberC == null)
						{
							CutsenePed4.IsVisible = false;
						}
						if (PlayingCutscene)
						{
							if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
							{
								Function.Call(Hash.STOP_CUTSCENE, true);
							}
							if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && PlayingCutscene)
							{
								Function.Call(Hash.REMOVE_CUTSCENE);
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
								if (RobberA != null)
								{
									RobberA.IsVisible = true;
								}
								if (RobberB != null)
								{
									RobberB.IsVisible = true;
								}
								if (RobberC != null)
								{
									RobberC.IsVisible = true;
								}
								TriggerFullResponce = true;
								Notification.Show("~y~ Lester ~w~ : Ok guards are now on high aleart! get out of the Diamond Casino while you still can");
							}
						}
					}
					if (EnteranceUsed == 7 && HackedWasteDisposalDoor)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(993.13f, 76.7f, 79.8f), "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, new Vector3(993.13f, 76.7f, 79.8f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(993.13f, 76.7f, 79.8f), "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								GTA.UI.Screen.FadeOut(300);
								Script.Wait(300);
								BypassMetalDetector = false;
								IsInterior = true;
								Script.Wait(1);
								Game.Player.Character.Position = new Vector3(2541.024f, -212.23f, -59f);
								if (RobberA != null)
								{
									RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(1f, 0f, 0f));
								}
								if (RobberB != null)
								{
									RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(2f, 0f, 0f));
								}
								if (RobberC != null)
								{
									RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(3f, 0f, 0f));
								}
								Script.Wait(300);
								GTA.UI.Screen.FadeIn(300);
								VaultClosed = true;
							}
						}
					}
					if (EnteranceUsed == 8)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, SideDoorEnterance, "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, SideDoorEnterance, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, SideDoorEnterance, "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								GTA.UI.Screen.FadeOut(300);
								Script.Wait(300);
								BypassMetalDetector = false;
								IsInterior = true;
								Script.Wait(1);
								Game.Player.Character.Position = SideDoorExit;
								if (RobberA != null)
								{
									RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(1f, 0f, 0f));
								}
								if (RobberB != null)
								{
									RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(2f, 0f, 0f));
								}
								if (RobberC != null)
								{
									RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(3f, 0f, 0f));
								}
								Script.Wait(300);
								GTA.UI.Screen.FadeIn(300);
								VaultClosed = true;
							}
						}
					}
					if (!VaultClosed)
					{
						if (ExitUsed == 2 || ExitUsed == 3)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2484.6f, -257.4f, -40f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(2484.6f, -257.4f, -40f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2484.6f, -257.4f, -40f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Casino (4)");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									Script.Wait(1);
									IsInterior = false;
									Game.Player.Character.Position = new Vector3(953.7f, 3.9f, 110f);
									if (RobberA != null && RobberA.IsAlive)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									}
									if (RobberB != null && RobberB.IsAlive)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2f, 0f));
									}
									if (RobberC != null && RobberC.IsAlive)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f));
									}
									try
									{
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
										VehicleChoosen = SaveCar.VehicleName;
										SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
										if (SecondaryGetawayVeh.AttachedBlip != null)
										{
											SecondaryGetawayVeh.AttachedBlip.Delete();
										}
										SecondaryGetawayVeh.AddBlip();
										SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
										SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
										SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.Repair();
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.IsEngineRunning = true;
										SecondaryGetawayVeh.IsDriveable = true;
										SecondaryGetawayVeh.IsInvincible = true;
										SecondaryGetawayVeh.IsPositionFrozen = true;
										HeadingtoyourAppartment = true;
									}
									catch
									{
										Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
									}
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2487.9f, -292.7f, -40f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(2487.9f, -292.7f, -40f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2487.9f, -292.7f, -40f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Casino (5)");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									Script.Wait(1);
									IsInterior = false;
									Game.Player.Character.Position = new Vector3(959.5f, 32.2f, 119f);
									try
									{
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars//DiamondHeist_Air.ini");
										VehicleChoosen = SaveCar.VehicleName;
										SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
										if (SecondaryGetawayVeh.AttachedBlip != null)
										{
											SecondaryGetawayVeh.AttachedBlip.Delete();
										}
										SecondaryGetawayVeh.AddBlip();
										SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
										SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
										SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.Repair();
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.IsEngineRunning = true;
										SecondaryGetawayVeh.IsDriveable = true;
										SecondaryGetawayVeh.IsInvincible = true;
										SecondaryGetawayVeh.IsPositionFrozen = true;
										HeadingtoyourAppartment = true;
									}
									catch
									{
										Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
									}
									VaultClosed = true;
								}
							}
						}
						if (ExitUsed == 6)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2541.024f, -212.23f, -59.5f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(2541.024f, -212.23f, -59.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2541.024f, -212.23f, -59.5f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Casino (1)");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									Script.Wait(1);
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									IsInterior = false;
									Script.Wait(1);
									Game.Player.Character.Position = new Vector3(993.13f, 76.7f, 79.5f);
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(1f, 0f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(2f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(3f, 0f, 0f));
									}
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
								}
							}
						}
						if (ExitUsed == 7)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, SideDoorExit, "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, SideDoorExit, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, SideDoorExit, "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Casino Via Side Door");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									GTA.UI.Screen.FadeOut(300);
									Script.Wait(300);
									IsInterior = false;
									Script.Wait(1);
									Game.Player.Character.Position = SideDoorEnterance;
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(1f, 0f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(2f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(3f, 0f, 0f));
									}
									Script.Wait(300);
									GTA.UI.Screen.FadeIn(300);
								}
							}
						}
					}
					if (VaultClosed && !PlayingCutscene)
					{
						Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2505.117f, -238.5348f, -69.94485f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_wall_damage"), 0, 0, 0);
						if (prop != null)
						{
							prop.IsVisible = false;
						}
						if (EnteranceUsed != 5 && EnteranceUsed != 1)
						{
							prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
							if (prop != null)
							{
								prop.Heading = 90f;
							}
						}
					}
					HeistTime += 0.35f;
					if (VaultClosed && CasinoVaultDoor != null)
					{
						if (PlayingCutscene)
						{
							CasinoVaultDoor.Opacity = 0;
						}
						if (!PlayingCutscene)
						{
							CasinoVaultDoor.Rotation = new Vector3(0f, 0f, 90f);
						}
					}
					if (!hackedDataPad)
					{
						if (EnteranceUsed != 1)
						{
							if (EnteranceUsed != 5)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2465.5f, -276.3f, -72f), "<", 100f) && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2465.5f, -276.3f, -72f), "<", 20f) && !NotifyHack)
								{
									UnlockedVaultEnterance = true;
									NotifyHack = true;
									Notification.Show("~y~ Lester ~w~ : you need to Hack the Vault Enterance Door to get access to the tunnel, leading to the vault!");
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2465.5f, -276.3f, -72f), "<", 100f))
								{
									World.DrawMarker(MarkerType.Cylinder, new Vector3(2465.5f, -276.3f, -72f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2465.5f, -276.3f, -72f), "<", 2f))
									{
										if (!ActiveHack)
										{
											DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Hack the door");
										}
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											if (!ActiveHack)
											{
												Notification.Show("~y~ You can cancel The hack at any time by pressing E");
												HackingSuccess = false;
												if (Game.Player.Character.CurrentVehicle != null)
												{
													Game.Player.Character.CurrentVehicle.IsPositionFrozen = true;
												}
												colOneY = 0.4f;
												colTwoY = 0.4f;
												colThreeY = 0.4f;
												colFourY = 0.4f;
												colFiveY = 0.74f;
												colSixY = 0.74f;
												colSevenY = 0.74f;
												colEightY = 0.74f;
												collumn1 = false;
												collumn2 = false;
												collumn3 = false;
												collumn4 = false;
												collumn5 = false;
												collumn6 = false;
												collumn7 = false;
												collumn8 = false;
												skip1 = false;
												skip2 = false;
												skip3 = false;
												loadingin = true;
												up = false;
												up1 = false;
												up2 = false;
												up3 = false;
												up4 = true;
												up5 = true;
												up6 = true;
												up7 = true;
												currentbar = 0;
												active = !active;
												indexx = 20;
												ActiveHack = true;
											}
											else if (ActiveHack)
											{
												if (Game.Player.Character.CurrentVehicle != null)
												{
													Game.Player.Character.CurrentVehicle.IsPositionFrozen = false;
												}
												HackingSuccess = false;
												Notification.Show("~y~ Lester ~w~ : The Hack has Reset");
												colOneY = 0.4f;
												colTwoY = 0.4f;
												colThreeY = 0.4f;
												colFourY = 0.4f;
												colFiveY = 0.74f;
												colSixY = 0.74f;
												colSevenY = 0.74f;
												colEightY = 0.74f;
												collumn1 = false;
												collumn2 = false;
												collumn3 = false;
												collumn4 = false;
												collumn5 = false;
												collumn6 = false;
												collumn7 = false;
												collumn8 = false;
												skip1 = false;
												skip2 = false;
												skip3 = false;
												loadingin = true;
												up = false;
												up1 = false;
												up2 = false;
												up3 = false;
												up4 = true;
												up5 = true;
												up6 = true;
												up7 = true;
												currentbar = 0;
												active = !active;
												indexx = 20;
												ActiveHack = false;
											}
										}
									}
								}
							}
							else if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2465.5f, -276.3f, -72f), "<", 100f))
							{
								HackingSuccess = true;
							}
						}
						else if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2465.5f, -276.3f, -72f), "<", 100f))
						{
							HackingSuccess = true;
						}
					}
					if (hackedDataPad && NotifyHack)
					{
						UnlockedVaultEnterance = true;
						NotifyHack = false;
						Notification.Show("~y~ Lester ~w~ :  the Vault Enterance Door has been Opened!");
					}
					if (MetalDetectorTimer == 0)
					{
						if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && Game.Player.Character.Weapons.Current.Hash != (WeaponHash)Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_BRIEFCASE"))
						{
							if (!ShutdownMetalDetector3)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2540.4f, -265.3f, -58f), "<", 2f))
								{
									MetalDetectorTimer = 1;
									Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
									Audio.PlaySoundFrontend("Beep_Green", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
									Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2541.8f, -265.1f, -58f), "<", 2f))
								{
									MetalDetectorTimer = 1;
									Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
									Audio.PlaySoundFrontend("Beep_Green", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
									Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
								}
							}
							if (!ShutdownMetalDetector1 && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2545.6f, -277.4f, -58f), "<", 2f))
							{
								MetalDetectorTimer = 1;
								Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
								Audio.PlaySoundFrontend("Beep_Green", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
								Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
							}
							if (!ShutdownMetalDetector2)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.6f, -269.9f, -58f), "<", 2f))
								{
									MetalDetectorTimer = 1;
									Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
									Audio.PlaySoundFrontend("Beep_Green", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
									Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.6f, -268.2f, -58f), "<", 2f))
								{
									MetalDetectorTimer = 1;
									Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
									Audio.PlaySoundFrontend("Beep_Green", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
									Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
								}
							}
						}
						if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && Game.Player.Character.Weapons.Current.Hash != (WeaponHash)Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_BRIEFCASE"))
						{
							if (!ShutdownMetalDetector3)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2540.4f, -265.3f, -58f), "<", 2f))
								{
									MetalDetectorTimer = 1;
									Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
									Audio.PlaySoundFrontend("Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
									Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2541.8f, -265.1f, -58f), "<", 2f))
								{
									MetalDetectorTimer = 1;
									Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
									Audio.PlaySoundFrontend("Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
									Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
								}
							}
							if (!ShutdownMetalDetector1 && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2545.6f, -277.4f, -58f), "<", 2f))
							{
								MetalDetectorTimer = 1;
								Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
								Audio.PlaySoundFrontend("Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
								Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
							}
							if (!ShutdownMetalDetector2)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.6f, -269.9f, -58f), "<", 2f))
								{
									MetalDetectorTimer = 1;
									Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
									Audio.PlaySoundFrontend("Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
									Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.6f, -268.2f, -58f), "<", 2f))
								{
									MetalDetectorTimer = 1;
									Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
									Audio.PlaySoundFrontend("Beep_Red", "DLC_HEIST_HACKING_SNAKE_SOUNDS");
									Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
								}
							}
						}
					}
					if (MetalDetectorTimer == 1 && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2540.4f, -265.3f, -58f), ">", 2f) && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2541.8f, -265.1f, -58f), ">", 2f) && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2545.6f, -277.4f, -58f), ">", 2f) && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.6f, -269.9f, -58f), ">", 2f) && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.6f, -268.2f, -58f), ">", 2f))
					{
						MetalDetectorTimer = 0;
					}
					if (ExtraValuable != null && ExtraVaultOpen && CurrentType < 4)
					{
						if (PlayingAnim)
						{
							ManageRobberyAnims.CheckCrate(Game.Player.Character, ExtraValuable, ExtraValuable.Position, ExtraValuable.Rotation, (ManageRobberyAnims.LootType)ExtraValuableType);
							if (!PlayingCashGrabIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1065353216)
							{
								if (MoneyProp != null)
								{
									CashCount++;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
								}
								if (CashCount == 40f && MoneyProp != null)
								{
									MoneyProp.IsVisible = false;
								}
								if (CashCount == 50f)
								{
									Model model = new Model("ch_prop_20dollar_pile_01a");
									if (CurrentType == 1)
									{
										new Model("ch_prop_20dollar_pile_01a").Request(250);
									}
									if (CurrentType == 2)
									{
										new Model("ch_prop_gold_bar_01a").Request(250);
									}
									if (CurrentType == 3)
									{
										new Model("ch_prop_vault_dimaondbox_01a").Request(250);
									}
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									CashCount = 0f;
								}
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
							{
								if (MoneyProp != null)
								{
									MoneyProp.Delete();
								}
								if (bagProp != null)
								{
									bagProp.Delete();
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
								PlayingAnim = false;
								Game.Player.Character.Task.ClearAll();
								RobTimer = 0;
							}
							RobTimer++;
						}
						if (!RobbedExtraValuable && ExtraValuable != null)
						{
							Vector3 offsetPosition2 = ExtraValuable.Loot.GetOffsetPosition(new Vector3(0f, 1f, -0.3f));
							if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition2, "<", 100f))
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition2, "<", 2f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Steal Valuables");
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition2, "<", 2f) && Game.IsControlJustPressed(GTA.Control.Context))
								{
									RobbedExtraValuable = true;
									PlayingAnim = true;
									ManageRobberyAnims.RobCrate(Game.Player.Character, ExtraValuable.Loot, ExtraValuable.Position, ExtraValuable.Rotation, (ManageRobberyAnims.LootType)ExtraValuableType, -1);
									ManageRobberyAnims.RobCrate(Game.Player.Character, ExtraValuable.Loot, ExtraValuable.Position, ExtraValuable.Rotation, (ManageRobberyAnims.LootType)ExtraValuableType, -1);
								}
								if (EnableRobberyMarkers)
								{
									World.DrawMarker(MarkerType.Cylinder, offsetPosition2, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
								}
							}
						}
					}
					if (ExtraValuable2 != null && ExtraValuable2.Loot != null && ExtraVaultOpen2)
					{
						if (PlayingAnim && CurrentType < 4)
						{
							ManageRobberyAnims.CheckCrate(Game.Player.Character, ExtraValuable2, ExtraValuable2.Position, ExtraValuable2.Rotation, (ManageRobberyAnims.LootType)ExtraValuableType2);
							if (!PlayingCashGrabIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1065353216)
							{
								if (MoneyProp != null)
								{
									CashCount++;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
								}
								if (CashCount == 40f && MoneyProp != null)
								{
									MoneyProp.IsVisible = false;
								}
								if (CashCount == 50f)
								{
									Model model2 = new Model("ch_prop_20dollar_pile_01a");
									if (CurrentType == 1)
									{
										model2 = new Model("ch_prop_20dollar_pile_01a");
										model2.Request(250);
									}
									if (CurrentType == 2)
									{
										model2 = new Model("ch_prop_gold_bar_01a");
										model2.Request(250);
									}
									if (CurrentType == 3)
									{
										model2 = new Model("ch_prop_vault_dimaondbox_01a");
										model2.Request(250);
									}
									model2.MarkAsNoLongerNeeded();
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									CashCount = 0f;
								}
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
							{
								if (MoneyProp != null)
								{
									MoneyProp.Delete();
								}
								if (bagProp != null)
								{
									bagProp.Delete();
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
								PlayingAnim = false;
								Game.Player.Character.Task.ClearAll();
								RobTimer = 0;
							}
							RobTimer++;
						}
						if (!RobbedExtraValuable2 && ExtraValuable2 != null)
						{
							Vector3 offsetPosition3 = ExtraValuable2.Loot.GetOffsetPosition(new Vector3(0f, 1f, -0.3f));
							if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition3, "<", 100f))
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition3, "<", 2f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Steal Valuables");
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition3, "<", 2f) && Game.IsControlJustPressed(GTA.Control.Context))
								{
									RobbedExtraValuable2 = true;
									PlayingAnim = true;
									ManageRobberyAnims.RobCrate(Game.Player.Character, ExtraValuable2.Loot, ExtraValuable2.Position, ExtraValuable2.Rotation, (ManageRobberyAnims.LootType)ExtraValuableType2, -1);
									ManageRobberyAnims.RobCrate(Game.Player.Character, ExtraValuable2.Loot, ExtraValuable2.Position, ExtraValuable2.Rotation, (ManageRobberyAnims.LootType)ExtraValuableType2, -1);
								}
								if (EnableRobberyMarkers)
								{
									World.DrawMarker(MarkerType.Cylinder, offsetPosition3, Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
								}
							}
						}
					}
					if (ExtraVaultOpen)
					{
						ManageDoor(new Vector3(2522.89f, -285.46f, -58.47315f), "ch_prop_ch_vault_d_door_01a", false);
					}
					if (!ExtraVaultOpen)
					{
						ManageDoor(new Vector3(2522.89f, -285.46f, -58.47315f), "ch_prop_ch_vault_d_door_01a", true);
					}
					if (!ExtraVaultOpen2)
					{
						if (!HackDone)
						{
							if (ExtraValuable != null && ExtraValuable.Loot != null)
							{
								ExtraValuable.Loot.Delete();
							}
							Random random4 = new Random();
							int num9 = random4.Next(0, 300);
							Vector3 position3 = new Vector3(2493.473f, -246.8996f, -55.123f);
							Vector3 vector = new Vector3(0f, 0f, 180f);
							if (num9 < 33)
							{
								Prop prop2 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01a"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop2.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop2);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop2);
								ExtraValuableType2 = 1;
							}
							if (num9 >= 33 && num9 < 67)
							{
								Prop prop3 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01a"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop3.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop3);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop3);
								ExtraValuableType2 = 2;
							}
							if (num9 >= 67 && num9 < 100)
							{
								Prop prop4 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01a"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop4.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop4);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop4);
								ExtraValuableType2 = 3;
							}
							if (num9 >= 100 && num9 < 133)
							{
								Prop prop5 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01b"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop5.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop5);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop5);
								ExtraValuableType2 = 1;
							}
							if (num9 >= 133 && num9 < 167)
							{
								Prop prop6 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01b"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop6.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop6);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop6);
								ExtraValuableType2 = 2;
							}
							if (num9 >= 167 && num9 < 200)
							{
								Prop prop7 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01b"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop7.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop7);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop7);
								ExtraValuableType2 = 3;
							}
							if (num9 >= 200 && num9 < 233)
							{
								Prop prop8 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01c"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop8.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop8);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop8);
								ExtraValuableType2 = 1;
							}
							if (num9 >= 233 && num9 < 267)
							{
								Prop prop9 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01c"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop9.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop9);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop9);
								ExtraValuableType2 = 2;
							}
							if (num9 >= 267 && num9 < 300)
							{
								Prop prop10 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01c"), position3, new Vector3(0f, 0f, vector.Z - 180f), true, false);
								prop10.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop10);
								ExtraValuable2 = ManageRobberyAnims.ReturnLootable(prop10);
								ExtraValuableType2 = 3;
							}
							ExtraVaultOpen2 = true;
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.4f, -280.4f, -59f), "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Hack the Secondary Vault");
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.4f, -280.4f, -59f), "<", 2f) && Game.IsControlJustPressed(GTA.Control.Context))
						{
							if (!ActiveHack)
							{
								Notification.Show("~y~ You can cancel The hack at any time by pressing E");
								HackingSuccess = false;
								if (Game.Player.Character.CurrentVehicle != null)
								{
									Game.Player.Character.CurrentVehicle.IsPositionFrozen = true;
								}
								Notification.Show("~y~ Lester ~w~ : Select ~b~ My Computer ~w~ > ~b~ External Device ~w~ > ~y~ Hackconnect.exe ~w~ to Start the Hack");
								Loadehacker(true);
								ActiveHack = true;
							}
							else if (ActiveHack)
							{
								if (Game.Player.Character.CurrentVehicle != null)
								{
									Game.Player.Character.CurrentVehicle.IsPositionFrozen = false;
								}
								HackingSuccess = false;
								Notification.Show("~y~ Lester ~w~ : The Hack has Reset");
								UnloadLoadehacker();
								ActiveHack = false;
							}
						}
					}
					if (!ExtraVaultOpen)
					{
						if (HackDone)
						{
							if (ExtraValuable != null && ExtraValuable.Loot != null)
							{
								ExtraValuable.Loot.Delete();
							}
							Random random5 = new Random();
							int num10 = random5.Next(0, 300);
							Vector3 position4 = new Vector3(2520.8f, -287.8f, -59.7f);
							Vector3 vector2 = new Vector3(0f, 0f, 180f);
							if (num10 < 33)
							{
								Prop prop11 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01a"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop11.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop11);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop11);
								ExtraValuableType = 1;
							}
							if (num10 >= 33 && num10 < 67)
							{
								Prop prop12 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01a"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop12.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop12);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop12);
								ExtraValuableType = 2;
							}
							if (num10 >= 67 && num10 < 100)
							{
								Prop prop13 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01a"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop13.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop13);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop13);
								ExtraValuableType = 3;
							}
							if (num10 >= 100 && num10 < 133)
							{
								Prop prop14 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01b"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop14.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop14);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop14);
								ExtraValuableType = 1;
							}
							if (num10 >= 133 && num10 < 167)
							{
								Prop prop15 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01b"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop15.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop15);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop15);
								ExtraValuableType = 2;
							}
							if (num10 >= 167 && num10 < 200)
							{
								Prop prop16 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01b"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop16.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop16);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop16);
								ExtraValuableType = 3;
							}
							if (num10 >= 200 && num10 < 233)
							{
								Prop prop17 = GlobalHelper.CreateProp(RequestModel("ch_prop_ch_cash_trolly_01c"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop17.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop17);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop17);
								ExtraValuableType = 1;
							}
							if (num10 >= 233 && num10 < 267)
							{
								Prop prop18 = GlobalHelper.CreateProp(RequestModel("ch_prop_gold_trolly_01c"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop18.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop18);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop18);
								ExtraValuableType = 2;
							}
							if (num10 >= 267 && num10 < 300)
							{
								Prop prop19 = GlobalHelper.CreateProp(RequestModel("ch_prop_diamond_trolly_01c"), position4, new Vector3(0f, 0f, vector2.Z - 180f), true, false);
								prop19.IsPositionFrozen = true;
								ManageRobberyAnims.AddPropAsLoot(prop19);
								ExtraValuable = ManageRobberyAnims.ReturnLootable(prop19);
								ExtraValuableType = 3;
							}
							ExtraVaultOpen = true;
							Notification.Show("~y~ Lester ~w~ : The Secondary Vault has been Opened!");
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.4f, -280.4f, -59f), "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Hack the Secondary Vault");
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.4f, -280.4f, -59f), "<", 2f) && Game.IsControlJustPressed(GTA.Control.Context))
						{
							if (!ActiveHack)
							{
								Notification.Show("~y~ You can cancel The hack at any time by pressing E");
								HackingSuccess = false;
								if (Game.Player.Character.CurrentVehicle != null)
								{
									Game.Player.Character.CurrentVehicle.IsPositionFrozen = true;
								}
								Notification.Show("~y~ Lester ~w~ : Select ~b~ My Computer ~w~ > ~b~ External Device ~w~ > ~y~ Hackconnect.exe ~w~ to Start the Hack");
								Loadehacker(true);
								ActiveHack = true;
							}
							else if (ActiveHack)
							{
								if (Game.Player.Character.CurrentVehicle != null)
								{
									Game.Player.Character.CurrentVehicle.IsPositionFrozen = false;
								}
								HackingSuccess = false;
								Notification.Show("~y~ Lester ~w~ : The Hack has Reset");
								UnloadLoadehacker();
								ActiveHack = false;
							}
						}
					}
					if (GetawayCar != null && GetDistance_VDIST2(Game.Player.Character.Position, GetawayCar.Position, "<", 100f) && !FreezeGetawayCar)
					{
						FreezeGetawayCar = true;
						GetawayCar.IsPositionFrozen = false;
						GetawayCar.IsInvincible = false;
					}
					if (SecondaryGetawayVeh != null && GetDistance_VDIST2(Game.Player.Character.Position, SecondaryGetawayVeh.Position, "<", 100f) && !FreezeGetawayCar2)
					{
						FreezeGetawayCar2 = true;
						SecondaryGetawayVeh.IsPositionFrozen = false;
						SecondaryGetawayVeh.IsInvincible = false;
					}
					if (IsInterior && AlarmsGoingOff)
					{
						Game.Player.WantedLevel = 4;
					}
					if (EnteranceUsed == 1 && Game.Player.Character.Weapons.Current.Hash == WeaponHash.Unarmed)
					{
						Game.Player.Character.Weapons.Give((WeaponHash)Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_BRIEFCASE"), 999999, true, true);
					}
					if (EnteranceUsed == 5 && ExitUsed == 4 && !PlantedBZGas && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2491.9f, -252.6f, -72f), "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, new Vector3(2491.9f, -252.6f, -72f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2491.9f, -252.6f, -72f), "<", 2f))
						{
							DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Plant BZ Gas");
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								PlantedBZGas = true;
								foreach (Prop smokeEmitter2 in SmokeEmitters)
								{
									if (smokeEmitter2 != null)
									{
										smokeEmitter2.Delete();
									}
								}
								Game.Player.Character.Rotation = new Vector3(0f, 0f, -90f);
								Game.Player.Character.Weapons.Give(WeaponHash.BZGas, 20, true, true);
								Game.Player.Character.Weapons.Select(WeaponHash.BZGas);
								Game.Player.Character.Task.PlayAnimation("weapons@projectile@sticky_bomb", "plant_vertical", 34f, 34f, 2500, AnimationFlags.None, 0f);
								Prop prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2499.8f, -244.04f, -71f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2500.4f, -244.04f, -71f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2500.1f, -2400.04f, -71f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2516.3f, -227.5f, -71f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2516.5f, -226.5f, -71f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2528.1f, -243.2f, -70f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2528.1f, -242.9f, -70f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2528.03f, -243.4f, -70f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2498.3f, -238.9f, -70f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2498.3f, -238.9f, -70f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								prop20 = GlobalHelper.CreateProp(RequestModel("prop_flare_01b"), new Vector3(2498.3f, -238.9f, -70f), new Vector3(0f, 0f, 0f), false, false);
								prop20.IsPositionFrozen = true;
								SmokeEmitters.Add(prop20);
								foreach (Prop smokeEmitter3 in SmokeEmitters)
								{
									if (smokeEmitter3 != null)
									{
										smokeEmitter3.IsVisible = false;
										Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "core");
										Function.Call(Hash.USE_PARTICLE_FX_ASSET, "core");
										Function.Call(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "ent_amb_fbi_smoke_fogball", smokeEmitter3, 0.0, 0.0, -0.5, 0.0, 0.0, 0.0, 1.0, false, false, false);
									}
								}
							}
						}
					}
					if (IsInterior)
					{
						if (((EnteranceUsed == 5 && ExitUsed == 4) || EnteranceUsed == 1) && TriggerFullResponce && TriggerFullResponce)
						{
							if (VaultClosed)
							{
								if (!PlacedBomb)
								{
									float num11 = -77f;
									float num12 = -67f;
									float z = Game.Player.Character.Position.Z;
									if (z > num11 && z < num12)
									{
										if (!TriggerNotifyNear && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2504.3f, -238.7f, -71f), "<", 40f))
										{
											if (GetawayCar != null && GetawayCar.Model == VehicleHash.Stockade)
											{
												if (ExitUsed == 0)
												{
													try
													{
														SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
														VehicleChoosen = SaveCar.VehicleName;
														SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(914.8f, 52.6f, 80.2f), -32f);
														if (SecondaryGetawayVeh.AttachedBlip != null)
														{
															SecondaryGetawayVeh.AttachedBlip.Delete();
														}
														SecondaryGetawayVeh.AddBlip();
														SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
														SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
														SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
														SecondaryGetawayVeh.IsPositionFrozen = false;
														SecondaryGetawayVeh.Repair();
														SecondaryGetawayVeh.IsPositionFrozen = false;
														SecondaryGetawayVeh.IsEngineRunning = true;
														SecondaryGetawayVeh.IsDriveable = true;
														SecondaryGetawayVeh.IsInvincible = true;
														SecondaryGetawayVeh.IsPositionFrozen = true;
													}
													catch
													{
														Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
													}
												}
												if (ExitUsed == 1)
												{
													try
													{
														SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
														VehicleChoosen = SaveCar.VehicleName;
														SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(988.9f, -146.2f, 33.9f), -132f);
														if (SecondaryGetawayVeh.AttachedBlip != null)
														{
															SecondaryGetawayVeh.AttachedBlip.Delete();
														}
														SecondaryGetawayVeh.AddBlip();
														SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
														SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
														SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
														SecondaryGetawayVeh.IsPositionFrozen = false;
														SecondaryGetawayVeh.Repair();
														SecondaryGetawayVeh.IsPositionFrozen = false;
														SecondaryGetawayVeh.IsEngineRunning = true;
														SecondaryGetawayVeh.IsDriveable = true;
														SecondaryGetawayVeh.IsInvincible = true;
														SecondaryGetawayVeh.IsPositionFrozen = true;
													}
													catch
													{
														Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
													}
												}
												if (ExitUsed == 2 || ExitUsed == 3)
												{
													try
													{
														if (SecondaryGetawayVeh != null)
														{
															SecondaryGetawayVeh.Delete();
														}
														SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
														VehicleChoosen = SaveCar.VehicleName;
														SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
														if (SecondaryGetawayVeh.AttachedBlip != null)
														{
															SecondaryGetawayVeh.AttachedBlip.Delete();
														}
														SecondaryGetawayVeh.AddBlip();
														SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
														SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
														SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
														SecondaryGetawayVeh.IsPositionFrozen = false;
														SecondaryGetawayVeh.Repair();
														SecondaryGetawayVeh.IsPositionFrozen = false;
														SecondaryGetawayVeh.IsEngineRunning = true;
														SecondaryGetawayVeh.IsDriveable = true;
														SecondaryGetawayVeh.IsInvincible = true;
														SecondaryGetawayVeh.IsPositionFrozen = true;
													}
													catch
													{
														Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
													}
												}
											}
											Prop prop21 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
											if (prop21 != null)
											{
												prop21.IsCollisionEnabled = true;
												prop21.IsVisible = true;
											}
											Notification.Show("~y~ Lester ~w~ : You need to take out any guards, near the vault, otherwise they will alert everyone in the building!");
											Notification.Show("~y~ Lester ~w~ : Press E to use ~b~C4~w~, Q to use ~g~Drill~w~, G to use ~o~Laser Drill~w~, when next to Vault");
											Notification.Show("~y~ Lester ~w~ : Also a Contact has Delivered your Getaway Car to your choosen Exit");
											TriggerNotifyNear = true;
										}
										if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2504.289f, -237.8028f, -72f), "<", 100f))
										{
											World.DrawMarker(MarkerType.Cylinder, new Vector3(2504.289f, -237.8028f, -72f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
										}
										if (IsPlanting && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "weapons@projectile@sticky_bomb", "plant_vertical", 1) && Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, Game.Player.Character, "weapons@projectile@sticky_bomb", "plant_vertical") > 0.85f && BombSewer == null)
										{
											SpawnPropBombSewer("ch_prop_ch_ld_bomb_01a", Game.Player.Character.Bones[Bone.SkelLeftHand].Position, new Vector3(0f, -90f, 0f));
										}
										if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2504.289f, -237.8028f, -72f), "<", 2f))
										{
											if (!DrillComplete && !PlacedBomb)
											{
												DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ OR ~INPUT_COVER~ OR ~INPUT_THROW_GRENADE~ for Vault Breaching Equipment ");
											}
											if (Game.IsControlJustPressed(GTA.Control.Context))
											{
												DrillComplete = false;
												DestoryDrill();
												IsPlanting = true;
												Game.Player.Character.Rotation = new Vector3(0f, 0f, -90f);
												Game.Player.Character.Position = new Vector3(2504.3f, -238.7f, -72f);
												Game.Player.Character.Weapons.Give(WeaponHash.StickyBomb, 20, true, true);
												Game.Player.Character.Weapons.Select(WeaponHash.StickyBomb);
												if (BombSewer != null)
												{
													BombSewer.Delete();
												}
												Game.Player.Character.Task.PlayAnimation("weapons@projectile@sticky_bomb", "plant_vertical", 34f, 34f, 2500, AnimationFlags.None, 0f);
												Script.Wait(2000);
												Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
												SpawnPropBombVault("ch_prop_ch_explosive_01a", new Vector3(2505f, -239.6f, -71f), new Vector3(14f, -90f, 0f));
												SpawnPropBombVault("ch_prop_ch_explosive_01a", new Vector3(2505f, -239.7f, -69.5f), new Vector3(76f, -90f, 0f));
												SpawnPropBombVault("ch_prop_ch_explosive_01a", new Vector3(2505.119f, -237.1551f, -69.3f), new Vector3(217f, -90f, 0f));
												SpawnPropBombVault("ch_prop_ch_explosive_01a", new Vector3(2505.1f, -237.4f, -71.3f), new Vector3(135f, -90f, 0f));
												PlacedBomb = true;
												IsPlanting = false;
											}
											if (Game.IsControlJustPressed(GTA.Control.Cover))
											{
												if (!DrillComplete)
												{
													DrillComplete = true;
													Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
													Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
													StartCasinoDrill();
													Notification.Show("You can Cancel the Drilling by pressing Q");
												}
												else if (DrillComplete)
												{
													DrillComplete = false;
													Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
													Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
													DestoryDrill();
												}
											}
											if (Game.IsControlJustPressed(GTA.Control.ThrowGrenade))
											{
												if (!DrillComplete)
												{
													DrillComplete = true;
													Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
													Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
													StartCasinoLaser();
													Notification.Show("You can Cancel the Drilling by pressing G");
												}
												else if (DrillComplete)
												{
													DrillComplete = false;
													Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
													Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
													DestoryDrill();
												}
											}
										}
									}
									if (!DrillingComplete)
									{
										Prop prop22 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
										if (prop22 != null)
										{
											CasinoVaultDoor = prop22;
											prop22.IsCollisionEnabled = true;
											prop22.IsVisible = true;
										}
									}
									if (DrillingComplete)
									{
										Function.Call(Hash.REMOVE_CUTSCENE);
										DrillComplete = true;
										PlayingCutscene = false;
										VaultOpenedByDrill = false;
									}
									if (!VaultOpenedByDrill)
									{
										if (PlayingCutscene && CasinoVaultDoor != null)
										{
											CasinoVaultDoor.IsCollisionEnabled = false;
											CasinoVaultDoor.IsVisible = false;
										}
										if (!PlayingCutscene && CasinoVaultDoor != null)
										{
											CasinoVaultDoor.IsCollisionEnabled = true;
											CasinoVaultDoor.IsVisible = true;
										}
										if (DrillingComplete && !PlayingCutscene)
										{
											DestoryDrill();
											DrillingComplete = false;
											int num13 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
											Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_Vault_Door");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "set_vault_dressing");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_vault_cash_01");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_vault_cash_02");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_vault_gold_01");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_vault_gold_02");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_vault_art_01");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_vault_art_02");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_vault_diamonds_01");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num13, "Set_vault_diamonds_02");
											Function.Call(Hash.REFRESH_INTERIOR, num13);
											int num14 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs3f_mul_vlt");
											if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene)
											{
												if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
												{
													Prop prop23 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
													if (prop23 != null)
													{
														CasinoVaultDoor = prop23;
														CasinoVaultDoor.IsVisible = false;
													}
													Ped[] nearbyPeds2 = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
													if (nearbyPeds2.Length != 0)
													{
														Ped[] array2 = nearbyPeds2;
														foreach (Ped ped2 in array2)
														{
															if (ped2 != null)
															{
																ped2.IsVisible = false;
															}
														}
													}
													SetEntityinCustscenes();
													Function.Call(Hash.START_CUTSCENE, num14);
													Script.Wait(50);
													GetPedOutfitCutscene("MP_1", Game.Player.Character);
													if (RobberA != null)
													{
														GetPedOutfitCutscene("MP_2", RobberA);
													}
													if (RobberB != null)
													{
														GetPedOutfitCutscene("MP_3", RobberB);
													}
													if (RobberC != null)
													{
														GetPedOutfitCutscene("MP_4", RobberC);
													}
												}
												if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
												{
													PlayingCutscene = true;
												}
											}
										}
										if (PlayingCutscene)
										{
											if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
											{
												Function.Call(Hash.STOP_CUTSCENE, true);
											}
											if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
											{
												Function.Call(Hash.REMOVE_CUTSCENE);
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
												if (RobberA != null)
												{
													RobberA.IsVisible = true;
												}
												if (RobberB != null)
												{
													RobberB.IsVisible = true;
												}
												if (RobberC != null)
												{
													RobberC.IsVisible = true;
												}
												if (PlayingCutscene)
												{
													foreach (ManageRobberyAnims.Lootable crate_Valuable in ManageRobberyAnims.Crate_Valuables)
													{
														if (crate_Valuable != null && crate_Valuable.LootBlip != null)
														{
															crate_Valuable.LootBlip.Alpha = 255;
														}
													}
													Notification.Show("~y~ Lester ~w~ : Get in and grab as much as you can carry!");
													GetTimeInVault();
													Notification.Show("~y~ Lester ~w~ : Get out of the Vault before that time Expires and the Alarms will not go off!");
													Game.Player.Character.Position = new Vector3(2507.7f, -238.3f, -70.7f);
													DrillingComplete = false;
													VaultClosed = false;
													VaultOpenedByDrill = true;
													Ped[] nearbyPeds3 = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
													if (nearbyPeds3.Length != 0)
													{
														Ped[] array3 = nearbyPeds3;
														foreach (Ped ped3 in array3)
														{
															if (ped3 != null)
															{
																ped3.IsVisible = true;
															}
														}
													}
													Script.Wait(500);
													if (CasinoVaultDoor != null)
													{
														CasinoVaultDoor.IsCollisionEnabled = true;
														CasinoVaultDoor.IsVisible = true;
													}
													PlacedBomb = false;
													VaultClosed = false;
													VaultOpenedByDrill = true;
													PlayingCutscene = false;
													Function.Call(Hash.REMOVE_CUTSCENE);
													int num15 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
													if (!PlacedBomb)
													{
														Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_Vault_Door");
													}
													Prop prop24 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
													if (prop24 != null)
													{
														prop24.IsCollisionEnabled = true;
														prop24.IsVisible = true;
													}
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "set_vault_dressing");
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_vault_cash_01");
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_vault_cash_02");
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_vault_gold_01");
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_vault_gold_02");
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_vault_art_01");
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_vault_art_02");
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_vault_diamonds_01");
													Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num15, "Set_vault_diamonds_02");
													Function.Call(Hash.REFRESH_INTERIOR, num15);
												}
											}
										}
									}
								}
								if (!DrillComplete && PlacedBomb && VaultClosed && BombVault_A != null && BombVault_B != null && BombVault_C != null && BombVault_D != null && GetDistance_VDIST2(Game.Player.Character.Position, BombVault_A.Position, "<", 40f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_THROW_GRENADE~ to Detonate Bombs");
									if (Game.IsControlJustPressed(GTA.Control.ThrowGrenade))
									{
										Prop prop25 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
										if (prop25 != null)
										{
											prop25.IsCollisionEnabled = true;
											prop25.IsVisible = true;
										}
										World.AddExplosion(BombVault_C.Position, ExplosionType.Grenade, 10f, 1f);
										World.AddExplosion(BombVault_B.Position, ExplosionType.Grenade, 10f, 1f);
										World.AddExplosion(BombVault_A.Position, ExplosionType.Grenade, 10f, 1f);
										World.AddExplosion(BombVault_D.Position, ExplosionType.Grenade, 10f, 1f);
										World.AddExplosion(BombVault_C.Position, ExplosionType.PetrolPump, 10f, 1f);
										World.AddExplosion(BombVault_B.Position, ExplosionType.PetrolPump, 10f, 1f);
										World.AddExplosion(BombVault_A.Position, ExplosionType.PetrolPump, 10f, 1f);
										World.AddExplosion(BombVault_D.Position, ExplosionType.PetrolPump, 10f, 1f);
										Script.Wait(1000);
										BombVault_A.Delete();
										BombVault_B.Delete();
										BombVault_C.Delete();
										BombVault_D.Delete();
										SpawnProp("ch_prop_ch_vaultdoor01x", new Vector3(2512.9f, -240.6f, -73.6f), new Vector3(90f, 135f, 0f));
										Props[Props.Count - 1].IsPositionFrozen = true;
										int num16 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_Vault_Door");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "set_vault_dressing");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_vault_cash_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_vault_cash_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_vault_gold_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_vault_gold_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_vault_art_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_vault_art_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_vault_diamonds_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num16, "Set_vault_diamonds_02");
										VaultClosed = false;
										foreach (ManageRobberyAnims.Lootable crate_Valuable2 in ManageRobberyAnims.Crate_Valuables)
										{
											if (crate_Valuable2 != null && crate_Valuable2.LootBlip != null)
											{
												crate_Valuable2.LootBlip.Alpha = 255;
											}
										}
										Notification.Show("~y~ Lester ~w~ : Get in and grab as much as you can carry!");
										GetTimeInVault();
										DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok you have got " + Waittime + " seconds, Before the Alarms Go off!");
										Notification.Show("~y~ Lester ~w~ : Get out of the Vault before that time Expires and the Alarms will not go off!");
									}
								}
							}
							if (VaultClosed)
							{
								Prop prop26 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_wall_damage"), 0, 0, 0);
								if (prop26 != null)
								{
									prop26.IsCollisionEnabled = false;
									prop26.IsVisible = false;
								}
								prop26 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
								if (prop26 != null)
								{
									prop26.IsCollisionEnabled = true;
									prop26.IsVisible = true;
								}
							}
							if (!VaultClosed)
							{
								if (PlacedBomb)
								{
									Prop prop27 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_wall_damage"), 0, 0, 0);
									if (prop27 != null)
									{
										prop27.IsCollisionEnabled = true;
										prop27.IsVisible = true;
									}
								}
								if (!PlacedBomb)
								{
									Prop prop28 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_wall_damage"), 0, 0, 0);
									if (prop28 != null)
									{
										prop28.IsCollisionEnabled = false;
										prop28.IsVisible = false;
									}
								}
							}
						}
						if (EnteranceUsed != 5 && EnteranceUsed != 1)
						{
							if (VaultClosed)
							{
								if (!PlacedBomb)
								{
									float num17 = -77f;
									float num18 = -67f;
									float z2 = Game.Player.Character.Position.Z;
									if (!TriggerNotifyNear && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2504.3f, -238.7f, -71f), "<", 40f))
									{
										if (GetawayCar != null && GetawayCar.Model == VehicleHash.Stockade)
										{
											if (ExitUsed == 0)
											{
												try
												{
													SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
													VehicleChoosen = SaveCar.VehicleName;
													SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(914.8f, 52.6f, 80.2f), -32f);
													if (SecondaryGetawayVeh.AttachedBlip != null)
													{
														SecondaryGetawayVeh.AttachedBlip.Delete();
													}
													SecondaryGetawayVeh.AddBlip();
													SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
													SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
													SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
													SecondaryGetawayVeh.IsPositionFrozen = false;
													SecondaryGetawayVeh.Repair();
													SecondaryGetawayVeh.IsPositionFrozen = false;
													SecondaryGetawayVeh.IsEngineRunning = true;
													SecondaryGetawayVeh.IsDriveable = true;
													SecondaryGetawayVeh.IsInvincible = true;
													SecondaryGetawayVeh.IsPositionFrozen = true;
												}
												catch
												{
													Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
												}
											}
											if (ExitUsed == 1)
											{
												try
												{
													SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
													VehicleChoosen = SaveCar.VehicleName;
													SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(988.9f, -146.2f, 33.9f), -132f);
													if (SecondaryGetawayVeh.AttachedBlip != null)
													{
														SecondaryGetawayVeh.AttachedBlip.Delete();
													}
													SecondaryGetawayVeh.AddBlip();
													SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
													SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
													SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
													SecondaryGetawayVeh.IsPositionFrozen = false;
													SecondaryGetawayVeh.Repair();
													SecondaryGetawayVeh.IsPositionFrozen = false;
													SecondaryGetawayVeh.IsEngineRunning = true;
													SecondaryGetawayVeh.IsDriveable = true;
													SecondaryGetawayVeh.IsInvincible = true;
													SecondaryGetawayVeh.IsPositionFrozen = true;
												}
												catch
												{
													Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
												}
											}
											if (ExitUsed == 2 || ExitUsed == 3)
											{
												try
												{
													if (SecondaryGetawayVeh != null)
													{
														SecondaryGetawayVeh.Delete();
													}
													SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
													VehicleChoosen = SaveCar.VehicleName;
													SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
													if (SecondaryGetawayVeh.AttachedBlip != null)
													{
														SecondaryGetawayVeh.AttachedBlip.Delete();
													}
													SecondaryGetawayVeh.AddBlip();
													SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
													SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
													SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
													SecondaryGetawayVeh.IsPositionFrozen = false;
													SecondaryGetawayVeh.Repair();
													SecondaryGetawayVeh.IsPositionFrozen = false;
													SecondaryGetawayVeh.IsEngineRunning = true;
													SecondaryGetawayVeh.IsDriveable = true;
													SecondaryGetawayVeh.IsInvincible = true;
													SecondaryGetawayVeh.IsPositionFrozen = true;
												}
												catch
												{
													Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
												}
											}
										}
										Prop prop29 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
										if (prop29 != null)
										{
											prop29.IsCollisionEnabled = true;
											prop29.IsVisible = true;
										}
										Notification.Show("~y~ Lester ~w~ : You need to take out any guards, near the vault, otherwise they will alert everyone in the building!");
										Notification.Show("~y~ Lester ~w~ : Press E to use ~b~C4~w~, Q to use ~g~Drill~w~, G to use ~o~Laser Drill~w~, when next to Vault");
										Notification.Show("~y~ Lester ~w~ : Also a Contact has Delivered your Getaway Car to your choosen Exit");
										TriggerNotifyNear = true;
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2504.289f, -237.8028f, -72f), "<", 100f))
									{
										World.DrawMarker(MarkerType.Cylinder, new Vector3(2504.289f, -237.8028f, -72f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
									}
									if (IsPlanting && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "weapons@projectile@sticky_bomb", "plant_vertical", 1) && Function.Call<float>(Hash.GET_ENTITY_ANIM_CURRENT_TIME, Game.Player.Character, "weapons@projectile@sticky_bomb", "plant_vertical") > 0.85f && BombSewer == null)
									{
										SpawnPropBombSewer("ch_prop_ch_ld_bomb_01a", Game.Player.Character.Bones[Bone.SkelLeftHand].Position, new Vector3(0f, -90f, 0f));
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2504.289f, -237.8028f, -72f), "<", 2f))
									{
										if (!DrillComplete && !PlacedBomb)
										{
											DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ Use C4\nPress ~INPUT_COVER~ use Drill\nPress ~INPUT_THROW_GRENADE~ use Thermal Drill", 1);
										}
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											DrillComplete = false;
											DestoryDrill();
											IsPlanting = true;
											Game.Player.Character.Rotation = new Vector3(0f, 0f, -90f);
											Game.Player.Character.Position = new Vector3(2504.3f, -238.7f, -72f);
											Game.Player.Character.Weapons.Give(WeaponHash.StickyBomb, 20, true, true);
											Game.Player.Character.Weapons.Select(WeaponHash.StickyBomb);
											if (BombSewer != null)
											{
												BombSewer.Delete();
											}
											Game.Player.Character.Task.PlayAnimation("weapons@projectile@sticky_bomb", "plant_vertical", 34f, 34f, 2500, AnimationFlags.None, 0f);
											Script.Wait(2000);
											Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
											SpawnPropBombVault("ch_prop_ch_explosive_01a", new Vector3(2505f, -239.6f, -71f), new Vector3(14f, -90f, 0f));
											SpawnPropBombVault("ch_prop_ch_explosive_01a", new Vector3(2505f, -239.7f, -69.5f), new Vector3(76f, -90f, 0f));
											SpawnPropBombVault("ch_prop_ch_explosive_01a", new Vector3(2505.119f, -237.1551f, -69.3f), new Vector3(217f, -90f, 0f));
											SpawnPropBombVault("ch_prop_ch_explosive_01a", new Vector3(2505.1f, -237.4f, -71.3f), new Vector3(135f, -90f, 0f));
											PlacedBomb = true;
											IsPlanting = false;
										}
										if (Game.IsControlJustPressed(GTA.Control.Cover))
										{
											if (!DrillComplete)
											{
												DrillComplete = true;
												Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
												Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
												StartCasinoDrill();
												Notification.Show("You can Cancel the Drilling by pressing Q");
											}
											else if (DrillComplete)
											{
												DrillComplete = false;
												Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
												Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
												DestoryDrill();
											}
										}
										if (Game.IsControlJustPressed(GTA.Control.ThrowGrenade))
										{
											if (!DrillComplete)
											{
												DrillComplete = true;
												Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
												Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
												StartCasinoLaser();
												Notification.Show("You can Cancel the Drilling by pressing G");
											}
											else if (DrillComplete)
											{
												DrillComplete = false;
												Game.Player.Character.Position = new Vector3(2504.289f, -237.8028f, -72f);
												Game.Player.Character.Rotation = new Vector3(0f, 0f, -91.29f);
												DestoryDrill();
											}
										}
									}
									if (!DrillingComplete && !PlacedBomb)
									{
										Prop prop30 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
										if (prop30 != null)
										{
											prop30.IsCollisionEnabled = true;
											prop30.IsVisible = true;
										}
									}
									if (DrillingComplete)
									{
										DrillComplete = true;
										PlayingCutscene = false;
										VaultOpenedByDrill = false;
										Function.Call(Hash.REMOVE_CUTSCENE);
										int num19 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs3f_mul_vlt");
									}
									if (!VaultOpenedByDrill)
									{
										if (PlayingCutscene && CasinoVaultDoor != null)
										{
											CasinoVaultDoor.IsCollisionEnabled = false;
											CasinoVaultDoor.IsVisible = false;
										}
										if (!PlayingCutscene && CasinoVaultDoor != null)
										{
											CasinoVaultDoor.IsCollisionEnabled = true;
											CasinoVaultDoor.IsVisible = true;
										}
										if (DrillingComplete && !PlayingCutscene)
										{
											DestoryDrill();
											DrillingComplete = false;
											int num20 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_Vault_Door");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "set_vault_dressing");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_vault_cash_01");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_vault_cash_02");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_vault_gold_01");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_vault_gold_02");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_vault_art_01");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_vault_art_02");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_vault_diamonds_01");
											Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num20, "Set_vault_diamonds_02");
											Function.Call(Hash.REFRESH_INTERIOR, num20);
											Prop prop31 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
											if (prop31 != null)
											{
												prop31.IsCollisionEnabled = false;
												prop31.IsVisible = false;
											}
											int num21 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs3f_mul_vlt");
											if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene)
											{
												if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
												{
													if (PlacedBomb)
													{
														prop31 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
														if (prop31 != null)
														{
															CasinoVaultDoor = prop31;
															CasinoVaultDoor.IsVisible = true;
														}
													}
													Ped[] nearbyPeds4 = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
													if (nearbyPeds4.Length != 0)
													{
														Ped[] array4 = nearbyPeds4;
														foreach (Ped ped4 in array4)
														{
															if (ped4 != null)
															{
																ped4.IsVisible = false;
															}
														}
													}
													SetEntityinCustscenes();
													Function.Call(Hash.START_CUTSCENE, num21);
													Script.Wait(50);
													GetPedOutfitCutscene("MP_1", Game.Player.Character);
													if (RobberA != null)
													{
														GetPedOutfitCutscene("MP_2", RobberA);
													}
													if (RobberB != null)
													{
														GetPedOutfitCutscene("MP_3", RobberB);
													}
													if (RobberC != null)
													{
														GetPedOutfitCutscene("MP_4", RobberC);
													}
												}
												if (Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
												{
													PlayingCutscene = true;
												}
											}
										}
										if (PlayingCutscene)
										{
											if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
											{
												Function.Call(Hash.STOP_CUTSCENE, true);
											}
											if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
											{
												Function.Call(Hash.REMOVE_CUTSCENE);
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
												if (RobberA != null)
												{
													RobberA.IsVisible = true;
												}
												if (RobberB != null)
												{
													RobberB.IsVisible = true;
												}
												if (RobberC != null)
												{
													RobberC.IsVisible = true;
												}
												if (PlayingCutscene)
												{
													foreach (ManageRobberyAnims.Lootable crate_Valuable3 in ManageRobberyAnims.Crate_Valuables)
													{
														if (crate_Valuable3 != null && crate_Valuable3.LootBlip != null)
														{
															crate_Valuable3.LootBlip.Alpha = 0;
														}
													}
													Notification.Show("~y~ Lester ~w~ : Get in and grab as much as you can carry!");
													GetTimeInVault();
													DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok you have got " + Waittime + " seconds, Before the Alarms Go off!");
													Notification.Show("~y~ Lester ~w~ : Get out of the Vault before that time Expires and the Alarms will not go off!");
													Game.Player.Character.Position = new Vector3(2507.7f, -238.3f, -70.7f);
													DrillingComplete = false;
													VaultClosed = false;
													VaultOpenedByDrill = true;
													Ped[] nearbyPeds5 = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
													if (nearbyPeds5.Length != 0)
													{
														Ped[] array5 = nearbyPeds5;
														foreach (Ped ped5 in array5)
														{
															if (ped5 != null)
															{
																ped5.IsVisible = true;
															}
														}
													}
													if (CasinoVaultDoor != null)
													{
														CasinoVaultDoor.IsCollisionEnabled = true;
														CasinoVaultDoor.IsVisible = true;
													}
													Notification.Show("STAGE 6");
													PlacedBomb = false;
													VaultClosed = false;
													VaultOpenedByDrill = true;
													PlayingCutscene = false;
													Function.Call(Hash.REMOVE_CUTSCENE);
													int num22 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
													if (!PlacedBomb)
													{
														Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_Vault_Door");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "set_vault_dressing");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_vault_cash_01");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_vault_cash_02");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_vault_gold_01");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_vault_gold_02");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_vault_art_01");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_vault_art_02");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_vault_diamonds_01");
														Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num22, "Set_vault_diamonds_02");
														Function.Call(Hash.REFRESH_INTERIOR, num22);
													}
													Prop prop32 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
													if (prop32 != null)
													{
														prop32.IsCollisionEnabled = true;
														prop32.IsVisible = true;
													}
												}
											}
										}
									}
								}
								if (!DrillComplete && PlacedBomb && VaultClosed && BombVault_A != null && BombVault_B != null && BombVault_C != null && BombVault_D != null && GetDistance_VDIST2(Game.Player.Character.Position, BombVault_A.Position, "<", 40f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_THROW_GRENADE~ to Detonate Bombs");
									if (Game.IsControlJustPressed(GTA.Control.ThrowGrenade))
									{
										Prop prop33 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
										if (prop33 != null)
										{
											prop33.IsCollisionEnabled = false;
											prop33.IsVisible = false;
										}
										World.AddExplosion(BombVault_C.Position, ExplosionType.Grenade, 10f, 1f);
										World.AddExplosion(BombVault_B.Position, ExplosionType.Grenade, 10f, 1f);
										World.AddExplosion(BombVault_A.Position, ExplosionType.Grenade, 10f, 1f);
										World.AddExplosion(BombVault_D.Position, ExplosionType.Grenade, 10f, 1f);
										World.AddExplosion(BombVault_C.Position, ExplosionType.PetrolPump, 10f, 1f);
										World.AddExplosion(BombVault_B.Position, ExplosionType.PetrolPump, 10f, 1f);
										World.AddExplosion(BombVault_A.Position, ExplosionType.PetrolPump, 10f, 1f);
										World.AddExplosion(BombVault_D.Position, ExplosionType.PetrolPump, 10f, 1f);
										Script.Wait(1000);
										BombVault_A.Delete();
										BombVault_B.Delete();
										BombVault_C.Delete();
										BombVault_D.Delete();
										SpawnProp("ch_prop_ch_vaultdoor01x", new Vector3(2512.9f, -240.6f, -73.6f), new Vector3(90f, 135f, 0f));
										Props[Props.Count - 1].IsPositionFrozen = true;
										int num23 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_Vault_Door");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "set_vault_dressing");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_vault_cash_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_vault_cash_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_vault_gold_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_vault_gold_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_vault_art_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_vault_art_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_vault_diamonds_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num23, "Set_vault_diamonds_02");
										VaultClosed = false;
										foreach (ManageRobberyAnims.Lootable crate_Valuable4 in ManageRobberyAnims.Crate_Valuables)
										{
											if (crate_Valuable4 != null && crate_Valuable4.LootBlip != null)
											{
												crate_Valuable4.LootBlip.Alpha = 255;
											}
										}
										Notification.Show("~y~ Lester ~w~ : Get in and grab as much as you can carry!");
										GetTimeInVault();
										DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok you have got " + Waittime + " seconds, Before the Alarms Go off!");
										Notification.Show("~y~ Lester ~w~ : Get out of the Vault before that time Expires and the Alarms will not go off!");
									}
								}
							}
							if (VaultClosed)
							{
								Prop prop34 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_wall_damage"), 0, 0, 0);
								if (prop34 != null)
								{
									prop34.IsVisible = false;
								}
								if (!PlacedBomb)
								{
									prop34 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
									if (prop34 != null)
									{
										prop34.IsCollisionEnabled = true;
										prop34.IsVisible = true;
									}
								}
							}
							if (!VaultClosed)
							{
								if (PlacedBomb)
								{
									Prop prop35 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vault_wall_damage"), 0, 0, 0);
									if (prop35 != null)
									{
										prop35.IsVisible = false;
									}
								}
								if (!PlacedBomb)
								{
									if (!VaultOpenedByDrill)
									{
										Prop prop36 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
										if (prop36 != null)
										{
											prop36.IsCollisionEnabled = false;
											prop36.IsVisible = false;
										}
									}
									if (VaultOpenedByDrill)
									{
										Prop prop37 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_vaultdoor01x"), 0, 0, 0);
										if (prop37 != null)
										{
											prop37.IsCollisionEnabled = true;
											prop37.IsVisible = true;
										}
									}
								}
							}
						}
						if (!VaultClosed)
						{
							if (OverideTasks)
							{
								if (RobberA != null && RobberARobbing)
								{
									RobberA.BlockPermanentEvents = true;
									RobberA.AlwaysKeepTask = true;
								}
								if (RobberA != null && RobberAMovingToLoot)
								{
									RobberA.AlwaysKeepTask = true;
								}
								else if (RobberA != null && !RobberAMovingToLoot && !RobberARobbing)
								{
									RobberA.BlockPermanentEvents = false;
									RobberA.AlwaysKeepTask = false;
								}
								if (RobberB != null && RobberBRobbing)
								{
									RobberB.BlockPermanentEvents = true;
									RobberB.AlwaysKeepTask = true;
								}
								if (RobberB != null && RobberBMovingToLoot)
								{
									RobberB.AlwaysKeepTask = true;
								}
								else if (RobberB != null && !RobberBMovingToLoot && !RobberBRobbing)
								{
									RobberB.BlockPermanentEvents = false;
									RobberB.AlwaysKeepTask = false;
								}
								if (RobberC != null && RobberCRobbing)
								{
									RobberC.BlockPermanentEvents = true;
									RobberC.AlwaysKeepTask = true;
								}
								if (RobberC != null && RobberCMovingToLoot)
								{
									RobberC.AlwaysKeepTask = true;
								}
								else if (RobberC != null && !RobberCMovingToLoot && !RobberCRobbing)
								{
									RobberC.BlockPermanentEvents = false;
									RobberC.AlwaysKeepTask = false;
								}
							}
							if (!CanNoLongerOverideCommands && !OverideTasks)
							{
								OverideTasks = true;
								if (RobberA != null)
								{
									RobberA.Task.ClearAll();
									RobberA.Opacity = 255;
								}
								if (RobberB != null)
								{
									RobberB.Task.ClearAll();
									RobberB.Opacity = 255;
								}
								if (RobberC != null)
								{
									RobberC.Task.ClearAll();
									RobberC.Opacity = 255;
								}
								RobberAMovingToLoot = false;
								RobberBMovingToLoot = false;
								RobberCMovingToLoot = false;
							}
							if (CurrentPay <= MaxPay)
							{
								if (CreatedCrates)
								{
									if (RobberA != null)
									{
										Vector3 offsetPosition4 = RobberA.GetOffsetPosition(new Vector3(0f, 0f, 1.55f));
										World.DrawMarker(MarkerType.Num1, offsetPosition4, Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), Color.Goldenrod);
									}
									if (RobberB != null)
									{
										Vector3 offsetPosition5 = RobberB.GetOffsetPosition(new Vector3(0f, 0f, 1.55f));
										World.DrawMarker(MarkerType.Num2, offsetPosition5, Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), Color.Goldenrod);
									}
									if (RobberC != null)
									{
										Vector3 offsetPosition6 = RobberC.GetOffsetPosition(new Vector3(0f, 0f, 1.55f));
										World.DrawMarker(MarkerType.Num3, offsetPosition6, Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), Color.Goldenrod);
									}
									if (!VaultDone)
									{
										VaultLooting();
									}
								}
								if (SetupPaintingsOrLockVault)
								{
									VaultLooting();
								}
								if (AllowMix && CreatedCrates)
								{
									VaultLooting();
								}
								if (!SetupPaintingsOrLockVault && CreatedCrates)
								{
									VaultLooting();
								}
							}
						}
						if (!OverideAlarm && (!NotifyOutVault || !NotifyOutVault2) && CurrentPay >= MaxPay && Waittime > 10)
						{
							Notification.Show("~y~ Lester ~w~ : You have got all you can Carry, Make your way out of the Vault and I'll inform you what to do");
							NotifyOutVault = true;
							NotifyOutVault2 = true;
						}
						if (OverideAlarm && !HeadingtoyourAppartment && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2492.6f, -238.5f, -70f), "<", 10f))
						{
							if (CurrentPay <= MaxPay)
							{
								RobberAMovingToLoot = false;
								RobberBMovingToLoot = false;
								RobberCMovingToLoot = false;
								VaultDone = true;
								Notification.Show("~y~ Lester ~w~ : Ok if you have got all you want/can carry make your way to your Getaway Car");
							}
							if (CurrentPay >= MaxPay)
							{
								RobberAMovingToLoot = false;
								RobberBMovingToLoot = false;
								RobberCMovingToLoot = false;
								VaultDone = true;
								Notification.Show("~y~ Lester ~w~ : Ok you have got all you can carry make your way to your Getaway Car");
							}
							if (ExitUsed == 0)
							{
								Notification.Show("~y~ Lester ~w~ : Your Getaway Car is located at the front of the Casino");
								try
								{
									if (SecondaryGetawayVeh != null)
									{
										SecondaryGetawayVeh.Delete();
									}
									SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
									VehicleChoosen = SaveCar.VehicleName;
									SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(914.8f, 52.6f, 80.2f), -32f);
									if (SecondaryGetawayVeh.AttachedBlip != null)
									{
										SecondaryGetawayVeh.AttachedBlip.Delete();
									}
									SecondaryGetawayVeh.AddBlip();
									SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
									SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
									SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.Repair();
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.IsEngineRunning = true;
									SecondaryGetawayVeh.IsDriveable = true;
									SecondaryGetawayVeh.IsInvincible = true;
									SecondaryGetawayVeh.IsPositionFrozen = true;
								}
								catch
								{
									Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
								}
							}
							if (ExitUsed == 1)
							{
								Notification.Show("~y~ Lester ~w~ : Your Getaway Car is located in the Sewers, Make your way back to the tunnel");
								try
								{
									if (SecondaryGetawayVeh != null)
									{
										SecondaryGetawayVeh.Delete();
									}
									SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
									VehicleChoosen = SaveCar.VehicleName;
									SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(988.9f, -146.2f, 33.9f), -132f);
									if (SecondaryGetawayVeh.AttachedBlip != null)
									{
										SecondaryGetawayVeh.AttachedBlip.Delete();
									}
									SecondaryGetawayVeh.AddBlip();
									SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
									SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
									SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.Repair();
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.IsEngineRunning = true;
									SecondaryGetawayVeh.IsDriveable = true;
									SecondaryGetawayVeh.IsInvincible = true;
									SecondaryGetawayVeh.IsPositionFrozen = true;
								}
								catch
								{
									Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
								}
							}
							if (ExitUsed == 2)
							{
								Notification.Show("~y~ Lester ~w~ : Your Getaway heli is located in the Roof, Make your way to the Hotel Floor");
								try
								{
									if (SecondaryGetawayVeh != null)
									{
										SecondaryGetawayVeh.Delete();
									}
									SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
									VehicleChoosen = SaveCar.VehicleName;
									SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
									if (SecondaryGetawayVeh.AttachedBlip != null)
									{
										SecondaryGetawayVeh.AttachedBlip.Delete();
									}
									SecondaryGetawayVeh.AddBlip();
									SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
									SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
									SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.Repair();
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.IsEngineRunning = true;
									SecondaryGetawayVeh.IsDriveable = true;
									SecondaryGetawayVeh.IsInvincible = true;
									SecondaryGetawayVeh.IsPositionFrozen = true;
								}
								catch
								{
									Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
								}
							}
							if (ExitUsed == 3)
							{
								Notification.Show("~y~ Lester ~w~ : Your Getaway heli is located in the Roof, Make your way to the Hotel Floor");
								try
								{
									if (SecondaryGetawayVeh != null)
									{
										SecondaryGetawayVeh.Delete();
									}
									SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
									VehicleChoosen = SaveCar.VehicleName;
									SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
									if (SecondaryGetawayVeh.AttachedBlip != null)
									{
										SecondaryGetawayVeh.AttachedBlip.Delete();
									}
									SecondaryGetawayVeh.AddBlip();
									SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
									SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
									SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.Repair();
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.IsEngineRunning = true;
									SecondaryGetawayVeh.IsDriveable = true;
									SecondaryGetawayVeh.IsInvincible = true;
									SecondaryGetawayVeh.IsPositionFrozen = true;
								}
								catch
								{
									Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
								}
							}
							int exitUsed = ExitUsed;
							NotifyOutVault = true;
							NotifyOutVault2 = true;
							HeadingtoyourAppartment = true;
						}
						if (!VaultDone)
						{
							if (!VaultClosed && !TriggeredWantedLevelEscape && !OverideAlarm)
							{
								if (Waittime != 0)
								{
									if (ManageRobberyAnims.CurrentPay >= MaxPay && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2499f, -238.5f, -70f), "<", 10f))
									{
										CanNoLongerOverideCommands = true;
										if (RobberA != null)
										{
											RobberA.Task.ClearAll();
											RobberA.Task.ClearAll();
										}
										if (RobberB != null)
										{
											RobberB.Task.ClearAll();
											RobberB.Task.ClearAll();
											Ped robberB = RobberB;
											PedGroup pedGroup = Game.Player.Character.PedGroup;
											Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberB, pedGroup);
											Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberB, 100);
											Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, robberB, true);
										}
										if (RobberC != null)
										{
											RobberC.Task.ClearAll();
											RobberC.Task.ClearAll();
											Ped robberC = RobberC;
											PedGroup pedGroup2 = Game.Player.Character.PedGroup;
											Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberC, pedGroup2);
											Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberC, 100);
											Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, robberC, true);
										}
										World.DrawMarker(MarkerType.Sphere, new Vector3(2499f, -238.5f, -70f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
										OverideAlarm = true;
										TriggeredWantedLevelEscape = true;
									}
									if (timer != 25)
									{
										timer++;
									}
									if (timer == 25)
									{
										timer = 0;
										Waittime--;
									}
								}
								if (((Waittime == 0 && ManageRobberyAnims.CurrentPay > 25000) || PoliceAlearted) && !TriggeredWantedLevelEscape)
								{
									if (CurrentPay >= 25000)
									{
										HeadingtoyourAppartment = true;
										TriggeredWantedLevelEscape = true;
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2499f, -238.5f, -70f), ">", 10f))
									{
										AlarmsGoingOff = true;
										Notification.Show("~y~ Lester ~w~ : Alarms have been triggered!");
									}
									if (AlarmsGoingOff || PoliceAlearted)
									{
										if (!OverideAlarm && !Game.Player.Character.IsInCombat)
										{
											OverideAlarm = true;
											Function.Call(Hash.REMOVE_CUTSCENE);
											int num24 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs3f_all_esc");
											Function.Call(Hash.START_CUTSCENE, num24);
											Script.Wait(12000);
											SetupSwat();
										}
										if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
										{
											foreach (ObjectPlacementMap swatEntty3 in SwatEntties)
											{
												swatEntty3.CreateObjatCoords();
												if (swatEntty3.Entity != null && swatEntty3.Entity.Model.IsPed)
												{
													DiamdondCasinoSmartGuards.SpawnGroupsUse.Add(new SmartGuards.SmartAiV2((Ped)swatEntty3.Entity, SmartGuards.EnemyGroup.Diamond_Casino_Security));
												}
											}
											TriggerFullResponce = true;
											Notification.Show("~y~Lester~w~ Police Alerted Diamond Casino Security to a possible intruder, Police have setup roadblocks and swat teams around the casino");
											SmartGuards.GetCurrentGroupSettings().IngorePlayer = false;
											TriggeredWantedLevelEscape = true;
										}
									}
									AlarmTimer = 0;
								}
							}
							bool triggeredWantedLevelEscape = TriggeredWantedLevelEscape;
						}
						if ((NotifyOutVault2 || NotifyOutVault) && Waittime >= -1 && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2499f, -238.5f, -70f), "<", 10f))
						{
							CanNoLongerOverideCommands = true;
							Waittime = -50;
							OverideTasks = false;
							if (RobberA != null)
							{
								RobberA.Task.RunTo(new Vector3(2494.6f, -238.5f, -70f), false, 9999);
							}
							if (RobberB != null)
							{
								RobberB.Task.RunTo(new Vector3(2494.6f, -238.5f, -70f), false, 9999);
							}
							if (RobberC != null)
							{
								RobberC.Task.RunTo(new Vector3(2494.6f, -238.5f, -70f), false, 9999);
							}
							if (ManageRobberyAnims.CurrentPay >= MaxPay)
							{
								Notification.Show("~y~ Lester ~w~ : Ok you have got all you can carry");
							}
							if (ExitUsed == 0)
							{
								Notification.Show("~y~ Lester ~w~ : Your Getaway Car is located at the front of the Casino");
								if (EnteranceUsed != 0)
								{
									try
									{
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
										VehicleChoosen = SaveCar.VehicleName;
										SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(914.8f, 52.6f, 80.2f), -32f);
										if (SecondaryGetawayVeh.AttachedBlip != null)
										{
											SecondaryGetawayVeh.AttachedBlip.Delete();
										}
										SecondaryGetawayVeh.AddBlip();
										SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
										SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
										SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.Repair();
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.IsEngineRunning = true;
										SecondaryGetawayVeh.IsDriveable = true;
										SecondaryGetawayVeh.IsInvincible = true;
										SecondaryGetawayVeh.IsPositionFrozen = true;
									}
									catch
									{
										Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
									}
								}
							}
							if (ExitUsed == 1)
							{
								Notification.Show("~y~ Lester ~w~ : Your Getaway Car is located in the Sewers, Make your way back to the tunnel");
								try
								{
									if (SecondaryGetawayVeh != null)
									{
										SecondaryGetawayVeh.Delete();
									}
									SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
									VehicleChoosen = SaveCar.VehicleName;
									SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(988.9f, -146.2f, 33.9f), -132f);
									if (SecondaryGetawayVeh.AttachedBlip != null)
									{
										SecondaryGetawayVeh.AttachedBlip.Delete();
									}
									SecondaryGetawayVeh.AddBlip();
									SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
									SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
									SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.Repair();
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.IsEngineRunning = true;
									SecondaryGetawayVeh.IsDriveable = true;
									SecondaryGetawayVeh.IsInvincible = true;
									SecondaryGetawayVeh.IsPositionFrozen = true;
								}
								catch
								{
									Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
								}
							}
							if (ExitUsed == 2)
							{
								Notification.Show("~y~ Lester ~w~ : Your Getaway heli is located in the Roof, Make your way to the Hotel Floor");
								try
								{
									if (SecondaryGetawayVeh != null)
									{
										SecondaryGetawayVeh.Delete();
									}
									SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
									VehicleChoosen = SaveCar.VehicleName;
									SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
									if (SecondaryGetawayVeh.AttachedBlip != null)
									{
										SecondaryGetawayVeh.AttachedBlip.Delete();
									}
									SecondaryGetawayVeh.AddBlip();
									SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
									SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
									SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.Repair();
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.IsEngineRunning = true;
									SecondaryGetawayVeh.IsDriveable = true;
									SecondaryGetawayVeh.IsInvincible = true;
									SecondaryGetawayVeh.IsPositionFrozen = true;
								}
								catch
								{
									Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
								}
							}
							if (ExitUsed == 3)
							{
								Notification.Show("~y~ Lester ~w~ : Your Getaway heli is located in the Roof, Make your way to the Hotel Floor");
								try
								{
									if (SecondaryGetawayVeh != null)
									{
										SecondaryGetawayVeh.Delete();
									}
									SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
									VehicleChoosen = SaveCar.VehicleName;
									SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
									if (SecondaryGetawayVeh.AttachedBlip != null)
									{
										SecondaryGetawayVeh.AttachedBlip.Delete();
									}
									SecondaryGetawayVeh.AddBlip();
									SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
									SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
									SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.Repair();
									SecondaryGetawayVeh.IsPositionFrozen = false;
									SecondaryGetawayVeh.IsEngineRunning = true;
									SecondaryGetawayVeh.IsDriveable = true;
									SecondaryGetawayVeh.IsInvincible = true;
									SecondaryGetawayVeh.IsPositionFrozen = true;
								}
								catch
								{
									Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
								}
							}
							NotifyOutVault = true;
							NotifyOutVault2 = true;
							HeadingtoyourAppartment = true;
						}
						if (GotCash && !NotifyOutVault)
						{
							if (!NotifyOutVault2 && ManageRobberyAnims.CurrentPay >= MaxPay)
							{
								foreach (ManageRobberyAnims.Lootable crate_Valuable5 in ManageRobberyAnims.Crate_Valuables)
								{
									if (crate_Valuable5 != null && crate_Valuable5.LootBlip != null)
									{
										crate_Valuable5.LootBlip.Alpha = 0;
									}
								}
								NotifyOutVault2 = true;
							}
							if (GotCash && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2494.6f, -238.5f, -70f), "<", 10f) && !HeadingtoyourAppartment)
							{
								CanNoLongerOverideCommands = true;
								OverideTasks = false;
								if (RobberA != null)
								{
									RobberA.Task.RunTo(new Vector3(2494.6f, -238.5f, -70f), false, 9999);
								}
								if (RobberB != null)
								{
									RobberB.Task.RunTo(new Vector3(2494.6f, -238.5f, -70f), false, 9999);
								}
								if (RobberC != null)
								{
									RobberC.Task.RunTo(new Vector3(2494.6f, -238.5f, -70f), false, 9999);
								}
								if (ManageRobberyAnims.CurrentPay <= MaxPay)
								{
									RobberAMovingToLoot = false;
									RobberBMovingToLoot = false;
									RobberCMovingToLoot = false;
									VaultDone = true;
									Notification.Show("~y~ Lester ~w~ : Ok if you have got all you want/can carry make your way to your Getaway Car");
								}
								if (ManageRobberyAnims.CurrentPay >= MaxPay)
								{
									RobberAMovingToLoot = false;
									RobberBMovingToLoot = false;
									RobberCMovingToLoot = false;
									VaultDone = true;
									Notification.Show("~y~ Lester ~w~ : Ok you have got all you can carry make your way to your Getaway Car");
								}
								if (ExitUsed == 0)
								{
									Notification.Show("~y~ Lester ~w~ : Your Getaway Car is located at the front of the Casino");
									if (EnteranceUsed != 0)
									{
										try
										{
											if (SecondaryGetawayVeh != null)
											{
												SecondaryGetawayVeh.Delete();
											}
											SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
											VehicleChoosen = SaveCar.VehicleName;
											SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(914.8f, 52.6f, 80.2f), -32f);
											if (SecondaryGetawayVeh.AttachedBlip != null)
											{
												SecondaryGetawayVeh.AttachedBlip.Delete();
											}
											SecondaryGetawayVeh.AddBlip();
											SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
											SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
											SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
											SecondaryGetawayVeh.IsPositionFrozen = false;
											SecondaryGetawayVeh.Repair();
											SecondaryGetawayVeh.IsPositionFrozen = false;
											SecondaryGetawayVeh.IsEngineRunning = true;
											SecondaryGetawayVeh.IsDriveable = true;
											SecondaryGetawayVeh.IsInvincible = true;
											SecondaryGetawayVeh.IsPositionFrozen = true;
										}
										catch
										{
											Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
										}
									}
								}
								if (ExitUsed == 1)
								{
									Notification.Show("~y~ Lester ~w~ : Your Getaway Car is located in the Sewers, Make your way back to the tunnel");
									try
									{
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
										VehicleChoosen = SaveCar.VehicleName;
										SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(988.9f, -146.2f, 33.9f), -132f);
										if (SecondaryGetawayVeh.AttachedBlip != null)
										{
											SecondaryGetawayVeh.AttachedBlip.Delete();
										}
										SecondaryGetawayVeh.AddBlip();
										SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.GetawayCar;
										SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
										SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Car";
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.Repair();
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.IsEngineRunning = true;
										SecondaryGetawayVeh.IsDriveable = true;
										SecondaryGetawayVeh.IsInvincible = true;
										SecondaryGetawayVeh.IsPositionFrozen = true;
									}
									catch
									{
										Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
									}
								}
								if (ExitUsed == 2)
								{
									Notification.Show("~y~ Lester ~w~ : Your Getaway heli is located in the Roof, Make your way to the Hotel Floor");
									try
									{
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
										VehicleChoosen = SaveCar.VehicleName;
										SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
										if (SecondaryGetawayVeh.AttachedBlip != null)
										{
											SecondaryGetawayVeh.AttachedBlip.Delete();
										}
										SecondaryGetawayVeh.AddBlip();
										SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
										SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
										SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.Repair();
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.IsEngineRunning = true;
										SecondaryGetawayVeh.IsDriveable = true;
										SecondaryGetawayVeh.IsInvincible = true;
										SecondaryGetawayVeh.IsPositionFrozen = true;
									}
									catch
									{
										Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
									}
								}
								if (ExitUsed == 3)
								{
									Notification.Show("~y~ Lester ~w~ : Your Getaway heli is located in the Roof, Make your way to the Hotel Floor");
									try
									{
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
										VehicleChoosen = SaveCar.VehicleName;
										SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
										if (SecondaryGetawayVeh.AttachedBlip != null)
										{
											SecondaryGetawayVeh.AttachedBlip.Delete();
										}
										SecondaryGetawayVeh.AddBlip();
										SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
										SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
										SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.Repair();
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.IsEngineRunning = true;
										SecondaryGetawayVeh.IsDriveable = true;
										SecondaryGetawayVeh.IsInvincible = true;
										SecondaryGetawayVeh.IsPositionFrozen = true;
									}
									catch
									{
										Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
									}
								}
								NotifyOutVault = true;
								NotifyOutVault2 = true;
								HeadingtoyourAppartment = true;
							}
						}
						if (EnteranceUsed == 2)
						{
							if (Intunnel)
							{
								if (RobberA != null)
								{
									RobberA.IsCollisionEnabled = false;
									RobberA.Position = Game.Player.Character.Position;
									RobberA.IsVisible = false;
								}
								if (RobberB != null)
								{
									RobberB.IsCollisionEnabled = false;
									RobberB.Position = Game.Player.Character.Position;
									RobberB.IsVisible = false;
								}
								if (RobberC != null)
								{
									RobberC.IsCollisionEnabled = false;
									RobberC.Position = Game.Player.Character.Position;
									RobberC.IsVisible = false;
								}
							}
							if (!DetonatedSewerBomb && SetSewerBomb && BombSewer != null && GetDistance_VDIST2(Game.Player.Character.Position, BombSewer.Position, "<", 40f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_THROW_GRENADE~ to Detonate C4");
								if (Game.IsControlJustPressed(GTA.Control.ThrowGrenade))
								{
									World.AddExplosion(BombSewer.Position, ExplosionType.Grenade, 10f, 1f);
									World.AddExplosion(BombSewer.Position, ExplosionType.PetrolPump, 10f, 1f);
									Prop prop38 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, 2504.97f, -240.3102f, -70.17885f, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_tunnel_fake_wall"), 0, 0, 0);
									if (prop38 != null)
									{
										prop38.IsCollisionEnabled = false;
										prop38.IsVisible = false;
									}
									Script.Wait(1000);
									BombSewer.Delete();
									DetonatedSewerBomb = true;
								}
							}
							if (!SetSewerBomb)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2480.1f, -293.3f, -72f), "<", 100f))
								{
									World.DrawMarker(MarkerType.Cylinder, new Vector3(2480.1f, -293.3f, -72f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2480.1f, -293.3f, -72f), "<", 2f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Plant C4 Charge");
									if (Game.IsControlJustPressed(GTA.Control.Context))
									{
										Game.Player.Character.Weapons.Give(WeaponHash.StickyBomb, 20, true, true);
										Game.Player.Character.Weapons.Select(WeaponHash.StickyBomb);
										if (BombSewer != null)
										{
											BombSewer.Delete();
										}
										Game.Player.Character.Task.PlayAnimation("weapons@projectile@sticky_bomb", "plant_vertical", 34f, 34f, 2500, AnimationFlags.None, 0f);
										Script.Wait(1700);
										Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
										SpawnPropBombSewer("ch_prop_ch_ld_bomb_01a", new Vector3(2479.9f, -292.3f, -70.8f), new Vector3(0f, -90f, 0f));
										SetSewerBomb = true;
									}
								}
							}
							if (SetSewerBomb && DetonatedSewerBomb)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2480.1f, -293.3f, -71f), "<", 2f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Move through Crevis");
									if (Game.IsControlJustPressed(GTA.Control.Context))
									{
										Game.Player.Character.Rotation = new Vector3(0f, 0f, 0f);
										Game.Player.Character.Task.PlayAnimation("missarmenian3mcs_1a", "climb_in_window_v2", 34f, 34f, 10500, AnimationFlags.None, 0f);
										Game.Player.Character.IsCollisionEnabled = false;
										Game.Player.Character.IsPositionFrozen = true;
										Script.Wait(6400);
										Game.Player.Character.IsCollisionEnabled = true;
										Game.Player.Character.IsPositionFrozen = false;
										Game.Player.Character.Position = new Vector3(2479.6f, -290.2f, -71f);
										if (RobberA != null)
										{
											RobberA.Task.ClearAll();
											RobberA.Position = new Vector3(2488.5f, -284.9f, -70.7f);
											RobberA.Task.ClearAll();
										}
										if (RobberB != null)
										{
											RobberB.Task.ClearAll();
											RobberB.Position = new Vector3(2487.3f, -284.9f, -70.7f);
											RobberB.Task.ClearAll();
											Ped robberB2 = RobberB;
											PedGroup pedGroup3 = Game.Player.Character.PedGroup;
											Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberB2, pedGroup3);
											Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberB2, 100);
										}
										if (RobberC != null)
										{
											RobberC.Task.ClearAll();
											RobberC.Position = new Vector3(2486.1f, -284.9f, -70.7f);
											RobberC.Task.ClearAll();
											Ped robberC2 = RobberC;
											PedGroup pedGroup4 = Game.Player.Character.PedGroup;
											Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberC2, pedGroup4);
											Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberC2, 100);
										}
										Intunnel = false;
										if (RobberA != null)
										{
											RobberA.IsCollisionEnabled = true;
											RobberA.Position = Game.Player.Character.Position.Around(1f);
											RobberA.IsVisible = true;
										}
										if (RobberB != null)
										{
											RobberB.IsCollisionEnabled = true;
											RobberB.Position = Game.Player.Character.Position.Around(1f);
											RobberB.IsVisible = true;
										}
										if (RobberC != null)
										{
											RobberC.IsCollisionEnabled = true;
											RobberC.Position = Game.Player.Character.Position.Around(1f);
											RobberC.IsVisible = true;
										}
									}
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2479.6f, -290.2f, -71f), "<", 2f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Move through Crevis");
									if (Game.IsControlJustPressed(GTA.Control.Context))
									{
										Game.Player.Character.Rotation = new Vector3(0f, 0f, 180f);
										Game.Player.Character.Task.PlayAnimation("missarmenian3mcs_1a", "climb_in_window_v2", 34f, 34f, 10500, AnimationFlags.None, 0f);
										Game.Player.Character.IsCollisionEnabled = false;
										Game.Player.Character.IsPositionFrozen = true;
										Script.Wait(6400);
										Game.Player.Character.IsCollisionEnabled = true;
										Game.Player.Character.IsPositionFrozen = false;
										Game.Player.Character.Position = new Vector3(2480.1f, -293.3f, -71f);
										if (RobberA != null)
										{
											RobberA.Task.ClearAll();
											RobberA.Position = new Vector3(2488.5f, -284.9f, -70.7f);
											RobberA.Task.ClearAll();
										}
										if (RobberB != null)
										{
											RobberB.Task.ClearAll();
											RobberB.Position = new Vector3(2487.3f, -284.9f, -70.7f);
											RobberB.Task.ClearAll();
											Ped robberB3 = RobberB;
											PedGroup pedGroup5 = Game.Player.Character.PedGroup;
											Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberB3, pedGroup5);
											Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberB3, 100);
										}
										if (RobberC != null)
										{
											RobberC.Task.ClearAll();
											RobberC.Position = new Vector3(2486.1f, -284.9f, -70.7f);
											RobberC.Task.ClearAll();
											Ped robberC3 = RobberC;
											PedGroup pedGroup6 = Game.Player.Character.PedGroup;
											Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberC3, pedGroup6);
											Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberC3, 100);
										}
										Intunnel = false;
										if (RobberA != null)
										{
											RobberA.IsCollisionEnabled = true;
											RobberA.Position = Game.Player.Character.Position.Around(1f);
											RobberA.IsVisible = true;
										}
										if (RobberB != null)
										{
											RobberB.IsCollisionEnabled = true;
											RobberB.Position = Game.Player.Character.Position.Around(1f);
											RobberB.IsVisible = true;
										}
										if (RobberC != null)
										{
											RobberC.IsCollisionEnabled = true;
											RobberC.Position = Game.Player.Character.Position.Around(1f);
											RobberC.IsVisible = true;
										}
									}
								}
							}
						}
						if ((EnteranceUsed != 1 || EnteranceUsed != 5) && !TriggeredAttack)
						{
							if (EnteranceUsed != 1 && EnteranceUsed != 5 && ExitUsed != 4 && Game.Player.Character.Weapons.Current != null && Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed)
							{
								WeaponHash hash = Game.Player.Character.Weapons.Current.Hash;
								Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_BRIEFCASE");
							}
							if ((EnteranceUsed == 1 || (EnteranceUsed == 5 && ExitUsed == 4)) && Game.Player.Character.Weapons.Current != null && Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed)
							{
								WeaponHash hash2 = Game.Player.Character.Weapons.Current.Hash;
								Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_BRIEFCASE");
							}
						}
						if (MetalDetectorOn)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2520.5f, -260.2f, -24.5f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(2520.5f, -260.2f, -24.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2520.5f, -260.2f, -24.5f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Disable Metal Detectors");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									Game.Player.Character.Task.PlayAnimation("anim@heists@humane_labs@emp@hack_door", "hack_intro", 34f, 34f, 2500, AnimationFlags.None, 0f);
									MetalDetectorOn = false;
								}
							}
						}
						if (ExitUsed != 0)
						{
							Vector3 vector3 = new Vector3(2485.079f, -288.2985f, -70.54587f);
							Prop prop39 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector3.X, vector3.Y, vector3.Z, 2.5f, Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_rc_door2"), 0, 0, 0);
							if (prop39 != null)
							{
								prop39.IsPositionFrozen = true;
							}
							ManageDoor(new Vector3(2500.935f, -278.3725f, -58.57317f), "ch_prop_casino_door_01c", true);
							ManageDoor(new Vector3(2524.829f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a", true);
							ManageDoor(new Vector3(2526.828f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a", true);
						}
						if (ExitUsed == 0 && !VaultClosed)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2468.9f, -287.1f, -59f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(2468.9f, -287.1f, -59f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2468.9f, -287.1f, -59f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Casino (2)");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									Script.Wait(1);
									IsInterior = false;
									Game.Player.Character.Position = new Vector3(925.7f, 45.8f, 81f);
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(1f, 0f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(2f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(3f, 0f, 0f));
									}
								}
							}
						}
						if (EnteranceUsed == 1)
						{
							if (VaultClosed && !TriggerFullResponce)
							{
								int num25 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs3f_sub_vlt");
								if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED))
								{
									if (!PlayingCutscene && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2486.3f, -238.5f, -70f), "<", 10f) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
									{
										Ped[] nearbyPeds6 = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
										if (nearbyPeds6.Length != 0)
										{
											Ped[] array6 = nearbyPeds6;
											foreach (Ped ped6 in array6)
											{
												if (ped6 != null)
												{
													ped6.IsVisible = false;
												}
											}
										}
										int num26 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_Vault_Door");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "set_vault_dressing");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_vault_cash_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_vault_cash_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_vault_gold_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_vault_gold_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_vault_art_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_vault_art_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_vault_diamonds_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num26, "Set_vault_diamonds_02");
										Function.Call(Hash.REFRESH_INTERIOR, num26);
										SetEntityinCustscenes();
										Function.Call(Hash.START_CUTSCENE, num25);
										Script.Wait(100);
										PlayingCutscene = true;
										GetPedOutfitCutscene("MP_1", Game.Player.Character);
										if (RobberA != null)
										{
											GetPedOutfitCutscene("MP_2", RobberA);
										}
										if (RobberB != null)
										{
											GetPedOutfitCutscene("MP_3", RobberB);
										}
										if (RobberC != null)
										{
											GetPedOutfitCutscene("MP_4", RobberC);
										}
									}
									if (PlayingCutscene)
									{
										if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
										{
											Function.Call(Hash.STOP_CUTSCENE, true);
										}
										if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
										{
											Function.Call(Hash.REMOVE_CUTSCENE);
											if (PlayingCutscene)
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
												if (RobberA != null)
												{
													RobberA.IsVisible = true;
												}
												if (RobberB != null)
												{
													RobberB.IsVisible = true;
												}
												if (RobberC != null)
												{
													RobberC.IsVisible = true;
												}
												Notification.Show("~y~ Lester ~w~ : Get in and grab as much as you can carry!");
												GetTimeInVault();
												Notification.Show("~y~ Lester ~w~ : Get out of the Vault before that time Expires and the Alarms will not go off!");
												int num27 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
												Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_Vault_Door");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "set_vault_dressing");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_vault_cash_01");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_vault_cash_02");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_vault_gold_01");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_vault_gold_02");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_vault_art_01");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_vault_art_02");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_vault_diamonds_01");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num27, "Set_vault_diamonds_02");
												Function.Call(Hash.REFRESH_INTERIOR, num27);
												VaultClosed = false;
												Ped[] nearbyPeds7 = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
												if (nearbyPeds7.Length != 0)
												{
													Ped[] array7 = nearbyPeds7;
													foreach (Ped ped7 in array7)
													{
														if (ped7 != null)
														{
															ped7.IsVisible = true;
														}
													}
												}
												Script.Wait(500);
												VaultClosed = false;
											}
										}
									}
								}
							}
							bool vaultClosed = VaultClosed;
						}
						if (EnteranceUsed == 5 && ExitUsed == 4)
						{
							if (!TriggerFullResponce && VaultClosed && !TriggerFullResponce)
							{
								int num29 = Function.Call<int>(Hash.REQUEST_CUTSCENE, "hs3f_sub_vlt");
								if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED))
								{
									if (!PlayingCutscene && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2486.3f, -238.5f, -70f), "<", 10f) && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
									{
										Ped[] nearbyPeds8 = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
										if (nearbyPeds8.Length != 0)
										{
											Ped[] array8 = nearbyPeds8;
											foreach (Ped ped8 in array8)
											{
												if (ped8 != null)
												{
													ped8.IsVisible = false;
												}
											}
										}
										int num31 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_Vault_Door");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "set_vault_dressing");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_vault_cash_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_vault_cash_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_vault_gold_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_vault_gold_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_vault_art_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_vault_art_02");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_vault_diamonds_01");
										Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num31, "Set_vault_diamonds_02");
										Function.Call(Hash.REFRESH_INTERIOR, num31);
										SetEntityinCustscenes();
										Function.Call(Hash.START_CUTSCENE, num29);
										Script.Wait(100);
										PlayingCutscene = true;
										GetPedOutfitCutscene("MP_1", Game.Player.Character);
										if (RobberA != null)
										{
											GetPedOutfitCutscene("MP_2", RobberA);
										}
										if (RobberB != null)
										{
											GetPedOutfitCutscene("MP_3", RobberB);
										}
										if (RobberC != null)
										{
											GetPedOutfitCutscene("MP_4", RobberC);
										}
									}
									if (PlayingCutscene)
									{
										if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
										{
											Function.Call(Hash.STOP_CUTSCENE, true);
										}
										if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
										{
											Function.Call(Hash.REMOVE_CUTSCENE);
											if (PlayingCutscene)
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
												if (RobberA != null)
												{
													RobberA.IsVisible = true;
												}
												if (RobberB != null)
												{
													RobberB.IsVisible = true;
												}
												if (RobberC != null)
												{
													RobberC.IsVisible = true;
												}
												Notification.Show("~y~ Lester ~w~ : Get in and grab as much as you can carry!");
												GetTimeInVault();
												Notification.Show("~y~ Lester ~w~ : Get out of the Vault before that time Expires and the Alarms will not go off!");
												int num32 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 2488.348, -267.3637, -71.64563);
												Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_Vault_Door");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "set_vault_dressing");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_vault_cash_01");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_vault_cash_02");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_vault_gold_01");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_vault_gold_02");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_vault_art_01");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_vault_art_02");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_vault_diamonds_01");
												Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, num32, "Set_vault_diamonds_02");
												Function.Call(Hash.REFRESH_INTERIOR, num32);
												VaultClosed = false;
												Ped[] nearbyPeds9 = World.GetNearbyPeds(new Vector3(2486.3f, -238.5f, -70f), 40f, "s_m_y_westsec_02");
												if (nearbyPeds9.Length != 0)
												{
													Ped[] array9 = nearbyPeds9;
													foreach (Ped ped9 in array9)
													{
														if (ped9 != null)
														{
															ped9.IsVisible = true;
														}
													}
												}
												Script.Wait(500);
												VaultClosed = false;
											}
										}
									}
								}
							}
							if (!VaultClosed && HeadingtoyourAppartment)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2652.2f, -341.3f, -66f), "<", 100f))
								{
									World.DrawMarker(MarkerType.Cylinder, new Vector3(2652.2f, -341.3f, -66f), Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 0.75f), Color.Goldenrod);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2652.2f, -341.3f, -66f), "<", 2f))
								{
									if (Game.Player.Character.CurrentVehicle != null)
									{
										if (Game.Player.Character.CurrentVehicle.Model == VehicleHash.Burrito2)
										{
											DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Casino as Bugstars");
											if (Game.IsControlJustPressed(GTA.Control.Context))
											{
												IsInterior = false;
												Game.Player.Character.CurrentVehicle.Position = new Vector3(2553.963f, -281.3805f, -65.72305f);
												Script.Wait(1);
												Game.Player.Character.CurrentVehicle.Position = new Vector3(932.7f, -4.3f, 78f);
												Game.Player.Character.CurrentVehicle.Rotation = new Vector3(0f, 0f, 144f);
											}
										}
										else
										{
											DisplayHelpTextThisFrame("You need to be in a Bugstars Van to Exit the Loading Bay");
										}
									}
									else
									{
										DisplayHelpTextThisFrame("You need to be in a Bugstars Van to Exit the Loading Bay");
									}
								}
							}
						}
						if (EnteranceUsed == 2 && ExitUsed == 1 && !VaultClosed)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2517.2f, -327.2f, -71.5f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(2517.2f, -327.2f, -71.5f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2517.2f, -327.2f, -71.5f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit Tunnel");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									BypassMetalDetector = false;
									IsInterior = false;
									Game.Player.Character.Position = new Vector3(992.7f, -141.7f, 34f);
									Function.Call(Hash.GET_INTERIOR_AT_COORDS, 992.7f, -141.7f, 34);
									if (GetawayCar != null)
									{
										GetawayCar.Position = new Vector3(1034f, -273.8f, 50f);
										GetawayCar.Rotation = new Vector3(0f, 0f, -147f);
										Game.Player.Character.Rotation = new Vector3(0f, 0f, -166f);
										Game.Player.Character.Position = new Vector3(1036f, -270f, 51f);
									}
									if (RobberA != null)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(1f, 0f, 0f));
									}
									if (RobberB != null)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(2f, 0f, 0f));
									}
									if (RobberC != null)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(3f, 0f, 0f));
									}
								}
							}
						}
						if (BypassMetalDetector && EnteranceUsed != 1 && EnteranceUsed != 2)
						{
							Notification.Show("~y~ Lester ~w~ To get to the Vault, you can either, shoot your way down, or disable the Metal detectors from the utility room on the last floor, to pass undetected");
							BypassMetalDetector = false;
						}
						if (VaultClosed && MetalDetectorOn)
						{
							if (!ShutdownMetalDetector3 && MDBox3.Health < 1000)
							{
								Notification.Show("~y~ Lester ~w~ A Metal Detector has been disabled!");
								ShutdownMetalDetector3 = true;
							}
							if (MDBox3.Health >= 1000)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2540.4f, -265.3f, -58f), "<", 2f))
								{
									MetalDetectorOn = false;
									BypassMetalDetector = false;
									Notification.Show("~y~ Lester ~w~ A Metal Detector has been triggered, Guards are now Hostile");
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2541.8f, -265.1f, -58f), "<", 2f))
								{
									MetalDetectorOn = false;
									BypassMetalDetector = false;
									Notification.Show("~y~ Lester ~w~ A Metal Detector has been triggered, Guards are now Hostile");
								}
							}
							if (!ShutdownMetalDetector1 && MDBox1.Health < 1000)
							{
								Notification.Show("~y~ Lester ~w~ A Metal Detector has been disabled!");
								ShutdownMetalDetector1 = true;
							}
							if (MDBox1.Health >= 1000)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2545.6f, -277.4f, -58f), "<", 2f))
								{
									MetalDetectorOn = false;
									BypassMetalDetector = false;
									Notification.Show("~y~ Lester ~w~ A Metal Detector has been triggered, Guards are now Hostile");
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2545.5f, -278.7f, -58f), "<", 2f))
								{
									MetalDetectorOn = false;
									BypassMetalDetector = false;
									Notification.Show("~y~ Lester ~w~ A Metal Detector has been triggered, Guards are now Hostile");
								}
							}
							if (!ShutdownMetalDetector2 && MDBox2.Health < 1000)
							{
								Notification.Show("~y~ Lester ~w~ A Metal Detector has been disabled!");
								ShutdownMetalDetector2 = true;
							}
							if (MDBox2.Health >= 1000)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.6f, -269.9f, -58f), "<", 2f))
								{
									MetalDetectorOn = false;
									BypassMetalDetector = false;
									Notification.Show("~y~ Lester ~w~ A Metal Detector has been triggered, Guards are now Hostile");
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.6f, -268.2f, -58f), "<", 2f))
								{
									MetalDetectorOn = false;
									BypassMetalDetector = false;
									Notification.Show("~y~ Lester ~w~ A Metal Detector has been triggered, Guards are now Hostile");
								}
							}
						}
						if (EnteranceUsed == 2 && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2485.4f, -288.7f, -70f), "<", 10f))
						{
							ManageDoor(new Vector3(2485.079f, -288.2985f, -70.54587f), "v_ilev_rc_door2", false);
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.9f, -254f, -60f), "<", 5f))
						{
							if (VaultClosed)
							{
								ManageDoor(new Vector3(2524.829f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a", true);
								ManageDoor(new Vector3(2526.828f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a", true);
							}
							if (!VaultClosed && ExitUsed == 0)
							{
								ManageDoor(new Vector3(2524.829f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a", false);
								ManageDoor(new Vector3(2526.828f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a", false);
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2506.3f, -269.7f, -60f), "<", 20f))
						{
							if (!MetalDetectorOn && VaultDoor != null)
							{
								VaultDoor.Delete();
							}
							if (MetalDetectorOn && !UnlockedStaffDoor)
							{
								ManageDoor(new Vector3(2506.721f, -270.5107f, -58.57315f), "ch_prop_ch_service_door_02b", true);
								ManageDoor(new Vector3(2506.721f, -268.5107f, -58.57315f), "ch_prop_ch_service_door_02b", true);
							}
							if (!MetalDetectorOn || UnlockedStaffDoor)
							{
								ManageDoor(new Vector3(2506.721f, -270.5107f, -58.57315f), "ch_prop_ch_service_door_02b", false);
								ManageDoor(new Vector3(2506.721f, -268.5107f, -58.57315f), "ch_prop_ch_service_door_02b", false);
							}
						}
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2500.4f, -237.3f, -55f), "<", 5f))
						{
							if (VaultClosed)
							{
								ManageDoor(new Vector3(2497.657f, -235.5822f, -54.9737f), "ch_prop_casino_door_01a", true);
								ManageDoor(new Vector3(2499.657f, -235.5822f, -54.9737f), "ch_prop_casino_door_01a", true);
								ManageDoor(new Vector3(2500.96f, -236.6798f, -54.9738f), "ch_prop_casino_door_01d", true);
							}
							if (!VaultClosed)
							{
								ManageDoor(new Vector3(2497.657f, -235.5822f, -54.9737f), "ch_prop_casino_door_01a", false);
								ManageDoor(new Vector3(2499.657f, -235.5822f, -54.9737f), "ch_prop_casino_door_01a", false);
								ManageDoor(new Vector3(2500.96f, -236.6798f, -54.9738f), "ch_prop_casino_door_01d", false);
							}
						}
						ManageDoor(new Vector3(2515.308f, -281.5983f, -64.57317f), "ch_prop_ch_gendoor_01", false);
						ManageDoor(new Vector3(2509.743f, -260.3417f, -38.973f), "ch_prop_casino_door_01g", false);
						ManageDoor(new Vector3(2509.743f, -260.3417f, -38.973f), "ch_prop_casino_door_01g", true);
						OpenCasinoDoors();
					}
					else
					{
						if (HeadingtoyourAppartment)
						{
							if (RobberA != null)
							{
								GetDistance_VDIST2(RobberA.Position, new Vector3(2494.6f, -238.5f, -70f), "<", 5f);
							}
							if (RobberB != null && GetDistance_VDIST2(RobberB.Position, new Vector3(2494.6f, -238.5f, -70f), "<", 5f))
							{
								Ped robberB4 = RobberB;
								PedGroup pedGroup7 = Game.Player.Character.PedGroup;
								Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberB4, pedGroup7);
								Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberB4, 100);
								Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, robberB4, true);
							}
							if (RobberC != null && GetDistance_VDIST2(RobberC.Position, new Vector3(2494.6f, -238.5f, -70f), "<", 5f))
							{
								Ped robberC4 = RobberC;
								PedGroup pedGroup8 = Game.Player.Character.PedGroup;
								Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberC4, pedGroup8);
								Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberC4, 100);
								Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, robberC4, true);
							}
							if (Game.Player.WantedLevel == 0)
							{
								foreach (Prop smokeEmitter4 in SmokeEmitters)
								{
									if (smokeEmitter4 != null)
									{
										smokeEmitter4.Delete();
									}
								}
								GetDistance_VDIST2(Game.Player.Character.Position, CheckAppartmentPos(), "<", 100f);
								if (GetDistance_VDIST2(Game.Player.Character.Position, CheckAppartmentPos(), "<", 4f))
								{
									PlayingCutscene = false;
									RequestEndCutscene = true;
									Random random6 = new Random();
									int num34 = random6.Next(0, 7);
									string text2 = "mph_fin_cel_bar";
									HTime = HeistTime;
									if (num34 == 0)
									{
										text2 = "mph_fin_cel_bar";
									}
									if (num34 == 1)
									{
										text2 = "mph_fin_cel_roo";
									}
									if (num34 == 2)
									{
										text2 = "mph_fin_cel_str";
									}
									if (num34 == 3)
									{
										text2 = "mph_tut_ext";
									}
									if (num34 == 4)
									{
										text2 = "hs3_ext";
									}
									if (num34 == 5)
									{
										text2 = "hs3_ext";
									}
									if (num34 == 6)
									{
										text2 = "hs3_ext";
									}
									if (num34 == 7)
									{
										text2 = "hs3_ext";
									}
									int num35 = Function.Call<int>(Hash.REQUEST_CUTSCENE, text2);
									if (Function.Call<bool>(Hash.HAS_CUTSCENE_LOADED) && !PlayingCutscene && !Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
									{
										num35 = Function.Call<int>(Hash.REQUEST_CUTSCENE, text2);
										SetEntityinCustscenes();
										Function.Call(Hash.START_CUTSCENE, num35);
										Script.Wait(100);
										PlayingCutscene = true;
										GetPedOutfitCutscene("MP_1", Game.Player.Character);
										if (RobberA != null)
										{
											GetPedOutfitCutscene("MP_2", RobberA);
										}
										if (RobberB != null)
										{
											GetPedOutfitCutscene("MP_3", RobberB);
										}
										if (RobberC != null)
										{
											GetPedOutfitCutscene("MP_4", RobberC);
										}
									}
								}
								if (RequestEndCutscene && PlayingCutscene)
								{
									if (Function.Call<int>(Hash.GET_CUTSCENE_TIME) >= Function.Call<int>(Hash.GET_CUTSCENE_TOTAL_DURATION) - 100)
									{
										Function.Call(Hash.STOP_CUTSCENE, true);
									}
									if (!Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING) && PlayingCutscene)
									{
										Function.Call(Hash.REMOVE_CUTSCENE);
										PlayingCutscene = false;
										Game.Player.Character.Position = new Vector3(CheckAppartmentPos().X, CheckAppartmentPos().Y, CheckAppartmentPos().Z + 2f);
										int crewAlive = 1;
										if (RobberA != null && RobberA.IsAlive)
										{
											crewAlive++;
										}
										if (RobberB != null && RobberB.IsAlive)
										{
											crewAlive++;
										}
										if (RobberC != null && RobberC.IsAlive)
										{
											crewAlive++;
										}
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
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										if (DeliveryPoint != null)
										{
											DeliveryPoint.Delete();
										}
										ManageRobberyAnims.DeleteLoot();
										if (Bank != null)
										{
											Bank.Delete();
										}
										if (GetawayCar != null)
										{
											GetawayCar.Delete();
										}
										foreach (Prop prop42 in Props)
										{
											if (prop42 != null)
											{
												prop42.Delete();
											}
										}
										if (RobberC != null)
										{
											RobberC.Delete();
										}
										if (RobberB != null)
										{
											RobberB.Delete();
										}
										foreach (Ped aircarftCarrierPed2 in AircarftCarrierPeds)
										{
											if (aircarftCarrierPed2 != null)
											{
												aircarftCarrierPed2.Delete();
											}
										}
										foreach (Vehicle aircraftCarrierVehicle2 in AircraftCarrierVehicles)
										{
											if (aircraftCarrierVehicle2 != null)
											{
												aircraftCarrierVehicle2.Delete();
											}
										}
										if (WheelProp != null)
										{
											WheelProp.Delete();
										}
										if (WheelProp2 != null)
										{
											WheelProp2.Delete();
										}
										if (BasePlate != null)
										{
											BasePlate.Delete();
										}
										foreach (Ped item4 in MainPed)
										{
											if (item4 != null)
											{
												item4.Delete();
											}
										}
										foreach (Prop table2 in Tables)
										{
											if (table2 != null)
											{
												table2.Delete();
											}
										}
										foreach (Prop item5 in Tables2)
										{
											if (item5 != null)
											{
												item5.Delete();
											}
										}
										if (Prizecar != null)
										{
											Prizecar.Delete();
										}
										foreach (Ped item6 in SMPed)
										{
											if (item6 != null)
											{
												item6.Delete();
											}
										}
										FreezeGetawayCar = false;
										FreezeGetawayCar2 = false;
										TriggerNotifyNear = false;
										PlacedBomb = false;
										EscapingCasino = false;
										VaultDone = false;
										VaultClosed = true;
										MetalDetectorOn = true;
										IsInterior = false;
										BypassMetalDetector = false;
										NotifyOutVault2 = false;
										AlarmsGoingOff = false;
										GotCash = false;
										TriggeredWantedLevelEscape = false;
										IsPlanting = false;
										NotifyOutVault = false;
										if (HackConsole != null)
										{
											HackConsole.Delete();
										}
										hackedDataPad = false;
										AlarmsGoingOff = false;
										HeadingtoyourAppartment = false;
										SetSewerBomb = false;
										DetonatedSewerBomb = false;
										EnteredNormalCasino = false;
										EnteredOffice = false;
										TriggeredAttack = false;
										AlarmTimer = 0;
										Game.Player.Character.Opacity = 0;
										Vector3 position5 = Game.Player.Character.Position;
										Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
										Script.Wait(1);
										Game.Player.Character.Position = position5;
										Game.Player.Character.Opacity = 255;
										MissionOn = false;
										float num36 = ManageRobberyAnims.CurrentPay;
										float num37 = (num36 -= TotalCost);
										if (PoliceAlearted)
										{
											Notification.Show("Additional 25% Cut was taken from The Contractor, due to Player Alerting the Police.");
											num37 = num37 / 100f * 75f;
										}
										float totalCost = TotalCost;
										Script.Wait(10);
										Main.TotalCrewEarned += totalCost;
										Main.TotalPlayerEarned += num37;
										XP.CompleteHeist_TheDiamondCasino++;
										Main.SaveCuts();
										Class1.SetNoLongerInHeist();
										MissionScreen.SetPass("", (int)num37, (int)totalCost, "The Player has died", crewAlive, HTime, 13);
										int num38 = Game.GameTime + 30000;
										while (HKHModHelperNew.Class1.HeistPassedScreen != -1 && Game.GameTime <= num38)
										{
											Script.Wait(0);
										}
										Script.Wait(2000);
										PoliceAlearted = false;
										LesterToldPlayerAboutContractCut = false;
										foreach (ObjectPlacementMap swatEntty4 in SwatEntties)
										{
											if (swatEntty4.Entity != null)
											{
												swatEntty4.Entity.Delete();
											}
										}
										XP.AddXP_Percent(XP.CompleteHeist_XPPercent_TheDiamondCasino);
										InHeist = false;
										ManagePlayerFunds.AddFunds((int)num37);
										HeadingtoyourAppartment = false;
										HeistStage = 1;
										Config.SetValue("THEDIAMONDHIEST", "HeistStage", HeistStage);
										Config.Save();
										CurrentPay = 0;
										ManageRobberyAnims.CurrentPay = 0;
										Notification.Show("~b~ Heist Complete ~w~");
										Notification.Show("~b~ Player Cut ~w~ : $" + num37.ToString("N"));
										CurrentPay = 0;
										Notification.Show("~b~ Crew Cut ~w~ : $" + TotalCost.ToString("N"));
										TotalCrewEarned = Config.GetValue("Misc", "TotalCrewEarned", TotalCrewEarned);
										TotalPlayerEarned = Config.GetValue("Misc", "TotalPlayerEarned", TotalPlayerEarned);
										TotalCrewEarned += TotalCost;
										TotalPlayerEarned += (int)num37;
										Config.SetValue("Misc", "TotalCrewEarned", TotalCrewEarned);
										Config.SetValue("Misc", "TotalPlayerEarned", TotalPlayerEarned);
										Config.Save();
										HeistTime = 0f;
										RequestEndCutscene = false;
										ManageRobberyAnims.Crate_loc.Clear();
										ManageRobberyAnims.Crate_Rot.Clear();
										ManageRobberyAnims.DeleteLoot();
									}
								}
							}
						}
						if (ExitUsed == 0)
						{
							if (VaultClosed)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(925.7f, 45.8f, 80f), "<", 100f))
								{
									World.DrawMarker(MarkerType.Cylinder, new Vector3(925.7f, 45.8f, 80f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(925.7f, 45.8f, 80f), "<", 2f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
									if (Game.IsControlJustPressed(GTA.Control.Context))
									{
										InNormalCasino = true;
										Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
										VaultClosed = true;
										BypassMetalDetector = true;
										IsInterior = true;
										Game.Player.Character.Position = new Vector3(1089.6f, 206.07f, -49f);
										if (RobberA != null)
										{
											RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
										}
										if (RobberB != null)
										{
											RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2f, 0f));
										}
										if (RobberC != null)
										{
											RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f));
										}
										Notification.Show("~y~ Lester ~w~ Be careful, the Guards are on High Alert, pulling a weapon out will call Swat teams! and fail the Heist");
									}
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1121f, 256.4f, -47f), "<", 100f))
								{
									World.DrawMarker(MarkerType.Cylinder, new Vector3(1121f, 256.4f, -47f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1121f, 256.4f, -47f), "<", 2f))
								{
									DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Offices");
									if (Game.IsControlJustPressed(GTA.Control.Context))
									{
										InNormalCasino = false;
										BypassMetalDetector = true;
										IsInterior = true;
										Game.Player.Character.Position = new Vector3(2501.4f, -237.3f, -55.4f);
										if (RobberA != null)
										{
											RobberA.Position = new Vector3(2505.09f, -234.4f, -55f);
										}
										if (RobberB != null)
										{
											RobberB.Position = new Vector3(2507.09f, -234.4f, -55f);
										}
										if (RobberC != null)
										{
											RobberC.Position = new Vector3(2509.09f, -234.4f, -55f);
										}
										Notification.Show("~y~ Lester ~w~ : Guards here are still on high Alert, pulling a weapon close to a guard will alert all of them!");
									}
								}
							}
							if (!VaultClosed)
							{
								if (TriggerFullResponce)
								{
									ManageDoor(new Vector3(2485.079f, -288.2985f, -70.54587f), "v_ilev_rc_door2", false);
									ManageDoor(new Vector3(2500.935f, -278.3725f, -58.57317f), "ch_prop_casino_door_01c", false);
									ManageDoor(new Vector3(2526.828f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a, ch_prop_casino_door_01a, 0, ch_casinodoors", false);
									ManageDoor(new Vector3(2524.829f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a, ch_prop_casino_door_01a, 0, ch_casinodoors", false);
								}
								if (!TriggerFullResponce)
								{
									ManageDoor(new Vector3(2500.935f, -278.3725f, -58.57317f), "ch_prop_casino_door_01c", true);
									ManageDoor(new Vector3(2526.828f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a, ch_prop_casino_door_01a, 0, ch_casinodoors", true);
									ManageDoor(new Vector3(2524.829f, -253.5669f, -60.17379f), "ch_prop_casino_door_01a, ch_prop_casino_door_01a, 0, ch_casinodoors", true);
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2502.02f, -278.86f, -60f), "<", 100f))
									{
										World.DrawMarker(MarkerType.Cylinder, new Vector3(2502.02f, -278.86f, -60f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2502.02f, -278.86f, -60f), "<", 2f))
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											InNormalCasino = true;
											Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
											VaultClosed = true;
											BypassMetalDetector = true;
											IsInterior = true;
											Game.Player.Character.Position = new Vector3(1120.3f, 214.58f, -50f);
											if (RobberA != null)
											{
												RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
											}
											if (RobberB != null)
											{
												RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2f, 0f));
											}
											if (RobberC != null)
											{
												RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f));
											}
											Notification.Show("~y~ Lester ~w~ Make your way out of the casino to your getaway car, Pulling a weapon will alert the cops while in the Casino");
										}
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.812f, -254.4151f, -60.8f), "<", 100f))
									{
										World.DrawMarker(MarkerType.Cylinder, new Vector3(2525.812f, -254.4151f, -60.8f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2525.812f, -254.4151f, -60.8f), "<", 2f))
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											VaultClosed = true;
											InNormalCasino = true;
											BypassMetalDetector = true;
											IsInterior = true;
											Game.Player.Character.Position = new Vector3(1146.621f, 241.25f, -51.5f);
											if (RobberA != null)
											{
												RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
											}
											if (RobberB != null)
											{
												RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2f, 0f));
											}
											if (RobberC != null)
											{
												RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f));
											}
											Notification.Show("~y~ Lester ~w~ Make your way out of the casino to your getaway car, Pulling a weapon will alert the cops while in the Casino");
										}
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1089.7f, 206.04f, -49.5f), "<", 100f))
									{
										World.DrawMarker(MarkerType.Cylinder, new Vector3(1121f, 256.4f, -47f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(1121f, 256.4f, -47f), "<", 2f))
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Exit Casino (3)");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											Script.Wait(1);
											IsInterior = false;
											Game.Player.Character.Position = new Vector3(924.52f, 45.33f, 81f);
											if (RobberA != null)
											{
												RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
											}
											if (RobberB != null)
											{
												RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2f, 0f));
											}
											if (RobberC != null)
											{
												RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f));
											}
											Notification.Show("~y~ Lester ~w~ : Ok, Congrats, return to the your Arcade");
										}
									}
								}
							}
						}
						if ((EnteranceUsed == 2 || (EnteranceUsed == 2 && ExitUsed == 1)) && VaultClosed)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(992.7f, -141.7f, 34f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(992.7f, -141.7f, 34f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(992.7f, -141.7f, 34f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Tunnel");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									Intunnel = true;
									BypassMetalDetector = false;
									IsInterior = true;
									Game.Player.Character.Position = new Vector3(2497.707f, -312.7705f, -71.46999f);
									Script.Wait(1);
									Game.Player.Character.Rotation = new Vector3(0f, 0f, 37f);
									Game.Player.Character.Position = new Vector3(2506.9f, -323.3f, -70.5f);
									if (RobberA != null)
									{
										RobberA.IsCollisionEnabled = false;
										RobberA.Position = Game.Player.Character.Position;
										RobberA.IsVisible = false;
									}
									if (RobberB != null)
									{
										RobberB.IsCollisionEnabled = false;
										RobberB.Position = Game.Player.Character.Position;
										RobberB.IsVisible = false;
									}
									if (RobberC != null)
									{
										RobberC.IsCollisionEnabled = false;
										RobberC.Position = Game.Player.Character.Position;
										RobberC.IsVisible = false;
									}
									Notification.Show("~y~ Lester ~w~: Your teamates will join you after you blow the wall into the Casino Vault area");
								}
							}
						}
						if ((EnteranceUsed == 3 || EnteranceUsed == 4) && VaultClosed)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(953.7f, 3.9f, 110f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(953.7f, 3.9f, 110f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(953.7f, 3.9f, 110f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									VaultClosed = true;
									BypassMetalDetector = true;
									IsInterior = true;
									Game.Player.Character.Position = new Vector3(2484.6f, -257.4f, -40f);
									if (RobberA != null && RobberA.IsAlive)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									}
									if (RobberB != null && RobberB.IsAlive)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2f, 0f));
									}
									if (RobberC != null && RobberC.IsAlive)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f));
									}
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(959.5f, 32.2f, 119f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(959.5f, 32.2f, 119f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(959.5f, 32.2f, 119f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									BypassMetalDetector = false;
									IsInterior = true;
									VaultClosed = true;
									Game.Player.Character.Position = new Vector3(2487.9f, -292.7f, -40f);
								}
							}
						}
						if ((ExitUsed == 2 || ExitUsed == 3) && !VaultClosed)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2484.6f, -257.4f, -40f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(2484.6f, -257.4f, -40f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2484.6f, -257.4f, -40f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									VaultClosed = true;
									BypassMetalDetector = true;
									IsInterior = true;
									Game.Player.Character.Position = new Vector3(953.7f, 3.9f, 110f);
									if (RobberA != null && RobberA.IsAlive)
									{
										RobberA.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
									}
									if (RobberB != null && RobberB.IsAlive)
									{
										RobberB.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 2f, 0f));
									}
									if (RobberC != null && RobberC.IsAlive)
									{
										RobberC.Position = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 3f, 0f));
									}
									try
									{
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
										VehicleChoosen = SaveCar.VehicleName;
										SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
										if (SecondaryGetawayVeh.AttachedBlip != null)
										{
											SecondaryGetawayVeh.AttachedBlip.Delete();
										}
										SecondaryGetawayVeh.AddBlip();
										SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
										SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
										SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.Repair();
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.IsEngineRunning = true;
										SecondaryGetawayVeh.IsDriveable = true;
										SecondaryGetawayVeh.IsInvincible = true;
										SecondaryGetawayVeh.IsPositionFrozen = true;
										HeadingtoyourAppartment = true;
									}
									catch
									{
										Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
									}
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2487.9f, -292.7f, -40f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(2487.9f, -292.7f, -40f), Vector3.Zero, Vector3.Zero, new Vector3(0.75f, 0.75f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(2487.9f, -292.7f, -40f), "<", 2f))
							{
								DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino");
								if (Game.IsControlJustPressed(GTA.Control.Context))
								{
									VaultClosed = true;
									BypassMetalDetector = false;
									IsInterior = true;
									Game.Player.Character.Position = new Vector3(959.5f, 32.2f, 119f);
									try
									{
										if (SecondaryGetawayVeh != null)
										{
											SecondaryGetawayVeh.Delete();
										}
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars//DiamondHeist_Air.ini");
										VehicleChoosen = SaveCar.VehicleName;
										SecondaryGetawayVeh = GlobalHelper.CreateVehicle(RequestModel(VehicleChoosen), new Vector3(965.8f, 42.04f, 122f), 57f);
										if (SecondaryGetawayVeh.AttachedBlip != null)
										{
											SecondaryGetawayVeh.AttachedBlip.Delete();
										}
										SecondaryGetawayVeh.AddBlip();
										SecondaryGetawayVeh.AttachedBlip.Sprite = BlipSprite.Helicopter;
										SecondaryGetawayVeh.AttachedBlip.Color = BlipColor.Yellow;
										SecondaryGetawayVeh.AttachedBlip.Name = "Getaway Heli";
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.Repair();
										SecondaryGetawayVeh.IsPositionFrozen = false;
										SecondaryGetawayVeh.IsEngineRunning = true;
										SecondaryGetawayVeh.IsDriveable = true;
										SecondaryGetawayVeh.IsInvincible = true;
										SecondaryGetawayVeh.IsPositionFrozen = true;
										HeadingtoyourAppartment = true;
									}
									catch
									{
										Notification.Show("~r~ Error ~w~ : Could not spawn in vehicle, please check ini for incorrect name");
									}
									VaultClosed = true;
								}
							}
						}
						if (EnteranceUsed == 5 && ExitUsed == 4)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(932.7f, -4.3f, 78f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(932.7f, -4.3f, 78f), Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(932.7f, -4.3f, 78f), "<", 2f))
							{
								if (Game.Player.Character.CurrentVehicle != null)
								{
									if (Game.Player.Character.CurrentVehicle.Model == VehicleHash.Burrito2)
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino as Bugstars");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											BypassMetalDetector = false;
											IsInterior = true;
											Game.Player.Character.CurrentVehicle.Position = new Vector3(2553.963f, -281.3805f, -65.72305f);
											Script.Wait(1);
											Game.Player.Character.CurrentVehicle.Position = new Vector3(2564.7f, -279.4f, -65.1f);
											Game.Player.Character.CurrentVehicle.Rotation = new Vector3(0f, 0f, 90f);
											VaultClosed = true;
										}
									}
									else
									{
										DisplayHelpTextThisFrame("You need to be in a Bugstars Van to Enter the Loading Bay");
									}
								}
								else
								{
									DisplayHelpTextThisFrame("You need to be in a Bugstars Van to Enter the Loading Bay");
								}
							}
						}
						if (EnteranceUsed == 1)
						{
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(932.7f, -4.3f, 78f), "<", 100f))
							{
								World.DrawMarker(MarkerType.Cylinder, new Vector3(932.7f, -4.3f, 78f), Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 0.75f), Color.Goldenrod);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(932.7f, -4.3f, 78f), "<", 2f))
							{
								if (Game.Player.Character.CurrentVehicle != null)
								{
									if (Game.Player.Character.CurrentVehicle.Model == VehicleHash.Stockade)
									{
										DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Enter Casino as Groupe6");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											BypassMetalDetector = false;
											IsInterior = true;
											Game.Player.Character.CurrentVehicle.Position = new Vector3(2553.963f, -281.3805f, -65.72305f);
											Script.Wait(1);
											Game.Player.Character.CurrentVehicle.Position = new Vector3(2564.7f, -279.4f, -65.1f);
											Game.Player.Character.CurrentVehicle.Rotation = new Vector3(0f, 0f, 90f);
											VaultClosed = true;
										}
									}
									else
									{
										DisplayHelpTextThisFrame("You need to be in a Stockade to Enter the Loading Bay");
									}
								}
								else
								{
									DisplayHelpTextThisFrame("You need to be in a Stockade to Enter the Loading Bay");
								}
							}
						}
					}
					int num39 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, 1100.0, 220.0, -50.0);
					Vector3 position6 = Game.Player.Character.Position;
					if (num39 == Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position6.X, position6.Y, position6.Z) && InNormalCasino && Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && Game.Player.Character.Weapons.Current.Hash != (WeaponHash)Function.Call<int>(Hash.GET_HASH_KEY, "WEAPON_BRIEFCASE"))
					{
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
						if (SecondaryGetawayVeh != null)
						{
							SecondaryGetawayVeh.Delete();
						}
						if (DeliveryPoint != null)
						{
							DeliveryPoint.Delete();
						}
						if (Bank != null)
						{
							Bank.Delete();
						}
						if (GetawayCar != null)
						{
							GetawayCar.Delete();
						}
						foreach (Prop prop43 in Props)
						{
							if (prop43 != null)
							{
								prop43.Delete();
							}
						}
						if (RobberC != null)
						{
							RobberC.Delete();
						}
						if (RobberB != null)
						{
							RobberB.Delete();
						}
						foreach (Ped aircarftCarrierPed3 in AircarftCarrierPeds)
						{
							if (aircarftCarrierPed3 != null)
							{
								aircarftCarrierPed3.Delete();
							}
						}
						foreach (Vehicle aircraftCarrierVehicle3 in AircraftCarrierVehicles)
						{
							if (aircraftCarrierVehicle3 != null)
							{
								aircraftCarrierVehicle3.Delete();
							}
						}
						if (WheelProp != null)
						{
							WheelProp.Delete();
						}
						if (WheelProp2 != null)
						{
							WheelProp2.Delete();
						}
						if (BasePlate != null)
						{
							BasePlate.Delete();
						}
						foreach (Ped item7 in MainPed)
						{
							if (item7 != null)
							{
								item7.Delete();
							}
						}
						foreach (Prop table3 in Tables)
						{
							if (table3 != null)
							{
								table3.Delete();
							}
						}
						foreach (Prop item8 in Tables2)
						{
							if (item8 != null)
							{
								item8.Delete();
							}
						}
						if (Prizecar != null)
						{
							Prizecar.Delete();
						}
						foreach (Ped item9 in SMPed)
						{
							if (item9 != null)
							{
								item9.Delete();
							}
						}
						FreezeGetawayCar = false;
						FreezeGetawayCar2 = false;
						TriggerNotifyNear = false;
						PlacedBomb = false;
						EscapingCasino = false;
						VaultDone = false;
						VaultClosed = true;
						MetalDetectorOn = true;
						IsInterior = false;
						BypassMetalDetector = false;
						NotifyOutVault2 = false;
						AlarmsGoingOff = false;
						GotCash = false;
						TriggeredWantedLevelEscape = false;
						IsPlanting = false;
						NotifyOutVault = false;
						OverideAlarm = false;
						CurrentPay = 0;
						ManageRobberyAnims.CurrentPay = 0;
						AlarmsGoingOff = false;
						HeadingtoyourAppartment = false;
						SetSewerBomb = false;
						DetonatedSewerBomb = false;
						EnteredNormalCasino = false;
						EnteredOffice = false;
						InNormalCasino = false;
						TriggeredAttack = false;
						AlarmTimer = 0;
						Game.Player.Character.Opacity = 0;
						Vector3 position7 = Game.Player.Character.Position;
						Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
						Script.Wait(1);
						Game.Player.Character.Position = position7;
						Game.Player.Character.Opacity = 255;
						MissionOn = false;
						MissionScreen.SetFail("", 0, 0, "You Draw the Attention of the Police", 0, HeistTime, HeistMissson);
						PoliceAlearted = false;
						LesterToldPlayerAboutContractCut = false;
						foreach (ObjectPlacementMap swatEntty5 in SwatEntties)
						{
							if (swatEntty5.Entity != null)
							{
								swatEntty5.Entity.Delete();
							}
						}
						foreach (ObjectPlacementMap bugStarVent5 in BugStarVents)
						{
							if (bugStarVent5.Entity != null)
							{
								bugStarVent5.Entity.Delete();
							}
						}
						foreach (ObjectPlacementMap groupe6Cam5 in Groupe6Cams)
						{
							if (groupe6Cam5.Entity != null)
							{
								groupe6Cam5.Entity.Delete();
							}
						}
						SmartGuards.ClearGenericGuards();
						ManageRobberyAnims.Crate_loc.Clear();
						ManageRobberyAnims.Crate_Rot.Clear();
						MissionOn = false;
					}
				}
				if (Game.Player.IsAlive)
				{
					break;
				}
				ManageRobberyAnims.DeleteLoot();
				foreach (Prop item10 in PaintingHolder)
				{
					if (item10 != null)
					{
						item10.Delete();
					}
				}
				foreach (Prop prop44 in Props)
				{
					if (prop44 != null)
					{
						prop44.Delete();
					}
				}
				foreach (Prop smokeEmitter5 in SmokeEmitters)
				{
					if (smokeEmitter5 != null)
					{
						smokeEmitter5.Delete();
					}
				}
				if (HackConsole != null)
				{
					HackConsole.Delete();
				}
				hackedDataPad = false;
				if (VaultDoor != null)
				{
					VaultDoor.Delete();
				}
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
				if (SecondaryGetawayVeh != null)
				{
					SecondaryGetawayVeh.Delete();
				}
				if (DeliveryPoint != null)
				{
					DeliveryPoint.Delete();
				}
				if (Bank != null)
				{
					Bank.Delete();
				}
				if (GetawayCar != null)
				{
					GetawayCar.Delete();
				}
				foreach (Prop prop45 in Props)
				{
					if (prop45 != null)
					{
						prop45.Delete();
					}
				}
				if (RobberC != null)
				{
					RobberC.Delete();
				}
				if (RobberB != null)
				{
					RobberB.Delete();
				}
				foreach (Ped aircarftCarrierPed4 in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed4 != null)
					{
						aircarftCarrierPed4.Delete();
					}
				}
				foreach (Vehicle aircraftCarrierVehicle4 in AircraftCarrierVehicles)
				{
					if (aircraftCarrierVehicle4 != null)
					{
						aircraftCarrierVehicle4.Delete();
					}
				}
				if (WheelProp != null)
				{
					WheelProp.Delete();
				}
				if (WheelProp2 != null)
				{
					WheelProp2.Delete();
				}
				if (BasePlate != null)
				{
					BasePlate.Delete();
				}
				foreach (Ped item11 in MainPed)
				{
					if (item11 != null)
					{
						item11.Delete();
					}
				}
				foreach (Prop table4 in Tables)
				{
					if (table4 != null)
					{
						table4.Delete();
					}
				}
				foreach (Prop item12 in Tables2)
				{
					if (item12 != null)
					{
						item12.Delete();
					}
				}
				if (Prizecar != null)
				{
					Prizecar.Delete();
				}
				foreach (Ped item13 in SMPed)
				{
					if (item13 != null)
					{
						item13.Delete();
					}
				}
				FreezeGetawayCar = false;
				FreezeGetawayCar2 = false;
				TriggerNotifyNear = false;
				PlacedBomb = false;
				EscapingCasino = false;
				VaultDone = false;
				VaultClosed = true;
				MetalDetectorOn = true;
				IsInterior = false;
				BypassMetalDetector = false;
				NotifyOutVault2 = false;
				AlarmsGoingOff = false;
				GotCash = false;
				TriggeredWantedLevelEscape = false;
				IsPlanting = false;
				NotifyOutVault = false;
				CurrentPay = 0;
				ManageRobberyAnims.CurrentPay = 0;
				OverideAlarm = false;
				AlarmsGoingOff = false;
				HeadingtoyourAppartment = false;
				SetSewerBomb = false;
				DetonatedSewerBomb = false;
				EnteredNormalCasino = false;
				EnteredOffice = false;
				TriggeredAttack = false;
				AlarmTimer = 0;
				InNormalCasino = false;
				Game.Player.Character.Opacity = 0;
				Vector3 position8 = Game.Player.Character.Position;
				Game.Player.Character.Position = new Vector3(322f, 152f, 612f);
				Script.Wait(1);
				Game.Player.Character.Position = position8;
				Game.Player.Character.Opacity = 255;
				MissionOn = false;
				MissionScreen.SetFail("", 0, 0, "you Died!", 0, HeistTime, HeistMissson);
				PoliceAlearted = false;
				LesterToldPlayerAboutContractCut = false;
				foreach (ObjectPlacementMap swatEntty6 in SwatEntties)
				{
					if (swatEntty6.Entity != null)
					{
						swatEntty6.Entity.Delete();
					}
				}
				foreach (ObjectPlacementMap bugStarVent6 in BugStarVents)
				{
					if (bugStarVent6.Entity != null)
					{
						bugStarVent6.Entity.Delete();
					}
				}
				foreach (ObjectPlacementMap groupe6Cam6 in Groupe6Cams)
				{
					if (groupe6Cam6.Entity != null)
					{
						groupe6Cam6.Entity.Delete();
					}
				}
				SmartGuards.ClearGenericGuards();
				ManageRobberyAnims.Crate_loc.Clear();
				ManageRobberyAnims.Crate_Rot.Clear();
				MissionOn = false;
				break;
			}
			case 3:
				if (Game.Player.IsAlive)
				{
					if (EnteranceUsed != 5 && ExitUsed != 4)
					{
						if (EnteranceUsed != 3 && EnteranceUsed != 4 && ExitUsed != 2 && ExitUsed != 3)
						{
							GTA.UI.Screen.ShowSubtitle("Aquire a ~b~Land Based Vehicle~w~ for the ~y~Diamond Casino Heist~w~ and deliver it to ~d~Lesters Factory");
						}
						if (EnteranceUsed == 3 || EnteranceUsed == 4 || ExitUsed == 2 || ExitUsed == 3)
						{
							GTA.UI.Screen.ShowSubtitle("Aquire a ~b~Helicopter~w~ for the ~y~Diamond Casino Heist");
						}
					}
					if (EnteranceUsed == 5 && ExitUsed == 4)
					{
						GTA.UI.Screen.ShowSubtitle("Aquire a ~b~Bugstars Van~w~ for the ~y~Diamond Casino Heist");
					}
					if (!LookingForVehicle)
					{
						break;
					}
					if (EnteranceUsed != 5 && ExitUsed != 4)
					{
						if (EnteranceUsed != 3 && EnteranceUsed != 4 && ExitUsed != 2 && ExitUsed != 3 && GetDistance_VDIST2(Game.Player.Character.Position, DeliveryLoc, "<", 100f) && GetDistance_VDIST2(Game.Player.Character.Position, DeliveryLoc, "<", 3f))
						{
							if (Game.Player.Character.CurrentVehicle != null)
							{
								if (!Game.Player.Character.CurrentVehicle.Model.IsHelicopter)
								{
									DisplayHelpTextThisFrame(" ~INPUT_CONTEXT~ to use this vehicle in the Diamond Casino Heist");
									if (Game.IsControlJustPressed(GTA.Control.Context))
									{
										MissionOn = false;
										Class1.SetNoLongerInHeist();
										MissionScreen.SetPassSetup("", "", 13);
										DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok good, we can finaly move on the Heist  ~y~|| Mission Complete ||~w~");
										SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Land.ini");
										SaveCar.SaveWithoutDelete(Game.Player.Character.CurrentVehicle);
										Notification.Show("~y~ Lester ~w~ : Ok we will use this car in the Heist");
										HeistStage = 4;
										Config.SetValue("TheDiamondHiest", "HeistStage", HeistStage);
										Config.Save();
										LookingForVehicle = false;
										if (DeliveryPoint != null)
										{
											DeliveryPoint.Delete();
										}
									}
								}
								else
								{
									DisplayHelpTextThisFrame("~y~ Lester ~w~ : Getaway vehicle cannot be a Helicopter");
								}
							}
							else
							{
								DisplayHelpTextThisFrame("~y~ Lester ~w~ : You need a vehicle for the Diamond Casino Heist, deliver one here");
							}
						}
						if ((EnteranceUsed == 3 || EnteranceUsed == 4 || ExitUsed == 2 || ExitUsed == 3) && GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-987.7f, -2992.6f, 14f), "<", 100f))
						{
							World.DrawMarker(MarkerType.Cylinder, new Vector3(-987.7f, -2992.6f, 14f), Vector3.Zero, Vector3.Zero, new Vector3(9f, 9f, 1f), Color.Red);
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(-987.7f, -2992.6f, 14f), "<", 3f))
							{
								if (Game.Player.Character.CurrentVehicle != null)
								{
									if (Game.Player.Character.CurrentVehicle.Model.IsHelicopter)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to use this vehicle in the Diamond Casino Heist ");
										if (Game.IsControlJustPressed(GTA.Control.Context))
										{
											MissionOn = false;
											Class1.SetNoLongerInHeist();
											MissionScreen.SetPassSetup("", "", 13);
											DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok good, we can finaly move on the Heist  ~y~|| Mission Complete ||~w~");
											SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Air.ini");
											SaveCar.SaveWithoutDelete(Game.Player.Character.CurrentVehicle);
											Notification.Show("~y~ Lester ~w~ : Ok we will use this car in the Heist");
											HeistStage = 4;
											Config.SetValue("TheDiamondHiest", "HeistStage", HeistStage);
											Config.Save();
											LookingForVehicle = false;
											if (DeliveryPoint != null)
											{
												DeliveryPoint.Delete();
											}
										}
									}
									else
									{
										DisplayHelpTextThisFrame("~y~ Lester ~w~ : Getaway vehicle has to be a Helicopter");
									}
								}
								else
								{
									DisplayHelpTextThisFrame("~y~ Lester ~w~ : You need a vehicle for the Diamond Casino Heist, deliver one here");
								}
							}
						}
					}
					if (EnteranceUsed != 5 || ExitUsed != 4 || !GetDistance_VDIST2(Game.Player.Character.Position, DeliveryLoc, "<", 100f))
					{
						break;
					}
					World.DrawMarker(MarkerType.Cylinder, DeliveryLoc, Vector3.Zero, Vector3.Zero, new Vector3(6f, 6f, 2f), Color.Red);
					if (!GetDistance_VDIST2(Game.Player.Character.Position, DeliveryLoc, "<", 3f))
					{
						break;
					}
					if (Game.Player.Character.CurrentVehicle != null)
					{
						if (Game.Player.Character.CurrentVehicle.DisplayName.Equals("BURRITO2"))
						{
							DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to use this vehicle in the Diamond Casino Heist ");
							if (Game.IsControlJustPressed(GTA.Control.Context))
							{
								MissionOn = false;
								Class1.SetNoLongerInHeist();
								MissionScreen.SetPassSetup("", "", 13);
								DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok good, we can finaly move on the Heist  ~y~|| Mission Complete ||~w~");
								SaveCar.LoadIniFile("scripts//HKH_SinglePlayerHeists//Arcade//DiamondCasinoHeist_GetawayCars/DiamondHeist_Special.ini");
								SaveCar.Save();
								Notification.Show("~y~ Lester ~w~ : Ok we will use this car in the Heist");
								HeistStage = 4;
								Config.SetValue("TheDiamondHiest", "HeistStage", HeistStage);
								Config.Save();
								LookingForVehicle = false;
								if (DeliveryPoint != null)
								{
									DeliveryPoint.Delete();
								}
							}
						}
						else
						{
							DisplayHelpTextThisFrame("~y~ Lester ~w~ : Getaway vehicle has to be a Bugstars Van");
						}
					}
					else
					{
						DisplayHelpTextThisFrame("~y~ Lester ~w~ : You need a vehicle for the Diamond Casino Heist, deliver one here");
					}
					break;
				}
				MissionOn = false;
				MissionScreen.SetFailSetup("", "You have Been Killed", 13);
				PoliceAlearted = false;
				LesterToldPlayerAboutContractCut = false;
				foreach (ObjectPlacementMap swatEntty7 in SwatEntties)
				{
					if (swatEntty7.Entity != null)
					{
						swatEntty7.Entity.Delete();
					}
				}
				LookingForVehicle = false;
				if (DeliveryPoint != null)
				{
					DeliveryPoint.Delete();
				}
				break;
			case 2:
				if (Game.Player.IsAlive)
				{
					if (!GotC4_1 && !GotC4_2)
					{
						if (EnteranceUsed != 1)
						{
							GTA.UI.Screen.ShowSubtitle("Aquire the ~g~C4~w~ down at the ~y~Merryweather Docks");
						}
						if (EnteranceUsed == 1)
						{
							GTA.UI.Screen.ShowSubtitle("Aquire the ~g~C4~w~ down at the ~y~Merryweather Docks~w~\n And Aquire a ~b~ Marked Stockade");
						}
						if (EnteranceUsed == 3 || EnteranceUsed == 4)
						{
							GTA.UI.Screen.ShowSubtitle("Aquire the ~g~C4~w~ down at the ~y~Merryweather Docks~w~\n And Aquire a ~b~VIP Pass");
						}
					}
					if (!SecuredAsset_C4)
					{
						if (Crate_1 != null && !GotC4_1)
						{
							Vector3 offsetPosition7 = Crate_1.GetOffsetPosition(new Vector3(2f, 0f, 0f));
							World.DrawMarker(MarkerType.Cylinder, new Vector3(463.6f, -3257.6f, 5.06f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.Red);
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(463.6f, -3257.6f, 6f), "<", 3f))
							{
								GotC4_1 = true;
								if (Crate_1.AttachedBlip != null)
								{
									Crate_1.AttachedBlip.Alpha = 0;
								}
								Notification.Show("~y~ Lester ~w~ : Good! that is that is the C4 we need, grab the rest");
								Game.Player.WantedLevel = 4;
								Vector3 vector4 = new Vector3(493.129f, -3377.43f, 6f);
								for (int num40 = 0; num40 < 7; num40++)
								{
									Ped ped10 = GlobalHelper.CreatePed(PedHash.MerryWeatherCutscene, vector4.Around(10f));
									ped10.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
									Peds.Add(ped10);
									foreach (Ped ped12 in Peds)
									{
										if (ped12 != null)
										{
											RelationshipGroup relationshipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "SECURITY_GUARD");
											Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped10, relationshipGroup);
											Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped10, 0, 0);
											Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped10, 1);
											ped10.Task.FightAgainst(Game.Player.Character);
										}
									}
									Script.Wait(10);
								}
							}
						}
						if (Crate_2 != null && !GotC4_2)
						{
							Vector3 offsetPosition8 = Crate_2.GetOffsetPosition(new Vector3(2f, 0f, 0f));
							World.DrawMarker(MarkerType.Cylinder, new Vector3(463.6f, -3245.5f, 5.06f), Vector3.Zero, Vector3.Zero, new Vector3(0.65f, 0.65f, 0.65f), Color.Red);
							if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(463.6f, -3245.5f, 6f), "<", 3f))
							{
								if (Crate_2.AttachedBlip != null)
								{
									Crate_2.AttachedBlip.Alpha = 0;
								}
								GotC4_2 = true;
								Notification.Show("~y~ Lester ~w~ : Good! that is that is the C4 we need, grab the rest");
								Game.Player.WantedLevel = 4;
								Vector3 vector5 = new Vector3(493.129f, -3377.43f, 6f);
								for (int num41 = 0; num41 < 7; num41++)
								{
									Ped ped11 = GlobalHelper.CreatePed(PedHash.MerryWeatherCutscene, vector5.Around(10f));
									ped11.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
									Peds.Add(ped11);
									foreach (Ped ped13 in Peds)
									{
										if (ped13 != null)
										{
											RelationshipGroup relationshipGroup2 = Function.Call<int>(Hash.GET_HASH_KEY, "SECURITY_GUARD");
											Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, ped11, relationshipGroup2);
											Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped11, 0, 0);
											Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped11, 1);
											ped11.Task.FightAgainst(Game.Player.Character);
										}
									}
									Script.Wait(10);
								}
							}
						}
						if (GotC4_1 && GotC4_2)
						{
							if (!TriggerUINotify)
							{
								TriggerUINotify = true;
								DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok lose the cops and head to the your Arcade when all objectives are complete");
								SecuredAsset_C4 = true;
							}
							else
							{
								DisplayHelpTextThisFrame("~y~ Lester ~w~ : Lose the cops!");
							}
						}
					}
					if (!SecuredAsset_Vip)
					{
						if (EnteranceUsed != 3 && EnteranceUsed != 4)
						{
							SecuredAsset_Vip = true;
						}
						if (EnteranceUsed == 3 || EnteranceUsed == 4)
						{
							if (FollowCar != null && GetDistance_VDIST2(Game.Player.Character.Position, FollowCar.Position, "<", 300f) && Game.Player.WantedLevel > 0)
							{
								if (Bank != null)
								{
									Bank.Delete();
								}
								if (Lester != null)
								{
									Lester.Delete();
								}
								if (MeetPed != null)
								{
									MeetPed.Delete();
								}
								foreach (Ped aircarftCarrierPed5 in AircarftCarrierPeds)
								{
									if (aircarftCarrierPed5 != null)
									{
										aircarftCarrierPed5.Delete();
									}
								}
								foreach (Vehicle aircraftCarrierVehicle5 in AircraftCarrierVehicles)
								{
									if (aircraftCarrierVehicle5 != null)
									{
										aircraftCarrierVehicle5.Delete();
									}
								}
								DiamdondCasinoSmartGuards.RemoveAIGuards();
								foreach (Ped aircarftCarrierPed6 in AircarftCarrierPeds)
								{
									if (aircarftCarrierPed6 != null)
									{
										aircarftCarrierPed6.Delete();
									}
								}
								foreach (Vehicle aircraftCarrierVehicle6 in AircraftCarrierVehicles)
								{
									if (aircraftCarrierVehicle6 != null)
									{
										aircraftCarrierVehicle6.Delete();
									}
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
								foreach (Ped ped14 in Peds)
								{
									if (ped14 != null)
									{
										ped14.Delete();
									}
								}
								foreach (Vehicle vehicle2 in Vehicles)
								{
									if (vehicle2 != null)
									{
										vehicle2.Delete();
									}
								}
								if (Crate_1 != null)
								{
									Crate_1.Delete();
								}
								if (Crate_2 != null)
								{
									Crate_2.Delete();
								}
								foreach (Vehicle item14 in Veh)
								{
									if (item14 != null)
									{
										item14.Delete();
									}
								}
								MissionOn = false;
								foreach (Prop item15 in PaintingHolder)
								{
									if (item15 != null)
									{
										item15.Delete();
									}
								}
								MissionScreen.SetFailSetup("", "The police were alerted", 13);
							}
							if (!isDriving)
							{
								if (GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 100f))
								{
									World.DrawMarker(MarkerType.Cylinder, Bank.Position, Vector3.Zero, Vector3.Zero, new Vector3(6f, 6f, 2f), Color.Goldenrod);
								}
								if (GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 5f))
								{
									Bank.Alpha = 0;
									if (MeetPed != null)
									{
										MeetPed.Delete();
									}
									foreach (Ped aircarftCarrierPed7 in AircarftCarrierPeds)
									{
										if (aircarftCarrierPed7 != null)
										{
											aircarftCarrierPed7.Delete();
										}
									}
									foreach (Vehicle aircraftCarrierVehicle7 in AircraftCarrierVehicles)
									{
										if (aircraftCarrierVehicle7 != null)
										{
											aircraftCarrierVehicle7.Delete();
										}
									}
									Vehicle vehicle = (FollowCar = GlobalHelper.CreateVehicle(VehicleHash.Riata, new Vector3(901f, 65.8f, 79f), 116f));
									AircraftCarrierVehicles.Add(vehicle);
									Script.Wait(2000);
									vehicle.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.Vinewood01AFY));
									FollowPed = vehicle.GetPedOnSeat(VehicleSeat.Driver);
									AircarftCarrierPeds.Add(vehicle.GetPedOnSeat(VehicleSeat.Driver));
									DriveStage = 0;
									RelationshipGroup relationshipGroup3 = Function.Call<int>(Hash.GET_HASH_KEY, "SECURITY_GUARD");
									Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, FollowPed, relationshipGroup3);
									Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, FollowPed, 0, 0);
									FollowPed.RelationshipGroup = relationshipGroup3;
									Notification.Show("~y~ Lester ~w~ There! that black Riata, follow it and i will hack the targets cellphone to get the VIP pass!");
									isDriving = true;
								}
							}
							if (isDriving)
							{
								if (FollowCar != null && FollowPed != null)
								{
									if (GetDistance_VDIST2(FollowCar.Position, new Vector3(901f, 65.8f, 79f), "<", 20f) && DriveStage == 0)
									{
										DriveStage = 1;
										FollowPed.Task.DriveTo(FollowCar, new Vector3(907.4f, -137.7f, 76f), 20f, 80f, DrivingStyle.AvoidTraffic);
									}
									if (GetDistance_VDIST2(FollowCar.Position, new Vector3(907.4f, -137.7f, 76f), "<", 20f) && DriveStage == 1)
									{
										DriveStage = 2;
										FollowPed.Task.DriveTo(FollowCar, new Vector3(778.9f, -342.8f, 49f), 20f, 70f, DrivingStyle.AvoidTraffic);
									}
									if (GetDistance_VDIST2(FollowCar.Position, new Vector3(778.9f, -342.8f, 49f), "<", 20f) && DriveStage == 2)
									{
										DriveStage = 3;
										FollowPed.Task.DriveTo(FollowCar, new Vector3(120.7f, -171.8f, 59f), 20f, 70f, DrivingStyle.AvoidTraffic);
									}
									if (GetDistance_VDIST2(FollowCar.Position, new Vector3(120.7f, -171.8f, 59f), "<", 20f) && DriveStage == 3)
									{
										DriveStage = 4;
										FollowPed.Task.DriveTo(FollowCar, new Vector3(-1740.8f, -460.9f, 42f), 20f, 80f, DrivingStyle.AvoidTraffic);
									}
									if (GetDistance_VDIST2(FollowCar.Position, new Vector3(-1740.8f, -460.9f, 42f), "<", 20f) && DriveStage == 4)
									{
										DriveStage = 0;
										FollowPed.Task.DriveTo(FollowCar, new Vector3(901f, 65.8f, 79f), 20f, 80f, DrivingStyle.AvoidTraffic);
									}
								}
								if (MeetPed != null && !MeetPed.IsAlive)
								{
									if (Bank != null)
									{
										Bank.Delete();
									}
									if (Lester != null)
									{
										Lester.Delete();
									}
									if (MeetPed != null)
									{
										MeetPed.Delete();
									}
									foreach (Ped aircarftCarrierPed8 in AircarftCarrierPeds)
									{
										if (aircarftCarrierPed8 != null)
										{
											aircarftCarrierPed8.Delete();
										}
									}
									foreach (Vehicle aircraftCarrierVehicle8 in AircraftCarrierVehicles)
									{
										if (aircraftCarrierVehicle8 != null)
										{
											aircraftCarrierVehicle8.Delete();
										}
									}
									DiamdondCasinoSmartGuards.RemoveAIGuards();
									foreach (Ped aircarftCarrierPed9 in AircarftCarrierPeds)
									{
										if (aircarftCarrierPed9 != null)
										{
											aircarftCarrierPed9.Delete();
										}
									}
									foreach (Vehicle aircraftCarrierVehicle9 in AircraftCarrierVehicles)
									{
										if (aircraftCarrierVehicle9 != null)
										{
											aircraftCarrierVehicle9.Delete();
										}
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
									foreach (Ped ped15 in Peds)
									{
										if (ped15 != null)
										{
											ped15.Delete();
										}
									}
									foreach (Vehicle vehicle3 in Vehicles)
									{
										if (vehicle3 != null)
										{
											vehicle3.Delete();
										}
									}
									if (Crate_1 != null)
									{
										Crate_1.Delete();
									}
									if (Crate_2 != null)
									{
										Crate_2.Delete();
									}
									foreach (Vehicle item16 in Veh)
									{
										if (item16 != null)
										{
											item16.Delete();
										}
									}
									returningtofactroy = false;
									isDriving = false;
									SecuredAsset_C4 = false;
									SecuredAsset_Stockade = false;
									SecuredAsset_Vip = false;
									MissionOn = false;
									foreach (Prop item17 in PaintingHolder)
									{
										if (item17 != null)
										{
											item17.Delete();
										}
									}
									MissionScreen.SetFailSetup("", "The Target was alerted", 13);
								}
								if (FollowPed != null && FollowCar != null)
								{
									if (HackProg == 100)
									{
										isDriving = false;
										SecuredAsset_Vip = true;
										Notification.Show("~y~ Lester ~w~  : Ok good, Secure any more outstanding Objectives and return to the your Arcade");
									}
									if (!FollowCar.IsAlive || !FollowPed.IsAlive)
									{
										if (Bank != null)
										{
											Bank.Delete();
										}
										if (Lester != null)
										{
											Lester.Delete();
										}
										if (MeetPed != null)
										{
											MeetPed.Delete();
										}
										foreach (Ped aircarftCarrierPed10 in AircarftCarrierPeds)
										{
											if (aircarftCarrierPed10 != null)
											{
												aircarftCarrierPed10.Delete();
											}
										}
										foreach (Vehicle aircraftCarrierVehicle10 in AircraftCarrierVehicles)
										{
											if (aircraftCarrierVehicle10 != null)
											{
												aircraftCarrierVehicle10.Delete();
											}
										}
										DiamdondCasinoSmartGuards.RemoveAIGuards();
										foreach (Ped aircarftCarrierPed11 in AircarftCarrierPeds)
										{
											if (aircarftCarrierPed11 != null)
											{
												aircarftCarrierPed11.Delete();
											}
										}
										foreach (Vehicle aircraftCarrierVehicle11 in AircraftCarrierVehicles)
										{
											if (aircraftCarrierVehicle11 != null)
											{
												aircraftCarrierVehicle11.Delete();
											}
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
										foreach (Ped ped16 in Peds)
										{
											if (ped16 != null)
											{
												ped16.Delete();
											}
										}
										foreach (Vehicle vehicle4 in Vehicles)
										{
											if (vehicle4 != null)
											{
												vehicle4.Delete();
											}
										}
										if (Crate_1 != null)
										{
											Crate_1.Delete();
										}
										if (Crate_2 != null)
										{
											Crate_2.Delete();
										}
										foreach (Vehicle item18 in Veh)
										{
											if (item18 != null)
											{
												item18.Delete();
											}
										}
										MissionOn = false;
										foreach (Prop item19 in PaintingHolder)
										{
											if (item19 != null)
											{
												item19.Delete();
											}
										}
										MissionScreen.SetFailSetup("", "The Target was alerted", 13);
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, FollowCar.Position, ">", 300f))
									{
										if (Bank != null)
										{
											Bank.Delete();
										}
										if (Lester != null)
										{
											Lester.Delete();
										}
										if (MeetPed != null)
										{
											MeetPed.Delete();
										}
										foreach (Ped aircarftCarrierPed12 in AircarftCarrierPeds)
										{
											if (aircarftCarrierPed12 != null)
											{
												aircarftCarrierPed12.Delete();
											}
										}
										foreach (Vehicle aircraftCarrierVehicle12 in AircraftCarrierVehicles)
										{
											if (aircraftCarrierVehicle12 != null)
											{
												aircraftCarrierVehicle12.Delete();
											}
										}
										DiamdondCasinoSmartGuards.RemoveAIGuards();
										foreach (Ped aircarftCarrierPed13 in AircarftCarrierPeds)
										{
											if (aircarftCarrierPed13 != null)
											{
												aircarftCarrierPed13.Delete();
											}
										}
										foreach (Vehicle aircraftCarrierVehicle13 in AircraftCarrierVehicles)
										{
											if (aircraftCarrierVehicle13 != null)
											{
												aircraftCarrierVehicle13.Delete();
											}
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
										foreach (Ped ped17 in Peds)
										{
											if (ped17 != null)
											{
												ped17.Delete();
											}
										}
										foreach (Vehicle vehicle5 in Vehicles)
										{
											if (vehicle5 != null)
											{
												vehicle5.Delete();
											}
										}
										if (Crate_1 != null)
										{
											Crate_1.Delete();
										}
										if (Crate_2 != null)
										{
											Crate_2.Delete();
										}
										foreach (Vehicle item20 in Veh)
										{
											if (item20 != null)
											{
												item20.Delete();
											}
										}
										returningtofactroy = false;
										isDriving = false;
										SecuredAsset_C4 = false;
										SecuredAsset_Stockade = false;
										SecuredAsset_Vip = false;
										GotC4_1 = false;
										GotC4_2 = false;
										MissionOn = false;
										returningtofactroy = false;
										isDriving = false;
										SecuredAsset_C4 = false;
										SecuredAsset_Stockade = false;
										SecuredAsset_Vip = false;
										MissionOn = false;
										MissionScreen.SetFailSetup("", "The Target escaped", 13);
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, FollowCar.Position, ">", 150f))
									{
										if (Bank != null)
										{
											Bank.Position = Game.Player.Character.Position;
										}
										DisplayHelpTextThisFrame("The Player needs to be within 40m to 150m to hack the Targets cellphone");
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, FollowCar.Position, "<", 50f))
									{
										if (Bank != null)
										{
											Bank.Position = Game.Player.Character.Position;
										}
										DisplayHelpTextThisFrame("The Player needs to be within 40m to 150m to hack the Targets cellphone");
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, FollowCar.Position, "<", 40f))
									{
										if (Bank != null)
										{
											Bank.Delete();
										}
										if (Lester != null)
										{
											Lester.Delete();
										}
										if (MeetPed != null)
										{
											MeetPed.Delete();
										}
										foreach (Ped aircarftCarrierPed14 in AircarftCarrierPeds)
										{
											if (aircarftCarrierPed14 != null)
											{
												aircarftCarrierPed14.Delete();
											}
										}
										foreach (Vehicle aircraftCarrierVehicle14 in AircraftCarrierVehicles)
										{
											if (aircraftCarrierVehicle14 != null)
											{
												aircraftCarrierVehicle14.Delete();
											}
										}
										DiamdondCasinoSmartGuards.RemoveAIGuards();
										foreach (Ped aircarftCarrierPed15 in AircarftCarrierPeds)
										{
											if (aircarftCarrierPed15 != null)
											{
												aircarftCarrierPed15.Delete();
											}
										}
										foreach (Vehicle aircraftCarrierVehicle15 in AircraftCarrierVehicles)
										{
											if (aircraftCarrierVehicle15 != null)
											{
												aircraftCarrierVehicle15.Delete();
											}
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
										foreach (Ped ped18 in Peds)
										{
											if (ped18 != null)
											{
												ped18.Delete();
											}
										}
										foreach (Vehicle vehicle6 in Vehicles)
										{
											if (vehicle6 != null)
											{
												vehicle6.Delete();
											}
										}
										if (Crate_1 != null)
										{
											Crate_1.Delete();
										}
										if (Crate_2 != null)
										{
											Crate_2.Delete();
										}
										foreach (Vehicle item21 in Veh)
										{
											if (item21 != null)
											{
												item21.Delete();
											}
										}
										returningtofactroy = false;
										isDriving = false;
										SecuredAsset_C4 = false;
										SecuredAsset_Stockade = false;
										SecuredAsset_Vip = false;
										GotC4_1 = false;
										GotC4_2 = false;
										MissionOn = false;
										isDriving = false;
										SecuredAsset_C4 = false;
										SecuredAsset_Stockade = false;
										SecuredAsset_Vip = false;
										foreach (Prop item22 in PaintingHolder)
										{
											if (item22 != null)
											{
												item22.Delete();
											}
										}
										MissionScreen.SetFailSetup("", "The Target was Alerted", 13);
									}
									if (GetDistance_VDIST2(Game.Player.Character.Position, FollowCar.Position, ">", 51f) && GetDistance_VDIST2(Game.Player.Character.Position, FollowCar.Position, "<", 150f))
									{
										if (Bank != null)
										{
											Bank.Position = FollowCar.Position;
										}
										if (HackProg != 100)
										{
											if (Hacktime != 125)
											{
												DisplayHelpTextThisFrameNoSound("Hack Progress : " + HackProg + "%, Dist " + GetDistance(Game.Player.Character.Position, FollowCar.Position));
												Hacktime++;
											}
											if (Hacktime == 125)
											{
												Hacktime = 0;
												HackProg++;
												DisplayHelpTextThisFrame("Hack Progress : " + HackProg + "%, Dist " + GetDistance(Game.Player.Character.Position, FollowCar.Position));
											}
										}
									}
								}
							}
						}
					}
					if (!SecuredAsset_Stockade)
					{
						if (EnteranceUsed != 1)
						{
							SecuredAsset_Stockade = true;
						}
						if (EnteranceUsed == 1)
						{
							if (StockadeA != null && StockadeB != null && StockadeC != null)
							{
								if (StockadeA.GetPedOnSeat(VehicleSeat.Driver) != Game.Player.Character)
								{
									if (GetDistance_VDIST2(StockadeA.Position, PointA, "<", 60f) && StockadeA.Opacity == 255)
									{
										StockadeA.Opacity = 254;
										StockadeA.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeA, PointB, 60f, 80f);
									}
									if (GetDistance_VDIST2(StockadeA.Position, PointB, "<", 60f) && StockadeA.Opacity == 254)
									{
										StockadeA.Opacity = 253;
										StockadeA.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeA, PointC, 60f, 80f);
									}
									if (GetDistance_VDIST2(StockadeA.Position, PointC, "<", 60f) && StockadeA.Opacity == 253)
									{
										StockadeA.Opacity = 255;
										StockadeA.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeA, PointA, 60f, 80f);
									}
								}
								if (StockadeB.GetPedOnSeat(VehicleSeat.Driver) != Game.Player.Character)
								{
									if (GetDistance_VDIST2(StockadeB.Position, PointA, "<", 60f) && StockadeB.Opacity == 255)
									{
										StockadeB.Opacity = 254;
										StockadeB.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeB, PointB, 60f, 80f);
									}
									if (GetDistance_VDIST2(StockadeB.Position, PointB, "<", 60f) && StockadeB.Opacity == 254)
									{
										StockadeB.Opacity = 253;
										StockadeB.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeB, PointC, 60f, 80f);
									}
									if (GetDistance_VDIST2(StockadeB.Position, PointC, "<", 60f) && StockadeB.Opacity == 253)
									{
										StockadeB.Opacity = 255;
										StockadeB.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeB, PointA, 60f, 80f);
									}
								}
								if (StockadeC.GetPedOnSeat(VehicleSeat.Driver) != Game.Player.Character)
								{
									if (GetDistance_VDIST2(StockadeC.Position, PointA, "<", 60f) && StockadeC.Opacity == 255)
									{
										StockadeC.Opacity = 254;
										StockadeC.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeC, PointB, 60f, 80f);
									}
									if (GetDistance_VDIST2(StockadeC.Position, PointB, "<", 60f) && StockadeC.Opacity == 254)
									{
										StockadeC.Opacity = 253;
										StockadeC.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeC, PointC, 60f, 80f);
									}
									if (GetDistance_VDIST2(StockadeC.Position, PointC, "<", 60f) && StockadeC.Opacity == 253)
									{
										StockadeC.Opacity = 255;
										StockadeC.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeC, PointA, 60f, 80f);
									}
								}
							}
							if (!SecuredAsset_Stockade && StockadeA != null && StockadeB != null && StockadeC != null)
							{
								if (!StockadeA.IsAlive && !StockadeB.IsAlive && !StockadeC.IsAlive)
								{
									if (Bank != null)
									{
										Bank.Delete();
									}
									if (Lester != null)
									{
										Lester.Delete();
									}
									if (MeetPed != null)
									{
										MeetPed.Delete();
									}
									foreach (Ped aircarftCarrierPed16 in AircarftCarrierPeds)
									{
										if (aircarftCarrierPed16 != null)
										{
											aircarftCarrierPed16.Delete();
										}
									}
									foreach (Vehicle aircraftCarrierVehicle16 in AircraftCarrierVehicles)
									{
										if (aircraftCarrierVehicle16 != null)
										{
											aircraftCarrierVehicle16.Delete();
										}
									}
									DiamdondCasinoSmartGuards.RemoveAIGuards();
									foreach (Ped aircarftCarrierPed17 in AircarftCarrierPeds)
									{
										if (aircarftCarrierPed17 != null)
										{
											aircarftCarrierPed17.Delete();
										}
									}
									foreach (Vehicle aircraftCarrierVehicle17 in AircraftCarrierVehicles)
									{
										if (aircraftCarrierVehicle17 != null)
										{
											aircraftCarrierVehicle17.Delete();
										}
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
									foreach (Ped ped19 in Peds)
									{
										if (ped19 != null)
										{
											ped19.Delete();
										}
									}
									foreach (Vehicle vehicle7 in Vehicles)
									{
										if (vehicle7 != null)
										{
											vehicle7.Delete();
										}
									}
									if (Crate_1 != null)
									{
										Crate_1.Delete();
									}
									if (Crate_2 != null)
									{
										Crate_2.Delete();
									}
									foreach (Vehicle item23 in Veh)
									{
										if (item23 != null)
										{
											item23.Delete();
										}
									}
									returningtofactroy = false;
									isDriving = false;
									SecuredAsset_C4 = false;
									SecuredAsset_Stockade = false;
									SecuredAsset_Vip = false;
									GotC4_1 = false;
									GotC4_2 = false;
									MissionOn = false;
									foreach (Prop item24 in PaintingHolder)
									{
										if (item24 != null)
										{
											item24.Delete();
										}
									}
									MissionScreen.SetFailSetup("", "All Stockades Were Destoyed", 13);
								}
								if (Game.Player.Character.CurrentVehicle != null)
								{
									Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
									if ((currentVehicle == StockadeA || currentVehicle == StockadeB || currentVehicle == StockadeC) && currentVehicle.IsAlive)
									{
										GetDistance_VDIST2(Game.Player.Character.Position, CheckAppartmentPos(), "<", 100f);
										if (GetDistance_VDIST2(Game.Player.Character.Position, CheckAppartmentPos(), "<", 5f))
										{
											SecuredAsset_Stockade = true;
											Notification.Show("~y~ Lester ~w~  : Ok, good we have secured a Stockade for the Heist, Secure C4 if you Havent already ~w~");
										}
									}
								}
							}
						}
					}
					if (SecuredAsset_C4 && SecuredAsset_Stockade)
					{
						bool securedAsset_Vip = SecuredAsset_Vip;
					}
					if (SecuredAsset_C4 && SecuredAsset_Stockade && SecuredAsset_Vip && !returningtofactroy)
					{
						returningtofactroy = true;
					}
					if (!returningtofactroy)
					{
						break;
					}
					if (Game.Player.WantedLevel == 0)
					{
						GTA.UI.Screen.ShowSubtitle("All Objectives Complete, return to the ~y~Arcade~w~");
					}
					else
					{
						GTA.UI.Screen.ShowSubtitle("Lose the ~r~Cops");
					}
					if (Game.Player.WantedLevel != 0)
					{
						break;
					}
					GetDistance_VDIST2(Game.Player.Character.Position, CheckAppartmentPos(), "<", 100f);
					if (!GetDistance_VDIST2(Game.Player.Character.Position, CheckAppartmentPos(), "<", 5f))
					{
						break;
					}
					DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok good, Weve got C4, now we can blow, the lock  ~y~|| Mission Complete ||~w~");
					MissionOn = false;
					GotPlans = false;
					Class1.SetNoLongerInHeist();
					MissionScreen.SetPassSetup("", "", 13);
					HeistStage = 3;
					Config.SetValue("TheDiamondHiest", "HeistStage", HeistStage);
					Config.Save();
					GotC4_1 = false;
					GotC4_2 = false;
					if (Bank != null)
					{
						Bank.Delete();
					}
					if (Lester != null)
					{
						Lester.Delete();
					}
					if (MeetPed != null)
					{
						MeetPed.Delete();
					}
					foreach (Ped aircarftCarrierPed18 in AircarftCarrierPeds)
					{
						if (aircarftCarrierPed18 != null)
						{
							aircarftCarrierPed18.Delete();
						}
					}
					foreach (Vehicle aircraftCarrierVehicle18 in AircraftCarrierVehicles)
					{
						if (aircraftCarrierVehicle18 != null)
						{
							aircraftCarrierVehicle18.Delete();
						}
					}
					DiamdondCasinoSmartGuards.RemoveAIGuards();
					foreach (Ped aircarftCarrierPed19 in AircarftCarrierPeds)
					{
						if (aircarftCarrierPed19 != null)
						{
							aircarftCarrierPed19.Delete();
						}
					}
					foreach (Vehicle aircraftCarrierVehicle19 in AircraftCarrierVehicles)
					{
						if (aircraftCarrierVehicle19 != null)
						{
							aircraftCarrierVehicle19.Delete();
						}
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
					foreach (Ped ped20 in Peds)
					{
						if (ped20 != null)
						{
							ped20.Delete();
						}
					}
					foreach (Vehicle vehicle8 in Vehicles)
					{
						if (vehicle8 != null)
						{
							vehicle8.Delete();
						}
					}
					if (Crate_1 != null)
					{
						Crate_1.Delete();
					}
					if (Crate_2 != null)
					{
						Crate_2.Delete();
					}
					foreach (Vehicle item25 in Veh)
					{
						if (item25 != null)
						{
							item25.Delete();
						}
					}
					returningtofactroy = false;
					isDriving = false;
					SecuredAsset_C4 = false;
					SecuredAsset_Stockade = false;
					SecuredAsset_Vip = false;
					GotC4_1 = false;
					GotC4_2 = false;
					{
						foreach (Prop item26 in PaintingHolder)
						{
							if (item26 != null)
							{
								item26.Delete();
							}
						}
						break;
					}
				}
				if (Bank != null)
				{
					Bank.Delete();
				}
				if (Lester != null)
				{
					Lester.Delete();
				}
				if (MeetPed != null)
				{
					MeetPed.Delete();
				}
				foreach (Ped aircarftCarrierPed20 in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed20 != null)
					{
						aircarftCarrierPed20.Delete();
					}
				}
				foreach (Vehicle aircraftCarrierVehicle20 in AircraftCarrierVehicles)
				{
					if (aircraftCarrierVehicle20 != null)
					{
						aircraftCarrierVehicle20.Delete();
					}
				}
				DiamdondCasinoSmartGuards.RemoveAIGuards();
				foreach (Ped aircarftCarrierPed21 in AircarftCarrierPeds)
				{
					if (aircarftCarrierPed21 != null)
					{
						aircarftCarrierPed21.Delete();
					}
				}
				foreach (Vehicle aircraftCarrierVehicle21 in AircraftCarrierVehicles)
				{
					if (aircraftCarrierVehicle21 != null)
					{
						aircraftCarrierVehicle21.Delete();
					}
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
				foreach (Ped ped21 in Peds)
				{
					if (ped21 != null)
					{
						ped21.Delete();
					}
				}
				foreach (Vehicle vehicle9 in Vehicles)
				{
					if (vehicle9 != null)
					{
						vehicle9.Delete();
					}
				}
				if (Crate_1 != null)
				{
					Crate_1.Delete();
				}
				if (Crate_2 != null)
				{
					Crate_2.Delete();
				}
				foreach (Vehicle item27 in Veh)
				{
					if (item27 != null)
					{
						item27.Delete();
					}
				}
				returningtofactroy = false;
				isDriving = false;
				SecuredAsset_C4 = false;
				SecuredAsset_Stockade = false;
				SecuredAsset_Vip = false;
				GotC4_1 = false;
				GotC4_2 = false;
				MissionOn = false;
				foreach (Prop item28 in PaintingHolder)
				{
					if (item28 != null)
					{
						item28.Delete();
					}
				}
				MissionScreen.SetFailSetup("", "You have Been Killed", 13);
				break;
			case 1:
				if (Lester != null && !Lester.IsAlive)
				{
					foreach (Ped ped22 in Peds)
					{
						if (ped22 != null)
						{
							ped22.Delete();
						}
					}
					foreach (Vehicle vehicle10 in Vehicles)
					{
						if (vehicle10 != null)
						{
							vehicle10.Delete();
						}
					}
					MissionOn = false;
					if (Bank != null)
					{
						Bank.Delete();
					}
					if (Lester != null)
					{
						Lester.Delete();
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
					MissionOn = false;
					MissionScreen.SetFailSetup("", "Lester has been killed", 13);
				}
				if (Game.Player.IsAlive)
				{
					if (GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 100f))
					{
						World.DrawMarker(MarkerType.Cylinder, Bank.Position, Vector3.Zero, Vector3.Zero, new Vector3(4f, 4f, 0.75f), Color.Goldenrod);
					}
					if (LocAt == 1)
					{
						GTA.UI.Screen.ShowSubtitle("Drive up to the ~y~Diamond Caisno~w~");
						if (GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 2f))
						{
							Notification.Show("~y~ Lester ~w~ Ok your first choice for both an exit and enterance is the Main Enterance");
							Notification.Show("~y~ Lester ~w~ With this Enterance you will likely need to fight past guards");
							Bank.Position = new Vector3(876.7f, -40.6f, 77f);
							LocAt = 2;
							Notification.Show("~y~ Lester ~w~ move to the next location, in the Casino Parking Lot");
						}
					}
					if (LocAt == 2)
					{
						GTA.UI.Screen.ShowSubtitle("Drive to the ~y~Parking Loty~w~");
						if (GetDistance_VDIST2(Game.Player.Character.Position, Bank.Position, "<", 2f))
						{
							Notification.Show("~y~ Lester ~w~ Looks like we could also enter through the Loading Bay");
							Notification.Show("~y~ Lester ~w~ see what vehicle we need");
							Bank.Position = new Vector3(876.7f, -40.6f, 77f);
							LocAt = 3;
							StockadeA = GlobalHelper.CreateVehicle(VehicleHash.Stockade, new Vector3(816.4f, -190.6f, 71f), -35f);
							StockadeA.CreatePedOnSeat(VehicleSeat.Driver, RequestModel(PedHash.SecuroGuardMale01));
							Peds.Add(StockadeA.GetPedOnSeat(VehicleSeat.Driver));
							StockadeA.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(StockadeA, new Vector3(932.7f, -3f, 77f), 5f, 30f);
						}
					}
					if (LocAt == 3 && StockadeA != null)
					{
						GTA.UI.Screen.ShowSubtitle("Investigate the ~b~Stockade~w~ ");
						if (GetDistance_VDIST2(StockadeA.Position, new Vector3(932.7f, -3f, 77f), "<", 5f))
						{
							Notification.Show("~y~ Lester ~w~ Ok, looks like a Stockade, you could steal a Stockade and enter desguised as Groupe 6");
							Bank.Position = new Vector3(990.9f, -145.5f, 34f);
							LocAt = 4;
							Notification.Show("~y~ Lester ~w~ move to the next location, in the LS Sewers");
						}
					}
					if (LocAt == 4)
					{
						GTA.UI.Screen.ShowSubtitle("Drive to the ~y~Sewer Enterance~w~");
						if (GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(990.9f, -145.5f, 34f), "<", 2f))
						{
							Notification.Show("~y~ Lester ~w~ With this you could enter through the Sewer tunnel, and i believe it ends up in the Casino, from the schematics ");
							Notification.Show("~y~ Lester ~w~ Using this enterace you could keep the Heist relativly quiet, Ok return to the your Arcade");
							Bank.Position = CheckAppartmentPos();
							LocAt = 5;
						}
					}
					if (LocAt == 6)
					{
						GTA.UI.Screen.ShowSubtitle("Choose an ~y~Enterance & Exit ");
						DisplayHelpTextThisFrame("Please Choose an Enterance & Exit");
						if (!modLemonUIObjectPool2.AreAnyVisible)
						{
							mainMenu2.Visible = true;
							if (HeistStartCam != null)
							{
								Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
								HeistStartCam.IsActive = false;
								HeistStartCam.Delete();
								Game.Player.Character.IsPositionFrozen = false;
								Game.Player.CanControlCharacter = true;
							}
							HeistStartCam = GlobalHelper.CreateCamera(new Vector3(910.9f, 43.3f, 82.5f), new Vector3(0f, 0f, -80f), 60f);
							HeistStartCam.IsActive = true;
							HeistStartCam.FarClip = 2000f;
							Game.Player.Character.Position = HeistStartCam.GetOffsetPosition(new Vector3(0f, -2f, 0f));
							Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
							Game.Player.Character.IsPositionFrozen = true;
							Game.Player.CanControlCharacter = false;
							World.RenderingCamera = HeistStartCam;
						}
					}
					if (LocAt != 5 && LocAt != 6 && LocAt != 7)
					{
						break;
					}
					GetDistance_VDIST2(Game.Player.Character.Position, CheckAppartmentPos(), "<", 100f);
					if (!GetDistance_VDIST2(Game.Player.Character.Position, CheckAppartmentPos(), "<", 5f))
					{
						break;
					}
					if (LocAt == 5)
					{
						LocAt = 6;
					}
					if (LocAt != 7)
					{
						break;
					}
					foreach (Ped ped23 in Peds)
					{
						if (ped23 != null)
						{
							ped23.Delete();
						}
					}
					foreach (Vehicle vehicle11 in Vehicles)
					{
						if (vehicle11 != null)
						{
							vehicle11.Delete();
						}
					}
					MissionOn = false;
					if (Bank != null)
					{
						Bank.Delete();
					}
					if (Lester != null)
					{
						Lester.Delete();
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
					DisplayHelpTextThisFrame("~y~ Lester ~w~ : Ok good, we can start planning out the heist  ~y~|| Mission Complete ||~w~");
					headingbacktoyourAppartment = false;
					HeistStage = 2;
					Config.SetValue("TheDiamondHiest", "HeistStage", HeistStage);
					Config.Save();
					Class1.SetNoLongerInHeist();
					MissionScreen.SetPassSetup("", "You have Been Killed or Lester died", 13);
					MissionOn = false;
					break;
				}
				foreach (Ped ped24 in Peds)
				{
					if (ped24 != null)
					{
						ped24.Delete();
					}
				}
				foreach (Vehicle vehicle12 in Vehicles)
				{
					if (vehicle12 != null)
					{
						vehicle12.Delete();
					}
				}
				MissionOn = false;
				if (Bank != null)
				{
					Bank.Delete();
				}
				if (Lester != null)
				{
					Lester.Delete();
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
				MissionOn = false;
				MissionScreen.SetFailSetup("", "You have Been Killed", 13);
				break;
			}
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
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "More Power");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Less Power");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
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
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "More Power");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Less Power");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 76, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
			scaleform.Dispose();
			scaleform.Dispose();
		}

		public void OnKeyDown(object sender, KeyEventArgs e)
		{
		}

		public void OnShutdown(object sender, EventArgs e)
		{
			PoliceAlearted = false;
			LesterToldPlayerAboutContractCut = false;
			foreach (ObjectPlacementMap swatEntty in SwatEntties)
			{
				if (swatEntty.Entity != null)
				{
					swatEntty.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap bugStarVent in BugStarVents)
			{
				if (bugStarVent.Entity != null)
				{
					bugStarVent.Entity.Delete();
				}
			}
			foreach (ObjectPlacementMap groupe6Cam in Groupe6Cams)
			{
				if (groupe6Cam.Entity != null)
				{
					groupe6Cam.Entity.Delete();
				}
			}
			if (1 == 0)
			{
				return;
			}
			if (SecurityCabinetProps.Count > 0)
			{
				foreach (SecCabinetProps securityCabinetProp in SecurityCabinetProps)
				{
					if (securityCabinetProp.GlassDisplayCase != null)
					{
						securityCabinetProp.GlassDisplayCase.Delete();
					}
					foreach (Prop prop in securityCabinetProp.Props)
					{
						if (prop != null)
						{
							prop.Delete();
						}
					}
				}
			}
			if (RobberACurrentLootBagProp != null)
			{
				RobberACurrentLootBagProp.Delete();
			}
			if (RobberBCurrentLootBagProp != null)
			{
				RobberBCurrentLootBagProp.Delete();
			}
			if (RobberCCurrentLootBagProp != null)
			{
				RobberCCurrentLootBagProp.Delete();
			}
			if (RobberACurrentLootBagProp != null)
			{
				RobberACurrentLootBagProp.Delete();
			}
			if (PlayerCurrentLootBagProp != null)
			{
				PlayerCurrentLootBagProp.Delete();
			}
			if (MDBox1 != null)
			{
				MDBox1.Delete();
			}
			if (MDBox2 != null)
			{
				MDBox2.Delete();
			}
			if (MDBox3 != null)
			{
				MDBox3.Delete();
			}
			foreach (KeypadClass keyPad in KeyPads)
			{
				if (keyPad.Keypad != null)
				{
					keyPad.Keypad.Delete();
				}
			}
			if (Phone != null)
			{
				Phone.Delete();
			}
			if (USB != null)
			{
				USB.Delete();
			}
			if (purchaseBlip != null)
			{
				purchaseBlip.Delete();
			}
			if (purchaseBlip2 != null)
			{
				purchaseBlip2.Delete();
			}
			if (RobberA_bagProp != null)
			{
				RobberA_bagProp.Delete();
			}
			if (RobberB_bagProp != null)
			{
				RobberB_bagProp.Delete();
			}
			if (RobberC_bagProp != null)
			{
				RobberC_bagProp.Delete();
			}
			if (RobberA_HoldingLootProp != null)
			{
				RobberA_HoldingLootProp.Delete();
			}
			if (RobberB_HoldingLootProp != null)
			{
				RobberB_HoldingLootProp.Delete();
			}
			if (RobberC_HoldingLootProp != null)
			{
				RobberC_HoldingLootProp.Delete();
			}
			foreach (Prop prop2 in Props)
			{
				if (prop2 != null)
				{
					prop2.Delete();
				}
			}
			Function.Call(Hash.STOP_SOUND, Sound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, Sound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Drill != null)
			{
				Drill.Delete();
			}
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			if (CutsenePed1 != null)
			{
				CutsenePed1.Delete();
			}
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
			foreach (Prop smokeEmitter in SmokeEmitters)
			{
				if (smokeEmitter != null)
				{
					smokeEmitter.Delete();
				}
			}
			if (Vent != null)
			{
				Vent.Delete();
			}
			Function.Call(Hash.REMOVE_PARTICLE_FX_FROM_ENTITY, Game.Player.Character);
			if (HackConsole != null)
			{
				HackConsole.Delete();
			}
			if (ExtraValuable != null && ExtraValuable.Loot != null)
			{
				ExtraValuable.Loot.Delete();
			}
			if (VaultDoor != null)
			{
				VaultDoor.Delete();
			}
			foreach (Ped swatPed in SwatPeds)
			{
				if (swatPed != null)
				{
					swatPed.Delete();
				}
			}
			foreach (Vehicle swatVehicle in SwatVehicles)
			{
				if (swatVehicle != null)
				{
					swatVehicle.Delete();
				}
			}
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
			if (SecondaryGetawayVeh != null)
			{
				SecondaryGetawayVeh.Delete();
			}
			if (DeliveryPoint != null)
			{
				DeliveryPoint.Delete();
			}
			foreach (Ped ped in Peds)
			{
				if (ped != null)
				{
					ped.Delete();
				}
			}
			foreach (Vehicle vehicle in Vehicles)
			{
				if (vehicle != null)
				{
					vehicle.Delete();
				}
			}
			if (Crate_1 != null)
			{
				Crate_1.Delete();
			}
			if (Crate_2 != null)
			{
				Crate_2.Delete();
			}
			foreach (Vehicle item in Veh)
			{
				if (item != null)
				{
					item.Delete();
				}
			}
			foreach (Ped ped2 in Peds)
			{
				if (ped2 != null)
				{
					ped2.Delete();
				}
			}
			foreach (Vehicle vehicle2 in Vehicles)
			{
				if (vehicle2 != null)
				{
					vehicle2.Delete();
				}
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
			if (Lester != null)
			{
				Lester.Delete();
			}
			if (Prizecar != null)
			{
				Prizecar.Delete();
			}
			foreach (Ped item2 in SMPed)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			if (WheelProp != null)
			{
				WheelProp.Delete();
			}
			if (WheelProp2 != null)
			{
				WheelProp2.Delete();
			}
			if (BasePlate != null)
			{
				BasePlate.Delete();
			}
			foreach (Ped item3 in MainPed)
			{
				if (item3 != null)
				{
					item3.Delete();
				}
			}
			foreach (Prop table in Tables)
			{
				if (table != null)
				{
					table.Delete();
				}
			}
			foreach (Prop item4 in Tables2)
			{
				if (item4 != null)
				{
					item4.Delete();
				}
			}
			foreach (Ped reinforcementsPed in ReinforcementsPeds)
			{
				if (reinforcementsPed != null)
				{
					reinforcementsPed.Delete();
				}
			}
			foreach (Vehicle reinforcementsVehicle in ReinforcementsVehicles)
			{
				if (reinforcementsVehicle != null)
				{
					reinforcementsVehicle.Delete();
				}
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			if (GetawayCar != null)
			{
				GetawayCar.Delete();
			}
			foreach (Vehicle item5 in AircraftCarrierVehicles2)
			{
				if (item5 != null)
				{
					item5.Delete();
				}
			}
			foreach (Prop prop3 in Props)
			{
				if (prop3 != null)
				{
					prop3.Delete();
				}
			}
			DiamdondCasinoSmartGuards.RemoveAIGuards();
			foreach (Ped aircarftCarrierPed in AircarftCarrierPeds)
			{
				if (aircarftCarrierPed != null)
				{
					aircarftCarrierPed.Delete();
				}
			}
			foreach (Vehicle aircraftCarrierVehicle in AircraftCarrierVehicles)
			{
				if (aircraftCarrierVehicle != null)
				{
					aircraftCarrierVehicle.Delete();
				}
			}
			foreach (Vehicle item6 in AttackAircraft)
			{
				if (item6 != null)
				{
					item6.Delete();
				}
			}
			foreach (Ped item7 in AttackPed)
			{
				if (item7 != null)
				{
					item7.Delete();
				}
			}
			if (HeistStartCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				HeistStartCam.IsActive = false;
				HeistStartCam.Delete();
				Game.Player.Character.IsPositionFrozen = false;
				Game.Player.CanControlCharacter = true;
			}
			if (Bank != null)
			{
				Bank.Delete();
			}
			if (PatrolCar1 != null)
			{
				PatrolCar1.Delete();
			}
			if (PatrolCar2 != null)
			{
				PatrolCar2.Delete();
			}
			if (PatrolCar != null)
			{
				PatrolCar.Delete();
			}
			foreach (Prop item8 in PaintingHolder)
			{
				if (item8 != null)
				{
					item8.Delete();
				}
			}
		}
	}
}
