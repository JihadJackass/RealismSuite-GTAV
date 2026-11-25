using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace GTAO_DiamondCasino.Penthouse
{
	internal class PenthouseJacuzzi : Script
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

			public int ObjectColor { get; set; }

			public ObjectType ObjType { get; set; }

			public VehicleHash VehicleHash { get; set; }

			public Model RequestModel(int Name)
			{
				Model result = new Model(Name);
				result.Request(50);
				if (result.IsInCdImage && result.IsValid && !result.IsLoaded)
				{
					return result;
				}
				result.MarkAsNoLongerNeeded();
				return result;
			}

			public Model RequestModel(VehicleHash Name)
			{
				Model result = new Model(Name);
				result.Request(250);
				if (result.IsInCdImage && result.IsValid && !result.IsLoaded)
				{
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
					if (result.IsInCdImage && result.IsValid && !result.IsLoaded)
					{
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

			public ObjectPlacementMap(int M, int AttH, uint H, Vector3 P, Vector3 R, int HTAT, int Bone, Vector3 Offset, Vector3 Rot, int objectcolor)
			{
				ObjectColor = objectcolor;
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

		public static Quaternion JaccuziEnterPosition = new Quaternion(949.2f, 11.7f, 116.21f, -170.27f);

		public static Quaternion JaccuziMiddlePosition = new Quaternion(949.39f, 9.71f, 115.21f, 169.65f);

		public static Quaternion JaccuziSeat;

		public static List<ObjectPlacementMap> Seats = new List<ObjectPlacementMap>
		{
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_drop_armscrate_01b"), 463363, 1877891248u, new Vector3(951.145752f, 10.0187931f, 115.880409f), new Vector3(0f, -0f, -98.54206f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_drop_armscrate_01b"), 620803, 1877891248u, new Vector3(950.7752f, 9.043041f, 115.880409f), new Vector3(0f, -0f, -121.572678f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_drop_armscrate_01b"), 184580, 1877891248u, new Vector3(950.004761f, 8.225697f, 115.880409f), new Vector3(0f, -0f, -146.8553f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_drop_armscrate_01b"), 313860, 1877891248u, new Vector3(949.077454f, 8.217877f, 115.880409f), new Vector3(0f, -0f, 163.54982f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_drop_armscrate_01b"), 637956, 1877891248u, new Vector3(948.2972f, 8.628521f, 115.880409f), new Vector3(0f, -0f, 139.339874f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "prop_drop_armscrate_01b"), 7429, 1877891248u, new Vector3(947.6987f, 9.70972f, 115.880409f), new Vector3(0f, 0f, 88.0556f), -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		};

		public static int JacuzziAnimDict = 5;

		public static bool PlayJacuzziIdles = false;

		public static bool IsInJacuzzi;

		public static int Scene1;

		public static Vector3 LastSeatPos;

		public static float LastSeatHeading;

		public static int PlayingEnter;

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public PenthouseJacuzzi()
		{
			if (_Debug.PenthouseParty != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static Vector3 CalculateNewCoordinates(float x, float y, float z, float heading, float distance, int offsetType)
		{
			float num = (float)((double)heading * Math.PI / 180.0);
			float num2 = 0f;
			float num3 = 0f;
			if (offsetType == 0)
			{
				num2 = (float)((double)distance * Math.Sin(num));
			}
			if (offsetType == 1)
			{
				num3 = (float)((double)distance * Math.Cos(num));
			}
			if (offsetType == 2)
			{
				num2 = (float)((double)distance * Math.Sin(num));
				num3 = (float)((double)distance * Math.Cos(num));
			}
			float x2 = x + num2;
			float y2 = y + num3;
			return new Vector3(x2, y2, z);
		}

		public static Vector3 CalculateNewCoordinates(float x, float y, float z, float heading, Vector3 Offset)
		{
			float num = (float)((double)heading * Math.PI / 180.0);
			float x2 = Offset.X;
			float y2 = Offset.Y;
			x2 = (float)((double)Offset.X * Math.Sin(num));
			y2 = (float)((double)Offset.Y * Math.Cos(num));
			float x3 = x + x2;
			float y3 = y + y2;
			return new Vector3(x3, y3, z);
		}

		public static Vector3 CalculateNewCoordinates(Vector3 Pos, float heading, Vector3 offset)
		{
			return new Vector3(Pos.X + (float)((double)offset.X * Math.Cos(heading) - (double)offset.Y * Math.Sin(heading)), Pos.Y + (float)((double)offset.X * Math.Sin(heading) + (double)offset.Y * Math.Cos(heading)), Pos.Z + offset.Z);
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

		public static void ToggleClothes(bool Swimware)
		{
			if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
				Type type = Assembly.LoadFrom("scripts/PlayableOnlineCharacter.dll").GetType("PlayableOnlineCharacter.CharacterMenu");
				MethodInfo method = type.GetMethod("ToggleTop");
				object[] parameters = new object[1] { Swimware };
				method.Invoke(method, parameters);
				method = type.GetMethod("ToggleBottom");
				parameters = new object[1] { Swimware };
				method.Invoke(method, parameters);
				method = type.GetMethod("ToggleShoes");
				parameters = new object[1] { Swimware };
				method.Invoke(method, parameters);
			}
			else
			{
				if (Game.Player.Character.Model == PedHash.Michael)
				{
				}
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
				}
				if (!(Game.Player.Character.Model == PedHash.Franklin))
				{
				}
			}
		}

		public unsafe static void OnTick(object sender, EventArgs e)
		{
			if (!PenthouseStuff.IsInPenhouse || !PenthouseStuff.IsInPenhouse)
			{
				return;
			}
			if (!IsInJacuzzi)
			{
				if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(JaccuziEnterPosition.X, JaccuziEnterPosition.Y, JaccuziEnterPosition.Z), Class1.GetDistSign.Less, 2.5f))
				{
					Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to enter jacuzzi.");
					if (Game.IsControlJustPressed(Control.Context) && !PlayJacuzziIdles)
					{
						float num = 0f;
						int num2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
						if (num2 <= 20)
						{
							num = -1f;
							JacuzziAnimDict = 1;
						}
						if (num2 > 20 && num2 <= 40)
						{
							num = -1f;
							JacuzziAnimDict = 2;
						}
						if (num2 > 40 && num2 <= 60)
						{
							num = -1f;
							JacuzziAnimDict = 3;
						}
						if (num2 > 60 && num2 <= 80)
						{
							num = 0f;
							JacuzziAnimDict = 4;
						}
						if (num2 > 80 && num2 <= 100)
						{
							num = 0f;
							JacuzziAnimDict = 5;
						}
						Notification.Show("JacuzziAnimDict " + JacuzziAnimDict);
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
						int taskID = 0;
						Utils.OpenSequenceTask(&taskID);
						int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, Seats.Count);
						Vector3 position = Seats[index].Position;
						JaccuziSeat = new Quaternion(w: Seats[index].Rotation.Z, x: position.X, y: position.Y, z: position.Z);
						Vector3 _ = new Vector3(JaccuziSeat.X, JaccuziSeat.Y, JaccuziSeat.Z);
						Utils.TaskPedSlideToCoord(0, new Vector3(JaccuziMiddlePosition.X, JaccuziMiddlePosition.Y, JaccuziMiddlePosition.Z), JaccuziMiddlePosition.W, 1f);
						Utils.TaskPedSlideToCoord(0, new Vector3(JaccuziSeat.X, JaccuziSeat.Y, JaccuziSeat.Z), JaccuziSeat.W, 1f);
						Function.Call(Hash.TASK_STAND_STILL, 0, 1);
						Utils.CloseSequenceTask(taskID);
						Utils.TaskPerformSequence(Game.Player.Character, taskID);
						Utils.ClearSequenceTask(&taskID);
						while (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(JaccuziMiddlePosition.X, JaccuziMiddlePosition.Y, JaccuziMiddlePosition.Z), Class1.GetDistSign.Greater, 1.2f))
						{
							Script.Wait(1);
						}
						ToggleClothes(true);
						while (Class1.GetDistance_VDIST2(Game.Player.Character.Position, _, Class1.GetDistSign.Greater, 0.75f))
						{
							Script.Wait(1);
						}
						Game.Player.Character.IsPositionFrozen = true;
						Game.Player.Character.Position = new Vector3(JaccuziSeat.X, JaccuziSeat.Y, JaccuziSeat.Z);
						Game.Player.Character.Heading = JaccuziSeat.W;
						_ = Game.Player.Character.GetOffsetPosition(new Vector3(0f, -0.55f, 0f));
						_ = new Vector3(_.X, _.Y, JaccuziSeat.Z + 0.25f);
						string dict = "anim@amb@yacht@jacuzzi@seated@female@variation_0" + JacuzziAnimDict + "@";
						float w = JaccuziSeat.W;
						w -= 180f;
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, _.X, _.Y, _.Z + num, 0.0, 0.0, w, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict), "enter", 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict(dict), 16f, -16f, 0, 1148846080);
						PlayJacuzziIdles = true;
						PlayingEnter = 1;
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 && PlayingEnter == 1)
				{
					IsInJacuzzi = true;
					LastSeatPos = Game.Player.Character.Position;
					LastSeatHeading = Game.Player.Character.Heading;
					PlayingEnter = 0;
				}
			}
			else
			{
				if (!IsInJacuzzi)
				{
					return;
				}
				if (PlayJacuzziIdles && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
				{
					string text = "idle_a";
					int num3 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
					if (num3 <= 25)
					{
						text = "idle_a";
					}
					if (num3 > 25 && num3 <= 50)
					{
						text = "idle_b";
					}
					if (num3 > 50 && num3 <= 75)
					{
						text = "idle_c";
					}
					if (num3 > 75 && num3 <= 100)
					{
						text = "idle_d";
					}
					Vector3 vector = Game.Player.Character.Position;
					if (JacuzziAnimDict == 1)
					{
						Game.Player.Character.Position = LastSeatPos;
						Game.Player.Character.Heading = LastSeatHeading;
						vector = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.4f, -1f));
						vector = new Vector3(vector.X, vector.Y, vector.Z - 0.5f);
					}
					if (JacuzziAnimDict == 2)
					{
						Game.Player.Character.Position = LastSeatPos;
						Game.Player.Character.Heading = LastSeatHeading;
						vector = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.4f, -1f));
						vector = new Vector3(vector.X, vector.Y, vector.Z - 0.5f);
					}
					if (JacuzziAnimDict == 3)
					{
						Game.Player.Character.Position = LastSeatPos;
						Game.Player.Character.Heading = LastSeatHeading;
						vector = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.4f, -1f));
						vector = new Vector3(vector.X, vector.Y, vector.Z - 0.5f);
					}
					if (JacuzziAnimDict == 4)
					{
						vector = Game.Player.Character.Position;
					}
					if (JacuzziAnimDict == 5)
					{
						vector = Game.Player.Character.Position;
					}
					float heading = Game.Player.Character.Heading;
					string dict2 = "anim@amb@yacht@jacuzzi@seated@female@variation_0" + JacuzziAnimDict + "@";
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector.X, vector.Y, vector.Z, 0.0, 0.0, heading, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict2), text, 16f, -16f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, text, LoadDict(dict2), 16f, -16f, 0, 1148846080);
				}
				Class1.DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to exit jacuzzi.");
				if (Game.IsControlJustPressed(Control.Context))
				{
					PlayJacuzziIdles = false;
					Vector3 vector2 = Game.Player.Character.Position;
					if (JacuzziAnimDict == 1)
					{
						Game.Player.Character.Position = LastSeatPos;
						Game.Player.Character.Heading = LastSeatHeading;
						vector2 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.4f, -1f));
						vector2 = new Vector3(vector2.X, vector2.Y, vector2.Z - 0.5f);
					}
					if (JacuzziAnimDict == 2)
					{
						Game.Player.Character.Position = LastSeatPos;
						Game.Player.Character.Heading = LastSeatHeading;
						vector2 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.4f, -1f));
						vector2 = new Vector3(vector2.X, vector2.Y, vector2.Z - 0.5f);
					}
					if (JacuzziAnimDict == 3)
					{
						Game.Player.Character.Position = LastSeatPos;
						Game.Player.Character.Heading = LastSeatHeading;
						vector2 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.4f, -1f));
						vector2 = new Vector3(vector2.X, vector2.Y, vector2.Z - 0.5f);
					}
					if (JacuzziAnimDict == 4)
					{
						vector2 = Game.Player.Character.Position;
					}
					if (JacuzziAnimDict == 5)
					{
						vector2 = Game.Player.Character.Position;
					}
					float heading2 = Game.Player.Character.Heading;
					string dict3 = "anim@amb@yacht@jacuzzi@seated@female@variation_0" + JacuzziAnimDict + "@";
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, vector2.X, vector2.Y, vector2.Z, 0.0, 0.0, heading2, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict3), "exit", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict(dict3), 16f, -16f, 0, 1148846080);
					Script.Wait(1);
					while (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1065353211)
					{
						Script.Wait(1);
					}
					Game.Player.Character.Task.ClearAllImmediately();
					Game.Player.Character.IsPositionFrozen = false;
					int taskID2 = 0;
					Utils.OpenSequenceTask(&taskID2);
					Function.Call(Hash.TASK_STAND_STILL, 0, 1);
					Utils.TaskPedSlideToCoord(0, new Vector3(JaccuziMiddlePosition.X, JaccuziMiddlePosition.Y, JaccuziMiddlePosition.Z), JaccuziMiddlePosition.W - 180f, 1f);
					Utils.TaskPedSlideToCoord(0, new Vector3(JaccuziEnterPosition.X, JaccuziEnterPosition.Y, JaccuziEnterPosition.Z), JaccuziEnterPosition.W - 180f, 1f);
					Utils.CloseSequenceTask(taskID2);
					Utils.TaskPerformSequence(Game.Player.Character, taskID2);
					Utils.ClearSequenceTask(&taskID2);
					IsInJacuzzi = false;
					while (Class1.GetDistance_VDIST2(Game.Player.Character.Position, new Vector3(JaccuziEnterPosition.X, JaccuziEnterPosition.Y, JaccuziEnterPosition.Z), Class1.GetDistSign.Greater, 1.25f))
					{
						Script.Wait(1);
					}
					ToggleClothes(false);
				}
			}
		}
	}
}
