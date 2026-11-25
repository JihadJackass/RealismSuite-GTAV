using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Menus;

namespace GTAO_DiamondCasino
{
	internal class DrinksMenu : Script
	{
		public enum Drinktype
		{
			None,
			Vodka,
			TheMountWiskey,
			RichardsWhiskey,
			MacbethWhiskey,
			CazafortunasTequila,
			ChampaineSilver,
			ChampaineGold,
			ChampaineDiamond
		}

		public static Vector3 BarPos1 = new Vector3(1109.90808f, 208.298447f, -49.4578857f);

		public static Vector3 BarPos2 = new Vector3(1113.91357f, 208.801529f, -49.47968f);

		public static Vector3 BarPos3 = new Vector3(945.228455f, 13.6324148f, 116.115761f);

		public static Vector3 BarPos4 = new Vector3(946.9315f, 16.13344f, 116.265991f);

		public static Utils.MissionOjects Bartenders = new Utils.MissionOjects(new List<Utils.ObjectPlacementMap>
		{
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 441919, 3163733717u, new Vector3(1109.90808f, 208.298447f, -49.4578857f), new Vector3(0f, 0f, 75.04008f), 2725352035u, 200, 0, "false", "anim@amb@clubhouse@bar@drink@base", "idle_a_bartender", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 118341, 3163733717u, new Vector3(1113.91357f, 208.801529f, -49.47968f), new Vector3(-0f, -0f, -64.73485f), 2725352035u, 200, 0, "false", "anim@amb@clubhouse@bar@drink@base", "idle_a_bartender", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 733525, 3163733717u, new Vector3(945.228455f, 13.6324148f, 116.115761f), new Vector3(0f, 0f, 59.94229f), 2725352035u, 200, 0, "false", "anim@amb@clubhouse@bar@drink@base", "idle_a_bartender", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f)),
			new Utils.ObjectPlacementMap(Function.Call<int>(Hash.GET_HASH_KEY, "Casino Staff"), 218207, 337826907u, new Vector3(946.9315f, 16.13344f, 116.265991f), new Vector3(0f, 0f, 58.30432f), 2725352035u, 200, 0, "false", "anim@amb@casino@mini@drinking@champagne_drinking@heels@base", "bartender_idle", -1, -1, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f))
		});

		public static ScaledRectangle RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 108));

		public static List<NativeMenu> GLemonUIMenusNativeMenus = new List<NativeMenu>();

		public static ObjectPool ObjectPool;

		public static NativeMenu MainMenu;

		public Vector3 Marker = new Vector3(1567.92f, 249.66f, -47f);

		public static Ped BarGirl;

		public static bool ChampagneLoop;

		public static int ChampagneStage;

		public static int ChampagneShakeTimer;

		public static int ChampagneShakes;

		public static int ChampagneSprayingTimer;

		public static float ChampagneShakeIntensity;

		public static Prop BarDrinkProp;

		public static Prop BarDrinkProp_2;

		public static float ChampagneMoveHorizontal;

		public static float ChampagneMoveVertical;

		public static int champagnefx;

		public static int barDismissScene;

		public static bool Bar = true;

		public static int DrinkStage = -1;

		public static Drinktype CurrentDrinktype;

		public static Vector3 DrinkOrigin;

		public static float DrinkHeading;

		public static float DrunkAmt;

		public static int GetNewBartnender;

		public static int Scene1;

		public static Prop ShotGlass;

		public static Prop Bottle;

		public static Prop SaltShaker;

		public static string SaltShakerModel = "h4_prop_h4_saltshaker_01a";

		public static Prop Plate;

		public static string PlateModel = "h4_prop_h4_tray_01a";

		public static Prop Lime;

		public static string LimeModel = "h4_prop_h4_lime_01a";

		public static string TequilaFlaskModel = "h4_prop_h4_t_bottle_01a";

		public static string TequilaGlassModel = "h4_p_cs_shot_glass_2_s";

		public static string Shotglass2Model = "h4_p_cs_shot_glass_2_s";

		public static string TheMountWhskey = "h4_prop_battle_whiskey_opaque_s";

		public static string RichardWhiskey = "h4_prop_battle_whiskey_bottle_s";

		public static string MacbethWhiskey = "h4_prop_battle_whiskey_bottle_2_s";

		public static string VodkaBottle = "h4_prop_battle_decanter_02_s";

		public static bool PlayingSyncScene;

		public static int WhiskeyShotsTaken = 0;

		public static float Drunkness;

		public static int NextDrunkTimer;

		public static bool IsDrunk;

		public static string DrunkLevel;

		public static float ActualDrunkness;

		public static int NextDrunkInc;

		public static bool RunMethodNow()
		{
			bool result = true;
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, BarPos1, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, BarPos2, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, BarPos3, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, BarPos4, Class1.GetDistSign.GreaterEqual, 5f))
			{
				result = false;
			}
			return result;
		}

		public static string LoadDict(string dict)
		{
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, dict))
			{
				Function.Call(Hash.REQUEST_ANIM_DICT, dict);
				Script.Yield();
			}
			return dict;
		}

		public static void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		public static Model RequestModel(int Name)
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

		public static Model RequestModel(PedHash Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid && !result.IsLoaded)
			{
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
		}

		public static Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(250);
			if (result.IsInCdImage && result.IsValid && !result.IsLoaded)
			{
				return result;
			}
			result.MarkAsNoLongerNeeded();
			return result;
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

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return (float)Math.Sqrt(Function.Call<float>(Hash.VDIST2, _1.X, _1.Y, _1.Z, _2.X, _2.Y, _2.Z));
		}

		public static void SetBanner_Special(NativeMenu menu)
		{
			menu.Banner = new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino");
		}

		public static void SetBanner(NativeMenu menu)
		{
			menu.Banner = new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino");
		}

		public static void CreateBanner()
		{
			RectangleGUI = new ScaledRectangle(new Point(0, 0), new Size(0, 0));
			RectangleGUI.Color = Color.FromArgb(255, 0, 0, 0);
		}

		public DrinksMenu()
		{
			if (_Debug.DrinksMenu == -1)
			{
				return;
			}
			base.Tick += OnTick;
			ObjectPool = new ObjectPool();
			MainMenu = new NativeMenu("", "Drinks", "", new ScaledTexture(PointF.Empty, new SizeF(512f, 128f), "shopui_title_casino", "shopui_title_casino"));
			GLemonUIMenusNativeMenus.Add(MainMenu);
			ObjectPool.Add(MainMenu);
			NativeItem nativeItem = new NativeItem("Vodka Shot", "", "$75");
			MainMenu.Add(nativeItem);
			nativeItem.Activated += delegate
			{
				if (Game.Player.Money >= 75)
				{
					Game.Player.Money -= 75;
					DrinkIntro(Drinktype.Vodka);
					DrinkStage = 0;
				}
			};
			NativeItem nativeItem2 = new NativeItem("The Mount Whiskey Shot", "", "$250");
			MainMenu.Add(nativeItem2);
			nativeItem2.Activated += delegate
			{
				if (Game.Player.Money >= 250)
				{
					Game.Player.Money -= 250;
					DrinkIntro(Drinktype.TheMountWiskey);
					DrinkStage = 0;
				}
			};
			NativeItem nativeItem3 = new NativeItem("Richard's Whiskey Shot", "", "$1,000");
			MainMenu.Add(nativeItem3);
			nativeItem3.Activated += delegate
			{
				if (Game.Player.Money >= 1000)
				{
					Game.Player.Money -= 1000;
					DrinkIntro(Drinktype.RichardsWhiskey);
					DrinkStage = 0;
				}
			};
			NativeItem nativeItem4 = new NativeItem("Macbeth Whiskey Shot", "", "$5,000");
			MainMenu.Add(nativeItem4);
			nativeItem4.Activated += delegate
			{
				if (Game.Player.Money >= 5000)
				{
					Game.Player.Money -= 5000;
					DrinkIntro(Drinktype.MacbethWhiskey);
					DrinkStage = 0;
				}
			};
			NativeItem nativeItem5 = new NativeItem("Cazafortunas Tequila", "", "$5,000");
			MainMenu.Add(nativeItem5);
			nativeItem5.Activated += delegate
			{
				if (Game.Player.Money >= 5000)
				{
					Game.Player.Money -= 5000;
					DrinkIntro(Drinktype.CazafortunasTequila);
					DrinkStage = 0;
				}
			};
			NativeItem nativeItem6 = new NativeItem("Bleuter'd Champaine Slver", "", "$30,000");
			MainMenu.Add(nativeItem6);
			nativeItem6.Activated += delegate
			{
				if (Game.Player.Money >= 30000)
				{
					Game.Player.Money -= 30000;
					DrinkIntro(Drinktype.ChampaineSilver);
					DrinkStage = 0;
				}
			};
			NativeItem nativeItem7 = new NativeItem("Bleuter'd Champaine Gold", "", "$50,000");
			MainMenu.Add(nativeItem7);
			nativeItem7.Activated += delegate
			{
				if (Game.Player.Money >= 50000)
				{
					Game.Player.Money -= 50000;
					DrinkIntro(Drinktype.ChampaineGold);
					DrinkStage = 0;
				}
			};
			NativeItem nativeItem8 = new NativeItem("Bleuter'd Champaine Diamond", "", "$150,000");
			MainMenu.Add(nativeItem8);
			nativeItem8.Activated += delegate
			{
				if (Game.Player.Money >= 150000)
				{
					Game.Player.Money -= 150000;
					DrinkIntro(Drinktype.ChampaineDiamond);
					DrinkStage = 0;
				}
			};
			for (int i = 0; i < GLemonUIMenusNativeMenus.Count; i++)
			{
				SetBanner(GLemonUIMenusNativeMenus[i]);
			}
		}

		public static void MOVE_TASK_REQUEST_STATE(Ped ped, string name)
		{
			Function.Call(Hash.REQUEST_TASK_MOVE_NETWORK_STATE_TRANSITION, ped, name);
		}

		public static void StartChampaineAnim()
		{
			Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_xs_celebration");
			Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_xs_celebration");
			while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_xs_celebration"))
			{
				Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_xs_celebration");
				Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_xs_celebration");
				Script.Wait(1);
			}
			Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
			if (champagnefx != -1)
			{
				Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, champagnefx);
			}
			if (BarDrinkProp != null)
			{
				BarDrinkProp.Delete();
			}
			if (BarDrinkProp_2 != null)
			{
				BarDrinkProp_2.Delete();
			}
			if (CurrentDrinktype == Drinktype.ChampaineDiamond)
			{
				BarDrinkProp = Bottle;
				BarDrinkProp_2 = GlobalHelper.CreateProp(RequestModel("h4_prop_battle_champ_open_03"), Game.Player.Character.Position, false, false);
				BarDrinkProp_2.IsVisible = false;
			}
			if (CurrentDrinktype == Drinktype.ChampaineGold)
			{
				BarDrinkProp = Bottle;
				BarDrinkProp_2 = GlobalHelper.CreateProp(RequestModel("h4_prop_battle_champ_open_02"), Game.Player.Character.Position, false, false);
				BarDrinkProp_2.IsVisible = false;
			}
			if (CurrentDrinktype == Drinktype.ChampaineSilver)
			{
				BarDrinkProp = Bottle;
				BarDrinkProp_2 = GlobalHelper.CreateProp(RequestModel("h4_prop_battle_champ_open"), Game.Player.Character.Position, false, false);
				BarDrinkProp_2.IsVisible = false;
			}
			ChampagneStage = 0;
			ChampagneMoveVertical = 0.5f;
			ChampagneMoveHorizontal = 0.5f;
			ChampagneShakes = 0;
			ChampagneShakeIntensity = 0f;
			Bar = false;
			LoadDict("ANIM@AMB@NIGHTCLUB@MINI@DRINKING@CHAMPAGNE_DRINKING@BASE@");
			Function.Call(Hash.TASK_MOVE_NETWORK_BY_NAME, Game.Player.Character, "Champagne_Drinking_Mini_Game", 0f, true, "ANIM@AMB@NIGHTCLUB@MINI@DRINKING@CHAMPAGNE_DRINKING@BASE@", 0);
			ChampagneLoop = true;
			PlayingSyncScene = true;
			Notification.Show("Start Champaine 2");
		}

		public static void DrinkIntro(Drinktype D)
		{
			ObjectPool.HideAll();
			DrinkOrigin = BarGirl.GetOffsetPosition(new Vector3(0.15f, 0.85f, 1.37f));
			DrinkHeading = BarGirl.Heading - 180f;
			CurrentDrinktype = D;
			if (D == Drinktype.Vodka)
			{
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel(Shotglass2Model), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(VodkaBottle), BarGirl.Position, false, false);
				string dict = "anim@amb@casino@mini@drinking@bar@drink@heels@base";
				LoadDict(dict);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict), "intro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "intro_bartender", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict), "intro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "intro_player", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "intro_shot_glass", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "intro_whiskey", LoadDict(dict), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.TheMountWiskey)
			{
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(TheMountWhskey), BarGirl.Position, false, false);
				string dict2 = "anim@amb@casino@mini@drinking@bar@drink@heels@base";
				LoadDict(dict2);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict2), "intro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "intro_bartender", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict2), "intro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "intro_player", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "intro_shot_glass", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "intro_whiskey", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.RichardsWhiskey)
			{
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(RichardWhiskey), BarGirl.Position, false, false);
				string dict3 = "anim@amb@casino@mini@drinking@bar@drink@heels@base";
				LoadDict(dict3);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict3), "intro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "intro_bartender", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict3), "intro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "intro_player", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "intro_shot_glass", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "intro_whiskey", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.MacbethWhiskey)
			{
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(MacbethWhiskey), BarGirl.Position, false, false);
				string dict4 = "anim@amb@casino@mini@drinking@bar@drink@heels@base";
				LoadDict(dict4);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict4), "intro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "intro_bartender", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict4), "intro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "intro_player", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "intro_shot_glass", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "intro_whiskey", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.CazafortunasTequila)
			{
				if (SaltShaker != null)
				{
					SaltShaker.Delete();
				}
				if (Plate != null)
				{
					Plate.Delete();
				}
				if (Lime != null)
				{
					Lime.Delete();
				}
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel(TequilaGlassModel), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(TequilaFlaskModel), BarGirl.Position, false, false);
				Lime = GlobalHelper.CreateProp(RequestModel(LimeModel), BarGirl.Position, false, false);
				SaltShaker = GlobalHelper.CreateProp(RequestModel(SaltShakerModel), BarGirl.Position, false, false);
				Plate = GlobalHelper.CreateProp(RequestModel(PlateModel), BarGirl.Position, false, false);
				string dict5 = "anim@amb@mini@drinking@tequila@base@";
				LoadDict(dict5);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict5), "intro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "intro_bartender", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict5), "intro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "intro_player", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "intro_shotglass", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "intro_tequila_bottle", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SaltShaker, Scene1, "intro_saltshaker", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Plate, Scene1, "intro_tray", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Lime, Scene1, "intro_lime", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.ChampaineSilver)
			{
				DrinkOrigin = BarGirl.GetOffsetPosition(new Vector3(0.2f, 0.85f, 1.35f));
				DrinkHeading = BarGirl.Heading - 180f;
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				Bottle = GlobalHelper.CreateProp(RequestModel("h4_prop_battle_champ_closed"), BarGirl.Position, false, false);
				string dict6 = "anim@amb@nightclub@mini@drinking@champagne_drinking@base@";
				LoadDict(dict6);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict6), "bartender_intro", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "bartender_intro", LoadDict(dict6), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "bartender_intro_bottle", LoadDict(dict6), 16f, -16f, 0, 1148846080);
			}
			if (D == Drinktype.ChampaineGold)
			{
				DrinkOrigin = BarGirl.GetOffsetPosition(new Vector3(0.2f, 0.85f, 1.35f));
				DrinkHeading = BarGirl.Heading - 180f;
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				Bottle = GlobalHelper.CreateProp(RequestModel("h4_prop_battle_champ_closed_02"), BarGirl.Position, false, false);
				string dict7 = "anim@amb@nightclub@mini@drinking@champagne_drinking@base@";
				LoadDict(dict7);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict7), "bartender_intro", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "bartender_intro", LoadDict(dict7), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "bartender_intro_bottle", LoadDict(dict7), 16f, -16f, 0, 1148846080);
			}
			if (D == Drinktype.ChampaineDiamond)
			{
				DrinkOrigin = BarGirl.GetOffsetPosition(new Vector3(0.2f, 0.85f, 1.35f));
				DrinkHeading = BarGirl.Heading - 180f;
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				Bottle = GlobalHelper.CreateProp(RequestModel("h4_prop_battle_champ_closed_03"), BarGirl.Position, false, false);
				string dict8 = "anim@amb@nightclub@mini@drinking@champagne_drinking@base@";
				LoadDict(dict8);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict8), "bartender_intro", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "bartender_intro", LoadDict(dict8), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "bartender_intro_bottle", LoadDict(dict8), 16f, -16f, 0, 1148846080);
			}
			PlayingSyncScene = true;
		}

		public static void DrinkOutro(Drinktype D)
		{
			Game.Player.Character.Task.ClearAllImmediately();
			CurrentDrinktype = D;
			if (D == Drinktype.Vodka)
			{
				BarGirl.Task.ClearAllImmediately();
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel(Shotglass2Model), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(VodkaBottle), BarGirl.Position, false, false);
				string dict = "anim@amb@casino@mini@drinking@bar@drink@heels@base";
				LoadDict(dict);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict), "outro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "outro_bartender", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict), "outro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "outro_player", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "outro_shot_glass", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "outro_whiskey", LoadDict(dict), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.TheMountWiskey)
			{
				BarGirl.Task.ClearAllImmediately();
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(TheMountWhskey), BarGirl.Position, false, false);
				string dict2 = "anim@amb@casino@mini@drinking@bar@drink@heels@base";
				LoadDict(dict2);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict2), "outro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "outro_bartender", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict2), "outro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "outro_player", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "outro_shot_glass", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "outro_whiskey", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.RichardsWhiskey)
			{
				BarGirl.Task.ClearAllImmediately();
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(RichardWhiskey), BarGirl.Position, false, false);
				string dict3 = "anim@amb@casino@mini@drinking@bar@drink@heels@base";
				LoadDict(dict3);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict3), "outro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "outro_bartender", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict3), "outro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "outro_player", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "outro_shot_glass", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "outro_whiskey", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.MacbethWhiskey)
			{
				BarGirl.Task.ClearAllImmediately();
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(MacbethWhiskey), BarGirl.Position, false, false);
				string dict4 = "anim@amb@casino@mini@drinking@bar@drink@heels@base";
				LoadDict(dict4);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict4), "outro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "outro_bartender", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict4), "outro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "outro_player", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "outro_shot_glass", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "outro_whiskey", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
			if (D == Drinktype.CazafortunasTequila)
			{
				BarGirl.Task.ClearAllImmediately();
				if (SaltShaker != null)
				{
					SaltShaker.Delete();
				}
				if (Plate != null)
				{
					Plate.Delete();
				}
				if (Lime != null)
				{
					Lime.Delete();
				}
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel(TequilaGlassModel), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(TequilaFlaskModel), BarGirl.Position, false, false);
				Lime = GlobalHelper.CreateProp(RequestModel(LimeModel), BarGirl.Position, false, false);
				SaltShaker = GlobalHelper.CreateProp(RequestModel(SaltShakerModel), BarGirl.Position, false, false);
				Plate = GlobalHelper.CreateProp(RequestModel(PlateModel), BarGirl.Position, false, false);
				string dict5 = "anim@amb@mini@drinking@tequila@base@";
				LoadDict(dict5);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict5), "outro_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "outro_bartender", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict5), "outro_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, "outro_player", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, "outro_shotglass", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "outro_tequila_bottle", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SaltShaker, Scene1, "outro_saltshaker", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Plate, Scene1, "outro_tray", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Lime, Scene1, "outro_lime", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
			}
		}

		public static void GetDrink(Drinktype D)
		{
			if (D == Drinktype.Vodka)
			{
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel(Shotglass2Model), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(VodkaBottle), BarGirl.Position, false, false);
				string dict = "anim@amb@casino@mini@drinking@bar@drink@heels@" + DrunkLevel;
				LoadDict(dict);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict), DrunkLevel + "_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, DrunkLevel + "_bartender", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict), DrunkLevel + "_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, DrunkLevel + "_player", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, DrunkLevel + "_shot_glass", LoadDict(dict), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, DrunkLevel + "_whiskey", LoadDict(dict), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
				AddDrunkness(0.25f);
			}
			if (D == Drinktype.TheMountWiskey)
			{
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(TheMountWhskey), BarGirl.Position, false, false);
				string dict2 = "anim@amb@casino@mini@drinking@bar@drink@heels@" + DrunkLevel;
				LoadDict(dict2);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict2), DrunkLevel + "_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, DrunkLevel + "_bartender", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict2), DrunkLevel + "_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, DrunkLevel + "_player", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, DrunkLevel + "_shot_glass", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, DrunkLevel + "_whiskey", LoadDict(dict2), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
				AddDrunkness(0.18f);
			}
			if (D == Drinktype.RichardsWhiskey)
			{
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(RichardWhiskey), BarGirl.Position, false, false);
				string dict3 = "anim@amb@casino@mini@drinking@bar@drink@heels@" + DrunkLevel;
				LoadDict(dict3);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict3), DrunkLevel + "_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, DrunkLevel + "_bartender", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict3), DrunkLevel + "_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, DrunkLevel + "_player", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, DrunkLevel + "_shot_glass", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, DrunkLevel + "_whiskey", LoadDict(dict3), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
				AddDrunkness(0.23f);
			}
			if (D == Drinktype.MacbethWhiskey)
			{
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel("p_cs_shot_glass_2_s"), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(MacbethWhiskey), BarGirl.Position, false, false);
				string dict4 = "anim@amb@casino@mini@drinking@bar@drink@heels@" + DrunkLevel;
				LoadDict(dict4);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict4), DrunkLevel + "_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, DrunkLevel + "_bartender", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict4), DrunkLevel + "_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, DrunkLevel + "_player", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, DrunkLevel + "_shot_glass", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, DrunkLevel + "_whiskey", LoadDict(dict4), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
				AddDrunkness(0.28f);
			}
			if (D == Drinktype.CazafortunasTequila)
			{
				if (SaltShaker != null)
				{
					SaltShaker.Delete();
				}
				if (Plate != null)
				{
					Plate.Delete();
				}
				if (Lime != null)
				{
					Lime.Delete();
				}
				if (ShotGlass != null)
				{
					ShotGlass.Delete();
				}
				if (Bottle != null)
				{
					Bottle.Delete();
				}
				Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
				ShotGlass = GlobalHelper.CreateProp(RequestModel(TequilaGlassModel), BarGirl.Position, false, false);
				Bottle = GlobalHelper.CreateProp(RequestModel(TequilaFlaskModel), BarGirl.Position, false, false);
				Lime = GlobalHelper.CreateProp(RequestModel(LimeModel), BarGirl.Position, false, false);
				SaltShaker = GlobalHelper.CreateProp(RequestModel(SaltShakerModel), BarGirl.Position, false, false);
				Plate = GlobalHelper.CreateProp(RequestModel(PlateModel), BarGirl.Position, false, false);
				string dict5 = "anim@amb@mini@drinking@tequila@" + DrunkLevel + "@";
				LoadDict(dict5);
				Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict5), DrunkLevel + "_bartender", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, DrunkLevel + "_bartender", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, Game.Player.Character, Scene1, LoadDict(dict5), DrunkLevel + "_player", 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Game.Player.Character, Scene1, DrunkLevel + "_player", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, ShotGlass, Scene1, DrunkLevel + "_shotglass", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, DrunkLevel + "_tequila_bottle", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, SaltShaker, Scene1, DrunkLevel + "_saltshaker", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Plate, Scene1, DrunkLevel + "_tray", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Lime, Scene1, DrunkLevel + "_lime", LoadDict(dict5), 16f, -16f, 0, 1148846080);
				PlayingSyncScene = true;
				AddDrunkness(0.42f);
			}
		}

		public static void AddDrunkness(float atm)
		{
			IsDrunk = true;
			Drunkness += 0.25f;
			NextDrunkTimer = Game.GameTime + 30000;
			Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "drunk");
			if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
			{
				Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
			}
			if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
			{
				Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, Drunkness);
				Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, "drunk", Drunkness);
			}
		}

		public unsafe static void OnTick(object sender, EventArgs e)
		{
			if (Class1.GetDistance_VDIST2(Game.Player.Character.Position, BarPos1, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, BarPos2, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, BarPos3, Class1.GetDistSign.GreaterEqual, 5f) && Class1.GetDistance_VDIST2(Game.Player.Character.Position, BarPos4, Class1.GetDistSign.GreaterEqual, 5f))
			{
				_Debug.DrinksMenu = 0;
				return;
			}
			_Debug.DrinksMenu = 1;
			if (Game.GameTime > NextDrunkInc)
			{
				NextDrunkInc = Game.GameTime + 250;
				if (ActualDrunkness != Drunkness)
				{
					if (ActualDrunkness < Drunkness)
					{
						ActualDrunkness += 0.05f;
					}
					if (ActualDrunkness < Drunkness)
					{
						ActualDrunkness -= 0.05f;
					}
				}
			}
			if (Drunkness >= 0.1f)
			{
				if (Drunkness <= 0.25f)
				{
					DrunkLevel = "one";
				}
				if (Drunkness >= 0.25f)
				{
					DrunkLevel = "two";
				}
				if (Drunkness >= 0.5f)
				{
					DrunkLevel = "three";
				}
				if (Drunkness >= 0.75f)
				{
					DrunkLevel = "four";
				}
				if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
				{
					Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
				}
				Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "drunk");
				Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, 1f);
				Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, "drunk", ActualDrunkness * 3f);
				if (Game.GameTime >= NextDrunkTimer)
				{
					if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
					{
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					}
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "drunk");
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, 1f);
					Function.Call(Hash.SET_TRANSITION_TIMECYCLE_MODIFIER, "drunk", ActualDrunkness * 3f);
					NextDrunkTimer = Game.GameTime + 1000;
					Drunkness -= 0.025f;
				}
			}
			else
			{
				ActualDrunkness = 0f;
				DrunkLevel = "one";
				if (IsDrunk)
				{
					IsDrunk = false;
					if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
					{
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					}
					Function.Call(Hash.SET_TIMECYCLE_MODIFIER, "default");
					if (Function.Call<int>(Hash.GET_TIMECYCLE_MODIFIER_INDEX) != -1)
					{
						Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);
					}
				}
			}
			if (!Class1.IsinInterior && !PenthouseStuff.IsInPenhouse)
			{
				_Debug.DrinksMenu = 0;
			}
			else
			{
				if (!Class1.IsinInterior && !PenthouseStuff.IsInPenhouse)
				{
					return;
				}
				_Debug.DrinksMenu = 1;
				if (PlayingSyncScene)
				{
					if (ChampagneLoop)
					{
						if (ChampagneStage < 4)
						{
							Function.Call(Hash.DISPLAY_RADAR, false);
						}
						if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, Game.Player.Character, Game.GenerateHash("PickupBottle")))
						{
							Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, BarDrinkProp, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 1, 2, 1);
						}
						if (ChampagneStage == 0 && Function.Call<bool>(Hash.GET_TASK_MOVE_NETWORK_EVENT, Game.Player.Character, "IntroFinished") && Function.Call<bool>(Hash.IS_TASK_MOVE_NETWORK_READY_FOR_TRANSITION, Game.Player.Character))
						{
							ChampagneStage = 1;
							ChampagneShakeTimer = Game.GameTime;
							MOVE_TASK_REQUEST_STATE(Game.Player.Character, "BottleHold");
						}
						if (ChampagneStage == 1)
						{
							DisplayHelpTextThisFrame("Press ~INPUT_FRONTEND_ACCEPT~ to start shaking.", -1);
							if (Game.IsControlJustPressed(Control.FrontendAccept))
							{
								ChampagneStage = 2;
								MOVE_TASK_REQUEST_STATE(Game.Player.Character, "BottleShake");
							}
						}
						if (ChampagneStage == 2)
						{
							DisplayHelpTextThisFrame("Tap ~INPUT_FRONTEND_ACCEPT~ to shake the champagne bottle\n Pressure " + Math.Round(ChampagneShakeIntensity * 100f) + "%", -1);
							Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Game.Player.Character, "SHAKE_INTENSITY", ChampagneShakeIntensity);
							Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Game.Player.Character, "HORIZONTAL_AIM", 0.5f);
							Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Game.Player.Character, "VERTICLE_AIM", 0.5f);
							if (Game.IsControlJustPressed(Control.FrontendAccept))
							{
								ChampagneShakeTimer = Game.GameTime;
								ChampagneShakes++;
								ChampagneShakeIntensity += 0.05f;
								if (ChampagneShakes > 20 && ChampagneShakeIntensity > 0.97f)
								{
									if (Bottle != null)
									{
										Bottle.Opacity = 0;
									}
									MOVE_TASK_REQUEST_STATE(Game.Player.Character, "ChampagneSpray");
									ChampagneStage = 3;
									ChampagneSprayingTimer = Game.GameTime;
									Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_xs_celebration");
									Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_xs_celebration");
									while (!Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "scr_xs_celebration"))
									{
										Function.Call(Hash.REQUEST_NAMED_PTFX_ASSET, "scr_xs_celebration");
										Function.Call(Hash.USE_PARTICLE_FX_ASSET, "scr_xs_celebration");
										Script.Wait(1);
									}
									champagnefx = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE, "scr_xs_champagne_spray", BarDrinkProp, 0f, 0f, 0f, 0f, 0f, 0f, Function.Call<int>(Hash.GET_ENTITY_BONE_INDEX_BY_NAME, BarDrinkProp, "VFX"), 1065353216, 0, 0, 0);
									Function.Call(Hash.SET_ENTITY_VISIBLE, BarDrinkProp, false, false);
									Function.Call(Hash.SET_ENTITY_VISIBLE, BarDrinkProp_2, true, false);
									Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, BarDrinkProp_2, Game.Player.Character, Game.Player.Character.Bones[Bone.PHRightHand].Index, 0f, 0f, 0f, 0f, 0f, 0f, 0, 0, 0, 1, 2, 1);
									BarDrinkProp_2.IsVisible = true;
									BarDrinkProp.IsVisible = false;
								}
							}
							if (Game.GameTime > ChampagneShakeTimer + 200 && ChampagneShakeIntensity > 0f)
							{
								ChampagneShakeIntensity -= 0.01f;
							}
						}
						if (ChampagneStage == 3)
						{
							DisplayHelpTextThisFrame("Use ~INPUT_MOVE_LR~ and ~INPUT_MOVE_UD~ to control the champagne spray", -1);
							if (Game.GameTime > ChampagneShakeTimer + 10000)
							{
								Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, champagnefx, false);
								MOVE_TASK_REQUEST_STATE(Game.Player.Character, "Outro");
								ChampagneStage = 4;
							}
							Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Game.Player.Character, "SHAKE_INTENSITY", 1f);
							Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Game.Player.Character, "HORIZONTAL_AIM", ChampagneMoveHorizontal);
							Function.Call(Hash.SET_TASK_MOVE_NETWORK_SIGNAL_FLOAT, Game.Player.Character, "VERTICLE_AIM", ChampagneMoveVertical);
							if (Game.IsControlPressed(Control.MoveRightOnly))
							{
								if (ChampagneMoveHorizontal <= 1f)
								{
									ChampagneMoveHorizontal += 0.005f;
								}
								else
								{
									ChampagneMoveHorizontal = 1f;
								}
							}
							if (Game.IsControlPressed(Control.MoveLeftOnly))
							{
								if (ChampagneMoveHorizontal >= 0f)
								{
									ChampagneMoveHorizontal -= 0.005f;
								}
								else
								{
									ChampagneMoveHorizontal = 0f;
								}
							}
							if (Game.IsControlPressed(Control.MoveUpOnly))
							{
								if (ChampagneMoveVertical >= 0f)
								{
									ChampagneMoveVertical -= 0.005f;
								}
								else
								{
									ChampagneMoveVertical = 0f;
								}
							}
							if (Game.IsControlPressed(Control.MoveDownOnly))
							{
								if (ChampagneMoveVertical <= 1f)
								{
									ChampagneMoveVertical += 0.005f;
								}
								else
								{
									ChampagneMoveVertical = 1f;
								}
							}
						}
						if (ChampagneStage == 4)
						{
							Function.Call(Hash.DISPLAY_RADAR, true);
							if (Function.Call<bool>(Hash.HAS_ANIM_EVENT_FIRED, Game.Player.Character, Game.GenerateHash("BottleDetach")))
							{
								Function.Call(Hash.DETACH_ENTITY, BarDrinkProp, true);
								Function.Call(Hash.FREEZE_ENTITY_POSITION, BarDrinkProp, true);
							}
							if (Function.Call<bool>(Hash.GET_TASK_MOVE_NETWORK_EVENT, Game.Player.Character, "OutroFinished") && Function.Call<bool>(Hash.IS_TASK_MOVE_NETWORK_READY_FOR_TRANSITION, Game.Player.Character))
							{
								Function.Call(Hash.CLEAR_PED_TASKS, Game.Player.Character);
								if (BarGirl != null)
								{
									AddDrunkness(0.55f);
									if (BarDrinkProp != null)
									{
										BarDrinkProp.Delete();
									}
									if (BarDrinkProp_2 != null)
									{
										BarDrinkProp_2.Delete();
									}
									ChampagneStage = -1;
									DrinkStage = 2;
									ChampagneLoop = false;
									Notification.Show("Stop Champain Anim");
									DrinkOrigin = BarGirl.GetOffsetPosition(new Vector3(0.2f, 0.85f, 1.35f));
									DrinkHeading = BarGirl.Heading - 180f;
									if (Bottle != null)
									{
										Bottle.Delete();
									}
									Game.Player.Character.Weapons.Select(WeaponHash.Unarmed);
									Bottle = GlobalHelper.CreateProp(RequestModel("h4_prop_battle_champ_closed_02"), BarGirl.Position, false, false);
									string dict = "anim@amb@nightclub@mini@drinking@champagne_drinking@base@";
									LoadDict(dict);
									Scene1 = Function.Call<int>(Hash.CREATE_SYNCHRONIZED_SCENE, DrinkOrigin.X, DrinkOrigin.Y, DrinkOrigin.Z - 1.1f, 0.0, 0.0, DrinkHeading, 2);
									Function.Call(Hash.TASK_SYNCHRONIZED_SCENE, BarGirl, Scene1, LoadDict(dict), "bartender_outro", 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, BarGirl, Scene1, "bartender_outro", LoadDict(dict), 16f, -16f, 0, 1148846080);
									Function.Call(Hash.PLAY_SYNCHRONIZED_ENTITY_ANIM, Bottle, Scene1, "bartender_outro_bottle", LoadDict(dict), 16f, -16f, 0, 1148846080);
								}
								else
								{
									Bar = true;
								}
							}
						}
					}
					if (DrinkStage == 0)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							if (CurrentDrinktype == Drinktype.ChampaineDiamond || CurrentDrinktype == Drinktype.ChampaineGold || CurrentDrinktype == Drinktype.ChampaineSilver)
							{
								Notification.Show("Start Champaine Anim");
								DrinkStage = 4;
								StartChampaineAnim();
							}
							else
							{
								GetDrink(CurrentDrinktype);
								DrinkStage = 1;
							}
						}
					}
					else if (DrinkStage == 1)
					{
						if (Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216)
						{
							DrinkStage = 2;
						}
					}
					else if (DrinkStage == 2 && Function.Call<int>(Hash.GET_SYNCHRONIZED_SCENE_PHASE, Scene1) >= 1065353216 && DrinkStage != 4)
					{
						Notification.Show("Stop Drink Anim");
						if (BarDrinkProp != null)
						{
							BarDrinkProp.Delete();
						}
						if (BarDrinkProp_2 != null)
						{
							BarDrinkProp_2.Delete();
						}
						DrinkStage = -1;
						Game.Player.Character.Task.ClearAll();
						PlayingSyncScene = false;
						if (SaltShaker != null)
						{
							SaltShaker.Delete();
						}
						if (Plate != null)
						{
							Plate.Delete();
						}
						if (Lime != null)
						{
							Lime.Delete();
						}
						if (ShotGlass != null)
						{
							ShotGlass.Delete();
						}
						if (Bottle != null)
						{
							Bottle.Delete();
						}
						if (BarGirl != null)
						{
							BarGirl.Task.PlayAnimation("mini@strip_club@drink@idle_a", "idle_a_bartender", 1f, -1, AnimationFlags.Loop);
						}
					}
				}
				if (Game.GameTime >= GetNewBartnender)
				{
					GetNewBartnender = Game.GameTime + 5000;
					float radius = 999f;
					foreach (Utils.ObjectPlacementMap entity in Bartenders.Entities)
					{
						if (entity.Entity != null && Class1.GetDistance_VDIST2(Game.Player.Character.Position, entity.Entity.Position, Class1.GetDistSign.Less, radius))
						{
							radius = GetDistance(Game.Player.Character.Position, entity.Entity.Position);
							BarGirl = (Ped)entity.Entity;
						}
					}
				}
				if (ObjectPool != null)
				{
					ObjectPool.Process();
				}
				if (PlayingSyncScene || !(BarGirl != null))
				{
					return;
				}
				Vector3 offsetPosition = BarGirl.GetOffsetPosition(new Vector3(0f, 1.75f, 0f));
				if (!Class1.GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, Class1.GetDistSign.Less, 30f) || DrinkStage != -1 || !Class1.GetDistance_VDIST2(Game.Player.Character.Position, offsetPosition, Class1.GetDistSign.Less, 2f))
				{
					return;
				}
				DisplayHelpTextThisFrame("Press ~INPUT_CONTEXT~ to Order A Drink", 1);
				if (Game.IsControlJustPressed(Control.Context))
				{
					int taskID = 0;
					Utils.OpenSequenceTask(&taskID);
					Utils.TaskPedSlideToCoord(0, offsetPosition, -90f, 8f);
					Utils.CloseSequenceTask(taskID);
					Utils.TaskPerformSequence(Game.Player.Character, taskID);
					Utils.ClearSequenceTask(&taskID);
					if (MainMenu != null && !MainMenu.Visible)
					{
						MainMenu.Visible = true;
					}
				}
			}
		}
	}
}
