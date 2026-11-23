using System;
using GTA;
using GTA.Math;
using GTA.Native;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	[StandardModule]
	public static class HomeActionPose
	{
		public static Vector3 Position(Ped actor)
		{
			if ((object)actor == null || !actor.Exists())
			{
				throw new InvalidOperationException("Home action actor is unavailable.");
			}
			Vector3 vector = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, actor.Handle, false);
			if (!PartyPlacement.FinitePoint(vector) || vector == Vector3.Zero)
			{
				throw new InvalidOperationException("Home action position is invalid.");
			}
			return vector;
		}

		public static float Heading(Ped actor)
		{
			float num = Function.Call<float>(Hash.GET_ENTITY_HEADING, actor.Handle);
			if (float.IsNaN(num) || float.IsInfinity(num))
			{
				throw new InvalidOperationException("Home action heading is invalid.");
			}
			return num;
		}

		public static bool Near(Vector3 position, Vector3 origin, float radius)
		{
			if (PartyPlacement.FinitePoint(position) && PartyPlacement.FinitePoint(origin) && origin != Vector3.Zero && !float.IsNaN(radius) && !float.IsInfinity(radius) && radius >= 0f && radius <= 5f)
			{
				return position.DistanceToSquared(origin) <= radius * radius;
			}
			return false;
		}
	}
}
