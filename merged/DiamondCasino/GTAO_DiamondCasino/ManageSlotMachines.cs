using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAO_DiamondCasino
{
	internal class ManageSlotMachines : Script
	{
		public static bool SlotMachinesRemoved;

		public ManageSlotMachines()
		{
			if (_Debug.ManageSlotMachines != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!Class1.Initialized || (!Class1.IsinInterior && !CasinoGames.CreatedSlotMachines))
			{
				return;
			}
			if (Class1.IsinInterior)
			{
				SlotMachinesRemoved = false;
				if (CasinoGames.CreatedSlotMachines)
				{
					_Debug.ManageSlotMachines = 0;
					return;
				}
			}
			if (SlotMachinesRemoved)
			{
				return;
			}
			if (Class1.IsinInterior)
			{
				_Debug.ManageSlotMachines = 1;
				if (CasinoGames.CreatedSlotMachines)
				{
					return;
				}
				foreach (Ped slotMachinePed in CasinoGames.SlotMachinePeds)
				{
					if (slotMachinePed != null)
					{
						slotMachinePed.Delete();
					}
				}
				CasinoGames.SlotMachineProps.Clear();
				CasinoGames.SlotMachinePeds.Clear();
				CasinoGames.SlotLocH.Clear();
				Prop[] nearbyProps = World.GetNearbyProps(Game.Player.Character.Position, 500f);
				foreach (Prop prop in nearbyProps)
				{
					if (prop != null)
					{
						if (prop.Model == CasinoGames.RequestModel("vw_prop_casino_slot_04a"))
						{
							prop.Delete();
						}
						if (prop.Model == CasinoGames.RequestModel("vw_prop_casino_slot_05a"))
						{
							prop.Delete();
						}
						if (prop.Model == CasinoGames.RequestModel("vw_prop_casino_slot_06a"))
						{
							prop.Delete();
						}
						if (prop.Model == CasinoGames.RequestModel("vw_prop_casino_slot_01a"))
						{
							prop.Delete();
						}
						if (prop.Model == CasinoGames.RequestModel("vw_prop_casino_slot_08a"))
						{
							prop.Delete();
						}
						if (prop.Model == CasinoGames.RequestModel("vw_prop_casino_slot_07a"))
						{
							prop.Delete();
						}
						if (prop.Model == CasinoGames.RequestModel("vw_prop_casino_slot_02a"))
						{
							prop.Delete();
						}
						if (prop.Model == CasinoGames.RequestModel("vw_prop_casino_slot_03a"))
						{
							prop.Delete();
						}
					}
				}
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1107.735f, 234.7909f, -50.84093f), 72.00006f, 252.0001f, "vw_prop_casino_slot_08a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1108.005f, 233.9177f, -50.84093f), 144f, 324f, "vw_prop_casino_slot_07a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1105.486f, 229.4322f, -50.84093f), -144f, 36.00002f, "vw_prop_casino_slot_03a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1105.781f, 230.2973f, -50.84093f), -71.99997f, 108f, "vw_prop_casino_slot_02a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1104.572f, 229.4451f, -50.84093f), 144f, 324f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1104.302f, 230.3183f, -50.84093f), 72.00006f, 252.0001f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1120.753f, 232.4274f, -50.84092f), 90f, 270f, "vw_prop_casino_slot_02a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1120.853f, 233.1621f, -50.84092f), 75.00009f, 255.0001f, "vw_prop_casino_slot_01a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1132.109f, 249.0355f, -52.04094f), -60.00005f, 120f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1117.576f, 228.8767f, -50.84093f), -144f, 36.00002f, "vw_prop_casino_slot_06a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1138.07f, 252.6677f, -52.04095f), 63.00006f, 243.0001f, "vw_prop_casino_slot_07a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1117.871f, 229.7419f, -50.84093f), -71.99997f, 108f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1117.139f, 230.2895f, -50.84093f), -1.525879E-05f, 180f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1116.392f, 229.7628f, -50.84093f), 72.00006f, 252.0001f, "vw_prop_casino_slot_08a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1116.662f, 228.8896f, -50.84093f), 144f, 324f, "vw_prop_casino_slot_07a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1114.554f, 233.6625f, -50.84093f), -144f, 36.00002f, "vw_prop_casino_slot_03a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1114.848f, 234.5277f, -50.84093f), -71.99997f, 108f, "vw_prop_casino_slot_02a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1114.117f, 235.0753f, -50.84093f), -1.525879E-05f, 180f, "vw_prop_casino_slot_01a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1113.37f, 234.5486f, -50.84093f), 72.00006f, 252.0001f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1113.64f, 233.6755f, -50.84093f), 144f, 324f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1108.938f, 239.4797f, -50.84093f), 135.0001f, 315.0001f, "vw_prop_casino_slot_01a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1109.536f, 239.0278f, -50.84093f), 150f, 330f, "vw_prop_casino_slot_02a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1138.195f, 251.8611f, -52.04094f), 135.0001f, 315.0001f, "vw_prop_casino_slot_06a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1133.952f, 256.1037f, -52.04095f), 135.0001f, 315.0001f, "vw_prop_casino_slot_03a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1105.049f, 230.845f, -50.84093f), -1.525879E-05f, 180f, "vw_prop_casino_slot_01a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1132.396f, 248.3382f, -52.04094f), -75.00009f, 104.9999f, "vw_prop_casino_slot_03a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1130.376f, 250.3577f, -52.04094f), -14.99998f, 165f, "vw_prop_casino_slot_07a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1129.64f, 250.451f, -52.04094f), -1.525879E-05f, 180f, "vw_prop_casino_slot_08a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1139f, 251.7306f, -52.04095f), -152.9999f, 27.00006f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1131.062f, 250.0776f, -52.04094f), -30.00002f, 150f, "vw_prop_casino_slot_06a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1132.492f, 247.5984f, -52.04094f), -90f, 90f, "vw_prop_casino_slot_02a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1108.482f, 235.3176f, -50.84093f), -1.525879E-05f, 180f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1109.214f, 234.7699f, -50.84093f), -71.99997f, 108f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1133.827f, 256.9098f, -52.04095f), 63.00006f, 243.0001f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1108.919f, 233.9048f, -50.84093f), -144f, 36.00002f, "vw_prop_casino_slot_06a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1101.221f, 231.6943f, -50.84092f), -105f, 74.99998f, "vw_prop_casino_slot_06a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1101.323f, 232.4321f, -50.84092f), -90f, 90f, "vw_prop_casino_slot_07a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1110.974f, 238.642f, -50.84093f), -180f, 0f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1101.229f, 233.1719f, -50.84092f), -74.99993f, 105.0001f, "vw_prop_casino_slot_08a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1112.407f, 239.0216f, -50.84093f), -150f, 30.00001f, "vw_prop_casino_slot_06a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1121.592f, 230.4106f, -50.84092f), 135.0001f, 315.0001f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1135.132f, 256.699f, -52.04095f), -81f, 99f, "vw_prop_casino_slot_01a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1131.655f, 249.6264f, -52.04094f), -44.99995f, 135f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1110.229f, 238.7428f, -50.84093f), 165f, 345f, "vw_prop_casino_slot_03a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1120.853f, 231.6886f, -50.84092f), 105f, 285f, "vw_prop_casino_slot_03a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1138.799f, 253.0363f, -52.04095f), -9.000015f, 171f, "vw_prop_casino_slot_08a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1121.135f, 230.9999f, -50.84092f), 120f, 300f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1100.939f, 231.0017f, -50.84092f), -120f, 60.00003f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1100.483f, 230.4082f, -50.84092f), -134.9999f, 45.00008f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1134.759f, 255.9734f, -52.04095f), -152.9999f, 27.00006f, "vw_prop_casino_slot_02a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1134.556f, 257.2778f, -52.04095f), -9.000015f, 171f, "vw_prop_casino_slot_05a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1139.372f, 252.4563f, -52.04095f), -81f, 99f, "vw_prop_casino_slot_04a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1112.999f, 239.4742f, -50.84093f), -135.0001f, 44.99994f, "vw_prop_casino_slot_07a"));
				CasinoGames.SlotLocH.Add(new SlotsClass(new Vector3(1111.716f, 238.7384f, -50.84093f), -165f, 14.99998f, "vw_prop_casino_slot_05a"));
				foreach (SlotsClass item in CasinoGames.SlotLocH)
				{
					item.MachineProp = GlobalHelper.CreateProp(CasinoGames.RequestModel(item.Machine), item.Loc, new Vector3(0f, 0f, item.Heading), false, false);
					item.MachineProp.IsPositionFrozen = true;
				}
				Script.Wait(500);
				for (int j = 0; j < CasinoGames.SlotLocH.Count; j++)
				{
					Prop machineProp = CasinoGames.SlotLocH[j].MachineProp;
					Ped ped = null;
					Vector3 offsetPosition = machineProp.GetOffsetPosition(new Vector3(0f, -0.9f, -0.3f));
					Random random = new Random();
					int num = random.Next(0, 200);
					int num2 = random.Next(1, 6);
					if (num <= 110)
					{
						Random random2 = new Random();
						Class1.ClubDancerList.Add("a_m_y_clubcust_01");
						Class1.ClubDancerList.Add("a_m_y_clubcust_01");
						Class1.ClubDancerList.Add("a_m_y_clubcust_01");
						Class1.ClubDancerList.Add("a_f_y_clubcust_01");
						Class1.ClubDancerList.Add("a_f_y_clubcust_01");
						Class1.ClubDancerList.Add("a_f_y_clubcust_01");
						ped = GlobalHelper.CreatePed(CasinoGames.RequestModel(CasinoPeds.ambientPedModelList[random2.Next(0, CasinoPeds.ambientPedModelList.Count)]), offsetPosition);
						ped.Heading = machineProp.Heading;
						ped.Position = Class1.MarkerEnter;
						Script.Wait(15);
						ped.Position = offsetPosition;
						CasinoGames.SetRandomOutfit(ped);
						ped.IsPositionFrozen = true;
						ped.AlwaysKeepTask = true;
						ped.IsPersistent = true;
						if (num2 == 1)
						{
							ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_a", 16f, 1f, -1, AnimationFlags.Loop, -1f);
						}
						if (num2 == 2)
						{
							ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_b", 16f, 1f, -1, AnimationFlags.Loop, -1f);
						}
						if (num2 == 3)
						{
							ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_c", 16f, 1f, -1, AnimationFlags.Loop, -1f);
						}
						if (num2 == 4)
						{
							ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_d", 16f, 1f, -1, AnimationFlags.Loop, -1f);
						}
						if (num2 == 5)
						{
							ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_e", 16f, 1f, -1, AnimationFlags.Loop, -1f);
						}
						if (num2 == 6)
						{
							ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_f", 16f, 1f, -1, AnimationFlags.Loop, -1f);
						}
						if (num2 > 6)
						{
							ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_b", 16f, 1f, -1, AnimationFlags.Loop, -1f);
						}
						if (num2 < 0)
						{
							ped.Task.PlayAnimation("anim_casino_a@amb@casino@games@slots@male", "base_idle_e", 16f, 1f, -1, AnimationFlags.Loop, -1f);
						}
						Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, ped, 0, 0);
						Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 17, 1);
						Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, ped, 1);
						Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, ped, 0, 0);
						Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, ped, 17, 1);
						ped.RelationshipGroup = 1;
						ped.AlwaysKeepTask = true;
						CasinoGames.SlotMachinePeds.Add(ped);
						CasinoGames.SlotLocH[j].In_use = true;
						CasinoGames.SlotLocH[j].SlotPed = ped;
					}
					else
					{
						CasinoGames.SlotLocH[j].In_use = false;
					}
					Script.Wait(1);
				}
				CasinoGames.CreatedSlotMachines = true;
			}
			else
			{
				if (!CasinoGames.CreatedSlotMachines)
				{
					return;
				}
				CasinoGames.CreatedSlotMachines = false;
				foreach (Ped slotMachinePed2 in CasinoGames.SlotMachinePeds)
				{
					if (slotMachinePed2 != null)
					{
						slotMachinePed2.Delete();
					}
				}
				foreach (SlotsClass item2 in CasinoGames.SlotLocH)
				{
					if (item2.MachineProp != null)
					{
						item2.MachineProp.Delete();
					}
				}
				CasinoGames.SlotMachineProps.Clear();
				CasinoGames.SlotMachinePeds.Clear();
				CasinoGames.SlotLocH.Clear();
				SlotMachinesRemoved = true;
			}
		}
	}
}
