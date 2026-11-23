using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionAutoLayout
	{
		private static readonly Dictionary<Mansion, int> nextScan = new Dictionary<Mansion, int>();

		public static float MansionHeading(Mansion m)
		{
			if (m == null)
			{
				return 0f;
			}
			switch (m.Key)
			{
			case "MANSION_VINEWOOD":
				return -51.69f;
			case "MANSION_RICHMAN":
				return 17.77f;
			case "MANSION_TONGVA":
				return -84.083f;
			default:
				return 0f;
			}
		}

		public static Vector3 MainAnchor(Mansion m)
		{
			if (m == null)
			{
				return Vector3.Zero;
			}
			Vector3 result;
			switch (m.Key)
			{
			case "MANSION_VINEWOOD":
				result = new Vector3(539.7012f, 749.089355f, 201.361649f);
				break;
			case "MANSION_RICHMAN":
				result = new Vector3(-1666.3678f, 478.9271f, 128.221558f);
				break;
			case "MANSION_TONGVA":
				result = new Vector3(-2586.065f, 1909.995f, 166.375427f);
				break;
			default:
				return Vector3.Zero;
			}
			return result;
		}

		public static Vector3 GarageScriptAnchor(Mansion m)
		{
			if (m == null)
			{
				return Vector3.Zero;
			}
			Vector3 result;
			switch (m.Key)
			{
			case "MANSION_VINEWOOD":
				result = new Vector3(548.6964f, 766.886841f, 190.504471f);
				break;
			case "MANSION_RICHMAN":
				result = new Vector3(-1679.87659f, 493.596f, 117.36438f);
				break;
			case "MANSION_TONGVA":
				result = new Vector3(-2568.93384f, 1920.20276f, 155.51825f);
				break;
			default:
				return Vector3.Zero;
			}
			return result;
		}

		public static Vector3 BasementAnchor(Mansion m)
		{
			if (m == null)
			{
				return Vector3.Zero;
			}
			Vector3 result;
			switch (m.Key)
			{
			case "MANSION_VINEWOOD":
				result = new Vector3(547.4955f, 734.136f, 190.504471f);
				break;
			case "MANSION_RICHMAN":
				result = new Vector3(-1649.63013f, 480.9779f, 117.364456f);
				break;
			case "MANSION_TONGVA":
				result = new Vector3(-2587.49585f, 1893.193f, 155.518326f);
				break;
			default:
				return Vector3.Zero;
			}
			return result;
		}

		public static Quaternion TransformPoint(Vector3 anchor, float heading, float x, float y, float z, float facing = 0f)
		{
			double num = (double)heading * Math.PI / 180.0;
			double num2 = Math.Cos(num);
			double num3 = Math.Sin(num);
			float num4 = (heading + facing) % 360f;
			if (num4 < 0f)
			{
				num4 += 360f;
			}
			return new Quaternion(anchor.X + (float)(num2 * (double)x - num3 * (double)y), anchor.Y + (float)(num3 * (double)x + num2 * (double)y), anchor.Z + z, num4);
		}

		public static Quaternion MainPoint(Mansion m, float x, float y, float z, float facing = 0f)
		{
			return TransformPoint(MainAnchor(m), MansionHeading(m), x, y, z, facing);
		}

		public static Quaternion GaragePoint(Mansion m, float x, float y, float z, float facing = 0f)
		{
			return TransformPoint(GarageScriptAnchor(m), MansionHeading(m), x, y, z, facing);
		}

		private static void SetMissing(ref Quaternion point, Quaternion value)
		{
			if (!MansionFeatures.IsSet(point))
			{
				point = value;
			}
		}

		private static void SetStop(Mansion m, string key, Quaternion value)
		{
			Quaternion value2;
			if (!m.ElevatorStops.TryGetValue(key, out value2) || !MansionFeatures.IsSet(value2))
			{
				m.ElevatorStops[key] = value;
			}
		}

		public static void ApplyDefaults(Mansion m)
		{
			if (m == null || MainAnchor(m) == Vector3.Zero)
			{
				return;
			}
			m.BasementProbe = BasementAnchor(m);
			SetStop(m, "Main", MainPoint(m, 19.581f, 1.226f, 0.5f, 37.917f));
			SetStop(m, "Lower", MainPoint(m, 19.581f, 1.226f, -3.654f, 37.917f));
			SetStop(m, "B1", MainPoint(m, 20.782f, -3.549f, -10.857f, 37.917f));
			SetStop(m, "B2", MainPoint(m, 20.782f, -3.549f, -14.785f, 37.917f));
			SetMissing(ref m.PodiumPos, GaragePoint(m, 14.471f, -13.724f, 7.521f, 96.9188f));
			SetMissing(ref m.WorkshopPos, GaragePoint(m, 41.3572f, -17.1508f, -4.261f, 34.79f));
			SetMissing(ref m.HelipadPos, MainPoint(m, 24.4139f, -0.7575f, 5.7428f, 89.3175f));
			SetMissing(ref m.GymPos, MainPoint(m, 18.0514f, -22.1767f, 0.0837f, 2.23f));
			SetMissing(ref m.ArmoryPos, MainPoint(m, 11.075f, 10.57f, -10.857f, 125f));
			SetMissing(ref m.BarPos, MainPoint(m, 28.319f, -13.421f, 0f, 342.23f));
			SetMissing(ref m.BedPos, OriginalMasterBedPoint(m));
			SetMissing(ref m.VaultPos, MainPoint(m, 18.9898f, 10.8599f, -15.285f));
			Vector3[] array = new Vector3[13]
			{
				new Vector3(3.1411f, -5.1791f, 0f),
				new Vector3(2.6011f, -12.3328f, 0f),
				new Vector3(36.355f, 3.438f, -2.125f),
				new Vector3(23.5864f, -7.0954f, 0f),
				new Vector3(22.9631f, 7.8564f, 0f),
				new Vector3(13.544f, 0.444f, -1.665f),
				new Vector3(18.006f, -9.862f, -3.654f),
				new Vector3(-0.731f, 8.719f, -4.054f),
				new Vector3(17.169f, 0.339f, -10.781f),
				new Vector3(30.059f, -8.102f, -15.285f),
				new Vector3(17.169f, 0.339f, -15.282f),
				new Vector3(29.962f, -9.431f, 1.463f),
				new Vector3(1.229f, 7.699f, -4.054f)
			};
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				Vector3 vector = array[i];
				Quaternion quaternion = MainPoint(m, vector.X, vector.Y, vector.Z);
				bool flag = false;
				foreach (Quaternion assistPanel in m.AssistPanels)
				{
					if (Helper.ToVector3(assistPanel).DistanceTo(Helper.ToVector3(quaternion)) < 1f)
					{
						flag = true;
						break;
					}
				}
				if (!flag && m.AssistPanels.Count < 21)
				{
					m.AssistPanels.Add(quaternion);
				}
			}
		}

		public static Quaternion OriginalShowroomPoint(Mansion m, int modelHash)
		{
			if (modelHash == 1380582820 || modelHash == Game.GenerateHash("vigilante"))
			{
				return GaragePoint(m, 15.543f, -13.358f, 7.825f, 96.9188f);
			}
			if (modelHash == Game.GenerateHash("baller7") || modelHash == Game.GenerateHash("slamtruck") || modelHash == Game.GenerateHash("halftrack") || modelHash == Game.GenerateHash("lurcher"))
			{
				return GaragePoint(m, 14.303f, -13.458f, 7.825f, 96.9188f);
			}
			if (modelHash == Game.GenerateHash("oppressor2"))
			{
				return GaragePoint(m, 14.4662f, -13.7239f, 7.7624f, 96.9188f);
			}
			return GaragePoint(m, 14.471f, -13.724f, 7.521f, 96.9188f);
		}

		public static Quaternion OriginalMasterBedPoint(Mansion m)
		{
			if (m == null || MainAnchor(m) == Vector3.Zero)
			{
				return default(Quaternion);
			}
			return MainPoint(m, 36.842f, -1.63f, -2.59f, 170f);
		}

		public static void AutoLayoutTick(Mansion m)
		{
			if (m != null)
			{
				ApplyDefaults(m);
				int value;
				if (m.IplsLoaded && m.SetsApplied && m.IsOwnedByPlayer() && (!nextScan.TryGetValue(m, out value) || Game.GameTime >= value))
				{
					nextScan[m] = checked(Game.GameTime + 5000);
					DiscoverVaultCash(m);
				}
			}
		}

		public static bool TryResolveNow(Mansion m)
		{
			if (m == null)
			{
				return false;
			}
			ApplyDefaults(m);
			if (m.IplsLoaded && m.SetsApplied && m.IsOwnedByPlayer())
			{
				DiscoverVaultCash(m);
			}
			return MansionFeatures.IsSet(m.PodiumPos) && MansionFeatures.IsSet(m.WorkshopPos) && MansionFeatures.IsSet(m.HelipadPos);
		}

		private static void DiscoverVaultCash(Mansion m)
		{
			if ((object)Helper.PP == null || !Helper.PP.Exists() || !MansionFeatures.IsSet(m.VaultPos) || m.CashPoints.Count >= 64 || Helper.PP.Position.DistanceTo(Helper.ToVector3(m.VaultPos)) > 30f)
			{
				return;
			}
			checked
			{
				try
				{
					int num = Game.GenerateHash("m25_2_prop_m52_cash_stack_01a");
					bool flag = false;
					int num2 = -1;
					do
					{
						int num3 = -1;
						do
						{
							Vector3 vector = Helper.ToVector3(m.VaultPos) + new Vector3((float)num2 * 2f, (float)num3 * 2f, 1f);
							int num4 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector.X, vector.Y, vector.Z, 4f, num, false, false, false);
							if (num4 != 0 && Function.Call<bool>(Hash.DOES_ENTITY_EXIST, num4))
							{
								Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, num4, true);
								if (!(Math.Abs(vector2.Z - m.VaultPos.Z) > 2f) && !(Mansion.Dist2D(vector2, Helper.ToVector3(m.VaultPos)) > 6f))
								{
									bool flag2 = false;
									foreach (CashPoint cashPoint in m.CashPoints)
									{
										if (cashPoint.Pos.DistanceTo(vector2) < 0.2f)
										{
											flag2 = true;
											break;
										}
									}
									if (!flag2 && m.CashPoints.Count < 64)
									{
										m.CashPoints.Add(new CashPoint
										{
											Pos = vector2,
											ModelHash = num
										});
										flag = true;
										Logger.Log($"> {m.Name}: automatically found a real vault cash stack");
									}
								}
							}
							num3++;
						}
						while (num3 <= 1);
						num2++;
					}
					while (num2 <= 1);
					if (flag)
					{
						Mansions.SaveMansions();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"Mansion vault discovery {m.Name}: {ex2.Message}");
					ProjectData.ClearProjectError();
				}
			}
		}
	}
}
