using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

public static class WebPrices
{
	public const int HiddenPrice = -1;

	private static readonly Dictionary<int, int> _overrides = new Dictionary<int, int>();

	private static readonly Dictionary<int, int> _shelved = new Dictionary<int, int>();

	private static bool _dirty;

	public const string IniFileName = "MpCarsPrices.ini";

	public static string IniPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MpCarsPrices.ini");

	public static event Action Changed;

	public static int Resolve(int modelHash, int stockPrice)
	{
		int value;
		if (!_overrides.TryGetValue(modelHash, out value))
		{
			return stockPrice;
		}
		return value;
	}

	public static bool HasOverride(int modelHash)
	{
		return _overrides.ContainsKey(modelHash);
	}

	public static int GetOverride(int modelHash)
	{
		int value;
		if (!_overrides.TryGetValue(modelHash, out value))
		{
			return int.MinValue;
		}
		return value;
	}

	public static bool IsHidden(int modelHash)
	{
		int value;
		if (_overrides.TryGetValue(modelHash, out value))
		{
			return value == -1;
		}
		return false;
	}

	public static Dictionary<int, int> GetAll()
	{
		return new Dictionary<int, int>(_overrides);
	}

	public static void SetPrice(int modelHash, int price)
	{
		if (price >= 0 || price == -1)
		{
			if (price != -1)
			{
				_shelved.Remove(modelHash);
			}
			int value;
			if (!_overrides.TryGetValue(modelHash, out value) || value != price)
			{
				_overrides[modelHash] = price;
				_dirty = true;
			}
		}
	}

	public static void SetHidden(int modelHash, bool hidden)
	{
		int value;
		bool flag = _overrides.TryGetValue(modelHash, out value);
		if (hidden)
		{
			if (!flag || value != -1)
			{
				if (flag)
				{
					_shelved[modelHash] = value;
				}
				SetPrice(modelHash, -1);
			}
		}
		else if (flag && value == -1)
		{
			int value2;
			if (_shelved.TryGetValue(modelHash, out value2))
			{
				_shelved.Remove(modelHash);
				SetPrice(modelHash, value2);
			}
			else
			{
				ClearOverride(modelHash);
			}
		}
	}

	public static int GetShelvedPrice(int modelHash)
	{
		int value;
		if (!_shelved.TryGetValue(modelHash, out value))
		{
			return int.MinValue;
		}
		return value;
	}

	public static void ClearOverride(int modelHash)
	{
		_shelved.Remove(modelHash);
		if (_overrides.Remove(modelHash))
		{
			_dirty = true;
		}
	}

	public static void ResetAll()
	{
		bool num = _overrides.Count > 0 || _shelved.Count > 0;
		_overrides.Clear();
		_shelved.Clear();
		if (num)
		{
			_dirty = true;
		}
	}

	internal static bool ConsumeDirty()
	{
		bool dirty = _dirty;
		_dirty = false;
		return dirty;
	}

	private static void RaiseChanged()
	{
		Action changed = WebPrices.Changed;
		if (changed != null)
		{
			try
			{
				changed();
			}
			catch
			{
			}
		}
	}

	public static void Load()
	{
		string iniPath = IniPath;
		if (!File.Exists(iniPath))
		{
			return;
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		try
		{
			int num = 0;
			string[] array = File.ReadAllLines(iniPath);
			for (int i = 0; i < array.Length; i++)
			{
				string text = StripComment(array[i]).Trim();
				if (text.Length == 0)
				{
					continue;
				}
				if (text[0] == '[')
				{
					num = (text.Equals("[Prices]", StringComparison.OrdinalIgnoreCase) ? 1 : (text.Equals("[HiddenPrices]", StringComparison.OrdinalIgnoreCase) ? 2 : 0));
				}
				else
				{
					if (num == 0)
					{
						continue;
					}
					int num2 = text.IndexOf('=');
					if (num2 <= 0)
					{
						continue;
					}
					string key = text.Substring(0, num2).Trim();
					string s = text.Substring(num2 + 1).Trim();
					int hash;
					int result;
					if (TryParseHash(key, out hash) && int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result) && (result >= 0 || result == -1))
					{
						if (num == 1)
						{
							dictionary[hash] = result;
						}
						else if (result != -1)
						{
							dictionary2[hash] = result;
						}
					}
				}
			}
		}
		catch
		{
			return;
		}
		_overrides.Clear();
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			_overrides[item.Key] = item.Value;
		}
		_shelved.Clear();
		foreach (KeyValuePair<int, int> item2 in dictionary2)
		{
			int value;
			if (_overrides.TryGetValue(item2.Key, out value) && value == -1)
			{
				_shelved[item2.Key] = item2.Value;
			}
		}
		_dirty = true;
		RaiseChanged();
	}

	public static bool Save()
	{
		string iniPath = IniPath;
		try
		{
			if (_overrides.Count == 0 && _shelved.Count == 0)
			{
				if (File.Exists(iniPath))
				{
					File.Delete(iniPath);
				}
				return true;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("; MP Cars in SP — переопределение цен каталога.");
			stringBuilder.AppendLine("; Ключ — хеш модели (или имя модели), значение — цена в $.");
			stringBuilder.AppendLine("; Цена -1 означает, что машина СКРЫТА из каталога.");
			stringBuilder.AppendLine("; Файл создаётся меню аддона, но его можно править руками.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("[Prices]");
			foreach (KeyValuePair<int, int> @override in _overrides)
			{
				stringBuilder.AppendLine(@override.Key.ToString(CultureInfo.InvariantCulture) + " = " + @override.Value.ToString(CultureInfo.InvariantCulture));
			}
			if (_shelved.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("; Цены скрытых машин — чтобы «показать» вернуло заданную цену.");
				stringBuilder.AppendLine("[HiddenPrices]");
				foreach (KeyValuePair<int, int> item in _shelved)
				{
					stringBuilder.AppendLine(item.Key.ToString(CultureInfo.InvariantCulture) + " = " + item.Value.ToString(CultureInfo.InvariantCulture));
				}
			}
			File.WriteAllText(iniPath, stringBuilder.ToString());
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void NotifyChanged()
	{
		RaiseChanged();
	}

	private static string StripComment(string line)
	{
		int num = line.IndexOfAny(new char[2] { ';', '#' });
		if (num >= 0)
		{
			return line.Substring(0, num);
		}
		return line;
	}

	private static bool TryParseHash(string key, out int hash)
	{
		if (int.TryParse(key, NumberStyles.Integer, CultureInfo.InvariantCulture, out hash))
		{
			return true;
		}
		uint result;
		if (key.StartsWith("0x", StringComparison.OrdinalIgnoreCase) && uint.TryParse(key.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
		{
			hash = (int)result;
			return true;
		}
		if (uint.TryParse(key, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
		{
			hash = (int)result;
			return true;
		}
		if (key.Length > 0 && (char.IsLetter(key[0]) || key[0] == '_'))
		{
			hash = (int)Joaat(key);
			return true;
		}
		return false;
	}

	public static uint Joaat(string s)
	{
		uint num = 0u;
		string text = s.ToLowerInvariant();
		foreach (char c in text)
		{
			num += c;
			num += num << 10;
			num ^= num >> 6;
		}
		num += num << 3;
		num ^= num >> 11;
		return num + (num << 15);
	}
}
