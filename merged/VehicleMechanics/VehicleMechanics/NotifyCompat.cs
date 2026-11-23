using System;
using System.Reflection;

namespace VehicleMechanics
{
	public static class NotifyCompat
	{
		private static MethodInfo _method;

		private static bool _resolved;

		private static bool _takesBool;

		public static void Show(string message)
		{
			try
			{
				if (!_resolved)
				{
					Resolve();
				}
				if (!(_method == null))
				{
					if (_takesBool)
					{
						_method.Invoke(null, new object[2] { message, false });
					}
					else
					{
						_method.Invoke(null, new object[1] { message });
					}
				}
			}
			catch
			{
			}
		}

		private static void Resolve()
		{
			_resolved = true;
			try
			{
				Type type = Type.GetType("GTA.UI.Notification, ScriptHookVDotNet3") ?? Type.GetType("GTA.UI.Notification");
				if (type == null)
				{
					return;
				}
				MethodInfo method = type.GetMethod("Show", new Type[2]
				{
					typeof(string),
					typeof(bool)
				});
				if (method != null)
				{
					_method = method;
					_takesBool = true;
					return;
				}
				method = type.GetMethod("PostTicker", new Type[2]
				{
					typeof(string),
					typeof(bool)
				});
				if (method != null)
				{
					_method = method;
					_takesBool = true;
					return;
				}
				method = type.GetMethod("Show", new Type[1] { typeof(string) });
				if (method != null)
				{
					_method = method;
					_takesBool = false;
					return;
				}
				method = type.GetMethod("PostTicker", new Type[1] { typeof(string) });
				if (method != null)
				{
					_method = method;
					_takesBool = false;
				}
			}
			catch
			{
			}
		}
	}
}
