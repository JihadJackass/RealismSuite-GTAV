using System;
using System.Windows.Forms;
using AddonWeapons2.Common;
using AddonWeapons2.UI;
using AddonWeapons2.Weapons;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace AddonWeapons2
{
	public class AddonWeapons : Script
	{
		private const int HeartbeatEveryTicks = 120;

		private ScriptSettings config_settings;

		private Keys menuOpenKey;

		private bool _weaponsLoaded;

		private bool _initFailureLogged;

		private int _tickCount;

		private int _ticksSinceHeartbeat;

		public AddonWeapons()
		{
			base.Tick += OnTick;
			base.KeyUp += onkeyup;
			base.Aborted += OnAborted;
			config_settings = ScriptSettings.Load("Scripts\\AddonWeapons\\settings.ini");
			menuOpenKey = config_settings.GetValue("MENU", "MenuOpenKey", Keys.None);
			Log("=== AddonWeapons script constructed ===");
		}

		private void OnTick(object sender, EventArgs e)
		{
			_tickCount++;
			LogHeartbeatIfDue();
			if (!_weaponsLoaded)
			{
				if (!IsPlayerReady())
				{
					return;
				}
				RunOneTimeInitialization();
				_weaponsLoaded = true;
			}
			SafeRun("Pool.Process", delegate
			{
				WeaponMenu.Pool.Process();
			});
			SafeRun("UpdateWeaponPreview", Items.UpdateWeaponPreview);
			SafeRun("UpdatePreviewCamera", UpdatePreviewCamera);
			SafeRun("UpdateAmmoBoxes", AmmoShopManager.UpdateAmmoBoxes);
			SafeRun("WaitForLoadedInventory", WeaponSave.WaitForLoadedInventory);
			SafeRun("ReOpenLastMenu", WeaponMenu.ReOpenLastMenu);
		}

		private static bool IsPlayerReady()
		{
			Ped ped = Game.Player?.Character;
			if (ped == null || !ped.Exists())
			{
				return false;
			}
			if (!Function.Call<bool>(Hash.IS_PLAYER_PLAYING, Game.Player))
			{
				return false;
			}
			Vector3 position = ped.Position;
			if (position.X == 0f && position.Y == 0f && position.Z == 0f)
			{
				return false;
			}
			return true;
		}

		private static void UpdatePreviewCamera()
		{
			if (!WeaponMenu.Pool.AreAnyVisible && WeaponPreviewCamera.IsActive)
			{
				WeaponPreviewCamera.Reset();
			}
		}

		private void LogHeartbeatIfDue()
		{
			if (++_ticksSinceHeartbeat >= 120)
			{
				_ticksSinceHeartbeat = 0;
				bool flag;
				try
				{
					flag = Game.IsLoading;
				}
				catch
				{
					flag = true;
				}
				bool flag2;
				bool flag3;
				try
				{
					Ped ped = Game.Player?.Character;
					flag2 = ped == null;
					flag3 = ped != null && ped.Exists();
				}
				catch
				{
					flag2 = true;
					flag3 = false;
				}
				Log($"heartbeat tick#{_tickCount} inited={_weaponsLoaded} " + $"IsLoading={flag} CharNull={flag2} CharExists={flag3}");
			}
		}

		private void RunOneTimeInitialization()
		{
			Log("--- one-time init: start ---");
			SafeRun("WeaponMenu.SetLanguage", WeaponMenu.SetLanguage);
			SafeRun("CategoriesManager.Init", CategoriesManager.InitializeCategories);
			SafeRun("WeaponMenu.Initialize", WeaponMenu.Initialize);
			SafeRun("WeaponInfo.LoadDlcWeaponModels", WeaponInfo.LoadDlcWeaponModels);
			SafeRun("Items.Initialize", Items.Initialize);
			SafeRun("WeaponMenu.BuildMainMenu", WeaponMenu.BuildMainMenu);
			SafeRun("AmmoShopManager.Initialize", AmmoShopManager.Initialize);
			Log("=== One-time initialization finished ===");
		}

		private void SafeRun(string stage, Action action)
		{
			try
			{
				action();
			}
			catch (Exception arg)
			{
				Log($"[ERROR] Stage '{stage}' failed: {arg}");
				if (!_initFailureLogged)
				{
					Notification.Show("~r~AddonWeapons: error in '" + stage + "'. See AddonWeapons.log");
					_initFailureLogged = true;
				}
			}
		}

		private void onkeyup(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == menuOpenKey)
			{
				SafeRun("OpenWeaponMenu", AmmoShopManager.OpenWeaponMenu);
			}
		}

		private void OnAborted(object sender, EventArgs e)
		{
			SafeRun("PreviewCamera.Reset", WeaponPreviewCamera.Reset);
			SafeRun("Cleanup", AmmoShopManager.Cleanup);
		}

		private void Log(string message)
		{
			FileLogger.Write("Main", message);
		}
	}
}
