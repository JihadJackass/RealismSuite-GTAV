using System;

namespace SPAII
{
	internal sealed class SpaBlipVisibility
	{
		private readonly int originalAlpha;

		private bool appliedHidden;

		internal bool MissionHidden;

		internal SpaBlipVisibility(int alpha)
		{
			originalAlpha = Math.Max(0, Math.Min(255, alpha));
		}

		internal bool Apply(bool toggleHidden, Func<int> readAlpha, Action<int> writeAlpha, Action<bool> hideLegend)
		{
			bool flag = toggleHidden || MissionHidden;
			int num = ((!flag) ? originalAlpha : 0);
			bool result = false;
			if (readAlpha() != num)
			{
				writeAlpha(num);
				result = true;
			}
			if (appliedHidden != flag)
			{
				hideLegend(flag);
				appliedHidden = flag;
				result = true;
			}
			return result;
		}
	}
}
