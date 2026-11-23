using System.Collections.Generic;
using System.Linq;

namespace SinglePlayerHeists
{
	public class FrontendLobbyMenuMissionDetails
	{
		public string Name { get; set; } = string.Empty;


		public string Developer { get; set; } = string.Empty;


		public string Type { get; set; } = string.Empty;


		public Dictionary<string, string> Texture { get; set; }

		public int RP { get; set; }

		public int Cash { get; set; } = 2;


		public int AP { get; set; }

		public FrontendLobbyMenuMissionDetails(string name, string developer, string type, Dictionary<string, string> texture)
		{
			Name = name;
			Developer = developer;
			Type = type;
			Texture = texture;
		}

		public void Show()
		{
			string text = string.Empty;
			string text2 = string.Empty;
			if (Texture != null)
			{
				KeyValuePair<string, string> keyValuePair = Texture.First();
				text = keyValuePair.Key;
				text2 = keyValuePair.Value;
			}
			BaseScaleform.CallFunctionFrontend("SET_COLUMN_TITLE", 1, string.Empty, Name, string.Empty, text, text2, 1, 2, RP, Cash.ToString("N"), AP);
			BaseScaleform.CallFunctionFrontend("DISPLAY_DATA_SLOT", 1);
		}
	}
}
