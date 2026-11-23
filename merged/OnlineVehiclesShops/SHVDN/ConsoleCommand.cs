using System;

namespace SHVDN
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ConsoleCommand : Attribute
	{
		public string Help { get; private set; }

		public ConsoleCommand()
		{
			Help = string.Empty;
		}

		public ConsoleCommand(string help)
		{
			Help = help;
		}
	}
}
