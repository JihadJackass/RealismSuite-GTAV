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
	internal static class NightclubExterior
	{
		internal struct Receipt
		{
			public Ped Actor;

			public IntPtr Address;

			public int ModelHash;

			public Vector3 Floor;
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<bool, bool> _0024I39_002D0;

			internal static Func<Receipt, Vector3> _0024I41_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__39_002D0(bool placed)
			{
				return placed;
			}

			[SpecialName]
			internal Vector3 _Lambda_0024__41_002D0(Receipt receipt)
			{
				return receipt.Floor;
			}
		}

		private const string ClubPrefix = "after_hours_";

		private const string Strawberry = "after_hours_strawberry";

		private const string WestVinewood = "after_hours_west_vinewood";

		private const string LaMesa = "after_hours_la_mesa";

		private const string CypressFlats = "after_hours_cypress_flats";

		private const string DowntownVinewood = "after_hours_downtown_vinewood";

		private const string ElysianIsland = "after_hours_elysian_island";

		private const string Bouncer = "s_m_m_bouncer_01";

		private const float ActivateDistanceSquared = 1024f;

		private const float RetireDistanceSquared = 2704f;

		private const int ModelTimeoutMs = 1500;

		private const int SpawnIntervalMs = 180;

		private const int BlockedRetryMs = 3000;

		private static readonly Quaternion[] WestVinewoodLayout = new Quaternion[7]
		{
			new Quaternion(4.831f, 219.2381f, 107.588f, 298.3113f),
			new Quaternion(6.214864f, 222.629288f, 108.089706f, 159.397659f),
			new Quaternion(6.558916f, 224.168076f, 108.296005f, 162.815948f),
			new Quaternion(6.96359158f, 225.311218f, 108.470337f, 155.869736f),
			new Quaternion(7.301252f, 226.405518f, 108.597649f, 144.130859f),
			new Quaternion(7.66653872f, 227.888687f, 108.776871f, 149.952255f),
			new Quaternion(8.0281105f, 228.9686f, 108.904015f, 158.971268f)
		};

		private static readonly string[] GuestModels = new string[6] { "a_f_y_clubcust_01", "a_m_y_clubcust_01", "a_f_y_clubcust_02", "a_m_y_clubcust_02", "a_f_y_clubcust_03", "a_m_y_clubcust_03" };

		private static readonly string[] GuestScenarios = new string[4] { "WORLD_HUMAN_STAND_MOBILE", "WORLD_HUMAN_AA_SMOKE", "WORLD_HUMAN_STAND_IMPATIENT", "WORLD_HUMAN_AA_COFFEE" };

		private static readonly List<Receipt> residents = new List<Receipt>();

		private static readonly bool[] completed = new bool[checked(GuestModels.Length + 1)];

		private static ExpansionProperty activeClub;

		private static int activeActor;

		private static IntPtr activeAddress;

		private static eOwner activeOwner;

		private static Quaternion[][] layouts;

		private static int preferredLayout = -1;

		private static int candidateIndex;

		private static int slot;

		private static int nextTick;

		private static int requestedAt;

		private static Model? pendingModel;

		private static bool ownsRequest;

		internal static Quaternion[] Layout(ExpansionProperty definition, int side)
		{
			if (Operators.CompareString(definition.Key, "after_hours_west_vinewood", false) == 0 && side > 0)
			{
				return (Quaternion[])WestVinewoodLayout.Clone();
			}
			if (Operators.CompareString(definition.Key, "after_hours_cypress_flats", false) == 0)
			{
				return CypressLayout((side > 0) ? 0f : (-0.48f), 0f);
			}
			if (Operators.CompareString(definition.Key, "after_hours_downtown_vinewood", false) == 0)
			{
				return DowntownLayout(side > 0, 0f, 0f);
			}
			checked
			{
				if (Operators.CompareString(definition.Key, "after_hours_la_mesa", false) == 0 && side > 0)
				{
					Quaternion[] array = new Quaternion[GuestModels.Length + 1];
					array[0] = new Quaternion(759.2f, -1331.35f, 25.28f, 354.7854f);
					int num = GuestModels.Length;
					for (int i = 1; i <= num; i++)
					{
						array[i] = new Quaternion(760.55f + (float)(i - 1) * 1.12f, -1330.55f, 25.28f, 110f);
					}
					return array;
				}
				return FrontageLayout(definition, side, 0.6f);
			}
		}

		private static Quaternion[] CypressLayout(float lateralOffset, float depthOffset)
		{
			checked
			{
				Quaternion[] array = new Quaternion[GuestModels.Length + 1];
				array[0] = new Quaternion(869.4386f + lateralOffset, -2097.188f + depthOffset, 29.3897f, 48f);
				int num = GuestModels.Length;
				for (int i = 1; i <= num; i++)
				{
					int num2 = i - 1;
					array[i] = new Quaternion(868.2f - (float)num2 * 0.16f + lateralOffset, -2097.55f + (float)num2 * 1.32f + depthOffset, 29.37f - (float)num2 * 0.014f, 187f);
				}
				return array;
			}
		}

		private static Quaternion[] DowntownLayout(bool westQueue, float offsetX, float offsetY)
		{
			Vector3 vector = new Vector3(371.922f, 255.041f, 101.962f);
			Vector3 vector2 = new Vector3(375.915f, 253.424f, 102.028f);
			Vector3 vector3 = (westQueue ? vector : vector2);
			Vector3 vector4 = (westQueue ? new Vector3(369.753f, 261.288f, 101.956f) : new Vector3(379.133f, 257.864f, 101.972f));
			Vector3 vector5 = (westQueue ? vector2 : vector);
			float w = (westQueue ? 199f : 144f);
			checked
			{
				Quaternion[] array = new Quaternion[GuestModels.Length + 1];
				array[0] = new Quaternion(vector5.X + offsetX, vector5.Y + offsetY, vector5.Z, 342.997f);
				int num = GuestModels.Length;
				for (int i = 1; i <= num; i++)
				{
					Vector3 vector6 = vector3 + (vector4 - vector3) * ((float)(i - 1) / (float)(GuestModels.Length - 1));
					array[i] = new Quaternion(vector6.X + offsetX, vector6.Y + offsetY, vector6.Z, w);
				}
				return array;
			}
		}

		private static Quaternion[] FrontageLayout(ExpansionProperty definition, int side, float distanceFromDoor)
		{
			Quaternion entrance = definition.Entrance;
			double num = (double)entrance.W * Math.PI / 180.0;
			Vector3 vector = new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
			Vector3 vector2 = new Vector3(0f - vector.Y, vector.X, 0f);
			Vector3 vector3 = Helper.ToVector3(entrance) + vector2 * distanceFromDoor;
			Vector3 vector4 = vector3 - vector * (1.75f * (float)side);
			checked
			{
				Quaternion[] array = new Quaternion[GuestModels.Length + 1];
				float w = entrance.W;
				string key = definition.Key;
				if (Operators.CompareString(key, "after_hours_strawberry", false) != 0)
				{
					if (Operators.CompareString(key, "after_hours_elysian_island", false) == 0)
					{
						w = (entrance.W + 180f) % 360f;
					}
				}
				else
				{
					w = 278.6f;
				}
				array[0] = new Quaternion(vector4.X, vector4.Y, vector4.Z, w);
				int num2 = GuestModels.Length;
				for (int i = 1; i <= num2; i++)
				{
					Vector3 vector5 = vector3 + vector * ((1.8f + (float)(i - 1) * 1.25f) * (float)side);
					array[i] = new Quaternion(vector5.X, vector5.Y, vector5.Z, entrance.W + (float)side * 90f);
				}
				return array;
			}
		}

		private static Quaternion[][] LayoutCandidates(ExpansionProperty definition)
		{
			if (Operators.CompareString(definition.Key, "after_hours_cypress_flats", false) == 0)
			{
				return new Quaternion[6][]
				{
					Layout(definition, 1),
					Layout(definition, -1),
					CypressLayout(0.48f, 0f),
					CypressLayout(0f, -0.35f),
					CypressLayout(-0.48f, -0.35f),
					CypressLayout(0.48f, -0.35f)
				};
			}
			if (Operators.CompareString(definition.Key, "after_hours_downtown_vinewood", false) == 0)
			{
				return new Quaternion[6][]
				{
					Layout(definition, 1),
					Layout(definition, -1),
					DowntownLayout(true, -0.35f, -0.3f),
					DowntownLayout(false, -0.35f, -0.3f),
					DowntownLayout(true, 0.35f, -0.3f),
					DowntownLayout(false, 0.35f, -0.3f)
				};
			}
			return new Quaternion[6][]
			{
				Layout(definition, 1),
				Layout(definition, -1),
				FrontageLayout(definition, 1, 2.6f),
				FrontageLayout(definition, -1, 2.6f),
				FrontageLayout(definition, 1, 4.1f),
				FrontageLayout(definition, -1, 4.1f)
			};
		}

		private static int LayoutIndex(int attempt, int preferred)
		{
			if (preferred < 0)
			{
				return attempt;
			}
			if (attempt == 0)
			{
				return preferred;
			}
			return (attempt <= preferred) ? checked(attempt - 1) : attempt;
		}

		public static void Tick(Ped actor, eOwner owner, IEnumerable<ExpansionProperty> definitions)
		{
			if (Game.IsMissionActive || owner < eOwner.Michael || owner > eOwner.Trevor)
			{
				Reset(false);
				return;
			}
			checked
			{
				try
				{
					int gameTime = Game.GameTime;
					if (gameTime < nextTick && (activeClub == null || (actor.Handle == activeActor && owner == activeOwner)))
					{
						return;
					}
					Vector3 position = actor.Position;
					if (activeClub != null && (actor.Handle != activeActor || actor.MemoryAddress != activeAddress || owner != activeOwner || position.DistanceToSquared(Helper.ToVector3(activeClub.Entrance)) > 2704f || !OnlineExpansion.IsOwned(activeClub, owner)))
					{
						Reset(false);
					}
					if (activeClub == null)
					{
						nextTick = gameTime + 300;
						IntPtr memoryAddress = actor.MemoryAddress;
						if (memoryAddress == IntPtr.Zero)
						{
							return;
						}
						float num = 1024f;
						foreach (ExpansionProperty definition in definitions)
						{
							if (definition.Key.StartsWith("after_hours_", StringComparison.Ordinal))
							{
								float num2 = position.DistanceToSquared(Helper.ToVector3(definition.Entrance));
								if (!(num2 >= num) && OnlineExpansion.IsOwned(definition, owner))
								{
									num = num2;
									activeClub = definition;
								}
							}
						}
						if (activeClub == null)
						{
							return;
						}
						activeActor = actor.Handle;
						activeAddress = memoryAddress;
						activeOwner = owner;
						layouts = LayoutCandidates(activeClub);
					}
					if (position.DistanceToSquared(Helper.ToVector3(activeClub.Entrance)) > 1024f)
					{
						nextTick = gameTime + 500;
					}
					else
					{
						TickRoster(actor, gameTime);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("After Hours exterior: " + ex2.Message);
					Reset(false);
					nextTick = Game.GameTime + 3000;
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void TickRoster(Ped actor, int now)
		{
			checked
			{
				while (slot < completed.Length && completed[slot])
				{
					slot++;
				}
				if (slot < completed.Length)
				{
					TickSlot(actor, now);
					return;
				}
				if (completed.All((_Closure_0024__._0024I39_002D0 != null) ? _Closure_0024__._0024I39_002D0 : (_Closure_0024__._0024I39_002D0 = _Closure_0024__._0024I._Lambda_0024__39_002D0)))
				{
					nextTick = now + 500;
					return;
				}
				slot = 0;
				nextTick = now + 3000;
			}
		}

		private static void TickSlot(Ped actor, int now)
		{
			checked
			{
				nextTick = now + 180;
				if (!pendingModel.HasValue)
				{
					Model value = new Model((slot == 0) ? "s_m_m_bouncer_01" : GuestModels[slot - 1]);
					if (!value.IsValid || !value.IsInCdImage)
					{
						CompleteSlot(true);
						return;
					}
					pendingModel = value;
					ownsRequest = !value.IsLoaded;
					requestedAt = now;
					if (ownsRequest)
					{
						value.Request();
						return;
					}
				}
				Model value2 = pendingModel.Value;
				if (!value2.IsLoaded)
				{
					if (now - requestedAt >= 1500)
					{
						CompleteSlot(false);
					}
					return;
				}
				int num = LayoutIndex(candidateIndex, preferredLayout);
				Quaternion pose = layouts[num][slot];
				Vector3 floor = default(Vector3);
				if (TryFloor(pose, actor, ref floor) && Spawn(value2, floor, pose.W))
				{
					if (slot > 0 && preferredLayout < 0)
					{
						preferredLayout = num;
					}
					CompleteSlot(true);
				}
				else
				{
					candidateIndex++;
					if (candidateIndex >= layouts.Length)
					{
						CompleteSlot(false);
					}
				}
			}
		}

		private static bool TryFloor(Quaternion pose, Ped actor, ref Vector3 floor)
		{
			Vector3 vector = Helper.ToVector3(pose);
			Vector3 vector2 = Helper.ToVector3(activeClub.Entrance);
			if (!PartyPlacement.IsSeparated(vector, new Vector3[2] { actor.Position, vector2 }, 1.25f))
			{
				return false;
			}
			if (!PartyPlacement.IsSeparated(vector, residents.Select((_Closure_0024__._0024I41_002D0 != null) ? _Closure_0024__._0024I41_002D0 : (_Closure_0024__._0024I41_002D0 = _Closure_0024__._0024I._Lambda_0024__41_002D0)), 0.92f))
			{
				return false;
			}
			if (activeClub.VehicleEntrance.HasValue && vector.DistanceToSquared(Helper.ToVector3(activeClub.VehicleEntrance.Value)) < 9f)
			{
				return false;
			}
			if (activeClub.VehicleExit.HasValue && vector.DistanceToSquared(Helper.ToVector3(activeClub.VehicleExit.Value)) < 9f)
			{
				return false;
			}
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, pose.X, pose.Y, pose.Z);
			bool hit = default(bool);
			Vector3 normal = default(Vector3);
			if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(vector + new Vector3(0f, 0f, 1.25f), vector - new Vector3(0f, 0f, 1.75f), 17, ref hit, ref floor, ref normal, actor), hit, floor, normal.Z) || Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, floor.X, floor.Y, floor.Z + 0.75f) != 0 || Function.Call<int>(Hash.IS_POINT_ON_ROAD, floor.X, floor.Y, floor.Z, 0) != 0)
			{
				return false;
			}
			Vector3[] array = new Vector3[4]
			{
				new Vector3(0.25f, 0f, 0f),
				new Vector3(-0.25f, 0f, 0f),
				new Vector3(0f, 0.25f, 0f),
				new Vector3(0f, -0.25f, 0f)
			};
			Vector3 point = default(Vector3);
			foreach (Vector3 vector3 in array)
			{
				Vector3 vector4 = floor + vector3;
				if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(vector4 + new Vector3(0f, 0f, 0.24f), vector4 - new Vector3(0f, 0f, 0.24f), 17, ref hit, ref point, ref normal, actor), hit, point, normal.Z) || Math.Abs(point.Z - floor.Z) > 0.18f)
				{
					return false;
				}
			}
			Vector3 point2 = default(Vector3);
			if (!PartyPlacement.BodyProbeClear(PartyPlacement.PhysicalRay(floor + new Vector3(0f, 0f, 0.16f), floor + new Vector3(0f, 0f, 1.8f), 87, ref hit, ref point2, ref normal, actor), hit))
			{
				return false;
			}
			Vector3[] array2 = new Vector3[2]
			{
				new Vector3(0.26f, 0f, 0f),
				new Vector3(0f, 0.26f, 0f)
			};
			foreach (Vector3 vector5 in array2)
			{
				Vector3 vector6 = floor + new Vector3(0f, 0f, 1f);
				if (!PartyPlacement.BodyProbeClear(PartyPlacement.PhysicalRay(vector6 - vector5, vector6 + vector5, 87, ref hit, ref point2, ref normal, actor), hit))
				{
					return false;
				}
			}
			return true;
		}

		private static bool Spawn(Model model, Vector3 floor, float heading)
		{
			Ped ped = World.CreatePed(model, floor, heading);
			if ((object)ped == null || !ped.Exists())
			{
				return false;
			}
			IntPtr memoryAddress = ped.MemoryAddress;
			if (memoryAddress == IntPtr.Zero)
			{
				ped.MarkAsNoLongerNeeded();
				return false;
			}
			residents.Add(new Receipt
			{
				Actor = ped,
				Address = memoryAddress,
				ModelHash = model.Hash,
				Floor = floor
			});
			ped.IsPersistent = true;
			ped.BlockPermanentEvents = true;
			Function.Call(Hash.SET_ENTITY_COORDS, ped.Handle, floor.X, floor.Y, floor.Z, false, false, false, false);
			ped.Heading = heading;
			string text = ((slot == 0) ? "WORLD_HUMAN_GUARD_STAND" : GuestScenarios[checked(slot - 1) % GuestScenarios.Length]);
			Function.Call(Hash.TASK_START_SCENARIO_IN_PLACE, ped.Handle, text, -1, false);
			return true;
		}

		private static void CompleteSlot(bool placed)
		{
			ReleaseModel();
			completed[slot] = placed;
			candidateIndex = 0;
			checked
			{
				slot++;
				if (residents.Count == completed.Length)
				{
					Logger.Log($"After Hours exterior {activeClub.Key}: placed {residents.Count} actors.");
				}
			}
		}

		private static void ReleaseModel()
		{
			Model? model = pendingModel;
			pendingModel = null;
			if (ownsRequest && model.HasValue)
			{
				ownsRequest = false;
				try
				{
					model.Value.MarkAsNoLongerNeeded();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("After Hours exterior model cleanup: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void Reset(bool storyLoad)
		{
			if (!storyLoad)
			{
				ReleaseModel();
				foreach (Receipt resident in residents)
				{
					try
					{
						Ped actor = resident.Actor;
						if (actor.Exists() && actor.MemoryAddress == resident.Address && actor.Model.Hash == resident.ModelHash)
						{
							actor.Delete();
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("After Hours exterior resident cleanup: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
				}
			}
			residents.Clear();
			pendingModel = null;
			ownsRequest = false;
			activeClub = null;
			layouts = null;
			Array.Clear(completed, 0, completed.Length);
			preferredLayout = -1;
			candidateIndex = 0;
			slot = 0;
			nextTick = 0;
		}
	}
}
