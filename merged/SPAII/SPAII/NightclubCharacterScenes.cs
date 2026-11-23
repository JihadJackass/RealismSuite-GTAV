using System;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class NightclubCharacterScenes
	{
		private sealed class SceneProp
		{
			public readonly Model Model;

			public bool AssetRequested;

			public int RequestedAt;

			public bool Done;

			public Prop Entity;

			public IntPtr Address;

			public SceneProp(string name)
			{
				Model = new Model(name);
			}
		}

		internal const string TonyModel = "ig_tonyprince";

		internal const string LazlowModel = "ig_lazlow_2";

		internal const string AttendantModel = "s_m_y_clubbar_01";

		private const int Tip = 1;

		private const int AssetTimeoutMs = 3000;

		internal static readonly Vector3 TonyPose = new Vector3(-1619.757f, -3010.882f, -76.769f);

		internal static readonly Vector3 LazlowPose = new Vector3(-1610.837f, -3014.688f, -76.20504f);

		internal static readonly Vector3 AttendantPose = new Vector3(-1612.076f, -3009.227f, -80.222f);

		private static readonly Vector3 TonyChair = new Vector3(-1619.696f, -3010.91f, -76.2f);

		private static readonly SceneProp Chair = new SceneProp("xm_prop_x17_avengerchair_02");

		private static readonly SceneProp Horse = new SceneProp("ba_prop_battle_hobby_horse");

		private static ExpansionSession current;

		private static bool greetedTony;

		private static int nextShoutout;

		internal static bool IsSeated(string modelName)
		{
			if (Operators.CompareString(modelName, "ig_tonyprince", false) != 0)
			{
				return Operators.CompareString(modelName, "s_m_y_clubbar_01", false) == 0;
			}
			return true;
		}

		internal static bool IsCameo(string modelName)
		{
			if (!IsSeated(modelName))
			{
				return Operators.CompareString(modelName, "ig_lazlow_2", false) == 0;
			}
			return true;
		}

		internal static bool TrySeatedPose(ExpansionSession session, Vector3 position, ref Vector3 seated)
		{
			if (session.Interior == 0 || session.Actor.Position.DistanceToSquared(position) < 0.64f)
			{
				return false;
			}
			Vector3 start = position + new Vector3(0f, 0f, 0.95f);
			int num = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS, start.X, start.Y, start.Z);
			if (num != session.Interior)
			{
				if (num != 0)
				{
					return false;
				}
				int num2 = Function.Call<int>(Hash.GET_INTERIOR_AT_COORDS_WITH_TYPE, start.X, start.Y, start.Z, session.Destination.InteriorType);
				if (num2 != session.Interior && (num2 != 0 || Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, session.Actor.Handle) != session.Interior))
				{
					return false;
				}
			}
			Function.Call(Hash.REQUEST_COLLISION_AT_COORD, position.X, position.Y, position.Z);
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			if (!PartyPlacement.IsUsableFloorHit(PartyPlacement.PhysicalRay(start, position - new Vector3(0f, 0f, 1.15f), 81, ref hit, ref point, ref normal, session.Actor), hit, point, normal.Z))
			{
				return false;
			}
			seated = position;
			return true;
		}

		public static bool Tick(ExpansionSession session, bool allowInteraction)
		{
			try
			{
				if (!OnlineExpansion.IsCurrent(session) || StoryLoadGuard.WorldLoading())
				{
					return false;
				}
				if (current != session)
				{
					Reset(false);
					current = session;
				}
				Ped ped = NightclubExpansion.ResidentByModel("ig_tonyprince");
				Ped ped2 = NightclubExpansion.ResidentByModel("ig_lazlow_2");
				if ((object)ped != null)
				{
					UpdateProp(Chair, ped, TonyChair, new Vector3(0f, 0f, -134.43f));
				}
				if ((object)ped2 != null)
				{
					UpdateProp(Horse, ped2, ped2.Position, Vector3.Zero);
				}
				Greet(session.Actor, ped, ped2);
				if (!allowInteraction || session.Actor.IsInVehicle() || PartyService.IsPlayerActionActive)
				{
					return false;
				}
				Ped ped3 = NightclubExpansion.ResidentByModel("s_m_y_clubbar_01");
				if ((object)ped3 == null || session.Actor.Position.DistanceToSquared(ped3.Position) > 3.0625f || !VisibleTo(session.Actor, ped3) || !OnlineExpansion.TryInteract(session, ped3.Position, 1.75f, "Press ~INPUT_CONTEXT~ to tip the attendant $1."))
				{
					return false;
				}
				int money = Game.Player.Money;
				if (money >= 1)
				{
					Game.Player.Money = checked(money - 1);
					Speak(ped3, "GENERIC_THANKS");
				}
				else
				{
					Notification.Show("You need $1 to tip the attendant.");
				}
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours characters: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static bool VisibleTo(Ped player, Ped actor)
		{
			if (Math.Abs(player.Position.Z - actor.Position.Z) > 2f)
			{
				return false;
			}
			int num = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, actor.Handle);
			if (num != 0 && Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, player.Handle) == num)
			{
				return true;
			}
			return Function.Call<bool>(Hash.HAS_ENTITY_CLEAR_LOS_TO_ENTITY, player.Handle, actor.Handle, 17);
		}

		private static void Greet(Ped player, Ped tony, Ped lazlow)
		{
			if (Game.IsMissionActive || Helper.MenuPool.IsAnyMenuOpen())
			{
				return;
			}
			if ((object)tony != null)
			{
				float num = player.Position.DistanceToSquared(tony.Position);
				if (num > 225f)
				{
					greetedTony = false;
				}
				if (!greetedTony && num <= 25f && VisibleTo(player, tony))
				{
					greetedTony = true;
					Speak(tony, "GREET_OWNER");
				}
			}
			if ((object)lazlow != null && Game.GameTime >= nextShoutout && player.Position.DistanceToSquared(lazlow.Position) <= 100f && VisibleTo(player, lazlow))
			{
				nextShoutout = checked(Game.GameTime + 12000);
				Speak(lazlow, "SHOUTOUT");
			}
		}

		internal static void Speak(Ped actor, string phrase)
		{
			if ((object)actor != null && actor.Exists() && !actor.IsDead && !Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, actor.Handle) && Function.Call<bool>(Hash.DOES_CONTEXT_EXIST_FOR_THIS_PED, actor.Handle, phrase, false))
			{
				Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, actor.Handle, phrase, "SPEECH_PARAMS_FORCE", 0);
			}
		}

		private static void UpdateProp(SceneProp asset, Ped actor, Vector3 position, Vector3 rotation)
		{
			if (asset.Done)
			{
				return;
			}
			try
			{
				if (!asset.Model.IsValid || !asset.Model.IsInCdImage)
				{
					asset.Done = true;
					return;
				}
				if (!asset.Model.IsLoaded)
				{
					if (!asset.AssetRequested)
					{
						asset.Model.Request();
						asset.AssetRequested = true;
						asset.RequestedAt = Game.GameTime;
					}
					else if (checked(Game.GameTime - asset.RequestedAt) >= 3000)
					{
						asset.Done = true;
						ReleaseRequest(asset);
					}
					return;
				}
				asset.Done = true;
				asset.Entity = World.CreateProp(asset.Model, position, rotation, false, false);
				if ((object)asset.Entity != null && asset.Entity.Exists())
				{
					asset.Address = asset.Entity.MemoryAddress;
					asset.Entity.IsPersistent = true;
					asset.Entity.IsCollisionEnabled = false;
					int num = Function.Call<int>(Hash.GET_ROOM_KEY_FROM_ENTITY, actor.Handle);
					if (num != 0)
					{
						Function.Call(Hash.FORCE_ROOM_FOR_ENTITY, asset.Entity.Handle, current.Interior, num);
					}
					if (asset == Horse)
					{
						Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, asset.Entity.Handle, actor.Handle, Function.Call<int>(Hash.GET_PED_BONE_INDEX, actor.Handle, 57005), 0.1f, -0.07f, -0.06f, -19.32f, 80f, 80f, false, false, false, true, 2, true);
					}
					else
					{
						asset.Entity.IsPositionFrozen = true;
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				asset.Done = true;
				Logger.Log("After Hours character prop: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (asset.Done)
				{
					ReleaseRequest(asset);
				}
			}
		}

		private static void ReleaseRequest(SceneProp asset)
		{
			if (asset.AssetRequested)
			{
				asset.AssetRequested = false;
				try
				{
					asset.Model.MarkAsNoLongerNeeded();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log("After Hours character asset: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void Close(ExpansionSession session)
		{
			if (current == session)
			{
				Reset(false);
			}
		}

		public static void Reset(bool storyLoad)
		{
			bool safe = !storyLoad && current != null && !StoryLoadGuard.WorldLoading();
			current = null;
			greetedTony = false;
			nextShoutout = 0;
			ClearProp(Chair, safe);
			ClearProp(Horse, safe);
		}

		private static void ClearProp(SceneProp asset, bool safe)
		{
			try
			{
				if (safe)
				{
					ReleaseRequest(asset);
					if ((object)asset.Entity != null && asset.Entity.Exists() && asset.Address != IntPtr.Zero && asset.Entity.MemoryAddress == asset.Address && asset.Entity.Model.Hash == asset.Model.Hash)
					{
						asset.Entity.Delete();
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("After Hours character cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				asset.AssetRequested = false;
				asset.Entity = null;
				asset.Address = IntPtr.Zero;
				asset.Done = false;
			}
		}
	}
}
