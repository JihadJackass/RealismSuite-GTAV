using System;
using System.Collections.Generic;
using GTA;

namespace Metadata
{
	public static class Decor
	{
		public enum eDecorType
		{
			Float = 1,
			Bool,
			Int,
			Unk,
			Time
		}

		private static readonly Dictionary<string, eDecorType> _registered = new Dictionary<string, eDecorType>(StringComparer.Ordinal);

		private static readonly Dictionary<string, Dictionary<int, object>> _store = new Dictionary<string, Dictionary<int, object>>(StringComparer.Ordinal);

		private static readonly object _lock = new object();

		private static Dictionary<int, object> Bucket(string propertyName)
		{
			Dictionary<int, object> value;
			if (!_store.TryGetValue(propertyName, out value))
			{
				value = new Dictionary<int, object>();
				_store[propertyName] = value;
			}
			return value;
		}

		private static int H(Entity entity)
		{
			if (!(entity == null))
			{
				return entity.Handle;
			}
			return 0;
		}

		public static bool ExistsOn(string propertyName)
		{
			lock (_lock)
			{
				Dictionary<int, object> value;
				if (!_store.TryGetValue(propertyName, out value))
				{
					return false;
				}
				foreach (int key in value.Keys)
				{
					Entity entity = Entity.FromHandle(key);
					if (entity != null && entity.Exists() && entity is Vehicle)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static bool ExistsOn(this Entity entity, string propertyName)
		{
			lock (_lock)
			{
				return Bucket(propertyName).ContainsKey(H(entity));
			}
		}

		public static float GetFloat(this Entity entity, string propertyName)
		{
			lock (_lock)
			{
				object value;
				if (Bucket(propertyName).TryGetValue(H(entity), out value) && value is float)
				{
					return (float)value;
				}
				return 0f;
			}
		}

		public static int GetInt(this Entity entity, string propertyName)
		{
			lock (_lock)
			{
				object value;
				if (Bucket(propertyName).TryGetValue(H(entity), out value) && value is int)
				{
					return (int)value;
				}
				return 0;
			}
		}

		public static bool GetBool(this Entity entity, string propertyName)
		{
			lock (_lock)
			{
				object value;
				if (Bucket(propertyName).TryGetValue(H(entity), out value) && value is bool)
				{
					return (bool)value;
				}
				return false;
			}
		}

		public static void SetBool(this Entity entity, string propertyName, bool value)
		{
			if (entity == null)
			{
				return;
			}
			lock (_lock)
			{
				Bucket(propertyName)[entity.Handle] = value;
			}
		}

		public static void SetInt(this Entity entity, string propertyName, int value)
		{
			if (entity == null)
			{
				return;
			}
			lock (_lock)
			{
				Bucket(propertyName)[entity.Handle] = value;
			}
		}

		public static void SetFloat(this Entity entity, string propertyName, float value)
		{
			if (entity == null)
			{
				return;
			}
			lock (_lock)
			{
				Bucket(propertyName)[entity.Handle] = value;
			}
		}

		public static void Register(string propertyName, eDecorType type)
		{
			lock (_lock)
			{
				if (!_registered.ContainsKey(propertyName))
				{
					_registered[propertyName] = type;
				}
				Bucket(propertyName);
			}
		}

		public static bool Registered(string propertyName, eDecorType type)
		{
			lock (_lock)
			{
				eDecorType value;
				return _registered.TryGetValue(propertyName, out value) && value == type;
			}
		}

		public static bool Remove(this Entity entity, string propertyName)
		{
			lock (_lock)
			{
				return Bucket(propertyName).Remove(H(entity));
			}
		}

		public static void Unlock()
		{
		}

		public static void Lock()
		{
		}

		public static void Cleanup()
		{
			lock (_lock)
			{
				foreach (Dictionary<int, object> value in _store.Values)
				{
					List<int> list = new List<int>();
					foreach (int key in value.Keys)
					{
						Entity entity = Entity.FromHandle(key);
						if (entity == null || !entity.Exists())
						{
							list.Add(key);
						}
					}
					foreach (int item in list)
					{
						value.Remove(item);
					}
				}
			}
		}
	}
}
