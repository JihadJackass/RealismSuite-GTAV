namespace iFruitAddon2
{
	public sealed class Wallpaper : PhoneImage
	{
		public static Wallpaper iFruitDefault => new Wallpaper("Phone_Wallpaper_ifruitdefault");

		public static Wallpaper BadgerDefault => new Wallpaper("Phone_Wallpaper_badgerdefault");

		public static Wallpaper Bittersweet => new Wallpaper("Phone_Wallpaper_bittersweet_b");

		public static Wallpaper PurpleGlow => new Wallpaper("Phone_Wallpaper_purpleglow");

		public static Wallpaper GreenSquares => new Wallpaper("Phone_Wallpaper_greensquares");

		public static Wallpaper OrangeHerringBone => new Wallpaper("Phone_Wallpaper_orangeherringbone");

		public static Wallpaper OrangeHalftone => new Wallpaper("Phone_Wallpaper_orangehalftone");

		public static Wallpaper GreenTriangles => new Wallpaper("Phone_Wallpaper_greentriangles");

		public static Wallpaper GreenShards => new Wallpaper("Phone_Wallpaper_greenshards");

		public static Wallpaper BlueAngles => new Wallpaper("Phone_Wallpaper_blueangles");

		public static Wallpaper BlueShards => new Wallpaper("Phone_Wallpaper_blueshards");

		public static Wallpaper BlueTriangles => new Wallpaper("Phone_Wallpaper_bluetriangles");

		public static Wallpaper BlueCircles => new Wallpaper("Phone_Wallpaper_bluecircles");

		public static Wallpaper Diamonds => new Wallpaper("Phone_Wallpaper_diamonds");

		public static Wallpaper GreenGlow => new Wallpaper("Phone_Wallpaper_greenglow");

		public static Wallpaper Orange8Bit => new Wallpaper("Phone_Wallpaper_orange8bit");

		public static Wallpaper OrangeTriangles => new Wallpaper("Phone_Wallpaper_orangetriangles");

		public static Wallpaper PurpleTartan => new Wallpaper("Phone_Wallpaper_purpletartan");

		public Wallpaper(string txd)
			: base(txd)
		{
		}
	}
}
