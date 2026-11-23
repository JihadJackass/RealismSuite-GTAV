using GTA.UI;

namespace CayoPericoHeistInSP
{
	public class FrontendLobbyMenuItem : FrontendLobbyMenuBaseItem
	{
		public new HudColor Color { get; set; } = HudColor.Invalid;


		public FrontendLobbyMenuItem(string text, string description)
			: base(text, description)
		{
		}

		public override void Add(int index)
		{
			base.Add(index);
			BaseScaleform.CallFunctionFrontend("SET_DATA_SLOT", 0, index, 0, index, (Color == HudColor.Invalid) ? 1 : 2, 0, true, base.Text, false, -1, 0, (int)Color, false);
		}
	}
}
