using GTA;

namespace INMNativeUI
{
	public class BigMessageThread : Script
	{
		public static BigMessageHandler MessageInstance { get; set; }

		public BigMessageThread()
		{
			MessageInstance = new BigMessageHandler();
			base.Tick += delegate
			{
				MessageInstance.Update();
			};
		}
	}
}
