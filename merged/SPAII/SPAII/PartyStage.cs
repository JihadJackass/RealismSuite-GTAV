using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class PartyStage
	{
		private struct Bounds
		{
			public Model Model;

			public Vector3 Minimum;

			public Vector3 Maximum;

			public float Width => Maximum.X - Minimum.X;

			public float Depth => Maximum.Y - Minimum.Y;

			public float Height => Maximum.Z - Minimum.Z;
		}

		public const int FogMachineCount = 2;

		private const string ParticleDictionary = "scr_ba_club";

		private const string ParticleName = "scr_ba_club_smoke_machine";

		private static readonly List<Prop> stageProps = new List<Prop>();

		private static readonly List<Vector3> reservations = new List<Vector3>();

		private static readonly List<Vector3> lights = new List<Vector3>();

		private static readonly List<Vector3> fogPositions = new List<Vector3>();

		private static readonly List<int> smokeHandles = new List<int>();

		private static readonly List<int> hazeHandles = new List<int>();

		private static bool coreRequested;

		private static bool coreFallback;

		private static int fogFailures;

		private static bool loggedFogReady;

		private static Vector3 stageCenter;

		private static Vector3 stageForward;

		private static Ped stageIgnoredPed;

		private static bool active;

		private static bool lightsEnabled = true;

		private static bool smokeEnabled = true;

		private static bool requestedParticles;

		private static bool particleRequestOwned;

		private static int particleDeadline;

		private static int nextFogAt;

		private static int fogStopsAt;

		private static int lastErrorAt;

		private static int lastDrawFrame = -1;

		private static int nextParticleRequestAt;

		public const float FogScale = 5f;

		internal static void ForgetForStoryLoad()
		{
			active = false;
			stageProps.Clear();
			reservations.Clear();
			lights.Clear();
			fogPositions.Clear();
			smokeHandles.Clear();
			hazeHandles.Clear();
			stageIgnoredPed = null;
			stageCenter = Vector3.Zero;
			stageForward = Vector3.Zero;
			coreRequested = false;
			coreFallback = false;
			fogFailures = 0;
			loggedFogReady = false;
			requestedParticles = false;
			particleRequestOwned = false;
			particleDeadline = 0;
			nextFogAt = 0;
			fogStopsAt = 0;
			lastErrorAt = 0;
			lastDrawFrame = -1;
			nextParticleRequestAt = 0;
		}

		public static Vector3 ForwardForHeading(float heading)
		{
			double num = (double)heading * Math.PI / 180.0;
			return new Vector3(0f - (float)Math.Sin(num), (float)Math.Cos(num), 0f);
		}

		public static Vector3 RightForHeading(float heading)
		{
			double num = (double)heading * Math.PI / 180.0;
			return new Vector3((float)Math.Cos(num), (float)Math.Sin(num), 0f);
		}

		public static Vector3 SpotDirection(Vector3 position, Vector3 center, float seconds, int index)
		{
			Vector3 result = center + RightForHeading(seconds * 24f + (float)index * 140f) * 1.8f - new Vector3(0f, 0f, 0.8f) - position;
			if (result.LengthSquared() < 0.01f)
			{
				return new Vector3(0f, 0f, -1f);
			}
			result.Normalize();
			return result;
		}

		public static bool FogPairReady(int handleCount, int liveHandles)
		{
			if (handleCount == 2)
			{
				return liveHandles == 2;
			}
			return false;
		}

		public static bool NeedsFogFallback(int failures)
		{
			return failures >= 2;
		}

		public static IEnumerable<Vector3> ReservedPositions()
		{
			return reservations.ToArray();
		}

		private static bool TryModel(string[] names, float minimumWidth, float maximumWidth, float maximumDepth, float minimumHeight, float maximumHeight, ref Bounds result)
		{
			foreach (string name in names)
			{
				Model model = new Model(name);
				if (!model.IsValid || !model.IsInCdImage)
				{
					continue;
				}
				model.Request(1200);
				if (!model.IsLoaded)
				{
					model.MarkAsNoLongerNeeded();
					continue;
				}
				OutputArgument outputArgument = new OutputArgument();
				OutputArgument outputArgument2 = new OutputArgument();
				Function.Call(Hash.GET_MODEL_DIMENSIONS, model.Hash, outputArgument, outputArgument2);
				Bounds bounds = default(Bounds);
				bounds.Model = model;
				bounds.Minimum = outputArgument.GetResult<Vector3>();
				bounds.Maximum = outputArgument2.GetResult<Vector3>();
				if (bounds.Width >= minimumWidth && bounds.Width <= maximumWidth && bounds.Depth > 0.05f && bounds.Depth <= maximumDepth && bounds.Height >= minimumHeight && bounds.Height <= maximumHeight)
				{
					result = bounds;
					return true;
				}
				model.MarkAsNoLongerNeeded();
			}
			return false;
		}

		private static bool ClearFootprint(Vector3 center, Vector3 desired, float heading, float width, float depth, ref Vector3 floor)
		{
			if (!PartyPlacement.TryGetFloor(center, desired, ref floor))
			{
				return false;
			}
			if (Mansion.Dist2D(center, floor) > 8.5f)
			{
				return false;
			}
			Vector3 vector = RightForHeading(heading);
			Vector3 vector2 = ForwardForHeading(heading);
			List<Vector3> list = new List<Vector3>();
			float[] array = new float[2] { -1f, 1f };
			Vector3 floorPoint = default(Vector3);
			foreach (float num in array)
			{
				float[] array2 = new float[2] { -1f, 1f };
				foreach (float num2 in array2)
				{
					Vector3 desired2 = floor + vector * (width * 0.48f * num) + vector2 * (depth * 0.48f * num2);
					if (!PartyPlacement.TryGetFloor(center, desired2, ref floorPoint) || Math.Abs(floorPoint.Z - floor.Z) > 0.1f)
					{
						return false;
					}
					list.Add(floorPoint);
				}
			}
			Entity ignore = (((object)stageIgnoredPed != null && stageIgnoredPed.Exists()) ? stageIgnoredPed : null);
			IntersectFlags intersectFlags = IntersectFlags.LosToEntity | IntersectFlags.Vehicles | IntersectFlags.PedCapsules;
			float[] array3 = new float[2] { 0.4f, 0.85f };
			checked
			{
				bool hit = default(bool);
				Vector3 point = default(Vector3);
				Vector3 normal = default(Vector3);
				bool hit2 = default(bool);
				Vector3 point2 = default(Vector3);
				Vector3 normal2 = default(Vector3);
				foreach (float z in array3)
				{
					int num3 = list.Count - 1;
					for (int l = 0; l <= num3; l++)
					{
						if (PartyPlacement.PhysicalRay(list[l] + new Vector3(0f, 0f, z), floor + new Vector3(0f, 0f, z), unchecked((int)(intersectFlags | IntersectFlags.Glass)), ref hit, ref point, ref normal, ignore) != 2 || hit)
						{
							return false;
						}
					}
					int num4 = Math.Max(1, (int)Math.Ceiling(depth / 0.4f));
					int num5 = num4;
					for (int m = 0; m <= num5; m++)
					{
						Vector3 vector3 = floor + vector2 * (depth * ((float)m / (float)num4 - 0.5f)) + new Vector3(0f, 0f, z);
						if (PartyPlacement.PhysicalRay(vector3 - vector * (width * 0.5f), vector3 + vector * (width * 0.5f), unchecked((int)(intersectFlags | IntersectFlags.Glass)), ref hit2, ref point2, ref normal2, ignore) != 2 || hit2)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		private static Prop CreateAtSurface(Bounds modelBounds, Vector3 surface, float heading, bool collision = true)
		{
			Vector3 vector = RightForHeading(heading);
			Vector3 vector2 = ForwardForHeading(heading);
			Vector3 position = surface - vector * ((modelBounds.Minimum.X + modelBounds.Maximum.X) * 0.5f) - vector2 * ((modelBounds.Minimum.Y + modelBounds.Maximum.Y) * 0.5f) - new Vector3(0f, 0f, modelBounds.Minimum.Z);
			Prop prop = World.CreateProp(modelBounds.Model, position, false, false);
			if ((object)prop == null || !prop.Exists())
			{
				return null;
			}
			stageProps.Add(prop);
			Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, prop.Handle, position.X, position.Y, position.Z, false, false, false);
			prop.Heading = heading;
			prop.IsPersistent = true;
			prop.IsInvincible = true;
			Function.Call(Hash.SET_ENTITY_COLLISION, prop.Handle, collision, true);
			Function.Call(Hash.FREEZE_ENTITY_POSITION, prop.Handle, true);
			return prop;
		}

		private static void ReserveRectangle(Vector3 center, float heading, float width, float depth)
		{
			Vector3 vector = RightForHeading(heading);
			Vector3 vector2 = ForwardForHeading(heading);
			checked
			{
				int num = Math.Max(1, (int)Math.Ceiling(width / 0.55f));
				int num2 = Math.Max(1, (int)Math.Ceiling(depth / 0.55f));
				int num3 = num;
				for (int i = 0; i <= num3; i++)
				{
					int num4 = num2;
					for (int j = 0; j <= num4; j++)
					{
						reservations.Add(center + vector * (width * ((float)i / (float)num - 0.5f)) + vector2 * (depth * ((float)j / (float)num2 - 0.5f)));
					}
				}
			}
		}

		private static void CreateFogPair(Vector3 center, Vector3 tableFloor, float heading, float tableWidth)
		{
			Bounds result = default(Bounds);
			if (!TryModel(new string[1] { "ba_prop_club_smoke_machine" }, 0.1f, 1.3f, 1.3f, 0.05f, 1f, ref result))
			{
				Logger.Log("Party stage: nightclub fog-machine model unavailable.");
				return;
			}
			try
			{
				Vector3 vector = RightForHeading(heading);
				Vector3 vector2 = ForwardForHeading(heading);
				Vector3[] array = null;
				float[] array2 = new float[4] { 0.1f, 0.65f, -0.55f, 1.15f };
				Vector3 floor = default(Vector3);
				foreach (float num in array2)
				{
					List<Vector3> list = new List<Vector3>();
					float[] array3 = new float[2] { -1f, 1f };
					foreach (float num2 in array3)
					{
						Vector3 desired = tableFloor + vector * ((tableWidth + result.Width) * 0.5f + 0.2f) * num2 + vector2 * num;
						if (!ClearFootprint(center, desired, heading, result.Width + 0.12f, result.Depth + 0.12f, ref floor))
						{
							break;
						}
						list.Add(floor);
					}
					if (list.Count == 2)
					{
						array = list.ToArray();
						break;
					}
				}
				if (array == null)
				{
					Logger.Log("Party stage: no supported, unobstructed position for a pair of fog machines.");
					return;
				}
				List<Prop> list2 = new List<Prop>();
				Vector3[] array4 = array;
				foreach (Vector3 surface in array4)
				{
					Prop prop = CreateAtSurface(result, surface, heading, false);
					if ((object)prop == null)
					{
						foreach (Prop item in list2)
						{
							stageProps.Remove(item);
							if (item.Exists())
							{
								item.Delete();
							}
						}
						return;
					}
					list2.Add(prop);
				}
				Vector3[] array5 = array;
				foreach (Vector3 vector3 in array5)
				{
					fogPositions.Add(vector3 + vector2 * (result.Depth * 0.4f) + new Vector3(0f, 0f, result.Height * 0.45f));
					ReserveRectangle(vector3, heading, result.Width + 0.15f, result.Depth + 0.15f);
				}
			}
			finally
			{
				result.Model.MarkAsNoLongerNeeded();
			}
		}

		private static void CreateLights(Vector3 center, Vector3 tableFloor, float heading, float width)
		{
			Vector3 vector = RightForHeading(heading);
			Vector3 vector2 = ForwardForHeading(heading);
			Bounds result = default(Bounds);
			bool flag = TryModel(new string[1] { "prop_spot_01" }, 0.05f, 1.4f, 1.4f, 0.1f, 2.6f, ref result);
			try
			{
				float[] array = new float[2] { -1f, 1f };
				Vector3 floor = default(Vector3);
				foreach (float num in array)
				{
					Vector3 desired = tableFloor + vector * (width * 0.5f + 0.5f) * num - vector2 * 0.8f;
					float num2 = (flag ? (Math.Max(result.Width, result.Depth) + 0.08f) : 0.25f);
					if (ClearFootprint(center, desired, heading, num2, num2, ref floor))
					{
						float z = 0.35f;
						if (flag && (object)CreateAtSurface(result, floor, heading, false) != null)
						{
							z = Math.Min(1.8f, result.Height * 0.8f);
							ReserveRectangle(floor, heading, result.Width, result.Depth);
						}
						lights.Add(floor + new Vector3(0f, 0f, z));
					}
				}
			}
			finally
			{
				if (flag)
				{
					result.Model.MarkAsNoLongerNeeded();
				}
			}
		}

		public static bool Start(Vector3 center, Vector3 djFloor, ref float djHeading, Ped dj = null)
		{
			Stop();
			stageIgnoredPed = dj;
			checked
			{
				bool flag = default(bool);
				Bounds result = default(Bounds);
				bool flag2 = default(bool);
				Bounds result2 = default(Bounds);
				bool flag3 = default(bool);
				Bounds result3 = default(Bounds);
				try
				{
					ScriptSettings scriptSettings = ScriptSettings.Load("scripts\\SPA II\\Party.ini");
					lightsEnabled = scriptSettings.GetValue("PARTY", "StageLights", true);
					smokeEnabled = scriptSettings.GetValue("PARTY", "FogMachines", true);
					flag = TryModel(new string[3] { "ba_prop_battle_dj_deck_01a", "prop_dj_deck_02", "prop_dj_deck_01" }, 0.15f, 0.9f, 0.9f, 0.03f, 0.45f, ref result);
					flag2 = TryModel(new string[2] { "ba_prop_battle_dj_mixer_01a", "ba_prop_battle_dj_kit_mixer" }, 0.1f, 0.7f, 0.9f, 0.03f, 0.45f, ref result2);
					float num = result.Width * 2f + result2.Width + 0.12f;
					flag3 = TryModel(new string[4] { "ba_prop_battle_dj_stand", "prop_table_03", "prop_table_03b", "prop_table_04" }, Math.Max(1.15f, num + 0.05f), 3.8f, 1.7f, 0.6f, 1.35f, ref result3);
					if (!flag3 || !flag || !flag2)
					{
						Logger.Log($"Party stage: usable table/deck/mixer loaded={flag3}/{flag}/{flag2}.");
						return false;
					}
					if (num > result3.Width - 0.05f || Math.Max(result.Depth, result2.Depth) > result3.Depth - 0.02f)
					{
						Logger.Log("Party stage: equipment does not fit the loaded tabletop; unsafe placement skipped.");
						return false;
					}
					bool flag4 = false;
					float[] array = new float[8] { 0f, 45f, -45f, 90f, -90f, 135f, -135f, 180f };
					Vector3 floor = default(Vector3);
					foreach (float num2 in array)
					{
						float num3 = djHeading + num2;
						Vector3 desired = djFloor + ForwardForHeading(num3) * (0.5f + result3.Depth * 0.5f);
						if (ClearFootprint(center, desired, num3, result3.Width + 0.08f, result3.Depth + 0.08f, ref floor))
						{
							Vector3 vector = ForwardForHeading(num3);
							Vector3 vector2 = RightForHeading(num3);
							if ((object)CreateAtSurface(result3, floor, num3) == null)
							{
								return false;
							}
							Vector3 vector3 = floor + new Vector3(0f, 0f, result3.Height + 0.012f);
							float num4 = (result2.Width + result.Width) * 0.5f + 0.03f;
							if ((object)CreateAtSurface(result, vector3 - vector2 * num4, num3, false) == null || (object)CreateAtSurface(result, vector3 + vector2 * num4, num3, false) == null || (object)CreateAtSurface(result2, vector3, num3, false) == null)
							{
								Stop();
								return false;
							}
							ReserveRectangle(floor, num3, result3.Width + 0.15f, result3.Depth + 0.15f);
							if (smokeEnabled)
							{
								CreateFogPair(center, floor, num3, result3.Width);
							}
							if (lightsEnabled)
							{
								CreateLights(center, floor, num3, result3.Width);
							}
							if ((!smokeEnabled || fogPositions.Count == 2) && (!lightsEnabled || lights.Count == 2))
							{
								stageCenter = center;
								stageForward = vector;
								djHeading = num3;
								flag4 = true;
								break;
							}
							Stop();
							stageIgnoredPed = dj;
						}
					}
					if (!flag4)
					{
						Logger.Log("Party stage: none of eight complete measured rig footprints fits around the DJ; partial props removed.");
						return false;
					}
					active = true;
					lastDrawFrame = -1;
					nextFogAt = Game.GameTime + 500;
					if (fogPositions.Count == 2)
					{
						Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_ba_club");
						requestedParticles = true;
						particleRequestOwned = true;
						particleDeadline = Game.GameTime + 7000;
					}
					Logger.Log($"Party stage ready: table + two decks + mixer, lights={lights.Count}, fog machines={fogPositions.Count}.");
					return true;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					Logger.Log($"Party stage start: {ex2.Message}");
					Stop();
					bool result4 = false;
					ProjectData.ClearProjectError();
					return result4;
				}
				finally
				{
					if (flag3)
					{
						result3.Model.MarkAsNoLongerNeeded();
					}
					if (flag)
					{
						result.Model.MarkAsNoLongerNeeded();
					}
					if (flag2)
					{
						result2.Model.MarkAsNoLongerNeeded();
					}
				}
			}
		}

		private static void ClearSmoke()
		{
			foreach (int item in smokeHandles.Concat(hazeHandles))
			{
				try
				{
					if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, item))
					{
						Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, item, false);
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
			smokeHandles.Clear();
			hazeHandles.Clear();
		}

		private static int StartEmitter(string dictionary, string effect, Vector3 position, float scale, float alpha)
		{
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, dictionary);
			int num = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effect, position.X, position.Y, position.Z, 0f, -10f, (float)(Math.Atan2(0f - stageForward.X, stageForward.Y) * 180.0 / Math.PI), scale, false, false, false, true);
			if (num == 0)
			{
				return 0;
			}
			Function.Call(Hash.SET_PARTICLE_FX_LOOPED_COLOUR, num, 0.9f, 0.95f, 1f, false);
			Function.Call(Hash.SET_PARTICLE_FX_LOOPED_ALPHA, num, alpha);
			Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, num, "FADE", 0f, false);
			Function.Call(Hash.SET_PARTICLE_FX_LOOPED_EVOLUTION, num, "FILL", 1f, false);
			Function.Call(Hash.SET_PARTICLE_FX_LOOPED_FAR_CLIP_DIST, num, 80f);
			return num;
		}

		public static void Tick()
		{
			if (!active || lastDrawFrame == Game.FrameCount)
			{
				return;
			}
			lastDrawFrame = Game.FrameCount;
			checked
			{
				try
				{
					Ped character = Game.Player.Character;
					if ((object)character == null || !character.Exists() || character.Position.DistanceToSquared(stageCenter) > 6400f)
					{
						ClearSmoke();
						nextFogAt = Game.GameTime + 1500;
						return;
					}
					int gameTime = Game.GameTime;
					float seconds = (float)gameTime / 1000f;
					int num = lights.Count - 1;
					for (int i = 0; i <= num; i++)
					{
						Vector3 position = lights[i] + new Vector3(0f, 0f, 1.6f);
						Vector3 vector = SpotDirection(position, stageCenter, seconds, i);
						unchecked
						{
							int num2 = ((i % 2 == 0) ? 180 : 35);
							int num3 = ((i % 2 == 0) ? 80 : 220);
							Function.Call(Hash.DRAW_SPOT_LIGHT, position.X, position.Y, position.Z, vector.X, vector.Y, vector.Z, num2, 35, num3, 18f, 12f, 0.6f, 28f, 1.2f);
							Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, lights[i].X, lights[i].Y, lights[i].Z + 0.5f, num2, 35, num3, 7f, 3f);
						}
					}
					if (fogPositions.Count != 2 || !requestedParticles)
					{
						return;
					}
					string text = (coreFallback ? "core" : "scr_ba_club");
					if (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, text))
					{
						if (gameTime >= nextParticleRequestAt)
						{
							Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, text);
							if (coreFallback)
							{
								coreRequested = true;
							}
							nextParticleRequestAt = gameTime + 1000;
						}
						if (gameTime > particleDeadline)
						{
							particleDeadline = gameTime + 15000;
							coreFallback = true;
							Logger.Log("Party stage: fog asset not ready; requesting local core steam fallback.");
						}
						return;
					}
					int num4 = 0;
					foreach (int smokeHandle in smokeHandles)
					{
						if (Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, smokeHandle))
						{
							num4++;
						}
					}
					if (smokeHandles.Count > 0 && !FogPairReady(smokeHandles.Count, num4))
					{
						ClearSmoke();
						fogFailures++;
						if (NeedsFogFallback(fogFailures))
						{
							coreFallback = true;
						}
						nextFogAt = gameTime + 1000;
						Logger.Log("Party stage: fog handles not live; retry=" + Conversions.ToString(fogFailures) + "; core fallback=" + Conversions.ToString(coreFallback));
					}
					if (smokeHandles.Count > 0 && gameTime >= fogStopsAt)
					{
						ClearSmoke();
						nextFogAt = gameTime + 6000;
					}
					else if (smokeHandles.Count == 0 && gameTime >= nextFogAt)
					{
						foreach (Vector3 fogPosition in fogPositions)
						{
							int num5 = StartEmitter(text, coreFallback ? "ent_amb_steam" : "scr_ba_club_smoke_machine", fogPosition, coreFallback ? 1.3f : 5f, 0.9f);
							if (num5 != 0)
							{
								smokeHandles.Add(num5);
							}
							if (!coreFallback)
							{
								int num6 = StartEmitter(text, "scr_ba_club_smoke", fogPosition + stageForward * 0.4f, 0.65f, 0.4f);
								if (num6 != 0)
								{
									hazeHandles.Add(num6);
								}
							}
						}
						if (smokeHandles.Count != 2)
						{
							ClearSmoke();
							fogFailures++;
							if (NeedsFogFallback(fogFailures))
							{
								coreFallback = true;
							}
							nextFogAt = gameTime + 1500;
							Logger.Log("Party stage: incomplete fog start; retry=" + Conversions.ToString(fogFailures) + "; core fallback=" + Conversions.ToString(coreFallback));
						}
						fogStopsAt = gameTime + 6500;
					}
					else if (num4 == 2 && !loggedFogReady)
					{
						loggedFogReady = true;
						Logger.Log("Party stage fog: two live emitters; dictionary=" + text + "; haze handles=" + Conversions.ToString(hazeHandles.Count) + "; visible output not measured.");
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					if (Game.GameTime > lastErrorAt + 10000)
					{
						lastErrorAt = Game.GameTime;
						Logger.Log($"Party stage tick: {ex2.Message}");
					}
					ProjectData.ClearProjectError();
				}
			}
		}

		public static void Stop()
		{
			active = false;
			ClearSmoke();
			foreach (Prop stageProp in stageProps)
			{
				try
				{
					if ((object)stageProp != null && stageProp.Exists())
					{
						stageProp.Delete();
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
			stageProps.Clear();
			reservations.Clear();
			lights.Clear();
			fogPositions.Clear();
			stageIgnoredPed = null;
			if (particleRequestOwned)
			{
				try
				{
					Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "scr_ba_club");
				}
				catch (Exception projectError2)
				{
					ProjectData.SetProjectError(projectError2);
					ProjectData.ClearProjectError();
				}
			}
			if (coreRequested)
			{
				try
				{
					Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "core");
				}
				catch (Exception projectError3)
				{
					ProjectData.SetProjectError(projectError3);
					ProjectData.ClearProjectError();
				}
			}
			requestedParticles = false;
			particleRequestOwned = false;
			nextFogAt = 0;
			fogStopsAt = 0;
			nextParticleRequestAt = 0;
			lastDrawFrame = -1;
			coreRequested = false;
			coreFallback = false;
			fogFailures = 0;
			loggedFogReady = false;
		}
	}
}
