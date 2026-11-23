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
	public static class MansionPetCare
	{
		private class Session
		{
			public Ped Player;

			public Ped Animal;

			public Mansion Home;

			public IntPtr PlayerAddress;

			public IntPtr AnimalAddress;

			public int PlayerModel;

			public int AnimalModel;

			public bool Cat;

			public string Name;

			public string Kind;

			public string Dict;

			public string PlayerClip;

			public string AnimalClip;

			public int Stage;

			public Stopwatch Clock;

			public long StageAt;

			public Vector3 Origin;

			public Vector3 AnimalOrigin;

			public Vector3 Target;

			public Vector3 Floor;

			public Vector3 SceneOrigin;

			public Vector3 Rotation;

			public FwSyncedScene Scene;

			public Prop Prop;

			public IntPtr PropAddress;

			public Model PropModel;

			public bool ModelRequested;

			public bool TasksChanged;

			public bool SawPlayer;

			public bool SawAnimal;

			public int Duration;

			public float LastPhase;

			public int Attempts;

			public int IssuedFrame;

			public bool PairConfirmed;

			public Vector3 PlayerFloor;

			public Vector3 BallStart;

			public Vector3 BallEnd;

			public int Leg;

			public long NextNav;

			public Session()
			{
				Clock = Stopwatch.StartNew();
			}
		}

		private const int PetSceneFlags = 13;

		private const int PetPlayerRagdollFlags = 16;

		private static Session current;

		public static bool IsBusy => current != null;

		public static bool IsUsing(Ped animal)
		{
			if (current != null && (object)animal != null)
			{
				return current.Animal.Handle == animal.Handle;
			}
			return false;
		}

		public static bool SupportedAction(string kind)
		{
			return Operators.CompareString(kind, "Pet", false) == 0;
		}

		public static string PetDictionary(bool cat, string breed, bool female)
		{
			string text = (cat ? "cat" : breed);
			if (Array.IndexOf(new string[7] { "cat", "rottweiler", "husky", "retriever", "shepherd", "poodle", "pug" }, text) < 0)
			{
				return null;
			}
			return "anim@scripted@creatures@" + text + "@amb@mansion@" + (female ? "heeled" : "male") + "@";
		}

		public static bool ApproachReady(Vector3 position, Vector3 target)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(target))
			{
				return false;
			}
			Vector3 vector = position - target;
			return vector.X * vector.X + vector.Y * vector.Y <= 0.1225f && Math.Abs(vector.Z) <= 0.25f;
		}

		public static bool RetryPair(long elapsed, int attempts, bool sawPlayer, bool sawAnimal, float phase)
		{
			if (elapsed >= 1800 && attempts < 2 && attempts > 0 && (!sawPlayer || !sawAnimal) && !float.IsNaN(phase) && phase >= 0f)
			{
				return phase < 0.2f;
			}
			return false;
		}

		public static bool CloseEnough(Vector3 playerFloor, Vector3 petFloor, float maximumDistance)
		{
			if (!PartyPlacement.FinitePoint(playerFloor) || !PartyPlacement.FinitePoint(petFloor) || float.IsNaN(maximumDistance) || maximumDistance <= 0f || maximumDistance > 8f)
			{
				return false;
			}
			Vector3 vector = playerFloor - petFloor;
			return Math.Abs(vector.Z) <= 0.3f && vector.X * vector.X + vector.Y * vector.Y <= maximumDistance * maximumDistance;
		}

		public static Vector3 BallPosition(Vector3 start, Vector3 finish, long elapsed)
		{
			float num = (float)Math.Max(0.0, Math.Min(1.0, (double)elapsed / 1200.0));
			return start + (finish - start) * num;
		}

		private static bool SameEntity(Ped actor, int model, IntPtr address)
		{
			if ((object)actor != null && actor.Exists() && actor.Model.Hash == model)
			{
				return actor.MemoryAddress == address;
			}
			return false;
		}

		public static bool Start(Mansion home, Ped animal, bool cat, string breed, string name, string kind)
		{
			if (IsBusy || !SupportedAction(kind) || home == null || !home.IsOwnedByPlayer() || !MansionLeisure.IsManagedPet(home, animal))
			{
				return false;
			}
			if ((object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || PartyService.IsPlayerActionActive || PartyService.CompanionSessionActive || PartyPlayerActions.IsKissing || MansionFeatures.IsWorkoutActive || Helper.MenuPool.IsAnyMenuOpen() || MansionMedia.IsFullscreenWatching || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				Notification.Show("Finish your current activity before caring for your pet.");
				return false;
			}
			Vector3 floorPoint = default(Vector3);
			Vector3 floorPoint2 = default(Vector3);
			if (!PartyPlacement.TryGetFloor(Helper.PP.Position, Helper.PP.Position, ref floorPoint) || !PartyPlacement.TryGetFloor(Helper.PP.Position, animal.Position, ref floorPoint2) || !CloseEnough(floorPoint, floorPoint2, 5f) || !ClearPath(floorPoint, floorPoint2))
			{
				Notification.Show("Call your pet close to you on the same clear floor first.");
				return false;
			}
			Session session = new Session
			{
				Home = home,
				Player = Helper.PP,
				Animal = animal,
				Cat = cat,
				Name = name,
				Kind = kind,
				PlayerAddress = Helper.PP.MemoryAddress,
				AnimalAddress = animal.MemoryAddress,
				PlayerModel = Helper.PP.Model.Hash,
				AnimalModel = animal.Model.Hash,
				Origin = Helper.PP.Position,
				AnimalOrigin = animal.Position,
				Floor = floorPoint2,
				PlayerFloor = floorPoint
			};
			if (Operators.CompareString(kind, "Pet", false) == 0)
			{
				session.Dict = PetDictionary(cat, breed, !Function.Call<bool>(Hash.IS_PED_MALE, Helper.PP.Handle));
				session.PlayerClip = "petting_player";
				session.AnimalClip = ((Operators.CompareString(breed, "rottweiler", false) == 0 && !cat) ? "petting_chop" : "petting");
			}
			else
			{
				session.Dict = ((Operators.CompareString(kind, "Feed", false) == 0) ? "pickup_object" : "weapons@projectile@");
				session.PlayerClip = ((Operators.CompareString(kind, "Feed", false) == 0) ? "putdown_low" : "throw_l_fb_stand");
				session.PropModel = new Model((Operators.CompareString(kind, "Feed", false) == 0) ? "m25_1_prop_m51_dog_bowl_full" : "prop_tennis_ball");
				if (!session.PropModel.IsValid || !session.PropModel.IsInCdImage)
				{
					Notification.Show("The pet food/toy model is not installed.");
					return false;
				}
			}
			if (string.IsNullOrEmpty(session.Dict))
			{
				return false;
			}
			current = session;
			try
			{
				PartyPlayerActions.StopPlayerDance();
				session.TasksChanged = true;
				Function.Call(Hash.TASK_STAND_STILL, session.Animal.Handle, -1);
				Logger.Log("Pet care queued: " + name + "; cat=" + Conversions.ToString(cat) + "; dict=" + session.Dict + "; player=" + session.PlayerClip + "; animal=" + session.AnimalClip);
				Function.Call(Hash.REQUEST_ANIM_DICT, session.Dict);
				if (Operators.CompareString(kind, "Pet", false) != 0)
				{
					session.PropModel.Request();
					session.ModelRequested = true;
				}
				Notification.Show(((Operators.CompareString(kind, "Pet", false) == 0) ? "Petting " : ((Operators.CompareString(kind, "Feed", false) == 0) ? "Feeding " : "Playing with ")) + name + "...");
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Pet care start: " + ex2.Message);
				Finish(false);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static bool ClearPath(Vector3 a, Vector3 b)
		{
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (PartyPlacement.PhysicalRay(a + new Vector3(0f, 0f, 0.3f), b + new Vector3(0f, 0f, 0.3f), 81, ref hit, ref point, ref normal, Helper.PP) != 2 || hit)
			{
				return false;
			}
			checked
			{
				int val = (int)Math.Ceiling(a.DistanceTo(b) / 0.35f);
				int num = Math.Max(1, val);
				Vector3 floorPoint = default(Vector3);
				for (int i = 0; i <= num; i++)
				{
					Vector3 vector = a + (b - a) * ((float)i / (float)Math.Max(1, val));
					if (!PartyPlacement.TryGetFloor(Helper.PP.Position, vector + new Vector3(0f, 0f, 1f), ref floorPoint) || Math.Abs(floorPoint.Z - vector.Z) > 0.2f)
					{
						return false;
					}
				}
				return true;
			}
		}

		private static void Navigate(Ped actor, Vector3 target, float speed)
		{
			Function.Call(Hash.TASK_FOLLOW_NAV_MESH_TO_COORD, actor.Handle, target.X, target.Y, target.Z, speed, 8000, 0.25f, false, 40000f);
		}

		private static void Prepare(Session s)
		{
			if (Operators.CompareString(s.Kind, "Pet", false) == 0)
			{
				Vector3 floorPoint = default(Vector3);
				Vector3 floorPoint2 = default(Vector3);
				if (!PartyPlacement.TryGetFloor(s.Player.Position, s.Player.Position, ref floorPoint) || !PartyPlacement.TryGetFloor(s.Player.Position, s.Animal.Position, ref floorPoint2) || !CloseEnough(floorPoint, floorPoint2, 5f))
				{
					Notification.Show("Your pet must be beside you on the same clear floor.");
					Finish(false);
					return;
				}
				s.AnimalOrigin = s.Animal.Position;
				s.Floor = floorPoint2;
				s.PlayerFloor = floorPoint;
				Vector3 vector = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, s.Dict, s.AnimalClip, 0f, 0f, 0f, 0f, 0f, 0f, 0.01f, 2);
				s.Rotation = new Vector3(0f, 0f, s.Animal.Heading - vector.Z);
				Vector3 vector2 = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, s.Dict, s.AnimalClip, s.AnimalOrigin.X, s.AnimalOrigin.Y, s.AnimalOrigin.Z, 0f, 0f, s.Rotation.Z, 0.01f, 2);
				s.SceneOrigin = s.AnimalOrigin * 2f - vector2;
				s.Target = Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_POSITION, s.Dict, s.PlayerClip, s.SceneOrigin.X, s.SceneOrigin.Y, s.SceneOrigin.Z, 0f, 0f, s.Rotation.Z, 0.01f, 2);
			}
			else
			{
				Vector3 vector3 = s.Animal.Position - s.Player.Position;
				vector3.Z = 0f;
				if (vector3.Length() < 0.1f)
				{
					vector3 = s.Player.ForwardVector;
				}
				else
				{
					vector3.Normalize();
				}
				s.Target = s.Animal.Position - vector3 * 1.1f;
				s.Target.Z = s.Floor.Z + 1f;
			}
			Vector3 floor = default(Vector3);
			if (!PartyPlacement.FinitePoint(s.Target) || s.Target.DistanceTo(s.Origin) > 5f || !PartyPlacement.TryStandingSpot(s.Target, s.Floor.Z - 0.2f, s.Floor.Z + 0.2f, ref floor) || !ClearPath(s.PlayerFloor, floor))
			{
				Notification.Show("There is not enough clear space beside your pet. Try an open area.");
				Finish(false);
				return;
			}
			s.TasksChanged = true;
			Function.Call(Hash.TASK_STAND_STILL, s.Animal.Handle, -1);
			if (Operators.CompareString(s.Kind, "Pet", false) == 0)
			{
				Function.Call(argument6: Function.Call<Vector3>(Hash.GET_ANIM_INITIAL_OFFSET_ROTATION, s.Dict, s.PlayerClip, s.SceneOrigin.X, s.SceneOrigin.Y, s.SceneOrigin.Z, 0f, 0f, s.Rotation.Z, 0.01f, 2).Z, hash: Hash.TASK_GO_STRAIGHT_TO_COORD, argument0: s.Player.Handle, argument1: s.Target.X, argument2: s.Target.Y, argument3: s.Target.Z, argument4: 1f, argument5: 8000, argument7: 0.05f);
				Logger.Log("Pet care approach: " + s.Name + "; target=" + s.Target.ToString() + "; scene=" + s.SceneOrigin.ToString());
			}
			else
			{
				Navigate(s.Player, s.Target, 1f);
			}
			s.Stage = 1;
			s.StageAt = s.Clock.ElapsedMilliseconds;
		}

		private static void PlayClip(Ped actor, string dict, string clip, int duration, bool looped)
		{
			Function.Call(Hash.TASK_PLAY_ANIM, actor.Handle, dict, clip, 3f, -3f, duration, looped ? 1 : 0, 1f, false, false, false);
		}

		private static bool Playing(Ped actor, string dict, string clip)
		{
			return Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, actor.Handle, dict, clip, 3);
		}

		private static void BeginActivity(Session s)
		{
			checked
			{
				if (Operators.CompareString(s.Kind, "Pet", false) == 0)
				{
					s.Duration = (int)Math.Round(Math.Min(25000f, Math.Max(2000f, Function.Call<float>(Hash.GET_ANIM_DURATION, s.Dict, s.PlayerClip) * 1000f)));
					s.Scene = FwSyncedScene.Create(s.SceneOrigin, s.Rotation);
					if ((object)s.Scene == null)
					{
						Finish(false);
						return;
					}
					s.Scene.IsLooped = false;
					s.Scene.HoldsLastFrame = false;
					s.Scene.Rate = 0f;
					s.SawPlayer = false;
					s.SawAnimal = false;
					s.PairConfirmed = false;
					s.LastPhase = 0f;
					s.Attempts++;
					s.IssuedFrame = Game.FrameCount;
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, s.Animal.Handle, s.Scene.Handle, s.Dict, s.AnimalClip, 2f, -2f, 13, 0, 1000f, 0);
					Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, s.Player.Handle, s.Scene.Handle, s.Dict, s.PlayerClip, 2f, -2f, 13, 16, 1000f, 0);
					s.Scene.Phase = 0f;
					s.Stage = 3;
					s.StageAt = s.Clock.ElapsedMilliseconds;
					Logger.Log("Pet care paired scene queued: " + s.Name + "; attempt=" + Conversions.ToString(s.Attempts));
					return;
				}
				Vector3 vector = s.Animal.Position - s.Player.Position;
				vector.Z = 0f;
				vector.Normalize();
				s.Player.Heading = (float)(Math.Atan2(0f - vector.X, vector.Y) * 180.0 / Math.PI);
				Vector3 vector2 = s.Floor - vector * 0.45f;
				if (!ClearPath(s.Floor, vector2))
				{
					Finish(false);
					return;
				}
				s.BallStart = vector2;
				s.Prop = World.CreateProp(s.PropModel, vector2, false, false);
				if ((object)s.Prop == null || !s.Prop.Exists())
				{
					Finish(false);
					return;
				}
				s.PropAddress = s.Prop.MemoryAddress;
				s.Prop.IsPersistent = true;
				Function.Call(Hash.SET_ENTITY_COLLISION, s.Prop.Handle, false, false);
				Function.Call(Hash.FREEZE_ENTITY_POSITION, s.Prop.Handle, true);
				Vector3 min;
				Vector3 max;
				s.PropModel.GetDimensions(out min, out max);
				s.Prop.Position = vector2 - new Vector3(0f, 0f, min.Z);
				PlayClip(s.Player, s.Dict, s.PlayerClip, 1600, false);
				if (Operators.CompareString(s.Kind, "Feed", false) == 0)
				{
					Function.Call(Hash.TASK_TURN_PED_TO_FACE_ENTITY, s.Animal.Handle, s.Prop.Handle, 800);
					s.Duration = 6000;
				}
				else if (!NextBallLeg(s))
				{
					Finish(false);
					return;
				}
				s.Stage = 2;
				s.StageAt = s.Clock.ElapsedMilliseconds;
			}
		}

		private static bool NextBallLeg(Session s)
		{
			Vector3 vector = s.Player.RightVector * ((s.Leg % 2 == 0) ? 1f : (-1f));
			Vector3 vector2 = s.Floor + vector * 2.4f;
			Vector3 floorPoint = default(Vector3);
			if (!PartyPlacement.TryGetFloor(s.Player.Position, vector2 + new Vector3(0f, 0f, 1f), ref floorPoint) || !CloseEnough(s.Floor, floorPoint, 4f) || !ClearPath(s.BallStart, floorPoint))
			{
				Notification.Show("Ball games need a little more clear floor space.");
				return false;
			}
			s.BallEnd = floorPoint;
			checked
			{
				s.Leg++;
				s.StageAt = s.Clock.ElapsedMilliseconds;
				s.NextNav = 0L;
				Navigate(s.Animal, floorPoint, s.Cat ? 1.8f : 2.5f);
				return true;
			}
		}

		public static void Tick()
		{
			Session session = current;
			if (session == null)
			{
				return;
			}
			checked
			{
				try
				{
					if (!SameEntity(session.Player, session.PlayerModel, session.PlayerAddress) || !SameEntity(session.Animal, session.AnimalModel, session.AnimalAddress) || (object)Helper.PP == null || Helper.PP.Handle != session.Player.Handle || !MansionLeisure.IsManagedPet(session.Home, session.Animal) || !session.Home.Enabled || !session.Home.IplsLoaded || !session.Home.IsOwnedByPlayer() || session.Player.IsDead || session.Animal.IsDead || session.Player.IsInVehicle() || session.Player.IsFalling || session.Player.IsRagdoll || session.Animal.IsFalling || session.Animal.IsRagdoll || session.Player.Position.DistanceTo(session.Origin) > 7f || session.Animal.Position.DistanceTo(session.AnimalOrigin) > 8f || Math.Abs(session.Player.Position.Z - session.Origin.Z) > 1f || Math.Abs(session.Animal.Position.Z - session.AnimalOrigin.Z) > 1f || session.Clock.ElapsedMilliseconds > 45000 || Helper.MenuPool.IsAnyMenuOpen() || Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel) || PartyPlayerActions.MovementRequested())
					{
						Finish(false);
						return;
					}
					Control[] array = new Control[5]
					{
						Control.Attack,
						Control.Aim,
						Control.Jump,
						Control.Enter,
						Control.Context
					};
					for (int i = 0; i < array.Length; i++)
					{
						Game.DisableControlThisFrame(array[i]);
					}
					Helper.ShowPersistentHelp("Pet care: move or press ~INPUT_FRONTEND_CANCEL~ to stop.");
					long num = session.Clock.ElapsedMilliseconds - session.StageAt;
					switch (session.Stage)
					{
					case 0:
						if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, session.Dict) && (Operators.CompareString(session.Kind, "Pet", false) == 0 || session.PropModel.IsLoaded))
						{
							Prepare(session);
						}
						else if (num > 5000)
						{
							Notification.Show("Pet care assets could not load. Please try again.");
							Finish(false);
						}
						break;
					case 1:
						if (session.Animal.Position.DistanceTo(session.AnimalOrigin) > 0.3f)
						{
							Finish(false);
						}
						else if (ApproachReady(session.Player.Position, session.Target))
						{
							BeginActivity(session);
						}
						else if (num > 8000)
						{
							Notification.Show("The spot beside your pet is not reachable.");
							Finish(false);
						}
						break;
					case 3:
						if (Game.FrameCount == session.IssuedFrame)
						{
							break;
						}
						if ((object)session.Scene != null && session.Scene.Exists())
						{
							session.Scene.Rate = 1f;
							session.Stage = 2;
							session.StageAt = session.Clock.ElapsedMilliseconds;
						}
						else if (num > 1800)
						{
							if (RetryPair(num, session.Attempts, false, false, 0f))
							{
								RetryActivity(session);
							}
							else
							{
								PairFailed(session);
							}
						}
						break;
					case 2:
						session.SawPlayer = session.SawPlayer || Playing(session.Player, session.Dict, session.PlayerClip);
						if (Operators.CompareString(session.Kind, "Pet", false) == 0)
						{
							session.SawAnimal = session.SawAnimal || Playing(session.Animal, session.Dict, session.AnimalClip);
							Function.Call(Hash.SET_ENTITY_NO_COLLISION_ENTITY, session.Player.Handle, session.Animal.Handle, true);
							if (!session.PairConfirmed && session.SawPlayer && session.SawAnimal)
							{
								session.PairConfirmed = true;
								Logger.Log("Pet care paired animation confirmed: " + session.Name + "; player+animal clips active.");
							}
							bool flag = (object)session.Scene != null && session.Scene.Exists();
							if (flag)
							{
								session.LastPhase = session.Scene.Phase;
							}
							if (!flag && session.PairConfirmed && session.LastPhase >= 0.9f)
							{
								Finish(true);
							}
							else if (flag && session.LastPhase >= 0.98f && session.PairConfirmed)
							{
								Finish(true);
							}
							else if (RetryPair(num, session.Attempts, session.SawPlayer, session.SawAnimal, session.LastPhase))
							{
								RetryActivity(session);
							}
							else if (num > session.Duration + 2000 || (num > 1800 && (!flag || !Playing(session.Player, session.Dict, session.PlayerClip) || !Playing(session.Animal, session.Dict, session.AnimalClip))))
							{
								PairFailed(session);
							}
							break;
						}
						if ((object)session.Prop == null || !session.Prop.Exists() || session.Prop.MemoryAddress != session.PropAddress)
						{
							Finish(false);
							break;
						}
						if (Operators.CompareString(session.Kind, "Feed", false) == 0)
						{
							Function.Call(Hash.TASK_LOOK_AT_ENTITY, session.Animal.Handle, session.Prop.Handle, 1000, 0, 2);
							if (num >= session.Duration)
							{
								Finish(session.SawPlayer);
							}
							break;
						}
						session.Prop.Position = BallPosition(session.BallStart, session.BallEnd, num) + new Vector3(0f, 0f, 0.07f);
						session.Prop.Rotation = new Vector3((float)unchecked(num % 1200) * 0.6f, 0f, session.Player.Heading);
						if (num > 1200 && session.Animal.Position.DistanceTo(session.BallEnd + new Vector3(0f, 0f, session.AnimalOrigin.Z - session.Floor.Z)) < 0.8f)
						{
							if (session.Leg >= 3)
							{
								Finish(true);
								break;
							}
							session.BallStart = session.BallEnd;
							if (!NextBallLeg(session))
							{
								Finish(false);
							}
						}
						else if (num > 9000)
						{
							Notification.Show("Your pet cannot reach the ball here.");
							Finish(false);
						}
						break;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("Pet care tick: " + ex2.Message);
					Finish(false);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void RetryActivity(Session s)
		{
			Logger.Log("Pet care: paired startup not confirmed; retrying once for " + s.Name);
			Function.Call(Hash.CLEAR_PED_TASKS, s.Player.Handle);
			Function.Call(Hash.CLEAR_PED_TASKS, s.Animal.Handle);
			s.Scene = null;
			BeginActivity(s);
		}

		private static void PairFailed(Session s)
		{
			Notification.Show("The petting animation could not start here. Try a clear spot beside your pet.");
			Finish(false);
		}

		public static void Cancel()
		{
			Finish(false);
		}

		private static bool TaskPending(Ped actor, string task)
		{
			int num = Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, actor.Handle, Game.GenerateHash(task));
			if (num != 0)
			{
				return num == 1;
			}
			return true;
		}

		private static void Finish(bool success)
		{
			Session session = current;
			current = null;
			if (session == null)
			{
				return;
			}
			try
			{
				if (SameEntity(session.Player, session.PlayerModel, session.PlayerAddress) && session.TasksChanged)
				{
					if (Playing(session.Player, session.Dict, session.PlayerClip))
					{
						if (Operators.CompareString(session.Kind, "Pet", false) == 0)
						{
							Function.Call(Hash.CLEAR_PED_TASKS, session.Player.Handle);
						}
						else
						{
							Function.Call(Hash.STOP_ANIM_TASK, session.Player.Handle, session.Dict, session.PlayerClip, -3f);
						}
					}
					else if (session.Stage == 1 && (TaskPending(session.Player, "SCRIPT_TASK_GO_STRAIGHT_TO_COORD") || TaskPending(session.Player, "SCRIPT_TASK_FOLLOW_NAV_MESH_TO_COORD")))
					{
						Function.Call(Hash.CLEAR_PED_TASKS, session.Player.Handle);
					}
					else if (Operators.CompareString(session.Kind, "Pet", false) == 0 && session.Stage >= 2 && TaskPending(session.Player, "SCRIPT_TASK_SYNCHRONIZED_SCENE"))
					{
						Function.Call(Hash.CLEAR_PED_TASKS, session.Player.Handle);
					}
				}
				if (SameEntity(session.Animal, session.AnimalModel, session.AnimalAddress) && session.TasksChanged)
				{
					Function.Call(Hash.CLEAR_PED_TASKS, session.Animal.Handle);
					MansionLeisure.ResumePet(session.Animal);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Pet care task cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				try
				{
					if ((object)session.Prop != null && session.Prop.Exists() && session.Prop.MemoryAddress == session.PropAddress && session.Prop.Model.Hash == session.PropModel.Hash)
					{
						session.Prop.Delete();
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("Pet care prop cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				if (session.ModelRequested)
				{
					session.PropModel.MarkAsNoLongerNeeded();
				}
				if (session.Dict != null)
				{
					Function.Call(Hash.REMOVE_ANIM_DICT, session.Dict);
				}
			}
			Logger.Log("Pet care ended: " + session.Name + "; success=" + Conversions.ToString(success) + "; stage=" + Conversions.ToString(session.Stage) + "; attempts=" + Conversions.ToString(session.Attempts) + "; sawPlayer=" + Conversions.ToString(session.SawPlayer) + "; sawAnimal=" + Conversions.ToString(session.SawAnimal) + "; phase=" + Conversions.ToString(session.LastPhase));
			if (success)
			{
				Notification.Show(session.Name + ((Operators.CompareString(session.Kind, "Feed", false) == 0) ? " has been fed." : ((Operators.CompareString(session.Kind, "Play", false) == 0) ? " enjoyed the ball game." : " enjoyed being petted.")));
			}
		}
	}
}
