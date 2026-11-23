using System;
using CayoPericoHeistInSP;
using GTA;
using GTA.Math;
using GTA.Native;
using HKHModHelperNew;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class ManageBodyguards : Script
	{
		public static int RobberA_FollowTimer;

		public static bool RobberA_Climbing;

		public static int RobberB_FollowTimer;

		public static bool RobberB_Climbing;

		public static int RobberC_FollowTimer;

		public static bool RobberC_Climbing;

		public int NextMoveCheck;

		public Vector3 LastRobberAMovePos;

		public float LastRobberAMovingSpeed;

		public Vector3 LastRobberBMovePos;

		public float LastRobberBMovingSpeed;

		public Vector3 LastRobberCMovePos;

		public float LastRobberCMovingSpeed;

		public static bool Block_Bodyguard_Mantle;

		public static Ped RobberA;

		public static Ped RobberB;

		public static Ped RobberC;

		public static int Reroute_Ai_Timer;

		public static Model LastPlayerModel;

		public static void Follow(Ped Ped)
		{
			if (Ped == RobberA && !RobberA.IsInCombat && !Class1.RobberARobbing && Class1.RobberACuttingStage == 0 && !Class1.RobberAMovingToLoot && !ManageRobberyAnims.RobberARobbing && ManageRobberyAnims.RobberACuttingStage == 0 && !ManageRobberyAnims.RobberAMovingToLoot)
			{
				Vector3 position = Game.Player.Character.Position;
				if (Class1.HeistPathfindVariation == 0)
				{
					if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">", 2f) && GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, "<", 5f))
					{
						Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position.X, position.Y, position.Z, 5f, -1, 0f, 0f);
					}
					else if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">=", 5f))
					{
						Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position.X, position.Y, position.Z, 15f, -1, 0f, 0f);
					}
					else if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, "<=", 2f))
					{
						Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position.X, position.Y, position.Z, 1f, -1, 0f, 0f);
					}
				}
				if (Class1.HeistPathfindVariation == 1 && GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">=", 5f))
				{
					Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: Ped, argument1: position.X, argument2: position.Y, argument3: position.Z, argument4: 30f, argument5: 20000, argument6: 5, argument7: 512);
				}
			}
			if (Ped == RobberB && !RobberB.IsInCombat && !Class1.RobberBRobbing && Class1.RobberBCuttingStage == 0 && !Class1.RobberBMovingToLoot && !ManageRobberyAnims.RobberBRobbing && ManageRobberyAnims.RobberBCuttingStage == 0 && !ManageRobberyAnims.RobberBMovingToLoot)
			{
				Vector3 position2 = Game.Player.Character.Position;
				if (Class1.HeistPathfindVariation == 0)
				{
					if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">", 2f) && GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, "<", 5f))
					{
						Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position2.X, position2.Y, position2.Z, 5f, -1, 0f, 0f);
					}
					else if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">=", 5f))
					{
						Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position2.X, position2.Y, position2.Z, 15f, -1, 0f, 0f);
					}
					else if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, "<=", 2f))
					{
						Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position2.X, position2.Y, position2.Z, 1f, -1, 0f, 0f);
					}
				}
				if (Class1.HeistPathfindVariation == 1 && GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">=", 5f))
				{
					Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: Ped, argument1: position2.X, argument2: position2.Y, argument3: position2.Z, argument4: 30f, argument5: 20000, argument6: 5, argument7: 512);
				}
			}
			if (!(Ped == RobberC) || RobberC.IsInCombat || Class1.RobberCRobbing || Class1.RobberCCuttingStage != 0 || Class1.RobberCMovingToLoot || ManageRobberyAnims.RobberCRobbing || ManageRobberyAnims.RobberCCuttingStage != 0 || ManageRobberyAnims.RobberCMovingToLoot)
			{
				return;
			}
			Vector3 position3 = Game.Player.Character.Position;
			if (Class1.HeistPathfindVariation == 0)
			{
				if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">", 2f) && GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, "<", 5f))
				{
					Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position3.X, position3.Y, position3.Z, 5f, -1, 0f, 0f);
				}
				else if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">=", 5f))
				{
					Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position3.X, position3.Y, position3.Z, 15f, -1, 0f, 0f);
				}
				else if (Class1.GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, "<=", 2f))
				{
					Function.Call(Hash.TASK_GO_STRAIGHT_TO_COORD, Ped, position3.X, position3.Y, position3.Z, 1f, -1, 0f, 0f);
				}
			}
			if (Class1.HeistPathfindVariation == 1 && GetDistance_VDIST2(Ped.Position, Game.Player.Character.Position, ">=", 5f))
			{
				Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: Ped, argument1: position3.X, argument2: position3.Y, argument3: position3.Z, argument4: 30f, argument5: 20000, argument6: 5, argument7: 512);
			}
		}

		public ManageBodyguards()
		{
			base.Tick += Ontick;
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

		public bool IsRobberA_BlockAdvancedFuncs()
		{
			bool result = false;
			if (Class1.RobberARobbing || Class1.RobberAMovingToLoot || Class1.RobberAMovingToLoot || Class1.RobberARobbing || Class1.RobberANearLoot || Class1.OverideTasks || TheDiamondHeist.RobberARobbing || TheDiamondHeist.RobberAMovingToLoot || TheDiamondHeist.OverideTasks || CayoPericoHeist.RobberARobbing || CayoPericoHeist.RobberAMovingToLoot || ManageRobberyAnims.RobberARobbing || ManageRobberyAnims.RobberAMovingToLoot)
			{
				result = true;
			}
			return result;
		}

		public bool IsRobberB_BlockAdvancedFuncs()
		{
			bool result = false;
			if (Class1.RobberBMovingToLoot || Class1.RobberBRobbing || Class1.OverideTasks || TheDiamondHeist.RobberBRobbing || TheDiamondHeist.RobberBMovingToLoot || TheDiamondHeist.OverideTasks || CayoPericoHeist.RobberBRobbing || CayoPericoHeist.RobberBMovingToLoot || ManageRobberyAnims.RobberBRobbing || ManageRobberyAnims.RobberBMovingToLoot)
			{
				result = true;
			}
			return result;
		}

		public bool IsRobberC_BlockAdvancedFuncs()
		{
			bool result = false;
			if (Class1.RobberCMovingToLoot || Class1.RobberCRobbing || Class1.OverideTasks || TheDiamondHeist.RobberCRobbing || TheDiamondHeist.RobberCMovingToLoot || TheDiamondHeist.OverideTasks || CayoPericoHeist.RobberCRobbing || CayoPericoHeist.RobberCMovingToLoot || ManageRobberyAnims.RobberCRobbing || ManageRobberyAnims.RobberCMovingToLoot)
			{
				result = true;
			}
			return result;
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public float GetRobberSpeed(Ped Robber)
		{
			float result = 0f;
			if (Robber != null && Game.GameTime >= NextMoveCheck)
			{
				if (Robber == RobberA)
				{
					LastRobberAMovingSpeed = GetDistance(RobberA.Position, LastRobberAMovePos);
					LastRobberAMovePos = RobberA.Position;
					NextMoveCheck = Game.GameTime + 100;
					result = LastRobberAMovingSpeed;
				}
				if (Robber == RobberB)
				{
					LastRobberBMovingSpeed = GetDistance(RobberB.Position, LastRobberBMovePos);
					LastRobberBMovePos = RobberB.Position;
					NextMoveCheck = Game.GameTime + 100;
					result = LastRobberBMovingSpeed;
				}
				if (Robber == RobberC)
				{
					LastRobberCMovingSpeed = GetDistance(RobberC.Position, LastRobberCMovePos);
					LastRobberCMovePos = RobberC.Position;
					NextMoveCheck = Game.GameTime + 100;
					result = LastRobberCMovingSpeed;
				}
			}
			return result;
		}

		public void AdvancedBodyguardFunctions(Ped Companion)
		{
			if (Block_Bodyguard_Mantle)
			{
				return;
			}
			if (Companion == RobberA && RobberA != null && !IsRobberA_BlockAdvancedFuncs())
			{
				if (Game.Player.Character.CurrentVehicle != null && RobberA.CurrentVehicle == null)
				{
					if (RobberA.VehicleTryingToEnter != Game.Player.Character.CurrentVehicle)
					{
						RobberA.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, 2000, 5f);
					}
					if (Game.IsControlJustPressed(Control.VehicleHorn) && RobberA.VehicleTryingToEnter != Game.Player.Character.CurrentVehicle)
					{
						RobberA.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, 2000, 5f);
					}
				}
				if (RobberA.CurrentVehicle == null && RobberA.VehicleTryingToEnter == null && !RobberA.IsTryingToEnterALockedVehicle && !RobberA.IsGettingIntoVehicle)
				{
					if (RobberA_FollowTimer == 0 && !RobberA.IsInCombat)
					{
						Vector3 offsetPosition = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						if (GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, ">=", 2f))
						{
							Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: Companion, argument1: offsetPosition.X, argument2: offsetPosition.Y, argument3: offsetPosition.Z, argument4: 30f, argument5: 20000, argument6: 5f, argument7: 512);
						}
					}
					if (RobberA_FollowTimer != 24)
					{
						RobberA_FollowTimer++;
					}
					if (RobberA_FollowTimer >= 24)
					{
						RobberA_FollowTimer = 0;
					}
				}
				if (RobberA != null && Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, RobberA) == 0)
				{
					if (!Game.Player.Character.IsInCombat && !RobberA.IsInCombat && Game.Player.Character.CurrentVehicle == null)
					{
						if (RobberA_Climbing && !Function.Call<bool>(Hash.IS_PED_CLIMBING, RobberA))
						{
							RobberA_Climbing = false;
						}
						Vector3 _ = new Vector3(0f, 0f, 0f);
						Vector3 offsetPosition2 = RobberA.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
						Vector3 offsetPosition3 = RobberA.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
						Vector3 offsetPosition4 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
						RaycastResult raycastResult = World.Raycast(offsetPosition2, offsetPosition4, IntersectFlags.LosToEntity);
						RaycastResult raycastResult2 = World.Raycast(offsetPosition3, Companion.GetOffsetPosition(new Vector3(0f, 0.4f, 10f)), IntersectFlags.Everything);
						bool flag = false;
						Entity entity = null;
						if (raycastResult2.DidHit)
						{
							entity = raycastResult2.HitEntity;
							flag = (GetDistance_VDIST2(offsetPosition2, raycastResult2.HitPosition, ">=", 3f) ? true : false);
							if (GetDistance_VDIST2(offsetPosition4, raycastResult2.HitPosition, "<", 1f))
							{
								flag = true;
							}
						}
						else
						{
							flag = true;
						}
						bool flag2 = false;
						Model model = null;
						if (entity != null)
						{
							model = entity.Model;
						}
						if (raycastResult.DidHit)
						{
							_ = raycastResult.HitPosition;
							flag2 = !(raycastResult.HitPosition.DistanceTo(offsetPosition4) < 0.25f);
						}
						bool flag3 = false;
						if (flag2 && flag && (double)GetRobberSpeed(RobberA) < 0.25 && (GetDistance_VDIST2(Companion.Position, _, "<=", 2.5f) || flag3) && !RobberA_Climbing)
						{
							flag3 = false;
							Function.Call(Hash.TASK_CLIMB, Companion, false);
							RobberA_Climbing = true;
						}
					}
					if (!RobberA_Climbing)
					{
						Vector3 vector = new Vector3(0f, 0f, 0f);
						Vector3 offsetPosition5 = Companion.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
						Vector3 offsetPosition6 = Companion.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
						Vector3 offsetPosition7 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
						RaycastResult raycastResult3 = World.Raycast(offsetPosition5, offsetPosition7, IntersectFlags.Everything);
						Entity entity2 = null;
						if (raycastResult3.DidHit)
						{
							vector = raycastResult3.HitPosition;
							entity2 = raycastResult3.HitEntity;
							if (entity2 != null && entity2 != Game.Player.Character && entity2 != Companion)
							{
								bool isPed = entity2.Model.IsPed;
							}
							if (Companion != null && (double)GetRobberSpeed(RobberA) < 0.25 && GetDistance_VDIST2(Game.Player.Character.Position, vector, ">", 1f) && GetDistance_VDIST2(Companion.Position, vector, "<=", 2.5f) && entity2 != Game.Player.Character && entity2 != Game.Player.Character && entity2 != Companion && entity2 != null && !entity2.Model.IsPed && entity2.Model.IsVehicle)
							{
								Function.Call(Hash.TASK_CLIMB, Companion, false);
								RobberA_Climbing = true;
							}
						}
					}
				}
			}
			if (Companion == RobberB && RobberB != null && !IsRobberB_BlockAdvancedFuncs())
			{
				if (Game.Player.Character.CurrentVehicle != null && RobberB.CurrentVehicle == null)
				{
					if (RobberB.VehicleTryingToEnter != Game.Player.Character.CurrentVehicle)
					{
						RobberB.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, 2000, 5f);
					}
					if (Game.IsControlJustPressed(Control.VehicleHorn) && RobberB.VehicleTryingToEnter != Game.Player.Character.CurrentVehicle)
					{
						RobberB.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, 2000, 5f);
					}
				}
				if (RobberB.CurrentVehicle == null && RobberB.VehicleTryingToEnter == null && !RobberB.IsTryingToEnterALockedVehicle && !RobberB.IsGettingIntoVehicle)
				{
					if (RobberB_FollowTimer == 0 && !RobberB.IsInCombat)
					{
						Vector3 offsetPosition8 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
						if (GetDistance_VDIST2(RobberB.Position, Game.Player.Character.Position, ">=", 2f))
						{
							Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: Companion, argument1: offsetPosition8.X, argument2: offsetPosition8.Y, argument3: offsetPosition8.Z, argument4: 30f, argument5: 20000, argument6: 5f, argument7: 512);
						}
					}
					if (RobberB_FollowTimer != 24)
					{
						RobberB_FollowTimer++;
					}
					if (RobberB_FollowTimer >= 24)
					{
						RobberB_FollowTimer = 0;
					}
				}
				if (RobberB != null && Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, RobberB) == 0)
				{
					if (!Game.Player.Character.IsInCombat && !RobberB.IsInCombat && Game.Player.Character.CurrentVehicle == null)
					{
						if (RobberB_Climbing && !Function.Call<bool>(Hash.IS_PED_CLIMBING, RobberB))
						{
							RobberB_Climbing = false;
						}
						Vector3 _2 = new Vector3(0f, 0f, 0f);
						Vector3 offsetPosition9 = RobberB.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
						Vector3 offsetPosition10 = RobberB.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
						Vector3 offsetPosition11 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
						RaycastResult raycastResult4 = World.Raycast(offsetPosition9, offsetPosition11, IntersectFlags.LosToEntity);
						RaycastResult raycastResult5 = World.Raycast(offsetPosition10, Companion.GetOffsetPosition(new Vector3(0f, 0.4f, 10f)), IntersectFlags.Everything);
						bool flag4 = false;
						Entity entity3 = null;
						if (raycastResult5.DidHit)
						{
							entity3 = raycastResult5.HitEntity;
							flag4 = (GetDistance_VDIST2(offsetPosition9, raycastResult5.HitPosition, ">=", 3f) ? true : false);
							if (GetDistance_VDIST2(offsetPosition11, raycastResult5.HitPosition, "<", 1f))
							{
								flag4 = true;
							}
						}
						else
						{
							flag4 = true;
						}
						bool flag5 = false;
						Model model2 = null;
						if (entity3 != null)
						{
							model2 = entity3.Model;
						}
						if (raycastResult4.DidHit)
						{
							_2 = raycastResult4.HitPosition;
							flag5 = !(raycastResult4.HitPosition.DistanceTo(offsetPosition11) < 0.25f);
						}
						bool flag6 = false;
						if (flag5 && flag4 && (double)GetRobberSpeed(RobberB) < 0.25 && (GetDistance_VDIST2(Companion.Position, _2, "<=", 2.5f) || flag6) && !RobberB_Climbing)
						{
							flag6 = false;
							Function.Call(Hash.TASK_CLIMB, Companion, false);
							RobberB_Climbing = true;
						}
					}
					if (!RobberB_Climbing)
					{
						Vector3 vector2 = new Vector3(0f, 0f, 0f);
						Vector3 offsetPosition12 = Companion.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
						Vector3 offsetPosition13 = Companion.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
						Vector3 offsetPosition14 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
						RaycastResult raycastResult6 = World.Raycast(offsetPosition12, offsetPosition14, IntersectFlags.Everything);
						Entity entity4 = null;
						if (raycastResult6.DidHit)
						{
							vector2 = raycastResult6.HitPosition;
							entity4 = raycastResult6.HitEntity;
							if (entity4 != null && entity4 != Game.Player.Character && entity4 != Companion)
							{
								bool isPed2 = entity4.Model.IsPed;
							}
							if (Companion != null && (double)GetRobberSpeed(RobberB) < 0.25 && GetDistance_VDIST2(Game.Player.Character.Position, vector2, ">", 1f) && GetDistance_VDIST2(Companion.Position, vector2, "<=", 2.5f) && entity4 != Game.Player.Character && entity4 != Game.Player.Character && entity4 != Companion && entity4 != null && !entity4.Model.IsPed && entity4.Model.IsVehicle)
							{
								Function.Call(Hash.TASK_CLIMB, Companion, false);
								RobberB_Climbing = true;
							}
						}
					}
				}
			}
			if (!(Companion == RobberC) || !(RobberC != null) || IsRobberC_BlockAdvancedFuncs())
			{
				return;
			}
			if (Game.Player.Character.CurrentVehicle != null && RobberC.CurrentVehicle == null)
			{
				if (RobberC.VehicleTryingToEnter != Game.Player.Character.CurrentVehicle)
				{
					RobberC.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, 2000, 5f);
				}
				if (Game.IsControlJustPressed(Control.VehicleHorn) && RobberC.VehicleTryingToEnter != Game.Player.Character.CurrentVehicle)
				{
					RobberC.Task.EnterVehicle(Game.Player.Character.CurrentVehicle, VehicleSeat.Any, 2000, 5f);
				}
			}
			if (RobberC.CurrentVehicle == null && RobberC.VehicleTryingToEnter == null && !RobberC.IsTryingToEnterALockedVehicle && !RobberC.IsGettingIntoVehicle)
			{
				if (RobberC_FollowTimer == 0 && !RobberC.IsInCombat)
				{
					Vector3 offsetPosition15 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, 0f));
					if (GetDistance_VDIST2(RobberC.Position, Game.Player.Character.Position, ">=", 2f))
					{
						Function.Call(argument8: Game.Player.Character.Rotation.Z, hash: Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, argument0: Companion, argument1: offsetPosition15.X, argument2: offsetPosition15.Y, argument3: offsetPosition15.Z, argument4: 30f, argument5: 20000, argument6: 5f, argument7: 512);
					}
				}
				if (RobberC_FollowTimer != 24)
				{
					RobberC_FollowTimer++;
				}
				if (RobberC_FollowTimer >= 24)
				{
					RobberC_FollowTimer = 0;
				}
			}
			if (!(RobberC != null) || Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, RobberC) != 0)
			{
				return;
			}
			if (!Game.Player.Character.IsInCombat && !RobberC.IsInCombat && Game.Player.Character.CurrentVehicle == null)
			{
				if (RobberC_Climbing && !Function.Call<bool>(Hash.IS_PED_CLIMBING, RobberC))
				{
					RobberC_Climbing = false;
				}
				Vector3 _3 = new Vector3(0f, 0f, 0f);
				Vector3 offsetPosition16 = RobberC.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
				Vector3 offsetPosition17 = RobberC.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
				Vector3 offsetPosition18 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
				RaycastResult raycastResult7 = World.Raycast(offsetPosition16, offsetPosition18, IntersectFlags.LosToEntity);
				RaycastResult raycastResult8 = World.Raycast(offsetPosition17, Companion.GetOffsetPosition(new Vector3(0f, 0.4f, 10f)), IntersectFlags.Everything);
				bool flag7 = false;
				Entity entity5 = null;
				if (raycastResult8.DidHit)
				{
					entity5 = raycastResult8.HitEntity;
					flag7 = (GetDistance_VDIST2(offsetPosition16, raycastResult8.HitPosition, ">=", 3f) ? true : false);
					if (GetDistance_VDIST2(offsetPosition18, raycastResult8.HitPosition, "<", 1f))
					{
						flag7 = true;
					}
				}
				else
				{
					flag7 = true;
				}
				bool flag8 = false;
				Model model3 = null;
				if (entity5 != null)
				{
					model3 = entity5.Model;
				}
				if (raycastResult7.DidHit)
				{
					_3 = raycastResult7.HitPosition;
					flag8 = !(raycastResult7.HitPosition.DistanceTo(offsetPosition18) < 0.25f);
				}
				bool flag9 = false;
				if (flag8 && flag7 && (double)GetRobberSpeed(RobberC) < 0.25 && (GetDistance_VDIST2(Companion.Position, _3, "<=", 2.5f) || flag9) && !RobberC_Climbing)
				{
					flag9 = false;
					Function.Call(Hash.TASK_CLIMB, Companion, false);
					RobberC_Climbing = true;
				}
			}
			if (RobberC_Climbing)
			{
				return;
			}
			Vector3 vector3 = new Vector3(0f, 0f, 0f);
			Vector3 offsetPosition19 = Companion.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
			Vector3 offsetPosition20 = Companion.GetOffsetPosition(new Vector3(0f, 0.8f, -0.8f));
			Vector3 offsetPosition21 = Game.Player.Character.GetOffsetPosition(new Vector3(0f, 0f, -0.8f));
			RaycastResult raycastResult9 = World.Raycast(offsetPosition19, offsetPosition21, IntersectFlags.Everything);
			Entity entity6 = null;
			if (raycastResult9.DidHit)
			{
				vector3 = raycastResult9.HitPosition;
				entity6 = raycastResult9.HitEntity;
				if (entity6 != null && entity6 != Game.Player.Character && entity6 != Companion)
				{
					bool isPed3 = entity6.Model.IsPed;
				}
				if (Companion != null && (double)GetRobberSpeed(RobberC) < 0.25 && GetDistance_VDIST2(Game.Player.Character.Position, vector3, ">", 1f) && GetDistance_VDIST2(Companion.Position, vector3, "<=", 2.5f) && entity6 != Game.Player.Character && entity6 != Game.Player.Character && entity6 != Companion && entity6 != null && !entity6.Model.IsPed && entity6.Model.IsVehicle)
				{
					Function.Call(Hash.TASK_CLIMB, Companion, false);
					RobberC_Climbing = true;
				}
			}
		}

		public void Ontick(object sender, EventArgs e)
		{
			int num = Function.Call<int>(Hash.GET_PLAYER_GROUP, Game.Player);
			Ped ped = Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num, 0);
			if (ped != null)
			{
				BodyguardMenu.RobberA = ped;
			}
			ped = Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num, 1);
			if (ped != null)
			{
				BodyguardMenu.RobberB = ped;
			}
			ped = Function.Call<Ped>(Hash.GET_PED_AS_GROUP_MEMBER, num, 2);
			if (ped != null)
			{
				BodyguardMenu.RobberC = ped;
			}
			if (BodyguardMenu.RobberA == null && BodyguardMenu.RobberB == null && BodyguardMenu.RobberC == null)
			{
				return;
			}
			if (Game.Player.Character.Model != LastPlayerModel)
			{
				LastPlayerModel = Game.Player.Character.Model;
				if (BodyguardMenu.RobberA != null)
				{
					Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, BodyguardMenu.RobberA, false);
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, BodyguardMenu.RobberA);
					BodyguardMenu.RobberA.BlockPermanentEvents = true;
					BodyguardMenu.RobberA.RelationshipGroup = 2;
					PedGroup pedGroup = Game.Player.Character.PedGroup;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, BodyguardMenu.RobberA, pedGroup);
					Function.Call(Hash.SET_PED_COMBAT_ABILITY, BodyguardMenu.RobberA, 100);
				}
				if (BodyguardMenu.RobberB != null)
				{
					Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, BodyguardMenu.RobberB, false);
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, BodyguardMenu.RobberB);
					BodyguardMenu.RobberB.BlockPermanentEvents = true;
					BodyguardMenu.RobberB.RelationshipGroup = 2;
					PedGroup pedGroup2 = Game.Player.Character.PedGroup;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, BodyguardMenu.RobberB, pedGroup2);
					Function.Call(Hash.SET_PED_COMBAT_ABILITY, BodyguardMenu.RobberB, 100);
				}
				if (BodyguardMenu.RobberC != null)
				{
					Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, BodyguardMenu.RobberC, false);
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, BodyguardMenu.RobberC);
					BodyguardMenu.RobberC.BlockPermanentEvents = true;
					BodyguardMenu.RobberC.RelationshipGroup = 2;
					num = Function.Call<int>(Hash.GET_PLAYER_GROUP, Game.Player);
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, BodyguardMenu.RobberC, num);
					PedGroup pedGroup3 = Game.Player.Character.PedGroup;
					Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, BodyguardMenu.RobberC, pedGroup3);
					Function.Call(Hash.SET_PED_COMBAT_ABILITY, BodyguardMenu.RobberC, 100);
				}
			}
			if (BodyguardMenu.CameraON)
			{
				return;
			}
			if (BodyguardMenu.RobberA != null && BodyguardMenu.RobberA.Exists())
			{
				BodyguardMenu.RobberA.Opacity = 255;
				BodyguardMenu.RobberA.IsVisible = true;
				if (BodyguardMenu.RobberA.IsAlive)
				{
					RobberA = BodyguardMenu.RobberA;
					if (!Class1.RobberARobbing && Class1.RobberACuttingStage == 0 && !Class1.RobberAMovingToLoot && !ManageRobberyAnims.RobberARobbing && ManageRobberyAnims.RobberACuttingStage == 0 && !ManageRobberyAnims.RobberAMovingToLoot && Function.Call<bool>(Hash.IS_PED_IN_GROUP))
					{
						Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberA, false);
						Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberA);
						RobberA.BlockPermanentEvents = true;
						RobberA.RelationshipGroup = 2;
					}
					Function.Call(Hash.SET_PED_CONFIG_FLAG, RobberA, 107, true);
					Function.Call(Hash.SET_PED_CONFIG_FLAG, RobberA, 109, true);
					Class1.RobberA = RobberA;
					TheDiamondHeist.RobberA = RobberA;
					CayoPericoHeist.RobberA = RobberA;
					ManageRobberyAnims.RobberA = RobberA;
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberA, 0);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberA, 4);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberA, 13);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberA, 14);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberA, 15);
				}
				else
				{
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, BodyguardMenu.RobberA);
					RobberA = null;
					Class1.RobberA = null;
					TheDiamondHeist.RobberA = null;
					CayoPericoHeist.RobberA = null;
					ManageRobberyAnims.RobberA = null;
					BodyguardMenu.RobberA = null;
				}
			}
			if (BodyguardMenu.RobberB != null && BodyguardMenu.RobberB.Exists())
			{
				BodyguardMenu.RobberB.Opacity = 255;
				BodyguardMenu.RobberB.IsVisible = true;
				if (BodyguardMenu.RobberB.IsAlive)
				{
					RobberB = BodyguardMenu.RobberB;
					if (!Class1.RobberBRobbing && Class1.RobberBCuttingStage == 0 && !Class1.RobberBMovingToLoot && !ManageRobberyAnims.RobberBRobbing && ManageRobberyAnims.RobberBCuttingStage == 0 && !ManageRobberyAnims.RobberBMovingToLoot && Function.Call<bool>(Hash.IS_PED_IN_GROUP))
					{
						Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberB, false);
						Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberB);
						RobberB.BlockPermanentEvents = true;
						RobberB.RelationshipGroup = 2;
					}
					Class1.RobberB = RobberB;
					TheDiamondHeist.RobberB = RobberB;
					CayoPericoHeist.RobberB = RobberB;
					ManageRobberyAnims.RobberB = RobberB;
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberB, 0);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberB, 4);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberB, 13);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberB, 14);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberB, 15);
				}
				else
				{
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, BodyguardMenu.RobberB);
					RobberB = null;
					Class1.RobberB = null;
					TheDiamondHeist.RobberB = null;
					CayoPericoHeist.RobberB = null;
					ManageRobberyAnims.RobberB = null;
					BodyguardMenu.RobberB = null;
				}
			}
			if (BodyguardMenu.RobberC != null && BodyguardMenu.RobberC.Exists())
			{
				BodyguardMenu.RobberC.Opacity = 255;
				BodyguardMenu.RobberC.IsVisible = true;
				if (BodyguardMenu.RobberC.IsAlive)
				{
					RobberC = BodyguardMenu.RobberC;
					if (!Class1.RobberCRobbing && Class1.RobberCCuttingStage == 0 && !Class1.RobberCMovingToLoot && !ManageRobberyAnims.RobberCRobbing && ManageRobberyAnims.RobberCCuttingStage == 0 && !ManageRobberyAnims.RobberCMovingToLoot && Function.Call<bool>(Hash.IS_PED_IN_GROUP))
					{
						Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, RobberC, false);
						Function.Call(Hash.REMOVE_PED_FROM_GROUP, RobberC);
						RobberC.BlockPermanentEvents = true;
						RobberC.RelationshipGroup = 2;
					}
					Function.Call(Hash.SET_PED_CONFIG_FLAG, RobberC, 107, true);
					Function.Call(Hash.SET_PED_CONFIG_FLAG, RobberC, 109, true);
					Class1.RobberC = RobberC;
					TheDiamondHeist.RobberC = RobberC;
					CayoPericoHeist.RobberC = RobberC;
					ManageRobberyAnims.RobberC = RobberC;
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberC, 0);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberC, 4);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberC, 13);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberC, 14);
					Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, RobberC, 15);
				}
				else
				{
					Function.Call(Hash.REMOVE_PED_FROM_GROUP, BodyguardMenu.RobberC);
					RobberC = null;
					Class1.RobberC = null;
					TheDiamondHeist.RobberC = null;
					CayoPericoHeist.RobberC = null;
					ManageRobberyAnims.RobberC = null;
					BodyguardMenu.RobberC = null;
				}
			}
			if (Game.GameTime >= Reroute_Ai_Timer)
			{
				Reroute_Ai_Timer = Game.GameTime + 750;
				if (RobberA != null && GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, ">=", 2.5f) && Game.Player.Character.CurrentVehicle == null)
				{
					Follow(RobberA);
				}
				if (RobberB != null && GetDistance_VDIST2(RobberB.Position, Game.Player.Character.Position, ">=", 2.5f) && Game.Player.Character.CurrentVehicle == null)
				{
					Follow(RobberB);
				}
				if (RobberC != null && GetDistance_VDIST2(RobberC.Position, Game.Player.Character.Position, ">=", 2.5f) && Game.Player.Character.CurrentVehicle == null)
				{
					Follow(RobberC);
				}
			}
			if (RobberA != null)
			{
				if (RobberA == HKHModHelperNew.Class1.PlayerCompanion_Companion)
				{
					if (!HKHModHelperNew.Class1.PlayerCompanion_Block_SPH_Follow)
					{
						AdvancedBodyguardFunctions(RobberA);
						if (GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, ">", 50f))
						{
							Vector3 safeCoordForPed = World.GetSafeCoordForPed(Game.Player.Character.Position);
							RobberA.Position = safeCoordForPed;
						}
					}
				}
				else
				{
					AdvancedBodyguardFunctions(RobberA);
					if (GetDistance_VDIST2(RobberA.Position, Game.Player.Character.Position, ">", 50f))
					{
						Vector3 safeCoordForPed2 = World.GetSafeCoordForPed(Game.Player.Character.Position);
						RobberA.Position = safeCoordForPed2;
					}
				}
			}
			if (RobberB != null)
			{
				AdvancedBodyguardFunctions(RobberB);
				if (GetDistance_VDIST2(RobberB.Position, Game.Player.Character.Position, ">", 50f))
				{
					Vector3 safeCoordForPed3 = World.GetSafeCoordForPed(Game.Player.Character.Position);
					RobberB.Position = safeCoordForPed3;
				}
			}
			if (RobberC != null)
			{
				AdvancedBodyguardFunctions(RobberC);
				if (GetDistance_VDIST2(RobberC.Position, Game.Player.Character.Position, ">", 50f))
				{
					Vector3 safeCoordForPed4 = World.GetSafeCoordForPed(Game.Player.Character.Position);
					RobberC.Position = safeCoordForPed4;
				}
			}
		}
	}
}
