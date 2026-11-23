using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubStageFog
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<int, bool> _0024I13_002D0;

			public static Func<int, bool> _0024I14_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__13_002D0(int handle)
			{
				return handle != 0;
			}

			[SpecialName]
			internal bool _Lambda_0024__14_002D0(int handle)
			{
				return Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, handle);
			}
		}

		private const string Plume = "scr_ba_club_smoke_machine";

		private const int PulseMs = 8500;

		private const int RestMs = 5000;

		private static readonly Vector3 Stage = new Vector3(-1597.229f, -3012.469f, -78.8f);

		private static readonly Quaternion[] Nozzles = new Quaternion[4]
		{
			new Quaternion(-1602.932f, -3019.1f, -79.99f, 66f),
			new Quaternion(-1593.238f, -3017.05f, -79.99f, 110f),
			new Quaternion(-1597.134f, -3008.2f, -79.99f, -122.53f),
			new Quaternion(-1589.966f, -8178f / MathF.E, -79.99f, -166.97f)
		};

		private static readonly int[] emitters = new int[checked(Nozzles.Length - 1 + 1)];

		private static ExpansionSession active;

		private static bool holdingAsset;

		private static bool checkingStart;

		private static int nextChange;

		private static int failures;

		private static bool announced;

		internal static void Tick(ExpansionSession session)
		{
			if (active != session)
			{
				Reset(false);
				active = session;
			}
			checked
			{
				try
				{
					if (session.Interior <= 0 || !NightclubExpansionMenu.DryIceEnabled(session.Definition.Key) || session.Actor.Position.DistanceToSquared(Stage) > 784f)
					{
						StopFog(false);
						return;
					}
					int gameTime = Game.GameTime;
					if (gameTime < nextChange)
					{
						return;
					}
					if (checkingStart)
					{
						CheckEmitters(gameTime);
						return;
					}
					if (emitters.Any((_Closure_0024__._0024I13_002D0 != null) ? _Closure_0024__._0024I13_002D0 : (_Closure_0024__._0024I13_002D0 = _Closure_0024__._0024I._Lambda_0024__13_002D0)))
					{
						StopEmitters(false);
						nextChange = gameTime + 5000;
						return;
					}
					if (!holdingAsset)
					{
						holdingAsset = true;
						NightclubParticles.Acquire();
					}
					if (!NightclubParticles.Ready())
					{
						NightclubParticles.Request();
						nextChange = gameTime + 750;
						return;
					}
					int num = Nozzles.Length - 1;
					for (int i = 0; i <= num; i++)
					{
						Quaternion quaternion = Nozzles[i];
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_ba_club");
						emitters[i] = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_ba_club_smoke_machine", quaternion.X, quaternion.Y, quaternion.Z, 0f, -10f, quaternion.W, 5f, false, false, false, true);
					}
					checkingStart = true;
					nextChange = gameTime + 150;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					StopFog(false);
					nextChange = Game.GameTime + 5000;
					Logger.Log("After Hours fog: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void CheckEmitters(int now)
		{
			checkingStart = false;
			int num = KeepLiveEmitters(emitters, (_Closure_0024__._0024I14_002D0 != null) ? _Closure_0024__._0024I14_002D0 : (_Closure_0024__._0024I14_002D0 = _Closure_0024__._0024I._Lambda_0024__14_002D0));
			checked
			{
				if (num < emitters.Length)
				{
					failures++;
					if (failures == 1)
					{
						Logger.Log($"After Hours fog: {num} of {emitters.Length} nozzles started. Retrying missing nozzles next pulse.");
					}
				}
				else
				{
					failures = 0;
				}
				if (num == 0)
				{
					nextChange = now + 1500;
					return;
				}
				nextChange = now + 8500;
				if (!announced)
				{
					announced = true;
					Logger.Log($"After Hours fog: {num} stage emitters started for {active.Definition.Key}.");
				}
			}
		}

		private static int KeepLiveEmitters(int[] slots, Func<int, bool> exists)
		{
			checked
			{
				int num = slots.Length - 1;
				int num3 = default(int);
				for (int i = 0; i <= num; i++)
				{
					int num2 = slots[i];
					if (num2 != 0)
					{
						if (exists(num2))
						{
							num3++;
						}
						else
						{
							slots[i] = 0;
						}
					}
				}
				return num3;
			}
		}

		internal static void Close(ExpansionSession session)
		{
			if (active == session)
			{
				Reset(StoryLoadGuard.WorldLoading());
			}
		}

		internal static void Reset(bool storyLoad)
		{
			StopFog(storyLoad);
			active = null;
			failures = 0;
			announced = false;
		}

		private static void StopFog(bool storyLoad)
		{
			checkingStart = false;
			StopEmitters(storyLoad);
			if (holdingAsset)
			{
				holdingAsset = false;
				NightclubParticles.Release(storyLoad);
			}
			nextChange = 0;
		}

		private static void StopEmitters(bool storyLoad)
		{
			checked
			{
				int num = emitters.Length - 1;
				for (int i = 0; i <= num; i++)
				{
					int num2 = emitters[i];
					emitters[i] = 0;
					if (storyLoad || num2 == 0)
					{
						continue;
					}
					try
					{
						if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, num2))
						{
							Function.Call(Hash.REMOVE_PARTICLE_FX, num2, true);
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("After Hours fog cleanup: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
				}
			}
		}
	}
}
