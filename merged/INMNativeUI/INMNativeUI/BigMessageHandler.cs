using System;
using GTA;
using GTA.Native;

namespace INMNativeUI
{
	public class BigMessageHandler
	{
		private Scaleform _sc;

		private int _start;

		private int _timer;

		public void Load()
		{
			if (_sc == null)
			{
				_sc = new Scaleform("MP_BIG_MESSAGE_FREEMODE");
				int num = 1000;
				DateTime now = DateTime.Now;
				while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, _sc.Handle) && DateTime.Now.Subtract(now).TotalMilliseconds < (double)num)
				{
					Script.Yield();
				}
			}
		}

		public void Dispose()
		{
			_sc.Dispose();
			_sc = null;
		}

		public void ShowMissionPassedMessage(string msg, int time = 5000)
		{
			Load();
			_start = Game.GameTime;
			_sc.CallFunction("SHOW_MISSION_PASSED_MESSAGE", msg, "", 100, true, 0, true);
			_timer = time;
		}

		public void ShowColoredShard(string msg, string desc, HudColor textColor, HudColor bgColor, int time = 5000)
		{
			Load();
			_start = Game.GameTime;
			_sc.CallFunction("SHOW_SHARD_CENTERED_MP_MESSAGE", msg, desc, (int)bgColor, (int)textColor);
			_timer = time;
		}

		public void ShowOldMessage(string msg, int time = 5000)
		{
			Load();
			_start = Game.GameTime;
			_sc.CallFunction("SHOW_MISSION_PASSED_MESSAGE", msg);
			_timer = time;
		}

		public void ShowSimpleShard(string title, string subtitle, int time = 5000)
		{
			Load();
			_start = Game.GameTime;
			_sc.CallFunction("SHOW_SHARD_CREW_RANKUP_MP_MESSAGE", title, subtitle);
			_timer = time;
		}

		public void ShowRankupMessage(string msg, string subtitle, int rank, int time = 5000)
		{
			Load();
			_start = Game.GameTime;
			_sc.CallFunction("SHOW_BIG_MP_MESSAGE", msg, subtitle, rank, "", "");
			_timer = time;
		}

		public void ShowWeaponPurchasedMessage(string bigMessage, string weaponName, WeaponHash weapon, int time = 5000)
		{
			Load();
			_start = Game.GameTime;
			_sc.CallFunction("SHOW_WEAPON_PURCHASED", bigMessage, weaponName, (int)weapon, "", 100);
			_timer = time;
		}

		public void ShowMpMessageLarge(string msg, int time = 5000)
		{
			Load();
			_start = Game.GameTime;
			_sc.CallFunction("SHOW_CENTERED_MP_MESSAGE_LARGE", msg, "test", 100, true, 100);
			_sc.CallFunction("TRANSITION_IN");
			_timer = time;
		}

		public void ShowCustomShard(string funcName, params object[] paremeters)
		{
			Load();
			_sc.CallFunction(funcName, paremeters);
		}

		internal void Update()
		{
			if (_sc != null)
			{
				_sc.Render2D();
				if (_start != 0 && Game.GameTime - _start > _timer)
				{
					_sc.CallFunction("TRANSITION_OUT");
					_start = 0;
					Dispose();
				}
			}
		}
	}
}
