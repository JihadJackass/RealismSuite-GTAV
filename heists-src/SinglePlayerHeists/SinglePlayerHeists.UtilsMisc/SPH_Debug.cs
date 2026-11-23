using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class SPH_Debug : Script
	{
		public static bool DebugOn;

		public static bool CreatedSilthouseint;

		public static bool ShowRobberyMarkers;

		public static bool ShowPaintingOffsets;

		public static bool ShowMainDebug;

		public static bool ShowAiLootingDebug;

		public static bool ShowAiChevrons;

		public static bool AllowSaveNewLocateAVeh;

		public static string LastInputtedCommand;

		public static int InputtedCommandStage;

		public static bool ShowRunSmartFuncs_AiAmt;

		public static bool CasinoDrillDebug;

		public static bool GetNearbyLootFromProps;

		public static bool ShowCrackSafeMSGsDebug;

		public static bool ShowRunningClassDebug;

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public SPH_Debug()
		{
			if (Game.Player.Name.ToUpper().Equals("CINNBARGNU"))
			{
				base.Tick += OnTick;
			}
		}

		public void DrawLine(Vector3 From, Vector3 To, Vector3 RGB)
		{
			Function.Call(Hash.DRAW_LINE, From.X, From.Y, From.Z, To.X, To.Y, To.Z, RGB.X, RGB.Y, RGB.Z, 255);
		}

		private void OnTick(object sender, EventArgs e)
		{
			if (!ShowPaintingOffsets || !ShowPaintingOffsets)
			{
				return;
			}
			Vector3 position = Game.Player.Character.Position;
			Prop prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01a"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult.DidHit)
				{
					float distance = GetDistance(prop.Position, raycastResult.HitPosition);
					if ((double)distance >= 0.95 && (double)distance <= 1.05)
					{
						DrawLine(prop.Position, raycastResult.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01b"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult2 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult2.DidHit)
				{
					float distance2 = GetDistance(prop.Position, raycastResult2.HitPosition);
					if ((double)distance2 >= 0.95 && (double)distance2 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult2.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult2.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01c"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult3 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult3.DidHit)
				{
					float distance3 = GetDistance(prop.Position, raycastResult3.HitPosition);
					if ((double)distance3 >= 0.95 && (double)distance3 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult3.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult3.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01d"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult4 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult4.DidHit)
				{
					float distance4 = GetDistance(prop.Position, raycastResult4.HitPosition);
					if ((double)distance4 >= 0.95 && (double)distance4 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult4.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult4.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01e"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult5 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult5.DidHit)
				{
					float distance5 = GetDistance(prop.Position, raycastResult5.HitPosition);
					if ((double)distance5 >= 0.95 && (double)distance5 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult5.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult5.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01f"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult6 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult6.DidHit)
				{
					float distance6 = GetDistance(prop.Position, raycastResult6.HitPosition);
					if ((double)distance6 >= 0.95 && (double)distance6 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult6.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult6.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01g"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult7 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult7.DidHit)
				{
					float distance7 = GetDistance(prop.Position, raycastResult7.HitPosition);
					if ((double)distance7 >= 0.95 && (double)distance7 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult7.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult7.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01h"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult8 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult8.DidHit)
				{
					float distance8 = GetDistance(prop.Position, raycastResult8.HitPosition);
					if ((double)distance8 >= 0.95 && (double)distance8 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult8.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult8.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01i"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult9 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult9.DidHit)
				{
					float distance9 = GetDistance(prop.Position, raycastResult9.HitPosition);
					if ((double)distance9 >= 0.95 && (double)distance9 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult9.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult9.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "ch_prop_vault_painting_01j"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult10 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult10.DidHit)
				{
					float distance10 = GetDistance(prop.Position, raycastResult10.HitPosition);
					if ((double)distance10 >= 0.95 && (double)distance10 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult10.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult10.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01a"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult11 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult11.DidHit)
				{
					float distance11 = GetDistance(prop.Position, raycastResult11.HitPosition);
					if ((double)distance11 >= 0.95 && (double)distance11 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult11.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult11.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01b"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult12 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult12.DidHit)
				{
					float distance12 = GetDistance(prop.Position, raycastResult12.HitPosition);
					if ((double)distance12 >= 0.95 && (double)distance12 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult12.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult12.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01c"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult13 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult13.DidHit)
				{
					float distance13 = GetDistance(prop.Position, raycastResult13.HitPosition);
					if ((double)distance13 >= 0.95 && (double)distance13 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult13.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult13.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01d"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult14 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult14.DidHit)
				{
					float distance14 = GetDistance(prop.Position, raycastResult14.HitPosition);
					if ((double)distance14 >= 0.95 && (double)distance14 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult14.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult14.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01e"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult15 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult15.DidHit)
				{
					float distance15 = GetDistance(prop.Position, raycastResult15.HitPosition);
					if ((double)distance15 >= 0.95 && (double)distance15 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult15.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult15.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01f"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult16 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult16.DidHit)
				{
					float distance16 = GetDistance(prop.Position, raycastResult16.HitPosition);
					if ((double)distance16 >= 0.95 && (double)distance16 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult16.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult16.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01g"), 0, 0, 0);
			if (prop != null)
			{
				RaycastResult raycastResult17 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
				if (raycastResult17.DidHit)
				{
					float distance17 = GetDistance(prop.Position, raycastResult17.HitPosition);
					if ((double)distance17 >= 0.95 && (double)distance17 <= 1.05)
					{
						DrawLine(prop.Position, raycastResult17.HitPosition, new Vector3(0f, 255f, 0f));
					}
					else
					{
						DrawLine(prop.Position, raycastResult17.HitPosition, new Vector3(255f, 0f, 0f));
					}
				}
			}
			prop = Function.Call<Prop>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 10f, Function.Call<int>(Hash.GET_HASH_KEY, "h4_prop_h4_painting_01h"), 0, 0, 0);
			if (!(prop != null))
			{
				return;
			}
			RaycastResult raycastResult18 = World.Raycast(prop.GetOffsetPosition(new Vector3(0f, 0f, -0.5f)), prop.GetOffsetPosition(new Vector3(0f, 0f, -10.5f)), IntersectFlags.Everything);
			if (raycastResult18.DidHit)
			{
				float distance18 = GetDistance(prop.Position, raycastResult18.HitPosition);
				if ((double)distance18 >= 0.95 && (double)distance18 <= 1.05)
				{
					DrawLine(prop.Position, raycastResult18.HitPosition, new Vector3(0f, 255f, 0f));
				}
				else
				{
					DrawLine(prop.Position, raycastResult18.HitPosition, new Vector3(255f, 0f, 0f));
				}
			}
		}
	}
}
