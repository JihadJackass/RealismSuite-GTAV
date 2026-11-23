using System;
using System.Collections;
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
	public static class ApartmentHomeServices
	{
		internal class ActionSession
		{
			public Ped Actor;

			public IntPtr Address;

			public int Model;

			public Vector3 Origin;

			public Stopwatch Clock;

			public ApartmentClass Home;

			public Mansion Mansion;

			public string Kind;

			public string LocationName;

			public string Dict;

			public string Clip;

			public bool WasFrozen;

			public bool ChangedFreeze;

			public bool Started;

			public int Phase;

			public int SleepClockBefore;

			public int SleepClockAfter;

			public bool SleepClockConfirmed;

			public int Particle;

			internal ShowerAppearance Appearance;

			public ActionSession()
			{
				Clock = Stopwatch.StartNew();
			}
		}

		internal class ShowerAppearance
		{
			public int[,] Components;

			public int[,] Props;

			public bool Captured;

			public bool Changed;

			public int[] Preset;

			public ShowerAppearance()
			{
				Components = new int[12, 3];
				Props = new int[8, 2];
			}
		}

		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Action _0024I60_002D3;

			public static Action _0024I60_002D6;

			public static Action _0024I63_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal void _Lambda_0024__60_002D3()
			{
				Screen.FadeIn(300);
			}

			[SpecialName]
			internal void _Lambda_0024__60_002D6()
			{
				Function.Call(Hash.REMOVE_NAMED_PTFX_ASSET, "scr_mp_house");
			}

			[SpecialName]
			internal void _Lambda_0024__63_002D0()
			{
				if (menu != null)
				{
					menu.Visible = false;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__57_002D0
		{
			public string _0024VB_0024Local_where;

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion value)
			{
				if (Operators.CompareString(value.Name, _0024VB_0024Local_where, false) == 0)
				{
					return MansionBedInHome(value);
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__59_002D0
		{
			internal ActionSession _0024VB_0024Local_s;

			public _Closure_0024__59_002D0(_Closure_0024__59_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_s = arg0._0024VB_0024Local_s;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				_0024VB_0024Local_s.Actor.Health = _0024VB_0024Local_s.Actor.MaxHealth;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__60_002D0
		{
			internal ActionSession _0024VB_0024Local_s;

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				if (SameActor(_0024VB_0024Local_s.Actor, _0024VB_0024Local_s.Model, _0024VB_0024Local_s.Address) && _0024VB_0024Local_s.ChangedFreeze)
				{
					_0024VB_0024Local_s.Actor.IsPositionFrozen = _0024VB_0024Local_s.WasFrozen;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				if (SameActor(_0024VB_0024Local_s.Actor, _0024VB_0024Local_s.Model, _0024VB_0024Local_s.Address) && _0024VB_0024Local_s.Started && Function.Call<bool>(Hash.IS_ENTITY_PLAYING_ANIM, _0024VB_0024Local_s.Actor.Handle, _0024VB_0024Local_s.Dict, _0024VB_0024Local_s.Clip, 3))
				{
					Function.Call(Hash.STOP_ANIM_TASK, _0024VB_0024Local_s.Actor.Handle, _0024VB_0024Local_s.Dict, _0024VB_0024Local_s.Clip, -3f);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__2()
			{
				RestoreShowerAppearance(_0024VB_0024Local_s);
			}

			[SpecialName]
			internal void _Lambda_0024__4()
			{
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, _0024VB_0024Local_s.Particle, false);
			}

			[SpecialName]
			internal void _Lambda_0024__5()
			{
				Function.Call(Hash.REMOVE_ANIM_DICT, _0024VB_0024Local_s.Dict);
			}
		}

		public const int SleepHours = 8;

		private static UIMenu menu;

		private static ApartmentClass menuHome;

		private static Mansion menuMansion;

		private static Ped menuActor;

		private static IntPtr menuAddress;

		private static int menuModel;

		private static Vector3 menuPoint;

		private static bool menuIsBed;

		private static ApartmentClass reachedBedHome;

		private static int apartmentTickFrame = int.MinValue;

		private static ActionSession active;

		private static readonly string[] showerModels = new string[5] { "v_16_high_bath_showerdoor", "p_mp_showerdoor_s", "ex_p_mp_h_showerdoor_s", "v_16_lo_shower", "prop_shower_rack_01" };

		private static ApartmentClass showerHome;

		private static Vector3 showerPoint;

		private static int showerScanAt;

		public static bool IsBusy => active != null;

		public static bool TriggerNear(Vector3 position, Vector3 target, float radius)
		{
			if (!PartyPlacement.FinitePoint(position) || !PartyPlacement.FinitePoint(target) || target == Vector3.Zero || radius <= 0f || radius > 5f)
			{
				return false;
			}
			Vector3 vector = position - target;
			return Math.Abs(vector.Z) <= 1.8f && vector.X * vector.X + vector.Y * vector.Y <= radius * radius;
		}

		public static string BedroomStation(Vector3 position, Vector3 bed, Vector3 closet)
		{
			bool flag = TriggerNear(position, bed, 2.2f);
			if (TriggerNear(position, closet, 2f) && (!flag || position.DistanceToSquared(closet) <= position.DistanceToSquared(bed)))
			{
				return "Closet";
			}
			if (flag)
			{
				return "Bed";
			}
			return null;
		}

		public static bool CompactBedroomOverlap(bool compactHome, Vector3 position, Vector3 bed, Vector3 closet)
		{
			if (compactHome && TriggerNear(position, bed, 2.2f))
			{
				return TriggerNear(position, closet, 2f);
			}
			return false;
		}

		public static string ApartmentBedroomStation(bool compactHome, Vector3 position, Vector3 bed, Vector3 closet)
		{
			if (CompactBedroomOverlap(compactHome, position, bed, closet))
			{
				return "Bedroom";
			}
			return BedroomStation(position, bed, closet);
		}

		public static bool BedReachable(Vector3 position, Vector3 bed, bool originalBedHint)
		{
			if (!TriggerNear(position, bed, 2.2f))
			{
				if (originalBedHint)
				{
					return TriggerNear(position, bed, 4.5f);
				}
				return false;
			}
			return true;
		}

		private static bool OriginalBedHintVisible()
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, "SA_BED_IN");
			return Function.Call<bool>(Hash.END_TEXT_COMMAND_IS_THIS_HELP_MESSAGE_BEING_DISPLAYED, 0);
		}

		public static int[] ShowerPreset(int model)
		{
			switch (model)
			{
			case 225514697:
				return new int[21]
				{
					3, 26, 0, 4, 18, 0, 5, 0, 0, 6,
					1, 0, 8, 0, 0, 10, 0, 0, 11, 0,
					0
				};
			case -1692214353:
				return new int[21]
				{
					3, 26, 0, 4, 18, 0, 5, 0, 0, 6,
					5, 0, 8, 14, 0, 10, 0, 0, 11, 0,
					0
				};
			case -1686040670:
				return new int[21]
				{
					3, 16, 0, 4, 22, 0, 5, 0, 0, 6,
					1, 0, 8, 0, 0, 10, 0, 0, 11, 0,
					0
				};
			default:
				return new int[0];
			}
		}

		private static bool CaptureShowerAppearance(ActionSession s)
		{
			if (!SameActor(s.Actor, s.Model, s.Address))
			{
				return false;
			}
			ShowerAppearance showerAppearance = new ShowerAppearance
			{
				Preset = ShowerPreset(s.Model)
			};
			if (showerAppearance.Preset.Length == 0)
			{
				return false;
			}
			checked
			{
				int num = showerAppearance.Preset.Length - 1;
				for (int i = 0; i <= num; i += 3)
				{
					if (!Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, s.Actor.Handle, showerAppearance.Preset[i], showerAppearance.Preset[i + 1], showerAppearance.Preset[i + 2]))
					{
						return false;
					}
				}
				int num2 = 0;
				do
				{
					showerAppearance.Components[num2, 0] = Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, s.Actor.Handle, num2);
					showerAppearance.Components[num2, 1] = Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, s.Actor.Handle, num2);
					showerAppearance.Components[num2, 2] = Function.Call<int>(Hash.GET_PED_PALETTE_VARIATION, s.Actor.Handle, num2);
					num2++;
				}
				while (num2 <= 11);
				int num3 = 0;
				do
				{
					showerAppearance.Props[num3, 0] = Function.Call<int>(Hash.GET_PED_PROP_INDEX, s.Actor.Handle, num3);
					showerAppearance.Props[num3, 1] = Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, s.Actor.Handle, num3);
					num3++;
				}
				while (num3 <= 7);
				showerAppearance.Captured = true;
				s.Appearance = showerAppearance;
				return true;
			}
		}

		private static bool ApplyShowerAppearance(ActionSession s)
		{
			ShowerAppearance appearance = s.Appearance;
			if (appearance == null || !appearance.Captured || !SameActor(s.Actor, s.Model, s.Address))
			{
				return false;
			}
			checked
			{
				int num = appearance.Preset.Length - 1;
				for (int i = 0; i <= num; i += 3)
				{
					if (!Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, s.Actor.Handle, appearance.Preset[i], appearance.Preset[i + 1], appearance.Preset[i + 2]))
					{
						return false;
					}
				}
				appearance.Changed = true;
				int num2 = appearance.Preset.Length - 1;
				for (int j = 0; j <= num2; j += 3)
				{
					if (!SameActor(s.Actor, s.Model, s.Address))
					{
						return false;
					}
					Function.Call(Hash.SET_PED_COMPONENT_VARIATION, s.Actor.Handle, appearance.Preset[j], appearance.Preset[j + 1], appearance.Preset[j + 2], 0);
				}
				int num3 = 0;
				do
				{
					if (!SameActor(s.Actor, s.Model, s.Address))
					{
						return false;
					}
					Function.Call(Hash.CLEAR_PED_PROP, s.Actor.Handle, num3, 0);
					num3++;
				}
				while (num3 <= 7);
				return true;
			}
		}

		private static void RestoreShowerAppearance(ActionSession s)
		{
			ShowerAppearance appearance = s.Appearance;
			if (appearance == null || !appearance.Captured || !appearance.Changed || !SameActor(s.Actor, s.Model, s.Address))
			{
				return;
			}
			checked
			{
				int num = appearance.Preset.Length - 1;
				for (int i = 0; i <= num; i += 3)
				{
					try
					{
						if (!SameActor(s.Actor, s.Model, s.Address))
						{
							return;
						}
						int num2 = appearance.Preset[i];
						Function.Call(Hash.SET_PED_COMPONENT_VARIATION, s.Actor.Handle, num2, appearance.Components[num2, 0], appearance.Components[num2, 1], appearance.Components[num2, 2]);
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						ProjectData.ClearProjectError();
					}
				}
				int num3 = 0;
				do
				{
					try
					{
						if (!SameActor(s.Actor, s.Model, s.Address))
						{
							return;
						}
						if (appearance.Props[num3, 0] < 0)
						{
							Function.Call(Hash.CLEAR_PED_PROP, s.Actor.Handle, num3, 0);
						}
						else
						{
							Function.Call(Hash.SET_PED_PROP_INDEX, s.Actor.Handle, num3, appearance.Props[num3, 0], appearance.Props[num3, 1], true, 0);
						}
					}
					catch (Exception projectError2)
					{
						ProjectData.SetProjectError(projectError2);
						ProjectData.ClearProjectError();
					}
					num3++;
				}
				while (num3 <= 7);
				appearance.Changed = false;
			}
		}

		private static bool SameActor(Ped actor, int model, IntPtr address)
		{
			if ((object)actor != null && actor.Exists() && actor.Model.Hash == model)
			{
				return actor.MemoryAddress == address;
			}
			return false;
		}

		public static bool Owns(ApartmentClass home)
		{
			if (home != null && home.Owner() != eOwner.Nobody)
			{
				return home.Owner() == Helper.GetPlayer();
			}
			return false;
		}

		public static bool SharedHomeInteriorMatches(bool boundHome, int currentInterior, int expectedInterior, bool nearInterior)
		{
			if (boundHome && nearInterior && currentInterior != 0 && expectedInterior != 0)
			{
				return currentInterior == expectedInterior;
			}
			return false;
		}

		public static bool BedInputRequested(bool contextPressed, bool disabledContextPressed, bool disabledFrontendPressed)
		{
			if (!contextPressed && !disabledContextPressed)
			{
				return disabledFrontendPressed;
			}
			return true;
		}

		private static void OwnBedContext()
		{
			Game.DisableControlThisFrame(Control.Context);
			Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 51, true);
		}

		public static void ValidateBedInput()
		{
			if (active != null)
			{
				if (Operators.CompareString(active.Kind, "Sleep", false) == 0 && SameActor(active.Actor, active.Model, active.Address) && (object)Helper.PP != null && Helper.PP.Handle == active.Actor.Handle && ServiceHomeValid(active.Home, active.Mansion))
				{
					OwnBedContext();
				}
			}
			else if (menuIsBed && menu != null && menu.Visible && SameActor(menuActor, menuModel, menuAddress) && (object)Helper.PP != null && Helper.PP.Handle == menuActor.Handle && ServiceHomeValid(menuHome, menuMansion) && TriggerNear(HomeActionPose.Position(Helper.PP), menuPoint, 3f))
			{
				OwnBedContext();
			}
		}

		private static bool MansionBedInHome(Mansion m)
		{
			if (m != null && m.Enabled && m.IplsLoaded && m.IsOwnedByPlayer() && (object)Helper.PP != null && Helper.PP.Exists() && !Helper.PP.IsDead && !Helper.PP.IsInVehicle() && !DestinationStreaming.IsPlayerSafetyHeld(Helper.PP))
			{
				return TriggerNear(HomeActionPose.Position(Helper.PP), Helper.ToVector3(m.BedPos), 3f);
			}
			return false;
		}

		private static bool ServiceHomeValid(ApartmentClass home, Mansion m)
		{
			if (home != null)
			{
				return m == null && InHome(home);
			}
			return MansionBedInHome(m);
		}

		private static bool CanUseBed(ApartmentClass home, Mansion m)
		{
			if (ServiceHomeValid(home, m) && !IsBusy && !PartyService.IsPlayerActionActive && !MansionFeatures.IsWorkoutActive && !PartyPlayerActions.IsKissing && !Helper.PP.IsFalling && !Helper.PP.IsRagdoll)
			{
				return !DestinationStreaming.IsPlayerSafetyHeld(Helper.PP);
			}
			return false;
		}

		public static bool InHome(ApartmentClass home)
		{
			if (!Owns(home) || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle())
			{
				return false;
			}
			if (ApartmentResidence.Current != null)
			{
				return ApartmentResidence.Accepts(home);
			}
			int num = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Helper.PP.Handle);
			Vector3 vector = HomeActionPose.Position(Helper.PP);
			if (home.ApartmentType == eApartmentType.LowEnd)
			{
				return SharedHomeInteriorMatches(LowEndApartment.Apartment == home, num, home.InteriorID(), vector.DistanceTo(LowEndApartment.Interior) < 45f);
			}
			if (home.ApartmentType == eApartmentType.MediumEnd)
			{
				return SharedHomeInteriorMatches(MediumEndApartment.Apartment == home, num, home.InteriorID(), vector.DistanceTo(MediumEndApartment.Interior) < 45f);
			}
			if (vector.DistanceTo(Helper.ToVector3(home.WardrobePos)) > 80f)
			{
				return false;
			}
			return num != 0 && num == home.InteriorID();
		}

		private static bool CanAct(ApartmentClass home)
		{
			if (InHome(home) && !IsBusy && !PartyService.IsPlayerActionActive && !MansionFeatures.IsWorkoutActive && !PartyPlayerActions.IsKissing && !Helper.PP.IsFalling)
			{
				return !Helper.PP.IsRagdoll;
			}
			return false;
		}

		private static bool NearShower(ApartmentClass home)
		{
			if (showerHome != home || Game.GameTime >= showerScanAt)
			{
				showerHome = home;
				showerPoint = Vector3.Zero;
				showerScanAt = checked(Game.GameTime + 400);
				Vector3 position = Helper.PP.Position;
				string[] array = showerModels;
				foreach (string input in array)
				{
					int num = Function.Call<int>(Hash.GET_CLOSEST_OBJECT_OF_TYPE, position.X, position.Y, position.Z, 1.6f, Game.GenerateHash(input), false, false, false);
					if (num != 0)
					{
						Prop prop = Entity.FromHandle(num) as Prop;
						if ((object)prop != null && prop.Exists() && TriggerNear(position, prop.Position, 1.5f))
						{
							showerPoint = prop.Position;
							break;
						}
					}
				}
			}
			return TriggerNear(Helper.PP.Position, showerPoint, 1.5f);
		}

		public static bool Tick(ApartmentClass home)
		{
			apartmentTickFrame = Function.Call<int>(Hash.GET_FRAME_COUNT);
			if (IsBusy || ApartmentTravel.IsBusy)
			{
				return true;
			}
			if (!CanAct(home) || Helper.MenuPool.IsAnyMenuOpen())
			{
				return false;
			}
			Vector3 position = HomeActionPose.Position(Helper.PP);
			string text = ApartmentBedroomStation(home.ApartmentType == eApartmentType.LowEnd, position, home.SavePos, Helper.ToVector3(home.WardrobePos));
			bool originalBedHint = TriggerNear(position, home.SavePos, 4.5f) && OriginalBedHintVisible();
			if (text == null && BedReachable(position, home.SavePos, originalBedHint))
			{
				text = "Bed";
			}
			if (Operators.CompareString(text, "Bed", false) == 0 || Operators.CompareString(text, "Bedroom", false) == 0)
			{
				if (reachedBedHome != home)
				{
					reachedBedHome = home;
					Logger.Log("Apartment bed station reached: " + home.Name + "; position=" + position.ToString() + "; bed=" + home.SavePos.ToString() + "; original hint=" + originalBedHint);
				}
				bool contextPressed = Game.IsEnabledControlJustPressed(Control.Context);
				bool flag = Helper.ShowApartmentBedHelp((Operators.CompareString(text, "Bedroom", false) == 0) ? "Press ~INPUT_CONTEXT~ for sleep, clothes and supplies." : "Press ~INPUT_CONTEXT~ for sleep options.", true);
				OwnBedContext();
				if (BedInputRequested(contextPressed, Game.IsControlJustPressed(Control.Context), Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 51)))
				{
					Logger.Log("Apartment bed input: " + home.Name + "; position=" + HomeActionPose.Position(Helper.PP).ToString() + "; hint=" + flag);
					OpenBed(home, originalBedHint);
				}
				return true;
			}
			if (reachedBedHome == home)
			{
				reachedBedHome = null;
			}
			if (NearShower(home))
			{
				if (Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ to shower here. Move or press Back to stop.") && Game.IsControlJustReleased(Control.Context))
				{
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 0, 51, true);
					StartShower(home);
				}
				return true;
			}
			if (Operators.CompareString(text, "Closet", false) == 0)
			{
				if (Helper.ShowPersistentHelp("Press ~INPUT_CONTEXT~ for clothes and free ammunition / body armour.") && Game.IsControlJustReleased(Control.Context))
				{
					OpenCloset(home);
				}
				return true;
			}
			return false;
		}

		private static void PrepareMenu(ApartmentClass home, Vector3 point, string title, bool bed = false)
		{
			if (menu == null)
			{
				menu = new UIMenu("", title, new Point(0, -107));
				menu.SoundEnabled = false;
				menu.SetBannerType(Helper.MenuBanner);
				menu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(menu);
				menu.OnItemSelect += SelectAction;
			}
			menuHome = home;
			menuMansion = null;
			menuPoint = point;
			menuActor = Helper.PP;
			menuAddress = Helper.PP.MemoryAddress;
			menuModel = Helper.PP.Model.Hash;
			menuIsBed = bed;
			menu.Clear();
			menu.Subtitle.Caption = title;
			menu.ParentMenu = null;
		}

		public static void OpenBed(ApartmentClass home, bool originalBedHint = false)
		{
			if (CanUseBed(home, null))
			{
				Vector3 vector = HomeActionPose.Position(Helper.PP);
				if (BedReachable(vector, home.SavePos, originalBedHint || (TriggerNear(vector, home.SavePos, 4.5f) && OriginalBedHintVisible())))
				{
					bool flag = CompactBedroomOverlap(home.ApartmentType == eApartmentType.LowEnd, vector, home.SavePos, Helper.ToVector3(home.WardrobePos));
					PrepareMenu(home, vector, flag ? "BEDROOM - SLEEP & CLOTHES" : "SLEEP", true);
					FillBedMenu(flag);
					Logger.Log("Apartment bed options opened: " + home.Name + "; actor=" + Helper.PP.Handle + "; position=" + HomeActionPose.Position(Helper.PP).ToString());
				}
			}
		}

		private static void FillBedMenu(bool includeCloset = false)
		{
			menu.AddItem(new UIMenuItem("Sleep 8 hours", "Rest for eight hours and restore health. To save your story progress, leave the property and use a standard GTA save location.")
			{
				Tag = "Sleep"
			});
			if (includeCloset)
			{
				menu.AddItem(new UIMenuItem("Change clothes")
				{
					Tag = "Clothes"
				});
				menu.AddItem(new UIMenuItem("Free ammunition and body armour", "Refill ammunition for weapons you own and equip full body armour. No charge.")
				{
					Tag = "Supplies"
				});
			}
			menu.RefreshIndex();
			menu.Visible = true;
		}

		public static bool MansionBedTick(Mansion m)
		{
			if (!CanUseBed(null, m) || Helper.MenuPool.IsAnyMenuOpen() || !TriggerNear(HomeActionPose.Position(Helper.PP), Helper.ToVector3(m.BedPos), 1.8f))
			{
				return false;
			}
			bool contextPressed = Game.IsEnabledControlJustPressed(Control.Context);
			Helper.ShowApartmentBedHelp("Press ~INPUT_CONTEXT~ for sleep options.", true);
			OwnBedContext();
			if (BedInputRequested(contextPressed, Game.IsControlJustPressed(Control.Context), Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 51)))
			{
				OpenMansionBed(m);
			}
			return true;
		}

		public static void OpenMansionBed(Mansion m)
		{
			if (CanUseBed(null, m))
			{
				PrepareMenu(null, Helper.ToVector3(m.BedPos), "SLEEP", true);
				menuMansion = m;
				FillBedMenu();
				Logger.Log("Mansion bed options opened: " + m.Name + "; actor=" + Helper.PP.Handle + "; position=" + HomeActionPose.Position(Helper.PP).ToString());
			}
		}

		public static void OpenCloset(ApartmentClass home)
		{
			if (CanAct(home) && TriggerNear(Helper.PP.Position, Helper.ToVector3(home.WardrobePos), 2.2f))
			{
				PrepareMenu(home, Helper.ToVector3(home.WardrobePos), "APARTMENT CLOSET & SUPPLIES");
				menu.AddItem(new UIMenuItem("Change clothes")
				{
					Tag = "Clothes"
				});
				menu.AddItem(new UIMenuItem("Free ammunition and body armour", "Refill ammunition for weapons you own and equip full body armour. No charge.")
				{
					Tag = "Supplies"
				});
				menu.RefreshIndex();
				menu.Visible = true;
			}
		}

		private static void SelectAction(UIMenu sender, UIMenuItem item, int index)
		{
			if (!SameActor(menuActor, menuModel, menuAddress) || (object)Helper.PP == null || Helper.PP.Handle != menuActor.Handle || !(menuIsBed ? CanUseBed(menuHome, menuMansion) : CanAct(menuHome)) || !TriggerNear(HomeActionPose.Position(Helper.PP), menuPoint, 3f))
			{
				menu.Visible = false;
				return;
			}
			string text = item.Tag as string;
			menu.Visible = false;
			string text2 = ((menuHome != null) ? menuHome.FriendlyName() : ((menuMansion != null) ? menuMansion.Name : "Home"));
			Logger.Log("Apartment home action selected: " + (text ?? "<invalid>") + "; home=" + text2);
			switch (text)
			{
			case "Clothes":
				Wardrobe.OpenApartment(menuHome);
				break;
			case "Supplies":
				RefillSupplies(menuHome);
				break;
			case "Sleep":
				StartSleep(text2, menuHome, menuMansion);
				break;
			}
		}

		public static void RefillSupplies(ApartmentClass home)
		{
			if (!CanAct(home))
			{
				return;
			}
			IEnumerator enumerator = default(IEnumerator);
			int num2 = default(int);
			try
			{
				enumerator = Enum.GetValues(typeof(WeaponHash)).GetEnumerator();
				while (enumerator.MoveNext())
				{
					uint num;
					if ((num = Conversions.ToUInteger(enumerator.Current)) == 2725352035u || !Function.Call<bool>(Hash.IS_WEAPON_VALID, num) || !Function.Call<bool>(Hash.HAS_PED_GOT_WEAPON, Helper.PP.Handle, num, false))
					{
						continue;
					}
					using (OutputArgument outputArgument = new OutputArgument())
					{
						if (Function.Call<bool>(Hash.GET_MAX_AMMO, Helper.PP.Handle, num, outputArgument))
						{
							int result = outputArgument.GetResult<int>();
							if (result > 0)
							{
								Function.Call(Hash.SET_PED_AMMO, Helper.PP.Handle, num, result, false);
								num2 = checked(num2 + 1);
							}
						}
					}
				}
			}
			finally
			{
				if (enumerator is IDisposable)
				{
					(enumerator as IDisposable).Dispose();
				}
			}
			Helper.PP.Armor = 100;
			Notification.Show($"Apartment supplies: {num2} weapon(s) refilled, full body armour. Free.");
		}

		public static bool SleepFadeMayProceed(bool screenFadedOut, long elapsed)
		{
			if (screenFadedOut && elapsed >= 500)
			{
				return elapsed <= 6000;
			}
			return false;
		}

		public static bool SleepClockShiftConfirmed(int before, int after)
		{
			if (before < 0 || before >= 86400 || after < 0 || after >= 86400)
			{
				return false;
			}
			int num = checked(after - before + 86400) % 86400;
			return num >= 28800 && num <= 28802;
		}

		private static int ReadClockSeconds()
		{
			return checked(Function.Call<int>(Hash.GET_CLOCK_HOURS) * 3600 + Function.Call<int>(Hash.GET_CLOCK_MINUTES) * 60 + Function.Call<int>(Hash.GET_CLOCK_SECONDS));
		}

		public static string SleepRestriction(int wantedLevel, bool missionActive, eOwner owner)
		{
			if (owner != 0 && owner != eOwner.Franklin && owner != eOwner.Trevor)
			{
				return "Sleeping is available for Michael, Franklin or Trevor.";
			}
			if (wantedLevel > 0)
			{
				return "Lose the wanted level before sleeping.";
			}
			if (missionActive)
			{
				return "Finish the current mission before sleeping.";
			}
			return null;
		}

		private static string CurrentSleepRestriction()
		{
			return SleepRestriction(Game.Player.WantedLevel, Game.IsMissionActive, Helper.GetPlayer());
		}

		public static void StartSleep(string where, ApartmentClass home = null, Mansion m = null)
		{
			_Closure_0024__57_002D0 closure_0024__57_002D = new _Closure_0024__57_002D0();
			closure_0024__57_002D._0024VB_0024Local_where = where;
			if (IsBusy || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || PartyService.IsPlayerActionActive)
			{
				return;
			}
			string text = CurrentSleepRestriction();
			if (text != null)
			{
				Notification.Show(text);
				return;
			}
			if (home == null && m == null)
			{
				m = Mansions.MansionList.FirstOrDefault(closure_0024__57_002D._Lambda_0024__0);
			}
			if (!ServiceHomeValid(home, m))
			{
				return;
			}
			ActionSession actionSession = (active = new ActionSession
			{
				Actor = Helper.PP,
				Address = Helper.PP.MemoryAddress,
				Model = Helper.PP.Model.Hash,
				Origin = HomeActionPose.Position(Helper.PP),
				Home = home,
				Mansion = m,
				Kind = "Sleep",
				LocationName = closure_0024__57_002D._0024VB_0024Local_where,
				WasFrozen = Helper.PP.IsPositionFrozen
			});
			Logger.Log("Apartment sleep started: " + closure_0024__57_002D._0024VB_0024Local_where + "; home=" + ((home == null) ? "mansion" : home.Name) + "; actor=" + actionSession.Actor.Handle);
			try
			{
				actionSession.Actor.IsPositionFrozen = true;
				actionSession.ChangedFreeze = true;
				Screen.FadeOut(500);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Apartment sleep start: " + ex2.Message);
				Finish();
				ProjectData.ClearProjectError();
			}
		}

		private static void StartShower(ApartmentClass home)
		{
			if (!CanAct(home))
			{
				return;
			}
			bool flag = Function.Call<bool>(Hash.IS_PED_MALE, Helper.PP.Handle);
			ActionSession actionSession = (active = new ActionSession
			{
				Actor = Helper.PP,
				Address = Helper.PP.MemoryAddress,
				Model = Helper.PP.Model.Hash,
				Origin = HomeActionPose.Position(Helper.PP),
				Home = home,
				Kind = "Shower",
				Dict = (flag ? "mp_safehouseshower@male@" : "mp_safehouseshower@female@"),
				Clip = (flag ? "male_shower_idle_a" : "shower_idle_a")
			});
			try
			{
				if (!CaptureShowerAppearance(actionSession))
				{
					Notification.Show("No original shower outfit is available for this character model.");
					Finish();
				}
				else
				{
					Function.Call(Hash.REQUEST_ANIM_DICT, actionSession.Dict);
					Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_mp_house");
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Finish();
				Logger.Log("Apartment shower start: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		public static void FrameTick()
		{
			_Closure_0024__59_002D0 arg = new _Closure_0024__59_002D0(null);
			ValidateBedInput();
			Wardrobe.ApartmentFrameTick();
			if (ApartmentResidence.Current != null && !IsBusy && !ApartmentTravel.IsBusy && !Helper.MenuPool.IsAnyMenuOpen() && apartmentTickFrame != Function.Call<int>(Hash.GET_FRAME_COUNT))
			{
				Tick(ApartmentResidence.Current);
			}
			if (menu != null && menu.Visible && (!SameActor(menuActor, menuModel, menuAddress) || !ServiceHomeValid(menuHome, menuMansion)))
			{
				menu.Visible = false;
			}
			arg._0024VB_0024Local_s = active;
			if (arg._0024VB_0024Local_s == null)
			{
				return;
			}
			try
			{
				if (!SameActor(arg._0024VB_0024Local_s.Actor, arg._0024VB_0024Local_s.Model, arg._0024VB_0024Local_s.Address) || (object)Helper.PP == null || Helper.PP.Handle != arg._0024VB_0024Local_s.Actor.Handle || arg._0024VB_0024Local_s.Actor.IsDead || arg._0024VB_0024Local_s.Actor.IsInVehicle() || !HomeActionPose.Near(HomeActionPose.Position(arg._0024VB_0024Local_s.Actor), arg._0024VB_0024Local_s.Origin, 1.5f) || !ServiceHomeValid(arg._0024VB_0024Local_s.Home, arg._0024VB_0024Local_s.Mansion))
				{
					Logger.Log("Home action cancelled: actor, position or home changed; action=" + arg._0024VB_0024Local_s.Kind);
					Finish();
					return;
				}
				if (Operators.CompareString(arg._0024VB_0024Local_s.Kind, "Sleep", false) == 0)
				{
					if (arg._0024VB_0024Local_s.Phase == 0 && arg._0024VB_0024Local_s.Clock.ElapsedMilliseconds > 6000)
					{
						Finish();
						Notification.Show("Sleep could not start while the screen was busy. No time was skipped.");
					}
					else if (arg._0024VB_0024Local_s.Phase == 0 && SleepFadeMayProceed(Screen.IsFadedOut, arg._0024VB_0024Local_s.Clock.ElapsedMilliseconds))
					{
						arg._0024VB_0024Local_s.SleepClockBefore = ReadClockSeconds();
						Function.Call(Hash.ADD_TO_CLOCK_TIME, 8, 0, 0);
						arg._0024VB_0024Local_s.Phase = 1;
						arg._0024VB_0024Local_s.SleepClockAfter = ReadClockSeconds();
						arg._0024VB_0024Local_s.SleepClockConfirmed = SleepClockShiftConfirmed(arg._0024VB_0024Local_s.SleepClockBefore, arg._0024VB_0024Local_s.SleepClockAfter);
						arg._0024VB_0024Local_s.Clock.Restart();
						ReleaseAction(arg._Lambda_0024__0);
						Screen.FadeIn(500);
					}
					else if (arg._0024VB_0024Local_s.Phase == 1 && ((Screen.IsFadedIn && arg._0024VB_0024Local_s.Clock.ElapsedMilliseconds >= 500) || arg._0024VB_0024Local_s.Clock.ElapsedMilliseconds > 6000))
					{
						string locationName = arg._0024VB_0024Local_s.LocationName;
						Finish(!Screen.IsFadedIn);
						if (arg._0024VB_0024Local_s.SleepClockConfirmed)
						{
							Logger.Log($"> slept ({locationName}); advanced {8} hours; clock seconds={arg._0024VB_0024Local_s.SleepClockBefore}->{arg._0024VB_0024Local_s.SleepClockAfter}");
						}
						else
						{
							Logger.Log($"> sleep clock not confirmed ({locationName}); requested={8} hours; clock seconds={arg._0024VB_0024Local_s.SleepClockBefore}->{arg._0024VB_0024Local_s.SleepClockAfter}");
							Notification.Show("The game clock did not confirm the eight-hour jump.");
						}
					}
					return;
				}
				if (Game.IsControlJustPressed(Control.FrontendCancel) || Game.IsControlJustPressed(Control.PhoneCancel) || PartyPlayerActions.MovementRequested() || Helper.MenuPool.IsAnyMenuOpen())
				{
					Finish();
					return;
				}
				Control[] array = new Control[5]
				{
					Control.Context,
					Control.Attack,
					Control.Aim,
					Control.Enter,
					Control.Jump
				};
				for (int i = 0; i < array.Length; i = checked(i + 1))
				{
					Game.DisableControlThisFrame(array[i]);
				}
				if (!arg._0024VB_0024Local_s.Started)
				{
					if (Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, arg._0024VB_0024Local_s.Dict))
					{
						if (!ApplyShowerAppearance(arg._0024VB_0024Local_s))
						{
							Finish();
							return;
						}
						Function.Call(Hash.TASK_PLAY_ANIM, arg._0024VB_0024Local_s.Actor.Handle, arg._0024VB_0024Local_s.Dict, arg._0024VB_0024Local_s.Clip, 3f, -3f, -1, 49, 1f, false, false, false);
						arg._0024VB_0024Local_s.Started = true;
					}
					else if (arg._0024VB_0024Local_s.Clock.ElapsedMilliseconds > 5000)
					{
						Finish();
						return;
					}
				}
				if (arg._0024VB_0024Local_s.Started && arg._0024VB_0024Local_s.Particle == 0 && Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_mp_house"))
				{
					Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_mp_house");
					arg._0024VB_0024Local_s.Particle = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "ent_amb_shower", arg._0024VB_0024Local_s.Origin.X, arg._0024VB_0024Local_s.Origin.Y, arg._0024VB_0024Local_s.Origin.Z + 0.9f, 0f, 0f, 0f, 1f, false, false, false, false);
				}
				Helper.ShowPersistentHelp("Showering: move or press Back to stop.");
				if (arg._0024VB_0024Local_s.Clock.ElapsedMilliseconds > 30000)
				{
					Finish();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Apartment home action: " + ex2.Message);
				Finish();
				ProjectData.ClearProjectError();
			}
		}

		private static void Finish(bool fadeBack = true)
		{
			_Closure_0024__60_002D0 closure_0024__60_002D = new _Closure_0024__60_002D0();
			closure_0024__60_002D._0024VB_0024Local_s = active;
			active = null;
			if (closure_0024__60_002D._0024VB_0024Local_s != null)
			{
				ReleaseAction(closure_0024__60_002D._Lambda_0024__0);
				ReleaseAction(closure_0024__60_002D._Lambda_0024__1);
				ReleaseAction(closure_0024__60_002D._Lambda_0024__2);
				if (Operators.CompareString(closure_0024__60_002D._0024VB_0024Local_s.Kind, "Sleep", false) == 0 && fadeBack)
				{
					ReleaseAction((_Closure_0024__._0024I60_002D3 != null) ? _Closure_0024__._0024I60_002D3 : (_Closure_0024__._0024I60_002D3 = _Closure_0024__._0024I._Lambda_0024__60_002D3));
				}
				if (closure_0024__60_002D._0024VB_0024Local_s.Particle != 0)
				{
					ReleaseAction(closure_0024__60_002D._Lambda_0024__4);
				}
				if (closure_0024__60_002D._0024VB_0024Local_s.Dict != null)
				{
					ReleaseAction(closure_0024__60_002D._Lambda_0024__5);
				}
				if (Operators.CompareString(closure_0024__60_002D._0024VB_0024Local_s.Kind, "Shower", false) == 0)
				{
					ReleaseAction((_Closure_0024__._0024I60_002D6 != null) ? _Closure_0024__._0024I60_002D6 : (_Closure_0024__._0024I60_002D6 = _Closure_0024__._0024I._Lambda_0024__60_002D6));
				}
			}
		}

		private static void ReleaseAction(Action action)
		{
			try
			{
				action();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}

		public static void ForgetForStoryLoad()
		{
			active = null;
			reachedBedHome = null;
			menuHome = null;
			menuMansion = null;
			menuActor = null;
			menuAddress = IntPtr.Zero;
			menuModel = 0;
			menuIsBed = false;
			showerHome = null;
			showerPoint = Vector3.Zero;
			showerScanAt = 0;
			apartmentTickFrame = int.MinValue;
		}

		public static void Cleanup()
		{
			reachedBedHome = null;
			Finish();
			ReleaseAction(Wardrobe.CleanupApartment);
			ReleaseAction((_Closure_0024__._0024I63_002D0 != null) ? _Closure_0024__._0024I63_002D0 : (_Closure_0024__._0024I63_002D0 = _Closure_0024__._0024I._Lambda_0024__63_002D0));
			menuHome = null;
			menuMansion = null;
			menuActor = null;
			showerHome = null;
			showerPoint = Vector3.Zero;
		}
	}
}
