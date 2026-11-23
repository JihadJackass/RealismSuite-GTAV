using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using INMNativeUI;
using Microsoft.VisualBasic.CompilerServices;
using SPAII.INM;

namespace SPAII
{
	[StandardModule]
	internal static class Wardrobe
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<char, bool> _0024I387_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__387_002D0(char c)
			{
				return char.IsDigit(c);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__431_002D0
		{
			public UIMenu _0024VB_0024Local_root;

			public bool _0024VB_0024Local_idleStarted;

			public Ped _0024VB_0024Local_actor;

			public int _0024VB_0024Local_model;

			public IntPtr _0024VB_0024Local_address;

			public bool _0024VB_0024Local_changedHeading;

			public float _0024VB_0024Local_heading;

			public bool _0024VB_0024Local_changedFreeze;

			public bool _0024VB_0024Local_wasFrozen;

			public WardrobeMemory _0024VB_0024Local_original;

			public bool _0024VB_0024Local_preserveForeignHud;

			public Camera _0024VB_0024Local_view;

			public bool _0024VB_0024Local_renderingAssigned;

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				WardrobeTreeVisible(_0024VB_0024Local_root, true);
			}

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				if (_0024VB_0024Local_idleStarted && WardrobeActorMatches(_0024VB_0024Local_actor, _0024VB_0024Local_model, _0024VB_0024Local_address))
				{
					int num = Function.Call<int>(Hash.GET_SCRIPT_TASK_STATUS, _0024VB_0024Local_actor.Handle, Game.GenerateHash("SCRIPT_TASK_STAND_STILL"));
					if (num == 0 || num == 1)
					{
						Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, _0024VB_0024Local_actor.Handle);
					}
				}
			}

			[SpecialName]
			internal void _Lambda_0024__2()
			{
				if (_0024VB_0024Local_changedHeading && WardrobeActorMatches(_0024VB_0024Local_actor, _0024VB_0024Local_model, _0024VB_0024Local_address))
				{
					Function.Call(Hash.SET_ENTITY_HEADING, _0024VB_0024Local_actor.Handle, _0024VB_0024Local_heading);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__3()
			{
				if (_0024VB_0024Local_changedFreeze && WardrobeActorMatches(_0024VB_0024Local_actor, _0024VB_0024Local_model, _0024VB_0024Local_address))
				{
					Function.Call(Hash.FREEZE_ENTITY_POSITION, _0024VB_0024Local_actor.Handle, _0024VB_0024Local_wasFrozen);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__4()
			{
				if (_0024VB_0024Local_original != null)
				{
					RestoreMemory(_0024VB_0024Local_actor, _0024VB_0024Local_original, _0024VB_0024Local_model, _0024VB_0024Local_address);
				}
			}

			[SpecialName]
			internal void _Lambda_0024__5()
			{
				Camera renderingCamera = World.RenderingCamera;
				_0024VB_0024Local_preserveForeignHud = (object)renderingCamera != null && renderingCamera.Exists() && ((object)_0024VB_0024Local_view == null || renderingCamera.Handle != _0024VB_0024Local_view.Handle);
				bool flag = (object)renderingCamera != null && renderingCamera.Handle != 0 && renderingCamera.Exists();
				bool flag2 = flag && (object)_0024VB_0024Local_view != null && Stability.IsSpaCamera(_0024VB_0024Local_view) && renderingCamera.Handle == _0024VB_0024Local_view.Handle;
				if (MayReleaseRenderingCamera(_0024VB_0024Local_renderingAssigned, flag2, flag && !flag2))
				{
					World.RenderingCamera = null;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__6()
			{
				if ((object)_0024VB_0024Local_view != null && _0024VB_0024Local_view.Exists() && Stability.IsSpaCamera(_0024VB_0024Local_view))
				{
					_0024VB_0024Local_view.Delete();
				}
			}

			[SpecialName]
			internal void _Lambda_0024__7()
			{
				if ((object)_0024VB_0024Local_view != null && (object)Camera == _0024VB_0024Local_view)
				{
					Camera = null;
				}
				if (_0024VB_0024Local_original != null && Memory == _0024VB_0024Local_original)
				{
					Memory = null;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__8()
			{
				if (!_0024VB_0024Local_preserveForeignHud)
				{
					Helper.HideHud = false;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__432_002D0
		{
			public ClothingSet.CS _0024VB_0024Local_captured;

			public _Closure_0024__432_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_2;

			public _Closure_0024__432_002D0(_Closure_0024__432_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_captured = arg0._0024VB_0024Local_captured;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				if (_0024VB_0024Local_captured != null && WardrobeActorMatches(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_model, _0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_address))
				{
					Helper.SetClothes(_0024VB_0024NonLocal__0024VB_0024Closure_2._0024VB_0024Local_actor, _0024VB_0024Local_captured);
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__432_002D1
		{
			public Ped _0024VB_0024Local_actor;

			public int _0024VB_0024Local_model;

			public IntPtr _0024VB_0024Local_address;

			public _Closure_0024__432_002D1(_Closure_0024__432_002D1 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_actor = arg0._0024VB_0024Local_actor;
					_0024VB_0024Local_model = arg0._0024VB_0024Local_model;
					_0024VB_0024Local_address = arg0._0024VB_0024Local_address;
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__432_002D2
		{
			public ClothingSet.CS _0024VB_0024Local_captured;

			public _Closure_0024__432_002D1 _0024VB_0024NonLocal__0024VB_0024Closure_3;

			public _Closure_0024__432_002D2(_Closure_0024__432_002D2 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_captured = arg0._0024VB_0024Local_captured;
				}
			}

			[SpecialName]
			internal void _Lambda_0024__1()
			{
				if (_0024VB_0024Local_captured != null && WardrobeActorMatches(_0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_model, _0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_address))
				{
					Helper.SetProps(_0024VB_0024NonLocal__0024VB_0024Closure_3._0024VB_0024Local_actor, _0024VB_0024Local_captured);
				}
			}
		}

		public static Camera Camera;

		public static int ScriptStatus = -1;

		public static WardrobeMemory Memory;

		public static UIMenuItem iOutfits;

		public static UIMenuItem giSuits;

		public static UIMenuItem iFullSuits;

		public static UIMenuItem iSuitJackets;

		public static UIMenuItem iSuitJacketsButtoned;

		public static UIMenuItem iSuitPants;

		public static UIMenuItem iSuitVests;

		public static UIMenuItem iSuitTies;

		public static UIMenuItem giCasualJackets;

		public static UIMenuItem iOpenShirts;

		public static UIMenuItem iCasualJackets;

		public static UIMenuItem iCasualJacketShirts;

		public static UIMenuItem iCasualJacketTShirts;

		public static UIMenuItem iEarrings;

		public static UIMenuItem giGlasses;

		public static UIMenuItem iGlasses;

		public static UIMenuItem iSportShades;

		public static UIMenuItem iStreetShades;

		public static UIMenuItem giHats;

		public static UIMenuItem iHats;

		public static UIMenuItem iCapsForward;

		public static UIMenuItem iCapsBackward;

		public static UIMenuItem iHoodies;

		public static UIMenuItem iJackets;

		public static UIMenuItem iPants;

		public static UIMenuItem iShoes;

		public static UIMenuItem iShirts;

		public static UIMenuItem iShorts;

		public static UIMenuItem iSmartShoes;

		public static UIMenuItem iTShirt;

		public static UIMenuItem iTankTops;

		public static UIMenuItem iTops;

		public static UIMenuItem iVests;

		public static UIMenuItem iPoloShirt;

		public static UIMenuItem iChains;

		private static ApartmentClass apartmentHome;

		private static Mansion apartmentMansion;

		private static Ped apartmentActor;

		private static IntPtr apartmentActorAddress;

		private static int apartmentActorModel;

		private static float apartmentActorHeading;

		private static Camera apartmentCamera;

		private static WardrobeMemory apartmentMemory;

		private static UIMenu apartmentRoot;

		private static Vector3 apartmentOrigin;

		private static Vector3 apartmentEye;

		private static int apartmentInterior;

		private static bool apartmentWasFrozen;

		private static bool apartmentChangedFreeze;

		private static bool apartmentIdleStarted;

		private static bool apartmentChangedHeading;

		private static bool apartmentClosing;

		private static Stopwatch apartmentOpening;

		private static bool apartmentRenderingAssigned;

		[System.Runtime.CompilerServices.AccessedThroughProperty("gmMichael")]
		private static UIMenu _gmMichael;

		public static UIMenu gmMichael
		{
			[CompilerGenerated]
			get
			{
				return _gmMichael;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				MenuCloseEvent value2 = Main_OnMenuClose;
				UIMenu uIMenu = _gmMichael;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose -= value2;
				}
				_gmMichael = value;
				uIMenu = _gmMichael;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("gmFranklin")]
		private static UIMenu _gmFranklin;

		public static UIMenu gmFranklin
		{
			[CompilerGenerated]
			get
			{
				return _gmFranklin;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				MenuCloseEvent value2 = Main_OnMenuClose;
				UIMenu uIMenu = _gmFranklin;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose -= value2;
				}
				_gmFranklin = value;
				uIMenu = _gmFranklin;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("gmTrevor")]
		private static UIMenu _gmTrevor;

		public static UIMenu gmTrevor
		{
			[CompilerGenerated]
			get
			{
				return _gmTrevor;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				MenuCloseEvent value2 = Main_OnMenuClose;
				UIMenu uIMenu = _gmTrevor;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose -= value2;
				}
				_gmTrevor = value;
				uIMenu = _gmTrevor;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("gmFMM")]
		private static UIMenu _gmFMM;

		public static UIMenu gmFMM
		{
			[CompilerGenerated]
			get
			{
				return _gmFMM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				MenuCloseEvent value2 = Main_OnMenuClose;
				UIMenu uIMenu = _gmFMM;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose -= value2;
				}
				_gmFMM = value;
				uIMenu = _gmFMM;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("gmFMF")]
		private static UIMenu _gmFMF;

		public static UIMenu gmFMF
		{
			[CompilerGenerated]
			get
			{
				return _gmFMF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				MenuCloseEvent value2 = Main_OnMenuClose;
				UIMenu uIMenu = _gmFMF;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose -= value2;
				}
				_gmFMF = value;
				uIMenu = _gmFMF;
				if (uIMenu != null)
				{
					uIMenu.OnMenuClose += value2;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mOutfitM")]
		private static UIMenu _mOutfitM;

		public static UIMenu mOutfitM
		{
			[CompilerGenerated]
			get
			{
				return _mOutfitM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SetClothing_OnItemSelect;
				IndexChangedEvent value3 = SetClothing_OnIndexChange;
				UIMenu uIMenu = _mOutfitM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mOutfitM = value;
				uIMenu = _mOutfitM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmSuitM")]
		public static UIMenu gmSuitM
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mFullSuitsM")]
		private static UIMenu _mFullSuitsM;

		public static UIMenu mFullSuitsM
		{
			[CompilerGenerated]
			get
			{
				return _mFullSuitsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SetClothing_OnItemSelect;
				IndexChangedEvent value3 = SetClothing_OnIndexChange;
				UIMenu uIMenu = _mFullSuitsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mFullSuitsM = value;
				uIMenu = _mFullSuitsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitJacketsM")]
		private static UIMenu _mSuitJacketsM;

		public static UIMenu mSuitJacketsM
		{
			[CompilerGenerated]
			get
			{
				return _mSuitJacketsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitJacketsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitJacketsM = value;
				uIMenu = _mSuitJacketsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitPantsM")]
		private static UIMenu _mSuitPantsM;

		public static UIMenu mSuitPantsM
		{
			[CompilerGenerated]
			get
			{
				return _mSuitPantsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitPantsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitPantsM = value;
				uIMenu = _mSuitPantsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitVestsM")]
		private static UIMenu _mSuitVestsM;

		public static UIMenu mSuitVestsM
		{
			[CompilerGenerated]
			get
			{
				return _mSuitVestsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitVestsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitVestsM = value;
				uIMenu = _mSuitVestsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmCasualJacketsM")]
		public static UIMenu gmCasualJacketsM
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mCasualJacketM")]
		private static UIMenu _mCasualJacketM;

		public static UIMenu mCasualJacketM
		{
			[CompilerGenerated]
			get
			{
				return _mCasualJacketM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mCasualJacketM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mCasualJacketM = value;
				uIMenu = _mCasualJacketM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mOutfitF")]
		private static UIMenu _mOutfitF;

		public static UIMenu mOutfitF
		{
			[CompilerGenerated]
			get
			{
				return _mOutfitF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SetClothing_OnItemSelect;
				IndexChangedEvent value3 = SetClothing_OnIndexChange;
				UIMenu uIMenu = _mOutfitF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mOutfitF = value;
				uIMenu = _mOutfitF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmSuitF")]
		public static UIMenu gmSuitF
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mFullSuitsF")]
		private static UIMenu _mFullSuitsF;

		public static UIMenu mFullSuitsF
		{
			[CompilerGenerated]
			get
			{
				return _mFullSuitsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SetClothing_OnItemSelect;
				IndexChangedEvent value3 = SetClothing_OnIndexChange;
				UIMenu uIMenu = _mFullSuitsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mFullSuitsF = value;
				uIMenu = _mFullSuitsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitJacketsF")]
		private static UIMenu _mSuitJacketsF;

		public static UIMenu mSuitJacketsF
		{
			[CompilerGenerated]
			get
			{
				return _mSuitJacketsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitJacketsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitJacketsF = value;
				uIMenu = _mSuitJacketsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitJacketsButtonedF")]
		private static UIMenu _mSuitJacketsButtonedF;

		public static UIMenu mSuitJacketsButtonedF
		{
			[CompilerGenerated]
			get
			{
				return _mSuitJacketsButtonedF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitJacketsButtonedF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitJacketsButtonedF = value;
				uIMenu = _mSuitJacketsButtonedF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitPantsF")]
		private static UIMenu _mSuitPantsF;

		public static UIMenu mSuitPantsF
		{
			[CompilerGenerated]
			get
			{
				return _mSuitPantsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitPantsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitPantsF = value;
				uIMenu = _mSuitPantsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitVestsF")]
		private static UIMenu _mSuitVestsF;

		public static UIMenu mSuitVestsF
		{
			[CompilerGenerated]
			get
			{
				return _mSuitVestsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitVestsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitVestsF = value;
				uIMenu = _mSuitVestsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitTiesF")]
		private static UIMenu _mSuitTiesF;

		public static UIMenu mSuitTiesF
		{
			[CompilerGenerated]
			get
			{
				return _mSuitTiesF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitTiesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitTiesF = value;
				uIMenu = _mSuitTiesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmCasualJacketsF")]
		public static UIMenu gmCasualJacketsF
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mCasualJacketF")]
		private static UIMenu _mCasualJacketF;

		public static UIMenu mCasualJacketF
		{
			[CompilerGenerated]
			get
			{
				return _mCasualJacketF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mCasualJacketF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mCasualJacketF = value;
				uIMenu = _mCasualJacketF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mOutfitT")]
		private static UIMenu _mOutfitT;

		public static UIMenu mOutfitT
		{
			[CompilerGenerated]
			get
			{
				return _mOutfitT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SetClothing_OnItemSelect;
				IndexChangedEvent value3 = SetClothing_OnIndexChange;
				UIMenu uIMenu = _mOutfitT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mOutfitT = value;
				uIMenu = _mOutfitT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmSuitT")]
		public static UIMenu gmSuitT
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mFullSuitsT")]
		private static UIMenu _mFullSuitsT;

		public static UIMenu mFullSuitsT
		{
			[CompilerGenerated]
			get
			{
				return _mFullSuitsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SetClothing_OnItemSelect;
				IndexChangedEvent value3 = SetClothing_OnIndexChange;
				UIMenu uIMenu = _mFullSuitsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mFullSuitsT = value;
				uIMenu = _mFullSuitsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitJacketsT")]
		private static UIMenu _mSuitJacketsT;

		public static UIMenu mSuitJacketsT
		{
			[CompilerGenerated]
			get
			{
				return _mSuitJacketsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitJacketsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitJacketsT = value;
				uIMenu = _mSuitJacketsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSuitPantsT")]
		private static UIMenu _mSuitPantsT;

		public static UIMenu mSuitPantsT
		{
			[CompilerGenerated]
			get
			{
				return _mSuitPantsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSuitPantsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSuitPantsT = value;
				uIMenu = _mSuitPantsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mOutfitFMM")]
		private static UIMenu _mOutfitFMM;

		public static UIMenu mOutfitFMM
		{
			[CompilerGenerated]
			get
			{
				return _mOutfitFMM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = FMOutfit_OnItemSelect;
				IndexChangedEvent value3 = FMOutfit_OnIndexChange;
				UIMenu uIMenu = _mOutfitFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mOutfitFMM = value;
				uIMenu = _mOutfitFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mEarringsFMM")]
		private static UIMenu _mEarringsFMM;

		public static UIMenu mEarringsFMM
		{
			[CompilerGenerated]
			get
			{
				return _mEarringsFMM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mEarringsFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mEarringsFMM = value;
				uIMenu = _mEarringsFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mGlassesFMM")]
		private static UIMenu _mGlassesFMM;

		public static UIMenu mGlassesFMM
		{
			[CompilerGenerated]
			get
			{
				return _mGlassesFMM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mGlassesFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mGlassesFMM = value;
				uIMenu = _mGlassesFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mHatsFMM")]
		private static UIMenu _mHatsFMM;

		public static UIMenu mHatsFMM
		{
			[CompilerGenerated]
			get
			{
				return _mHatsFMM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mHatsFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mHatsFMM = value;
				uIMenu = _mHatsFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mChainsFMM")]
		private static UIMenu _mChainsFMM;

		public static UIMenu mChainsFMM
		{
			[CompilerGenerated]
			get
			{
				return _mChainsFMM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mChainsFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mChainsFMM = value;
				uIMenu = _mChainsFMM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mOutfitFMF")]
		private static UIMenu _mOutfitFMF;

		public static UIMenu mOutfitFMF
		{
			[CompilerGenerated]
			get
			{
				return _mOutfitFMF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = FMOutfit_OnItemSelect;
				IndexChangedEvent value3 = FMOutfit_OnIndexChange;
				UIMenu uIMenu = _mOutfitFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mOutfitFMF = value;
				uIMenu = _mOutfitFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mEarringsFMF")]
		private static UIMenu _mEarringsFMF;

		public static UIMenu mEarringsFMF
		{
			[CompilerGenerated]
			get
			{
				return _mEarringsFMF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mEarringsFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mEarringsFMF = value;
				uIMenu = _mEarringsFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mGlassesFMF")]
		private static UIMenu _mGlassesFMF;

		public static UIMenu mGlassesFMF
		{
			[CompilerGenerated]
			get
			{
				return _mGlassesFMF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mGlassesFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mGlassesFMF = value;
				uIMenu = _mGlassesFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mHatsFMF")]
		private static UIMenu _mHatsFMF;

		public static UIMenu mHatsFMF
		{
			[CompilerGenerated]
			get
			{
				return _mHatsFMF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mHatsFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mHatsFMF = value;
				uIMenu = _mHatsFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mChainsFMF")]
		private static UIMenu _mChainsFMF;

		public static UIMenu mChainsFMF
		{
			[CompilerGenerated]
			get
			{
				return _mChainsFMF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mChainsFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mChainsFMF = value;
				uIMenu = _mChainsFMF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mCasualJacketTShirtM")]
		private static UIMenu _mCasualJacketTShirtM;

		public static UIMenu mCasualJacketTShirtM
		{
			[CompilerGenerated]
			get
			{
				return _mCasualJacketTShirtM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mCasualJacketTShirtM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mCasualJacketTShirtM = value;
				uIMenu = _mCasualJacketTShirtM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmGlassesM")]
		public static UIMenu gmGlassesM
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSportShadesM")]
		private static UIMenu _mSportShadesM;

		public static UIMenu mSportShadesM
		{
			[CompilerGenerated]
			get
			{
				return _mSportShadesM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSportShadesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSportShadesM = value;
				uIMenu = _mSportShadesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mStreetShadesM")]
		private static UIMenu _mStreetShadesM;

		public static UIMenu mStreetShadesM
		{
			[CompilerGenerated]
			get
			{
				return _mStreetShadesM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mStreetShadesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mStreetShadesM = value;
				uIMenu = _mStreetShadesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mHoodiesM")]
		private static UIMenu _mHoodiesM;

		public static UIMenu mHoodiesM
		{
			[CompilerGenerated]
			get
			{
				return _mHoodiesM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mHoodiesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mHoodiesM = value;
				uIMenu = _mHoodiesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mJacketsM")]
		private static UIMenu _mJacketsM;

		public static UIMenu mJacketsM
		{
			[CompilerGenerated]
			get
			{
				return _mJacketsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mJacketsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mJacketsM = value;
				uIMenu = _mJacketsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mPantsM")]
		private static UIMenu _mPantsM;

		public static UIMenu mPantsM
		{
			[CompilerGenerated]
			get
			{
				return _mPantsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mPantsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mPantsM = value;
				uIMenu = _mPantsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mCasualJacketTShirtF")]
		private static UIMenu _mCasualJacketTShirtF;

		public static UIMenu mCasualJacketTShirtF
		{
			[CompilerGenerated]
			get
			{
				return _mCasualJacketTShirtF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mCasualJacketTShirtF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mCasualJacketTShirtF = value;
				uIMenu = _mCasualJacketTShirtF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mEarringsF")]
		private static UIMenu _mEarringsF;

		public static UIMenu mEarringsF
		{
			[CompilerGenerated]
			get
			{
				return _mEarringsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mEarringsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mEarringsF = value;
				uIMenu = _mEarringsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmGlassesF")]
		public static UIMenu gmGlassesF
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSportShadesF")]
		private static UIMenu _mSportShadesF;

		public static UIMenu mSportShadesF
		{
			[CompilerGenerated]
			get
			{
				return _mSportShadesF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSportShadesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSportShadesF = value;
				uIMenu = _mSportShadesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mStreetShadesF")]
		private static UIMenu _mStreetShadesF;

		public static UIMenu mStreetShadesF
		{
			[CompilerGenerated]
			get
			{
				return _mStreetShadesF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mStreetShadesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mStreetShadesF = value;
				uIMenu = _mStreetShadesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmHatsF")]
		public static UIMenu gmHatsF
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mCapsForwardF")]
		private static UIMenu _mCapsForwardF;

		public static UIMenu mCapsForwardF
		{
			[CompilerGenerated]
			get
			{
				return _mCapsForwardF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mCapsForwardF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mCapsForwardF = value;
				uIMenu = _mCapsForwardF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mCapsBackwardF")]
		private static UIMenu _mCapsBackwardF;

		public static UIMenu mCapsBackwardF
		{
			[CompilerGenerated]
			get
			{
				return _mCapsBackwardF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mCapsBackwardF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mCapsBackwardF = value;
				uIMenu = _mCapsBackwardF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mHoodiesF")]
		private static UIMenu _mHoodiesF;

		public static UIMenu mHoodiesF
		{
			[CompilerGenerated]
			get
			{
				return _mHoodiesF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mHoodiesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mHoodiesF = value;
				uIMenu = _mHoodiesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mJacketsF")]
		private static UIMenu _mJacketsF;

		public static UIMenu mJacketsF
		{
			[CompilerGenerated]
			get
			{
				return _mJacketsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mJacketsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mJacketsF = value;
				uIMenu = _mJacketsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mPantsF")]
		private static UIMenu _mPantsF;

		public static UIMenu mPantsF
		{
			[CompilerGenerated]
			get
			{
				return _mPantsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mPantsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mPantsF = value;
				uIMenu = _mPantsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[field: AccessedThroughProperty("gmGlassesT")]
		public static UIMenu gmGlassesT
		{
			get; [MethodImpl(MethodImplOptions.Synchronized)]
			set;
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSportShadesT")]
		private static UIMenu _mSportShadesT;

		public static UIMenu mSportShadesT
		{
			[CompilerGenerated]
			get
			{
				return _mSportShadesT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSportShadesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSportShadesT = value;
				uIMenu = _mSportShadesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mStreetShadesT")]
		private static UIMenu _mStreetShadesT;

		public static UIMenu mStreetShadesT
		{
			[CompilerGenerated]
			get
			{
				return _mStreetShadesT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mStreetShadesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mStreetShadesT = value;
				uIMenu = _mStreetShadesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mCapsForwardT")]
		private static UIMenu _mCapsForwardT;

		public static UIMenu mCapsForwardT
		{
			[CompilerGenerated]
			get
			{
				return _mCapsForwardT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mCapsForwardT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mCapsForwardT = value;
				uIMenu = _mCapsForwardT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mHoodiesT")]
		private static UIMenu _mHoodiesT;

		public static UIMenu mHoodiesT
		{
			[CompilerGenerated]
			get
			{
				return _mHoodiesT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mHoodiesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mHoodiesT = value;
				uIMenu = _mHoodiesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mJacketsT")]
		private static UIMenu _mJacketsT;

		public static UIMenu mJacketsT
		{
			[CompilerGenerated]
			get
			{
				return _mJacketsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mJacketsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mJacketsT = value;
				uIMenu = _mJacketsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mPantsT")]
		private static UIMenu _mPantsT;

		public static UIMenu mPantsT
		{
			[CompilerGenerated]
			get
			{
				return _mPantsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mPantsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mPantsT = value;
				uIMenu = _mPantsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShoesM")]
		private static UIMenu _mShoesM;

		public static UIMenu mShoesM
		{
			[CompilerGenerated]
			get
			{
				return _mShoesM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShoesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShoesM = value;
				uIMenu = _mShoesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShirtsM")]
		private static UIMenu _mShirtsM;

		public static UIMenu mShirtsM
		{
			[CompilerGenerated]
			get
			{
				return _mShirtsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShirtsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShirtsM = value;
				uIMenu = _mShirtsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShortsM")]
		private static UIMenu _mShortsM;

		public static UIMenu mShortsM
		{
			[CompilerGenerated]
			get
			{
				return _mShortsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShortsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShortsM = value;
				uIMenu = _mShortsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTShirtM")]
		private static UIMenu _mTShirtM;

		public static UIMenu mTShirtM
		{
			[CompilerGenerated]
			get
			{
				return _mTShirtM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTShirtM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTShirtM = value;
				uIMenu = _mTShirtM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTankTopsM")]
		private static UIMenu _mTankTopsM;

		public static UIMenu mTankTopsM
		{
			[CompilerGenerated]
			get
			{
				return _mTankTopsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTankTopsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTankTopsM = value;
				uIMenu = _mTankTopsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTopsM")]
		private static UIMenu _mTopsM;

		public static UIMenu mTopsM
		{
			[CompilerGenerated]
			get
			{
				return _mTopsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTopsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTopsM = value;
				uIMenu = _mTopsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mGlassesM")]
		private static UIMenu _mGlassesM;

		public static UIMenu mGlassesM
		{
			[CompilerGenerated]
			get
			{
				return _mGlassesM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mGlassesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mGlassesM = value;
				uIMenu = _mGlassesM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mPoloShirtsM")]
		private static UIMenu _mPoloShirtsM;

		public static UIMenu mPoloShirtsM
		{
			[CompilerGenerated]
			get
			{
				return _mPoloShirtsM;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mPoloShirtsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mPoloShirtsM = value;
				uIMenu = _mPoloShirtsM;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShoesF")]
		private static UIMenu _mShoesF;

		public static UIMenu mShoesF
		{
			[CompilerGenerated]
			get
			{
				return _mShoesF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShoesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShoesF = value;
				uIMenu = _mShoesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShirtsF")]
		private static UIMenu _mShirtsF;

		public static UIMenu mShirtsF
		{
			[CompilerGenerated]
			get
			{
				return _mShirtsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShirtsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShirtsF = value;
				uIMenu = _mShirtsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShortsF")]
		private static UIMenu _mShortsF;

		public static UIMenu mShortsF
		{
			[CompilerGenerated]
			get
			{
				return _mShortsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShortsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShortsF = value;
				uIMenu = _mShortsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSmartShoesF")]
		private static UIMenu _mSmartShoesF;

		public static UIMenu mSmartShoesF
		{
			[CompilerGenerated]
			get
			{
				return _mSmartShoesF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSmartShoesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSmartShoesF = value;
				uIMenu = _mSmartShoesF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTShirtF")]
		private static UIMenu _mTShirtF;

		public static UIMenu mTShirtF
		{
			[CompilerGenerated]
			get
			{
				return _mTShirtF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTShirtF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTShirtF = value;
				uIMenu = _mTShirtF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTankTopsF")]
		private static UIMenu _mTankTopsF;

		public static UIMenu mTankTopsF
		{
			[CompilerGenerated]
			get
			{
				return _mTankTopsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTankTopsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTankTopsF = value;
				uIMenu = _mTankTopsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTopsF")]
		private static UIMenu _mTopsF;

		public static UIMenu mTopsF
		{
			[CompilerGenerated]
			get
			{
				return _mTopsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTopsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTopsF = value;
				uIMenu = _mTopsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mVestsF")]
		private static UIMenu _mVestsF;

		public static UIMenu mVestsF
		{
			[CompilerGenerated]
			get
			{
				return _mVestsF;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mVestsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mVestsF = value;
				uIMenu = _mVestsF;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShoesT")]
		private static UIMenu _mShoesT;

		public static UIMenu mShoesT
		{
			[CompilerGenerated]
			get
			{
				return _mShoesT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShoesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShoesT = value;
				uIMenu = _mShoesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShirtsT")]
		private static UIMenu _mShirtsT;

		public static UIMenu mShirtsT
		{
			[CompilerGenerated]
			get
			{
				return _mShirtsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShirtsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShirtsT = value;
				uIMenu = _mShirtsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mShortsT")]
		private static UIMenu _mShortsT;

		public static UIMenu mShortsT
		{
			[CompilerGenerated]
			get
			{
				return _mShortsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mShortsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mShortsT = value;
				uIMenu = _mShortsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mSmartShoesT")]
		private static UIMenu _mSmartShoesT;

		public static UIMenu mSmartShoesT
		{
			[CompilerGenerated]
			get
			{
				return _mSmartShoesT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mSmartShoesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mSmartShoesT = value;
				uIMenu = _mSmartShoesT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTShirtT")]
		private static UIMenu _mTShirtT;

		public static UIMenu mTShirtT
		{
			[CompilerGenerated]
			get
			{
				return _mTShirtT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTShirtT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTShirtT = value;
				uIMenu = _mTShirtT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTankTopsT")]
		private static UIMenu _mTankTopsT;

		public static UIMenu mTankTopsT
		{
			[CompilerGenerated]
			get
			{
				return _mTankTopsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTankTopsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTankTopsT = value;
				uIMenu = _mTankTopsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mTopsT")]
		private static UIMenu _mTopsT;

		public static UIMenu mTopsT
		{
			[CompilerGenerated]
			get
			{
				return _mTopsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mTopsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mTopsT = value;
				uIMenu = _mTopsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		[System.Runtime.CompilerServices.AccessedThroughProperty("mPoloShirtsT")]
		private static UIMenu _mPoloShirtsT;

		public static UIMenu mPoloShirtsT
		{
			[CompilerGenerated]
			get
			{
				return _mPoloShirtsT;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[CompilerGenerated]
			set
			{
				ItemSelectEvent value2 = SingleClothing_OnItemSelect;
				IndexChangedEvent value3 = SingleClothing_OnIndexChange;
				UIMenu uIMenu = _mPoloShirtsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect -= value2;
					uIMenu.OnIndexChange -= value3;
				}
				_mPoloShirtsT = value;
				uIMenu = _mPoloShirtsT;
				if (uIMenu != null)
				{
					uIMenu.OnItemSelect += value2;
					uIMenu.OnIndexChange += value3;
				}
			}
		}

		public static string ResolveWardrobeText(string label, string localized)
		{
			if (!string.IsNullOrWhiteSpace(localized) && Operators.CompareString(localized, "NULL", false) != 0)
			{
				return localized;
			}
			string text = label ?? "";
			if (!text.StartsWith("CSHOP_", StringComparison.OrdinalIgnoreCase))
			{
				return string.IsNullOrWhiteSpace(text) ? "Clothing" : text.Replace("_", " ");
			}
			switch (new string(text.Reverse().TakeWhile((_Closure_0024__._0024I387_002D0 != null) ? _Closure_0024__._0024I387_002D0 : (_Closure_0024__._0024I387_002D0 = _Closure_0024__._0024I._Lambda_0024__387_002D0)).Reverse()
				.ToArray()))
			{
			case "1":
				return "Wardrobe";
			case "2":
				return "Jackets";
			case "3":
			case "80":
				return "T-Shirts";
			case "4":
			case "72":
				return "Shirts";
			case "5":
				return "Hoodies";
			case "8":
				return "Polo shirts";
			case "36":
				return "Outfits";
			case "37":
				return "Tops";
			case "38":
				return "Pants";
			case "39":
			case "32":
				return "Shoes";
			case "40":
			case "131":
			case "132":
				return "Hats";
			case "42":
			case "70":
			case "71":
				return "Glasses";
			case "45":
				return "Tank tops";
			case "46":
			case "126":
				return "Suit jackets";
			case "47":
				return "Suit pants";
			case "49":
			case "125":
				return "Suits";
			case "50":
				return "Vests";
			case "51":
				return "Shorts / accessories";
			case "74":
				return "Earrings";
			case "79":
				return "Casual jackets";
			case "81":
				return "Ties";
			case "115":
				return "Chains";
			case "127":
				return "Caps forwards";
			case "128":
				return "Caps backwards";
			default:
				return string.IsNullOrWhiteSpace(text) ? "Clothing" : text.Replace("_", " ");
			}
		}

		public static string WardrobeText(string label)
		{
			return ResolveWardrobeText(label, Game.GetLocalizedString(label));
		}

		public static void LoadWardrobe()
		{
			try
			{
				Helper.RequestAdditionalText("clo_mnu", "CSHOP_TITLE1");
				iOutfits = new UIMenuItem(WardrobeText("CSHOP_ITEM36"))
				{
					Tag = "Outfit"
				};
				giSuits = new UIMenuItem(WardrobeText("CSHOP_ITEM49"))
				{
					Tag = "Suit"
				};
				iFullSuits = new UIMenuItem(WardrobeText("CSHOP_ITEM125"))
				{
					Tag = "FullSuit"
				};
				iSuitJackets = new UIMenuItem(WardrobeText("CSHOP_ITEM46"))
				{
					Tag = "SuitJacket"
				};
				iSuitJacketsButtoned = new UIMenuItem(WardrobeText("CSHOP_ITEM126"))
				{
					Tag = "SuitJacketButtoned"
				};
				iSuitPants = new UIMenuItem(WardrobeText("CSHOP_ITEM47"))
				{
					Tag = "SuitPant"
				};
				iSuitVests = new UIMenuItem(WardrobeText("CSHOP_ITEM50"))
				{
					Tag = "Vest"
				};
				iSuitTies = new UIMenuItem(WardrobeText("CSHOP_ITEM81"))
				{
					Tag = "Tie"
				};
				giCasualJackets = new UIMenuItem(WardrobeText("CSHOP_ITEM79"))
				{
					Tag = "CasualJacket"
				};
				iOpenShirts = new UIMenuItem(WardrobeText("CSHOP_ITEM72"))
				{
					Tag = "OpenShirt"
				};
				iCasualJackets = new UIMenuItem(WardrobeText("CSHOP_ITEM79"))
				{
					Tag = "CasualJacketJacket"
				};
				iCasualJacketShirts = new UIMenuItem(WardrobeText("CSHOP_ITEM4"))
				{
					Tag = "CasualJacketShirt"
				};
				iCasualJacketTShirts = new UIMenuItem(WardrobeText("CSHOP_ITEM80"))
				{
					Tag = "CasualJacketTShirt"
				};
				iEarrings = new UIMenuItem(WardrobeText("CSHOP_ITEM74"))
				{
					Tag = "Earrings"
				};
				iGlasses = new UIMenuItem(WardrobeText("CSHOP_ITEM42"))
				{
					Tag = "Glasses"
				};
				giGlasses = new UIMenuItem(WardrobeText("CSHOP_ITEM42"))
				{
					Tag = "Glasses"
				};
				iSportShades = new UIMenuItem(WardrobeText("CSHOP_ITEM70"))
				{
					Tag = "SportShades"
				};
				iStreetShades = new UIMenuItem(WardrobeText("CSHOP_ITEM71"))
				{
					Tag = "StreetShades"
				};
				giHats = new UIMenuItem(WardrobeText("CSHOP_ITEM40"))
				{
					Tag = "Hat"
				};
				iHats = new UIMenuItem(WardrobeText("CSHOP_ITEM40"))
				{
					Tag = "HatsTrevor"
				};
				iCapsForward = new UIMenuItem(WardrobeText("CSHOP_ITEM127"))
				{
					Tag = "CapForward"
				};
				iCapsBackward = new UIMenuItem(WardrobeText("CSHOP_ITEM128"))
				{
					Tag = "CapBackward"
				};
				iHoodies = new UIMenuItem(WardrobeText("CSHOP_ITEM5"))
				{
					Tag = "Hoodie"
				};
				iJackets = new UIMenuItem(WardrobeText("CSHOP_ITEM2"))
				{
					Tag = "Jacket"
				};
				iPants = new UIMenuItem(WardrobeText("CSHOP_ITEM38"))
				{
					Tag = "Pants"
				};
				iShoes = new UIMenuItem(WardrobeText("CSHOP_ITEM39"))
				{
					Tag = "Shoes"
				};
				iShirts = new UIMenuItem(WardrobeText("CSHOP_ITEM4"))
				{
					Tag = "Shirt"
				};
				iShorts = new UIMenuItem(WardrobeText("CSHOP_ITEM51"))
				{
					Tag = "Shorts"
				};
				iSmartShoes = new UIMenuItem(WardrobeText("CSHOP_ITEM32"))
				{
					Tag = "SmartShoes"
				};
				iTShirt = new UIMenuItem(WardrobeText("CSHOP_ITEM3"))
				{
					Tag = "TShirt"
				};
				iTankTops = new UIMenuItem(WardrobeText("CSHOP_ITEM45"))
				{
					Tag = "TankTop"
				};
				iTops = new UIMenuItem(WardrobeText("CSHOP_ITEM37"))
				{
					Tag = "Tops"
				};
				iVests = new UIMenuItem(WardrobeText("CSHOP_ITEM50"))
				{
					Tag = "Vest"
				};
				iPoloShirt = new UIMenuItem(WardrobeText("CSHOP_ITEM8"))
				{
					Tag = "PoloShirt"
				};
				iChains = new UIMenuItem(WardrobeText("CSHOP_ITEM115"))
				{
					Tag = "Chain"
				};
				CreateMichaelWardrobeMenu();
				mOutfitM = LoadSetClothings(mOutfitM, "CSHOP_TITLE36", gmMichael, iOutfits, WardrobeList1.Michael_Outfits, Helper.MichaelBanner);
				mFullSuitsM = LoadSetClothings(mFullSuitsM, "CSHOP_TITLE125", gmSuitM, iFullSuits, WardrobeList1.Michael_FullSuits, Helper.MichaelBanner);
				mSuitJacketsM = LoadSingleClothings(mSuitJacketsM, "CSHOP_TITLE46", gmSuitM, iSuitJackets, WardrobeList1.Michael_SuitJackets, Helper.MichaelBanner);
				mSuitPantsM = LoadSingleClothings(mSuitPantsM, "CSHOP_TITLE47", gmSuitM, iSuitPants, WardrobeList1.Michael_SuitPants, Helper.MichaelBanner);
				mSuitVestsM = LoadSingleClothings(mSuitVestsM, "CSHOP_TITLE50", gmSuitM, iSuitVests, WardrobeList1.Michael_SuitVests, Helper.MichaelBanner);
				mGlassesM = LoadSingleClothings(mGlassesM, "CSHOP_TITLE42", gmGlassesM, iGlasses, WardrobeList1.Michael_Glasses, Helper.MichaelBanner);
				mSportShadesM = LoadSingleClothings(mSportShadesM, "CSHOP_TITLE70", gmGlassesM, iSportShades, WardrobeList1.Michael_SportShades, Helper.MichaelBanner);
				mStreetShadesM = LoadSingleClothings(mStreetShadesM, "CSHOP_TITLE71", gmGlassesM, iStreetShades, WardrobeList1.Michael_StreetShades, Helper.MichaelBanner);
				mHoodiesM = LoadSingleClothings(mHoodiesM, "CSHOP_TITLE5", gmMichael, iHoodies, WardrobeList1.Michael_Hoodies, Helper.MichaelBanner);
				mJacketsM = LoadSingleClothings(mJacketsM, "CSHOP_TITLE2", gmMichael, iJackets, WardrobeList1.Michael_Jackets, Helper.MichaelBanner);
				mPantsM = LoadSingleClothings(mPantsM, "CSHOP_TITLE38", gmMichael, iPants, WardrobeList1.Michael_Pants, Helper.MichaelBanner);
				mPoloShirtsM = LoadSingleClothings(mPoloShirtsM, "CSHOP_TITLE8", gmMichael, iPoloShirt, WardrobeList1.Michael_Polos, Helper.MichaelBanner);
				mShoesM = LoadSingleClothings(mShoesM, "CSHOP_TITLE39", gmMichael, iShoes, WardrobeList1.Michael_Shoes, Helper.MichaelBanner);
				mCasualJacketM = LoadSingleClothings(mCasualJacketM, "CSHOP_TITLE79", gmCasualJacketsM, iCasualJackets, WardrobeList1.Michael_CasualJackets, Helper.MichaelBanner);
				mCasualJacketTShirtM = LoadSingleClothings(mCasualJacketTShirtM, "CSHOP_TITLE3", gmCasualJacketsM, iCasualJacketTShirts, WardrobeList1.Michael_CasualTShirts, Helper.MichaelBanner);
				mShirtsM = LoadSingleClothings(mShirtsM, "CSHOP_TITLE4", gmMichael, iShirts, WardrobeList1.Michael_Shirts, Helper.MichaelBanner);
				mShortsM = LoadSingleClothings(mShortsM, "CSHOP_TITLE51", gmMichael, iShorts, WardrobeList1.Michael_Shorts, Helper.MichaelBanner);
				mTShirtM = LoadSingleClothings(mTShirtM, "CSHOP_TITLE3", gmMichael, iTShirt, WardrobeList1.Michael_TShirts, Helper.MichaelBanner);
				mTankTopsM = LoadSingleClothings(mTankTopsM, "CSHOP_TITLE45", gmMichael, iTankTops, WardrobeList1.Michael_TankTops, Helper.MichaelBanner);
				mTopsM = LoadSingleClothings(mTopsM, "CSHOP_TITLE37", gmMichael, iTops, WardrobeList1.Michael_Tops, Helper.MichaelBanner);
				CreateFranklinWardrobeMenu();
				mOutfitF = LoadSetClothings(mOutfitF, "CSHOP_TITLE36", gmFranklin, iOutfits, WardrobeList1.Franklin_Outfits, Helper.FranklinBanner);
				mFullSuitsF = LoadSetClothings(mFullSuitsF, "CSHOP_TITLE125", gmSuitF, iFullSuits, WardrobeList1.Franklin_FullSuits, Helper.FranklinBanner);
				mSuitJacketsF = LoadSingleClothings(mSuitJacketsF, "CSHOP_TITLE46", gmSuitF, iSuitJackets, WardrobeList1.Franklin_SuitJackets, Helper.FranklinBanner);
				mSuitJacketsButtonedF = LoadSingleClothings(mSuitJacketsButtonedF, "CSHOP_TITLE126", gmSuitF, iSuitJacketsButtoned, WardrobeList1.Franklin_SuitJacketsButtoned, Helper.FranklinBanner);
				mSuitPantsF = LoadSingleClothings(mSuitPantsF, "CSHOP_TITLE47", gmSuitF, iSuitPants, WardrobeList1.Franklin_SuitPants, Helper.FranklinBanner);
				mSuitVestsF = LoadSingleClothings(mSuitVestsF, "CSHOP_TITLE50", gmSuitF, iSuitVests, WardrobeList1.Franklin_SuitVests, Helper.FranklinBanner);
				mSuitTiesF = LoadSingleClothings(mSuitTiesF, "CSHOP_TITLE81", gmSuitF, iSuitTies, WardrobeList1.Franklin_Ties, Helper.FranklinBanner);
				mCasualJacketF = LoadSingleClothings(mCasualJacketF, "CSHOP_TITLE79", gmCasualJacketsF, iCasualJackets, WardrobeList1.Franklin_CasualJackets, Helper.FranklinBanner);
				mCasualJacketTShirtF = LoadSingleClothings(mCasualJacketTShirtF, "CSHOP_TITLE3", gmCasualJacketsF, iCasualJacketTShirts, WardrobeList1.Franklin_CasualTShirts, Helper.FranklinBanner);
				mEarringsF = LoadSingleClothings(mEarringsF, "CSHOP_TITLE74", gmFranklin, iEarrings, WardrobeList1.Franklin_Earrings, Helper.FranklinBanner);
				mSportShadesF = LoadSingleClothings(mSportShadesF, "CSHOP_TITLE70", gmGlassesF, iSportShades, WardrobeList1.Franklin_SportShades, Helper.FranklinBanner);
				mStreetShadesF = LoadSingleClothings(mStreetShadesF, "CSHOP_TITLE71", gmGlassesF, iStreetShades, WardrobeList1.Franklin_StreetShades, Helper.FranklinBanner);
				mCapsForwardF = LoadSingleClothings(mCapsForwardF, "CSHOP_TITLE131", gmHatsF, iCapsForward, WardrobeList1.Franklin_CapsForward, Helper.FranklinBanner);
				mCapsBackwardF = LoadSingleClothings(mCapsBackwardF, "CSHOP_TITLE132", gmHatsF, iCapsBackward, WardrobeList1.Franklin_CapsBackward, Helper.FranklinBanner);
				mHoodiesF = LoadSingleClothings(mHoodiesF, "CSHOP_TITLE5", gmFranklin, iHoodies, WardrobeList1.Franklin_Hoodies, Helper.FranklinBanner);
				mJacketsF = LoadSingleClothings(mJacketsF, "CSHOP_TITLE2", gmFranklin, iJackets, WardrobeList1.Franklin_Jackets, Helper.FranklinBanner);
				mPantsF = LoadSingleClothings(mPantsF, "CSHOP_TITLE38", gmFranklin, iPants, WardrobeList1.Franklin_Pants, Helper.FranklinBanner);
				mShoesF = LoadSingleClothings(mShoesF, "CSHOP_TITLE39", gmFranklin, iShoes, WardrobeList1.Franklin_Shoes, Helper.FranklinBanner);
				mShirtsF = LoadSingleClothings(mShirtsF, "CSHOP_TITLE4", gmFranklin, iShirts, WardrobeList1.Franklin_Shirts, Helper.FranklinBanner);
				mShortsF = LoadSingleClothings(mShortsF, "CSHOP_TITLE51", gmFranklin, iShorts, WardrobeList1.Franklin_Shorts, Helper.FranklinBanner);
				mSmartShoesF = LoadSingleClothings(mSmartShoesF, "CSHOP_TITLE32", gmFranklin, iSmartShoes, WardrobeList1.Franklin_SmartShoes, Helper.FranklinBanner);
				mTShirtF = LoadSingleClothings(mTShirtF, "CSHOP_TITLE3", gmFranklin, iTShirt, WardrobeList1.Franklin_TShirts, Helper.FranklinBanner);
				mTankTopsF = LoadSingleClothings(mTankTopsF, "CSHOP_TITLE45", gmFranklin, iTankTops, WardrobeList1.Franklin_TankTops, Helper.FranklinBanner);
				mTopsF = LoadSingleClothings(mTopsF, "CSHOP_TITLE37", gmFranklin, iTops, WardrobeList1.Franklin_Tops, Helper.FranklinBanner);
				mVestsF = LoadSingleClothings(mVestsF, "CSHOP_TITLE57", gmFranklin, iVests, WardrobeList1.Franklin_Vests, Helper.FranklinBanner);
				CreateTrevorWardrobeMenu();
				mOutfitT = LoadSetClothings(mOutfitT, "CSHOP_TITLE36", gmTrevor, iOutfits, WardrobeList1.Trevor_Outfits, Helper.TrevorBanner);
				mFullSuitsT = LoadSetClothings(mFullSuitsT, "CSHOP_TITLE125", gmSuitT, iFullSuits, WardrobeList1.Trevor_FullSuits, Helper.TrevorBanner);
				mSuitJacketsT = LoadSingleClothings(mSuitJacketsT, "CSHOP_TITLE46", gmSuitT, iSuitJackets, WardrobeList1.Trevor_SuitJackets, Helper.TrevorBanner);
				mSuitPantsT = LoadSingleClothings(mSuitPantsT, "CSHOP_TITLE47", gmSuitT, iSuitPants, WardrobeList1.Trevor_SuitPants, Helper.TrevorBanner);
				mSportShadesT = LoadSingleClothings(mSportShadesT, "CSHOP_TITLE70", gmGlassesT, iSportShades, WardrobeList1.Trevor_SportShades, Helper.TrevorBanner);
				mStreetShadesT = LoadSingleClothings(mStreetShadesT, "CSHOP_TITLE71", gmGlassesT, iStreetShades, WardrobeList1.Trevor_StreetShades, Helper.TrevorBanner);
				mCapsForwardT = LoadSingleClothings(mCapsForwardT, "CSHOP_TITLE131", gmTrevor, iCapsForward, WardrobeList1.Trevor_CapsForward, Helper.TrevorBanner);
				mHoodiesT = LoadSingleClothings(mHoodiesT, "CSHOP_TITLE5", gmTrevor, iHoodies, WardrobeList1.Trevor_Hoodies, Helper.TrevorBanner);
				mJacketsT = LoadSingleClothings(mJacketsT, "CSHOP_TITLE2", gmTrevor, iJackets, WardrobeList1.Trevor_Jackets, Helper.TrevorBanner);
				mPantsT = LoadSingleClothings(mPantsT, "CSHOP_TITLE38", gmTrevor, iPants, WardrobeList1.Trevor_Pants, Helper.TrevorBanner);
				mPoloShirtsT = LoadSingleClothings(mPoloShirtsT, "CSHOP_TITLE8", gmTrevor, iPoloShirt, WardrobeList1.Trevor_Polos, Helper.TrevorBanner);
				mShoesT = LoadSingleClothings(mShoesT, "CSHOP_TITLE39", gmTrevor, iShoes, WardrobeList1.Trevor_Shoes, Helper.TrevorBanner);
				mShirtsT = LoadSingleClothings(mShirtsT, "CSHOP_TITLE4", gmTrevor, iShirts, WardrobeList1.Trevor_Shirts, Helper.TrevorBanner);
				mShortsT = LoadSingleClothings(mShortsT, "CSHOP_TITLE51", gmTrevor, iShorts, WardrobeList1.Trevor_Shorts, Helper.TrevorBanner);
				mSmartShoesT = LoadSingleClothings(mSmartShoesT, "CSHOP_TITLE32", gmTrevor, iSmartShoes, WardrobeList1.Trevor_SmartShoes, Helper.TrevorBanner);
				mTShirtT = LoadSingleClothings(mTShirtT, "CSHOP_TITLE3", gmTrevor, iTShirt, WardrobeList1.Trevor_TShirts, Helper.TrevorBanner);
				mTankTopsT = LoadSingleClothings(mTankTopsT, "CSHOP_TITLE45", gmTrevor, iTankTops, WardrobeList1.Trevor_TankTops, Helper.TrevorBanner);
				mTopsT = LoadSingleClothings(mTopsT, "CSHOP_TITLE37", gmTrevor, iTops, WardrobeList1.Trevor_Tops, Helper.TrevorBanner);
				gmFMM = CreateFreemodeWardrobeMenu(gmFMM);
				mOutfitFMM = LoadSetClothings(mOutfitFMM, "CSHOP_TITLE36", gmFMM, iOutfits, WardrobeList2.FreemodeMale_Outfits);
				mGlassesFMM = LoadSingleClothings(mGlassesFMM, "CSHOP_TITLE42", gmFMM, iGlasses, WardrobeList2.FreemodeMale_Glasses);
				mEarringsFMM = LoadSingleClothings(mEarringsFMM, "CSHOP_TITLE74", gmFMM, iEarrings, WardrobeList2.FreemodeMale_Earrings);
				mHatsFMM = LoadSingleClothings(mHatsFMM, "CSHOP_TITLE40", gmFMM, iHats, WardrobeList2.FreemodeMale_Hats);
				mChainsFMM = LoadSingleClothings(mChainsFMM, "CSHOP_TITLE115", gmFMM, iChains, WardrobeList2.FreemodeMale_Chains);
				gmFMF = CreateFreemodeWardrobeMenu(gmFMF);
				mOutfitFMF = LoadSetClothings(mOutfitFMF, "CSHOP_TITLE36", gmFMF, iOutfits, WardrobeList2.FreemodeFemale_Outfits);
				mGlassesFMF = LoadSingleClothings(mGlassesFMF, "CSHOP_TITLE42", gmFMF, iGlasses, WardrobeList2.FreemodeFemale_Glasses);
				mEarringsFMF = LoadSingleClothings(mEarringsFMF, "CSHOP_TITLE74", gmFMF, iEarrings, WardrobeList2.FreemodeFemale_Earrings);
				mHatsFMF = LoadSingleClothings(mHatsFMF, "CSHOP_TITLE40", gmFMF, iHats, WardrobeList2.FreemodeFemale_Hats);
				mChainsFMF = LoadSingleClothings(mChainsFMF, "CSHOP_TITLE115", gmFMF, iChains, WardrobeList2.FreemodeFemale_Chains);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static Vector3[] ApartmentCameraCandidates(Vector3 position, float heading)
		{
			List<Vector3> list = new List<Vector3>();
			if (!PartyPlacement.FinitePoint(position) || float.IsNaN(heading) || float.IsInfinity(heading))
			{
				return list.ToArray();
			}
			float[] array = new float[3] { 1.35f, 1.75f, 2.2f };
			foreach (float num in array)
			{
				float[] array2 = new float[8] { 0f, 45f, -45f, 90f, -90f, 135f, -135f, 180f };
				foreach (float num2 in array2)
				{
					double num3 = ((double)heading + (double)num2) * Math.PI / 180.0;
					list.Add(position + new Vector3((0f - (float)Math.Sin(num3)) * num, (float)Math.Cos(num3) * num, 0.15f));
				}
			}
			return list.ToArray();
		}

		private static bool WardrobeActorMatches(Ped actor, int model, IntPtr address)
		{
			if (address != IntPtr.Zero && (object)actor != null && actor.Exists() && actor.Model.Hash == model)
			{
				return actor.MemoryAddress == address;
			}
			return false;
		}

		public static bool PreviewPoseAllowed(Vector3 origin, Vector3 actorPosition, Vector3 requestedEye, Vector3 actualEye)
		{
			if (!HomeActionPose.Near(actorPosition, origin, 0.75f) || !HomeActionPose.Near(requestedEye, origin, 2.3f) || !HomeActionPose.Near(actualEye, requestedEye, 0.1f))
			{
				return false;
			}
			Vector3 vector = requestedEye - origin;
			return vector.X * vector.X + vector.Y * vector.Y >= 1.44f && Math.Abs(vector.Z) <= 0.5f;
		}

		private static bool ApartmentPoseValid()
		{
			if (!SameApartmentWardrobeActor() || (object)apartmentCamera == null || !apartmentCamera.Exists())
			{
				return false;
			}
			return PreviewPoseAllowed(apartmentOrigin, HomeActionPose.Position(apartmentActor), apartmentEye, Function.Call<Vector3>(Hash.GET_CAM_COORD, apartmentCamera.Handle)) && Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, apartmentActor.Handle) == apartmentInterior;
		}

		private static bool ClearCameraSpace(Vector3 origin, Vector3 candidate, Ped actor)
		{
			Vector3[] array = new Vector3[7]
			{
				Vector3.Zero,
				new Vector3(0.18f, 0f, 0f),
				new Vector3(-0.18f, 0f, 0f),
				new Vector3(0f, 0.18f, 0f),
				new Vector3(0f, -0.18f, 0f),
				new Vector3(0f, 0f, 0.18f),
				new Vector3(0f, 0f, -0.18f)
			};
			bool hit = default(bool);
			Vector3 point = default(Vector3);
			Vector3 normal = default(Vector3);
			foreach (Vector3 vector in array)
			{
				if (PartyPlacement.PhysicalRay(origin, candidate + vector, 81, ref hit, ref point, ref normal, actor) != 2 || hit)
				{
					return false;
				}
			}
			return true;
		}

		private static void CaptureApartmentPose(Ped actor)
		{
			apartmentOrigin = HomeActionPose.Position(actor);
			apartmentInterior = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle);
			apartmentActorHeading = HomeActionPose.Heading(actor);
			apartmentWasFrozen = actor.IsPositionFrozen;
			apartmentChangedFreeze = false;
			apartmentIdleStarted = false;
			apartmentChangedHeading = false;
		}

		private static void HoldWardrobeActor()
		{
			apartmentChangedFreeze = true;
			Function.Call(Hash.FREEZE_ENTITY_POSITION, apartmentActor.Handle, true);
			apartmentIdleStarted = true;
			Function.Call(Hash.TASK_STAND_STILL, apartmentActor.Handle, -1);
		}

		public static int OpeningDecision(bool ownView, bool foreignView, bool stable, long elapsed)
		{
			if (!stable || foreignView || elapsed < 0 || elapsed > 1200)
			{
				return 2;
			}
			if (ownView)
			{
				return 1;
			}
			return 0;
		}

		public static bool MayReleaseRenderingCamera(bool assigned, bool ownedView, bool foreignView)
		{
			if (!foreignView)
			{
				if (!assigned)
				{
					return ownedView;
				}
				return true;
			}
			return false;
		}

		private static void BeginApartmentView()
		{
			apartmentOpening = Stopwatch.StartNew();
			apartmentRenderingAssigned = true;
			Camera = apartmentCamera;
			World.RenderingCamera = apartmentCamera;
		}

		private static bool TryActivateApartmentView()
		{
			if (apartmentOpening == null)
			{
				return HasApartmentSession();
			}
			Camera renderingCamera = World.RenderingCamera;
			bool flag = (object)renderingCamera != null && renderingCamera.Handle != 0 && renderingCamera.Exists();
			bool flag2 = flag && (object)apartmentCamera != null && renderingCamera.Handle == apartmentCamera.Handle;
			bool stable = SameApartmentWardrobeActor() && (object)Helper.PP != null && Helper.PP.Handle == apartmentActor.Handle && ((apartmentMansion != null) ? MansionWardrobe.SessionInHome(apartmentMansion) : ApartmentHomeServices.InHome(apartmentHome)) && ApartmentPoseValid() && !Helper.MenuPool.IsAnyMenuOpen() && !Game.IsControlJustPressed(Control.FrontendCancel) && !Game.IsControlJustPressed(Control.PhoneCancel);
			switch (OpeningDecision(flag2, flag && !flag2, stable, apartmentOpening.ElapsedMilliseconds))
			{
			case 1:
				apartmentOpening = null;
				Helper.HideHud = true;
				apartmentRoot.Visible = true;
				Logger.Log("Wardrobe local camera and menu confirmed; actor=" + apartmentOrigin.ToString() + "; camera=" + apartmentEye.ToString());
				return true;
			case 2:
				Logger.Log("Wardrobe activation cancelled or timed out; rendering=" + (((object)renderingCamera == null) ? "none" : renderingCamera.Handle.ToString()));
				CleanupApartment();
				return false;
			default:
				return false;
			}
		}

		private static bool SameApartmentWardrobeActor()
		{
			return WardrobeActorMatches(apartmentActor, apartmentActorModel, apartmentActorAddress);
		}

		private static bool HasApartmentSession()
		{
			if (apartmentHome == null && apartmentMansion == null && (object)apartmentActor == null && apartmentMemory == null && (object)apartmentCamera == null)
			{
				return apartmentRoot != null;
			}
			return true;
		}

		public static bool ApartmentInputMayProceed(bool hasSession, bool sameActor, bool currentActor, bool ownMemory, bool sameHome, bool ownCamera, bool sameRenderingCamera, bool treeVisible)
		{
			if (hasSession)
			{
				if (sameActor && currentActor && ownMemory && sameHome && ownCamera && sameRenderingCamera)
				{
					return treeVisible;
				}
				return false;
			}
			return true;
		}

		private static bool ApartmentInputReady()
		{
			if (apartmentClosing)
			{
				return false;
			}
			if (!HasApartmentSession())
			{
				return true;
			}
			try
			{
				Camera renderingCamera = World.RenderingCamera;
				if (ApartmentInputMayProceed(true, SameApartmentWardrobeActor(), (object)Helper.PP != null && (object)apartmentActor != null && Helper.PP.Handle == apartmentActor.Handle, apartmentMemory != null && Memory == apartmentMemory, (object)apartmentActor != null && !apartmentActor.IsDead && ((apartmentMansion != null) ? MansionWardrobe.SessionInHome(apartmentMansion) : ApartmentHomeServices.InHome(apartmentHome)), (object)apartmentCamera != null && apartmentCamera.Exists() && Stability.IsSpaCamera(apartmentCamera) && ApartmentPoseValid(), (object)renderingCamera != null && renderingCamera.Exists() && (object)apartmentCamera != null && renderingCamera.Handle == apartmentCamera.Handle, WardrobeTreeVisible(apartmentRoot)))
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Wardrobe session validation: " + ex2.Message);
				ProjectData.ClearProjectError();
			}
			Logger.Log("Wardrobe session closed: captured actor/home/camera/menu no longer valid.");
			CleanupApartment();
			return false;
		}

		public static void RunApartmentCleanup(params Action[] actions)
		{
			if (actions == null)
			{
				return;
			}
			foreach (Action action in actions)
			{
				if (action != null)
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
			}
		}

		private static UIMenu RootForCurrentActor()
		{
			switch (Helper.GetPlayer())
			{
			case eOwner.Michael:
				return gmMichael;
			case eOwner.Franklin:
				return gmFranklin;
			case eOwner.Trevor:
				return gmTrevor;
			case eOwner.Others:
				return (Helper.PP.Gender == Gender.Male) ? gmFMM : gmFMF;
			default:
				return null;
			}
		}

		private static bool WardrobeTreeVisible(UIMenu root, bool close = false)
		{
			if (root == null)
			{
				return false;
			}
			Stack<UIMenu> stack = new Stack<UIMenu>();
			HashSet<UIMenu> hashSet = new HashSet<UIMenu>();
			stack.Push(root);
			bool flag = default(bool);
			while (stack.Count > 0)
			{
				UIMenu uIMenu = stack.Pop();
				if (uIMenu == null || !hashSet.Add(uIMenu))
				{
					continue;
				}
				flag = flag || uIMenu.Visible;
				if (close && uIMenu.Visible)
				{
					uIMenu.Visible = false;
				}
				foreach (UIMenu value in uIMenu.Children.Values)
				{
					stack.Push(value);
				}
			}
			return flag;
		}

		public static bool OpenApartment(ApartmentClass home)
		{
			if (HasApartmentSession() || home == null || !ApartmentHomeServices.InHome(home) || (object)Helper.PP == null || !Helper.PP.Exists() || Helper.PP.IsDead || Helper.PP.IsInVehicle() || Helper.PP.IsFalling || Helper.PP.IsRagdoll || DestinationStreaming.IsPlayerSafetyHeld(Helper.PP) || !ApartmentHomeServices.TriggerNear(HomeActionPose.Position(Helper.PP), Helper.ToVector3(home.WardrobePos), 3f))
			{
				return false;
			}
			WardrobeMemory memory = Memory;
			Camera camera = Camera;
			bool flag2 = default(bool);
			bool flag3 = default(bool);
			try
			{
				Camera renderingCamera = World.RenderingCamera;
				if ((object)renderingCamera != null && renderingCamera.Handle != 0 && renderingCamera.Exists())
				{
					return false;
				}
				if (!Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Game.Player.Handle))
				{
					return false;
				}
				Ped pP = Helper.PP;
				float w = home.WardrobePos.W;
				Vector3 vector = HomeActionPose.Position(pP);
				Vector3[] array = ApartmentCameraCandidates(vector, w);
				Vector3 pos = default(Vector3);
				bool flag = default(bool);
				foreach (Vector3 vector2 in array)
				{
					if (ClearCameraSpace(vector, vector2, pP))
					{
						pos = vector2;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Notification.Show("Move a little away from the wardrobe furniture and try again.");
					return false;
				}
				CleanupApartment();
				flag2 = true;
				apartmentHome = home;
				apartmentActor = pP;
				apartmentActorAddress = pP.MemoryAddress;
				apartmentActorModel = pP.Model.Hash;
				CaptureApartmentPose(pP);
				apartmentEye = pos;
				apartmentRoot = RootForCurrentActor();
				if (apartmentRoot == null)
				{
					return false;
				}
				apartmentMemory = new WardrobeMemory(pP);
				Memory = apartmentMemory;
				RefreshWardrobeMenus();
				if (!SameApartmentWardrobeActor() || (object)Helper.PP == null || Helper.PP.Handle != pP.Handle)
				{
					return false;
				}
				float num = pos.DistanceTo(apartmentOrigin);
				float fov = ((num < 1.6f) ? 70f : ((num < 2f) ? 60f : 50f));
				apartmentCamera = Stability.CreateCam(pos, GameplayCamera.Rotation, fov);
				if ((object)apartmentCamera == null || !apartmentCamera.Exists())
				{
					return false;
				}
				if (!ApartmentPoseValid())
				{
					throw new InvalidOperationException("Wardrobe camera did not accept its local coordinates.");
				}
				Function.Call(Hash.SET_CAM_NEAR_CLIP, apartmentCamera.Handle, 0.08f);
				apartmentCamera.PointAt(apartmentOrigin);
				apartmentChangedHeading = true;
				Function.Call(Hash.SET_ENTITY_HEADING, pP.Handle, w);
				HoldWardrobeActor();
				BeginApartmentView();
				flag3 = (TryActivateApartmentView() ? ApartmentInputReady() : HasApartmentSession());
				if (!flag3)
				{
					return false;
				}
				Logger.Log("Apartment wardrobe opened: " + home.Name + "; actor=" + apartmentOrigin.ToString() + "; camera=" + apartmentEye.ToString());
				return true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				try
				{
					Logger.Log("Apartment wardrobe: " + ex2.Message);
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if (flag2 && !flag3)
				{
					WardrobeMemory wardrobeMemory = apartmentMemory;
					Camera camera2 = apartmentCamera;
					try
					{
						CleanupApartment();
					}
					catch (Exception projectError2)
					{
						ProjectData.SetProjectError(projectError2);
						ProjectData.ClearProjectError();
					}
					finally
					{
						if (wardrobeMemory != null && (Memory == wardrobeMemory || Memory == null))
						{
							Memory = memory;
						}
						if ((object)camera2 != null && ((object)Camera == camera2 || (object)Camera == null))
						{
							Camera = camera;
						}
					}
				}
			}
		}

		public static bool OpenMansion(Mansion m)
		{
			if (HasApartmentSession() || !MansionWardrobe.SessionInHome(m) || MansionBarber.IsBusy || PartyService.IsPlayerActionActive || MansionFeatures.IsWorkoutActive || PartyPlayerActions.IsKissing)
			{
				return false;
			}
			bool flag2 = default(bool);
			bool flag3 = default(bool);
			try
			{
				Camera renderingCamera = World.RenderingCamera;
				if ((object)renderingCamera != null && renderingCamera.Handle != 0 && renderingCamera.Exists())
				{
					return false;
				}
				if (!Function.Call<bool>(Hash.IS_PLAYER_CONTROL_ON, Game.Player.Handle))
				{
					return false;
				}
				CleanupApartment();
				Ped pP = Helper.PP;
				Vector3 vector = HomeActionPose.Position(pP);
				float heading = HomeActionPose.Heading(pP);
				Vector3[] array = ApartmentCameraCandidates(vector, heading);
				Vector3 pos = default(Vector3);
				bool flag = default(bool);
				foreach (Vector3 vector2 in array)
				{
					if (ClearCameraSpace(vector, vector2, pP))
					{
						pos = vector2;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Notification.Show("Move a little away from the wardrobe furniture and try again.");
					return false;
				}
				flag2 = true;
				apartmentMansion = m;
				apartmentActor = pP;
				apartmentActorAddress = pP.MemoryAddress;
				apartmentActorModel = pP.Model.Hash;
				CaptureApartmentPose(pP);
				apartmentEye = pos;
				apartmentRoot = RootForCurrentActor();
				if (apartmentRoot == null)
				{
					return false;
				}
				apartmentMemory = new WardrobeMemory(pP);
				Memory = apartmentMemory;
				RefreshWardrobeMenus();
				if (!SameApartmentWardrobeActor() || (object)Helper.PP == null || Helper.PP.Handle != pP.Handle || !MansionWardrobe.SessionInHome(m))
				{
					return false;
				}
				apartmentCamera = Stability.CreateCam(pos, GameplayCamera.Rotation, 65f);
				if ((object)apartmentCamera == null || !apartmentCamera.Exists())
				{
					return false;
				}
				if (!ApartmentPoseValid())
				{
					throw new InvalidOperationException("Wardrobe camera did not accept its local coordinates.");
				}
				Function.Call(Hash.SET_CAM_NEAR_CLIP, apartmentCamera.Handle, 0.08f);
				apartmentCamera.PointAt(apartmentOrigin);
				HoldWardrobeActor();
				BeginApartmentView();
				flag3 = (TryActivateApartmentView() ? ApartmentInputReady() : HasApartmentSession());
				if (flag3)
				{
					Logger.Log("Mansion wardrobe opened: " + m.Name + "; actor=" + apartmentOrigin.ToString() + "; camera=" + apartmentEye.ToString());
				}
				return flag3;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log("Mansion wardrobe approach: " + ex2.Message);
				Notification.Show("The wardrobe view could not open safely. Move away from the furniture and try again.");
				bool result = false;
				ProjectData.ClearProjectError();
				return result;
			}
			finally
			{
				if (flag2 && !flag3)
				{
					CleanupApartment();
				}
			}
		}

		public static void ApartmentFrameTick()
		{
			if (!HasApartmentSession())
			{
				return;
			}
			if (apartmentOpening != null && !TryActivateApartmentView())
			{
				if (HasApartmentSession())
				{
					Game.DisableControlThisFrame(Control.Context);
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 51, true);
				}
			}
			else if (ApartmentInputReady())
			{
				Game.DisableControlThisFrame(Control.Context);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 51, true);
			}
		}

		public static void ForgetForStoryLoad()
		{
			if ((object)apartmentCamera != null && (object)Camera == apartmentCamera)
			{
				Camera = null;
			}
			if (apartmentMemory != null && Memory == apartmentMemory)
			{
				Memory = null;
			}
			apartmentHome = null;
			apartmentMansion = null;
			apartmentActor = null;
			apartmentActorAddress = IntPtr.Zero;
			apartmentActorModel = 0;
			apartmentActorHeading = 0f;
			apartmentCamera = null;
			apartmentMemory = null;
			apartmentRoot = null;
			apartmentOrigin = default(Vector3);
			apartmentEye = default(Vector3);
			apartmentInterior = 0;
			apartmentWasFrozen = false;
			apartmentChangedFreeze = false;
			apartmentIdleStarted = false;
			apartmentChangedHeading = false;
			apartmentOpening = null;
			apartmentRenderingAssigned = false;
			apartmentClosing = false;
		}

		public static void CleanupApartment()
		{
			_Closure_0024__431_002D0 closure_0024__431_002D = new _Closure_0024__431_002D0();
			if (!apartmentClosing && HasApartmentSession())
			{
				apartmentClosing = true;
				closure_0024__431_002D._0024VB_0024Local_actor = apartmentActor;
				closure_0024__431_002D._0024VB_0024Local_original = apartmentMemory;
				closure_0024__431_002D._0024VB_0024Local_view = apartmentCamera;
				closure_0024__431_002D._0024VB_0024Local_root = apartmentRoot;
				closure_0024__431_002D._0024VB_0024Local_model = apartmentActorModel;
				closure_0024__431_002D._0024VB_0024Local_address = apartmentActorAddress;
				closure_0024__431_002D._0024VB_0024Local_heading = apartmentActorHeading;
				closure_0024__431_002D._0024VB_0024Local_changedFreeze = apartmentChangedFreeze;
				closure_0024__431_002D._0024VB_0024Local_wasFrozen = apartmentWasFrozen;
				closure_0024__431_002D._0024VB_0024Local_idleStarted = apartmentIdleStarted;
				closure_0024__431_002D._0024VB_0024Local_changedHeading = apartmentChangedHeading;
				closure_0024__431_002D._0024VB_0024Local_renderingAssigned = apartmentRenderingAssigned;
				apartmentHome = null;
				apartmentMansion = null;
				apartmentActor = null;
				apartmentMemory = null;
				apartmentCamera = null;
				apartmentRoot = null;
				apartmentChangedFreeze = false;
				apartmentIdleStarted = false;
				apartmentChangedHeading = false;
				apartmentOpening = null;
				apartmentRenderingAssigned = false;
				try
				{
					RunApartmentCleanup(closure_0024__431_002D._Lambda_0024__0, closure_0024__431_002D._Lambda_0024__1, closure_0024__431_002D._Lambda_0024__2, closure_0024__431_002D._Lambda_0024__3, closure_0024__431_002D._Lambda_0024__4, closure_0024__431_002D._Lambda_0024__5, closure_0024__431_002D._Lambda_0024__6, closure_0024__431_002D._Lambda_0024__7, closure_0024__431_002D._Lambda_0024__8);
				}
				finally
				{
					apartmentClosing = false;
				}
			}
		}

		private static void RestoreMemory(Ped actor, WardrobeMemory original, int model, IntPtr address)
		{
			_Closure_0024__432_002D1 arg = new _Closure_0024__432_002D1(null);
			arg._0024VB_0024Local_actor = actor;
			arg._0024VB_0024Local_model = model;
			arg._0024VB_0024Local_address = address;
			ClothingSet.CS[] array = new ClothingSet.CS[12]
			{
				original.Accessories, original.AuxiliaryTorso, original.Beard, original.Eyes, original.Face, original.Feet, original.Hair, original.Hands, original.Legs, original.Props,
				original.Textures, original.Torso
			};
			_Closure_0024__432_002D0 closure_0024__432_002D = default(_Closure_0024__432_002D0);
			foreach (ClothingSet.CS _0024VB_0024Local_captured in array)
			{
				closure_0024__432_002D = new _Closure_0024__432_002D0(closure_0024__432_002D);
				closure_0024__432_002D._0024VB_0024NonLocal__0024VB_0024Closure_2 = arg;
				closure_0024__432_002D._0024VB_0024Local_captured = _0024VB_0024Local_captured;
				RunApartmentCleanup(closure_0024__432_002D._Lambda_0024__0);
			}
			ClothingSet.CS[] array2 = new ClothingSet.CS[3] { original.Earrings, original.Glasses, original.Helmet };
			_Closure_0024__432_002D2 closure_0024__432_002D2 = default(_Closure_0024__432_002D2);
			foreach (ClothingSet.CS _0024VB_0024Local_captured2 in array2)
			{
				closure_0024__432_002D2 = new _Closure_0024__432_002D2(closure_0024__432_002D2);
				closure_0024__432_002D2._0024VB_0024NonLocal__0024VB_0024Closure_3 = arg;
				closure_0024__432_002D2._0024VB_0024Local_captured = _0024VB_0024Local_captured2;
				RunApartmentCleanup(closure_0024__432_002D2._Lambda_0024__1);
			}
		}

		public static void MakeCamera(Ped ped, Vector3 position, float heading, bool positionActor = true, Vector3? cameraPosition = null)
		{
			try
			{
				Memory = new WardrobeMemory(ped);
				if (apartmentHome == null)
				{
					Helper.RequestAdditionalText("clo_mnu", "CSHOP_TITLE1");
				}
				RefreshWardrobeMenus();
				if (positionActor)
				{
					ped.Position = position;
				}
				ped.Heading = heading;
				Vector3 pos = cameraPosition ?? (positionActor ? (ped.Position + ped.ForwardVector * 5f) : MansionWardrobe.CameraPoint(ped));
				float fov = (positionActor ? 30f : 50f);
				if (cameraPosition.HasValue)
				{
					float num = pos.DistanceTo(ped.Position);
					fov = ((num < 1.6f) ? 70f : ((num < 2f) ? 60f : 50f));
				}
				Camera = Stability.CreateCam(pos, GameplayCamera.Rotation, fov);
				Camera.PointAt(ped);
				World.RenderingCamera = Camera;
				V2Compat.StartScenario(ped.Task, "PROP_HUMAN_STAND_IMPATIENT");
				Helper.HideHud = true;
				switch (Helper.GetPlayer())
				{
				case eOwner.Michael:
					gmMichael.Visible = true;
					break;
				case eOwner.Franklin:
					gmFranklin.Visible = true;
					break;
				case eOwner.Trevor:
					gmTrevor.Visible = true;
					break;
				case eOwner.Others:
					switch (ped.Gender)
					{
					case Gender.Male:
						gmFMM.Visible = true;
						break;
					case Gender.Female:
						gmFMF.Visible = true;
						break;
					}
					break;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void RefreshWardrobeMenus()
		{
			try
			{
				switch (Helper.GetPlayer())
				{
				case eOwner.Michael:
					RefreshSingle(mSuitJacketsM);
					RefreshSingle(mSuitPantsM);
					RefreshSingle(mSuitVestsM);
					RefreshSingle(mGlassesM);
					RefreshSingle(mSportShadesM);
					RefreshSingle(mStreetShadesM);
					RefreshSingle(mHoodiesM);
					RefreshSingle(mJacketsM);
					RefreshSingle(mPantsM);
					RefreshSingle(mPoloShirtsM);
					RefreshSingle(mShoesM);
					RefreshSingle(mCasualJacketM);
					RefreshSingle(mCasualJacketTShirtM);
					RefreshSingle(mShirtsM);
					RefreshSingle(mShortsM);
					RefreshSingle(mTShirtM);
					RefreshSingle(mTankTopsM);
					RefreshSingle(mTopsM);
					break;
				case eOwner.Franklin:
					RefreshSingle(mSuitJacketsF);
					RefreshSingle(mSuitJacketsButtonedF);
					RefreshSingle(mSuitPantsF);
					RefreshSingle(mSuitVestsF);
					RefreshSingle(mSuitTiesF);
					RefreshSingle(mCasualJacketF);
					RefreshSingle(mCasualJacketTShirtF);
					RefreshSingle(mEarringsF);
					RefreshSingle(mSportShadesF);
					RefreshSingle(mStreetShadesF);
					RefreshSingle(mCapsForwardF);
					RefreshSingle(mCapsBackwardF);
					RefreshSingle(mHoodiesF);
					RefreshSingle(mJacketsF);
					RefreshSingle(mPantsF);
					RefreshSingle(mShoesF);
					RefreshSingle(mShirtsF);
					RefreshSingle(mShortsF);
					RefreshSingle(mSmartShoesF);
					RefreshSingle(mTShirtF);
					RefreshSingle(mTankTopsF);
					RefreshSingle(mTopsF);
					RefreshSingle(mVestsF);
					break;
				case eOwner.Trevor:
					RefreshSingle(mSuitJacketsT);
					RefreshSingle(mSuitPantsT);
					RefreshSingle(mSportShadesT);
					RefreshSingle(mStreetShadesT);
					RefreshSingle(mCapsForwardT);
					RefreshSingle(mHoodiesT);
					RefreshSingle(mJacketsT);
					RefreshSingle(mPantsT);
					RefreshSingle(mPoloShirtsT);
					RefreshSingle(mShoesT);
					RefreshSingle(mShirtsT);
					RefreshSingle(mShortsT);
					RefreshSingle(mSmartShoesT);
					RefreshSingle(mTShirtT);
					RefreshSingle(mTankTopsT);
					RefreshSingle(mTopsT);
					break;
				case eOwner.Others:
					switch (Helper.PP.Gender)
					{
					case Gender.Male:
						RefreshSingle(mGlassesFMM);
						RefreshSingle(mEarringsFMM);
						RefreshSingle(mHatsFMM);
						RefreshSingle(mChainsFMM);
						break;
					case Gender.Female:
						RefreshSingle(mGlassesFMF);
						RefreshSingle(mEarringsFMF);
						RefreshSingle(mHatsFMF);
						RefreshSingle(mChainsFMF);
						break;
					}
					break;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void CreateMichaelWardrobeMenu()
		{
			try
			{
				Helper.RequestAdditionalText("clo_mnu", "CSHOP_TITLE1");
				gmMichael = new UIMenu("", WardrobeText("CSHOP_TITLE1"));
				UIMenu uIMenu = gmMichael;
				uIMenu.SetBannerType(Helper.MichaelBanner);
				uIMenu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmMichael);
				uIMenu.AddItem(iOutfits);
				uIMenu.AddItem(giSuits);
				uIMenu.AddItem(giGlasses);
				uIMenu.AddItem(iHoodies);
				uIMenu.AddItem(iJackets);
				uIMenu.AddItem(giCasualJackets);
				uIMenu.AddItem(iPants);
				uIMenu.AddItem(iPoloShirt);
				uIMenu.AddItem(iShoes);
				uIMenu.AddItem(iShirts);
				uIMenu.AddItem(iShorts);
				uIMenu.AddItem(iTShirt);
				uIMenu.AddItem(iTankTops);
				uIMenu.AddItem(iTops);
				uIMenu.RefreshIndex();
				gmSuitM = new UIMenu("", WardrobeText("CSHOP_TITLE49"));
				UIMenu uIMenu2 = gmSuitM;
				uIMenu2.SetBannerType(Helper.MichaelBanner);
				uIMenu2.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmSuitM);
				uIMenu2.AddItem(iFullSuits);
				uIMenu2.AddItem(iSuitJackets);
				uIMenu2.AddItem(iSuitPants);
				uIMenu2.AddItem(iSuitVests);
				uIMenu2.RefreshIndex();
				gmMichael.BindMenuToItem(gmSuitM, giSuits);
				gmGlassesM = new UIMenu("", WardrobeText("CSHOP_TITLE42"));
				UIMenu uIMenu3 = gmGlassesM;
				uIMenu3.SetBannerType(Helper.MichaelBanner);
				uIMenu3.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmGlassesM);
				uIMenu3.AddItem(iGlasses);
				uIMenu3.AddItem(iSportShades);
				uIMenu3.AddItem(iStreetShades);
				uIMenu3.RefreshIndex();
				gmMichael.BindMenuToItem(gmGlassesM, giGlasses);
				gmCasualJacketsM = new UIMenu("", WardrobeText("CSHOP_TITLE79"));
				UIMenu uIMenu4 = gmCasualJacketsM;
				uIMenu4.SetBannerType(Helper.MichaelBanner);
				uIMenu4.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmCasualJacketsM);
				uIMenu4.AddItem(iCasualJackets);
				uIMenu4.AddItem(iCasualJacketTShirts);
				uIMenu4.RefreshIndex();
				gmMichael.BindMenuToItem(gmCasualJacketsM, giCasualJackets);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void CreateFranklinWardrobeMenu()
		{
			try
			{
				Helper.RequestAdditionalText("clo_mnu", "CSHOP_TITLE1");
				gmFranklin = new UIMenu("", WardrobeText("CSHOP_TITLE1"));
				UIMenu uIMenu = gmFranklin;
				uIMenu.SetBannerType(Helper.FranklinBanner);
				uIMenu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmFranklin);
				uIMenu.AddItem(iOutfits);
				uIMenu.AddItem(giSuits);
				uIMenu.AddItem(giCasualJackets);
				uIMenu.AddItem(iEarrings);
				uIMenu.AddItem(giGlasses);
				uIMenu.AddItem(giHats);
				uIMenu.AddItem(iHoodies);
				uIMenu.AddItem(iJackets);
				uIMenu.AddItem(iPants);
				uIMenu.AddItem(iShoes);
				uIMenu.AddItem(iShirts);
				uIMenu.AddItem(iShorts);
				uIMenu.AddItem(iTShirt);
				uIMenu.AddItem(iTankTops);
				uIMenu.AddItem(iTops);
				uIMenu.AddItem(iVests);
				uIMenu.RefreshIndex();
				gmSuitF = new UIMenu("", WardrobeText("CSHOP_TITLE49"));
				UIMenu uIMenu2 = gmSuitF;
				uIMenu2.SetBannerType(Helper.FranklinBanner);
				uIMenu2.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmSuitF);
				uIMenu2.AddItem(iFullSuits);
				uIMenu2.AddItem(iSuitJackets);
				uIMenu2.AddItem(iSuitJacketsButtoned);
				uIMenu2.AddItem(iSuitPants);
				uIMenu2.AddItem(iSuitVests);
				uIMenu2.AddItem(iSuitTies);
				uIMenu2.RefreshIndex();
				gmFranklin.BindMenuToItem(gmSuitF, giSuits);
				gmCasualJacketsF = new UIMenu("", WardrobeText("CSHOP_TITLE79"));
				UIMenu uIMenu3 = gmCasualJacketsF;
				uIMenu3.SetBannerType(Helper.FranklinBanner);
				uIMenu3.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmCasualJacketsF);
				uIMenu3.AddItem(iCasualJackets);
				uIMenu3.AddItem(iCasualJacketTShirts);
				uIMenu3.RefreshIndex();
				gmFranklin.BindMenuToItem(gmCasualJacketsF, giCasualJackets);
				gmGlassesF = new UIMenu("", WardrobeText("CSHOP_TITLE42"));
				UIMenu uIMenu4 = gmGlassesF;
				uIMenu4.SetBannerType(Helper.FranklinBanner);
				uIMenu4.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmGlassesF);
				uIMenu4.AddItem(iSportShades);
				uIMenu4.AddItem(iStreetShades);
				uIMenu4.RefreshIndex();
				gmFranklin.BindMenuToItem(gmGlassesF, giGlasses);
				gmHatsF = new UIMenu("", WardrobeText("CSHOP_TITLE40"));
				UIMenu uIMenu5 = gmHatsF;
				uIMenu5.SetBannerType(Helper.FranklinBanner);
				uIMenu5.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmHatsF);
				uIMenu5.AddItem(iCapsForward);
				uIMenu5.AddItem(iCapsBackward);
				uIMenu5.RefreshIndex();
				gmFranklin.BindMenuToItem(gmHatsF, giHats);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static void CreateTrevorWardrobeMenu()
		{
			try
			{
				Helper.RequestAdditionalText("clo_mnu", "CSHOP_TITLE1");
				gmTrevor = new UIMenu("", WardrobeText("CSHOP_TITLE1"));
				UIMenu uIMenu = gmTrevor;
				uIMenu.SetBannerType(Helper.TrevorBanner);
				uIMenu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmTrevor);
				uIMenu.AddItem(iOutfits);
				uIMenu.AddItem(giSuits);
				uIMenu.AddItem(giGlasses);
				uIMenu.AddItem(iHats);
				uIMenu.AddItem(iHoodies);
				uIMenu.AddItem(iJackets);
				uIMenu.AddItem(giCasualJackets);
				uIMenu.AddItem(iPants);
				uIMenu.AddItem(iPoloShirt);
				uIMenu.AddItem(iShoes);
				uIMenu.AddItem(iShirts);
				uIMenu.AddItem(iShorts);
				uIMenu.AddItem(iSmartShoes);
				uIMenu.AddItem(iTShirt);
				uIMenu.AddItem(iTankTops);
				uIMenu.AddItem(iTops);
				uIMenu.RefreshIndex();
				gmSuitT = new UIMenu("", WardrobeText("CSHOP_TITLE49"));
				UIMenu uIMenu2 = gmSuitT;
				uIMenu2.SetBannerType(Helper.TrevorBanner);
				uIMenu2.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmSuitT);
				uIMenu2.AddItem(iFullSuits);
				uIMenu2.AddItem(iSuitJackets);
				uIMenu2.AddItem(iSuitPants);
				uIMenu2.RefreshIndex();
				gmTrevor.BindMenuToItem(gmSuitT, giSuits);
				gmGlassesT = new UIMenu("", WardrobeText("CSHOP_TITLE42"));
				UIMenu uIMenu3 = gmGlassesT;
				uIMenu3.SetBannerType(Helper.TrevorBanner);
				uIMenu3.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(gmGlassesT);
				uIMenu3.AddItem(iSportShades);
				uIMenu3.AddItem(iStreetShades);
				uIMenu3.RefreshIndex();
				gmTrevor.BindMenuToItem(gmGlassesT, giGlasses);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
			}
		}

		public static UIMenu CreateFreemodeWardrobeMenu(this UIMenu menu)
		{
			try
			{
				Helper.RequestAdditionalText("clo_mnu", "CSHOP_TITLE1");
				menu = new UIMenu(Game.Player.Name, WardrobeText("CSHOP_TITLE1"));
				UIMenu uIMenu = menu;
				uIMenu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(menu);
				uIMenu.AddItem(iOutfits);
				uIMenu.AddItem(iGlasses);
				uIMenu.AddItem(iEarrings);
				uIMenu.AddItem(giHats);
				uIMenu.AddItem(iChains);
				uIMenu.RefreshIndex();
				return menu;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log($"{ex2.Message} {ex2.StackTrace}");
				ProjectData.ClearProjectError();
				return menu;
			}
		}

		public static UIMenu LoadSetClothings(this UIMenu menu, string titleGXT, UIMenu bindMenu, UIMenuItem bindItem, List<ClothingSet> list, object banner = null)
		{
			try
			{
				menu = new UIMenu((banner == null) ? Game.Player.Name : "", WardrobeText(titleGXT));
				UIMenu uIMenu = menu;
				if (banner != null)
				{
					UIMenu instance = uIMenu;
					object[] obj = new object[1] { banner };
					object[] array = obj;
					bool[] obj2 = new bool[1] { true };
					bool[] array2 = obj2;
					NewLateBinding.LateCall(instance, null, "SetBannerType", obj, null, null, obj2, true);
					if (array2[0])
					{
						banner = RuntimeHelpers.GetObjectValue(array[0]);
					}
				}
				uIMenu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(menu);
				foreach (ClothingSet item in list)
				{
					menu.AddItem(new UIMenuItem(ResolveWardrobeText(item.LocalizedName, item.GetLocalizedName()))
					{
						Tag = item
					});
				}
				menu.RefreshIndex();
				bindMenu.BindMenuToItem(menu, bindItem);
				uIMenu = null;
				return menu;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log(string.Format("{0} {1}{2}{3}", ex2.Message, list.First().LocalizedName, "\r\n", ex2.StackTrace));
				ProjectData.ClearProjectError();
				return menu;
			}
		}

		public static UIMenu LoadSingleClothings(this UIMenu menu, string titleGXT, UIMenu bindMenu, UIMenuItem bindItem, List<Clothing> list, object banner = null)
		{
			try
			{
				menu = new UIMenu((banner == null) ? Game.Player.Name : "", WardrobeText(titleGXT));
				UIMenu uIMenu = menu;
				if (banner != null)
				{
					UIMenu instance = uIMenu;
					object[] obj = new object[1] { banner };
					object[] array = obj;
					bool[] obj2 = new bool[1] { true };
					bool[] array2 = obj2;
					NewLateBinding.LateCall(instance, null, "SetBannerType", obj, null, null, obj2, true);
					if (array2[0])
					{
						banner = RuntimeHelpers.GetObjectValue(array[0]);
					}
				}
				uIMenu.MouseEdgeEnabled = false;
				Helper.MenuPool.Add(menu);
				foreach (Clothing item in list)
				{
					menu.AddItem(new UIMenuItem(ResolveWardrobeText(item.LocalizedName, item.GetLocalizedName()))
					{
						Tag = item
					});
				}
				menu.RefreshIndex();
				bindMenu.BindMenuToItem(menu, bindItem);
				uIMenu = null;
				return menu;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log(string.Format("{0} {1}{2}{3}", ex2.Message, list.First().LocalizedName, "\r\n", ex2.StackTrace));
				ProjectData.ClearProjectError();
				return menu;
			}
		}

		public static void RefreshSingle(this UIMenu menu)
		{
			try
			{
				foreach (UIMenuItem menuItem in menu.MenuItems)
				{
					Clothing clothing = (Clothing)menuItem.Tag;
					switch (clothing.Type)
					{
					case eClothingType.CapBackward:
					case eClothingType.CapForward:
					case eClothingType.Earrings:
					case eClothingType.Glasses:
					case eClothingType.Hat:
					case eClothingType.SportShades:
					case eClothingType.StreetShades:
						switch (clothing.ComponentID)
						{
						case 0:
							if (Memory.Helmet == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 1:
							if (Memory.Glasses == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 2:
							if (Memory.Earrings == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						}
						break;
					case eClothingType.CasualJacket:
					case eClothingType.CasualTShirt:
					case eClothingType.Chain:
					case eClothingType.Hoodie:
					case eClothingType.Jacket:
					case eClothingType.Pants:
					case eClothingType.Polo:
					case eClothingType.Shirt:
					case eClothingType.Shoes:
					case eClothingType.Shorts:
					case eClothingType.SuitJacket:
					case eClothingType.SuitJacketButtoned:
					case eClothingType.SuitPants:
					case eClothingType.SuitTie:
					case eClothingType.SuitVest:
					case eClothingType.TankTop:
					case eClothingType.Top:
					case eClothingType.TShirt:
					case eClothingType.Vest:
						switch (clothing.ComponentID)
						{
						case 0:
							if (Memory.Face == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 1:
							if (Memory.Beard == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 2:
							if (Memory.Hair == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 3:
							if (Memory.Torso == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 4:
							if (Memory.Legs == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 5:
							if (Memory.Hands == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 6:
							if (Memory.Feet == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 7:
							if (Memory.Eyes == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 8:
							if (Memory.Accessories == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 9:
							if (Memory.Props == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 10:
							if (Memory.Textures == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						case 11:
							if (Memory.AuxiliaryTorso == clothing.CS1())
							{
								menuItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
							}
							break;
						}
						break;
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Logger.Log(string.Format("{0} {1}{2}{3}", ex2.Message, menu.Subtitle.Caption, "\r\n", ex2.StackTrace));
				ProjectData.ClearProjectError();
			}
		}

		private static void SingleClothing_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (!ApartmentInputReady())
			{
				return;
			}
			Clothing clothing = (Clothing)selectedItem.Tag;
			switch (clothing.Type)
			{
			case eClothingType.CapBackward:
			case eClothingType.CapForward:
			case eClothingType.Earrings:
			case eClothingType.Glasses:
			case eClothingType.Hat:
			case eClothingType.SportShades:
			case eClothingType.StreetShades:
				Helper.SetProps(Helper.PP, clothing.CS1());
				Memory.SetProps(clothing.CS1());
				break;
			case eClothingType.CasualJacket:
				Helper.SetClothes(Helper.PP, clothing.CS1());
				Helper.SetClothes(Helper.PP, clothing.CS2());
				Memory.SetClothes(clothing.CS1());
				Memory.SetClothes(clothing.CS2());
				break;
			case eClothingType.CasualTShirt:
			case eClothingType.Chain:
			case eClothingType.Pants:
			case eClothingType.Shoes:
			case eClothingType.SuitPants:
				Helper.SetClothes(Helper.PP, clothing.CS1());
				Memory.SetClothes(clothing.CS1());
				break;
			case eClothingType.Hoodie:
			case eClothingType.Jacket:
			case eClothingType.Polo:
			case eClothingType.Shirt:
			case eClothingType.TankTop:
			case eClothingType.Top:
			case eClothingType.TShirt:
				Helper.SetClothes(Helper.PP, clothing.CS1());
				Memory.SetClothes(clothing.CS1());
				if (Helper.GetPlayer() == eOwner.Franklin)
				{
					Helper.SetClothes(Helper.PP, 8, 14, 0);
					Memory.SetClothes(new ClothingSet.CS(8, 14, 0));
				}
				else
				{
					Helper.SetClothes(Helper.PP, 8, 0, 0);
					Memory.SetClothes(new ClothingSet.CS(8, 0, 0));
				}
				Helper.SetClothes(Helper.PP, 11, 0, 0);
				Helper.SetClothes(Helper.PP, 10, 0, 0);
				Memory.SetClothes(new ClothingSet.CS(11, 0, 0));
				Memory.SetClothes(new ClothingSet.CS(10, 0, 0));
				break;
			case eClothingType.Shorts:
				if (Helper.GetPlayer() == eOwner.Franklin)
				{
					Helper.SetProps(Helper.PP, clothing.CS1());
					Memory.SetClothes(clothing.CS1());
					break;
				}
				Helper.SetClothes(Helper.PP, clothing.CS1());
				Helper.SetClothes(Helper.PP, clothing.CS2());
				Memory.SetClothes(clothing.CS1());
				Memory.SetClothes(clothing.CS2());
				break;
			case eClothingType.SuitJacket:
			case eClothingType.SuitJacketButtoned:
				if (Helper.GetPlayer() == eOwner.Franklin)
				{
					int drawableID = Memory.Accessories.DrawableID;
					int drawableID5 = Memory.AuxiliaryTorso.DrawableID;
					if (drawableID != 14 || drawableID != 15)
					{
						Helper.SetClothes(Helper.PP, clothing.CS1());
						Helper.SetClothes(Helper.PP, 8, 14, 0);
						Helper.SetClothes(Helper.PP, 11, 3, 0);
						Memory.SetClothes(clothing.CS1());
						Memory.SetClothes(new ClothingSet.CS(8, 14, 0));
						Memory.SetClothes(new ClothingSet.CS(11, 3, 0));
					}
					else
					{
						Helper.SetClothes(Helper.PP, clothing.CS1());
						Memory.SetClothes(clothing.CS1());
					}
				}
				else
				{
					Helper.SetClothes(Helper.PP, clothing.CS1());
					Helper.SetClothes(Helper.PP, clothing.CS2());
					Memory.SetClothes(clothing.CS1());
					Memory.SetClothes(clothing.CS2());
				}
				break;
			case eClothingType.SuitTie:
			{
				ClothingSet.CS cS3 = clothing.CS1();
				ClothingSet.CS cS4 = clothing.CS2();
				int drawableID3 = Memory.Torso.DrawableID;
				int drawableID4 = Memory.AuxiliaryTorso.DrawableID;
				if (!(drawableID3 != 18 || drawableID3 != 23))
				{
					switch (drawableID4)
					{
					case 5:
						Helper.SetClothes(Helper.PP, cS3);
						Memory.SetClothes(cS3);
						break;
					case 3:
						Helper.SetClothes(Helper.PP, cS4);
						Memory.SetClothes(cS4);
						break;
					}
				}
				break;
			}
			case eClothingType.SuitVest:
				if (Helper.GetPlayer() == eOwner.Franklin)
				{
					int drawableID2 = Memory.Torso.DrawableID;
					if (drawableID2 != 18 || drawableID2 != 23)
					{
						Helper.SetClothes(Helper.PP, clothing.CS1());
						Helper.SetClothes(Helper.PP, 3, 18, 0);
						Memory.SetClothes(clothing.CS1());
						Memory.SetClothes(new ClothingSet.CS(3, 18, 0));
					}
					else
					{
						Helper.SetClothes(Helper.PP, clothing.CS1());
						Memory.SetClothes(clothing.CS1());
					}
				}
				else
				{
					Helper.SetClothes(Helper.PP, clothing.CS1());
					Memory.SetClothes(clothing.CS1());
				}
				break;
			case eClothingType.Vest:
			{
				ClothingSet.CS cS = clothing.CS2();
				ClothingSet.CS cS2 = clothing.CS3();
				if (Memory.Accessories.DrawableID != 15)
				{
					Helper.SetClothes(Helper.PP, clothing.CS1());
					Helper.SetClothes(Helper.PP, cS2);
					Memory.SetClothes(clothing.CS1());
					Memory.SetClothes(cS2);
				}
				else
				{
					Helper.SetClothes(Helper.PP, clothing.CS1());
					Helper.SetClothes(Helper.PP, cS);
					Memory.SetClothes(clothing.CS1());
					Memory.SetClothes(cS);
				}
				break;
			}
			}
			foreach (UIMenuItem menuItem in sender.MenuItems)
			{
				menuItem.SetRightBadge(UIMenuItem.BadgeStyle.None);
			}
			selectedItem.SetRightBadge(UIMenuItem.BadgeStyle.Clothes);
		}

		private static void SingleClothing_OnIndexChange(UIMenu sender, int index)
		{
			if (!ApartmentInputReady())
			{
				return;
			}
			Clothing clothing = (Clothing)sender.MenuItems[index].Tag;
			switch (clothing.Type)
			{
			case eClothingType.CapBackward:
			case eClothingType.CapForward:
			case eClothingType.Earrings:
			case eClothingType.Glasses:
			case eClothingType.Hat:
			case eClothingType.SportShades:
			case eClothingType.StreetShades:
				Helper.SetProps(Helper.PP, clothing.CS1());
				break;
			case eClothingType.CasualJacket:
				Helper.SetClothes(Helper.PP, clothing.CS1());
				Helper.SetClothes(Helper.PP, clothing.CS2());
				break;
			case eClothingType.CasualTShirt:
			case eClothingType.Chain:
			case eClothingType.Pants:
			case eClothingType.Shoes:
			case eClothingType.SuitPants:
				Helper.SetClothes(Helper.PP, clothing.CS1());
				break;
			case eClothingType.Hoodie:
			case eClothingType.Jacket:
			case eClothingType.Polo:
			case eClothingType.Shirt:
			case eClothingType.TankTop:
			case eClothingType.Top:
			case eClothingType.TShirt:
				Helper.SetClothes(Helper.PP, clothing.CS1());
				if (Helper.GetPlayer() == eOwner.Franklin)
				{
					Helper.SetClothes(Helper.PP, 8, 14, 0);
				}
				else
				{
					Helper.SetClothes(Helper.PP, 8, 0, 0);
				}
				Helper.SetClothes(Helper.PP, 11, 0, 0);
				Helper.SetClothes(Helper.PP, 10, 0, 0);
				break;
			case eClothingType.Shorts:
				if (Helper.GetPlayer() == eOwner.Franklin)
				{
					Helper.SetProps(Helper.PP, clothing.CS1());
					break;
				}
				Helper.SetClothes(Helper.PP, clothing.CS1());
				Helper.SetClothes(Helper.PP, clothing.CS2());
				break;
			case eClothingType.SuitJacket:
			case eClothingType.SuitJacketButtoned:
				if (Helper.GetPlayer() == eOwner.Franklin)
				{
					int drawableID = Memory.Accessories.DrawableID;
					int drawableID5 = Memory.AuxiliaryTorso.DrawableID;
					if (drawableID != 14 || drawableID != 15)
					{
						Helper.SetClothes(Helper.PP, clothing.CS1());
						Helper.SetClothes(Helper.PP, 8, 14, 0);
						Helper.SetClothes(Helper.PP, 11, 3, 0);
					}
					else
					{
						Helper.SetClothes(Helper.PP, clothing.CS1());
					}
				}
				else
				{
					Helper.SetClothes(Helper.PP, clothing.CS1());
					Helper.SetClothes(Helper.PP, clothing.CS2());
				}
				break;
			case eClothingType.SuitTie:
			{
				ClothingSet.CS cs3 = clothing.CS1();
				ClothingSet.CS cs4 = clothing.CS2();
				int drawableID3 = Memory.Torso.DrawableID;
				int drawableID4 = Memory.AuxiliaryTorso.DrawableID;
				if (!(drawableID3 != 18 || drawableID3 != 23))
				{
					switch (drawableID4)
					{
					case 5:
						Helper.SetClothes(Helper.PP, cs3);
						break;
					case 3:
						Helper.SetClothes(Helper.PP, cs4);
						break;
					}
				}
				break;
			}
			case eClothingType.SuitVest:
				if (Helper.GetPlayer() == eOwner.Franklin)
				{
					int drawableID2 = Memory.Torso.DrawableID;
					if (drawableID2 != 18 || drawableID2 != 23)
					{
						Helper.SetClothes(Helper.PP, clothing.CS1());
						Helper.SetClothes(Helper.PP, 3, 18, 0);
					}
					else
					{
						Helper.SetClothes(Helper.PP, clothing.CS1());
					}
				}
				else
				{
					Helper.SetClothes(Helper.PP, clothing.CS1());
				}
				break;
			case eClothingType.Vest:
			{
				ClothingSet.CS cs = clothing.CS2();
				ClothingSet.CS cs2 = clothing.CS3();
				if (Memory.Accessories.DrawableID != 15)
				{
					Helper.SetClothes(Helper.PP, clothing.CS1());
					Helper.SetClothes(Helper.PP, cs2);
				}
				else
				{
					Helper.SetClothes(Helper.PP, clothing.CS1());
					Helper.SetClothes(Helper.PP, cs);
				}
				break;
			}
			case eClothingType.FullSuit:
			case eClothingType.Outfit:
				break;
			}
		}

		private static void SetClothing_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (ApartmentInputReady())
			{
				ClothingSet clothingSet = (ClothingSet)sender.MenuItems[index].Tag;
				switch (clothingSet.Type)
				{
				case eClothingType.FullSuit:
					Helper.SetClothes(Helper.PP, clothingSet.Set1);
					Helper.SetClothes(Helper.PP, clothingSet.Set2);
					Helper.SetClothes(Helper.PP, clothingSet.Set3);
					Helper.SetClothes(Helper.PP, clothingSet.Set4);
					Helper.SetClothes(Helper.PP, clothingSet.Set5);
					Helper.SetClothes(Helper.PP, clothingSet.Set6);
					Helper.SetClothes(Helper.PP, clothingSet.Set7);
					Memory.SetClothes(clothingSet.Set1);
					Memory.SetClothes(clothingSet.Set2);
					Memory.SetClothes(clothingSet.Set3);
					Memory.SetClothes(clothingSet.Set4);
					Memory.SetClothes(clothingSet.Set5);
					Memory.SetClothes(clothingSet.Set6);
					Memory.SetClothes(clothingSet.Set7);
					break;
				case eClothingType.Outfit:
					Helper.SetClothes(Helper.PP, clothingSet.Set1);
					Helper.SetClothes(Helper.PP, clothingSet.Set2);
					Helper.SetClothes(Helper.PP, clothingSet.Set3);
					Helper.SetClothes(Helper.PP, clothingSet.Set4);
					Helper.SetClothes(Helper.PP, clothingSet.Set5);
					Helper.SetClothes(Helper.PP, clothingSet.Set6);
					Helper.SetClothes(Helper.PP, clothingSet.Set7);
					Helper.SetProps(Helper.PP, clothingSet.Set8);
					Helper.SetProps(Helper.PP, clothingSet.Set9);
					Helper.SetProps(Helper.PP, clothingSet.Set10);
					Memory.SetClothes(clothingSet.Set1);
					Memory.SetClothes(clothingSet.Set2);
					Memory.SetClothes(clothingSet.Set3);
					Memory.SetClothes(clothingSet.Set4);
					Memory.SetClothes(clothingSet.Set5);
					Memory.SetClothes(clothingSet.Set6);
					Memory.SetClothes(clothingSet.Set7);
					Memory.SetProps(clothingSet.Set8);
					Memory.SetProps(clothingSet.Set9);
					Memory.SetProps(clothingSet.Set10);
					break;
				}
			}
		}

		private static void SetClothing_OnIndexChange(UIMenu sender, int index)
		{
			if (ApartmentInputReady())
			{
				ClothingSet clothingSet = (ClothingSet)sender.MenuItems[index].Tag;
				switch (clothingSet.Type)
				{
				case eClothingType.FullSuit:
					Helper.SetClothes(Helper.PP, clothingSet.Set1);
					Helper.SetClothes(Helper.PP, clothingSet.Set2);
					Helper.SetClothes(Helper.PP, clothingSet.Set3);
					Helper.SetClothes(Helper.PP, clothingSet.Set4);
					Helper.SetClothes(Helper.PP, clothingSet.Set5);
					Helper.SetClothes(Helper.PP, clothingSet.Set6);
					Helper.SetClothes(Helper.PP, clothingSet.Set7);
					break;
				case eClothingType.Outfit:
					Helper.SetClothes(Helper.PP, clothingSet.Set1);
					Helper.SetClothes(Helper.PP, clothingSet.Set2);
					Helper.SetClothes(Helper.PP, clothingSet.Set3);
					Helper.SetClothes(Helper.PP, clothingSet.Set4);
					Helper.SetClothes(Helper.PP, clothingSet.Set5);
					Helper.SetClothes(Helper.PP, clothingSet.Set6);
					Helper.SetClothes(Helper.PP, clothingSet.Set7);
					Helper.SetProps(Helper.PP, clothingSet.Set8);
					Helper.SetProps(Helper.PP, clothingSet.Set9);
					Helper.SetProps(Helper.PP, clothingSet.Set10);
					break;
				}
			}
		}

		private static void FMOutfit_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
		{
			if (ApartmentInputReady())
			{
				ClothingSet clothingSet = (ClothingSet)sender.MenuItems[index].Tag;
				Helper.SetClothes(Helper.PP, clothingSet.Set1);
				Helper.SetClothes(Helper.PP, clothingSet.Set2);
				Helper.SetClothes(Helper.PP, clothingSet.Set3);
				Helper.SetClothes(Helper.PP, clothingSet.Set4);
				Helper.SetClothes(Helper.PP, clothingSet.Set5);
				Helper.SetClothes(Helper.PP, clothingSet.Set6);
				Helper.SetClothes(Helper.PP, clothingSet.Set7);
				Helper.SetClothes(Helper.PP, clothingSet.Set8);
				Helper.SetProps(Helper.PP, clothingSet.Set9);
				Helper.SetProps(Helper.PP, clothingSet.Set10);
				Helper.SetProps(Helper.PP, clothingSet.Set11);
				Memory.SetClothes(clothingSet.Set1);
				Memory.SetClothes(clothingSet.Set2);
				Memory.SetClothes(clothingSet.Set3);
				Memory.SetClothes(clothingSet.Set4);
				Memory.SetClothes(clothingSet.Set5);
				Memory.SetClothes(clothingSet.Set6);
				Memory.SetClothes(clothingSet.Set7);
				Memory.SetClothes(clothingSet.Set8);
				Memory.SetProps(clothingSet.Set9);
				Memory.SetProps(clothingSet.Set10);
				Memory.SetProps(clothingSet.Set11);
			}
		}

		private static void FMOutfit_OnIndexChange(UIMenu sender, int index)
		{
			if (ApartmentInputReady())
			{
				ClothingSet clothingSet = (ClothingSet)sender.MenuItems[index].Tag;
				Helper.SetClothes(Helper.PP, clothingSet.Set1);
				Helper.SetClothes(Helper.PP, clothingSet.Set2);
				Helper.SetClothes(Helper.PP, clothingSet.Set3);
				Helper.SetClothes(Helper.PP, clothingSet.Set4);
				Helper.SetClothes(Helper.PP, clothingSet.Set5);
				Helper.SetClothes(Helper.PP, clothingSet.Set6);
				Helper.SetClothes(Helper.PP, clothingSet.Set7);
				Helper.SetClothes(Helper.PP, clothingSet.Set8);
				Helper.SetProps(Helper.PP, clothingSet.Set9);
				Helper.SetProps(Helper.PP, clothingSet.Set10);
				Helper.SetProps(Helper.PP, clothingSet.Set11);
			}
		}

		private static void Main_OnMenuClose(UIMenu sender)
		{
			if (!apartmentClosing)
			{
				if (HasApartmentSession())
				{
					CleanupApartment();
					return;
				}
				World.RenderingCamera = null;
				Stability.DestroySpaCameras();
				Helper.PP.Task.ClearAllImmediately();
				Helper.HideHud = false;
				Helper.SetClothes(Helper.PP, Memory.Accessories);
				Helper.SetClothes(Helper.PP, Memory.AuxiliaryTorso);
				Helper.SetClothes(Helper.PP, Memory.Beard);
				Helper.SetProps(Helper.PP, Memory.Earrings);
				Helper.SetClothes(Helper.PP, Memory.Eyes);
				Helper.SetClothes(Helper.PP, Memory.Face);
				Helper.SetClothes(Helper.PP, Memory.Feet);
				Helper.SetProps(Helper.PP, Memory.Glasses);
				Helper.SetClothes(Helper.PP, Memory.Hair);
				Helper.SetClothes(Helper.PP, Memory.Hands);
				Helper.SetProps(Helper.PP, Memory.Helmet);
				Helper.SetClothes(Helper.PP, Memory.Legs);
				Helper.SetClothes(Helper.PP, Memory.Props);
				Helper.SetClothes(Helper.PP, Memory.Textures);
				Helper.SetClothes(Helper.PP, Memory.Torso);
			}
		}
	}
}
