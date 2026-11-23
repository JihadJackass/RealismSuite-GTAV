using System;
using System.Runtime.CompilerServices;
using GTA;
using GTA.Native;
using iFruitSlotFix;

namespace iFruitAddon2
{
	public class iFruitContact
	{
		private bool _dialActive;

		private bool _busyActive;

		private int _dialSoundID = -1;

		private int _busySoundID = -1;

		private int _callTimer;

		private int _busyTimer;

		public string Name { get; set; } = "";


		private int _003CIndex_003Ek__BackingField;

		public int Index
		{
			[CompilerGenerated]
			get
			{
				return Slots.Map(_003CIndex_003Ek__BackingField);
			}
			[CompilerGenerated]
			private set
			{
				_003CIndex_003Ek__BackingField = value;
			}
		}

		public bool Active { get; set; } = true;


		public int DialTimeout { get; set; }

		public ContactIcon Icon { get; set; } = ContactIcon.Generic;


		public bool Bold { get; set; }

		public event ContactAnsweredEvent Answered;

		protected virtual void OnAnswered(iFruitContact sender)
		{
			this.Answered?.Invoke(this);
		}

		public iFruitContact(string name)
		{
			Name = name;
			Index = iFruitAddon2.GetNextAvailableContactIndex();
			Logger.Debug($"{DateTime.Now} : New contact created: {Name} ({Index})");
		}

		internal void Draw(int handle)
		{
			if (Index > -1)
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "SET_DATA_SLOT");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 2);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, Index);
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Name);
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "CELL_999");
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "CELL_2000");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Icon.Name.SetBold(Bold));
				Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		internal void Update()
		{
			if (_busyActive && Game.GameTime > _busyTimer)
			{
				if (iFruitAddon2.IsEnhanced)
				{
					Tools.Game.TaskPutAwayMobilePhone();
				}
				else
				{
					Game.Player.Character.Task.PutAwayMobilePhone();
				}
				Function.Call(Hash.STOP_SOUND, _busySoundID);
				Function.Call(Hash.RELEASE_SOUND_ID, _busySoundID);
				_busySoundID = -1;
				_busyActive = false;
			}
			if (_dialActive && Game.GameTime > _callTimer)
			{
				Function.Call(Hash.STOP_SOUND, _dialSoundID);
				Function.Call(Hash.RELEASE_SOUND_ID, _dialSoundID);
				_dialSoundID = -1;
				if (!Active)
				{
					iFruitContactCollection.DisplayCallUI(CustomiFruit.Instance.Handle, Name, "CELL_220", Icon.Name.SetBold(Bold));
					_busySoundID = Function.Call<int>(Hash.GET_SOUND_ID);
					Function.Call(Hash.PLAY_SOUND_FRONTEND, _busySoundID, "Remote_Engaged", "Phone_SoundSet_Default", 1);
					_busyTimer = Game.GameTime + 5000;
					_busyActive = true;
				}
				else
				{
					iFruitContactCollection.DisplayCallUI(CustomiFruit.Instance.Handle, Name, "CELL_219", Icon.Name.SetBold(Bold));
					OnAnswered(this);
				}
				_dialActive = false;
			}
		}

		public void Call()
		{
			if (_dialActive || _busyActive)
			{
				Logger.Debug("Canceling call (already on call or contact is busy)");
				return;
			}
			Logger.Debug("Player play task UseMobilePhone");
			if (iFruitAddon2.IsEnhanced)
			{
				Tools.Game.TaskUseMobilePhone();
			}
			else
			{
				Game.Player.Character.Task.UseMobilePhone();
			}
			if (DialTimeout > 0)
			{
				Logger.Debug("Playing dial sound...");
				iFruitContactCollection.DisplayCallUI(CustomiFruit.Instance.Handle, Name, "CELL_220", Icon.Name.SetBold(Bold));
				_dialSoundID = Function.Call<int>(Hash.GET_SOUND_ID);
				Function.Call(Hash.PLAY_SOUND_FRONTEND, _dialSoundID, "Dial_and_Remote_Ring", "Phone_SoundSet_Default", 1);
				_callTimer = Game.GameTime + DialTimeout;
				_dialActive = true;
				Logger.Debug("Sound played!");
			}
			else
			{
				iFruitContactCollection.DisplayCallUI(CustomiFruit.Instance.Handle, Name, "CELL_219", Icon.Name.SetBold(Bold));
				OnAnswered(this);
			}
		}

		public void EndCall()
		{
			if (_dialActive)
			{
				Function.Call(Hash.STOP_SOUND, _dialSoundID);
				Function.Call(Hash.RELEASE_SOUND_ID, _dialSoundID);
				_dialSoundID = -1;
				_dialActive = false;
			}
			if (_busyActive)
			{
				Function.Call(Hash.STOP_SOUND, _busySoundID);
				Function.Call(Hash.RELEASE_SOUND_ID, _busySoundID);
				_busySoundID = -1;
				_busyActive = false;
			}
		}
	}
}
