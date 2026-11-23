using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class MansionOriginalVoice
	{
		private class Request
		{
			public Mansion Home;

			public string EventName;

			public string VoiceName;

			public string SpeechName;

			public int ExpiresAt;

			public int BankDeadline;
		}

		public static string VoiceMode = "Original";

		public const string AudioBank = "DLC_25-2/DLC_25-2_Freemode";

		private static readonly Hash H_PLAY_POSITIONAL = Hash.PLAY_AMBIENT_SPEECH_FROM_POSITION_NATIVE;

		private static readonly Hash H_PLAY_WITH_VOICE = Hash.PLAY_PED_AMBIENT_SPEECH_WITH_VOICE_NATIVE;

		private static readonly Hash H_PED_SPEAKING = Hash.IS_AMBIENT_SPEECH_PLAYING;

		private static readonly Hash H_STOP_PED_AMBIENT = Hash.STOP_CURRENT_PLAYING_AMBIENT_SPEECH;

		private static readonly Hash H_POSITIONAL_PLAYING = Hash.IS_ANY_POSITIONAL_SPEECH_PLAYING;

		private static readonly Hash H_REQUEST_BANK = Hash.REQUEST_SCRIPT_AUDIO_BANK;

		private static readonly Hash H_RELEASE_BANK = Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK;

		private static readonly Hash H_PHONE_ONGOING = Hash.IS_MOBILE_PHONE_CALL_ONGOING;

		private static readonly Hash H_CONVERSATION_ONGOING = Hash.IS_SCRIPTED_CONVERSATION_ONGOING;

		private static Request pending;

		private static Request active;

		private static Ped speaker;

		private static Model? speakerModel;

		private static int speakerModelDeadline;

		private static string speakerVoice;

		private static bool bankReady;

		private static bool bankRequested;

		private static int nextBankTryAt;

		private static int lastStartedAt = -10000;

		private static int phase;

		private static int attempt;

		private static int phaseAt;

		private static int verifyUntil;

		private static int lastSpeakerUseAt;

		private static int observedPlayingAt;

		private static bool mpAudioRequested;

		private static readonly Dictionary<string, int> warningAt = new Dictionary<string, int>(StringComparer.Ordinal);

		private static string lastResultText = "Original GTA assistant audio is selected.";

		public const int MaximumPlaybackAttempts = 3;

		private const string SpeakerModelName = "s_m_m_highsec_01";

		public static string LastResult => lastResultText;

		public static string NormalizeMode(string value)
		{
			switch ((value ?? "").Trim().ToUpperInvariant())
			{
			case "WAV":
			case "CUSTOM":
				return "Wav";
			case "OFF":
			case "NONE":
			case "AUS":
			case "SILENT":
				return "Off";
			default:
				return "Original";
			}
		}

		public static string VoiceNameFor(string assistant)
		{
			string left = (assistant ?? "").Trim().ToLowerInvariant();
			if (Operators.CompareString(left, "og", false) != 0)
			{
				if (Operators.CompareString(left, "haviland", false) == 0)
				{
					return "XM25_AIBUTLER";
				}
				return "XM25_AISECRETARY";
			}
			return "XM25_AIGANG";
		}

		public static string SpeechNameFor(string voiceEvent, bool femalePlayer, long cash = 0L)
		{
			string text = (femalePlayer ? "_FEMALE" : "_MALE");
			string text2 = (femalePlayer ? "_F" : "_M");
			switch ((voiceEvent ?? "").Trim().ToLowerInvariant())
			{
			case "hello":
				return "XM25_WALL_PANEL" + text;
			case "travel":
			case "car":
			case "chauffeur":
			case "heli":
				return "XM25_GENERIC_POSITIVE" + text;
			case "car_luxury":
				return "XM25_REQUEST_LUXURY_VEH" + text2;
			case "champagne":
				return "XM25_CHAMPAGNE_ORDERED" + text;
			case "party":
				return "XM25_THROW_PARTY" + text;
			case "gym":
				return "XM25_GYM" + text;
			case "armory":
				return "XM25_ARMORY" + text;
			case "vault":
				if (cash >= 999999999)
				{
					return "XM25_VAULT_IDLE_BILLIONAIRE" + text2;
				}
				if (cash >= 1000000)
				{
					return "XM25_VAULT_IDLE_MONEY_HIGH" + text2;
				}
				return "XM25_VAULT_IDLE_MONEY_LOW" + text2;
			case "talk":
				return "XM25_GENERIC_HI" + text;
			case "yoga":
				return "XM25_YOGA" + text;
			case "champagne_arrived":
				return "XM25_CHAMPAGNE_ARRIVED" + text;
			case "party_end":
				return "XM25_CLEAN_PARTY" + text;
			case "workshop":
				return "XM25_VEHICLE_MODIFIED" + text;
			default:
				return null;
			}
		}

		public static bool TryPlay(Mansion m, string voiceEvent)
		{
			if (Operators.CompareString(NormalizeMode(VoiceMode), "Original", false) != 0 || m == null)
			{
				return false;
			}
			try
			{
				if ((object)Helper.PP == null || !Helper.PP.Exists())
				{
					return false;
				}
				string text = SpeechNameFor(voiceEvent, Helper.PP.Gender == Gender.Female, Game.Player.Money);
				if (string.IsNullOrEmpty(text))
				{
					return false;
				}
				pending = checked(new Request
				{
					Home = m,
					EventName = voiceEvent,
					VoiceName = VoiceNameFor(m.Assistant),
					SpeechName = text,
					ExpiresAt = Game.GameTime + 15000,
					BankDeadline = Game.GameTime + 5000
				});
				lastResultText = "Original GTA audio queued.";
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Warn("queue", "Original assistant voice queue: " + ex2.Message);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private static void Warn(string key, string message)
		{
			int value;
			if (!warningAt.TryGetValue(key, out value) || checked(Game.GameTime - value) >= 30000)
			{
				warningAt[key] = Game.GameTime;
				Logger.Log(message);
			}
		}

		public static string PlaybackRouteForAttempt(int index)
		{
			switch (index)
			{
			case 0:
				return "PositionalForce";
			case 1:
				return "PedForce";
			case 2:
				return "PedFrontend";
			default:
				return null;
			}
		}

		public static string SpeechParameterForAttempt(int index)
		{
			switch (index)
			{
			case 0:
			case 1:
				return "SPEECH_PARAMS_FORCE";
			case 2:
				return "SPEECH_PARAMS_FORCE_FRONTEND";
			default:
				return null;
			}
		}

		public static bool PlaybackStateSustained(int firstObservedAt, int now)
		{
			if (firstObservedAt >= 0 && now >= firstObservedAt)
			{
				return checked(now - firstObservedAt) >= 120;
			}
			return false;
		}

		private static bool BusyConversation()
		{
			if (!Function.Call<bool>(H_PHONE_ONGOING))
			{
				return Function.Call<bool>(H_CONVERSATION_ONGOING);
			}
			return true;
		}

		private static void RequestMultiplayerAudio()
		{
			Function.Call(Hash.SET_AUDIO_FLAG, "LoadMPData", true);
			if (!mpAudioRequested)
			{
				Logger.Log("Original assistant audio: LoadMPData enabled; mansion positional FORCE playback, no silent preload stage.");
			}
			mpAudioRequested = true;
		}

		private static bool PrepareSpeaker()
		{
			if ((object)speaker != null && speaker.Exists())
			{
				MoveSpeaker();
				return true;
			}
			if (!speakerModel.HasValue)
			{
				Model value = new Model("s_m_m_highsec_01");
				if (!value.IsValid || !value.IsInCdImage || !value.IsPed)
				{
					throw new InvalidOperationException("The local original-audio speaker model is unavailable.");
				}
				speakerModel = value;
				speakerModelDeadline = checked(Game.GameTime + 3000);
			}
			Model value2 = speakerModel.Value;
			Function.Call(Hash.REQUEST_MODEL, value2.Hash);
			if (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, value2.Hash))
			{
				if (Game.GameTime > speakerModelDeadline)
				{
					throw new InvalidOperationException("Timed out loading the local original-audio speaker.");
				}
				return false;
			}
			speaker = World.CreatePed(value2, Helper.PP.Position, Helper.PP.Heading);
			if ((object)speaker == null || !speaker.Exists())
			{
				throw new InvalidOperationException("Could not create the local original-audio speaker.");
			}
			speaker.IsPersistent = true;
			speaker.BlockPermanentEvents = true;
			speaker.IsInvincible = true;
			speaker.CanBeTargetted = false;
			speaker.CanRagdoll = false;
			Function.Call(Hash.SET_ENTITY_VISIBLE, speaker.Handle, false, false);
			Function.Call(Hash.SET_ENTITY_ALPHA, speaker.Handle, 0, false);
			Function.Call(Hash.SET_ENTITY_COLLISION, speaker.Handle, false, false);
			Function.Call(Hash.FREEZE_ENTITY_POSITION, speaker.Handle, true);
			Function.Call(Hash.SET_PED_CAN_PLAY_AMBIENT_ANIMS, speaker.Handle, false);
			value2.MarkAsNoLongerNeeded();
			speakerModel = null;
			speakerVoice = null;
			MoveSpeaker();
			return true;
		}

		private static void MoveSpeaker()
		{
			if ((object)speaker != null && speaker.Exists() && (object)Helper.PP != null && Helper.PP.Exists())
			{
				Vector3 position = Helper.PP.Position;
				Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, speaker.Handle, position.X, position.Y, position.Z, false, false, false);
				Function.Call(Hash.SET_ENTITY_VISIBLE, speaker.Handle, false, false);
				lastSpeakerUseAt = Game.GameTime;
			}
		}

		private static void AssignSpeakerVoice(Request requestToPlay)
		{
			if (!string.Equals(speakerVoice, requestToPlay.VoiceName, StringComparison.Ordinal))
			{
				Function.Call(Hash.SET_AMBIENT_VOICE_NAME, speaker.Handle, requestToPlay.VoiceName);
				speakerVoice = requestToPlay.VoiceName;
			}
		}

		private static void StartAttempt()
		{
			string text = PlaybackRouteForAttempt(attempt);
			string text2 = SpeechParameterForAttempt(attempt);
			switch (attempt)
			{
			case 0:
			{
				Vector3 vector = Helper.PP.Position + new Vector3(0f, 0f, 0.8f);
				Function.Call(H_PLAY_POSITIONAL, active.SpeechName, active.VoiceName, vector.X, vector.Y, vector.Z, text2);
				break;
			}
			case 1:
			case 2:
				AssignSpeakerVoice(active);
				Function.Call(H_STOP_PED_AMBIENT, speaker.Handle);
				Function.Call(H_PLAY_WITH_VOICE, speaker.Handle, active.SpeechName, active.VoiceName, text2, false);
				break;
			default:
				throw new InvalidOperationException("Unknown original assistant audio route.");
			}
			phase = 2;
			checked
			{
				phaseAt = Game.GameTime + 20;
				verifyUntil = Game.GameTime + 2500;
				observedPlayingAt = -1;
				lastStartedAt = Game.GameTime;
				lastResultText = "Original GTA assistant speech requested (" + text + ").";
				Logger.Log($"Original assistant voice requested ({active.EventName}): {active.VoiceName}/{active.SpeechName}; route={text}; params={text2}; MPData=True");
			}
		}

		private static bool OwnSpeechPlaying()
		{
			if (attempt == 0)
			{
				return Function.Call<bool>(H_POSITIONAL_PLAYING);
			}
			return (object)speaker != null && speaker.Exists() && Function.Call<bool>(H_PED_SPEAKING, speaker.Handle);
		}

		private static void TickActive()
		{
			if (active == null)
			{
				return;
			}
			MoveSpeaker();
			checked
			{
				if (Helper.PP.IsDead || (phase != 3 && Game.GameTime > active.ExpiresAt))
				{
					active = null;
				}
				else
				{
					if (Game.GameTime < phaseAt)
					{
						return;
					}
					switch (phase)
					{
					case 0:
						if (!BusyConversation() && !Function.Call<bool>(H_POSITIONAL_PLAYING) && (attempt == 0 || PrepareSpeaker()))
						{
							StartAttempt();
						}
						break;
					case 2:
					{
						bool num = OwnSpeechPlaying();
						if (num && observedPlayingAt < 0)
						{
							observedPlayingAt = Game.GameTime;
						}
						if (!num)
						{
							observedPlayingAt = -1;
						}
						if (num && PlaybackStateSustained(observedPlayingAt, Game.GameTime))
						{
							lastResultText = "GTA reports an active original speech channel (" + PlaybackRouteForAttempt(attempt) + "); audibility requires listening in game.";
							Logger.Log($"Original assistant voice engine channel active: {active.VoiceName}/{active.SpeechName}; route={PlaybackRouteForAttempt(attempt)}; not an audibility measurement");
							phase = 3;
							verifyUntil = Game.GameTime + 30000;
							phaseAt = Game.GameTime + 100;
						}
						else if (Game.GameTime >= verifyUntil)
						{
							if (attempt + 1 < 3)
							{
								Logger.Log($"Original assistant voice retry: route {PlaybackRouteForAttempt(attempt)} had no sustained engine channel; trying {PlaybackRouteForAttempt(attempt + 1)}.");
								attempt++;
								phase = 0;
								break;
							}
							lastResultText = "GTA could not start this original DLC recording after loading MP audio and three playback routes.";
							Warn(active.VoiceName + "/" + active.SpeechName, "Original assistant audio unavailable: " + active.VoiceName + "/" + active.SpeechName + "; MP audio enabled, DLC bank ready, all three original-only routes had no sustained engine channel. No synthesized WAV was used.");
							Notification.Show("Assistant audio: GTA could not load this original recording. See SPA II.log.");
							active = null;
						}
						else
						{
							phaseAt = Game.GameTime + 20;
						}
						break;
					}
					case 3:
						if (OwnSpeechPlaying() && Game.GameTime < verifyUntil)
						{
							phaseAt = Game.GameTime + 100;
						}
						else
						{
							active = null;
						}
						break;
					case 1:
						break;
					}
				}
			}
		}

		public static void Tick()
		{
			checked
			{
				try
				{
					if (Operators.CompareString(NormalizeMode(VoiceMode), "Original", false) != 0)
					{
						pending = null;
						active = null;
						DeleteSpeaker();
					}
					else
					{
						if ((object)Helper.PP == null || !Helper.PP.Exists() || Game.IsLoading)
						{
							return;
						}
						if (Helper.PP.IsDead)
						{
							pending = null;
							active = null;
							DeleteSpeaker();
						}
						else if (active != null)
						{
							TickActive();
						}
						else if (pending == null)
						{
							if ((object)speaker != null && Game.GameTime - lastSpeakerUseAt > 15000)
							{
								DeleteSpeaker();
							}
						}
						else if (Game.GameTime > pending.ExpiresAt)
						{
							lastResultText = "Original assistant request expired while audio was busy.";
							pending = null;
						}
						else
						{
							if (Game.GameTime - lastStartedAt < 1500 || BusyConversation() || Function.Call<bool>(H_POSITIONAL_PLAYING) || ((object)speaker != null && speaker.Exists() && Function.Call<bool>(H_PED_SPEAKING, speaker.Handle)))
							{
								return;
							}
							RequestMultiplayerAudio();
							if (!bankReady)
							{
								if (Game.GameTime < nextBankTryAt)
								{
									return;
								}
								nextBankTryAt = Game.GameTime + 100;
								bankRequested = true;
								bankReady = Function.Call<bool>(H_REQUEST_BANK, "DLC_25-2/DLC_25-2_Freemode", false, -1);
								if (!bankReady)
								{
									if (Game.GameTime >= pending.BankDeadline)
									{
										lastResultText = "The original mansion DLC audio bank was not ready.";
										Warn("bank", "Original assistant voice: DLC bank not ready: DLC_25-2/DLC_25-2_Freemode");
										pending = null;
									}
								}
								else
								{
									nextBankTryAt = Game.GameTime + 150;
								}
							}
							else if (Game.GameTime >= nextBankTryAt)
							{
								active = pending;
								pending = null;
								attempt = 0;
								phase = 0;
								phaseAt = Game.GameTime;
								TickActive();
							}
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					pending = null;
					active = null;
					DeleteSpeaker();
					lastResultText = "Original assistant audio failed; details are in SPA II.log.";
					Warn("tick", "Original assistant voice: " + ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		private static void DeleteSpeaker()
		{
			try
			{
				if ((object)speaker != null && speaker.Exists())
				{
					speaker.Delete();
				}
				if (speakerModel.HasValue)
				{
					speakerModel.Value.MarkAsNoLongerNeeded();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Warn("speaker_cleanup", "Original assistant speaker cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				speaker = null;
				speakerModel = null;
				speakerVoice = null;
			}
		}

		public static void Cleanup()
		{
			pending = null;
			active = null;
			DeleteSpeaker();
			try
			{
				if (bankRequested)
				{
					Function.Call(H_RELEASE_BANK, "DLC_25-2/DLC_25-2_Freemode");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Original assistant audio cleanup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			bankReady = false;
			bankRequested = false;
			nextBankTryAt = 0;
			lastStartedAt = -10000;
			warningAt.Clear();
		}
	}
}
