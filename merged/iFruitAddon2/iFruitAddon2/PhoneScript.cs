using System.Collections.Generic;
using GTA;

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

		internal static Scaleform GetCurrentScaleform()
		{
			PedHash pedHash = PedHash.Michael;
			pedHash = (iFruitAddon2.IsEnhanced ? Tools.Game.GetPlayerPedModelHash() : ((PedHash)Game.Player.Character.Model.Hash));
			Scaleform scaleform = ((!_characterScaleformDict.ContainsKey(pedHash)) ? new Scaleform(_characterScaleformDict[PedHash.Michael]) : new Scaleform(_characterScaleformDict[pedHash]));
			int num = Game.GameTime + 2000;
			while (!scaleform.IsLoaded && Game.GameTime < num)
			{
				Script.Yield();
			}
			return scaleform;
		}
	}
}
