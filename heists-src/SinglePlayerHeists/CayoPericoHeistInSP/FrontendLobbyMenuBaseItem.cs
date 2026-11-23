using GTA.UI;

namespace CayoPericoHeistInSP
{
	public abstract class FrontendLobbyMenuBaseItem
	{
		private string _text = string.Empty;

		private string _description = string.Empty;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
				PromptUpdate = true;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
				PromptUpdate = true;
			}
		}

		public bool PromptUpdate { get; set; }

		public FrontendMenuItemActivatedEventHandler Activated { get; set; }

		public HudColor Color { get; set; } = HudColor.Invalid;


		public FrontendLobbyMenuBaseItem(string text, string description)
		{
			Text = text;
			Description = description;
		}

		public virtual void Add(int index)
		{
		}

		public virtual void Update(int index)
		{
		}

		public virtual void Process(int index)
		{
		}
	}
}
