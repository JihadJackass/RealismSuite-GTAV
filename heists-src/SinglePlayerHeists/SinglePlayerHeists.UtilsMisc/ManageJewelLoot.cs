using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class ManageJewelLoot : Script
	{
		private bool checkbool;

		public static int checkint;

		private int jewelsmashint;

		private int jewelsmashint2;

		private int jewelsmashint3;

		private int jewelsmashint4;

		private int avicci2;

		private int extra;

		private int extra2;

		private int smashparticlefx;

		private Prop brokenjewelglass;

		private Prop glass1;

		private Prop glass2;

		private Prop glass3;

		private Prop glass4;

		private Prop glassroot;

		private Prop glassrootb;

		private Vector3 jewelsmashpos;

		private float jewelsmashrot;

		private int unstucktimer;

		private WeaponHash guns;

		private List<Prop> jewelbrokenglasslist = new List<Prop>();

		private List<Prop> jewelrootlist = new List<Prop>();

		private List<Prop> jewelrootBlist = new List<Prop>();

		private Camera jewelcam1;

		private void OnTick(object sender, EventArgs e)
		{
		}

		private void jewelcabsmash(Prop glass)
		{
			switch (jewelsmashint)
			{
			case 0:
			{
				World.DrawMarker(MarkerType.Cylinder, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				if (!(Game.Player.Character.Position.DistanceTo(new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z)) < 1f))
				{
					break;
				}
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to smash the cabinet");
				if (!Game.IsControlJustPressed(Control.Context))
				{
					break;
				}
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
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-617.937f, -230.731f, 37.0856f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-617.937f, -230.731f, 37.0856f);
					jewelsmashrot = -54f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-620.055f, -227.817f, 37.0856f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-620.055f, -227.817f, 37.0856f);
					jewelsmashrot = -54f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-624.293f, -227.831f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-624.293f, -227.831f, 37.0946f);
					jewelsmashrot = -143.511f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-626.894f, -238.2f, 37.0856f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-626.894f, -238.2f, 37.0856f);
					jewelsmashrot = -144f;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Game.Player.Character, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
				unstucktimer = Game.GameTime + 2000;
				jewelsmashint = 1;
				break;
			}
			case 1:
				disablewalkingvoid();
				if (!Function.Call<bool>(Hash.IS_PED_WALKING, Game.Player.Character) && Game.GameTime > unstucktimer)
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
					brokenjewelglass = GlobalHelper.CreateProp(model, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
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
					glassroot = GlobalHelper.CreateProp(model2, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
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
					glassrootb = GlobalHelper.CreateProp(model3, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassrootb.IsPositionFrozen = true;
					glassrootb.IsVisible = false;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				avicci2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, avicci2, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
				Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				jewelsmashint = 20;
				break;
			}
			case 20:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, avicci2) > 0.2f)
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
						smashparticlefx = GlobalHelper.CreatePTFX_LoopedAtCoord("scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
					}
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glass.IsVisible = false;
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
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, avicci2) > 0.99f)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra, 0f);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra2, 0f);
					glass.IsVisible = false;
					brokenjewelglass.IsVisible = true;
					glassroot.IsVisible = false;
					glassrootb.IsVisible = false;
					brokenjewelglass.MarkAsNoLongerNeeded();
					glassrootb.MarkAsNoLongerNeeded();
					glassroot.MarkAsNoLongerNeeded();
					jewelbrokenglasslist.Add(brokenjewelglass);
					jewelrootlist.Add(glassroot);
					jewelrootBlist.Add(glassrootb);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					ManageRobberyAnims.AddCash(Game.Player.Character, (int)(ManageRobberyAnims.Heist_Average_Pay / 80f), (int)(ManageRobberyAnims.Heist_Average_Pay / 120f));
					glass.Delete();
					Game.Player.Character.Task.ClearAll();
					Script.Wait(50);
					Game.Player.Character.Task.PlayAnimation("missheist_jewel", "smash_case_f_end", 4f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary);
					Script.Wait(2000);
					jewelsmashint = 0;
				}
				break;
			}
		}

		private void jewelcabsmash2(Prop glass)
		{
			switch (jewelsmashint2)
			{
			case 0:
			{
				World.DrawMarker(MarkerType.Cylinder, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				if (!(Game.Player.Character.Position.DistanceTo(new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z)) < 1f))
				{
					break;
				}
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to smash the cabinet");
				if (!Game.IsControlJustPressed(Control.Context))
				{
					break;
				}
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
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Game.Player.Character, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
				unstucktimer = Game.GameTime + 2000;
				jewelsmashint2 = 1;
				break;
			}
			case 1:
				disablewalkingvoid();
				if (!Function.Call<bool>(Hash.IS_PED_WALKING, Game.Player.Character) && Game.GameTime > unstucktimer)
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
					brokenjewelglass = GlobalHelper.CreateProp(model, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
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
					glassroot = GlobalHelper.CreateProp(model2, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
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
					glassrootb = GlobalHelper.CreateProp(model3, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassrootb.IsPositionFrozen = true;
					glassrootb.IsVisible = false;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				avicci2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, avicci2, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
				Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				jewelsmashint2 = 20;
				break;
			}
			case 20:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, avicci2) > 0.2f)
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
						smashparticlefx = GlobalHelper.CreatePTFX_LoopedAtCoord("scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
					}
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glass.IsVisible = false;
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
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, avicci2) > 0.99f)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra, 0f);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra2, 0f);
					glass.IsVisible = false;
					brokenjewelglass.IsVisible = true;
					glassroot.IsVisible = false;
					glassrootb.IsVisible = false;
					brokenjewelglass.MarkAsNoLongerNeeded();
					glassrootb.MarkAsNoLongerNeeded();
					glassroot.MarkAsNoLongerNeeded();
					jewelbrokenglasslist.Add(brokenjewelglass);
					jewelrootlist.Add(glassroot);
					jewelrootBlist.Add(glassrootb);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					ManageRobberyAnims.AddCash(Game.Player.Character, (int)(ManageRobberyAnims.Heist_Average_Pay / 80f), (int)(ManageRobberyAnims.Heist_Average_Pay / 120f));
					glass.Delete();
					Game.Player.Character.Task.ClearAll();
					Script.Wait(50);
					Game.Player.Character.Task.PlayAnimation("missheist_jewel", "smash_case_f_end", 4f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary);
					Script.Wait(2000);
					jewelsmashint2 = 0;
				}
				break;
			}
		}

		private void jewelcabsmash3(Prop glass)
		{
			switch (jewelsmashint3)
			{
			case 0:
			{
				World.DrawMarker(MarkerType.Cylinder, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				if (!(Game.Player.Character.Position.DistanceTo(new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z)) < 1f))
				{
					break;
				}
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to smash the cabinet");
				if (!Game.IsControlJustPressed(Control.Context))
				{
					break;
				}
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
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-625.57f, -234.962f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-625.57f, -234.962f, 37.0946f);
					jewelsmashrot = 36f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-624.939f, -231.247f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-624.939f, -231.247f, 37.0946f);
					jewelsmashrot = -54.13f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-620.184f, -233.729f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-620.184f, -233.729f, 37.0946f);
					jewelsmashrot = 36.398f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-623.688f, -227.437f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-623.688f, -227.437f, 37.0946f);
					jewelsmashrot = 36f;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_b", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Game.Player.Character, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
				unstucktimer = Game.GameTime + 2000;
				jewelsmashint3 = 1;
				break;
			}
			case 1:
				disablewalkingvoid();
				if (!Function.Call<bool>(Hash.IS_PED_WALKING, Game.Player.Character) && Game.GameTime > unstucktimer)
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
					glassroot = GlobalHelper.CreateProp(model, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
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
					glassrootb = GlobalHelper.CreateProp(model2, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 1f), true, false);
					glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassrootb.IsPositionFrozen = true;
					glassrootb.IsVisible = false;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				avicci2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, avicci2, "missheist_jewel", "smash_case_tray_b", 2f, -4f, 1, 0, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
				Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				jewelsmashint3 = 20;
				break;
			}
			case 20:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, avicci2) > 0.05f)
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
						smashparticlefx = GlobalHelper.CreatePTFX_LoopedAtCoord("scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
					}
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glass.IsVisible = false;
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
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, avicci2) > 0.999f)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra, 0f);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra2, 0f);
					glass.IsVisible = false;
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glassrootb.MarkAsNoLongerNeeded();
					glassroot.MarkAsNoLongerNeeded();
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					ManageRobberyAnims.AddCash(Game.Player.Character, (int)(ManageRobberyAnims.Heist_Average_Pay / 80f), (int)(ManageRobberyAnims.Heist_Average_Pay / 120f));
					glass.Delete();
					Game.Player.Character.Task.ClearAll();
					Script.Wait(50);
					Game.Player.Character.Task.PlayAnimation("missheist_jewel", "smash_case_f_end", 4f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary);
					Script.Wait(2000);
					jewelsmashint3 = 0;
				}
				break;
			}
		}

		private void jewelcabsmash4(Prop glass)
		{
			switch (jewelsmashint4)
			{
			case 0:
			{
				World.DrawMarker(MarkerType.Cylinder, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), Color.FromArgb(0, 0, 225));
				if (!(Game.Player.Character.Position.DistanceTo(new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z)) < 1f))
				{
					break;
				}
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to smash the cabinet");
				if (!Game.IsControlJustPressed(Control.Context))
				{
					break;
				}
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
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-628.187f, -233.538f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-628.187f, -233.538f, 37.0946f);
					jewelsmashrot = -144f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-627.136f, -232.775f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-627.136f, -232.775f, 37.0946f);
					jewelsmashrot = -144f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-623.3596f, -233.2296f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-623.3596f, -233.2296f, 37.0946f);
					jewelsmashrot = -52.984f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-619.408f, -230.1969f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-619.408f, -230.1969f, 37.0946f);
					jewelsmashrot = 126.352f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-620.797f, -226.79f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-620.797f, -226.79f, 37.0946f);
					jewelsmashrot = -54f;
				}
				if (Game.Player.Character.Position.DistanceTo(new Vector3(-620.44f, -234.084f, 37.0946f)) < 1.5f)
				{
					jewelsmashpos = new Vector3(-620.44f, -234.084f, 37.0946f);
					jewelsmashrot = -144f;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel"))
				{
					Script.Wait(0);
				}
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, "missheist_jewel", "smash_case_tray_a", jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: Game.Player.Character, argument1: vector.X, argument2: vector.Y, argument3: vector.Z, argument4: 1f, argument5: 3000, argument7: 0f);
				unstucktimer = Game.GameTime + 2000;
				jewelsmashint4 = 1;
				break;
			}
			case 1:
				disablewalkingvoid();
				if (!Function.Call<bool>(Hash.IS_PED_WALKING, Game.Player.Character) && Game.GameTime > unstucktimer)
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
					brokenjewelglass = GlobalHelper.CreateProp(model, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.6f), true, false);
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
					glassroot = GlobalHelper.CreateProp(model2, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
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
					glassrootb = GlobalHelper.CreateProp(model3, new Vector3(glass.Position.X, glass.Position.Y, glass.Position.Z - 0.8f), true, false);
					glassrootb.Rotation = new Vector3(0f, 0f, glass.Rotation.Z);
					glassrootb.IsPositionFrozen = true;
					glassrootb.IsVisible = false;
				}
				Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel");
				avicci2 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				extra = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, jewelsmashpos.X, jewelsmashpos.Y, jewelsmashpos.Z, 0f, 0f, jewelsmashrot, 0f, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, avicci2, "missheist_jewel", "smash_case_tray_a", 2f, -4f, 1, 0, 1148846080, 0);
				Function.Call(Hash.FORCE_ENTITY_AI_AND_ANIMATION_UPDATE, Game.Player.Character);
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
				Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
				jewelsmashint4 = 20;
				break;
			}
			case 20:
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, avicci2) > 0.2f)
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
						smashparticlefx = GlobalHelper.CreatePTFX_LoopedAtCoord("scr_jewel_cab_smash", glassrootb.Position.X, glassrootb.Position.Y, glassrootb.Position.Z + 0.3f, 0f, 0f, glass.Rotation.Z, 1f, 0, 0, 0);
					}
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.LOAD_STREAM, "SMASH_CABINET_PLAYER", "JEWEL_HEIST_SOUNDS");
					Function.Call(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", 1);
					Function.Call(Hash.PLAY_SOUND_FROM_COORD, 0, "SMASH_CABINET_PLAYER", Game.Player.Character.Position.X, Game.Player.Character.Position.Y, Game.Player.Character.Position.Z, "JEWEL_HEIST_SOUNDS", 0, 0, 0);
					glassroot.IsVisible = true;
					glassrootb.IsVisible = true;
					glass.IsVisible = false;
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
				if (Function.Call<float>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, avicci2) > 0.99f)
				{
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra, 0f);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, extra2, 0f);
					glass.IsVisible = false;
					brokenjewelglass.IsVisible = true;
					glassroot.IsVisible = false;
					glassrootb.IsVisible = false;
					brokenjewelglass.MarkAsNoLongerNeeded();
					glassrootb.MarkAsNoLongerNeeded();
					glassroot.MarkAsNoLongerNeeded();
					jewelbrokenglasslist.Add(brokenjewelglass);
					jewelrootlist.Add(glassroot);
					jewelrootBlist.Add(glassrootb);
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, smashparticlefx, false);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_INCREASE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "REMOTE_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
					ManageRobberyAnims.AddCash(Game.Player.Character, (int)(ManageRobberyAnims.Heist_Average_Pay / 80f), (int)(ManageRobberyAnims.Heist_Average_Pay / 120f));
					glass.Delete();
					Game.Player.Character.Task.ClearAll();
					Script.Wait(50);
					Game.Player.Character.Task.PlayAnimation("missheist_jewel", "smash_case_f_end", 4f, 500, AnimationFlags.UpperBodyOnly | AnimationFlags.Secondary);
					Script.Wait(2000);
					jewelsmashint4 = 0;
				}
				break;
			}
		}

		private void Class1_Aborted(object sender, EventArgs e)
		{
			if (jewelrootBlist.Count <= 0)
			{
				foreach (Prop item in jewelrootBlist.ToList())
				{
					if (item != null)
					{
						item.Delete();
						jewelrootBlist.Remove(item);
					}
				}
			}
			if (jewelrootlist.Count <= 0)
			{
				foreach (Prop item2 in jewelrootlist.ToList())
				{
					if (item2 != null)
					{
						item2.Delete();
						jewelrootlist.Remove(item2);
					}
				}
			}
			if (jewelbrokenglasslist.Count <= 0)
			{
				foreach (Prop item3 in jewelbrokenglasslist.ToList())
				{
					if (item3 != null)
					{
						item3.Delete();
						jewelbrokenglasslist.Remove(item3);
					}
				}
			}
			Game.Player.Character.Task.ClearAll();
			Function.Call(Hash.SET_EVERYONE_IGNORE_PLAYER, Game.Player, false);
		}

		public static void DisplayHelpTextThisFrame(string text)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, -1);
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
	}
}
