using System;
using System.IO;
using System.Reflection;
using GTA;
using GTA.Native;
using GTA.UI;

namespace HKHModHelperNew
{
	public class ManagePlayerFunds : Script
	{
		public static bool UseDirtyMoney;

		public ManagePlayerFunds()
		{
			base.Tick += MainTick;
		}

		public void MainTick(object sender, EventArgs e)
		{
			UseDirtyMoney = File.Exists("scripts/Dirty Money.dll");
		}

		public static void TextNotification(string avatar, string author, string title, string message)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "CONFIRM_BEEP", "HUD_MINI_GAME_SOUNDSET");
			Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, new InputArgument[1] { "STRING" });
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, new InputArgument[1] { message });
			Function.Call<int>(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, new InputArgument[6] { avatar, avatar, true, 0, title, author });
		}

		public static bool AreFundsEqual(int Value)
		{
			bool result = false;
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				if (DirtyMoneyWalletValue() == Value)
				{
					result = true;
				}
				if (Game.Player.Money == Value)
				{
					result = true;
				}
			}
			else if (Game.Player.Money == Value)
			{
				result = true;
			}
			return result;
		}

		public static bool AreFundsHigher(int Value)
		{
			bool result = false;
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				if (DirtyMoneyWalletValue() >= Value)
				{
					result = true;
				}
				if (Game.Player.Money >= Value)
				{
					result = true;
				}
			}
			else if (Game.Player.Money >= Value)
			{
				result = true;
			}
			return result;
		}

		public static bool AreFundsLower(int Value)
		{
			bool result = false;
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				if (DirtyMoneyWalletValue() <= Value)
				{
					result = true;
				}
				if (Game.Player.Money <= Value)
				{
					result = true;
				}
			}
			else if (Game.Player.Money <= Value)
			{
				result = true;
			}
			return result;
		}

		public static bool AreFundsEqual(float Value)
		{
			bool result = false;
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				if (DirtyMoneyWalletValue() == (int)Value)
				{
					result = true;
				}
				if (Game.Player.Money == (int)Value)
				{
					result = true;
				}
			}
			else if (Game.Player.Money == (int)Value)
			{
				result = true;
			}
			return result;
		}

		public static bool AreFundsHigher(float Value)
		{
			bool result = false;
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				if (DirtyMoneyWalletValue() >= (int)Value)
				{
					result = true;
				}
				if (Game.Player.Money >= (int)Value)
				{
					result = true;
				}
			}
			else if (Game.Player.Money >= (int)Value)
			{
				result = true;
			}
			return result;
		}

		public static bool AreFundsLower(float Value)
		{
			bool result = false;
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				if ((float)DirtyMoneyWalletValue() <= Value)
				{
					result = true;
				}
				if (Game.Player.Money <= (int)Value)
				{
					result = true;
				}
			}
			else if (Game.Player.Money <= (int)Value)
			{
				result = true;
			}
			return result;
		}

		public static void RemoveFunds(int Value)
		{
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				int num = Game.GameTime + 2500;
				int num2 = 0;
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					TextNotification("CHAR_BANK_FLEECA", "Fleeca Bank", "Auto Withdrawal", "Choose an option, Press 1 for Player Account, Press 2 for Dirty Money");
					int num3 = num + 25;
					while (Game.GameTime <= num3)
					{
						if (Game.IsControlPressed(Control.SelectWeaponUnarmed))
						{
							num3 = Game.GameTime - 50;
							num2 = 1;
						}
						if (Game.IsControlPressed(Control.SelectWeaponMelee))
						{
							num3 = Game.GameTime - 50;
							num2 = 2;
						}
						if (Game.GameTime >= num)
						{
							num2 = 1;
						}
						Script.Wait(0);
					}
				}
				if (Game.LastInputMethod == InputMethod.GamePad)
				{
					num2 = 1;
				}
				if (num2 == 1)
				{
					if (Game.Player.Money >= Value)
					{
						Game.Player.Money -= Value;
					}
					else if (DirtyMoneyWalletValue() >= Value)
					{
						DeductDirtyMoney(Value);
					}
					else
					{
						Notification.Show("Player does not have enough money to complete transaction.");
					}
				}
				if (num2 == 2)
				{
					if (DirtyMoneyWalletValue() >= Value)
					{
						DeductDirtyMoney(Value);
					}
					else if (Game.Player.Money >= Value)
					{
						Game.Player.Money -= Value;
					}
					else
					{
						Notification.Show("Player does not have enough money to complete transaction.");
					}
				}
			}
			else if (Game.Player.Money >= Value)
			{
				Game.Player.Money -= Value;
			}
			else
			{
				Notification.Show("Player does not have enough money to complete transaction.");
			}
		}

		public static void RemoveFunds(float Value)
		{
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				int num = Game.GameTime + 2500;
				int num2 = 0;
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					TextNotification("CHAR_BANK_FLEECA", "Fleeca Bank", "Auto Withdrawal", "Choose an option, Press 1 for Player Account, Press 2 for Dirty Money");
					int num3 = num + 25;
					while (Game.GameTime <= num3)
					{
						if (Game.IsControlPressed(Control.SelectWeaponUnarmed))
						{
							num3 = Game.GameTime - 50;
							num2 = 1;
						}
						if (Game.IsControlPressed(Control.SelectWeaponMelee))
						{
							num3 = Game.GameTime - 50;
							num2 = 2;
						}
						if (Game.GameTime >= num)
						{
							num2 = 1;
						}
						Script.Wait(0);
					}
				}
				if (Game.LastInputMethod == InputMethod.GamePad)
				{
					num2 = 1;
				}
				if (num2 == 1)
				{
					if ((float)Game.Player.Money >= Value)
					{
						Game.Player.Money -= (int)Value;
					}
					else if ((float)DirtyMoneyWalletValue() >= Value)
					{
						DeductDirtyMoney((int)Value);
					}
					else
					{
						Notification.Show("Player does not have enough money to complete transaction.");
					}
				}
				if (num2 == 2)
				{
					if ((float)DirtyMoneyWalletValue() >= Value)
					{
						DeductDirtyMoney((int)Value);
					}
					else if ((float)Game.Player.Money >= Value)
					{
						Game.Player.Money -= (int)Value;
					}
					else
					{
						Notification.Show("Player does not have enough money to complete transaction.");
					}
				}
			}
			else
			{
				Notification.Show("Player does not have enough money to complete transaction.");
			}
		}

		public static void AddFunds(int Value)
		{
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				int num = Game.GameTime + 2500;
				int num2 = 0;
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					TextNotification("CHAR_BANK_FLEECA", "Fleeca Bank", "Auto Deposit", "Choose an option, Press 1 for Player Account, Press 2 for Dirty Money");
					int num3 = num + 25;
					while (Game.GameTime <= num3)
					{
						if (Game.IsControlPressed(Control.SelectWeaponUnarmed))
						{
							num3 = Game.GameTime - 50;
							num2 = 1;
						}
						if (Game.IsControlPressed(Control.SelectWeaponMelee))
						{
							num3 = Game.GameTime - 50;
							num2 = 2;
						}
						if (Game.GameTime >= num)
						{
							num2 = 1;
						}
						Script.Wait(0);
					}
				}
				if (Game.LastInputMethod == InputMethod.GamePad)
				{
					num2 = 1;
				}
				if (num2 == 1)
				{
					Game.Player.Money += Value;
				}
				if (num2 == 2)
				{
					AddDirtyMoney(Value);
				}
			}
			else
			{
				Game.Player.Money += Value;
			}
		}

		public static void AddFunds(float Value)
		{
			UseDirtyMoney = File.Exists("scripts//Dirty Money.dll");
			if (UseDirtyMoney)
			{
				int num = Game.GameTime + 2500;
				int num2 = 0;
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					TextNotification("CHAR_BANK_FLEECA", "Fleeca Bank", "Auto Deposit", "Choose an option, Press 1 for Player Account, Press 2 for Dirty Money");
					int num3 = num + 25;
					while (Game.GameTime <= num3)
					{
						if (Game.IsControlPressed(Control.SelectWeaponUnarmed))
						{
							num3 = Game.GameTime - 50;
							num2 = 1;
						}
						if (Game.IsControlPressed(Control.SelectWeaponMelee))
						{
							num3 = Game.GameTime - 50;
							num2 = 2;
						}
						if (Game.GameTime >= num)
						{
							num2 = 1;
						}
						Script.Wait(0);
					}
				}
				if (Game.LastInputMethod == InputMethod.GamePad)
				{
					num2 = 1;
				}
				if (num2 == 1)
				{
					Game.Player.Money += (int)Value;
				}
				if (num2 == 2)
				{
					AddDirtyMoney((int)Value);
				}
			}
			else
			{
				Game.Player.Money += (int)Value;
			}
		}

		public static void AddDirtyMoney(int value)
		{
			MethodInfo method = Assembly.LoadFrom("scripts/Dirty Money.dll").GetType("Dirty_Money.AddRemove").GetMethod("AddDirtyMoney");
			object[] parameters = new object[1] { value };
			method.Invoke(method, parameters);
		}

		public static int DirtyMoneyWalletValue()
		{
			MethodInfo method = Assembly.LoadFrom("scripts/Dirty Money.dll").GetType("Dirty_Money.AddRemove").GetMethod("DirtyMoneyWallet");
			return (int)method.Invoke(method, null);
		}

		public static void DeductDirtyMoney(int value)
		{
			MethodInfo method = Assembly.LoadFrom("scripts/Dirty Money.dll").GetType("Dirty_Money.AddRemove").GetMethod("DeductDirtyMoney");
			object[] parameters = new object[1] { value };
			method.Invoke(method, parameters);
		}
	}
}
