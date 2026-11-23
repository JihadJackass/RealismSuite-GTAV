using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class ExpansionExterior
	{
		private sealed class SignState
		{
			public int Handle;

			public int ModelHash;

			public IntPtr Address;

			public int NextAttempt;

			public bool Retiring;
		}

		private const float MarkerDistanceSquared = 225f;

		private const float SignDistanceSquared = 400f;

		private const float SignRetireDistanceSquared = 4225f;

		private static readonly Dictionary<ExpansionProperty, SignState> signs = new Dictionary<ExpansionProperty, SignState>();

		private static Model? requestedModel;

		private static int requestedAt;

		private static int nextSweep;

		public static void BeginFrame(Ped actor)
		{
			if (signs.Count == 0 && !requestedModel.HasValue)
			{
				return;
			}
			int gameTime = Game.GameTime;
			checked
			{
				if (requestedModel.HasValue && gameTime - requestedAt >= 5000)
				{
					ReleaseModel();
				}
				if (signs.Count == 0 || gameTime < nextSweep)
				{
					return;
				}
				nextSweep = gameTime + 2000;
				Vector3 position = actor.Position;
				KeyValuePair<ExpansionProperty, SignState>[] array = signs.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<ExpansionProperty, SignState> keyValuePair = array[i];
					if (keyValuePair.Value.Retiring || position.DistanceToSquared(Helper.ToVector3(keyValuePair.Key.Entrance)) > 4225f)
					{
						RemoveSign(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}

		public static void Tick(ExpansionProperty definition, Ped actor, Vehicle vehicle, bool owned, bool forSale)
		{
			Vector3 vector = vehicle?.Position ?? actor.Position;
			Vector3 vector2 = Helper.ToVector3(definition.Entrance);
			float num = vector.DistanceToSquared(vector2);
			if (owned || forSale)
			{
				bool flag = num <= 225f;
				bool flag2 = forSale && definition.AllowDrivewayPurchase && (object)vehicle != null;
				if ((owned || flag2) && ((object)vehicle != null || definition.OnEnterGarage != null) && definition.VehicleEntrance.HasValue && definition.OnEnterVehicle != null)
				{
					Vector3 vector3 = Helper.ToVector3(definition.VehicleEntrance.Value);
					if (vector.DistanceToSquared(vector3) <= 225f)
					{
						if (Operators.CompareString(definition.Key, "vinewood-club-garage", false) == 0)
						{
							Helper.DrawMarker(vector3, default(Color), new Vector3(5.5f, 5.5f, 0.16f));
						}
						else
						{
							Helper.DrawMarker(vector3);
						}
						if (((object)vehicle != null && definition.AllowDrivewayPurchase) || vector2.DistanceToSquared(vector3) <= 1f)
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					Helper.DrawMarker(vector2);
				}
			}
			SignState value = null;
			signs.TryGetValue(definition, out value);
			if (!forSale)
			{
				if (value != null && !value.Retiring)
				{
					Purchased(definition);
				}
			}
			else
			{
				if (num > 400f)
				{
					return;
				}
				if (value == null)
				{
					value = new SignState();
					signs.Add(definition, value);
				}
				if (Game.GameTime < value.NextAttempt)
				{
					return;
				}
				value.NextAttempt = checked(Game.GameTime + 2000);
				try
				{
					if ((object)ResolveSign(value) == null)
					{
						value.Handle = 0;
						SpawnSign(definition, actor, value);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Expansion sale sign " + definition.Key + ": " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void Purchased(ExpansionProperty definition)
		{
			SignState value = null;
			if (signs.TryGetValue(definition, out value))
			{
				RemoveSign(definition, value);
			}
		}

		public static void Reset(bool storyLoad)
		{
			if (!storyLoad)
			{
				KeyValuePair<ExpansionProperty, SignState>[] array = signs.ToArray();
				for (int i = 0; i < array.Length; i = checked(i + 1))
				{
					KeyValuePair<ExpansionProperty, SignState> keyValuePair = array[i];
					RemoveSign(keyValuePair.Key, keyValuePair.Value);
				}
				ReleaseModel();
			}
			signs.Clear();
			requestedModel = null;
			nextSweep = 0;
		}

		private static void SpawnSign(ExpansionProperty definition, Ped actor, SignState state)
		{
			Model model = new Model(Helper.ForSaleSign);
			checked
			{
				if (!model.IsLoaded)
				{
					if (!requestedModel.HasValue)
					{
						model.Request();
						requestedModel = model;
						requestedAt = Game.GameTime;
					}
					state.NextAttempt = Game.GameTime + 250;
					return;
				}
				try
				{
					Vector3 min;
					Vector3 max;
					model.GetDimensions(out min, out max);
					if (!MansionSignPlacement.ValidBounds(min, max))
					{
						return;
					}
					Quaternion[] array = SignCandidates(definition.Entrance, definition.VehicleEntrance, definition.VehicleExit, max.X - min.X);
					Quaternion pose = default(Quaternion);
					for (int i = 0; i < array.Length; i++)
					{
						if (!TryGround(array[i], actor, min, max, ref pose))
						{
							continue;
						}
						Prop prop = World.CreateProp(model, Helper.ToVector3(pose), new Vector3(0f, 0f, pose.W), false, false);
						if ((object)prop != null && prop.Exists())
						{
							state.Handle = prop.Handle;
							state.ModelHash = prop.Model.Hash;
							state.Address = prop.MemoryAddress;
							prop.IsCollisionEnabled = false;
							if (state.Address == IntPtr.Zero)
							{
								prop.MarkAsNoLongerNeeded();
								break;
							}
							prop.IsPersistent = true;
							Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, prop.Handle, pose.X, pose.Y, pose.Z, false, false, false);
							prop.IsPositionFrozen = true;
						}
						break;
					}
				}
				finally
				{
					ReleaseModel();
				}
			}
		}

		private static void ReleaseModel()
		{
			if (requestedModel.HasValue)
			{
				Model value = requestedModel.Value;
				requestedModel = null;
				value.MarkAsNoLongerNeeded();
			}
		}

		internal static Quaternion[] SignCandidates(Quaternion entrance, Quaternion? driveway, Quaternion? vehicleExit, float width)
		{
			Vector3 vector = Helper.ToVector3(entrance);
			if (vector == Vector3.Zero || !PartyPlacement.FinitePoint(vector) || float.IsNaN(entrance.W) || float.IsInfinity(entrance.W) || !(width > 0f) || !(width <= 4f))
			{
				return new Quaternion[0];
			}
			double num = (double)entrance.W * Math.PI / 180.0;
			Vector3 vector2 = new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
			float clearance = 2.15f + width * 0.5f;
			List<Quaternion> list = new List<Quaternion>();
			float[] array = new float[2]
			{
				1.6f + width * 0.5f,
				2.8f + width * 0.5f
			};
			foreach (float num2 in array)
			{
				float[] array2 = new float[2] { -1f, 1f };
				foreach (float num3 in array2)
				{
					Vector3 position = vector + vector2 * (num2 * num3);
					if (ClearsPoint(position, driveway, clearance) && ClearsPoint(position, vehicleExit, clearance))
					{
						list.Add(new Quaternion(position.X, position.Y, position.Z, entrance.W + 180f));
					}
				}
			}
			return list.ToArray();
		}

		private static bool ClearsPoint(Vector3 position, Quaternion? driveway, float clearance)
		{
			if (!driveway.HasValue)
			{
				return true;
			}
			float num = position.X - driveway.Value.X;
			float num2 = position.Y - driveway.Value.Y;
			return num * num + num2 * num2 >= clearance * clearance;
		}

		private static bool TryGround(Quaternion candidate, Ped actor, Vector3 minimum, Vector3 maximum, ref Quaternion pose)
		{
			Vector3 anchor = Helper.ToVector3(candidate);
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, anchor.X, anchor.Y, anchor.Z);
			float num = float.MaxValue;
			float num2 = float.MinValue;
			float y = (minimum.Y + maximum.Y) * 0.5f;
			float[] array = new float[3]
			{
				minimum.X + 0.05f,
				(minimum.X + maximum.X) * 0.5f,
				maximum.X - 0.05f
			};
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (float x in array)
			{
				Vector3 vector = Helper.ToVector3(MansionAutoLayout.TransformPoint(anchor, candidate.W, x, y, 0f));
				if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(vector + new Vector3(0f, 0f, 0.65f), vector - new Vector3(0f, 0f, 2f), 81, ref hit, ref point, ref normal, actor), hit, point, normal.Z))
				{
					return false;
				}
				num = Math.Min(num, point.Z);
				num2 = Math.Max(num2, point.Z);
			}
			if (num2 - num > 0.18f)
			{
				return false;
			}
			Vector3 anchor2 = new Vector3(anchor.X, anchor.Y, num2 - minimum.Z);
			float[] array2 = new float[2]
			{
				minimum.Z + 0.2f,
				maximum.Z - 0.05f
			};
			bool hit2 = default(bool);
			Vector3 point2 = default(Vector3);
			Vector3 normal2 = default(Vector3);
			foreach (float z in array2)
			{
				Vector3 start = Helper.ToVector3(MansionAutoLayout.TransformPoint(anchor2, candidate.W, minimum.X, y, z));
				Vector3 finish = Helper.ToVector3(MansionAutoLayout.TransformPoint(anchor2, candidate.W, maximum.X, y, z));
				if (PartyPlacement.PhysicalRay(start, finish, 83, ref hit2, ref point2, ref normal2, actor) != 2 || hit2)
				{
					return false;
				}
			}
			pose = new Quaternion(anchor2.X, anchor2.Y, anchor2.Z, candidate.W);
			return true;
		}

		private static Prop ResolveSign(SignState state)
		{
			if (state.Handle == 0 || !Function.Call<bool>(Hash.DOES_ENTITY_EXIST, state.Handle))
			{
				return null;
			}
			Entity entity = Entity.FromHandle(state.Handle);
			if ((object)entity == null || state.Address == IntPtr.Zero)
			{
				throw new InvalidOperationException("Sale sign identity is unavailable.");
			}
			Prop prop = entity as Prop;
			if ((object)prop == null || !prop.Exists() || !Mansions.OwnedSaleSignIdentityMatches(state.Handle, state.ModelHash, state.Address, prop.Handle, prop.Model.Hash, prop.MemoryAddress))
			{
				return null;
			}
			return prop;
		}

		private static void RemoveSign(ExpansionProperty definition, SignState state)
		{
			bool retiring = state.Retiring;
			state.Retiring = true;
			try
			{
				ResolveSign(state)?.Delete();
				if ((object)ResolveSign(state) == null)
				{
					signs.Remove(definition);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if (!retiring)
				{
					Logger.Log("Expansion sale sign removal " + definition.Key + ": " + ex2.Message);
				}
				ProjectData.ClearProjectError();
			}
		}
	}
}
