namespace SinglePlayerHeists
{
	public struct FrontendMenuHeading
	{
		public string Text { get; set; }

		public float Width { get; set; }

		public FrontendMenuHeading(string text, float width)
		{
			Text = text;
			Width = width;
		}
	}
}
