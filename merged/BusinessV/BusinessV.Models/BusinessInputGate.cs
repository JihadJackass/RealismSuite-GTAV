namespace BusinessV.Models
{
	internal sealed class BusinessInputGate
	{
		internal const int DebounceMilliseconds = 150;

		private int lastAcceptedAt = int.MinValue;

		public bool TryAccept(int gameTime)
		{
			if (lastAcceptedAt != int.MinValue)
			{
				int num = gameTime - lastAcceptedAt;
				if (num >= 0 && num < 150)
				{
					return false;
				}
			}
			lastAcceptedAt = gameTime;
			return true;
		}
	}
}
