using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using SmartAIGuards;

namespace CayoPericoHeistInSP
{
	public class CayoPericoCameraManager : Script
	{
		public CayoPericoCameraManager()
		{
			base.Tick += onTick;
		}

		public void onTick(object sender, EventArgs e)
		{
			if (CayoPericoHeist.ShutDownCCTV || !CayoPericoHeist.MissionOn || (CayoPericoHeist.HeistStage != 1 && CayoPericoHeist.HeistStage != 7) || CayoPericoHeist.CameraTrigger >= Game.GameTime)
			{
				return;
			}
			Random random = new Random();
			CayoPericoHeist.CameraTrigger = Game.GameTime + 1000;
			foreach (CayoPericoHeist.SecurityCamera securityCamera in CayoPericoHeist.SecurityCameras)
			{
				bool flag = false;
				if (CayoPericoHeist.GetDistance_VDIST2(Game.Player.Character.Position, securityCamera.Position, "<", 100f))
				{
					if (securityCamera.Camera == null && securityCamera.Stand == null)
					{
						securityCamera.Stand = World.CreateProp(CayoPericoHeist.RequestModel("h4_prop_h4_cctv_pole_04"), securityCamera.Position, new Vector3(0f, 0f, securityCamera.Heading), false, false);
						securityCamera.Camera = World.CreateProp(CayoPericoHeist.RequestModel("ch_prop_ch_cctv_cam_01a"), securityCamera.Stand.GetOffsetPosition(new Vector3(-0.45f, 0f, 4.25f)), new Vector3(0f, 0f, securityCamera.Heading - 90f), false, false);
						securityCamera.InitialRotation = securityCamera.Camera.Heading;
						securityCamera.MaxRotation = securityCamera.InitialRotation + 75f;
						securityCamera.MinRotation = securityCamera.InitialRotation - 75f;
						securityCamera.CameraRotation = securityCamera.Camera.Heading;
						securityCamera.Mode = random.Next(0, 100);
						int num = random.Next(0, 100);
						if (num >= 50)
						{
							securityCamera.RotationDirection = "-";
						}
						else
						{
							securityCamera.RotationDirection = "+";
						}
					}
					if (!(securityCamera.Camera != null) || !(securityCamera.Stand != null))
					{
						continue;
					}
					if (securityCamera.RotationDirection != null)
					{
						if (securityCamera.RotationDirection.Equals("-"))
						{
							securityCamera.DegressRotatedThisDirection += Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 1.0, 5.0);
							securityCamera.Camera.Rotation = new Vector3(securityCamera.Camera.Rotation.X, securityCamera.Camera.Rotation.Y, securityCamera.Camera.Rotation.Z - 3.5f);
							if (securityCamera.DegressRotatedThisDirection >= 75f)
							{
								securityCamera.RotationDirection = "+";
								securityCamera.DegressRotatedThisDirection = -75f;
							}
						}
						else if (securityCamera.RotationDirection.Equals("+"))
						{
							securityCamera.DegressRotatedThisDirection += Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 1.0, 5.0);
							securityCamera.Camera.Rotation = new Vector3(securityCamera.Camera.Rotation.X, securityCamera.Camera.Rotation.Y, securityCamera.Camera.Rotation.Z + 3.5f);
							if (securityCamera.DegressRotatedThisDirection >= 75f)
							{
								securityCamera.RotationDirection = "-";
								securityCamera.DegressRotatedThisDirection = -75f;
							}
						}
					}
					if (securityCamera.Camera.AttachedBlip == null)
					{
						securityCamera.Camera.AddBlip();
						if (securityCamera.Camera.AttachedBlip != null)
						{
							securityCamera.Camera.AttachedBlip.Sprite = BlipSprite.CCTV;
							securityCamera.Camera.AttachedBlip.Color = BlipColor.Red;
							securityCamera.Camera.AttachedBlip.Name = "Camera";
							securityCamera.Camera.AttachedBlip.Scale = 1.5f;
						}
					}
					else
					{
						securityCamera.Camera.AttachedBlip.Rotation = (int)securityCamera.Camera.Heading;
						if (CayoPericoHeist.InCompound)
						{
							CayoPericoHeist.UpdateBlipConeData(securityCamera.Camera.AttachedBlip, 0.325f, 16.25f, false);
						}
						if (!CayoPericoHeist.InCompound)
						{
							CayoPericoHeist.UpdateBlipConeData(securityCamera.Camera.AttachedBlip, 0.52f, 43.3333321f, false);
						}
					}
					if (securityCamera.Camera.Health > 999)
					{
						if (CayoPericoHeist.GetDistance_VDIST2(Game.Player.Character.Position, securityCamera.Position, "<", 100f))
						{
							if (securityCamera.Camera.AttachedBlip != null)
							{
								securityCamera.Camera.AttachedBlip.Alpha = 255;
							}
						}
						else if (securityCamera.Camera.AttachedBlip != null)
						{
							securityCamera.Camera.AttachedBlip.Alpha = 255;
						}
					}
					else
					{
						if (!ManageCayoPericoDialogue.AudioVar_Finale_DestroyedCamera)
						{
							if (CayoPericoHeist.CamerasDestoryed == 1)
							{
								ManageCayoPericoDialogue.DialogueLines.Clear();
								ManageCayoPericoDialogue.DialogueLines.Add("HS4P_CIAA");
								ManageCayoPericoDialogue.DialogueLines.Add("HS4P_CIAB");
								ManageCayoPericoDialogue.Speaker = "HS4_PAVEL";
								ManageCayoPericoDialogue.StartVoiceOver();
								CayoPericoHeist.CamerasDestoryed++;
							}
							else
							{
								CayoPericoHeist.CamerasDestoryed++;
								if (CayoPericoHeist.CamerasDestoryed + 1 >= CayoPericoHeist.MaxCamerasDestroyed)
								{
									if (!ManageCayoPericoDialogue.AudioVar_Finale_DestroyedCameraOneLeft)
									{
										ManageCayoPericoDialogue.AudioVar_Finale_DestroyedCameraOneLeft = true;
										ManageCayoPericoDialogue.DialogueLines.Clear();
										ManageCayoPericoDialogue.DialogueLines.Add("HS4P_CHAA");
										ManageCayoPericoDialogue.DialogueLines.Add("HS4P_CHAB");
										ManageCayoPericoDialogue.Speaker = "HS4_PAVEL";
										ManageCayoPericoDialogue.StartVoiceOver();
										CayoPericoHeist.CamerasDestoryed++;
									}
								}
								else if (CayoPericoHeist.CamerasDestoryed >= CayoPericoHeist.MaxCamerasDestroyed && !ManageCayoPericoDialogue.AudioVar_Finale_DestroyedtooManyCameras)
								{
									ManageCayoPericoDialogue.DialogueLines.Clear();
									ManageCayoPericoDialogue.DialogueLines.Add("HS4P_CKAA");
									ManageCayoPericoDialogue.DialogueLines.Add("HS4P_AVAA");
									ManageCayoPericoDialogue.Speaker = "HS4_PAVEL";
									ManageCayoPericoDialogue.StartVoiceOver();
									ManageCayoPericoDialogue.AudioVar_Finale_DestroyedCamera = true;
									ManageCayoPericoDialogue.AudioVar_Finale_DestroyedtooManyCameras = true;
								}
							}
						}
						if (securityCamera.Camera.AttachedBlip != null)
						{
							securityCamera.Camera.AttachedBlip.Alpha = 255;
						}
					}
					if (securityCamera.Camera.Health < 999)
					{
						if (securityCamera.Camera.AttachedBlip != null)
						{
							securityCamera.Camera.AttachedBlip.Alpha = 0;
						}
						continue;
					}
					bool flag2 = false;
					if (!CayoPericoHeist.InCompound)
					{
						flag2 = CayoPericoHeist.CanCameraPropSeePlayer(securityCamera.Camera, 52f, 43.3333321f);
					}
					if (CayoPericoHeist.InCompound)
					{
						flag2 = CayoPericoHeist.CanCameraPropSeePlayer(securityCamera.Camera, 32.5f, 16.25f);
					}
					if (0 == 0)
					{
						continue;
					}
					if (CayoPericoHeist.HeistStage == 1)
					{
						bool flag3 = false;
						if (CayoPericoHeist.MissionStage >= 5 || CayoPericoHeist.MissionStage == 14)
						{
							flag3 = true;
						}
						if (flag3 && !CayoPericoHeist.PlayerCaught && !CayoPericoHeist.InSafezone1 && !CayoPericoHeist.InSafezone2)
						{
							CayoPericoHeist.PlayerCaught = true;
							CayoPericoHeist.CaughtStage = 0;
						}
					}
					if (CayoPericoHeist.HeistStage == 7)
					{
						if (!ManageCayoPericoDialogue.AudioVar_Finale_CCTVCaughtPlayer)
						{
							ManageCayoPericoDialogue.DialogueLines.Clear();
							ManageCayoPericoDialogue.DialogueLines.Add("HS4P_CIAB");
							ManageCayoPericoDialogue.DialogueLines.Add("HS4P_CJAA");
							ManageCayoPericoDialogue.Speaker = "HS4_PAVEL";
							ManageCayoPericoDialogue.StartVoiceOver();
							ManageCayoPericoDialogue.AudioVar_Finale_CCTVCaughtPlayer = true;
						}
						if (!CayoPericoHeist.GuardsAlerted)
						{
							Notification.Show(CayoPericoHeist.HeistLeadername + " :  The CCTV have detected you!, The compound has sent in reinforcements");
							SmartGuards.SpawnMerryweatherHeli(Game.Player.Character.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 150, 300)), 0f, "Buzzard", false, new List<uint> { Function.Call<uint>(Hash.GET_HASH_KEY, "g_m_m_cartelguards_01") }, SmartGuards.EnemyGroup.Cayo_Perico_Cartel);
							Script.Wait(0);
							SmartGuards.SpawnMerryweatherHeli(Game.Player.Character.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 150, 300)), 0f, "Buzzard", false, new List<uint> { Function.Call<uint>(Hash.GET_HASH_KEY, "g_m_m_cartelguards_01") }, SmartGuards.EnemyGroup.Cayo_Perico_Cartel);
							Script.Wait(0);
							Vector3 pos = CayoPericoHeist.GenerateSpawnPos(Game.Player.Character.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 200, 300)), CayoPericoHeist.Nodetype.AnyRoad, false);
							SmartGuards.SpawnMerryweatherCar(pos, 0f, "vetir", false, new List<uint> { Function.Call<uint>(Hash.GET_HASH_KEY, "g_m_m_cartelguards_01") }, SmartGuards.EnemyGroup.Cayo_Perico_Cartel);
							pos = CayoPericoHeist.GenerateSpawnPos(Game.Player.Character.Position.Around(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 200, 300)), CayoPericoHeist.Nodetype.AnyRoad, false);
							SmartGuards.SpawnMerryweatherCar(pos, 0f, "vetir", false, new List<uint> { Function.Call<uint>(Hash.GET_HASH_KEY, "g_m_m_cartelguards_01") }, SmartGuards.EnemyGroup.Cayo_Perico_Cartel);
							CayoPericoHeist.startalarmsint = 0;
							CayoPericoHeist.GuardsAlerted = true;
						}
					}
				}
				else if (securityCamera.Camera != null && securityCamera.Camera.AttachedBlip != null)
				{
					securityCamera.Camera.AttachedBlip.Alpha = 0;
				}
			}
		}
	}
}
