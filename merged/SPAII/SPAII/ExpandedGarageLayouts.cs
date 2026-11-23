using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class ExpandedGarageLayouts
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<bool> _0024I26_002D2;

			public static Func<Quaternion, Quaternion> _0024I27_002D0;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__26_002D2()
			{
				using (Process process = Process.GetCurrentProcess())
				{
					return NightclubProjector.IsEnhancedProcessName(process.ProcessName);
				}
			}

			[SpecialName]
			internal Quaternion _Lambda_0024__27_002D0(Quaternion bay)
			{
				return new Quaternion(VinewoodAnchor.X + bay.X, VinewoodAnchor.Y + bay.Y, VinewoodAnchor.Z + bay.Z, bay.W);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__19_002D0
		{
			public int _0024VB_0024Local_selected;

			public _Closure_0024__19_002D0(_Closure_0024__19_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_selected = arg0._0024VB_0024Local_selected;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(int interior)
			{
				return PrepareEclipse(interior, _0024VB_0024Local_selected);
			}

			[SpecialName]
			internal bool _Lambda_0024__1(int interior)
			{
				return CanRecoverFloor(interior, _0024VB_0024Local_selected, NumberSet);
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__23_002D0
		{
			public int _0024VB_0024Local_interior;

			public Func<int, string> _0024VB_0024Local_setName;

			[SpecialName]
			internal bool _Lambda_0024__0(int other)
			{
				return IsActive(_0024VB_0024Local_interior, _0024VB_0024Local_setName(other));
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__26_002D0
		{
			public int _0024VB_0024Local_selected;

			public _Closure_0024__26_002D0(_Closure_0024__26_002D0 arg0)
			{
				if (arg0 != null)
				{
					_0024VB_0024Local_selected = arg0._0024VB_0024Local_selected;
				}
			}

			[SpecialName]
			internal bool _Lambda_0024__0(int interior)
			{
				return PrepareVinewood(interior, _0024VB_0024Local_selected);
			}

			[SpecialName]
			internal bool _Lambda_0024__1(int interior)
			{
				return CanRecoverFloor(interior, _0024VB_0024Local_selected, VinewoodLevelSet);
			}
		}

		internal const int EclipseStorageId = 9320;

		internal const int VinewoodStorageId = 9321;

		internal const string VinewoodKey = "vinewood-club-garage";

		internal const string EclipseInterior = "xm3_dlc_int_02_xm3";

		internal const string VinewoodInterior = "m23_2_dlc_int_garage";

		private const string EclipseExteriorIpl = "xm3_garage_fix";

		private const string EclipseShell = "entity_set_shell_01";

		private const string EclipseTint = "entity_set_tint_01";

		private const Hash SetInteriorEntitySetTintIndex = Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX;

		private static readonly Vector3 EclipseAnchor = new Vector3(520f, -2625f, -50f);

		private static readonly Quaternion EclipseArrival = new Quaternion(528.2484f, -2637.728f, -49.9999f, 90f);

		private static readonly Quaternion EclipseMainFloor = new Quaternion(520.007f, -2624.996f, -49.9999f, 0f);

		private static readonly Vector3 EclipseElevator = new Vector3(530.001f, -2637.68f, -50f);

		private static readonly Vector3 VinewoodAnchor = new Vector3(1000f, -2274f, -50f);

		private static readonly Quaternion VinewoodArrival = new Quaternion(1000.02692f, -2276.45068f, -50.0011f, 0f);

		private static readonly Quaternion VinewoodElevatorArrival = new Quaternion(1011.33118f, -2278.09937f, -49.999f, 90f);

		private static readonly Vector3 VinewoodElevator = new Vector3(1013.293f, -2278.55f, -49.999f);

		private static readonly Vector3 VinewoodSecondElevator = new Vector3(986.664f, -2278.766f, -49.999f);

		internal static ExpansionProperty Eclipse()
		{
			List<ExpansionDestination> list = new List<ExpansionDestination>();
			List<Quaternion[]> list2 = new List<Quaternion[]>();
			int num = 0;
			checked
			{
				_Closure_0024__19_002D0 closure_0024__19_002D = default(_Closure_0024__19_002D0);
				do
				{
					closure_0024__19_002D = new _Closure_0024__19_002D0(closure_0024__19_002D);
					closure_0024__19_002D._0024VB_0024Local_selected = num;
					list.Add(new ExpansionDestination
					{
						Key = "level" + (num + 1).ToString(CultureInfo.InvariantCulture),
						Pose = EclipseArrival,
						FallbackPose = EclipseMainFloor,
						Anchor = EclipseAnchor,
						InteriorType = "xm3_dlc_int_02_xm3",
						Radius = 35f,
						EnableInterior = true,
						RetryResidency = true,
						OptionalIpls = new string[1] { "xm3_garage_fix" },
						Prepare = closure_0024__19_002D._Lambda_0024__0,
						CanRecover = closure_0024__19_002D._Lambda_0024__1
					});
					list2.Add(EclipseBays());
					num++;
				}
				while (num <= 4);
				ExpansionProperty obj = new ExpansionProperty
				{
					Key = "eclipse-blvd-garage",
					Name = "Eclipse Blvd Garage",
					Price = 2740000,
					BlipSprite = 357,
					Entrance = new Quaternion(-283.9974f, 281.4941f, 88.8874f, 180f),
					VehicleEntrance = new Quaternion(-272.7355f, 282.5825f, 89.02803f, 0f),
					VehicleExit = new Quaternion(-272.4846f, 274.5334f, 89.0368f, 180f),
					Arrival = list[0],
					Destinations = list
				};
				EclipseGarageLounge.Attach(obj);
				ExpandedGarage.Bind(obj, 9320, list.ToArray(), list2.ToArray(), EclipseElevator);
				return obj;
			}
		}

		internal static Quaternion[] EclipseBays()
		{
			Quaternion[] array = new Quaternion[10];
			int num = 0;
			do
			{
				float num2 = ((num < 5) ? (-4.3f) : 4f);
				float num3 = 11.15f - (float)(num % 5) * 5f;
				array[num] = new Quaternion(EclipseAnchor.X + num2, EclipseAnchor.Y + num3, EclipseAnchor.Z, (num < 5) ? 241.4f : 118.6f);
				num = checked(num + 1);
			}
			while (num <= 9);
			return array;
		}

		private static string NumberSet(int level)
		{
			return "entity_set_numbers_" + checked(level + 1).ToString("00", CultureInfo.InvariantCulture);
		}

		private static bool IsActive(int interior, string name)
		{
			return Function.Call<int>(Hash.IS_INTERIOR_ENTITY_SET_ACTIVE, interior, name) != 0;
		}

		private static bool CanRecoverFloor(int interior, int level, Func<int, string> setName)
		{
			_Closure_0024__23_002D0 closure_0024__23_002D = new _Closure_0024__23_002D0();
			closure_0024__23_002D._0024VB_0024Local_interior = interior;
			closure_0024__23_002D._0024VB_0024Local_setName = setName;
			if (IsActive(closure_0024__23_002D._0024VB_0024Local_interior, closure_0024__23_002D._0024VB_0024Local_setName(level)))
			{
				return true;
			}
			return !Enumerable.Range(0, 5).Any(closure_0024__23_002D._Lambda_0024__0);
		}

		private static bool ApplyFloorSet(int interior, int level, Func<int, string> setName)
		{
			int num = 0;
			bool result = default(bool);
			do
			{
				string text = setName(num);
				bool flag = num == level;
				if (IsActive(interior, text) != flag)
				{
					if (flag)
					{
						Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, interior, text);
					}
					else
					{
						Function.Call(Hash.DEACTIVATE_INTERIOR_ENTITY_SET, interior, text);
					}
					result = true;
				}
				num = checked(num + 1);
			}
			while (num <= 4);
			return result;
		}

		private static bool PrepareEclipse(int interior, int level)
		{
			bool flag = ApplyFloorSet(interior, level, NumberSet);
			if (!IsActive(interior, "entity_set_shell_01") && !IsActive(interior, "entity_set_shell_02") && !IsActive(interior, "entity_set_shell_03"))
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, interior, "entity_set_shell_01");
				flag = true;
			}
			if (!IsActive(interior, "entity_set_tint_01"))
			{
				Function.Call(Hash.ACTIVATE_INTERIOR_ENTITY_SET, interior, "entity_set_tint_01");
				Function.Call(Hash.SET_INTERIOR_ENTITY_SET_TINT_INDEX, interior, "entity_set_tint_01", 1);
				flag = true;
			}
			if (flag)
			{
				Function.Call(Hash.REFRESH_INTERIOR, interior);
			}
			return !flag;
		}

		internal static ExpansionProperty Vinewood()
		{
			List<ExpansionDestination> list = new List<ExpansionDestination>();
			List<Quaternion[]> list2 = new List<Quaternion[]>();
			int num = 0;
			checked
			{
				_Closure_0024__26_002D0 closure_0024__26_002D = default(_Closure_0024__26_002D0);
				do
				{
					closure_0024__26_002D = new _Closure_0024__26_002D0(closure_0024__26_002D);
					closure_0024__26_002D._0024VB_0024Local_selected = num;
					list.Add(new ExpansionDestination
					{
						Key = "basement" + (num + 1).ToString(CultureInfo.InvariantCulture),
						Pose = VinewoodArrival,
						FallbackPose = VinewoodElevatorArrival,
						Anchor = VinewoodAnchor,
						InteriorType = "m23_2_dlc_int_garage",
						Radius = 47f,
						EnableInterior = true,
						RetryResidency = true,
						VehicleArrivalFallback = VinewoodArrival,
						Prepare = closure_0024__26_002D._Lambda_0024__0,
						CanRecover = closure_0024__26_002D._Lambda_0024__1
					});
					list2.Add(VinewoodBays());
					num++;
				}
				while (num <= 4);
				ExpansionProperty obj = new ExpansionProperty
				{
					Key = "vinewood-club-garage",
					Name = "The Vinewood Club Garage",
					Price = 4000000,
					BlipSprite = 872,
					Entrance = new Quaternion(175.1616f, -1157.58923f, 28.3337173f, 180f),
					VehicleEntrance = new Quaternion(181.2f, -1153.5f, 28.7f, 180f),
					VehicleEntranceHalfWidth = 7.5f,
					AllowDrivewayPurchase = true,
					VehicleExit = new Quaternion(181.1315f, -1151.089f, 28.955f, 0f),
					VehicleExitFallback = new Quaternion(186f, -1151f, 28.955f, 0f),
					Arrival = list[0],
					Destinations = list,
					IsSupported = ((_Closure_0024__._0024I26_002D2 != null) ? _Closure_0024__._0024I26_002D2 : (_Closure_0024__._0024I26_002D2 = _Closure_0024__._0024I._Lambda_0024__26_002D2))
				};
				ExpandedGarage.Bind(obj, 9321, list.ToArray(), list2.ToArray(), VinewoodElevator, VinewoodSecondElevator);
				return obj;
			}
		}

		internal static Quaternion[] VinewoodBays()
		{
			return new Quaternion[20]
			{
				new Quaternion(11.0891f, 19.4619f, 0.5629f, 162.0001f),
				new Quaternion(16.1702f, 19.4619f, 0.2459f, 162f),
				new Quaternion(26.1991f, 19.4619f, 0.2599f, 157.3195f),
				new Quaternion(30.8219f, 19.4619f, 0.2521f, 162f),
				new Quaternion(35.8113f, 19.4619f, 0.4293f, 158.0405f),
				new Quaternion(35.4529f, 5.1006f, 0.5152f, 20.52f),
				new Quaternion(30.8843f, 5.1006f, 0.338f, 13.68f),
				new Quaternion(26.1129f, 5.1006f, 0.5243f, 21.24f),
				new Quaternion(16.0397f, 5.1006f, 0.5302f, 19.44f),
				new Quaternion(10.8653f, 5.1006f, 0.3333f, 19.44f),
				new Quaternion(-11.4595f, 5.1006f, 0.43f, 340.5667f),
				new Quaternion(-16.5939f, 5.1006f, 0.5026f, 341.6408f),
				new Quaternion(-25.8829f, 5.1006f, 0.3732f, 340.5607f),
				new Quaternion(-30.327f, 5.1006f, 0.3738f, 344.16f),
				new Quaternion(-35.2385f, 5.1006f, 0.2545f, 343.44f),
				new Quaternion(-35.1112f, 19.4619f, 0.288f, 198f),
				new Quaternion(-30.2843f, 19.4619f, 0.5562f, 203.26f),
				new Quaternion(-25.516f, 19.4619f, 0.5663f, 194.4f),
				new Quaternion(-16.7326f, 19.4619f, 0.6315f, 200.88f),
				new Quaternion(-11.6395f, 19.4619f, 0.3534f, 195.12f)
			}.Select((_Closure_0024__._0024I27_002D0 != null) ? _Closure_0024__._0024I27_002D0 : (_Closure_0024__._0024I27_002D0 = _Closure_0024__._0024I._Lambda_0024__27_002D0)).ToArray();
		}

		private static string VinewoodLevelSet(int level)
		{
			return "set_level_b" + checked(level + 1).ToString(CultureInfo.InvariantCulture);
		}

		private static bool PrepareVinewood(int interior, int level)
		{
			bool num = ApplyFloorSet(interior, level, VinewoodLevelSet);
			if (num)
			{
				Function.Call(Hash.REFRESH_INTERIOR, interior);
			}
			return !num;
		}
	}
}
