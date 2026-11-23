using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;

namespace CayoPericoHeistInSP
{
	public class ManageRobberyAnims : Script
	{
		public class WashMachCashGrab
		{
			public int WashMachCase = -1;

			public bool CanGrab = true;

			public Blip WashMachBlip;

			public Camera CutsceneCam;

			public Camera CutsceneCam2;

			public Prop WashMach;

			public Prop WashMachDryerMoney;

			public Prop LooseMoney;

			public Prop Bag;

			public int ReturnValue;

			public int AnimID;

			public float AnimPhase;

			public int clickytimer;

			public int clickyint;

			private const string AnimDict = "anim@scripted@cbr1@ig1_washmach_grab_cash@male@";

			public Vector4 Pos;

			public static void HudandRadar(bool Hud, bool Radar)
			{
				Function.Call(Hash.DISPLAY_HUD, Hud);
				Function.Call(Hash.DISPLAY_RADAR, Radar);
			}

			public static void Hud(bool Hud)
			{
				Function.Call(Hash.DISPLAY_HUD, Hud);
			}

			public static void Radar(bool Radar)
			{
				Function.Call(Hash.DISPLAY_RADAR, Radar);
			}

			public unsafe static void ClearSequenceTask(int* taskID)
			{
				Function.Call(Hash.CLEAR_SEQUENCE_TASK, taskID);
			}

			public static void TaskPerformSequence(Ped ped, int taskID)
			{
				Function.Call(Hash.TASK_PERFORM_SEQUENCE, ped, taskID);
			}

			public static int CreateSynchronizedScene(Vector3 scenePos, Vector3 sceneRot)
			{
				return Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, scenePos.X, scenePos.Y, scenePos.Z, sceneRot.X, sceneRot.Y, sceneRot.Z, 2);
			}

			public static void TaskSynchronizedScene(Ped ped, int sceneID, string animDict, string animName, float blendIn, float blendOut, int flags, int ragdollBlockFlags, float moverBlendDelta, int ikFlags)
			{
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, ped, sceneID, animDict, animName, blendIn, blendOut, flags, ragdollBlockFlags, moverBlendDelta, ikFlags);
			}

			public static bool PlaySynchronizedEntityAnim(Entity ent, int sceneID, string animName, string animDict, float blendInDelta, float blendOutDelta = 8f, int flags = 0, float moverBlendInDelta = 1000f)
			{
				return Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ent, sceneID, animName, animDict, blendInDelta, blendOutDelta, flags, moverBlendInDelta);
			}

			public static void SetSynchronizedSceneLooped(int sceneID, bool toggle)
			{
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, sceneID, toggle);
			}

			public static float GetSynchronizedScenePhase(int sceneID)
			{
				return Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, sceneID);
			}

			public static void TakeOwnershipOfSynchronizedScene(int sceneID)
			{
				Function.Call(Hash.TAKE_OWNERSHIP_OF_SYNCHRONIZED_SCENE, sceneID);
			}

			public static void SetSynchronizedScenePhase(int sceneID, float phase)
			{
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, sceneID, phase);
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

			public unsafe void WashMachMethod(Lootable DryerLoot)
			{
				if (!CanGrab)
				{
					return;
				}
				switch (WashMachCase)
				{
				case 0:
				{
					CleanupWashMach();
					Prop loot = DryerLoot.Loot;
					Pos = new Vector4(loot.Position.X, loot.Position.Y, loot.Position.Z, loot.Heading);
					WashMach = loot;
					WashMach.IsPositionFrozen = true;
					WashMachDryerMoney = World.CreatePropNoOffset(RequestModel("m23_2_prop_m32_money_dry_01a"), new Vector3(Pos.X, Pos.Y, Pos.Z), new Vector3(0f, 0f, Pos.W), false);
					WashMachDryerMoney.AttachTo(WashMach, new Vector3(0f, -0.2f, 0.7f), new Vector3(0f, 0f, 0f));
					WashMachDryerMoney.IsPositionFrozen = true;
					Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, true);
					LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@");
					Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "enter", WashMach.Position.X, WashMach.Position.Y, WashMach.Position.Z, WashMach.Rotation.X, WashMach.Rotation.Y, WashMach.Rotation.Z, 0f, 2);
					Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "enter", WashMach.Position.X, WashMach.Position.Y, WashMach.Position.Z, WashMach.Rotation.X, WashMach.Rotation.Y, WashMach.Rotation.Z, 0f, 2);
					int num = 0;
					Function.Call(Hash.OPEN_SEQUENCE_TASK, &num);
					Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, 0, vector.X, vector.Y, vector.Z, vector2.Z, 8f);
					Function.Call(Hash.CLOSE_SEQUENCE_TASK, num);
					Function.Call(Hash.TASK_PERFORM_SEQUENCE, Game.Player.Character, num);
					Function.Call(Hash.CLEAR_SEQUENCE_TASK, &num);
					Script.Wait(100);
					while (Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Game.Player.Character) != -1)
					{
						Script.Wait(0);
					}
					LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@");
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					HudandRadar(false, false);
					Bag = World.CreateProp(RequestModel("ch_p_m_bag_var02_arm_s"), Game.Player.Character.Position, false, false);
					Bag.IsVisible = false;
					Bag.IsCollisionEnabled = false;
					Bag.IsPositionFrozen = true;
					LooseMoney = World.CreateProp(RequestModel("m23_2_prop_m32_money_loose_01a"), Game.Player.Character.Position, false, false);
					LooseMoney.IsVisible = true;
					LooseMoney.IsCollisionEnabled = false;
					LooseMoney.IsPositionFrozen = true;
					LooseMoney.AttachTo(Game.Player.Character.Bones[Bone.PHRightHand], new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
					AnimID = CreateSynchronizedScene(new Vector3(WashMach.Position.X, WashMach.Position.Y, WashMach.Position.Z), new Vector3(WashMach.Rotation.X, WashMach.Rotation.Y, WashMach.Rotation.Z));
					TaskSynchronizedScene(Game.Player.Character, AnimID, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "enter", 8f, 8f, 0, 0, 8f, 8192);
					PlaySynchronizedEntityAnim(WashMach, AnimID, "enter_dryer", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1f, 1f, 0, 1f);
					PlaySynchronizedEntityAnim(Bag, AnimID, "enter_bag", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000f, 1000f);
					PlaySynchronizedEntityAnim(LooseMoney, AnimID, "enter_dryermoney", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000f, 1000f);
					Bag.IsVisible = true;
					while ((double)GetSynchronizedScenePhase(AnimID) < 0.100000001490116)
					{
						Script.Wait(0);
					}
					SetBagOff();
					while ((double)GetSynchronizedScenePhase(AnimID) < 1.0)
					{
						Script.Wait(0);
					}
					LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@");
					AnimID = CreateSynchronizedScene(new Vector3(WashMach.Position.X, WashMach.Position.Y, WashMach.Position.Z), new Vector3(WashMach.Rotation.X, WashMach.Rotation.Y, WashMach.Rotation.Z));
					TaskSynchronizedScene(Game.Player.Character, AnimID, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "grab", 8f, 8f, 0, 0, 8f, 8192);
					PlaySynchronizedEntityAnim(WashMach, AnimID, "grab_dryer", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1f, 1f, 0, 1f);
					PlaySynchronizedEntityAnim(Bag, AnimID, "grab_bag", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000f, 1000f);
					PlaySynchronizedEntityAnim(LooseMoney, AnimID, "grab_dryermoney", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000f, 1000f);
					WashMachCase = 1;
					break;
				}
				case 1:
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						DisplayHelpTextThisFrame("Repeatedly tap ~INPUT_ATTACK~ to quickly grab the cash.");
					}
					else
					{
						DisplayHelpTextThisFrame("Repeatedly tap ~INPUT_FRONTEND_ACCEPT~ to quickly grab the cash.");
					}
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "grab", 3))
					{
						AnimID = CreateSynchronizedScene(new Vector3(WashMach.Position.X, WashMach.Position.Y, WashMach.Position.Z), new Vector3(WashMach.Rotation.X, WashMach.Rotation.Y, WashMach.Rotation.Z));
						TaskSynchronizedScene(Game.Player.Character, AnimID, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "grab", 8f, 8f, 0, 0, 8f, 8192);
						PlaySynchronizedEntityAnim(WashMach, AnimID, "grab_dryer", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1f, 1f, 0, 1f);
						PlaySynchronizedEntityAnim(Bag, AnimID, "grab_bag", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000f, 1000f);
						PlaySynchronizedEntityAnim(LooseMoney, AnimID, "grab_dryermoney", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000f, 1000f);
					}
					if (Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "grab", 3))
					{
						if (clickyint < 7)
						{
							if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
							{
								if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237))
								{
									clickyint++;
									clickytimer = Game.GameTime + 500;
								}
							}
							else if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201))
							{
								clickyint++;
								clickytimer = Game.GameTime + 500;
							}
						}
						if (clickyint > 0 && Game.GameTime > clickytimer)
						{
							clickyint--;
							clickytimer = Game.GameTime + 500;
						}
						switch (clickyint)
						{
						case 0:
							if ((double)Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, AnimID) != 0.550000011920929)
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, AnimID, 0.55);
							}
							break;
						case 1:
							if ((double)Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, AnimID) != 0.600000023841858)
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, AnimID, 0.6);
							}
							break;
						case 2:
							if ((double)Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, AnimID) != 0.75)
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, AnimID, 0.75);
							}
							break;
						case 3:
							if ((double)Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, AnimID) != 0.899999976158142)
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, AnimID, 0.9);
							}
							break;
						case 4:
							if ((double)Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, AnimID) != 1.0)
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, AnimID, 1.0);
							}
							break;
						case 5:
							if ((double)Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, AnimID) != 1.04999995231628)
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, AnimID, 1.05);
							}
							break;
						case 6:
							if ((double)Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, AnimID) != 1.12000000476837)
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, AnimID, 1.12);
							}
							break;
						case 7:
							if ((double)Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_RATE, AnimID) != 1.32000005245209)
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, AnimID, 1.32);
							}
							break;
						}
					}
					if ((double)GetSynchronizedScenePhase(AnimID) >= 1.0)
					{
						WashMachDryerMoney.Delete();
						WashMachDryerMoney = null;
						AnimID = CreateSynchronizedScene(new Vector3(WashMach.Position.X, WashMach.Position.Y, WashMach.Position.Z), new Vector3(WashMach.Rotation.X, WashMach.Rotation.Y, WashMach.Rotation.Z));
						TaskSynchronizedScene(Game.Player.Character, AnimID, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "exit", 8f, 8f, 0, 0, 8f, 8192);
						PlaySynchronizedEntityAnim(WashMach, AnimID, "exit_dryer", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1f, 1f, 0, 1f);
						PlaySynchronizedEntityAnim(Bag, AnimID, "exit_bag", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000f, 1000f);
						PlaySynchronizedEntityAnim(LooseMoney, AnimID, "exit_dryermoney", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000f, 1000f);
						GameplayCamera.RelativeHeading = Game.Player.Character.Heading - Game.Player.Character.Heading;
						GameplayCamera.RelativePitch = 0f;
						Script.Wait(50);
						while ((double)GetSynchronizedScenePhase(AnimID) < 1.0)
						{
							Script.Wait(0);
						}
						while (WashMach == null)
						{
							Script.Wait(0);
						}
						WashMach.IsPositionFrozen = true;
						LooseMoney.Delete();
						SetBagOn();
						Bag.Delete();
						TakeOwnershipOfSynchronizedScene(AnimID);
						AnimID = 0;
						if (WashMachBlip != null)
						{
							WashMachBlip.Delete();
							WashMachBlip = null;
						}
						Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, false);
						Game.Player.Character.Task.ClearAll();
						HudandRadar(true, true);
						CanGrab = false;
						WashMachCase = -1;
						DryerLoot.GotValuables = true;
						CleanupWashMach();
						PlayingAnim = false;
					}
					break;
				}
			}

			public void CleanupWashMach()
			{
				if (WashMachDryerMoney != null)
				{
					WashMachDryerMoney.Delete();
				}
				if (LooseMoney != null)
				{
					LooseMoney.Delete();
				}
				if (Bag != null)
				{
					Bag.Delete();
				}
			}
		}

		public class Lootable
		{
			public CayoPericoHeist.LootType LootModel { get; set; }

			public Prop FakeCabinet { get; set; }

			public Prop Cabinet { get; set; }

			public Prop GlassCut { get; set; }

			public Prop GlassDisplayCase { get; set; }

			public List<Prop> SecondaryLoot { get; set; } = new List<Prop>();


			public bool GotValuables { get; set; }

			public LootType LootType { get; set; }

			public bool IsRobbed { get; set; }

			public Prop Loot { get; set; }

			public Prop Holder { get; set; }

			public Blip LootBlip { get; set; }

			public Vector3 Position => Loot.Position;

			public Vector3 Rotation => Loot.Rotation;

			public Lootable()
			{
			}

			public Lootable(LootType T, Prop L, Blip B, bool R)
			{
				LootType = T;
				Loot = L;
				LootBlip = B;
				IsRobbed = R;
			}

			public Lootable(LootType T, Prop L, Prop H, Blip B, bool R)
			{
				LootType = T;
				Loot = L;
				Holder = H;
				LootBlip = B;
				IsRobbed = R;
			}

			public Lootable(LootType T, Prop L, List<Prop> GlassCutProps, Blip B, bool R)
			{
				LootType = T;
				Loot = L;
				SecondaryLoot = GlassCutProps;
				LootBlip = B;
				IsRobbed = R;
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
						Entity = World.CreatePed(RequestModel(Hash), Position, Rotation.Z);
					}
					else
					{
						Entity = World.CreatePed(RequestModel(Inputtedhash, ObjectType.Ped), Position, Rotation.Z);
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
						Entity = World.CreateVehicle(RequestModel(Hash), Position, Rotation.Z);
					}
					else
					{
						Entity = World.CreateVehicle(RequestModel(Inputtedhash, ObjectType.Vehicle), Position, Rotation.Z);
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
						Entity = World.CreateProp(RequestModel(Hash), Position, Rotation, true, false);
					}
					else
					{
						Entity = World.CreateProp(RequestModel(Inputtedhash, ObjectType.Prop), Position, Rotation, true, false);
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
						Entity = World.CreatePed(RequestModel(Hash), Position, Rotation.Z);
					}
					else
					{
						Entity = World.CreatePed(RequestModel(Inputtedhash, ObjectType.Ped), Position, Rotation.Z);
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
						Entity = World.CreateVehicle(RequestModel(Hash), Position, Rotation.Z);
					}
					else
					{
						Entity = World.CreateVehicle(RequestModel(Inputtedhash, ObjectType.Vehicle), Position, Rotation.Z);
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
						Entity = World.CreateProp(RequestModel(Hash), Position, Rotation, true, false);
					}
					else
					{
						Entity = World.CreateProp(RequestModel(Inputtedhash, ObjectType.Prop), Position, Rotation, true, false);
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
			Painting,
			CashDryer,
			XM3_Safe,
			M32_Safe,
			M42_JewelCabinet,
			M42_ServerStack,
			SafetyDepositBoxes_XM3
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

		public class WoodenContainer
		{
			public enum Loot
			{
				Gold
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

		public class GlassCabinetProps
		{
			public bool GotValuables { get; set; }

			public Prop FakeCabinet { get; set; }

			public Prop Cabinet { get; set; }

			public Prop GlassCut { get; set; }

			public List<Prop> Valuables { get; set; }

			public GlassCabinetProps()
			{
			}

			public GlassCabinetProps(Prop C, List<Prop> V)
			{
				GotValuables = false;
				Cabinet = C;
				Valuables = V;
			}
		}

		public enum AOSCabinetType
		{
			Idol,
			Egg,
			Statue,
			Jewellery
		}

		public static WashMachCashGrab CurrentWashDryer;

		public static bool Allow_Get_Keycard_From_SDB_MP2023_2;

		public static bool Has_Got_Keycard_From_SDB_MP2023_2;

		public static int Get_Keycard_From_SDB_MP2023_2_Chance = 40;

		public static int Get_Keycard_From_SDB_MP2023_2_Chance_Increase = 5;

		public static bool FreelookCameraOn;

		public static float _radius = 1.35f;

		public static float _yawDeg = 0f;

		public static float _pitchDeg = 15f;

		private const float MinRadius = 1.5f;

		private const float MaxRadius = 60f;

		private const float MinPitch = -85f;

		private const float MaxPitch = 85f;

		public static float MouseYawSpeed = 0.05f;

		public static float MousePitchSpeed = 0.05f;

		public static float ZoomSpeed = 0.75f;

		private const float KeyYawSpeed = 1.25f;

		private const float KeyPitchSpeed = 1f;

		public static Vector3 _target;

		public static bool Give_Cash_Package_On_Crack_MP2023_2_Safe = true;

		public static bool ShowAiLootingDebug;

		public static bool ShowCrackSafeMSGsDebug;

		public static int ValuableLootLeft;

		public static int DrillSound;

		public static bool IsDrillSoundPlaying;

		public static List<Prop> JewelCases_Smashed = new List<Prop>();

		public static List<MissionOjects> SpawnedSilverCrates = new List<MissionOjects>();

		public static int IncreasePayPerFrames = 120;

		public static bool PlayedFirstAnim;

		public static Vector3 VaultPos = new Vector3(805.6f, -3000.6f, -69.6f);

		public static Prop CurrentPaintingHolder;

		public static int CurrentPaitingID;

		public static Prop CurrentPainting;

		public static bool CanNoLongerOverideCommands;

		public static int VaultType = 1;

		public static FiringPattern RobberAFP = FiringPattern.BurstFire;

		public static FiringPattern RobberBFP = FiringPattern.BurstFire;

		public static FiringPattern RobberCFP = FiringPattern.BurstFire;

		public static int RobberAtimer = 0;

		public static int RobberBtimer = 0;

		public static int RobberCtimer = 0;

		public static List<Lootable> Crate_Valuables = new List<Lootable>();

		public static int CuttingStage;

		public static float nextTick;

		public static List<string> PaintingIDs = new List<string> { "ch_prop_vault_painting_01j", "ch_prop_vault_painting_01i", "ch_prop_vault_painting_01h", "ch_prop_vault_painting_01g", "ch_prop_vault_painting_01f", "ch_prop_vault_painting_01e", "ch_prop_vault_painting_01d", "ch_prop_vault_painting_01c", "ch_prop_vault_painting_01b", "ch_prop_vault_painting_01a" };

		public static bool PlayerGotJewels;

		public static int PriorityLoot = 1;

		public static bool ShowingDisplayMSG;

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

		public static bool SetRob = false;

		public static float Heist_Average_Pay = 1000000f;

		public static int CurrentPay;

		public static int Scene1;

		public static Prop bagProp;

		public static Camera AnimatedCamera;

		public static int PlayerGrabbingStage;

		public static Prop _cashGrabTray1;

		public static Prop MoneyProp;

		public static bool PlayingAnim;

		public static bool CreatedCrates = true;

		public static LootType CurrentType;

		public static float CashCount;

		public static int RobTimer;

		public static bool PlayerPlayingFinalAnim;

		public static bool PlayerStartedCuttingAnim;

		public static bool PlayerFirstAnim;

		public static bool RobberAFirstAnim;

		public static bool RobberBFirstAnim;

		public static bool RobberCFirstAnim;

		public static Prop PlayerRobbingProp;

		public static Prop PlayerCurrentPaintingNew;

		public static Prop RobberA_CurrentPaintingNew;

		public static Prop RobberB_CurrentPaintingNew;

		public static Prop RobberC_CurrentPaintingNew;

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

		public static bool RobberANearLoot;

		public static bool RobberBNearLoot;

		public static bool RobberCNearLoot;

		public static bool PlayingCashGrabIdle;

		public static float CashGrabLastSavePlace;

		public static int IdleScene;

		public static Prop PlayerLastLootProp;

		public static int CurrentLootTarget;

		public static Ped RobberATarget;

		public static Ped RobberBTarget;

		public static Ped RobberCTarget;

		public static Prop RobberA_bagProp;

		public static Prop RobberB_bagProp;

		public static Prop RobberC_bagProp;

		public static Prop RobberA_HoldingLootProp;

		public static Prop RobberB_HoldingLootProp;

		public static Prop RobberC_HoldingLootProp;

		public static int RobberA_CashGrabTimer;

		public static int RobberB_CashGrabTimer;

		public static int RobberC_CashGrabTimer;

		public static Vector3 RobberA_RobberMarker = new Vector3(0f, 0f, 0f);

		public static float RobberA_DistanceToTarget;

		public static int RobberA_LastTargetId;

		public static Vector3 RobberB_RobberMarker = new Vector3(0f, 0f, 0f);

		public static float RobberB_DistanceToTarget;

		public static int RobberB_LastTargetId;

		public static Vector3 RobberC_RobberMarker = new Vector3(0f, 0f, 0f);

		public static float RobberC_DistanceToTarget;

		public static int RobberC_LastTargetId;

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

		public static bool InitialAnimDone = false;

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

		public static Prop RobberACurrentPaitingHolder;

		public static Prop RobberBCurrentPaitingHolder;

		public static Prop RobberCCurrentPaitingHolder;

		public static int RobberACurrentID;

		public static int RobberBCurrentID;

		public static int RobberCCurrentID;

		public static int PlayerCurrentID;

		public static Ped RobberA;

		public static Ped RobberB;

		public static Ped RobberC;

		public static string PlayerCurrentSDDict;

		public static int PlayerCurrentLockbox;

		public static int PlayerCurrentLockboxStage;

		public static Prop PlayerCurrentLockBoxProp;

		public static string PlayerCurrentLockBoxType;

		public static Prop PlayerCurrentLootBagProp;

		public static float PlayerCurrentLootBoxRotation;

		public static float PlayerGrabLootDelayStop;

		public static string RobberACurrentSDDict;

		public static int RobberACurrentLockbox;

		public static int RobberACurrentLockboxStage;

		public static Prop RobberACurrentLockBoxProp;

		public static string RobberACurrentLockBoxType;

		public static Prop RobberACurrentLootBagProp;

		public static float RobberACurrentLootBoxRotation;

		public static string RobberBCurrentSDDict;

		public static int RobberBCurrentLockbox;

		public static int RobberBCurrentLockboxStage;

		public static Prop RobberBCurrentLockBoxProp;

		public static string RobberBCurrentLockBoxType;

		public static Prop RobberBCurrentLootBagProp;

		public static float RobberBCurrentLootBoxRotation;

		public static string RobberCCurrentSDDict;

		public static int RobberCCurrentLockbox;

		public static int RobberCCurrentLockboxStage;

		public static Prop RobberCCurrentLockBoxProp;

		public static string RobberCCurrentLockBoxType;

		public static Prop RobberCCurrentLootBagProp;

		public static float RobberCCurrentLootBoxRotation;

		public static bool RobberAInitialAnimDone = false;

		public static bool RobberBInitialAnimDone = false;

		public static bool RobberCInitialAnimDone = false;

		public static bool RobberASetRob = false;

		public static bool RobberBSetRob = false;

		public static bool RobberCSetRob = false;

		public static WoodenContainer CurrentWoodenContainer;

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

		public static bool UseSetSafeCode;

		public static int CodeNumber1;

		public static int CodeNumber2;

		public static int CodeNumber3;

		public static int iLocal_26708;

		public static int Local_26709f_9;

		public static bool OverridePay;

		public static int jewelsmashint = 0;

		public static int jewelsmashint2 = 0;

		public static int jewelsmashint3 = 0;

		public static int jewelsmashint4 = 0;

		public static int RobberA_jewelsmashint = 0;

		public static int RobberA_jewelsmashint2 = 0;

		public static int RobberA_jewelsmashint3 = 0;

		public static int RobberA_jewelsmashint4 = 0;

		public static int RobberB_jewelsmashint = 0;

		public static int RobberB_jewelsmashint2 = 0;

		public static int RobberB_jewelsmashint3 = 0;

		public static int RobberB_jewelsmashint4 = 0;

		public static int RobberC_jewelsmashint = 0;

		public static int RobberC_jewelsmashint2 = 0;

		public static int RobberC_jewelsmashint3 = 0;

		public static int RobberC_jewelsmashint4 = 0;

		public static bool RobberA_JustFinishedSmashingCabinet;

		public static int RobberA_ResetSmashTimer;

		public static bool RobberB_JustFinishedSmashingCabinet;

		public static int RobberB_ResetSmashTimer;

		public static bool RobberC_JustFinishedSmashingCabinet;

		public static int RobberC_ResetSmashTimer;

		public static int JewelStoreSyncScene;

		public static int RobberA_JewelStoreSyncScene;

		public static int RobberB_JewelStoreSyncScene;

		public static int RobberC_JewelStoreSyncScene;

		public static int RobberA_extra;

		public static int RobberA_extra2;

		public static int RobberB_extra;

		public static int RobberB_extra2;

		public static int RobberC_extra;

		public static int RobberC_extra2;

		public static int extra;

		public static int extra2;

		public static int smashparticlefx;

		public static Prop brokenjewelglass;

		public static Prop glass1;

		public static Prop glass2;

		public static Prop glass3;

		public static Prop glass4;

		public static Prop glassroot;

		public static Prop glassrootb;

		public static List<Prop> jewelbrokenglasslist = new List<Prop>();

		public static List<Prop> jewelrootlist = new List<Prop>();

		public static List<Prop> jewelrootBlist = new List<Prop>();

		public static Vector3 jewelsmashpos;

		public static float jewelsmashrot;

		public static int unstucktimer;

		public static WeaponHash guns;

		public static int GetNearestLootPlayer;

		public static int NearestCrateId = -1;

		public static int mainindex = 0;

		public static bool mainbool = true;

		public static bool somebool = false;

		public static int DrillSyncScene;

		public static int takethingint2 = 680;

		public static int takethingint5 = 680;

		public static int takethingint6 = 680;

		public static int someint;

		public static int myint;

		public static int timer1 = 0;

		public static Prop drillmodelprop2;

		public static Prop bagprop2;

		public static Prop Moneymodelprop2;

		public static Prop depobox1;

		public static Prop depobox2;

		public static Prop depobox3;

		public static Prop depobox4;

		public static Prop depobox5;

		public static List<Prop> depositlist = new List<Prop>();

		public static Prop sounddissapearProp;

		public static float DrillSpeed = 1.5f;

		public static int TimeBetweenIsDrillingCheck;

		public static int TimeToDrillOpenDepositBox = 50;

		public static Prop PlayerLastSafeDepositBox;

		public static int GetNearLootCheck;

		public static bool ShowChevronMarkersOnLoot;

		public static bool ShowLootTypeText;

		public static List<Prop> ArtFrames = new List<Prop>();

		public static float InitialLoot;

		public static float LootLeft;

		public static bool RobberAInPlay;

		public static bool RobberBInPlay;

		public static bool RobberCInPlay;

		public static string DebugName = "CINNBARGNU";

		public static bool CreatedPathereStatue;

		public static Vector3 PanthereSpawn;

		public static float PanthereHeading;

		public static Prop PanthereCaseB;

		public static Prop PanthereCaseA;

		public static Prop PanthereStatue;

		public bool GotnearestCokeStack;

		public static List<Prop> LootFound = new List<Prop>();

		public static List<Prop> SecondaryProps = new List<Prop>();

		public static List<int> LootPropsHashes = new List<int>
		{
			Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_prtmachine_dryer_op"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01b"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01c"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01d"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01e"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01f"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01g"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01h"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01b"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01c"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01d"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01e"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01f"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01g"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01h"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01b"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01b"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"),
			Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_cash_trolly_01"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01b"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01b"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"),
			Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_coke_trolly"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_weed_stack_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_vancrate_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start"),
			Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"),
			Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start"),
			Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01b"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01c"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01d"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01e"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01f"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01g"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01h"),
			Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01i"),
			Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_glass_disp_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_safe_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "xm3_Prop_xm3_safe_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_1_prop_m41_painting_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_1_prop_m41_painting_01b"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_1_prop_m41_painting_01c"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_1_prop_m41_painting_01d"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_CokeCabinet_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_CokeCabinet_02a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_book_stack_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_harddrive_stack_01a"),
			Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_server_stack_01a")
		};

		public int CheckLootLeft;

		public int CheckNearbyProps;

		public static bool ShowSecondarySafeUI;

		public static bool PaintingCut_Disable_Require_Input;

		public static bool Cannot_Steal_Loot;

		public static int CutIndex = 0;

		public static int CutEnter;

		public static int CutLop;

		public static int CutIdle;

		public static int CutHeat;

		public static int CutLev;

		public static int CutExit;

		public static float PlasSpeed;

		public static float HeatFloat;

		public static int PlasTimer = 0;

		public static int TapTimer = 0;

		public static int PlasCheck = 0;

		public static int PlasEffect;

		public static int OverheatEffect;

		public static int SoundId;

		public static bool Cutting = false;

		public static Camera PlasCam;

		public static float barperc;

		public static bool CAN_DRAW_HEAT;

		public static bool CutGlassCabinet;

		public static int RobCabinetStage = -1;

		public static int RobCabinetScene;

		public static AOSCabinetType AosCabinetType;

		public static Prop RobCabinetLoot;

		public static int RobCabinetPTFX;

		private bool FoundServerLoot;

		public static bool SpawnedTestLoot;

		public static float Clamp(float v, float min, float max)
		{
			if (!(v < min))
			{
				if (!(v > max))
				{
					return v;
				}
				return max;
			}
			return min;
		}

		public static void OrbitCameraControls(Vector3 Target)
		{
			Game.DisableControlThisFrame(Control.Phone);
			Game.DisableControlThisFrame(Control.Attack);
			Game.DisableControlThisFrame(Control.Attack2);
			Game.DisableControlThisFrame(Control.MeleeAttack1);
			Game.DisableControlThisFrame(Control.MeleeAttack2);
			Game.DisableControlThisFrame(Control.Phone);
			Game.DisableControlThisFrame(Control.SelectWeapon);
			_target = Target;
			bool flag = Game.IsControlPressed(Control.Aim);
			flag = true;
			float num = Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 0, 1);
			float num2 = Function.Call<float>(Hash.GET_DISABLED_CONTROL_NORMAL, 0, 2);
			if (flag)
			{
				_yawDeg += num * 180f * MouseYawSpeed;
				_pitchDeg -= num2 * 180f * MousePitchSpeed;
			}
			if (Game.LastInputMethod == InputMethod.GamePad)
			{
				MouseYawSpeed = 0.01f;
				MousePitchSpeed = 0.01f;
				ZoomSpeed = 0.75f;
			}
			if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
			{
				MouseYawSpeed = 0.05f;
				MousePitchSpeed = 0.05f;
				ZoomSpeed = 0.75f;
			}
			if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
			{
				if (Game.IsControlPressed(Control.Cover))
				{
					_yawDeg -= 1.25f;
				}
				if (Game.IsControlPressed(Control.Context))
				{
					_yawDeg += 1.25f;
				}
				if (Game.IsControlPressed(Control.Reload))
				{
					_pitchDeg += 1f;
				}
				if (Game.IsControlPressed(Control.VehicleExit))
				{
					_pitchDeg -= 1f;
				}
			}
			if (Game.LastInputMethod == InputMethod.GamePad)
			{
				if (Game.IsControlPressed(Control.MoveLeftOnly))
				{
					_yawDeg -= 1.25f;
				}
				if (Game.IsControlPressed(Control.MoveRightOnly))
				{
					_yawDeg += 1.25f;
				}
				if (Game.IsControlPressed(Control.MoveUpOnly))
				{
					_pitchDeg += 1f;
				}
				if (Game.IsControlPressed(Control.MoveDownOnly))
				{
					_pitchDeg -= 1f;
				}
			}
			_radius = Clamp(_radius, 1.5f, 60f);
			_pitchDeg = Clamp(_pitchDeg, -85f, 85f);
			_yawDeg = NormalizeAngle(_yawDeg);
		}

		public static void UpdateCamera(Camera MainCam, Vector3 Target)
		{
			OrbitCameraControls(Target);
			if (MainCam != null)
			{
				float num = DegToRad(_yawDeg);
				float num2 = DegToRad(_pitchDeg);
				float num3 = (float)Math.Cos(num2);
				Vector3 vector = new Vector3(_radius * num3 * (float)Math.Cos(num), _radius * num3 * (float)Math.Sin(num), _radius * (float)Math.Sin(num2));
				Vector3 vector3 = (MainCam.Position = _target + vector);
				MainCam.PointAt(_target);
			}
		}

		private static float DegToRad(float deg)
		{
			return deg * MathF.PI / 180f;
		}

		private static float NormalizeAngle(float deg)
		{
			deg %= 360f;
			if (deg < 0f)
			{
				deg += 360f;
			}
			return deg;
		}

		private static float HeadingFromTo(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			return NormalizeAngle((float)(Math.Atan2(vector.Y, vector.X) * 180.0 / Math.PI));
		}

		public static void ResetKeycardVars()
		{
			Allow_Get_Keycard_From_SDB_MP2023_2 = true;
			Has_Got_Keycard_From_SDB_MP2023_2 = false;
			Get_Keycard_From_SDB_MP2023_2_Chance = 40;
			Get_Keycard_From_SDB_MP2023_2_Chance_Increase = 10;
		}

		public static bool CanGetKeycard()
		{
			bool result = false;
			if (Has_Got_Keycard_From_SDB_MP2023_2)
			{
				result = false;
			}
			if (!Has_Got_Keycard_From_SDB_MP2023_2 && Allow_Get_Keycard_From_SDB_MP2023_2)
			{
				int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num < Get_Keycard_From_SDB_MP2023_2_Chance)
				{
					result = true;
					Has_Got_Keycard_From_SDB_MP2023_2 = true;
				}
				else
				{
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
					result = false;
				}
			}
			if (!Allow_Get_Keycard_From_SDB_MP2023_2)
			{
				result = false;
			}
			return result;
		}

		public static void SetBagOn()
		{
			if (!File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
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
			}
			if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
				Assembly assembly = Assembly.LoadFrom("scripts//PlayableOnlineCharacter.dll");
				Type type = assembly.GetType("PlayableOnlineCharacter.Class1");
				MethodInfo method = type.GetMethod("ApplyDuffelBag");
				method.Invoke(method, null);
			}
		}

		public static void SetBagOff()
		{
			if (!File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
				if (Game.Player.Character.Model == PedHash.Trevor)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
				}
				if (Game.Player.Character.Model == PedHash.Michael)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
				}
				if (Game.Player.Character.Model == PedHash.Franklin)
				{
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 9, 0, 0, 1);
				}
			}
			if (File.Exists("scripts//PlayableOnlineCharacter.dll"))
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, Game.Player.Character, 5, 0, 0, 1);
			}
		}

		public static void DeleteCamera()
		{
			if (AnimatedCamera != null)
			{
				AnimatedCamera.IsActive = false;
				AnimatedCamera.Delete();
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 2000, 1, 0);
				FreelookCameraOn = false;
			}
		}

		public static void CreateCamera(Prop P, LootType Type)
		{
			if (AnimatedCamera != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				AnimatedCamera.IsActive = false;
				AnimatedCamera.Delete();
			}
			AnimatedCamera = Camera.Create("DEFAULT_SCRIPTED_CAMERA", P.GetOffsetPosition(new Vector3(0f, -0.25f, 1.25f)), P.Rotation - new Vector3(0f, 0f, P.Rotation.Z), GameplayCamera.FieldOfView);
			if (Type == LootType.Trolly_Cash || Type == LootType.Trolly_Diamonds || Type == LootType.Trolly_Coke || Type == LootType.Trolly_Gold)
			{
				AnimatedCamera.Position = P.GetOffsetPosition(new Vector3(0.65f, 0.5f, 1.6f));
				AnimatedCamera.PointAt(P);
			}
			if (Type == LootType.CP_Cash || Type == LootType.CP_Gold || Type == LootType.M42_ServerStack)
			{
				AnimatedCamera.Position = P.GetOffsetPosition(new Vector3(0f, 1f, 0.45f));
				FreelookCameraOn = true;
			}
			AnimatedCamera.IsActive = true;
			Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 2000, 1, 0);
		}

		public static Lootable ReturnLootable(Prop E)
		{
			Lootable result = null;
			for (int i = 0; i < Crate_Valuables.Count; i++)
			{
				if (Crate_Valuables[i].Loot != null && Crate_Valuables[i].Loot == E)
				{
					result = Crate_Valuables[i];
				}
			}
			return result;
		}

		public static bool IsObjectLooted(Prop E)
		{
			bool result = false;
			for (int i = 0; i < Crate_Valuables.Count; i++)
			{
				if (Crate_Valuables[i].Loot != null && Crate_Valuables[i].Loot == E && Crate_Valuables[i].IsRobbed)
				{
					result = true;
				}
			}
			return result;
		}

		public static void ATTACH_ENTITY_TO_ENTITY(Entity entity1, Entity entity2, int boneIndex, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, bool p9, bool useSoftPinning, bool collision, bool isPed, int vertexIndex, bool fixedRot)
		{
			Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, entity1, entity2, boneIndex, xPos, yPos, zPos, xRot, yRot, zRot, p9, useSoftPinning, collision, isPed, vertexIndex, fixedRot);
		}

		public static void DisplayHelpTextThisFrameNoSound(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			ShowingDisplayMSG = true;
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
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

		private static float progresswidth(float percent)
		{
			return 0.08f * percent;
		}

		private static float progressxcoord(float percent)
		{
			float num = 0.04f * percent;
			return 0.9f + num;
		}

		private static void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
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

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
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

		public static float increaseCurrentPay(int min, int max)
		{
			float num = 0f;
			Random random = new Random();
			if (min < max)
			{
				num = random.Next(min, max);
				CurrentPay += (int)num;
			}
			if (min > max)
			{
				num = random.Next(max, min);
				CurrentPay += (int)num;
			}
			return num;
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			float num = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			num = (float)Math.Sqrt(num);
			return num;
		}

		public static void AddCash(Ped Ped, int min, int max)
		{
			Random random = new Random();
			float num = increaseCurrentPay(min, max);
			CurrentPay += (int)num;
		}

		public static void CalculatePay(Ped ped, LootType Type)
		{
			if (Type == LootType.Trolly_Cash)
			{
				Random random = new Random();
				AddCash(ped, (int)(Heist_Average_Pay / 425f), (int)(Heist_Average_Pay / 325f));
				bool flag = ped == Game.Player.Character;
			}
			if (Type == LootType.Trolly_Gold)
			{
				Random random2 = new Random();
				AddCash(ped, (int)(Heist_Average_Pay / 425f), (int)(Heist_Average_Pay / 400f));
				if (ped == Game.Player.Character)
				{
					Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
					Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
				}
			}
			if (Type == LootType.Trolly_Diamonds)
			{
				Random random3 = new Random();
				AddCash(ped, (int)(Heist_Average_Pay / 420f), (int)(Heist_Average_Pay / 480f));
				if (ped == Game.Player.Character)
				{
					Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
					Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
				}
			}
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

		public static void func_4443()
		{
			BEGIN_SCALEFORM_MOVIE_METHOD(Local_26709f_9, "SET_CURSOR_POSITION");
			Add_param_int(SelectedNum);
			END_SCALEFORM_MOVIE_METHOD();
		}

		public static void func_4437(uint iParam0, string SafeRT)
		{
			if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, SafeRT))
			{
				Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, SafeRT, false);
				return;
			}
			Function.Call(Hash.LINK_NAMED_RENDERTARGET, iParam0);
			if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, iParam0))
			{
				Function.Call(Hash.LINK_NAMED_RENDERTARGET, iParam0);
			}
			else
			{
				Local_26709f_8 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, SafeRT);
			}
		}

		public static int func_4438()
		{
			Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
			{
				Script.Wait(1);
				Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
				GTA.UI.Screen.ShowSubtitle("Loading Scaleform DIGITAL_SAFE_DISPLAY");
			}
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

		public static void CheckCrate(Ped Ped, Lootable Loot, Vector3 Pos, Vector3 Rot, LootType type)
		{
			if (Ped == Game.Player.Character)
			{
				if (CurrentType != LootType.WoodCrate && CurrentType != LootType.CP_Safe && CurrentType != LootType.M32_Safe && CurrentType != LootType.XM3_Safe && CurrentType != LootType.JewelStoreCabinet && CurrentType != LootType.Painting)
				{
					if (CurrentType == LootType.CashDryer)
					{
						CurrentWashDryer.WashMachMethod(Loot);
					}
					if (CurrentType == LootType.CP_Cash || CurrentType == LootType.CP_Gold || CurrentType == LootType.M42_ServerStack)
					{
						if (FreelookCameraOn)
						{
							UpdateCamera(AnimatedCamera, Game.Player.Character.Position + new Vector3(0f, 0f, 0.25f));
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							if (CurrentType == LootType.CP_Cash && !PlayerFirstAnim)
							{
								PlayerFirstAnim = true;
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab_cash", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_cash", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 16f, -16f, 0, 1148846080);
								InitialAnimDone = true;
							}
							if (CurrentType == LootType.CP_Gold && !PlayerFirstAnim)
							{
								PlayerFirstAnim = true;
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_gold", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 16f, -16f, 0, 1148846080);
								InitialAnimDone = true;
							}
							if (CurrentType == LootType.M42_ServerStack && !PlayerFirstAnim)
							{
								PlayerFirstAnim = true;
								string dict = "anim@scripted@heist@ig1_table_grab@gold@male@";
								if (PlayerRobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
								{
									dict = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
								}
								LoadDict(dict);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, LoadDict(dict), "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_gold", LoadDict(dict), 16f, -16f, 0, 1148846080);
								InitialAnimDone = true;
							}
							if (CurrentType != LootType.CP_Cash && CurrentType != LootType.CP_Gold && CurrentType != LootType.M42_ServerStack)
							{
								InitialAnimDone = true;
							}
						}
					}
					if ((CurrentType == LootType.Trolly_Cash || CurrentType == LootType.Trolly_Gold || CurrentType == LootType.Trolly_Diamonds || CurrentType == LootType.Trolly_Coke) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1024802024)
					{
						InitialAnimDone = true;
					}
					if (InitialAnimDone)
					{
						if (CurrentType != LootType.SafetyDepositBoxes && CurrentType != LootType.SafetyDepositBoxes_XM3)
						{
							if (CurrentType == LootType.Trolly_Cash || CurrentType == LootType.Trolly_Gold || CurrentType == LootType.Trolly_Diamonds || CurrentType == LootType.Trolly_Coke)
							{
								if (MoneyProp != null && GetDistance_VDIST2(MoneyProp.Position, bagProp.Position, "<", 0.2f))
								{
									MoneyProp.IsVisible = false;
								}
								if (MoneyProp != null && GetDistance_VDIST2(MoneyProp.Position, bagProp.Position, ">", 0.5f))
								{
									MoneyProp.IsVisible = true;
								}
								if (PlayerGrabLootDelayStop <= 0f && MoneyProp != null && GetDistance_VDIST2(MoneyProp.Position, bagProp.Position, "<", 0.2f) && !PlayingCashGrabIdle)
								{
									PlayingCashGrabIdle = true;
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 0f);
									CashGrabLastSavePlace = Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1);
									IdleScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerLastLootProp.Position.X, PlayerLastLootProp.Position.Y, PlayerLastLootProp.Position.Z, 0.0, 0.0, PlayerCurrentLootBoxRotation, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, IdleScene, "anim@heists@ornate_bank@grab_cash", "grab_idle", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, IdleScene, "grab_idle", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, IdleScene, "bag_grab_idle", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									MoneyProp.IsVisible = false;
								}
							}
							if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@ornate_bank@grab_cash", "bag_intro", 1) && SetRob)
							{
								if (CurrentType == LootType.Trolly_Cash)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									MoneyProp = World.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), Game.Player.Character.Position, default(Vector3), false, false);
									MoneyProp.IsPositionFrozen = true;
									MoneyProp.IsCollisionEnabled = false;
									MoneyProp.IsInvincible = true;
									PlayingAnim = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									SetRob = false;
									InitialAnimDone = false;
								}
								if (CurrentType == LootType.Trolly_Gold)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									MoneyProp = World.CreateProp(RequestModel("ch_prop_gold_bar_01a"), Game.Player.Character.Position, default(Vector3), false, false);
									MoneyProp.IsPositionFrozen = true;
									MoneyProp.IsCollisionEnabled = false;
									MoneyProp.IsInvincible = true;
									PlayingAnim = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									CurrentType = LootType.Trolly_Gold;
									SetRob = false;
									InitialAnimDone = false;
								}
								if (CurrentType == LootType.Trolly_Diamonds)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									MoneyProp = World.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), Game.Player.Character.Position, default(Vector3), false, false);
									MoneyProp.IsPositionFrozen = true;
									MoneyProp.IsCollisionEnabled = false;
									MoneyProp.IsInvincible = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									CurrentType = LootType.Trolly_Diamonds;
									SetRob = false;
									InitialAnimDone = false;
								}
								if (CurrentType == LootType.CP_Cash)
								{
									if (!PlayerWaitingForLastAnim && !PlayerWaitingForLastAnim)
									{
										if (!PlayerFirstAnim)
										{
											PlayerFirstAnim = true;
											InitialAnimDone = true;
										}
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab_cash", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_cash", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 16f, -16f, 0, 1148846080);
										PlayerWaitingForLastAnim = true;
										return;
									}
									if (PlayerWaitingForLastAnim && !PlayerFinishedLastAnim)
									{
										if (RobTimer >= IncreasePayPerFrames)
										{
											RobTimer = 0;
											if (!CayoPericoHeist.DisableUnlimitedLootBag)
											{
												AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 120f), (int)(Heist_Average_Pay / 80f));
											}
										}
										if (RobTimer != IncreasePayPerFrames)
										{
											RobTimer++;
										}
										if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) == 0)
										{
											PlayerFinishedLastAnim = true;
											Rot = Game.Player.Character.Rotation;
											bagProp.IsPositionFrozen = false;
											Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
											return;
										}
									}
									if (PlayerFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
									{
										PlayerFinishedLastAnim = false;
										Crate_Valuables[CurrentLootTarget].GotValuables = true;
										Game.Player.Character.Task.ClearAll();
										if (!CayoPericoHeist.DisableUnlimitedLootBag)
										{
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 120f), (int)(Heist_Average_Pay / 80f));
										}
										else
										{
											CayoPericoHeist.LootType lootModel = Crate_Valuables[CurrentLootTarget].LootModel;
											if (Crate_Valuables[CurrentLootTarget].LootModel == CayoPericoHeist.LootType.Money)
											{
												CurrentPay += 80000;
											}
											if (Crate_Valuables[CurrentLootTarget].LootModel == CayoPericoHeist.LootType.Weed)
											{
												CurrentPay += 132000;
											}
											if (Crate_Valuables[CurrentLootTarget].LootModel == CayoPericoHeist.LootType.Cocaine)
											{
												CurrentPay += 200000;
											}
											if (Crate_Valuables[CurrentLootTarget].LootModel == CayoPericoHeist.LootType.Gold)
											{
												CurrentPay += 330000;
											}
										}
										SetBagOn();
										PlayingAnim = false;
										if (MoneyProp != null)
										{
											MoneyProp.Delete();
											MoneyProp.MarkAsNoLongerNeeded();
											MoneyProp = null;
										}
										DeleteCamera();
										if (bagProp != null)
										{
											bagProp.Delete();
											bagProp.MarkAsNoLongerNeeded();
											bagProp = null;
										}
										return;
									}
								}
								if (CurrentType == LootType.CP_Gold)
								{
									if (!PlayerWaitingForLastAnim && !PlayerWaitingForLastAnim)
									{
										if (!PlayerFirstAnim)
										{
											PlayerFirstAnim = true;
											InitialAnimDone = true;
										}
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_gold", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 16f, -16f, 0, 1148846080);
										PlayerWaitingForLastAnim = true;
										return;
									}
									if (PlayerWaitingForLastAnim && !PlayerFinishedLastAnim)
									{
										if (RobTimer >= IncreasePayPerFrames)
										{
											RobTimer = 0;
											if (!CayoPericoHeist.DisableUnlimitedLootBag)
											{
												AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 80f), (int)(Heist_Average_Pay / 60f));
											}
										}
										if (RobTimer != IncreasePayPerFrames)
										{
											RobTimer++;
										}
										if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) == 0)
										{
											PlayerFinishedLastAnim = true;
											Rot = Game.Player.Character.Rotation;
											Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
											return;
										}
									}
									if (PlayerFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
									{
										PlayerFinishedLastAnim = false;
										Game.Player.Character.Task.ClearAll();
										Crate_Valuables[CurrentLootTarget].GotValuables = true;
										SetBagOn();
										if (!CayoPericoHeist.DisableUnlimitedLootBag)
										{
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 40f));
										}
										else
										{
											CayoPericoHeist.LootType lootModel2 = Crate_Valuables[CurrentLootTarget].LootModel;
											if (Crate_Valuables[CurrentLootTarget].LootModel == CayoPericoHeist.LootType.Money)
											{
												CurrentPay += 80000;
											}
											if (Crate_Valuables[CurrentLootTarget].LootModel == CayoPericoHeist.LootType.Weed)
											{
												CurrentPay += 132000;
											}
											if (Crate_Valuables[CurrentLootTarget].LootModel == CayoPericoHeist.LootType.Cocaine)
											{
												CurrentPay += 200000;
											}
											if (Crate_Valuables[CurrentLootTarget].LootModel == CayoPericoHeist.LootType.Gold)
											{
												CurrentPay += 330000;
											}
										}
										PlayingAnim = false;
										if (MoneyProp != null)
										{
											MoneyProp.Delete();
											MoneyProp.MarkAsNoLongerNeeded();
											MoneyProp = null;
										}
										DeleteCamera();
										if (bagProp != null)
										{
											bagProp.Delete();
											bagProp.MarkAsNoLongerNeeded();
											bagProp = null;
										}
										return;
									}
								}
								if (CurrentType == LootType.M42_ServerStack)
								{
									if (!PlayerWaitingForLastAnim && !PlayerWaitingForLastAnim)
									{
										if (!PlayerFirstAnim)
										{
											PlayerFirstAnim = true;
											InitialAnimDone = true;
										}
										string dict2 = "anim@scripted@heist@ig1_table_grab@gold@male@";
										if (PlayerRobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
										{
											dict2 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
										}
										LoadDict(dict2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict2), "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "grab_bag", LoadDict(dict2), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, LoadDict(dict2), "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_gold", LoadDict(dict2), 16f, -16f, 0, 1148846080);
										PlayerWaitingForLastAnim = true;
										return;
									}
									if (PlayerWaitingForLastAnim && !PlayerFinishedLastAnim)
									{
										if (RobTimer >= IncreasePayPerFrames)
										{
											RobTimer = 0;
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 80f), (int)(Heist_Average_Pay / 60f));
										}
										if (RobTimer != IncreasePayPerFrames)
										{
											RobTimer++;
										}
										if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) == 0)
										{
											PlayerFinishedLastAnim = true;
											Rot = Game.Player.Character.Rotation;
											Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, PlayerRobbingProp.Position.X, PlayerRobbingProp.Position.Y, PlayerRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
											return;
										}
									}
									if (PlayerFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
									{
										PlayerFinishedLastAnim = false;
										Game.Player.Character.Task.ClearAll();
										Crate_Valuables[CurrentLootTarget].GotValuables = true;
										SetBagOn();
										AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 40f));
										PlayingAnim = false;
										if (MoneyProp != null)
										{
											MoneyProp.Delete();
											MoneyProp.MarkAsNoLongerNeeded();
											MoneyProp = null;
										}
										DeleteCamera();
										if (bagProp != null)
										{
											bagProp.Delete();
											bagProp.MarkAsNoLongerNeeded();
											bagProp = null;
										}
										return;
									}
								}
								if (CurrentType == LootType.Trolly_Coke)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									MoneyProp = World.CreateProp(RequestModel("imp_prop_impexp_coke_pile"), Game.Player.Character.Position, default(Vector3), false, false);
									MoneyProp.IsPositionFrozen = true;
									MoneyProp.IsCollisionEnabled = false;
									MoneyProp.IsInvincible = true;
									PlayingAnim = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									SetRob = false;
									InitialAnimDone = false;
								}
								if (CurrentType == LootType.LootBox_Plates)
								{
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1038078278 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1039000000)
									{
										if (!PlayerPlayingFinalAnim)
										{
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 0f);
										}
										if (PlayerPlayingFinalAnim)
										{
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
										}
										DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ To Open the Crate");
										if (Game.IsControlPressed(Control.Attack) && !PlayerPlayingFinalAnim)
										{
											PlayerPlayingFinalAnim = true;
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
										}
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
									{
										Game.Player.Character.Task.ClearAll();
										SetBagOn();
										Crate_Valuables[CurrentLootTarget].GotValuables = true;
										AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 10f), (int)(Heist_Average_Pay / 20f));
										PlayingAnim = false;
										if (MoneyProp != null)
										{
											MoneyProp.Delete();
											MoneyProp.MarkAsNoLongerNeeded();
											MoneyProp = null;
										}
										DeleteCamera();
										if (bagProp != null)
										{
											bagProp.Delete();
											bagProp.MarkAsNoLongerNeeded();
											bagProp = null;
										}
										SetRob = false;
									}
									return;
								}
								if (CurrentType == LootType.LootBox_HDD)
								{
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1038078278 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1039000000)
									{
										if (!PlayerPlayingFinalAnim)
										{
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 0f);
										}
										if (PlayerPlayingFinalAnim)
										{
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
										}
										if (!PlayerPlayingFinalAnim)
										{
											PlayerPlayingFinalAnim = true;
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
										}
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
									{
										Game.Player.Character.Task.ClearAll();
										Crate_Valuables[CurrentLootTarget].GotValuables = true;
										SetBagOn();
										AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 10f), (int)(Heist_Average_Pay / 20f));
										PlayingAnim = false;
										if (MoneyProp != null)
										{
											MoneyProp.Delete();
											MoneyProp.MarkAsNoLongerNeeded();
											MoneyProp = null;
										}
										DeleteCamera();
										if (bagProp != null)
										{
											bagProp.Delete();
											bagProp.MarkAsNoLongerNeeded();
											bagProp = null;
										}
										SetRob = false;
									}
									return;
								}
								if (CurrentType == LootType.Server_HDD)
								{
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1038078278 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) <= 1039000000)
									{
										if (!PlayerPlayingFinalAnim)
										{
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 0f);
										}
										if (PlayerPlayingFinalAnim)
										{
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
										}
										if (!PlayerPlayingFinalAnim)
										{
											PlayerPlayingFinalAnim = true;
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
										}
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
									{
										Game.Player.Character.Task.ClearAll();
										Crate_Valuables[CurrentLootTarget].GotValuables = true;
										SetBagOn();
										AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 10f), (int)(Heist_Average_Pay / 20f));
										PlayingAnim = false;
										if (MoneyProp != null)
										{
											MoneyProp.Delete();
											MoneyProp.MarkAsNoLongerNeeded();
											MoneyProp = null;
										}
										DeleteCamera();
										if (bagProp != null)
										{
											bagProp.Delete();
											bagProp.MarkAsNoLongerNeeded();
											bagProp = null;
										}
										Game.Player.Character.Task.ClearAllImmediately();
										Notification.Show("Final Anim Grab Server");
										SetRob = false;
									}
									return;
								}
								if (CurrentType == LootType.CashDryer)
								{
									if (PlayerRobbingProp != null && PlayerRobbingProp.IsAttached())
									{
										PlayerRobbingProp.Detach();
									}
									if (!PlayerWaitingForLastAnim && !PlayerWaitingForLastAnim)
									{
										if (!PlayerFirstAnim)
										{
											PlayerFirstAnim = true;
											InitialAnimDone = true;
										}
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@cbr1@ig1_washmach_grab_cash@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "grab_bag", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Loot.Loot, Scene1, "anim@scripted@cbr1@ig1_washmach_grab_cash@male@", "grab_dryer", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Loot.Loot, Scene1, "grab_dryer", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 16f, -16f, 0, 1148846080);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, PlayerRobbingProp, Scene1, "anim@scripted@cbr1@ig1_washmach_grab_cash@male@", "grab_dryermoney", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerRobbingProp, Scene1, "grab_dryermoney", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 16f, -16f, 0, 1148846080);
										PlayerWaitingForLastAnim = true;
										return;
									}
									if (PlayerWaitingForLastAnim && !PlayerFinishedLastAnim)
									{
										if (RobTimer >= IncreasePayPerFrames)
										{
											RobTimer = 0;
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 120f), (int)(Heist_Average_Pay / 80f));
										}
										if (RobTimer != IncreasePayPerFrames)
										{
											RobTimer++;
										}
										if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) == 0)
										{
											PlayerFinishedLastAnim = true;
											Rot = Game.Player.Character.Rotation;
											bagProp.IsPositionFrozen = false;
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "exit_bag", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict("anim@scripted@cbr1@ig1_washmach_grab_cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
											return;
										}
									}
									if (PlayerFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
									{
										PlayerFinishedLastAnim = false;
										Game.Player.Character.Task.ClearAll();
										Crate_Valuables[CurrentLootTarget].GotValuables = true;
										SetBagOn();
										PlayingAnim = false;
										if (MoneyProp != null)
										{
											MoneyProp.Delete();
											MoneyProp.MarkAsNoLongerNeeded();
											MoneyProp = null;
										}
										DeleteCamera();
										if (bagProp != null)
										{
											bagProp.Delete();
											bagProp.MarkAsNoLongerNeeded();
											bagProp = null;
										}
										return;
									}
								}
							}
						}
						if (CurrentType == LootType.SafetyDepositBoxes)
						{
							if (true)
							{
								return;
							}
							if (PlayerCurrentLockboxStage == 1)
							{
								DisplayHelpTextThisFrame("~INPUT_ATTACK~ to drill into the safety deposit boxes");
								if (Game.IsControlPressed(Control.Attack))
								{
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_FRONTEND_CUSTOM_SOUNDSET", 1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_FRONTEND_CUSTOM_SOUNDSET", 1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist3\\casino_heist_finale_general_01", 1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
									Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1.5f);
									while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "drill", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
									{
										GTA.UI.Screen.ShowSubtitle("Waiting on Audio bank");
										Script.Wait(1);
									}
								}
								else
								{
									IsDrillSoundPlaying = false;
									Function.Call(Hash.STOP_SOUND, DrillSound);
									Function.Call(Hash.RELEASE_SOUND_ID, DrillSound);
									DrillSound = -1;
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 0f);
								}
							}
							else
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
							}
							if (PlayerCurrentLockboxStage == 3)
							{
								LoadDict(PlayerCurrentSDDict);
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerCurrentLockBoxProp.Position.X, PlayerCurrentLockBoxProp.Position.Y, PlayerCurrentLockBoxProp.Position.Z, 0.0, 0.0, PlayerCurrentLootBoxRotation, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, PlayerCurrentSDDict, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "action", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "action_p_m_bag_var22_arm_s", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "action_ch_prop_vault_drill_01a", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentLockBoxProp, Scene1, "action_ch_prop_ch_sec_cabinet_01" + PlayerCurrentLockBoxType, LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								PlayerCurrentLockboxStage = 1;
								if (PlayerCurrentLootBagProp != null)
								{
									PlayerCurrentLootBagProp.Delete();
								}
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
							{
								if (PlayerCurrentLockboxStage == 0)
								{
									LoadDict(PlayerCurrentSDDict);
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerCurrentLockBoxProp.Position.X, PlayerCurrentLockBoxProp.Position.Y, PlayerCurrentLockBoxProp.Position.Z, 0.0, 0.0, PlayerCurrentLootBoxRotation, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, PlayerCurrentSDDict, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "action", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "action_p_m_bag_var22_arm_s", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "action_ch_prop_vault_drill_01a", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentLockBoxProp, Scene1, "action_ch_prop_ch_sec_cabinet_01" + PlayerCurrentLockBoxType, LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									PlayerCurrentLockboxStage++;
									if (PlayerCurrentLootBagProp != null)
									{
										PlayerCurrentLootBagProp.Delete();
									}
								}
								else if (PlayerCurrentLockboxStage == 1)
								{
									if (PlayerCurrentLootBagProp != null)
									{
										PlayerCurrentLootBagProp.Delete();
									}
									PlayerCurrentLootBagProp = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
									Random random = new Random();
									int num = random.Next(0, 100);
									if (num >= 50)
									{
										PlayerCurrentLootBagProp.IsVisible = false;
										LoadDict(PlayerCurrentSDDict);
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerCurrentLockBoxProp.Position.X, PlayerCurrentLockBoxProp.Position.Y, PlayerCurrentLockBoxProp.Position.Z, 0.0, 0.0, PlayerCurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, PlayerCurrentSDDict, "no_reward", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "no_reward", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "no_reward_p_m_bag_var22_arm_s", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "no_reward_ch_prop_vault_drill_01a", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentLockBoxProp, Scene1, "no_reward_ch_prop_ch_sec_cabinet_01" + PlayerCurrentLockBoxType, LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										PlayerCurrentLockboxStage++;
										if (PlayerCurrentLockbox + 1 < 5)
										{
											PlayerCurrentSDDict = PlayerCurrentSDDict.Replace("lockbox_0" + PlayerCurrentLockbox, "lockbox_0" + (PlayerCurrentLockbox + 1));
										}
										PlayerCurrentLockbox++;
									}
									else
									{
										PlayerCurrentLootBagProp.IsVisible = true;
										LoadDict(PlayerCurrentSDDict);
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerCurrentLockBoxProp.Position.X, PlayerCurrentLockBoxProp.Position.Y, PlayerCurrentLockBoxProp.Position.Z, 0.0, 0.0, PlayerCurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, PlayerCurrentSDDict, "reward", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentLockBoxProp, Scene1, "reward_ch_prop_ch_sec_cabinet_01" + PlayerCurrentLockBoxType, LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "reward_p_m_bag_var22_arm_s", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "reward_ch_prop_vault_drill_01a", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentLootBagProp, Scene1, "reward_ch_prop_ch_moneybag_01a", LoadDict(PlayerCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										PlayerCurrentLockboxStage++;
										if (PlayerCurrentLockbox + 1 < 5)
										{
											PlayerCurrentSDDict = PlayerCurrentSDDict.Replace("lockbox_0" + PlayerCurrentLockbox, "lockbox_0" + (PlayerCurrentLockbox + 1));
										}
										PlayerCurrentLockbox++;
										int min = (int)(Heist_Average_Pay / 25f);
										int max = (int)(Heist_Average_Pay / 10f);
										AddCash(Game.Player.Character, min, max);
									}
								}
								else if (PlayerCurrentLockboxStage == 2)
								{
									if (PlayerCurrentLockbox == 5)
									{
										if (PlayerCurrentLootBagProp != null)
										{
											PlayerCurrentLootBagProp.Delete();
										}
										PlayerCurrentLockboxStage = 4;
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, 0.0, 0.0, PlayerCurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(PlayerCurrentSDDict), "exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict(PlayerCurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "exit_ch_prop_vault_drill_01a", LoadDict(PlayerCurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "exit_p_m_bag_var22_arm_s", LoadDict(PlayerCurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Loot.Loot, Scene1, "exit_ch_prop_ch_sec_cabinet_01" + PlayerCurrentLockBoxType, LoadDict(PlayerCurrentSDDict), 1f, -1f, 0, 1148846080);
									}
									else
									{
										if (PlayerCurrentLootBagProp != null)
										{
											PlayerCurrentLootBagProp.Delete();
										}
										PlayerCurrentLockboxStage = 3;
									}
								}
								if (PlayerCurrentLockboxStage == 4)
								{
									PlayingAnim = false;
									Game.Player.Character.Task.ClearAllImmediately();
									SetRob = false;
									InitialAnimDone = false;
									PlayingAnim = false;
									if (MoneyProp != null)
									{
										MoneyProp.Delete();
									}
									if (bagProp != null)
									{
										bagProp.Delete();
									}
									Ped character = Game.Player.Character;
									SetBagOff();
								}
							}
						}
					}
					if (CurrentType == LootType.GlassCut_Cabinet)
					{
						DoPlasmaCutter(Loot);
						return;
					}
					if (CurrentType == LootType.M42_JewelCabinet)
					{
						RobAOSCase(Loot.Loot);
					}
				}
				if (CurrentType == LootType.WoodCrate)
				{
					Vector3 position = Game.Player.Character.Position;
					Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 20f, Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"), 0, 0, 0);
					WoodenContainer currentWoodenContainer = CurrentWoodenContainer;
					bool hasRetrievedSupply = currentWoodenContainer.HasRetrievedSupplies;
					switch (currentWoodenContainer.RobLootStage)
					{
					case 0:
						currentWoodenContainer.RobLootStage = 10;
						break;
					case 1:
						currentWoodenContainer.RobLootStage = 10;
						break;
					case 10:
						if (Game.Player.Character.Position.DistanceTo(currentWoodenContainer.LootBox.Position) < 2f)
						{
							Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@trn_ig1_loot@male@");
							Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@trn_ig1_loot@male@");
							Function.Call(Hash.TASK_PED_SLIDE_TO_COORD, Game.Player.Character, currentWoodenContainer.LootBox.Position.X, currentWoodenContainer.LootBox.Position.Y, currentWoodenContainer.LootBox.Position.Z, currentWoodenContainer.LootBox.Rotation.Z, 1f);
							currentWoodenContainer.RobLootStage = 11;
						}
						break;
					case 11:
						currentWoodenContainer.RobLootStage = 12;
						break;
					case 12:
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@trn_ig1_loot@male@");
						currentWoodenContainer.CrowBar = World.CreateProp(RequestModel("w_me_crowbar"), Game.Player.Character.Position, false, false);
						currentWoodenContainer.CrowBar.IsCollisionEnabled = false;
						currentWoodenContainer.LootProp1 = World.CreateProp(RequestModel("bkr_prop_coke_powderbottle_02"), Game.Player.Character.Position, false, false);
						currentWoodenContainer.LootProp1.IsCollisionEnabled = false;
						currentWoodenContainer.LootProp2 = World.CreateProp(RequestModel("ex_office_swag_silver"), Game.Player.Character.Position, false, false);
						currentWoodenContainer.LootProp2.IsCollisionEnabled = false;
						currentWoodenContainer.LootProp2.AttachTo(currentWoodenContainer.LootProp1, new Vector3(0f, 0f, 0.25f), new Vector3(0f, 0f, 0f));
						currentWoodenContainer.LootProp1.Opacity = 0;
						currentWoodenContainer.RobLootStage = 20;
						break;
					case 20:
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@scripted@player@mission@trn_ig1_loot@male@"))
						{
							Script.Wait(0);
						}
						currentWoodenContainer.SyncScene2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, currentWoodenContainer.LootBox.Position.X, currentWoodenContainer.LootBox.Position.Y, currentWoodenContainer.LootBox.Position.Z, 0, 0, currentWoodenContainer.LootBox.Rotation.Z, 2);
						currentWoodenContainer.SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, currentWoodenContainer.LootBox.Position.X, currentWoodenContainer.LootBox.Position.Y, currentWoodenContainer.LootBox.Position.Z, 0, 0, currentWoodenContainer.LootBox.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, currentWoodenContainer.SyncScene, "anim@scripted@player@mission@trn_ig1_loot@male@", "loot", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, currentWoodenContainer.CrowBar, currentWoodenContainer.SyncScene2, "loot_crowbar", "anim@scripted@player@mission@trn_ig1_loot@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, currentWoodenContainer.LootBox, currentWoodenContainer.SyncScene2, "loot_crate", "anim@scripted@player@mission@trn_ig1_loot@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, currentWoodenContainer.LootProp1, currentWoodenContainer.SyncScene2, "loot_can", "anim@scripted@player@mission@trn_ig1_loot@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						currentWoodenContainer.RobLootStage = 21;
						break;
					case 21:
						if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, currentWoodenContainer.SyncScene2) > 0.3f)
						{
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, currentWoodenContainer.SyncScene, 0f);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, currentWoodenContainer.SyncScene2, 0f);
							Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@player@mission@trn_ig1_loot@male@");
							DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to break open ~g~Crate ");
							if (Game.IsControlJustPressed(Control.Attack))
							{
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, currentWoodenContainer.SyncScene, 1f);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, currentWoodenContainer.SyncScene2, 1f);
								currentWoodenContainer.RobLootStage = 30;
							}
						}
						break;
					case 30:
						if (!(Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, currentWoodenContainer.SyncScene2) > 0.99f))
						{
							break;
						}
						if (CurrentWoodenContainer != null)
						{
							if (CurrentWoodenContainer.CrowBar != null)
							{
								CurrentWoodenContainer.CrowBar.Delete();
							}
							if (CurrentWoodenContainer.LootProp2 != null)
							{
								CurrentWoodenContainer.LootProp2.Delete();
							}
							if (CurrentWoodenContainer.LootProp1 != null)
							{
								CurrentWoodenContainer.LootProp1.Delete();
							}
							if (CurrentWoodenContainer.BagProp != null)
							{
								CurrentWoodenContainer.BagProp.Delete();
							}
							foreach (ObjectPlacementMap prop2 in CurrentWoodenContainer.Props)
							{
								if (prop2.Entity != null)
								{
									prop2.Entity.Delete();
								}
							}
						}
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, currentWoodenContainer.SyncScene, 0f);
						currentWoodenContainer.CrowBar.Delete();
						currentWoodenContainer.LootProp1.Delete();
						currentWoodenContainer.LootProp2.Delete();
						Crate_Valuables[CurrentLootTarget].GotValuables = true;
						SetBagOn();
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 10f), (int)(Heist_Average_Pay / 20f));
						Game.Player.Character.Task.ClearAll();
						currentWoodenContainer.RobLootStage = -1;
						Game.Player.Character.Task.ClearAllImmediately();
						currentWoodenContainer.HasRetrievedSupplies = true;
						PlayingAnim = false;
						break;
					}
				}
				if (CurrentType == LootType.Painting)
				{
					Vector3 offsetPosition = PlayerCurrentPaintingNew.GetOffsetPosition(new Vector3(0f, -0.45f, -1.15f));
					if (!PaintingCut_Disable_Require_Input)
					{
						if (CuttingStage == 0)
						{
							Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
							string dict3 = "anim_heist@hs3f@ig11_steal_painting@male@";
							LoadDict(dict3);
							PlayerCurrentID = PlayerCurrentID;
							PlayerCurrentPaintingNew = Loot.Loot;
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict3), "ver_01_top_left_enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_top_left_enter", LoadDict(dict3), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
							CuttingStage = 1;
							nextTick = Game.GameTime + 1500;
						}
						if (CuttingStage == 1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_MOVE_RIGHT_ONLY~ to Cut Painting");
							if (Game.IsControlJustPressed(Control.MoveRightOnly))
							{
								PlayerCurrentPaintingNew = Loot.Loot;
								Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
								string dict4 = "anim_heist@hs3f@ig11_steal_painting@male@";
								LoadDict(dict4);
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict4), "ver_01_cutting_top_left_to_right", 3000.0, -2.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_cutting_top_left_to_right", LoadDict(dict4), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_cutting_top_left_to_right_ch_prop_vault_painting_01a", LoadDict(dict4), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
								CuttingStage = 2;
								nextTick = Game.GameTime + 2250;
							}
						}
						if (CuttingStage == 2 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_BRAKE~ to Cut Painting");
							if (Game.IsControlJustPressed(Control.VehicleBrake))
							{
								PlayerCurrentPaintingNew = Loot.Loot;
								Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
								string dict5 = "anim_heist@hs3f@ig11_steal_painting@male@";
								LoadDict(dict5);
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict5), "ver_01_cutting_right_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_cutting_right_top_to_bottom", LoadDict(dict5), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_cutting_right_top_to_bottom_ch_prop_vault_painting_01a", LoadDict(dict5), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
								CuttingStage = 3;
								nextTick = Game.GameTime + 2500;
							}
						}
						if (CuttingStage == 3 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_MOVE_LEFT_ONLY~ to Cut Painting");
							if (Game.IsControlJustPressed(Control.MoveLeftOnly))
							{
								PlayerCurrentPaintingNew = Loot.Loot;
								Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
								string dict6 = "anim_heist@hs3f@ig11_steal_painting@male@";
								LoadDict(dict6);
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict6), "ver_01_cutting_bottom_right_to_left", 3000.0, -2.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_cutting_bottom_right_to_left", LoadDict(dict6), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_cutting_bottom_right_to_left_ch_prop_vault_painting_01a", LoadDict(dict6), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
								nextTick = Game.GameTime + 2500;
								CuttingStage = 4;
							}
						}
						if (CuttingStage == 4 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_BRAKE~ to Cut Painting");
							if (Game.IsControlJustPressed(Control.VehicleBrake))
							{
								PlayerCurrentPaintingNew = Loot.Loot;
								Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
								string dict7 = "anim_heist@hs3f@ig11_steal_painting@male@";
								LoadDict(dict7);
								Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict7), "ver_01_re-enter_cutting_left_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
								Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_re-enter_cutting_left_top_to_bottom", LoadDict(dict7), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_re-enter_cutting_left_top_to_bottom_ch_prop_vault_painting_01a ", LoadDict(dict7), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
								nextTick = Game.GameTime + 2000;
								CuttingStage = 5;
							}
						}
						if (CuttingStage == 5 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							PlayerCurrentPaintingNew = Loot.Loot;
							string dict8 = "anim_heist@hs3f@ig11_steal_painting@male@";
							LoadDict(dict8);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict8), "ver_01_with_painting_exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_with_painting_exit", LoadDict(dict8), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_with_painting_exit_ch_prop_vault_painting_01a", LoadDict(dict8), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
							nextTick = Game.GameTime + 7000;
							CuttingStage = 6;
						}
						if (CuttingStage == 6 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							PlayerCurrentPaintingNew = Loot.Loot;
							if (!CayoPericoHeist.DisableUnlimitedLootBag)
							{
								AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 30f));
							}
							else
							{
								CurrentPay += 170000;
							}
							CuttingStage = -1;
							PlayingAnim = false;
							Game.Player.Character.Task.ClearAllImmediately();
							Game.Player.Character.Task.ClearAll();
						}
					}
					else
					{
						if (CuttingStage == 0)
						{
							Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
							string dict9 = "anim_heist@hs3f@ig11_steal_painting@male@";
							LoadDict(dict9);
							PlayerCurrentID = PlayerCurrentID;
							PlayerCurrentPaintingNew = Loot.Loot;
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict9), "ver_01_top_left_enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_top_left_enter", LoadDict(dict9), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
							CuttingStage = 1;
							nextTick = Game.GameTime + 1500;
						}
						if (CuttingStage == 1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							PlayerCurrentPaintingNew = Loot.Loot;
							Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
							string dict10 = "anim_heist@hs3f@ig11_steal_painting@male@";
							LoadDict(dict10);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict10), "ver_01_cutting_top_left_to_right", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_cutting_top_left_to_right", LoadDict(dict10), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_cutting_top_left_to_right_ch_prop_vault_painting_01a", LoadDict(dict10), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
							CuttingStage = 2;
							nextTick = Game.GameTime + 2250;
						}
						if (CuttingStage == 2 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							PlayerCurrentPaintingNew = Loot.Loot;
							Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
							string dict11 = "anim_heist@hs3f@ig11_steal_painting@male@";
							LoadDict(dict11);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict11), "ver_01_cutting_right_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_cutting_right_top_to_bottom", LoadDict(dict11), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_cutting_right_top_to_bottom_ch_prop_vault_painting_01a", LoadDict(dict11), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
							CuttingStage = 3;
							nextTick = Game.GameTime + 2500;
						}
						if (CuttingStage == 3 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							PlayerCurrentPaintingNew = Loot.Loot;
							Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
							string dict12 = "anim_heist@hs3f@ig11_steal_painting@male@";
							LoadDict(dict12);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict12), "ver_01_cutting_bottom_right_to_left", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_cutting_bottom_right_to_left", LoadDict(dict12), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_cutting_bottom_right_to_left_ch_prop_vault_painting_01a", LoadDict(dict12), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
							nextTick = Game.GameTime + 2500;
							CuttingStage = 4;
						}
						if (CuttingStage == 4 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							PlayerCurrentPaintingNew = Loot.Loot;
							Game.Player.Character.Weapons.Select(WeaponHash.SwitchBlade);
							string dict13 = "anim_heist@hs3f@ig11_steal_painting@male@";
							LoadDict(dict13);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict13), "ver_01_re-enter_cutting_left_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_re-enter_cutting_left_top_to_bottom", LoadDict(dict13), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_re-enter_cutting_left_top_to_bottom_ch_prop_vault_painting_01a ", LoadDict(dict13), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
							nextTick = Game.GameTime + 2000;
							CuttingStage = 5;
						}
						if (CuttingStage == 5 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							PlayerCurrentPaintingNew = Loot.Loot;
							string dict14 = "anim_heist@hs3f@ig11_steal_painting@male@";
							LoadDict(dict14);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, PlayerCurrentPaintingNew.Rotation.Z, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict14), "ver_01_with_painting_exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "ver_01_with_painting_exit", LoadDict(dict14), 3000.0, -2f, -2f, Game.Player.Character.Rotation.Z - 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerCurrentPaintingNew, Scene1, "ver_01_with_painting_exit_ch_prop_vault_painting_01a", LoadDict(dict14), 3000.0, -2f, -2f, PlayerCurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
							nextTick = Game.GameTime + 7000;
							CuttingStage = 6;
						}
						if (CuttingStage == 6 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							PlayerCurrentPaintingNew = Loot.Loot;
							if (!CayoPericoHeist.DisableUnlimitedLootBag)
							{
								AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 30f));
							}
							else
							{
								CurrentPay += 170000;
							}
							CuttingStage = -1;
							PlayingAnim = false;
							Crate_Valuables[CurrentLootTarget].GotValuables = true;
							Game.Player.Character.Task.ClearAllImmediately();
							Game.Player.Character.Task.ClearAll();
						}
					}
				}
				if (CurrentType == LootType.JewelStoreCabinet)
				{
					if (PlayerRobbingProp.Model == new Model("des_jewel_cab2_start"))
					{
						glass1 = PlayerRobbingProp;
						jewelcabsmash(Game.Player.Character, glass1);
					}
					if (PlayerRobbingProp.Model == new Model("des_jewel_cab3_start"))
					{
						glass2 = PlayerRobbingProp;
						jewelcabsmash2(Game.Player.Character, glass2);
					}
					if (PlayerRobbingProp.Model == new Model("des_jewel_cab4_start"))
					{
						glass3 = PlayerRobbingProp;
						jewelcabsmash3(Game.Player.Character, glass3);
					}
					if (PlayerRobbingProp.Model == new Model("des_jewel_cab_start"))
					{
						glass4 = PlayerRobbingProp;
						jewelcabsmash4(Game.Player.Character, glass4);
					}
				}
			}
			if (Ped == RobberA)
			{
				if (RobberACurrentType != LootType.Painting)
				{
					if (RobberACurrentType != LootType.GlassCut_Cabinet && RobberACurrentType != LootType.JewelStoreCabinet)
					{
						if (RobberACurrentType != LootType.SafetyDepositBoxes && RobberACurrentType != LootType.SafetyDepositBoxes_XM3)
						{
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1024802024 && RobberACurrentType < LootType.CP_Cash)
							{
								RobberAInitialAnimDone = true;
							}
							if (RobberAInitialAnimDone && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, RobberA, "anim@heists@ornate_bank@grab_cash", "bag_intro", 1) && RobberASetRob)
							{
								if (RobberACurrentType == LootType.Trolly_Cash)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberA_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), RobberA.Position, default(Vector3), false, false);
									RobberA_HoldingLootProp.IsPositionFrozen = true;
									RobberA_HoldingLootProp.IsCollisionEnabled = false;
									RobberA_HoldingLootProp.IsInvincible = true;
									RobberARobbing = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberA_HoldingLootProp, RobberA, RobberA.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									RobberACurrentType = LootType.Trolly_Cash;
									RobberASetRob = false;
									RobberAInitialAnimDone = false;
								}
								if (RobberACurrentType == LootType.Trolly_Gold)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberA_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_gold_bar_01a"), RobberA.Position, default(Vector3), false, false);
									RobberA_HoldingLootProp.IsPositionFrozen = true;
									RobberA_HoldingLootProp.IsCollisionEnabled = false;
									RobberA_HoldingLootProp.IsInvincible = true;
									RobberARobbing = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberA_HoldingLootProp, RobberA, RobberA.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									RobberACurrentType = LootType.Trolly_Gold;
									RobberASetRob = false;
									RobberAInitialAnimDone = false;
								}
								if (RobberACurrentType == LootType.Trolly_Diamonds)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberA_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), RobberA.Position, default(Vector3), false, false);
									RobberA_HoldingLootProp.IsPositionFrozen = true;
									RobberA_HoldingLootProp.IsCollisionEnabled = false;
									RobberA_HoldingLootProp.IsInvincible = true;
									RobberARobbing = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberA_HoldingLootProp, RobberA, RobberA.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									RobberACurrentType = LootType.Trolly_Diamonds;
									RobberASetRob = false;
									RobberAInitialAnimDone = false;
								}
								if (RobberACurrentType == LootType.CP_Cash)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberACurrentType = LootType.CP_Cash;
									RobberASetRob = false;
									RobberAInitialAnimDone = false;
									RobberAWaitingForLastAnim = true;
									return;
								}
								if (RobberACurrentType == LootType.CP_Gold)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberACurrentType = LootType.CP_Gold;
									RobberASetRob = false;
									RobberAInitialAnimDone = false;
									RobberAWaitingForLastAnim = true;
									return;
								}
								if (RobberACurrentType == LootType.M42_ServerStack)
								{
									string dict15 = "anim@scripted@heist@ig1_table_grab@gold@male@";
									if (RobberARobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
									{
										dict15 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
									}
									LoadDict(dict15);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(dict15), "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "grab_bag", LoadDict(dict15), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberACurrentType = LootType.M42_ServerStack;
									RobberASetRob = false;
									RobberAInitialAnimDone = false;
									RobberAWaitingForLastAnim = true;
									return;
								}
							}
						}
						if (RobberACurrentType == LootType.SafetyDepositBoxes)
						{
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberASyncScene, 1.25f);
							if (RobberACurrentLockboxStage == 3)
							{
								LoadDict(RobberACurrentSDDict);
								RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberACurrentLockBoxProp.Position.X, RobberACurrentLockBoxProp.Position.Y, RobberACurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberACurrentLootBoxRotation, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, RobberACurrentSDDict, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "action", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "action_p_m_bag_var22_arm_s", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "action_ch_prop_vault_drill_01a", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberACurrentLockBoxProp, RobberASyncScene, "action_ch_prop_ch_sec_cabinet_01" + RobberACurrentLockBoxType, LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberACurrentLockboxStage = 1;
								if (RobberACurrentLootBagProp != null)
								{
									RobberACurrentLootBagProp.Delete();
								}
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
							{
								if (RobberACurrentLockboxStage == 0)
								{
									LoadDict(RobberACurrentSDDict);
									RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberACurrentLockBoxProp.Position.X, RobberACurrentLockBoxProp.Position.Y, RobberACurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberACurrentLootBoxRotation, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, RobberACurrentSDDict, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "action", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "action_p_m_bag_var22_arm_s", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "action_ch_prop_vault_drill_01a", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberACurrentLockBoxProp, RobberASyncScene, "action_ch_prop_ch_sec_cabinet_01" + RobberACurrentLockBoxType, LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberACurrentLockboxStage++;
									if (RobberACurrentLootBagProp != null)
									{
										RobberACurrentLootBagProp.Delete();
									}
								}
								else if (RobberACurrentLockboxStage == 1)
								{
									if (RobberACurrentLootBagProp != null)
									{
										RobberACurrentLootBagProp.Delete();
									}
									RobberACurrentLootBagProp = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), RobberA.Position, false, false);
									Random random2 = new Random();
									int num2 = random2.Next(0, 100);
									if (num2 >= 50)
									{
										RobberACurrentLootBagProp.IsVisible = false;
										LoadDict(RobberACurrentSDDict);
										RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberACurrentLockBoxProp.Position.X, RobberACurrentLockBoxProp.Position.Y, RobberACurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberACurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, RobberACurrentSDDict, "no_reward", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "no_reward", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "no_reward_p_m_bag_var22_arm_s", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "no_reward_ch_prop_vault_drill_01a", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberACurrentLockBoxProp, RobberASyncScene, "no_reward_ch_prop_ch_sec_cabinet_01" + RobberACurrentLockBoxType, LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										RobberACurrentLockboxStage++;
										if (RobberACurrentLockbox + 1 < 5)
										{
											RobberACurrentSDDict = RobberACurrentSDDict.Replace("lockbox_0" + RobberACurrentLockbox, "lockbox_0" + (RobberACurrentLockbox + 1));
										}
										RobberACurrentLockbox++;
									}
									else
									{
										RobberACurrentLootBagProp.IsVisible = true;
										LoadDict(RobberACurrentSDDict);
										RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberACurrentLockBoxProp.Position.X, RobberACurrentLockBoxProp.Position.Y, RobberACurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberACurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, RobberACurrentSDDict, "reward", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberACurrentLockBoxProp, RobberASyncScene, "reward_ch_prop_ch_sec_cabinet_01" + RobberACurrentLockBoxType, LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "reward_p_m_bag_var22_arm_s", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "reward_ch_prop_vault_drill_01a", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberACurrentLootBagProp, RobberASyncScene, "reward_ch_prop_ch_moneybag_01a", LoadDict(RobberACurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										RobberACurrentLockboxStage++;
										if (RobberACurrentLockbox + 1 < 5)
										{
											RobberACurrentSDDict = RobberACurrentSDDict.Replace("lockbox_0" + RobberACurrentLockbox, "lockbox_0" + (RobberACurrentLockbox + 1));
										}
										RobberACurrentLockbox++;
										int min2 = (int)(Heist_Average_Pay / 25f);
										int max2 = (int)(Heist_Average_Pay / 10f);
										increaseCurrentPay(min2, max2);
									}
								}
								else if (RobberACurrentLockboxStage == 2)
								{
									if (RobberACurrentLockbox == 5)
									{
										if (RobberACurrentLootBagProp != null)
										{
											RobberACurrentLootBagProp.Delete();
										}
										RobberACurrentLockboxStage = 4;
										RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, 0.0, 0.0, RobberACurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(RobberACurrentSDDict), "exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene));
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "exit", LoadDict(RobberACurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "exit_ch_prop_vault_drill_01a", LoadDict(RobberACurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "exit_p_m_bag_var22_arm_s", LoadDict(RobberACurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Loot.Loot, RobberASyncScene, "exit_ch_prop_ch_sec_cabinet_01" + RobberACurrentLockBoxType, LoadDict(RobberACurrentSDDict), 1f, -1f, 0, 1148846080);
									}
									else
									{
										if (RobberACurrentLootBagProp != null)
										{
											RobberACurrentLootBagProp.Delete();
										}
										RobberACurrentLockboxStage = 3;
									}
								}
								if (RobberACurrentLockboxStage == 4)
								{
									RobberARobbing = false;
									RobberA.Task.ClearAllImmediately();
									RobberASetRob = false;
									RobberAInitialAnimDone = false;
									RobberARobbing = false;
									if (RobberA_HoldingLootProp != null)
									{
										RobberA_HoldingLootProp.Delete();
									}
									if (RobberA_bagProp != null)
									{
										RobberA_bagProp.Delete();
									}
								}
							}
						}
						if (RobberACurrentType == LootType.CP_Cash || RobberACurrentType == LootType.CP_Gold || RobberACurrentType == LootType.M42_ServerStack)
						{
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
							{
								if (RobberACurrentType == LootType.CP_Cash && !RobberAFirstAnim)
								{
									string dict16 = "anim@scripted@heist@ig1_table_grab@gold@male@";
									if (RobberARobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
									{
										dict16 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
									}
									LoadDict(dict16);
									RobberAFirstAnim = true;
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberARobbingProp, RobberASyncScene, LoadDict(dict16), "grab_cash", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberARobbingProp, RobberASyncScene, "grab_cash", LoadDict(dict16), 16f, -16f, 0, 1148846080);
									RobberAInitialAnimDone = true;
								}
								if (RobberACurrentType == LootType.CP_Gold && !RobberAFirstAnim)
								{
									RobberAFirstAnim = true;
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberARobbingProp, RobberASyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberARobbingProp, RobberASyncScene, "grab_gold", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 16f, -16f, 0, 1148846080);
									RobberAInitialAnimDone = true;
								}
								if (RobberACurrentType == LootType.M42_ServerStack && !RobberAFirstAnim)
								{
									string dict17 = "anim@scripted@heist@ig1_table_grab@gold@male@";
									if (RobberARobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
									{
										dict17 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
									}
									LoadDict(dict17);
									RobberAFirstAnim = true;
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberARobbingProp, RobberASyncScene, LoadDict(dict17), "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberARobbingProp, RobberASyncScene, "grab_gold", LoadDict(dict17), 16f, -16f, 0, 1148846080);
									RobberAInitialAnimDone = true;
								}
							}
							if (RobberACurrentType == LootType.CP_Cash && !RobberASetRob)
							{
								if (RobberAWaitingForLastAnim)
								{
									if (RobberARobTimer >= IncreasePayPerFrames)
									{
										RobberARobTimer = 0;
										AddCash(RobberA, (int)(Heist_Average_Pay / 140f), (int)(Heist_Average_Pay / 90f));
									}
									if (RobberARobTimer != IncreasePayPerFrames)
									{
										RobberARobTimer++;
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
									{
										RobberAFinishedLastAnim = true;
										RobberAWaitingForLastAnim = false;
										Rot = RobberA.Rotation;
										RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberARobbingProp.Position.X, RobberARobbingProp.Position.Y, RobberARobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberARobbingProp.Position.X, RobberARobbingProp.Position.Y, RobberARobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										return;
									}
								}
								if (RobberAFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
								{
									RobberAFinishedLastAnim = false;
									if (RobberA_HoldingLootProp != null)
									{
										RobberA_HoldingLootProp.Delete();
									}
									if (RobberA_bagProp != null)
									{
										RobberA_bagProp.Delete();
									}
									RobberA.Task.ClearAll();
									RobberARobbing = false;
									RobberA_RobberMarker = default(Vector3);
									return;
								}
							}
							if (RobberACurrentType == LootType.CP_Gold && !RobberASetRob)
							{
								if (RobberAWaitingForLastAnim)
								{
									if (RobberARobTimer >= IncreasePayPerFrames)
									{
										RobberARobTimer = 0;
										AddCash(RobberA, (int)(Heist_Average_Pay / 100f), (int)(Heist_Average_Pay / 80f));
									}
									if (RobberARobTimer != IncreasePayPerFrames)
									{
										RobberARobTimer++;
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
									{
										RobberAFinishedLastAnim = true;
										RobberAWaitingForLastAnim = false;
										Rot = RobberA.Rotation;
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberARobbingProp.Position.X, RobberARobbingProp.Position.Y, RobberARobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberARobbingProp.Position.X, RobberARobbingProp.Position.Y, RobberARobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
										return;
									}
								}
								if (RobberAFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
								{
									RobberAFinishedLastAnim = false;
									if (RobberA_HoldingLootProp != null)
									{
										RobberA_HoldingLootProp.Delete();
									}
									if (RobberA_bagProp != null)
									{
										RobberA_bagProp.Delete();
									}
									RobberA.Task.ClearAll();
									AddCash(RobberA, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 40f));
									RobberARobbing = false;
									RobberA_RobberMarker = default(Vector3);
									return;
								}
							}
							if (RobberACurrentType == LootType.M42_ServerStack && !RobberASetRob)
							{
								if (RobberAWaitingForLastAnim)
								{
									if (RobberARobTimer >= IncreasePayPerFrames)
									{
										RobberARobTimer = 0;
										AddCash(RobberA, (int)(Heist_Average_Pay / 100f), (int)(Heist_Average_Pay / 80f));
									}
									if (RobberARobTimer != IncreasePayPerFrames)
									{
										RobberARobTimer++;
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
									{
										RobberAFinishedLastAnim = true;
										RobberAWaitingForLastAnim = false;
										Rot = RobberA.Rotation;
										string dict18 = "anim@scripted@heist@ig1_table_grab@gold@male@";
										if (RobberARobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
										{
											dict18 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
										}
										LoadDict(dict18);
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberARobbingProp.Position.X, RobberARobbingProp.Position.Y, RobberARobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, RobberARobbingProp.Position.X, RobberARobbingProp.Position.Y, RobberARobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "exit_bag", LoadDict(dict18), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(dict18), "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "exit", LoadDict(dict18), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
										return;
									}
								}
								if (RobberAFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
								{
									RobberAFinishedLastAnim = false;
									if (RobberA_HoldingLootProp != null)
									{
										RobberA_HoldingLootProp.Delete();
									}
									if (RobberA_bagProp != null)
									{
										RobberA_bagProp.Delete();
									}
									RobberA.Task.ClearAll();
									AddCash(RobberA, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 40f));
									RobberARobbing = false;
									RobberA_RobberMarker = default(Vector3);
									return;
								}
							}
						}
					}
					if (RobberACurrentType == LootType.JewelStoreCabinet)
					{
						if (RobberARobbingProp.Model == new Model("des_jewel_cab2_start"))
						{
							glass1 = RobberARobbingProp;
							jewelcabsmash(RobberA, glass1);
						}
						if (RobberARobbingProp.Model == new Model("des_jewel_cab3_start"))
						{
							glass2 = RobberARobbingProp;
							jewelcabsmash2(RobberA, glass2);
						}
						if (RobberARobbingProp.Model == new Model("des_jewel_cab4_start"))
						{
							glass3 = RobberARobbingProp;
							jewelcabsmash3(RobberA, glass3);
						}
						if (RobberARobbingProp.Model == new Model("des_jewel_cab_start"))
						{
							glass4 = RobberARobbingProp;
							jewelcabsmash4(RobberA, glass4);
						}
					}
				}
				if (RobberACurrentType == LootType.Painting)
				{
					Vector3 offsetPosition2 = RobberA_CurrentPaintingNew.GetOffsetPosition(new Vector3(0f, -0.45f, -1.15f));
					if (RobberACuttingStage == 0)
					{
						RobberA.Weapons.Select(WeaponHash.SwitchBlade);
						string dict19 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict19);
						PlayerCurrentID = PlayerCurrentID;
						RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, RobberA_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(dict19), "ver_01_top_left_enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "ver_01_top_left_enter", LoadDict(dict19), 3000.0, -2f, -2f, RobberA.Rotation.Z - 180f, 1148846080, 0);
						RobberACuttingStage = 1;
					}
					if (RobberACuttingStage == 1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
					{
						RobberA.Weapons.Select(WeaponHash.SwitchBlade);
						string dict20 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict20);
						RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, RobberA_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(dict20), "ver_01_cutting_top_left_to_right", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "ver_01_cutting_top_left_to_right", LoadDict(dict20), 3000.0, -2f, -2f, RobberA.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_CurrentPaintingNew, RobberASyncScene, "ver_01_cutting_top_left_to_right_ch_prop_vault_painting_01a", LoadDict(dict20), 3000.0, -2f, -2f, RobberA_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberACuttingStage = 2;
					}
					if (RobberACuttingStage == 2 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
					{
						RobberA.Weapons.Select(WeaponHash.SwitchBlade);
						string dict21 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict21);
						RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, RobberA_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(dict21), "ver_01_cutting_right_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "ver_01_cutting_right_top_to_bottom", LoadDict(dict21), 3000.0, -2f, -2f, RobberA.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_CurrentPaintingNew, RobberASyncScene, "ver_01_cutting_right_top_to_bottom_ch_prop_vault_painting_01a", LoadDict(dict21), 3000.0, -2f, -2f, RobberA_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberACuttingStage = 3;
					}
					if (RobberACuttingStage == 3 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
					{
						RobberA.Weapons.Select(WeaponHash.SwitchBlade);
						string dict22 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict22);
						RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, RobberA_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(dict22), "ver_01_cutting_bottom_right_to_left", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "ver_01_cutting_bottom_right_to_left", LoadDict(dict22), 3000.0, -2f, -2f, RobberA.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_CurrentPaintingNew, RobberASyncScene, "ver_01_cutting_bottom_right_to_left_ch_prop_vault_painting_01a", LoadDict(dict22), 3000.0, -2f, -2f, RobberA_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberACuttingStage = 4;
					}
					if (RobberACuttingStage == 4 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
					{
						RobberA.Weapons.Select(WeaponHash.SwitchBlade);
						string dict23 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict23);
						RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, RobberA_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(dict23), "ver_01_re-enter_cutting_left_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "ver_01_re-enter_cutting_left_top_to_bottom", LoadDict(dict23), 3000.0, -2f, -2f, RobberA.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_CurrentPaintingNew, RobberASyncScene, "ver_01_re-enter_cutting_left_top_to_bottom_ch_prop_vault_painting_01a ", LoadDict(dict23), 3000.0, -2f, -2f, RobberA_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberACuttingStage = 5;
					}
					if (RobberACuttingStage == 5 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
					{
						string dict24 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict24);
						RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, RobberA_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(dict24), "ver_01_with_painting_exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "ver_01_with_painting_exit", LoadDict(dict24), 3000.0, -2f, -2f, RobberA.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_CurrentPaintingNew, RobberASyncScene, "ver_01_with_painting_exit_ch_prop_vault_painting_01a", LoadDict(dict24), 3000.0, -2f, -2f, RobberA_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberACuttingStage = 6;
					}
					if (RobberACuttingStage == 6 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
					{
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 30f));
						RobberARobbing = false;
						RobberA_RobberMarker = default(Vector3);
						RobberA.Task.ClearAllImmediately();
						RobberA.Task.ClearAll();
						RobberACuttingStage = -1;
					}
				}
			}
			if (Ped == RobberB)
			{
				if (RobberBCurrentType != LootType.Painting)
				{
					if (RobberBCurrentType != LootType.GlassCut_Cabinet && RobberBCurrentType != LootType.JewelStoreCabinet)
					{
						if (RobberBCurrentType != LootType.SafetyDepositBoxes && RobberBCurrentType != LootType.SafetyDepositBoxes_XM3)
						{
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1024802024 && RobberBCurrentType < LootType.CP_Cash)
							{
								RobberBInitialAnimDone = true;
							}
							if (RobberBInitialAnimDone && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, RobberB, "anim@heists@ornate_bank@grab_cash", "bag_intro", 1) && RobberBSetRob)
							{
								if (RobberBCurrentType == LootType.Trolly_Cash)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberB_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), RobberB.Position, default(Vector3), false, false);
									RobberB_HoldingLootProp.IsPositionFrozen = true;
									RobberB_HoldingLootProp.IsCollisionEnabled = false;
									RobberB_HoldingLootProp.IsInvincible = true;
									RobberBRobbing = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberB_HoldingLootProp, RobberB, RobberB.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									RobberBCurrentType = LootType.Trolly_Cash;
									RobberBSetRob = false;
									RobberBInitialAnimDone = false;
								}
								if (RobberBCurrentType == LootType.Trolly_Gold)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberB_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_gold_bar_01a"), RobberB.Position, default(Vector3), false, false);
									RobberB_HoldingLootProp.IsPositionFrozen = true;
									RobberB_HoldingLootProp.IsCollisionEnabled = false;
									RobberB_HoldingLootProp.IsInvincible = true;
									RobberBRobbing = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberB_HoldingLootProp, RobberB, RobberB.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									RobberBCurrentType = LootType.Trolly_Gold;
									RobberBSetRob = false;
									RobberBInitialAnimDone = false;
								}
								if (RobberBCurrentType == LootType.Trolly_Diamonds)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberB_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), RobberB.Position, default(Vector3), false, false);
									RobberB_HoldingLootProp.IsPositionFrozen = true;
									RobberB_HoldingLootProp.IsCollisionEnabled = false;
									RobberB_HoldingLootProp.IsInvincible = true;
									RobberBRobbing = true;
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberB_HoldingLootProp, RobberB, RobberB.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
									RobberBCurrentType = LootType.Trolly_Diamonds;
									RobberBSetRob = false;
									RobberBInitialAnimDone = false;
								}
								if (RobberBCurrentType == LootType.CP_Cash)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberBCurrentType = LootType.CP_Cash;
									RobberBSetRob = false;
									RobberBInitialAnimDone = false;
									RobberBWaitingForLastAnim = true;
									return;
								}
								if (RobberBCurrentType == LootType.CP_Gold)
								{
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberBCurrentType = LootType.CP_Gold;
									RobberBSetRob = false;
									RobberBInitialAnimDone = false;
									RobberBWaitingForLastAnim = true;
									return;
								}
								if (RobberBCurrentType == LootType.M42_ServerStack)
								{
									string dict25 = "anim@scripted@heist@ig1_table_grab@gold@male@";
									if (RobberBRobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
									{
										dict25 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
									}
									LoadDict(dict25);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(dict25), "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "grab_bag", LoadDict(dict25), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberBCurrentType = LootType.M42_ServerStack;
									RobberBSetRob = false;
									RobberBInitialAnimDone = false;
									RobberBWaitingForLastAnim = true;
									return;
								}
							}
						}
						if (RobberBCurrentType == LootType.SafetyDepositBoxes)
						{
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberBSyncScene, 1.25f);
							if (RobberBCurrentLockboxStage == 3)
							{
								LoadDict(RobberBCurrentSDDict);
								RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberBCurrentLockBoxProp.Position.X, RobberBCurrentLockBoxProp.Position.Y, RobberBCurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberBCurrentLootBoxRotation, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, RobberBCurrentSDDict, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "action", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "action_p_m_bag_var22_arm_s", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "action_ch_prop_vault_drill_01a", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberBCurrentLockBoxProp, RobberBSyncScene, "action_ch_prop_ch_sec_cabinet_01" + RobberBCurrentLockBoxType, LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberBCurrentLockboxStage = 1;
								if (RobberBCurrentLootBagProp != null)
								{
									RobberBCurrentLootBagProp.Delete();
								}
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
							{
								if (RobberBCurrentLockboxStage == 0)
								{
									LoadDict(RobberBCurrentSDDict);
									RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberBCurrentLockBoxProp.Position.X, RobberBCurrentLockBoxProp.Position.Y, RobberBCurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberBCurrentLootBoxRotation, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, RobberBCurrentSDDict, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "action", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "action_p_m_bag_var22_arm_s", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "action_ch_prop_vault_drill_01a", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberBCurrentLockBoxProp, RobberBSyncScene, "action_ch_prop_ch_sec_cabinet_01" + RobberBCurrentLockBoxType, LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberBCurrentLockboxStage++;
									if (RobberBCurrentLootBagProp != null)
									{
										RobberBCurrentLootBagProp.Delete();
									}
								}
								else if (RobberBCurrentLockboxStage == 1)
								{
									if (RobberBCurrentLootBagProp != null)
									{
										RobberBCurrentLootBagProp.Delete();
									}
									RobberBCurrentLootBagProp = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), RobberB.Position, false, false);
									Random random3 = new Random();
									int num3 = random3.Next(0, 100);
									if (num3 >= 50)
									{
										RobberBCurrentLootBagProp.IsVisible = false;
										LoadDict(RobberBCurrentSDDict);
										RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberBCurrentLockBoxProp.Position.X, RobberBCurrentLockBoxProp.Position.Y, RobberBCurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberBCurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, RobberBCurrentSDDict, "no_reward", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "no_reward", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "no_reward_p_m_bag_var22_arm_s", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "no_reward_ch_prop_vault_drill_01a", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberBCurrentLockBoxProp, RobberBSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01" + RobberBCurrentLockBoxType, LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										RobberBCurrentLockboxStage++;
										if (RobberBCurrentLockbox + 1 < 5)
										{
											RobberBCurrentSDDict = RobberBCurrentSDDict.Replace("lockbox_0" + RobberBCurrentLockbox, "lockbox_0" + (RobberBCurrentLockbox + 1));
										}
										RobberBCurrentLockbox++;
									}
									else
									{
										RobberBCurrentLootBagProp.IsVisible = true;
										LoadDict(RobberBCurrentSDDict);
										RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberBCurrentLockBoxProp.Position.X, RobberBCurrentLockBoxProp.Position.Y, RobberBCurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberBCurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, RobberBCurrentSDDict, "reward", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberBCurrentLockBoxProp, RobberBSyncScene, "reward_ch_prop_ch_sec_cabinet_01" + RobberBCurrentLockBoxType, LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "reward_p_m_bag_var22_arm_s", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "reward_ch_prop_vault_drill_01a", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberBCurrentLootBagProp, RobberBSyncScene, "reward_ch_prop_ch_moneybag_01a", LoadDict(RobberBCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										RobberBCurrentLockboxStage++;
										if (RobberBCurrentLockbox + 1 < 5)
										{
											RobberBCurrentSDDict = RobberBCurrentSDDict.Replace("lockbox_0" + RobberBCurrentLockbox, "lockbox_0" + (RobberBCurrentLockbox + 1));
										}
										RobberBCurrentLockbox++;
										int min3 = (int)(Heist_Average_Pay / 25f);
										int max3 = (int)(Heist_Average_Pay / 10f);
										increaseCurrentPay(min3, max3);
									}
								}
								else if (RobberBCurrentLockboxStage == 2)
								{
									if (RobberBCurrentLockbox == 5)
									{
										if (RobberBCurrentLootBagProp != null)
										{
											RobberBCurrentLootBagProp.Delete();
										}
										RobberBCurrentLockboxStage = 4;
										RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, 0.0, 0.0, RobberBCurrentLootBoxRotation, 2);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(RobberBCurrentSDDict), "exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene));
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "exit", LoadDict(RobberBCurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "exit_ch_prop_vault_drill_01a", LoadDict(RobberBCurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "exit_p_m_bag_var22_arm_s", LoadDict(RobberBCurrentSDDict), 1f, -1f, 0, 1148846080);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Loot.Loot, RobberBSyncScene, "exit_ch_prop_ch_sec_cabinet_01" + RobberBCurrentLockBoxType, LoadDict(RobberBCurrentSDDict), 1f, -1f, 0, 1148846080);
									}
									else
									{
										if (RobberBCurrentLootBagProp != null)
										{
											RobberBCurrentLootBagProp.Delete();
										}
										RobberBCurrentLockboxStage = 3;
									}
								}
								if (RobberBCurrentLockboxStage == 4)
								{
									RobberBRobbing = false;
									RobberB.Task.ClearAllImmediately();
									RobberBSetRob = false;
									RobberBInitialAnimDone = false;
									RobberBRobbing = false;
									if (RobberB_HoldingLootProp != null)
									{
										RobberB_HoldingLootProp.Delete();
									}
									if (RobberB_bagProp != null)
									{
										RobberB_bagProp.Delete();
									}
								}
							}
						}
						if (RobberBCurrentType == LootType.CP_Cash || RobberBCurrentType == LootType.CP_Gold || RobberBCurrentType == LootType.M42_ServerStack)
						{
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
							{
								if (RobberBCurrentType == LootType.CP_Cash && !RobberBFirstAnim)
								{
									RobberBFirstAnim = true;
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberBRobbingProp, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab_cash", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberBRobbingProp, RobberBSyncScene, "grab_cash", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 16f, -16f, 0, 1148846080);
									RobberBInitialAnimDone = true;
								}
								if (RobberBCurrentType == LootType.CP_Gold && !RobberBFirstAnim)
								{
									RobberBFirstAnim = true;
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberBRobbingProp, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberBRobbingProp, RobberBSyncScene, "grab_gold", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 16f, -16f, 0, 1148846080);
									RobberBInitialAnimDone = true;
								}
								if (RobberBCurrentType == LootType.M42_ServerStack && !RobberBFirstAnim)
								{
									string dict26 = "anim@scripted@heist@ig1_table_grab@gold@male@";
									if (RobberBRobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
									{
										dict26 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
									}
									LoadDict(dict26);
									RobberBFirstAnim = true;
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberBRobbingProp, RobberBSyncScene, LoadDict(dict26), "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberBRobbingProp, RobberBSyncScene, "grab_gold", LoadDict(dict26), 16f, -16f, 0, 1148846080);
									RobberBInitialAnimDone = true;
								}
							}
							if (RobberBCurrentType == LootType.CP_Cash && !RobberBSetRob)
							{
								if (RobberBWaitingForLastAnim)
								{
									if (RobberBRobTimer >= IncreasePayPerFrames)
									{
										RobberBRobTimer = 0;
										AddCash(RobberB, (int)(Heist_Average_Pay / 120f), (int)(Heist_Average_Pay / 80f));
									}
									if (RobberBRobTimer != IncreasePayPerFrames)
									{
										RobberBRobTimer++;
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
									{
										RobberBFinishedLastAnim = true;
										RobberBWaitingForLastAnim = false;
										Rot = RobberB.Rotation;
										RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberBRobbingProp.Position.X, RobberBRobbingProp.Position.Y, RobberBRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberBRobbingProp.Position.X, RobberBRobbingProp.Position.Y, RobberBRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
										return;
									}
								}
								if (RobberBFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
								{
									RobberBFinishedLastAnim = false;
									if (RobberB_HoldingLootProp != null)
									{
										RobberB_HoldingLootProp.Delete();
									}
									if (RobberB_bagProp != null)
									{
										RobberB_bagProp.Delete();
									}
									RobberB.Task.ClearAll();
									RobberBRobbing = false;
									RobberB_RobberMarker = default(Vector3);
									return;
								}
							}
							if (RobberBCurrentType == LootType.CP_Gold && !RobberBSetRob)
							{
								if (RobberBWaitingForLastAnim)
								{
									if (RobberBRobTimer >= IncreasePayPerFrames)
									{
										RobberBRobTimer = 0;
										AddCash(RobberB, (int)(Heist_Average_Pay / 80f), (int)(Heist_Average_Pay / 60f));
									}
									if (RobberBRobTimer != IncreasePayPerFrames)
									{
										RobberBRobTimer++;
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
									{
										RobberBFinishedLastAnim = true;
										RobberBWaitingForLastAnim = false;
										Rot = RobberB.Rotation;
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberBRobbingProp.Position.X, RobberBRobbingProp.Position.Y, RobberBRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberBRobbingProp.Position.X, RobberBRobbingProp.Position.Y, RobberBRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
										return;
									}
								}
								if (RobberBFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
								{
									RobberBFinishedLastAnim = false;
									if (RobberB_HoldingLootProp != null)
									{
										RobberB_HoldingLootProp.Delete();
									}
									if (RobberB_bagProp != null)
									{
										RobberB_bagProp.Delete();
									}
									RobberB.Task.ClearAll();
									AddCash(RobberB, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 40f));
									RobberBRobbing = false;
									RobberB_RobberMarker = default(Vector3);
									return;
								}
							}
							if (RobberBCurrentType == LootType.M42_ServerStack && !RobberBSetRob)
							{
								if (RobberBWaitingForLastAnim)
								{
									if (RobberBRobTimer >= IncreasePayPerFrames)
									{
										RobberBRobTimer = 0;
										AddCash(RobberB, (int)(Heist_Average_Pay / 80f), (int)(Heist_Average_Pay / 60f));
									}
									if (RobberBRobTimer != IncreasePayPerFrames)
									{
										RobberBRobTimer++;
									}
									if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
									{
										RobberBFinishedLastAnim = true;
										RobberBWaitingForLastAnim = false;
										Rot = RobberB.Rotation;
										string dict27 = "anim@scripted@heist@ig1_table_grab@gold@male@";
										if (RobberBRobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
										{
											dict27 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
										}
										LoadDict(dict27);
										Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberBRobbingProp.Position.X, RobberBRobbingProp.Position.Y, RobberBRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, RobberBRobbingProp.Position.X, RobberBRobbingProp.Position.Y, RobberBRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "exit_bag", LoadDict(dict27), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(dict27), "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "exit", LoadDict(dict27), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
										return;
									}
								}
								if (RobberBFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
								{
									RobberBFinishedLastAnim = false;
									if (RobberB_HoldingLootProp != null)
									{
										RobberB_HoldingLootProp.Delete();
									}
									if (RobberB_bagProp != null)
									{
										RobberB_bagProp.Delete();
									}
									RobberB.Task.ClearAll();
									AddCash(RobberB, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 40f));
									RobberBRobbing = false;
									RobberB_RobberMarker = default(Vector3);
									return;
								}
							}
						}
					}
					if (RobberBCurrentType == LootType.JewelStoreCabinet)
					{
						if (RobberBRobbingProp.Model == new Model("des_jewel_cab2_start"))
						{
							glass1 = RobberBRobbingProp;
							jewelcabsmash(RobberB, glass1);
						}
						if (RobberBRobbingProp.Model == new Model("des_jewel_cab3_start"))
						{
							glass2 = RobberBRobbingProp;
							jewelcabsmash2(RobberB, glass2);
						}
						if (RobberBRobbingProp.Model == new Model("des_jewel_cab4_start"))
						{
							glass3 = RobberBRobbingProp;
							jewelcabsmash3(RobberB, glass3);
						}
						if (RobberBRobbingProp.Model == new Model("des_jewel_cab_start"))
						{
							glass4 = RobberBRobbingProp;
							jewelcabsmash4(RobberB, glass4);
						}
					}
				}
				if (RobberBCurrentType == LootType.Painting)
				{
					Vector3 offsetPosition3 = RobberB_CurrentPaintingNew.GetOffsetPosition(new Vector3(0f, -0.45f, -1.15f));
					if (RobberBCuttingStage == 0)
					{
						RobberB.Weapons.Select(WeaponHash.SwitchBlade);
						string dict28 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict28);
						PlayerCurrentID = PlayerCurrentID;
						RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, RobberB_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(dict28), "ver_01_top_left_enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "ver_01_top_left_enter", LoadDict(dict28), 3000.0, -2f, -2f, RobberB.Rotation.Z - 180f, 1148846080, 0);
						RobberBCuttingStage = 1;
					}
					if (RobberBCuttingStage == 1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
					{
						RobberB.Weapons.Select(WeaponHash.SwitchBlade);
						string dict29 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict29);
						RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, RobberB_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(dict29), "ver_01_cutting_top_left_to_right", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "ver_01_cutting_top_left_to_right", LoadDict(dict29), 3000.0, -2f, -2f, RobberB.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_CurrentPaintingNew, RobberBSyncScene, "ver_01_cutting_top_left_to_right_ch_prop_vault_painting_01a", LoadDict(dict29), 3000.0, -2f, -2f, RobberB_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberBCuttingStage = 2;
					}
					if (RobberBCuttingStage == 2 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
					{
						RobberB.Weapons.Select(WeaponHash.SwitchBlade);
						string dict30 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict30);
						RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, RobberB_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(dict30), "ver_01_cutting_right_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "ver_01_cutting_right_top_to_bottom", LoadDict(dict30), 3000.0, -2f, -2f, RobberB.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_CurrentPaintingNew, RobberBSyncScene, "ver_01_cutting_right_top_to_bottom_ch_prop_vault_painting_01a", LoadDict(dict30), 3000.0, -2f, -2f, RobberB_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberBCuttingStage = 3;
					}
					if (RobberBCuttingStage == 3 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
					{
						RobberB.Weapons.Select(WeaponHash.SwitchBlade);
						string dict31 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict31);
						RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, RobberB_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(dict31), "ver_01_cutting_bottom_right_to_left", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "ver_01_cutting_bottom_right_to_left", LoadDict(dict31), 3000.0, -2f, -2f, RobberB.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_CurrentPaintingNew, RobberBSyncScene, "ver_01_cutting_bottom_right_to_left_ch_prop_vault_painting_01a", LoadDict(dict31), 3000.0, -2f, -2f, RobberB_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberBCuttingStage = 4;
					}
					if (RobberBCuttingStage == 4 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
					{
						RobberB.Weapons.Select(WeaponHash.SwitchBlade);
						string dict32 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict32);
						RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, RobberB_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(dict32), "ver_01_re-enter_cutting_left_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "ver_01_re-enter_cutting_left_top_to_bottom", LoadDict(dict32), 3000.0, -2f, -2f, RobberB.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_CurrentPaintingNew, RobberBSyncScene, "ver_01_re-enter_cutting_left_top_to_bottom_ch_prop_vault_painting_01a ", LoadDict(dict32), 3000.0, -2f, -2f, RobberB_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberBCuttingStage = 5;
					}
					if (RobberBCuttingStage == 5 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
					{
						string dict33 = "anim_heist@hs3f@ig11_steal_painting@male@";
						LoadDict(dict33);
						RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, RobberB_CurrentPaintingNew.Rotation.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(dict33), "ver_01_with_painting_exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "ver_01_with_painting_exit", LoadDict(dict33), 3000.0, -2f, -2f, RobberB.Rotation.Z - 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_CurrentPaintingNew, RobberBSyncScene, "ver_01_with_painting_exit_ch_prop_vault_painting_01a", LoadDict(dict33), 3000.0, -2f, -2f, RobberB_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
						RobberBCuttingStage = 6;
					}
					if (RobberBCuttingStage == 6 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
					{
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 30f));
						RobberBRobbing = false;
						RobberB_RobberMarker = default(Vector3);
						RobberB.Task.ClearAllImmediately();
						RobberB.Task.ClearAll();
						RobberBCuttingStage = -1;
					}
				}
			}
			if (!(Ped == RobberC))
			{
				return;
			}
			if (RobberCCurrentType != LootType.Painting)
			{
				if (RobberCCurrentType != LootType.GlassCut_Cabinet && RobberCCurrentType != LootType.JewelStoreCabinet)
				{
					if (RobberCCurrentType != LootType.SafetyDepositBoxes)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1024802024 && RobberCCurrentType < LootType.CP_Cash)
						{
							RobberCInitialAnimDone = true;
						}
						if (RobberCInitialAnimDone && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, RobberC, "anim@heists@ornate_bank@grab_cash", "bag_intro", 1) && RobberCSetRob)
						{
							if (RobberCCurrentType == LootType.Trolly_Cash)
							{
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberC_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), RobberC.Position, default(Vector3), false, false);
								RobberC_HoldingLootProp.IsPositionFrozen = true;
								RobberC_HoldingLootProp.IsCollisionEnabled = false;
								RobberC_HoldingLootProp.IsInvincible = true;
								RobberCRobbing = true;
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberC_HoldingLootProp, RobberC, RobberC.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
								RobberCCurrentType = LootType.Trolly_Cash;
								RobberCSetRob = false;
								RobberCInitialAnimDone = false;
							}
							if (RobberCCurrentType == LootType.Trolly_Gold)
							{
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberC_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_gold_bar_01a"), RobberC.Position, default(Vector3), false, false);
								RobberC_HoldingLootProp.IsPositionFrozen = true;
								RobberC_HoldingLootProp.IsCollisionEnabled = false;
								RobberC_HoldingLootProp.IsInvincible = true;
								RobberCRobbing = true;
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberC_HoldingLootProp, RobberC, RobberC.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
								RobberCCurrentType = LootType.Trolly_Gold;
								RobberCSetRob = false;
								RobberCInitialAnimDone = false;
							}
							if (RobberCCurrentType == LootType.Trolly_Diamonds)
							{
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberC_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_vault_dimaondbox_01a"), RobberC.Position, default(Vector3), false, false);
								RobberC_HoldingLootProp.IsPositionFrozen = true;
								RobberC_HoldingLootProp.IsCollisionEnabled = false;
								RobberC_HoldingLootProp.IsInvincible = true;
								RobberCRobbing = true;
								Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberC_HoldingLootProp, RobberC, RobberC.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
								RobberCCurrentType = LootType.Trolly_Diamonds;
								RobberCSetRob = false;
								RobberCInitialAnimDone = false;
							}
							if (RobberCCurrentType == LootType.CP_Cash)
							{
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberCCurrentType = LootType.CP_Cash;
								RobberCSetRob = false;
								RobberCInitialAnimDone = false;
								RobberCWaitingForLastAnim = true;
								return;
							}
							if (RobberCCurrentType == LootType.CP_Gold)
							{
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "grab_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberCCurrentType = LootType.CP_Gold;
								RobberCSetRob = false;
								RobberCInitialAnimDone = false;
								RobberCWaitingForLastAnim = true;
								return;
							}
							if (RobberCCurrentType == LootType.M42_ServerStack)
							{
								string dict34 = "anim@scripted@heist@ig1_table_grab@gold@male@";
								if (RobberCRobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
								{
									dict34 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
								}
								LoadDict(dict34);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(dict34), "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "grab_bag", LoadDict(dict34), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberCCurrentType = LootType.M42_ServerStack;
								RobberCSetRob = false;
								RobberCInitialAnimDone = false;
								RobberCWaitingForLastAnim = true;
								return;
							}
						}
					}
					if (RobberCCurrentType == LootType.SafetyDepositBoxes)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberCSyncScene, 1.25f);
						if (RobberCCurrentLockboxStage == 3)
						{
							LoadDict(RobberCCurrentSDDict);
							RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberCCurrentLockBoxProp.Position.X, RobberCCurrentLockBoxProp.Position.Y, RobberCCurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberCCurrentLootBoxRotation, 2);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, RobberCCurrentSDDict, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "action", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "action_p_m_bag_var22_arm_s", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "action_ch_prop_vault_drill_01a", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberCCurrentLockBoxProp, RobberCSyncScene, "action_ch_prop_ch_sec_cabinet_01" + RobberCCurrentLockBoxType, LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
							RobberCCurrentLockboxStage = 1;
							if (RobberCCurrentLootBagProp != null)
							{
								RobberCCurrentLootBagProp.Delete();
							}
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
						{
							if (RobberCCurrentLockboxStage == 0)
							{
								LoadDict(RobberCCurrentSDDict);
								RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberCCurrentLockBoxProp.Position.X, RobberCCurrentLockBoxProp.Position.Y, RobberCCurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberCCurrentLootBoxRotation, 2);
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, RobberCCurrentSDDict, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "action", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "action_p_m_bag_var22_arm_s", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "action_ch_prop_vault_drill_01a", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberCCurrentLockBoxProp, RobberCSyncScene, "action_ch_prop_ch_sec_cabinet_01" + RobberCCurrentLockBoxType, LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
								RobberCCurrentLockboxStage++;
								if (RobberCCurrentLootBagProp != null)
								{
									RobberCCurrentLootBagProp.Delete();
								}
							}
							else if (RobberCCurrentLockboxStage == 1)
							{
								if (RobberCCurrentLootBagProp != null)
								{
									RobberCCurrentLootBagProp.Delete();
								}
								RobberCCurrentLootBagProp = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), RobberC.Position, false, false);
								Random random4 = new Random();
								int num4 = random4.Next(0, 100);
								if (num4 >= 50)
								{
									RobberCCurrentLootBagProp.IsVisible = false;
									LoadDict(RobberCCurrentSDDict);
									RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberCCurrentLockBoxProp.Position.X, RobberCCurrentLockBoxProp.Position.Y, RobberCCurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberCCurrentLootBoxRotation, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, RobberCCurrentSDDict, "no_reward", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "no_reward", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "no_reward_p_m_bag_var22_arm_s", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "no_reward_ch_prop_vault_drill_01a", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberCCurrentLockBoxProp, RobberCSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01" + RobberCCurrentLockBoxType, LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberCCurrentLockboxStage++;
									if (RobberCCurrentLockbox + 1 < 5)
									{
										RobberCCurrentSDDict = RobberCCurrentSDDict.Replace("lockbox_0" + RobberCCurrentLockbox, "lockbox_0" + (RobberCCurrentLockbox + 1));
									}
									RobberCCurrentLockbox++;
								}
								else
								{
									RobberCCurrentLootBagProp.IsVisible = true;
									LoadDict(RobberCCurrentSDDict);
									RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberCCurrentLockBoxProp.Position.X, RobberCCurrentLockBoxProp.Position.Y, RobberCCurrentLockBoxProp.Position.Z, 0.0, 0.0, RobberCCurrentLootBoxRotation, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, RobberCCurrentSDDict, "reward", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberCCurrentLockBoxProp, RobberCSyncScene, "reward_ch_prop_ch_sec_cabinet_01" + RobberCCurrentLockBoxType, LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "reward_p_m_bag_var22_arm_s", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "reward_ch_prop_vault_drill_01a", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberCCurrentLootBagProp, RobberCSyncScene, "reward_ch_prop_ch_moneybag_01a", LoadDict(RobberCCurrentSDDict), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									RobberCCurrentLockboxStage++;
									if (RobberCCurrentLockbox + 1 < 5)
									{
										RobberCCurrentSDDict = RobberCCurrentSDDict.Replace("lockbox_0" + RobberCCurrentLockbox, "lockbox_0" + (RobberCCurrentLockbox + 1));
									}
									RobberCCurrentLockbox++;
									int min4 = (int)(Heist_Average_Pay / 25f);
									int max4 = (int)(Heist_Average_Pay / 10f);
									increaseCurrentPay(min4, max4);
								}
							}
							else if (RobberCCurrentLockboxStage == 2)
							{
								if (RobberCCurrentLockbox == 5)
								{
									if (RobberCCurrentLootBagProp != null)
									{
										RobberCCurrentLootBagProp.Delete();
									}
									RobberCCurrentLockboxStage = 4;
									RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, 0.0, 0.0, RobberCCurrentLootBoxRotation, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(RobberCCurrentSDDict), "exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene));
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "exit", LoadDict(RobberCCurrentSDDict), 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "exit_ch_prop_vault_drill_01a", LoadDict(RobberCCurrentSDDict), 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "exit_p_m_bag_var22_arm_s", LoadDict(RobberCCurrentSDDict), 1f, -1f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Loot.Loot, RobberCSyncScene, "exit_ch_prop_ch_sec_cabinet_01" + RobberCCurrentLockBoxType, LoadDict(RobberCCurrentSDDict), 1f, -1f, 0, 1148846080);
								}
								else
								{
									if (RobberCCurrentLootBagProp != null)
									{
										RobberCCurrentLootBagProp.Delete();
									}
									RobberCCurrentLockboxStage = 3;
								}
							}
							if (RobberCCurrentLockboxStage == 4)
							{
								RobberCRobbing = false;
								RobberC.Task.ClearAllImmediately();
								RobberCSetRob = false;
								RobberCInitialAnimDone = false;
								RobberCRobbing = false;
								if (RobberC_HoldingLootProp != null)
								{
									RobberC_HoldingLootProp.Delete();
								}
								if (RobberC_bagProp != null)
								{
									RobberC_bagProp.Delete();
								}
							}
						}
					}
					if (RobberCCurrentType == LootType.CP_Gold || RobberCCurrentType == LootType.CP_Cash || RobberCCurrentType == LootType.M42_ServerStack)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
						{
							if (RobberCCurrentType == LootType.CP_Cash && !RobberCFirstAnim)
							{
								RobberCFirstAnim = true;
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberCRobbingProp, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "grab_cash", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberCRobbingProp, RobberCSyncScene, "grab_cash", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 16f, -16f, 0, 1148846080);
								RobberCInitialAnimDone = true;
							}
							if (RobberCCurrentType == LootType.CP_Gold && !RobberCFirstAnim)
							{
								RobberCFirstAnim = true;
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberCRobbingProp, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberCRobbingProp, RobberCSyncScene, "grab_gold", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 16f, -16f, 0, 1148846080);
								RobberCInitialAnimDone = true;
							}
							if (RobberCCurrentType == LootType.M42_ServerStack && !RobberCFirstAnim)
							{
								string dict35 = "anim@scripted@heist@ig1_table_grab@gold@male@";
								if (RobberCRobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
								{
									dict35 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
								}
								LoadDict(dict35);
								RobberCFirstAnim = true;
								Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberCRobbingProp, RobberCSyncScene, LoadDict(dict35), "grab_gold", 1000.0, -4.0, 128, 0, 1148846080, 0);
								Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberCRobbingProp, RobberCSyncScene, "grab_gold", LoadDict(dict35), 16f, -16f, 0, 1148846080);
								RobberCInitialAnimDone = true;
							}
						}
						if (RobberCCurrentType == LootType.CP_Cash && !RobberCSetRob)
						{
							if (RobberCWaitingForLastAnim)
							{
								if (RobberCRobTimer >= IncreasePayPerFrames)
								{
									RobberCRobTimer = 0;
									AddCash(RobberC, (int)(Heist_Average_Pay / 120f), (int)(Heist_Average_Pay / 80f));
								}
								if (RobberCRobTimer != IncreasePayPerFrames)
								{
									RobberCRobTimer++;
								}
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
								{
									RobberCFinishedLastAnim = true;
									RobberCWaitingForLastAnim = false;
									Rot = RobberC.Rotation;
									RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberCRobbingProp.Position.X, RobberCRobbingProp.Position.Y, RobberCRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberCRobbingProp.Position.X, RobberCRobbingProp.Position.Y, RobberCRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
									return;
								}
							}
							if (RobberCFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
							{
								RobberCFinishedLastAnim = false;
								if (RobberC_HoldingLootProp != null)
								{
									RobberC_HoldingLootProp.Delete();
								}
								if (RobberC_bagProp != null)
								{
									RobberC_bagProp.Delete();
								}
								RobberC.Task.ClearAll();
								RobberCRobbing = false;
								RobberC_RobberMarker = default(Vector3);
								return;
							}
						}
						if (RobberCCurrentType == LootType.CP_Gold && !RobberCSetRob)
						{
							if (RobberCWaitingForLastAnim)
							{
								if (RobberARobTimer >= IncreasePayPerFrames)
								{
									RobberCRobTimer = 0;
									AddCash(RobberA, (int)(Heist_Average_Pay / 80f), (int)(Heist_Average_Pay / 60f));
								}
								if (RobberARobTimer != IncreasePayPerFrames)
								{
									RobberCRobTimer++;
								}
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
								{
									RobberCFinishedLastAnim = true;
									RobberCWaitingForLastAnim = false;
									Rot = RobberC.Rotation;
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberCRobbingProp.Position.X, RobberCRobbingProp.Position.Y, RobberCRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberCRobbingProp.Position.X, RobberCRobbingProp.Position.Y, RobberCRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "exit_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "exit", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
									return;
								}
							}
							if (RobberCFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
							{
								RobberCFinishedLastAnim = false;
								if (RobberC_HoldingLootProp != null)
								{
									RobberC_HoldingLootProp.Delete();
								}
								if (RobberC_bagProp != null)
								{
									RobberC_bagProp.Delete();
								}
								RobberC.Task.ClearAll();
								AddCash(RobberC, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 40f));
								RobberCRobbing = false;
								RobberC_RobberMarker = default(Vector3);
								return;
							}
						}
						if (RobberCCurrentType == LootType.M42_ServerStack && !RobberCSetRob)
						{
							if (RobberCWaitingForLastAnim)
							{
								if (RobberARobTimer >= IncreasePayPerFrames)
								{
									RobberCRobTimer = 0;
									AddCash(RobberA, (int)(Heist_Average_Pay / 80f), (int)(Heist_Average_Pay / 60f));
								}
								if (RobberARobTimer != IncreasePayPerFrames)
								{
									RobberCRobTimer++;
								}
								if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
								{
									RobberCFinishedLastAnim = true;
									RobberCWaitingForLastAnim = false;
									string dict36 = "anim@scripted@heist@ig1_table_grab@gold@male@";
									if (RobberCRobbingProp.Model == Function.Call<int>(Hash.GET_HASH_KEY, "anim@scripted@heist@ig1_table_grab@gold@male@"))
									{
										dict36 = "ANIM@SCRIPTED@PLAYER@MISSION@TUN_TABLE_GRAB@GOLD@";
									}
									LoadDict(dict36);
									RobberCFirstAnim = true;
									Rot = RobberC.Rotation;
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, RobberCRobbingProp.Position.X, RobberCRobbingProp.Position.Y, RobberCRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, 2);
									Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, RobberCRobbingProp.Position.X, RobberCRobbingProp.Position.Y, RobberCRobbingProp.Position.Z, 0.0, 0.0, Rot.Z, true);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "exit_bag", LoadDict(dict36), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(dict36), "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "exit", LoadDict(dict36), 1000.0, -8f, -8f, Rot.Z, 1148846080, 0);
									return;
								}
							}
							if (RobberCFinishedLastAnim && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
							{
								RobberCFinishedLastAnim = false;
								if (RobberC_HoldingLootProp != null)
								{
									RobberC_HoldingLootProp.Delete();
								}
								if (RobberC_bagProp != null)
								{
									RobberC_bagProp.Delete();
								}
								RobberC.Task.ClearAll();
								AddCash(RobberC, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 40f));
								RobberCRobbing = false;
								RobberC_RobberMarker = default(Vector3);
								return;
							}
						}
					}
				}
				if (RobberCCurrentType == LootType.JewelStoreCabinet)
				{
					if (RobberCRobbingProp.Model == new Model("des_jewel_cab2_start"))
					{
						glass1 = RobberCRobbingProp;
						jewelcabsmash(RobberC, glass1);
					}
					if (RobberCRobbingProp.Model == new Model("des_jewel_cab3_start"))
					{
						glass2 = RobberCRobbingProp;
						jewelcabsmash2(RobberC, glass2);
					}
					if (RobberCRobbingProp.Model == new Model("des_jewel_cab4_start"))
					{
						glass3 = RobberCRobbingProp;
						jewelcabsmash3(RobberC, glass3);
					}
					if (RobberCRobbingProp.Model == new Model("des_jewel_cab_start"))
					{
						glass4 = RobberCRobbingProp;
						jewelcabsmash4(RobberC, glass4);
					}
				}
			}
			if (RobberCCurrentType == LootType.Painting)
			{
				Vector3 offsetPosition4 = RobberC_CurrentPaintingNew.GetOffsetPosition(new Vector3(0f, -0.45f, -1.15f));
				if (RobberCCuttingStage == 0)
				{
					RobberC.Weapons.Select(WeaponHash.SwitchBlade);
					string dict37 = "anim_heist@hs3f@ig11_steal_painting@male@";
					LoadDict(dict37);
					PlayerCurrentID = PlayerCurrentID;
					RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, RobberC_CurrentPaintingNew.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(dict37), "ver_01_top_left_enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene));
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "ver_01_top_left_enter", LoadDict(dict37), 3000.0, -2f, -2f, RobberC.Rotation.Z - 180f, 1148846080, 0);
					RobberCCuttingStage = 1;
				}
				if (RobberCCuttingStage == 1 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
				{
					RobberC.Weapons.Select(WeaponHash.SwitchBlade);
					string dict38 = "anim_heist@hs3f@ig11_steal_painting@male@";
					LoadDict(dict38);
					RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, RobberC_CurrentPaintingNew.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(dict38), "ver_01_cutting_top_left_to_right", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene));
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "ver_01_cutting_top_left_to_right", LoadDict(dict38), 3000.0, -2f, -2f, RobberC.Rotation.Z - 180f, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_CurrentPaintingNew, RobberCSyncScene, "ver_01_cutting_top_left_to_right_ch_prop_vault_painting_01a", LoadDict(dict38), 3000.0, -2f, -2f, RobberC_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
					RobberCCuttingStage = 2;
				}
				if (RobberCCuttingStage == 2 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
				{
					RobberC.Weapons.Select(WeaponHash.SwitchBlade);
					string dict39 = "anim_heist@hs3f@ig11_steal_painting@male@";
					LoadDict(dict39);
					RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, RobberC_CurrentPaintingNew.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(dict39), "ver_01_cutting_right_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene));
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "ver_01_cutting_right_top_to_bottom", LoadDict(dict39), 3000.0, -2f, -2f, RobberC.Rotation.Z - 180f, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_CurrentPaintingNew, RobberCSyncScene, "ver_01_cutting_right_top_to_bottom_ch_prop_vault_painting_01a", LoadDict(dict39), 3000.0, -2f, -2f, RobberC_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
					RobberCCuttingStage = 3;
				}
				if (RobberCCuttingStage == 3 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
				{
					RobberC.Weapons.Select(WeaponHash.SwitchBlade);
					string dict40 = "anim_heist@hs3f@ig11_steal_painting@male@";
					LoadDict(dict40);
					RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, RobberC_CurrentPaintingNew.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(dict40), "ver_01_cutting_bottom_right_to_left", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene));
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "ver_01_cutting_bottom_right_to_left", LoadDict(dict40), 3000.0, -2f, -2f, RobberC.Rotation.Z - 180f, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_CurrentPaintingNew, RobberCSyncScene, "ver_01_cutting_bottom_right_to_left_ch_prop_vault_painting_01a", LoadDict(dict40), 3000.0, -2f, -2f, RobberC_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
					RobberCCuttingStage = 4;
				}
				if (RobberCCuttingStage == 4 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
				{
					RobberC.Weapons.Select(WeaponHash.SwitchBlade);
					string dict41 = "anim_heist@hs3f@ig11_steal_painting@male@";
					LoadDict(dict41);
					RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, RobberC_CurrentPaintingNew.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(dict41), "ver_01_re-enter_cutting_left_top_to_bottom", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene));
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "ver_01_re-enter_cutting_left_top_to_bottom", LoadDict(dict41), 3000.0, -2f, -2f, RobberC.Rotation.Z - 180f, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_CurrentPaintingNew, RobberCSyncScene, "ver_01_re-enter_cutting_left_top_to_bottom_ch_prop_vault_painting_01a ", LoadDict(dict41), 3000.0, -2f, -2f, RobberC_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
					RobberCCuttingStage = 5;
				}
				if (RobberCCuttingStage == 5 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
				{
					string dict42 = "anim_heist@hs3f@ig11_steal_painting@male@";
					LoadDict(dict42);
					RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, RobberC_CurrentPaintingNew.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(dict42), "ver_01_with_painting_exit", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene));
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "ver_01_with_painting_exit", LoadDict(dict42), 3000.0, -2f, -2f, RobberC.Rotation.Z - 180f, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_CurrentPaintingNew, RobberCSyncScene, "ver_01_with_painting_exit_ch_prop_vault_painting_01a", LoadDict(dict42), 3000.0, -2f, -2f, RobberC_CurrentPaintingNew.Rotation.Z - 180f, 1148846080, 0);
					RobberCCuttingStage = 6;
				}
				if (RobberCCuttingStage == 6 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
				{
					AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 20f), (int)(Heist_Average_Pay / 30f));
					RobberCRobbing = false;
					RobberC_RobberMarker = default(Vector3);
					RobberC.Task.ClearAllImmediately();
					RobberC.Task.ClearAll();
					RobberCCuttingStage = -1;
				}
			}
		}

		public static void CheckRobberyAnimations()
		{
			if (RobberARobbing && RobberA != null && RobberACurrentType < LootType.CP_Cash)
			{
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) <= 1065353216)
				{
					if (RobberA_HoldingLootProp != null)
					{
						RobberA.Opacity = 255;
						Crate_Valuables[RobberACurrentValualabe].IsRobbed = true;
						RobberAMovingToLoot = false;
						RobberA_CashGrabTimer++;
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberA_HoldingLootProp, RobberA, RobberA.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
					}
					if (RobberA_CashGrabTimer == 40 && RobberA_HoldingLootProp != null)
					{
						RobberA_HoldingLootProp.IsVisible = false;
						CalculatePay(RobberA, RobberACurrentType);
					}
					if (RobberA_CashGrabTimer == 50)
					{
						RobberA_HoldingLootProp.Delete();
						Model model = new Model("ch_prop_20dollar_pile_01a");
						if (RobberACurrentType == LootType.Trolly_Cash)
						{
							model = new Model("ch_prop_20dollar_pile_01a");
							model.Request(250);
						}
						if (RobberACurrentType == LootType.Trolly_Gold)
						{
							model = new Model("ch_prop_gold_bar_01a");
							model.Request(250);
						}
						if (RobberACurrentType == LootType.Trolly_Diamonds)
						{
							model = new Model("ch_prop_vault_dimaondbox_01a");
							model.Request(250);
						}
						RobberA_HoldingLootProp = World.CreateProp(model, RobberA.Position, default(Vector3), false, false);
						RobberA_HoldingLootProp.IsPositionFrozen = true;
						RobberA_HoldingLootProp.IsCollisionEnabled = false;
						RobberA_HoldingLootProp.IsInvincible = true;
						model.MarkAsNoLongerNeeded();
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberA_HoldingLootProp, RobberA, RobberA.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
						RobberA_CashGrabTimer = 0;
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene) >= 1065353216)
				{
					if (RobberA_HoldingLootProp != null)
					{
						RobberA_HoldingLootProp.Delete();
					}
					if (RobberA_bagProp != null)
					{
						RobberA_bagProp.Delete();
					}
					RobberA_RobberMarker = new Vector3(0f, 0f, 0f);
					RobberARobbing = false;
					RobberA.Task.ClearAll();
				}
			}
			if (RobberBRobbing && RobberB != null && RobberBCurrentType < LootType.CP_Cash)
			{
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) <= 1065353216)
				{
					if (RobberB_HoldingLootProp != null)
					{
						RobberB.Opacity = 255;
						Crate_Valuables[RobberBCurrentValualabe].IsRobbed = true;
						RobberBMovingToLoot = false;
						RobberB_CashGrabTimer++;
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberB_HoldingLootProp, RobberB, RobberB.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
					}
					if (RobberB_CashGrabTimer == 40 && RobberB_HoldingLootProp != null)
					{
						RobberB_HoldingLootProp.IsVisible = false;
						CalculatePay(RobberB, RobberBCurrentType);
					}
					if (RobberB_CashGrabTimer == 50)
					{
						RobberB_HoldingLootProp.Delete();
						Model model2 = new Model("ch_prop_20dollar_pile_01a");
						if (RobberBCurrentType == LootType.Trolly_Cash)
						{
							model2 = new Model("ch_prop_20dollar_pile_01a");
							model2.Request(250);
						}
						if (RobberBCurrentType == LootType.Trolly_Gold)
						{
							model2 = new Model("ch_prop_gold_bar_01a");
							model2.Request(250);
						}
						if (RobberBCurrentType == LootType.Trolly_Diamonds)
						{
							model2 = new Model("ch_prop_vault_dimaondbox_01a");
							model2.Request(250);
						}
						RobberB_HoldingLootProp = World.CreateProp(model2, RobberB.Position, default(Vector3), false, false);
						RobberB_HoldingLootProp.IsPositionFrozen = true;
						RobberB_HoldingLootProp.IsCollisionEnabled = false;
						RobberB_HoldingLootProp.IsInvincible = true;
						model2.MarkAsNoLongerNeeded();
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberB_HoldingLootProp, RobberB, RobberB.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
						RobberB_CashGrabTimer = 0;
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene) >= 1065353216)
				{
					if (RobberB_HoldingLootProp != null)
					{
						RobberB_HoldingLootProp.Delete();
					}
					if (RobberB_bagProp != null)
					{
						RobberB_bagProp.Delete();
					}
					RobberB_RobberMarker = new Vector3(0f, 0f, 0f);
					RobberBRobbing = false;
					RobberB.Task.ClearAll();
				}
			}
			if (RobberCRobbing && RobberC != null && (RobberCCurrentType == LootType.Trolly_Cash || RobberCCurrentType == LootType.Trolly_Gold || RobberCCurrentType == LootType.Trolly_Diamonds || RobberCCurrentType == LootType.Trolly_Coke))
			{
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) <= 1065353216)
				{
					if (RobberC_HoldingLootProp != null)
					{
						RobberC.Opacity = 255;
						Crate_Valuables[RobberCCurrentValualabe].IsRobbed = true;
						RobberCMovingToLoot = false;
						RobberC_CashGrabTimer++;
						RobberC_HoldingLootProp.Rotation = new Vector3(90f, 90f, 0f);
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberC_HoldingLootProp, RobberC, RobberC.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
					}
					if (RobberC_CashGrabTimer == 40 && RobberC_HoldingLootProp != null)
					{
						RobberC_HoldingLootProp.IsVisible = false;
						CalculatePay(RobberC, RobberCCurrentType);
					}
					if (RobberC_CashGrabTimer == 50)
					{
						RobberC_HoldingLootProp.Delete();
						Model model3 = new Model("ch_prop_20dollar_pile_01a");
						if (RobberCCurrentType == LootType.Trolly_Cash)
						{
							model3 = new Model("ch_prop_20dollar_pile_01a");
							model3.Request(250);
						}
						if (RobberCCurrentType == LootType.Trolly_Gold)
						{
							model3 = new Model("ch_prop_gold_bar_01a");
							model3.Request(250);
						}
						if (RobberCCurrentType == LootType.Trolly_Diamonds)
						{
							model3 = new Model("ch_prop_vault_dimaondbox_01a");
							model3.Request(250);
						}
						RobberC_HoldingLootProp = World.CreateProp(model3, RobberC.Position, default(Vector3), false, false);
						RobberC_HoldingLootProp.IsPositionFrozen = true;
						RobberC_HoldingLootProp.IsCollisionEnabled = false;
						RobberC_HoldingLootProp.IsInvincible = true;
						model3.MarkAsNoLongerNeeded();
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, RobberC_HoldingLootProp, RobberC, RobberC.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
						RobberC_CashGrabTimer = 0;
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene) >= 1065353216)
				{
					if (RobberC_HoldingLootProp != null)
					{
						RobberC_HoldingLootProp.Delete();
					}
					if (RobberC_bagProp != null)
					{
						RobberC_bagProp.Delete();
					}
					RobberC_RobberMarker = new Vector3(0f, 0f, 0f);
					RobberCRobbing = false;
					RobberC.Task.ClearAll();
				}
			}
			if (!PlayingAnim)
			{
				return;
			}
			if (CurrentType == LootType.CP_Safe)
			{
				Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
				{
					Script.Wait(1);
					Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
					if (ShowCrackSafeMSGsDebug)
					{
						drawText("Attempting to Load Scaleform Movie 'DIGITAL_SAFE_DISPLAY'", 0.1f, 0.25f, 0.5f, 255, 255, 255);
					}
				}
				if (ShowCrackSafeMSGsDebug)
				{
					drawText("Showing Scaleform? " + ShowingSafeScaleform + " | LKoaded Scaleform? " + Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9), 0.1f, 0.1f, 0.5f, 255, 255, 255);
				}
				if (ShowingSafeScaleform)
				{
					if (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
					{
						string text = ((int)InputedNumber1).ToString();
						if (InputedNumber1 < 10f)
						{
							text = "0" + (int)InputedNumber1;
						}
						string text2 = ((int)InputedNumber2).ToString();
						if (InputedNumber2 < 10f)
						{
							text2 = "0" + (int)InputedNumber2;
						}
						string text3 = ((int)InputedNumber3).ToString();
						if (InputedNumber3 < 10f)
						{
							text3 = "0" + (int)InputedNumber3;
						}
						drawText(text + "   |   " + text2 + "   |   " + text3, 0.48f, 0.68f, 0.65f, 255, 255, 255);
						drawSprite2("timerbars", "all_black_bg", 0.5f, 0.7f, 0.2f, 0.1f, 255, 255, 255, 130);
					}
					if (Game.IsControlJustPressed(Control.Context) && ShowingSafeScaleform)
					{
						PlayingAnim = false;
						Random random = new Random();
						if (!UseSetSafeCode)
						{
							CodeNumber1 = random.Next(1, 99);
							CodeNumber2 = random.Next(1, 99);
							CodeNumber3 = random.Next(1, 99);
						}
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
						string dict = "anim@scripted@heist@ig15_safe_crack@male@";
						LoadDict(dict);
						Vector3 rotation = Safe.Rotation;
						Vector3 offsetPosition = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, rotation.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, rotation.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "exit_safe", LoadDict(dict), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "exit_bag", LoadDict(dict), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict), "exit_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "exit_player", LoadDict(dict), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Script.Wait(1000);
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
						Ped character = Game.Player.Character;
						SetBagOn();
						Game.Player.Character.Task.ClearAllImmediately();
						if (bagProp != null)
						{
							bagProp.Delete();
							bagProp.MarkAsNoLongerNeeded();
							bagProp = null;
						}
						ReadyForCode = false;
						ShowingSafeScaleform = false;
						GotIncorrectSafeCode = false;
						GotCorrectSafeCode = false;
						PlayingSafeFinalAnim = false;
						FinishedSafeAnimations = false;
						PlayingIdle = false;
						Script.Wait(1);
					}
					if (ShowingSafeScaleform)
					{
						ShowSafeButtons();
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Quit, ~INPUT_ATTACK~ to Check Code");
						while (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "safe_01a"))
						{
							Script.Wait(1);
							Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "safe_01a", 0);
						}
						while (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 1098122770))
						{
							Script.Wait(1);
							Function.Call(Hash.LINK_NAMED_RENDERTARGET, 1098122770);
						}
						Local_26709f_8 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "safe_01a");
						if (ShowCrackSafeMSGsDebug)
						{
							drawText("Safe Reg? " + Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "safe_01a"), 0.1f, 0.15f, 0.5f, 255, 255, 255);
							drawText("Safe Link? " + Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 1098122770), 0.1f, 0.25f, 0.5f, 255, 255, 255);
							drawText("FinishedSafeAnimations " + FinishedSafeAnimations, 0.1f, 0.3f, 0.5f, 255, 255, 255);
							drawText("ReadyForCode " + ReadyForCode, 0.1f, 0.35f, 0.5f, 255, 255, 255);
						}
						if (!FinishedSafeAnimations)
						{
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
							bool flag = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "safe_01a");
							bool flag2 = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 1098122770);
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216 && !ReadyForCode)
							{
								ReadyForCode = true;
							}
							if (ShowCrackSafeMSGsDebug)
							{
								drawText("ScaleformLoaded " + Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9), 0.1f, 0.4f, 0.5f, 255, 255, 255);
							}
							if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
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
											SafeValuables = World.CreateProp(RequestModel("h4_prop_h4_file_cylinder_01a"), Safe.Position, new Vector3(0f, 0f, Safe.Heading), false, false);
											GotIncorrectSafeCode = false;
											GotCorrectSafeCode = true;
											PlayingIdle = false;
											string dict2 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict2);
											Vector3 rotation2 = Safe.Rotation;
											Vector3 offsetPosition2 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, rotation2.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, rotation2.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "door_open_safe", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "door_open_bag", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SafeValuables, SafeScene1, "door_open_file_cylinder", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict2), "door_open_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "door_open_player", LoadDict(dict2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
										}
										else
										{
											GotIncorrectSafeCode = true;
											GotCorrectSafeCode = false;
											PlayingIdle = false;
											string dict3 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict3);
											Vector3 rotation3 = Safe.Rotation;
											Vector3 offsetPosition3 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, rotation3.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, rotation3.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "fail_safe", LoadDict(dict3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "fail_bag", LoadDict(dict3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict3), "fail_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "fail_player", LoadDict(dict3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
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
										SetBagOn();
										Game.Player.Character.Task.ClearAllImmediately();
										PlayingAnim = false;
										Notification.Show("Finish Final Anim");
									}
									if (GotCorrectSafeCode && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										GotCorrectSafeCode = false;
										PlayingSafeFinalAnim = true;
										PlayingIdle = false;
										string dict4 = "anim@scripted@heist@ig15_safe_crack@male@";
										LoadDict(dict4);
										Vector3 rotation4 = Safe.Rotation;
										Vector3 offsetPosition4 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation4.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation4.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "success_with_metal_cylinder_safe", LoadDict(dict4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "success_with_metal_cylinder_bag", LoadDict(dict4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SafeValuables, SafeScene1, "success_with_metal_cylinder_file_cylinder", LoadDict(dict4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict4), "success_with_metal_cylinder_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "success_with_metal_cylinder_player", LoadDict(dict4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
										Crate_Valuables[CurrentLootTarget].IsRobbed = true;
										if (!OverridePay)
										{
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 40f), (int)(Heist_Average_Pay / 60f));
										}
										if (OverridePay)
										{
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 2f), (int)(Heist_Average_Pay / 1.25f));
										}
									}
									if (GotIncorrectSafeCode && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										GotCorrectSafeCode = false;
										GotIncorrectSafeCode = false;
									}
									if (!GotCorrectSafeCode && !GotIncorrectSafeCode && !PlayingSafeFinalAnim && !Game.IsControlPressed(Control.PhoneUp) && !Game.IsControlPressed(Control.PhoneDown) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										PlayingIdle = true;
										string dict5 = "anim@scripted@heist@ig15_safe_crack@male@";
										LoadDict(dict5);
										Vector3 rotation5 = Safe.Rotation;
										Vector3 offsetPosition5 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition5.X, offsetPosition5.Y, offsetPosition5.Z, 0.0, 0.0, rotation5.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition5.X, offsetPosition5.Y, offsetPosition5.Z, 0.0, 0.0, rotation5.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "idle_safe", LoadDict(dict5), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "idle_bag", LoadDict(dict5), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict5), "idle_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "idle_player", LoadDict(dict5), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
									}
									if (Game.IsControlPressed(Control.PhoneUp))
									{
										if (SelectedNum == 0)
										{
											InputedNumber1 += 0.15f;
											if (InputedNumber1 > 99f)
											{
												InputedNumber1 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber1 == CodeNumber1)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 1)
										{
											InputedNumber2 += 0.15f;
											if (InputedNumber2 > 99f)
											{
												InputedNumber2 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber2 == CodeNumber2)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 2)
										{
											InputedNumber3 += 0.15f;
											if (InputedNumber3 > 99f)
											{
												InputedNumber3 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber3 == CodeNumber3)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (!PlayingIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
										{
											string dict6 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict6);
											Vector3 rotation6 = Safe.Rotation;
											Vector3 offsetPosition6 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition6.X, offsetPosition6.Y, offsetPosition6.Z, 0.0, 0.0, rotation6.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition6.X, offsetPosition6.Y, offsetPosition6.Z, 0.0, 0.0, rotation6.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_left_safe", LoadDict(dict6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_left_bag", LoadDict(dict6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict6), "input_safe_code_left_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_left_player", LoadDict(dict6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
										}
										if (PlayingIdle)
										{
											PlayingIdle = false;
											string dict7 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict7);
											Vector3 rotation7 = Safe.Rotation;
											Vector3 offsetPosition7 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition7.X, offsetPosition7.Y, offsetPosition7.Z, 0.0, 0.0, rotation7.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition7.X, offsetPosition7.Y, offsetPosition7.Z, 0.0, 0.0, rotation7.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_left_safe", LoadDict(dict7), 1000.0, -8f, -8f, rotation7.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_left_bag", LoadDict(dict7), 1000.0, -8f, -8f, rotation7.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict7), "input_safe_code_left_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_left_player", LoadDict(dict7), 1000.0, -8f, -8f, rotation7.Z + 180f, 1148846080, 0);
										}
									}
									if (Game.IsControlPressed(Control.PhoneDown))
									{
										if (SelectedNum == 0)
										{
											InputedNumber1 -= 0.15f;
											if (InputedNumber1 < 0f)
											{
												InputedNumber1 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber1 == CodeNumber1)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 1)
										{
											InputedNumber2 -= 0.15f;
											if (InputedNumber2 < 0f)
											{
												InputedNumber2 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber2 == CodeNumber2)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 2)
										{
											InputedNumber3 -= 0.15f;
											if (InputedNumber3 < 0f)
											{
												InputedNumber3 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber3 == CodeNumber3)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (!PlayingIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
										{
											string dict8 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict8);
											Vector3 rotation8 = Safe.Rotation;
											Vector3 offsetPosition8 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition8.X, offsetPosition8.Y, offsetPosition8.Z, 0.0, 0.0, rotation8.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition8.X, offsetPosition8.Y, offsetPosition8.Z, 0.0, 0.0, rotation8.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_right_safe", LoadDict(dict8), 1000.0, -8f, -8f, rotation8.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_right_bag", LoadDict(dict8), 1000.0, -8f, -8f, rotation8.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict8), "input_safe_code_right_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_right_player", LoadDict(dict8), 1000.0, -8f, -8f, rotation8.Z + 180f, 1148846080, 0);
										}
										if (PlayingIdle)
										{
											PlayingIdle = false;
											string dict9 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict9);
											Vector3 rotation9 = Safe.Rotation;
											Vector3 offsetPosition9 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition9.X, offsetPosition9.Y, offsetPosition9.Z, 0.0, 0.0, rotation9.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition9.X, offsetPosition9.Y, offsetPosition9.Z, 0.0, 0.0, rotation9.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_right_safe", LoadDict(dict9), 1000.0, -8f, -8f, rotation9.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_right_bag", LoadDict(dict9), 1000.0, -8f, -8f, rotation9.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict9), "input_safe_code_right_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_right_player", LoadDict(dict9), 1000.0, -8f, -8f, rotation9.Z + 180f, 1148846080, 0);
										}
									}
									if (Game.IsControlJustPressed(Control.PhoneLeft))
									{
										Notification.Show("Move Left");
										SelectedNum--;
										if (SelectedNum < 0)
										{
											Notification.Show("Left Over" + SelectedNum);
											SelectedNum = 2;
										}
										else
										{
											Notification.Show("Move Left");
										}
										return;
									}
									if (Game.IsControlJustPressed(Control.PhoneRight))
									{
										SelectedNum++;
										if (SelectedNum >= 3)
										{
											Notification.Show("Right Over" + SelectedNum);
											SelectedNum = 0;
										}
										else
										{
											Notification.Show("Move Right");
										}
										return;
									}
								}
								if (CodeNumber1 == 0 && CodeNumber2 == 0 && CodeNumber3 == 0)
								{
									Random random2 = new Random();
									if (!UseSetSafeCode)
									{
										CodeNumber1 = random2.Next(1, 99);
										CodeNumber2 = random2.Next(1, 99);
										CodeNumber3 = random2.Next(1, 99);
									}
								}
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("Local_26709f_8 " + Local_26709f_8, 0.1f, 0.45f, 0.5f, 255, 255, 255);
								}
								if (Local_26709f_8 == -1)
								{
									func_4437(1098122770u, "safe_01a");
									return;
								}
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("ReadyForCode (2)" + ReadyForCode, 0.1f, 0.5f, 0.5f, 255, 255, 255);
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
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("Finished Render Que", 0.1f, 0.55f, 0.5f, 255, 255, 255);
								}
								if (ShowSecondarySafeUI)
								{
									string text4 = ((int)InputedNumber1).ToString();
									if (InputedNumber1 < 10f)
									{
										text4 = "0" + (int)InputedNumber1;
									}
									string text5 = ((int)InputedNumber2).ToString();
									if (InputedNumber2 < 10f)
									{
										text5 = "0" + (int)InputedNumber2;
									}
									string text6 = ((int)InputedNumber3).ToString();
									if (InputedNumber3 < 10f)
									{
										text6 = "0" + (int)InputedNumber3;
									}
									drawText(text4 + "   |   " + text5 + "   |   " + text6, 0.48f, 0.68f, 0.65f, 255, 255, 255);
									drawSprite2("timerbars", "all_black_bg", 0.5f, 0.7f, 0.2f, 0.1f, 255, 255, 255, 130);
								}
							}
						}
					}
				}
			}
			if (CurrentType == LootType.XM3_Safe)
			{
				Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
				{
					Script.Wait(1);
					Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
					if (ShowCrackSafeMSGsDebug)
					{
						drawText("Attempting to Load Scaleform Movie 'DIGITAL_SAFE_DISPLAY'", 0.1f, 0.25f, 0.5f, 255, 255, 255);
					}
				}
				if (ShowCrackSafeMSGsDebug)
				{
					drawText("Showing Scaleform? " + ShowingSafeScaleform + " | LKoaded Scaleform? " + Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9), 0.1f, 0.1f, 0.5f, 255, 255, 255);
				}
				if (ShowingSafeScaleform)
				{
					if (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
					{
						string text7 = ((int)InputedNumber1).ToString();
						if (InputedNumber1 < 10f)
						{
							text7 = "0" + (int)InputedNumber1;
						}
						string text8 = ((int)InputedNumber2).ToString();
						if (InputedNumber2 < 10f)
						{
							text8 = "0" + (int)InputedNumber2;
						}
						string text9 = ((int)InputedNumber3).ToString();
						if (InputedNumber3 < 10f)
						{
							text9 = "0" + (int)InputedNumber3;
						}
						drawText(text7 + "   |   " + text8 + "   |   " + text9, 0.48f, 0.68f, 0.65f, 255, 255, 255);
						drawSprite2("timerbars", "all_black_bg", 0.5f, 0.7f, 0.2f, 0.1f, 255, 255, 255, 130);
					}
					if (Game.IsControlJustPressed(Control.Context) && ShowingSafeScaleform)
					{
						PlayingAnim = false;
						Random random3 = new Random();
						if (!UseSetSafeCode)
						{
							CodeNumber1 = random3.Next(1, 99);
							CodeNumber2 = random3.Next(1, 99);
							CodeNumber3 = random3.Next(1, 99);
						}
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
						string dict10 = "anim@scripted@heist@ig15_safe_crack@male@";
						LoadDict(dict10);
						Vector3 rotation10 = Safe.Rotation;
						Vector3 offsetPosition10 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition10.X, offsetPosition10.Y, offsetPosition10.Z, 0.0, 0.0, rotation10.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition10.X, offsetPosition10.Y, offsetPosition10.Z, 0.0, 0.0, rotation10.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "exit_safe", LoadDict(dict10), 1000.0, -8f, -8f, rotation10.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "exit_bag", LoadDict(dict10), 1000.0, -8f, -8f, rotation10.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict10), "exit_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "exit_player", LoadDict(dict10), 1000.0, -8f, -8f, rotation10.Z + 180f, 1148846080, 0);
						Script.Wait(1000);
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
						Ped character3 = Game.Player.Character;
						SetBagOn();
						Game.Player.Character.Task.ClearAllImmediately();
						if (bagProp != null)
						{
							bagProp.Delete();
							bagProp.MarkAsNoLongerNeeded();
							bagProp = null;
						}
						ReadyForCode = false;
						ShowingSafeScaleform = false;
						GotIncorrectSafeCode = false;
						GotCorrectSafeCode = false;
						PlayingSafeFinalAnim = false;
						FinishedSafeAnimations = false;
						PlayingIdle = false;
						Script.Wait(1);
					}
					if (ShowingSafeScaleform)
					{
						ShowSafeButtons();
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Quit, ~INPUT_ATTACK~ to Check Code");
						while (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "xm3_safe_01a"))
						{
							Script.Wait(1);
							Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "xm3_safe_01a", 0);
							drawText("Attempting to Link Named Render Target 'xm3_safe_01a'", 0.1f, 0.25f, 0.5f, 255, 255, 255);
						}
						while (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 3616325110u))
						{
							Script.Wait(1);
							Function.Call(Hash.LINK_NAMED_RENDERTARGET, 3616325110u);
							drawText("Attempting to Link Named Render Target '3616325110'", 0.1f, 0.25f, 0.5f, 255, 255, 255);
						}
						Local_26709f_8 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "xm3_safe_01a");
						if (ShowCrackSafeMSGsDebug)
						{
							drawText("Safe Reg? " + Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "xm3_safe_01a"), 0.1f, 0.15f, 0.5f, 255, 255, 255);
							drawText("Safe Link? " + Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 3616325110u), 0.1f, 0.25f, 0.5f, 255, 255, 255);
							drawText("FinishedSafeAnimations " + FinishedSafeAnimations, 0.1f, 0.3f, 0.5f, 255, 255, 255);
							drawText("ReadyForCode " + ReadyForCode, 0.1f, 0.35f, 0.5f, 255, 255, 255);
						}
						if (!FinishedSafeAnimations)
						{
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
							bool flag3 = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "xm3_safe_01a");
							bool flag4 = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 3616325110u);
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216 && !ReadyForCode)
							{
								ReadyForCode = true;
							}
							if (ShowCrackSafeMSGsDebug)
							{
								drawText("ScaleformLoaded " + Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9), 0.1f, 0.4f, 0.5f, 255, 255, 255);
							}
							if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
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
											SafeValuables = World.CreateProp(RequestModel("h4_prop_h4_file_cylinder_01a"), Safe.Position, new Vector3(0f, 0f, Safe.Heading), false, false);
											GotIncorrectSafeCode = false;
											GotCorrectSafeCode = true;
											PlayingIdle = false;
											string dict11 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict11);
											Vector3 rotation11 = Safe.Rotation;
											Vector3 offsetPosition11 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition11.X, offsetPosition11.Y, offsetPosition11.Z, 0.0, 0.0, rotation11.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition11.X, offsetPosition11.Y, offsetPosition11.Z, 0.0, 0.0, rotation11.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "door_open_safe", LoadDict(dict11), 1000.0, -8f, -8f, rotation11.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "door_open_bag", LoadDict(dict11), 1000.0, -8f, -8f, rotation11.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SafeValuables, SafeScene1, "door_open_file_cylinder", LoadDict(dict11), 1000.0, -8f, -8f, rotation11.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict11), "door_open_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "door_open_player", LoadDict(dict11), 1000.0, -8f, -8f, rotation11.Z + 180f, 1148846080, 0);
										}
										else
										{
											GotIncorrectSafeCode = true;
											GotCorrectSafeCode = false;
											PlayingIdle = false;
											string dict12 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict12);
											Vector3 rotation12 = Safe.Rotation;
											Vector3 offsetPosition12 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition12.X, offsetPosition12.Y, offsetPosition12.Z, 0.0, 0.0, rotation12.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition12.X, offsetPosition12.Y, offsetPosition12.Z, 0.0, 0.0, rotation12.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "fail_safe", LoadDict(dict12), 1000.0, -8f, -8f, rotation12.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "fail_bag", LoadDict(dict12), 1000.0, -8f, -8f, rotation12.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict12), "fail_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "fail_player", LoadDict(dict12), 1000.0, -8f, -8f, rotation12.Z + 180f, 1148846080, 0);
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
										Ped character4 = Game.Player.Character;
										SetBagOn();
										Game.Player.Character.Task.ClearAllImmediately();
										PlayingAnim = false;
										Notification.Show("Finish Final Anim");
									}
									if (GotCorrectSafeCode && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										GotCorrectSafeCode = false;
										PlayingSafeFinalAnim = true;
										PlayingIdle = false;
										string dict13 = "anim@scripted@heist@ig15_safe_crack@male@";
										LoadDict(dict13);
										Vector3 rotation13 = Safe.Rotation;
										Vector3 offsetPosition13 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition13.X, offsetPosition13.Y, offsetPosition13.Z, 0.0, 0.0, rotation13.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition13.X, offsetPosition13.Y, offsetPosition13.Z, 0.0, 0.0, rotation13.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "success_with_metal_cylinder_safe", LoadDict(dict13), 1000.0, -8f, -8f, rotation13.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "success_with_metal_cylinder_bag", LoadDict(dict13), 1000.0, -8f, -8f, rotation13.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SafeValuables, SafeScene1, "success_with_metal_cylinder_file_cylinder", LoadDict(dict13), 1000.0, -8f, -8f, rotation13.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict13), "success_with_metal_cylinder_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "success_with_metal_cylinder_player", LoadDict(dict13), 1000.0, -8f, -8f, rotation13.Z + 180f, 1148846080, 0);
										Crate_Valuables[CurrentLootTarget].IsRobbed = true;
										if (!OverridePay)
										{
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 40f), (int)(Heist_Average_Pay / 60f));
										}
										if (OverridePay)
										{
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 2f), (int)(Heist_Average_Pay / 1.25f));
										}
									}
									if (GotIncorrectSafeCode && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										GotCorrectSafeCode = false;
										GotIncorrectSafeCode = false;
									}
									if (!GotCorrectSafeCode && !GotIncorrectSafeCode && !PlayingSafeFinalAnim && !Game.IsControlPressed(Control.PhoneUp) && !Game.IsControlPressed(Control.PhoneDown) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										PlayingIdle = true;
										string dict14 = "anim@scripted@heist@ig15_safe_crack@male@";
										LoadDict(dict14);
										Vector3 rotation14 = Safe.Rotation;
										Vector3 offsetPosition14 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition14.X, offsetPosition14.Y, offsetPosition14.Z, 0.0, 0.0, rotation14.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition14.X, offsetPosition14.Y, offsetPosition14.Z, 0.0, 0.0, rotation14.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "idle_safe", LoadDict(dict14), 1000.0, -8f, -8f, rotation14.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "idle_bag", LoadDict(dict14), 1000.0, -8f, -8f, rotation14.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict14), "idle_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "idle_player", LoadDict(dict14), 1000.0, -8f, -8f, rotation14.Z + 180f, 1148846080, 0);
									}
									if (Game.IsControlPressed(Control.PhoneUp))
									{
										if (SelectedNum == 0)
										{
											InputedNumber1 += 0.15f;
											if (InputedNumber1 > 99f)
											{
												InputedNumber1 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber1 == CodeNumber1)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 1)
										{
											InputedNumber2 += 0.15f;
											if (InputedNumber2 > 99f)
											{
												InputedNumber2 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber2 == CodeNumber2)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 2)
										{
											InputedNumber3 += 0.15f;
											if (InputedNumber3 > 99f)
											{
												InputedNumber3 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber3 == CodeNumber3)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (!PlayingIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
										{
											string dict15 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict15);
											Vector3 rotation15 = Safe.Rotation;
											Vector3 offsetPosition15 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition15.X, offsetPosition15.Y, offsetPosition15.Z, 0.0, 0.0, rotation15.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition15.X, offsetPosition15.Y, offsetPosition15.Z, 0.0, 0.0, rotation15.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_left_safe", LoadDict(dict15), 1000.0, -8f, -8f, rotation15.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_left_bag", LoadDict(dict15), 1000.0, -8f, -8f, rotation15.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict15), "input_safe_code_left_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_left_player", LoadDict(dict15), 1000.0, -8f, -8f, rotation15.Z + 180f, 1148846080, 0);
										}
										if (PlayingIdle)
										{
											PlayingIdle = false;
											string dict16 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict16);
											Vector3 rotation16 = Safe.Rotation;
											Vector3 offsetPosition16 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition16.X, offsetPosition16.Y, offsetPosition16.Z, 0.0, 0.0, rotation16.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition16.X, offsetPosition16.Y, offsetPosition16.Z, 0.0, 0.0, rotation16.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_left_safe", LoadDict(dict16), 1000.0, -8f, -8f, rotation16.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_left_bag", LoadDict(dict16), 1000.0, -8f, -8f, rotation16.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict16), "input_safe_code_left_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_left_player", LoadDict(dict16), 1000.0, -8f, -8f, rotation16.Z + 180f, 1148846080, 0);
										}
									}
									if (Game.IsControlPressed(Control.PhoneDown))
									{
										if (SelectedNum == 0)
										{
											InputedNumber1 -= 0.15f;
											if (InputedNumber1 < 0f)
											{
												InputedNumber1 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber1 == CodeNumber1)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 1)
										{
											InputedNumber2 -= 0.15f;
											if (InputedNumber2 < 0f)
											{
												InputedNumber2 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber2 == CodeNumber2)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 2)
										{
											InputedNumber3 -= 0.15f;
											if (InputedNumber3 < 0f)
											{
												InputedNumber3 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber3 == CodeNumber3)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (!PlayingIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
										{
											string dict17 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict17);
											Vector3 rotation17 = Safe.Rotation;
											Vector3 offsetPosition17 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition17.X, offsetPosition17.Y, offsetPosition17.Z, 0.0, 0.0, rotation17.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition17.X, offsetPosition17.Y, offsetPosition17.Z, 0.0, 0.0, rotation17.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_right_safe", LoadDict(dict17), 1000.0, -8f, -8f, rotation17.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_right_bag", LoadDict(dict17), 1000.0, -8f, -8f, rotation17.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict17), "input_safe_code_right_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_right_player", LoadDict(dict17), 1000.0, -8f, -8f, rotation17.Z + 180f, 1148846080, 0);
										}
										if (PlayingIdle)
										{
											PlayingIdle = false;
											string dict18 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict18);
											Vector3 rotation18 = Safe.Rotation;
											Vector3 offsetPosition18 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition18.X, offsetPosition18.Y, offsetPosition18.Z, 0.0, 0.0, rotation18.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition18.X, offsetPosition18.Y, offsetPosition18.Z, 0.0, 0.0, rotation18.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_right_safe", LoadDict(dict18), 1000.0, -8f, -8f, rotation18.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_right_bag", LoadDict(dict18), 1000.0, -8f, -8f, rotation18.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict18), "input_safe_code_right_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_right_player", LoadDict(dict18), 1000.0, -8f, -8f, rotation18.Z + 180f, 1148846080, 0);
										}
									}
									if (Game.IsControlJustPressed(Control.PhoneLeft))
									{
										Notification.Show("Move Left");
										SelectedNum--;
										if (SelectedNum < 0)
										{
											Notification.Show("Left Over" + SelectedNum);
											SelectedNum = 2;
										}
										else
										{
											Notification.Show("Move Left");
										}
										return;
									}
									if (Game.IsControlJustPressed(Control.PhoneRight))
									{
										SelectedNum++;
										if (SelectedNum >= 3)
										{
											Notification.Show("Right Over" + SelectedNum);
											SelectedNum = 0;
										}
										else
										{
											Notification.Show("Move Right");
										}
										return;
									}
								}
								if (CodeNumber1 == 0 && CodeNumber2 == 0 && CodeNumber3 == 0)
								{
									Random random4 = new Random();
									if (!UseSetSafeCode)
									{
										CodeNumber1 = random4.Next(1, 99);
										CodeNumber2 = random4.Next(1, 99);
										CodeNumber3 = random4.Next(1, 99);
									}
								}
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("Local_26709f_8 " + Local_26709f_8, 0.1f, 0.45f, 0.5f, 255, 255, 255);
								}
								if (Local_26709f_8 == -1)
								{
									func_4437(3616325110u, "xm3_safe_01a");
									return;
								}
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("ReadyForCode (2)" + ReadyForCode, 0.1f, 0.5f, 0.5f, 255, 255, 255);
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
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("Finished Render Que", 0.1f, 0.55f, 0.5f, 255, 255, 255);
								}
								if (ShowSecondarySafeUI)
								{
									string text10 = ((int)InputedNumber1).ToString();
									if (InputedNumber1 < 10f)
									{
										text10 = "0" + (int)InputedNumber1;
									}
									string text11 = ((int)InputedNumber2).ToString();
									if (InputedNumber2 < 10f)
									{
										text11 = "0" + (int)InputedNumber2;
									}
									string text12 = ((int)InputedNumber3).ToString();
									if (InputedNumber3 < 10f)
									{
										text12 = "0" + (int)InputedNumber3;
									}
									drawText(text10 + "   |   " + text11 + "   |   " + text12, 0.48f, 0.68f, 0.65f, 255, 255, 255);
									drawSprite2("timerbars", "all_black_bg", 0.5f, 0.7f, 0.2f, 0.1f, 255, 255, 255, 130);
								}
							}
						}
					}
				}
			}
			if (CurrentType == LootType.M32_Safe)
			{
				Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
				{
					Script.Wait(1);
					Local_26709f_9 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "DIGITAL_SAFE_DISPLAY");
					if (ShowCrackSafeMSGsDebug)
					{
						drawText("Attempting to Load Scaleform Movie 'DIGITAL_SAFE_DISPLAY'", 0.1f, 0.25f, 0.5f, 255, 255, 255);
					}
				}
				if (ShowCrackSafeMSGsDebug)
				{
					drawText("Showing Scaleform? " + ShowingSafeScaleform + " | LKoaded Scaleform? " + Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9), 0.1f, 0.1f, 0.5f, 255, 255, 255);
				}
				if (ShowingSafeScaleform)
				{
					if (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
					{
						string text13 = ((int)InputedNumber1).ToString();
						if (InputedNumber1 < 10f)
						{
							text13 = "0" + (int)InputedNumber1;
						}
						string text14 = ((int)InputedNumber2).ToString();
						if (InputedNumber2 < 10f)
						{
							text14 = "0" + (int)InputedNumber2;
						}
						string text15 = ((int)InputedNumber3).ToString();
						if (InputedNumber3 < 10f)
						{
							text15 = "0" + (int)InputedNumber3;
						}
						drawText(text13 + "   |   " + text14 + "   |   " + text15, 0.48f, 0.68f, 0.65f, 255, 255, 255);
						drawSprite2("timerbars", "all_black_bg", 0.5f, 0.7f, 0.2f, 0.1f, 255, 255, 255, 130);
					}
					if (Game.IsControlJustPressed(Control.Context) && ShowingSafeScaleform)
					{
						PlayingAnim = false;
						Random random5 = new Random();
						if (!UseSetSafeCode)
						{
							CodeNumber1 = random5.Next(1, 99);
							CodeNumber2 = random5.Next(1, 99);
							CodeNumber3 = random5.Next(1, 99);
						}
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
						string dict19 = "anim@scripted@heist@ig15_safe_crack@male@";
						LoadDict(dict19);
						Vector3 rotation19 = Safe.Rotation;
						Vector3 offsetPosition19 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition19.X, offsetPosition19.Y, offsetPosition19.Z, 0.0, 0.0, rotation19.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition19.X, offsetPosition19.Y, offsetPosition19.Z, 0.0, 0.0, rotation19.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "exit_safe", LoadDict(dict19), 1000.0, -8f, -8f, rotation19.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "exit_bag", LoadDict(dict19), 1000.0, -8f, -8f, rotation19.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict19), "exit_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "exit_player", LoadDict(dict19), 1000.0, -8f, -8f, rotation19.Z + 180f, 1148846080, 0);
						Script.Wait(1000);
						Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
						Ped character5 = Game.Player.Character;
						SetBagOn();
						Game.Player.Character.Task.ClearAllImmediately();
						if (bagProp != null)
						{
							bagProp.Delete();
							bagProp.MarkAsNoLongerNeeded();
							bagProp = null;
						}
						ReadyForCode = false;
						ShowingSafeScaleform = false;
						GotIncorrectSafeCode = false;
						GotCorrectSafeCode = false;
						PlayingSafeFinalAnim = false;
						FinishedSafeAnimations = false;
						PlayingIdle = false;
						Script.Wait(1);
					}
					if (ShowingSafeScaleform)
					{
						ShowSafeButtons();
						DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Quit, ~INPUT_ATTACK~ to Check Code");
						while (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "m32_safe_01a"))
						{
							Script.Wait(1);
							Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, "m32_safe_01a", 0);
							drawText("Attempting to Link Named Render Target 'm32_safe_01a'", 0.1f, 0.25f, 0.5f, 255, 255, 255);
						}
						while (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 1157006711))
						{
							Script.Wait(1);
							Function.Call(Hash.LINK_NAMED_RENDERTARGET, 1157006711);
							drawText("Attempting to Link Named Render Target '1157006711'", 0.1f, 0.25f, 0.5f, 255, 255, 255);
						}
						Local_26709f_8 = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, "m32_safe_01a");
						if (ShowCrackSafeMSGsDebug)
						{
							drawText("Safe Reg? " + Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "m32_safe_01a"), 0.1f, 0.15f, 0.5f, 255, 255, 255);
							drawText("Safe Link? " + Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 1157006711), 0.1f, 0.25f, 0.5f, 255, 255, 255);
							drawText("FinishedSafeAnimations " + FinishedSafeAnimations, 0.1f, 0.3f, 0.5f, 255, 255, 255);
							drawText("ReadyForCode " + ReadyForCode, 0.1f, 0.35f, 0.5f, 255, 255, 255);
						}
						if (!FinishedSafeAnimations)
						{
							Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
							bool flag5 = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, "m32_safe_01a");
							bool flag6 = Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, 1157006711);
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216 && !ReadyForCode)
							{
								ReadyForCode = true;
							}
							if (ShowCrackSafeMSGsDebug)
							{
								drawText("ScaleformLoaded " + Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9), 0.1f, 0.4f, 0.5f, 255, 255, 255);
							}
							if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Local_26709f_9))
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
											SafeValuables = World.CreateProp(RequestModel("m23_2_prop_m32_cashwrapped_01a"), Safe.Position, new Vector3(0f, 0f, Safe.Heading), false, false);
											GotIncorrectSafeCode = false;
											GotCorrectSafeCode = true;
											PlayingIdle = false;
											string dict20 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict20);
											Vector3 rotation20 = Safe.Rotation;
											Vector3 offsetPosition20 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition20.X, offsetPosition20.Y, offsetPosition20.Z, 0.0, 0.0, rotation20.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition20.X, offsetPosition20.Y, offsetPosition20.Z, 0.0, 0.0, rotation20.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "door_open_safe", LoadDict(dict20), 1000.0, -8f, -8f, rotation20.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "door_open_bag", LoadDict(dict20), 1000.0, -8f, -8f, rotation20.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SafeValuables, SafeScene1, "door_open_cash_bond", LoadDict(dict20), 1000.0, -8f, -8f, rotation20.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict20), "door_open_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "door_open_player", LoadDict(dict20), 1000.0, -8f, -8f, rotation20.Z + 180f, 1148846080, 0);
										}
										else
										{
											GotIncorrectSafeCode = true;
											GotCorrectSafeCode = false;
											PlayingIdle = false;
											string dict21 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict21);
											Vector3 rotation21 = Safe.Rotation;
											Vector3 offsetPosition21 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition21.X, offsetPosition21.Y, offsetPosition21.Z, 0.0, 0.0, rotation21.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition21.X, offsetPosition21.Y, offsetPosition21.Z, 0.0, 0.0, rotation21.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "fail_safe", LoadDict(dict21), 1000.0, -8f, -8f, rotation21.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "fail_bag", LoadDict(dict21), 1000.0, -8f, -8f, rotation21.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict21), "fail_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "fail_player", LoadDict(dict21), 1000.0, -8f, -8f, rotation21.Z + 180f, 1148846080, 0);
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
										Ped character6 = Game.Player.Character;
										SetBagOn();
										Game.Player.Character.Task.ClearAllImmediately();
										PlayingAnim = false;
										Crate_Valuables[CurrentLootTarget].GotValuables = true;
									}
									if (GotCorrectSafeCode && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										GotCorrectSafeCode = false;
										PlayingSafeFinalAnim = true;
										PlayingIdle = false;
										string dict22 = "anim@scripted@heist@ig15_safe_crack@male@";
										LoadDict(dict22);
										Vector3 rotation22 = Safe.Rotation;
										Vector3 offsetPosition22 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition22.X, offsetPosition22.Y, offsetPosition22.Z, 0.0, 0.0, rotation22.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition22.X, offsetPosition22.Y, offsetPosition22.Z, 0.0, 0.0, rotation22.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "success_with_stack_bonds_safe", LoadDict(dict22), 1000.0, -8f, -8f, rotation22.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "success_with_stack_bonds_bag", LoadDict(dict22), 1000.0, -8f, -8f, rotation22.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SafeValuables, SafeScene1, "success_with_stack_bonds_cash_bond", LoadDict(dict22), 1000.0, -8f, -8f, rotation22.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict22), "success_with_stack_bonds_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "success_with_stack_bonds_player", LoadDict(dict22), 1000.0, -8f, -8f, rotation22.Z + 180f, 1148846080, 0);
										Crate_Valuables[CurrentLootTarget].IsRobbed = true;
										if (!OverridePay)
										{
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 40f), (int)(Heist_Average_Pay / 60f));
										}
										if (OverridePay)
										{
											AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 2f), (int)(Heist_Average_Pay / 1.25f));
										}
									}
									if (GotIncorrectSafeCode && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										GotCorrectSafeCode = false;
										GotIncorrectSafeCode = false;
									}
									if (!GotCorrectSafeCode && !GotIncorrectSafeCode && !PlayingSafeFinalAnim && !Game.IsControlPressed(Control.PhoneUp) && !Game.IsControlPressed(Control.PhoneDown) && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
									{
										PlayingIdle = true;
										string dict23 = "anim@scripted@heist@ig15_safe_crack@male@";
										LoadDict(dict23);
										Vector3 rotation23 = Safe.Rotation;
										Vector3 offsetPosition23 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
										SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition23.X, offsetPosition23.Y, offsetPosition23.Z, 0.0, 0.0, rotation23.Z, 2);
										Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition23.X, offsetPosition23.Y, offsetPosition23.Z, 0.0, 0.0, rotation23.Z, true);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "idle_safe", LoadDict(dict23), 1000.0, -8f, -8f, rotation23.Z + 180f, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "idle_bag", LoadDict(dict23), 1000.0, -8f, -8f, rotation23.Z + 180f, 1148846080, 0);
										Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict23), "idle_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
										Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "idle_player", LoadDict(dict23), 1000.0, -8f, -8f, rotation23.Z + 180f, 1148846080, 0);
									}
									if (Game.IsControlPressed(Control.PhoneUp))
									{
										if (SelectedNum == 0)
										{
											InputedNumber1 += 0.15f;
											if (InputedNumber1 > 99f)
											{
												InputedNumber1 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber1 == CodeNumber1)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 1)
										{
											InputedNumber2 += 0.15f;
											if (InputedNumber2 > 99f)
											{
												InputedNumber2 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber2 == CodeNumber2)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 2)
										{
											InputedNumber3 += 0.15f;
											if (InputedNumber3 > 99f)
											{
												InputedNumber3 = 0f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber3 == CodeNumber3)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (!PlayingIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
										{
											string dict24 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict24);
											Vector3 rotation24 = Safe.Rotation;
											Vector3 offsetPosition24 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition24.X, offsetPosition24.Y, offsetPosition24.Z, 0.0, 0.0, rotation24.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition24.X, offsetPosition24.Y, offsetPosition24.Z, 0.0, 0.0, rotation24.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_left_safe", LoadDict(dict24), 1000.0, -8f, -8f, rotation24.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_left_bag", LoadDict(dict24), 1000.0, -8f, -8f, rotation24.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict24), "input_safe_code_left_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_left_player", LoadDict(dict24), 1000.0, -8f, -8f, rotation24.Z + 180f, 1148846080, 0);
										}
										if (PlayingIdle)
										{
											PlayingIdle = false;
											string dict25 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict25);
											Vector3 rotation25 = Safe.Rotation;
											Vector3 offsetPosition25 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition25.X, offsetPosition25.Y, offsetPosition25.Z, 0.0, 0.0, rotation25.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition25.X, offsetPosition25.Y, offsetPosition25.Z, 0.0, 0.0, rotation25.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_left_safe", LoadDict(dict25), 1000.0, -8f, -8f, rotation25.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_left_bag", LoadDict(dict25), 1000.0, -8f, -8f, rotation25.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict25), "input_safe_code_left_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_left_player", LoadDict(dict25), 1000.0, -8f, -8f, rotation25.Z + 180f, 1148846080, 0);
										}
									}
									if (Game.IsControlPressed(Control.PhoneDown))
									{
										if (SelectedNum == 0)
										{
											InputedNumber1 -= 0.15f;
											if (InputedNumber1 < 0f)
											{
												InputedNumber1 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber1 == CodeNumber1)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 1)
										{
											InputedNumber2 -= 0.15f;
											if (InputedNumber2 < 0f)
											{
												InputedNumber2 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber2 == CodeNumber2)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (SelectedNum == 2)
										{
											InputedNumber3 -= 0.15f;
											if (InputedNumber3 < 0f)
											{
												InputedNumber3 = 99f;
											}
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
											Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
											if ((int)InputedNumber3 == CodeNumber3)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
											}
										}
										if (!PlayingIdle && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SafeScene1) >= 1065353216)
										{
											string dict26 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict26);
											Vector3 rotation26 = Safe.Rotation;
											Vector3 offsetPosition26 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition26.X, offsetPosition26.Y, offsetPosition26.Z, 0.0, 0.0, rotation26.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition26.X, offsetPosition26.Y, offsetPosition26.Z, 0.0, 0.0, rotation26.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_right_safe", LoadDict(dict26), 1000.0, -8f, -8f, rotation26.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_right_bag", LoadDict(dict26), 1000.0, -8f, -8f, rotation26.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict26), "input_safe_code_right_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_right_player", LoadDict(dict26), 1000.0, -8f, -8f, rotation26.Z + 180f, 1148846080, 0);
										}
										if (PlayingIdle)
										{
											PlayingIdle = false;
											string dict27 = "anim@scripted@heist@ig15_safe_crack@male@";
											LoadDict(dict27);
											Vector3 rotation27 = Safe.Rotation;
											Vector3 offsetPosition27 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
											SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition27.X, offsetPosition27.Y, offsetPosition27.Z, 0.0, 0.0, rotation27.Z, 2);
											Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition27.X, offsetPosition27.Y, offsetPosition27.Z, 0.0, 0.0, rotation27.Z, true);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "input_safe_code_right_safe", LoadDict(dict27), 1000.0, -8f, -8f, rotation27.Z + 180f, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "input_safe_code_right_bag", LoadDict(dict27), 1000.0, -8f, -8f, rotation27.Z + 180f, 1148846080, 0);
											Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict27), "input_safe_code_right_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
											Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "input_safe_code_right_player", LoadDict(dict27), 1000.0, -8f, -8f, rotation27.Z + 180f, 1148846080, 0);
										}
									}
									if (Game.IsControlJustPressed(Control.PhoneLeft))
									{
										Notification.Show("Move Left");
										SelectedNum--;
										if (SelectedNum < 0)
										{
											Notification.Show("Left Over" + SelectedNum);
											SelectedNum = 2;
										}
										else
										{
											Notification.Show("Move Left");
										}
										return;
									}
									if (Game.IsControlJustPressed(Control.PhoneRight))
									{
										SelectedNum++;
										if (SelectedNum >= 3)
										{
											Notification.Show("Right Over" + SelectedNum);
											SelectedNum = 0;
										}
										else
										{
											Notification.Show("Move Right");
										}
										return;
									}
								}
								if (CodeNumber1 == 0 && CodeNumber2 == 0 && CodeNumber3 == 0)
								{
									Random random6 = new Random();
									if (!UseSetSafeCode)
									{
										CodeNumber1 = random6.Next(1, 99);
										CodeNumber2 = random6.Next(1, 99);
										CodeNumber3 = random6.Next(1, 99);
									}
								}
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("Local_26709f_8 " + Local_26709f_8, 0.1f, 0.45f, 0.5f, 255, 255, 255);
								}
								if (Local_26709f_8 == -1)
								{
									func_4437(1157006711u, "m32_safe_01a");
									return;
								}
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("ReadyForCode (2)" + ReadyForCode, 0.1f, 0.5f, 0.5f, 255, 255, 255);
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
								if (ShowCrackSafeMSGsDebug)
								{
									drawText("Finished Render Que", 0.1f, 0.55f, 0.5f, 255, 255, 255);
								}
								if (ShowSecondarySafeUI)
								{
									string text16 = ((int)InputedNumber1).ToString();
									if (InputedNumber1 < 10f)
									{
										text16 = "0" + (int)InputedNumber1;
									}
									string text17 = ((int)InputedNumber2).ToString();
									if (InputedNumber2 < 10f)
									{
										text17 = "0" + (int)InputedNumber2;
									}
									string text18 = ((int)InputedNumber3).ToString();
									if (InputedNumber3 < 10f)
									{
										text18 = "0" + (int)InputedNumber3;
									}
									drawText(text16 + "   |   " + text17 + "   |   " + text18, 0.48f, 0.68f, 0.65f, 255, 255, 255);
									drawSprite2("timerbars", "all_black_bg", 0.5f, 0.7f, 0.2f, 0.1f, 255, 255, 255, 130);
								}
							}
						}
					}
				}
			}
			if (CurrentType == LootType.Trolly_Cash || CurrentType == LootType.Trolly_Gold || CurrentType == LootType.Trolly_Diamonds || CurrentType == LootType.Trolly_Coke)
			{
				if (Game.IsControlJustPressed(Control.Attack))
				{
					if (PlayingCashGrabIdle)
					{
						MoneyProp.IsVisible = true;
						PlayingCashGrabIdle = false;
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerLastLootProp.Position.X, PlayerLastLootProp.Position.Y, PlayerLastLootProp.Position.Z, 0.0, 0.0, PlayerCurrentLootBoxRotation, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "grab", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, PlayerCurrentLootBoxRotation + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_grab", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, PlayerCurrentLootBoxRotation + 180f, 1148846080, 0);
						PlayingAnim = true;
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
						SetRob = false;
						InitialAnimDone = false;
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerLastLootProp, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, CashGrabLastSavePlace);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
						PlayerGrabLootDelayStop += 100f;
					}
					PlayerGrabLootDelayStop += 50f;
				}
				if (CurrentType == LootType.Trolly_Cash)
				{
					DisplayHelpTextThisFrameNoSound("Repeatedly tap ~INPUT_ATTACK~ to grab the ~g~Cash~w~");
				}
				if (CurrentType == LootType.Trolly_Gold)
				{
					DisplayHelpTextThisFrameNoSound("Repeatedly tap ~INPUT_ATTACK~ to grab the ~g~Gold~w~");
				}
				if (CurrentType == LootType.Trolly_Diamonds)
				{
					DisplayHelpTextThisFrameNoSound("Repeatedly tap ~INPUT_ATTACK~ to grab the ~g~Diamonds~w~");
				}
				if (CurrentType == LootType.Trolly_Coke)
				{
					DisplayHelpTextThisFrameNoSound("Repeatedly tap ~INPUT_ATTACK~ to grab the ~g~Coke~w~");
				}
				if (PlayerGrabLootDelayStop > 0f)
				{
					PlayerGrabLootDelayStop -= 1f;
				}
				if (!PlayingCashGrabIdle && PlayerGrabLootDelayStop <= 700f)
				{
					if (PlayerGrabLootDelayStop >= 100f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1.1f);
					}
					if (PlayerGrabLootDelayStop >= 150f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1.2f);
					}
					if (PlayerGrabLootDelayStop >= 200f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1.3f);
					}
					if (PlayerGrabLootDelayStop >= 300f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1.4f);
					}
					if (PlayerGrabLootDelayStop < 100f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, 1f);
					}
				}
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
						CalculatePay(Game.Player.Character, CurrentType);
					}
					if (CashCount == 50f)
					{
						Model model4 = new Model("ch_prop_20dollar_pile_01a");
						if (CurrentType == LootType.Trolly_Cash)
						{
							model4 = new Model("ch_prop_20dollar_pile_01a");
							model4.Request(250);
						}
						if (CurrentType == LootType.Trolly_Gold)
						{
							model4 = new Model("ch_prop_gold_bar_01a");
							model4.Request(250);
						}
						if (CurrentType == LootType.Trolly_Diamonds)
						{
							model4 = new Model("ch_prop_vault_dimaondbox_01a");
							model4.Request(250);
						}
						if (CurrentType == LootType.Trolly_Coke)
						{
							model4 = new Model("imp_prop_impexp_coke_pile");
							model4.Request(250);
						}
						model4.MarkAsNoLongerNeeded();
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHLeftHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "cash_grab_trolley", "anim@heists@ornate_bank@cash_trolley", 1f, -1f, 0, 1148846080);
						MoneyProp.IsVisible = true;
						CashCount = 0f;
					}
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 && !PlayingCashGrabIdle)
				{
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					if (bagProp != null)
					{
						bagProp.Delete();
					}
					Ped character7 = Game.Player.Character;
					SetBagOn();
					PlayingAnim = false;
					Game.Player.Character.Task.ClearAll();
					RobTimer = 0;
					DeleteCamera();
					Crate_Valuables[CurrentLootTarget].GotValuables = true;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, PlayingCashGrabIdle) >= 1065353216 && PlayingCashGrabIdle)
				{
					IdleScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, PlayerLastLootProp.Position.X, PlayerLastLootProp.Position.Y, PlayerLastLootProp.Position.Z, 0.0, 0.0, PlayerCurrentLootBoxRotation, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, IdleScene, "anim@heists@ornate_bank@grab_cash", "grab_idle", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, IdleScene, "grab_idle", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, PlayerCurrentLootBoxRotation + 180f, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, IdleScene, "bag_grab_idle", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, PlayerCurrentLootBoxRotation + 180f, 1148846080, 0);
				}
			}
			RobTimer++;
		}

		public static void RobCrate(Ped Ped, Prop P, Vector3 Pos, Vector3 Rot, LootType type, int ID)
		{
			if (P != null && P.IsAttached())
			{
				P.Detach();
			}
			if (Ped == Game.Player.Character)
			{
				Ped character = Game.Player.Character;
				SetBagOff();
				if (MoneyProp != null)
				{
					MoneyProp.Delete();
				}
				if (bagProp != null)
				{
					bagProp.Delete();
				}
				if (type == LootType.Trolly_Cash)
				{
					SetBagOff();
					CreateCamera(P, LootType.Trolly_Cash);
					PlayerGrabLootDelayStop = 200f;
					PlayingAnim = true;
					RobTimer = 0;
					bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
					bagProp.IsCollisionEnabled = false;
					bagProp.IsInvincible = true;
					PlayerCurrentLootBoxRotation = Rot.Z;
					PlayerLastLootProp = P;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					SetRob = true;
					CurrentType = LootType.Trolly_Cash;
				}
				if (type == LootType.Trolly_Gold)
				{
					SetBagOff();
					CreateCamera(P, LootType.Trolly_Gold);
					PlayerGrabLootDelayStop = 200f;
					PlayingAnim = true;
					RobTimer = 0;
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					if (bagProp != null)
					{
						bagProp.Delete();
					}
					bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
					bagProp.IsCollisionEnabled = false;
					bagProp.IsInvincible = true;
					PlayerCurrentLootBoxRotation = Rot.Z;
					PlayerLastLootProp = P;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					SetRob = true;
					CurrentType = LootType.Trolly_Gold;
				}
				if (type == LootType.Trolly_Diamonds)
				{
					SetBagOff();
					CreateCamera(P, LootType.Trolly_Diamonds);
					PlayerGrabLootDelayStop = 200f;
					PlayingAnim = true;
					RobTimer = 0;
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					if (bagProp != null)
					{
						bagProp.Delete();
					}
					bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
					bagProp.IsCollisionEnabled = false;
					bagProp.IsInvincible = true;
					PlayerCurrentLootBoxRotation = Rot.Z;
					PlayerLastLootProp = P;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					SetRob = true;
					CurrentType = LootType.Trolly_Diamonds;
				}
				if (type == LootType.CP_Cash)
				{
					SetBagOff();
					CreateCamera(P, LootType.CP_Cash);
					InitialAnimDone = false;
					PlayerWaitingForLastAnim = false;
					PlayerFinishedLastAnim = false;
					PlayerFirstAnim = false;
					PlayingAnim = true;
					RobTimer = 0;
					if (bagProp != null)
					{
						bagProp.Delete();
						bagProp.MarkAsNoLongerNeeded();
						bagProp = null;
					}
					bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
					bagProp.IsCollisionEnabled = false;
					bagProp.IsInvincible = true;
					PlayerRobbingProp = P;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@cash@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					SetRob = true;
					CurrentType = LootType.CP_Cash;
				}
				if (type == LootType.CP_Gold)
				{
					SetBagOff();
					CreateCamera(P, LootType.CP_Gold);
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
					InitialAnimDone = false;
					PlayerWaitingForLastAnim = false;
					PlayerFinishedLastAnim = false;
					PlayerFirstAnim = false;
					PlayingAnim = true;
					RobTimer = 0;
					if (bagProp != null)
					{
						bagProp.Delete();
						bagProp.MarkAsNoLongerNeeded();
						bagProp = null;
					}
					bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
					bagProp.IsCollisionEnabled = false;
					bagProp.IsInvincible = true;
					PlayerRobbingProp = P;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					SetRob = true;
					CurrentType = LootType.CP_Gold;
				}
				if (type == LootType.M42_ServerStack)
				{
					SetBagOff();
					CreateCamera(P, LootType.M42_ServerStack);
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 1, 3000, 1, 0);
					InitialAnimDone = false;
					PlayerWaitingForLastAnim = false;
					PlayerFinishedLastAnim = false;
					PlayerFirstAnim = false;
					PlayingAnim = true;
					RobTimer = 0;
					if (bagProp != null)
					{
						bagProp.Delete();
						bagProp.MarkAsNoLongerNeeded();
						bagProp = null;
					}
					bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
					bagProp.IsCollisionEnabled = false;
					bagProp.IsInvincible = true;
					PlayerRobbingProp = P;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					SetRob = true;
					CurrentType = LootType.M42_ServerStack;
				}
				if (type == LootType.Trolly_Coke)
				{
					SetBagOff();
					CreateCamera(P, LootType.Trolly_Coke);
					PlayerGrabLootDelayStop = 40f;
					PlayingAnim = true;
					RobTimer = 0;
					bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
					bagProp.IsCollisionEnabled = false;
					bagProp.IsInvincible = true;
					PlayerCurrentLootBoxRotation = Rot.Z;
					PlayerLastLootProp = P;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					SetRob = true;
					CurrentType = LootType.Trolly_Coke;
				}
				switch (type)
				{
				case LootType.GlassCut_Cabinet:
				{
					SetBagOff();
					CurrentType = LootType.GlassCut_Cabinet;
					Random random = new Random();
					RequiredPowerOutput = random.Next(10, 80);
					PlayingAnim = true;
					return;
				}
				case LootType.SafetyDepositBoxes:
					if (1 == 0)
					{
						PlayerCurrentLockBoxProp = P;
						RobTimer = 0;
						if (MoneyProp != null)
						{
							MoneyProp.Delete();
						}
						if (bagProp != null)
						{
							bagProp.Delete();
						}
						MoneyProp = World.CreateProp(RequestModel("ch_prop_vault_drill_01a"), Game.Player.Character.Position, false, false);
						bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
						bagProp.IsCollisionEnabled = false;
						bagProp.IsInvincible = true;
						PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01a"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
							PlayerCurrentLockBoxType = "abc";
						}
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01b"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
							PlayerCurrentLockBoxType = "abc";
						}
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01c"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
							PlayerCurrentLockBoxType = "abc";
						}
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01d"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
							PlayerCurrentLockBoxType = "def";
						}
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01e"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
							PlayerCurrentLockBoxType = "def";
						}
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01f"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
							PlayerCurrentLockBoxType = "def";
						}
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01g"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
							PlayerCurrentLockBoxType = "ghi";
						}
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01h"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
							PlayerCurrentLockBoxType = "ghi";
						}
						if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01i"))
						{
							PlayerCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
							PlayerCurrentLockBoxType = "ghi";
						}
						PlayerCurrentLootBoxRotation = Rot.Z;
						LoadDict(PlayerCurrentSDDict);
						Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(PlayerCurrentSDDict), "enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 0.0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict(PlayerCurrentSDDict), 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "enter_ch_prop_vault_drill_01a", LoadDict(PlayerCurrentSDDict), 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "enter_p_m_bag_var22_arm_s", LoadDict(PlayerCurrentSDDict), 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "enter_ch_prop_ch_sec_cabinet_01" + PlayerCurrentLockBoxType, LoadDict(PlayerCurrentSDDict), 1f, -1f, 0, 1148846080);
						PlayerCurrentLockbox = 1;
						PlayerCurrentLockboxStage = 1;
						InitialAnimDone = true;
					}
					else
					{
						PlayerLastSafeDepositBox = P;
					}
					CurrentType = LootType.SafetyDepositBoxes;
					PlayingAnim = true;
					break;
				}
				if (type == LootType.SafetyDepositBoxes_XM3)
				{
					bool flag = true;
					PlayerLastSafeDepositBox = P;
					CurrentType = LootType.SafetyDepositBoxes_XM3;
					PlayingAnim = true;
				}
				if (type == LootType.LootBox_Plates)
				{
					InitialAnimDone = false;
					PlayedFirstAnim = false;
					InitialAnimDone = false;
					PlayerPlayingFinalAnim = false;
					PlayerWaitingForLastAnim = false;
					PlayerLastLootProp = P;
					PlayerGrabLootDelayStop = 40f;
					PlayingAnim = true;
					RobTimer = 0;
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					if (bagProp != null)
					{
						bagProp.Delete();
					}
					bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
					bagProp.IsCollisionEnabled = false;
					bagProp.IsInvincible = true;
					MoneyProp = World.CreateProp(RequestModel("tr_prop_tr_carry_box_01a"), Game.Player.Character.Position, false, false);
					MoneyProp.IsCollisionEnabled = false;
					MoneyProp.IsInvincible = true;
					PlayerCurrentLootBoxRotation = Rot.Z;
					PlayerLastLootProp = P;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z - 180f, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z - 180f, true);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, "anim@scripted@player@mission@tunf_conv_ig1_monyplate@male@", "action", 0.0, 0.0, 0, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, Scene1, "action_bag", LoadDict("anim@scripted@player@mission@tunf_conv_ig1_monyplate@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "action_carry_box", LoadDict("anim@scripted@player@mission@tunf_conv_ig1_monyplate@male@"), 1000f, 0f, 0, 1000f);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerLastLootProp, Scene1, "action_case", LoadDict("anim@scripted@player@mission@tunf_conv_ig1_monyplate@male@"), 1000f, 0f, 0, 1000f);
					SetRob = true;
					CurrentType = LootType.LootBox_Plates;
					InitialAnimDone = true;
				}
				if (type == LootType.LootBox_HDD)
				{
					SetBagOn();
					InitialAnimDone = false;
					PlayedFirstAnim = false;
					InitialAnimDone = false;
					PlayerPlayingFinalAnim = false;
					PlayerWaitingForLastAnim = false;
					PlayerLastLootProp = P;
					PlayerGrabLootDelayStop = 40f;
					PlayingAnim = true;
					RobTimer = 0;
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					LoadDict("anim@GangOps@VAN@DRIVE_GRAB@");
					MoneyProp = World.CreateProp(RequestModel("prop_cs_server_drive"), Game.Player.Character.Position, false, false);
					MoneyProp.IsCollisionEnabled = false;
					MoneyProp.IsInvincible = true;
					PlayerCurrentLootBoxRotation = Rot.Z;
					PlayerLastLootProp = P;
					Vector3 offsetPosition = P.GetOffsetPosition(new Vector3(0f, -2.15f, -0.1f));
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, Rot.Z - 180f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@GangOps@VAN@DRIVE_GRAB@"), "grab_drive", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "grab_drive", LoadDict("anim@GangOps@VAN@DRIVE_GRAB@"), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "drive_grab_drive", LoadDict("anim@GangOps@VAN@DRIVE_GRAB@"), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, PlayerLastLootProp, Scene1, "crate_grab_drive", LoadDict("anim@GangOps@VAN@DRIVE_GRAB@"), 1f, -1f, 0, 1148846080);
					SetRob = true;
					CurrentType = LootType.LootBox_HDD;
					InitialAnimDone = true;
				}
				if (type == LootType.WoodCrate)
				{
					SetBagOn();
					InitialAnimDone = false;
					PlayedFirstAnim = false;
					InitialAnimDone = false;
					PlayerPlayingFinalAnim = false;
					PlayerWaitingForLastAnim = false;
					PlayerLastLootProp = P;
					PlayerGrabLootDelayStop = 40f;
					PlayingAnim = true;
					RobTimer = 0;
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					CurrentWoodenContainer = new WoodenContainer(P, WoodenContainer.Loot.Gold);
					CurrentWoodenContainer.SilverCrate.CreateEntites();
					foreach (ObjectPlacementMap entity in CurrentWoodenContainer.SilverCrate.Entities)
					{
						if (entity.Entity != null)
						{
							ATTACH_ENTITY_TO_ENTITY(entity.Entity, P, -1, entity.EntityToAttachOffset.X, entity.EntityToAttachOffset.Y, entity.EntityToAttachOffset.Z, entity.EntityToAttachRotation.X, entity.EntityToAttachRotation.Y, entity.EntityToAttachRotation.Z, false, false, true, false, 2, true);
						}
					}
					SpawnedSilverCrates.Add(CurrentWoodenContainer.SilverCrate);
					SetRob = true;
					CurrentType = LootType.WoodCrate;
				}
				if (type == LootType.Server_HDD)
				{
					SetBagOn();
					InitialAnimDone = false;
					PlayedFirstAnim = false;
					InitialAnimDone = false;
					PlayerPlayingFinalAnim = false;
					PlayerWaitingForLastAnim = false;
					PlayerLastLootProp = P;
					PlayerGrabLootDelayStop = 40f;
					PlayingAnim = true;
					RobTimer = 0;
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					LoadDict("anim@GangOps@VAN@DRIVE_GRAB@");
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
					}
					MoneyProp = World.CreateProp(RequestModel("prop_cs_server_drive"), Game.Player.Character.Position, false, false);
					MoneyProp.IsCollisionEnabled = false;
					MoneyProp.IsInvincible = true;
					string dict = "anim@scripted@player@fix_club_ig1_hdd_remove@male@";
					LoadDict(dict);
					Vector3 position = P.Position;
					float heading = P.Heading;
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position.X, position.Y, position.Z, 0.0, 0.0, heading, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1));
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict("anim@scripted@player@fix_club_ig1_hdd_remove@male@"), "action", 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "action", LoadDict("anim@scripted@player@fix_club_ig1_hdd_remove@male@"), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, Scene1, "action_drive", LoadDict("anim@scripted@player@fix_club_ig1_hdd_remove@male@"), 16f, -16f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, Scene1, "action_Server", LoadDict("anim@scripted@player@fix_club_ig1_hdd_remove@male@"), 16f, -16f, 0, 1148846080);
					SetRob = true;
					CurrentType = LootType.Server_HDD;
					InitialAnimDone = true;
				}
				if (type == LootType.CP_Safe)
				{
					SetBagOn();
					if (!ShowingSafeScaleform)
					{
						ReadyForCode = false;
						ShowingSafeScaleform = false;
						GotIncorrectSafeCode = false;
						GotCorrectSafeCode = false;
						PlayingSafeFinalAnim = false;
						FinishedSafeAnimations = false;
						PlayingIdle = false;
						Safe = P;
						Random random2 = new Random();
						if (!UseSetSafeCode)
						{
							CodeNumber1 = random2.Next(1, 99);
							CodeNumber2 = random2.Next(1, 99);
							CodeNumber3 = random2.Next(1, 99);
						}
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
						if (SafeValuables != null)
						{
							SafeValuables.Delete();
						}
						bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
						bagProp.IsCollisionEnabled = false;
						bagProp.IsInvincible = true;
						ReadyForCode = false;
						SelectedNum = 0;
						InputedNumber1 = 0f;
						InputedNumber2 = 0f;
						InputedNumber3 = 0f;
						ShowingSafeScaleform = true;
						string dict2 = "anim@scripted@heist@ig15_safe_crack@male@";
						LoadDict(dict2);
						Vector3 rotation = Safe.Rotation;
						Vector3 offsetPosition2 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, rotation.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition2.X, offsetPosition2.Y, offsetPosition2.Z, 0.0, 0.0, rotation.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "enter_safe", LoadDict(dict2), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "enter_bag", LoadDict(dict2), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict2), "enter_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "enter_player", LoadDict(dict2), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
						CurrentType = LootType.CP_Safe;
						PlayingAnim = true;
					}
				}
				if (type == LootType.XM3_Safe)
				{
					SetBagOn();
					if (!ShowingSafeScaleform)
					{
						ReadyForCode = false;
						ShowingSafeScaleform = false;
						GotIncorrectSafeCode = false;
						GotCorrectSafeCode = false;
						PlayingSafeFinalAnim = false;
						FinishedSafeAnimations = false;
						PlayingIdle = false;
						Safe = P;
						Random random3 = new Random();
						if (!UseSetSafeCode)
						{
							CodeNumber1 = random3.Next(1, 99);
							CodeNumber2 = random3.Next(1, 99);
							CodeNumber3 = random3.Next(1, 99);
						}
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
						if (SafeValuables != null)
						{
							SafeValuables.Delete();
						}
						bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
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
						Vector3 rotation2 = Safe.Rotation;
						Vector3 offsetPosition3 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, rotation2.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition3.X, offsetPosition3.Y, offsetPosition3.Z, 0.0, 0.0, rotation2.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "enter_safe", LoadDict(dict3), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "enter_bag", LoadDict(dict3), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict3), "enter_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "enter_player", LoadDict(dict3), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
						CurrentType = LootType.XM3_Safe;
						PlayingAnim = true;
					}
				}
				if (type == LootType.M32_Safe)
				{
					SetBagOn();
					if (!ShowingSafeScaleform)
					{
						ReadyForCode = false;
						ShowingSafeScaleform = false;
						GotIncorrectSafeCode = false;
						GotCorrectSafeCode = false;
						PlayingSafeFinalAnim = false;
						FinishedSafeAnimations = false;
						PlayingIdle = false;
						Safe = P;
						Random random4 = new Random();
						if (!UseSetSafeCode)
						{
							CodeNumber1 = random4.Next(1, 99);
							CodeNumber2 = random4.Next(1, 99);
							CodeNumber3 = random4.Next(1, 99);
						}
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
						if (SafeValuables != null)
						{
							SafeValuables.Delete();
						}
						bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
						bagProp.IsCollisionEnabled = false;
						bagProp.IsInvincible = true;
						ReadyForCode = false;
						SelectedNum = 0;
						InputedNumber1 = 0f;
						InputedNumber2 = 0f;
						InputedNumber3 = 0f;
						ShowingSafeScaleform = true;
						string dict4 = "anim@scripted@heist@ig15_safe_crack@male@";
						LoadDict(dict4);
						Vector3 rotation3 = Safe.Rotation;
						Vector3 offsetPosition4 = Safe.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						SafeScene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation3.Z, 2);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, SafeScene1, offsetPosition4.X, offsetPosition4.Y, offsetPosition4.Z, 0.0, 0.0, rotation3.Z, true);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Safe, SafeScene1, "enter_safe", LoadDict(dict4), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, SafeScene1, "enter_bag", LoadDict(dict4), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SafeScene1, LoadDict(dict4), "enter_player", 1000.0, -4.0, 128, 0, 1148846080, 0);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SafeScene1, "enter_player", LoadDict(dict4), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
						CurrentType = LootType.M32_Safe;
						PlayingAnim = true;
					}
				}
				if (type == LootType.JewelStoreCabinet)
				{
					CurrentType = LootType.JewelStoreCabinet;
					PlayerRobbingProp = P;
					PlayingAnim = true;
				}
				if (type == LootType.Painting)
				{
					CurrentType = LootType.Painting;
					PlayerCurrentPaintingNew = P;
					PlayingAnim = true;
				}
				if (type == LootType.CashDryer)
				{
					if (CurrentWashDryer != null)
					{
						CurrentWashDryer.CleanupWashMach();
					}
					CurrentWashDryer = new WashMachCashGrab();
					CurrentWashDryer.CleanupWashMach();
					CurrentWashDryer.CanGrab = true;
					CurrentWashDryer.WashMachCase = 0;
					PlayingAnim = true;
					RobTimer = 0;
					CurrentType = LootType.CashDryer;
				}
				if (type == LootType.M42_JewelCabinet)
				{
					CurrentType = LootType.M42_JewelCabinet;
					PlayerRobbingProp = P;
					PlayingAnim = true;
				}
			}
			if (Ped == RobberA)
			{
				if (RobberA_HoldingLootProp != null)
				{
					RobberA_HoldingLootProp.Delete();
				}
				if (RobberA_bagProp != null)
				{
					RobberA_bagProp.Delete();
				}
				if (type == LootType.Trolly_Cash)
				{
					RobberARobbing = true;
					RobberA_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberA.Position, false, false);
					RobberA_bagProp.IsCollisionEnabled = false;
					RobberA_bagProp.IsInvincible = true;
					RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberASyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					RobberASetRob = true;
					RobberACurrentType = LootType.Trolly_Cash;
				}
				if (type == LootType.Trolly_Gold)
				{
					RobberARobbing = true;
					if (RobberA_HoldingLootProp != null)
					{
						RobberA_HoldingLootProp.Delete();
					}
					if (RobberA_bagProp != null)
					{
						RobberA_bagProp.Delete();
					}
					RobberA_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberA.Position, false, false);
					RobberA_bagProp.IsCollisionEnabled = false;
					RobberA_bagProp.IsInvincible = true;
					RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberASyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					RobberASetRob = true;
					RobberACurrentType = LootType.Trolly_Gold;
				}
				if (type == LootType.Trolly_Diamonds)
				{
					RobberARobbing = true;
					if (RobberA_HoldingLootProp != null)
					{
						RobberA_HoldingLootProp.Delete();
					}
					if (RobberA_bagProp != null)
					{
						RobberA_bagProp.Delete();
					}
					RobberA_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberA.Position, false, false);
					RobberA_bagProp.IsCollisionEnabled = false;
					RobberA_bagProp.IsInvincible = true;
					RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberASyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					RobberASetRob = true;
					RobberACurrentType = LootType.Trolly_Diamonds;
				}
				if (type == LootType.CP_Cash)
				{
					RobberAFirstAnim = false;
					RobberARobbing = true;
					RobberARobTimer = 0;
					RobberA_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberA.Position, false, false);
					RobberA_bagProp.IsCollisionEnabled = false;
					RobberA_bagProp.IsInvincible = true;
					RobberARobbingProp = P;
					RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					RobberASetRob = true;
					RobberACurrentType = LootType.CP_Cash;
				}
				if (type == LootType.CP_Gold)
				{
					RobberAFirstAnim = false;
					RobberARobbing = true;
					RobberARobTimer = 0;
					RobberA_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberA.Position, false, false);
					RobberA_bagProp.IsCollisionEnabled = false;
					RobberA_bagProp.IsInvincible = true;
					RobberARobbingProp = P;
					RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					RobberASetRob = true;
					RobberACurrentType = LootType.CP_Gold;
				}
				if (type == LootType.M42_ServerStack)
				{
					RobberAFirstAnim = false;
					RobberARobbing = true;
					RobberARobTimer = 0;
					RobberA_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberA.Position, false, false);
					RobberA_bagProp.IsCollisionEnabled = false;
					RobberA_bagProp.IsInvincible = true;
					RobberARobbingProp = P;
					RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberASyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					RobberASetRob = true;
					RobberACurrentType = LootType.M42_ServerStack;
				}
				if (type == LootType.SafetyDepositBoxes)
				{
					RobberACurrentLockBoxProp = P;
					RobberAtimer = 0;
					if (RobberA_HoldingLootProp != null)
					{
						RobberA_HoldingLootProp.Delete();
					}
					if (RobberA_bagProp != null)
					{
						RobberA_bagProp.Delete();
					}
					RobberA_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_vault_drill_01a"), RobberA.Position, false, false);
					RobberA_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberA.Position, false, false);
					RobberA_bagProp.IsCollisionEnabled = false;
					RobberA_bagProp.IsInvincible = true;
					RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01a"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
						RobberACurrentLockBoxType = "abc";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01b"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
						RobberACurrentLockBoxType = "abc";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01c"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
						RobberACurrentLockBoxType = "abc";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01d"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
						RobberACurrentLockBoxType = "def";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01e"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
						RobberACurrentLockBoxType = "def";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01f"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
						RobberACurrentLockBoxType = "def";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01g"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
						RobberACurrentLockBoxType = "ghi";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01h"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
						RobberACurrentLockBoxType = "ghi";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01i"))
					{
						RobberACurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
						RobberACurrentLockBoxType = "ghi";
					}
					RobberACurrentLootBoxRotation = Rot.Z;
					LoadDict(RobberACurrentSDDict);
					RobberASyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberA, RobberASyncScene, LoadDict(RobberACurrentSDDict), "enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene));
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberASyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA, RobberASyncScene, "enter", LoadDict(RobberACurrentSDDict), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_HoldingLootProp, RobberASyncScene, "enter_ch_prop_vault_drill_01a", LoadDict(RobberACurrentSDDict), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberA_bagProp, RobberASyncScene, "enter_p_m_bag_var22_arm_s", LoadDict(RobberACurrentSDDict), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberASyncScene, "enter_ch_prop_ch_sec_cabinet_01" + RobberACurrentLockBoxType, LoadDict(RobberACurrentSDDict), 1f, -1f, 0, 1148846080);
					RobberACurrentLockbox = 1;
					RobberACurrentLockboxStage = 1;
					RobberAInitialAnimDone = true;
					RobberACurrentType = LootType.SafetyDepositBoxes;
					RobberARobbing = true;
					Crate_Valuables[RobberACurrentValualabe].IsRobbed = true;
					Crate_Valuables[RobberACurrentValualabe].LootBlip.Alpha = 0;
				}
				if (type == LootType.JewelStoreCabinet)
				{
					RobberACuttingStage = 0;
					RobberACurrentType = LootType.JewelStoreCabinet;
					RobberARobbingProp = P;
					RobberARobbing = true;
				}
				if (type == LootType.Painting)
				{
					RobberACurrentType = LootType.Painting;
					RobberA_CurrentPaintingNew = P;
					RobberARobbing = true;
				}
			}
			if (Ped == RobberB)
			{
				if (RobberB_HoldingLootProp != null)
				{
					RobberB_HoldingLootProp.Delete();
				}
				if (RobberB_bagProp != null)
				{
					RobberB_bagProp.Delete();
				}
				if (type == LootType.Trolly_Cash)
				{
					RobberBRobbing = true;
					RobberB_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberB.Position, false, false);
					RobberB_bagProp.IsCollisionEnabled = false;
					RobberB_bagProp.IsInvincible = true;
					RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberBSyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					RobberBSetRob = true;
					RobberBCurrentType = LootType.Trolly_Cash;
				}
				if (type == LootType.Trolly_Gold)
				{
					RobberBRobbing = true;
					if (RobberB_HoldingLootProp != null)
					{
						RobberB_HoldingLootProp.Delete();
					}
					if (RobberB_bagProp != null)
					{
						RobberB_bagProp.Delete();
					}
					RobberB_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberB.Position, false, false);
					RobberB_bagProp.IsCollisionEnabled = false;
					RobberB_bagProp.IsInvincible = true;
					RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberBSyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					RobberBSetRob = true;
					RobberBCurrentType = LootType.Trolly_Gold;
				}
				if (type == LootType.Trolly_Diamonds)
				{
					RobberBRobbing = true;
					if (RobberB_HoldingLootProp != null)
					{
						RobberB_HoldingLootProp.Delete();
					}
					if (RobberB_bagProp != null)
					{
						RobberB_bagProp.Delete();
					}
					RobberB_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberB.Position, false, false);
					RobberB_bagProp.IsCollisionEnabled = false;
					RobberB_bagProp.IsInvincible = true;
					RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberBSyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
					RobberBSetRob = true;
					RobberBCurrentType = LootType.Trolly_Diamonds;
				}
				if (type == LootType.CP_Cash)
				{
					RobberBFirstAnim = false;
					RobberBRobbing = true;
					RobberBRobTimer = 0;
					RobberB_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberB.Position, false, false);
					RobberB_bagProp.IsCollisionEnabled = false;
					RobberB_bagProp.IsInvincible = true;
					RobberBRobbingProp = P;
					RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					RobberBSetRob = true;
					RobberBCurrentType = LootType.CP_Cash;
				}
				if (type == LootType.CP_Gold)
				{
					RobberBFirstAnim = false;
					RobberBRobbing = true;
					RobberBRobTimer = 0;
					RobberB_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberB.Position, false, false);
					RobberB_bagProp.IsCollisionEnabled = false;
					RobberB_bagProp.IsInvincible = true;
					RobberBRobbingProp = P;
					RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					RobberBSetRob = true;
					RobberBCurrentType = LootType.CP_Gold;
				}
				if (type == LootType.M42_ServerStack)
				{
					RobberBFirstAnim = false;
					RobberBRobbing = true;
					RobberBRobTimer = 0;
					RobberB_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberB.Position, false, false);
					RobberB_bagProp.IsCollisionEnabled = false;
					RobberB_bagProp.IsInvincible = true;
					RobberBRobbingProp = P;
					RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberBSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
					RobberBSetRob = true;
					RobberBCurrentType = LootType.M42_ServerStack;
				}
				if (type == LootType.SafetyDepositBoxes)
				{
					RobberBCurrentLockBoxProp = P;
					RobberBtimer = 0;
					if (RobberB_HoldingLootProp != null)
					{
						RobberB_HoldingLootProp.Delete();
					}
					if (RobberB_bagProp != null)
					{
						RobberB_bagProp.Delete();
					}
					RobberB_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_vault_drill_01a"), RobberB.Position, false, false);
					RobberB_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberB.Position, false, false);
					RobberB_bagProp.IsCollisionEnabled = false;
					RobberB_bagProp.IsInvincible = true;
					RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01a"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
						RobberBCurrentLockBoxType = "abc";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01b"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
						RobberBCurrentLockBoxType = "abc";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01c"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
						RobberBCurrentLockBoxType = "abc";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01d"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
						RobberBCurrentLockBoxType = "def";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01e"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
						RobberBCurrentLockBoxType = "def";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01f"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
						RobberBCurrentLockBoxType = "def";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01g"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
						RobberBCurrentLockBoxType = "ghi";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01h"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
						RobberBCurrentLockBoxType = "ghi";
					}
					if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01i"))
					{
						RobberBCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
						RobberBCurrentLockBoxType = "ghi";
					}
					RobberBCurrentLootBoxRotation = Rot.Z;
					LoadDict(RobberBCurrentSDDict);
					RobberBSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberB, RobberBSyncScene, LoadDict(RobberBCurrentSDDict), "enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene));
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberBSyncScene, 0.0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB, RobberBSyncScene, "enter", LoadDict(RobberBCurrentSDDict), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_HoldingLootProp, RobberBSyncScene, "enter_ch_prop_vault_drill_01a", LoadDict(RobberBCurrentSDDict), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberB_bagProp, RobberBSyncScene, "enter_p_m_bag_var22_arm_s", LoadDict(RobberBCurrentSDDict), 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberBSyncScene, "enter_ch_prop_ch_sec_cabinet_01" + RobberBCurrentLockBoxType, LoadDict(RobberBCurrentSDDict), 1f, -1f, 0, 1148846080);
					RobberBCurrentLockbox = 1;
					RobberBCurrentLockboxStage = 1;
					RobberBInitialAnimDone = true;
					RobberBCurrentType = LootType.SafetyDepositBoxes;
					RobberBRobbing = true;
					Crate_Valuables[RobberBCurrentValualabe].IsRobbed = true;
					Crate_Valuables[RobberBCurrentValualabe].LootBlip.Alpha = 0;
				}
				if (type == LootType.JewelStoreCabinet)
				{
					RobberBCurrentType = LootType.JewelStoreCabinet;
					RobberBRobbingProp = P;
					RobberBRobbing = true;
				}
				if (type == LootType.Painting)
				{
					RobberBCuttingStage = 0;
					RobberBCurrentType = LootType.Painting;
					RobberB_CurrentPaintingNew = P;
					RobberBRobbing = true;
				}
			}
			if (!(Ped == RobberC))
			{
				return;
			}
			if (RobberC_HoldingLootProp != null)
			{
				RobberC_HoldingLootProp.Delete();
			}
			if (RobberC_bagProp != null)
			{
				RobberC_bagProp.Delete();
			}
			if (type == LootType.Trolly_Cash)
			{
				RobberCRobbing = true;
				RobberC_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberC.Position, false, false);
				RobberC_bagProp.IsCollisionEnabled = false;
				RobberC_bagProp.IsInvincible = true;
				RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberCSyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
				RobberCSetRob = true;
				RobberCCurrentType = LootType.Trolly_Cash;
			}
			if (type == LootType.Trolly_Gold)
			{
				Notification.Show("Call Robber C loot Found Gold");
				RobberCRobbing = true;
				if (RobberC_HoldingLootProp != null)
				{
					RobberC_HoldingLootProp.Delete();
				}
				if (RobberC_bagProp != null)
				{
					RobberC_bagProp.Delete();
				}
				RobberC_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberC.Position, false, false);
				RobberC_bagProp.IsCollisionEnabled = false;
				RobberC_bagProp.IsInvincible = true;
				RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberCSyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
				RobberCSetRob = true;
				RobberCCurrentType = LootType.Trolly_Gold;
			}
			if (type == LootType.Trolly_Diamonds)
			{
				Notification.Show("Call Robber C loot Found Diamonds");
				RobberCRobbing = true;
				if (RobberC_HoldingLootProp != null)
				{
					RobberC_HoldingLootProp.Delete();
				}
				if (RobberC_bagProp != null)
				{
					RobberC_bagProp.Delete();
				}
				RobberC_bagProp = World.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), RobberC.Position, false, false);
				RobberC_bagProp.IsCollisionEnabled = false;
				RobberC_bagProp.IsInvincible = true;
				RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "bag_intro", LoadDict("anim@heists@ornate_bank@grab_cash"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@heists@ornate_bank@grab_cash", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberCSyncScene, "cart_cash_dissapear", LoadDict("anim@heists@ornate_bank@grab_cash"), 16f, -16f, 0, 1148846080);
				RobberCSetRob = true;
				RobberCCurrentType = LootType.Trolly_Diamonds;
			}
			if (type == LootType.CP_Cash)
			{
				RobberCFirstAnim = false;
				RobberCRobbing = true;
				RobberCRobTimer = 0;
				RobberC_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberC.Position, false, false);
				RobberC_bagProp.IsCollisionEnabled = false;
				RobberC_bagProp.IsInvincible = true;
				RobberCRobbingProp = P;
				RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@cash@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@cash@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				RobberCSetRob = true;
				RobberCCurrentType = LootType.CP_Cash;
			}
			if (type == LootType.CP_Gold)
			{
				RobberCFirstAnim = false;
				RobberCRobbing = true;
				RobberCRobTimer = 0;
				RobberC_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberC.Position, false, false);
				RobberC_bagProp.IsCollisionEnabled = false;
				RobberC_bagProp.IsInvincible = true;
				RobberCRobbingProp = P;
				RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				RobberCSetRob = true;
				RobberCCurrentType = LootType.CP_Gold;
			}
			if (type == LootType.M42_ServerStack)
			{
				RobberCFirstAnim = false;
				RobberCRobbing = true;
				RobberCRobTimer = 0;
				RobberC_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberC.Position, false, false);
				RobberC_bagProp.IsCollisionEnabled = false;
				RobberC_bagProp.IsInvincible = true;
				RobberCRobbingProp = P;
				RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, RobberCSyncScene, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "enter_bag", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, "anim@scripted@heist@ig1_table_grab@gold@male@", "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "enter", LoadDict("anim@scripted@heist@ig1_table_grab@gold@male@"), 1000.0, -8f, -8f, Rot.Z + 180f, 1148846080, 0);
				RobberCSetRob = true;
				RobberCCurrentType = LootType.M42_ServerStack;
			}
			if (type == LootType.SafetyDepositBoxes)
			{
				RobberCCurrentLockBoxProp = P;
				RobberCtimer = 0;
				if (RobberC_HoldingLootProp != null)
				{
					RobberC_HoldingLootProp.Delete();
				}
				if (RobberC_bagProp != null)
				{
					RobberC_bagProp.Delete();
				}
				RobberC_HoldingLootProp = World.CreateProp(RequestModel("ch_prop_vault_drill_01a"), RobberC.Position, false, false);
				RobberC_bagProp = World.CreateProp(RequestModel("h4_p_h4_m_bag_var22_arm_s"), RobberC.Position, false, false);
				RobberC_bagProp.IsCollisionEnabled = false;
				RobberC_bagProp.IsInvincible = true;
				RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01a"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
					RobberCCurrentLockBoxType = "abc";
				}
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01b"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
					RobberCCurrentLockBoxType = "abc";
				}
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01c"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@";
					RobberCCurrentLockBoxType = "abc";
				}
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01d"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
					RobberCCurrentLockBoxType = "def";
				}
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01e"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
					RobberCCurrentLockBoxType = "def";
				}
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01f"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@";
					RobberCCurrentLockBoxType = "def";
				}
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01g"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
					RobberCCurrentLockBoxType = "ghi";
				}
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01h"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
					RobberCCurrentLockBoxType = "ghi";
				}
				if (P.Model == RequestModel("ch_prop_ch_sec_cabinet_01i"))
				{
					RobberCCurrentSDDict = "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@";
					RobberCCurrentLockBoxType = "ghi";
				}
				RobberCCurrentLootBoxRotation = Rot.Z;
				LoadDict(RobberCCurrentSDDict);
				RobberCSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, P.Position.X, P.Position.Y, P.Position.Z, 0.0, 0.0, Rot.Z, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, RobberC, RobberCSyncScene, LoadDict(RobberCCurrentSDDict), "enter", 3000.0, -2.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene));
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, RobberCSyncScene, 0.0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC, RobberCSyncScene, "enter", LoadDict(RobberCCurrentSDDict), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_HoldingLootProp, RobberCSyncScene, "enter_ch_prop_vault_drill_01a", LoadDict(RobberCCurrentSDDict), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobberC_bagProp, RobberCSyncScene, "enter_p_m_bag_var22_arm_s", LoadDict(RobberCCurrentSDDict), 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, P, RobberCSyncScene, "enter_ch_prop_ch_sec_cabinet_01" + RobberCCurrentLockBoxType, LoadDict(RobberCCurrentSDDict), 1f, -1f, 0, 1148846080);
				RobberCCurrentLockbox = 1;
				RobberCCurrentLockboxStage = 1;
				RobberCInitialAnimDone = true;
				RobberCCurrentType = LootType.SafetyDepositBoxes;
				RobberCRobbing = true;
				Crate_Valuables[RobberCCurrentValualabe].IsRobbed = true;
				Crate_Valuables[RobberCCurrentValualabe].LootBlip.Alpha = 0;
			}
			if (type == LootType.JewelStoreCabinet)
			{
				RobberCCurrentType = LootType.JewelStoreCabinet;
				RobberCRobbingProp = P;
				RobberCRobbing = true;
			}
			if (type == LootType.Painting)
			{
				RobberCCuttingStage = 0;
				RobberCCurrentType = LootType.Painting;
				RobberC_CurrentPaintingNew = P;
				RobberCRobbing = true;
			}
		}

		private static void disablewalkingvoid()
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

		public static void requestanimdict(string dict)
		{
			Function.Call(Hash.REQUEST_ANIM_DICT, dict);
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Script.Wait(0);
			}
		}

		private static void jewelcabsmash(Ped Ped, Prop glass)
		{
			if (Ped == Game.Player.Character)
			{
				switch (jewelsmashint)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					jewelsmashint = 1;
					break;
				}
				case 1:
					disablewalkingvoid();
					if (!Function.Call<bool>(Hash.IS_PED_WALKING, Ped) && Game.GameTime > unstucktimer)
					{
						jewelsmashint = 10;
					}
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model = new Model("des_jewel_cab2_end");
					model.Request(10000);
					if (model.IsValid && model.IsInCdImage)
					{
						while (!model.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model2 = new Model("des_jewel_cab2_root");
					model2.Request(10000);
					if (model2.IsValid && model2.IsInCdImage)
					{
						while (!model2.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model2, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model3 = new Model("des_jewel_cab2_rootb");
					model3.Request(10000);
					if (model3.IsValid && model3.IsInCdImage)
					{
						while (!model3.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model3, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					jewelsmashint = 20;
					break;
				}
				case 20:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop != null)
						{
							prop.Opacity = 0;
						}
						Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop2 != null)
						{
							prop2.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab2");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab2"))
						{
							Script.Wait(0);
						}
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, extra, "des_jewel_cab2_root", "des_jewel_cab2", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, extra2, "des_jewel_cab2_rootb", "des_jewel_cab2", 1f, -1f, 0, 1148846080);
						jewelsmashint = 30;
					}
					break;
				case 30:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, JewelStoreSyncScene) > 0.99f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						Script.Wait(50);
						Ped.Task.PlayAnimation("missheist_jewel", "smash_case_f_end", 4f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary);
						Script.Wait(1000);
						jewelsmashint = 0;
						PlayingAnim = false;
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 32, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 33, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 34, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 35, true);
					}
					break;
				}
			}
			if (Ped == RobberA)
			{
				switch (RobberA_jewelsmashint)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector2.X, argument2: vector2.Y, argument3: vector2.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					RobberA_jewelsmashint = 1;
					break;
				}
				case 1:
					RobberA_jewelsmashint = 10;
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model4 = new Model("des_jewel_cab2_end");
					model4.Request(10000);
					if (model4.IsValid && model4.IsInCdImage)
					{
						while (!model4.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model4, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model5 = new Model("des_jewel_cab2_root");
					model5.Request(10000);
					if (model5.IsValid && model5.IsInCdImage)
					{
						while (!model5.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model5, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model6 = new Model("des_jewel_cab2_rootb");
					model6.Request(10000);
					if (model6.IsValid && model6.IsInCdImage)
					{
						while (!model6.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model6, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					RobberA_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					RobberA_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberA_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					RobberA_jewelsmashint = 20;
					break;
				}
				case 20:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberA_jewelsmashint = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberA_JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop3 != null)
						{
							prop3.Opacity = 0;
						}
						Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop4 != null)
						{
							prop4.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab2");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab2"))
						{
							Script.Wait(0);
						}
						RobberA_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						RobberA_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberA_extra, "des_jewel_cab2_root", "des_jewel_cab2", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberA_extra2, "des_jewel_cab2_rootb", "des_jewel_cab2", 1f, -1f, 0, 1148846080);
						RobberA_jewelsmashint = 30;
					}
					break;
				case 30:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberA_jewelsmashint = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberA_JewelStoreSyncScene) > 0.99f)
					{
						RobberA_JustFinishedSmashingCabinet = true;
						RobberA_ResetSmashTimer = Game.GameTime + 1000;
						RobberARobbing = false;
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberA_extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberA_extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						RobberA_jewelsmashint = 60;
						Script.Wait(1);
						RobberARobbing = false;
						Script.Wait(1);
						RobberARobbing = false;
					}
					break;
				}
			}
			if (Ped == RobberB)
			{
				switch (RobberB_jewelsmashint)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector3 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector3.X, argument2: vector3.Y, argument3: vector3.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					RobberB_jewelsmashint = 1;
					break;
				}
				case 1:
					RobberB_jewelsmashint = 10;
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model7 = new Model("des_jewel_cab2_end");
					model7.Request(10000);
					if (model7.IsValid && model7.IsInCdImage)
					{
						while (!model7.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model7, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model8 = new Model("des_jewel_cab2_root");
					model8.Request(10000);
					if (model8.IsValid && model8.IsInCdImage)
					{
						while (!model8.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model8, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model9 = new Model("des_jewel_cab2_rootb");
					model9.Request(10000);
					if (model9.IsValid && model9.IsInCdImage)
					{
						while (!model9.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model9, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					RobberB_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					RobberB_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberB_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					RobberB_jewelsmashint = 20;
					break;
				}
				case 20:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberB_jewelsmashint = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberB_JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop5 != null)
						{
							prop5.Opacity = 0;
						}
						Prop prop6 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop6 != null)
						{
							prop6.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab2");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab2"))
						{
							Script.Wait(0);
						}
						RobberB_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						RobberB_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberB_extra, "des_jewel_cab2_root", "des_jewel_cab2", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberB_extra2, "des_jewel_cab2_rootb", "des_jewel_cab2", 1f, -1f, 0, 1148846080);
						RobberB_jewelsmashint = 30;
					}
					break;
				case 30:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberB_jewelsmashint = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberB_JewelStoreSyncScene) > 0.99f)
					{
						RobberB_JustFinishedSmashingCabinet = true;
						RobberBRobbing = false;
						RobberB_ResetSmashTimer = Game.GameTime + 1000;
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberB_extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberB_extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						RobberB_jewelsmashint = 60;
						Script.Wait(1);
						RobberBRobbing = false;
						Script.Wait(1);
						RobberBRobbing = false;
					}
					break;
				}
			}
			if (!(Ped == RobberC))
			{
				return;
			}
			switch (RobberC_jewelsmashint)
			{
			case 0:
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
				jewelsmashrot = glass.Rotation.Z;
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Vector3 vector4 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector4.X, argument2: vector4.Y, argument3: vector4.Z, argument4: 1f, argument5: 3000, argument7: 0f);
				unstucktimer = Game.GameTime + 2000;
				RobberC_jewelsmashint = 1;
				break;
			}
			case 1:
				RobberC_jewelsmashint = 10;
				break;
			case 10:
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Model model10 = new Model("des_jewel_cab2_end");
				model10.Request(10000);
				if (model10.IsValid && model10.IsInCdImage)
				{
					while (!model10.IsLoaded)
					{
						Script.Wait(50);
					}
					brokenjewelglass = World.CreateProp(model10, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
					brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					brokenjewelglass.IsPositionFrozen = true;
					brokenjewelglass.IsVisible = false;
				}
				Model model11 = new Model("des_jewel_cab2_root");
				model11.Request(10000);
				if (model11.IsValid && model11.IsInCdImage)
				{
					while (!model11.IsLoaded)
					{
						Script.Wait(50);
					}
					glassroot = World.CreateProp(model11, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassroot.IsPositionFrozen = true;
					glassroot.IsVisible = false;
				}
				Model model12 = new Model("des_jewel_cab2_rootb");
				model12.Request(10000);
				if (model12.IsValid && model12.IsInCdImage)
				{
					while (!model12.IsLoaded)
					{
						Script.Wait(50);
					}
					glassrootb = World.CreateProp(model12, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassrootb.IsPositionFrozen = true;
					glassrootb.IsVisible = false;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				RobberC_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				RobberC_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberC_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
				Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				RobberC_jewelsmashint = 20;
				break;
			}
			case 20:
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
				{
					RobberC_jewelsmashint = 10;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberC_JewelStoreSyncScene) > 0.2f)
				{
					while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Script.Wait(0);
					}
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
					if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
					}
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glass.IsVisible = false;
					Prop prop7 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
					if (prop7 != null)
					{
						prop7.Opacity = 0;
					}
					Prop prop8 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
					if (prop8 != null)
					{
						prop8.Opacity = 0;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab2");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab2"))
					{
						Script.Wait(0);
					}
					RobberC_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
					RobberC_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberC_extra, "des_jewel_cab2_root", "des_jewel_cab2", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberC_extra2, "des_jewel_cab2_rootb", "des_jewel_cab2", 1f, -1f, 0, 1148846080);
					RobberC_jewelsmashint = 30;
				}
				break;
			case 30:
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
				{
					RobberC_jewelsmashint = 10;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberC_JewelStoreSyncScene) > 0.99f)
				{
					RobberC_JustFinishedSmashingCabinet = true;
					RobberCRobbing = false;
					RobberC_ResetSmashTimer = Game.GameTime + 1000;
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberC_extra, 0f);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberC_extra2, 0f);
					glass.IsVisible = false;
					brokenjewelglass.IsVisible = true;
					glassroot.IsVisible = false;
					glassrootb.IsVisible = false;
					JewelCases_Smashed.Add(brokenjewelglass);
					JewelCases_Smashed.Add(glassroot);
					JewelCases_Smashed.Add(glassrootb);
					brokenjewelglass.MarkAsNoLongerNeeded();
					glassrootb.MarkAsNoLongerNeeded();
					glassroot.MarkAsNoLongerNeeded();
					jewelbrokenglasslist.Add(brokenjewelglass);
					jewelrootlist.Add(glassroot);
					jewelrootBlist.Add(glassrootb);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
					glass.Delete();
					Ped.Task.ClearAll();
					RobberC_jewelsmashint = 60;
					Script.Wait(1);
					RobberCRobbing = false;
					Script.Wait(1);
					RobberCRobbing = false;
				}
				break;
			}
		}

		private static void jewelcabsmash2(Ped Ped, Prop glass)
		{
			if (Ped == Game.Player.Character)
			{
				switch (jewelsmashint2)
				{
				case 0:
				{
					if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && Game.Player.Character.Weapons.Current.Group == WeaponGroup.AssaultRifle)
					{
						guns = Game.Player.Character.Weapons.Current.Hash;
					}
					if (Game.Player.Character.Weapons.Current.Group == WeaponGroup.AssaultRifle)
					{
						if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.CompactRifle)
						{
							if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AssaultRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.HeavyRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AdvancedRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AdvancedRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultrifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AssaultrifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbine))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbine);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
							}
						}
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AssaultRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.HeavyRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AdvancedRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AdvancedRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultrifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AssaultrifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbine);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					if (Game.Player.Character.Position.DistanceTo(new Vector3(-626.62f, -235.725f, 37.0946f)) < 1.5f)
					{
						jewelsmashpos = new Vector3(-626.62f, -235.725f, 37.0946f);
						jewelsmashrot = 36f;
					}
					if (Game.Player.Character.Position.DistanceTo(new Vector3(-619.39f, -233.32f, 37.0946f)) < 1.5f)
					{
						jewelsmashpos = new Vector3(-619.39f, -233.32f, 37.0946f);
						jewelsmashrot = -144f;
					}
					if (Game.Player.Character.Position.DistanceTo(new Vector3(-618.679f, -229.704f, 37.0946f)) < 1.5f)
					{
						jewelsmashpos = new Vector3(-618.679f, -229.704f, 37.0946f);
						jewelsmashrot = -54f;
					}
					if (Game.Player.Character.Position.DistanceTo(new Vector3(-624.738f, -228.2f, 37.0946f)) < 1.5f)
					{
						jewelsmashpos = new Vector3(-624.738f, -228.2f, 37.0946f);
						jewelsmashrot = 36f;
					}
					if (Game.Player.Character.Position.DistanceTo(new Vector3(-620.864f, -228.481f, 37.0946f)) < 1.5f)
					{
						jewelsmashpos = new Vector3(-620.864f, -228.481f, 37.0946f);
						jewelsmashrot = 126.925f;
					}
					if (Game.Player.Character.Position.DistanceTo(new Vector3(-625.867f, -237.458f, 37.0946f)) < 1.5f)
					{
						jewelsmashpos = new Vector3(-625.867f, -237.458f, 37.0946f);
						jewelsmashrot = -144f;
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					jewelsmashint2 = 1;
					break;
				}
				case 1:
					disablewalkingvoid();
					if (!Function.Call<bool>(Hash.IS_PED_WALKING, Ped) && Game.GameTime > unstucktimer)
					{
						jewelsmashint2 = 10;
					}
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model = new Model("des_jewel_cab3_end");
					model.Request(10000);
					if (model.IsValid && model.IsInCdImage)
					{
						while (!model.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model2 = new Model("des_jewel_cab3_root");
					model2.Request(10000);
					if (model2.IsValid && model2.IsInCdImage)
					{
						while (!model2.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model2, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model3 = new Model("des_jewel_cab3_rootb");
					model3.Request(10000);
					if (model3.IsValid && model3.IsInCdImage)
					{
						while (!model3.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model3, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					jewelsmashint2 = 20;
					break;
				}
				case 20:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop != null)
						{
							prop.Opacity = 0;
						}
						Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop2 != null)
						{
							prop2.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab3");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab3"))
						{
							Script.Wait(0);
						}
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, extra, "des_jewel_cab3_root", "des_jewel_cab3", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, extra2, "des_jewel_cab3_rootb", "des_jewel_cab3", 1f, -1f, 0, 1148846080);
						jewelsmashint2 = 30;
					}
					break;
				case 30:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, JewelStoreSyncScene) > 0.99f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						Script.Wait(50);
						Ped.Task.PlayAnimation("missheist_jewel", "smash_case_f_end", 4f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary);
						Script.Wait(1000);
						jewelsmashint2 = 0;
						PlayingAnim = false;
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 32, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 33, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 34, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 35, true);
					}
					break;
				}
			}
			if (Ped == RobberA)
			{
				switch (RobberA_jewelsmashint2)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector2.X, argument2: vector2.Y, argument3: vector2.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					RobberA_jewelsmashint2 = 1;
					break;
				}
				case 1:
					RobberA_jewelsmashint2 = 10;
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model4 = new Model("des_jewel_cab3_end");
					model4.Request(10000);
					if (model4.IsValid && model4.IsInCdImage)
					{
						while (!model4.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model4, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model5 = new Model("des_jewel_cab3_root");
					model5.Request(10000);
					if (model5.IsValid && model5.IsInCdImage)
					{
						while (!model5.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model5, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model6 = new Model("des_jewel_cab3_rootb");
					model6.Request(10000);
					if (model6.IsValid && model6.IsInCdImage)
					{
						while (!model6.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model6, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					RobberA_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					RobberA_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberA_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					RobberA_jewelsmashint2 = 20;
					break;
				}
				case 20:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberA_jewelsmashint2 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberA_JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop3 != null)
						{
							prop3.Opacity = 0;
						}
						Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop4 != null)
						{
							prop4.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab3");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab3"))
						{
							Script.Wait(0);
						}
						RobberA_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						RobberA_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberA_extra, "des_jewel_cab3_root", "des_jewel_cab3", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberA_extra2, "des_jewel_cab3_rootb", "des_jewel_cab3", 1f, -1f, 0, 1148846080);
						RobberA_jewelsmashint2 = 30;
					}
					break;
				case 30:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberA_jewelsmashint2 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberA_JewelStoreSyncScene) > 0.99f)
					{
						RobberA_JustFinishedSmashingCabinet = true;
						RobberA_ResetSmashTimer = Game.GameTime + 1000;
						RobberARobbing = false;
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberA_extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberA_extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						RobberA_jewelsmashint2 = 60;
						Script.Wait(1);
						RobberARobbing = false;
						Script.Wait(1);
						RobberARobbing = false;
						Notification.Show("Finish Robber A Smash Cabinet " + RobberARobbing);
					}
					break;
				}
			}
			if (Ped == RobberB)
			{
				switch (RobberB_jewelsmashint2)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector3 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector3.X, argument2: vector3.Y, argument3: vector3.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					RobberB_jewelsmashint2 = 1;
					break;
				}
				case 1:
					RobberB_jewelsmashint2 = 10;
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model7 = new Model("des_jewel_cab3_end");
					model7.Request(10000);
					if (model7.IsValid && model7.IsInCdImage)
					{
						while (!model7.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model7, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model8 = new Model("des_jewel_cab3_root");
					model8.Request(10000);
					if (model8.IsValid && model8.IsInCdImage)
					{
						while (!model8.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model8, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model9 = new Model("des_jewel_cab3_rootb");
					model9.Request(10000);
					if (model9.IsValid && model9.IsInCdImage)
					{
						while (!model9.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model9, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					RobberB_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					RobberB_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberB_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					RobberB_jewelsmashint2 = 20;
					break;
				}
				case 20:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberB_jewelsmashint2 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberB_JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop5 != null)
						{
							prop5.Opacity = 0;
						}
						Prop prop6 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop6 != null)
						{
							prop6.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab3");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab3"))
						{
							Script.Wait(0);
						}
						RobberB_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						RobberB_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberB_extra, "des_jewel_cab3_root", "des_jewel_cab3", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberB_extra2, "des_jewel_cab3_rootb", "des_jewel_cab3", 1f, -1f, 0, 1148846080);
						RobberB_jewelsmashint2 = 30;
					}
					break;
				case 30:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberB_jewelsmashint2 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberB_JewelStoreSyncScene) > 0.99f)
					{
						RobberB_JustFinishedSmashingCabinet = true;
						RobberB_ResetSmashTimer = Game.GameTime + 1000;
						RobberBRobbing = false;
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberB_extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberB_extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						RobberB_jewelsmashint2 = 60;
						Script.Wait(1);
						RobberBRobbing = false;
						Script.Wait(1);
						RobberBRobbing = false;
					}
					break;
				}
			}
			if (!(Ped == RobberC))
			{
				return;
			}
			switch (RobberC_jewelsmashint2)
			{
			case 0:
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
				jewelsmashrot = glass.Rotation.Z;
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Vector3 vector4 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector4.X, argument2: vector4.Y, argument3: vector4.Z, argument4: 1f, argument5: 3000, argument7: 0f);
				unstucktimer = Game.GameTime + 2000;
				RobberC_jewelsmashint2 = 1;
				break;
			}
			case 1:
				RobberC_jewelsmashint2 = 10;
				break;
			case 10:
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Model model10 = new Model("des_jewel_cab3_end");
				model10.Request(10000);
				if (model10.IsValid && model10.IsInCdImage)
				{
					while (!model10.IsLoaded)
					{
						Script.Wait(50);
					}
					brokenjewelglass = World.CreateProp(model10, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
					brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					brokenjewelglass.IsPositionFrozen = true;
					brokenjewelglass.IsVisible = false;
				}
				Model model11 = new Model("des_jewel_cab3_root");
				model11.Request(10000);
				if (model11.IsValid && model11.IsInCdImage)
				{
					while (!model11.IsLoaded)
					{
						Script.Wait(50);
					}
					glassroot = World.CreateProp(model11, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassroot.IsPositionFrozen = true;
					glassroot.IsVisible = false;
				}
				Model model12 = new Model("des_jewel_cab3_rootb");
				model12.Request(10000);
				if (model12.IsValid && model12.IsInCdImage)
				{
					while (!model12.IsLoaded)
					{
						Script.Wait(50);
					}
					glassrootb = World.CreateProp(model12, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassrootb.IsPositionFrozen = true;
					glassrootb.IsVisible = false;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				RobberC_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				RobberC_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberC_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
				Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				RobberC_jewelsmashint2 = 20;
				break;
			}
			case 20:
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
				{
					RobberC_jewelsmashint2 = 10;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberC_JewelStoreSyncScene) > 0.2f)
				{
					while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Script.Wait(0);
					}
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
					if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
					}
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glass.IsVisible = false;
					Prop prop7 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
					if (prop7 != null)
					{
						prop7.Opacity = 0;
					}
					Prop prop8 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
					if (prop8 != null)
					{
						prop8.Opacity = 0;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab3");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab3"))
					{
						Script.Wait(0);
					}
					RobberC_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
					RobberC_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberC_extra, "des_jewel_cab3_root", "des_jewel_cab3", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberC_extra2, "des_jewel_cab3_rootb", "des_jewel_cab3", 1f, -1f, 0, 1148846080);
					RobberC_jewelsmashint2 = 30;
				}
				break;
			case 30:
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
				{
					RobberC_jewelsmashint2 = 10;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberC_JewelStoreSyncScene) > 0.99f)
				{
					RobberC_JustFinishedSmashingCabinet = true;
					RobberC_ResetSmashTimer = Game.GameTime + 1000;
					RobberCRobbing = false;
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberC_extra, 0f);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberC_extra2, 0f);
					glass.IsVisible = false;
					brokenjewelglass.IsVisible = true;
					glassroot.IsVisible = false;
					glassrootb.IsVisible = false;
					JewelCases_Smashed.Add(brokenjewelglass);
					JewelCases_Smashed.Add(glassroot);
					JewelCases_Smashed.Add(glassrootb);
					brokenjewelglass.MarkAsNoLongerNeeded();
					glassrootb.MarkAsNoLongerNeeded();
					glassroot.MarkAsNoLongerNeeded();
					jewelbrokenglasslist.Add(brokenjewelglass);
					jewelrootlist.Add(glassroot);
					jewelrootBlist.Add(glassrootb);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
					glass.Delete();
					Ped.Task.ClearAll();
					RobberC_jewelsmashint2 = 60;
					Script.Wait(1);
					RobberCRobbing = false;
					Script.Wait(1);
					RobberCRobbing = false;
				}
				break;
			}
		}

		private static void jewelcabsmash3(Ped Ped, Prop glass)
		{
			if (Ped == Game.Player.Character)
			{
				switch (jewelsmashint3)
				{
				case 0:
				{
					if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && Game.Player.Character.Weapons.Current.Group == WeaponGroup.AssaultRifle)
					{
						guns = Game.Player.Character.Weapons.Current.Hash;
					}
					if (Game.Player.Character.Weapons.Current.Group == WeaponGroup.AssaultRifle)
					{
						if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.CompactRifle)
						{
							if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AssaultRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.HeavyRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AdvancedRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AdvancedRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultrifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AssaultrifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbine))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbine);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
							}
						}
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AssaultRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.HeavyRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AdvancedRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AdvancedRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultrifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AssaultrifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbine);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(1f, 0f, -0.6f));
					jewelsmashrot = glass.Rotation.Z + 90f;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					jewelsmashint3 = 1;
					break;
				}
				case 1:
					disablewalkingvoid();
					if (!Function.Call<bool>(Hash.IS_PED_WALKING, Ped) && Game.GameTime > unstucktimer)
					{
						jewelsmashint3 = 10;
					}
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model = new Model("des_jewel_cab4_root");
					model.Request(10000);
					if (model.IsValid && model.IsInCdImage)
					{
						while (!model.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model2 = new Model("des_jewel_cab4_rootb");
					model2.Request(10000);
					if (model2.IsValid && model2.IsInCdImage)
					{
						while (!model2.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model2, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_b", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					jewelsmashint3 = 20;
					break;
				}
				case 20:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, JewelStoreSyncScene) > 0.05f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop != null)
						{
							prop.Opacity = 0;
						}
						Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop2 != null)
						{
							prop2.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab4");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab4"))
						{
							Script.Wait(0);
						}
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, extra, "des_jewel_cab4_root", "des_jewel_cab4", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, extra2, "des_jewel_cab4_rootb", "des_jewel_cab4", 1f, -1f, 0, 1148846080);
						jewelsmashint3 = 30;
					}
					break;
				case 30:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, JewelStoreSyncScene) > 0.999f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra2, 0f);
						glass.IsVisible = false;
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						JewelCases_Smashed.Add(glassrootb);
						JewelCases_Smashed.Add(glassrootb);
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Game.Player.Character.Task.ClearAll();
						Script.Wait(50);
						Game.Player.Character.Task.PlayAnimation("missheist_jewel", "smash_case_f_end", 4f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary);
						Script.Wait(1000);
						jewelsmashint3 = 0;
						PlayingAnim = false;
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 32, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 33, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 34, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 35, true);
						Crate_Valuables[CurrentLootTarget].GotValuables = true;
					}
					break;
				}
			}
			if (Ped == RobberA)
			{
				switch (RobberA_jewelsmashint3)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(1f, 0f, -0.6f));
					jewelsmashrot = glass.Rotation.Z + 90f;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector2.X, argument2: vector2.Y, argument3: vector2.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					RobberA_jewelsmashint3 = 1;
					break;
				}
				case 1:
					RobberA_jewelsmashint3 = 10;
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model3 = new Model("des_jewel_cab4_root");
					model3.Request(10000);
					if (model3.IsValid && model3.IsInCdImage)
					{
						while (!model3.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model3, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model4 = new Model("des_jewel_cab4_rootb");
					model4.Request(10000);
					if (model4.IsValid && model4.IsInCdImage)
					{
						while (!model4.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model4, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					RobberA_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					RobberA_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberA_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_b", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					RobberA_jewelsmashint3 = 20;
					break;
				}
				case 20:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_b", 3))
					{
						RobberA_jewelsmashint3 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberA_JewelStoreSyncScene) > 0.05f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop3 != null)
						{
							prop3.Opacity = 0;
						}
						Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop4 != null)
						{
							prop4.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab4");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab4"))
						{
							Script.Wait(0);
						}
						RobberA_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						RobberA_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberA_extra, "des_jewel_cab4_root", "des_jewel_cab4", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberA_extra2, "des_jewel_cab4_rootb", "des_jewel_cab4", 1f, -1f, 0, 1148846080);
						RobberA_jewelsmashint3 = 30;
					}
					break;
				case 30:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_b", 3))
					{
						RobberA_jewelsmashint3 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberA_JewelStoreSyncScene) > 0.999f)
					{
						RobberA_JustFinishedSmashingCabinet = true;
						RobberA_ResetSmashTimer = Game.GameTime + 1000;
						RobberARobbing = false;
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberA_extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberA_extra2, 0f);
						glass.IsVisible = false;
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						JewelCases_Smashed.Add(glassrootb);
						JewelCases_Smashed.Add(glassrootb);
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						RobberA.Task.ClearAll();
						RobberA_jewelsmashint3 = 60;
						Script.Wait(1);
						RobberARobbing = false;
						Script.Wait(1);
						RobberARobbing = false;
						Notification.Show("Finish Robber A Smash Cabinet " + RobberARobbing);
					}
					break;
				}
			}
			if (Ped == RobberB)
			{
				switch (RobberB_jewelsmashint3)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(1f, 0f, -0.6f));
					jewelsmashrot = glass.Rotation.Z + 90f;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector3 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector3.X, argument2: vector3.Y, argument3: vector3.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 2000;
					RobberB_jewelsmashint3 = 1;
					break;
				}
				case 1:
					RobberB_jewelsmashint3 = 10;
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model5 = new Model("des_jewel_cab4_root");
					model5.Request(10000);
					if (model5.IsValid && model5.IsInCdImage)
					{
						while (!model5.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model5, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model6 = new Model("des_jewel_cab4_rootb");
					model6.Request(10000);
					if (model6.IsValid && model6.IsInCdImage)
					{
						while (!model6.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model6, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					RobberB_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					RobberB_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberB_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_b", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					RobberB_jewelsmashint3 = 20;
					break;
				}
				case 20:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_b", 3))
					{
						RobberB_jewelsmashint3 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberB_JewelStoreSyncScene) > 0.05f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop5 != null)
						{
							prop5.Opacity = 0;
						}
						Prop prop6 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop6 != null)
						{
							prop6.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab4");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab4"))
						{
							Script.Wait(0);
						}
						RobberB_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						RobberB_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberB_extra, "des_jewel_cab4_root", "des_jewel_cab4", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberB_extra2, "des_jewel_cab4_rootb", "des_jewel_cab4", 1f, -1f, 0, 1148846080);
						RobberB_jewelsmashint3 = 30;
					}
					break;
				case 30:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_b", 3))
					{
						RobberB_jewelsmashint3 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberB_JewelStoreSyncScene) > 0.999f)
					{
						RobberB_JustFinishedSmashingCabinet = true;
						RobberB_ResetSmashTimer = Game.GameTime + 1000;
						RobberBRobbing = false;
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberB_extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberB_extra2, 0f);
						glass.IsVisible = false;
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						JewelCases_Smashed.Add(glassrootb);
						JewelCases_Smashed.Add(glassrootb);
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						RobberB.Task.ClearAll();
						RobberB_jewelsmashint3 = 60;
						Script.Wait(1);
						RobberBRobbing = false;
						Script.Wait(1);
						RobberBRobbing = false;
					}
					break;
				}
			}
			if (!(Ped == RobberC))
			{
				return;
			}
			switch (RobberC_jewelsmashint3)
			{
			case 0:
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				jewelsmashpos = glass.GetOffsetPosition(new Vector3(1f, 0f, -0.6f));
				jewelsmashrot = glass.Rotation.Z + 90f;
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Vector3 vector4 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector4.X, argument2: vector4.Y, argument3: vector4.Z, argument4: 1f, argument5: 3000, argument7: 0f);
				unstucktimer = Game.GameTime + 2000;
				RobberC_jewelsmashint3 = 1;
				break;
			}
			case 1:
				RobberC_jewelsmashint3 = 10;
				break;
			case 10:
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Model model7 = new Model("des_jewel_cab4_root");
				model7.Request(10000);
				if (model7.IsValid && model7.IsInCdImage)
				{
					while (!model7.IsLoaded)
					{
						Script.Wait(50);
					}
					glassroot = World.CreateProp(model7, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
					glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassroot.IsPositionFrozen = true;
					glassroot.IsVisible = false;
				}
				Model model8 = new Model("des_jewel_cab4_rootb");
				model8.Request(10000);
				if (model8.IsValid && model8.IsInCdImage)
				{
					while (!model8.IsLoaded)
					{
						Script.Wait(50);
					}
					glassrootb = World.CreateProp(model8, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
					glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassrootb.IsPositionFrozen = true;
					glassrootb.IsVisible = false;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				RobberC_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				RobberC_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberC_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_b", 2f, -4f, 1, 0, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
				Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				RobberC_jewelsmashint3 = 20;
				break;
			}
			case 20:
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_b", 3))
				{
					RobberC_jewelsmashint3 = 10;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberC_JewelStoreSyncScene) > 0.05f)
				{
					while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Script.Wait(0);
					}
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
					if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
					}
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glass.IsVisible = false;
					Prop prop7 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
					if (prop7 != null)
					{
						prop7.Opacity = 0;
					}
					Prop prop8 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
					if (prop8 != null)
					{
						prop8.Opacity = 0;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab4");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab4"))
					{
						Script.Wait(0);
					}
					RobberC_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
					RobberC_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberC_extra, "des_jewel_cab4_root", "des_jewel_cab4", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberC_extra2, "des_jewel_cab4_rootb", "des_jewel_cab4", 1f, -1f, 0, 1148846080);
					RobberC_jewelsmashint3 = 30;
				}
				break;
			case 30:
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_b", 3))
				{
					RobberC_jewelsmashint3 = 10;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberC_JewelStoreSyncScene) > 0.999f)
				{
					RobberC_JustFinishedSmashingCabinet = true;
					RobberC_ResetSmashTimer = Game.GameTime + 1000;
					RobberCRobbing = false;
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberC_extra, 0f);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberC_extra2, 0f);
					glass.IsVisible = false;
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					JewelCases_Smashed.Add(glassrootb);
					JewelCases_Smashed.Add(glassrootb);
					glassrootb.MarkAsNoLongerNeeded();
					glassroot.MarkAsNoLongerNeeded();
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
					glass.Delete();
					RobberC.Task.ClearAll();
					RobberC_jewelsmashint3 = 60;
					Script.Wait(1);
					RobberCRobbing = false;
					Script.Wait(1);
					RobberCRobbing = false;
				}
				break;
			}
		}

		private static void jewelcabsmash4(Ped Ped, Prop glass)
		{
			if (Ped == Game.Player.Character)
			{
				switch (jewelsmashint4)
				{
				case 0:
				{
					if (Game.Player.Character.Weapons.Current.Hash != WeaponHash.Unarmed && Game.Player.Character.Weapons.Current.Group == WeaponGroup.AssaultRifle)
					{
						guns = Game.Player.Character.Weapons.Current.Hash;
					}
					if (Game.Player.Character.Weapons.Current.Group == WeaponGroup.AssaultRifle)
					{
						if (Game.Player.Character.Weapons.Current.Hash == WeaponHash.CompactRifle)
						{
							if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AssaultRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.HeavyRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AdvancedRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AdvancedRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifle);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultrifleMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.AssaultrifleMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbine))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbine);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
							}
							else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
							{
								Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
							}
						}
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AssaultRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.HeavyRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.HeavyRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AdvancedRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AdvancedRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifle);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.BullpupRifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.BullpupRifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.CarbineRifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.AssaultrifleMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.AssaultrifleMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbine))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbine);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.SpecialCarbineMk2))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.SpecialCarbineMk2);
					}
					else if (Game.Player.Character.Weapons.HasWeapon(WeaponHash.MilitaryRifle))
					{
						Game.Player.Character.Weapons.Select(WeaponHash.MilitaryRifle);
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 1000;
					jewelsmashint4 = 1;
					break;
				}
				case 1:
					disablewalkingvoid();
					if (!Function.Call<bool>(Hash.IS_PED_WALKING, Ped) && Game.GameTime > unstucktimer)
					{
						jewelsmashint4 = 10;
					}
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model = new Model("des_jewel_cab_end");
					model.Request(10000);
					if (model.IsValid && model.IsInCdImage)
					{
						while (!model.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model2 = new Model("des_jewel_cab_root");
					model2.Request(10000);
					if (model2.IsValid && model2.IsInCdImage)
					{
						while (!model2.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model2, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model3 = new Model("des_jewel_cab_root2");
					model3.Request(10000);
					if (model3.IsValid && model3.IsInCdImage)
					{
						while (!model3.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model3, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					jewelsmashint4 = 20;
					break;
				}
				case 20:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop != null)
						{
							prop.Opacity = 0;
						}
						Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop2 != null)
						{
							prop2.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab"))
						{
							Script.Wait(0);
						}
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, extra, "des_jewel_cab_root", "des_jewel_cab", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, extra2, "des_jewel_cab_root2", "des_jewel_cab", 1f, -1f, 0, 1148846080);
						jewelsmashint4 = 30;
					}
					break;
				case 30:
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, JewelStoreSyncScene) > 0.99f)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						Script.Wait(50);
						Ped.Task.PlayAnimation("missheist_jewel", "smash_case_f_end", 4f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary);
						Script.Wait(1000);
						jewelsmashint4 = 0;
						PlayingAnim = false;
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 32, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 33, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 34, true);
						Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 35, true);
					}
					break;
				}
			}
			if (Ped == RobberA)
			{
				switch (RobberA_jewelsmashint4)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector2.X, argument2: vector2.Y, argument3: vector2.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 1000;
					RobberA_jewelsmashint4 = 1;
					break;
				}
				case 1:
					RobberA_jewelsmashint4 = 10;
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model4 = new Model("des_jewel_cab_end");
					model4.Request(10000);
					if (model4.IsValid && model4.IsInCdImage)
					{
						while (!model4.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model4, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model5 = new Model("des_jewel_cab_root");
					model5.Request(10000);
					if (model5.IsValid && model5.IsInCdImage)
					{
						while (!model5.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model5, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model6 = new Model("des_jewel_cab_root2");
					model6.Request(10000);
					if (model6.IsValid && model6.IsInCdImage)
					{
						while (!model6.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model6, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					RobberA_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					RobberA_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberA_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					RobberA_jewelsmashint4 = 20;
					break;
				}
				case 20:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberA_jewelsmashint4 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberA_JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop3 != null)
						{
							prop3.Opacity = 0;
						}
						Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop4 != null)
						{
							prop4.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab"))
						{
							Script.Wait(0);
						}
						RobberA_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						RobberA_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberA_extra, "des_jewel_cab_root", "des_jewel_cab", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberA_extra2, "des_jewel_cab_root2", "des_jewel_cab", 1f, -1f, 0, 1148846080);
						RobberA_jewelsmashint4 = 30;
					}
					break;
				case 30:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberA_jewelsmashint4 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberA_JewelStoreSyncScene) > 0.99f)
					{
						RobberARobbing = false;
						RobberA_JustFinishedSmashingCabinet = true;
						RobberA_ResetSmashTimer = Game.GameTime + 1000;
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberA_extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberA_extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						RobberA_jewelsmashint4 = 60;
						Script.Wait(1);
						RobberARobbing = false;
						Script.Wait(1);
						RobberARobbing = false;
						Notification.Show("Finish Robber A Smash Cabinet " + RobberARobbing);
					}
					break;
				}
			}
			if (Ped == RobberB)
			{
				switch (RobberB_jewelsmashint4)
				{
				case 0:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
					jewelsmashrot = glass.Rotation.Z;
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Vector3 vector3 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector3.X, argument2: vector3.Y, argument3: vector3.Z, argument4: 1f, argument5: 3000, argument7: 0f);
					unstucktimer = Game.GameTime + 1000;
					RobberB_jewelsmashint4 = 1;
					break;
				}
				case 1:
					RobberB_jewelsmashint4 = 10;
					break;
				case 10:
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
					{
						Script.Wait(0);
					}
					Model model7 = new Model("des_jewel_cab_end");
					model7.Request(10000);
					if (model7.IsValid && model7.IsInCdImage)
					{
						while (!model7.IsLoaded)
						{
							Script.Wait(50);
						}
						brokenjewelglass = World.CreateProp(model7, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
						brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						brokenjewelglass.IsPositionFrozen = true;
						brokenjewelglass.IsVisible = false;
					}
					Model model8 = new Model("des_jewel_cab_root");
					model8.Request(10000);
					if (model8.IsValid && model8.IsInCdImage)
					{
						while (!model8.IsLoaded)
						{
							Script.Wait(50);
						}
						glassroot = World.CreateProp(model8, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassroot.IsPositionFrozen = true;
						glassroot.IsVisible = false;
					}
					Model model9 = new Model("des_jewel_cab_root2");
					model9.Request(10000);
					if (model9.IsValid && model9.IsInCdImage)
					{
						while (!model9.IsLoaded)
						{
							Script.Wait(50);
						}
						glassrootb = World.CreateProp(model9, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
						glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
						glassrootb.IsPositionFrozen = true;
						glassrootb.IsVisible = false;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
					RobberB_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					RobberB_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberB_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					RobberB_jewelsmashint4 = 20;
					break;
				}
				case 20:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberB_jewelsmashint4 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberB_JewelStoreSyncScene) > 0.2f)
					{
						while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Script.Wait(0);
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
							smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
						}
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
						Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
						Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
						glassroot.IsVisible = true;
						glassrootb.IsVisible = true;
						glass.IsVisible = false;
						Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
						if (prop5 != null)
						{
							prop5.Opacity = 0;
						}
						Prop prop6 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
						if (prop6 != null)
						{
							prop6.Opacity = 0;
						}
						Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab");
						while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab"))
						{
							Script.Wait(0);
						}
						RobberB_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
						RobberB_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberB_extra, "des_jewel_cab_root", "des_jewel_cab", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberB_extra2, "des_jewel_cab_root2", "des_jewel_cab", 1f, -1f, 0, 1148846080);
						RobberB_jewelsmashint4 = 30;
					}
					break;
				case 30:
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
					{
						RobberB_jewelsmashint4 = 10;
					}
					if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberB_JewelStoreSyncScene) > 0.99f)
					{
						RobberBRobbing = false;
						RobberB_JustFinishedSmashingCabinet = true;
						RobberB_ResetSmashTimer = Game.GameTime + 1000;
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberB_extra, 0f);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberB_extra2, 0f);
						glass.IsVisible = false;
						brokenjewelglass.IsVisible = true;
						glassroot.IsVisible = false;
						glassrootb.IsVisible = false;
						JewelCases_Smashed.Add(brokenjewelglass);
						JewelCases_Smashed.Add(glassroot);
						JewelCases_Smashed.Add(glassrootb);
						brokenjewelglass.MarkAsNoLongerNeeded();
						glassrootb.MarkAsNoLongerNeeded();
						glassroot.MarkAsNoLongerNeeded();
						jewelbrokenglasslist.Add(brokenjewelglass);
						jewelrootlist.Add(glassroot);
						jewelrootBlist.Add(glassrootb);
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
						glass.Delete();
						Ped.Task.ClearAll();
						RobberB_jewelsmashint4 = 60;
						Script.Wait(1);
						RobberBRobbing = false;
						Script.Wait(1);
						RobberBRobbing = false;
					}
					break;
				}
			}
			if (!(Ped == RobberC))
			{
				return;
			}
			switch (RobberC_jewelsmashint4)
			{
			case 0:
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				jewelsmashpos = glass.GetOffsetPosition(new Vector3(0f, -1f, -0.6f));
				jewelsmashrot = glass.Rotation.Z;
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Vector3 vector4 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Ped, argument1: vector4.X, argument2: vector4.Y, argument3: vector4.Z, argument4: 1f, argument5: 3000, argument7: 0f);
				unstucktimer = Game.GameTime + 1000;
				RobberC_jewelsmashint4 = 1;
				break;
			}
			case 1:
				RobberC_jewelsmashint4 = 10;
				break;
			case 10:
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Model model10 = new Model("des_jewel_cab_end");
				model10.Request(10000);
				if (model10.IsValid && model10.IsInCdImage)
				{
					while (!model10.IsLoaded)
					{
						Script.Wait(50);
					}
					brokenjewelglass = World.CreateProp(model10, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
					brokenjewelglass.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					brokenjewelglass.IsPositionFrozen = true;
					brokenjewelglass.IsVisible = false;
				}
				Model model11 = new Model("des_jewel_cab_root");
				model11.Request(10000);
				if (model11.IsValid && model11.IsInCdImage)
				{
					while (!model11.IsLoaded)
					{
						Script.Wait(50);
					}
					glassroot = World.CreateProp(model11, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassroot.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassroot.IsPositionFrozen = true;
					glassroot.IsVisible = false;
				}
				Model model12 = new Model("des_jewel_cab_root2");
				model12.Request(10000);
				if (model12.IsValid && model12.IsInCdImage)
				{
					while (!model12.IsLoaded)
					{
						Script.Wait(50);
					}
					glassrootb = World.CreateProp(model12, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassrootb.IsPositionFrozen = true;
					glassrootb.IsVisible = false;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				RobberC_JewelStoreSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				RobberC_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Ped, RobberC_JewelStoreSyncScene, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Ped);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
				Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				RobberC_jewelsmashint4 = 20;
				break;
			}
			case 20:
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
				{
					RobberC_jewelsmashint4 = 10;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberC_JewelStoreSyncScene) > 0.2f)
				{
					while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Script.Wait(0);
					}
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
					if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_jewelheist"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_jewelheist");
						smashparticlefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
					}
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Ped.Position.X, Ped.Position.Y, Ped.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glass.IsVisible = false;
					Prop prop7 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_02_glass"), 0, 0, 0);
					if (prop7 != null)
					{
						prop7.Opacity = 0;
					}
					Prop prop8 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, glass.Position.X, glass.Position.Y, glass.Position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "p_counter_04_glass"), 0, 0, 0);
					if (prop8 != null)
					{
						prop8.Opacity = 0;
					}
					Function.Call(Hash.REQUEST_ANIM_DICT, "des_jewel_cab");
					while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "des_jewel_cab"))
					{
						Script.Wait(0);
					}
					RobberC_extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glass.Position.X, glass.Position.Y, glass.Position.Z, 0f, 0f, glass.Rotation.Z, 2);
					RobberC_extra2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z, 0f, 0f, glassrootb.Rotation.Z, 2);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassroot, RobberC_extra, "des_jewel_cab_root", "des_jewel_cab", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, glassrootb, RobberC_extra2, "des_jewel_cab_root2", "des_jewel_cab", 1f, -1f, 0, 1148846080);
					RobberC_jewelsmashint4 = 30;
				}
				break;
			case 30:
				if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Ped, "missheist_jewel", "smash_case_tray_a", 3))
				{
					RobberC_jewelsmashint4 = 10;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobberC_JewelStoreSyncScene) > 0.99f)
				{
					RobberCRobbing = false;
					RobberC_JustFinishedSmashingCabinet = true;
					RobberC_ResetSmashTimer = Game.GameTime + 1000;
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberC_extra, 0f);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, RobberC_extra2, 0f);
					glass.IsVisible = false;
					brokenjewelglass.IsVisible = true;
					glassroot.IsVisible = false;
					glassrootb.IsVisible = false;
					JewelCases_Smashed.Add(brokenjewelglass);
					JewelCases_Smashed.Add(glassroot);
					JewelCases_Smashed.Add(glassrootb);
					brokenjewelglass.MarkAsNoLongerNeeded();
					glassrootb.MarkAsNoLongerNeeded();
					glassroot.MarkAsNoLongerNeeded();
					jewelbrokenglasslist.Add(brokenjewelglass);
					jewelrootlist.Add(glassroot);
					jewelrootBlist.Add(glassrootb);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					AddCash(Ped, (int)(Heist_Average_Pay / 35f), (int)(Heist_Average_Pay / 55f));
					glass.Delete();
					Ped.Task.ClearAll();
					RobberC_jewelsmashint4 = 60;
					Script.Wait(1);
					RobberCRobbing = false;
					Script.Wait(1);
					RobberCRobbing = false;
				}
				break;
			}
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

		public static void DrillScene2(Prop Cabinetprop)
		{
			switch (takethingint2)
			{
			case 680:
			{
				Cabinetprop.IsInvincible = true;
				Cabinetprop.IsInvincible = true;
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				SetBagOff();
				depositlist.Add(Cabinetprop);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@");
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@");
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@");
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@");
				Model model2 = new Model("ch_prop_vault_drill_01a");
				model2.Request(10000);
				if (model2.IsValid && model2.IsInCdImage)
				{
					while (!model2.IsLoaded)
					{
						Script.Wait(50);
					}
					drillmodelprop2 = World.CreateProp(model2, Game.Player.Character.Position, false, false);
					drillmodelprop2.IsCollisionEnabled = false;
					takethingint2 = 690;
				}
				Model model3 = new Model("hei_p_m_bag_var22_arm_s");
				model3.Request(10000);
				if (model3.IsValid && model3.IsInCdImage)
				{
					while (!model3.IsLoaded)
					{
						Script.Wait(50);
					}
					bagprop2 = World.CreateProp(model3, Game.Player.Character.Position, false, false);
					bagprop2.IsCollisionEnabled = false;
					takethingint2 = 690;
				}
				Model model4 = new Model("hei_p_m_bag_var22_arm_s");
				model4.Request(10000);
				if (model4.IsValid && model4.IsInCdImage)
				{
					while (!model4.IsLoaded)
					{
						Script.Wait(50);
					}
					sounddissapearProp = World.CreateProp(model3, new Vector3(0f, 0f, 0f), false, false);
					sounddissapearProp.IsCollisionEnabled = false;
					takethingint2 = 690;
				}
				break;
			}
			case 690:
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "enter_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint2 = 700;
				break;
			case 700:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					someint = 0;
					somebool = true;
					takethingint2 = 710;
				}
				break;
			case 710:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@");
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@");
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint2 = 720;
				}
				break;
			case 720:
			{
				Model model = new Model("ch_prop_ch_moneybag_01a");
				model.Request(10000);
				if (model.IsValid && model.IsInCdImage)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
					Moneymodelprop2 = World.CreateProp(model, Game.Player.Character.Position, false, false);
					Moneymodelprop2.IsCollisionEnabled = false;
				}
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint2 = 730;
				break;
			}
			case 730:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min3 = (int)(Heist_Average_Pay / 25f);
					int max3 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min3, max3);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint2 = 740;
				}
				break;
			case 740:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@");
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@");
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint2 = 750;
				}
				break;
			case 750:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint2 = 760;
				break;
			case 760:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min2 = (int)(Heist_Average_Pay / 25f);
					int max2 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min2, max2);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint2 = 770;
				}
				break;
			case 770:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@");
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@");
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint2 = 780;
				}
				break;
			case 780:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint2 = 790;
				break;
			case 790:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min4 = (int)(Heist_Average_Pay / 25f);
					int max4 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min4, max4);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint2 = 800;
				}
				break;
			case 800:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@");
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@");
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint2 = 810;
				}
				break;
			case 810:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "reward_ch_prop_vault_drill_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, extra, "reward_ch_prop_ch_sec_cabinet_01abc", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, extra, "reward_ch_prop_ch_moneybag_01a", "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint2 = 820;
				break;
			case 820:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min = (int)(Heist_Average_Pay / 25f);
					int max = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min, max);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					someint = 0;
					somebool = false;
					SetBagOn();
					bagprop2.Delete();
					drillmodelprop2.Delete();
					Moneymodelprop2.Delete();
					sounddissapearProp.Delete();
					Game.Player.Character.Task.ClearAll();
					Crate_Valuables[CurrentLootTarget].GotValuables = true;
					Cabinetprop.IsInvincible = false;
					Cabinetprop.Health = 0;
					takethingint2 = 680;
					PlayingAnim = false;
				}
				break;
			}
		}

		public static void DrillScene5(Prop Cabinetprop, string dict, string dict2, string dict3, string dict4)
		{
			switch (takethingint5)
			{
			case 680:
			{
				Cabinetprop.IsInvincible = true;
				Cabinetprop.IsInvincible = true;
				requestanimdict(dict);
				requestanimdict(dict2);
				requestanimdict(dict3);
				requestanimdict(dict4);
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, dict, "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, dict, "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				SetBagOff();
				depositlist.Add(Cabinetprop);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict2);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict3);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict4);
				Model model2 = new Model("ch_prop_vault_drill_01a");
				model2.Request(10000);
				if (model2.IsValid && model2.IsInCdImage)
				{
					while (!model2.IsLoaded)
					{
						Script.Wait(50);
					}
					drillmodelprop2 = World.CreateProp(model2, Game.Player.Character.Position, false, false);
					drillmodelprop2.IsCollisionEnabled = false;
					takethingint5 = 690;
				}
				Model model3 = new Model("hei_p_m_bag_var22_arm_s");
				model3.Request(10000);
				if (model3.IsValid && model3.IsInCdImage)
				{
					while (!model3.IsLoaded)
					{
						Script.Wait(50);
					}
					bagprop2 = World.CreateProp(model3, Game.Player.Character.Position, false, false);
					bagprop2.IsCollisionEnabled = false;
					takethingint5 = 690;
				}
				Model model4 = new Model("hei_p_m_bag_var22_arm_s");
				model4.Request(10000);
				if (model4.IsValid && model4.IsInCdImage)
				{
					while (!model4.IsLoaded)
					{
						Script.Wait(50);
					}
					sounddissapearProp = World.CreateProp(model3, new Vector3(0f, 0f, 0f), false, false);
					sounddissapearProp.IsCollisionEnabled = false;
					takethingint5 = 690;
				}
				break;
			}
			case 690:
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict, "enter", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "enter_p_m_bag_var22_arm_s", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint5 = 700;
				break;
			case 700:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					someint = 0;
					somebool = true;
					takethingint5 = 710;
				}
				break;
			case 710:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, dict);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", dict, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01def", dict, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, dict);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", dict, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01def", dict, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint5 = 720;
				}
				break;
			case 720:
			{
				Model model = new Model("ch_prop_ch_moneybag_01a");
				model.Request(10000);
				if (model.IsValid && model.IsInCdImage)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
					Moneymodelprop2 = World.CreateProp(model, Game.Player.Character.Position, false, false);
					Moneymodelprop2.IsCollisionEnabled = false;
				}
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist@hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01def", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint5 = 730;
				break;
			}
			case 730:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min3 = (int)(Heist_Average_Pay / 25f);
					int max3 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min3, max3);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint5 = 740;
				}
				break;
			case 740:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, dict2);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict2, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", dict2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01def", dict2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, dict2);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict2, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", dict2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01def", dict2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint5 = 750;
				}
				break;
			case 750:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict2);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict2, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01def", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint5 = 760;
				break;
			case 760:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min2 = (int)(Heist_Average_Pay / 25f);
					int max2 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min2, max2);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint5 = 770;
				}
				break;
			case 770:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, dict3);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict3, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", dict3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01def", dict3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, dict3);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict3, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", dict3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01def", dict3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint5 = 780;
				}
				break;
			case 780:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict3);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict3, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01def", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint5 = 790;
				break;
			case 790:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min4 = (int)(Heist_Average_Pay / 25f);
					int max4 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min4, max4);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint5 = 800;
				}
				break;
			case 800:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, dict4);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict4, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", dict4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01def", dict4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, dict4);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict4, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", dict4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01def", dict4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint5 = 810;
				}
				break;
			case 810:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict4);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict4, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", dict4, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "reward_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, extra, "reward_ch_prop_ch_sec_cabinet_01def", dict4, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, extra, "reward_ch_prop_ch_moneybag_01a", dict4, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint5 = 820;
				break;
			case 820:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min = (int)(Heist_Average_Pay / 25f);
					int max = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min, max);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					someint = 0;
					somebool = false;
					SetBagOn();
					bagprop2.Delete();
					drillmodelprop2.Delete();
					Moneymodelprop2.Delete();
					sounddissapearProp.Delete();
					Game.Player.Character.Task.ClearAll();
					Cabinetprop.IsInvincible = false;
					Cabinetprop.Health = 0;
					takethingint5 = 680;
					PlayingAnim = false;
				}
				break;
			}
		}

		public static void DrillScene6(Prop Cabinetprop, string dict, string dict2, string dict3, string dict4)
		{
			switch (takethingint6)
			{
			case 680:
			{
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				Cabinetprop.IsInvincible = true;
				Cabinetprop.IsInvincible = true;
				requestanimdict(dict);
				requestanimdict(dict2);
				requestanimdict(dict3);
				requestanimdict(dict4);
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, dict, "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, dict, "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				SetBagOff();
				depositlist.Add(Cabinetprop);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict2);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict3);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict4);
				Model model2 = new Model("ch_prop_vault_drill_01a");
				model2.Request(10000);
				if (model2.IsValid && model2.IsInCdImage)
				{
					while (!model2.IsLoaded)
					{
						Script.Wait(50);
					}
					drillmodelprop2 = World.CreateProp(model2, Game.Player.Character.Position, false, false);
					drillmodelprop2.IsCollisionEnabled = false;
					takethingint6 = 690;
				}
				Model model3 = new Model("hei_p_m_bag_var22_arm_s");
				model3.Request(10000);
				if (model3.IsValid && model3.IsInCdImage)
				{
					while (!model3.IsLoaded)
					{
						Script.Wait(50);
					}
					bagprop2 = World.CreateProp(model3, Game.Player.Character.Position, false, false);
					bagprop2.IsCollisionEnabled = false;
					takethingint6 = 690;
				}
				Model model4 = new Model("hei_p_m_bag_var22_arm_s");
				model4.Request(10000);
				if (model4.IsValid && model4.IsInCdImage)
				{
					while (!model4.IsLoaded)
					{
						Script.Wait(50);
					}
					sounddissapearProp = World.CreateProp(model3, new Vector3(0f, 0f, 0f), false, false);
					sounddissapearProp.IsCollisionEnabled = false;
					takethingint6 = 690;
				}
				break;
			}
			case 690:
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict, "enter", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "enter_p_m_bag_var22_arm_s", dict, 1f, -1f, 0, 1148846080);
				Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint6 = 700;
				break;
			case 700:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					someint = 0;
					somebool = true;
					takethingint6 = 710;
				}
				break;
			case 710:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, dict);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", dict, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01ghi", dict, 1f, -1f, 0, 1148846080);
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, dict);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", dict, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01ghi", dict, 1f, -1f, 0, 1148846080);
						Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint6 = 720;
				}
				break;
			case 720:
			{
				Model model = new Model("ch_prop_ch_moneybag_01a");
				model.Request(10000);
				if (model.IsValid && model.IsInCdImage)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
					Moneymodelprop2 = World.CreateProp(model, Game.Player.Character.Position, false, false);
					Moneymodelprop2.IsCollisionEnabled = false;
				}
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim_heist @hs3f@ig10_lockbox_drill@pattern_01@lockbox_01@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01ghi", dict, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", dict, 1f, -1f, 0, 1148846080);
				Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint6 = 730;
				break;
			}
			case 730:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min3 = (int)(Heist_Average_Pay / 25f);
					int max3 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min3, max3);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint6 = 740;
				}
				break;
			case 740:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, dict2);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict2, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", dict2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01ghi", dict2, 1f, -1f, 0, 1148846080);
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, dict2);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict2, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", dict2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01ghi", dict2, 1f, -1f, 0, 1148846080);
						Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint6 = 750;
				}
				break;
			case 750:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict2);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict2, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01ghi", dict2, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", dict2, 1f, -1f, 0, 1148846080);
				Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint6 = 760;
				break;
			case 760:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min2 = (int)(Heist_Average_Pay / 25f);
					int max2 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min2, max2);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint6 = 770;
				}
				break;
			case 770:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, dict3);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict3, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", dict3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01ghi", dict3, 1f, -1f, 0, 1148846080);
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, dict3);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict3, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", dict3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01ghi", dict3, 1f, -1f, 0, 1148846080);
						Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint6 = 780;
				}
				break;
			case 780:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, dict3);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict3, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01ghi", dict3, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", dict3, 1f, -1f, 0, 1148846080);
				Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint6 = 790;
				break;
			case 790:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min4 = (int)(Heist_Average_Pay / 25f);
					int max4 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min4, max4);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint6 = 800;
				}
				break;
			case 800:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, dict4);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict4, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", dict4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01ghi", dict4, 1f, -1f, 0, 1148846080);
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, dict4);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, dict4, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", dict4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01ghi", dict4, 1f, -1f, 0, 1148846080);
						Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint6 = 810;
				}
				break;
			case 810:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, extra, dict4, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, extra, "reward_p_m_bag_var22_arm_s", dict4, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "reward_ch_prop_vault_drill_01a", dict4, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, extra, "reward_ch_prop_ch_sec_cabinet_01ghi", dict4, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, extra, "reward_ch_prop_ch_moneybag_01a", dict4, 1f, -1f, 0, 1148846080);
				Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				timer1 = Game.GameTime + 4000;
				takethingint6 = 811;
				break;
			case 811:
				if (Game.GameTime > timer1)
				{
					int min = (int)(Heist_Average_Pay / 25f);
					int max = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min, max);
					takethingint6 = 820;
				}
				break;
			case 820:
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
				someint = 0;
				somebool = false;
				SetBagOff();
				bagprop2.Delete();
				drillmodelprop2.Delete();
				Moneymodelprop2.Delete();
				sounddissapearProp.Delete();
				Game.Player.Character.Task.ClearAll();
				takethingint6 = 680;
				PlayingAnim = false;
				Cabinetprop.IsInvincible = false;
				Cabinetprop.Health = 0;
				break;
			}
		}

		public static void DrillScene2_XM3(Prop Cabinetprop)
		{
			string dict = "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@";
			string dict2 = "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@";
			string dict3 = "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@";
			string dict4 = "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@";
			LoadDict(dict);
			LoadDict(dict2);
			LoadDict(dict3);
			LoadDict(dict4);
			switch (takethingint2)
			{
			case 680:
			{
				PlayingAnim = true;
				Cabinetprop.IsInvincible = true;
				Cabinetprop.IsInvincible = true;
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				SetBagOff();
				depositlist.Add(Cabinetprop);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@");
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@");
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@");
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@");
				Model model = new Model("ch_prop_vault_drill_01a");
				model.Request(10000);
				if (model.IsValid && model.IsInCdImage)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
					drillmodelprop2 = World.CreateProp(model, Game.Player.Character.Position, false, false);
					drillmodelprop2.IsCollisionEnabled = false;
					takethingint2 = 690;
				}
				Model model2 = new Model("hei_p_m_bag_var22_arm_s");
				model2.Request(10000);
				if (model2.IsValid && model2.IsInCdImage)
				{
					while (!model2.IsLoaded)
					{
						Script.Wait(50);
					}
					bagprop2 = World.CreateProp(model2, Game.Player.Character.Position, false, false);
					bagprop2.IsCollisionEnabled = false;
					takethingint2 = 690;
				}
				Model model3 = new Model("hei_p_m_bag_var22_arm_s");
				model3.Request(10000);
				if (model3.IsValid && model3.IsInCdImage)
				{
					while (!model3.IsLoaded)
					{
						Script.Wait(50);
					}
					sounddissapearProp = World.CreateProp(model2, new Vector3(0f, 0f, 0f), false, false);
					sounddissapearProp.IsCollisionEnabled = false;
					takethingint2 = 690;
				}
				break;
			}
			case 690:
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", "enter", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "enter_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint2 = 700;
				break;
			case 700:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					someint = 0;
					somebool = true;
					takethingint2 = 710;
				}
				break;
			case 710:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@");
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01abc", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@");
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01abc", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint2 = 720;
				}
				break;
			case 720:
				if (Moneymodelprop2 != null)
				{
					Moneymodelprop2.Delete();
				}
				Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
				if (Moneymodelprop2 != null)
				{
					Moneymodelprop2.Opacity = 0;
				}
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint2 = 725;
				break;
			case 725:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text3 = "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@";
				LoadDict(text3);
				int num3 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num3 >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text3, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01abc", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num3 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text3, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01abc", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text3, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01abc", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint2 = 730;
				break;
			}
			case 730:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint2 = 725;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min4 = (int)(Heist_Average_Pay / 25f);
					int max4 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min4, max4);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint2 = 740;
				}
				break;
			case 740:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@");
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01abc", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@");
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01abc", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint2 = 750;
				}
				break;
			case 750:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint2 = 755;
				break;
			case 755:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text2 = "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_02@male@";
				LoadDict(text2);
				int num2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num2 >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text2, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01abc", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num2 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text2, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01abc", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text2, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01abc", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint2 = 760;
				break;
			}
			case 760:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint2 = 755;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min3 = (int)(Heist_Average_Pay / 25f);
					int max3 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min3, max3);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint2 = 770;
				}
				break;
			case 770:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@");
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@", "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01abc", "anim@scripted@cbr5@ig3_drill_box@attern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@");
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01abc", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint2 = 780;
				}
				break;
			case 780:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint2 = 785;
				break;
			case 785:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text4 = "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_03@male@";
				LoadDict(text4);
				int num4 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num4 >= 60)
				{
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text4, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01abc", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num4 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text4, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01abc", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text4, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01abc", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint2 = 790;
				break;
			}
			case 790:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint2 = 785;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min2 = (int)(Heist_Average_Pay / 25f);
					int max2 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min2, max2);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint2 = 800;
				}
				break;
			case 800:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@");
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01abc", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@");
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01abc", "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@", 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint2 = 810;
				}
				break;
			case 810:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint2 = 815;
				break;
			case 815:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text = "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_04@male@";
				LoadDict(text);
				int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01abc", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01abc", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01abc", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint2 = 820;
				break;
			}
			case 820:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint2 = 815;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min = (int)(Heist_Average_Pay / 25f);
					int max = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min, max);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					someint = 0;
					somebool = false;
					SetBagOn();
					bagprop2.Delete();
					drillmodelprop2.Delete();
					Moneymodelprop2.Delete();
					sounddissapearProp.Delete();
					Game.Player.Character.Task.ClearAll();
					Cabinetprop.IsInvincible = false;
					Cabinetprop.Health = 0;
					takethingint2 = 680;
					PlayingAnim = false;
				}
				break;
			}
		}

		public static void DrillScene5_XM3(Prop Cabinetprop)
		{
			string text = "anim@scripted@cbr5@ig3_drill_box@pattern_02@lockbox_01@male@";
			string text2 = "anim@scripted@cbr5@ig3_drill_box@pattern_02@lockbox_02@male@";
			string text3 = "anim@scripted@cbr5@ig3_drill_box@pattern_02@lockbox_03@male@";
			string text4 = "anim@scripted@cbr5@ig3_drill_box@pattern_02@lockbox_04@male@";
			LoadDict(text);
			LoadDict(text2);
			LoadDict(text3);
			LoadDict(text4);
			switch (takethingint5)
			{
			case 680:
			{
				PlayingAnim = true;
				Cabinetprop.IsInvincible = true;
				Cabinetprop.IsInvincible = true;
				requestanimdict(text);
				requestanimdict(text2);
				requestanimdict(text3);
				requestanimdict(text4);
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, text, "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, text, "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				SetBagOff();
				depositlist.Add(Cabinetprop);
				Function.Call(Hash.REQUEST_ANIM_DICT, text);
				Function.Call(Hash.REQUEST_ANIM_DICT, text2);
				Function.Call(Hash.REQUEST_ANIM_DICT, text3);
				Function.Call(Hash.REQUEST_ANIM_DICT, text4);
				Model model = new Model("ch_prop_vault_drill_01a");
				model.Request(10000);
				if (model.IsValid && model.IsInCdImage)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
					drillmodelprop2 = World.CreateProp(model, Game.Player.Character.Position, false, false);
					drillmodelprop2.IsCollisionEnabled = false;
					takethingint5 = 690;
				}
				Model model2 = new Model("hei_p_m_bag_var22_arm_s");
				model2.Request(10000);
				if (model2.IsValid && model2.IsInCdImage)
				{
					while (!model2.IsLoaded)
					{
						Script.Wait(50);
					}
					bagprop2 = World.CreateProp(model2, Game.Player.Character.Position, false, false);
					bagprop2.IsCollisionEnabled = false;
					takethingint5 = 690;
				}
				Model model3 = new Model("hei_p_m_bag_var22_arm_s");
				model3.Request(10000);
				if (model3.IsValid && model3.IsInCdImage)
				{
					while (!model3.IsLoaded)
					{
						Script.Wait(50);
					}
					sounddissapearProp = World.CreateProp(model2, new Vector3(0f, 0f, 0f), false, false);
					sounddissapearProp.IsCollisionEnabled = false;
					takethingint5 = 690;
				}
				break;
			}
			case 690:
				Function.Call(Hash.REQUEST_ANIM_DICT, text);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "enter", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "enter_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint5 = 700;
				break;
			case 700:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					someint = 0;
					somebool = true;
					takethingint5 = 710;
				}
				break;
			case 710:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, text);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01def", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, text);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01def", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint5 = 720;
				}
				break;
			case 720:
				if (Moneymodelprop2 != null)
				{
					Moneymodelprop2.Delete();
				}
				Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
				if (Moneymodelprop2 != null)
				{
					Moneymodelprop2.Opacity = 0;
				}
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@cbr5@ig3_drill_box@pattern_01@lockbox_01@male@");
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint5 = 725;
				break;
			case 725:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text6 = text;
				LoadDict(text6);
				int num2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num2 >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text6, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01def", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num2 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text6, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01def", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text6, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01def", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint5 = 730;
				break;
			}
			case 730:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint5 = 725;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min2 = (int)(Heist_Average_Pay / 25f);
					int max2 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min2, max2);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint5 = 740;
				}
				break;
			case 740:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, text2);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text2, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01def", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, text2);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text2, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01def", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint5 = 750;
				}
				break;
			case 750:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, text2);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint5 = 755;
				break;
			case 755:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text7 = text2;
				LoadDict(text7);
				int num3 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num3 >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text7, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01def", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num3 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text7, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01def", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text7, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01def", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint5 = 760;
				break;
			}
			case 760:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint5 = 755;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min4 = (int)(Heist_Average_Pay / 25f);
					int max4 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min4, max4);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint5 = 770;
				}
				break;
			case 770:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, text3);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text3, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01def", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, text3);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text3, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01def", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint5 = 780;
				}
				break;
			case 780:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, text3);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint5 = 785;
				break;
			case 785:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text8 = text3;
				LoadDict(text8);
				int num4 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num4 >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text8, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01def", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num4 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text8, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01def", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text8, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01def", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint5 = 790;
				break;
			}
			case 790:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint5 = 785;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min3 = (int)(Heist_Average_Pay / 25f);
					int max3 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min3, max3);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint5 = 800;
				}
				break;
			case 800:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, text4);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text4, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01def", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, text4);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text4, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01def", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint5 = 810;
				}
				break;
			case 810:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, text4);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint5 = 815;
				break;
			case 815:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text5 = text4;
				LoadDict(text5);
				int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text5, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01def", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text5, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01def", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text5, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01def", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint5 = 820;
				break;
			}
			case 820:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint5 = 815;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min = (int)(Heist_Average_Pay / 25f);
					int max = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min, max);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					someint = 0;
					somebool = false;
					SetBagOn();
					bagprop2.Delete();
					drillmodelprop2.Delete();
					Moneymodelprop2.Delete();
					sounddissapearProp.Delete();
					Game.Player.Character.Task.ClearAll();
					Cabinetprop.IsInvincible = false;
					Cabinetprop.Health = 0;
					takethingint5 = 680;
					PlayingAnim = false;
				}
				break;
			}
		}

		public static void DrillScene6_XM3(Prop Cabinetprop)
		{
			string text = "anim@scripted@cbr5@ig3_drill_box@pattern_03@lockbox_01@male@";
			string text2 = "anim@scripted@cbr5@ig3_drill_box@pattern_03@lockbox_02@male@";
			string text3 = "anim@scripted@cbr5@ig3_drill_box@pattern_03@lockbox_03@male@";
			string text4 = "anim@scripted@cbr5@ig3_drill_box@pattern_03@lockbox_04@male@";
			LoadDict(text);
			LoadDict(text2);
			LoadDict(text3);
			LoadDict(text4);
			switch (takethingint6)
			{
			case 680:
			{
				PlayingAnim = true;
				Cabinetprop.IsInvincible = true;
				Cabinetprop.IsInvincible = true;
				requestanimdict(text);
				requestanimdict(text2);
				requestanimdict(text3);
				requestanimdict(text4);
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, text, "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, text, "enter", Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 0f, 2);
				SetBagOff();
				depositlist.Add(Cabinetprop);
				Function.Call(Hash.REQUEST_ANIM_DICT, text);
				Function.Call(Hash.REQUEST_ANIM_DICT, text2);
				Function.Call(Hash.REQUEST_ANIM_DICT, text3);
				Function.Call(Hash.REQUEST_ANIM_DICT, text4);
				Model model = new Model("ch_prop_vault_drill_01a");
				model.Request(10000);
				if (model.IsValid && model.IsInCdImage)
				{
					while (!model.IsLoaded)
					{
						Script.Wait(50);
					}
					drillmodelprop2 = World.CreateProp(model, Game.Player.Character.Position, false, false);
					drillmodelprop2.IsCollisionEnabled = false;
					takethingint6 = 690;
				}
				Model model2 = new Model("hei_p_m_bag_var22_arm_s");
				model2.Request(10000);
				if (model2.IsValid && model2.IsInCdImage)
				{
					while (!model2.IsLoaded)
					{
						Script.Wait(50);
					}
					bagprop2 = World.CreateProp(model2, Game.Player.Character.Position, false, false);
					bagprop2.IsCollisionEnabled = false;
					takethingint6 = 690;
				}
				Model model3 = new Model("hei_p_m_bag_var22_arm_s");
				model3.Request(10000);
				if (model3.IsValid && model3.IsInCdImage)
				{
					while (!model3.IsLoaded)
					{
						Script.Wait(50);
					}
					sounddissapearProp = World.CreateProp(model2, new Vector3(0f, 0f, 0f), false, false);
					sounddissapearProp.IsCollisionEnabled = false;
					takethingint6 = 690;
				}
				break;
			}
			case 690:
				Function.Call(Hash.REQUEST_ANIM_DICT, text);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "enter", 4f, -15f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "enter_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
				Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
				takethingint6 = 700;
				break;
			case 700:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					someint = 0;
					somebool = true;
					takethingint6 = 710;
				}
				break;
			case 710:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, text);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01ghi", text, 1f, -1f, 0, 1148846080);
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, text);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01ghi", text, 1f, -1f, 0, 1148846080);
						Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint6 = 720;
				}
				break;
			case 720:
				if (Moneymodelprop2 != null)
				{
					Moneymodelprop2.Delete();
				}
				Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
				if (Moneymodelprop2 != null)
				{
					Moneymodelprop2.Opacity = 0;
				}
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text, 1f, -1f, 0, 1148846080);
				takethingint6 = 725;
				break;
			case 725:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text6 = text;
				LoadDict(text6);
				int num2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num2 >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text6, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01ghi", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text6, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num2 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text6, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01ghi", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text6, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01ghi", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text6, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint6 = 730;
				break;
			}
			case 730:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint6 = 725;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min2 = (int)(Heist_Average_Pay / 25f);
					int max2 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min2, max2);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint6 = 740;
				}
				break;
			case 740:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, text2);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text2, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01ghi", text2, 1f, -1f, 0, 1148846080);
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, text2);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text2, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", text2, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01ghi", text2, 1f, -1f, 0, 1148846080);
						Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint6 = 750;
				}
				break;
			case 750:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, text2);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint6 = 755;
				break;
			case 755:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text7 = text2;
				LoadDict(text7);
				int num3 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num3 >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text7, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01ghi", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text7, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num3 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text7, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01ghi", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text7, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01ghi", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text7, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint6 = 760;
				break;
			}
			case 760:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint6 = 755;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min4 = (int)(Heist_Average_Pay / 25f);
					int max4 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min4, max4);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint6 = 770;
				}
				break;
			case 770:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, text3);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text3, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01ghi", text3, 1f, -1f, 0, 1148846080);
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, text3);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text3, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", text3, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01ghi", text3, 1f, -1f, 0, 1148846080);
						Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint6 = 780;
				}
				break;
			case 780:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				Function.Call(Hash.REQUEST_ANIM_DICT, text3);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint6 = 785;
				break;
			case 785:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text8 = text3;
				LoadDict(text8);
				int num4 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num4 >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text8, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01ghi", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text8, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num4 < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text8, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01ghi", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text8, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01ghi", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text8, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				takethingint6 = 790;
				break;
			}
			case 790:
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) == 0)
				{
					takethingint6 = 785;
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216)
				{
					int min3 = (int)(Heist_Average_Pay / 25f);
					int max3 = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min3, max3);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					somebool = true;
					someint = 0;
					takethingint6 = 800;
				}
				break;
			case 800:
				if (!Game.IsControlPressed(Control.Attack) && !somebool)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
					myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
					Function.Call(Hash.REQUEST_ANIM_DICT, text4);
					DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text4, "idle", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "idle_p_m_bag_var22_arm_s", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "idle_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "idle_ch_prop_ch_sec_cabinet_01ghi", text4, 1f, -1f, 0, 1148846080);
					Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					somebool = true;
				}
				World.DrawMarker(MarkerType.Cylinder, new Vector3(Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				DisplayHelpTextThisFrame("Press ~INPUT_ATTACK~ to drill ");
				if (Game.IsControlPressed(Control.Attack))
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, DrillSyncScene) >= 1065353216 && Game.GameTime >= TimeBetweenIsDrillingCheck)
					{
						TimeBetweenIsDrillingCheck = Game.GameTime + 500;
						somebool = true;
					}
					someint++;
					if (somebool)
					{
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
						myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", drillmodelprop2, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
						Function.Call(Hash.REQUEST_ANIM_DICT, text4);
						DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text4, "action", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "action_p_m_bag_var22_arm_s", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "action_ch_prop_vault_drill_01a", text4, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "action_ch_prop_ch_sec_cabinet_01ghi", text4, 1f, -1f, 0, 1148846080);
						Function.Call<int>(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, DrillSpeed);
						somebool = false;
					}
				}
				if (someint == TimeToDrillOpenDepositBox)
				{
					takethingint6 = 810;
				}
				break;
			case 810:
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_MPHEIST/HEIST_FLEECA_DRILL", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "casino_heist_finale_general_01", 0, -1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 1);
				myint = Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, 0, "Drill", sounddissapearProp, "DLC_HEIST_FLEECA_SOUNDSET", 1, 0);
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				takethingint6 = 811;
				break;
			case 811:
			{
				DrillSyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinetprop.Position.X, Cabinetprop.Position.Y, Cabinetprop.Position.Z, Cabinetprop.Rotation.X, Cabinetprop.Rotation.Y, Cabinetprop.Rotation.Z, 2);
				string text5 = text4;
				LoadDict(text5);
				int num = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 100);
				if (num >= 60)
				{
					if (Moneymodelprop2 != null)
					{
						Moneymodelprop2.Opacity = 0;
					}
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text5, "no_reward", 4f, -15f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "no_reward_p_m_bag_var22_arm_s", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "no_reward_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "no_reward_ch_prop_ch_sec_cabinet_01ghi", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "no_reward_ch_prop_ch_moneybag_01a", text5, 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					Get_Keycard_From_SDB_MP2023_2_Chance += Get_Keycard_From_SDB_MP2023_2_Chance_Increase;
				}
				if (num < 60)
				{
					if (!CanGetKeycard())
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("ch_prop_ch_moneybag_01a"), Game.Player.Character.Position, false, false);
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Opacity = 255;
						}
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text5, "reward", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_p_m_bag_var22_arm_s", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_ch_prop_ch_sec_cabinet_01ghi", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_ch_prop_ch_moneybag_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
					else
					{
						if (Moneymodelprop2 != null)
						{
							Moneymodelprop2.Delete();
						}
						Moneymodelprop2 = World.CreateProp(RequestModel("m23_2_prop_m32_cbkeycard_01a"), Game.Player.Character.Position, false, false);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, extra, "enter_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, DrillSyncScene, text5, "reward_keycard", 4f, -15f, 3341, 16, 1148846080, 0);
						Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagprop2, DrillSyncScene, "reward_keycard_p_m_bag_var22_arm_s", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, drillmodelprop2, DrillSyncScene, "reward_keycard_ch_prop_vault_drill_01a", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Cabinetprop, DrillSyncScene, "reward_keycard_ch_prop_ch_sec_cabinet_01ghi", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Moneymodelprop2, DrillSyncScene, "reward_keycard_prop_m32_cbkeycard_01", text5, 1f, -1f, 0, 1148846080);
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, DrillSyncScene, 1f);
					}
				}
				timer1 = Game.GameTime + 4000;
				takethingint6 = 812;
				break;
			}
			case 812:
				if (Game.GameTime > timer1)
				{
					int min = (int)(Heist_Average_Pay / 25f);
					int max = (int)(Heist_Average_Pay / 10f);
					increaseCurrentPay(min, max);
					takethingint6 = 820;
				}
				break;
			case 820:
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
				someint = 0;
				somebool = false;
				SetBagOff();
				bagprop2.Delete();
				drillmodelprop2.Delete();
				Moneymodelprop2.Delete();
				sounddissapearProp.Delete();
				Game.Player.Character.Task.ClearAll();
				takethingint6 = 680;
				PlayingAnim = false;
				Cabinetprop.IsInvincible = false;
				Cabinetprop.Health = 0;
				break;
			}
		}

		public void VaultLooting()
		{
			if (PlayingAnim && PlayerLastSafeDepositBox != null)
			{
				if (CurrentType == LootType.SafetyDepositBoxes)
				{
					if (PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01b") || PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01a") || PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01c"))
					{
						depobox1 = PlayerLastSafeDepositBox;
						depobox1.IsInvincible = true;
						if (depobox1.Health > 0)
						{
							DrillScene2(depobox1);
						}
					}
					if (PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01d") || PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01e") || PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01f"))
					{
						depobox2 = PlayerLastSafeDepositBox;
						depobox2.IsInvincible = true;
						if (depobox2.Health > 0)
						{
							DrillScene5(depobox2, "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_01@male@", "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_02@male@", "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_03@male@", "anim_heist@hs3f@ig10_lockbox_drill@pattern_02@lockbox_04@male@");
						}
					}
					if (PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01g") || PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01h") || PlayerLastSafeDepositBox.Model == new Model("ch_prop_ch_sec_cabinet_01i"))
					{
						depobox5 = PlayerLastSafeDepositBox;
						depobox5.IsInvincible = true;
						if (depobox5.Health > 0)
						{
							DrillScene6(depobox5, "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_01@male@", "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_02@male@", "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_03@male@", "anim_heist@hs3f@ig10_lockbox_drill@pattern_03@lockbox_04@male@");
						}
					}
				}
				if (CurrentType == LootType.SafetyDepositBoxes_XM3)
				{
					if (PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01b") || PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01a") || PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01c"))
					{
						depobox1 = PlayerLastSafeDepositBox;
						depobox1.IsInvincible = true;
						if (depobox1.Health > 0)
						{
							DrillScene2_XM3(depobox1);
						}
					}
					if (PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01d") || PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01e") || PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01f"))
					{
						depobox2 = PlayerLastSafeDepositBox;
						depobox2.IsInvincible = true;
						if (depobox2.Health > 0)
						{
							DrillScene5_XM3(depobox2);
						}
					}
					if (PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01g") || PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01h") || PlayerLastSafeDepositBox.Model == new Model("m23_2_prop_m32_sec_cabinet_01i"))
					{
						depobox5 = PlayerLastSafeDepositBox;
						depobox5.IsInvincible = true;
						if (depobox5.Health > 0)
						{
							DrillScene6_XM3(depobox5);
						}
					}
				}
			}
			bool flag = true;
			int num = Function.Call<int>(Hash.GET_PLAYER_GROUP, Game.Player);
			if (LootLeft > 0f)
			{
				if (!RobberARobbing && !Game.Player.Character.IsInCombat && RobberA != null && Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, RobberA, num) && !RobberA.IsInCombat && RobberA != null)
				{
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberA);
					RobberA.BlockPermanentEvents = true;
					RobberA.RelationshipGroup = 2;
				}
				if (!RobberBRobbing && !Game.Player.Character.IsInCombat && RobberB != null && Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, RobberB, num) && !RobberB.IsInCombat && RobberB != null)
				{
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberB);
					RobberB.BlockPermanentEvents = true;
					RobberB.RelationshipGroup = 2;
				}
				if (!RobberCRobbing && !Game.Player.Character.IsInCombat && RobberC != null && Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, RobberC, num) && !RobberC.IsInCombat && RobberC != null)
				{
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberC);
					RobberC.BlockPermanentEvents = true;
					RobberC.RelationshipGroup = 2;
				}
			}
			else
			{
				if (!RobberARobbing && RobberA != null && RobberA.Exists() && !Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, RobberA, num) && RobberA != null)
				{
					RobberA.Task.ClearAll();
					Ped robberA = RobberA;
					PedGroup pedGroup = Game.Player.Character.PedGroup;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberA, pedGroup);
					Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberA, 100);
					RobberA.Weapons.Give(WeaponHash.PistolMk2, 9999, true, true);
				}
				if (!RobberBRobbing && RobberB != null && RobberB.Exists() && !Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, RobberB, num) && RobberB != null)
				{
					RobberB.Task.ClearAll();
					Ped robberB = RobberB;
					PedGroup pedGroup2 = Game.Player.Character.PedGroup;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberB, pedGroup2);
					Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberB, 100);
					RobberB.Weapons.Give(WeaponHash.PistolMk2, 9999, true, true);
				}
				if (!RobberCRobbing && RobberC != null && RobberC.Exists() && !Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, RobberC, num) && RobberC != null)
				{
					RobberC.Task.ClearAll();
					Ped robberC = RobberC;
					PedGroup pedGroup3 = Game.Player.Character.PedGroup;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberC, pedGroup3);
					Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberC, 100);
					RobberC.Weapons.Give(WeaponHash.PistolMk2, 9999, true, true);
				}
			}
			if (CreatedCrates)
			{
				bool flag2 = false;
				if (Game.GameTime >= GetNearLootCheck)
				{
					GetNearLootCheck = Game.GameTime + 5000;
					if (RobberA != null && RobberA.Exists())
					{
						for (int i = 0; i < Crate_Valuables.Count; i++)
						{
							if (!Crate_Valuables[i].IsRobbed && GetDistance_VDIST2(RobberA.Position, Crate_Valuables[i].Position, "<", 50f))
							{
								flag2 = true;
								break;
							}
						}
					}
					RobberANearLoot = flag2;
					if (RobberB != null && RobberB.Exists())
					{
						for (int j = 0; j < Crate_Valuables.Count; j++)
						{
							if (!Crate_Valuables[j].IsRobbed && GetDistance_VDIST2(RobberB.Position, Crate_Valuables[j].Position, "<", 50f))
							{
								flag2 = true;
								break;
							}
						}
					}
					RobberBNearLoot = flag2;
					if (RobberC != null && RobberC.Exists())
					{
						for (int k = 0; k < Crate_Valuables.Count; k++)
						{
							if (!Crate_Valuables[k].IsRobbed && GetDistance_VDIST2(RobberC.Position, Crate_Valuables[k].Position, "<", 50f))
							{
								flag2 = true;
								break;
							}
						}
					}
					RobberCNearLoot = flag2;
				}
				if (ShowAiLootingDebug)
				{
					drawText("Data " + RobberANearLoot + "," + RobberACurrentValualabe + "//" + RobberBNearLoot + "," + RobberBCurrentValualabe + "//" + RobberCNearLoot + "," + RobberCCurrentValualabe, 0.1f, 0.1f, 0.3f, 255, 255, 255);
					drawText("Data " + RobberARobbing + "," + RobberAMoveTimer + "//" + RobberBRobbing + "," + RobberBMoveTimer + "//" + RobberCRobbing + "," + RobberCMoveTimer, 0.1f, 0.15f, 0.3f, 255, 255, 255);
					float num2 = 0.2f;
					drawText("~o~A~w~ | " + RobberA_JewelStoreSyncScene + "//" + RobberA_jewelsmashint + "," + RobberA_jewelsmashint2 + "," + RobberA_jewelsmashint3 + "," + RobberA_jewelsmashint4 + "/ Painting Stage " + RobberACuttingStage + " | Marker " + RobberA_RobberMarker.ToString() + " | Going to Loot " + RobberAMovingToLoot, 0.1f, num2, 0.3f, 255, 255, 255);
					drawText("~o~A~w~ | Robing " + RobberARobbing + " | NearL " + RobberANearLoot + " | Last Target ID " + RobberA_LastTargetId + " | SetRob " + RobberASetRob + " | Init " + RobberAInitialAnimDone, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
					num2 += 0.03f;
					drawText("~o~A~w~ | Target Type " + RobberACurrentType.ToString() + " | Lockbox " + RobberACurrentLockboxStage + " | LastAnim " + RobberAWaitingForLastAnim, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
					num2 += 0.06f;
					drawText("~y~B~w~ |" + RobberB_JewelStoreSyncScene + "//" + RobberB_jewelsmashint + "," + RobberB_jewelsmashint2 + "," + RobberB_jewelsmashint3 + "," + RobberB_jewelsmashint4 + "/ Painting Stage " + RobberBCuttingStage + " | Marker " + RobberB_RobberMarker.ToString() + " | Going to Loot " + RobberBMovingToLoot, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
					num2 += 0.03f;
					drawText("~y~B~w~ | Robing " + RobberBRobbing + " | NearL " + RobberBNearLoot + " | Last Target ID " + RobberB_LastTargetId + " | SetRob " + RobberBSetRob + " | Init " + RobberBInitialAnimDone, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
					num2 += 0.03f;
					drawText("~y~B~w~ | Target Type " + RobberBCurrentType.ToString() + " | Lockbox " + RobberBCurrentLockboxStage + " | LastAnim " + RobberBWaitingForLastAnim, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
					num2 += 0.06f;
					drawText("~g~C~w~ | " + RobberC_JewelStoreSyncScene + "//" + RobberC_jewelsmashint + "," + RobberC_jewelsmashint2 + "," + RobberC_jewelsmashint3 + "," + RobberC_jewelsmashint4 + "/ Painting Stage " + RobberCCuttingStage + " | Marker " + RobberC_RobberMarker.ToString() + " | Going to Loot " + RobberCMovingToLoot, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
					num2 += 0.03f;
					drawText("~g~C~w~ | Robing " + RobberCRobbing + " | NearL " + RobberCNearLoot + " | Last Target ID " + RobberC_LastTargetId + " | SetRob " + RobberCSetRob + " | Init " + RobberCInitialAnimDone, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
					num2 += 0.03f;
					drawText("~g~C~w~ | Target Type " + RobberCCurrentType.ToString() + " | Lockbox " + RobberCCurrentLockboxStage + " | LastAnim " + RobberCWaitingForLastAnim, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
					num2 += 0.1f;
					drawText("Smash Cabinet " + RobberA_JustFinishedSmashingCabinet + "," + RobberB_JustFinishedSmashingCabinet + "," + RobberC_JustFinishedSmashingCabinet, 0.1f, num2 + 0.03f, 0.3f, 255, 255, 255);
				}
				if (RobberA_JustFinishedSmashingCabinet && Game.GameTime >= RobberA_ResetSmashTimer)
				{
					if (RobberA_jewelsmashint == 60)
					{
						RobberARobbing = false;
						RobberA_jewelsmashint = 0;
					}
					if (RobberA_jewelsmashint2 == 60)
					{
						RobberARobbing = false;
						RobberA_jewelsmashint2 = 0;
					}
					if (RobberA_jewelsmashint3 == 60)
					{
						RobberARobbing = false;
						RobberA_jewelsmashint3 = 0;
					}
					if (RobberA_jewelsmashint4 == 60)
					{
						RobberARobbing = false;
						RobberA_jewelsmashint4 = 0;
					}
					RobberA_JustFinishedSmashingCabinet = false;
					RobberARobbing = false;
					if (RobberA != null)
					{
						RobberA.Task.ClearAllImmediately();
					}
				}
				if (!RobberARobbing && !RobberA_JustFinishedSmashingCabinet && (RobberA_jewelsmashint != 0 || RobberA_jewelsmashint2 != 0 || RobberA_jewelsmashint3 != 0 || RobberA_jewelsmashint4 != 0))
				{
					RobberA_jewelsmashint = 0;
					RobberA_jewelsmashint2 = 0;
					RobberA_jewelsmashint3 = 0;
					RobberA_jewelsmashint4 = 0;
					if (RobberA != null)
					{
						RobberA.Task.ClearAllImmediately();
					}
				}
				if (RobberB_JustFinishedSmashingCabinet && Game.GameTime >= RobberB_ResetSmashTimer)
				{
					if (RobberB_jewelsmashint == 60)
					{
						RobberBRobbing = false;
						RobberB_jewelsmashint = 0;
					}
					if (RobberB_jewelsmashint2 == 60)
					{
						RobberBRobbing = false;
						RobberB_jewelsmashint2 = 0;
					}
					if (RobberB_jewelsmashint3 == 60)
					{
						RobberBRobbing = false;
						RobberB_jewelsmashint3 = 0;
					}
					if (RobberB_jewelsmashint4 == 60)
					{
						RobberBRobbing = false;
						RobberB_jewelsmashint4 = 0;
					}
					RobberB_JustFinishedSmashingCabinet = false;
					RobberBRobbing = false;
					if (RobberB != null)
					{
						RobberB.Task.ClearAllImmediately();
					}
				}
				if (!RobberBRobbing && !RobberB_JustFinishedSmashingCabinet && (RobberB_jewelsmashint != 0 || RobberB_jewelsmashint2 != 0 || RobberB_jewelsmashint3 != 0 || RobberB_jewelsmashint4 != 0))
				{
					RobberB_jewelsmashint = 0;
					RobberB_jewelsmashint2 = 0;
					RobberB_jewelsmashint3 = 0;
					RobberB_jewelsmashint4 = 0;
					if (RobberA != null)
					{
						RobberA.Task.ClearAllImmediately();
					}
				}
				if (RobberC_JustFinishedSmashingCabinet && Game.GameTime >= RobberC_ResetSmashTimer)
				{
					if (RobberC_jewelsmashint == 60)
					{
						RobberCRobbing = false;
						RobberC_jewelsmashint = 0;
					}
					if (RobberC_jewelsmashint2 == 60)
					{
						RobberCRobbing = false;
						RobberC_jewelsmashint2 = 0;
					}
					if (RobberC_jewelsmashint3 == 60)
					{
						RobberCRobbing = false;
						RobberC_jewelsmashint3 = 0;
					}
					if (RobberC_jewelsmashint4 == 60)
					{
						RobberCRobbing = false;
						RobberC_jewelsmashint4 = 0;
					}
					RobberC_JustFinishedSmashingCabinet = false;
					RobberCRobbing = false;
					if (RobberC != null)
					{
						RobberC.Task.ClearAllImmediately();
					}
				}
				if (!RobberCRobbing && !RobberC_JustFinishedSmashingCabinet && (RobberC_jewelsmashint != 0 || RobberC_jewelsmashint2 != 0 || RobberC_jewelsmashint3 != 0 || RobberC_jewelsmashint4 != 0))
				{
					RobberC_jewelsmashint = 0;
					RobberC_jewelsmashint2 = 0;
					RobberC_jewelsmashint3 = 0;
					RobberC_jewelsmashint4 = 0;
					if (RobberA != null)
					{
						RobberA.Task.ClearAllImmediately();
					}
				}
				CheckRobberyAnimations();
				Vector3 vector = default(Vector3);
				float radius = 100f;
				if (Game.GameTime >= GetNearestLootPlayer && !PlayingAnim)
				{
					bool flag3 = false;
					GetNearestLootPlayer = Game.GameTime + 1000;
					if (Crate_Valuables.Count > 0)
					{
						for (int l = 0; l < Crate_Valuables.Count; l++)
						{
							if (!Crate_Valuables[l].IsRobbed && GetDistance_VDIST2(Game.Player.Character.Position, Crate_Valuables[l].Loot.Position, "<", radius))
							{
								flag3 = true;
								radius = GetDistance(Game.Player.Character.Position, Crate_Valuables[l].Loot.Position);
								vector = Crate_Valuables[l].Loot.Position;
								NearestCrateId = l;
							}
						}
					}
					if (!flag3)
					{
						NearestCrateId = -1;
					}
					if (Crate_Valuables.Count > 0)
					{
						for (int m = 0; m < Crate_Valuables.Count; m++)
						{
							bool isRobbed = Crate_Valuables[m].IsRobbed;
							if (GetDistance_VDIST2(Game.Player.Character.Position, Crate_Valuables[m].Position, "<", 30f) && Crate_Valuables[m].LootBlip != null)
							{
								if (!Crate_Valuables[m].IsRobbed)
								{
									Crate_Valuables[m].LootBlip.Alpha = 255;
								}
								else
								{
									Crate_Valuables[m].LootBlip.Alpha = 1;
								}
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, Crate_Valuables[m].Position, ">", 30f) && !Crate_Valuables[m].IsRobbed && Crate_Valuables[m].LootBlip != null)
							{
								Crate_Valuables[m].LootBlip.Alpha = 1;
							}
						}
					}
				}
				try
				{
					if (Crate_Valuables.Count > 0 && NearestCrateId != -1)
					{
						int nearestCrateId = NearestCrateId;
						if (Crate_Valuables[nearestCrateId].IsRobbed)
						{
							Crate_Valuables[nearestCrateId].LootBlip.Alpha = 1;
						}
						if (PlayingAnim)
						{
							CheckCrate(Game.Player.Character, Crate_Valuables[nearestCrateId], Crate_Valuables[nearestCrateId].Position, Crate_Valuables[nearestCrateId].Rotation, Crate_Valuables[nearestCrateId].LootType);
						}
						if (Crate_Valuables[nearestCrateId] != null)
						{
							Vector3 lootOffset = GetLootOffset(nearestCrateId);
							Vector3 _ = new Vector3(lootOffset.X, lootOffset.Y - 0.1f, lootOffset.Z + 0.2f);
							if (Crate_Valuables[nearestCrateId].LootType == LootType.GlassCut_Cabinet)
							{
								_ = new Vector3(lootOffset.X, lootOffset.Y - 0.1f, lootOffset.Z + 0.5f);
							}
							if (GetDistance_VDIST2(Game.Player.Character.Position, lootOffset, "<", 1.25f) || GetDistance_VDIST2(Game.Player.Character.Position, _, "<", 1.25f))
							{
								bool flag4 = Crate_Valuables[nearestCrateId].Loot != null;
								if (!PlayingAnim && !Crate_Valuables[nearestCrateId].IsRobbed)
								{
									if (Crate_Valuables[nearestCrateId].LootType == LootType.Trolly_Cash)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Cash From Trolly");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.Trolly_Gold)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Gold From Trolly");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.Trolly_Diamonds)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Diamonds From Trolly");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.CP_Cash)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Cash From Table");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.CP_Gold)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Gold From Table");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.M42_ServerStack)
									{
										if (Crate_Valuables[nearestCrateId].Loot.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_harddrive_stack_01a"))
										{
											DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Hard Drives");
										}
										if (Crate_Valuables[nearestCrateId].Loot.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_server_stack_01a"))
										{
											DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Server Drives");
										}
										if (Crate_Valuables[nearestCrateId].Loot.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_book_stack_01a"))
										{
											DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Documents");
										}
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.GlassCut_Cabinet)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Use Glass Cutter");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.LootBox_HDD)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Open Crate");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.LootBox_Plates)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Open Crate");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.WoodCrate)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Open Wooden Crate");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.TreasureChest)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Treasure Chest");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.Server_HDD)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Harddrive");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.SafetyDepositBoxes)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Drill The Safety Deposit Boxes");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.SafetyDepositBoxes_XM3)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Drill The Safety Deposit Boxes");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.CP_Safe)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Crack Safe");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.M32_Safe)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Crack Safe (M32)");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.XM3_Safe)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Crack Safe (XM3)");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.JewelStoreCabinet)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Smash Cabinet");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.M42_JewelCabinet)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Smash Cabinet");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.Painting)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Painting");
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.CashDryer)
									{
										DisplayHelpTextThisFrame("~INPUT_CONTEXT~ to Steal Cash from Cash Dryer");
									}
								}
								if (Game.IsControlJustPressed(Control.Context) && !Crate_Valuables[nearestCrateId].IsRobbed)
								{
									if (Crate_Valuables[nearestCrateId].LootType != LootType.CP_Safe && Crate_Valuables[nearestCrateId].LootType != LootType.M32_Safe && Crate_Valuables[nearestCrateId].LootType != LootType.XM3_Safe)
									{
										Crate_Valuables[nearestCrateId].IsRobbed = true;
									}
									PlayingAnim = true;
									Crate_Valuables[nearestCrateId].LootBlip.Alpha = 1;
									LootLeft -= 1f;
									CurrentLootTarget = nearestCrateId;
									Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
									if (Crate_Valuables[nearestCrateId].LootType == LootType.JewelStoreCabinet)
									{
										PlayingAnim = false;
									}
									if (Crate_Valuables[nearestCrateId].LootType == LootType.Painting)
									{
										CuttingStage = 0;
										PlayerCurrentID = nearestCrateId;
										PlayerCurrentPaintingNew = Crate_Valuables[nearestCrateId].Loot;
									}
									RobCrate(Game.Player.Character, Crate_Valuables[nearestCrateId].Loot, Crate_Valuables[nearestCrateId].Position, Crate_Valuables[nearestCrateId].Rotation, Crate_Valuables[nearestCrateId].LootType, nearestCrateId);
									RobCrate(Game.Player.Character, Crate_Valuables[nearestCrateId].Loot, Crate_Valuables[nearestCrateId].Position, Crate_Valuables[nearestCrateId].Rotation, Crate_Valuables[nearestCrateId].LootType, nearestCrateId);
								}
							}
						}
					}
				}
				catch
				{
				}
				if (RobberARobbing && RobberA != null && RobberA.Exists())
				{
					CheckCrate(RobberA, Crate_Valuables[RobberACurrentValualabe], Crate_Valuables[RobberACurrentValualabe].Position, Crate_Valuables[RobberACurrentValualabe].Rotation, Crate_Valuables[RobberACurrentValualabe].LootType);
				}
				if (RobberBRobbing && RobberB != null && RobberB.Exists())
				{
					CheckCrate(RobberB, Crate_Valuables[RobberBCurrentValualabe], Crate_Valuables[RobberBCurrentValualabe].Position, Crate_Valuables[RobberBCurrentValualabe].Rotation, Crate_Valuables[RobberBCurrentValualabe].LootType);
				}
				if (RobberCRobbing && RobberC != null && RobberC.Exists())
				{
					CheckCrate(RobberC, Crate_Valuables[RobberCCurrentValualabe], Crate_Valuables[RobberCCurrentValualabe].Position, Crate_Valuables[RobberCCurrentValualabe].Rotation, Crate_Valuables[RobberCCurrentValualabe].LootType);
				}
				Random random = new Random();
				if (RobberA != null && RobberANearLoot && RobberA.Exists())
				{
					if (!RobberARobbing && RobberAMoveTimer <= Game.GameTime)
					{
						RobberAMoveTimer = Game.GameTime + 1250;
						if (!RobberARobbing || RobberACuttingStage == -1)
						{
							if (RobberACuttingStage == -1)
							{
								RobberACuttingStage = 0;
								RobberARobbing = false;
							}
							if (RobberACuttingStage < 1)
							{
								int num3 = -1;
								float num4 = 9999f;
								for (int n = 0; n < Crate_Valuables.Count; n++)
								{
									if (!Crate_Valuables[n].IsRobbed && GetDistance_VDIST2(RobberA.Position, Crate_Valuables[n].Position, "<", num4))
									{
										num4 = GetDistance(RobberA.Position, Crate_Valuables[n].Position);
										num3 = n;
									}
								}
								RobberA_LastTargetId = num3;
								if (num3 != -1)
								{
									if (!Crate_Valuables[num3].IsRobbed)
									{
										if (Crate_Valuables[num3].LootType != LootType.GlassCut_Cabinet && !RobberA.IsWalking && !RobberA.IsSprinting && !RobberA.IsRunning)
										{
											RobberA_RobberMarker = GetLootOffset(num3);
											if (RobberA_RobberMarker.DistanceTo(new Vector3(0f, 0f, 0f)) > 10f)
											{
												if (RobberA_DistanceToTarget == num4)
												{
													Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberA, RobberA_RobberMarker.X, RobberA_RobberMarker.Y, RobberA_RobberMarker.Z, 10f, -1, Crate_Valuables[num3].Loot.Heading, 0f);
												}
												else
												{
													Vector3 robberA_RobberMarker = RobberA_RobberMarker;
													Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberA, robberA_RobberMarker.X, robberA_RobberMarker.Y, robberA_RobberMarker.Z, 30f, 20000, 0.1f, 512, 0);
												}
												RobberA_DistanceToTarget = num4;
												RobberAMovingToLoot = true;
												RobberACurrentValualabe = num3;
											}
										}
									}
									else if (GetDistance_VDIST2(RobberA.Position, RobberA_RobberMarker, ">=", 5f))
									{
										RobberA_RobberMarker = default(Vector3);
									}
								}
							}
						}
					}
					if (!RobberARobbing && GetDistance_VDIST2(RobberA.Position, RobberA_RobberMarker, "<", 1.25f) && Crate_Valuables[RobberACurrentValualabe].LootType != LootType.GlassCut_Cabinet && !Crate_Valuables[RobberACurrentValualabe].IsRobbed)
					{
						LootLeft -= 1f;
						if (RobberA_RobberMarker == RobberB_RobberMarker)
						{
							RobberB_RobberMarker = default(Vector3);
						}
						if (RobberA_RobberMarker == RobberC_RobberMarker)
						{
							RobberC_RobberMarker = default(Vector3);
						}
						Crate_Valuables[RobberACurrentValualabe].LootBlip.Alpha = 1;
						Crate_Valuables[RobberACurrentValualabe].IsRobbed = true;
						RobCrate(RobberA, Crate_Valuables[RobberACurrentValualabe].Loot, Crate_Valuables[RobberACurrentValualabe].Position, Crate_Valuables[RobberACurrentValualabe].Rotation, Crate_Valuables[RobberACurrentValualabe].LootType, RobberACurrentValualabe);
						RobCrate(RobberA, Crate_Valuables[RobberACurrentValualabe].Loot, Crate_Valuables[RobberACurrentValualabe].Position, Crate_Valuables[RobberACurrentValualabe].Rotation, Crate_Valuables[RobberACurrentValualabe].LootType, RobberACurrentValualabe);
					}
				}
				if (RobberB != null && RobberBNearLoot && RobberB.Exists())
				{
					if (!RobberBRobbing && RobberBMoveTimer <= Game.GameTime)
					{
						RobberBMoveTimer = Game.GameTime + 1250;
						if (!RobberBRobbing || RobberBCuttingStage == -1)
						{
							if (RobberBCuttingStage == -1)
							{
								RobberBCuttingStage = 0;
								RobberBRobbing = false;
							}
							if (RobberBCuttingStage < 1)
							{
								int num5 = -1;
								float num6 = 9999f;
								for (int num7 = 0; num7 < Crate_Valuables.Count; num7++)
								{
									if (!Crate_Valuables[num7].IsRobbed && GetDistance_VDIST2(RobberB.Position, Crate_Valuables[num7].Position, "<", num6))
									{
										num6 = GetDistance(RobberB.Position, Crate_Valuables[num7].Position);
										num5 = num7;
									}
								}
								RobberB_LastTargetId = num5;
								if (num5 != -1)
								{
									if (!Crate_Valuables[num5].IsRobbed)
									{
										if (Crate_Valuables[num5].LootType != LootType.GlassCut_Cabinet && !RobberB.IsWalking && !RobberB.IsSprinting && !RobberB.IsRunning)
										{
											RobberB_RobberMarker = GetLootOffset(num5);
											if (RobberB_RobberMarker.DistanceTo(new Vector3(0f, 0f, 0f)) > 10f)
											{
												if (RobberB_DistanceToTarget == num6)
												{
													Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberB, RobberB_RobberMarker.X, RobberB_RobberMarker.Y, RobberB_RobberMarker.Z, 10f, -1, Crate_Valuables[num5].Loot.Heading, 0f);
												}
												else
												{
													Vector3 robberB_RobberMarker = RobberB_RobberMarker;
													Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberB, robberB_RobberMarker.X, robberB_RobberMarker.Y, robberB_RobberMarker.Z, 30f, 20000, 0.1f, 512, 0);
												}
												RobberB_DistanceToTarget = num6;
												RobberBMovingToLoot = true;
												RobberBCurrentValualabe = num5;
											}
										}
									}
									else if (GetDistance_VDIST2(RobberB.Position, RobberB_RobberMarker, ">=", 5f))
									{
										RobberB_RobberMarker = default(Vector3);
									}
								}
							}
						}
					}
					if (!RobberBRobbing && GetDistance_VDIST2(RobberB.Position, RobberB_RobberMarker, "<", 1.25f) && Crate_Valuables[RobberBCurrentValualabe].LootType != LootType.GlassCut_Cabinet && !Crate_Valuables[RobberBCurrentValualabe].IsRobbed)
					{
						LootLeft -= 1f;
						if (RobberB_RobberMarker == RobberA_RobberMarker)
						{
							RobberA_RobberMarker = default(Vector3);
						}
						if (RobberB_RobberMarker == RobberC_RobberMarker)
						{
							RobberC_RobberMarker = default(Vector3);
						}
						Crate_Valuables[RobberBCurrentValualabe].LootBlip.Alpha = 1;
						Crate_Valuables[RobberBCurrentValualabe].IsRobbed = true;
						RobCrate(RobberB, Crate_Valuables[RobberBCurrentValualabe].Loot, Crate_Valuables[RobberBCurrentValualabe].Position, Crate_Valuables[RobberBCurrentValualabe].Rotation, Crate_Valuables[RobberBCurrentValualabe].LootType, RobberBCurrentValualabe);
						RobCrate(RobberB, Crate_Valuables[RobberBCurrentValualabe].Loot, Crate_Valuables[RobberBCurrentValualabe].Position, Crate_Valuables[RobberBCurrentValualabe].Rotation, Crate_Valuables[RobberBCurrentValualabe].LootType, RobberBCurrentValualabe);
					}
				}
				if (RobberC != null && RobberCNearLoot && RobberC.Exists())
				{
					if (!RobberCRobbing && RobberCMoveTimer <= Game.GameTime)
					{
						RobberCMoveTimer = Game.GameTime + 1250;
						if (!RobberCRobbing || RobberCCuttingStage == -1)
						{
							if (RobberCCuttingStage == -1)
							{
								RobberCCuttingStage = 0;
								RobberCRobbing = false;
							}
							if (RobberCCuttingStage < 1)
							{
								int num8 = -1;
								float num9 = 9999f;
								for (int num10 = 0; num10 < Crate_Valuables.Count; num10++)
								{
									if (!Crate_Valuables[num10].IsRobbed && GetDistance_VDIST2(RobberC.Position, Crate_Valuables[num10].Position, "<", num9))
									{
										num9 = GetDistance(RobberC.Position, Crate_Valuables[num10].Position);
										num8 = num10;
									}
								}
								RobberC_LastTargetId = num8;
								if (num8 != -1)
								{
									if (!Crate_Valuables[num8].IsRobbed)
									{
										if (Crate_Valuables[num8].LootType != LootType.GlassCut_Cabinet && !RobberC.IsWalking && !RobberC.IsSprinting && !RobberC.IsRunning)
										{
											RobberC_RobberMarker = GetLootOffset(num8);
											if (RobberC_RobberMarker.DistanceTo(new Vector3(0f, 0f, 0f)) > 10f)
											{
												if (RobberC_DistanceToTarget == num9)
												{
													Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, RobberC, RobberC_RobberMarker.X, RobberC_RobberMarker.Y, RobberC_RobberMarker.Z, 10f, -1, Crate_Valuables[num8].Loot.Heading, 0f);
												}
												else
												{
													Vector3 robberC_RobberMarker = RobberC_RobberMarker;
													Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, RobberC, robberC_RobberMarker.X, robberC_RobberMarker.Y, robberC_RobberMarker.Z, 30f, 20000, 0.1f, 512, 0);
												}
												RobberC_DistanceToTarget = num9;
												RobberCMovingToLoot = true;
												RobberCCurrentValualabe = num8;
											}
										}
									}
									else if (GetDistance_VDIST2(RobberC.Position, RobberC_RobberMarker, ">=", 5f))
									{
										RobberC_RobberMarker = default(Vector3);
									}
								}
							}
						}
					}
					if (!RobberCRobbing && GetDistance_VDIST2(RobberC.Position, RobberC_RobberMarker, "<", 1.25f) && Crate_Valuables[RobberCCurrentValualabe].LootType != LootType.GlassCut_Cabinet && !Crate_Valuables[RobberCCurrentValualabe].IsRobbed)
					{
						LootLeft -= 1f;
						if (RobberC_RobberMarker == RobberA_RobberMarker)
						{
							RobberA_RobberMarker = default(Vector3);
						}
						if (RobberC_RobberMarker == RobberB_RobberMarker)
						{
							RobberB_RobberMarker = default(Vector3);
							Notification.Show("Remove Robber B marker 6");
						}
						Crate_Valuables[RobberCCurrentValualabe].LootBlip.Alpha = 1;
						Crate_Valuables[RobberCCurrentValualabe].IsRobbed = true;
						RobCrate(RobberC, Crate_Valuables[RobberCCurrentValualabe].Loot, Crate_Valuables[RobberCCurrentValualabe].Position, Crate_Valuables[RobberCCurrentValualabe].Rotation, Crate_Valuables[RobberCCurrentValualabe].LootType, RobberCCurrentValualabe);
						RobCrate(RobberC, Crate_Valuables[RobberCCurrentValualabe].Loot, Crate_Valuables[RobberCCurrentValualabe].Position, Crate_Valuables[RobberCCurrentValualabe].Rotation, Crate_Valuables[RobberCCurrentValualabe].LootType, RobberCCurrentValualabe);
					}
				}
			}
			if (0 == 0 && RobberA != null && RobberA.Exists())
			{
				if (GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, ">", 50f) && RobberA.IsAlive)
				{
					RobberA.Position = Game.Player.Character.Position.Around(3f);
				}
				if (!Function.Call<bool>(Hash.IS_PED_GROUP_MEMBER, RobberA, num) && RobberA != null)
				{
					RobberA.Task.ClearAll();
					Ped robberA2 = RobberA;
					PedGroup pedGroup4 = Game.Player.Character.PedGroup;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, robberA2, pedGroup4);
					Function.Call(Hash.SET_PED_COMBAT_ABILITY, robberA2, 100);
					RobberA.Weapons.Give(WeaponHash.PistolMk2, 9999, true, true);
				}
			}
		}

		public ManageRobberyAnims()
		{
			base.Tick += onTick;
			base.Aborted += onShutdown;
		}

		public void onShutdown(object sender, EventArgs e)
		{
			if (CurrentWashDryer != null)
			{
				CurrentWashDryer.CleanupWashMach();
			}
			if (PlasCam != null)
			{
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				World.RenderingCamera = null;
				PlasCam.Delete();
				PlasCam.IsActive = false;
				PlasCam = null;
			}
			if (AnimatedCamera != null)
			{
				World.RenderingCamera = null;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
				AnimatedCamera.IsActive = false;
				AnimatedCamera.Delete();
			}
			if (depositlist.Count > 0)
			{
				foreach (Prop item in depositlist.ToList())
				{
					if (item != null)
					{
						item.Delete();
						depositlist.Remove(item);
					}
				}
			}
			if (bagprop2 != null)
			{
				bagprop2.Delete();
			}
			if (drillmodelprop2 != null)
			{
				drillmodelprop2.Delete();
			}
			if (sounddissapearProp != null)
			{
				sounddissapearProp.Delete();
			}
			if (Moneymodelprop2 != null)
			{
				Moneymodelprop2.Delete();
			}
			if (DrillSound != -1)
			{
				Function.Call(Hash.STOP_SOUND, DrillSound);
				Function.Call(Hash.RELEASE_SOUND_ID, DrillSound);
			}
			foreach (Prop item2 in JewelCases_Smashed)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			JewelCases_Smashed.Clear();
			foreach (Prop secondaryProp in SecondaryProps)
			{
				if (secondaryProp != null)
				{
					secondaryProp.Delete();
				}
			}
			SecondaryProps.Clear();
			if (jewelrootlist.Count <= 0)
			{
				foreach (Prop item3 in jewelrootlist.ToList())
				{
					if (item3 != null)
					{
						item3.Delete();
						jewelrootlist.Remove(item3);
					}
				}
			}
			if (jewelbrokenglasslist.Count <= 0)
			{
				foreach (Prop item4 in jewelbrokenglasslist.ToList())
				{
					if (item4 != null)
					{
						item4.Delete();
						jewelbrokenglasslist.Remove(item4);
					}
				}
			}
			Game.Player.Character.Task.ClearAll();
			Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, false);
			if (bagProp != null)
			{
				bagProp.Delete();
				bagProp.MarkAsNoLongerNeeded();
				bagProp = null;
			}
			if (SafeValuables != null)
			{
				SafeValuables.Delete();
			}
			foreach (MissionOjects spawnedSilverCrate in SpawnedSilverCrates)
			{
				spawnedSilverCrate.DeleteEntites();
			}
			if (CurrentWoodenContainer != null)
			{
				if (CurrentWoodenContainer.CrowBar != null)
				{
					CurrentWoodenContainer.CrowBar.Delete();
				}
				if (CurrentWoodenContainer.LootProp2 != null)
				{
					CurrentWoodenContainer.LootProp2.Delete();
				}
				if (CurrentWoodenContainer.LootProp1 != null)
				{
					CurrentWoodenContainer.LootProp1.Delete();
				}
				if (CurrentWoodenContainer.BagProp != null)
				{
					CurrentWoodenContainer.BagProp.Delete();
				}
				foreach (ObjectPlacementMap prop in CurrentWoodenContainer.Props)
				{
					if (prop.Entity != null)
					{
						prop.Entity.Delete();
					}
				}
			}
			if (PanthereCaseB != null)
			{
				PanthereCaseB.Delete();
			}
			if (PanthereCaseA != null)
			{
				PanthereCaseA.Delete();
			}
			if (PanthereStatue != null)
			{
				PanthereStatue.Delete();
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
			foreach (Prop smokeEmitter in SmokeEmitters)
			{
				if (smokeEmitter != null)
				{
					smokeEmitter.Delete();
				}
			}
			Function.Call(Hash.REMOVE_PARTICLE_FX_FROM_ENTITY, Game.Player.Character);
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
			foreach (Lootable crate_Valuable in Crate_Valuables)
			{
				crate_Valuable.GotValuables = false;
				if (crate_Valuable.GlassCut != null)
				{
					crate_Valuable.GlassCut.Delete();
				}
				if (crate_Valuable.Cabinet != null)
				{
					crate_Valuable.Cabinet.Delete();
				}
				if (crate_Valuable.FakeCabinet != null)
				{
					crate_Valuable.FakeCabinet.Delete();
				}
				foreach (Prop item5 in crate_Valuable.SecondaryLoot)
				{
					if (item5 != null)
					{
						item5.Delete();
					}
				}
				if (crate_Valuable.Loot != null)
				{
					crate_Valuable.Loot.Delete();
				}
				if (crate_Valuable.LootBlip != null)
				{
					crate_Valuable.LootBlip.Delete();
				}
			}
		}

		public static void DeleteLoot()
		{
			Cannot_Steal_Loot = false;
			foreach (Prop artFrame in ArtFrames)
			{
				if (artFrame != null)
				{
					artFrame.Delete();
				}
			}
			ArtFrames.Clear();
			Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 32, true);
			Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 33, true);
			Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 34, true);
			Function.Call(Hash.ENABLE_CONTROL_ACTION, 0, 35, true);
			if (depositlist.Count > 0)
			{
				foreach (Prop item in depositlist.ToList())
				{
					if (item != null)
					{
						item.Delete();
						depositlist.Remove(item);
					}
				}
			}
			if (bagprop2 != null)
			{
				bagprop2.Delete();
			}
			if (drillmodelprop2 != null)
			{
				drillmodelprop2.Delete();
			}
			if (sounddissapearProp != null)
			{
				sounddissapearProp.Delete();
			}
			if (Moneymodelprop2 != null)
			{
				Moneymodelprop2.Delete();
			}
			foreach (Prop item2 in JewelCases_Smashed)
			{
				if (item2 != null)
				{
					item2.Delete();
				}
			}
			JewelCases_Smashed.Clear();
			foreach (Lootable crate_Valuable in Crate_Valuables)
			{
				if (crate_Valuable.Loot != null)
				{
					crate_Valuable.Loot.Delete();
				}
				if (crate_Valuable.LootBlip != null)
				{
					crate_Valuable.LootBlip.Delete();
				}
			}
			Crate_Valuables.Clear();
			LootFound.Clear();
			LootLeft = 0f;
			if (PanthereCaseB != null)
			{
				PanthereCaseB.Delete();
			}
			if (PanthereCaseA != null)
			{
				PanthereCaseA.Delete();
			}
			if (PanthereStatue != null)
			{
				PanthereStatue.Delete();
			}
			CreatedCrates = false;
		}

		public static Lootable GetLootableFromProp(Prop O)
		{
			Lootable result = null;
			if (LootFound.Contains(O))
			{
				int num = LootFound.IndexOf(O);
				if (num != -1)
				{
					result = Crate_Valuables[num];
				}
			}
			return result;
		}

		public static Lootable AddPropAsLoot(Prop O, List<Prop> SecondaryLoot = null)
		{
			Lootable lootable = new Lootable();
			if (!LootFound.Contains(O))
			{
				if (O == null)
				{
					return lootable;
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_prtmachine_dryer_op"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip = O.AddBlip();
						blip.Sprite = BlipSprite.Standard;
						blip.Color = BlipColor.Green;
						blip.Name = "Steal Valuables";
						blip.Scale = 0.5f;
						blip.IsShortRange = true;
					}
					lootable = new Lootable(LootType.CashDryer, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip2 = O.AddBlip();
						blip2.Sprite = BlipSprite.Standard;
						blip2.Color = BlipColor.Green;
						blip2.Name = "Steal Valuables";
						blip2.Scale = 0.5f;
						blip2.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip3 = O.AddBlip();
						blip3.Sprite = BlipSprite.Standard;
						blip3.Color = BlipColor.Green;
						blip3.Name = "Steal Valuables";
						blip3.Scale = 0.5f;
						blip3.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip4 = O.AddBlip();
						blip4.Sprite = BlipSprite.Standard;
						blip4.Color = BlipColor.Green;
						blip4.Name = "Steal Valuables";
						blip4.Scale = 0.5f;
						blip4.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01d"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip5 = O.AddBlip();
						blip5.Sprite = BlipSprite.Standard;
						blip5.Color = BlipColor.Green;
						blip5.Name = "Steal Valuables";
						blip5.Scale = 0.5f;
						blip5.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_013"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip6 = O.AddBlip();
						blip6.Sprite = BlipSprite.Standard;
						blip6.Color = BlipColor.Green;
						blip6.Name = "Steal Valuables";
						blip6.Scale = 0.5f;
						blip6.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01f"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip7 = O.AddBlip();
						blip7.Sprite = BlipSprite.Standard;
						blip7.Color = BlipColor.Green;
						blip7.Name = "Steal Valuables";
						blip7.Scale = 0.5f;
						blip7.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01g"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip8 = O.AddBlip();
						blip8.Sprite = BlipSprite.Standard;
						blip8.Color = BlipColor.Green;
						blip8.Name = "Steal Valuables";
						blip8.Scale = 0.5f;
						blip8.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01h"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip9 = O.AddBlip();
						blip9.Sprite = BlipSprite.Standard;
						blip9.Color = BlipColor.Green;
						blip9.Name = "Steal Valuables";
						blip9.Scale = 0.5f;
						blip9.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip10 = O.AddBlip();
						blip10.Sprite = BlipSprite.Standard;
						blip10.Color = BlipColor.Green;
						blip10.Name = "Steal Valuables";
						blip10.Scale = 0.5f;
						blip10.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip11 = O.AddBlip();
						blip11.Sprite = BlipSprite.Standard;
						blip11.Color = BlipColor.Green;
						blip11.Name = "Steal Valuables";
						blip11.Scale = 0.5f;
						blip11.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip12 = O.AddBlip();
						blip12.Sprite = BlipSprite.Standard;
						blip12.Color = BlipColor.Green;
						blip12.Name = "Steal Valuables";
						blip12.Scale = 0.5f;
						blip12.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip13 = O.AddBlip();
						blip13.Sprite = BlipSprite.Standard;
						blip13.Color = BlipColor.Green;
						blip13.Name = "Steal Valuables";
						blip13.Scale = 0.5f;
						blip13.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip14 = O.AddBlip();
						blip14.Sprite = BlipSprite.Standard;
						blip14.Color = BlipColor.Green;
						blip14.Name = "Steal Valuables";
						blip14.Scale = 0.5f;
						blip14.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01d"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip15 = O.AddBlip();
						blip15.Sprite = BlipSprite.Standard;
						blip15.Color = BlipColor.Green;
						blip15.Name = "Steal Valuables";
						blip15.Scale = 0.5f;
						blip15.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01e"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip16 = O.AddBlip();
						blip16.Sprite = BlipSprite.Standard;
						blip16.Color = BlipColor.Green;
						blip16.Name = "Steal Valuables";
						blip16.Scale = 0.5f;
						blip16.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01f"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip17 = O.AddBlip();
						blip17.Sprite = BlipSprite.Standard;
						blip17.Color = BlipColor.Green;
						blip17.Name = "Steal Valuables";
						blip17.Scale = 0.5f;
						blip17.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01g"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip18 = O.AddBlip();
						blip18.Sprite = BlipSprite.Standard;
						blip18.Color = BlipColor.Green;
						blip18.Name = "Steal Valuables";
						blip18.Scale = 0.5f;
						blip18.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01h"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip19 = O.AddBlip();
						blip19.Sprite = BlipSprite.Standard;
						blip19.Color = BlipColor.Green;
						blip19.Name = "Steal Valuables";
						blip19.Scale = 0.5f;
						blip19.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_1_prop_m41_painting_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip20 = O.AddBlip();
						blip20.Sprite = BlipSprite.Standard;
						blip20.Color = BlipColor.Green;
						blip20.Name = "Steal Valuables";
						blip20.Scale = 0.5f;
						blip20.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_1_prop_m41_painting_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip21 = O.AddBlip();
						blip21.Sprite = BlipSprite.Standard;
						blip21.Color = BlipColor.Green;
						blip21.Name = "Steal Valuables";
						blip21.Scale = 0.5f;
						blip21.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_1_prop_m41_painting_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip22 = O.AddBlip();
						blip22.Sprite = BlipSprite.Standard;
						blip22.Color = BlipColor.Green;
						blip22.Name = "Steal Valuables";
						blip22.Scale = 0.5f;
						blip22.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_1_prop_m41_painting_01d"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip23 = O.AddBlip();
						blip23.Sprite = BlipSprite.Standard;
						blip23.Color = BlipColor.Green;
						blip23.Name = "Steal Valuables";
						blip23.Scale = 0.5f;
						blip23.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Painting, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip24 = O.AddBlip();
						blip24.Sprite = BlipSprite.Standard;
						blip24.Color = BlipColor.Green;
						blip24.Name = "Steal Valuables";
						blip24.Scale = 0.5f;
						blip24.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip25 = O.AddBlip();
						blip25.Sprite = BlipSprite.Standard;
						blip25.Color = BlipColor.Green;
						blip25.Name = "Steal Valuables";
						blip25.Scale = 0.5f;
						blip25.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_cash_trolly_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip26 = O.AddBlip();
						blip26.Sprite = BlipSprite.Standard;
						blip26.Color = BlipColor.Green;
						blip26.Name = "Steal Valuables";
						blip26.Scale = 0.5f;
						blip26.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip27 = O.AddBlip();
						blip27.Sprite = BlipSprite.Standard;
						blip27.Color = BlipColor.Green;
						blip27.Name = "Steal Valuables";
						blip27.Scale = 0.5f;
						blip27.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip28 = O.AddBlip();
						blip28.Sprite = BlipSprite.Standard;
						blip28.Color = BlipColor.Green;
						blip28.Name = "Steal Valuables";
						blip28.Scale = 0.5f;
						blip28.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_cash_low_trolly_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip29 = O.AddBlip();
						blip29.Sprite = BlipSprite.Standard;
						blip29.Color = BlipColor.Green;
						blip29.Name = "Steal Valuables";
						blip29.Scale = 0.5f;
						blip29.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "hei_prop_hei_cash_trolly_01"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip30 = O.AddBlip();
						blip30.Sprite = BlipSprite.Standard;
						blip30.Color = BlipColor.Green;
						blip30.Name = "Steal Valuables";
						blip30.Scale = 0.5f;
						blip30.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip31 = O.AddBlip();
						blip31.Sprite = BlipSprite.Standard;
						blip31.Color = BlipColor.Green;
						blip31.Name = "Steal Valuables";
						blip31.Scale = 0.5f;
						blip31.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Gold, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip32 = O.AddBlip();
						blip32.Sprite = BlipSprite.Standard;
						blip32.Color = BlipColor.Green;
						blip32.Name = "Steal Valuables";
						blip32.Scale = 0.5f;
						blip32.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Gold, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_gold_trolly_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip33 = O.AddBlip();
						blip33.Sprite = BlipSprite.Standard;
						blip33.Color = BlipColor.Green;
						blip33.Name = "Steal Valuables";
						blip33.Scale = 0.5f;
						blip33.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Gold, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip34 = O.AddBlip();
						blip34.Sprite = BlipSprite.Standard;
						blip34.Color = BlipColor.Green;
						blip34.Name = "Steal Valuables";
						blip34.Scale = 0.5f;
						blip34.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Diamonds, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip35 = O.AddBlip();
						blip35.Sprite = BlipSprite.Standard;
						blip35.Color = BlipColor.Green;
						blip35.Name = "Steal Valuables";
						blip35.Scale = 0.5f;
						blip35.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Diamonds, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_diamond_trolly_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip36 = O.AddBlip();
						blip36.Sprite = BlipSprite.Standard;
						blip36.Color = BlipColor.Green;
						blip36.Name = "Steal Valuables";
						blip36.Scale = 0.5f;
						blip36.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Diamonds, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "imp_prop_impexp_coke_trolly"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip37 = O.AddBlip();
						blip37.Sprite = BlipSprite.Standard;
						blip37.Color = BlipColor.Green;
						blip37.Name = "Steal Valuables";
						blip37.Scale = 0.5f;
						blip37.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Trolly_Coke, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_cash_stack_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip38 = O.AddBlip();
						blip38.Sprite = BlipSprite.Standard;
						blip38.Color = BlipColor.Green;
						blip38.Name = "Steal Valuables";
						blip38.Scale = 0.5f;
						blip38.IsShortRange = true;
					}
					lootable = new Lootable(LootType.CP_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_weed_stack_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip39 = O.AddBlip();
						blip39.Sprite = BlipSprite.Standard;
						blip39.Color = BlipColor.Green;
						blip39.Name = "Steal Valuables";
						blip39.Scale = 0.5f;
						blip39.IsShortRange = true;
					}
					lootable = new Lootable(LootType.CP_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_coke_stack_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip40 = O.AddBlip();
						blip40.Sprite = BlipSprite.Standard;
						blip40.Color = BlipColor.Green;
						blip40.Name = "Steal Valuables";
						blip40.Scale = 0.5f;
						blip40.IsShortRange = true;
					}
					lootable = new Lootable(LootType.CP_Cash, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_gold_stack_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip41 = O.AddBlip();
						blip41.Sprite = BlipSprite.Standard;
						blip41.Color = BlipColor.Green;
						blip41.Name = "Steal Valuables";
						blip41.Scale = 0.5f;
						blip41.IsShortRange = true;
					}
					lootable = new Lootable(LootType.CP_Gold, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "xm_prop_vancrate_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip42 = O.AddBlip();
						blip42.Sprite = BlipSprite.Standard;
						blip42.Color = BlipColor.Green;
						blip42.Name = "Steal Valuables";
						blip42.Scale = 0.5f;
						blip42.IsShortRange = true;
					}
					lootable = new Lootable(LootType.LootBox_HDD, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "xm3_prop_xm3_crate_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip43 = O.AddBlip();
						blip43.Sprite = BlipSprite.Standard;
						blip43.Color = BlipColor.Green;
						blip43.Name = "Steal Valuables";
						blip43.Scale = 0.5f;
						blip43.IsShortRange = true;
					}
					lootable = new Lootable(LootType.WoodCrate, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_ser_storage_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip44 = O.AddBlip();
						blip44.Sprite = BlipSprite.Standard;
						blip44.Color = BlipColor.Green;
						blip44.Name = "Steal Valuables";
						blip44.Scale = 0.5f;
						blip44.IsShortRange = true;
					}
					lootable = new Lootable(LootType.Server_HDD, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "tr_prop_tr_adv_case_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip45 = O.AddBlip();
						blip45.Sprite = BlipSprite.Standard;
						blip45.Color = BlipColor.Green;
						blip45.Name = "Steal Valuables";
						blip45.Scale = 0.5f;
						blip45.IsShortRange = true;
					}
					lootable = new Lootable(LootType.LootBox_Plates, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_safe_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip46 = O.AddBlip();
						blip46.Sprite = BlipSprite.Standard;
						blip46.Color = BlipColor.Green;
						blip46.Name = "Steal Valuables";
						blip46.Scale = 0.5f;
						blip46.IsShortRange = true;
					}
					lootable = new Lootable(LootType.CP_Safe, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_safe_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip47 = O.AddBlip();
						blip47.Sprite = BlipSprite.Standard;
						blip47.Color = BlipColor.Green;
						blip47.Name = "Steal Valuables";
						blip47.Scale = 0.5f;
						blip47.IsShortRange = true;
					}
					lootable = new Lootable(LootType.M32_Safe, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "xm3_Prop_xm3_safe_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip48 = O.AddBlip();
						blip48.Sprite = BlipSprite.Standard;
						blip48.Color = BlipColor.Green;
						blip48.Name = "Steal Valuables";
						blip48.Scale = 0.5f;
						blip48.IsShortRange = true;
					}
					lootable = new Lootable(LootType.XM3_Safe, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab2_start"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip49 = O.AddBlip();
						blip49.Sprite = BlipSprite.Standard;
						blip49.Color = BlipColor.Green;
						blip49.Name = "Steal Valuables";
						blip49.Scale = 0.5f;
						blip49.IsShortRange = true;
					}
					lootable = new Lootable(LootType.JewelStoreCabinet, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab3_start"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip50 = O.AddBlip();
						blip50.Sprite = BlipSprite.Standard;
						blip50.Color = BlipColor.Green;
						blip50.Name = "Steal Valuables";
						blip50.Scale = 0.5f;
						blip50.IsShortRange = true;
					}
					lootable = new Lootable(LootType.JewelStoreCabinet, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip51 = O.AddBlip();
						blip51.Sprite = BlipSprite.Standard;
						blip51.Color = BlipColor.Green;
						blip51.Name = "Steal Valuables";
						blip51.Scale = 0.5f;
						blip51.IsShortRange = true;
					}
					lootable = new Lootable(LootType.JewelStoreCabinet, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab_start"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip52 = O.AddBlip();
						blip52.Sprite = BlipSprite.Standard;
						blip52.Color = BlipColor.Green;
						blip52.Name = "Steal Valuables";
						blip52.Scale = 0.5f;
						blip52.IsShortRange = true;
					}
					lootable = new Lootable(LootType.JewelStoreCabinet, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip53 = O.AddBlip();
						blip53.Sprite = BlipSprite.Standard;
						blip53.Color = BlipColor.Green;
						blip53.Name = "Steal Valuables";
						blip53.Scale = 0.5f;
						blip53.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip54 = O.AddBlip();
						blip54.Sprite = BlipSprite.Standard;
						blip54.Color = BlipColor.Green;
						blip54.Name = "Steal Valuables";
						blip54.Scale = 0.5f;
						blip54.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip55 = O.AddBlip();
						blip55.Sprite = BlipSprite.Standard;
						blip55.Color = BlipColor.Green;
						blip55.Name = "Steal Valuables";
						blip55.Scale = 0.5f;
						blip55.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01d"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip56 = O.AddBlip();
						blip56.Sprite = BlipSprite.Standard;
						blip56.Color = BlipColor.Green;
						blip56.Name = "Steal Valuables";
						blip56.Scale = 0.5f;
						blip56.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01e"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip57 = O.AddBlip();
						blip57.Sprite = BlipSprite.Standard;
						blip57.Color = BlipColor.Green;
						blip57.Name = "Steal Valuables";
						blip57.Scale = 0.5f;
						blip57.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01f"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip58 = O.AddBlip();
						blip58.Sprite = BlipSprite.Standard;
						blip58.Color = BlipColor.Green;
						blip58.Name = "Steal Valuables";
						blip58.Scale = 0.5f;
						blip58.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01g"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip59 = O.AddBlip();
						blip59.Sprite = BlipSprite.Standard;
						blip59.Color = BlipColor.Green;
						blip59.Name = "Steal Valuables";
						blip59.Scale = 0.5f;
						blip59.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01h"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip60 = O.AddBlip();
						blip60.Sprite = BlipSprite.Standard;
						blip60.Color = BlipColor.Green;
						blip60.Name = "Steal Valuables";
						blip60.Scale = 0.5f;
						blip60.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_ch_sec_cabinet_01i"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip61 = O.AddBlip();
						blip61.Sprite = BlipSprite.Standard;
						blip61.Color = BlipColor.Green;
						blip61.Name = "Steal Valuables";
						blip61.Scale = 0.5f;
						blip61.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip62 = O.AddBlip();
						blip62.Sprite = BlipSprite.Standard;
						blip62.Color = BlipColor.Green;
						blip62.Name = "Steal Valuables";
						blip62.Scale = 0.5f;
						blip62.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01b"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip63 = O.AddBlip();
						blip63.Sprite = BlipSprite.Standard;
						blip63.Color = BlipColor.Green;
						blip63.Name = "Steal Valuables";
						blip63.Scale = 0.5f;
						blip63.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01c"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip64 = O.AddBlip();
						blip64.Sprite = BlipSprite.Standard;
						blip64.Color = BlipColor.Green;
						blip64.Name = "Steal Valuables";
						blip64.Scale = 0.5f;
						blip64.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01d"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip65 = O.AddBlip();
						blip65.Sprite = BlipSprite.Standard;
						blip65.Color = BlipColor.Green;
						blip65.Name = "Steal Valuables";
						blip65.Scale = 0.5f;
						blip65.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01e"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip66 = O.AddBlip();
						blip66.Sprite = BlipSprite.Standard;
						blip66.Color = BlipColor.Green;
						blip66.Name = "Steal Valuables";
						blip66.Scale = 0.5f;
						blip66.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01f"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip67 = O.AddBlip();
						blip67.Sprite = BlipSprite.Standard;
						blip67.Color = BlipColor.Green;
						blip67.Name = "Steal Valuables";
						blip67.Scale = 0.5f;
						blip67.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01g"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip68 = O.AddBlip();
						blip68.Sprite = BlipSprite.Standard;
						blip68.Color = BlipColor.Green;
						blip68.Name = "Steal Valuables";
						blip68.Scale = 0.5f;
						blip68.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01h"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip69 = O.AddBlip();
						blip69.Sprite = BlipSprite.Standard;
						blip69.Color = BlipColor.Green;
						blip69.Name = "Steal Valuables";
						blip69.Scale = 0.5f;
						blip69.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m23_2_prop_m32_sec_cabinet_01i"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip70 = O.AddBlip();
						blip70.Sprite = BlipSprite.Standard;
						blip70.Color = BlipColor.Green;
						blip70.Name = "Steal Valuables";
						blip70.Scale = 0.5f;
						blip70.IsShortRange = true;
					}
					lootable = new Lootable(LootType.SafetyDepositBoxes_XM3, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_glass_disp_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip71 = O.AddBlip();
						blip71.Sprite = BlipSprite.Standard;
						blip71.Color = BlipColor.Green;
						blip71.Name = "Steal Valuables";
						blip71.Scale = 0.5f;
						blip71.IsShortRange = true;
					}
					if (SecondaryLoot == null)
					{
						List<Prop> glassCutProps = World.GetNearbyProps(O.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), 0.45f).ToList();
						lootable = new Lootable(LootType.GlassCut_Cabinet, O, glassCutProps, O.AttachedBlip, false);
						Crate_Valuables.Add(lootable);
					}
					else
					{
						List<Prop> list = World.GetNearbyProps(O.GetOffsetPosition(new Vector3(0f, 0f, 1.25f)), 0.45f).ToList();
						lootable = new Lootable(LootType.GlassCut_Cabinet, O, SecondaryLoot, O.AttachedBlip, false);
						Crate_Valuables.Add(lootable);
					}
					O.Opacity = 255;
					O.IsVisible = true;
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_CokeCabinet_02a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip72 = O.AddBlip();
						blip72.Sprite = BlipSprite.Standard;
						blip72.Color = BlipColor.Green;
						blip72.Name = "Steal Valuables";
						blip72.Scale = 0.5f;
						blip72.IsShortRange = true;
					}
					lootable = new Lootable(LootType.M42_JewelCabinet, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_CokeCabinet_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip73 = O.AddBlip();
						blip73.Sprite = BlipSprite.Standard;
						blip73.Color = BlipColor.Green;
						blip73.Name = "Steal Valuables";
						blip73.Scale = 0.5f;
						blip73.IsShortRange = true;
					}
					lootable = new Lootable(LootType.M42_JewelCabinet, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_server_stack_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip74 = O.AddBlip();
						blip74.Sprite = BlipSprite.Standard;
						blip74.Color = BlipColor.Green;
						blip74.Name = "Steal Valuables";
						blip74.Scale = 0.5f;
						blip74.IsShortRange = true;
					}
					lootable = new Lootable(LootType.M42_ServerStack, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_harddrive_stack_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip75 = O.AddBlip();
						blip75.Sprite = BlipSprite.Standard;
						blip75.Color = BlipColor.Green;
						blip75.Name = "Steal Valuables";
						blip75.Scale = 0.5f;
						blip75.IsShortRange = true;
					}
					lootable = new Lootable(LootType.M42_ServerStack, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
				if (O.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_book_stack_01a"))
				{
					LootFound.Add(O);
					if (O.AttachedBlip == null)
					{
						Blip blip76 = O.AddBlip();
						blip76.Sprite = BlipSprite.Standard;
						blip76.Color = BlipColor.Green;
						blip76.Name = "Steal Valuables";
						blip76.Scale = 0.5f;
						blip76.IsShortRange = true;
					}
					lootable = new Lootable(LootType.M42_ServerStack, O, O.AttachedBlip, false);
					Crate_Valuables.Add(lootable);
				}
			}
			return lootable;
		}

		private Vector2 World3DToScreen2d2(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public Vector3 GetLootOffset(int LootID)
		{
			Vector3 vector = new Vector3(0f, 0f, 0f);
			vector = ((Crate_Valuables[LootID].LootType != LootType.Server_HDD && Crate_Valuables[LootID].LootType != LootType.WoodCrate && Crate_Valuables[LootID].LootType != LootType.CP_Cash && Crate_Valuables[LootID].LootType != LootType.CP_Gold && Crate_Valuables[LootID].LootType != LootType.M42_JewelCabinet && Crate_Valuables[LootID].LootType != LootType.M42_ServerStack && Crate_Valuables[LootID].LootType != LootType.CP_Safe && Crate_Valuables[LootID].LootType != LootType.XM3_Safe && Crate_Valuables[LootID].LootType != LootType.M32_Safe && Crate_Valuables[LootID].LootType != LootType.SafetyDepositBoxes && Crate_Valuables[LootID].LootType != LootType.SafetyDepositBoxes_XM3 && Crate_Valuables[LootID].LootType != LootType.Painting && Crate_Valuables[LootID].LootType != LootType.JewelStoreCabinet && Crate_Valuables[LootID].LootType != LootType.CashDryer && Crate_Valuables[LootID].LootType != LootType.GlassCut_Cabinet) ? Crate_Valuables[LootID].Loot.GetOffsetPosition(new Vector3(0f, 1f, -0.3f)) : ((Crate_Valuables[LootID].Loot.Model.Hash != Function.Call<int>(Hash.GET_HASH_KEY, "des_jewel_cab4_start")) ? Crate_Valuables[LootID].Loot.GetOffsetPosition(new Vector3(0f, -1f, -0.3f)) : Crate_Valuables[LootID].Loot.GetOffsetPosition(new Vector3(1f, 0f, -0.3f))));
			if (Crate_Valuables[LootID].LootType == LootType.SafetyDepositBoxes || Crate_Valuables[LootID].LootType == LootType.SafetyDepositBoxes_XM3)
			{
				vector = Crate_Valuables[LootID].Loot.GetOffsetPosition(new Vector3(0f, -1f, -0.3f));
			}
			return vector;
		}

		public static void HudandRadar(bool Hud, bool Radar)
		{
			Function.Call(Hash.DISPLAY_HUD, Hud);
			Function.Call(Hash.DISPLAY_RADAR, Hud);
		}

		public static void DoPlasmaCutter(Lootable Loot)
		{
			if (CAN_DRAW_HEAT)
			{
				float num = 180f + 45f * barperc;
				if (num > 255f)
				{
					num = 255f;
				}
				float num2 = 180f - 150f * barperc;
				if (num2 <= 0f)
				{
					num2 = 0f;
				}
				float num3 = 180f - 150f * barperc;
				if (num3 <= 0f)
				{
					num3 = 0f;
				}
				UI.DrawProgressBar("HEAT:", barperc, (int)num, (int)num2, (int)num3, 255);
			}
			if (Cutting)
			{
				barperc += 0.002f;
				if (barperc > 1f)
				{
					barperc = 1f;
				}
			}
			if (!Cutting)
			{
				barperc -= 0.01f;
				if (barperc < 0f)
				{
					barperc = 0f;
				}
			}
			switch (CutIndex)
			{
			case 0:
				if (Loot.Loot != null)
				{
					LoadDict("anim@scripted@heist@ig16_glass_cut@male@");
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_H4_anims_glass_cutter_Sounds", -1, false);
					Function.Call(Hash.RELEASE_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01", -1, false);
					CutIndex = 1;
				}
				break;
			case 1:
			{
				if (PlasCam == null)
				{
					PlasCam = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_ANIMATED_CAMERA", 0);
				}
				Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player.Character, true);
				Vector3 position = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@heist@ig16_glass_cut@male@", "enter", Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, 0f, 0f, Loot.Loot.Rotation.Z, 0f, 2);
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@heist@ig16_glass_cut@male@", "enter", Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, 0f, 0f, Loot.Loot.Rotation.Z, 0f, 2);
				Game.Player.Character.Task.SlideTo(position, vector.Z);
				Script.Wait(2000);
				HudandRadar(false, false);
				SetBagOff();
				MoneyProp = World.CreateProp("h4_prop_h4_cutter_01a", new Vector3(Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z - 20f), false, false);
				Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, MoneyProp, Game.Player.Character, Function.Call<int>(Hash.GET_PED_BONE_INDEX, Game.Player.Character, 28422), 0f, 0f, 0f, 0f, 0f, 0f, false, false, false, false, 0, true);
				bagProp = World.CreateProp("h4_p_h4_m_bag_var22_arm_s", new Vector3(Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z - 20f), false, false);
				CutEnter = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, Loot.Loot.Rotation.X, Loot.Loot.Rotation.Y, Loot.Loot.Rotation.Z);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CutEnter, "anim@scripted@heist@ig16_glass_cut@male@", "enter", 1f, -1f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CutEnter, "enter_cutter", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CutEnter, "enter_bag", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, CutEnter, "enter_cam", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				World.RenderingCamera = PlasCam;
				CutIndex = 2;
				break;
			}
			case 2:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CutEnter) > 0.999f)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_H4_anims_glass_cutter_Sounds", -1, false);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01", -1, false);
					Function.Call(Hash.DETACH_ENTITY, MoneyProp, false, false);
					Game.Player.Character.CanSwitchWeapons = false;
					CutIndex = 9;
				}
				break;
			case 9:
				DisplayHelpTextThisFrame("Use ~INPUT_ATTACK~ to cut through the glass. Be careful or the cutter will overheat.", 1);
				if (Game.IsControlJustPressed(Control.Attack) & (Game.GameTime > TapTimer))
				{
					Audio.StopSound(SoundId);
					Audio.ReleaseSound(SoundId);
					SoundId = 0;
					CAN_DRAW_HEAT = true;
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
					PlasEffect = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE, "scr_ih_fin_glass_cutter_cut", MoneyProp, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, MoneyProp, "Tip_01"));
					Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, PlasEffect, "heat", 10f, false);
					CutLop = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, Loot.Loot.Rotation.X, Loot.Loot.Rotation.Y, Loot.Loot.Rotation.Z);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CutLop, "anim@scripted@heist@ig16_glass_cut@male@", "cutting_loop", 1f, -1f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CutLop, "cutting_loop_cutter", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Loot.Loot, CutLop, "cutting_loop_glass_display", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CutLop, "cutting_loop_bag", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, CutLop, "cutting_loop_cam", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, CutLop, PlasSpeed);
					TapTimer = Game.GameTime + 700;
					PlasSpeed += 0.35f;
					Cutting = true;
					PlasCheck = 1;
				}
				if (PlasCheck == 1 && SoundId == 0)
				{
					SoundId = Function.Call<int>(Hash.GET_SOUND_ID);
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, SoundId, "StartCutting", MoneyProp, "DLC_H4_anims_glass_cutter_Sounds", true, 80);
					PlasCheck = 2;
				}
				if (PlasCheck == 2)
				{
					if (!Game.IsControlJustReleased(Control.Attack) & (barperc > 0.999f))
					{
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PlasEffect);
						Audio.StopSound(SoundId);
						Audio.ReleaseSound(SoundId);
						SoundId = 0;
						CAN_DRAW_HEAT = false;
						Cutting = false;
						CutIndex = 8;
					}
					if (Game.IsControlJustReleased(Control.Attack))
					{
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PlasEffect);
						Audio.StopSound(SoundId);
						Audio.ReleaseSound(SoundId);
						SoundId = 0;
						Cutting = false;
						PlasCheck = 0;
					}
				}
				if ((Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CutLop) > 0.999f) & (PlasCheck > 0))
				{
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PlasEffect);
					Audio.StopSound(SoundId);
					Audio.ReleaseSound(SoundId);
					SoundId = 0;
					CAN_DRAW_HEAT = false;
					Cutting = false;
					Loot.Loot.IsVisible = false;
					Loot.FakeCabinet = World.CreateProp("h4_prop_h4_glass_disp_01b", Loot.Loot.Position, Loot.Loot.Rotation, false, false);
					CutIndex = 6;
				}
				break;
			case 5:
				CutIdle = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, Loot.Loot.Rotation.X, Loot.Loot.Rotation.Y, Loot.Loot.Rotation.Z);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CutIdle, "anim@scripted@heist@ig16_glass_cut@male@", "idle", 1f, -1f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CutIdle, "idle_cutter", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CutIdle, "idle_bag", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, CutIdle, "idle_cam", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_LOOPED, CutIdle, true);
				CutIndex = 9;
				break;
			case 6:
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PlasEffect);
				Function.Call(Hash.STOP_SOUND, SoundId);
				CutLev = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, Loot.Loot.Rotation.X, Loot.Loot.Rotation.Y, Loot.Loot.Rotation.Z);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CutLev, "anim@scripted@heist@ig16_glass_cut@male@", "success", 1f, -1f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CutLev, "success_cutter", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, CutLev, "success_bag", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, CutLev, "success_cam", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				CutIndex = 7;
				break;
			case 7:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CutLev) > 0.5f)
				{
					foreach (Prop item in Loot.SecondaryLoot)
					{
						if (item != null)
						{
							item.Delete();
						}
					}
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CutLev) > 0.999f)
				{
					AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 5f), (int)(Heist_Average_Pay / 15f));
					CutIndex = 14;
				}
				break;
			case 8:
				PlasCheck = 0;
				Game.Player.CanControlCharacter = false;
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PlasEffect);
				Function.Call(Hash.STOP_SOUND, -1);
				Function.Call(Hash.RELEASE_SOUND_ID, -1);
				Function.Call<int>(Hash.PLAY_SOUND_FROM_ENTITY, -1, "Overheated", Game.Player.Character, "DLC_H4_anims_glass_cutter_Sounds", 0, 1);
				Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ih_fin");
				Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ih_fin");
				PlasEffect = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "scr_ih_fin_glass_cutter_overheat", MoneyProp, 0f, 0f, 0f, 0f, 0f, 0f, 1065353216, 0, 0, 0, 1065353216, 1065353216, 1065353216, 0);
				Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, PlasEffect, "heat", 0f, 0);
				CutHeat = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, Loot.Loot.Rotation.X, Loot.Loot.Rotation.Y, Loot.Loot.Rotation.Z);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CutHeat, "anim@scripted@heist@ig16_glass_cut@male@", "overheat_react_01", 1f, -1f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CutHeat, "overheat_react_01_cutter", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, CutHeat, "overheat_react_01_cam", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				CutIndex = 11;
				break;
			case 11:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CutHeat) > 0.999f)
				{
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, PlasEffect);
					Game.Player.CanControlCharacter = true;
					CutIndex = 9;
				}
				break;
			case 13:
				CutExit = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Loot.Loot.Position.X, Loot.Loot.Position.Y, Loot.Loot.Position.Z, Loot.Loot.Rotation.X, Loot.Loot.Rotation.Y, Loot.Loot.Rotation.Z);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, CutExit, "anim@scripted@heist@ig16_glass_cut@male@", "exit", 1f, -1f, 3341, 16, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, MoneyProp, CutExit, "exit_cutter", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, CutExit, "exit_cam", "anim@scripted@heist@ig16_glass_cut@male@", 1f, -1f, 0, 1148846080);
				CutIndex = 15;
				break;
			case 14:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CutLev) > 0.999f)
				{
					if (bagProp != null)
					{
						bagProp.Delete();
						bagProp = null;
					}
					HudandRadar(true, true);
					World.RenderingCamera = null;
					Game.Player.Character.CanSwitchWeapons = true;
					Game.Player.CanControlCharacter = true;
					Game.Player.Character.Task.ClearAll();
					CutIndex = -1;
					Loot.IsRobbed = true;
					PlayingAnim = false;
					if (PlasCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						World.RenderingCamera = null;
						PlasCam.Delete();
						PlasCam.IsActive = false;
						PlasCam = null;
					}
					CutGlassCabinet = true;
				}
				break;
			case 15:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, CutExit) > 0.999f)
				{
					if (MoneyProp != null)
					{
						MoneyProp.Delete();
						MoneyProp = null;
					}
					if (bagProp != null)
					{
						bagProp.Delete();
						bagProp = null;
					}
					Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player.Character, false);
					HudandRadar(true, true);
					World.RenderingCamera = null;
					Game.Player.Character.CanSwitchWeapons = true;
					Game.Player.CanControlCharacter = true;
					Game.Player.Character.Task.ClearAll();
					CutIndex = 0;
					CutGlassCabinet = false;
				}
				break;
			case 3:
			case 4:
			case 10:
			case 12:
				break;
			}
		}

		public static void RobAOSCase(Prop Cabinet)
		{
			switch (RobCabinetStage)
			{
			case -1:
				RobCabinetStage = 0;
				break;
			case 0:
			{
				Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player.Character, true);
				Vector3 position3 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "anim@scripted@freemode@ig4_cabinet_smash@male@", "smash_case_idol", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, 0f, 0f, Cabinet.Rotation.Z, 0f, 2);
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "anim@scripted@freemode@ig4_cabinet_smash@male@", "smash_case_idol", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, 0f, 0f, Cabinet.Rotation.Z, 0f, 2);
				Game.Player.Character.Task.SlideTo(position3, vector.Z);
				Script.Wait(2000);
				HudandRadar(false, false);
				SetBagOff();
				LoadDict("anim@scripted@freemode@ig4_cabinet_smash@male@");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_H4_anims_glass_cutter_Sounds", -1, false);
				Function.Call(Hash.RELEASE_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01", -1, false);
				Vector3 offsetPosition = Cabinet.GetOffsetPosition(new Vector3(0f, 0f, 0.85f));
				Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 1f, Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_oegg_01a"), 0, 0, 0);
				if (prop3 != null && prop3.Exists())
				{
					RobCabinetLoot = prop3;
					prop3.Detach();
					AosCabinetType = AOSCabinetType.Egg;
				}
				Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 1f, Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_idol_01a"), 0, 0, 0);
				if (prop4 != null && prop4.Exists())
				{
					RobCabinetLoot = prop4;
					prop4.Detach();
					AosCabinetType = AOSCabinetType.Idol;
				}
				Prop prop5 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 1f, Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_idol_02a"), 0, 0, 0);
				if (prop5 != null && prop5.Exists())
				{
					RobCabinetLoot = prop5;
					prop5.Detach();
					AosCabinetType = AOSCabinetType.Statue;
				}
				Prop prop6 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 1f, Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_cabinet_set_01"), 0, 0, 0);
				if (prop6 != null && prop6.Exists())
				{
					RobCabinetLoot = prop6;
					prop6.Detach();
					AosCabinetType = AOSCabinetType.Jewellery;
				}
				Prop prop7 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 1f, Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_cabinet_set_02"), 0, 0, 0);
				if (prop7 != null && prop7.Exists())
				{
					RobCabinetLoot = prop7;
					prop7.Detach();
					AosCabinetType = AOSCabinetType.Jewellery;
				}
				Prop prop8 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 1f, Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_cabinet_set_03"), 0, 0, 0);
				if (prop8 != null && prop8.Exists())
				{
					RobCabinetLoot = prop8;
					prop8.Detach();
					AosCabinetType = AOSCabinetType.Jewellery;
				}
				SetBagOff();
				if (bagProp != null)
				{
					bagProp.Delete();
				}
				bagProp = World.CreateProp("h4_p_h4_m_bag_var22_arm_s", Game.Player.Character.Position, false, false);
				RobCabinetStage = 1;
				Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifle, true);
				break;
			}
			case 1:
				if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, RobCabinetPTFX))
				{
					Function.Call<bool>(Hash.STOP_PARTICLE_FX_LOOPED, RobCabinetPTFX);
				}
				if (PlasCam != null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
					World.RenderingCamera = null;
					PlasCam.Delete();
					PlasCam.IsActive = false;
					PlasCam = null;
				}
				if (PlasCam == null)
				{
					PlasCam = Function.Call<Camera>(Hash.CREATE_CAM, "DEFAULT_ANIMATED_CAMERA", 0);
				}
				if (AosCabinetType == AOSCabinetType.Idol)
				{
					RobCabinetScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, RobCabinetScene, "anim@scripted@freemode@ig4_cabinet_smash@male@", "smash_case_idol", 1f, -1f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobCabinetLoot, RobCabinetScene, "smash_case_idol_idol", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, RobCabinetScene, "smash_case_idol_bag", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, RobCabinetScene, "smash_case_idol_cam", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
				}
				if (AosCabinetType == AOSCabinetType.Egg)
				{
					RobCabinetScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, RobCabinetScene, "anim@scripted@freemode@ig4_cabinet_smash@male@", "smash_case_egg", 1f, -1f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobCabinetLoot, RobCabinetScene, "smash_case_egg_egg", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, RobCabinetScene, "smash_case_egg_bag", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, RobCabinetScene, "smash_case_egg_cam", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
				}
				if (AosCabinetType == AOSCabinetType.Statue)
				{
					RobCabinetScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, RobCabinetScene, "anim@scripted@freemode@ig4_cabinet_smash@male@", "smash_case_statue", 1f, -1f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, RobCabinetLoot, RobCabinetScene, "smash_case_statue_idol", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, RobCabinetScene, "smash_case_statue_bag", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, RobCabinetScene, "smash_case_statue_cam", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
				}
				if (AosCabinetType == AOSCabinetType.Jewellery)
				{
					RobCabinetScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, Cabinet.Rotation.X, Cabinet.Rotation.Y, Cabinet.Rotation.Z);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, RobCabinetScene, "anim@scripted@freemode@ig4_cabinet_smash@male@", "smash_case_unarmed", 1f, -1f, 3341, 16, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, bagProp, RobCabinetScene, "smash_case_unarmed_bag", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
					Function.Call(Hash.PLAY_SYNCHRONIZED_CAM_ANIM, PlasCam, RobCabinetScene, "smash_case_unarmed_cam", "anim@scripted@freemode@ig4_cabinet_smash@male@", 1f, -1f, 0, 1148846080);
				}
				World.RenderingCamera = PlasCam;
				RobCabinetStage = 2;
				Game.Player.Character.Weapons.Select(WeaponHash.CarbineRifle, true);
				break;
			case 2:
			{
				float num = 0.3f;
				if (AosCabinetType == AOSCabinetType.Jewellery)
				{
					num = 0.3f;
				}
				if (AosCabinetType == AOSCabinetType.Egg)
				{
					num = 0.135f;
				}
				if (AosCabinetType == AOSCabinetType.Idol)
				{
					num = 0.135f;
				}
				if (AosCabinetType == AOSCabinetType.Statue)
				{
					num = 0.135f;
				}
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobCabinetScene) > num)
				{
					while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_sv_cbr"))
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_sv_cbr");
						Script.Wait(0);
					}
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_sv_cbr");
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_sv_cbr");
					RobCabinetPTFX = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "scr_sv_cbr_glass_smash", Cabinet, 0, 0, 0, 0, 0, 0, 1f, false, false, false);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Cabinet.Position.X, Cabinet.Position.Y, Cabinet.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed, true);
					Notification.Show("Play Smash Glass");
					if (Cabinet.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_CokeCabinet_01a"))
					{
						Vector3 position = Cabinet.Position;
						Vector3 rotation = Cabinet.Rotation;
						Prop prop = World.CreateProp(RequestModel("m24_2_prop_m42_cokecabinetbrk_01a"), position, rotation, false, false);
						prop.IsCollisionEnabled = false;
						prop.IsPositionFrozen = true;
						Cabinet.Opacity = 0;
						prop.MarkAsNoLongerNeeded();
					}
					if (Cabinet.Model.Hash == Function.Call<int>(Hash.GET_HASH_KEY, "m24_2_prop_m42_CokeCabinet_02a"))
					{
						Vector3 position2 = Cabinet.Position;
						Vector3 rotation2 = Cabinet.Rotation;
						Prop prop2 = World.CreateProp(RequestModel("m24_2_prop_m42_cokecabinetbrk_02a"), position2, rotation2, false, false);
						prop2.IsCollisionEnabled = false;
						prop2.IsPositionFrozen = true;
						Cabinet.Opacity = 0;
						prop2.MarkAsNoLongerNeeded();
					}
					RobCabinetStage = 3;
				}
				break;
			}
			case 3:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, RobCabinetScene) > 0.999f)
				{
					if (AosCabinetType == AOSCabinetType.Jewellery)
					{
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 10f), (int)(Heist_Average_Pay / 35f));
					}
					if (AosCabinetType == AOSCabinetType.Egg)
					{
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 2f), (int)(Heist_Average_Pay / 10f));
					}
					if (AosCabinetType == AOSCabinetType.Idol)
					{
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 3f), (int)(Heist_Average_Pay / 12f));
					}
					if (AosCabinetType == AOSCabinetType.Statue)
					{
						AddCash(Game.Player.Character, (int)(Heist_Average_Pay / 5f), (int)(Heist_Average_Pay / 15f));
					}
					PlayingAnim = false;
					Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player.Character, false);
					if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, RobCabinetPTFX))
					{
						Function.Call<bool>(Hash.STOP_PARTICLE_FX_LOOPED, RobCabinetPTFX);
					}
					if (PlasCam != null)
					{
						Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 0, 3000, 1, 0);
						World.RenderingCamera = null;
						PlasCam.Delete();
						PlasCam.IsActive = false;
						PlasCam = null;
					}
					if (RobCabinetLoot != null)
					{
						RobCabinetLoot.Delete();
						RobCabinetLoot = null;
					}
					if (bagProp != null)
					{
						bagProp.Delete();
						bagProp = null;
					}
					Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player.Character, false);
					HudandRadar(true, true);
					World.RenderingCamera = null;
					Game.Player.Character.CanSwitchWeapons = true;
					Game.Player.CanControlCharacter = true;
					Game.Player.Character.Task.ClearAll();
					RobCabinetStage = -1;
				}
				break;
			}
		}

		public void onTick(object sender, EventArgs e)
		{
			if (Cannot_Steal_Loot || Crate_Valuables.Count == 0 || Crate_Valuables.Count == 0)
			{
				return;
			}
			if (ShowChevronMarkersOnLoot && Crate_Valuables.Count > 0)
			{
				for (int i = 0; i < Crate_Valuables.Count; i++)
				{
					if (!Crate_Valuables[i].IsRobbed && GetDistance_VDIST2(Game.Player.Character.Position, Crate_Valuables[i].Position, "<", 5f) && ShowLootTypeText)
					{
						Vector2 vector = World3DToScreen2d2(Crate_Valuables[i].Loot.GetOffsetPosition(new Vector3(0f, 0f, 0.75f)));
					}
				}
			}
			if (Crate_Valuables.Count > 0 && Game.GameTime >= CheckLootLeft)
			{
				CheckLootLeft = Game.GameTime + 1500;
				ValuableLootLeft = Crate_Valuables.Count;
				for (int j = 0; j < Crate_Valuables.Count; j++)
				{
					Vector3 position = Crate_Valuables[j].Position;
					if (!Crate_Valuables[j].IsRobbed)
					{
						break;
					}
					if (Crate_Valuables[j].IsRobbed)
					{
						ValuableLootLeft--;
						Crate_Valuables[j].LootBlip.Alpha = 0;
					}
				}
			}
			if (Crate_Valuables.Count != 0 || ValuableLootLeft != 0 || RobberARobbing || RobberBRobbing || RobberCRobbing)
			{
				CreatedCrates = true;
				bool flag = false;
				VaultLooting();
			}
		}
	}
}
