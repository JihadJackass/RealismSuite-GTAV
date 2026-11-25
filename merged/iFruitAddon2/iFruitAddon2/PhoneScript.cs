using System.Collections.Generic;
using System;
using GTA;
using GTA.Native;

namespace iFruitAddon2
{
	internal static class PhoneScript
	{
		public static readonly int CellphoneHandHash = Game.GenerateHash("cellphone_flashhand");

		private static readonly Dictionary<PedHash, string> _characterScaleformDict = new Dictionary<PedHash, string>
		{
			{
				PedHash.Michael,
				"cellphone_ifruit"
			},
			{
				PedHash.Franklin,
				"cellphone_badger"
			},
			{
				PedHash.Trevor,
				"cellphone_facade"
			}
		};

		// RealismSuite: the original made a new Scaleform("cellphone_...") on every call, i.e. every frame while the phone
		// is up. Each one is a request that adds a hold on the game's own phone screen and is never given back, so the
		// screen can't be unloaded and rebuilt when an app needs it (taking a photo and sending it, for one). Same fault as
		// the browser screen (see RealismSuite.BrowserMovie): take the handle, give the hold straight back.
		internal static unsafe int CurrentHandle()
		{
			PedHash pedHash = (iFruitAddon2.IsEnhanced ? Tools.Game.GetPlayerPedModelHash() : ((PedHash)Game.Player.Character.Model.Hash));
			string name = _characterScaleformDict.ContainsKey(pedHash) ? _characterScaleformDict[pedHash] : _characterScaleformDict[PedHash.Michael];
			// only borrow the screen the game already has up: if it isn't loaded, asking would load a copy of our own,
			// and giving that back straight away would unload it again every frame
			int h = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, name);
			if (h == 0)
			{
				return 0;
			}
			bool loaded = Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, h);
			int n = h;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &n);
			return loaded ? h : 0;
		}

		internal static void Call(string method, params object[] args)
		{
			int h = CurrentHandle();
			if (h == 0)
			{
				return;
			}
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, h, method);
			foreach (object a in args)
			{
				if (a is bool)
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)a);
				}
				else
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Convert.ToInt32(a));
				}
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}
	}
}
