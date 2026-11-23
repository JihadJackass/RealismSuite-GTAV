using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionArcade
	{
		internal class Cabinet
		{
			public Mansion Home;

			public int Index;

			public Prop Actor;

			public IntPtr Address;

			public Model Model;

			public Vector3 Minimum;

			public Vector3 Maximum;

			public bool Requested;

			public int ReportAt;

			public string LastReport;

			public Stopwatch Clock;

			public Cabinet()
			{
				Clock = Stopwatch.StartNew();
			}
		}

		public class Dot
		{
			public float X;

			public float Y;

			public float VX;

			public float VY;

			public bool Alive;

			public Dot()
			{
				Alive = true;
			}
		}

		public class ArcadeState
		{
			[Serializable]
			[CompilerGenerated]
			internal sealed class _Closure_0024__
			{
				public static readonly _Closure_0024__ _0024I;

				public static Func<Dot, bool> _0024I19_002D0;

				public static Func<Dot, bool> _0024I19_002D1;

				public static Predicate<Dot> _0024I19_002D2;

				public static Func<Dot, bool> _0024I19_002D3;

				public static Func<Dot, bool> _0024I19_002D4;

				public static Func<Dot, bool> _0024I19_002D5;

				public static Func<Dot, bool> _0024I21_002D0;

				public static Func<Dot, bool> _0024I21_002D1;

				public static Func<Dot, bool> _0024I22_002D0;

				public static Predicate<Dot> _0024I22_002D1;

				static _Closure_0024__()
				{
					_0024I = new _Closure_0024__();
				}

				[SpecialName]
				internal bool _Lambda_0024__19_002D0(Dot p)
				{
					return p.Alive;
				}

				[SpecialName]
				internal bool _Lambda_0024__19_002D1(Dot p)
				{
					return p.Alive;
				}

				[SpecialName]
				internal bool _Lambda_0024__19_002D2(Dot p)
				{
					return !p.Alive;
				}

				[SpecialName]
				internal bool _Lambda_0024__19_002D3(Dot p)
				{
					return p.Alive;
				}

				[SpecialName]
				internal bool _Lambda_0024__19_002D4(Dot p)
				{
					if (p.Alive)
					{
						return p.Y >= 0.83f;
					}
					return false;
				}

				[SpecialName]
				internal bool _Lambda_0024__19_002D5(Dot p)
				{
					return !p.Alive;
				}

				[SpecialName]
				internal bool _Lambda_0024__21_002D0(Dot p)
				{
					return p.Alive;
				}

				[SpecialName]
				internal bool _Lambda_0024__21_002D1(Dot p)
				{
					return !p.Alive;
				}

				[SpecialName]
				internal bool _Lambda_0024__22_002D0(Dot p)
				{
					return p.Alive;
				}

				[SpecialName]
				internal bool _Lambda_0024__22_002D1(Dot p)
				{
					return !p.Alive;
				}
			}

			public readonly int Kind;

			public int Score;

			public int Lives;

			public float PlayerX;

			public Dot Ball;

			public bool Launched;

			public readonly List<Dot> Targets;

			public readonly List<Dot> Shots;

			public int Level;

			private double clock;

			private double fireAt;

			private double spawnAt;

			private double invulnerableUntil;

			private int spawnNumber;

			public bool GameOver => Lives <= 0;

			public ArcadeState(int kind)
			{
				Lives = 3;
				PlayerX = 0.5f;
				Ball = new Dot
				{
					X = 0.5f,
					Y = 0.84f,
					VX = 0.32f,
					VY = -0.45f
				};
				Targets = new List<Dot>();
				Shots = new List<Dot>();
				Level = 1;
				if (kind < 0 || kind > 2)
				{
					throw new ArgumentOutOfRangeException("kind");
				}
				Kind = kind;
				ResetTargets();
			}

			private void ResetTargets()
			{
				Targets.Clear();
				checked
				{
					if (Kind == 0)
					{
						int num = 0;
						do
						{
							int num2 = 0;
							do
							{
								Targets.Add(new Dot
								{
									X = 0.18f + (float)num2 * 0.16f,
									Y = 0.15f + (float)num * 0.09f
								});
								num2++;
							}
							while (num2 <= 4);
							num++;
						}
						while (num <= 2);
					}
					else
					{
						if (Kind != 1)
						{
							return;
						}
						int num3 = 0;
						do
						{
							int num4 = 0;
							do
							{
								Targets.Add(new Dot
								{
									X = 0.1f + (float)num4 * 0.16f,
									Y = 0.16f + (float)num3 * 0.07f
								});
								num4++;
							}
							while (num4 <= 5);
							num3++;
						}
						while (num3 <= 3);
					}
				}
			}

			public void Advance(float seconds, float axis, bool fire)
			{
				if (GameOver || float.IsNaN(seconds) || float.IsInfinity(seconds) || seconds <= 0f || float.IsNaN(axis) || float.IsInfinity(axis))
				{
					return;
				}
				float num = Math.Min(seconds, 0.05f);
				axis = Math.Max(-1f, Math.Min(1f, axis));
				while (num > 0f)
				{
					float num2 = Math.Min(num, 1f / 120f);
					num -= num2;
					clock += num2;
					PlayerX = Math.Max(0.08f, Math.Min(0.92f, PlayerX + axis * num2 * 0.72f));
					switch (Kind)
					{
					case 0:
						Shooter(num2, fire);
						break;
					case 1:
						Breaker(num2, fire);
						break;
					case 2:
						Runner(num2);
						break;
					}
					if (GameOver)
					{
						break;
					}
				}
			}

			private void Shooter(float dt, bool fire)
			{
				if (fire && clock >= fireAt)
				{
					Shots.Add(new Dot
					{
						X = PlayerX,
						Y = 0.84f
					});
					fireAt = clock + 0.22;
				}
				Dot[] array = Shots.Where((_Closure_0024__._0024I19_002D0 != null) ? _Closure_0024__._0024I19_002D0 : (_Closure_0024__._0024I19_002D0 = _Closure_0024__._0024I._Lambda_0024__19_002D0)).ToArray();
				checked
				{
					foreach (Dot dot in array)
					{
						dot.Y -= dt * 0.95f;
						if (dot.Y < 0f)
						{
							dot.Alive = false;
							continue;
						}
						foreach (Dot item in Targets.Where((_Closure_0024__._0024I19_002D1 != null) ? _Closure_0024__._0024I19_002D1 : (_Closure_0024__._0024I19_002D1 = _Closure_0024__._0024I._Lambda_0024__19_002D1)))
						{
							if (Math.Abs(dot.X - EnemyX(item)) < 0.055f && Math.Abs(dot.Y - item.Y) < 0.045f)
							{
								item.Alive = false;
								dot.Alive = false;
								Score += 20;
								break;
							}
						}
					}
					Shots.RemoveAll((_Closure_0024__._0024I19_002D2 != null) ? _Closure_0024__._0024I19_002D2 : (_Closure_0024__._0024I19_002D2 = _Closure_0024__._0024I._Lambda_0024__19_002D2));
					foreach (Dot item2 in Targets.Where((_Closure_0024__._0024I19_002D3 != null) ? _Closure_0024__._0024I19_002D3 : (_Closure_0024__._0024I19_002D3 = _Closure_0024__._0024I._Lambda_0024__19_002D3)))
					{
						item2.Y += dt * (0.018f + (float)Level * 0.003f);
					}
					if (Targets.Any((_Closure_0024__._0024I19_002D4 != null) ? _Closure_0024__._0024I19_002D4 : (_Closure_0024__._0024I19_002D4 = _Closure_0024__._0024I._Lambda_0024__19_002D4)))
					{
						Lives--;
						ResetTargets();
						Shots.Clear();
					}
					else if (Targets.All((_Closure_0024__._0024I19_002D5 != null) ? _Closure_0024__._0024I19_002D5 : (_Closure_0024__._0024I19_002D5 = _Closure_0024__._0024I._Lambda_0024__19_002D5)))
					{
						Level++;
						Score += 100;
						ResetTargets();
					}
				}
			}

			public float EnemyX(Dot target)
			{
				return target.X + (float)(Math.Sin(clock * 1.5) * 0.08);
			}

			private void Breaker(float dt, bool fire)
			{
				if (!Launched)
				{
					Ball.X = PlayerX;
					Ball.Y = 0.84f;
					if (fire)
					{
						Launched = true;
					}
					return;
				}
				float y = Ball.Y;
				Ball.X += Ball.VX * dt;
				Ball.Y += Ball.VY * dt;
				if (Ball.X < 0.025f || Ball.X > 0.975f)
				{
					Ball.X = Math.Max(0.025f, Math.Min(0.975f, Ball.X));
					Ball.VX = 0f - Ball.VX;
				}
				if (Ball.Y < 0.08f)
				{
					Ball.Y = 0.08f;
					Ball.VY = Math.Abs(Ball.VY);
				}
				if (Ball.VY > 0f && y <= 0.88f && Ball.Y >= 0.88f && Math.Abs(Ball.X - PlayerX) <= 0.115f)
				{
					Ball.Y = 0.875f;
					Ball.VY = 0f - Math.Abs(Ball.VY);
					Ball.VX = (Ball.X - PlayerX) * 3f;
				}
				checked
				{
					foreach (Dot item in Targets.Where((_Closure_0024__._0024I21_002D0 != null) ? _Closure_0024__._0024I21_002D0 : (_Closure_0024__._0024I21_002D0 = _Closure_0024__._0024I._Lambda_0024__21_002D0)))
					{
						if (Math.Abs(Ball.X - item.X) < 0.075f && Math.Abs(Ball.Y - item.Y) < 0.035f)
						{
							item.Alive = false;
							Score += 10;
							Ball.VY = 0f - Ball.VY;
							break;
						}
					}
					if (Ball.Y > 1f)
					{
						Lives--;
						Launched = false;
						Ball.VX = 0.32f;
						Ball.VY = -0.45f;
					}
					else if (Targets.All((_Closure_0024__._0024I21_002D1 != null) ? _Closure_0024__._0024I21_002D1 : (_Closure_0024__._0024I21_002D1 = _Closure_0024__._0024I._Lambda_0024__21_002D1)))
					{
						Level++;
						Score += 100;
						ResetTargets();
						Launched = false;
						Ball.VX = 0.32f;
						Ball.VY = 0f - Math.Min(0.8f, 0.45f + (float)Level * 0.03f);
					}
				}
			}

			private void Runner(float dt)
			{
				if (clock >= spawnAt)
				{
					int num = checked(spawnNumber * 7 + 3) % 5;
					Targets.Add(new Dot
					{
						X = 0.14f + (float)num * 0.18f,
						Y = 0.05f
					});
					checked
					{
						spawnNumber++;
						spawnAt = clock + Math.Max(0.35, 0.8 - (double)Level * 0.025);
					}
				}
				checked
				{
					foreach (Dot item in Targets.Where((_Closure_0024__._0024I22_002D0 != null) ? _Closure_0024__._0024I22_002D0 : (_Closure_0024__._0024I22_002D0 = _Closure_0024__._0024I._Lambda_0024__22_002D0)))
					{
						item.Y += dt * (0.28f + (float)Level * 0.02f);
						if (Math.Abs(item.Y - 0.86f) < 0.055f && Math.Abs(item.X - PlayerX) < 0.075f && clock >= invulnerableUntil)
						{
							item.Alive = false;
							Lives--;
							invulnerableUntil = clock + 0.8;
						}
						else if (item.Y > 1f)
						{
							item.Alive = false;
							Score += 10;
							Level = 1 + unchecked(Score / 150);
						}
					}
					Targets.RemoveAll((_Closure_0024__._0024I22_002D1 != null) ? _Closure_0024__._0024I22_002D1 : (_Closure_0024__._0024I22_002D1 = _Closure_0024__._0024I._Lambda_0024__22_002D1));
				}
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static ItemSelectEvent _0024I37_002D0;

			public static Func<Dot, bool> _0024I45_002D0;

			public static Func<Dot, bool> _0024I45_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal void _Lambda_0024__37_002D0(UIMenu sender, UIMenuItem item, int index)
			{
				if (Start(selectedHome, Conversions.ToInteger(item.Tag)))
				{
					chooser.Visible = false;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__45_002D0(Dot p)
			{
				return p.Alive;
			}

			[SpecialName]
			internal bool _Lambda_0024__45_002D1(Dot p)
			{
				return p.Alive;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__27_002D0
		{
			public Mansion _0024VB_0024Local_m;

			internal Func<Cabinet, bool> _0024I0;

			public _Closure_0024__27_002D0(_Closure_0024__27_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_m = arg0._0024VB_0024Local_m;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Cabinet p)
			{
				return p.Home == _0024VB_0024Local_m;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__28_002D0
		{
			public Mansion _0024VB_0024Local_m;

			public Vector3 _0024VB_0024Local_position;

			[SpecialName]
			internal bool _Lambda_0024__0(Cabinet c)
			{
				if (c.Home == _0024VB_0024Local_m)
				{
					return NearCabinet(c, _0024VB_0024Local_position);
				}
				return false;
			}

			[SpecialName]
			internal float _Lambda_0024__1(Cabinet c)
			{
				return c.Actor.Position.DistanceToSquared(_0024VB_0024Local_position);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__35_002D0
		{
			public Mansion _0024VB_0024Local_m;

			public _Closure_0024__35_002D0(_Closure_0024__35_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_m = arg0._0024VB_0024Local_m;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__35_002D1
		{
			public int _0024VB_0024Local_cabinetIndex;

			public _Closure_0024__35_002D0 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__35_002D1(_Closure_0024__35_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_cabinetIndex = arg0._0024VB_0024Local_cabinetIndex;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(Cabinet p)
			{
				if (p.Home == _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m)
				{
					return p.Index == _0024VB_0024Local_cabinetIndex;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__38_002D0
		{
			public Mansion _0024VB_0024Local_m;

			public int _0024VB_0024Local_index;

			[SpecialName]
			internal bool _Lambda_0024__0(Cabinet c)
			{
				if (c.Home == _0024VB_0024Local_m && c.Index == _0024VB_0024Local_index)
				{
					return NearCabinet(c, Helper.PP.Position);
				}
				return false;
			}
		}

		public static readonly string[] Titles = new string[3] { "Star Defender", "Brick Breaker", "Lane Runner" };

		public static readonly string[] CabinetModels = new string[3] { "ch_prop_arcade_invade_01a", "ch_prop_arcade_degenatron_01a", "ch_prop_arcade_monkey_01a" };

		private static readonly List<Cabinet> cabinets = new List<Cabinet>();

		private static readonly int[] best = new int[3];

		private static Ped actor;

		private static IntPtr actorAddress;

		private static int actorModel;

		private static Mansion home;

		private static Vector3 origin;

		private static bool wasFrozen;

		private static Stopwatch playClock;

		private static long lastElapsed;

		private static ArcadeState run;

		private static UIMenu chooser;

		private static Mansion selectedHome;

		private static readonly Dictionary<Mansion, int> nextScan = new Dictionary<Mansion, int>();

		private static int maintenanceFrame = -1;

		private static Cabinet promptCabinet;

		public static bool IsPlaying => run != null;

		public static Quaternion CabinetPoint(Mansion m, int index)
		{
			switch (index)
			{
			case 0:
				return MansionAutoLayout.MainPoint(m, -11.8617f, -6.01872f, -3.65443f, 180f);
			case 1:
				return MansionAutoLayout.MainPoint(m, -13.7492f, -6.01872f, -3.65443f, 180f);
			case 2:
				return MansionAutoLayout.MainPoint(m, -13.2441f, 0.573402f, -3.65443f);
			default:
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public static bool FrontInteractionNear(Vector3 position, Vector3 cabinetOrigin, float heading, Vector3 minimum, Vector3 maximum)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(cabinetOrigin) || !PartyPlacement.FinitePoint(minimum) || !PartyPlacement.FinitePoint(maximum) || float.IsNaN(heading) || float.IsInfinity(heading) || maximum.X <= minimum.X || maximum.Y <= minimum.Y || maximum.Z <= minimum.Z)
			{
				return false;
			}
			Vector3 vector = MansionGaragePlacement.GarageLocalPoint(position, cabinetOrigin, heading);
			float z = minimum.Z;
			return vector.Z >= z + 0.1f && vector.Z <= z + 2f && vector.X >= minimum.X - 0.45f && vector.X <= maximum.X + 0.45f && vector.Y >= Math.Min(minimum.Y, -0.5f) - 1.65f && vector.Y <= Math.Min(maximum.Y + 0.1f, 0.35f);
		}

		private static bool NearCabinet(Cabinet c, Vector3 position)
		{
			if (OwnCabinet(c))
			{
				return FrontInteractionNear(position, c.Actor.Position, c.Actor.Heading, c.Minimum, c.Maximum);
			}
			return false;
		}

		public static bool IsNear(Mansion m, Vector3 position)
		{
			_Closure_0024__27_002D0 arg = new _Closure_0024__27_002D0(null);
			arg._0024VB_0024Local_m = m;
			if (arg._0024VB_0024Local_m == null || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			foreach (Cabinet item in cabinets.Where((arg._0024I0 == null) ? (arg._0024I0 = arg._Lambda_0024__0) : arg._0024I0))
			{
				if (NearCabinet(item, position))
				{
					return true;
				}
			}
			int num = 0;
			do
			{
				Quaternion q = CabinetPoint(arg._0024VB_0024Local_m, num);
				if (FrontInteractionNear(position, Helper.ToVector3(q), q.W, new Vector3(-0.6f, -0.7f, 0f), new Vector3(0.6f, 0.6f, 2.2f)))
				{
					return true;
				}
				num = checked(num + 1);
			}
			while (num <= 2);
			return false;
		}

		private static Cabinet NearestCabinet(Mansion m, Vector3 position)
		{
			_Closure_0024__28_002D0 closure_0024__28_002D = new _Closure_0024__28_002D0();
			closure_0024__28_002D._0024VB_0024Local_m = m;
			closure_0024__28_002D._0024VB_0024Local_position = position;
			return cabinets.Where(closure_0024__28_002D._Lambda_0024__0).OrderBy(closure_0024__28_002D._Lambda_0024__1).FirstOrDefault();
		}

		public static bool CabinetSightClear(int status, bool didHit, int hitEntity, int cabinetEntity)
		{
			if (status == 2 && cabinetEntity != 0)
			{
				if (didHit)
				{
					return hitEntity == cabinetEntity;
				}
				return true;
			}
			return false;
		}

		private static bool CabinetVisible(Cabinet c)
		{
			if (!OwnCabinet(c))
			{
				return false;
			}
			Vector3 position = c.Actor.Position;
			double num = (double)c.Actor.Heading * Math.PI / 180.0;
			Vector3 vector = new Vector3((c.Minimum.X + c.Maximum.X) * 0.5f, -0.5f, c.Minimum.Z + 1.3f);
			Vector3 vector2 = new Vector3(position.X + (float)(Math.Cos(num) * (double)vector.X - Math.Sin(num) * (double)vector.Y), position.Y + (float)(Math.Sin(num) * (double)vector.X + Math.Cos(num) * (double)vector.Y), position.Z + vector.Z);
			Vector3 vector3 = Helper.PP.Position + new Vector3(0f, 0f, 0.45f);
			int num2 = Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE, vector3.X, vector3.Y, vector3.Z, vector2.X, vector2.Y, vector2.Z, 81, Helper.PP.Handle, 4);
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			OutputArgument outputArgument3 = new OutputArgument();
			OutputArgument outputArgument4 = new OutputArgument();
			int num3 = Function.Call<int>(Hash.GET_SHAPE_TEST_RESULT, num2, outputArgument, outputArgument2, outputArgument3, outputArgument4);
			if (num3 != 2)
			{
				return false;
			}
			return CabinetSightClear(num3, outputArgument.GetResult<int>() != 0, outputArgument4.GetResult<int>(), c.Actor.Handle);
		}

		private static void Report(Cabinet c, string reason)
		{
			if (c != null && (Operators.CompareString(c.LastReport, reason, false) != 0 || Game.GameTime >= c.ReportAt))
			{
				c.LastReport = reason;
				c.ReportAt = checked(Game.GameTime + 10000);
				Logger.Log("Mansion arcade: " + c.Home.Name + "; cabinet=" + Conversions.ToString(c.Index) + "; " + reason);
			}
		}

		private static bool ContextReleased()
		{
			if (!Game.IsControlJustReleased(Control.Context))
			{
				return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, 51);
			}
			return true;
		}

		private static bool OwnCabinet(Cabinet c)
		{
			if ((object)c.Actor != null && c.Actor.Exists() && c.Actor.MemoryAddress == c.Address)
			{
				return c.Actor.Model.Hash == c.Model.Hash;
			}
			return false;
		}

		private static void DisposeCabinet(Cabinet c)
		{
			try
			{
				if (OwnCabinet(c))
				{
					c.Actor.Delete();
				}
			}
			finally
			{
				if (c.Requested)
				{
					c.Model.MarkAsNoLongerNeeded();
					c.Requested = false;
				}
				cabinets.Remove(c);
			}
		}

		private static void Maintain(Mansion m)
		{
			_Closure_0024__35_002D0 arg = new _Closure_0024__35_002D0(null);
			arg._0024VB_0024Local_m = m;
			int value;
			if (nextScan.TryGetValue(arg._0024VB_0024Local_m, out value) && Game.GameTime < value)
			{
				return;
			}
			checked
			{
				nextScan[arg._0024VB_0024Local_m] = Game.GameTime + 700;
				Cabinet[] array = cabinets.ToArray();
				foreach (Cabinet cabinet in array)
				{
					if (!cabinet.Home.Enabled || !cabinet.Home.IplsLoaded || !cabinet.Home.IsOwnedByPlayer() || Helper.PP.Position.DistanceTo(Helper.ToVector3(CabinetPoint(cabinet.Home, 0))) > 70f)
					{
						DisposeCabinet(cabinet);
					}
				}
				if (!arg._0024VB_0024Local_m.Enabled || !arg._0024VB_0024Local_m.IplsLoaded || !arg._0024VB_0024Local_m.IsOwnedByPlayer() || Helper.PP.Position.DistanceTo(Helper.ToVector3(CabinetPoint(arg._0024VB_0024Local_m, 0))) > 30f)
				{
					return;
				}
				int num = 0;
				_Closure_0024__35_002D1 closure_0024__35_002D = default(_Closure_0024__35_002D1);
				Vector3 floorPoint = default(Vector3);
				do
				{
					closure_0024__35_002D = new _Closure_0024__35_002D1(closure_0024__35_002D);
					closure_0024__35_002D._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
					closure_0024__35_002D._0024VB_0024Local_cabinetIndex = num;
					Cabinet cabinet2 = cabinets.FirstOrDefault(closure_0024__35_002D._Lambda_0024__0);
					if (cabinet2 == null)
					{
						cabinet2 = new Cabinet
						{
							Home = closure_0024__35_002D._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m,
							Index = num,
							Model = new Model(CabinetModels[num])
						};
						cabinets.Add(cabinet2);
					}
					if (!OwnCabinet(cabinet2) && cabinet2.Model.IsValid && cabinet2.Model.IsInCdImage)
					{
						if (!cabinet2.Requested)
						{
							cabinet2.Model.Request();
							cabinet2.Requested = true;
							cabinet2.Clock.Restart();
						}
						if (cabinet2.Clock.ElapsedMilliseconds > 10000)
						{
							cabinet2.Model.MarkAsNoLongerNeeded();
							cabinet2.Requested = false;
						}
						else if (cabinet2.Model.IsLoaded)
						{
							Quaternion q = CabinetPoint(closure_0024__35_002D._0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_m, num);
							if (PartyPlacement.TryGetFloor(Helper.PP.Position, Helper.ToVector3(q) + new Vector3(0f, 0f, 1f), ref floorPoint) && !(Math.Abs(floorPoint.Z - q.Z) > 0.3f))
							{
								Vector3 min;
								Vector3 max;
								cabinet2.Model.GetDimensions(out min, out max);
								cabinet2.Minimum = min;
								cabinet2.Maximum = max;
								cabinet2.Actor = World.CreateProp(cabinet2.Model, floorPoint - new Vector3(0f, 0f, min.Z), false, false);
								if ((object)cabinet2.Actor != null && cabinet2.Actor.Exists())
								{
									cabinet2.Address = cabinet2.Actor.MemoryAddress;
									cabinet2.Actor.Position = floorPoint - new Vector3(0f, 0f, min.Z);
									cabinet2.Actor.Heading = q.W;
									cabinet2.Actor.IsPersistent = true;
									Function.Call(Hash.FREEZE_ENTITY_POSITION, cabinet2.Actor.Handle, true);
									cabinet2.Actor.IsInvincible = true;
									cabinet2.Model.MarkAsNoLongerNeeded();
									cabinet2.Requested = false;
									Report(cabinet2, "ready; origin=" + cabinet2.Actor.Position.ToString() + "; min=" + min.ToString() + "; max=" + max.ToString());
								}
							}
						}
					}
					num++;
				}
				while (num <= 2);
			}
		}

		public static bool Tick(Mansion m)
		{
			if ((object)Helper.PP == null || !Helper.PP.Exists() || m == null)
			{
				return false;
			}
			Maintain(m);
			if (IsPlaying)
			{
				return true;
			}
			if (m == null || !m.IsOwnedByPlayer() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive)
			{
				return false;
			}
			Cabinet cabinet = NearestCabinet(m, Helper.PP.Position);
			if (cabinet == null)
			{
				promptCabinet = null;
				return false;
			}
			if (!CabinetVisible(cabinet))
			{
				Report(cabinet, "front reached but physical sight is blocked or collision is not ready");
				promptCabinet = null;
				return false;
			}
			if (promptCabinet != cabinet)
			{
				Report(cabinet, "physical play prompt active at " + Helper.PP.Position.ToString());
				promptCabinet = cabinet;
			}
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to play " + Titles[cabinet.Index] + "."))
			{
				return false;
			}
			if (ContextReleased())
			{
				Start(m, cabinet.Index);
			}
			return true;
		}

		public static void Open(Mansion m, UIMenu parent)
		{
			if (m == null || !m.IsOwnedByPlayer() || IsPlaying)
			{
				return;
			}
			checked
			{
				if (chooser == null)
				{
					chooser = new UIMenu("", "MANSION ARCADE", new Point(0, -107));
					chooser.SetBannerType(Helper.MenuBanner);
					chooser.MouseEdgeEnabled = false;
					Helper.MenuPool.Add(chooser);
					int num = Titles.Length - 1;
					for (int i = 0; i <= num; i++)
					{
						chooser.AddItem(new UIMenuItem(Titles[i], "Walk to the arcade cabinets to play. Free play; no bets or payouts.")
						{
							Tag = i
						});
					}
					chooser.OnItemSelect += ((_Closure_0024__._0024I37_002D0 != null) ? _Closure_0024__._0024I37_002D0 : (_Closure_0024__._0024I37_002D0 = _Closure_0024__._0024I._Lambda_0024__37_002D0));
				}
				selectedHome = m;
				chooser.ParentMenu = parent;
				if (parent != null)
				{
					parent.Visible = false;
				}
				chooser.RefreshIndex();
				chooser.Visible = true;
			}
		}

		private static bool Start(Mansion m, int index)
		{
			_Closure_0024__38_002D0 closure_0024__38_002D = new _Closure_0024__38_002D0();
			closure_0024__38_002D._0024VB_0024Local_m = m;
			closure_0024__38_002D._0024VB_0024Local_index = index;
			if (closure_0024__38_002D._0024VB_0024Local_index < 0 || closure_0024__38_002D._0024VB_0024Local_index >= Titles.Length)
			{
				return false;
			}
			if (closure_0024__38_002D._0024VB_0024Local_m == null || !closure_0024__38_002D._0024VB_0024Local_m.Enabled || !closure_0024__38_002D._0024VB_0024Local_m.IplsLoaded || !closure_0024__38_002D._0024VB_0024Local_m.IsOwnedByPlayer() || IsPlaying || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				return false;
			}
			Cabinet cabinet = cabinets.FirstOrDefault(closure_0024__38_002D._Lambda_0024__0);
			if (cabinet == null || !CabinetVisible(cabinet))
			{
				Notification.Show("Go to the " + Titles[closure_0024__38_002D._0024VB_0024Local_index] + " cabinet in the mansion arcade room first.");
				return false;
			}
			actor = Helper.PP;
			actorAddress = actor.MemoryAddress;
			actorModel = actor.Model.Hash;
			origin = actor.Position;
			home = closure_0024__38_002D._0024VB_0024Local_m;
			wasFrozen = actor.IsPositionFrozen;
			try
			{
				actor.IsPositionFrozen = true;
				PartyPlayerActions.StopPlayerDance();
				run = new ArcadeState(closure_0024__38_002D._0024VB_0024Local_index);
				playClock = Stopwatch.StartNew();
				lastElapsed = 0L;
				Report(cabinet, "game started: " + Titles[closure_0024__38_002D._0024VB_0024Local_index]);
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion arcade start: " + ex2.Message);
				StopGame();
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static bool Pressed(Control control)
		{
			if (!Game.IsControlPressed(control))
			{
				return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, (int)control);
			}
			return true;
		}

		private static bool Tapped(Control control)
		{
			if (!Game.IsControlJustPressed(control))
			{
				return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, (int)control);
			}
			return true;
		}

		public static void FrameTick()
		{
			checked
			{
				if ((object)Helper.PP != null && Helper.PP.Exists() && maintenanceFrame != Game.FrameCount)
				{
					maintenanceFrame = Game.FrameCount;
					Mansion[] array = Mansions.MansionList.ToArray();
					for (int i = 0; i < array.Length; i++)
					{
						Maintain(array[i]);
					}
				}
				if (run == null)
				{
					return;
				}
				try
				{
					if ((object)actor == null || !actor.Exists() || actor.MemoryAddress != actorAddress || actor.Model.Hash != actorModel || (object)Helper.PP == null || Helper.PP.Handle != actor.Handle || actor.IsDead || actor.IsInVehicle() || !home.Enabled || !home.IplsLoaded || !home.IsOwnedByPlayer() || actor.Position.DistanceTo(origin) > 2f || Helper.MenuPool.IsAnyMenuOpen())
					{
						StopGame();
						return;
					}
					float axis = Game.GetDisabledControlValueNormalized(Control.MoveLeftRight);
					if (Pressed(Control.FrontendLeft))
					{
						axis = -1f;
					}
					if (Pressed(Control.FrontendRight))
					{
						axis = 1f;
					}
					bool fire = Pressed(Control.Jump) || Pressed(Control.FrontendAccept);
					bool num = Tapped(Control.FrontendCancel) || Tapped(Control.PhoneCancel);
					Function.Call(Hash.DISABLE_ALL_CONTROL_ACTIONS, 0);
					if (num)
					{
						StopGame();
						return;
					}
					long elapsedMilliseconds = playClock.ElapsedMilliseconds;
					float seconds = (float)((double)(elapsedMilliseconds - lastElapsed) / 1000.0);
					lastElapsed = elapsedMilliseconds;
					if (run.GameOver)
					{
						if (Tapped(Control.FrontendAccept) || Tapped(Control.Jump))
						{
							run = new ArcadeState(run.Kind);
						}
					}
					else
					{
						int score = run.Score;
						int lives = run.Lives;
						run.Advance(seconds, axis, fire);
						if (run.Lives < lives)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
						}
						else if (run.Score > score)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true);
						}
						best[run.Kind] = Math.Max(best[run.Kind], run.Score);
					}
					DrawGame();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Mansion arcade: " + ex2.Message);
					StopGame();
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void Rect(float x, float y, float width, float height, int r, int g, int b, int a = 255)
		{
			Function.Call(Hash.DRAW_RECT, x, y, width, height, r, g, b, a);
		}

		private static void TextAt(string caption, float x, float y, float size, int r, int g, int b)
		{
			Function.Call(Hash.SET_TEXT_FONT, 0);
			Function.Call(Hash.SET_TEXT_SCALE, size, size);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_CENTRE, true);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, caption);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0);
		}

		private static void Tile(float x, float y, float width, float height, int r, int g, int b)
		{
			Rect(0.2f + x * 0.6f, 0.15f + y * 0.65f, width * 0.6f, height * 0.65f, r, g, b);
		}

		private static void DrawGame()
		{
			Rect(0.5f, 0.5f, 1f, 1f, 0, 0, 0, 235);
			Rect(0.5f, 0.47f, 0.62f, 0.69f, 22, 32, 51);
			TextAt(Titles[run.Kind], 0.5f, 0.06f, 0.65f, 255, 210, 50);
			TextAt("SCORE " + Conversions.ToString(run.Score) + "     LIVES " + Conversions.ToString(run.Lives) + "     SESSION BEST " + Conversions.ToString(best[run.Kind]), 0.5f, 0.11f, 0.32f, 230, 240, 255);
			Tile(run.PlayerX, 0.9f, (run.Kind == 1) ? 0.2f : 0.065f, 0.025f, 60, 240, 190);
			switch (run.Kind)
			{
			case 0:
				foreach (Dot item in run.Targets.Where((_Closure_0024__._0024I45_002D0 != null) ? _Closure_0024__._0024I45_002D0 : (_Closure_0024__._0024I45_002D0 = _Closure_0024__._0024I._Lambda_0024__45_002D0)))
				{
					Tile(run.EnemyX(item), item.Y, 0.075f, 0.045f, 240, 90, 180);
				}
				foreach (Dot shot in run.Shots)
				{
					Tile(shot.X, shot.Y, 0.01f, 0.03f, 255, 240, 150);
				}
				break;
			case 1:
				foreach (Dot item2 in run.Targets.Where((_Closure_0024__._0024I45_002D1 != null) ? _Closure_0024__._0024I45_002D1 : (_Closure_0024__._0024I45_002D1 = _Closure_0024__._0024I._Lambda_0024__45_002D1)))
				{
					Tile(item2.X, item2.Y, 0.14f, 0.05f, 235, 130, 50);
				}
				Tile(run.Ball.X, run.Ball.Y, 0.025f, 0.025f, 255, 255, 255);
				break;
			case 2:
			{
				int num = 1;
				do
				{
					Tile((float)num / 5f, 0.5f, 0.002f, 0.9f, 60, 70, 95);
					num = checked(num + 1);
				}
				while (num <= 4);
				foreach (Dot target in run.Targets)
				{
					Tile(target.X, target.Y, 0.09f, 0.06f, 255, 100, 70);
				}
				break;
			}
			}
			if (run.GameOver)
			{
				TextAt("GAME OVER - press ~INPUT_FRONTEND_ACCEPT~ to restart", 0.5f, 0.45f, 0.5f, 255, 255, 255);
			}
			TextAt("A/D, arrows or left stick: move   |   Space / A: fire or launch   |   Back / B: quit", 0.5f, 0.86f, 0.31f, 220, 230, 255);
		}

		private static void StopGame()
		{
			run = null;
			try
			{
				if ((object)actor != null && actor.Exists() && actor.MemoryAddress == actorAddress && actor.Model.Hash == actorModel)
				{
					actor.IsPositionFrozen = wasFrozen;
				}
			}
			finally
			{
				actor = null;
				home = null;
				playClock = null;
			}
		}

		public static void Cleanup()
		{
			StopGame();
			Cabinet[] array = cabinets.ToArray();
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				DisposeCabinet(array[i]);
			}
			nextScan.Clear();
			promptCabinet = null;
			maintenanceFrame = -1;
			if (chooser != null)
			{
				chooser.Visible = false;
			}
		}
	}
}
