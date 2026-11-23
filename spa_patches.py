#!/usr/bin/env python3
# RealismSuite's own changes to SPA II's converted source. Run after fix_spa.py (which regenerates merged/SPAII).
def edit(p, a, b):
    s=open(p).read(); assert s.count(a)==1,(p,a[:60],s.count(a)); open(p,'w').write(s.replace(a,b))

# 1. Stuck-HUD guard. SPA II hides the HUD and minimap every frame while Helper.HideHud is set (camera scenes, menus).
#    It is set in 58 places and cleared in 33, so a path that forgets to clear it takes the minimap away for good.
edit('merged/SPAII/SPAII/Timer.cs', '''		private void Timer_Tick(object sender, EventArgs e)
		{
			if (Helper.HideHud && !StoryLoadGuard.WorldLoading())
			{''', '''		// RealismSuite: if the flag stays on for 90 s while you're in control and outdoors, clear it and note it.
		private bool _hideWas;
		private int _hideSince;

		private static void Note(string m)
		{
			try
			{
				string dir = System.IO.Path.Combine(global::RealismSuite.Module.ScriptsDir, "RealismSuite");
				System.IO.File.AppendAllText(System.IO.Path.Combine(dir, "MissionGuard.log"), DateTime.Now.ToString("HH:mm:ss") + "  " + m + Environment.NewLine);
			}
			catch { }
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			bool hide = Helper.HideHud;
			if (hide != _hideWas)
			{
				_hideWas = hide; _hideSince = Game.GameTime;
				Note("SPA II HUD hide " + (hide ? "ON" : "off"));
			}
			if (hide && Game.GameTime - _hideSince > 90000 && Game.Player.CanControlCharacter && !Game.Player.Character.IsInVehicle()
				&& Function.Call<int>(Hash.GET_INTERIOR_FROM_ENTITY, Game.Player.Character) == 0 && !Function.Call<bool>(Hash.IS_CUTSCENE_ACTIVE))
			{
				Helper.HideHud = false;
				Note("SPA II HUD hide was stuck on for 90 s outdoors: cleared");
			}
			if (Helper.HideHud && !StoryLoadGuard.WorldLoading())
			{''')

# 2. Stuck-menu release. An SPA II menu that is open disables every control except walking and looking. If one ever gets
#    stuck open, holding Backspace for 1.5 s closes all SPA II menus. A menu holding the controls for 2 minutes is noted.
edit('merged/SPAII/SPAII/Timer.cs', '''			bool hide = Helper.HideHud;''', '''			if (INMNativeUI.UIMenu.LockFrame >= Game.FrameCount - 1)
			{
				if (_lockSince < 0) { _lockSince = Game.GameTime; _lockNoted = false; }
				if (!_lockNoted && Game.GameTime - _lockSince > 120000)
				{
					_lockNoted = true;
					Note("An SPA II menu has held the controls for 2 minutes" + LockTitle());
				}
				if (Game.IsKeyPressed(System.Windows.Forms.Keys.Back))
				{
					if (_backSince < 0) _backSince = Game.GameTime;
					else if (Game.GameTime - _backSince > 1500)
					{
						Note("SPA II menus force-closed by holding Backspace" + LockTitle());
						try { Helper.MenuPool.CloseAllMenus(); } catch { }
						try { if (INMNativeUI.UIMenu.LockedBy != null) INMNativeUI.UIMenu.LockedBy.Visible = false; } catch { }
						_backSince = -1;
					}
				}
				else _backSince = -1;
			}
			else { _lockSince = -1; _backSince = -1; }
			bool hide = Helper.HideHud;''')
edit('merged/SPAII/SPAII/Timer.cs', '''		private bool _hideWas;''', '''		private bool _hideWas, _lockNoted;
		private int _lockSince = -1, _backSince = -1;

		private static string LockTitle()
		{
			try { return INMNativeUI.UIMenu.LockedBy != null ? " (" + INMNativeUI.UIMenu.LockedBy.Title.Caption + ")" : ""; } catch { return ""; }
		}
''')
