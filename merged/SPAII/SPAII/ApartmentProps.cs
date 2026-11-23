using System;
using System.Collections.Generic;
using System.Globalization;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	internal static class ApartmentProps
	{
		public static Prop ClosestProp = null;

		public static List<Model> ClosestPropList = new List<Model>
		{
			"prop_tv_flat_01", "hei_heist_str_avunitl_03", "apa_mp_h_str_avunitm_03", "apa_mp_h_str_avunits_01", "apa_mp_h_str_avunits_04", "prop_tv_03", "apa_mp_h_str_avunitm_01", "apa_mp_h_str_avunitl_01_b", "apa_mp_h_str_avunitl_04", "ex_prop_ex_tv_flat_01",
			"v_res_mm_audio", "prop_mp3_dock", "prop_boombox_01", "prop_tapeplayer_01", "v_res_fh_speakerdock", "prop_radio_01", "prop_bong_01"
		};

		public static List<Model> TVModels = new List<Model> { "prop_tv_flat_01", "hei_heist_str_avunitl_03", "apa_mp_h_str_avunitm_03", "apa_mp_h_str_avunits_01", "apa_mp_h_str_avunits_04", "prop_tv_03", "apa_mp_h_str_avunitm_01", "apa_mp_h_str_avunitl_01_b", "apa_mp_h_str_avunitl_04", "ex_prop_ex_tv_flat_01" };

		public static bool TVOn = false;

		public static bool TIBOn = false;

		public static bool SubtitleOn = false;

		public static float TVSound = 0f;

		public static int TVChannel = 0;

		public static int rendertargetid;

		public static int ex_rendertargetid;

		public static Camera TVCamera;

		public static List<int> TVChannelList = new List<int> { 0, 1 };

		private static TvRenderSurface tvSurface;

		private static bool tvDecoderStarted;

		private static Ped tvActor;

		private static IntPtr tvActorAddress;

		private static int tvActorModel;

		private static int tvInterior;

		private static int lastTVFrame = -1;

		private static Scaleform _ibScaleform = null;

		public static List<Model> RadioModels = new List<Model> { "v_res_mm_audio", "prop_mp3_dock", "prop_boombox_01", "prop_tapeplayer_01", "v_res_fh_speakerdock", "prop_radio_01" };

		public static bool RadioOn = false;

		public static int RadioChannel = 0;

		public static Prop RadioEmitter = null;

		public static Vector3 RadioPos = Vector3.Zero;

		internal const Control EclipseRadioToggle = Control.ContextSecondary;

		private const int UnboundButtonIcon = 995;

		private static readonly Control[] EclipseRadioAliases = new Control[3]
		{
			Control.Cover,
			Control.Detonate,
			Control.ThrowGrenade
		};

		private static int radioRetry = 0;

		private static ExpansionSession eclipseRadioSession;

		public static List<Model> BongModels = new List<Model> { "prop_bong_01" };

		public static int BongTaskScriptStatus = -1;

		public static int BongStage = 1;

		public static Prop BongProp = null;

		public static Prop LighterProp = null;

		public static PropMemory BongMemory;

		public static PropMemory LighterMemory;

		public static int LastPropTick = 0;

		private static int nextPropScan = 0;

		private static int lastPropFrame = -1;

		private static bool drinksRegistered;

		private static int propScanIndex = -1;

		private static Vector3 propScanOrigin;

		private static Prop propScanBest;

		private static int propScanBestModel;

		private static int closestPropModel;

		private static IntPtr propScanBestAddress;

		private static IntPtr closestPropAddress;

		private static bool drinkFallbackUsedThisScan;

		private static long nextDrinkFallback;

		public const int MaximumDrinkFallbackCandidates = 64;

		public const int DrinkFallbackCadenceMilliseconds = 750;

		public static Scaleform IBScaleform
		{
			get
			{
				if (_ibScaleform == null)
				{
					_ibScaleform = new Scaleform("instructional_buttons");
				}
				return _ibScaleform;
			}
		}

		public static void ReleaseTellyNow()
		{
			TvRenderSurface tvRenderSurface = tvSurface;
			bool flag = tvDecoderStarted;
			int tVChannel = TVChannel;
			tvSurface = null;
			tvDecoderStarted = false;
			tvActor = null;
			tvActorAddress = IntPtr.Zero;
			tvActorModel = 0;
			tvInterior = 0;
			TVOn = false;
			TIBOn = false;
			rendertargetid = 0;
			ex_rendertargetid = 0;
			try
			{
				if (flag && Function.Call<int>(Hash.GET_TV_CHANNEL) == tVChannel)
				{
					Function.Call(Hash.SET_TV_CHANNEL, -1);
				}
			}
			finally
			{
				tvRenderSurface?.Dispose();
			}
		}

		public static void DrawTVControlInstructionalButtons()
		{
			IBScaleform.CallFunction("CLEAR_ALL");
			IBScaleform.CallFunction("CREATE_CONTAINER");
			IBScaleform.CallFunction("SET_DATA_SLOT", 0, Helper.GetControlInstructionalButton(Control.ScriptLeftAxisX), Game.GetLocalizedString("HUD_INPUT75"));
			IBScaleform.CallFunction("SET_DATA_SLOT", 1, Helper.GetControlInstructionalButton(Control.ScriptRUp), Game.GetLocalizedString("HUD_INPUT69"));
			IBScaleform.CallFunction("SET_DATA_SLOT", 2, Helper.GetControlInstructionalButton(Control.Context), Game.GetLocalizedString("HUD_INPUT82"));
			IBScaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			IBScaleform.Render2D();
		}

		public static string TvTargetName(int modelHash)
		{
			if (modelHash != new Model("ex_prop_ex_tv_flat_01").Hash)
			{
				return "tvscreen";
			}
			return "ex_tvscreen";
		}

		public static int TurnOnTV(this Prop tv, string tvscreen, int channel, float vol)
		{
			if ((object)tv == null || !tv.Exists() || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead)
			{
				return 0;
			}
			if (!TVOn && !MansionMedia.OwnsDecoder && Function.Call<int>(Hash.GET_TV_CHANNEL) >= 0)
			{
				Notification.Show("Another television is using the video player. Switch it off before using this TV.");
				return 0;
			}
			ReleaseTellyNow();
			MansionMedia.Cleanup();
			tvSurface = new TvRenderSurface(tv, VideoSurfaceModel(tv.Model.Hash), tvscreen);
			TVChannel = channel;
			TVSound = vol;
			tvActor = Helper.PP;
			tvActorAddress = Helper.PP.MemoryAddress;
			tvActorModel = Helper.PP.Model.Hash;
			tvInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle);
			tvDecoderStarted = false;
			TVOn = true;
			Logger.Log("TV screen queued: furniture=" + tv.Model.Hash + "; surface=" + VideoSurfaceModel(tv.Model.Hash) + "; target=" + tvscreen);
			return -1;
		}

		public static string VideoSurfaceModel(int furnitureHash)
		{
			if (furnitureHash == new Model("prop_tv_03").Hash)
			{
				return "prop_tv_03_overlay";
			}
			if (furnitureHash == new Model("ex_prop_ex_tv_flat_01").Hash)
			{
				return "ex_prop_ex_tv_flat_01";
			}
			if (furnitureHash == new Model("apa_mp_h_str_avunitm_03").Hash)
			{
				return "apa_mp_h_str_avunitm_03";
			}
			if (furnitureHash == new Model("apa_mp_h_str_avunits_01").Hash)
			{
				return "apa_mp_h_str_avunits_01";
			}
			if (furnitureHash == new Model("apa_mp_h_str_avunits_04").Hash)
			{
				return "apa_mp_h_str_avunits_04";
			}
			if (furnitureHash == new Model("apa_mp_h_str_avunitm_01").Hash)
			{
				return "apa_mp_h_str_avunitm_01";
			}
			if (furnitureHash == new Model("apa_mp_h_str_avunitl_01_b").Hash)
			{
				return "apa_mp_h_str_avunitl_01_b";
			}
			if (furnitureHash == new Model("apa_mp_h_str_avunitl_04").Hash)
			{
				return "apa_mp_h_str_avunitl_04";
			}
			return "prop_tv_flat_01_screen";
		}

		public static void TurnOffTV(string tvscreen)
		{
			ReleaseTellyNow();
		}

		internal static bool OwnsTellySource(Prop original)
		{
			if (TVOn && tvSurface != null)
			{
				return tvSurface.OwnsSource(original);
			}
			return false;
		}

		public static void RenderTV(int rtid)
		{
			if (rtid != 0)
			{
				try
				{
					Function.Call(Hash.SET_TEXT_RENDER_ID, rtid);
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
					Function.Call(Hash.DRAW_TV_CHANNEL, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
				}
				finally
				{
					Function.Call(Hash.SET_TEXT_RENDER_ID, Function.Call<int>(Hash.GET_DEFAULT_SCRIPT_RENDERTARGET_RENDER_ID));
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, false);
				}
			}
		}

		public static bool PlaybackMayRemain(bool actorMatches, bool sourceMatches, int expectedInterior, int currentInterior, float distanceSquared)
		{
			if (actorMatches && sourceMatches && expectedInterior != 0 && currentInterior == expectedInterior && !float.IsNaN(distanceSquared) && !float.IsInfinity(distanceSquared) && distanceSquared >= 0f)
			{
				return distanceSquared <= 1600f;
			}
			return false;
		}

		public static void FrameTick()
		{
			RenderTellyFrame();
		}

		public static void RenderTellyFrame()
		{
			if (!TVOn || tvSurface == null || lastTVFrame == Game.FrameCount)
			{
				return;
			}
			lastTVFrame = Game.FrameCount;
			try
			{
				if ((object)tvActor == null || !tvActor.Exists() || tvActor.IsDead || !(tvActor.MemoryAddress == tvActorAddress) || tvActor.Model.Hash != tvActorModel || Game.Player.Character.Handle != tvActor.Handle || !tvSurface.ViewerMayRemain(tvActor, tvInterior))
				{
					ReleaseTellyNow();
					return;
				}
				if (tvDecoderStarted && Function.Call<int>(Hash.GET_TV_CHANNEL) != TVChannel)
				{
					Logger.Log("TV playback released: decoder channel changed outside this session.");
					ReleaseTellyNow();
					return;
				}
				tvSurface.Advance();
				if (tvSurface.Failed)
				{
					Notification.Show("The TV video screen could not finish loading. Please try again.");
					ReleaseTellyNow();
				}
				else
				{
					if (!tvSurface.Ready)
					{
						return;
					}
					if (!tvDecoderStarted)
					{
						if (Function.Call<int>(Hash.GET_TV_CHANNEL) >= 0)
						{
							Logger.Log("TV queue cancelled: another decoder became active before binding completed.");
							ReleaseTellyNow();
							return;
						}
						Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 0, "PL_STD_CNT", false);
						Function.Call(Hash.SET_TV_CHANNEL_PLAYLIST, 1, "PL_STD_WZL", false);
						Function.Call(Hash.SET_TV_CHANNEL, TVChannel);
						tvDecoderStarted = true;
						Function.Call(Hash.SET_TV_VOLUME, TVSound);
						Function.Call(Hash.ENABLE_MOVIE_SUBTITLES, false);
						Function.Call(Hash.ATTACH_TV_AUDIO_TO_ENTITY, tvSurface.Surface.Handle);
						Logger.Log("TV video surface ready: renderId=" + tvSurface.RenderId);
					}
					if (Operators.CompareString(tvSurface.Target, "ex_tvscreen", false) == 0)
					{
						ex_rendertargetid = tvSurface.RenderId;
					}
					else
					{
						rendertargetid = tvSurface.RenderId;
					}
					RenderTV(tvSurface.RenderId);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("TV frame: " + ex2.Message);
				ReleaseTellyNow();
				ProjectData.ClearProjectError();
			}
		}

		public static bool IsPropTelly(this Prop prop)
		{
			if (prop == null)
			{
				return false;
			}
			return TVModels.Contains(prop.Model);
		}

		private static void ChangeTVChannel(int requestedChannel)
		{
			if (tvDecoderStarted)
			{
				Function.Call(Hash.SET_TV_CHANNEL, requestedChannel);
			}
			TVChannel = requestedChannel;
		}

		public static void TellyTick()
		{
			if (PenthouseExpansion.HandlesTelevision(Helper.PP))
			{
				return;
			}
			if (!Helper.MenuPool.IsAnyMenuOpen() && !PartyService.IsPlayerActionActive && IsPropTelly(ClosestProp))
			{
				if (ClosestProp.Position.DistanceToSquared(Game.Player.Character.Position) <= 4f)
				{
					if (!TVOn)
					{
						if (Helper.ShowPersistentHelp(Game.GetLocalizedString("MPTV_GRGE")) && Game.IsControlJustPressed(Control.Context))
						{
							string text = TvTargetName(ClosestProp.Model.Hash);
							int num = TurnOnTV(ClosestProp, text, TVChannel, TVSound);
							rendertargetid = ((Operators.CompareString(text, "tvscreen", false) == 0) ? num : 0);
							ex_rendertargetid = ((Operators.CompareString(text, "ex_tvscreen", false) == 0) ? num : 0);
							TVOn = num != 0;
							if (num == 0)
							{
								Notification.Show("This TV screen could not be queued.");
							}
							Script.Yield();
						}
					}
					else
					{
						bool flag = TIBOn;
						if (!TIBOn)
						{
							flag = Helper.ShowPersistentHelp(tvDecoderStarted ? "Press ~INPUT_CONTEXT~ to switch off. Press ~INPUT_SCRIPT_RUP~ for channel controls." : "TV screen loading... Press ~INPUT_CONTEXT~ to cancel.");
						}
						if (flag && Game.IsControlJustPressed(Control.Context))
						{
							TurnOffTV("tvscreen");
							TurnOffTV("ex_tvscreen");
							TVOn = false;
							TIBOn = false;
							Script.Yield();
						}
						else if (flag && tvDecoderStarted && Game.IsControlJustPressed(Control.ScriptRUp) && !TIBOn)
						{
							TIBOn = true;
							Script.Yield();
						}
					}
					if (TIBOn)
					{
						if (Game.IsControlJustPressed(Control.ScriptLeftAxisX))
						{
							int requestedChannel = TVChannel;
							switch (TVChannel)
							{
							case 0:
								requestedChannel = 1;
								break;
							case 1:
								requestedChannel = 0;
								break;
							}
							ChangeTVChannel(requestedChannel);
							Script.Yield();
						}
						else if (Game.IsControlJustPressed(Control.ScriptRUp) && TIBOn)
						{
							TIBOn = false;
							Script.Yield();
						}
					}
				}
				else if (TIBOn)
				{
					TIBOn = false;
				}
			}
			RenderTellyFrame();
			if (TIBOn)
			{
				DrawTVControlInstructionalButtons();
			}
			if (ClosestProp == null)
			{
				TIBOn = false;
			}
		}

		public static int GetPlayerRadioStation()
		{
			return Function.Call<int>(Hash.GET_PLAYER_RADIO_STATION_INDEX);
		}

		public static void TurnOnRadio(Prop radio, string emitter = "SE_DLC_APT_Yacht_Bar")
		{
			RadioChannel = GetPlayerRadioStation();
			if (RadioChannel == 255 || RadioChannel < 0)
			{
				RadioChannel = 0;
			}
			RadioPos = radio?.Position ?? Helper.PP.Position;
			Function.Call(Hash.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY, true);
			Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, true);
			UpdateRadio((RadioStation)RadioChannel);
			Logger.Log($"> radio on ({RadioChannel})");
		}

		public static void TurnOffRadio(string emitter = "SE_DLC_APT_Yacht_Bar")
		{
			Function.Call(Hash.SET_STATIC_EMITTER_ENABLED, emitter, false);
			if (eclipseRadioSession == null)
			{
				ReleaseMobileRadio(false);
			}
		}

		private static void ReleaseMobileRadio(bool storyLoad)
		{
			bool flag = RadioPos != Vector3.Zero;
			RadioPos = Vector3.Zero;
			if (!storyLoad && flag && !PartyService.OwnsRadioRoute())
			{
				Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, false);
				Function.Call(Hash.SET_MOBILE_RADIO_ENABLED_DURING_GAMEPLAY, false);
			}
		}

		internal static void CloseEclipseRadio(ExpansionSession session, bool storyLoad)
		{
			if (session != null && eclipseRadioSession == session)
			{
				eclipseRadioSession = null;
				RadioOn = false;
				ReleaseMobileRadio(storyLoad);
			}
		}

		public static void UpdateRadio(RadioStation station, string emitter = "SE_DLC_APT_Yacht_Bar")
		{
			if (station == RadioStation.StillSlippingLosSantos)
			{
				station = RadioStation.LosSantosRockRadio;
			}
			if (station == (RadioStation)256)
			{
				station = RadioStation.LosSantosRockRadio;
			}
			if (station != RadioStation.RadioOff)
			{
				RadioChannel = (int)station;
				string text = Function.Call<string>(Hash.GET_RADIO_STATION_NAME, (int)station);
				if (string.IsNullOrEmpty(text))
				{
					RadioChannel = 0;
					text = Function.Call<string>(Hash.GET_RADIO_STATION_NAME, 0);
				}
				Function.Call(Hash.SET_RADIO_TO_STATION_NAME, text);
			}
		}

		public static void RadioDistanceCheck()
		{
			if (!RadioOn || RadioPos == Vector3.Zero)
			{
				return;
			}
			if (Helper.PP.Position.DistanceTo(RadioPos) > 30f || Helper.PP.IsInVehicle())
			{
				if (eclipseRadioSession == null)
				{
					TurnOffRadio();
					RadioOn = false;
				}
				else
				{
					CloseEclipseRadio(eclipseRadioSession, false);
				}
			}
			else if (Game.GameTime > radioRetry && !PartyService.OwnsRadioRoute())
			{
				radioRetry = checked(Game.GameTime + 2000);
				if (!Function.Call<bool>(Hash.IS_MOBILE_PHONE_RADIO_ACTIVE))
				{
					Function.Call(Hash.SET_MOBILE_PHONE_RADIO_STATE, true);
				}
			}
		}

		public static void DrawRadioControlInstructionalButtons(Control toggle = Control.Context)
		{
			IBScaleform.CallFunction("CLEAR_ALL");
			IBScaleform.CallFunction("CREATE_CONTAINER");
			if (RadioOn)
			{
				IBScaleform.CallFunction("SET_DATA_SLOT", 0, Helper.GetControlInstructionalButton(Control.ScriptRUp), string.Format("{0} ({1})", Game.GetLocalizedString("HUD_INPUT80"), Game.GetLocalizedString(Function.Call<string>(Hash.GET_RADIO_STATION_NAME, RadioChannel))));
			}
			IBScaleform.CallFunction("SET_DATA_SLOT", RadioOn ? 1 : 0, Helper.GetControlInstructionalButton(toggle), RadioOn ? Game.GetLocalizedString("HUD_INPUT82") : "Turn on the radio");
			IBScaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			IBScaleform.Render2D();
		}

		public static Prop CreateRadioPropNoOffset(Model model, Vector3 position, bool dynamic)
		{
			if (!model.Request(1000))
			{
				return null;
			}
			Prop prop = Entity.FromHandle(Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, model.Hash, position.X, position.Y, position.Z, true, true, dynamic)) as Prop;
			model.MarkAsNoLongerNeeded();
			if ((object)prop == null)
			{
				return null;
			}
			prop.IsInvincible = true;
			prop.IsPositionFrozen = true;
			prop.IsCollisionEnabled = false;
			prop.IsVisible = false;
			return prop;
		}

		public static bool IsPropRadio(this Prop prop)
		{
			if (prop == null)
			{
				return false;
			}
			return RadioModels.Contains(prop.Model);
		}

		public static void RadioTick()
		{
			if (UseRadio(ClosestProp, Helper.PP))
			{
				Script.Yield();
			}
		}

		internal static bool RadioWithinReach(Prop radio, Ped actor)
		{
			if ((object)radio != null && radio.Exists() && IsPropRadio(radio) && (object)actor != null && actor.Exists())
			{
				return radio.Position.DistanceToSquared(actor.Position) <= 2f;
			}
			return false;
		}

		internal static void TickEclipseRadio(ExpansionSession session, Prop radio)
		{
			if (!OnlineExpansion.IsCurrent(session) || !CanUseRadio(radio, session.Actor, session) || !IBScaleform.IsLoaded)
			{
				return;
			}
			DrawRadioControlInstructionalButtons(Control.ContextSecondary);
			ReserveEclipseRadioToggle();
			if (Game.IsControlJustPressed(Control.ContextSecondary))
			{
				if (RadioOn)
				{
					CloseEclipseRadio(session, false);
				}
				else
				{
					StartRadio(radio, session);
				}
			}
			else if (RadioOn && Game.IsControlJustPressed(Control.ScriptRUp))
			{
				UpdateRadio((RadioStation)checked(RadioChannel + 1));
			}
		}

		private static void ReserveEclipseRadioToggle()
		{
			string controlInstructionalButton = Helper.GetControlInstructionalButton(Control.ContextSecondary);
			if (!HasSingleRadioBinding(controlInstructionalButton))
			{
				return;
			}
			Control[] eclipseRadioAliases = EclipseRadioAliases;
			foreach (Control control in eclipseRadioAliases)
			{
				if (string.Equals(controlInstructionalButton, Helper.GetControlInstructionalButton(control), StringComparison.Ordinal))
				{
					Game.DisableControlThisFrame(control);
				}
			}
		}

		internal static bool HasSingleRadioBinding(string binding)
		{
			if (string.IsNullOrWhiteSpace(binding) || binding.Length <= 2 || binding.Contains("%"))
			{
				return false;
			}
			if (binding.StartsWith("t_", StringComparison.Ordinal))
			{
				return !string.IsNullOrWhiteSpace(binding.Substring(2));
			}
			int result;
			return binding.StartsWith("b_", StringComparison.Ordinal) && int.TryParse(binding.Substring(2), NumberStyles.None, CultureInfo.InvariantCulture, out result) && result != 995;
		}

		private static bool CanUseRadio(Prop radio, Ped actor, ExpansionSession session)
		{
			if (!Helper.MenuPool.IsAnyMenuOpen() && !PartyService.IsPlayerActionActive && !PartyService.OwnsRadioRoute() && (!RadioOn || eclipseRadioSession == session))
			{
				return RadioWithinReach(radio, actor);
			}
			return false;
		}

		private static void StartRadio(Prop radio, ExpansionSession session)
		{
			TurnOnRadio(radio);
			eclipseRadioSession = session;
			RadioOn = true;
		}

		private static bool UseRadio(Prop radio, Ped actor)
		{
			if (!CanUseRadio(radio, actor, null))
			{
				return false;
			}
			if (!RadioOn)
			{
				if (!Helper.ShowPersistentHelp(Game.GetLocalizedString("MPRD_CTXT")) || !Game.IsControlJustPressed(Control.Context))
				{
					return false;
				}
				StartRadio(radio, null);
				return true;
			}
			if (!Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to switch off the radio. Press ~INPUT_SCRIPT_RUP~ to change station."))
			{
				return false;
			}
			DrawRadioControlInstructionalButtons();
			if (Game.IsControlJustPressed(Control.ScriptRUp))
			{
				UpdateRadio((RadioStation)checked(RadioChannel + 1));
				return true;
			}
			if (!Game.IsControlJustPressed(Control.Context))
			{
				return false;
			}
			TurnOffRadio();
			RadioOn = false;
			return true;
		}

		public static bool IsPropBong(this Prop prop)
		{
			if (prop == null)
			{
				return false;
			}
			return BongModels.Contains(prop.Model);
		}

		public static void BongTick()
		{
			if (BongTaskScriptStatus == -1 && (Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive))
			{
				return;
			}
			if (IsPropBong(ClosestProp) && ClosestProp.Position.DistanceToSquared(Helper.PP.Position) <= 2f && BongTaskScriptStatus == -1 && Helper.ShowPersistentHelp(Game.GetLocalizedString("SA_BONG2")) && Game.IsControlJustPressed(Control.Context))
			{
				BongProp = ClosestProp;
				BongMemory = new PropMemory(BongProp);
				LighterProp = Helper.GetClosestProp(BongProp.Position, 10f, "p_cs_lighter_01", false);
				if ((object)LighterProp != null && !LighterProp.Exists())
				{
					LighterProp = null;
				}
				if ((object)LighterProp != null)
				{
					LighterMemory = new PropMemory(LighterProp);
				}
				BongTaskScriptStatus = 0;
			}
			if (BongTaskScriptStatus == -1)
			{
				return;
			}
			checked
			{
				switch (BongTaskScriptStatus)
				{
				case 0:
					if (BongStage > 4)
					{
						BongStage = 1;
						Helper.PP.Task.PlayAnimation("anim@safehouse@bong", $"bong_stage{BongStage}");
					}
					else
					{
						Helper.PP.Task.PlayAnimation("anim@safehouse@bong", $"bong_stage{BongStage}");
						BongStage++;
					}
					Script.Wait(1500);
					BongProp.AttachTo(Helper.PP.Bones[Bone.PHLeftHand], Vector3.Zero, Vector3.Zero);
					if ((object)LighterProp != null)
					{
						LighterProp.AttachTo(Helper.PP.Bones[Bone.PHRightHand], new Vector3(0f, 0f, -0.05f), Vector3.Zero);
					}
					BongTaskScriptStatus = 1;
					break;
				case 1:
					if (BongProp.IsAttachedTo(Helper.PP))
					{
						Script.Wait(10000);
						BongProp.Detach();
						BongProp.Position = BongMemory.Position;
						BongProp.Rotation = BongMemory.Rotation;
						if ((object)LighterProp != null)
						{
							LighterProp.Detach();
							LighterProp.Position = LighterMemory.Position;
							LighterProp.Rotation = LighterMemory.Rotation;
						}
						BongTaskScriptStatus = 2;
					}
					break;
				case 2:
					Helper.StartScreenFx("DrugsDrivingIn", 5000, false, true);
					Helper.StartScreenFx("DrugsDrivingOut", 5000, false, true);
					BongProp = null;
					LighterProp = null;
					BongTaskScriptStatus = -1;
					break;
				}
			}
		}

		public static bool PropSelectionReceiptMatches(int expectedModel, IntPtr expectedAddress, int observedModel, IntPtr observedAddress, bool drink, bool attached)
		{
			if (expectedModel != 0 && expectedAddress != IntPtr.Zero && expectedModel == observedModel && expectedAddress == observedAddress)
			{
				if (drink)
				{
					return !attached;
				}
				return true;
			}
			return false;
		}

		private static bool DiscoveredPropMatches(Prop candidate, int expectedModel, IntPtr expectedAddress)
		{
			if ((object)candidate == null || !candidate.Exists())
			{
				return false;
			}
			Model model = candidate.Model;
			bool flag = ApartmentDrinks.IsDrinkModel(model);
			bool attached = flag && Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, candidate.Handle);
			return PropSelectionReceiptMatches(expectedModel, expectedAddress, model.Hash, candidate.MemoryAddress, flag, attached);
		}

		public static bool DrinkFallbackMayRun(bool rejectedHeldDrink, bool alreadyUsedInScan, long now, long nextAllowed)
		{
			if (rejectedHeldDrink && !alreadyUsedInScan && now >= 0 && nextAllowed >= 0)
			{
				return now >= nextAllowed;
			}
			return false;
		}

		public static bool LocalDrinkCandidateAllowed(bool supported, bool attached, bool receiptMatches, float distance, int ordinal)
		{
			if (receiptMatches && ordinal > 0 && ordinal <= 64)
			{
				return ApartmentDrinks.WorldDrinkSourceAllowed(supported, attached, distance);
			}
			return false;
		}

		private static void KeepCloserScannedProp(Prop candidate, int expectedModel, IntPtr address, Vector3 position)
		{
			if (DiscoveredPropMatches(candidate, expectedModel, address) && ((object)propScanBest == null || candidate.Position.DistanceToSquared(position) < propScanBest.Position.DistanceToSquared(position)))
			{
				propScanBest = candidate;
				propScanBestModel = expectedModel;
				propScanBestAddress = address;
			}
		}

		private static void SeedPlacedDrink(Vector3 position)
		{
			propScanBest = null;
			if (DiscoveredPropMatches(ClosestProp, closestPropModel, closestPropAddress) && ApartmentDrinks.IsDrinkModel(ClosestProp.Model) && ApartmentDrinks.WorldDrinkSourceAllowed(true, false, ClosestProp.Position.DistanceTo(position)))
			{
				propScanBest = ClosestProp;
				propScanBestModel = closestPropModel;
				propScanBestAddress = closestPropAddress;
			}
		}

		private static void FindPlacedDrinkAfterHeld(Prop rejected, int expectedModel, IntPtr address, Vector3 position)
		{
			if ((object)rejected == null || !rejected.Exists())
			{
				return;
			}
			Model model = rejected.Model;
			if (!DrinkFallbackMayRun(PropSelectionReceiptMatches(expectedModel, address, model.Hash, rejected.MemoryAddress, false, false) && ApartmentDrinks.IsDrinkModel(model) && Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, rejected.Handle), drinkFallbackUsedThisScan, Game.GameTime, nextDrinkFallback))
			{
				return;
			}
			drinkFallbackUsedThisScan = true;
			checked
			{
				nextDrinkFallback = unchecked((long)Game.GameTime) + 750L;
				Prop[] nearbyProps = World.GetNearbyProps(position, 1.8f);
				int num = default(int);
				foreach (Prop prop in nearbyProps)
				{
					num++;
					if (num > 64)
					{
						break;
					}
					if ((object)prop == null || !prop.Exists())
					{
						continue;
					}
					Model model2 = prop.Model;
					IntPtr memoryAddress = prop.MemoryAddress;
					bool flag = ApartmentDrinks.IsDrinkModel(model2);
					if (flag)
					{
						bool attached = Function.Call<bool>(Hash.IS_ENTITY_ATTACHED, prop.Handle);
						bool receiptMatches = PropSelectionReceiptMatches(model2.Hash, memoryAddress, prop.Model.Hash, prop.MemoryAddress, flag, attached);
						if (LocalDrinkCandidateAllowed(flag, attached, receiptMatches, prop.Position.DistanceTo(position), num))
						{
							KeepCloserScannedProp(prop, model2.Hash, memoryAddress, position);
						}
					}
				}
			}
		}

		public static int PropScanCount(int cursor, int count)
		{
			if (cursor < 0 || count < 0 || cursor >= count)
			{
				return 0;
			}
			return Math.Min(4, checked(count - cursor));
		}

		public static void PropOnTick()
		{
			if (lastPropFrame == Game.FrameCount)
			{
				return;
			}
			lastPropFrame = Game.FrameCount;
			if (ArcadiusOfficeExpansion.HandlesOfficeProps(Helper.PP))
			{
				RenderTellyFrame();
				return;
			}
			if (!drinksRegistered)
			{
				ClosestPropList.AddRange(ApartmentDrinks.Models);
				drinksRegistered = true;
			}
			LastPropTick = Game.GameTime;
			Vector3 position = HomeActionPose.Position(Helper.PP);
			if ((object)propScanBest != null && !DiscoveredPropMatches(propScanBest, propScanBestModel, propScanBestAddress))
			{
				propScanBest = null;
			}
			if (propScanIndex < 0 && Game.GameTime >= nextPropScan)
			{
				propScanIndex = 0;
				propScanOrigin = position;
				drinkFallbackUsedThisScan = false;
				SeedPlacedDrink(position);
			}
			checked
			{
				if (propScanIndex >= 0)
				{
					if (position.DistanceToSquared(propScanOrigin) > 2.25f)
					{
						propScanIndex = 0;
						propScanOrigin = position;
						drinkFallbackUsedThisScan = false;
						SeedPlacedDrink(position);
					}
					int num = propScanIndex + PropScanCount(propScanIndex, ClosestPropList.Count);
					int num2 = propScanIndex;
					int num3 = num - 1;
					for (int i = num2; i <= num3; i++)
					{
						Model model = ClosestPropList[i];
						int num4 = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 2.1f, model.Hash, false, false, false);
						if (num4 == 0 || (tvSurface != null && tvSurface.OwnsSurface(num4)))
						{
							continue;
						}
						Prop prop = Entity.FromHandle(num4) as Prop;
						if ((object)prop != null && prop.Exists())
						{
							IntPtr memoryAddress = prop.MemoryAddress;
							if (!DiscoveredPropMatches(prop, model.Hash, memoryAddress))
							{
								FindPlacedDrinkAfterHeld(prop, model.Hash, memoryAddress, position);
							}
							else
							{
								KeepCloserScannedProp(prop, model.Hash, memoryAddress, position);
							}
						}
					}
					propScanIndex = num;
					if ((object)propScanBest != null)
					{
						ClosestProp = propScanBest;
						closestPropModel = propScanBestModel;
						closestPropAddress = propScanBestAddress;
					}
					if (propScanIndex >= ClosestPropList.Count)
					{
						ClosestProp = propScanBest;
						closestPropModel = propScanBestModel;
						closestPropAddress = propScanBestAddress;
						propScanIndex = -1;
						nextPropScan = Game.GameTime + 250;
					}
				}
				if ((object)ClosestProp != null && !DiscoveredPropMatches(ClosestProp, closestPropModel, closestPropAddress))
				{
					ClosestProp = null;
				}
				if (EclipseGarageLounge.HandlesLoungeProps(Helper.PP))
				{
					RenderTellyFrame();
					LastPropTick = Game.GameTime;
					return;
				}
				if (ApartmentDrinks.Tick(ClosestProp))
				{
					RenderTellyFrame();
					LastPropTick = Game.GameTime;
					return;
				}
				TellyTick();
				RadioTick();
				BongTick();
				LastPropTick = Game.GameTime;
			}
		}
	}
}
