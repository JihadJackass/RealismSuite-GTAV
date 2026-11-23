namespace iFruitAddon2
{
	internal static class Extensions
	{
		internal static string SetBold(this string text, bool bold)
		{
			if (bold)
			{
				return text.ToLower();
			}
			return text.ToUpper();
		}
	}
}
