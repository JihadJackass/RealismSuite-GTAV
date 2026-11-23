using System;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class DrillingMinigames : Script
	{
		public enum DrillUsing
		{
			None,
			Fleeca,
			CasinoLaser,
			CasinoDrill
		}

		public static bool FleecaDrillActive;

		public static bool firsttime = true;

		public static float drillspeed;

		public static float temperature = 0f;

		public static float discs = 4f;

		public static float fvar0 = 1f;

		public static float depth;

		public static float paramf_12 = 1f;

		public static float DrillPos = 1f;

		public static bool activeCasLaser = false;

		public static bool activeCasDrill = false;

		public static bool frozen = false;

		public static bool pin_one = false;

		public static bool pin_two = false;

		public static bool pin_three = false;

		public static bool pin_four = false;

		public static bool pin_five = false;

		public static bool pin_six = false;

		public static bool pin_seven = false;

		public static bool pin_eight = false;

		public static float NumOfDiscs = 6f;

		public static int Drillstage = 1;

		public static bool PlayDrillFXSound;

		public static bool DrillingComplete;

		public static bool UseCasinoDrill = false;

		public static bool CalledDrillEndViaComplete;

		public bool Intunnel;

		public static int scaleformloading;

		public static bool SoundPlaying;

		public static int SoundPlayingId;

		public static Prop Drill;

		public static bool Isdrilliing;

		public static int DrillFX;

		public static int DrillFXSound;

		public static float Zone;

		public static int FailSound;

		public static float Speed;

		public static float Temp;

		public static float Pos;

		public static int scale;

		public static bool triggerLockA;

		public static bool triggerLockB;

		public static bool triggerLockC;

		public static bool triggerLockD;

		public static int PinSound;

		public static bool DrillDone;

		public static Vector3 DrillLoc;

		public static float DrillHeading;

		public static int SyncScene;

		public static Prop Bag;

		public static DrillUsing CurrentDrill;

		public static int CheckPlaySoundTimer;

		public static bool FleecaDrillFullyDone;

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

		public static int activatescaleCas(string scale)
		{
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scaleformloading))
			{
				return 1;
			}
			return 0;
		}

		public static void DestroyScaleform(string scale)
		{
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, scale);
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
		}

		public static void DestoryDrill()
		{
			if (scaleformloading != -1)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, scaleformloading, "RESET");
				scaleformloading = -1;
			}
			Function.Call(Hash.STOP_SOUND, DrillFXSound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
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
			DrillFXSound = -1;
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
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
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
		}

		public static void EndCasinoDrillorLaser()
		{
			Function.Call(Hash.STOP_SOUND, DrillFXSound);
			Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
			{
				Function.Call(Hash.STOP_SOUND, DrillFXSound);
				Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
			{
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
			{
				Function.Call(Hash.STOP_SOUND, PinSound);
				Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
			}
			CurrentDrill = DrillUsing.None;
			if (scaleformloading != -1)
			{
				Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, scaleformloading, "RESET");
				scaleformloading = -1;
			}
			Function.Call(Hash.STOP_SOUND, DrillFXSound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
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
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
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
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			Vector3 vector = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 1f;
			Script.Wait(1);
			if (Drill != null)
			{
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
			if (Bag != null)
			{
				Bag.Delete();
			}
			Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
			Bag.IsCollisionEnabled = false;
			Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.2f));
			float heading = Game.Player.Character.Heading;
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, heading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SyncScene, "anim@heists@fleeca_bank@drilling", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SyncScene, "exit", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, SyncScene, "bag_exit", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Drill, SyncScene, "exit", LoadDict("anim@heists@fleeca_bank@drilling"), 16f, -16f, 0, 1148846080);
			Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, SyncScene, 0.0);
			Script.Wait(6000);
			Game.Player.Character.Task.ClearAllImmediately();
			if (Drill != null)
			{
				Drill.Delete();
			}
			if (Bag != null)
			{
				Bag.Delete();
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
			{
				Function.Call(Hash.STOP_SOUND, DrillFXSound);
				Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
			{
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
			{
				Function.Call(Hash.STOP_SOUND, PinSound);
				Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
			}
		}

		public unsafe static void StartFleecaDrill()
		{
			Script.Wait(1);
			CurrentDrill = DrillUsing.Fleeca;
			LoadDict("anim@heists@fleeca_bank@drilling");
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
			SoundPlaying = false;
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
			{
				Function.Call(Hash.STOP_SOUND, DrillFXSound);
				Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
			{
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
			{
				Function.Call(Hash.STOP_SOUND, PinSound);
				Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
			}
			FleecaDrillActive = true;
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			if (Isdrilliing)
			{
				DrillScreenStart();
			}
			Pos = 0f;
			Speed = 0f;
			Temp = 0f;
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				int num = scale;
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
				scale = 0;
			}
			scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				Script.Wait(100);
			}
			RunScaleformFunction(scale, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_HOLE_DEPTH", 0.1f, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Zone = 551f;
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			if (Drill != null)
			{
				Drill.Delete();
			}
			if (Bag != null)
			{
				Bag.Delete();
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			if (Bag != null)
			{
				Bag.Delete();
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
			Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.2f));
			float heading = Game.Player.Character.Heading;
			Notification.Show("Origin " + offsetPosition.ToString() + ",H:" + heading);
			Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
			Bag.IsCollisionEnabled = false;
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, heading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SyncScene, "anim@heists@fleeca_bank@drilling", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SyncScene, "intro", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, SyncScene, "bag_intro", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Drill, SyncScene, "intro", LoadDict("anim@heists@fleeca_bank@drilling"), 16f, -16f, 0, 1148846080);
			Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, SyncScene, 0.0);
			DrillFXSound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			Game.Player.Character.IsPositionFrozen = true;
		}

		public static void EndFleecaDrill()
		{
			CurrentDrill = DrillUsing.None;
			LoadDict("anim@heists@fleeca_bank@drilling");
			if (!CalledDrillEndViaComplete)
			{
				DrillDone = false;
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
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				RunScaleformFunction(scale, "RESET");
			}
			SoundPlaying = false;
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
			{
				Function.Call(Hash.STOP_SOUND, DrillFXSound);
				Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
			{
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
			{
				Function.Call(Hash.STOP_SOUND, PinSound);
				Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
			}
			FleecaDrillActive = false;
			triggerLockA = false;
			triggerLockB = false;
			triggerLockC = false;
			triggerLockD = false;
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
				scale = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_INSTANCE, "DRILLING");
				Script.Wait(1);
			}
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scale))
			{
				RunScaleformFunction(scale, "RESET");
			}
			RunScaleformFunction(scale, "SET_SPEED", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_HOLE_DEPTH", 0.1f, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_DRILL_POSITION", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_TEMPERATURE", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
			Zone = 551f;
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			DrillFXSound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
			{
				Script.Wait(100);
			}
			if (Bag != null)
			{
				Bag.Delete();
			}
			if (Isdrilliing)
			{
				DrillScreenStart();
			}
			SoundPlaying = false;
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
			{
				Function.Call(Hash.STOP_SOUND, DrillFXSound);
				Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
			{
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
			{
				Function.Call(Hash.STOP_SOUND, PinSound);
				Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
			}
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			DrillFXSound = GlobalHelper.CreateSoundID();
			PinSound = GlobalHelper.CreateSoundID();
			FailSound = GlobalHelper.CreateSoundID();
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			RunScaleformFunction(scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
			Isdrilliing = false;
			Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.2f));
			float heading = Game.Player.Character.Heading;
			Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
			Bag.IsCollisionEnabled = false;
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, heading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SyncScene, "anim@heists@fleeca_bank@drilling", "exit", 1000.0, -4.0, 128, 0, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SyncScene, "exit", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, SyncScene, "bag_exit", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Drill, SyncScene, "exit", LoadDict("anim@heists@fleeca_bank@drilling"), 16f, -16f, 0, 1148846080);
			Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, SyncScene, 0.0);
			Script.Wait(5000);
			Game.Player.Character.Task.ClearAll();
			Game.Player.Character.Task.ClearAllImmediately();
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, true);
			if (Bag != null)
			{
				Bag.Delete();
				if (Drill != null)
				{
					Drill.Delete();
				}
				Game.Player.Character.IsPositionFrozen = false;
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
			if (CalledDrillEndViaComplete)
			{
				FleecaDrillFullyDone = true;
				DrillDone = false;
			}
			Game.Player.Character.IsPositionFrozen = false;
		}

		public static void StartCasinoDrill()
		{
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
			{
				Function.Call(Hash.STOP_SOUND, DrillFXSound);
				Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
			{
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
			{
				Function.Call(Hash.STOP_SOUND, PinSound);
				Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
			}
			CurrentDrill = DrillUsing.CasinoDrill;
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
			Function.Call(Hash.STOP_SOUND, DrillFXSound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			DrillFXSound = GlobalHelper.CreateSoundID();
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
			if (Bag != null)
			{
				Bag.Delete();
			}
			Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.2f));
			float heading = Game.Player.Character.Heading;
			Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
			Bag.IsCollisionEnabled = false;
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, heading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SyncScene, "anim@heists@fleeca_bank@drilling", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SyncScene, "intro", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, SyncScene, "bag_intro", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Drill, SyncScene, "intro", LoadDict("anim@heists@fleeca_bank@drilling"), 16f, -16f, 0, 1148846080);
			Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, SyncScene, 0.0);
			firsttime = true;
		}

		public static void StartCasinoLaser()
		{
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
			{
				Function.Call(Hash.STOP_SOUND, DrillFXSound);
				Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
			{
				Function.Call(Hash.STOP_SOUND, FailSound);
				Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
			{
				Function.Call(Hash.STOP_SOUND, PinSound);
				Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
			}
			CurrentDrill = DrillUsing.CasinoLaser;
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
			Function.Call(Hash.STOP_SOUND, DrillFXSound);
			Function.Call(Hash.STOP_SOUND, FailSound);
			Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
			Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
			if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
			{
				Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
			}
			Game.Player.Character.Weapons.Give(WeaponHash.Unarmed, 0, true, true);
			Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, false);
			Game.Player.Character.IsPositionFrozen = true;
			DrillFXSound = GlobalHelper.CreateSoundID();
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
			if (Bag != null)
			{
				Bag.Delete();
			}
			Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0.2f));
			float heading = Game.Player.Character.Heading;
			Bag = GlobalHelper.CreateProp(RequestModel("hei_p_m_bag_var22_arm_s"), Game.Player.Character.Position, false, false);
			Bag.IsCollisionEnabled = false;
			SyncScene = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, offsetPosition.X, offsetPosition.Y, offsetPosition.Z, 0.0, 0.0, heading, 2);
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, SyncScene, "anim@heists@fleeca_bank@drilling", "intro", 1000.0, -4.0, 128, 0, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, SyncScene, "intro", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bag, SyncScene, "bag_intro", LoadDict("anim@heists@fleeca_bank@drilling"), 1000.0, -8f, -8f, heading + 180f, 1148846080, 0);
			Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Drill, SyncScene, "intro", LoadDict("anim@heists@fleeca_bank@drilling"), 16f, -16f, 0, 1148846080);
			Function.Call(Hash.SET_SYNCHRONIZED_SCENE_PHASE, SyncScene, 0.0);
			firsttime = true;
		}

		public static void DrillScreen(float z)
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scale, 255, 255, 255, 255);
			RunScaleformFunction(scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 24, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public static void DrillScreenStart()
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scale, 255, 255, 255, 255);
			Speed = 0f;
			Temp = 0f;
			Pos = 0f;
			RunScaleformFunction(scale, "SET_TEMPERATURE", Temp, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_DRILL_POSITION", Pos, -1082130432, -1082130432, -1082130432, -1082130432);
			RunScaleformFunction(scale, "SET_SPEED", Speed, -1082130432, -1082130432, -1082130432, -1082130432);
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 24, 0), "Use Drill");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
			scaleform.Dispose();
		}

		public DrillingMinigames()
		{
			base.Tick += Ontick;
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

		private void DisplayHelpTextThisFrame(string helpText)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, 1);
		}

		public static void RunScaleformFunction(int Handle, string name, params object[] args)
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

		public void Ontick(object sender, EventArgs e)
		{
			if (CurrentDrill == DrillUsing.None)
			{
				return;
			}
			if (SPH_Debug.ShowRunningClassDebug)
			{
				Class1.drawText("Drilling Minigames Running", 0.8f, 0.15f, 0.5f, 255, 255, 255);
			}
			if (DrillDone && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SyncScene) >= 1065353216)
			{
				DrillDone = false;
				SyncScene = -1;
				FleecaDrillFullyDone = true;
			}
			DrillLoc = Game.Player.Character.Position;
			DrillHeading = Game.Player.Character.Heading;
			if (Game.Player.Name.ToUpper().Equals("CINNBARGNU2"))
			{
				if ((Game.IsControlPressed(Control.Sprint) && Game.IsControlJustPressed(Control.Context)) || (CurrentDrill == DrillUsing.Fleeca && DrillDone))
				{
					if (FleecaDrillActive)
					{
						EndFleecaDrill();
					}
					else if (!FleecaDrillActive)
					{
						StartFleecaDrill();
					}
				}
				if ((Game.IsControlPressed(Control.Sprint) && Game.IsControlJustPressed(Control.VehicleDuck)) || (CurrentDrill == DrillUsing.CasinoDrill && (DrillDone || DrillingComplete)))
				{
					if (CurrentDrill == DrillUsing.None)
					{
						StartCasinoDrill();
					}
					else
					{
						EndCasinoDrillorLaser();
					}
				}
				if ((Game.IsControlPressed(Control.Sprint) && Game.IsControlJustPressed(Control.Cover)) || (CurrentDrill == DrillUsing.CasinoLaser && (DrillDone || DrillingComplete)))
				{
					if (CurrentDrill == DrillUsing.None)
					{
						StartCasinoLaser();
					}
					else
					{
						EndCasinoDrillorLaser();
					}
				}
			}
			if (CurrentDrill == DrillUsing.Fleeca && FleecaDrillActive)
			{
				if (FleecaDrillActive)
				{
					DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Quit using Drill");
					if (Game.IsControlJustPressed(Control.Context))
					{
						EndFleecaDrill();
					}
				}
				if (Pos >= 0.9f)
				{
					DrillDone = true;
					CalledDrillEndViaComplete = true;
					EndFleecaDrill();
					Game.Player.Character.IsPositionFrozen = false;
				}
				if (Drill != null)
				{
					if (Temp >= 3f)
					{
						Pos = 0f;
						Temp = 0f;
						Speed = 0f;
						DrillScreen(Zone);
						SoundPlaying = false;
						if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
						{
							Notification.Show("Stop DrillFX Sound 1");
							Function.Call(Hash.STOP_SOUND, DrillFXSound);
							Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
						}
						if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
						{
							Function.Call(Hash.STOP_SOUND, FailSound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
						}
						Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, 1, AnimationFlags.Loop, -1f);
					}
					if (Isdrilliing)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SyncScene) >= 1065353216 || SyncScene == -1)
						{
							if (!(Zone > 349f) || !(Zone < 752f))
							{
								Zone = 551f;
								if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
								{
									Function.Call(Hash.STOP_SOUND, FailSound);
									Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
									Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
									Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
								}
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, 1, AnimationFlags.Loop, -1f);
							}
							if (!Game.IsControlPressed(Control.Attack))
							{
								Function.Call(Hash.STOP_SOUND, DrillFXSound);
								Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
								if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
								{
									Function.Call(Hash.STOP_SOUND, PinSound);
									Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
								}
								SoundPlayingId = 0;
								if (SoundPlaying)
								{
									SoundPlaying = false;
									if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
									{
										Notification.Show("Stop DrillFX Sound 2");
										Function.Call(Hash.STOP_SOUND, DrillFXSound);
										Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
									}
								}
								if (Speed >= 0f)
								{
									Speed -= 0.05f;
								}
								if (Temp >= 0f)
								{
									Temp -= 0.1f;
								}
								DrillScreen(Zone);
							}
							if (Game.IsControlPressed(Control.Attack))
							{
								if (Game.GameTime >= CheckPlaySoundTimer)
								{
									if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
									{
										Notification.Show("Play SoundFX");
										Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, DrillFXSound, "Drill", Drill, "DLC_HEIST_FLEECA_SOUNDSET", true, 0);
									}
									CheckPlaySoundTimer = Game.GameTime + 1000;
								}
								if (Speed >= 0.05f)
								{
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "fm_mission_controler");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "fm_mission_controler");
									if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
									{
										DrillFX = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_drill_debris", Drill, 0f, -0.55f, 0.01f, 90f, 90f, 90f, 0.8f, 0, 0, 0);
									}
									Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, DrillFX, "power", 0.7f, 0);
								}
								Random random = new Random();
								Zone += random.Next(1, 3);
								Temp += 0.015f;
								if (Temp < 1.8f)
								{
									Speed += 0.1f;
									Pos += 0.003f;
								}
								else if (Temp >= 1.8f)
								{
									Speed += 0.05f;
									Pos += 0.0005f;
								}
								DrillScreen(Zone);
								if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
								{
									Function.Call(Hash.STOP_SOUND, PinSound);
								}
								if (Pos >= 0.326f && !triggerLockA)
								{
									SoundPlaying = false;
									Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
									triggerLockA = true;
								}
								if (Pos >= 0.476f && !triggerLockB)
								{
									SoundPlaying = false;
									Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
									triggerLockB = true;
								}
								if (Pos >= 0.625f && !triggerLockC)
								{
									SoundPlaying = false;
									Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
									triggerLockC = true;
								}
								if (Pos >= 0.776f && !triggerLockD)
								{
									SoundPlaying = false;
									Function.Call(Hash.PLAY_SOUND_FRONTEND, PinSound, "Drill_Pin_Break", "DLC_HEIST_FLEECA_SOUNDSET", 1);
									triggerLockD = true;
								}
								DrillScreen(Zone);
							}
						}
						if (!Game.IsControlPressed(Control.Attack))
						{
							if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
							{
								Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
							}
							if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SyncScene) >= 1065353216 || SyncScene == -1)
							{
								SyncScene = -1;
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
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_idle", 6f, 1f, 1, AnimationFlags.Loop, -1f);
							}
						}
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SyncScene) >= 1065353216 || SyncScene == -1)
						{
							SyncScene = -1;
							if (Game.IsControlPressed(Control.Attack) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_right_end", 3))
							{
								if (Zone > 349f && Zone < 752f)
								{
									if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
									{
										Function.Call(Hash.STOP_SOUND, FailSound);
										Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
									}
									Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_idle", 6f, 1f, 1, AnimationFlags.None, -1f);
								}
								else
								{
									if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
									{
										Function.Call(Hash.STOP_SOUND, FailSound);
										Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
										Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
									}
									Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, 1, AnimationFlags.Loop, -1f);
								}
								if (Game.IsControlPressed(Control.Attack) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_right_end", 3))
								{
									if (Zone > 349f && Zone < 752f)
									{
										DrillScreen(Zone);
										Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 3f, 1f, 20000, AnimationFlags.StayInEndFrame, -1f);
									}
									else
									{
										Zone = 551f;
										if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
										{
											Function.Call(Hash.STOP_SOUND, FailSound);
											Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
											Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
										}
										Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, 1, AnimationFlags.Loop, -1f);
									}
								}
								if (Game.IsControlPressed(Control.Attack) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_right_end", 3))
								{
									if ((Zone > 349f) & (Zone < 752f))
									{
										DrillScreen(Zone);
										Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 3f, 1f, 20000, AnimationFlags.StayInEndFrame, -1f);
									}
									else
									{
										Zone = 551f;
										Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
										if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound) || !SoundPlaying)
										{
											SoundPlaying = false;
											Function.Call(Hash.STOP_SOUND, FailSound);
											Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
										}
										Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, 1, AnimationFlags.Loop, -1f);
									}
								}
							}
							else
							{
								if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
								{
									Function.Call(Hash.STOP_SOUND, DrillFXSound);
									Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
								}
								if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, FailSound))
								{
									Function.Call(Hash.STOP_SOUND, FailSound);
									Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
								}
								if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, PinSound))
								{
									Function.Call(Hash.STOP_SOUND, PinSound);
									Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
								}
							}
						}
					}
				}
			}
			if (CurrentDrill != DrillUsing.CasinoDrill && CurrentDrill != DrillUsing.CasinoLaser)
			{
				return;
			}
			DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Quit using Drill/Laser Cutter");
			if (Game.IsControlJustPressed(Control.Context))
			{
				EndCasinoDrillorLaser();
			}
			if (!(Drill != null) || !Isdrilliing)
			{
				return;
			}
			if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, SyncScene) >= 1065353216 || SyncScene == -1)
			{
				if (Game.IsControlPressed(Control.Attack))
				{
					if (activeCasLaser)
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "fm_mission_controler");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "fm_mission_controler");
						if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
						{
							DrillFX = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_drill_debris", Drill, 0f, -0.55f, 0.01f, 90f, 90f, 90f, 0.8f, 0, 0, 0);
						}
						Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, DrillFX, "power", 0.7f, 0);
					}
					else
					{
						if (Game.GameTime >= CheckPlaySoundTimer)
						{
							if (Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
							{
								Notification.Show("Play SoundFX");
								Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, DrillFXSound, "Drill", Drill, "DLC_HEIST_FLEECA_SOUNDSET", true, 0);
							}
							CheckPlaySoundTimer = Game.GameTime + 1000;
						}
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "fm_mission_controler");
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "fm_mission_controler");
						if (!Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
						{
							DrillFX = GlobalHelper.CreatePTFX_LoopedOnEntity("scr_drill_debris", Drill, 0f, -0.55f, 0.01f, 90f, 90f, 90f, 0.8f, 0, 0, 0);
						}
						Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, DrillFX, "power", 0.7f, 0);
					}
				}
				else
				{
					Function.Call(Hash.STOP_SOUND, DrillFXSound);
					Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
					Function.Call(Hash.STOP_SOUND, FailSound);
					Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
					Function.Call(Hash.STOP_SOUND, PinSound);
					Function.Call(Hash.RELEASE_SOUND_ID, PinSound);
				}
				SyncScene = -1;
				if (!UseCasinoDrill && Temp >= 3f && Game.IsControlPressed(Control.Attack))
				{
					Pos = 0f;
					Temp = 0f;
					Speed = 0f;
					if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1))
					{
						Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
						Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
						Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, -1, AnimationFlags.Loop, -1f);
					}
				}
				if (UseCasinoDrill && temperature >= 1f && Game.IsControlPressed(Control.Attack) && !Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1))
				{
					Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
					Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, -1, AnimationFlags.Loop, -1f);
					Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
				}
				if (!UseCasinoDrill && !Game.IsControlPressed(Control.Attack) && !UseCasinoDrill)
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
						DrillingComplete = true;
						Isdrilliing = false;
					}
					if (Temp <= 3f)
					{
						if (Game.IsControlPressed(Control.MoveDownOnly))
						{
							Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							Random random2 = new Random();
							Zone += random2.Next(1, 3);
							Temp += 0.015f;
							if (Temp < 1.8f)
							{
								Speed += 0.1f;
								Pos += 0.001f;
							}
							else if (Temp >= 1.8f)
							{
								Speed += 0.05f;
								Pos += 0.0006f;
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
						if (Game.IsControlPressed(Control.Attack))
						{
							Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							Random random3 = new Random();
							Zone -= random3.Next(1, 3);
							Temp += 0.075f;
							if (Temp < 1.8f)
							{
								Speed += 0.1f;
								Pos += 0.001f;
							}
							else if (Temp >= 1.8f)
							{
								Speed += 0.05f;
								Pos += 0.0006f;
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
						if (temperature <= 1.2f && Drill != null && Isdrilliing && Game.IsControlPressed(Control.Attack) && temperature >= 1f)
						{
							Function.Call(Hash.STOP_SOUND, FailSound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
							Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
							if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1))
							{
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							}
						}
						if (temperature <= 1f && Game.IsControlPressed(Control.Attack))
						{
							drillspeed += 0.035f;
							DrillPos += 0.0025f;
							temperature += 0.0135f;
							Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
						}
					}
					if (activeCasDrill && activatescaleCas("VAULT_DRILL") == 1)
					{
						if (temperature <= 1.2f && Drill != null && Isdrilliing && Game.IsControlPressed(Control.Attack) && temperature >= 1f)
						{
							Function.Call(Hash.STOP_SOUND, FailSound);
							Function.Call(Hash.RELEASE_SOUND_ID, FailSound);
							Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
							Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, FailSound, "Drill_Jam", Game.Player.Character, "DLC_HEIST_FLEECA_SOUNDSET", 1, 20);
							if (!Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, Game.Player.Character, "anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1))
							{
								Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_straight_fail", 1f, 1f, -1, AnimationFlags.Loop, -1f);
							}
						}
						if (temperature <= 1f && Game.IsControlPressed(Control.Attack))
						{
							Isdrilliing = true;
							drillspeed += 0.015f;
							DrillPos += 0.0015f;
							temperature += 0.0045f;
							Game.Player.Character.Task.PlayAnimation("anim@heists@fleeca_bank@drilling", "drill_right_end", 1f, 1f, -1, AnimationFlags.Loop, -1f);
						}
					}
				}
				if (UseCasinoDrill)
				{
					if (activeCasDrill && activatescaleCas("VAULT_DRILL") == 1 && temperature <= 100f)
					{
						if (!Game.IsControlPressed(Control.Attack))
						{
							if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, DrillFX))
							{
								Function.Call(Hash.REMOVE_PARTICLE_FX, DrillFX, 0);
							}
							if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, DrillFXSound))
							{
								Notification.Show("Stop DrillFX Sound 2");
								Function.Call(Hash.STOP_SOUND, DrillFXSound);
								Function.Call(Hash.RELEASE_SOUND_ID, DrillFXSound);
							}
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
						float num = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
						num = 0f - num + 0.9f;
						if (DrillPos > 0.242f && !pin_one)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_one = true;
							if (NumOfDiscs == 1f)
							{
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
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
					}
					if (activeCasLaser && activatescaleCas("VAULT_LASER") == 1 && temperature <= 100f)
					{
						if (!Game.IsControlPressed(Control.Attack))
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
						float num2 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
						num2 = 0f - num2 + 0.9f;
						if (DrillPos > 0.242f && !pin_one)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "laser_pin_break", "dlc_ch_heist_finale_laser_drill_sounds", 1);
							pin_one = true;
							if (NumOfDiscs == 1f)
							{
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
								DrillingComplete = true;
								Isdrilliing = false;
							}
						}
					}
				}
			}
			if (!UseCasinoDrill)
			{
				return;
			}
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
	}
}
