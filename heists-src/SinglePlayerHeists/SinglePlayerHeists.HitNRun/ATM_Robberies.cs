using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using HKHModHelperNew;
using SinglePlayerHeists.UtilsMisc;

namespace SinglePlayerHeists.HitNRun
{
	internal class ATM_Robberies : Script
	{
		public int SawTimer;

		public Prop Saw;

		public Prop Bag;

		public bool UsingSaw;

		public float SceneRate;

		public int Scene1;

		public int Sound;

		public bool PlayingPTFX;

		public int ParticleFx;

		public static bool Enable_ATM_Robberies;

		public static int MinSpawnAmt = 7;

		public static int MaxSpawnAmt = 65;

		public static int MinMoneyAmt = 100;

		public static int MaxMoneyAmt = 5000;

		public static int Ammount;

		public List<Prop> Money = new List<Prop>();

		public float TotalRobbed;

		public Vector3 MoneySpawnPos;

		public ATM_Robberies()
		{
			base.Tick += onTick;
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

		public Model RequestModel(string Name)
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

		public Model RequestModel(PedHash Name)
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

		public Model RequestModel(VehicleHash Name)
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

		public Model RequestModel(int Name)
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

		private void onTick(object sender, EventArgs e)
		{
			if (!Enable_ATM_Robberies)
			{
				return;
			}
			if (SPH_Debug.ShowRunningClassDebug)
			{
				Class1.drawText("ATM Robberies Running", 0.8f, 0.09f, 0.5f, 255, 255, 255);
			}
			if (Game.Player.WantedLevel == 0)
			{
				Vector3 position = Game.Player.Character.Position;
				Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 20f, -1364697528, 0, 0, 0);
				Prop prop2 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 20f, -1126237515, 0, 0, 0);
				Prop prop3 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 20f, 506770882, 0, 0, 0);
				Prop prop4 = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 20f, -870868698, 0, 0, 0);
				if (prop != null)
				{
					Prop prop5 = prop;
					if (World.GetDistance(prop5.Position, Game.Player.Character.Position) < 4f)
					{
						if (SawTimer == 0)
						{
							Vector3 offsetPosition = prop5.GetOffsetPosition(new Vector3(0f, -1f, 0f));
							offsetPosition = new Vector3(offsetPosition.X, offsetPosition.Y, World.GetGroundHeight(offsetPosition));
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_DUCK~ to Rob ATM\nPress ~INPUT_CONTEXT~ Use ATM ");
						}
						if (Game.IsControlJustPressed(Control.VehicleDuck))
						{
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
							Game.Player.Character.Position = prop5.GetOffsetPosition(new Vector3(0f, 2f, 0f));
							Game.Player.Character.Heading = prop5.Heading;
							if (Saw != null)
							{
								Saw.Delete();
							}
							if (Bag != null)
							{
								Bag.Delete();
							}
							while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
							{
								Script.Wait(100);
							}
							Vector3 rotation = Game.Player.Character.Rotation;
							Saw = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_grinder_01a"), Game.Player.Character.Position, new Vector3(0f, 0f, 0f), false, false);
							Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
							Saw.IsCollisionEnabled = false;
							Bag.IsCollisionEnabled = false;
							Game.Player.Character.SetNoCollision(Saw, true);
							Game.Player.Character.SetNoCollision(Bag, true);
							string text = "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@";
							LoadDict(text);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position.X, position.Y, position.Z - 1f, 0.0, 0.0, rotation.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, position.X, position.Y, position.Z - 1f, 0.0, 0.0, rotation.Z, true);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, Scene1, "action_bag", LoadDict(text), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Saw, Scene1, "action_angle_grinder", LoadDict(text), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, text, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "action", LoadDict(text), 1000.0, -8f, -8f, rotation.Z + 180f, 1148846080, 0);
							SawTimer = 1;
							UsingSaw = true;
							SceneRate = 1f;
						}
					}
				}
				if (prop2 != null)
				{
					Prop prop6 = prop2;
					if (World.GetDistance(prop6.Position, Game.Player.Character.Position) < 4f)
					{
						if (SawTimer == 0)
						{
							Vector3 offsetPosition2 = prop6.GetOffsetPosition(new Vector3(0f, -1f, 0f));
							offsetPosition2 = new Vector3(offsetPosition2.X, offsetPosition2.Y, World.GetGroundHeight(offsetPosition2));
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_DUCK~ to Rob ATM\nPress ~INPUT_CONTEXT~ Use ATM ");
						}
						if (Game.IsControlJustPressed(Control.VehicleDuck))
						{
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
							Game.Player.Character.Position = prop6.GetOffsetPosition(new Vector3(0f, 2f, 0f));
							Game.Player.Character.Heading = prop6.Heading;
							if (Saw != null)
							{
								Saw.Delete();
							}
							if (Bag != null)
							{
								Bag.Delete();
							}
							while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
							{
								Script.Wait(100);
							}
							Vector3 rotation2 = Game.Player.Character.Rotation;
							Saw = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_grinder_01a"), Game.Player.Character.Position, new Vector3(0f, 0f, 0f), false, false);
							Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
							Saw.IsCollisionEnabled = false;
							Bag.IsCollisionEnabled = false;
							Game.Player.Character.SetNoCollision(Saw, true);
							Game.Player.Character.SetNoCollision(Bag, true);
							string text2 = "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@";
							LoadDict(text2);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position.X, position.Y, position.Z - 1f, 0.0, 0.0, rotation2.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, position.X, position.Y, position.Z - 1f, 0.0, 0.0, rotation2.Z, true);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, Scene1, "action_bag", LoadDict(text2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Saw, Scene1, "action_angle_grinder", LoadDict(text2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, text2, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "action", LoadDict(text2), 1000.0, -8f, -8f, rotation2.Z + 180f, 1148846080, 0);
							SawTimer = 1;
							UsingSaw = true;
							SceneRate = 1f;
						}
					}
				}
				if (prop3 != null)
				{
					Prop prop7 = prop3;
					if (World.GetDistance(prop7.Position, Game.Player.Character.Position) < 4f)
					{
						if (SawTimer == 0)
						{
							Vector3 offsetPosition3 = prop7.GetOffsetPosition(new Vector3(0f, -1f, 0f));
							offsetPosition3 = new Vector3(offsetPosition3.X, offsetPosition3.Y, World.GetGroundHeight(offsetPosition3));
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_DUCK~ to Rob ATM\nPress ~INPUT_CONTEXT~ Use ATM ");
						}
						if (Game.IsControlJustPressed(Control.VehicleDuck))
						{
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
							Game.Player.Character.Position = prop7.GetOffsetPosition(new Vector3(0f, 2f, 0f));
							Game.Player.Character.Heading = prop7.Heading;
							if (Saw != null)
							{
								Saw.Delete();
							}
							if (Bag != null)
							{
								Bag.Delete();
							}
							while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
							{
								Script.Wait(100);
							}
							Vector3 rotation3 = Game.Player.Character.Rotation;
							Saw = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_grinder_01a"), Game.Player.Character.Position, new Vector3(0f, 0f, 0f), false, false);
							Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
							Saw.IsCollisionEnabled = false;
							Bag.IsCollisionEnabled = false;
							Game.Player.Character.SetNoCollision(Saw, true);
							Game.Player.Character.SetNoCollision(Bag, true);
							string text3 = "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@";
							LoadDict(text3);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position.X, position.Y, position.Z - 1f, 0.0, 0.0, rotation3.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, position.X, position.Y, position.Z - 1f, 0.0, 0.0, rotation3.Z, true);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, Scene1, "action_bag", LoadDict(text3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Saw, Scene1, "action_angle_grinder", LoadDict(text3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, text3, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "action", LoadDict(text3), 1000.0, -8f, -8f, rotation3.Z + 180f, 1148846080, 0);
							SawTimer = 1;
							UsingSaw = true;
							SceneRate = 1f;
						}
					}
				}
				if (prop4 != null)
				{
					Prop prop8 = prop4;
					if (World.GetDistance(prop8.Position, Game.Player.Character.Position) < 4f)
					{
						if (SawTimer == 0)
						{
							Vector3 offsetPosition4 = prop8.GetOffsetPosition(new Vector3(0f, -1f, 0f));
							offsetPosition4 = new Vector3(offsetPosition4.X, offsetPosition4.Y, World.GetGroundHeight(offsetPosition4));
							DisplayHelpTextThisFrame("Press ~INPUT_VEH_DUCK~ to Rob ATM\nPress ~INPUT_CONTEXT~ Use ATM ");
						}
						if (Game.IsControlJustPressed(Control.VehicleDuck))
						{
							Ped character4 = Game.Player.Character;
							if (Game.Player.Character.Model == PedHash.Trevor)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 0, 0, 1);
							}
							if (Game.Player.Character.Model == PedHash.Michael)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 0, 0, 1);
							}
							if (Game.Player.Character.Model == PedHash.Franklin)
							{
								Function.Call(Hash.SET_PED_COMPONENT_VARIATION, character4, 9, 0, 0, 1);
							}
							Game.Player.Character.Position = prop8.GetOffsetPosition(new Vector3(0f, 1.5f, 0f));
							Game.Player.Character.Heading = prop8.Heading;
							if (Saw != null)
							{
								Saw.Delete();
							}
							if (Bag != null)
							{
								Bag.Delete();
							}
							while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
							{
								Script.Wait(100);
							}
							Vector3 rotation4 = Game.Player.Character.Rotation;
							Saw = GlobalHelper.CreateProp(RequestModel("tr_prop_tr_grinder_01a"), Game.Player.Character.Position, new Vector3(0f, 0f, 0f), false, false);
							Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
							Saw.IsCollisionEnabled = false;
							Bag.IsCollisionEnabled = false;
							Game.Player.Character.SetNoCollision(Saw, true);
							Game.Player.Character.SetNoCollision(Bag, true);
							string text4 = "anim@scripted@player@mission@tunf_train_ig1_container_p1@male@";
							LoadDict(text4);
							Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, position.X, position.Y, position.Z - 1f, 0.0, 0.0, rotation4.Z, 2);
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, position.X, position.Y, position.Z - 1f, 0.0, 0.0, rotation4.Z, true);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, Scene1, "action_bag", LoadDict(text4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Saw, Scene1, "action_angle_grinder", LoadDict(text4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
							Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, text4, "action", 1000.0, -4.0, 128, 0, 1148846080, 0);
							Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "action", LoadDict(text4), 1000.0, -8f, -8f, rotation4.Z + 180f, 1148846080, 0);
							SawTimer = 1;
							UsingSaw = true;
							SceneRate = 1f;
						}
					}
				}
			}
			if (!UsingSaw)
			{
				return;
			}
			if (SawTimer <= 20)
			{
				int num = Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1);
				if (num >= 1048400991 && num <= 1055937297)
				{
					DisplayHelpTextThisFrame("Hold ~INPUT_ATTACK~ to use the Saw");
					if (Game.IsControlPressed(Control.Attack))
					{
						if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01", false, -1))
						{
							Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLCHEI4_GENERIC_01", false, -1);
						}
						if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", false, -1))
						{
							Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/JWL_HEIST_RAID_GLASS_SMASH", false, -1);
						}
						if (!Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/DLC_HEIST_FLEECA_SOUNDSET", false, -1))
						{
							Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SCRIPT/DLC_HEIST_FLEECA_SOUNDSET", false, -1);
						}
						if (!PlayingPTFX)
						{
							Function.Call(Hash.STOP_SOUND, Sound);
							Sound = GlobalHelper.CreateSoundID();
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, Sound, "Drill", Saw, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
							PlayingPTFX = true;
						}
						if (SceneRate <= 1f)
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_tn_tr");
							Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_tn_tr");
							if (Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_tn_tr"))
							{
								if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, ParticleFx))
								{
									Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, ParticleFx, false);
								}
								if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, ParticleFx))
								{
									ParticleFx = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_tn_tr_angle_grinder_sparks", Saw, 0f, 0.25f, 0f, 0f, 0f, 0f, 1f, false, false, false);
								}
							}
							SceneRate += 0.05f;
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, SceneRate);
						}
						else
						{
							SceneRate = 1f;
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, SceneRate);
						}
					}
					else
					{
						MoneySpawnPos = Saw.Position;
						PlayingPTFX = false;
						if (SceneRate >= 1f)
						{
							SceneRate -= 0.05f;
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, SceneRate);
						}
						else
						{
							Function.Call(Hash.STOP_SOUND, Sound);
							if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, ParticleFx))
							{
								Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, ParticleFx, false);
							}
							SceneRate = 0f;
							Function.Call(Hash.SET_SYNCHRONIZED_SCENE_RATE, Scene1, SceneRate);
						}
					}
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1053311909)
					{
						Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, Scene1, 1052311909);
						SawTimer = 21;
						Vector3 position2 = Game.Player.Character.Position;
						Ped[] nearbyPeds = World.GetNearbyPeds(position2, 20f);
						Ped[] array = nearbyPeds;
						foreach (Ped ped in array)
						{
							if (ped != null && ped != Game.Player.Character)
							{
								ped.Task.ClearAll();
								ped.Task.FleeFrom(Game.Player.Character);
							}
						}
					}
				}
			}
			if (SawTimer == 21)
			{
				Function.Call(Hash.STOP_SOUND, Sound);
				if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, ParticleFx))
				{
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, ParticleFx, false);
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1060353216)
				{
					Vector3 position3 = Game.Player.Character.Position;
					Ped[] nearbyPeds2 = World.GetNearbyPeds(position3, 20f);
					Ped[] array2 = nearbyPeds2;
					foreach (Ped ped2 in array2)
					{
						if (ped2 != null && ped2 != Game.Player.Character)
						{
							ped2.Task.ClearAll();
							ped2.Task.FleeFrom(Game.Player.Character);
						}
					}
					Function.Call(Hash.ADD_SHOCKING_EVENT_AT_POSITION, 0, position3.X, position3.Y, position3.Z, 2000);
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
					if (Bag != null)
					{
						Bag.Delete();
					}
					Game.Player.Character.Task.ClearAllImmediately();
					if (Saw != null)
					{
						Saw.Delete();
					}
					Random random = new Random();
					int num2 = (Ammount = random.Next(MinSpawnAmt, MaxSpawnAmt));
					if (Money.Count > 0)
					{
						Money.Clear();
					}
					for (int k = 0; k < num2; k++)
					{
						Script.Wait(1);
						float num3 = random.Next(MinMoneyAmt, MaxMoneyAmt);
						Prop prop9 = GlobalHelper.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), MoneySpawnPos, true, false);
						prop9.IsPositionFrozen = false;
						prop9.SetNoCollision(Game.Player.Character, false);
						prop9.Velocity = new Vector3(0f, -3f, 0f);
						prop9.MarkAsNoLongerNeeded();
						Money.Add(prop9);
						Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
						Audio.PlaySoundFrontend("LOCAL_PLYR_CASH_COUNTER_COMPLETE", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
						Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
					}
					Game.Player.Character.Task.TurnTo(Money[0].Position);
					Script.Wait(2000);
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
					Bag = GlobalHelper.CreateProp(RequestModel(-679192147), Game.Player.Character.Position, false, false);
					Bag.IsCollisionEnabled = false;
					Vector3 offsetPosition5 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0.2f, 0f));
					Vector3 rotation5 = Game.Player.Character.Rotation;
					string text5 = "anim@heists@money_grab@duffel";
					LoadDict(text5);
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition5.X, offsetPosition5.Y, offsetPosition5.Z - 1f, 0.0, 0.0, rotation5.Z - 180f, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, offsetPosition5.X, offsetPosition5.Y, offsetPosition5.Z - 1f, 0.0, 0.0, rotation5.Z - 180f, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, Scene1, "enter_bag", LoadDict(text5), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, text5, "enter", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "enter", LoadDict(text5), 1000.0, -8f, -8f, rotation5.Z + 180f, 1148846080, 0);
					SawTimer = 22;
				}
			}
			if (SawTimer == 22 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
			{
				Vector3 position4 = Game.Player.Character.Position;
				Ped[] nearbyPeds3 = World.GetNearbyPeds(position4, 20f);
				Ped[] array3 = nearbyPeds3;
				foreach (Ped ped3 in array3)
				{
					if (ped3 != null && ped3 != Game.Player.Character)
					{
						ped3.Task.ClearAll();
						ped3.Task.FleeFrom(Game.Player.Character);
					}
				}
				Saw = GlobalHelper.CreateProp(RequestModel("ch_prop_20dollar_pile_01a"), Game.Player.Character.Position, new Vector3(0f, 0f, 0f), false, false);
				SawTimer = 23;
				Vector3 offsetPosition6 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1.2f, 0f));
				Vector3 rotation6 = Game.Player.Character.Rotation;
				string text6 = "anim@heists@money_grab@duffel";
				LoadDict(text6);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition6.X, offsetPosition6.Y, offsetPosition6.Z - 1f, 0.0, 0.0, rotation6.Z - 180f, 2);
				Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, offsetPosition6.X, offsetPosition6.Y, offsetPosition6.Z - 1f, 0.0, 0.0, rotation6.Z - 180f, true);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, Scene1, "loop_bag", LoadDict(text6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Saw, Scene1, "loop_cash", LoadDict(text6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, text6, "loop", 1000.0, -4.0, 128, 0, 1148846080, 0);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "loop", LoadDict(text6), 1000.0, -8f, -8f, rotation6.Z + 180f, 1148846080, 0);
			}
			if (SawTimer == 23)
			{
				Vector3 position5 = Game.Player.Character.Position;
				Ped[] nearbyPeds4 = World.GetNearbyPeds(position5, 20f);
				Ped[] array4 = nearbyPeds4;
				foreach (Ped ped4 in array4)
				{
					if (ped4 != null && ped4 != Game.Player.Character)
					{
						ped4.Task.ClearAll();
						ped4.Task.FleeFrom(Game.Player.Character);
					}
				}
				Random random2 = new Random();
				for (int n = 0; n < Ammount; n++)
				{
					if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) < 1065353216)
					{
						try
						{
							Money[n].Opacity = 0;
						}
						catch
						{
						}
						Script.Wait(100);
						float num4 = random2.Next(MinMoneyAmt, MaxMoneyAmt);
						Game.Player.Money += (int)num4;
						TotalRobbed += num4;
					}
				}
				if (Money.Count > 0)
				{
					Money.Clear();
				}
				if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
				{
					if (Saw != null)
					{
						Saw.Delete();
					}
					SawTimer = 24;
					Vector3 offsetPosition7 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f));
					Vector3 rotation7 = Game.Player.Character.Rotation;
					string text7 = "anim@heists@money_grab@duffel";
					LoadDict(text7);
					Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition7.X, offsetPosition7.Y, offsetPosition7.Z - 1f, 0.0, 0.0, rotation7.Z - 180f, 2);
					Function.Call(Hash.SET_SYNCHRONIZED_SCENE_ORIGIN, Scene1, offsetPosition7.X, offsetPosition7.Y, offsetPosition7.Z - 1f, 0.0, 0.0, rotation7.Z - 180f, true);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, Scene1, "exit_bag", LoadDict(text7), 1000.0, -8f, -8f, rotation7.Z + 180f, 1148846080, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, text7, "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
					Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "exit", LoadDict(text7), 1000.0, -8f, -8f, rotation7.Z + 180f, 1148846080, 0);
				}
			}
			if (SawTimer != 24 || Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) < 1065353216)
			{
				return;
			}
			Vector3 position6 = Game.Player.Character.Position;
			Ped[] nearbyPeds5 = World.GetNearbyPeds(position6, 20f);
			Ped[] array5 = nearbyPeds5;
			foreach (Ped ped5 in array5)
			{
				if (ped5 != null && ped5 != Game.Player.Character)
				{
					ped5.Task.ClearAll();
					ped5.Task.FleeFrom(Game.Player.Character);
				}
			}
			UsingSaw = false;
			SawTimer = 0;
			if (Saw != null)
			{
				Saw.Delete();
			}
			if (Bag != null)
			{
				Bag.Delete();
			}
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
			Random random3 = new Random();
			if (Game.Player.WantedLevel == 0)
			{
				Game.Player.WantedLevel = random3.Next(1, 4);
			}
			Game.Player.Character.Task.ClearAll();
			HKHModHelperNew.Class1.scaleformDescription = "Single Player Heists ATM Robbery Completed, Pay $" + TotalRobbed.ToString("N");
			HKHModHelperNew.Class1.scaleformTitle = "ATM Robbery Completed";
			HKHModHelperNew.Class1.ScaleformColor = HKHModHelperNew.Class1.ScaleformColour.Green;
			HKHModHelperNew.Class1.ShowScaleform = true;
			HKHModHelperNew.Class1.Passed_Index = 0;
			TotalRobbed = 0f;
		}
	}
}
