using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	internal static class PropertyOwnershipText
	{
		[Serializable]
		[CompilerGenerated]
		internal sealed class _Closure_0024__
		{
			public static readonly _Closure_0024__ _0024I;

			public static Func<eOwner, bool> _0024I0_002D0;

			public static Func<eOwner, string> _0024I0_002D1;

			static _Closure_0024__()
			{
				_0024I = new _Closure_0024__();
			}

			[SpecialName]
			internal bool _Lambda_0024__0_002D0(eOwner owner)
			{
				if (owner >= eOwner.Michael)
				{
					return owner <= eOwner.Trevor;
				}
				return false;
			}

			[SpecialName]
			internal string _Lambda_0024__0_002D1(eOwner owner)
			{
				return owner.ToString();
			}
		}

		internal static string OwnerLabel(IEnumerable<eOwner> owners, bool german)
		{
			string[] array = owners.Where((_Closure_0024__._0024I0_002D0 != null) ? _Closure_0024__._0024I0_002D0 : (_Closure_0024__._0024I0_002D0 = _Closure_0024__._0024I._Lambda_0024__0_002D0)).Distinct().Select((_Closure_0024__._0024I0_002D1 != null) ? _Closure_0024__._0024I0_002D1 : (_Closure_0024__._0024I0_002D1 = _Closure_0024__._0024I._Lambda_0024__0_002D1))
				.ToArray();
			if (array.Length == 0)
			{
				return german ? "Privatbesitz" : "Privately owned";
			}
			return (german ? "Gehört " : "Owned by ") + string.Join(", ", array);
		}

		internal static string OwnerLabel(eOwner owner, bool german)
		{
			return OwnerLabel(new eOwner[1] { owner }, german);
		}

		internal static string ForSaleLabel(bool german)
		{
			if (!german)
			{
				return "For sale";
			}
			return "Zum Verkauf";
		}

		internal static string RequiresExecutiveGarage(bool german)
		{
			if (!german)
			{
				return "Requires an Arcadius Executive Garage";
			}
			return "Arcadius-Executive-Garage erforderlich";
		}
	}
}
