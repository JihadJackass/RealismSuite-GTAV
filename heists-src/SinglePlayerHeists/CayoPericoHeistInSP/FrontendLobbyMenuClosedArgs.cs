namespace CayoPericoHeistInSP
{
	public class FrontendLobbyMenuClosedArgs
	{
		public FrontendLobbyMenu Menu { get; }

		internal FrontendLobbyMenuClosedArgs(FrontendLobbyMenu menu)
		{
			Menu = menu;
		}
	}
}
