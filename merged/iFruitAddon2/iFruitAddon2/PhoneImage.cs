using GTA.Native;

namespace iFruitAddon2
{
	public abstract class PhoneImage
	{
		internal string Name { get; set; }

		public PhoneImage(string name)
		{
			LoadTextureDict(name);
			Name = name;
		}

		private void LoadTextureDict(string txd)
		{
			if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, txd))
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, txd, 0);
			}
		}
	}
}
