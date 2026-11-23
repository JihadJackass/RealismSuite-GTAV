using GTA;

namespace Metadata
{
	public static class NewFunc
	{
		public static bool HideHud;

		public static bool IsCheating(string cheat)
		{
			return Game.WasCheatStringJustEntered(cheat);
		}

		public static int Hash(string input)
		{
			return Game.GenerateHash(input);
		}
	}
}
