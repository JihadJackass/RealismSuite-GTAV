namespace SinglePlayerHeists
{
	public class FrontendMenuItemActivatedArgs
	{
		public FrontendLobbyMenuBaseItem Item { get; }

		internal FrontendMenuItemActivatedArgs(FrontendLobbyMenuBaseItem item)
		{
			Item = item;
		}
	}
}
