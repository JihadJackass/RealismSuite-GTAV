using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CayoPericoHeistInSP;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using iFruitAddon2;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace SinglePlayerHeists.UtilsMisc
{
	public class LesterContact : Script
	{
		public class Vector4
		{
			public Vector3 XYZ { get; set; }

			public float X { get; set; }

			public float Y { get; set; }

			public float Z { get; set; }

			public float H { get; set; }

			public Vector4(float x, float y, float z, float h)
			{
				XYZ = new Vector3(x, y, z);
				X = x;
				Y = y;
				Z = z;
				H = h;
			}
		}

		public class ButtonData
		{
			public int ButtonID { get; set; }

			public string DisplayName { get; set; }

			public string KeyboardKey { get; set; }

			public string ControllerKey { get; set; }

			public ButtonData()
			{
			}

			public ButtonData(int bid, string dn, string kk, string ck)
			{
				ButtonID = bid;
				DisplayName = dn;
				KeyboardKey = kk;
				ControllerKey = ck;
			}
		}

		public static ScriptSettings Config;

		private Keys Key1;

		private bool firstTime = true;

		public bool CreatePeds;

		public int num;

		public bool boughtvehicle;

		private ObjectPool modLemonUIObjectPool;

		private NativeMenu mainMenu;

		private NativeMenu Inventory;

		private NativeMenu Bribes;

		private NativeMenu TaxiService;

		public static int purchasedsnacks;

		public static int purchasedarmour;

		public static int purchasedvehicle = 0;

		public int wantedlevelprice;

		public bool invunrablewait;

		public int InvunrableTime;

		public int invunrabletimer;

		public bool offradarbool;

		public int offradartimer;

		public bool mobileoptions;

		public Vector3 OfficeMarker1;

		public Vector3 OfficeMarker2;

		public Vector3 OfficeMarker3;

		public Vector3 OfficeMarker4;

		public Vector3 BunkerPos = new Vector3(-3023.31f, 3334.41f, 9f);

		public Vector3 FacilityPos = new Vector3(-2228.79f, 2399.25f, 11f);

		public Vector3 HangerPos = new Vector3(-1146.24f, -3409.77f, 13f);

		public Vector3 MethBusinessPos = new Vector3(1313.79f, 4367.51f, 40.5f);

		public Vector3 Safehouse1Pos = new Vector3(2479.4f, 4086.53f, 37f);

		public Vehicle TaxiCab;

		public bool taxisetup;

		public Blip TaxiBlip;

		public bool Deliveredplayer;

		public int officenum;

		private CustomiFruit ifruit;

		public int Snack_hotkey;

		public int ARMOUR_hotkey;

		public bool Blackoutwait;

		public int BlackoutTimer;

		public bool noRestrictedzones;

		public int NoRestrictedZonesTimer;

		public int OffradarTimer;

		public List<Vehicle> SupportVeh = new List<Vehicle>();

		public List<Ped> SupportPed = new List<Ped>();

		public int T;

		public Ped Target;

		public Prop Armour;

		public Prop Armour2;

		public Prop Armour3;

		public Prop Armour4;

		public NativeMenu CustomBodyGuardSetup;

		public int RecallTimer;

		public Keys OrderAllEnter = Keys.X;

		public Keys OrderAllExit = Keys.Z;

		public Keys OrderAllFollow = Keys.C;

		public int RobberAtimer;

		public ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));

		public List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static bool HiredBodyguardfromPhone;

		public static Vector3 LastPosition;

		public bool NoCops;

		public int NoCopstime;

		public static NativeItem ClearWanted;

		public static NativeItem LocateABoat;

		public static NativeItem LocateAPlane;

		public static NativeItem LocateAHelicopter;

		public static NativeItem LocateACar;

		public static NativeItem CopsTurnBlindEye;

		public List<Vector4> Planes_Spawns = new List<Vector4>
		{
			new Vector4(2926.719f, 4708.947f, 50.87331f, 108.1412f),
			new Vector4(1280.665f, 6574.547f, 3.136494f, 78.53793f),
			new Vector4(784.2865f, 18054f / MathF.E, 2.637662f, 276.4899f),
			new Vector4(-407.3497f, 6450.901f, 4.112767f, 317.1119f),
			new Vector4(-2023.938f, 4567.659f, 3.58287f, 77.86713f),
			new Vector4(-2386.49f, 4331.315f, 4.861107f, 149.8464f),
			new Vector4(-2727.507f, 3330.413f, 33.40232f, 232.0413f),
			new Vector4(-2773.916f, 2518.366f, 3.279677f, 164.3401f),
			new Vector4(-3244.558f, 1253.591f, 3.454576f, 165.9624f),
			new Vector4(-1869.939f, -673.5375f, 10.4306f, 235.4846f),
			new Vector4(-2099.694f, -555.1393f, 3.625469f, 223.5399f),
			new Vector4(-1436.757f, -1147.937f, 2.890437f, 122.3362f),
			new Vector4(-1409.765f, -1292.942f, 4.617238f, 139.5555f),
			new Vector4(-1148.519f, -1688.833f, 4.910413f, 117.5875f),
			new Vector4(-1007.509f, -115.4527f, 41.99393f, 88.91827f),
			new Vector4(-2427.278f, -325.378f, 4.517624f, 56.57677f),
			new Vector4(-3075.509f, 534.7827f, 2.963972f, 12.53684f),
			new Vector4(-2733.365f, 2924.716f, 2.862744f, 168.8241f),
			new Vector4(27.039f, 3054.916f, 41.339f, 300.7954f),
			new Vector4(1066.571f, 3010.142f, 42.42347f, 284.2534f),
			new Vector4(1388.856f, 3131.756f, 41.19477f, 295.2688f),
			new Vector4(1777.876f, 3295.613f, 42.06717f, 135.8476f),
			new Vector4(1436.115f, 3081.401f, 41.50254f, 355.9196f),
			new Vector4(2141.616f, 3892.283f, 33.79065f, 116.9267f),
			new Vector4(1299.45f, 3672.619f, 33.38387f, 199.4567f),
			new Vector4(856.8387f, 3617.447f, 33.96971f, 34.32959f),
			new Vector4(1919.096f, 4729.926f, 42.00261f, 279.6833f),
			new Vector4(2114.962f, 4826.936f, 41.96791f, 174.7809f),
			new Vector4(2114.961f, 4826.936f, 41.96811f, 174.8038f),
			new Vector4(-1257.467f, -2145.977f, 14.51166f, 149.6433f),
			new Vector4(-1172.813f, -2382.311f, 14.52933f, 61.73795f),
			new Vector4(-1338.537f, -2385.852f, 14.53484f, 155.2009f),
			new Vector4(-1341.705f, -2735.674f, 14.53172f, 150.3424f),
			new Vector4(-968.4804f, -2944.39f, 14.55825f, 150.3627f),
			new Vector4(-984.2346f, -3111.704f, 14.5556f, 172.326f),
			new Vector4(-1031.467f, -3545.999f, 14.79005f, 339.8048f),
			new Vector4(-1295.816f, -3322.829f, 14.53496f, 328.8038f),
			new Vector4(-1507.953f, -3268.197f, 14.55879f, 323.304f),
			new Vector4(-382.849f, -2809.373f, 6.576535f, 133.2913f),
			new Vector4(1270.484f, -3291.429f, 6.494024f, 179.6079f),
			new Vector4(1455.563f, -2766.729f, 2.09383f, 289.4702f)
		};

		public List<Vector4> Boats_Spawns = new List<Vector4>
		{
			new Vector4(-3109.827f, 63.80479f, 0.7802718f, 197.389f),
			new Vector4(-2827.336f, -73.04639f, 0.5547334f, 230.4439f),
			new Vector4(-2533.027f, -284.6574f, 0.4872338f, 224.9224f),
			new Vector4(-2195.496f, -470.3411f, 0.5845167f, 219.4231f),
			new Vector4(-3230.251f, 788.4618f, 0.3927293f, 39.68952f),
			new Vector4(-3238.714f, 1362.468f, 0.4525207f, 333.5026f),
			new Vector4(-3136.179f, 1657.871f, 0.5713761f, 33.87946f),
			new Vector4(-3111.615f, 1957.291f, 0.7124327f, 22.96485f),
			new Vector4(-2884.463f, 2286.915f, 0.4915948f, 316.4005f),
			new Vector4(-2806.405f, 2486.23f, 0.5565974f, 22.50588f),
			new Vector4(-2615.498f, 2548.824f, 0.4557842f, 272.7635f),
			new Vector4(-2394.259f, 2631.101f, 0.3412207f, 190.1619f),
			new Vector4(-2085.459f, 2605.141f, 0.3944319f, 200.3476f),
			new Vector4(-1769.002f, 2632.104f, 0.4155131f, 85.57042f),
			new Vector4(-2047.354f, 2531.672f, 0.5992806f, 50.07394f),
			new Vector4(-2740.872f, 2704.305f, 0.4233544f, 132.1064f),
			new Vector4(-2959.382f, 3117.47f, 0.4088104f, 33.10245f),
			new Vector4(-3131.598f, 3365.262f, 0.4024113f, 5.642817f),
			new Vector4(-2889.007f, 3585.358f, 0.3926006f, 283.1348f),
			new Vector4(-2485.75f, 4251.742f, 0.5743086f, 332.476f),
			new Vector4(-1943.444f, 4622.224f, 0.1345597f, 32.84398f),
			new Vector4(-1771.356f, 5108.617f, 0.4214032f, 332.6212f),
			new Vector4(-1614.19f, 5266.912f, 0.4657737f, 293.8825f),
			new Vector4(-1092.571f, 5517.606f, 0.559985f, 293.4441f),
			new Vector4(-964.9985f, 6112.978f, 0.3230628f, 23.75028f),
			new Vector4(-673.8532f, 6192.372f, 0.2327855f, 346.7789f),
			new Vector4(-396.7932f, 6517.029f, 0.3809033f, 293.0353f),
			new Vector4(-284.2791f, 6641.3f, 0.367864f, 314.9492f),
			new Vector4(-15.95322f, 7013.973f, 0.3786352f, 331.4371f),
			new Vector4(86.50207f, 7247.341f, 0.3678303f, 215.886f),
			new Vector4(277.332f, 7032.884f, 0.4597042f, 217.3938f),
			new Vector4(694.4186f, 6688.775f, 0.464375f, 236.9817f),
			new Vector4(1161.623f, 6607.643f, 0.5078496f, 286.5124f),
			new Vector4(1463.573f, 6640.063f, 0.9447675f, 80.25108f),
			new Vector4(1896.139f, 6701.631f, 0.5207273f, 346.5479f),
			new Vector4(3359.084f, 5383.828f, 0.5163498f, 100.0971f),
			new Vector4(3369.706f, 5172.083f, 0.3593786f, 177.0726f),
			new Vector4(3221.616f, 5110.207f, 0.7736683f, 286.9073f),
			new Vector4(3677.577f, 4690.355f, 0.5781723f, 319.7646f),
			new Vector4(3799.864f, 4577.862f, 0.4666316f, 187.2802f),
			new Vector4(3860.945f, 4452.719f, 0.3130917f, 271.7144f),
			new Vector4(3898.477f, 4214.152f, 0.2913427f, 207.1041f),
			new Vector4(3936.866f, 3990.984f, 0.7986478f, 126.5439f),
			new Vector4(3773.711f, 3826.234f, 0.6700509f, 153.6825f),
			new Vector4(3846.722f, 3658.974f, 0.5539785f, 321.1735f),
			new Vector4(3998.361f, 3587.677f, 0.5539785f, 166.1735f),
			new Vector4(3895.564f, 3275.601f, 0.5539785f, 163.6735f),
			new Vector4(3764.471f, 3049.861f, 0.5539785f, 103.6735f),
			new Vector4(3625f, 2909.876f, 0.5539785f, 131.1735f),
			new Vector4(3517.578f, 2769.749f, 0.5539785f, 134.1735f),
			new Vector4(3440.282f, 2740.455f, 0.5539785f, 94.17349f),
			new Vector4(3343.633f, 2511.17f, 0.5539785f, 208.6735f),
			new Vector4(3299.305f, 2222.794f, 0.5539785f, 235.6735f),
			new Vector4(3110.509f, 2065.137f, 0.5539785f, 206.6735f),
			new Vector4(3042.21f, 2083.347f, 0.5539785f, 260.6735f),
			new Vector4(3036.456f, 1903.95f, 0.5539785f, 181.1735f),
			new Vector4(2899.25f, 1771.641f, 0.5539785f, 219.6735f),
			new Vector4(2978.449f, 1551.8f, 0.5539785f, 159.1735f),
			new Vector4(2799.87f, 1258.758f, 0.5539785f, 164.6735f),
			new Vector4(2794.517f, 1152.799f, 0.5539785f, 175.1735f),
			new Vector4(2783.537f, 1038.336f, 0.372423f, 152.6043f),
			new Vector4(2814.12f, 938.5156f, 0.372423f, 188.1043f),
			new Vector4(2857.701f, 839.0114f, 0.372423f, 223.6043f),
			new Vector4(2961.321f, 709.9481f, 0.372423f, 156.6043f),
			new Vector4(2946.53f, 643.5781f, 0.372423f, 159.1043f),
			new Vector4(2996.912f, 589.2118f, 0.372423f, 214.1043f),
			new Vector4(2991.039f, 462.4746f, 0.372423f, 125.1043f),
			new Vector4(2933.503f, 336.7091f, 0.7172349f, 199.2965f),
			new Vector4(2820.273f, 164.61f, 0.695769f, 154.0085f),
			new Vector4(2871.051f, -69.27341f, 0.9696974f, 224.5678f),
			new Vector4(2927.296f, -194.7946f, 0.8307194f, 92.68348f),
			new Vector4(2947.778f, -296.4498f, 0.8307194f, 168.6835f),
			new Vector4(2947.35f, -300.0244f, 0.8307194f, 167.6835f),
			new Vector4(2923.686f, -384.3476f, 0.8307194f, 141.6835f),
			new Vector4(2854.921f, -619.0284f, 0.396319f, 157.1819f),
			new Vector4(2827.082f, -838.7744f, 0.6397005f, 123.9228f),
			new Vector4(2677.566f, -975.6877f, 0.6397005f, 193.9228f),
			new Vector4(2658.115f, -1168.73f, 0.6397005f, 134.9228f),
			new Vector4(2552.93f, -1241.699f, 0.6397005f, 211.9228f),
			new Vector4(2537.482f, -1535.267f, 0.6397005f, 214.9228f),
			new Vector4(2707.026f, -1610.525f, 0.6397005f, 199.4228f),
			new Vector4(2681.479f, -1729.525f, 0.6397005f, 146.9228f),
			new Vector4(2676.932f, -1924.165f, 0.6397005f, 233.9228f),
			new Vector4(2418.459f, -2162.67f, 0.6397005f, 59.42279f),
			new Vector4(2348.682f, -2143.915f, 0.6397005f, 131.4228f),
			new Vector4(2275.979f, -2203.78f, 0.6397005f, 168.9228f),
			new Vector4(1878.999f, -2683.67f, 0.5045288f, 111.394f),
			new Vector4(1766.835f, -2726.159f, 0.4715167f, 70.87762f),
			new Vector4(1613.822f, -2747.781f, 0.4715167f, 116.8776f),
			new Vector4(1489.164f, -2769.966f, 0.4715167f, 95.37762f),
			new Vector4(1219.568f, -2713.9f, 0.4715167f, 62.37762f),
			new Vector4(1011.88f, -2683.847f, 0.4715167f, 88.87762f),
			new Vector4(671.5539f, -3052.571f, 0.3624709f, 179.3569f),
			new Vector4(611.2103f, -3222.169f, 0.3541311f, 179.3035f),
			new Vector4(1298.072f, -3353.983f, 0.6231175f, 265.9204f),
			new Vector4(484.8405f, -3392.907f, 0.8202804f, 90.77216f),
			new Vector4(103.3802f, -3116.68f, 0.823448f, 357.1227f),
			new Vector4(-529.0374f, -2921.177f, 0.823448f, 21.62272f),
			new Vector4(-532.871f, -2753.644f, 0.823448f, 316.1227f),
			new Vector4(-403.9464f, -2498.947f, 0.823448f, 54.12273f),
			new Vector4(1272.087f, 3686.937f, 30.529f, 291.9971f),
			new Vector4(1427.413f, 3774.995f, 30.39064f, 319.8773f),
			new Vector4(1537.768f, 3929.757f, 30.39064f, 264.3773f),
			new Vector4(1719.541f, 3977.175f, 30.39064f, 324.8773f),
			new Vector4(1859.786f, 4000.793f, 30.39064f, 271.8773f),
			new Vector4(2026.427f, 3958.803f, 30.39064f, 271.8773f),
			new Vector4(2070.906f, 3899.409f, 30.39064f, 312.8772f),
			new Vector4(2252.463f, 3939.998f, 30.39064f, 322.3773f),
			new Vector4(2351.746f, 4098.466f, 30.39064f, 59.87726f),
			new Vector4(2405.545f, 4460.758f, 30.39064f, 97.87725f),
			new Vector4(2363.507f, 4592.119f, 30.39064f, 84.37725f),
			new Vector4(2122.922f, 4577.488f, 30.39064f, 123.8773f),
			new Vector4(1843.538f, 4537.065f, 30.39064f, 107.3773f),
			new Vector4(1609.661f, 4444.738f, 30.39064f, 137.8772f),
			new Vector4(1312.864f, 4238.358f, 30.39064f, 263.8772f),
			new Vector4(1293.558f, 4222.545f, 30.39064f, 167.3772f),
			new Vector4(720.0428f, 4120.402f, 30.39064f, 171.3772f),
			new Vector4(402.3326f, 4279.261f, 30.39064f, 152.3772f),
			new Vector4(119.7086f, 4304.405f, 30.39064f, 208.3772f),
			new Vector4(-148.0721f, 3923.996f, 30.39064f, 213.3773f),
			new Vector4(-107.7247f, 3823.008f, 30.39064f, 239.3772f),
			new Vector4(32.09358f, 3815.379f, 30.39064f, 271.3773f),
			new Vector4(166.4947f, 3721.782f, 30.39064f, 191.3772f),
			new Vector4(143.2688f, 3452.743f, 30.39064f, 162.8772f),
			new Vector4(60.2131f, 3170.482f, 25.69281f, 156.3468f),
			new Vector4(-219.1078f, 3004.316f, 18.26336f, 139.274f),
			new Vector4(-431.5231f, 2932.966f, 13.52392f, 128.96f),
			new Vector4(-633.5408f, 2962.322f, 13.24848f, 127.6022f),
			new Vector4(-729.9872f, 2861.179f, 13.33036f, 101.8449f),
			new Vector4(-876.724f, 2793.78f, 9.67446f, 61.92184f),
			new Vector4(-1915.838f, -815.0593f, 0.8156457f, 205.4239f),
			new Vector4(-1788.109f, -973.5596f, 0.02182204f, 112.1368f),
			new Vector4(-1720.39f, -1068.365f, 0.5719915f, 228.7612f),
			new Vector4(-1549.261f, -1278.339f, 0.6853406f, 190.271f),
			new Vector4(-1489.815f, -1518.683f, 0.5357407f, 217.9127f),
			new Vector4(-1254.905f, -1872.397f, 0.5723106f, 107.8952f),
			new Vector4(-794.7715f, -1507.411f, 0.4512252f, 110.1781f),
			new Vector4(-859.6552f, -1477.356f, 0.2064795f, 288.4868f),
			new Vector4(-918.3547f, -1475.287f, 0.4512125f, 18.523f),
			new Vector4(-847.0665f, -1353.119f, 0.4512608f, 111.7017f),
			new Vector4(-1115.893f, -1133.96f, 0.4549289f, 212.5538f),
			new Vector4(-491.5586f, -1558.105f, 0.5347537f, 242.4158f),
			new Vector4(76.30257f, -2247.529f, 0.4249468f, 104.0571f),
			new Vector4(425.6283f, -2255.954f, 0.4647824f, 150.6925f),
			new Vector4(371.7563f, -2831.546f, 1.004128f, 123.3076f),
			new Vector4(302.3619f, -3331.117f, 0.6514316f, 178.3946f),
			new Vector4(6.926219f, -2793.742f, 0.7022737f, 2.155237f),
			new Vector4(-259.7587f, -2708.027f, 0.5784347f, 150.6436f),
			new Vector4(-389.4161f, -2844.669f, 0.4664539f, 134.3566f),
			new Vector4(-724.6634f, -3233.538f, 0.9253626f, 212.3085f),
			new Vector4(-1083.565f, -3573.668f, 0.3757629f, 85.74163f),
			new Vector4(-1315.605f, -3488.492f, 0.623948f, 75.45891f),
			new Vector4(-1577.959f, -3410.189f, 0.6552141f, 75.63369f),
			new Vector4(-1810.301f, -3318.584f, 0.6022441f, 79.62511f),
			new Vector4(-2008.625f, -3013.822f, 0.5822365f, 2.130183f),
			new Vector4(-1590.295f, -2247.008f, 0.3448582f, 358.8631f)
		};

		public List<Vector4> Helicopters_Spawns = new List<Vector4>
		{
			new Vector4(-302.2234f, 2950.99f, 28.47087f, 271.7028f),
			new Vector4(191.545f, 3281.172f, 41.19722f, 274.9536f),
			new Vector4(339.4587f, 3565.575f, 33.18104f, 341.9426f),
			new Vector4(774.4288f, 3562.021f, 33.66328f, 284.4307f),
			new Vector4(960.3703f, 3646.65f, 31.94882f, 268.8617f),
			new Vector4(1067.398f, 4247.345f, 36.8216f, 329.554f),
			new Vector4(1739.166f, 4624.759f, 42.28072f, 297.5851f),
			new Vector4(2043.207f, 3878.281f, 31.48067f, 154.5294f),
			new Vector4(1770.305f, 3654.503f, 34.13227f, 115.4832f),
			new Vector4(1769.895f, 3240.04f, 41.85074f, 101.7952f),
			new Vector4(1574.431f, 3136.417f, 41.83482f, 129.3513f),
			new Vector4(1488.291f, 3157.362f, 40.64297f, 321.5425f),
			new Vector4(1113.252f, 3001.541f, 40.82496f, 49.89311f),
			new Vector4(594.2449f, 2803.946f, 41.61512f, 6.479378f),
			new Vector4(186.882f, 2713.421f, 41.86211f, 174.6329f),
			new Vector4(746.7661f, 2518.851f, 72.85113f, 356.9209f),
			new Vector4(1729.977f, 2852.572f, 45.12676f, 304.6904f),
			new Vector4(2029.483f, 3413.616f, 43.92936f, 325.8856f),
			new Vector4(1604.692f, 3528.04f, 35.96667f, 113.8253f),
			new Vector4(1371.64f, 3704.682f, 32.60558f, 106.9008f),
			new Vector4(2412.955f, 4127.387f, 35.32001f, 10.83487f),
			new Vector4(2537.975f, 4356.849f, 39.72024f, 314.3091f),
			new Vector4(2563.045f, 4788.553f, 32.38628f, 43.12641f),
			new Vector4(2067.015f, 4751.105f, 41.27504f, 22.8581f),
			new Vector4(1965.801f, 4755.53f, 41.51114f, 120.444f),
			new Vector4(2400.476f, 5041.208f, 45.89943f, 225.2384f),
			new Vector4(1604.614f, 6621.511f, 15.65576f, 321.1815f),
			new Vector4(274.3068f, 6415.859f, 31.6496f, 95.88152f),
			new Vector4(-279.5118f, 6034.339f, 31.23294f, 37.22147f),
			new Vector4(-592.0092f, 6372.91f, 3.188716f, 26.2022f),
			new Vector4(-154.8021f, 6698.603f, 0.8228874f, 289.7749f),
			new Vector4(-731.5915f, 5799.729f, 17.3087f, 9.907924f),
			new Vector4(-472.3433f, 5370.638f, 80.38233f, 160.6197f),
			new Vector4(-158.3822f, 3904.665f, 31.38287f, 182.616f),
			new Vector4(-67.13516f, 896.1697f, 235.2367f, 197.4254f),
			new Vector4(193.2905f, 385.3009f, 107.8106f, 261.7661f),
			new Vector4(631.9683f, 141.9937f, 95.26547f, 62.24413f),
			new Vector4(1136.455f, 117.8338f, 80.51856f, 327.9478f),
			new Vector4(1376.805f, -740.8609f, 66.95283f, 74.20828f),
			new Vector4(1140.384f, -1281.482f, 34.32779f, 175.1169f),
			new Vector4(1542.036f, -2082.599f, 76.51745f, 174.3829f),
			new Vector4(1442.667f, -2599.938f, 47.87439f, 68.41483f),
			new Vector4(780.2386f, -2530.742f, 20.05151f, 349.2889f),
			new Vector4(562.1188f, -2400.285f, 5.596131f, 99.55132f),
			new Vector4(-411.3748f, -2275.519f, 7.338206f, 87.95325f),
			new Vector4(-251.576f, -2104.294f, 27.32069f, 255.4563f),
			new Vector4(-300.5495f, -1642.938f, 31.3793f, 282.8237f),
			new Vector4(-346.4127f, -1355.1f, 31.08939f, 2.795731f),
			new Vector4(-716.3f, -1284.749f, 4.576348f, 140.091f),
			new Vector4(-744.9962f, -1468.06f, 4.774429f, 142.5602f),
			new Vector4(-1129.686f, -1761.421f, 3.409448f, 124.2734f),
			new Vector4(-1440.332f, -1540.321f, 1.130228f, 20.97221f),
			new Vector4(-1456.176f, -1258.751f, 2.742244f, 167.0078f),
			new Vector4(-1812.258f, -1241.323f, 12.7575f, 88.93974f),
			new Vector4(-1716.942f, -810.8776f, 9.085096f, 132.4023f),
			new Vector4(-1860.212f, -621.9059f, 10.96767f, 319.4534f),
			new Vector4(-1664.277f, -243.4555f, 54.5129f, 149.8075f),
			new Vector4(-1299.323f, -209.5251f, 60.36734f, 33.60303f),
			new Vector4(-938.2299f, -167.261f, 41.60231f, 297.7702f),
			new Vector4(-499.7856f, 45.54996f, 56.21747f, 353.1602f),
			new Vector4(-247.8049f, -139.1885f, 84.80735f, 240.3721f),
			new Vector4(-161.2345f, -310.3577f, 81.22681f, 23.41458f),
			new Vector4(63.40522f, -342.0492f, 66.87309f, 245.8033f),
			new Vector4(183.1316f, -243.2479f, 64.72156f, 67.53889f),
			new Vector4(372.6071f, 29.56528f, 91.71777f, 279.8798f),
			new Vector4(512.0079f, -49.40693f, 88.59159f, 151.5999f),
			new Vector4(420.3635f, -1158.278f, 28.96838f, 92.32964f),
			new Vector4(457.1662f, -1356.513f, 43.29359f, 136.7715f),
			new Vector4(94.11004f, -1616.178f, 29.20332f, 124.3603f),
			new Vector4(93.29221f, -1788.22f, 34.98935f, 140.8764f),
			new Vector4(568.103f, -1957.584f, 24.47468f, 344.071f),
			new Vector4(657.4419f, -1698.019f, 9.201172f, 358.0936f),
			new Vector4(626.8488f, -1474.052f, 9.414593f, 14.60888f),
			new Vector4(618.5265f, -966.8715f, 10.28885f, 0.3928658f),
			new Vector4(637.5546f, -426.7994f, 24.16644f, 0.0235757f),
			new Vector4(964.1567f, -373.1064f, 44.46226f, 298.6479f)
		};

		public List<Vector4> Cars_Spawns = new List<Vector4>
		{
			new Vector4(2908.846f, 4480.214f, 47.7722f, 137.5336f),
			new Vector4(2981.414f, 3490.744f, 71.05273f, 254.8869f),
			new Vector4(2616.96f, 3265.509f, 54.89052f, 294.1208f),
			new Vector4(1578.192f, 2906.326f, 56.54894f, 219.5349f),
			new Vector4(1532.245f, 2190.647f, 78.77408f, 35.65417f),
			new Vector4(1091.855f, 2644.071f, 37.58302f, 359.7924f),
			new Vector4(-212.2445f, 4210.768f, 44.3439f, 25.14014f),
			new Vector4(-736.3169f, 4416.67f, 20.95136f, 83.47981f),
			new Vector4(-1851.464f, 4507.122f, 21.63104f, 101.4627f),
			new Vector4(-2123.508f, 4552.024f, 4.34937f, 104.3607f),
			new Vector4(-807.167f, 5409.357f, 33.71852f, 61.55654f),
			new Vector4(1366.8f, 4383.65f, 44.01088f, 181.6175f),
			new Vector4(2475.907f, 1589.98f, 32.38488f, 266.7451f),
			new Vector4(2535.375f, -273.7903f, 92.6661f, 199.995f),
			new Vector4(1292.071f, -2553.569f, 43.41571f, 13.60728f),
			new Vector4(-66.93775f, 1888.828f, 196.0829f, 190.1995f),
			new Vector4(-452.2092f, 1589.837f, 358.6433f, 232.9519f),
			new Vector4(-1911.316f, 2034.169f, 140.4099f, 233.4053f),
			new Vector4(757.9188f, 1286.704f, 359.9677f, 268.9373f),
			new Vector4(1408.016f, 1118.709f, 114.5008f, 89.21499f),
			new Vector4(1527.139f, 786.814f, 77.11494f, 63.04953f),
			new Vector4(673.7578f, 615.3953f, 128.5822f, 249.2549f),
			new Vector4(1523.366f, 1733.912f, 109.9736f, 108.4943f),
			new Vector4(1217.843f, 1841.862f, 78.72816f, 125.6681f),
			new Vector4(157.7691f, -1487.893f, 28.49598f, 131.2425f),
			new Vector4(1202.052f, -3241.076f, 5.339464f, 1.892946f),
			new Vector4(817.3223f, -3340.449f, 5.273727f, 180.5177f),
			new Vector4(763.8843f, -3158.977f, 5.274129f, 271.8171f),
			new Vector4(788.1002f, -2917.837f, 5.246607f, 271.2767f),
			new Vector4(603.4117f, -3247.946f, 5.459314f, 0.4186251f),
			new Vector4(499.3233f, -2958.162f, 5.436298f, 272.0434f),
			new Vector4(461.0945f, -2691.165f, 5.448327f, 182.626f),
			new Vector4(144.2452f, -2384.869f, 5.39854f, 268.8786f),
			new Vector4(-164.4044f, -2707.797f, 5.400749f, 270.1183f),
			new Vector4(-563.0228f, -2815.282f, 5.392111f, 294.1218f),
			new Vector4(756.6918f, -1673.629f, 28.63235f, 353.0954f),
			new Vector4(600.8567f, -1865.507f, 24.04297f, 144.0083f),
			new Vector4(538.9362f, -1984.382f, 24.0317f, 121.5374f),
			new Vector4(206.4445f, -2006.268f, 18.13984f, 229.5016f),
			new Vector4(-47.12731f, -2019.179f, 17.32954f, 21.89431f),
			new Vector4(-557.6726f, -1799.578f, 21.91441f, 332.6271f),
			new Vector4(-854.6203f, -1255.208f, 4.312304f, 231.3882f),
			new Vector4(-717.3134f, -1144.511f, 9.925322f, 128.0189f),
			new Vector4(-881.5139f, -1138.198f, 5.036555f, 31.19304f),
			new Vector4(-1035.464f, -1599.298f, 4.211222f, 34.45655f),
			new Vector4(-1072.786f, -1654.813f, 3.74941f, 128.5321f),
			new Vector4(-1101.33f, -1497.189f, 4.13877f, 212.3594f),
			new Vector4(-1277.871f, -1311.96f, 3.327076f, 108.7988f),
			new Vector4(-1321.771f, -1172.094f, 4.087599f, 358.8043f),
			new Vector4(-1216.538f, -1053.187f, 7.701442f, 19.44256f),
			new Vector4(-1040.784f, -1005.831f, 1.463654f, 208.437f),
			new Vector4(-831.2654f, -745.8225f, 22.76245f, 90.46847f),
			new Vector4(-607.8167f, -1035.015f, 21.10011f, 91.19731f),
			new Vector4(-475.08f, -1024.391f, 22.75248f, 0.7269359f),
			new Vector4(-472.3013f, -725.6422f, 44.33201f, 178.593f),
			new Vector4(-1177.222f, -902.5812f, 12.86451f, 300.0305f),
			new Vector4(-957.1473f, -885.1205f, 1.317433f, 219.0338f),
			new Vector4(-440.0172f, -739.3041f, 30.65382f, 178.5417f),
			new Vector4(-309.8249f, -741.0294f, 52.55853f, 157.8587f),
			new Vector4(58.92454f, -458.963f, 39.21114f, 73.43995f),
			new Vector4(-370.9523f, -463.521f, 30.03916f, 85.33768f),
			new Vector4(-771.7932f, -444.0047f, 35.51103f, 96.25507f),
			new Vector4(-1190.621f, -722.1213f, 20.45618f, 219.3277f),
			new Vector4(157.7676f, -1647.246f, 28.64582f, 206.538f),
			new Vector4(229.9702f, -1657.769f, 28.74656f, 318.0319f),
			new Vector4(232.7438f, -1757.181f, 28.36384f, 228.1754f),
			new Vector4(481.4136f, -1395.82f, 28.66015f, 177.7976f),
			new Vector4(740.5902f, -533.8474f, 26.32532f, 151.883f),
			new Vector4(829.3095f, -807.0326f, 26.29145f, 10.69474f),
			new Vector4(694.1859f, -1022.682f, 21.92185f, 175.9884f),
			new Vector4(946.7417f, -1209.69f, 25.17342f, 94.3028f),
			new Vector4(974.6949f, -1517.474f, 30.41445f, 179.6585f),
			new Vector4(1146.672f, -1618.064f, 34.07173f, 273.3357f),
			new Vector4(1155.344f, -1360.133f, 34.05093f, 266.6904f),
			new Vector4(1317.695f, -1664.607f, 50.57969f, 34.74467f),
			new Vector4(1346.458f, -1850.223f, 56.48981f, 162.6819f),
			new Vector4(1098.539f, -1981.18f, 30.31761f, 57.16496f),
			new Vector4(906.5049f, -1971.492f, 29.90759f, 84.19881f),
			new Vector4(874.6549f, -2051.599f, 29.80617f, 175.2474f),
			new Vector4(834.4218f, -2161.333f, 29.29096f, 180.7712f),
			new Vector4(956.0196f, -2365.996f, 29.8802f, 256.5155f),
			new Vector4(901.5684f, -2550.076f, 27.76551f, 77.89549f),
			new Vector4(507.0944f, -2121.363f, 5.268847f, 303.3767f),
			new Vector4(176.092f, -2215.317f, 5.387786f, 358.8554f),
			new Vector4(167.1272f, -1925.676f, 20.36752f, 229.7465f),
			new Vector4(171.9752f, -1818.356f, 27.97451f, 44.94935f),
			new Vector4(15.95158f, -1823.567f, 24.25424f, 137.214f),
			new Vector4(-199.1619f, -1734.392f, 32.00651f, 319.7388f),
			new Vector4(-78.25626f, -1396.733f, 28.67435f, 179.6548f),
			new Vector4(485.5253f, -1179.351f, 28.62285f, 0.1547307f),
			new Vector4(341.0377f, -1086.173f, 28.7325f, 0.1558865f),
			new Vector4(152.5866f, -1055.932f, 28.57124f, 339.7046f),
			new Vector4(14.36035f, -1328.859f, 28.62285f, 177.7036f),
			new Vector4(-2177.709f, 4452.652f, 61.9869f, 161.0061f),
			new Vector4(-2169.968f, 4278.318f, 48.42154f, 147.5517f),
			new Vector4(-2475.552f, 3775.905f, 18.19005f, 159.4472f),
			new Vector4(-2220.86f, 3481.646f, 29.70178f, 181.7366f),
			new Vector4(-2525.858f, 2340.802f, 32.59359f, 215.3646f),
			new Vector4(-2215.43f, 2316.192f, 32.17837f, 292.4376f),
			new Vector4(-1156.896f, 2663.905f, 17.62646f, 226.78f),
			new Vector4(-273.5474f, 2885.497f, 45.33754f, 260.8311f),
			new Vector4(-101.803f, 2796.353f, 52.87008f, 274.5969f),
			new Vector4(41.84475f, 2801.608f, 57.41041f, 141.1003f),
			new Vector4(204.4195f, 3060.412f, 42.14877f, 12.39692f),
			new Vector4(65.84356f, 3601.171f, 39.2322f, 77.15089f),
			new Vector4(629.6224f, 3504.465f, 33.51008f, 274.4066f),
			new Vector4(1370.057f, 3183.308f, 40.33274f, 283.3253f),
			new Vector4(2008.213f, 3053.379f, 46.58578f, 330.5495f),
			new Vector4(2835.145f, 1629.389f, 23.99063f, 111.6543f),
			new Vector4(2546.973f, 342.6437f, 107.9964f, 267.2037f),
			new Vector4(1205.619f, -1230.354f, 34.78842f, 270.2647f),
			new Vector4(1108.544f, -779.7579f, 57.7952f, 357.9726f),
			new Vector4(1129.569f, -442.6757f, 66.02184f, 165.4753f),
			new Vector4(1136.325f, -310.1779f, 68.50471f, 8.988988f),
			new Vector4(1053.946f, -139.4402f, 73.65582f, 58.4093f),
			new Vector4(1111.981f, 217.5597f, 80.52344f, 325.974f),
			new Vector4(935.1271f, 148.0041f, 80.36306f, 243.9015f),
			new Vector4(709.3184f, 256.5184f, 92.95065f, 153.7664f),
			new Vector4(615.2566f, 89.01411f, 91.49998f, 161.3029f),
			new Vector4(543.0467f, -224.8709f, 50.44188f, 129.959f),
			new Vector4(308.2559f, -176.8983f, 57.01552f, 66.69116f),
			new Vector4(135.7572f, -122.4751f, 54.32469f, 67.40604f),
			new Vector4(269.5999f, -322.4406f, 44.4519f, 250.9327f),
			new Vector4(703.1121f, -292.371f, 58.64838f, 289.7591f),
			new Vector4(694.3516f, -671.3352f, 25.71703f, 179.7069f),
			new Vector4(497.818f, -635.072f, 24.40111f, 261.2879f),
			new Vector4(528.7947f, -1035.573f, 27.8595f, 173.5418f),
			new Vector4(486.3365f, -1179.841f, 28.82422f, 3.039702f),
			new Vector4(340.4897f, -1095.206f, 28.87381f, 357.5369f),
			new Vector4(309.192f, -1003.099f, 28.85263f, 85.5532f),
			new Vector4(137.3975f, -1154.345f, 28.84707f, 91.05807f),
			new Vector4(12.08414f, -1035.506f, 28.7389f, 68.90684f),
			new Vector4(59.78284f, -835.644f, 30.51324f, 68.95349f),
			new Vector4(258.4442f, -747.299f, 34.12879f, 340.916f),
			new Vector4(476.2806f, -545.1654f, 28.03308f, 208.2147f),
			new Vector4(888.5359f, -923.844f, 25.81598f, 99.17569f),
			new Vector4(732.5098f, -941.3585f, 24.27951f, 89.40993f),
			new Vector4(743.9838f, -1100.532f, 21.862f, 267.8116f),
			new Vector4(891.6705f, -1158.756f, 24.43472f, 270.4106f),
			new Vector4(914.8229f, -1268.292f, 25.05478f, 31.9668f),
			new Vector4(743.0217f, -1335.26f, 25.77145f, 268.6621f),
			new Vector4(847.6765f, -1409.504f, 25.6329f, 85.03105f),
			new Vector4(848.4387f, 2203.964f, 50.78771f, 9.136927f),
			new Vector4(360.6236f, 2568.625f, 43.05188f, 29.53666f),
			new Vector4(589.7719f, 2737.669f, 41.5407f, 181.4949f),
			new Vector4(310.45f, 2877.458f, 43.02768f, 119.9341f),
			new Vector4(322.7816f, 3415.454f, 36.19878f, 254.9123f),
			new Vector4(404.1619f, 3520.123f, 33.76699f, 4.263566f),
			new Vector4(887.3103f, 3660.539f, 32.37645f, 177.6296f),
			new Vector4(977.8574f, 3614.968f, 32.22903f, 357.434f),
			new Vector4(1278.666f, 3623.743f, 32.57354f, 2.277754f),
			new Vector4(1504.384f, 3761.483f, 33.52034f, 203.5065f),
			new Vector4(2062.533f, 3761.124f, 32.06534f, 247.3972f),
			new Vector4(2432.984f, 4021.697f, 36.44216f, 252.9356f),
			new Vector4(2504.39f, 4196.662f, 39.475f, 148.6891f),
			new Vector4(1814.097f, 4591.259f, 36.21739f, 165.361f),
			new Vector4(1704.247f, 4803.374f, 41.32481f, 92.71139f),
			new Vector4(1681.303f, 4919.767f, 41.62576f, 24.19528f),
			new Vector4(1716.238f, 4657.231f, 43.03181f, 98.20377f),
			new Vector4(1636.84f, 4837.435f, 41.47603f, 184.576f),
			new Vector4(1701.086f, 6412.073f, 32.40558f, 64.0333f),
			new Vector4(1502.538f, 6504.97f, 20.94793f, 69.68187f),
			new Vector4(1312.534f, 6511.95f, 19.3231f, 141.7883f),
			new Vector4(1073.955f, 6505.541f, 20.54107f, 87.40759f),
			new Vector4(178.6694f, 6571.225f, 31.37777f, 110.04f),
			new Vector4(-88.54631f, 6555.477f, 31.02403f, 226.0934f),
			new Vector4(-0.7573378f, 6437.238f, 30.95817f, 226.5271f),
			new Vector4(-576.906f, 5713.821f, 36.88632f, 162.607f),
			new Vector4(-747.5623f, 5816.06f, 16.9399f, 161.1277f),
			new Vector4(1779.292f, 2398.063f, 45.38816f, 122.2725f),
			new Vector4(1092.237f, 2106.475f, 52.91426f, 52.47581f),
			new Vector4(848.4387f, 2203.964f, 50.78771f, 9.136927f),
			new Vector4(360.6236f, 2568.625f, 43.05188f, 29.53666f),
			new Vector4(589.7719f, 2737.669f, 41.5407f, 181.4949f),
			new Vector4(310.45f, 2877.458f, 43.02768f, 119.9341f),
			new Vector4(322.7816f, 3415.454f, 36.19878f, 254.9123f),
			new Vector4(404.1619f, 3520.123f, 33.76699f, 4.263566f),
			new Vector4(887.3103f, 3660.539f, 32.37645f, 177.6296f),
			new Vector4(977.8574f, 3614.968f, 32.22903f, 357.434f),
			new Vector4(1278.666f, 3623.743f, 32.57354f, 2.277754f),
			new Vector4(1504.384f, 3761.483f, 33.52034f, 203.5065f),
			new Vector4(2062.533f, 3761.124f, 32.06534f, 247.3972f),
			new Vector4(2432.984f, 4021.697f, 36.44216f, 252.9356f),
			new Vector4(2504.39f, 4196.662f, 39.475f, 148.6891f),
			new Vector4(1814.097f, 4591.259f, 36.21739f, 165.361f),
			new Vector4(1704.247f, 4803.374f, 41.32481f, 92.71139f),
			new Vector4(1681.303f, 4919.767f, 41.62576f, 24.19528f),
			new Vector4(1716.238f, 4657.231f, 43.03181f, 98.20377f),
			new Vector4(1636.84f, 4837.435f, 41.47603f, 184.576f),
			new Vector4(1701.086f, 6412.073f, 32.40558f, 64.0333f),
			new Vector4(1502.538f, 6504.97f, 20.94793f, 69.68187f),
			new Vector4(1312.534f, 6511.95f, 19.3231f, 141.7883f),
			new Vector4(1073.955f, 6505.541f, 20.54107f, 87.40759f),
			new Vector4(178.6694f, 6571.225f, 31.37777f, 110.04f),
			new Vector4(-88.54631f, 6555.477f, 31.02403f, 226.0934f),
			new Vector4(-0.7573378f, 6437.238f, 30.95817f, 226.5271f),
			new Vector4(-576.906f, 5713.821f, 36.88632f, 162.607f),
			new Vector4(-747.5623f, 5816.06f, 16.9399f, 161.1277f),
			new Vector4(1708.317f, 3593.723f, 34.95748f, 26.52231f),
			new Vector4(1835.768f, 3950.648f, 32.61317f, 95.33018f),
			new Vector4(2097.435f, 3567.135f, 41.41328f, 304.0204f),
			new Vector4(2046.771f, 3391.363f, 44.75264f, 200.9585f),
			new Vector4(2177.119f, 3314.89f, 45.55929f, 296.8406f),
			new Vector4(2331.047f, 3143.374f, 47.69791f, 173.3829f),
			new Vector4(2560.908f, 2738.047f, 42.65128f, 301.4293f),
			new Vector4(2522.732f, 2609.18f, 37.47763f, 291.8714f),
			new Vector4(2373.057f, 2618.042f, 46.05958f, 68.62781f),
			new Vector4(-404.255371f, -2732.21777f, 5.90163946f, 145.5913f),
			new Vector4(-1145.663f, -2864.267f, 13.8450565f, -31.7906189f),
			new Vector4(-745.5079f, -1468.69775f, 4.89958429f, -40.2942963f),
			new Vector4(299.507324f, -1453.33679f, 46.4085541f, -37.8503227f),
			new Vector4(1810.254f, -1290.63647f, 95.8732758f, 104.258789f),
			new Vector4(580.253967f, 11.7617826f, 103.621346f, -179.082764f),
			new Vector4(-1742.21094f, -834.4876f, 8.478429f, 81.62008f),
			new Vector4(-3138.4873f, 1280.94592f, 20.0484962f, -71.2746353f),
			new Vector4(-2754.5918f, 2380.25513f, 2.66715479f, -7.547337f),
			new Vector4(1794.58887f, 3244.25024f, 42.5050354f, 12.1940432f),
			new Vector4(2753.208f, 1347.94238f, 24.4228382f, -3.492491f),
			new Vector4(714.434753f, 4174.83838f, 40.60771f, -69.6127243f),
			new Vector4(340.643829f, 3566.395f, 33.3385658f, -2.03787684f),
			new Vector4(2074.617f, 4752.39941f, 41.371067f, 48.35261f),
			new Vector4(1601.97412f, 6624.61963f, 15.6700392f, 149.19812f),
			new Vector4(-233.370575f, 6258.77f, 31.3686733f, -127.686073f),
			new Vector4(-474.86087f, 5989.07f, 31.2356758f, -44.69158f),
			new Vector4(-1579.10461f, 5159.47559f, 19.6309223f, -162.948486f)
		};

		public List<string> Posssible_Planes = new List<string> { "VELUM", "LUXOR", "VESTRA" };

		public List<string> Posssible_Boats = new List<string> { "MARQUIS", "TORO", "JETMAX" };

		public List<string> Posssible_Helicopters = new List<string> { "BUZZARD2", "SWIFT", "FROGGER" };

		public List<string> Posssible_Cars = new List<string> { "T20", "OMNIS", "NERO" };

		private List<string> Lines_Planes = new List<string>();

		private List<string> Lines_Helicopters = new List<string>();

		private List<string> Lines_Boats = new List<string>();

		private List<string> Lines_Cars = new List<string>();

		public Vehicle LocatedVehicle;

		public static int DrawCallsPerFrame = 500;

		public static Vector2 DrawAspect = new Vector2(1.1f, 1f);

		public string SelectedButtonIndex;

		public List<ButtonData> ButtonIndex = new List<ButtonData>
		{
			new ButtonData(0, "INPUT_NEXT_CAMERA", "V", "BACK"),
			new ButtonData(1, "INPUT_LOOK_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(2, "INPUT_LOOK_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(3, "INPUT_LOOK_UP_ONLY", "(NONE)", "RIGHT STICK"),
			new ButtonData(4, "INPUT_LOOK_DOWN_ONLY", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(5, "INPUT_LOOK_LEFT_ONLY", "(NONE)", "RIGHT STICK"),
			new ButtonData(6, "INPUT_LOOK_RIGHT_ONLY", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(7, "INPUT_CINEMATIC_SLOWMO", "(NONE)", "R3"),
			new ButtonData(8, "INPUT_SCRIPTED_FLY_UD", "S", "LEFT STICK"),
			new ButtonData(9, "INPUT_SCRIPTED_FLY_LR", "D", "LEFT STICK"),
			new ButtonData(10, "INPUT_SCRIPTED_FLY_ZUP", "PAGEUP", "LT"),
			new ButtonData(11, "INPUT_SCRIPTED_FLY_ZDOWN", "PAGEDOWN", "RT"),
			new ButtonData(12, "INPUT_WEAPON_WHEEL_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(13, "INPUT_WEAPON_WHEEL_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(14, "INPUT_WEAPON_WHEEL_NEXT", "SCROLLWHEEL DOWN", "DPAD RIGHT"),
			new ButtonData(15, "INPUT_WEAPON_WHEEL_PREV", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(16, "INPUT_SELECT_NEXT_WEAPON", "SCROLLWHEEL", "DOWN (NONE)"),
			new ButtonData(17, "INPUT_SELECT_PREV_WEAPON", "SCROLLWHEEL", "UP (NONE)"),
			new ButtonData(18, "INPUT_SKIP_CUTSCENE", "ENTER / LEFT MOUSE BUTTON/SPACEBAR", "A"),
			new ButtonData(19, "INPUT_CHARACTER_WHEEL", "LEFT ALT", "DPAD DOWN"),
			new ButtonData(20, "INPUT_MULTIPLAYER_INFO", "Z", "DPAD DOWN"),
			new ButtonData(21, "INPUT_SPRINT", "LEFT", "SHIFT A"),
			new ButtonData(22, "INPUT_JUMP", "SPACEBAR", "X"),
			new ButtonData(23, "INPUT_ENTER", "F", "Y"),
			new ButtonData(24, "INPUT_ATTACK", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(25, "INPUT_AIM", "RIGHT MOUSE", "BUTTON LT"),
			new ButtonData(26, "INPUT_LOOK_BEHIND", "C", "R3"),
			new ButtonData(27, "INPUT_PHONE", "ARROW UP / SCROLLWHEEL BUTTON(PRESS)DPAD", "UP"),
			new ButtonData(28, "INPUT_SPECIAL_ABILITY", "(NONE)", "L3"),
			new ButtonData(29, "INPUT_SPECIAL_ABILITY_SECONDARY", "B", "R3"),
			new ButtonData(30, "INPUT_MOVE_LR", "D", "LEFT STICK"),
			new ButtonData(31, "INPUT_MOVE_UD", "S", "LEFT STICK"),
			new ButtonData(32, "INPUT_MOVE_UP_ONLY", "W", "LEFT STICK"),
			new ButtonData(33, "INPUT_MOVE_DOWN_ONLY", "S", "LEFT STICK"),
			new ButtonData(34, "INPUT_MOVE_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(35, "INPUT_MOVE_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(36, "INPUT_DUCK", "LEFT", "CTRL L3"),
			new ButtonData(37, "INPUT_SELECT_WEAPON", "TAB", "LB"),
			new ButtonData(38, "INPUT_PICKUP", "E", "LB"),
			new ButtonData(39, "INPUT_SNIPER_ZOOM", "[LEFT", "STICK"),
			new ButtonData(40, "INPUT_SNIPER_ZOOM_IN_ONLY]", "LEFT", "STICK"),
			new ButtonData(41, "INPUT_SNIPER_ZOOM_OUT_ONLY", "[LEFT", "STICK"),
			new ButtonData(42, "INPUT_SNIPER_ZOOM_IN_SECONDARY]", "DPAD", "UP"),
			new ButtonData(43, "INPUT_SNIPER_ZOOM_OUT_SECONDARY", "[DPAD", "DOWN"),
			new ButtonData(44, "INPUT_COVER", "Q", "RB"),
			new ButtonData(45, "INPUT_RELOAD", "R", "B"),
			new ButtonData(46, "INPUT_TALK", "E", "DPAD RIGHT"),
			new ButtonData(47, "INPUT_DETONATE", "G", "DPAD LEFT"),
			new ButtonData(48, "INPUT_HUD_SPECIAL", "Z", "DPAD DOWN"),
			new ButtonData(49, "INPUT_ARREST", "F", "Y"),
			new ButtonData(50, "INPUT_ACCURATE_AIM", "SCROLLWHEEL", "DOWN R3"),
			new ButtonData(51, "INPUT_CONTEXT", "E", "DPAD RIGHT"),
			new ButtonData(52, "INPUT_CONTEXT_SECONDARY", "Q", "DPAD LEFT"),
			new ButtonData(53, "INPUT_WEAPON_SPECIAL", "(NONE)", "Y"),
			new ButtonData(54, "INPUT_WEAPON_SPECIAL_TWO", "E", "DPAD RIGHT"),
			new ButtonData(55, "INPUT_DIVE", "SPACEBAR", "RB"),
			new ButtonData(56, "INPUT_DROP_WEAPON", "F9", "Y"),
			new ButtonData(57, "INPUT_DROP_AMMO", "F10", "B"),
			new ButtonData(58, "INPUT_THROW_GRENADE", "G", "DPAD LEFT"),
			new ButtonData(59, "INPUT_VEH_MOVE_LR", "D", "LEFT STICK"),
			new ButtonData(60, "INPUT_VEH_MOVE_UD", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(61, "INPUT_VEH_MOVE_UP_ONLY", "LEFT SHIFT", "LEFT STICK"),
			new ButtonData(62, "INPUT_VEH_MOVE_DOWN_ONLY", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(63, "INPUT_VEH_MOVE_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(64, "INPUT_VEH_MOVE_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(65, "INPUT_VEH_SPECIAL", "(NONE)", "(NONE)"),
			new ButtonData(66, "INPUT_VEH_GUN_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(67, "INPUT_VEH_GUN_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(68, "INPUT_VEH_AIM", "RIGHT MOUSE", "BUTTON LB"),
			new ButtonData(69, "INPUT_VEH_ATTACK", "LEFT MOUSE", "BUTTON RB"),
			new ButtonData(70, "INPUT_VEH_ATTACK2", "RIGHT MOUSE", "BUTTON A"),
			new ButtonData(71, "INPUT_VEH_ACCELERATE", "W", "RT"),
			new ButtonData(72, "INPUT_VEH_BRAKE", "S", "LT"),
			new ButtonData(73, "INPUT_VEH_DUCK", "X", "A"),
			new ButtonData(74, "INPUT_VEH_HEADLIGHT", "H", "DPAD RIGHT"),
			new ButtonData(75, "INPUT_VEH_EXIT", "F", "Y"),
			new ButtonData(76, "INPUT_VEH_HANDBRAKE", "SPACEBAR", "RB"),
			new ButtonData(77, "INPUT_VEH_HOTWIRE_LEFT", "W", "LT"),
			new ButtonData(78, "INPUT_VEH_HOTWIRE_RIGHT", "S", "RT"),
			new ButtonData(79, "INPUT_VEH_LOOK_BEHIND", "C", "R3"),
			new ButtonData(80, "INPUT_VEH_CIN_CAM", "R", "B"),
			new ButtonData(81, "INPUT_VEH_NEXT_RADIO", ".", "(NONE)"),
			new ButtonData(82, "INPUT_VEH_PREV_RADIO", ",", "(NONE)"),
			new ButtonData(83, "INPUT_VEH_NEXT_RADIO_TRACK", "=", "(NONE)"),
			new ButtonData(84, "INPUT_VEH_PREV_RADIO_TRACK", "-", "(NONE)"),
			new ButtonData(85, "INPUT_VEH_RADIO_WHEEL", "Q", "DPAD LEFT"),
			new ButtonData(86, "INPUT_VEH_HORN", "E", "L3"),
			new ButtonData(87, "INPUT_VEH_FLY_THROTTLE_UP", "W", "RT"),
			new ButtonData(88, "INPUT_VEH_FLY_THROTTLE_DOWN", "S", "LT"),
			new ButtonData(89, "INPUT_VEH_FLY_YAW_LEFT", "A", "LB"),
			new ButtonData(90, "INPUT_VEH_FLY_YAW_RIGHT", "D", "RB"),
			new ButtonData(91, "INPUT_VEH_PASSENGER_AIM", "RIGHT MOUSE", "BUTTON LT"),
			new ButtonData(92, "INPUT_VEH_PASSENGER_ATTACK", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(93, "INPUT_VEH_SPECIAL_ABILITY_FRANKLIN", "(NONE)", "R3"),
			new ButtonData(94, "INPUT_VEH_STUNT_UD", "(NONE)", "(NONE)"),
			new ButtonData(95, "INPUT_VEH_CINEMATIC_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(96, "INPUT_VEH_CINEMATIC_UP_ONLY", "NUMPAD- /", "SCROLLWHEEL UP(NONE)"),
			new ButtonData(97, "INPUT_VEH_CINEMATIC_DOWN_ONLY", "NUMPAD+ /", "SCROLLWHEEL DOWN(NONE)"),
			new ButtonData(98, "INPUT_VEH_CINEMATIC_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(99, "INPUT_VEH_SELECT_NEXT_WEAPON", "SCROLLWHEEL", "UP X"),
			new ButtonData(100, "INPUT_VEH_SELECT_PREV_WEAPON", "[", "(NONE)"),
			new ButtonData(101, "INPUT_VEH_ROOF", "H", "DPAD RIGHT"),
			new ButtonData(102, "INPUT_VEH_JUMP", "SPACEBAR", "RB"),
			new ButtonData(103, "INPUT_VEH_GRAPPLING_HOOK", "E", "DPAD RIGHT"),
			new ButtonData(104, "INPUT_VEH_SHUFFLE", "H", "DPAD RIGHT"),
			new ButtonData(105, "INPUT_VEH_DROP_PROJECTILE", "X", "A"),
			new ButtonData(106, "INPUT_VEH_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(107, "INPUT_VEH_FLY_ROLL_LR", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(108, "INPUT_VEH_FLY_ROLL_LEFT_ONLY", "NUMPAD 4", "LEFT STICK"),
			new ButtonData(109, "INPUT_VEH_FLY_ROLL_RIGHT_ONLY", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(110, "INPUT_VEH_FLY_PITCH_UD", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(111, "INPUT_VEH_FLY_PITCH_UP_ONLY", "NUMPAD 8", "LEFT STICK"),
			new ButtonData(112, "INPUT_VEH_FLY_PITCH_DOWN_ONLY", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(113, "INPUT_VEH_FLY_UNDERCARRIAGE", "G", "L3"),
			new ButtonData(114, "INPUT_VEH_FLY_ATTACK", "RIGHT MOUSE", "BUTTON A"),
			new ButtonData(115, "INPUT_VEH_FLY_SELECT_NEXT_WEAPON", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(116, "INPUT_VEH_FLY_SELECT_PREV_WEAPON", "[", "(NONE)"),
			new ButtonData(117, "INPUT_VEH_FLY_SELECT_TARGET_LEFT", "NUMPAD", "7 LB"),
			new ButtonData(118, "INPUT_VEH_FLY_SELECT_TARGET_RIGHT", "NUMPAD", "9 RB"),
			new ButtonData(119, "INPUT_VEH_FLY_VERTICAL_FLIGHT_MODE", "E", "DPAD RIGHT"),
			new ButtonData(120, "INPUT_VEH_FLY_DUCK", "X", "A"),
			new ButtonData(121, "INPUT_VEH_FLY_ATTACK_CAMERA", "INSERT", "R3"),
			new ButtonData(122, "INPUT_VEH_FLY_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(123, "INPUT_VEH_SUB_TURN_LR", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(124, "INPUT_VEH_SUB_TURN_LEFT_ONLY", "NUMPAD 4", "LEFT STICK"),
			new ButtonData(125, "INPUT_VEH_SUB_TURN_RIGHT_ONLY", "NUMPAD 6", "LEFT STICK"),
			new ButtonData(126, "INPUT_VEH_SUB_PITCH_UD", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(127, "INPUT_VEH_SUB_PITCH_UP_ONLY", "NUMPAD 8", "LEFT STICK"),
			new ButtonData(128, "INPUT_VEH_SUB_PITCH_DOWN_ONLY", "NUMPAD 5", "LEFT STICK"),
			new ButtonData(129, "INPUT_VEH_SUB_THROTTLE_UP", "W", "RT"),
			new ButtonData(130, "INPUT_VEH_SUB_THROTTLE_DOWN", "S", "LT"),
			new ButtonData(131, "INPUT_VEH_SUB_ASCEND", "LEFT", "SHIFT X"),
			new ButtonData(132, "INPUT_VEH_SUB_DESCEND", "LEFT", "CTRL A"),
			new ButtonData(133, "INPUT_VEH_SUB_TURN_HARD_LEFT", "A", "LB"),
			new ButtonData(134, "INPUT_VEH_SUB_TURN_HARD_RIGHT", "D", "RB"),
			new ButtonData(135, "INPUT_VEH_SUB_MOUSE_CONTROL_OVERRIDE", "LEFT", "MOUSE BUTTON(NONE)"),
			new ButtonData(136, "INPUT_VEH_PUSHBIKE_PEDAL", "W", "A"),
			new ButtonData(137, "INPUT_VEH_PUSHBIKE_SPRINT", "CAPSLOCK", "A"),
			new ButtonData(138, "INPUT_VEH_PUSHBIKE_FRONT_BRAKE", "Q", "LT"),
			new ButtonData(139, "INPUT_VEH_PUSHBIKE_REAR_BRAKE", "S", "RT"),
			new ButtonData(140, "INPUT_MELEE_ATTACK_LIGHT", "R", "B"),
			new ButtonData(141, "INPUT_MELEE_ATTACK_HEAVY", "Q", "A"),
			new ButtonData(142, "INPUT_MELEE_ATTACK_ALTERNATE", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(143, "INPUT_MELEE_BLOCK", "SPACEBAR", "X"),
			new ButtonData(144, "INPUT_PARACHUTE_DEPLOY", "F / LEFT MOUSE BUTTON", "Y"),
			new ButtonData(145, "INPUT_PARACHUTE_DETACH", "F", "Y"),
			new ButtonData(146, "INPUT_PARACHUTE_TURN_LR", "D", "LEFT STICK"),
			new ButtonData(147, "INPUT_PARACHUTE_TURN_LEFT_ONLY", "A", "LEFT STICK"),
			new ButtonData(148, "INPUT_PARACHUTE_TURN_RIGHT_ONLY", "D", "LEFT STICK"),
			new ButtonData(149, "INPUT_PARACHUTE_PITCH_UD", "S", "LEFT STICK"),
			new ButtonData(150, "INPUT_PARACHUTE_PITCH_UP_ONLY", "W", "LEFT STICK"),
			new ButtonData(151, "INPUT_PARACHUTE_PITCH_DOWN_ONLY", "S", "LEFT STICK"),
			new ButtonData(152, "INPUT_PARACHUTE_BRAKE_LEFT", "Q", "LB"),
			new ButtonData(153, "INPUT_PARACHUTE_BRAKE_RIGHT", "E", "RB"),
			new ButtonData(154, "INPUT_PARACHUTE_SMOKE", "X", "A"),
			new ButtonData(155, "INPUT_PARACHUTE_PRECISION_LANDING", "LEFT", "SHIFT(NONE)"),
			new ButtonData(156, "INPUT_MAP", "(NONE)", "(NONE)"),
			new ButtonData(157, "INPUT_SELECT_WEAPON_UNARMED", "1", "(NONE)"),
			new ButtonData(158, "INPUT_SELECT_WEAPON_MELEE", "2", "(NONE)"),
			new ButtonData(159, "INPUT_SELECT_WEAPON_HANDGUN", "6", "(NONE)"),
			new ButtonData(160, "INPUT_SELECT_WEAPON_SHOTGUN", "3", "(NONE)"),
			new ButtonData(161, "INPUT_SELECT_WEAPON_SMG", "7", "(NONE)"),
			new ButtonData(162, "INPUT_SELECT_WEAPON_AUTO_RIFLE", "8", "(NONE)"),
			new ButtonData(163, "INPUT_SELECT_WEAPON_SNIPER", "9", "(NONE)"),
			new ButtonData(164, "INPUT_SELECT_WEAPON_HEAVY", "4", "(NONE)"),
			new ButtonData(165, "INPUT_SELECT_WEAPON_SPECIAL", "5", "(NONE)"),
			new ButtonData(166, "INPUT_SELECT_CHARACTER_MICHAEL", "F5", "(NONE)"),
			new ButtonData(167, "INPUT_SELECT_CHARACTER_FRANKLIN", "F6", "(NONE)"),
			new ButtonData(168, "INPUT_SELECT_CHARACTER_TREVOR", "F7", "(NONE)"),
			new ButtonData(169, "INPUT_SELECT_CHARACTER_MULTIPLAYER", "F8(CONSOLE)", "(NONE)"),
			new ButtonData(170, "INPUT_SAVE_REPLAY_CLIP", "F3", "B"),
			new ButtonData(171, "INPUT_SPECIAL_ABILITY_PC", "CAPSLOCK", "(NONE)"),
			new ButtonData(172, "INPUT_CELLPHONE_UP", "ARROW UP", "DPAD UP"),
			new ButtonData(173, "INPUT_CELLPHONE_DOWN", "ARROW DOWN", "DPAD DOWN"),
			new ButtonData(174, "INPUT_CELLPHONE_LEFT", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(175, "INPUT_CELLPHONE_RIGHT", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(176, "INPUT_CELLPHONE_SELECT", "ENTER / LEFT MOUSE BUTTON", "A"),
			new ButtonData(177, "INPUT_CELLPHONE_CANCEL", "BACKSPACE / ESC / RIGHTMOUSEBUTTON", "B"),
			new ButtonData(178, "INPUT_CELLPHONE_OPTION", "DELETE", "Y"),
			new ButtonData(179, "INPUT_CELLPHONE_EXTRA_OPTION", "SPACEBAR", "X"),
			new ButtonData(180, "INPUT_CELLPHONE_SCROLL_FORWARD", "SCROLLWHEEL", "DOWN(NONE)"),
			new ButtonData(181, "INPUT_CELLPHONE_SCROLL_BACKWARD", "SCROLLWHEEL", "UP(NONE)"),
			new ButtonData(182, "INPUT_CELLPHONE_CAMERA_FOCUS_LOCK", "L", "RT"),
			new ButtonData(183, "INPUT_CELLPHONE_CAMERA_GRID", "G", "RB"),
			new ButtonData(184, "INPUT_CELLPHONE_CAMERA_SELFIE", "E", "R3"),
			new ButtonData(185, "INPUT_CELLPHONE_CAMERA_DOF", "F", "LB"),
			new ButtonData(186, "INPUT_CELLPHONE_CAMERA_EXPRESSION", "X", "L3"),
			new ButtonData(187, "INPUT_FRONTEND_DOWN", "ARROW DOWN", "DPAD DOWN"),
			new ButtonData(188, "INPUT_FRONTEND_UP", "ARROW UP", "DPAD UP"),
			new ButtonData(189, "INPUT_FRONTEND_LEFT", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(190, "INPUT_FRONTEND_RIGHT", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(191, "INPUT_FRONTEND_RDOWN", "ENTER", "A"),
			new ButtonData(192, "INPUT_FRONTEND_RUP", "TAB", "Y"),
			new ButtonData(193, "INPUT_FRONTEND_RLEFT", "(NONE)", "X"),
			new ButtonData(194, "INPUT_FRONTEND_RRIGHT", "BACKSPACE", "B"),
			new ButtonData(195, "INPUT_FRONTEND_AXIS_X", "D", "LEFT STICK"),
			new ButtonData(196, "INPUT_FRONTEND_AXIS_Y", "S", "LEFT STICK"),
			new ButtonData(197, "INPUT_FRONTEND_RIGHT_AXIS_X]", "RIGHT", "STICK"),
			new ButtonData(198, "INPUT_FRONTEND_RIGHT_AXIS_Y", "SCROLLWHEEL DOWN", "RIGHT STICK"),
			new ButtonData(199, "INPUT_FRONTEND_PAUSE", "P", "START"),
			new ButtonData(200, "INPUT_FRONTEND_PAUSE_ALTERNATE", "ESC", "(NONE)"),
			new ButtonData(201, "INPUT_FRONTEND_ACCEPT", "ENTER / NUMPAD ENTER", "A"),
			new ButtonData(202, "INPUT_FRONTEND_CANCEL", "BACKSPACE /", "ESC B"),
			new ButtonData(203, "INPUT_FRONTEND_X", "SPACEBAR", "X"),
			new ButtonData(204, "INPUT_FRONTEND_Y", "TAB", "Y"),
			new ButtonData(205, "INPUT_FRONTEND_LB", "Q", "LB"),
			new ButtonData(206, "INPUT_FRONTEND_RB", "E", "RB"),
			new ButtonData(207, "INPUT_FRONTEND_LT", "PAGE", "DOWN LT"),
			new ButtonData(208, "INPUT_FRONTEND_RT", "PAGE", "UP RT"),
			new ButtonData(209, "INPUT_FRONTEND_LS", "LEFT", "SHIFT L3"),
			new ButtonData(210, "INPUT_FRONTEND_RS", "LEFT", "CONTROL R3"),
			new ButtonData(211, "INPUT_FRONTEND_LEADERBOARD", "TAB", "RB"),
			new ButtonData(212, "INPUT_FRONTEND_SOCIAL_CLUB", "HOME", "BACK"),
			new ButtonData(213, "INPUT_FRONTEND_SOCIAL_CLUB_SECONDARY", "HOME", "RB"),
			new ButtonData(214, "INPUT_FRONTEND_DELETE", "DELETE", "X"),
			new ButtonData(215, "INPUT_FRONTEND_ENDSCREEN_ACCEPT", "ENTER", "A"),
			new ButtonData(216, "INPUT_FRONTEND_ENDSCREEN_EXPAND", "SPACEBAR", "X"),
			new ButtonData(217, "INPUT_FRONTEND_SELECT", "CAPSLOCK", "BACK"),
			new ButtonData(218, "INPUT_SCRIPT_LEFT_AXIS_X", "D", "LEFT STICK"),
			new ButtonData(219, "INPUT_SCRIPT_LEFT_AXIS_Y", "S", "LEFT STICK"),
			new ButtonData(220, "INPUT_SCRIPT_RIGHT_AXIS_X", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(221, "INPUT_SCRIPT_RIGHT_AXIS_Y", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(222, "INPUT_SCRIPT_RUP", "RIGHT MOUSE", "BUTTON Y"),
			new ButtonData(223, "INPUT_SCRIPT_RDOWN", "LEFT MOUSE", "BUTTON A"),
			new ButtonData(224, "INPUT_SCRIPT_RLEFT", "LEFT", "CTRL X"),
			new ButtonData(225, "INPUT_SCRIPT_RRIGHT", "RIGHT MOUSE", "BUTTON B"),
			new ButtonData(226, "INPUT_SCRIPT_LB", "(NONE)", "LB"),
			new ButtonData(227, "INPUT_SCRIPT_RB", "(NONE)", "LT"),
			new ButtonData(228, "INPUT_SCRIPT_LT", "(NONE)", "LB"),
			new ButtonData(229, "INPUT_SCRIPT_RT", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(230, "INPUT_SCRIPT_LS", "(NONE)", "L3"),
			new ButtonData(231, "INPUT_SCRIPT_RS", "(NONE)", "R3"),
			new ButtonData(232, "INPUT_SCRIPT_PAD_UP", "W", "DPAD UP"),
			new ButtonData(233, "INPUT_SCRIPT_PAD_DOWN", "S", "DPAD DOWN"),
			new ButtonData(234, "INPUT_SCRIPT_PAD_LEFT", "A", "DPAD LEFT"),
			new ButtonData(235, "INPUT_SCRIPT_PAD_RIGHT", "D", "DPAD RIGHT"),
			new ButtonData(236, "INPUT_SCRIPT_SELECT", "V", "BACK"),
			new ButtonData(237, "INPUT_CURSOR_ACCEPT", "LEFT MOUSE", "BUTTON (NONE)"),
			new ButtonData(238, "INPUT_CURSOR_CANCEL", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(239, "INPUT_CURSOR_X", "(NONE)", "(NONE)"),
			new ButtonData(240, "INPUT_CURSOR_Y", "(NONE)", "(NONE)"),
			new ButtonData(241, "INPUT_CURSOR_SCROLL_UP", "SCROLLWHEEL", "UP (NONE)"),
			new ButtonData(242, "INPUT_CURSOR_SCROLL_DOWN", "SCROLLWHEEL", "DOWN (NONE)"),
			new ButtonData(243, "INPUT_ENTER_CHEAT_CODE", "~ /", "` (NONE)"),
			new ButtonData(244, "INPUT_INTERACTION_MENU", "M", "BACK"),
			new ButtonData(245, "INPUT_MP_TEXT_CHAT_ALL", "T", "(NONE)"),
			new ButtonData(246, "INPUT_MP_TEXT_CHAT_TEAM", "Y", "(NONE)"),
			new ButtonData(247, "INPUT_MP_TEXT_CHAT_FRIENDS", "(NONE)", "(NONE)"),
			new ButtonData(248, "INPUT_MP_TEXT_CHAT_CREW", "(NONE)", "(NONE)"),
			new ButtonData(249, "INPUT_PUSH_TO_TALK", "N", "(NONE)"),
			new ButtonData(250, "INPUT_CREATOR_LS", "R", "L3"),
			new ButtonData(251, "INPUT_CREATOR_RS", "F", "R3"),
			new ButtonData(252, "INPUT_CREATOR_LT", "X", "LT"),
			new ButtonData(253, "INPUT_CREATOR_RT", "C", "RT"),
			new ButtonData(254, "INPUT_CREATOR_MENU_TOGGLE", "LEFT", "SHIFT (NONE)"),
			new ButtonData(255, "INPUT_CREATOR_ACCEPT", "SPACEBAR", "A"),
			new ButtonData(256, "INPUT_CREATOR_DELETE", "DELETE", "X"),
			new ButtonData(257, "INPUT_ATTACK2", "LEFT MOUSE", "BUTTON RT"),
			new ButtonData(258, "INPUT_RAPPEL_JUMP", "(NONE)", "A"),
			new ButtonData(259, "INPUT_RAPPEL_LONG_JUMP", "(NONE)", "X"),
			new ButtonData(260, "INPUT_RAPPEL_SMASH_WINDOW", "(NONE)", "RT"),
			new ButtonData(261, "INPUT_PREV_WEAPON", "SCROLLWHEEL UP", "DPAD LEFT"),
			new ButtonData(262, "INPUT_NEXT_WEAPON", "SCROLLWHEEL DOWN", "DPAD RIGHT"),
			new ButtonData(263, "INPUT_MELEE_ATTACK1", "R", "B"),
			new ButtonData(264, "INPUT_MELEE_ATTACK2", "Q", "A"),
			new ButtonData(265, "INPUT_WHISTLE", "(NONE)", "(NONE)"),
			new ButtonData(266, "INPUT_MOVE_LEFT", "D", "LEFT STICK"),
			new ButtonData(267, "INPUT_MOVE_RIGHT", "D", "LEFT STICK"),
			new ButtonData(268, "INPUT_MOVE_UP", "S", "LEFT STICK"),
			new ButtonData(269, "INPUT_MOVE_DOWN", "S", "LEFT STICK"),
			new ButtonData(270, "INPUT_LOOK_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(271, "INPUT_LOOK_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(272, "INPUT_LOOK_UP", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(273, "INPUT_LOOK_DOWN", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(274, "INPUT_SNIPER_ZOOM_IN", "[RIGHT", "STICK"),
			new ButtonData(275, "INPUT_SNIPER_ZOOM_OUT", "[RIGHT", "STICK"),
			new ButtonData(276, "INPUT_SNIPER_ZOOM_IN_ALTERNATE", "[LEFT", "STICK"),
			new ButtonData(277, "INPUT_SNIPER_ZOOM_OUT_ALTERNATE", "[LEFT", "STICK"),
			new ButtonData(278, "INPUT_VEH_MOVE_LEFT", "D", "LEFT STICK"),
			new ButtonData(279, "INPUT_VEH_MOVE_RIGHT", "D", "LEFT STICK"),
			new ButtonData(280, "INPUT_VEH_MOVE_UP", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(281, "INPUT_VEH_MOVE_DOWN", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(282, "INPUT_VEH_GUN_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(283, "INPUT_VEH_GUN_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(284, "INPUT_VEH_GUN_UP", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(285, "INPUT_VEH_GUN_DOWN", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(286, "INPUT_VEH_LOOK_LEFT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(287, "INPUT_VEH_LOOK_RIGHT", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(288, "INPUT_REPLAY_START_STOP_RECORDING", "F1", "A"),
			new ButtonData(289, "INPUT_REPLAY_START_STOP_RECORDING_SECONDARY", "F2", "X"),
			new ButtonData(290, "INPUT_SCALED_LOOK_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(291, "INPUT_SCALED_LOOK_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(292, "INPUT_SCALED_LOOK_UP_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(293, "INPUT_SCALED_LOOK_DOWN_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(294, "INPUT_SCALED_LOOK_LEFT_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(295, "INPUT_SCALED_LOOK_RIGHT_ONLY(NONE)", "RIGHT", "STICK"),
			new ButtonData(296, "INPUT_REPLAY_MARKER_DELETE", "DELETE", "X"),
			new ButtonData(297, "INPUT_REPLAY_CLIP_DELETE", "DELETE", "Y"),
			new ButtonData(298, "INPUT_REPLAY_PAUSE", "SPACEBAR", "A"),
			new ButtonData(299, "INPUT_REPLAY_REWIND", "ARROW", "DOWN LB"),
			new ButtonData(300, "INPUT_REPLAY_FFWD", "ARROW", "UP RB"),
			new ButtonData(301, "INPUT_REPLAY_NEWMARKER", "M", "A"),
			new ButtonData(302, "INPUT_REPLAY_RECORD", "S", "(NONE)"),
			new ButtonData(303, "INPUT_REPLAY_SCREENSHOT", "U", "DPAD UP"),
			new ButtonData(304, "INPUT_REPLAY_HIDEHUD", "H", "R3"),
			new ButtonData(305, "INPUT_REPLAY_STARTPOINT", "B", "(NONE)"),
			new ButtonData(306, "INPUT_REPLAY_ENDPOINT", "N", "(NONE)"),
			new ButtonData(307, "INPUT_REPLAY_ADVANCE", "ARROW RIGHT", "DPAD RIGHT"),
			new ButtonData(308, "INPUT_REPLAY_BACK", "ARROW LEFT", "DPAD LEFT"),
			new ButtonData(309, "INPUT_REPLAY_TOOLS", "T", "DPAD DOWN"),
			new ButtonData(310, "INPUT_REPLAY_RESTART", "R", "BACK"),
			new ButtonData(311, "INPUT_REPLAY_SHOWHOTKEY", "K", "DPAD DOWN"),
			new ButtonData(312, "INPUT_REPLAY_CYCLEMARKERLEFT", "[DPAD", "LEFT"),
			new ButtonData(313, "INPUT_REPLAY_CYCLEMARKERRIGHT]", "DPAD", "RIGHT"),
			new ButtonData(314, "INPUT_REPLAY_FOVINCREASE", "NUMPAD", "+ RB"),
			new ButtonData(315, "INPUT_REPLAY_FOVDECREASE", "NUMPAD", "- LB"),
			new ButtonData(316, "INPUT_REPLAY_CAMERAUP", "PAGE", "UP (NONE)"),
			new ButtonData(317, "INPUT_REPLAY_CAMERADOWN", "PAGE", "DOWN (NONE)"),
			new ButtonData(318, "INPUT_REPLAY_SAVE", "F5", "START"),
			new ButtonData(319, "INPUT_REPLAY_TOGGLETIME", "C", "(NONE)"),
			new ButtonData(320, "INPUT_REPLAY_TOGGLETIPS", "V", "(NONE)"),
			new ButtonData(321, "INPUT_REPLAY_PREVIEW", "SPACEBAR", "(NONE)"),
			new ButtonData(322, "INPUT_REPLAY_TOGGLE_TIMELINE", "ESC", "(NONE)"),
			new ButtonData(323, "INPUT_REPLAY_TIMELINE_PICKUP_CLIP", "X", "(NONE)"),
			new ButtonData(324, "INPUT_REPLAY_TIMELINE_DUPLICATE_CLIP", "C", "(NONE)"),
			new ButtonData(325, "INPUT_REPLAY_TIMELINE_PLACE_CLIP", "V", "(NONE)"),
			new ButtonData(326, "INPUT_REPLAY_CTRL", "LEFT CTRL", "(NONE)"),
			new ButtonData(327, "INPUT_REPLAY_TIMELINE_SAVE", "F5", "CTRL (NONE)"),
			new ButtonData(328, "INPUT_REPLAY_PREVIEW_AUDIO", "SPACEBAR", "RT"),
			new ButtonData(329, "INPUT_VEH_DRIVE_LOOK", "LEFT MOUSE", "BUTTON (NONE)"),
			new ButtonData(330, "INPUT_VEH_DRIVE_LOOK2", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(331, "INPUT_VEH_FLY_ATTACK2", "RIGHT MOUSE", "BUTTON (NONE)"),
			new ButtonData(332, "INPUT_RADIO_WHEEL_UD", "MOUSE DOWN", "RIGHT STICK"),
			new ButtonData(333, "INPUT_RADIO_WHEEL_LR", "MOUSE RIGHT", "RIGHT STICK"),
			new ButtonData(334, "INPUT_VEH_SLOWMO_UD", "SCROLLWHEEL DOWN", "LEFT STICK"),
			new ButtonData(335, "INPUT_VEH_SLOWMO_UP_ONLY", "SCROLLWHEEL UP", "LEFT STICK"),
			new ButtonData(336, "INPUT_VEH_SLOWMO_DOWN_ONLY", "SCROLLWHEEL DOWN", "LEFT STICK"),
			new ButtonData(337, "INPUT_VEH_HYDRAULICS_CONTROL_TOGGLE", "X", "A"),
			new ButtonData(338, "INPUT_VEH_HYDRAULICS_CONTROL_LEFT", "A", "LEFT STICK"),
			new ButtonData(339, "INPUT_VEH_HYDRAULICS_CONTROL_RIGHT", "D", "LEFT STICK"),
			new ButtonData(340, "INPUT_VEH_HYDRAULICS_CONTROL_UP", "LEFT SHIFT", "LEFT STICK"),
			new ButtonData(341, "INPUT_VEH_HYDRAULICS_CONTROL_DOWN", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(342, "INPUT_VEH_HYDRAULICS_CONTROL_UD", "D", "LEFT STICK"),
			new ButtonData(343, "INPUT_VEH_HYDRAULICS_CONTROL_LR", "LEFT CTRL", "LEFT STICK"),
			new ButtonData(344, "INPUT_SWITCH_VISOR", "F11", "DPAD RIGHT"),
			new ButtonData(345, "INPUT_VEH_MELEE_HOLD", "X", "A"),
			new ButtonData(346, "INPUT_VEH_MELEE_LEFT", "LEFT MOUSE", "BUTTON LB"),
			new ButtonData(347, "INPUT_VEH_MELEE_RIGHT", "RIGHT MOUSE", "BUTTON RB"),
			new ButtonData(348, "INPUT_MAP_POI", "SCROLLWHEEL BUTTON", "(PRESS) Y"),
			new ButtonData(349, "INPUT_REPLAY_SNAPMATIC_PHOTO", "TAB", "X"),
			new ButtonData(350, "INPUT_VEH_CAR_JUMP", "E", "L3"),
			new ButtonData(351, "INPUT_VEH_ROCKET_BOOST", "E", "L3"),
			new ButtonData(352, "INPUT_VEH_FLY_BOOST", "LEFT", "SHIFT L3"),
			new ButtonData(353, "INPUT_VEH_PARACHUTE", "SPACEBAR", "A"),
			new ButtonData(354, "INPUT_VEH_BIKE_WINGS", "X", "A"),
			new ButtonData(355, "INPUT_VEH_FLY_BOMB_BAY", "E", "DPAD RIGHT"),
			new ButtonData(356, "INPUT_VEH_FLY_COUNTER", "E", "DPAD RIGHT"),
			new ButtonData(357, "INPUT_VEH_TRANSFORM", "X", "A")
		};

		public int Hold;

		private Blip ClosestBLip;

		public bool FrontEnd;

		public static bool ConvStarted;

		public static Ped ConvLester;

		public bool MenuOpen;

		public LesterContact()
		{
			ifruit = new CustomiFruit
			{
				CenterButtonColor = Color.Orange,
				LeftButtonColor = Color.LimeGreen,
				RightButtonColor = Color.Purple,
				CenterButtonIcon = SoftKeyIcon.Fire,
				LeftButtonIcon = SoftKeyIcon.Police,
				RightButtonIcon = SoftKeyIcon.Website
			};
			ifruit.SetWallpaper(new Wallpaper("char_facebook"));
			ifruit.SetWallpaper(Wallpaper.BadgerDefault);
			iFruitContact iFruitContact = new iFruitContact("Lester (Jobs)");
			iFruitContact.Answered += loadMenu;
			iFruitContact.DialTimeout = 3000;
			iFruitContact.Active = true;
			iFruitContact.Icon = ContactIcon.Lester;
			iFruitContact.Name = "Lester";
			ifruit.Contacts.Add(iFruitContact);
			Setup();
			base.Tick += onTick;
			base.Interval = 1;
			LoadIniFile("scripts//HKH_SinglePlayerHeists//LesterContact.ini");
		}

		public void SetBanner(NativeMenu menu)
		{
			menu.Banner = RectangleGUI;
		}

		public void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		private void Setup()
		{
			CreateBanner();
			modLemonUIObjectPool = new ObjectPool();
			mainMenu = new NativeMenu("Single Player Heists", "Select an Option");
			GLemonUIMenusNativeMenus.Add(mainMenu);
			modLemonUIObjectPool.Add(mainMenu);
			Inventory = new NativeMenu("Single Player Heists", "Inventory");
			mainMenu.AddSubMenu(Inventory);
			modLemonUIObjectPool.Add(Inventory);
			GLemonUIMenusNativeMenus.Add(Inventory);
			Bribes = new NativeMenu("Single Player Heists", "Bribes");
			mainMenu.AddSubMenu(Bribes);
			modLemonUIObjectPool.Add(Bribes);
			GLemonUIMenusNativeMenus.Add(Bribes);
			NativeItem nativeItem = new NativeItem("Hire A Crew");
			mainMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				HiredBodyguardfromPhone = true;
				LastPosition = Game.Player.Character.Position;
				modLemonUIObjectPool.HideAll();
				Game.Player.Character.Position = new Vector3(403f, -999.63f, -98.56f);
				Script.Wait(1);
				Game.Player.Character.Position = new Vector3(403f, -999.63f, -98.56f);
				Game.Player.Character.Rotation = new Vector3(0f, 0f, -73f);
				Function.Call(Hash.SET_FOCUS_POS_AND_VEL, 403f, -999.63f, -98.56, 0, 0, 0);
				BodyguardMenu.mainMenu.Visible = !BodyguardMenu.mainMenu.Visible;
				BodyguardMenu.CameraON = true;
				BodyguardMenu.HeistStartCam = GlobalHelper.CreateCamera(new Vector3(403f, -999.63f, -98.56f), new Vector3(0f, 0f, 0f), 60f);
				BodyguardMenu.HeistStartCam.IsActive = true;
				BodyguardMenu.HeistStartCam.FarClip = 2000f;
				Function.Call(Hash.RENDER_SCRIPT_CAMS, 1, 0, 3000, 1, 0);
				Game.Player.Character.IsPositionFrozen = true;
				Game.Player.CanControlCharacter = false;
				Game.Player.Character.Opacity = 0;
				World.RenderingCamera = BodyguardMenu.HeistStartCam;
			};
			OfficeMarker1 = new Vector3(-52.78f, -787.104f, 45f);
			OfficeMarker2 = new Vector3(-1548.77f, -641.732f, 28f);
			OfficeMarker3 = new Vector3(-1393.46f, -527.473f, 31f);
			OfficeMarker4 = new Vector3(-104.988f, -611.686f, 36f);
			setupInventory();
			SetupBribes();
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count(); i++)
			{
				NativeMenu banner = GLemonUIMenusNativeMenus[i];
				SetBanner(banner);
			}
		}

		public void CBS()
		{
			List<object> list = new List<object>();
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);
			list.Add(5);
			list.Add(6);
			List<dynamic> pedH = new List<object>();
			PedHash[] array = (PedHash[])Enum.GetValues(typeof(PedHash));
			foreach (PedHash pedHash in array)
			{
				pedH.Add(pedHash);
			}
			List<dynamic> Veh = new List<object>();
			VehicleHash[] array2 = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));
			foreach (VehicleHash vehicleHash in array2)
			{
				Veh.Add(vehicleHash);
			}
			List<dynamic> Pw = new List<object>();
			WeaponHash[] array3 = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
			foreach (WeaponHash weaponHash in array3)
			{
				Pw.Add(weaponHash);
			}
			List<dynamic> HPA = new List<object>();
			HPA.Add(100);
			HPA.Add(200);
			HPA.Add(300);
			HPA.Add(400);
			HPA.Add(500);
			HPA.Add(600);
			HPA.Add(700);
			HPA.Add(800);
			HPA.Add(900);
			HPA.Add(1000);
			HPA.Add(1100);
			HPA.Add(1200);
			HPA.Add(1300);
			HPA.Add(1400);
			HPA.Add(1500);
			HPA.Add(1600);
			HPA.Add(1700);
			HPA.Add(1800);
			HPA.Add(1900);
			HPA.Add(2000);
			NativeMenu nativeMenu = new NativeMenu("Single Player Heists", "Custom Body Guards");
			CustomBodyGuardSetup.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeListItem<dynamic> NAI = new NativeListItem<object>("Number of AI : ", list.ToArray());
			nativeMenu.Add(NAI);
			NativeListItem<dynamic> pedM = new NativeListItem<object>("Ped Model : ", pedH.ToArray());
			nativeMenu.Add(pedM);
			NativeListItem<dynamic> VehM = new NativeListItem<object>("Vehicle Model : ", Veh.ToArray());
			nativeMenu.Add(VehM);
			NativeListItem<dynamic> PMW = new NativeListItem<object>("Primary Weapon : ", Pw.ToArray());
			nativeMenu.Add(PMW);
			NativeListItem<dynamic> SMW = new NativeListItem<object>("Secondary Weapon : ", Pw.ToArray());
			nativeMenu.Add(SMW);
			NativeListItem<dynamic> hp = new NativeListItem<object>("Health : ", HPA.ToArray());
			nativeMenu.Add(hp);
			NativeListItem<dynamic> arm = new NativeListItem<object>("Armour  : ", HPA.ToArray());
			nativeMenu.Add(arm);
			NativeItem nativeItem = new NativeItem("Call Squad");
			nativeMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				Notification.Show("Lester : Your Advanced Bodyguard Squad is on its way to you!");
				SetupAddvacedSquad(NAI.SelectedIndex + 1, pedH[pedM.SelectedIndex], Veh[VehM.SelectedIndex], Pw[PMW.SelectedIndex], Pw[SMW.SelectedIndex], HPA[hp.SelectedIndex], HPA[arm.SelectedIndex]);
			};
		}

		public Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public Model RequestModel(PedHash Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public Model RequestModel(VehicleHash Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid)
			{
				while (!result.IsLoaded)
				{
					Script.Wait(50);
				}
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public void SetupAddvacedSquad(int NoP, PedHash P, VehicleHash V, WeaponHash PW, WeaponHash SW, int HP, int ARM)
		{
			Vehicle vehicle = GlobalHelper.CreateVehicle(RequestModel(V), Game.Player.Character.Position.Around(200f));
			SupportVeh.Add(vehicle);
			for (int i = 0; i < NoP; i++)
			{
				Ped ped = vehicle.CreatePedOnSeat((VehicleSeat)(i - 1), P);
				if (vehicle.IsSeatFree(VehicleSeat.Driver))
				{
					ped.Opacity = 254;
					Vehicle vehicle2 = vehicle;
					ped.Task.WarpOutOfVehicle(vehicle2);
					ped.Task.WarpIntoVehicle(vehicle2, VehicleSeat.Driver);
					Notification.Show("NO DRIVER");
				}
				ped.AddBlip();
				ped.AttachedBlip.Sprite = BlipSprite.Enemy;
				ped.AttachedBlip.Color = BlipColor.Blue;
				ped.AttachedBlip.Name = "Heist Partner";
				ped.CanWrithe = false;
				ped.Health = HP;
				ped.Armor = ARM;
				ped.Weapons.Give(PW, 9999, true, true);
				ped.Weapons.Give(SW, 9999, true, true);
				ped.CanRagdoll = false;
				ped.CanSufferCriticalHits = false;
				ped.Task.FightAgainstHatedTargets(200f, 10);
				SupportPed.Add(ped);
			}
			if (!vehicle.Model.IsHelicopter)
			{
				vehicle.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(vehicle, Game.Player.Character.Position, 20f, 100f);
				vehicle.PlaceOnNextStreet();
			}
			if (vehicle.Model.IsHelicopter)
			{
				vehicle.Position = new Vector3(vehicle.Position.X, vehicle.Position.Y, vehicle.Position.Z + 100f);
				vehicle.GetPedOnSeat(VehicleSeat.Driver).Task.FightAgainst(Game.Player.Character);
			}
		}

		public void SetupVeh(VehicleHash Veh, int SeatCount, int Pedtype)
		{
			Vehicle vehicle = GlobalHelper.CreateVehicle(Veh, Game.Player.Character.Position.Around(200f));
			SupportVeh.Add(vehicle);
			if (vehicle.Model.IsHelicopter || vehicle.Model.IsPlane)
			{
				vehicle.Position = new Vector3(vehicle.Position.X, vehicle.Position.Y, vehicle.Position.Z + 100f);
			}
			else
			{
				vehicle.PlaceOnNextStreet();
			}
			if (SeatCount == 1)
			{
				SetupPed(VehicleSeat.Driver, Pedtype, vehicle);
			}
			if (SeatCount == 2)
			{
				SetupPed(VehicleSeat.Driver, Pedtype, vehicle);
				SetupPed(VehicleSeat.Passenger, Pedtype, vehicle);
				if (!vehicle.Model.IsHelicopter)
				{
					vehicle.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(vehicle, Game.Player.Character.Position, 20f, 100f);
					vehicle.PlaceOnNextStreet();
				}
				if (vehicle.Model.IsHelicopter)
				{
					vehicle.GetPedOnSeat(VehicleSeat.Driver).Task.FightAgainst(Game.Player.Character);
				}
			}
			if (SeatCount == 4)
			{
				SetupPed(VehicleSeat.Driver, Pedtype, vehicle);
				SetupPed(VehicleSeat.LeftRear, Pedtype, vehicle);
				SetupPed(VehicleSeat.Passenger, Pedtype, vehicle);
				SetupPed(VehicleSeat.RightRear, Pedtype, vehicle);
				if (!vehicle.Model.IsHelicopter)
				{
					vehicle.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(vehicle, Game.Player.Character.Position, 20f, 100f);
					vehicle.PlaceOnNextStreet();
				}
				if (vehicle.Model.IsHelicopter)
				{
					vehicle.GetPedOnSeat(VehicleSeat.Driver).Task.FightAgainst(Game.Player.Character);
				}
			}
		}

		public void SetupPed(VehicleSeat Seat, int Type, Vehicle Veh)
		{
			if (Type == 1)
			{
				Ped ped = GlobalHelper.CreatePed(PedHash.FreemodeMale01, Veh.Position);
				ped.SetIntoVehicle(Veh, Seat);
				Function.Call(Hash.REQUEST_ANIM_SET, "ANIM_GROUP_MOVE_BALLISTIC");
				Function.Call(Hash.REQUEST_ANIM_SET, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.REQUEST_CLIP_SET, "move_ballistic_2h");
				Ped ped2 = ped;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 1, 91, 10, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 3, 46, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 4, 84, 10, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 6, 10, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 8, 97, 10, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 11, 186, 10, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped2, 12, 0, 0, 1);
				Function.Call(Hash.SET_PED_USING_ACTION_MODE, ped2, true, -1, 0);
				Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, ped2, "ANIM_GROUP_MOVE_BALLISTIC", 5f);
				Function.Call(Hash.SET_PED_STRAFE_CLIPSET, ped2, "MOVE_STRAFE_BALLISTIC");
				Function.Call(Hash.SET_WEAPON_ANIMATION_OVERRIDE, ped2, 1429513766);
				ped2.AddBlip();
				ped2.AttachedBlip.Sprite = BlipSprite.Enemy;
				ped2.AttachedBlip.Color = BlipColor.Blue;
				ped2.AttachedBlip.Name = "Heist Partner";
				ped2.CanWrithe = false;
				ped2.Health = 1200;
				ped2.Armor = 1400;
				ped2.Weapons.Give(WeaponHash.CombatMGMk2, 9999, true, true);
				ped2.CanRagdoll = false;
				ped2.CanSufferCriticalHits = false;
				SupportPed.Add(ped2);
				ped2.Opacity = 0;
			}
			if (Type == 2)
			{
				Ped ped3 = GlobalHelper.CreatePed(PedHash.FreemodeMale01, Veh.Position);
				ped3.SetIntoVehicle(Veh, Seat);
				Ped ped4 = ped3;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 1, 125, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 3, 17, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 4, 34, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 6, 69, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 8, 128, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 11, 130, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped4, 12, 0, 0, 1);
				Ped ped5 = ped4;
				PedGroup pedGroup = Game.Player.Character.PedGroup;
				Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, ped5, pedGroup);
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, ped5, 100);
				ped4.AddBlip();
				ped4.AttachedBlip.Sprite = BlipSprite.Enemy;
				ped4.AttachedBlip.Color = BlipColor.Blue;
				ped4.AttachedBlip.Name = "Heist Partner";
				ped4.CanWrithe = false;
				ped4.Health = 600;
				ped4.Armor = 500;
				ped4.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				ped4.CanRagdoll = false;
				ped4.CanSufferCriticalHits = false;
				ped4.Task.FightAgainstHatedTargets(200f, 10);
				SupportPed.Add(ped4);
				ped4.Opacity = 0;
			}
			if (Type == 3)
			{
				Ped ped6 = GlobalHelper.CreatePed(PedHash.FreemodeMale01, Veh.Position);
				ped6.SetIntoVehicle(Veh, Seat);
				Ped ped7 = ped6;
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 0, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 1, 125, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 2, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 3, 17, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 4, 34, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 5, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 6, 69, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 7, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 8, 128, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 9, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 10, 0, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 11, 130, 0, 1);
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, ped7, 12, 0, 0, 1);
				Ped ped8 = ped7;
				PedGroup pedGroup2 = Game.Player.Character.PedGroup;
				Function.Call(Hash.SET_PED_AS_GROUP_MEMBER, ped8, pedGroup2);
				Function.Call(Hash.SET_PED_COMBAT_ABILITY, ped8, 100);
				ped7.AddBlip();
				ped7.AttachedBlip.Sprite = BlipSprite.Enemy;
				ped7.AttachedBlip.Color = BlipColor.Blue;
				ped7.AttachedBlip.Name = "Heist Partner";
				ped7.CanWrithe = false;
				ped7.Health = 600;
				ped7.Armor = 500;
				ped7.Weapons.Give(WeaponHash.CarbineRifle, 9999, true, true);
				ped7.CanRagdoll = false;
				ped7.CanSufferCriticalHits = false;
				ped7.Task.FightAgainstHatedTargets(200f, 10);
				SupportPed.Add(ped7);
				ped7.Opacity = 0;
			}
		}

		private void LoadIniFile(string iniName)
		{
			try
			{
				Config = ScriptSettings.Load(iniName);
				Key1 = Config.GetValue("Configurations", "Key1", Key1);
				purchasedarmour = Config.GetValue("Setup", "Armour", purchasedarmour);
				purchasedsnacks = Config.GetValue("Setup", "Snacks", purchasedsnacks);
				Snack_hotkey = Config.GetValue("Setup", "Snack_hotkey", Snack_hotkey);
				ARMOUR_hotkey = Config.GetValue("Setup", "ARMOUR_hotkey", ARMOUR_hotkey);
				OrderAllEnter = Config.GetValue("Setup", "OrderAllEnter", OrderAllEnter);
				OrderAllExit = Config.GetValue("Setup", "OrderAllExit", OrderAllExit);
				OrderAllFollow = Config.GetValue("Setup", "OrderAllFollow", OrderAllFollow);
			}
			catch (Exception)
			{
				Notification.Show("~r~Error~w~: Config.ini Failed To Load.");
			}
		}

		private void setupInventory()
		{
			NativeMenu nativeMenu = new NativeMenu("Single Player Heists", "Purchase Snacks/Body Armour");
			Inventory.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			NativeMenu nativeMenu2 = new NativeMenu("Single Player Heists", " Use Snacks/Body Armour");
			Inventory.AddSubMenu(nativeMenu2);
			modLemonUIObjectPool.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			NativeItem nativeItem = new NativeItem("Purchase Snacks : $20");
			nativeMenu.Add(nativeItem);
			NativeItem nativeItem2 = new NativeItem("Purchase Body Armour : $2500");
			nativeMenu.Add(nativeItem2);
			NativeItem nativeItem3 = new NativeItem("Purchase Snacks x10 : $200");
			nativeMenu.Add(nativeItem3);
			NativeItem nativeItem4 = new NativeItem("Purchase Body Armour x10 : $25000");
			nativeMenu.Add(nativeItem4);
			NativeItem nativeItem5 = new NativeItem("Use Snacks");
			nativeMenu2.Add(nativeItem5);
			NativeItem nativeItem6 = new NativeItem("Use Body Armour");
			nativeMenu2.Add(nativeItem6);
			nativeItem5.Activated += delegate
			{
				Notification.Show("Purchased Snacks " + purchasedsnacks);
				if (purchasedsnacks > 0)
				{
					Random random2 = new Random();
					Game.Player.Character.Health = Game.Player.Character.Health + random2.Next(5, 20);
					Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
					Game.Player.Character.Task.PlayAnimation("amb@code_human_wander_eating_donut_fat@male@base", "static", 1f, 1f, 2500, AnimationFlags.Loop, 0f);
					Notification.Show("Player: Eating a snack ");
					purchasedsnacks--;
					Config.SetValue("Setup", "Armour", purchasedarmour);
					Config.SetValue("Setup", "Snacks", purchasedsnacks);
					Config.Save();
				}
				else
				{
					Notification.Show("Lester: you have no snacks ");
				}
			};
			nativeItem6.Activated += delegate
			{
				Notification.Show("Purchased Armour " + purchasedarmour);
				if (purchasedarmour > 0)
				{
					Random random = new Random();
					Game.Player.Character.Armor = Game.Player.Character.Health + random.Next(10, 100);
					Notification.Show("Player: putting on body armor, cover me!");
					purchasedarmour--;
					Config.SetValue("Setup", "Armour", purchasedarmour);
					Config.SetValue("Setup", "Snacks", purchasedsnacks);
					Config.Save();
				}
				else
				{
					Notification.Show("Lester: you have no body armour ");
				}
			};
			nativeItem.Activated += delegate
			{
				if (Game.Player.Money > 20)
				{
					Game.Player.Money = Game.Player.Money - 20;
					purchasedsnacks++;
					Config.SetValue("Setup", "Armour", purchasedarmour);
					Config.SetValue("Setup", "Snacks", purchasedsnacks);
					Config.Save();
					Notification.Show("Lester: Here you go , i hope this works");
				}
				else
				{
					Notification.Show("Lester: you do not have enought money to purchase a snacks!");
				}
			};
			nativeItem2.Activated += delegate
			{
				if (Game.Player.Money > 2500)
				{
					Game.Player.Money = Game.Player.Money - 2500;
					purchasedarmour++;
					Config.SetValue("Setup", "Armour", purchasedarmour);
					Config.SetValue("Setup", "Snacks", purchasedsnacks);
					Config.Save();
					Notification.Show("Lester: Here you go , i hope this works");
				}
				else
				{
					Notification.Show("Lester: you do not have enought money to purchase a armour!");
				}
			};
			nativeItem3.Activated += delegate
			{
				if (Game.Player.Money > 200)
				{
					Game.Player.Money = Game.Player.Money - 200;
					purchasedsnacks += 10;
					Config.SetValue("Setup", "Armour", purchasedarmour);
					Config.SetValue("Setup", "Snacks", purchasedsnacks);
					Config.Save();
					Notification.Show("Lester: Here you go , i hope this works");
				}
				else
				{
					Notification.Show("Lester: you do not have enought money to purchase a snacks!");
				}
			};
			nativeItem4.Activated += delegate
			{
				if (Game.Player.Money > 25000)
				{
					Game.Player.Money = Game.Player.Money - 25000;
					purchasedarmour += 10;
					Config.SetValue("Setup", "Armour", purchasedarmour);
					Config.SetValue("Setup", "Snacks", purchasedsnacks);
					Config.Save();
					Notification.Show("Lester: Here you go , i hope this works");
				}
				else
				{
					Notification.Show("Lester: you do not have enought money to purchase a armour!");
				}
			};
		}

		private void SetupBribes()
		{
			NativeMenu nativeMenu = new NativeMenu("Single Player Heists", "Player");
			Bribes.AddSubMenu(nativeMenu);
			modLemonUIObjectPool.Add(nativeMenu);
			GLemonUIMenusNativeMenus.Add(nativeMenu);
			ClearWanted = new NativeItem("Remove Wanted Level");
			nativeMenu.Add(ClearWanted);
			NativeMenu nativeMenu2 = new NativeMenu("Single Player Heists", "Request A Job");
			nativeMenu.AddSubMenu(nativeMenu2);
			modLemonUIObjectPool.Add(nativeMenu2);
			GLemonUIMenusNativeMenus.Add(nativeMenu2);
			LocateABoat = new NativeItem("Locate a Boat", "NOT YET IMPLEMENTED,", "$400");
			nativeMenu.Add(LocateABoat);
			LocateABoat.Activated += delegate
			{
				if (Game.Player.Money > 400)
				{
					List<Vector4> list4 = new List<Vector4>();
					foreach (Vector4 boats_Spawn in Boats_Spawns)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, boats_Spawn.XYZ, "<", 500f))
						{
							list4.Add(boats_Spawn);
						}
					}
					if (list4.Count > 0)
					{
						if (LocatedVehicle != null)
						{
							LocatedVehicle.Delete();
						}
						int index4 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, Posssible_Boats.Count);
						int num4 = 0;
						num4 = ((list4.Count != 1) ? Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list4.Count) : 0);
						LocatedVehicle = GlobalHelper.CreateVehicle(RequestModel(Posssible_Boats[index4]), list4[num4].XYZ, list4[num4].H);
						LocatedVehicle.AddBlip();
						LocatedVehicle.AttachedBlip.Name = "Located Vehicle";
						LocatedVehicle.AttachedBlip.Color = BlipColor.White;
						Game.Player.Money = Game.Player.Money - 400;
					}
					else
					{
						DeclineItem();
						Notification.Show("Lester: sorry, I cant locate a Boat at this time");
					}
				}
				else
				{
					DeclineItem();
					Notification.Show("Lester: you do not have enought money to Locate a Boat!");
				}
			};
			LocateAHelicopter = new NativeItem("Locate a Helicopter", "NOT YET IMPLEMENTED", "$700");
			nativeMenu.Add(LocateAHelicopter);
			LocateAHelicopter.Activated += delegate
			{
				if (Game.Player.Money > 700)
				{
					List<Vector4> list3 = new List<Vector4>();
					foreach (Vector4 helicopters_Spawn in Helicopters_Spawns)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, helicopters_Spawn.XYZ, "<", 500f))
						{
							list3.Add(helicopters_Spawn);
						}
					}
					if (list3.Count > 0)
					{
						if (LocatedVehicle != null)
						{
							LocatedVehicle.Delete();
						}
						int index3 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, Posssible_Helicopters.Count);
						int num3 = 0;
						num3 = ((list3.Count != 1) ? Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list3.Count) : 0);
						LocatedVehicle = GlobalHelper.CreateVehicle(RequestModel(Posssible_Helicopters[index3]), list3[num3].XYZ, list3[num3].H);
						LocatedVehicle.AddBlip();
						LocatedVehicle.AttachedBlip.Name = "Located Vehicle";
						LocatedVehicle.AttachedBlip.Color = BlipColor.White;
						Game.Player.Money = Game.Player.Money - 700;
					}
					else
					{
						DeclineItem();
						Notification.Show("Lester: sorry, I cant locate a Helicopter at this time");
					}
				}
				else
				{
					DeclineItem();
					Notification.Show("Lester: you do not have enought money to Locate a Helicopter!");
				}
			};
			LocateACar = new NativeItem("Locate a Car", "NOT YET IMPLEMENTED", "$100");
			nativeMenu.Add(LocateACar);
			LocateACar.Activated += delegate
			{
				if (Game.Player.Money > 100)
				{
					List<Vector4> list2 = new List<Vector4>();
					foreach (Vector4 cars_Spawn in Cars_Spawns)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, cars_Spawn.XYZ, "<", 500f))
						{
							list2.Add(cars_Spawn);
						}
					}
					if (list2.Count > 0)
					{
						if (LocatedVehicle != null)
						{
							LocatedVehicle.Delete();
						}
						int index2 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, Posssible_Cars.Count);
						int num2 = 0;
						num2 = ((list2.Count != 1) ? Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list2.Count) : 0);
						LocatedVehicle = GlobalHelper.CreateVehicle(RequestModel(Posssible_Cars[index2]), list2[num2].XYZ, list2[num2].H);
						LocatedVehicle.AddBlip();
						LocatedVehicle.AttachedBlip.Name = "Located Vehicle";
						LocatedVehicle.AttachedBlip.Color = BlipColor.White;
						Game.Player.Money = Game.Player.Money - 100;
					}
					else
					{
						DeclineItem();
						Notification.Show("Lester: sorry, I cant locate a Car at this time");
					}
				}
				else
				{
					DeclineItem();
					Notification.Show("Lester: you do not have enought money to Locate a Car!");
				}
			};
			LocateAPlane = new NativeItem("Locate a Plane", "NOT YET IMPLEMENTED", "$1000");
			nativeMenu.Add(LocateAPlane);
			LocateAPlane.Activated += delegate
			{
				if (Game.Player.Money > 1000)
				{
					List<Vector4> list = new List<Vector4>();
					foreach (Vector4 planes_Spawn in Planes_Spawns)
					{
						if (GetDistance_VDIST2(Game.Player.Character.Position, planes_Spawn.XYZ, "<", 500f))
						{
							list.Add(planes_Spawn);
						}
					}
					if (list.Count > 0)
					{
						if (LocatedVehicle != null)
						{
							LocatedVehicle.Delete();
						}
						int index = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, Posssible_Planes.Count);
						int num = 0;
						num = ((list.Count != 1) ? Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count) : 0);
						LocatedVehicle = GlobalHelper.CreateVehicle(RequestModel(Posssible_Planes[index]), list[num].XYZ, list[num].H);
						LocatedVehicle.AddBlip();
						LocatedVehicle.AttachedBlip.Name = "Located Vehicle";
						LocatedVehicle.AttachedBlip.Color = BlipColor.White;
						Game.Player.Money = Game.Player.Money - 1000;
					}
					else
					{
						DeclineItem();
						Notification.Show("Lester: sorry, I cant locate a Plane at this time");
					}
				}
				else
				{
					DeclineItem();
					Notification.Show("Lester: you do not have enought money to Locate a Plane!");
				}
			};
			CopsTurnBlindEye = new NativeItem("Cops Turn Blind Eye/2m", "", "$5000");
			nativeMenu.Add(CopsTurnBlindEye);
			NativeItem item2 = new NativeItem("(Smart) Guards Ignore You/60s", "", "$75,000");
			nativeMenu.Add(item2);
			NativeItem nativeItem = new NativeItem("Invincible/60s", "", "$1,000,000");
			nativeMenu.Add(nativeItem);
			CopsTurnBlindEye.Activated += delegate
			{
				if (Game.Player.Money > 5000)
				{
					Game.Player.Money = Game.Player.Money - 5000;
					Notification.Show("Lester: diverted cops attention away from you for 1 minute,");
					NoCops = true;
					NoCopstime = Game.GameTime + 5000;
					AcceptItem();
				}
				else
				{
					DeclineItem();
				}
			};
			NativeItem nativeItem2 = new NativeItem("Blackout/60s", "", "$750,000");
			nativeMenu.Add(nativeItem2);
			NativeItem nativeItem3 = new NativeItem("No Restricted Zones/60s", "", "$2,200,000");
			nativeMenu.Add(nativeItem3);
			NativeItem nativeItem4 = new NativeItem("Juggernaut Support", "", "$750,000");
			nativeMenu.Add(nativeItem4);
			NativeItem nativeItem5 = new NativeItem("Valkyrie Support", "", "$1,250,000");
			nativeMenu.Add(nativeItem5);
			NativeItem nativeItem6 = new NativeItem("Bounty Hunter Support", "", "$550,000");
			nativeMenu.Add(nativeItem6);
			NativeItem nativeItem7 = new NativeItem("Clear Support Units", "", "$0");
			nativeMenu.Add(nativeItem7);
			nativeItem7.Activated += delegate
			{
				foreach (Ped item3 in SupportPed)
				{
					if (item3 != null)
					{
						item3.Delete();
					}
				}
				foreach (Vehicle item4 in SupportVeh)
				{
					if (item4 != null)
					{
						item4.Delete();
					}
				}
			};
			nativeItem6.Activated += delegate
			{
				if (Game.Player.Money > 550000)
				{
					Game.Player.Money = Game.Player.Money - 550000;
					Notification.Show("Lester : Im calling in some backup for you");
					SetupVeh(VehicleHash.XLS2, 4, 2);
					AcceptItem();
				}
				else
				{
					DeclineItem();
				}
			};
			nativeItem4.Activated += delegate
			{
				if (Game.Player.Money > 750000)
				{
					Game.Player.Money = Game.Player.Money - 750000;
					Notification.Show("Lester : Im calling in some backup for you");
					SetupVeh(VehicleHash.Mesa3, 4, 1);
					AcceptItem();
				}
				else
				{
					DeclineItem();
				}
			};
			nativeItem5.Activated += delegate
			{
				if (Game.Player.Money > 1250000)
				{
					Game.Player.Money = Game.Player.Money - 1250000;
					Notification.Show("Lester : Im calling in some backup for you");
					SetupVeh(VehicleHash.Valkyrie, 4, 2);
				}
			};
			nativeItem3.Activated += delegate
			{
				if (Game.Player.Money > 2200000)
				{
					Game.Player.Money = Game.Player.Money - 2200000;
					Notification.Show("Lester: Here you go, ive got you 60 seconds to enter any restricted zone, to do whatever you want");
					noRestrictedzones = true;
					NoRestrictedZonesTimer = Game.GameTime + 60000;
					AcceptItem();
				}
				else
				{
					DeclineItem();
				}
			};
			nativeItem2.Activated += delegate
			{
				if (Game.Player.Money > 500000)
				{
					Game.Player.Money = Game.Player.Money - 500000;
					Game.Player.WantedLevel = 0;
					Blackoutwait = true;
					BlackoutTimer = Game.GameTime + 60000;
					Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, true);
					Notification.Show("Lester: Here you go , i hope this works");
					AcceptItem();
				}
				else
				{
					DeclineItem();
					Notification.Show("Lester: you do not have enought money to purchase blackout!");
				}
			};
			nativeItem.Activated += delegate
			{
				if (Game.Player.Money > 1000000)
				{
					Game.Player.Money = Game.Player.Money - 1000000;
					Game.Player.Character.IsInvincible = true;
					invunrablewait = true;
					InvunrableTime = Game.GameTime + 60000;
					Notification.Show("Lester: Here you go , i hope this works");
				}
				else
				{
					Notification.Show("Lester: you do not have enought money to Invincibility!");
				}
			};
		}

		public void ChopperGunnerTarget(Ped p)
		{
			List<object> list = new List<object>();
			try
			{
				Ped[] nearbyPeds = World.GetNearbyPeds(Game.Player.Character.Position, 200f);
				Ped[] array = nearbyPeds;
				foreach (Ped ped in array)
				{
					if (ped.IsInCombatAgainst(Game.Player.Character) && ped.IsAlive)
					{
						list.Add(ped);
					}
				}
				Random random = new Random();
				int index = random.Next(0, list.Count);
				if (Target == null)
				{
					Target = (dynamic)list[index];
					p.Task.FightAgainst(Target);
				}
				if (Target != null && Target.IsDead)
				{
					Target = (dynamic)list[index];
					p.Task.FightAgainst(Target);
				}
			}
			catch
			{
				Notification.Show("error 232/index");
			}
		}

		private void FireGun(Vector3 Source, Ped Owner, Vector3 Target)
		{
			WeaponAsset weaponAsset = Function.Call<int>(Hash.GET_HASH_KEY, "VEHICLE_WEAPON_TURRET_TECHNICAL");
			if (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset))
			{
				Function.Call(Hash.REQUEST_WEAPON_ASSET, weaponAsset);
				while (!Function.Call<bool>(Hash.HAS_WEAPON_ASSET_LOADED, weaponAsset))
				{
					Script.Wait(0);
				}
			}
			World.ShootBullet(Source, Target.Around(1f), Owner, weaponAsset, 3);
			World.ShootBullet(Source, Target.Around(1f), Owner, weaponAsset, 3);
			World.ShootBullet(Source, Target.Around(1f), Owner, weaponAsset, 3);
		}

		public string ReturnButtonString(int ID)
		{
			SelectedButtonIndex = "Error Invald Key ID";
			if (ID < 358)
			{
				foreach (ButtonData item in ButtonIndex)
				{
					if (item.ButtonID == ID)
					{
						SelectedButtonIndex = item.DisplayName;
						string selectedButtonIndex = SelectedButtonIndex;
						SelectedButtonIndex = "~" + selectedButtonIndex + "~";
					}
				}
			}
			else
			{
				SelectedButtonIndex = "Error Invald Key ID :" + ID;
			}
			return SelectedButtonIndex;
		}

		public string ReturnButtonString_KeyboardKey(int ID)
		{
			SelectedButtonIndex = "Error Invald Key ID";
			if (ID < 358)
			{
				foreach (ButtonData item in ButtonIndex)
				{
					if (item.ButtonID == ID)
					{
						SelectedButtonIndex = item.KeyboardKey;
						string selectedButtonIndex = SelectedButtonIndex;
						SelectedButtonIndex = "~" + selectedButtonIndex + "~";
					}
				}
			}
			else
			{
				SelectedButtonIndex = "Error Invald Key ID :" + ID;
			}
			return SelectedButtonIndex;
		}

		public int ReturnButtonID(int ID)
		{
			return ID;
		}

		public static bool GetDistance_VDIST2(Vector3 _1, Vector3 _2, string Sign, float Radius)
		{
			bool result = false;
			float num = Radius * Radius;
			float num2 = Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z);
			if (Sign.Equals("<=") && num2 <= num)
			{
				result = true;
			}
			if (Sign.Equals("<") && num2 < num)
			{
				result = true;
			}
			if (Sign.Equals(">=") && num2 >= num)
			{
				result = true;
			}
			if (Sign.Equals(">") && num2 > num)
			{
				result = true;
			}
			return result;
		}

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, 1);
		}

		public static void AddArmour(int Amt)
		{
			purchasedarmour += Amt;
			Config.SetValue("Setup", "Armour", purchasedarmour);
			Config.Save();
		}

		public static void AddHealth(int Amt)
		{
			purchasedsnacks += Amt;
			Config.SetValue("Setup", "Snacks", purchasedsnacks);
			Config.Save();
		}

		private void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public void ShowNearbySpawns()
		{
			if (!SPH_Debug.AllowSaveNewLocateAVeh)
			{
				return;
			}
			foreach (Vector4 boats_Spawn in Boats_Spawns)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, boats_Spawn.XYZ, "<", 500f))
				{
					World.DrawMarker(MarkerType.Cylinder, boats_Spawn.XYZ, Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 100f), Color.Red);
				}
			}
			foreach (Vector4 cars_Spawn in Cars_Spawns)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, cars_Spawn.XYZ, "<", 500f))
				{
					World.DrawMarker(MarkerType.Cylinder, cars_Spawn.XYZ, Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 100f), Color.Green);
				}
			}
			foreach (Vector4 helicopters_Spawn in Helicopters_Spawns)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, helicopters_Spawn.XYZ, "<", 500f))
				{
					World.DrawMarker(MarkerType.Cylinder, helicopters_Spawn.XYZ, Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 100f), Color.Blue);
				}
			}
			foreach (Vector4 planes_Spawn in Planes_Spawns)
			{
				if (GetDistance_VDIST2(Game.Player.Character.Position, planes_Spawn.XYZ, "<", 500f))
				{
					World.DrawMarker(MarkerType.Cylinder, planes_Spawn.XYZ, Vector3.Zero, Vector3.Zero, new Vector3(3f, 3f, 100f), Color.Yellow);
				}
			}
			if (Game.IsControlPressed(GTA.Control.Sprint) && Game.IsControlJustPressed(GTA.Control.Context) && Game.Player.Character.CurrentVehicle != null)
			{
				Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;
				if (currentVehicle.Model.IsHelicopter)
				{
					Vector4 item = new Vector4(currentVehicle.Position.X, currentVehicle.Position.Y, currentVehicle.Position.Z, currentVehicle.Heading);
					Helicopters_Spawns.Add(item);
					Lines_Helicopters.Add("new Vector4(" + currentVehicle.Position.X + "f," + currentVehicle.Position.Y + "f," + currentVehicle.Position.Z + "f," + currentVehicle.Heading + "f),");
					File.WriteAllLines("scripts\\HelicopterSpawns.txt", Lines_Helicopters.ToArray());
				}
				if (currentVehicle.Model.IsPlane)
				{
					Vector4 item2 = new Vector4(currentVehicle.Position.X, currentVehicle.Position.Y, currentVehicle.Position.Z, currentVehicle.Heading);
					Planes_Spawns.Add(item2);
					Lines_Planes.Add("new Vector4(" + currentVehicle.Position.X + "f," + currentVehicle.Position.Y + "f," + currentVehicle.Position.Z + "f," + currentVehicle.Heading + "f),");
					File.WriteAllLines("scripts\\PlanesSpawns.txt", Lines_Planes.ToArray());
				}
				if (currentVehicle.Model.IsBoat)
				{
					Vector4 item3 = new Vector4(currentVehicle.Position.X, currentVehicle.Position.Y, currentVehicle.Position.Z, currentVehicle.Heading);
					Boats_Spawns.Add(item3);
					Lines_Boats.Add("new Vector4(" + currentVehicle.Position.X + "f," + currentVehicle.Position.Y + "f," + currentVehicle.Position.Z + "f," + currentVehicle.Heading + "f),");
					File.WriteAllLines("scripts\\BoatSpawns.txt", Lines_Boats.ToArray());
				}
				if (currentVehicle.Model.IsCar)
				{
					Vector4 item4 = new Vector4(currentVehicle.Position.X, currentVehicle.Position.Y, currentVehicle.Position.Z, currentVehicle.Heading);
					Cars_Spawns.Add(item4);
					Lines_Cars.Add("new Vector4(" + currentVehicle.Position.X + "f," + currentVehicle.Position.Y + "f," + currentVehicle.Position.Z + "f," + currentVehicle.Heading + "f),");
					File.WriteAllLines("scripts\\CarsSpawns.txt", Lines_Cars.ToArray());
				}
			}
		}

		private unsafe void onTick(object sender, EventArgs e)
		{
			if (ConvStarted && !Function.Call<bool>(Hash.IS_SCRIPTED_CONVERSATION_ONGOING))
			{
				ConvStarted = false;
				if (ConvLester != null)
				{
					ConvLester.Delete();
				}
			}
			int num = default(int);
			int num2 = default(int);
			Function.Call(Hash.GET_ACTUAL_SCREEN_RESOLUTION, &num, &num2);
			bool flag = false;
			if (modLemonUIObjectPool.AreAnyVisible)
			{
				MenuOpen = true;
			}
			else if (MenuOpen)
			{
				CloseMenuItem();
				MenuOpen = false;
			}
			if (Game.IsControlPressed(GTA.Control.SelectWeapon) && Game.Player.Character.CurrentVehicle == null && !CayoPericoHeist.DisplayingCantChooseWeaponDialogue)
			{
				DisplayHelpTextThisFrame("Press " + ReturnButtonString(Snack_hotkey) + " to eat a snack.\nPress " + ReturnButtonString(ARMOUR_hotkey) + " to use armor.", 1);
				float num3 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
				float num4 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
				if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, YTDLoader.YTDtoLoad))
				{
					UI.drawSprite3(YTDLoader.YTDtoLoad, "Item_Food", -0.5299f, 0.0768f, 120f, 120f, 255, 255, 255, 255);
					UI.drawSprite3(YTDLoader.YTDtoLoad, "Item_Vest", -0.3799f, 0.0768f, 120f, 120f, 255, 255, 255, 255);
					drawText(purchasedsnacks.ToString(), 0.0361400023f, 0.1046f, 0.35f, 255, 255, 255);
					drawText(purchasedarmour.ToString(), 0.1052f, 0.1046f, 0.35f, 255, 255, 255);
				}
				flag = true;
			}
			if (Game.Player.Character.CurrentVehicle == null && flag)
			{
				if (Game.IsControlJustPressed((GTA.Control)Snack_hotkey))
				{
					if (purchasedsnacks > 0)
					{
						Random random = new Random();
						Game.Player.Character.Health = Game.Player.Character.Health + random.Next(5, 20);
						Notification.Show("Player: Eating a snack ");
						purchasedsnacks--;
						Config.SetValue("Setup", "Armour", purchasedarmour);
						Config.SetValue("Setup", "Snacks", purchasedsnacks);
						Config.Save();
					}
					else
					{
						Notification.Show("Lester: you have no snacks ");
					}
				}
				if (Game.IsControlJustPressed((GTA.Control)ARMOUR_hotkey))
				{
					if (purchasedarmour > 0)
					{
						Random random2 = new Random();
						if (Game.Player.Character.Armor + 99 < Function.Call<int>(Hash.GET_PLAYER_MAX_ARMOUR, Game.Player))
						{
							Game.Player.Character.Armor += 100;
							Notification.Show("Player: putting on body armor, cover me!");
							purchasedarmour--;
							Config.SetValue("Setup", "Armour", purchasedarmour);
							Config.SetValue("Setup", "Snacks", purchasedsnacks);
							Config.Save();
						}
						else
						{
							Game.Player.Character.Armor = Function.Call<int>(Hash.GET_PLAYER_MAX_ARMOUR, Game.Player);
							Notification.Show("Player: Armour Full!");
						}
					}
					else
					{
						Notification.Show("Lester: you have no body armour ");
					}
				}
			}
			if (ifruit != null)
			{
				ifruit.Update();
			}
			if (modLemonUIObjectPool != null)
			{
				modLemonUIObjectPool.Process();
			}
			if (noRestrictedzones)
			{
				Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "am_armybase");
				Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "re_armybase");
				Function.Call(Hash.TERMINATE_ALL_SCRIPTS_WITH_THIS_NAME, "RestrictedAreas");
				if (Game.GameTime >= NoRestrictedZonesTimer)
				{
					Notification.Show("Lester: ok thats it, law inforcement will detect you now in Restricted Zones");
					noRestrictedzones = false;
				}
			}
			if (invunrablewait)
			{
				Game.Player.Character.Health = Game.Player.Character.MaxHealth;
				if (Game.GameTime >= InvunrableTime)
				{
					Notification.Show("Lester: ok thats it, invunrability has worn off");
					invunrablewait = false;
				}
			}
			if (NoCops)
			{
				Game.MaxWantedLevel = 0;
				if (Game.GameTime >= NoCopstime)
				{
					Game.MaxWantedLevel = 5;
					NoCops = false;
					Notification.Show("Lester: Thats it, your back on the radar, be carefull.");
				}
			}
			if (Blackoutwait)
			{
				if (Game.GameTime >= BlackoutTimer)
				{
					Notification.Show("Lester: ok thats it, City has restored power to the grid.");
					Blackoutwait = false;
				}
				Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, true);
				Vehicle[] nearbyVehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 10000f);
				Vehicle[] array = nearbyVehicles;
				foreach (Vehicle vehicle in array)
				{
					if (vehicle.GetPedOnSeat(VehicleSeat.Driver) != null && (vehicle.Model != "POLICE" || vehicle.Model != "POLICE2" || vehicle.Model != "POLICE3" || vehicle.Model != "POLICE4" || vehicle.Model != "FBI2" || vehicle.Model != "FBI" || vehicle.Model != "POLICET"))
					{
						vehicle.GetPedOnSeat(VehicleSeat.Driver).DrivingStyle = DrivingStyle.Rushed;
						Ped[] nearbyPeds = World.GetNearbyPeds(Game.Player.Character.Position, 1000f);
						int maxValue = nearbyPeds.Length;
						Random random3 = new Random();
						int num5 = random3.Next(1, maxValue);
						if (!vehicle.GetPedOnSeat(VehicleSeat.Driver).IsPlayer)
						{
							vehicle.GetPedOnSeat(VehicleSeat.Driver).Task.ReactAndFlee(nearbyPeds[num5]);
						}
					}
				}
				Game.Player.IgnoredByPolice = true;
				Game.MaxWantedLevel = 0;
				Game.Player.WantedLevel = 0;
				offradartimer++;
				offradarbool = true;
			}
			if (offradarbool && offradartimer == 2880)
			{
				offradarbool = false;
				offradartimer = 0;
				Game.Player.IgnoredByPolice = false;
				Game.MaxWantedLevel = 5;
				Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, false);
				Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, false);
				Notification.Show("Lester: ok thats it, you are back on the radar ");
				if (Blackoutwait)
				{
					Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, false);
					Notification.Show("Lester: ok thats it, blackout has disipated ");
				}
			}
			if (Game.Player.WantedLevel == 0)
			{
				wantedlevelprice = 0;
			}
			else if (Game.Player.WantedLevel == 1)
			{
				wantedlevelprice = 200;
			}
			else if (Game.Player.WantedLevel == 2)
			{
				wantedlevelprice = 2500;
			}
			else if (Game.Player.WantedLevel == 3)
			{
				wantedlevelprice = 10000;
			}
			else if (Game.Player.WantedLevel == 4)
			{
				wantedlevelprice = 100000;
			}
			else if (Game.Player.WantedLevel == 5)
			{
				wantedlevelprice = 3000000;
			}
		}

		public void CloseMenuItem()
		{
			if (ConvLester != null)
			{
				ConvLester.Delete();
			}
			ConvLester = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvLester.Opacity = 0;
			ConvLester.IsInvincible = true;
			ConvLester.IsCollisionEnabled = false;
			ConvLester.AttachTo(Game.Player.Character);
			ConvLester.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvLester, "lester");
			Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_MQAA", "lester", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
		}

		public void AcceptItem()
		{
			if (ConvLester != null)
			{
				ConvLester.Delete();
			}
			ConvLester = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvLester.Opacity = 0;
			ConvLester.IsInvincible = true;
			ConvLester.IsCollisionEnabled = false;
			ConvLester.AttachTo(Game.Player.Character);
			ConvLester.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvLester, "lester");
			Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_MNAA", "lester", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
		}

		public void DeclineItem()
		{
			if (ConvLester != null)
			{
				ConvLester.Delete();
			}
			ConvLester = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvLester.Opacity = 0;
			ConvLester.IsInvincible = true;
			ConvLester.IsCollisionEnabled = false;
			ConvLester.AttachTo(Game.Player.Character);
			ConvLester.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvLester, "lester");
			Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_MOAA", "lester", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
		}

		public void loadMenu(iFruitContact contact)
		{
			if (ConvLester != null)
			{
				ConvLester.Delete();
			}
			ConvLester = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvLester.Opacity = 0;
			ConvLester.IsInvincible = true;
			ConvLester.IsCollisionEnabled = false;
			ConvLester.AttachTo(Game.Player.Character);
			ConvLester.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvLester, "lester");
			Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, "MPCT_AAAA", "lester", 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
			ifruit.Close();
			mainMenu.Visible = !mainMenu.Visible;
		}
	}
}
