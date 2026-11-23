using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionVault
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal int _Lambda_0024__0_002D0(string name)
			{
				return new Model(name).Hash;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__14_002D0
		{
			public Vector3 _0024VB_0024Local_point;

			[SpecialName]
			internal bool _Lambda_0024__0(CashPoint c)
			{
				if (Math.Abs(c.Pos.Z - _0024VB_0024Local_point.Z) < 1.5f)
				{
					return Mansion.Dist2D(c.Pos, _0024VB_0024Local_point) < 0.75f;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__23_002D0
		{
			public Vector3 _0024VB_0024Local_p;

			[SpecialName]
			internal float _Lambda_0024__0(CashPoint x)
			{
				return x.Pos.DistanceTo(_0024VB_0024Local_p);
			}
		}

		public static int VaultCashAmount = 10000;

		public static int VaultRefillHours = 24;

		public static bool VaultHideTaken = true;

		public const int MaxCashPoints = 64;

		private const string TimeFormat = "yyyy-MM-dd HH:mm";

		private static readonly string[] cashModels = new string[10] { "m25_2_prop_m52_cash_stack_01a", "m25_2_prop_m52_cash_trolley_lrg_full", "m25_2_prop_m52_cash_trolley_lrg_half", "m25_2_prop_m52_cash_trolly_01a", "hei_prop_hei_cash_trolly_01", "hei_prop_hei_cash_trolly_03", "ch_prop_ch_cash_trolly_01a", "prop_cash_pile_01", "prop_cash_pile_02", "prop_cash_trolly" };

		private static readonly HashSet<int> cashHashes = new HashSet<int>(cashModels.Select(_Closure_0024__._0024I._Lambda_0024__0_002D0));

		private static readonly Dictionary<Mansion, int> nextDiscovery = new Dictionary<Mansion, int>();

		public static DateTime GameNow()
		{
			try
			{
				int val = Function.Call<int>(Hash.GET_CLOCK_YEAR);
				int val2 = checked(Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1);
				int val3 = Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH);
				int val4 = Function.Call<int>(Hash.GET_CLOCK_HOURS);
				int val5 = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
				val = Math.Max(1, Math.Min(9998, val));
				val2 = Math.Max(1, Math.Min(12, val2));
				val3 = Math.Max(1, Math.Min(DateTime.DaysInMonth(val, val2), val3));
				return new DateTime(val, val2, val3, Math.Max(0, Math.Min(23, val4)), Math.Max(0, Math.Min(59, val5)), 0);
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				DateTime minValue = DateTime.MinValue;
				ProjectData.ClearProjectError();
				return minValue;
			}
		}

		public static bool CashAvailable(string taken, DateTime now, int refillHours)
		{
			if (DateTime.Compare(now, DateTime.MinValue) == 0 || refillHours < 1)
			{
				return false;
			}
			if (string.IsNullOrEmpty(taken))
			{
				return true;
			}
			DateTime result;
			if (!DateTime.TryParseExact(taken, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
			{
				return false;
			}
			return DateTime.Compare(now, result) >= 0 && (now - result).TotalHours >= (double)refillHours;
		}

		public static bool IsFull(CashPoint c)
		{
			if (c != null)
			{
				return CashAvailable(c.Taken, GameNow(), VaultRefillHours);
			}
			return false;
		}

		private static int HoursLeft(CashPoint c)
		{
			DateTime result;
			if (!DateTime.TryParseExact(c.Taken, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
			{
				return 0;
			}
			return Math.Max(1, checked((int)Math.Ceiling((double)VaultRefillHours - (GameNow() - result).TotalHours)));
		}

		public static bool IsVaultCashModel(int hash)
		{
			return cashHashes.Contains(hash);
		}

		public static bool CashPointIsNew(Vector3 point, IEnumerable<CashPoint> existing)
		{
			_Closure_0024__14_002D0 closure_0024__14_002D = new _Closure_0024__14_002D0();
			closure_0024__14_002D._0024VB_0024Local_point = point;
			if (PartyPlacement.FinitePoint(closure_0024__14_002D._0024VB_0024Local_point))
			{
				if (existing != null)
				{
					return !existing.Any(closure_0024__14_002D._Lambda_0024__0);
				}
				return true;
			}
			return false;
		}

		private static void DiscoverCash(Mansion m)
		{
			int value;
			if (m == null || !m.IplsLoaded || !m.IsOwnedByPlayer() || m.CashPoints.Count >= 64 || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.Position.DistanceTo(Helper.ToVector3(m.VaultPos)) > 15f || (nextDiscovery.TryGetValue(m, out value) && Game.GameTime < value))
			{
				return;
			}
			checked
			{
				nextDiscovery[m] = Game.GameTime + 5000;
				List<CashPoint> list = new List<CashPoint>();
				try
				{
					Dictionary<int, Prop> dictionary = new Dictionary<int, Prop>();
					Prop[] nearbyProps = World.GetNearbyProps(Helper.ToVector3(m.VaultPos), 12f);
					foreach (Prop prop in nearbyProps)
					{
						if ((object)prop != null && prop.Exists() && IsVaultCashModel(prop.Model.Hash))
						{
							dictionary[prop.Handle] = prop;
						}
					}
					string[] array = cashModels;
					for (int j = 0; j < array.Length; j++)
					{
						int hash = new Model(array[j]).Hash;
						float[] array2 = new float[3] { -4f, 0f, 4f };
						foreach (float x in array2)
						{
							float[] array3 = new float[3] { -4f, 0f, 4f };
							foreach (float y in array3)
							{
								Vector3 vector = Helper.ToVector3(m.VaultPos) + new Vector3(x, y, 0.8f);
								int num = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, vector.X, vector.Y, vector.Z, 6f, hash, false, false, false);
								if (num != 0 && !dictionary.ContainsKey(num))
								{
									Prop prop2 = Entity.FromHandle(num) as Prop;
									if ((object)prop2 != null && prop2.Exists())
									{
										dictionary[num] = prop2;
									}
								}
							}
						}
					}
					foreach (Prop value2 in dictionary.Values)
					{
						Vector3 position = value2.Position;
						if (IsVaultCashModel(value2.Model.Hash) && !(position.DistanceTo(Helper.ToVector3(m.VaultPos)) > 12f) && !(Math.Abs(position.Z - m.VaultPos.Z) > 2f) && CashPointIsNew(position, m.CashPoints))
						{
							CashPoint item = new CashPoint
							{
								Pos = position,
								ModelHash = value2.Model.Hash
							};
							m.CashPoints.Add(item);
							list.Add(item);
							if (m.CashPoints.Count >= 64)
							{
								break;
							}
						}
					}
					if (list.Count <= 0)
					{
						return;
					}
					if (!Mansions.SaveMansions())
					{
						foreach (CashPoint item2 in list)
						{
							m.CashPoints.Remove(item2);
						}
						Logger.Log("Vault discovery could not be saved for " + m.Name);
					}
					else
					{
						Logger.Log("Vault cash discovered: " + m.Name + "; added=" + list.Count + "; total=" + m.CashPoints.Count);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					foreach (CashPoint item3 in list)
					{
						m.CashPoints.Remove(item3);
					}
					Logger.Log("Vault cash discovery: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static bool CashInteractionNear(Vector3 player, CashPoint cash)
		{
			if (cash != null && PartyPlacement.FinitePoint(player) && PartyPlacement.FinitePoint(cash.Pos) && Mansion.Dist2D(player, cash.Pos) <= 1.3f)
			{
				return Math.Abs(player.Z - cash.Pos.Z) <= 1.8f;
			}
			return false;
		}

		public static bool CashTick(Mansion m)
		{
			if (VaultCashAmount <= 0 || !m.IsOwnedByPlayer() || m.CashPoints.Count == 0 || Helper.PP.IsInVehicle())
			{
				return false;
			}
			Vector3 position = Helper.PP.Position;
			foreach (CashPoint cashPoint in m.CashPoints)
			{
				if (!CashInteractionNear(position, cashPoint))
				{
					continue;
				}
				if (IsFull(cashPoint))
				{
					if (!Helper.ShowPersistentHelp($"Press ~INPUT_CONTEXT~ to take the cash (~g~${VaultCashAmount:N0}~s~)."))
					{
						return false;
					}
					if (Game.IsControlJustReleased(Control.Context))
					{
						TakeCash(m, cashPoint);
					}
				}
				else
				{
					int num = HoursLeft(cashPoint);
					Helper.ShowPersistentHelp(string.Format("Empty - refilled in about {0} hour{1} (game time).", num, (num == 1) ? "" : "s"));
				}
				return true;
			}
			return false;
		}

		public static void CashSlowTick(Mansion m)
		{
			DiscoverCash(m);
			if (m.CashPoints.Count == 0)
			{
				return;
			}
			bool flag = m.IplsLoaded && Helper.PP.Position.DistanceTo(m.Reference) < 150f;
			foreach (CashPoint cashPoint in m.CashPoints)
			{
				if (cashPoint.ModelHash != 0)
				{
					if (flag && VaultHideTaken && !IsFull(cashPoint))
					{
						Function.Call(Hash.CREATE_MODEL_HIDE, cashPoint.Pos.X, cashPoint.Pos.Y, cashPoint.Pos.Z, 0.6f, cashPoint.ModelHash, true);
						cashPoint.Hidden = true;
					}
					else if (cashPoint.Hidden)
					{
						ShowCash(cashPoint);
					}
				}
			}
		}

		private static void ShowCash(CashPoint c)
		{
			try
			{
				Function.Call(Hash.REMOVE_MODEL_HIDE, c.Pos.X, c.Pos.Y, c.Pos.Z, 0.6f, c.ModelHash, false);
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			c.Hidden = false;
		}

		public static void VaultCleanup()
		{
			foreach (Mansion mansion in Mansions.MansionList)
			{
				foreach (CashPoint cashPoint in mansion.CashPoints)
				{
					if (cashPoint.Hidden)
					{
						ShowCash(cashPoint);
					}
				}
			}
		}

		private static void TakeCash(Mansion m, CashPoint c)
		{
			if (m == null || c == null || !m.IsOwnedByPlayer() || !IsFull(c) || VaultCashAmount <= 0)
			{
				return;
			}
			eOwner player = Helper.GetPlayer();
			int balance = default(int);
			int updated = default(int);
			if (!Helper.TryGetOwnerMoney(player, ref balance) || !Helper.TryCashAfterChange(balance, VaultCashAmount, ref updated))
			{
				Notification.Show("The vault payout could not fit in this character's wallet.");
				return;
			}
			DateTime t = GameNow();
			if (DateTime.Compare(t, DateTime.MinValue) == 0)
			{
				return;
			}
			string taken = c.Taken;
			bool flag = false;
			bool flag2 = false;
			try
			{
				c.Taken = t.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
				using (AtomicFileTransaction atomicFileTransaction = Mansions.BeginMansionSaveTransaction())
				{
					if (!Helper.TryChangeOwnerMoney(player, VaultCashAmount))
					{
						throw new InvalidOperationException("Character payout failed");
					}
					flag2 = true;
					atomicFileTransaction.Complete();
					flag = true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if (flag2 && !flag && !Helper.TryChangeOwnerMoney(player, checked(-VaultCashAmount)))
				{
					Logger.Log("Vault payout rollback failed");
				}
				if (!flag)
				{
					c.Taken = taken;
				}
				Logger.Log("Vault cash transaction: " + ex2.Message);
				Notification.Show("The vault payout could not be saved. Please try again.");
				ProjectData.ClearProjectError();
				return;
			}
			try
			{
				float num = c.Pos.X - Helper.PP.Position.X;
				float num2 = c.Pos.Y - Helper.PP.Position.Y;
				Helper.PP.Heading = (float)(Math.Atan2(0f - num, num2) * 180.0 / Math.PI);
				Function.Call(Hash.REQUEST_ANIM_DICT, "mp_common");
				if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "mp_common"))
				{
					Function.Call(Hash.TASK_PLAY_ANIM, Helper.PP.Handle, "mp_common", "givetake1_a", 8f, -8f, 1200, 48, 0f, false, false, false);
					Function.Call(Hash.REMOVE_ANIM_DICT, "mp_common");
				}
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "PICK_UP", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
				Notification.Show($"~g~+${VaultCashAmount:N0}~w~ from the vault of {m.Name}.");
				CashSlowTick(m);
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
				Logger.Log("Vault cash presentation: " + ex4.Message);
				ProjectData.ClearProjectError();
			}
			Logger.Log($"> vault {m.Name}: took {VaultCashAmount} at {c.Pos}");
		}

		public static bool RecordCash(Mansion m)
		{
			int num = checked(Game.GameTime + 60000);
			Script.Wait(150);
			while (Game.GameTime < num)
			{
				Vector3 position = GameplayCamera.Position;
				Vector3 direction = GameplayCamera.Direction;
				RaycastResult raycastResult = World.Raycast(position, position + direction * 15f, IntersectFlags.LosToEntity, Helper.PP);
				if (raycastResult.DidHit)
				{
					World.DrawMarker(MarkerType.Cone, raycastResult.HitPosition + new Vector3(0f, 0f, 0.35f), Vector3.Zero, Vector3.Zero, new Vector3(0.25f, 0.25f, 0.25f), Color.FromArgb(200, 60, 200, 60));
				}
				if (Helper.ShowPersistentHelp("Aim at a money stack in the vault and press ~INPUT_CONTEXT~. ~INPUT_FRONTEND_CANCEL~ cancels.") && raycastResult.DidHit && Game.IsControlJustReleased(Control.Context))
				{
					CashPoint cashPoint = new CashPoint
					{
						Pos = raycastResult.HitPosition
					};
					try
					{
						Entity hitEntity = raycastResult.HitEntity;
						if ((object)hitEntity != null && hitEntity.Exists())
						{
							cashPoint.ModelHash = hitEntity.Model.Hash;
						}
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
					if (m.CashPoints.Count >= 64)
					{
						m.CashPoints.RemoveAt(0);
					}
					m.CashPoints.Add(cashPoint);
					Mansions.SaveMansions();
					Notification.Show($"{m.Name}: vault cash {m.CashPoints.Count} saved" + ((cashPoint.ModelHash != 0) ? " (the stack disappears when taken)." : " (no object found - the cash stays visible)."));
					Logger.Log($"> vault {m.Name}: cash point {cashPoint.Pos} model {cashPoint.ModelHash}");
					return true;
				}
				if (Game.IsControlJustReleased(Control.FrontendCancel) || Game.IsControlJustReleased(Control.PhoneCancel))
				{
					return false;
				}
				Script.Wait(0);
			}
			return false;
		}

		public static void RemoveNearestCash(Mansion m)
		{
			_Closure_0024__23_002D0 closure_0024__23_002D = new _Closure_0024__23_002D0();
			if (m.CashPoints.Count != 0)
			{
				closure_0024__23_002D._0024VB_0024Local_p = Helper.PP.Position;
				CashPoint cashPoint = m.CashPoints.OrderBy(closure_0024__23_002D._Lambda_0024__0).First();
				if (cashPoint.Hidden)
				{
					ShowCash(cashPoint);
				}
				m.CashPoints.Remove(cashPoint);
				Mansions.SaveMansions();
			}
		}

		public static void ClearCash(Mansion m)
		{
			foreach (CashPoint cashPoint in m.CashPoints)
			{
				if (cashPoint.Hidden)
				{
					ShowCash(cashPoint);
				}
			}
			m.CashPoints.Clear();
		}
	}
}
