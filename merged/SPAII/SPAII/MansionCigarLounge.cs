using System;
using System.Diagnostics;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class MansionCigarLounge
	{
		private class Session
		{
			public Mansion Home;

			public Ped Actor;

			public IntPtr ActorAddress;

			public int ActorModel;

			public Prop Sofa;

			public IntPtr SofaAddress;

			public Vector3 Origin;

			public Vector3 Rotation;

			public Prop Cigar;

			public IntPtr CigarAddress;

			public readonly Model CigarModel;

			public readonly Stopwatch Clock;

			public FwSyncedScene Scene;

			public int Stage;

			public long StageAt;

			public int StageFrame;

			public float Duration;

			public bool SawAnimation;

			public int Particle;

			public Session()
			{
				CigarModel = new Model("m25_2_prop_m52_cigar_lit");
				Clock = Stopwatch.StartNew();
				Stage = -1;
			}
		}

		private const string SofaName = "m25_2_prop_m52_sofa_01a";

		private const string CigarName = "m25_2_prop_m52_cigar_lit";

		private const string Dictionary = "anim@scripted@freemode@ig4_sit_cigar@male@";

		private const string ParticleDictionary = "scr_safehouse";

		private static readonly string[] pedClips = new string[3] { "enter", "action_smoke", "exit_cigar" };

		private static readonly string[] propClips = new string[3] { "enter_cigar", "action_smoke_cigar", "exit_cigar_cigar" };

		private static Session current;

		public static bool IsBusy => current != null;

		public static Quaternion[] SeatPoses(string key)
		{
			switch (key)
			{
			case "MANSION_VINEWOOD":
				return new Quaternion[2]
				{
					new Quaternion(558.1376f, 724.8394f, 201.3819f, 0f),
					new Quaternion(559.223f, 728.4606f, 201.3819f, 0f)
				};
			case "MANSION_RICHMAN":
				return new Quaternion[2]
				{
					new Quaternion(-1637.19f, 487.6807f, 128.2418f, 0f),
					new Quaternion(-1640.2f, 489.9678f, 128.2418f, 0f)
				};
			case "MANSION_TONGVA":
				return new Quaternion[2]
				{
					new Quaternion(-2583.491f, 1879.641f, 166.3957f, 0f),
					new Quaternion(-2580.635f, 1882.117f, 166.3957f, 0f)
				};
			default:
				return new Quaternion[0];
			}
		}

		private static bool AtCigarRoom(Mansion home, Vector3 position)
		{
			if (home == null || !home.Enabled || !home.IplsLoaded || !PartyPlacement.FinitePoint(position))
			{
				return false;
			}
			Quaternion[] array = SeatPoses(home.Key);
			foreach (Quaternion q in array)
			{
				Vector3 vector = position - Helper.ToVector3(q);
				if (Math.Abs(vector.Z) <= 1.5f && vector.X * vector.X + vector.Y * vector.Y <= 9f)
				{
					return true;
				}
			}
			return false;
		}

		private static Prop NearbySofa(Mansion home, Vector3 position)
		{
			if (!AtCigarRoom(home, position))
			{
				return null;
			}
			int num = Game.GenerateHash("m25_2_prop_m52_sofa_01a");
			int num2 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 2f, num, false, false, false);
			if (num2 == 0)
			{
				return null;
			}
			Prop prop = Entity.FromHandle(num2) as Prop;
			if ((object)prop == null || !prop.Exists() || prop.Model.Hash != num || !AtCigarRoom(home, prop.Position) || prop.Position.DistanceToSquared(position) > 4f)
			{
				return null;
			}
			return prop;
		}

		public static bool IsNear(Mansion home, Vector3 position)
		{
			return (object)NearbySofa(home, position) != null;
		}

		public static bool Tick(Mansion home)
		{
			Ped pP = Helper.PP;
			if ((object)pP == null || !pP.Exists() || pP.IsDead || pP.IsInVehicle() || pP.IsFalling || pP.IsRagdoll || DestinationStreaming.IsPlayerSafetyHeld(pP))
			{
				return false;
			}
			Prop prop = NearbySofa(home, pP.Position);
			if ((object)prop == null)
			{
				return false;
			}
			int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, pP.Handle);
			if (num == 0 || (num != home.InteriorId && num != home.BasementId))
			{
				return false;
			}
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to sit and smoke a cigar."))
			{
				return false;
			}
			if (Game.IsControlJustReleased(Control.Context) || Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, 51))
			{
				Session session = new Session
				{
					Home = home,
					Actor = pP,
					ActorAddress = pP.MemoryAddress,
					ActorModel = pP.Model.Hash,
					Sofa = prop,
					SofaAddress = prop.MemoryAddress,
					Origin = prop.Position,
					Rotation = new Vector3(0f, 0f, prop.Heading)
				};
				if (!session.CigarModel.IsValid || !session.CigarModel.IsInCdImage || !PartyPlacement.FinitePoint(session.Rotation))
				{
					return true;
				}
				current = session;
				try
				{
					PartyPlayerActions.StopPlayerDance();
					session.CigarModel.Request();
					Function.Call(Hash.REQUEST_ANIM_DICT, "anim@scripted@freemode@ig4_sit_cigar@male@");
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_safehouse");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Mansion cigar lounge request: " + ex2.Message);
					Cleanup();
					ProjectData.ClearProjectError();
				}
			}
			return true;
		}

		private static bool OwnActor(Session s)
		{
			if (s.ActorAddress != IntPtr.Zero && s.Actor.Exists() && s.Actor.MemoryAddress == s.ActorAddress)
			{
				return s.Actor.Model.Hash == s.ActorModel;
			}
			return false;
		}

		private static bool OwnCigar(Session s)
		{
			if ((object)s.Cigar != null && s.CigarAddress != IntPtr.Zero && s.Cigar.Exists() && s.Cigar.MemoryAddress == s.CigarAddress)
			{
				return s.Cigar.Model.Hash == s.CigarModel.Hash;
			}
			return false;
		}

		private static bool StartStage(Session s, int stage)
		{
			s.Scene = FwSyncedScene.Create(s.Origin, s.Rotation);
			if ((object)s.Scene == null)
			{
				return false;
			}
			s.Scene.IsLooped = false;
			s.Scene.HoldsLastFrame = true;
			s.Scene.Rate = 0f;
			s.Scene.Phase = 0f;
			if (!Function.Call<bool>(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, s.Cigar.Handle, s.Scene.Handle, propClips[stage], "anim@scripted@freemode@ig4_sit_cigar@male@", 16f, -16f, 0, 1000f))
			{
				return false;
			}
			s.Stage = stage;
			s.StageAt = s.Clock.ElapsedMilliseconds;
			s.StageFrame = Game.FrameCount;
			s.Duration = Function.Call<float>(Hash.GET_ANIM_DURATION, "anim@scripted@freemode@ig4_sit_cigar@male@", pedClips[stage]) * 1000f;
			s.SawAnimation = false;
			Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, s.Actor.Handle, s.Scene.Handle, "anim@scripted@freemode@ig4_sit_cigar@male@", pedClips[stage], 16f, -16f, 0, 0, 1000f, 0);
			return true;
		}

		public static void FrameTick()
		{
			Session session = current;
			if (session == null)
			{
				return;
			}
			if (!OwnActor(session) || (object)Helper.PP == null || Helper.PP.Handle != session.Actor.Handle || Game.Player.Character.Handle != session.Actor.Handle || session.Actor.IsDead || session.Actor.IsInVehicle() || session.Actor.IsRagdoll || session.Actor.IsFalling || !session.Home.IsOwnedByPlayer() || !AtCigarRoom(session.Home, session.Actor.Position) || !session.Sofa.Exists() || session.Sofa.MemoryAddress != session.SofaAddress || session.Sofa.Model.Hash != Game.GenerateHash("m25_2_prop_m52_sofa_01a") || DestinationStreaming.IsPlayerSafetyHeld(session.Actor) || Helper.MenuPool.IsAnyMenuOpen() || PartyPlayerActions.MovementRequested() || Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel))
			{
				Cleanup();
				return;
			}
			Helper.ShowPersistentHelp("Move or press ~INPUT_FRONTEND_CANCEL~ to stop.");
			checked
			{
				if (session.Stage < 0)
				{
					if (session.CigarModel.IsLoaded && Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@scripted@freemode@ig4_sit_cigar@male@"))
					{
						int num = pedClips.Length - 1;
						for (int i = 0; i <= num; i++)
						{
							if (Function.Call<float>(Hash.GET_ANIM_DURATION, "anim@scripted@freemode@ig4_sit_cigar@male@", pedClips[i]) <= 0f || Function.Call<float>(Hash.GET_ANIM_DURATION, "anim@scripted@freemode@ig4_sit_cigar@male@", propClips[i]) <= 0f)
							{
								Notification.Show("The cigar animations are unavailable.");
								Cleanup();
								return;
							}
						}
						session.Cigar = World.CreateProp(session.CigarModel, session.Origin, false, false);
						if ((object)session.Cigar == null || !session.Cigar.Exists())
						{
							Cleanup();
							return;
						}
						session.CigarAddress = session.Cigar.MemoryAddress;
						session.Cigar.IsPersistent = true;
						Function.Call(Hash.SET_ENTITY_COLLISION, session.Cigar.Handle, false, false);
						if (!StartStage(session, 0))
						{
							Cleanup();
						}
					}
					else if (session.Clock.ElapsedMilliseconds > 5000)
					{
						Notification.Show("The cigar scene did not load. Please try again.");
						Cleanup();
					}
					return;
				}
				if (!OwnCigar(session) || (object)session.Scene == null || !session.Scene.Exists())
				{
					Cleanup();
					return;
				}
				long num2 = session.Clock.ElapsedMilliseconds - session.StageAt;
				bool flag = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, session.Actor.Handle, "anim@scripted@freemode@ig4_sit_cigar@male@", pedClips[session.Stage], 3);
				bool flag2 = Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, session.Cigar.Handle, "anim@scripted@freemode@ig4_sit_cigar@male@", propClips[session.Stage], 3);
				if (!session.SawAnimation && Game.FrameCount != session.StageFrame && flag && flag2)
				{
					session.SawAnimation = true;
					session.Scene.Rate = 1f;
					if (session.Stage == 1 && Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_safehouse"))
					{
						Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_safehouse");
						session.Particle = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY, "scr_sh_cig_smoke", session.Cigar.Handle, 0.093f, 0f, 0f, 0f, 0f, 0f, 2.5f, false, false, false);
					}
				}
				if ((float)num2 > session.Duration + 2500f || (num2 > 1500 && (!flag || !flag2)))
				{
					Cleanup();
				}
				else if (session.SawAnimation && session.Scene.Phase >= 0.985f && (session.Stage == pedClips.Length - 1 || !StartStage(session, session.Stage + 1)))
				{
					Cleanup();
				}
			}
		}

		public static void Cleanup()
		{
			Session session = current;
			current = null;
			if (session == null)
			{
				return;
			}
			try
			{
				if ((object)session.Scene != null && session.Scene.Exists())
				{
					session.Scene.HoldsLastFrame = false;
					session.Scene.Rate = 1f;
				}
				if (session.Stage >= 0 && OwnActor(session) && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, session.Actor.Handle, "anim@scripted@freemode@ig4_sit_cigar@male@", pedClips[session.Stage], 3))
				{
					Function.Call(Hash.CLEAR_PED_TASKS, session.Actor.Handle);
				}
			}
			finally
			{
				if (session.Particle != 0 && OwnCigar(session))
				{
					Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, session.Particle, false);
				}
				if (OwnCigar(session))
				{
					session.Cigar.Delete();
				}
				session.CigarModel.MarkAsNoLongerNeeded();
				Function.Call(Hash.REMOVE_ANIM_DICT, "anim@scripted@freemode@ig4_sit_cigar@male@");
				Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "scr_safehouse");
			}
		}

		public static void ForgetForStoryLoad()
		{
			current = null;
		}
	}
}
