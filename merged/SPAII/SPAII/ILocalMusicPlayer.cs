using System;

namespace SPAII
{
	public interface ILocalMusicPlayer : IDisposable
	{
		int ReadState(ref string failure);

		void SetLevels(int volume, float pan);

		void SetPaused(bool value);
	}
}
