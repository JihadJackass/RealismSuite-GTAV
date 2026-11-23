namespace CayoPericoHeistInSP
{
	public class FrontendMenuSliderItemValueChangedArgs<T>
	{
		public T Value { get; }

		public int Index { get; }

		public FrontendLobbyMenuSliderItem<T> Item { get; }

		internal FrontendMenuSliderItemValueChangedArgs(T value, int index, FrontendLobbyMenuSliderItem<T> item)
		{
			Value = value;
			Index = index;
			Item = item;
		}
	}
}
