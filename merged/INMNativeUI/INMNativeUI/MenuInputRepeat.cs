namespace INMNativeUI
{
	internal sealed class MenuInputRepeat
	{
		internal const uint InitialDelay = 350u;

		internal const uint RepeatDelay = 150u;

		internal const int Pressed = 1;

		internal const int Repeated = 2;

		internal const int Released = 4;

		private bool down;

		private bool suppressed;

		private uint due;

		internal void Suppress(bool held)
		{
			down = held;
			suppressed = held;
		}

		internal int Update(bool held, uint now, bool allowRepeat)
		{
			if (suppressed)
			{
				down = held;
				if (!held)
				{
					suppressed = false;
				}
				return 0;
			}
			if (!held)
			{
				bool num = down;
				down = false;
				if (!num)
				{
					return 0;
				}
				return 4;
			}
			if (!down)
			{
				down = true;
				due = now + 350;
				return 1;
			}
			if (!allowRepeat || (int)(now - due) < 0)
			{
				return 0;
			}
			due += 150u;
			if ((int)(now - due) >= 0)
			{
				due = now + 150;
			}
			return 2;
		}
	}
}
