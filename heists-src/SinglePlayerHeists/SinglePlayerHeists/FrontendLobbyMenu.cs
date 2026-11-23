using System;
using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Native;

namespace SinglePlayerHeists
{
	public class FrontendLobbyMenu
	{
		public static bool AwaitingMenuUpdate;

		public const string MenuHash = "FE_MENU_VERSION_CORONA";

		public Dictionary<int, int> Highlights;

		private bool _isActive;

		public string Title { get; set; } = string.Empty;


		public string Description { get; set; } = string.Empty;


		public List<FrontendMenuHeading> Headings { get; private set; }

		public InstructionalButtons InstructionalButtons { get; private set; }

		public List<FrontendLobbyMenuBaseItem> Items { get; private set; }

		public FrontendLobbyMenuBaseItem CurrentItem
		{
			get
			{
				if (Items != null && Items.Count > 0 && Selection >= 0 && Selection < Items.Count)
				{
					return Items[Selection];
				}
				return null;
			}
		}

		public FrontendLobbyMenuMissionDetails MissionDetails { get; private set; }

		public bool IsActive
		{
			get
			{
				return Function.Call<int>(Hash.GET_CURRENT_FRONTEND_MENU_VERSION) == Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_CORONA");
			}
			set
			{
				if (_isActive && !value)
				{
					Closed(EventArgs.Empty);
				}
				_isActive = value;
			}
		}

		public bool IsReadyForControl => Function.Call<bool>(Hash.IS_FRONTEND_READY_FOR_CONTROL);

		public int Selection { get; private set; }

		public bool Inited { get; private set; }

		public int InitedGameTime { get; private set; }

		public event EventHandler OnClosed;

		public FrontendLobbyMenu(string title, string description, Dictionary<int, int> highlights = null)
		{
			Title = title;
			Description = description;
			Highlights = highlights;
			Headings = new List<FrontendMenuHeading>();
			InstructionalButtons = new InstructionalButtons();
			InstructionalButtons.AddButton(new Dictionary<Control, string> { 
			{
				Control.FrontendAccept,
				"Select"
			} });
			InstructionalButtons.AddButton(new Dictionary<Control, string> { 
			{
				Control.FrontendCancel,
				"Cancel"
			} });
			Items = new List<FrontendLobbyMenuBaseItem>();
		}

		private void TakeControl()
		{
			Function.Call(Hash.TAKE_CONTROL_OF_FRONTEND);
		}

		private void ReleaseControl()
		{
			Function.Call(Hash.RELEASE_CONTROL_OF_FRONTEND);
		}

		private void Activate()
		{
			Function.Call(Hash.ACTIVATE_FRONTEND_MENU, Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_CORONA"), false, -1);
		}

		private void Restart()
		{
			Function.Call(Hash.RESTART_FRONTEND_MENU, Function.Call<int>(Hash.GET_HASH_KEY, "FE_MENU_VERSION_CORONA"), -1);
		}

		public bool AddHeading(FrontendMenuHeading heading)
		{
			if (Headings != null && !Headings.Contains(heading))
			{
				Headings.Add(heading);
				return true;
			}
			return false;
		}

		public bool RemoveHeading(FrontendMenuHeading heading)
		{
			if (Headings != null && Headings.Contains(heading))
			{
				Headings.Remove(heading);
				return true;
			}
			return false;
		}

		public bool AddItem(FrontendLobbyMenuBaseItem item)
		{
			if (Items != null && !Items.Contains(item))
			{
				Items.Add(item);
				return true;
			}
			return false;
		}

		public bool RemoveItem(FrontendLobbyMenuBaseItem item)
		{
			if (Items != null && Items.Contains(item))
			{
				Items.Remove(item);
				return true;
			}
			return false;
		}

		public void AddMissionDetails(FrontendLobbyMenuMissionDetails missionDetails)
		{
			MissionDetails = missionDetails;
		}

		public void RemoveMissionDetails(FrontendLobbyMenuMissionDetails missionDetails)
		{
			MissionDetails = null;
		}

		public void Show()
		{
			InstructionalButtons?.Load();
			TakeControl();
			Activate();
		}

		public void Release()
		{
			InstructionalButtons?.Dispose();
			ReleaseControl();
			Activate();
			Inited = false;
		}

		public unsafe virtual void Update()
		{
			if (!Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) || Function.Call<bool>(Hash.IS_PAUSE_MENU_RESTARTING) || !IsReadyForControl)
			{
				return;
			}
			if (!Inited)
			{
				BaseScaleform.CallFunctionFrontend("SET_DATA_SLOT_EMPTY", 0);
				BaseScaleform.CallFunctionFrontend("SET_DATA_SLOT_EMPTY", 1);
				BaseScaleform.CallFunctionFrontend("SET_DATA_SLOT_EMPTY", 3);
				Script.Wait(0);
				if (Highlights != null)
				{
					KeyValuePair<int, int> keyValuePair = Highlights.First();
					BaseScaleform.CallFunctionFrontendHeader("SET_ALL_HIGHLIGHTS", keyValuePair.Key, keyValuePair.Value);
				}
				BaseScaleform.CallFunctionFrontendHeader("SHOW_HEADING_DETAILS", false);
				BaseScaleform.CallFunctionFrontendHeader("SHIFT_CORONA_DESC", true, false);
				Script.Wait(0);
				BaseScaleform.CallFunctionFrontendHeader("SET_HEADER_TITLE", Title ?? string.Empty, false, Description ?? string.Empty, false);
				Script.Wait(0);
				List<FrontendMenuHeading> headings = Headings;
				if (headings != null && headings.Count > 0)
				{
					for (int i = 0; i < Headings.Count; i++)
					{
						BaseScaleform.CallFunctionFrontendHeader("SET_MENU_HEADER_TEXT_BY_INDEX", i, Headings[i].Text, Headings[i].Width);
					}
				}
				Script.Wait(0);
				List<FrontendLobbyMenuBaseItem> items = Items;
				if (items != null && items.Count > 0)
				{
					for (int j = 0; j < Items.Count; j++)
					{
						Items[j].Add(j);
					}
				}
				Script.Wait(0);
				BaseScaleform.CallFunctionFrontend("DISPLAY_DATA_SLOT", 0);
				BaseScaleform.CallFunctionFrontend("DISPLAY_DATA_SLOT", 1);
				Script.Wait(0);
				MissionDetails?.Show();
				Script.Wait(0);
				BaseScaleform.CallFunctionFrontend("SET_COLUMN_FOCUS", 0, false, false, false);
				Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2, 201, 1f);
				Script.Wait(0);
				Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2, 187, 1f);
				Script.Wait(0);
				Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2, 188, 1f);
				Script.Wait(0);
				Function.Call(Hash.SET_CONTROL_VALUE_NEXT_FRAME, 2, 201, 1f);
				Script.Wait(0);
				InitedGameTime = Game.GameTime;
				Inited = true;
				return;
			}
			if (InstructionalButtons != null)
			{
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, true);
				InstructionalButtons.Draw();
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, false);
			}
			if (CurrentItem == null)
			{
				return;
			}
			CurrentItem.Process(Selection);
			bool flag = CurrentItem.PromptUpdate;
			CurrentItem.PromptUpdate = false;
			if (Function.Call<bool>(Hash.HAS_MENU_TRIGGER_EVENT_OCCURRED))
			{
				int num = 0;
				int selection = 0;
				Function.Call(Hash.GET_MENU_TRIGGER_EVENT_DETAILS, &num, &selection);
				Selection = selection;
				if (Game.GameTime - InitedGameTime > 500)
				{
					CurrentItem?.Activated?.Invoke(this, new FrontendMenuItemActivatedArgs(CurrentItem));
				}
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
				flag = true;
			}
			if (Function.Call<bool>(Hash.HAS_MENU_LAYOUT_CHANGED_EVENT_OCCURRED))
			{
				int num2 = 0;
				int num3 = 0;
				int selection2 = 0;
				Function.Call(Hash.GET_MENU_LAYOUT_CHANGED_EVENT_DETAILS, &num2, &num3, &selection2);
				Selection = selection2;
				flag = true;
			}
			if (flag)
			{
				try
				{
					CurrentItem.Update(Selection);
				}
				catch
				{
				}
				BaseScaleform.CallFunctionFrontend("SET_DESCRIPTION", 0, CurrentItem?.Description ?? string.Empty, true, false);
			}
		}

		protected virtual void Closed(EventArgs e)
		{
			this.OnClosed?.Invoke(this, e);
		}
	}
}
