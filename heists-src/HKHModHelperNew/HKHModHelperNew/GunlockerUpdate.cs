using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace HKHModHelperNew
{
	internal class GunlockerUpdate : Script
	{
		public GunlockerUpdate()
		{
			base.Tick += OnTick;
		}

		public void OnTick(object Sender, EventArgs e)
		{
			if (Class1.GunLockerOpenLastCheck)
			{
				if (Class1.Gun != null)
				{
					Class1.Gun.IsCollisionEnabled = false;
					Vector3 position = Game.Player.Character.Position;
					int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position.X, position.Y, position.Z);
					int num2 = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character);
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Class1.Gun, num, num2);
				}
				if (Class1.GunObject != null)
				{
					Class1.GunObject.IsCollisionEnabled = false;
					Vector3 position2 = Game.Player.Character.Position;
					int num3 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position2.X, position2.Y, position2.Z);
					int num4 = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character);
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Class1.GunObject, num3, num4);
				}
				if (Class1.CameraHolder != null)
				{
					Class1.CameraHolder.IsCollisionEnabled = false;
					Vector3 position3 = Game.Player.Character.Position;
					int num5 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position3.X, position3.Y, position3.Z);
					int num6 = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character);
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Class1.CameraHolder, num5, num6);
				}
				if (Class1.CameraProp != null)
				{
					Class1.CameraProp.IsCollisionEnabled = false;
					Vector3 position4 = Game.Player.Character.Position;
					int num7 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, position4.X, position4.Y, position4.Z);
					int num8 = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, Game.Player.Character);
					Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, Class1.CameraProp, num7, num8);
				}
			}
			if (!Class1.RebreatherEquipped)
			{
				Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, -1f);
				if (Class1.Rebreather != null)
				{
					Class1.Rebreather.Delete();
					Class1.Rebreather = null;
				}
			}
			if (Class1.RebreatherEquipped)
			{
				Function.Call(Hash.SET_PED_MAX_TIME_UNDERWATER, Game.Player.Character, 250f);
				if (Class1.Rebreather != null)
				{
					if (Game.Player.Character.IsSwimmingUnderWater || Game.Player.Character.IsSwimming)
					{
						Class1.Rebreather.Opacity = 255;
					}
					else
					{
						Class1.Rebreather.Opacity = 0;
					}
				}
				if (Class1.Rebreather == null)
				{
					Class1.Rebreather = World.CreateProp(Class1.RequestModel("reh_prop_reh_rebreather_01a"), Game.Player.Character.Position, false, false);
					if (Game.Player.Character.Model == PedHash.Trevor)
					{
						Class1.ATTACH_ENTITY_TO_ENTITY(Class1.Rebreather, Game.Player.Character, Game.Player.Character.Bones[Bone.IKHead].Index, -0.064f, 0.127f, 0f, 0f, -90f, 180f, false, false, false, false, 2, true);
					}
					else if (Game.Player.Character.Model == PedHash.Franklin)
					{
						Class1.ATTACH_ENTITY_TO_ENTITY(Class1.Rebreather, Game.Player.Character, Game.Player.Character.Bones[Bone.IKHead].Index, -0.071f, 0.141f, 0f, 0f, -90f, 180f, false, false, false, false, 2, true);
					}
					else if (Game.Player.Character.Model == PedHash.Michael)
					{
						Class1.ATTACH_ENTITY_TO_ENTITY(Class1.Rebreather, Game.Player.Character, Game.Player.Character.Bones[Bone.IKHead].Index, -0.071f, 0.138f, 0f, 0f, -90f, 180f, false, false, false, false, 2, true);
					}
					else
					{
						Class1.ATTACH_ENTITY_TO_ENTITY(Class1.Rebreather, Game.Player.Character, Game.Player.Character.Bones[Bone.IKHead].Index, -0.071f, 0.138f, 0f, 0f, -90f, 180f, false, false, false, false, 2, true);
					}
					Class1.Rebreather.Opacity = 0;
				}
			}
			if (Game.GameTime >= Class1.CheckGunlockerOpenTimer)
			{
				Class1.CheckGunlockerOpenTimer = Game.GameTime + 2000;
				Class1.GunLockerOpenLastCheck = Class1.GunlockerOpen();
				if (!Class1.GunLockerOpenLastCheck)
				{
					Class1.GunlockerIsRunning = false;
				}
			}
			Class1.GunLockerCheck = Game.GameTime + 2500;
			if (!Class1.GunlockerIsRunning)
			{
				Class1.GetLocation();
			}
			if (!Class1.GunLockerOpenLastCheck && Class1.Gun != null)
			{
				Class1.AwaitingInitialMenu = false;
				if (Class1.CameraHolder != null)
				{
					Class1.CameraHolder.Delete();
				}
				if (Class1.CameraProp != null)
				{
					Class1.CameraProp.Delete();
				}
				Game.Player.CanControlCharacter = true;
				Class1.Gun.Delete();
				if (Class1.WeaponCamera != null)
				{
					Game.Player.Character.IsVisible = true;
					Game.Player.Character.IsPositionFrozen = false;
					Class1.WeaponCamera.IsActive = false;
					Class1.WeaponCamera.Delete();
					Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 0);
				}
			}
			if (Class1.GunLockerOpenLastCheck)
			{
				Game.DisableControlThisFrame(Control.Phone);
				if (Class1.WeaponCamera != null)
				{
					Class1.CameraProp.Opacity = 0;
					Class1.CameraHolder.Opacity = 0;
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
					Game.DisableControlThisFrame(Control.SelectWeapon);
					Game.DisableControlThisFrame(Control.PhoneUp);
					Game.DisableControlThisFrame(Control.Attack);
					Game.Player.CanControlCharacter = true;
					if (Class1.CameraHolder == null)
					{
						Class1.CameraProp = World.CreateProp(Class1.RequestModel("hei_Prop_heist_box"), Class1.WeaponCamera.Position, Class1.Gun.Rotation, false, false);
						Class1.CameraHolder = World.CreateProp(Class1.RequestModel("hei_Prop_heist_box"), Class1.Gun.GetPositionOffset(new Vector3(0f, 0f, -0.1f)), Class1.Gun.Rotation, false, false);
					}
					Class1.RotationToDirection(Class1.WeaponCamera.Rotation);
					Class1.OffsetRotX -= (float)((double)Game.GetControlValueNormalized(Control.LookUpDown) * (double)Class1.Precision * 4.0);
					Class1.OffsetRotZ -= (float)((double)Game.GetControlValueNormalized(Control.LookLeftRight) * (double)Class1.Precision * 4.0);
					if ((double)Class1.OffsetRotX > 90.0)
					{
						Class1.OffsetRotX = 90f;
					}
					else if ((double)Class1.OffsetRotX < -90.0)
					{
						Class1.OffsetRotX = -90f;
					}
					if (Game.GameTime >= Class1.TimeSinceLastEaseCam)
					{
						Class1.CameraHolder.Rotation = new Vector3(Class1.OffsetRotY, Class1.OffsetRotX, Class1.OffsetRotZ);
					}
					else
					{
						Class1.OffsetRotY = 0f;
						Class1.OffsetRotX = 0f;
						if (Class1.GunHeading > 0f)
						{
							Class1.OffsetRotZ = Class1.Gun.Heading - 90f;
							Class1.MinRotationZ = Class1.OffsetRotZ - 75f;
							Class1.MaxRotationZ = Class1.OffsetRotZ + 75f;
							Class1.CameraHolder.Rotation = new Vector3(0f, 0f, Class1.Gun.Heading - 90f);
						}
						else
						{
							Class1.OffsetRotZ = Class1.Gun.Heading + 90f;
							Class1.MinRotationZ = Class1.OffsetRotZ - 75f;
							Class1.MaxRotationZ = Class1.OffsetRotZ + 75f;
							Class1.CameraHolder.Rotation = new Vector3(0f, 0f, Class1.Gun.Heading + 90f);
						}
					}
					if (Class1.CameraHolder.Rotation.Z < 0f)
					{
						Vector3 rotation = Class1.CameraHolder.Rotation;
					}
					if (Class1.OffsetRotZ < Class1.MinRotationZ)
					{
						Class1.CameraHolder.Rotation = new Vector3(Class1.CameraHolder.Rotation.X, Class1.CameraHolder.Rotation.Y, Class1.MinRotationZ);
					}
					if (Class1.OffsetRotZ > Class1.MaxRotationZ)
					{
						Class1.CameraHolder.Rotation = new Vector3(Class1.CameraHolder.Rotation.X, Class1.CameraHolder.Rotation.Y, Class1.MaxRotationZ);
					}
					if (Class1.CameraHolder.Rotation.Y > 35f)
					{
						Class1.CameraHolder.Rotation = new Vector3(Class1.CameraHolder.Rotation.X, 35f, Class1.CameraHolder.Rotation.Z);
					}
					if (Class1.CameraHolder.Rotation.Y < -30f)
					{
						Class1.CameraHolder.Rotation = new Vector3(Class1.CameraHolder.Rotation.X, -30f, Class1.CameraHolder.Rotation.Z);
					}
					Class1.WeaponCamera.Position = Class1.CameraHolder.GetOffsetPosition(new Vector3(0.7f, 0f, 0f));
					Class1.WeaponCamera.NearClip = 0.01f;
					Class1.WeaponCamera.PointAt(Class1.CameraHolder);
					Class1.CameraProp.Position = Class1.WeaponCamera.Position;
				}
				if (Class1.Gun == null && !Class1.GunlockerIsRunning && Class1.WeaponCamera != null && Class1.Gun != null)
				{
					Class1.GunlockerIsRunning = true;
					Class1.ReloadGunData(Game.Player.Character.Weapons.BestWeapon.Hash);
					Class1.CreateWeaponCamera();
					if (Class1.CameraHolder != null)
					{
						Class1.CameraHolder.Delete();
					}
					if (Class1.CameraProp != null)
					{
						Class1.CameraProp.Delete();
					}
					Class1.CameraProp = World.CreateProp(Class1.RequestModel("hei_Prop_heist_box"), Class1.WeaponCamera.Position, Class1.Gun.Rotation, false, false);
					Class1.CameraHolder = World.CreateProp(Class1.RequestModel("hei_Prop_heist_box"), Class1.Gun.Position, new Vector3(Class1.Gun.Rotation.X, Class1.Gun.Rotation.Y, Class1.Gun.Rotation.Z - 180f), false, false);
				}
				if (Class1.Gun != null && !Class1.Gun.Exists() && !Class1.GunlockerIsRunning)
				{
					Class1.GunlockerIsRunning = true;
					Class1.ReloadGunData(Game.Player.Character.Weapons.BestWeapon.Hash);
					Class1.CreateWeaponCamera();
					if (Class1.CameraHolder != null)
					{
						Class1.CameraHolder.Delete();
					}
					if (Class1.CameraProp != null)
					{
						Class1.CameraProp.Delete();
					}
					Class1.CameraProp = World.CreateProp(Class1.RequestModel("hei_Prop_heist_box"), Class1.WeaponCamera.Position, Class1.Gun.Rotation, false, false);
					Class1.CameraHolder = World.CreateProp(Class1.RequestModel("hei_Prop_heist_box"), Class1.Gun.Position, new Vector3(Class1.Gun.Rotation.X, Class1.Gun.Rotation.Y, Class1.Gun.Rotation.Z - 180f), false, false);
				}
			}
			if (Class1.WeaponCamera == null && Class1.Gun == null && !Class1.GunLockerOpenLastCheck)
			{
				return;
			}
			if (Class1.WeaponCamera != null)
			{
				if (Class1.ModifyingWeapons)
				{
					if (Game.Player.Character.Weapons.CurrentWeaponObject != null)
					{
						Game.Player.Character.Weapons.CurrentWeaponObject.Opacity = 0;
					}
				}
				else if (Game.Player.Character.Weapons.CurrentWeaponObject != null)
				{
					Game.Player.Character.Weapons.CurrentWeaponObject.Opacity = 255;
				}
			}
			Class1.DisplayingCannotEnterGarageMsg = false;
		}
	}
}
