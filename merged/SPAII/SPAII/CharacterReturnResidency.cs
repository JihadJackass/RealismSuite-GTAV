using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class CharacterReturnResidency
	{
		internal class Visit
		{
			public eOwner Owner;

			public int Interior;

			public Mansion Home;
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__4_002D0
		{
			public eOwner _0024VB_0024Local_owner;

			public int _0024VB_0024Local_actual;

			[SpecialName]
			internal bool _Lambda_0024__0(Mansion home)
			{
				if (home != null && home.Enabled && home.IplsLoaded && home.Owner == _0024VB_0024Local_owner)
				{
					if (home.InteriorId != _0024VB_0024Local_actual)
					{
						return home.BasementId == _0024VB_0024Local_actual;
					}
					return true;
				}
				return false;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__7_002D0
		{
			public int _0024VB_0024Local_interior;

			[SpecialName]
			internal bool _Lambda_0024__0(Visit visit)
			{
				return visit.Interior == _0024VB_0024Local_interior;
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__8_002D0
		{
			public Mansion _0024VB_0024Local_home;

			[SpecialName]
			internal bool _Lambda_0024__0(Visit visit)
			{
				if (visit.Home == _0024VB_0024Local_home)
				{
					return visit.Owner == _0024VB_0024Local_home.Owner;
				}
				return false;
			}
		}

		private static Visit observed;

		private static readonly Dictionary<eOwner, Visit> suspended = new Dictionary<eOwner, Visit>();

		internal static void Observe(eOwner owner, Ped actor, int interior)
		{
			_Closure_0024__4_002D0 closure_0024__4_002D = new _Closure_0024__4_002D0();
			closure_0024__4_002D._0024VB_0024Local_owner = owner;
			Visit visit = observed;
			observed = null;
			if ((!Stability.IsPinnedInterior(interior) && MansionNativeGarage.ActiveMansion == null) || closure_0024__4_002D._0024VB_0024Local_owner < eOwner.Michael || closure_0024__4_002D._0024VB_0024Local_owner > eOwner.Trevor || (object)actor == null || !actor.Exists() || actor.IsDead)
			{
				return;
			}
			closure_0024__4_002D._0024VB_0024Local_actual = Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, actor.Handle);
			if (closure_0024__4_002D._0024VB_0024Local_actual == 0 || !Stability.IsPinnedInterior(closure_0024__4_002D._0024VB_0024Local_actual))
			{
				return;
			}
			Mansion mansion = Mansions.MansionList.FirstOrDefault(closure_0024__4_002D._Lambda_0024__0);
			if (mansion == null)
			{
				Mansion activeMansion = MansionNativeGarage.ActiveMansion;
				if (activeMansion != null && activeMansion.Enabled && activeMansion.IplsLoaded && activeMansion.Owner == closure_0024__4_002D._0024VB_0024Local_owner)
				{
					Vector3 vector = MansionNativeGarage.Origin(activeMansion);
					if (vector != Vector3.Zero && MansionGaragePlacement.IsInsideGarageVolume(MansionGaragePlacement.GarageLocalPoint(actor.Position, vector, MansionNativeGarage.Rotation(activeMansion))))
					{
						mansion = activeMansion;
					}
				}
			}
			observed = ((visit != null && visit.Owner == closure_0024__4_002D._0024VB_0024Local_owner && visit.Interior == closure_0024__4_002D._0024VB_0024Local_actual && visit.Home == mansion) ? visit : new Visit
			{
				Owner = closure_0024__4_002D._0024VB_0024Local_owner,
				Interior = closure_0024__4_002D._0024VB_0024Local_actual,
				Home = mansion
			});
		}

		internal static void ChangeOwner(eOwner outgoing, eOwner incoming)
		{
			if (outgoing != incoming)
			{
				if (observed != null && observed.Owner == outgoing)
				{
					suspended[outgoing] = observed;
				}
				suspended.Remove(incoming);
				observed = null;
			}
		}

		internal static bool HoldsOnlineMap()
		{
			return suspended.Count != 0;
		}

		internal static bool HoldsInterior(int interior)
		{
			_Closure_0024__7_002D0 closure_0024__7_002D = new _Closure_0024__7_002D0();
			closure_0024__7_002D._0024VB_0024Local_interior = interior;
			return suspended.Values.Any(closure_0024__7_002D._Lambda_0024__0);
		}

		internal static bool HoldsMansion(Mansion home)
		{
			_Closure_0024__8_002D0 closure_0024__8_002D = new _Closure_0024__8_002D0();
			closure_0024__8_002D._0024VB_0024Local_home = home;
			if (closure_0024__8_002D._0024VB_0024Local_home != null)
			{
				return suspended.Values.Any(closure_0024__8_002D._Lambda_0024__0);
			}
			return false;
		}

		internal static void Clear()
		{
			observed = null;
			suspended.Clear();
		}
	}
}
