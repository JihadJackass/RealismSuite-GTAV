namespace SPAII
{
	public class MansionTerrainMapState
	{
		private bool active;

		private bool deactivating;

		private bool returned;

		public bool IsActive => active;

		public bool IsDeactivating => deactivating;

		public bool SwitchReturned => returned;

		public bool Activate()
		{
			if (deactivating)
			{
				return false;
			}
			active = true;
			return true;
		}

		public void PrepareDeactivation()
		{
			if (active)
			{
				deactivating = true;
				returned = false;
			}
		}

		public void NativeReturned()
		{
			if (deactivating)
			{
				returned = true;
			}
		}

		public bool Complete(bool ccsPending)
		{
			if (!active)
			{
				return true;
			}
			if (!deactivating || !returned || ccsPending)
			{
				return false;
			}
			active = false;
			deactivating = false;
			returned = false;
			return true;
		}

		public void ForgetForStoryLoad()
		{
			active = false;
			deactivating = false;
			returned = false;
		}

		public bool Recover()
		{
			if (!active)
			{
				return false;
			}
			deactivating = false;
			returned = false;
			return true;
		}
	}
}
