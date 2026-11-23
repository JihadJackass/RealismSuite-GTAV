using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class VinewoodGarageExterior
	{
		internal sealed class GateReceipt
		{
			public Vector3 Position;

			public Vector3 Center;

			public bool Hidden;

			public int? ClearSince;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			internal static Func<GateReceipt, bool> _0024I21_002D0;

			internal static Func<GateReceipt, bool> _0024I21_002D1;

			internal static Func<GateReceipt, bool> _0024I25_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__21_002D0(GateReceipt gate)
			{
				return gate.Hidden;
			}

			[SpecialName]
			internal bool _Lambda_0024__21_002D1(GateReceipt gate)
			{
				return gate.Hidden;
			}

			[SpecialName]
			internal bool _Lambda_0024__25_002D0(GateReceipt gate)
			{
				return gate.Hidden;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__22_002D0
		{
			public int _0024VB_0024Local_closest;

			public _Closure_0024__22_002D0(_Closure_0024__22_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_closest = arg0._0024VB_0024Local_closest;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Prop prop)
			{
				return prop.Handle == _0024VB_0024Local_closest;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__22_002D1
		{
			public Vector3 _0024VB_0024Local_point;

			public _Closure_0024__22_002D1(_Closure_0024__22_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_point = arg0._0024VB_0024Local_point;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__1(GateReceipt gate)
			{
				return gate.Position.DistanceToSquared(_0024VB_0024Local_point) < 1f;
			}
		}

		private const string FacadeIpl = "m23_2_vinewood_garage";

		private const string GateModel = "prop_fnclink_06gate2";

		private const float PreloadRadius = 75f;

		private const float MaximumHeightDifference = 10f;

		private const long CheckIntervalMs = 1000L;

		private const long GateCheckMs = 200L;

		private const float GateHideRadius = 0.8f;

		private const long ClearDelayMs = 2000L;

		private static int? lastCheckAt;

		private static int? lastGateCheckAt;

		private static int gateModelHash;

		private static bool restoreAfterLoad;

		private static int missedGateSearches;

		private static bool missingGateLogged;

		private static readonly List<GateReceipt> gates = new List<GateReceipt>();

		internal static bool ShouldPreload(ExpansionProperty definition, Vector3 position)
		{
			if (definition == null || Operators.CompareString(definition.Key, "vinewood-club-garage", false) != 0 || !definition.VehicleEntrance.HasValue || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Vector3 vector = position - Helper.ToVector3(definition.VehicleEntrance.Value);
			return Math.Abs(vector.Z) <= 10f && vector.X * vector.X + vector.Y * vector.Y <= 5625f;
		}

		internal static bool GateCandidate(Vector3 position, Vector3 entrance)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(entrance))
			{
				return false;
			}
			Vector3 vector = position - entrance;
			return Math.Abs(vector.X) <= 16f && vector.Y >= -6f && vector.Y <= 30f && Math.Abs(vector.Z) <= 5f;
		}

		internal static bool VehicleApproaching(Vector3 position, Vector3 gate)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(gate))
			{
				return false;
			}
			Vector3 vector = position - gate;
			return Math.Abs(vector.Z) <= 5f && vector.X * vector.X + vector.Y * vector.Y <= 625f;
		}

		internal static bool ClearLongEnough(int now, int? since)
		{
			if (!since.HasValue)
			{
				return false;
			}
			checked
			{
				return unchecked((long)now) - unchecked((long)since.Value) >= 2000;
			}
		}

		internal static void Tick(ExpansionProperty definition, Vector3 position, Vehicle vehicle = null)
		{
			if (definition == null || Operators.CompareString(definition.Key, "vinewood-club-garage", false) != 0 || StoryLoadGuard.WorldLoading())
			{
				return;
			}
			int gameTime = Game.GameTime;
			checked
			{
				try
				{
					bool flag = ShouldPreload(definition, position);
					if (flag && (!lastCheckAt.HasValue || unchecked((long)gameTime) - unchecked((long)lastCheckAt.Value) >= 1000 || gameTime < lastCheckAt.Value))
					{
						lastCheckAt = gameTime;
						if (!Function.Call<bool>(Hash.IS_IPL_ACTIVE, "m23_2_vinewood_garage"))
						{
							Function.Call(Hash.REQUEST_IPL, "m23_2_vinewood_garage");
						}
						if (!gates.Any((_Closure_0024__._0024I21_002D0 != null) ? _Closure_0024__._0024I21_002D0 : (_Closure_0024__._0024I21_002D0 = _Closure_0024__._0024I._Lambda_0024__21_002D0)) && !restoreAfterLoad)
						{
							DiscoverGates(definition);
							if (gates.Count == 0 && (object)vehicle != null && position.DistanceToSquared(Helper.ToVector3(definition.VehicleEntrance.Value)) < 2025f)
							{
								missedGateSearches++;
								if (missedGateSearches >= 3 && !missingGateLogged)
								{
									missingGateLogged = true;
									Logger.Log("Vinewood forecourt gate: no matching prop_fnclink_06gate2 was exposed in the bounded forecourt; gate geometry needs an in-game check.");
								}
							}
						}
					}
					else if (!flag)
					{
						missedGateSearches = 0;
						missingGateLogged = false;
					}
					if (lastGateCheckAt.HasValue && gameTime >= lastGateCheckAt.Value && unchecked((long)gameTime) - unchecked((long)lastGateCheckAt.Value) < 200)
					{
						return;
					}
					lastGateCheckAt = gameTime;
					bool flag2 = vehicle?.Exists() ?? false;
					foreach (GateReceipt gate in gates)
					{
						if (!restoreAfterLoad && flag2 && VehicleApproaching(position, gate.Center))
						{
							gate.ClearSince = null;
							if (!gate.Hidden)
							{
								Function.Call(Hash.CREATE_MODEL_HIDE, gate.Position.X, gate.Position.Y, gate.Position.Z, 0.8f, gateModelHash, true);
								gate.Hidden = true;
							}
						}
						else if (gate.Hidden)
						{
							if (PassageOccupied(gate.Center))
							{
								gate.ClearSince = null;
							}
							else if (!gate.ClearSince.HasValue || gameTime < gate.ClearSince.Value)
							{
								gate.ClearSince = gameTime;
							}
							else if (ClearLongEnough(gameTime, gate.ClearSince))
							{
								RestoreGate(gate);
							}
						}
					}
					if (restoreAfterLoad && !gates.Any((_Closure_0024__._0024I21_002D1 != null) ? _Closure_0024__._0024I21_002D1 : (_Closure_0024__._0024I21_002D1 = _Closure_0024__._0024I._Lambda_0024__21_002D1)))
					{
						gates.Clear();
						restoreAfterLoad = false;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Vinewood forecourt gate: " + ex2.Message);
					lastGateCheckAt = gameTime;
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void DiscoverGates(ExpansionProperty definition)
		{
			_Closure_0024__22_002D0 arg = new _Closure_0024__22_002D0(null);
			if (!definition.VehicleEntrance.HasValue)
			{
				return;
			}
			if (gateModelHash == 0)
			{
				gateModelHash = Game.GenerateHash("prop_fnclink_06gate2");
			}
			Vector3 vector = Helper.ToVector3(definition.VehicleEntrance.Value);
			Vector3 position = vector + new Vector3(0f, 10f, 1f);
			List<Prop> list = World.GetNearbyProps(position, 34f).ToList();
			arg._0024VB_0024Local_closest = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 34f, gateModelHash, false, false, false);
			if (arg._0024VB_0024Local_closest != 0 && !list.Any(arg._Lambda_0024__0))
			{
				Prop prop = Entity.FromHandle(arg._0024VB_0024Local_closest) as Prop;
				if ((object)prop != null)
				{
					list.Add(prop);
				}
			}
			_Closure_0024__22_002D1 closure_0024__22_002D = default(_Closure_0024__22_002D1);
			foreach (Prop item in list)
			{
				closure_0024__22_002D = new _Closure_0024__22_002D1(closure_0024__22_002D);
				if ((object)item == null || !item.Exists() || item.Model.Hash != gateModelHash)
				{
					continue;
				}
				closure_0024__22_002D._0024VB_0024Local_point = item.Position;
				if (!GateCandidate(closure_0024__22_002D._0024VB_0024Local_point, vector) || gates.Any(closure_0024__22_002D._Lambda_0024__1))
				{
					continue;
				}
				ValueTuple<Vector3, Vector3> dimensions = item.Model.Dimensions;
				Vector3 vector2 = (dimensions.Item1 + dimensions.Item2) * 0.5f;
				Vector3 vector3 = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, item.Handle, vector2.X, vector2.Y, vector2.Z);
				if (PartyPlacement.FinitePoint(vector3))
				{
					gates.Add(new GateReceipt
					{
						Position = closure_0024__22_002D._0024VB_0024Local_point,
						Center = vector3
					});
					Logger.Log(string.Format("Vinewood forecourt gate verified at runtime: model={0}; origin={1}; center={2}.", "prop_fnclink_06gate2", closure_0024__22_002D._0024VB_0024Local_point, vector3));
					if (gates.Count >= 4)
					{
						break;
					}
				}
			}
		}

		private static bool PassageOccupied(Vector3 center)
		{
			if (!Function.Call<bool>(Hash.IS_ANY_VEHICLE_NEAR_POINT, center.X, center.Y, center.Z, 14f))
			{
				return Function.Call<bool>(Hash.IS_ANY_PED_NEAR_POINT, center.X, center.Y, center.Z, 7f);
			}
			return true;
		}

		private static void RestoreGate(GateReceipt gate)
		{
			if (gate.Hidden)
			{
				Function.Call(Hash.REMOVE_MODEL_HIDE, gate.Position.X, gate.Position.Y, gate.Position.Z, 0.8f, gateModelHash, false);
				gate.Hidden = false;
				gate.ClearSince = null;
			}
		}

		internal static void Reset(bool storyLoad = false)
		{
			lastCheckAt = null;
			lastGateCheckAt = null;
			missedGateSearches = 0;
			missingGateLogged = false;
			if (storyLoad)
			{
				restoreAfterLoad = gates.Any((_Closure_0024__._0024I25_002D0 != null) ? _Closure_0024__._0024I25_002D0 : (_Closure_0024__._0024I25_002D0 = _Closure_0024__._0024I._Lambda_0024__25_002D0));
				if (!restoreAfterLoad)
				{
					gates.Clear();
				}
				return;
			}
			foreach (GateReceipt gate in gates)
			{
				try
				{
					RestoreGate(gate);
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Vinewood forecourt gate cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
			gates.Clear();
			restoreAfterLoad = false;
		}
	}
}
