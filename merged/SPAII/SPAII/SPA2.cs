using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Metadata;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	public class SPA2 : Script
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<BuildingClass, bool> _0024I24_002D0;

			public static Func<BuildingClass, IEnumerable<ApartmentClass>> _0024I24_002D1;

			public static Func<ApartmentClass, bool> _0024I24_002D2;

			public static Func<ApartmentClass, int> _0024I24_002D3;

			public static Func<int, int> _0024I24_002D4;

			public static Func<int, string> _0024I24_002D5;

			public static Action _0024I28_002D0;

			public static Func<ApartmentClass, bool> _0024I31_002D0;

			public static Func<Mansion, bool> _0024I31_002D1;

			public static Action _0024I31_002D2;

			public static Action _0024I35_002D0;

			public static Action _0024I35_002D1;

			public static Action _0024I35_002D2;

			public static Action _0024I35_002D5;

			public static Action _0024I35_002D6;

			public static Action _0024I35_002D7;

			public static Action _0024I35_002D8;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__24_002D0(BuildingClass b)
			{
				if (b != null)
				{
					return b.Apartments != null;
				}
				return false;
			}

			[SpecialName]
			internal IEnumerable<ApartmentClass> _Lambda_0024__24_002D1(BuildingClass b)
			{
				return b.Apartments;
			}

			[SpecialName]
			internal bool _Lambda_0024__24_002D2(ApartmentClass a)
			{
				if (a != null && a.ID >= 9301)
				{
					return a.ID <= 9304;
				}
				return false;
			}

			[SpecialName]
			internal int _Lambda_0024__24_002D3(ApartmentClass a)
			{
				return a.ID;
			}

			[SpecialName]
			internal int _Lambda_0024__24_002D4(int id)
			{
				return id;
			}

			[SpecialName]
			internal string _Lambda_0024__24_002D5(int id)
			{
				return id.ToString();
			}

			[SpecialName]
			internal void _Lambda_0024__28_002D0()
			{
				ApartmentProps.TurnOffRadio();
			}

			[SpecialName]
			internal bool _Lambda_0024__31_002D0(ApartmentClass apt)
			{
				return apt.Owner() == Helper.GetPlayer();
			}

			[SpecialName]
			internal bool _Lambda_0024__31_002D1(Mansion m)
			{
				if (m.IplsLoaded && m.IsOwnedByPlayer())
				{
					return MansionFeatures.AtMansion(m);
				}
				return false;
			}

			[SpecialName]
			internal void _Lambda_0024__31_002D2()
			{
				PrivateDancerPreview.Close();
			}

			[SpecialName]
			internal void _Lambda_0024__35_002D0()
			{
				ExecutiveGarage.Cleanup();
			}

			[SpecialName]
			internal void _Lambda_0024__35_002D1()
			{
				if ((object)HighEndApartment.Building.SaleSignProp != null && HighEndApartment.Building.SaleSignProp.Exists())
				{
					HighEndApartment.Building.SaleSignProp.Delete();
				}
			}

			[SpecialName]
			internal void _Lambda_0024__35_002D2()
			{
				HighEndApartment.Building.ClearOwnedMapBlips();
			}

			[SpecialName]
			internal void _Lambda_0024__35_002D5()
			{
				if (Mechanic.iFruit != null && Mechanic.MechanicContact != null)
				{
					Mechanic.iFruit.Contacts.Remove(Mechanic.MechanicContact);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__35_002D6()
			{
				if (Mechanic.iFruit != null && Mechanic.InsuranceContact != null)
				{
					Mechanic.iFruit.Contacts.Remove(Mechanic.InsuranceContact);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__35_002D7()
			{
				if ((object)ApartmentProps.RadioEmitter != null && ApartmentProps.RadioEmitter.Exists())
				{
					ApartmentProps.RadioEmitter.Delete();
				}
			}

			[SpecialName]
			internal void _Lambda_0024__35_002D8()
			{
				if (ApartmentProps.TVOn)
				{
					ApartmentProps.ReleaseTellyNow();
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__35_002D0
		{
			public Vehicle _0024VB_0024Local_vehicle;

			public _Closure_0024__35_002D0(_Closure_0024__35_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_vehicle = arg0._0024VB_0024Local_vehicle;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__3()
			{
				if (_0024VB_0024Local_vehicle.Exists())
				{
					Blip blip = V2Compat.CurrentBlip(_0024VB_0024Local_vehicle);
					if ((object)blip != null && blip.Exists())
					{
						V2Compat.Remove(blip);
					}
				}
			}

			[SpecialName]
			internal void _Lambda_0024__4()
			{
				if (_0024VB_0024Local_vehicle.Exists())
				{
					_0024VB_0024Local_vehicle.Delete();
				}
			}
		}

		internal const string ReleaseVersion = "4.1";

		private const string ReleaseName = "SPA II 4.1";

		private bool initialized;

		private bool initFailed;

		private readonly Stopwatch startupClock;

		private readonly Stopwatch startupWarmupClock;

		private readonly StartupLoadingPolicy startupReadiness;

		private bool mansionDefinitionsPrepared;

		private bool startupWarmupFinished;

		private int startupPedHandle;

		private string startupConfigFailure;

		private bool onlineMapDone;

		private bool onlineMapRequested;

		private Semaphore instanceGate;

		private bool storyLoadSuspended;

		private eOwner frameOwner;

		private bool characterSwitchPending;

		private readonly StartupLoadingPolicy switchReadiness;

		private int nextBlipCheck;

		private int keyLogCount;

		public SPA2()
		{
			base.Tick += SPA2_Tick;
			base.Aborted += SPA2_Aborted;
			base.KeyUp += SPA2_KeyUp;
			base.KeyDown += SPA2_KeyDown;
			initialized = false;
			initFailed = false;
			startupClock = Stopwatch.StartNew();
			startupWarmupClock = new Stopwatch();
			startupReadiness = new StartupLoadingPolicy();
			frameOwner = eOwner.Nobody;
			switchReadiness = new StartupLoadingPolicy();
			nextBlipCheck = 0;
			keyLogCount = 0;
			base.Interval = 0;
			try
			{
				Semaphore semaphore = new Semaphore(1, 1, "Local\\SPAII_Main_" + Process.GetCurrentProcess().Id);
				if (!semaphore.WaitOne(0))
				{
					semaphore.Dispose();
					initFailed = true;
					Logger.Log("Duplicate SPA II script instance ignored; the active instance retains ownership of the world.");
				}
				else
				{
					instanceGate = semaphore;
					ConfigHelper.LoadStartupConfig();
					Logger.Log("SPA II 4.1 instance created; waiting for the story world.");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				startupConfigFailure = ex2.Message;
				if (instanceGate == null)
				{
					initFailed = true;
				}
				ProjectData.ClearProjectError();
			}
		}

		private bool IsStartupStreamingReady()
		{
			startupPedHandle = 0;
			try
			{
				if (Game.IsLoading)
				{
					return false;
				}
				if (Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE))
				{
					return false;
				}
				if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
				{
					return false;
				}
				if (Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE) || Function.Call<bool>(Hash.IS_CUTSCENE_PLAYING))
				{
					return false;
				}
				if (Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS))
				{
					return false;
				}
				if (GTA.UI.Screen.IsFadingOut)
				{
					return false;
				}
				Ped character = Game.Player.Character;
				if ((object)character == null || !character.Exists() || character.IsDead)
				{
					return false;
				}
				if (!Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Game.Player.Handle))
				{
					return false;
				}
				if (Function.Call<bool>(Hash.IS_PLAYER_BEING_ARRESTED, Game.Player.Handle, true))
				{
					return false;
				}
				if (character.Position == Vector3.Zero || !PartyPlacement.FinitePoint(character.Position))
				{
					return false;
				}
				startupPedHandle = character.Handle;
				return true;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private bool IsWorldReady()
		{
			try
			{
				return IsStartupStreamingReady() && Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Game.Player.Handle) && GTA.UI.Screen.IsFadedIn;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
		}

		private void PrepareMansionDefinitions()
		{
			if (!mansionDefinitionsPrepared)
			{
				InitStep("mansion definitions");
				Mansions.LoadMansions();
				mansionDefinitionsPrepared = true;
			}
		}

		private void TickStartupWarmup()
		{
			if (startupWarmupFinished || !onlineMapDone)
			{
				return;
			}
			if (!startupWarmupClock.IsRunning)
			{
				startupWarmupClock.Start();
			}
			if (!StartupLoadingPolicy.WarmupAllowed(startupWarmupClock.ElapsedMilliseconds) || !Helper.mpMapActive)
			{
				startupWarmupFinished = true;
				startupWarmupClock.Stop();
				return;
			}
			try
			{
				if (!Mansions.PrepareStartupNearbyTick(Game.Player.Character.Position))
				{
					startupWarmupFinished = true;
					startupWarmupClock.Stop();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				startupWarmupFinished = true;
				startupWarmupClock.Stop();
				Logger.Log("startup nearby warmup: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		private void InitStep(string name)
		{
			Logger.Log($"init: {name}");
		}

		private void Initialize()
		{
			Helper.ResetPersistentHelp();
			Logger.Log("SPA II 4.1 (SHVDN3/Enhanced) initializing...");
			Logger.Log($"target: game={Game.FileVersion}, installed SHVDN API={typeof(Script).Assembly.GetName().Version}");
			Stability.SelfCheck();
			InitStep("decor");
			Decor.Unlock();
			Decor.Register(Helper.vehIdDecor, Decor.eDecorType.Int);
			Decor.Register(Helper.vehUidDecor, Decor.eDecorType.Int);
			Decor.Lock();
			InitStep("config");
			ConfigHelper.GenerateModConfig();
			ConfigHelper.LoadModConfig();
			InitStep("buildings");
			BuildingList.LoadBuildings();
			InitStep("contacts");
			Mechanic.LoadContacts();
			InitStep("party");
			PartyService.LoadPartyConfig();
			PartyService.LoadPartyContact();
			InitStep("wardrobe");
			Wardrobe.LoadWardrobe();
			InitStep("garage menus");
			TwoCarGarage.LoadGarageMenu();
			SixCarGarage.LoadGarageMenu();
			TenCarGarage.LoadGarageMenu();
			InitStep("debug menu");
			DebugHelper.LoadDebugMenu();
			PrepareMansionDefinitions();
			MansionSecurity.Initialize();
			InitStep("mansion menus");
			Mansions.LoadMansionMenus();
			InitStep("mansion blips");
			Mansions.LoadMansionBlips();
			InitStep("mansion features");
			MansionFeatures.LoadMansionFeatures();
			InitStep("shop global");
			Helper.SetShopControllerGlobal();
			CleanupStep("online expansion registration", OnlineExpansion.Initialize);
			Logger.Log(string.Format("{0} initialized. Online map request={1}, ready={2}, configured delay={3}s from script start (OnlineMap={4}).", "SPA II 4.1", onlineMapRequested, onlineMapDone, ConfigHelper.OnlineMapDelay, ConfigHelper.OnlineMap));
			int[] array = Helper.buildings.Where((_Closure_0024__._0024I24_002D0 != null) ? _Closure_0024__._0024I24_002D0 : (_Closure_0024__._0024I24_002D0 = _Closure_0024__._0024I._Lambda_0024__24_002D0)).SelectMany((_Closure_0024__._0024I24_002D1 != null) ? _Closure_0024__._0024I24_002D1 : (_Closure_0024__._0024I24_002D1 = _Closure_0024__._0024I._Lambda_0024__24_002D1)).Where((_Closure_0024__._0024I24_002D2 != null) ? _Closure_0024__._0024I24_002D2 : (_Closure_0024__._0024I24_002D2 = _Closure_0024__._0024I._Lambda_0024__24_002D2))
				.Select((_Closure_0024__._0024I24_002D3 != null) ? _Closure_0024__._0024I24_002D3 : (_Closure_0024__._0024I24_002D3 = _Closure_0024__._0024I._Lambda_0024__24_002D3))
				.OrderBy((_Closure_0024__._0024I24_002D4 != null) ? _Closure_0024__._0024I24_002D4 : (_Closure_0024__._0024I24_002D4 = _Closure_0024__._0024I._Lambda_0024__24_002D4))
				.ToArray();
			Logger.Log("South LS actual world IDs=" + string.Join(",", array.Select((_Closure_0024__._0024I24_002D5 != null) ? _Closure_0024__._0024I24_002D5 : (_Closure_0024__._0024I24_002D5 = _Closure_0024__._0024I._Lambda_0024__24_002D5)).ToArray()) + "; removed world ID9304 present=" + array.Contains(9304));
			Notification.Show("SPA II 4.1 initialized. South LS homes: " + array.Length + ".");
		}

		private void ForgetStreamingForStoryLoad()
		{
			onlineMapRequested = false;
			onlineMapDone = false;
			startupWarmupFinished = false;
			startupWarmupClock.Reset();
			Helper.ForgetOnlineMapForStoryLoad();
			Mansions.ForgetForStoryLoad();
		}

		private bool HandleStoryLoadState(StoryLoadState state)
		{
			if (!StoryLoadGuard.MustSuspend(state))
			{
				return false;
			}
			startupReadiness.Reset();
			if (state != StoryLoadState.Loading)
			{
				return true;
			}
			if (!storyLoadSuspended)
			{
				frameOwner = eOwner.Nobody;
				characterSwitchPending = false;
				switchReadiness.Reset();
				ClassicInteriorSwitch.ForgetForStoryLoad();
				CharacterReturnResidency.Clear();
				Logger.Log("Story load detected: property work suspended.");
				PartyService.ForgetForStoryLoad();
				ForgetStreamingForStoryLoad();
				Helper.MenuPool.CloseAllMenus();
				Helper.HideHud = false;
				Wardrobe.ForgetForStoryLoad();
				MansionVehicleShop.ForgetForStoryLoad();
				MansionHotTub.ForgetForStoryLoad();
				Stability.ForgetPresentationForStoryLoad();
				DebugCamera.ForgetForStoryLoad();
				TwoCarGarage.ForgetForStoryLoad();
				SixCarGarage.ForgetForStoryLoad();
				TenCarGarage.ForgetForStoryLoad();
				ExecutiveGarage.ForgetForStoryLoad();
				OnlineExpansion.ForgetForStoryLoad();
				SpaBlips.ForgetForStoryLoad();
			}
			storyLoadSuspended = true;
			ApartmentHomeServices.ForgetForStoryLoad();
			ApartmentTravel.ForgetForStoryLoad();
			ApartmentResidence.ForgetForStoryLoad();
			DestinationStreaming.ForgetForStoryLoad();
			Stability.ForgetPinsForStoryLoad();
			return true;
		}

		private bool PrepareCharacterFrame(ref Ped actor, ref eOwner owner)
		{
			actor = Game.Player.Character;
			if ((object)actor == null || !actor.Exists())
			{
				switchReadiness.Reset();
				return false;
			}
			owner = Helper.GetPlayer();
			bool flag = frameOwner != eOwner.Nobody && owner != frameOwner;
			if (characterSwitchPending || flag)
			{
				characterSwitchPending = true;
				bool worldUsable = (ApartmentTravel.IsBusy ? IsStartupStreamingReady() : IsWorldReady()) && startupPedHandle == actor.Handle;
				if (!switchReadiness.Observe(worldUsable, actor.Handle, Game.FrameCount))
				{
					return false;
				}
				characterSwitchPending = false;
				switchReadiness.Reset();
			}
			return true;
		}

		private void ClosePreviousCharacterSession()
		{
			Helper.MenuPool.CloseAllMenus();
			MansionFeatures.AssistantKeyRequested = false;
			CleanupStep("character switch apartment transfer", ApartmentTravel.Cancel);
			CleanupStep("character switch apartment actions", ApartmentHomeServices.Cleanup);
			CleanupStep("character switch mansion barber", MansionBarber.Cleanup);
			CleanupStep("character switch hot tub", MansionHotTub.Cleanup);
			CleanupStep("character switch party", PartyService.EndParty);
			CleanupStep("character switch properties", OnlineExpansion.OnStoryCharacterChanged);
			CleanupStep("character switch Executive Garage", ExecutiveGarage.OnStoryCharacterChanged);
			CleanupStep("character switch mansion garage", MansionNativeGarage.OnStoryCharacterChanged);
			if (ApartmentProps.TVOn)
			{
				CleanupStep("character switch TV", ApartmentProps.ReleaseTellyNow);
			}
			if (ApartmentProps.RadioOn)
			{
				ApartmentProps.RadioOn = false;
				CleanupStep("character switch radio", (_Closure_0024__._0024I28_002D0 != null) ? _Closure_0024__._0024I28_002D0 : (_Closure_0024__._0024I28_002D0 = _Closure_0024__._0024I._Lambda_0024__28_002D0));
			}
			CleanupStep("character switch help", Helper.ResetPersistentHelp);
			Helper.HideHud = false;
		}

		private bool CharacterSwitchIsBusy(Ped actor)
		{
			if (ApartmentTravel.IsBusy || DestinationStreaming.IsBusy || ExecutiveGarage.IsTransitioning || OnlineExpansion.IsTransitioning || Helper.MenuPool.IsAnyMenuOpen() || PartyService.IsPlayerActionActive || MansionBarber.IsBusy || MansionHotTub.IsUsingTub || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing)
			{
				return true;
			}
			return actor.IsInVehicle() && (ClassicInteriorSwitch.IsDrivingInGarage(actor) || ExecutiveGarage.HoldsOnlineMap() || OnlineExpansion.HoldsOnlineMap() || MansionNativeGarage.ActiveMansion != null);
		}

		private void GuardCharacterControls(Ped actor)
		{
			if (CharacterSwitchIsBusy(actor))
			{
				Game.DisableControlThisFrame(GTA.Control.CharacterWheel);
				Game.DisableControlThisFrame(GTA.Control.SelectCharacterFranklin);
				Game.DisableControlThisFrame(GTA.Control.SelectCharacterMichael);
				Game.DisableControlThisFrame(GTA.Control.SelectCharacterTrevor);
				Game.DisableControlThisFrame(GTA.Control.SelectCharacterMultiplayer);
			}
		}

		private void SPA2_Tick(object sender, EventArgs e)
		{
			if (initFailed)
			{
				return;
			}
			checked
			{
				if (startupConfigFailure != null)
				{
					initFailed = true;
					Logger.Log("Startup settings could not be read; no map request was made: " + startupConfigFailure);
				}
				else
				{
					if (HandleStoryLoadState(StoryLoadGuard.ReadState()))
					{
						return;
					}
					if (storyLoadSuspended)
					{
						if (!startupReadiness.Observe(IsStartupStreamingReady(), startupPedHandle, Game.FrameCount))
						{
							return;
						}
						storyLoadSuspended = false;
						Logger.Log("Story world available again; property work resumed.");
					}
					Ped actor = null;
					bool flag = false;
					eOwner owner = default(eOwner);
					if (initialized)
					{
						if (Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS))
						{
							characterSwitchPending = true;
							switchReadiness.Reset();
							return;
						}
						flag = DestinationStreaming.TickSafetyHold();
						if (!PrepareCharacterFrame(ref actor, ref owner))
						{
							return;
						}
						GuardCharacterControls(actor);
					}
					bool flag2 = true;
					bool flag3 = false;
					if (!initialized || !onlineMapRequested || !startupWarmupFinished)
					{
						flag2 = IsStartupStreamingReady();
						flag3 = startupReadiness.Observe(flag2, startupPedHandle, Game.FrameCount);
					}
					if (flag3 && !initialized)
					{
						PropertyProfiles.ActiveCharacter = Helper.GetPlayer();
					}
					if (flag3 && !mansionDefinitionsPrepared)
					{
						PrepareMansionDefinitions();
					}
					if (!onlineMapRequested && flag3 && StartupLoadingPolicy.DelayElapsed(startupClock.ElapsedMilliseconds, ConfigHelper.OnlineMapDelay))
					{
						onlineMapRequested = true;
						InitStep("early online map request");
						if (Helper.EnableOnlineMap())
						{
							startupWarmupFinished = true;
							Logger.Log("init: online map request rejected; loading is not marked complete");
						}
						else
						{
							onlineMapDone = true;
						}
						return;
					}
					if (flag3)
					{
						TickStartupWarmup();
					}
					if (!initialized)
					{
						if (StartupLoadingPolicy.InitializationAllowed(flag3, IsWorldReady()))
						{
							try
							{
								Initialize();
								initialized = true;
							}
							catch (Exception ex)
							{
								ProjectData.SetProjectError(ex);
								Exception ex2 = ex;
								initFailed = true;
								Logger.Log($"SPA II initialization failed: {ex2.Message} {ex2.StackTrace}");
								Notification.Show("~r~SPA II~w~: initialization failed, see SPAII.log");
								ProjectData.ClearProjectError();
							}
						}
					}
					else
					{
						if (flag)
						{
							return;
						}
						Helper.RegisterDecor(Helper.vehIdDecor, Decor.eDecorType.Int);
						Helper.RegisterDecor(Helper.vehUidDecor, Decor.eDecorType.Int);
						Helper.PP = actor;
						Helper.LV = Helper.PP.LastVehicle;
						Helper.PM = Game.Player.Money;
						Helper.Player = Game.Player;
						if (frameOwner != eOwner.Nobody && owner != frameOwner)
						{
							Logger.Log($"Story character changed: {frameOwner} to {owner}.");
							CharacterReturnResidency.ChangeOwner(frameOwner, owner);
							ClosePreviousCharacterSession();
						}
						if (PropertyProfiles.ActiveCharacter != owner)
						{
							PropertyProfiles.ActiveCharacter = owner;
							CleanupStep("mansion character profile", Mansions.ApplyMansionCharacterProfiles);
							foreach (BuildingClass building in Helper.buildings)
							{
								HighEndApartment.Building = building;
								try
								{
									foreach (ApartmentClass apartment in HighEndApartment.Building.Apartments)
									{
										apartment.ApplyCharacterStyle();
									}
									HighEndApartment.Building.RefreshBuyMenu();
									HighEndApartment.Building.RefreshAptMenu();
									HighEndApartment.Building.RefreshGrgMenu();
									HighEndApartment.Building.RefreshSaleSign();
									HighEndApartment.Building.RefreshBlips();
								}
								catch (Exception ex3)
								{
									ProjectData.SetProjectError(ex3);
									Exception ex4 = ex3;
									Logger.Log("Refresh character property markers: " + ex4.Message);
									ProjectData.ClearProjectError();
								}
							}
						}
						frameOwner = owner;
						try
						{
							if (ClassicInteriorSwitch.FrameTick(Helper.PP))
							{
								return;
							}
						}
						catch (Exception ex5)
						{
							ProjectData.SetProjectError(ex5);
							Exception ex6 = ex5;
							Logger.Log("Classic property session: " + ex6.Message);
							ProjectData.ClearProjectError();
							return;
						}
						try
						{
							if (ApartmentTravel.FrameTick())
							{
								return;
							}
							ApartmentResidence.FrameTick();
						}
						catch (Exception ex7)
						{
							ProjectData.SetProjectError(ex7);
							Exception ex8 = ex7;
							Logger.Log("Apartment travel/session: " + ex8.Message);
							CleanupStep("apartment transfer recovery", ApartmentTravel.Cancel);
							ProjectData.ClearProjectError();
						}
						try
						{
							ExecutiveGarage.FrameTick();
						}
						catch (Exception ex9)
						{
							ProjectData.SetProjectError(ex9);
							Exception ex10 = ex9;
							Logger.Log("Executive Garage tick: " + ex10.Message);
							ProjectData.ClearProjectError();
						}
						try
						{
							MansionFeatures.AssistantInputTick();
						}
						catch (Exception ex11)
						{
							ProjectData.SetProjectError(ex11);
							Exception ex12 = ex11;
							Logger.Log($"Assistant input: {ex12.Message}");
							ProjectData.ClearProjectError();
						}
						try
						{
							Vector3 position = Helper.PP.Position;
							Helper.PI = Helper.GetInterior(position);
							CharacterReturnResidency.Observe(frameOwner, Helper.PP, Helper.PI);
							bool flag4 = Helper.PP.IsInVehicle();
							Vector3 vehPos = position;
							if (flag4)
							{
								Vehicle currentVehicle = Helper.PP.CurrentVehicle;
								if ((object)currentVehicle != null)
								{
									vehPos = currentVehicle.Position;
								}
							}
							bool flag5 = Helper.MenuPool.IsAnyMenuOpen();
							bool flag6 = Helper.IsInInterior();
							bool flag7 = false;
							bool hidden = false;
							if (ConfigHelper.HideBlipsOnMission && Game.GameTime >= nextBlipCheck)
							{
								nextBlipCheck = Game.GameTime + 1000;
								flag7 = true;
								hidden = Game.IsMissionActive;
							}
							if (!Helper.forSaleSignSpawned)
							{
								BuildingList.SpawnForSaleSignsAndLockDoors();
							}
							if (Helper.buildingsLoaded)
							{
								SouthLSHousingExterior.FrameTick();
								int num = Helper.buildings.Count - 1;
								for (int i = 0; i <= num; i++)
								{
									BuildingClass buildingClass = Helper.buildings[i];
									Vector3 vector = SouthLSHousingExterior.DisplayEntrancePosition(buildingClass);
									float num2 = position.DistanceToSquared(vector);
									float num3 = buildingClass.GarageDistanceFrom(position, vehPos, flag4);
									float num4 = buildingClass.SaleSignDistanceFrom(position, num2);
									if (!flag5 && !Game.IsPaused && ((!flag4 && num2 <= 2f && vector != Vector3.Zero) || num3 <= 5f))
									{
										string text = buildingClass.LockedEntranceLabel(owner, Game.Language == Language.German);
										if (text != null)
										{
											Helper.ShowPersistentHelp(text);
											continue;
										}
									}
									if (num4 <= 3f && !flag5 && !flag4)
									{
										bool flag8 = false;
										switch (buildingClass.BuildingType)
										{
										case eBuildingType.Apartment:
										case eBuildingType.ClubHouse:
											flag8 = Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_PROP_PUR0"));
											break;
										case eBuildingType.Garage:
										case eBuildingType.Hangar:
											flag8 = Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_PROP_PUR1"));
											break;
										case eBuildingType.Office:
										case eBuildingType.Warehouse:
										case eBuildingType.NightClub:
										case eBuildingType.Bunker:
											flag8 = Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_PROP_OFF_BUY"));
											break;
										}
										if (flag8 && Game.IsControlJustReleased(GTA.Control.Context))
										{
											Stability.Trace($"buy menu {buildingClass.Name}");
											if (buildingClass.GarageType == eGarageType.SixtyCarGarage)
											{
												buildingClass.BuyMenu.Visible = true;
											}
											else
											{
												Helper.FadeScreen(1);
												buildingClass.BuyMenu.Visible = true;
												World.RenderingCamera = Stability.CreateCam(buildingClass.CameraPos.Position, buildingClass.CameraPos.Rotation, buildingClass.CameraPos.FOV);
												Helper.HideHud = true;
												Helper.FadeScreen(0);
											}
											flag5 = true;
										}
									}
									if (num2 <= 2f && !flag5 && !flag4)
									{
										bool flag9 = false;
										switch (buildingClass.BuildingType)
										{
										case eBuildingType.Apartment:
											flag9 = Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_PROP_BUZZ1"));
											break;
										case eBuildingType.ClubHouse:
											flag9 = Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_BUZZ_CLU"));
											break;
										case eBuildingType.Office:
											flag9 = Helper.ShowPersistentHelp(Game.GetLocalizedString("MP_BUZZ_OFF"));
											break;
										}
										if (flag9 && Game.IsControlJustReleased(GTA.Control.Context))
										{
											eBuildingType buildingType = buildingClass.BuildingType;
											if (buildingType == eBuildingType.Apartment || unchecked((uint)(buildingType - 2)) <= 1u)
											{
												Stability.Trace($"apartment menu {buildingClass.Name}");
												Helper.FadeScreen(1);
												buildingClass.AptMenu.Visible = true;
												World.RenderingCamera = Stability.CreateCam(buildingClass.CameraPos.Position, buildingClass.CameraPos.Rotation, buildingClass.CameraPos.FOV);
												Helper.HideHud = true;
												Helper.FadeScreen(0);
												flag5 = true;
											}
										}
									}
									if (num3 <= 5f && !flag5 && (buildingClass.GarageType != eGarageType.SixtyCarGarage || buildingClass.Apartments.Any((_Closure_0024__._0024I31_002D0 != null) ? _Closure_0024__._0024I31_002D0 : (_Closure_0024__._0024I31_002D0 = _Closure_0024__._0024I._Lambda_0024__31_002D0))))
									{
										bool flag10 = buildingClass.BuyAtGarageEntrance && buildingClass.IsVacant();
										if ((!flag10 || buildingClass.AtGaragePurchaseEntrance(Helper.PP)) && Helper.ShowPersistentHelp(Game.GetLocalizedString(flag10 ? "MP_PROP_PUR1" : "MP_PROP_BUZZ1B")) && Game.IsControlJustReleased(GTA.Control.Context))
										{
											Stability.Trace(string.Format("{0} menu {1}", flag10 ? "buy" : "garage", buildingClass.Name));
											UIMenu uIMenu = (flag10 ? buildingClass.BuyMenu : buildingClass.GrgMenu);
											if (buildingClass.GarageType == eGarageType.SixtyCarGarage)
											{
												uIMenu.Visible = true;
											}
											else
											{
												Helper.FadeScreen(1);
												uIMenu.Visible = true;
												World.RenderingCamera = Stability.CreateCam(buildingClass.CameraPos.Position, buildingClass.CameraPos.Rotation, buildingClass.CameraPos.FOV);
												Helper.HideHud = true;
												Helper.FadeScreen(0);
											}
											flag5 = true;
										}
									}
									if (num2 <= 300f && !(buildingClass.BuildingInPos == Helper.QuaternionZero()) && buildingClass.BuildingType != eBuildingType.Garage)
									{
										if (SouthLSHousingExterior.IsCustom(buildingClass))
										{
											Helper.DrawMarker(SouthLSHousingExterior.EntranceMarkerPosition(buildingClass), default(Color), new Vector3(0.8f, 0.8f, 0.08f));
										}
										else
										{
											Helper.DrawMarker(Helper.ToVector3(buildingClass.BuildingInPos));
										}
									}
									bool flag11 = buildingClass.GarageType == eGarageType.SixtyCarGarage;
									if (flag11 && num4 <= 300f)
									{
										Helper.DrawMarker(buildingClass.GarageAccessPoint(false));
									}
									if (num3 <= 300f && (!flag11 || num4 == float.MaxValue))
									{
										if (SouthLSHousingExterior.IsCustom(buildingClass))
										{
											Helper.DrawMarker(SouthLSHousingExterior.GarageMarkerPosition(buildingClass), default(Color), new Vector3(0.8f, 0.8f, 0.08f));
										}
										else
										{
											Helper.DrawMarker(buildingClass.GarageAccessPoint(flag4));
										}
									}
									if (ConfigHelper.DebugMode)
									{
										if (num2 <= 1000f)
										{
											if (!(buildingClass.BuildingLobby == Helper.QuaternionZero()))
											{
												Helper.DrawMarker(Helper.ToVector3(buildingClass.BuildingLobby), Color.LightPink, default(Vector3), "Lobby");
											}
											if (!(buildingClass.BuildingOutPos == Helper.QuaternionZero()))
											{
												Helper.DrawMarker(Helper.ToVector3(buildingClass.BuildingOutPos), Color.Red, default(Vector3), "Out Pos");
											}
										}
										if (num3 <= 1000f)
										{
											if (!(buildingClass.GarageInPos == Vector3.Zero))
											{
												Helper.DrawMarker(buildingClass.GarageInPos, Color.Orange, default(Vector3), "Car in Pos");
											}
											if (!(buildingClass.GarageFootOutPos == Helper.QuaternionZero()))
											{
												Helper.DrawMarker(Helper.ToVector3(buildingClass.GarageFootOutPos), Color.Yellow, default(Vector3), "Foot out Pos");
											}
											if (!(buildingClass.GarageOutPos == Helper.QuaternionZero()))
											{
												Helper.DrawMarker(Helper.ToVector3(buildingClass.GarageOutPos), Color.Green, default(Vector3), "Car out Pos");
											}
											if (!(buildingClass.GarageWaypoint == Helper.QuaternionZero()))
											{
												Helper.DrawMarker(Helper.ToVector3(buildingClass.GarageWaypoint), Color.Indigo, default(Vector3), "Garage waypoint");
											}
										}
										int num5 = buildingClass.Apartments.Count - 1;
										for (int j = 0; j <= num5; j++)
										{
											ApartmentClass apartmentClass = buildingClass.Apartments[j];
											if (apartmentClass.ExitDistance() <= 3000f)
											{
												if (!(apartmentClass.ApartmentDoorPos == Helper.QuaternionZero()))
												{
													Helper.DrawMarker(Helper.ToVector3(apartmentClass.ApartmentDoorPos), Color.Red, default(Vector3), $"Door Pos {apartmentClass.FriendlyName()}");
												}
												if (!(apartmentClass.ApartmentInPos == Vector3.Zero))
												{
													Helper.DrawMarker(apartmentClass.ApartmentInPos, Color.Green, default(Vector3), $"Teleport in Pos {apartmentClass.FriendlyName()}");
												}
												if (!(apartmentClass.ApartmentOutPos == Vector3.Zero))
												{
													Helper.DrawMarker(apartmentClass.ApartmentOutPos, Color.Blue, default(Vector3), $"Exit Pos {apartmentClass.FriendlyName()}");
												}
												if (!(apartmentClass.WardrobePos == Helper.QuaternionZero()))
												{
													Helper.DrawMarker(Helper.ToVector3(apartmentClass.WardrobePos), Color.Purple, default(Vector3), $"Wardrobe Pos {apartmentClass.FriendlyName()}");
												}
												if (!(apartmentClass.SavePos == Vector3.Zero))
												{
													Helper.DrawMarker(apartmentClass.SavePos, Color.Pink, default(Vector3), $"Save Pos {apartmentClass.FriendlyName()}");
												}
											}
										}
									}
									if (flag7)
									{
										SetBlipHidden(buildingClass.BuildingBlip, hidden);
										SetBlipHidden(buildingClass.GarageBlip, hidden);
									}
								}
								if (flag6)
								{
									if (HighEndApartment.Building != null)
									{
										HighEndApartment.Building.HideExterior();
									}
									if (TwoCarGarage.Apartment != null)
									{
										TwoCarGarage.TwoCarGarageOnTick();
									}
									if (SixCarGarage.Apartment != null)
									{
										SixCarGarage.SixCarGarageOnTick();
									}
									if (TenCarGarage.Apartment != null)
									{
										TenCarGarage.TenCarGarageOnTick();
									}
									Game.DisableControlThisFrame(GTA.Control.SelectCharacterMultiplayer);
								}
								if (ApartmentHomeServices.InHome(MediumEndApartment.Apartment))
								{
									MediumEndApartment.MediumEndApartmentOnTick();
								}
								if (ApartmentHomeServices.InHome(LowEndApartment.Apartment))
								{
									LowEndApartment.LowEndApartmentOnTick();
								}
								if (HighEndApartment.Building != null)
								{
									HighEndApartment.HighEndApartmentOnTick(position, flag6);
								}
								if (flag6 && Mansions.MansionList.Any((_Closure_0024__._0024I31_002D1 != null) ? _Closure_0024__._0024I31_002D1 : (_Closure_0024__._0024I31_002D1 = _Closure_0024__._0024I._Lambda_0024__31_002D1)))
								{
									ApartmentProps.PropOnTick();
								}
								if (Helper.PI != 0)
								{
									int pI = Helper.PI;
									if (pI == Helper.GetInterior(TenCarGarage.Interior) || pI == Helper.GetInterior(SixCarGarage.Interior) || pI == Helper.GetInterior(TwoCarGarage.Interior) || pI == Helper.GetInterior(LowEndApartment.Interior) || pI == Helper.GetInterior(MediumEndApartment.Interior))
									{
										ApartmentProps.PropOnTick();
									}
								}
								if (ApartmentProps.TVOn && Game.GameTime - ApartmentProps.LastPropTick > 5000)
								{
									ApartmentProps.ReleaseTellyNow();
								}
								if ((object)Helper.LV != null && Helper.LV.Exists() && Helper.IsPersonalVehicle(Helper.LV))
								{
									Blip attachedBlip = Helper.LV.AttachedBlip;
									if ((object)attachedBlip != null && attachedBlip.Exists())
									{
										SpaBlips.SetMissionHidden(attachedBlip, flag4);
									}
								}
								Mansions.MansionsTick();
								ApartmentProps.RadioDistanceCheck();
								BuildingClass.ExteriorHideCheck();
								Helper.OnlineMapNearTick();
								Stability.InteriorGuardTick();
								Stability.HeartbeatTick(position, Helper.PI);
								if (ConfigHelper.DebugMode && Helper.MenuPool.IsAnyMenuOpen())
								{
									if (Game.IsControlJustPressed(GTA.Control.FrontendAccept))
									{
										Logger.Log("input: FrontendAccept pressed while menu open");
									}
									if (Game.IsControlJustPressed(GTA.Control.PhoneSelect))
									{
										Logger.Log("input: PhoneSelect pressed while menu open");
									}
									if (Game.IsControlJustPressed(GTA.Control.Context))
									{
										Logger.Log("input: Context pressed while menu open");
									}
								}
							}
							if (ConfigHelper.DebugMode)
							{
								Helper.Debug();
								if (Helper.Player.IsAiming)
								{
									Entity targetedEntity = V2Compat.GetTargetedEntity(Helper.Player);
									if (targetedEntity == null)
									{
										Helper.debug3rdLine = "Nothing to show";
									}
									else
									{
										Helper.debug3rdLine = $"New Door({targetedEntity.Model.Hash}, New Vector3({targetedEntity.Position.X}F, {targetedEntity.Position.Y}F, {targetedEntity.Position.Z}F))";
									}
								}
							}
							if (NewFunc.IsCheating("spadebug"))
							{
								ConfigHelper.DebugMode = !ConfigHelper.DebugMode;
							}
							if (NewFunc.IsCheating("spadebugcam"))
							{
								DebugCamera.Toggle();
							}
						}
						catch (Exception ex13)
						{
							ProjectData.SetProjectError(ex13);
							Exception ex14 = ex13;
							Logger.Log($"{ex14.Message} {ex14.StackTrace}");
							ProjectData.ClearProjectError();
						}
						try
						{
							ApartmentHomeServices.ValidateBedInput();
						}
						catch (Exception ex15)
						{
							ProjectData.SetProjectError(ex15);
							Exception ex16 = ex15;
							Logger.Log($"Apartment bed input guard: {ex16.Message}");
							CleanupStep("apartment bed input recovery", ApartmentHomeServices.Cleanup);
							ProjectData.ClearProjectError();
						}
						try
						{
							PrivateDancerPreview.ValidateInput();
						}
						catch (Exception ex17)
						{
							ProjectData.SetProjectError(ex17);
							Exception ex18 = ex17;
							Logger.Log($"Private preview input guard: {ex18.Message}");
							CleanupStep("private preview input recovery", (_Closure_0024__._0024I31_002D2 != null) ? _Closure_0024__._0024I31_002D2 : (_Closure_0024__._0024I31_002D2 = _Closure_0024__._0024I._Lambda_0024__31_002D2));
							ProjectData.ClearProjectError();
						}
						try
						{
							MansionBarber.FrameTick();
						}
						catch (Exception ex19)
						{
							ProjectData.SetProjectError(ex19);
							Exception ex20 = ex19;
							Logger.Log($"Mansion barber input guard: {ex20.Message}");
							CleanupStep("mansion barber input recovery", MansionBarber.Cleanup);
							ProjectData.ClearProjectError();
						}
						try
						{
							Wardrobe.ApartmentFrameTick();
						}
						catch (Exception ex21)
						{
							ProjectData.SetProjectError(ex21);
							Exception ex22 = ex21;
							Logger.Log($"Apartment wardrobe input guard: {ex22.Message}");
							CleanupStep("apartment wardrobe input recovery", Wardrobe.CleanupApartment);
							ProjectData.ClearProjectError();
						}
						try
						{
							OnlineExpansion.FrameTick(Helper.PP);
						}
						catch (Exception ex23)
						{
							ProjectData.SetProjectError(ex23);
							Exception ex24 = ex23;
							Logger.Log("Online expansion tick: " + ex24.Message);
							ProjectData.ClearProjectError();
						}
						try
						{
							Helper.MenuPool.ProcessMenus();
							if (Helper.MenuPool.IsAnyMenuOpen())
							{
								Helper.DrawText("SPA II 4.1", 0.35f, 0.35f, GTA.UI.Font.ChaletLondon, new Vector2(0.5f, 0.026f));
							}
						}
						catch (Exception ex25)
						{
							ProjectData.SetProjectError(ex25);
							Exception ex26 = ex25;
							Logger.Log($"Menu processing: {ex26.Message} {ex26.StackTrace}");
							ProjectData.ClearProjectError();
						}
						try
						{
							Mechanic.iFruit.Update();
						}
						catch (Exception ex27)
						{
							ProjectData.SetProjectError(ex27);
							Exception ex28 = ex27;
							Logger.Log($"Phone update: {ex28.Message} {ex28.StackTrace}");
							ProjectData.ClearProjectError();
						}
						try
						{
							MansionFeatures.AssistantMenuTick();
						}
						catch (Exception ex29)
						{
							ProjectData.SetProjectError(ex29);
							Exception ex30 = ex29;
							Logger.Log($"Assistant dispatch: {ex30.Message} {ex30.StackTrace}");
							ProjectData.ClearProjectError();
						}
						try
						{
							MansionFeatures.MansionActionsTick();
						}
						catch (Exception ex31)
						{
							ProjectData.SetProjectError(ex31);
							Exception ex32 = ex31;
							Logger.Log($"Mansion player actions: {ex32.Message} {ex32.StackTrace}");
							ProjectData.ClearProjectError();
						}
						try
						{
							PartyService.PartyTick();
						}
						catch (Exception ex33)
						{
							ProjectData.SetProjectError(ex33);
							Exception ex34 = ex33;
							Logger.Log($"Party services: {ex34.Message} {ex34.StackTrace}");
							ProjectData.ClearProjectError();
						}
						try
						{
							MansionOriginalVoice.Tick();
						}
						catch (Exception ex35)
						{
							ProjectData.SetProjectError(ex35);
							Exception ex36 = ex35;
							Logger.Log($"Original assistant audio: {ex36.Message}");
							ProjectData.ClearProjectError();
						}
						try
						{
							ApartmentHomeServices.FrameTick();
						}
						catch (Exception ex37)
						{
							ProjectData.SetProjectError(ex37);
							Exception ex38 = ex37;
							Logger.Log($"Apartment actions: {ex38.Message}");
							CleanupStep("apartment action recovery", ApartmentHomeServices.Cleanup);
							ProjectData.ClearProjectError();
						}
						try
						{
							ApartmentProps.FrameTick();
						}
						catch (Exception ex39)
						{
							ProjectData.SetProjectError(ex39);
							Exception ex40 = ex39;
							Logger.Log($"Apartment TV: {ex40.Message}");
							CleanupStep("apartment TV recovery", ApartmentProps.ReleaseTellyNow);
							ProjectData.ClearProjectError();
						}
						try
						{
							MansionAmenities.FrameTick();
							MansionLeisure.Tick();
							MansionSecurity.Tick();
						}
						catch (Exception ex41)
						{
							ProjectData.SetProjectError(ex41);
							Exception ex42 = ex41;
							Logger.Log($"Mansion leisure: {ex42.Message}");
							ProjectData.ClearProjectError();
						}
						try
						{
							MansionVehicleShop.Tick();
						}
						catch (Exception ex43)
						{
							ProjectData.SetProjectError(ex43);
							Exception ex44 = ex43;
							Logger.Log($"Mansion vehicle catalogue: {ex44.Message}");
							ProjectData.ClearProjectError();
						}
					}
				}
			}
		}

		private static void SetBlipHidden(Blip b, bool hidden)
		{
			SpaBlips.SetMissionHidden(b, hidden);
		}

		private void CleanupStep(string name, Action action)
		{
			try
			{
				action();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				try
				{
					Logger.Log("Cleanup " + name + ": " + ex2.Message);
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				ProjectData.ClearProjectError();
			}
		}

		private void SPA2_Aborted(object sender, EventArgs e)
		{
			if (instanceGate == null)
			{
				return;
			}
			checked
			{
				try
				{
					CharacterReturnResidency.Clear();
					startupClock.Stop();
					startupWarmupClock.Stop();
					startupReadiness.Reset();
					startupWarmupFinished = true;
					try
					{
						PartyLocalMusic.Reset();
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						Logger.Log("Cleanup party audio: " + ex2.Message);
						ProjectData.ClearProjectError();
					}
					StoryLoadState state = StoryLoadGuard.ReadState();
					if (storyLoadSuspended || StoryLoadGuard.MustSuspend(state))
					{
						Logger.Log("SPA II unload: skipped outgoing-world native cleanup; load state=" + state.ToString() + "; resume pending=" + storyLoadSuspended);
						return;
					}
					CleanupStep("apartment transfer", ApartmentTravel.Cancel);
					CleanupStep("apartment residence", ApartmentResidence.Cleanup);
					CleanupStep("Executive Garage", (_Closure_0024__._0024I35_002D0 != null) ? _Closure_0024__._0024I35_002D0 : (_Closure_0024__._0024I35_002D0 = _Closure_0024__._0024I._Lambda_0024__35_002D0));
					CleanupStep("online expansion", OnlineExpansion.Cleanup);
					CleanupStep("destination streaming", DestinationStreaming.CancelOwned);
					CleanupStep("destination recovery", DestinationStreaming.CancelSafetyHold);
					CleanupStep("party", PartyService.PartyAborted);
					CleanupStep("gates", Mansions.RestoreAllGates);
					CleanupStep("persistent hints", Helper.ResetPersistentHelp);
					CleanupStep("mansion barber", MansionBarber.Cleanup);
					CleanupStep("mansion indoor map", MansionOrientation.Cleanup);
					CleanupStep("apartment home services", ApartmentHomeServices.Cleanup);
					CleanupStep("mansion services", MansionFeatures.MansionFeaturesCleanup);
					CleanupStep("original terrain", MansionTerrain.RestoreAfterAbort);
					CleanupStep("building exterior", BuildingClass.RestoreHiddenExterior);
					CleanupStep("South LS housing", SouthLSHousingExterior.Cleanup);
					BuildingClass[] array = Helper.buildings.ToArray();
					for (int i = 0; i < array.Length; i++)
					{
						HighEndApartment.Building = array[i];
						if (HighEndApartment.Building != null)
						{
							CleanupStep("sale sign", (_Closure_0024__._0024I35_002D1 != null) ? _Closure_0024__._0024I35_002D1 : (_Closure_0024__._0024I35_002D1 = _Closure_0024__._0024I._Lambda_0024__35_002D1));
							CleanupStep("building map icons", (_Closure_0024__._0024I35_002D2 != null) ? _Closure_0024__._0024I35_002D2 : (_Closure_0024__._0024I35_002D2 = _Closure_0024__._0024I._Lambda_0024__35_002D2));
						}
					}
					Vehicle[] array2 = Helper.outVehicleList.ToArray();
					_Closure_0024__35_002D0 closure_0024__35_002D = default(_Closure_0024__35_002D0);
					for (int j = 0; j < array2.Length; j++)
					{
						closure_0024__35_002D = new _Closure_0024__35_002D0(closure_0024__35_002D);
						closure_0024__35_002D._0024VB_0024Local_vehicle = array2[j];
						if ((object)closure_0024__35_002D._0024VB_0024Local_vehicle != null)
						{
							CleanupStep("outside vehicle blip", closure_0024__35_002D._Lambda_0024__3);
							CleanupStep("outside vehicle", closure_0024__35_002D._Lambda_0024__4);
						}
					}
					Helper.outVehicleList.Clear();
					Helper.ClearOutsideVehicleOwners();
					CleanupStep("mechanic contact", (_Closure_0024__._0024I35_002D5 != null) ? _Closure_0024__._0024I35_002D5 : (_Closure_0024__._0024I35_002D5 = _Closure_0024__._0024I._Lambda_0024__35_002D5));
					CleanupStep("insurance contact", (_Closure_0024__._0024I35_002D6 != null) ? _Closure_0024__._0024I35_002D6 : (_Closure_0024__._0024I35_002D6 = _Closure_0024__._0024I._Lambda_0024__35_002D6));
					CleanupStep("two-car garage", TwoCarGarage.Clear);
					CleanupStep("six-car garage", SixCarGarage.Clear);
					CleanupStep("ten-car garage", TenCarGarage.Clear);
					CleanupStep("low-end apartment", LowEndApartment.Clear);
					CleanupStep("medium apartment", MediumEndApartment.Clear);
					CleanupStep("apartment radio", (_Closure_0024__._0024I35_002D7 != null) ? _Closure_0024__._0024I35_002D7 : (_Closure_0024__._0024I35_002D7 = _Closure_0024__._0024I._Lambda_0024__35_002D7));
					CleanupStep("television", (_Closure_0024__._0024I35_002D8 != null) ? _Closure_0024__._0024I35_002D8 : (_Closure_0024__._0024I35_002D8 = _Closure_0024__._0024I._Lambda_0024__35_002D8));
					CleanupStep("stat scaleforms", Stability.ReleaseStatScaleforms);
					CleanupStep("interior pins", Stability.ReleaseAllInteriors);
					CleanupStep("owned cameras", Stability.DestroySpaCameras);
				}
				finally
				{
					Semaphore semaphore = instanceGate;
					instanceGate = null;
					try
					{
						semaphore.Release();
					}
					catch (Exception ex3)
					{
						ProjectData.SetProjectError(ex3);
						Exception ex4 = ex3;
						Logger.Log("SPA II instance gate release: " + ex4.Message);
						ProjectData.ClearProjectError();
					}
					finally
					{
						semaphore.Dispose();
					}
				}
			}
		}

		private void SPA2_KeyUp(object sender, KeyEventArgs e)
		{
			SpaBlips.ReleaseKey(e.KeyCode);
		}

		private void SPA2_KeyDown(object sender, KeyEventArgs e)
		{
			if (!initialized || storyLoadSuspended || characterSwitchPending || StoryLoadGuard.WorldLoading() || Function.Call<bool>(Hash.IS_PLAYER_SWITCH_IN_PROGRESS) || Helper.GetPlayer() != frameOwner || SpaBlips.HandleKey(e.KeyCode))
			{
				return;
			}
			if (keyLogCount < 40)
			{
				checked
				{
					keyLogCount++;
				}
				Logger.Log($"key: {e.KeyCode} ({(int)e.KeyCode}), assistant key = {MansionFeatures.AssistantKey}, mansion menu key = {MansionFeatures.MansionMenuKey}");
			}
			if (e.KeyCode == MansionFeatures.AssistantKey)
			{
				MansionFeatures.AssistantKeyRequested = true;
			}
			if (!ConfigHelper.DebugMode)
			{
				return;
			}
			if (Game.IsKeyPressed(Keys.Up))
			{
				if (Game.Player.Character.IsInVehicle())
				{
					Vector3 position = Game.Player.Character.CurrentVehicle.Position;
					Logger.Logg($"New Vector3({position.X}F, {position.Y}F, {position.Z}F)");
				}
				else
				{
					Vector3 position2 = Game.Player.Character.Position;
					Logger.Logg($"New Vector3({position2.X}F, {position2.Y}F, {position2.Z - 1f}F)");
				}
				GTA.UI.Screen.ShowSubtitle("Position copied");
			}
			if (Game.IsKeyPressed(Keys.Down))
			{
				if (Game.Player.Character.IsInVehicle())
				{
					Vector3 position3 = Game.Player.Character.CurrentVehicle.Position;
					float heading = Game.Player.Character.CurrentVehicle.Heading;
					Logger.Logg($"New Quaternion({position3.X}F, {position3.Y}F, {position3.Z}F, {heading}F)");
				}
				else
				{
					Vector3 position4 = Game.Player.Character.Position;
					float heading2 = Game.Player.Character.Heading;
					Logger.Logg($"New Quaternion({position4.X}F, {position4.Y}F, {position4.Z - 1f}F, {heading2}F)");
				}
				GTA.UI.Screen.ShowSubtitle("Quaternion copied");
			}
			if (Game.IsKeyPressed(Keys.Left) && DebugCamera.IsEnabled())
			{
				Camera camera = DebugCamera.Camera;
				Logger.Logg($"New CameraPRH(New Vector3({camera.Position.X}F, {camera.Position.Y}F, {camera.Position.Z}F), New Vector3({camera.Rotation.X}F, {camera.Rotation.Y}F, {camera.Rotation.Z}F), {camera.FieldOfView}F)");
				GTA.UI.Screen.ShowSubtitle("Gameplay camera copied");
			}
			if (Game.IsKeyPressed(Keys.Right))
			{
				Logger.Logg(Helper.debug3rdLine);
				GTA.UI.Screen.ShowSubtitle("Prop captured");
			}
			if (Game.IsKeyPressed(Keys.Delete))
			{
				DebugCamera.Toggle();
			}
			if (Game.IsKeyPressed(Keys.End))
			{
				DebugHelper.DebugMenu.Visible = true;
			}
		}
	}
}
