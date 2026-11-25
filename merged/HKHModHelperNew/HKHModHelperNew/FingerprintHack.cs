using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;

namespace HKHModHelperNew
{
	public class FingerprintHack : Script
	{
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

		public class PrintTileID
		{
			public int PrintID { get; set; }

			public int TileID { get; set; }

			public PrintTileID()
			{
			}

			public PrintTileID(int PID, int TID)
			{
				PrintID = PID;
				TileID = TID;
			}
		}

		public class TextureID
		{
			public string Path { get; set; }

			public string Name { get; set; }

			public int ID { get; set; }

			public void NewRandomID()
			{
				while (IDs.Contains(ID))
				{
					ID = Memory.CreateTexture(Path);
				}
				if (!IDs.Contains(ID))
				{
					IDs.Add(ID);
				}
			}

			public TextureID()
			{
			}

			public TextureID(int id)
			{
				ID = id;
				IDs.Add(ID);
			}

			public TextureID(string N, int id, string P)
			{
				Name = N;
				ID = id;
				IDs.Add(ID);
				Path = P;
			}

			public TextureID(char N)
			{
				if (N.Equals('1'))
				{
					Name = "1";
					Path = Environment.CurrentDirectory + FilePath + "textures//_1.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('2'))
				{
					Name = "2";
					Path = Environment.CurrentDirectory + FilePath + "textures//_2.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('3'))
				{
					Name = "3";
					Path = Environment.CurrentDirectory + FilePath + "textures//_3.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('4'))
				{
					Name = "4";
					Path = Environment.CurrentDirectory + FilePath + "textures//_4.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('5'))
				{
					Name = "5";
					Path = Environment.CurrentDirectory + FilePath + "textures//_5.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('6'))
				{
					Name = "6";
					Path = Environment.CurrentDirectory + FilePath + "textures//_6.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('7'))
				{
					Name = "7";
					Path = Environment.CurrentDirectory + FilePath + "textures//_7.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('8'))
				{
					Name = "8";
					Path = Environment.CurrentDirectory + FilePath + "textures//_8.png/";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('9'))
				{
					Name = "9";
					Path = Environment.CurrentDirectory + FilePath + "textures//_9.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('0'))
				{
					Name = "0";
					Path = Environment.CurrentDirectory + FilePath + "textures// _0.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('$'))
				{
					Name = "$";
					Path = Environment.CurrentDirectory + FilePath + "textures//_Dollar.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals(','))
				{
					Name = ",";
					Path = Environment.CurrentDirectory + FilePath + "textures//_Comma.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
				if (N.Equals('.'))
				{
					Name = ".";
					Path = Environment.CurrentDirectory + FilePath + "textures//_Dot.png";
					ID = Memory.CreateTexture(Path);
					IDs.Add(ID);
				}
			}
		}

		private struct RGB
		{
			public int R;

			public int G;

			public int B;

			public int A;

			public RGB(int r, int g, int b, int a = 255)
			{
				R = r;
				G = g;
				B = b;
				A = a;
			}
		}

		public enum BruteForceBackground
		{
			Black,
			Purple,
			Gray,
			LightBlue,
			Wallpaper1,
			DarkFade
		}

		public class VariableTimer
		{
			public delegate void TimerExpired(object sender);

			private int TimerMax;

			private decimal TimerCounter;

			private bool IsRunning;

			public bool AutoReset;

			public int Counter => (int)TimerCounter;

			public event TimerExpired OnTimerExpired;

			public VariableTimer(int interval)
			{
				TimerCounter = interval;
				TimerMax = interval;
			}

			public void AddTime(decimal amount)
			{
				TimerCounter += amount;
			}

			public void RemoveTime(decimal amount)
			{
				TimerCounter -= amount;
				if (TimerCounter < 0m)
				{
					TimerCounter = default(decimal);
				}
			}

			public void Update(float timescale)
			{
				if (!IsRunning)
				{
					return;
				}
				float num = Game.LastFrameTime * 1000f;
				TimerCounter -= (decimal)(num * timescale);
				if (TimerCounter <= 0m)
				{
					this.OnTimerExpired?.Invoke(this);
					if (AutoReset)
					{
						TimerCounter += (decimal)TimerMax;
					}
					else
					{
						Stop();
					}
				}
			}

			public void Stop()
			{
				IsRunning = false;
			}

			public void Start()
			{
				IsRunning = true;
			}

			public void Reset()
			{
				TimerCounter = TimerMax;
			}
		}

		public class Vector4
		{
			public float x1 { get; set; }

			public float y1 { get; set; }

			public float x2 { get; set; }

			public float y2 { get; set; }

			public Vector4(float X1, float Y1, float X2, float Y2)
			{
				x1 = X1;
				x2 = X2;
				y1 = Y1;
				y2 = Y2;
			}
		}

		public class Vector6
		{
			public float StartX { get; set; }

			public float StartY { get; set; }

			public float EndX { get; set; }

			public float EndY { get; set; }

			public float Type { get; set; }

			public float Unk { get; set; }

			public Vector6(float SX, float SY, float EX, float EY, float T, float U)
			{
				StartX = SX;
				StartY = SY;
				EndX = EX;
				EndY = EY;
				Type = T;
				Unk = U;
			}
		}

		public class ButtonPressMinigameSet
		{
			public List<Control> ControlSet { get; set; }
		}

		public class Boundary
		{
			public Vector2 Start { get; set; }

			public Vector2 End { get; set; }

			public Boundary()
			{
			}

			public Boundary(Vector2 S, Vector2 E)
			{
				Start = S;
				End = E;
			}
		}

		public class Node
		{
			public int Frame { get; set; }

			public int FrameTime { get; set; }

			public bool isDestroyed { get; set; }

			public List<Vector2> Collisions { get; set; } = new List<Vector2>();


			public NodeType NodeType { get; set; }

			public Vector2 Origin { get; set; }

			public float Angle { get; set; }

			public bool AngleChanged { get; set; }

			public Node()
			{
			}

			public Node(Vector2 Orig, float Ang, NodeType NodeT)
			{
				isDestroyed = false;
				Origin = Orig;
				Angle = Ang;
				NodeType = NodeT;
				Vector2 item = new Vector2(Origin.X - 0.045f, Origin.Y - 0.04f);
				Vector2 item2 = new Vector2(Origin.X + 0.045f, Origin.Y - 0.04f);
				Vector2 item3 = new Vector2(Origin.X + 0.045f, Origin.Y + 0.04f);
				Vector2 item4 = new Vector2(Origin.X - 0.045f, Origin.Y + 0.04f);
				if (NodeType == NodeType.Packet || NodeType == NodeType.Firewall)
				{
					if (Collisions.Count == 0)
					{
						Collisions.Add(item);
						Collisions.Add(item2);
						Collisions.Add(item3);
						Collisions.Add(item4);
						Collisions.Add(item);
					}
				}
				else if (NodeType == NodeType.Prism)
				{
					if (Collisions.Count == 0)
					{
						Collisions.Add(item);
						Collisions.Add(item2);
						Collisions.Add(item3);
						Collisions.Add(item4);
						Collisions.Add(item);
					}
				}
				else if (NodeType == NodeType.Rotating_Mirror)
				{
					if (Collisions.Count == 0)
					{
						Collisions.Add(item);
						Collisions.Add(item4);
						return;
					}
					Collisions[0] = GetVector2Offset(new Vector3(Origin.X, Origin.Y, 0f), Angle, new Vector3(-0.035f, 0f, 0f));
					Collisions[1] = GetVector2Offset(new Vector3(Origin.X, Origin.Y, 0f), Angle, new Vector3(0.035f, 0f, 0f));
					item = Collisions[0];
					item4 = Collisions[1];
				}
				else if (NodeType == NodeType.Static_Mirror)
				{
					if (Collisions.Count == 0)
					{
						Collisions.Add(item);
						Collisions.Add(item4);
						return;
					}
					Collisions[0] = GetVector2Offset(new Vector3(Origin.X, Origin.Y, 0f), Angle - 135f, new Vector3(0.025f, -0.025f, 0f));
					Collisions[1] = GetVector2Offset(new Vector3(Origin.X, Origin.Y, 0f), Angle - 135f, new Vector3(-0.025f, 0.025f, 0f));
					item = Collisions[0];
					item4 = Collisions[1];
				}
			}

			public Node(Vector2 Orig, List<Vector2> Coll, float Ang, NodeType NodeT)
			{
				isDestroyed = false;
				Origin = Orig;
				Angle = Ang;
				NodeType = NodeT;
				Collisions = Coll;
			}

			public Node(List<Vector2> Coll, float Ang, NodeType NodeT)
			{
				Collisions = Coll;
				isDestroyed = false;
				Angle = Ang;
				NodeType = NodeT;
			}
		}

		public enum NodeType
		{
			None,
			Rotating_Mirror,
			Static_Mirror,
			Packet,
			Firewall,
			Prism,
			Proxy
		}

		public class Beam
		{
			public Vector2 Origin { get; set; }

			public bool InteractingWithMirror { get; set; }

			public float Angle { get; set; }

			public Node ConnectedMirror { get; set; }

			public Beam()
			{
			}

			public Beam(Vector2 Orig, bool Mirror, float Ang)
			{
				Origin = Orig;
				InteractingWithMirror = Mirror;
				Angle = Ang;
			}

			public Beam(Vector2 Orig, Node CMirror, bool Mirror, float Ang)
			{
				Origin = Orig;
				InteractingWithMirror = Mirror;
				Angle = Ang;
				ConnectedMirror = CMirror;
			}
		}

		public static class Constants
		{
			public const int BH_MAX_BEAM_SEGMENTS = 30;

			public const int BH_MAX_OUTER_WALLS = 4;

			public const int BH_MAX_NODES = 20;

			public const int BH_MAX_PACKETS = 20;

			public const int BH_MAX_INNER_WALL_RECTS = 30;

			public const int BH_MAX_FIREWALLS = 10;

			public const int BH_MAX_ROTATING_NODES = 10;

			public const int BH_MAX_PROXIES = 5;

			public const int BH_NODE_POINTS = 2;

			public const int BH_PACKET_POINTS = 4;

			public const int BH_INNER_WALL_POINTS = 4;

			public const int BH_FIREWALL_POINTS = 4;

			public const int BH_PROXY_POINTS = 4;
		}

		public class sbeamLevel
		{
			public Vector2[] OuterWalls = new Vector2[4];

			public Vector2[][] Nodes = new Vector2[20][];

			public Vector2[][] Packets = new Vector2[20][];

			public Vector2[][] Firewalls = new Vector2[10][];

			public Vector2[][] InnerWalls = new Vector2[30][];

			public Vector2[][] Proxies = new Vector2[5][];

			public int MaxNodes;

			public int MaxRotatingNodes;

			public int MaxPackets;

			public int MaxFirewalls;

			public int MaxInnerWalls;

			public int MaxProxies;

			public sbeamLevel()
			{
				for (int i = 0; i < 4; i++)
				{
					OuterWalls[i] = Vector2.Zero;
				}
				for (int j = 0; j < 30; j++)
				{
					InnerWalls[j] = new Vector2[4];
					for (int k = 0; k < 4; k++)
					{
						InnerWalls[j][k] = Vector2.Zero;
					}
				}
				for (int l = 0; l < 20; l++)
				{
					Nodes[l] = new Vector2[2];
					for (int m = 0; m < 2; m++)
					{
						Nodes[l][m] = Vector2.Zero;
					}
				}
				for (int n = 0; n < 20; n++)
				{
					Packets[n] = new Vector2[4];
					for (int num = 0; num < 4; num++)
					{
						Packets[n][num] = Vector2.Zero;
					}
				}
				for (int num2 = 0; num2 < 10; num2++)
				{
					Firewalls[num2] = new Vector2[4];
					for (int num3 = 0; num3 < 4; num3++)
					{
						Firewalls[num2][num3] = Vector2.Zero;
					}
				}
				for (int num4 = 0; num4 < 5; num4++)
				{
					Proxies[num4] = new Vector2[4];
					for (int num5 = 0; num5 < 4; num5++)
					{
						Proxies[num4][num5] = Vector2.Zero;
					}
				}
				MaxNodes = 0;
				MaxRotatingNodes = 0;
				MaxPackets = 0;
				MaxFirewalls = 0;
				MaxInnerWalls = 0;
				MaxProxies = 0;
			}
		}

		public enum ShapeTypes
		{
			ST_MAX_SHAPES,
			ST_ROTATING_NODE,
			ST_NODE,
			ST_PACKET,
			ST_FIREWALL,
			ST_PROXY,
			ST_INNER_WALL
		}

		public static int CleanupHacksID;

		public static int CannotCallHackToggleTimeout = -1;

		public string AdminName = "CINNBARGNU2";

		public static int InstanceGunlocker = 0;

		public static int InstanceSaveCar = 0;

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
			new ButtonData(303, "INPUT_REPLAY_SCREENSHOT", "U", "DPAD UP"),
			new ButtonData(304, "INPUT_REPLAY_HIDEHUD", "H", "R3"),
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

		public static int HackKey = 51;

		public static bool FingerprintHackOn;

		public static bool KeypadHackOn;

		public static int HackStage;

		public float fLocal_564;

		public int Lives = 3;

		public bool DEBUGON;

		public int ChoosenNumber1;

		public int ChoosenNumber2;

		public int ChoosenNumber3;

		public int ChoosenNumber4;

		public List<PrintTileID> PrintId = new List<PrintTileID>();

		public int CurrentLoadingSegment;

		public List<int> RandomPrints = new List<int>();

		public List<int> SelectedPrints = new List<int>();

		public int CurrentSelectedTile;

		public int CurrentSelectedPrintID;

		public int SelectedTilePosX = 1;

		public int SelectedTilePosY = 1;

		public int SelectedTile1ID;

		public int SelectedTile2ID;

		public int SelectedTile3ID;

		public int SelectedTile4ID;

		public int SelectedTile5ID;

		public int SelectedTile6ID;

		public int SelectedTile1PrintID;

		public int SelectedTile2PrintID;

		public int SelectedTile3PrintID;

		public int SelectedTile4PrintID;

		public int SelectedTile5PrintID;

		public int SelectedTile6PrintID;

		public bool CreatedRandomPatterns;

		public int ChoosenRandomPattern;

		public int RandomPatternAt;

		public int CurrentPrint;

		public int StagesToComplete;

		public bool CheckingPrint;

		public int SelectionStage;

		public int DotHackFailed;

		public static int HackFinishedID;

		public static float screenaspectratio;

		private static readonly Vector2 L1 = new Vector2(0.104f, 0.2361f);

		private static readonly Vector2 L2 = new Vector2(0.104f, 0.4986f);

		private static readonly Vector2 L3 = new Vector2(0.104f, 0.7631f);

		private static readonly Vector2 R1 = new Vector2(0.8564f, 0.2361f);

		private static readonly Vector2 R2 = new Vector2(0.8564f, 0.4986f);

		private static readonly Vector2 R3 = new Vector2(0.8564f, 0.7631f);

		private static readonly Vector2 C1 = new Vector2(0.2889f, 0.2361f);

		private static readonly Vector2 C2 = new Vector2(0.2889f, 0.4993f);

		private static readonly Vector2 C3 = new Vector2(0.2889f, 0.7611f);

		private static readonly Vector2 M1 = new Vector2(0.4802f, 0.2361f);

		private static readonly Vector2 M2 = new Vector2(0.4802f, 0.4993f);

		private static readonly Vector2 M3 = new Vector2(0.4802f, 0.7611f);

		private static readonly Vector2 D1 = new Vector2(0.6758f, 0.4972f);

		private static readonly Vector2 D2 = new Vector2(0.6726f, 0.7625f);

		public static int DrawCallsPerFrame = 500;

		public static Vector2 DrawAspect = new Vector2(1f, 1f);

		public static float masterScaleRace = 1f;

		public static List<int> IDs = new List<int>();

		public static string FilePath = "\\scripts\\RealismSuite\\HKHModHelper\\";

		public static int GameVersiontoDrawWithMemory = 3407;

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Foreground_ID = -1;
		public static int PCBHack_Foreground_ID { get { if (_PCBHack_Foreground_ID == -1) _PCBHack_Foreground_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_background.png")).ID; return _PCBHack_Foreground_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_NoLight1_ID = -1;
		public static int PCBHack_NoLight1_ID { get { if (_PCBHack_NoLight1_ID == -1) _PCBHack_NoLight1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_offlight.png")).ID; return _PCBHack_NoLight1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_NoLight2_ID = -1;
		public static int PCBHack_NoLight2_ID { get { if (_PCBHack_NoLight2_ID == -1) _PCBHack_NoLight2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_offlight.png")).ID; return _PCBHack_NoLight2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_GreenLight1_ID = -1;
		public static int PCBHack_GreenLight1_ID { get { if (_PCBHack_GreenLight1_ID == -1) _PCBHack_GreenLight1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_greenlight.png")).ID; return _PCBHack_GreenLight1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_GreenLight2_ID = -1;
		public static int PCBHack_GreenLight2_ID { get { if (_PCBHack_GreenLight2_ID == -1) _PCBHack_GreenLight2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_greenlight.png")).ID; return _PCBHack_GreenLight2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Pass = -1;
		public static int PCBHack_Pass { get { if (_PCBHack_Pass == -1) _PCBHack_Pass = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pass.png")).ID; return _PCBHack_Pass; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Fail = -1;
		public static int PCBHack_Fail { get { if (_PCBHack_Fail == -1) _PCBHack_Fail = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//fail.png")).ID; return _PCBHack_Fail; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_RedLight_ID = -1;
		public static int PCBHack_RedLight_ID { get { if (_PCBHack_RedLight_ID == -1) _PCBHack_RedLight_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_redlight.png")).ID; return _PCBHack_RedLight_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Selection_ID = -1;
		public static int PCBHack_Selection_ID { get { if (_PCBHack_Selection_ID == -1) _PCBHack_Selection_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_selection.png")).ID; return _PCBHack_Selection_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_EndSolderBottom_ID = -1;
		public static int PCBHack_EndSolderBottom_ID { get { if (_PCBHack_EndSolderBottom_ID == -1) _PCBHack_EndSolderBottom_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_endsolder_bottom.png")).ID; return _PCBHack_EndSolderBottom_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_EndSolderMiddle_ID = -1;
		public static int PCBHack_EndSolderMiddle_ID { get { if (_PCBHack_EndSolderMiddle_ID == -1) _PCBHack_EndSolderMiddle_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_endsolder_middle.png")).ID; return _PCBHack_EndSolderMiddle_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_EndSolderTop_ID = -1;
		public static int PCBHack_EndSolderTop_ID { get { if (_PCBHack_EndSolderTop_ID == -1) _PCBHack_EndSolderTop_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_endsolder_top.png")).ID; return _PCBHack_EndSolderTop_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_StartSolderBottom_ID = -1;
		public static int PCBHack_StartSolderBottom_ID { get { if (_PCBHack_StartSolderBottom_ID == -1) _PCBHack_StartSolderBottom_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_startsolder_bottom.png")).ID; return _PCBHack_StartSolderBottom_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_StartSolderMiddle_ID = -1;
		public static int PCBHack_StartSolderMiddle_ID { get { if (_PCBHack_StartSolderMiddle_ID == -1) _PCBHack_StartSolderMiddle_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_startsolder_middle.png")).ID; return _PCBHack_StartSolderMiddle_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_StartSolderTop_ID = -1;
		public static int PCBHack_StartSolderTop_ID { get { if (_PCBHack_StartSolderTop_ID == -1) _PCBHack_StartSolderTop_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//pcb_hack_startsolder_top.png")).ID; return _PCBHack_StartSolderTop_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level0_Block1_ID = -1;
		public static int PCBHack_Level0_Block1_ID { get { if (_PCBHack_Level0_Block1_ID == -1) _PCBHack_Level0_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level0//pcb_hack_block1.png")).ID; return _PCBHack_Level0_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level0_Block2_ID = -1;
		public static int PCBHack_Level0_Block2_ID { get { if (_PCBHack_Level0_Block2_ID == -1) _PCBHack_Level0_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level0//pcb_hack_block2.png")).ID; return _PCBHack_Level0_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level0_Block3_ID = -1;
		public static int PCBHack_Level0_Block3_ID { get { if (_PCBHack_Level0_Block3_ID == -1) _PCBHack_Level0_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level0//pcb_hack_block3.png")).ID; return _PCBHack_Level0_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level0_Block4_ID = -1;
		public static int PCBHack_Level0_Block4_ID { get { if (_PCBHack_Level0_Block4_ID == -1) _PCBHack_Level0_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level0//pcb_hack_block4.png")).ID; return _PCBHack_Level0_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level0_Block5_ID = -1;
		public static int PCBHack_Level0_Block5_ID { get { if (_PCBHack_Level0_Block5_ID == -1) _PCBHack_Level0_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level0//pcb_hack_block5.png")).ID; return _PCBHack_Level0_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level0_Block6_ID = -1;
		public static int PCBHack_Level0_Block6_ID { get { if (_PCBHack_Level0_Block6_ID == -1) _PCBHack_Level0_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level0//pcb_hack_block6.png")).ID; return _PCBHack_Level0_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level0_Block7_ID = -1;
		public static int PCBHack_Level0_Block7_ID { get { if (_PCBHack_Level0_Block7_ID == -1) _PCBHack_Level0_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level0//pcb_hack_block7.png")).ID; return _PCBHack_Level0_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level0_Block8_ID = -1;
		public static int PCBHack_Level0_Block8_ID { get { if (_PCBHack_Level0_Block8_ID == -1) _PCBHack_Level0_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level0//pcb_hack_block8.png")).ID; return _PCBHack_Level0_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level1_Block1_ID = -1;
		public static int PCBHack_Level1_Block1_ID { get { if (_PCBHack_Level1_Block1_ID == -1) _PCBHack_Level1_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level1//pcb_hack1_block1.png")).ID; return _PCBHack_Level1_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level1_Block2_ID = -1;
		public static int PCBHack_Level1_Block2_ID { get { if (_PCBHack_Level1_Block2_ID == -1) _PCBHack_Level1_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level1//pcb_hack1_block2.png")).ID; return _PCBHack_Level1_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level1_Block3_ID = -1;
		public static int PCBHack_Level1_Block3_ID { get { if (_PCBHack_Level1_Block3_ID == -1) _PCBHack_Level1_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level1//pcb_hack1_block3.png")).ID; return _PCBHack_Level1_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level1_Block4_ID = -1;
		public static int PCBHack_Level1_Block4_ID { get { if (_PCBHack_Level1_Block4_ID == -1) _PCBHack_Level1_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level1//pcb_hack1_block4.png")).ID; return _PCBHack_Level1_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level1_Block5_ID = -1;
		public static int PCBHack_Level1_Block5_ID { get { if (_PCBHack_Level1_Block5_ID == -1) _PCBHack_Level1_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level1//pcb_hack1_block5.png")).ID; return _PCBHack_Level1_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level1_Block6_ID = -1;
		public static int PCBHack_Level1_Block6_ID { get { if (_PCBHack_Level1_Block6_ID == -1) _PCBHack_Level1_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level1//pcb_hack1_block6.png")).ID; return _PCBHack_Level1_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level1_Block7_ID = -1;
		public static int PCBHack_Level1_Block7_ID { get { if (_PCBHack_Level1_Block7_ID == -1) _PCBHack_Level1_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level1//pcb_hack1_block7.png")).ID; return _PCBHack_Level1_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level1_Block8_ID = -1;
		public static int PCBHack_Level1_Block8_ID { get { if (_PCBHack_Level1_Block8_ID == -1) _PCBHack_Level1_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level1//pcb_hack1_block8.png")).ID; return _PCBHack_Level1_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level2_Block1_ID = -1;
		public static int PCBHack_Level2_Block1_ID { get { if (_PCBHack_Level2_Block1_ID == -1) _PCBHack_Level2_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level2//pcb_hack2_block1.png")).ID; return _PCBHack_Level2_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level2_Block2_ID = -1;
		public static int PCBHack_Level2_Block2_ID { get { if (_PCBHack_Level2_Block2_ID == -1) _PCBHack_Level2_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level2//pcb_hack2_block2.png")).ID; return _PCBHack_Level2_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level2_Block3_ID = -1;
		public static int PCBHack_Level2_Block3_ID { get { if (_PCBHack_Level2_Block3_ID == -1) _PCBHack_Level2_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level2//pcb_hack2_block3.png")).ID; return _PCBHack_Level2_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level2_Block4_ID = -1;
		public static int PCBHack_Level2_Block4_ID { get { if (_PCBHack_Level2_Block4_ID == -1) _PCBHack_Level2_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level2//pcb_hack2_block4.png")).ID; return _PCBHack_Level2_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level2_Block5_ID = -1;
		public static int PCBHack_Level2_Block5_ID { get { if (_PCBHack_Level2_Block5_ID == -1) _PCBHack_Level2_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level2//pcb_hack2_block5.png")).ID; return _PCBHack_Level2_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level2_Block6_ID = -1;
		public static int PCBHack_Level2_Block6_ID { get { if (_PCBHack_Level2_Block6_ID == -1) _PCBHack_Level2_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level2//pcb_hack2_block6.png")).ID; return _PCBHack_Level2_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level2_Block7_ID = -1;
		public static int PCBHack_Level2_Block7_ID { get { if (_PCBHack_Level2_Block7_ID == -1) _PCBHack_Level2_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level2//pcb_hack2_block7.png")).ID; return _PCBHack_Level2_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level2_Block8_ID = -1;
		public static int PCBHack_Level2_Block8_ID { get { if (_PCBHack_Level2_Block8_ID == -1) _PCBHack_Level2_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level2//pcb_hack2_block8.png")).ID; return _PCBHack_Level2_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level3_Block1_ID = -1;
		public static int PCBHack_Level3_Block1_ID { get { if (_PCBHack_Level3_Block1_ID == -1) _PCBHack_Level3_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level3//pcb_hack3_block1.png")).ID; return _PCBHack_Level3_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level3_Block2_ID = -1;
		public static int PCBHack_Level3_Block2_ID { get { if (_PCBHack_Level3_Block2_ID == -1) _PCBHack_Level3_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level3//pcb_hack3_block2.png")).ID; return _PCBHack_Level3_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level3_Block3_ID = -1;
		public static int PCBHack_Level3_Block3_ID { get { if (_PCBHack_Level3_Block3_ID == -1) _PCBHack_Level3_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level3//pcb_hack3_block3.png")).ID; return _PCBHack_Level3_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level3_Block4_ID = -1;
		public static int PCBHack_Level3_Block4_ID { get { if (_PCBHack_Level3_Block4_ID == -1) _PCBHack_Level3_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level3//pcb_hack3_block4.png")).ID; return _PCBHack_Level3_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level3_Block5_ID = -1;
		public static int PCBHack_Level3_Block5_ID { get { if (_PCBHack_Level3_Block5_ID == -1) _PCBHack_Level3_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level3//pcb_hack3_block5.png")).ID; return _PCBHack_Level3_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level3_Block6_ID = -1;
		public static int PCBHack_Level3_Block6_ID { get { if (_PCBHack_Level3_Block6_ID == -1) _PCBHack_Level3_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level3//pcb_hack3_block6.png")).ID; return _PCBHack_Level3_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level3_Block7_ID = -1;
		public static int PCBHack_Level3_Block7_ID { get { if (_PCBHack_Level3_Block7_ID == -1) _PCBHack_Level3_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level3//pcb_hack3_block7.png")).ID; return _PCBHack_Level3_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level3_Block8_ID = -1;
		public static int PCBHack_Level3_Block8_ID { get { if (_PCBHack_Level3_Block8_ID == -1) _PCBHack_Level3_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level3//pcb_hack3_block8.png")).ID; return _PCBHack_Level3_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level4_Block1_ID = -1;
		public static int PCBHack_Level4_Block1_ID { get { if (_PCBHack_Level4_Block1_ID == -1) _PCBHack_Level4_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level4//pcb_hack4_block1.png")).ID; return _PCBHack_Level4_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level4_Block2_ID = -1;
		public static int PCBHack_Level4_Block2_ID { get { if (_PCBHack_Level4_Block2_ID == -1) _PCBHack_Level4_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level4//pcb_hack4_block2.png")).ID; return _PCBHack_Level4_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level4_Block3_ID = -1;
		public static int PCBHack_Level4_Block3_ID { get { if (_PCBHack_Level4_Block3_ID == -1) _PCBHack_Level4_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level4//pcb_hack4_block3.png")).ID; return _PCBHack_Level4_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level4_Block4_ID = -1;
		public static int PCBHack_Level4_Block4_ID { get { if (_PCBHack_Level4_Block4_ID == -1) _PCBHack_Level4_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level4//pcb_hack4_block4.png")).ID; return _PCBHack_Level4_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level4_Block5_ID = -1;
		public static int PCBHack_Level4_Block5_ID { get { if (_PCBHack_Level4_Block5_ID == -1) _PCBHack_Level4_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level4//pcb_hack4_block5.png")).ID; return _PCBHack_Level4_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level4_Block6_ID = -1;
		public static int PCBHack_Level4_Block6_ID { get { if (_PCBHack_Level4_Block6_ID == -1) _PCBHack_Level4_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level4//pcb_hack4_block6.png")).ID; return _PCBHack_Level4_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level4_Block7_ID = -1;
		public static int PCBHack_Level4_Block7_ID { get { if (_PCBHack_Level4_Block7_ID == -1) _PCBHack_Level4_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level4//pcb_hack4_block7.png")).ID; return _PCBHack_Level4_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level4_Block8_ID = -1;
		public static int PCBHack_Level4_Block8_ID { get { if (_PCBHack_Level4_Block8_ID == -1) _PCBHack_Level4_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level4//pcb_hack4_block8.png")).ID; return _PCBHack_Level4_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level5_Block1_ID = -1;
		public static int PCBHack_Level5_Block1_ID { get { if (_PCBHack_Level5_Block1_ID == -1) _PCBHack_Level5_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level5//pcb_hack5_block1.png")).ID; return _PCBHack_Level5_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level5_Block2_ID = -1;
		public static int PCBHack_Level5_Block2_ID { get { if (_PCBHack_Level5_Block2_ID == -1) _PCBHack_Level5_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level5//pcb_hack5_block2.png")).ID; return _PCBHack_Level5_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level5_Block3_ID = -1;
		public static int PCBHack_Level5_Block3_ID { get { if (_PCBHack_Level5_Block3_ID == -1) _PCBHack_Level5_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level5//pcb_hack5_block3.png")).ID; return _PCBHack_Level5_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level5_Block4_ID = -1;
		public static int PCBHack_Level5_Block4_ID { get { if (_PCBHack_Level5_Block4_ID == -1) _PCBHack_Level5_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level5//pcb_hack5_block4.png")).ID; return _PCBHack_Level5_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level5_Block5_ID = -1;
		public static int PCBHack_Level5_Block5_ID { get { if (_PCBHack_Level5_Block5_ID == -1) _PCBHack_Level5_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level5//pcb_hack5_block5.png")).ID; return _PCBHack_Level5_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level5_Block6_ID = -1;
		public static int PCBHack_Level5_Block6_ID { get { if (_PCBHack_Level5_Block6_ID == -1) _PCBHack_Level5_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level5//pcb_hack5_block6.png")).ID; return _PCBHack_Level5_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level5_Block7_ID = -1;
		public static int PCBHack_Level5_Block7_ID { get { if (_PCBHack_Level5_Block7_ID == -1) _PCBHack_Level5_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level5//pcb_hack5_block7.png")).ID; return _PCBHack_Level5_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level5_Block8_ID = -1;
		public static int PCBHack_Level5_Block8_ID { get { if (_PCBHack_Level5_Block8_ID == -1) _PCBHack_Level5_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level5//pcb_hack5_block8.png")).ID; return _PCBHack_Level5_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level6_Block1_ID = -1;
		public static int PCBHack_Level6_Block1_ID { get { if (_PCBHack_Level6_Block1_ID == -1) _PCBHack_Level6_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level6//pcb_hack6_block1.png")).ID; return _PCBHack_Level6_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level6_Block2_ID = -1;
		public static int PCBHack_Level6_Block2_ID { get { if (_PCBHack_Level6_Block2_ID == -1) _PCBHack_Level6_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level6//pcb_hack6_block2.png")).ID; return _PCBHack_Level6_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level6_Block3_ID = -1;
		public static int PCBHack_Level6_Block3_ID { get { if (_PCBHack_Level6_Block3_ID == -1) _PCBHack_Level6_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level6//pcb_hack6_block3.png")).ID; return _PCBHack_Level6_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level6_Block4_ID = -1;
		public static int PCBHack_Level6_Block4_ID { get { if (_PCBHack_Level6_Block4_ID == -1) _PCBHack_Level6_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level6//pcb_hack6_block4.png")).ID; return _PCBHack_Level6_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level6_Block5_ID = -1;
		public static int PCBHack_Level6_Block5_ID { get { if (_PCBHack_Level6_Block5_ID == -1) _PCBHack_Level6_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level6//pcb_hack6_block5.png")).ID; return _PCBHack_Level6_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level6_Block6_ID = -1;
		public static int PCBHack_Level6_Block6_ID { get { if (_PCBHack_Level6_Block6_ID == -1) _PCBHack_Level6_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level6//pcb_hack6_block6.png")).ID; return _PCBHack_Level6_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level6_Block7_ID = -1;
		public static int PCBHack_Level6_Block7_ID { get { if (_PCBHack_Level6_Block7_ID == -1) _PCBHack_Level6_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level6//pcb_hack6_block7.png")).ID; return _PCBHack_Level6_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level6_Block8_ID = -1;
		public static int PCBHack_Level6_Block8_ID { get { if (_PCBHack_Level6_Block8_ID == -1) _PCBHack_Level6_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level6//pcb_hack6_block8.png")).ID; return _PCBHack_Level6_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level7_Block1_ID = -1;
		public static int PCBHack_Level7_Block1_ID { get { if (_PCBHack_Level7_Block1_ID == -1) _PCBHack_Level7_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level7//pcb_hack7_block1.png")).ID; return _PCBHack_Level7_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level7_Block2_ID = -1;
		public static int PCBHack_Level7_Block2_ID { get { if (_PCBHack_Level7_Block2_ID == -1) _PCBHack_Level7_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level7//pcb_hack7_block2.png")).ID; return _PCBHack_Level7_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level7_Block3_ID = -1;
		public static int PCBHack_Level7_Block3_ID { get { if (_PCBHack_Level7_Block3_ID == -1) _PCBHack_Level7_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level7//pcb_hack7_block3.png")).ID; return _PCBHack_Level7_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level7_Block4_ID = -1;
		public static int PCBHack_Level7_Block4_ID { get { if (_PCBHack_Level7_Block4_ID == -1) _PCBHack_Level7_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level7//pcb_hack7_block4.png")).ID; return _PCBHack_Level7_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level7_Block5_ID = -1;
		public static int PCBHack_Level7_Block5_ID { get { if (_PCBHack_Level7_Block5_ID == -1) _PCBHack_Level7_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level7//pcb_hack7_block5.png")).ID; return _PCBHack_Level7_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level7_Block6_ID = -1;
		public static int PCBHack_Level7_Block6_ID { get { if (_PCBHack_Level7_Block6_ID == -1) _PCBHack_Level7_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level7//pcb_hack7_block6.png")).ID; return _PCBHack_Level7_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level7_Block7_ID = -1;
		public static int PCBHack_Level7_Block7_ID { get { if (_PCBHack_Level7_Block7_ID == -1) _PCBHack_Level7_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level7//pcb_hack7_block7.png")).ID; return _PCBHack_Level7_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level7_Block8_ID = -1;
		public static int PCBHack_Level7_Block8_ID { get { if (_PCBHack_Level7_Block8_ID == -1) _PCBHack_Level7_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level7//pcb_hack7_block8.png")).ID; return _PCBHack_Level7_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level8_Block1_ID = -1;
		public static int PCBHack_Level8_Block1_ID { get { if (_PCBHack_Level8_Block1_ID == -1) _PCBHack_Level8_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level8//pcb_hack8_block1.png")).ID; return _PCBHack_Level8_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level8_Block2_ID = -1;
		public static int PCBHack_Level8_Block2_ID { get { if (_PCBHack_Level8_Block2_ID == -1) _PCBHack_Level8_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level8//pcb_hack8_block2.png")).ID; return _PCBHack_Level8_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level8_Block3_ID = -1;
		public static int PCBHack_Level8_Block3_ID { get { if (_PCBHack_Level8_Block3_ID == -1) _PCBHack_Level8_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level8//pcb_hack8_block3.png")).ID; return _PCBHack_Level8_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level8_Block4_ID = -1;
		public static int PCBHack_Level8_Block4_ID { get { if (_PCBHack_Level8_Block4_ID == -1) _PCBHack_Level8_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level8//pcb_hack8_block4.png")).ID; return _PCBHack_Level8_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level8_Block5_ID = -1;
		public static int PCBHack_Level8_Block5_ID { get { if (_PCBHack_Level8_Block5_ID == -1) _PCBHack_Level8_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level8//pcb_hack8_block5.png")).ID; return _PCBHack_Level8_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level8_Block6_ID = -1;
		public static int PCBHack_Level8_Block6_ID { get { if (_PCBHack_Level8_Block6_ID == -1) _PCBHack_Level8_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level8//pcb_hack8_block6.png")).ID; return _PCBHack_Level8_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level8_Block7_ID = -1;
		public static int PCBHack_Level8_Block7_ID { get { if (_PCBHack_Level8_Block7_ID == -1) _PCBHack_Level8_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level8//pcb_hack8_block7.png")).ID; return _PCBHack_Level8_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level8_Block8_ID = -1;
		public static int PCBHack_Level8_Block8_ID { get { if (_PCBHack_Level8_Block8_ID == -1) _PCBHack_Level8_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level8//pcb_hack8_block8.png")).ID; return _PCBHack_Level8_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level9_Block1_ID = -1;
		public static int PCBHack_Level9_Block1_ID { get { if (_PCBHack_Level9_Block1_ID == -1) _PCBHack_Level9_Block1_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level9//pcb_hack9_block1.png")).ID; return _PCBHack_Level9_Block1_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level9_Block2_ID = -1;
		public static int PCBHack_Level9_Block2_ID { get { if (_PCBHack_Level9_Block2_ID == -1) _PCBHack_Level9_Block2_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level9//pcb_hack9_block2.png")).ID; return _PCBHack_Level9_Block2_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level9_Block3_ID = -1;
		public static int PCBHack_Level9_Block3_ID { get { if (_PCBHack_Level9_Block3_ID == -1) _PCBHack_Level9_Block3_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level9//pcb_hack9_block3.png")).ID; return _PCBHack_Level9_Block3_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level9_Block4_ID = -1;
		public static int PCBHack_Level9_Block4_ID { get { if (_PCBHack_Level9_Block4_ID == -1) _PCBHack_Level9_Block4_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level9//pcb_hack9_block4.png")).ID; return _PCBHack_Level9_Block4_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level9_Block5_ID = -1;
		public static int PCBHack_Level9_Block5_ID { get { if (_PCBHack_Level9_Block5_ID == -1) _PCBHack_Level9_Block5_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level9//pcb_hack9_block5.png")).ID; return _PCBHack_Level9_Block5_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level9_Block6_ID = -1;
		public static int PCBHack_Level9_Block6_ID { get { if (_PCBHack_Level9_Block6_ID == -1) _PCBHack_Level9_Block6_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level9//pcb_hack9_block6.png")).ID; return _PCBHack_Level9_Block6_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level9_Block7_ID = -1;
		public static int PCBHack_Level9_Block7_ID { get { if (_PCBHack_Level9_Block7_ID == -1) _PCBHack_Level9_Block7_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level9//pcb_hack9_block7.png")).ID; return _PCBHack_Level9_Block7_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PCBHack_Level9_Block8_ID = -1;
		public static int PCBHack_Level9_Block8_ID { get { if (_PCBHack_Level9_Block8_ID == -1) _PCBHack_Level9_Block8_ID = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//Level9//pcb_hack9_block8.png")).ID; return _PCBHack_Level9_Block8_ID; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Background = -1;
		public static int PasscodeHack_Background { get { if (_PasscodeHack_Background == -1) _PasscodeHack_Background = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_background.png")).ID; return _PasscodeHack_Background; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Popup = -1;
		public static int PasscodeHack_Popup { get { if (_PasscodeHack_Popup == -1) _PasscodeHack_Popup = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_popup.png")).ID; return _PasscodeHack_Popup; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Popup_Correct = -1;
		public static int PasscodeHack_Popup_Correct { get { if (_PasscodeHack_Popup_Correct == -1) _PasscodeHack_Popup_Correct = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_popup_complete.png")).ID; return _PasscodeHack_Popup_Correct; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Popup_InCorrect = -1;
		public static int PasscodeHack_Popup_InCorrect { get { if (_PasscodeHack_Popup_InCorrect == -1) _PasscodeHack_Popup_InCorrect = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_popup_incorrect.png")).ID; return _PasscodeHack_Popup_InCorrect; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Popup_OutofTime = -1;
		public static int PasscodeHack_Popup_OutofTime { get { if (_PasscodeHack_Popup_OutofTime == -1) _PasscodeHack_Popup_OutofTime = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_popup_locked.png")).ID; return _PasscodeHack_Popup_OutofTime; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Letter_Bottom;
		public static List<int> PasscodeHack_Letter_Bottom { get { if (_PasscodeHack_Letter_Bottom == null) _PasscodeHack_Letter_Bottom = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID
		}; return _PasscodeHack_Letter_Bottom; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Letter_Bottom_Background;
		public static List<int> PasscodeHack_Letter_Bottom_Background { get { if (_PasscodeHack_Letter_Bottom_Background == null) _PasscodeHack_Letter_Bottom_Background = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker.png")).ID
		}; return _PasscodeHack_Letter_Bottom_Background; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Letter_Top;
		public static List<int> PasscodeHack_Letter_Top { get { if (_PasscodeHack_Letter_Top == null) _PasscodeHack_Letter_Top = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_marker_top.png")).ID
		}; return _PasscodeHack_Letter_Top; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Letter_Top_Correct;
		public static List<int> PasscodeHack_Letter_Top_Correct { get { if (_PasscodeHack_Letter_Top_Correct == null) _PasscodeHack_Letter_Top_Correct = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_correct.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_correct.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_correct.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_correct.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_correct.png")).ID
		}; return _PasscodeHack_Letter_Top_Correct; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Letter_Top_InCorrect;
		public static List<int> PasscodeHack_Letter_Top_InCorrect { get { if (_PasscodeHack_Letter_Top_InCorrect == null) _PasscodeHack_Letter_Top_InCorrect = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_incorrect.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_incorrect.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_incorrect.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_incorrect.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_selection_incorrect.png")).ID
		}; return _PasscodeHack_Letter_Top_InCorrect; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Lines_White;
		public static List<int> PasscodeHack_Lines_White { get { if (_PasscodeHack_Lines_White == null) _PasscodeHack_Lines_White = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID
		}; return _PasscodeHack_Lines_White; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Lines_Green;
		public static List<int> PasscodeHack_Lines_Green { get { if (_PasscodeHack_Lines_Green == null) _PasscodeHack_Lines_Green = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID
		}; return _PasscodeHack_Lines_Green; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Lines_Red;
		public static List<int> PasscodeHack_Lines_Red { get { if (_PasscodeHack_Lines_Red == null) _PasscodeHack_Lines_Red = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_node_line.png")).ID
		}; return _PasscodeHack_Lines_Red; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Nodes_Green;
		public static List<int> PasscodeHack_Nodes_Green { get { if (_PasscodeHack_Nodes_Green == null) _PasscodeHack_Nodes_Green = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_green_node.png")).ID
		}; return _PasscodeHack_Nodes_Green; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Nodes_Normal;
		public static List<int> PasscodeHack_Nodes_Normal { get { if (_PasscodeHack_Nodes_Normal == null) _PasscodeHack_Nodes_Normal = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_empty_node.png")).ID
		}; return _PasscodeHack_Nodes_Normal; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Nodes_Red;
		public static List<int> PasscodeHack_Nodes_Red { get { if (_PasscodeHack_Nodes_Red == null) _PasscodeHack_Nodes_Red = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_red_node.png")).ID
		}; return _PasscodeHack_Nodes_Red; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Line_1 = -1;
		public static int PasscodeHack_Line_1 { get { if (_PasscodeHack_Line_1 == -1) _PasscodeHack_Line_1 = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_line_1.png")).ID; return _PasscodeHack_Line_1; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Line_2 = -1;
		public static int PasscodeHack_Line_2 { get { if (_PasscodeHack_Line_2 == -1) _PasscodeHack_Line_2 = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_line_2.png")).ID; return _PasscodeHack_Line_2; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Line_3 = -1;
		public static int PasscodeHack_Line_3 { get { if (_PasscodeHack_Line_3 == -1) _PasscodeHack_Line_3 = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_line_3.png")).ID; return _PasscodeHack_Line_3; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Line_4 = -1;
		public static int PasscodeHack_Line_4 { get { if (_PasscodeHack_Line_4 == -1) _PasscodeHack_Line_4 = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_line_4.png")).ID; return _PasscodeHack_Line_4; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static int _PasscodeHack_Line_5 = -1;
		public static int PasscodeHack_Line_5 { get { if (_PasscodeHack_Line_5 == -1) _PasscodeHack_Line_5 = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_line_5.png")).ID; return _PasscodeHack_Line_5; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Life_Background;
		public static List<int> PasscodeHack_Life_Background { get { if (_PasscodeHack_Life_Background == null) _PasscodeHack_Life_Background = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life_background.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life_background.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life_background.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life_background.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life_background.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life_background.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life_background.png")).ID
		}; return _PasscodeHack_Life_Background; } }

		// RealismSuite: created the first time it is used, not when the scripts load (textures are never freed)
		static List<int> _PasscodeHack_Life;
		public static List<int> PasscodeHack_Life { get { if (_PasscodeHack_Life == null) _PasscodeHack_Life = new List<int>
		{
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life.png")).ID,
			new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//wm_life.png")).ID
		}; return _PasscodeHack_Life; } }

		public float PHTimeLeft;

		public int Selections;

		public int DiscTimer;

		public int PrintFlickerTimer;

		public int HighLightPrint;

		public int PatternTimer;

		public float CheckDelay;

		public int BarOn;

		public int LoadingTimer;

		public int ResizeMode;

		public float SizeX;

		public float SizeY;

		public List<Vector2> LoadingBarPoints = new List<Vector2>
		{
			new Vector2(0.3755814f, 83f / 160f),
			new Vector2(0.3883721f, 83f / 160f),
			new Vector2(0.4020349f, 83f / 160f),
			new Vector2(0.4145349f, 0.5180556f),
			new Vector2(0.4264535f, 0.5173611f),
			new Vector2(0.4386628f, 83f / 160f),
			new Vector2(0.4514535f, 83f / 160f),
			new Vector2(0.4636628f, 0.5173611f),
			new Vector2(0.4761628f, 0.5173611f),
			new Vector2(0.490407f, 0.5173611f),
			new Vector2(0.5026163f, 0.5166667f),
			new Vector2(0.5159883f, 0.5166667f),
			new Vector2(0.5287791f, 0.5173611f),
			new Vector2(0.5421512f, 0.5173611f),
			new Vector2(0.5552326f, 0.5173611f),
			new Vector2(0.5677326f, 0.5166667f),
			new Vector2(0.5790698f, 0.5173611f),
			new Vector2(0.5915698f, 0.5180556f),
			new Vector2(0.6034884f, 0.5180556f),
			new Vector2(0.6156977f, 0.5180556f),
			new Vector2(0.6273256f, 0.5180556f)
		};

		public bool BlipTile1;

		public bool BlipTile2;

		public bool BlipTile3;

		public bool BlipTile4;

		public bool BlipTile5;

		public bool BlipTile6;

		public bool BlipRow2;

		public bool BlipRow3;

		public bool BlipRow4;

		public bool BlipRow5;

		public bool BlipRow6;

		public int CurrentDot;

		public int BlipTimer1;

		public int BlipTimer2;

		public List<int> ChoosenNumbers = new List<int>();

		public int LastFlickerInterval = 108;

		public static bool VideoPlaying;

		public int SoundId;

		public static int HackGUI;

		public int HackTimer;

		public int Number1;

		public bool GotNo1;

		public int Number2;

		public bool GotNo2;

		public int Number3;

		public bool GotNo3;

		public static bool RunningPadLockMinigame;

		public float Rotation;

		public float ActualRotation;

		public float LeftMP = 1.6f;

		public float RightMP = 1.6f;

		public bool PDLHackComplete;

		public bool AllowUnlockonNonStop;

		public static float cfWireOffset = 0.076f;

		public bool SuccessHotwire;

		public static bool HotWireMinigameOn;

		public int SelectedLeftNode_1;

		public int SelectedRightNode_1;

		public int SelectedLeftNode_2;

		public int SelectedRightNode_2;

		public int SelectedLeftNode_3;

		public int SelectedRightNode_3;

		public int SelectedLeftNode_4;

		public int SelectedRightNode_4;

		public bool LeftSelected;

		public int LeftNode;

		public static float fSeparation = 0.071f;

		public static List<Vector2> LeftNode1 = new List<Vector2>
		{
			new Vector2(0.1f - fSeparation, 0.32f),
			new Vector2(0.1f - fSeparation, 0.245f),
			new Vector2(0.2f - fSeparation, 0.245f),
			new Vector2(0.2f - fSeparation, 0.32f)
		};

		public static List<Vector2> LeftNode2 = new List<Vector2>
		{
			new Vector2(0.1f - fSeparation, 0.47f),
			new Vector2(0.1f - fSeparation, 0.395f),
			new Vector2(0.2f - fSeparation, 0.395f),
			new Vector2(0.2f - fSeparation, 0.47f)
		};

		public static List<Vector2> LeftNode3 = new List<Vector2>
		{
			new Vector2(0.1f - fSeparation, 0.62f),
			new Vector2(0.1f - fSeparation, 0.545f),
			new Vector2(0.2f - fSeparation, 0.545f),
			new Vector2(0.2f - fSeparation, 0.62f)
		};

		public static List<Vector2> LeftNode4 = new List<Vector2>
		{
			new Vector2(0.1f - fSeparation, 0.77f),
			new Vector2(0.1f - fSeparation, 0.695f),
			new Vector2(0.2f - fSeparation, 0.695f),
			new Vector2(0.2f - fSeparation, 0.77f)
		};

		public bool RightSelected;

		public int RightNode;

		public int _1ID;

		public int _2ID;

		public int _3ID;

		public int _4ID;

		public int Correct_1ID;

		public int Correct_2ID;

		public int Correct_3ID;

		public int Correct_4ID;

		public int RightNode_1Lives;

		public int RightNode_2Lives;

		public int RightNode_3Lives;

		public int RightNode_4Lives;

		public int TokensSelected;

		public int CheckHotwireTime;

		public float HotwireloadingDelay;

		public float fBary = 0.5f;

		public float fBarMultiplier = 22.7f;

		public float fDownloadAmount;

		public int sHotwireSoundSet;

		public float fSuccessWindowAlpha;

		public float HotwireloadingDelayTimer;

		public static int HotwirePassState;

		public int LeftNode_1Locked;

		public int LeftNode_2Locked;

		public int LeftNode_3Locked;

		public int LeftNode_4Locked;

		public static int DelayInputTime;

		public float HotwireTimeLeft;

		public float LastNodeX;

		public float LastNodeY;

		public static float cfWireHeight = 0.004f;

		public bool SelectingNode;

		public List<float> Node1Warnings = new List<float> { 255f, 255f, 255f, 255f };

		public List<float> Node2Warnings = new List<float> { 255f, 255f, 255f, 255f };

		public List<float> Node3Warnings = new List<float> { 255f, 255f, 255f, 255f };

		public List<float> Node4Warnings = new List<float> { 255f, 255f, 255f, 255f };

		public Scaleform scaleform;

		public Scaleform scaleform2;

		public float RoundedNumber;

		public float NumberAtCheck;

		public static float VoltageMG_Scale = 0.8f;

		private static readonly Vector2 NodeCoord_L1 = new Vector2(0.104f, 0.2361f);

		private static readonly Vector2 NodeCoord_L2 = new Vector2(0.104f, 0.4986f);

		private static readonly Vector2 NodeCoord_L3 = new Vector2(0.104f, 0.7631f);

		private static readonly Vector2 NodeCoord_R1 = new Vector2(0.8564f, 0.2361f);

		private static readonly Vector2 NodeCoord_R2 = new Vector2(0.8564f, 0.4986f);

		private static readonly Vector2 NodeCoord_R3 = new Vector2(0.8564f, 0.7631f);

		private static readonly Vector2 R3_ALT = new Vector2(0.8566f, 0.7731f);

		private static readonly Vector2 P_2889_2381 = new Vector2(0.2889f, 0.2381f);

		private static readonly Vector2 P_2875_5000 = new Vector2(0.2875f, 0.5f);

		private static readonly Vector2 P_4790_4993 = new Vector2(0.479f, 0.4993f);

		private static readonly Vector2 P_4802_7611 = new Vector2(0.4802f, 0.7611f);

		private static readonly Vector2 P_4139_5000 = new Vector2(0.4139f, 0.5f);

		private static readonly Vector2 P_4156_3688 = new Vector2(0.4156f, 59f / 160f);

		private static readonly Vector2 P_6046_3680 = new Vector2(0.6046f, 0.368f);

		private static readonly Vector2 P_6075_2340 = new Vector2(0.6075f, 0.23402f);

		private static readonly Vector2 P_3526_4986 = new Vector2(0.3526f, 0.4986f);

		private static readonly Vector2 P_3529_7597 = new Vector2(0.3529f, 0.7597f);

		private static readonly Vector2 P_5418_7604 = new Vector2(0.5418f, 0.7604f);

		private static readonly Vector2 P_5427_6313 = new Vector2(0.5427f, 101f / 160f);

		private static readonly Vector2 P_7328_6313 = new Vector2(0.7328f, 101f / 160f);

		private static readonly Vector2 P_7319_2381 = new Vector2(0.7319f, 0.2381f);

		private static readonly Vector2 P_5409_6965 = new Vector2(0.5409f, 0.6965f);

		private static readonly Vector2 P_7331_6944 = new Vector2(0.7331f, 0.6944f);

		private static readonly Vector2 P_7319_5000 = new Vector2(0.7319f, 0.5f);

		private static readonly RGB YELLOW = new RGB(252, 140, 3);

		private static readonly RGB RED = new RGB(145, 0, 0);

		private static readonly RGB BLUE = new RGB(0, 0, 255);

		public static bool PlayingVoltageMG;

		public static int VolthackTargetNumber = 0;

		public static int ShownTargetNumber;

		public static int ScrableNumberTimer;

		public static bool DisableControlls;

		public static int ScrableCount;

		public static int ScrableNumberTime = 90000;

		public static int RNo1;

		public static int RNo2;

		public static int RNo3;

		public static int LNo1;

		public static int LNo2;

		public static int LNo3;

		public static int VTL1node;

		public static int VTR1node;

		public static int VTL2node;

		public static int VTR2node;

		public static int VTL3node;

		public static int VTR3node;

		public int VTL1nodeSelected;

		public int VTR1nodeSelected;

		public int VTL2nodeSelected;

		public int VTR2nodeSelected;

		public int VTL3nodeSelected;

		public int VTR3nodeSelected;

		public int RightNodeOn;

		public int LeftNodeOn;

		public int LastLeftNodeSelected;

		public bool SelectedLN1;

		public bool SelectedRN1;

		public bool SelectedLN2;

		public bool SelectedRN2;

		public bool SelectedLN3;

		public bool SelectedRN3;

		public int NodeOn;

		public int Delay;

		public bool AwaitingDelay;

		public int WrongDelay;

		public static bool CopyPrintHackOn;

		public int Print;

		public string PrintDict;

		public int PrintNum;

		public List<string> Prints = new List<string>
		{
			"mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_01", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_02", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_03", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_04", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_05", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_06", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_07", "mpfclone_retro_printfull1,fingerprint_hacking_minigame_fingerprints_01_slice_08", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_01", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_02",
			"mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_03", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_04", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_05", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_06", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_07", "mpfclone_retro_printfull2,fingerprint_hacking_minigame_fingerprints_02_slice_08", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_01", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_02", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_03", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_04",
			"mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_05", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_06", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_07", "mpfclone_retro_printfull3,fingerprint_hacking_minigame_fingerprints_03_slice_08", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_01", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_02", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_03", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_04", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_05", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_06",
			"mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_07", "mpfclone_retro_printfull4,fingerprint_hacking_minigame_fingerprints_04_slice_08", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_01", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_02", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_03", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_04", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_05", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_06", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_07", "mpfclone_retro_printfull5,fingerprint_hacking_minigame_fingerprints_05_slice_08",
			"mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_01", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_02", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_03", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_04", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_05", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_06", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_07", "mpfclone_retro_printfull6,fingerprint_hacking_minigame_fingerprints_06_slice_08", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_01", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_02",
			"mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_03", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_04", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_05", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_06", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_07", "mpfclone_retro_printfull7,fingerprint_hacking_minigame_fingerprints_07_slice_08", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_01", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_02", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_03", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_04",
			"mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_05", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_06", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_07", "mpfclone_retro_printfull8,fingerprint_hacking_minigame_fingerprints_08_slice_08"
		};

		public List<string> Section1Print = new List<string>();

		public List<string> Section2Print = new List<string>();

		public List<string> Section3Print = new List<string>();

		public List<string> Section4Print = new List<string>();

		public List<string> Section5Print = new List<string>();

		public List<string> Section6Print = new List<string>();

		public List<string> Section7Print = new List<string>();

		public List<string> Section8Print = new List<string>();

		public int Selection1;

		public int Selection2;

		public int Selection3;

		public int Selection4;

		public int Selection5;

		public int Selection6;

		public int Selection7;

		public int Selection8;

		public int SectionSelected = 1;

		public static int HacksCompleted;

		public static int HacksToComplete;

		public static int OverrideHacksToComplete;

		public bool Checking;

		public int CDPoint;

		public bool PHHackSuccess;

		public static int Hacking_Index = -1;

		public static bool ActiveHack;

		public static int hackingScalwform = 0;

		public static int uLocal_70 = 0;

		public static int Hacking_SelectedIndex = -1;

		public static float Hacking_Lives = 3f;

		public static int FeedBackNumber = -1;

		public static bool DrawHackScaleform = false;

		public static int ErrorTimer = -1;

		public static bool TRYDL = false;

		public static bool IpTry = false;

		public static int IpTryTimer = -1;

		public static int IpTryFailedTimer = -1;

		public static bool PlayBoth = true;

		public static int IPSound = 0;

		public static int MoveSound = 0;

		public static int MoveTimer = -1;

		public static int MoveSoundTimer = -1;

		public static bool BruteforceStartInstantly;

		public static int BruteForceBackgroundID = 4;

		public static int Anim_Timer = -1;

		private VariableTimer MyHackingTimer;

		public static bool HackingSuccess;

		public static bool HackDone;

		public static List<dynamic> HackingProps = new List<object>
		{
			"hei_prop_hei_securitypanel", "hei_prop_hei_keypad_01", "hei_prop_hei_keypad_02", "hei_prop_hei_keypad_03", "prop_ld_keypad_01", "prop_ld_keypad_01b", "prop_ld_keypad_01b_lod", "hei_prop_hei_cs_keyboard", "prop_cs_keyboard_01", "prop_keyboard_01a",
			"prop_keyboard_01b", "prop_laptop_jimmy", "prop_laptop_lester", "prop_laptop_lester2", "p_amb_lap_top_02", "p_cs_laptop_02", "p_laptop_02_s", "prop_laptop_01a"
		};

		public static int BruteforceType;

		public static int GUIMT;

		public static float GUIPosY;

		public static float GUIPosX;

		public static bool IPHackDone;

		public static bool BruteForcehackDone;

		public static int HackToComplete;

		public bool Active;

		public static int FIBHackPercentAmt;

		public int iLocal_8077;

		public static int LastFeedBackNumber = -1;

		public static Scaleform GUIscaleform;

		public static bool firstbounce;

		public static bool justrecieved = true;

		public static int scaleformloading;

		public static bool loadingin = true;

		public static bool up = false;

		public static bool up1 = false;

		public static bool up2 = false;

		public static bool up3 = false;

		public static bool up4 = true;

		public static bool up5 = true;

		public static bool up6 = true;

		public static bool up7 = true;

		public static bool active = false;

		public static int indexx = -1;

		public static int currentbar = 0;

		public static float colOneY = 0.4f;

		public static float colTwoY = 0.4f;

		public static float colThreeY = 0.4f;

		public static float colFourY = 0.4f;

		public static float colFiveY = 0.74f;

		public static float colSixY = 0.74f;

		public static float colSevenY = 0.74f;

		public static float colEightY = 0.74f;

		public static int GameTimeReference1 = -1;

		public static float fLocal_240 = 0.53f;

		public static float fLocal_239 = 0.62f;

		public static bool collumn1 = false;

		public static bool collumn2 = false;

		public static bool collumn3 = false;

		public static bool collumn4 = false;

		public static bool collumn5 = false;

		public static bool collumn6 = false;

		public static bool collumn7 = false;

		public static bool collumn8 = false;

		public static int sound = 2;

		public static bool skip1 = false;

		public static bool skip2 = false;

		public static bool skip3 = false;

		public static int BINKMovie = -1;

		public static int VoltageHack = 0;

		public static List<int> MultiplicationsChoosen = new List<int>();

		public static int CopyPrintHack = 0;

		public static int PadLockHack = 0;

		public static int HotwireHack = 0;

		public static bool BruteForceHackOn = false;

		public static int BruteforceHack = 0;

		public static bool FIBHackOn = false;

		public static int FIBHack = 0;

		public static bool DataCrackOn = false;

		public static int DataHack = 0;

		private string uParam0f_741 = "DLC_H3_Cas_Finger_Minigame_Sounds";

		private string uParam1f_741 = "DLC_H4_Voltage_Minigame_Sounds";

		public static bool GUIOn;

		public static bool SecuroServPhoneHackOn;

		public static int SecuroServPhoneHack;

		public static int UPLOADVIRUS = 0;

		public static int VirusStage;

		public static float SecuroServ_UploadProgress;

		public int uploadtimer;

		public int uploadviruskeypad;

		public int hacksounds;

		public float uploadcomplete;

		public static bool SecuroServ_UploadProgress_ViaHelper = true;

		public static bool AppTrackifyOn;

		public static int AppTrackify;

		public static List<Vector3> TrackifyTargets_Vector3 = new List<Vector3>();

		public static List<Entity> TrackifyTargets_Enitity = new List<Entity>();

		public static int TrackifySequence;

		private int TrackifySeq;

		private int NextTrackifyCall;

		public static Vector2 DrawPhoneUiPos = new Vector2(0.10017f, 0.18136f);

		public static Scaleform AppTrackifyHandle;

		public static bool AppTrackifyWasRunning;

		public static int TrackifyOpenTime;

		public static int mobilePhoneRenderId;

		public static bool CircuitHackOn;

		public static int CircuitHack;

		public string Direction;

		private float fVar10;

		private float fVar11;

		private float fVar12;

		private float fVar13;

		private float fVar14;

		private float fVar15;

		private float fVar16;

		private float fVar17;

		private float fVar18;

		private float fVar19;

		private float fVar20;

		private float fVar21;

		private float fVar22;

		private float fVar23;

		private float fVar24;

		private float fVar25;

		private float fVar26;

		private float fVar27;

		private float fVar28;

		private float fVar29;

		public int LevelChosen;

		public float SnakeSpeed = 0.003f;

		public List<Vector4> CollisionZonesLevel1 = new List<Vector4>
		{
			new Vector4(0.0029f, 0.15f, 0.2f, 0.58f),
			new Vector4(0.2568f, 0.55f, 0.3421f, 0.6388f),
			new Vector4(0.4242f, 0.439f, 0.607f, 0.852f),
			new Vector4(0.2198602f, 0.1558074f, 0.313046f, 0.4752125f),
			new Vector4(0.3392545f, 0.1543909f, 0.3881771f, 0.378187f),
			new Vector4(0.4120559f, 0.1508498f, 0.4627257f, 0.3767705f),
			new Vector4(0.4866045f, 0.1543909f, 0.5361095f, 0.3796034f),
			new Vector4(0.5594059f, 0.1522663f, 0.6301689f, 0.3038244f),
			new Vector4(0.6511357f, 0.1543909f, 0.7227723f, 0.3066572f),
			new Vector4(0.7483984f, 0.1536827f, 0.8576005f, 0.2556657f),
			new Vector4(0.7987769f, 0.3130312f, 1f, 0.4497167f),
			new Vector4(0.6316249f, 0.3746459f, 1f, 0.4511331f),
			new Vector4(0.6357018f, 0.4766289f, 0.8156669f, 0.8477337f),
			new Vector4(0.8450786f, 0.4773371f, 1f, 0.8449008f),
			new Vector4(0.1924869f, 0.7004249f, 0.395166f, 0.8463173f)
		};

		public List<Vector4> CollisionZonesLevel2 = new List<Vector4>
		{
			new Vector4(-0.03f, 0.6444759f, 0.1089109f, 0.2712465f),
			new Vector4(0.1080373f, 0.1536827f, 0.3083867f, 0.26983f),
			new Vector4(0.3078043f, 0.1543909f, 0.3672102f, 0.3512748f),
			new Vector4(0.2070472f, 0.3590651f, 0.2568433f, 0.8463173f),
			new Vector4(0.262085f, 0.4164306f, 0.4068142f, 0.8477337f),
			new Vector4(0.4373908f, 0.2358357f, 0.5200932f, 0.8505666f),
			new Vector4(0.5655212f, 0.4603399f, 0.5978451f, 0.5488669f),
			new Vector4(0.5894001f, 0.1508498f, 0.7859639f, 0.5495751f),
			new Vector4(0.7912056f, 0.3087819f, 0.8488643f, 0.4546742f),
			new Vector4(0.8404194f, 0.1529745f, 0.8998253f, 0.3016997f),
			new Vector4(0.5818288f, 0.6352691f, 0.8046011f, 0.7875354f),
			new Vector4(0.8744904f, 0.5134561f, 0.9656377f, 0.6820113f),
			new Vector4(0.8756552f, 0.7485836f, 1.03f, 0.8463173f),
			new Vector4(0.9327315f, 0.3576487f, 1.03f, 0.4560907f)
		};

		public List<Vector4> CollisionZonesLevel3 = new List<Vector4>
		{
			new Vector4(-0.03f, 0.2995751f, 0.2320909f, 0.7181303f),
			new Vector4(0.2355853f, 0.4086402f, 0.498544f, 0.5736544f),
			new Vector4(0.290332f, 0.6728045f, 0.4024461f, 0.8463173f),
			new Vector4(0.501456f, 0.572238f, 0.6025044f, 0.7740793f),
			new Vector4(0.6016307f, 0.4723796f, 0.7312173f, 0.5623229f),
			new Vector4(0.5017473f, 0.2507082f, 0.6106581f, 0.4709632f),
			new Vector4(0.186954f, 0.1529745f, 0.4117647f, 0.2471671f),
			new Vector4(0.3054747f, 0.2471671f, 0.4472918f, 0.3406515f),
			new Vector4(0.6758882f, 0.6487252f, 1.03f, 0.8441926f),
			new Vector4(0.8069307f, 0.4291785f, 0.9991264f, 0.5630311f),
			new Vector4(0.6779266f, 0.1536827f, 1.03f, 0.3859773f)
		};

		public List<Vector4> CollisionZonesLevel4 = new List<Vector4>
		{
			new Vector4(-0.03f, 0.1529745f, 0.2251019f, 0.5899433f),
			new Vector4(0.1977286f, 0.4879603f, 0.5629004f, 0.6558074f),
			new Vector4(0.375364f, 0.6565156f, 0.5614444f, 0.7783286f),
			new Vector4(0.1514269f, 0.7287535f, 0.3150845f, 0.8470255f),
			new Vector4(0.6214327f, 0.7167138f, 1.03f, 0.8505666f),
			new Vector4(0.8663366f, 0.48017f, 1.03f, 0.8548158f),
			new Vector4(0.6191031f, 0.3711048f, 0.905067f, 0.4752125f),
			new Vector4(0.7216074f, 0.23017f, 0.7880023f, 0.3668555f),
			new Vector4(0.9088526f, 0.4305949f, 0.9405941f, 0.4553824f),
			new Vector4(0.7888759f, 0.371813f, 0.9059406f, 0.4305949f),
			new Vector4(0.9373908f, 0.3966006f, 1.03f, 0.4220963f),
			new Vector4(0.8476995f, 0.1543909f, 0.9321491f, 0.2620397f),
			new Vector4(0.4365172f, 0.1522663f, 0.6511357f, 0.2627479f),
			new Vector4(0.271986f, 0.220255f, 0.3759464f, 0.2946176f),
			new Vector4(0.2976121f, 0.3002833f, 0.3771113f, 0.4327195f),
			new Vector4(0.2009319f, 0.5934844f, 0.5629004f, 0.6536827f),
			new Vector4(0.5550379f, 0.555949f, 0.7929528f, 0.6529745f),
			new Vector4(0.2967385f, 0.3109065f, 0.6924869f, 0.4008498f),
			new Vector4(0.1150262f, 0.5134561f, 0.2338381f, 0.5347025f)
		};

		public List<Vector4> CollisionZonesLevel5 = new List<Vector4>
		{
			new Vector4(-0.03f, 0.5800283f, 0.05969715f, 0.6352691f),
			new Vector4(0.02504368f, 0.4383853f, 0.114735f, 0.5141643f),
			new Vector4(0.1179383f, 0.1529745f, 0.2335469f, 0.7025496f),
			new Vector4(0.2341293f, 0.5304533f, 0.7338381f, 0.6352691f),
			new Vector4(0.6616191f, 0.6352691f, 0.7347117f, 0.6813031f),
			new Vector4(0.4956319f, 0.483711f, 0.7323821f, 0.536119f),
			new Vector4(0.2449039f, 0.7067989f, 0.2979033f, 0.7832861f),
			new Vector4(0.116191f, 0.5354108f, 0.2964473f, 0.6933427f),
			new Vector4(0.3430402f, 0.6373938f, 0.4528247f, 0.6706799f),
			new Vector4(0.6627839f, 0.641643f, 0.7335469f, 0.6791785f),
			new Vector4(0.4927199f, 0.7053824f, 0.5757135f, 0.7386686f),
			new Vector4(0.3715783f, 0.7443343f, 0.737915f, 0.8449008f),
			new Vector4(0.07979033f, 0.7620397f, 0.195399f, 0.8470255f),
			new Vector4(0.8008154f, 0.4971671f, 0.8785673f, 0.8427762f),
			new Vector4(0.8663366f, 0.4943343f, 0.9085615f, 0.6430595f),
			new Vector4(0.8893419f, 0.4298867f, 0.9720443f, 0.5538244f),
			new Vector4(0.9799068f, 0.6147309f, 1.03f, 0.7082153f),
			new Vector4(0.6220151f, 0.3689802f, 0.9458358f, 0.4298867f),
			new Vector4(0.7460687f, 0.1983003f, 0.8211998f, 0.3640227f),
			new Vector4(0.5503786f, 0.2450425f, 0.6141526f, 0.3016997f),
			new Vector4(0.1167734f, 0.1565156f, 0.6616191f, 0.25f),
			new Vector4(0.3587653f, 0.2613314f, 0.4062318f, 0.3555241f),
			new Vector4(0.3188701f, 0.4150141f, 0.436226f, 0.4950425f),
			new Vector4(0.8934188f, 0.1572238f, 0.9271986f, 0.2188385f),
			new Vector4(0.9953407f, 0.2584986f, 1.03f, 0.325779f),
			new Vector4(0.4720443f, 0.3038244f, 0.5174723f, 0.4298867f),
			new Vector4(0.5171812f, 0.3661473f, 0.5963891f, 0.4298867f)
		};

		public List<Vector4> CollisionZonesLevel6 = new List<Vector4>
		{
			new Vector4(0.08182877f, 0.1536827f, 0.1176471f, 0.2216714f),
			new Vector4(0.07251019f, 0.2783286f, 0.1030868f, 0.3271955f),
			new Vector4(-0.03f, 0.332153f, 0.05096098f, 0.5127479f),
			new Vector4(0.0521258f, 0.3307365f, 0.1051252f, 0.4263456f),
			new Vector4(0f, 0.5736544f, 0.04571928f, 0.8491501f),
			new Vector4(0.04105999f, 0.5729462f, 0.05562027f, 0.6232294f),
			new Vector4(0.09668025f, 0.6501417f, 0.126092f, 0.7188385f),
			new Vector4(0.1840419f, 0.6055241f, 0.2271404f, 0.6968839f),
			new Vector4(0.0419336f, 0.7577904f, 0.1461852f, 0.8463173f),
			new Vector4(0.1135702f, 0.473796f, 0.1857892f, 0.5679886f),
			new Vector4(0.1878276f, 0.4730878f, 0.2242283f, 0.5368272f),
			new Vector4(0.1921957f, 0.1543909f, 0.2684916f, 0.5332861f),
			new Vector4(0.1916133f, 0.1536827f, 0.332848f, 0.2492918f),
			new Vector4(0.3034362f, 0.3378187f, 0.3380897f, 0.4100567f),
			new Vector4(0.2262667f, 0.4582153f, 0.3060571f, 0.7528328f),
			new Vector4(0.2288876f, 0.7592068f, 0.4994176f, 0.8066572f),
			new Vector4(0.3092603f, 0.5432011f, 0.3826441f, 0.5977337f),
			new Vector4(0.4432149f, 0.4752125f, 0.4784508f, 0.6168555f),
			new Vector4(0.37682f, 0.6536827f, 0.5538731f, 0.7124646f),
			new Vector4(0.5384392f, 0.5134561f, 0.6065813f, 0.6501417f),
			new Vector4(0.60629f, 0.5127479f, 0.7350029f, 0.5956091f),
			new Vector4(0.6534653f, 0.4263456f, 0.7350029f, 0.509915f),
			new Vector4(0.3826441f, 0.3434844f, 25f / 34f, 0.4249292f),
			new Vector4(0.7009318f, 0.2195467f, 0.7556785f, 0.3364023f),
			new Vector4(0.5171812f, 0.1635977f, 0.6391963f, 0.2875354f),
			new Vector4(0.8080955f, 0.1558074f, 0.8832266f, 0.5793201f),
			new Vector4(0.8875946f, 0.4582153f, 0.9612697f, 0.555949f),
			new Vector4(0.780431f, 0.5779037f, 0.8849738f, 0.6281869f),
			new Vector4(0.6412347f, 0.7478753f, 0.7382062f, 0.8470255f),
			new Vector4(0.72947f, 0.6933427f, 0.9006989f, 0.8484419f),
			new Vector4(0.896622f, 0.7464589f, 1.03f, 0.8470255f),
			new Vector4(0.9426325f, 0.6161473f, 1.03f, 0.6947592f),
			new Vector4(0.5594059f, 0.6565156f, 0.6409435f, 0.8477337f),
			new Vector4(0.3771113f, 0.6543909f, 0.5527082f, 0.7145892f),
			new Vector4(0.5384392f, 0.5162889f, 0.6059988f, 0.6501417f),
			new Vector4(0.6077461f, 0.5148725f, 0.7347117f, 0.5970255f),
			new Vector4(0.655795f, 0.3456091f, 0.7358766f, 0.509915f),
			new Vector4(0.426325f, 0.1515581f, 0.4606872f, 0.2266289f),
			new Vector4(0.1639487f, 0.2492918f, 0.1898661f, 0.3286119f),
			new Vector4(0.9347699f, 0.3186969f, 1.03f, 0.4015581f)
		};

		public List<Vector6> SnakeLinesPositions = new List<Vector6>();

		public List<Vector3> CurrentSnakeLines = new List<Vector3>();

		public Vector2 StartPoint;

		public Vector2 EndPoint;

		public int RecordRouteTimer;

		public int iLocal_166;

		public int descriptionTimer;

		public int NextCallFrameTimer;

		public bool ShowingCurrentHackComplete;

		public Vector2 Port2Pos;

		public ScriptSettings Config;

		public int PointReturnID = 1;

		public Vector4 PointReturn = new Vector4(0f, 0f, 0f, 0f);

		public int CreatedCollisions;

		public int TrailSound;

		public static string BruteforcePassword;

		public static int ExitKey = 51;

		public float Heading;

		public float TailWidth;

		public int Editfvar;

		private float CircuitHack_fVar1;

		private float CircuitHack_fVar2;

		private float CircuitHack_fVar3;

		private float CircuitHack_fVar5;

		private float CircuitHack_fVar4;

		private float CircuitHack_fVar6;

		private float CircuitHack_fVar7;

		private float CircuitHack_fVar8;

		private float CircuitHack_fVar9;

		public static float ButtonPress_HackProgress = 0f;

		public static int ButtonAt = 0;

		public static int ButtonsPressed;

		public static bool JustClickedButton;

		public static int ChangeColorTimer;

		public static int FlashesDone;

		public static int FlashesToComplete;

		public static int SetsComplete;

		public static List<ButtonPressMinigameSet> CurrentSet = new List<ButtonPressMinigameSet>();

		public static List<Control> ButtonPressMinigameAllowedKeys = new List<Control>
		{
			Control.MoveUpOnly,
			Control.MoveDownOnly,
			Control.MoveLeftOnly,
			Control.MoveRightOnly,
			Control.PhoneUp,
			Control.PhoneDown,
			Control.PhoneLeft,
			Control.PhoneRight,
			Control.VehicleDuck,
			Control.Context,
			Control.Cover,
			Control.ThrowGrenade,
			Control.VehicleHeadlight,
			Control.Reload,
			Control.MultiplayerInfo,
			Control.Jump,
			Control.LookBehind,
			Control.MeleeBlock,
			Control.HUDSpecial,
			Control.PhoneCameraFocusLock,
			Control.ScriptSelect,
			Control.InteractionMenu,
			Control.ReplayScreenshot,
			Control.ReplayTools,
			Control.ReplayShowhotkey,
			Control.Enter,
			Control.Arrest,
			Control.ParachuteDetach,
			Control.PhoneExtraOption
		};

		public static bool ButtonPressMinigameCompleted;

		public static bool PlayingButtonPressMinigame;

		private static int _mScriptHash;

		private static int appcontactsHash;

		private static int appemailHash;

		private static int appsettingsHash;

		private static int appCameraHash;

		private static int appInternetHash;

		private static int appTextsHash;

		private static int appCheckListHash;

		public static int WallPaper = 0;

		private float fLocal_207;

		public static bool PCBHackOn;

		public static int PCBHackTime;

		public static int PCBHHackStage = 0;

		public static string PCBHHackCurrentLevel;

		public static string PCBHHackCurrentLevelID;

		public static int PCBHackCheckTime;

		public static int PCBHackLevelId;

		public static int PCBHackBar1_Alignment;

		public static int PCBHackBar1_AlignmentCorrect;

		public static int PCBHackBar2_Alignment;

		public static int PCBHackBar2_AlignmentCorrect;

		public static int PCBHackBar3_Alignment;

		public static int PCBHackBar3_AlignmentCorrect;

		public static int PCBHackBar4_Alignment;

		public static int PCBHackBar4_AlignmentCorrect;

		public static int PCBHackBar5_Alignment;

		public static int PCBHackBar5_AlignmentCorrect;

		public static int PCBHackBar6_Alignment;

		public static int PCBHackBar6_AlignmentCorrect;

		public static int PCBHackBar7_Alignment;

		public static int PCBHackBar7_AlignmentCorrect;

		public static int PCBHackBar8_Alignment;

		public static int PCBHackBar8_AlignmentCorrect;

		public static int PCBHackBarSelection = 0;

		public static bool FiveLetterPasscodeHack;

		public static int FiveLetterPasscodeHackTime;

		public static string Letter1Input;

		public static string Letter1CorrectInput;

		public static string Letter2Input;

		public static string Letter2CorrectInput;

		public static string Letter3Input;

		public static string Letter3CorrectInput;

		public static string Letter4Input;

		public static string Letter4CorrectInput;

		public static string Letter5Input;

		public static string Letter5CorrectInput;

		public static int LetterAt;

		public static int ResetIn;

		public static int FiveLetterPasscodeStage = 0;

		public static int SelectionAlpha;

		public static int AlphaStage;

		public static int NextAlphaChange;

		public static List<string> PassCodes = new List<string>
		{
			"AGENT", "HEIST", "STAGE", "INPUT", "ALPHA", "CODEX", "GRIAN", "PAVEL", "SOLIS", "AVAIL",
			"SQUAD", "POKER", "JOKER", "QUEST", "ADDER", "EAGLE", "MUMBO", "FACES", "IBIZA", "SABRE",
			"VAGOS", "YACHT"
		};

		public static string Passcode;

		public static string JumbledPasscode;

		public static int LetterSelection;

		public static int AttemptsLeft = 5;

		public static int ResetHackIn = -1;

		public static int HackCompleteType;

		public static float TextAdd = 0.015f;

		public List<int> DrawnIDsThisFrame = new List<int>();

		public static float BeamRotation = 0f;

		public static float RotationAmt = 0.05f;

		public static float BeamLength = 0.25f;

		public static int BeamLengthInt = 1;

		public static List<Beam> vBeam = new List<Beam>
		{
			new Beam(new Vector2(0.5f, 0.5f), false, 0f)
		};

		public static float cfSCREEN_CENTER = 0.5f;

		public static int BeamFxFrame;

		public static float FloatTolerance = 0.001f;

		public static float SameWallHitTolerance = 0.0006f;

		public static float IntersectMaxDist = 0.0225f;

		public static bool BeamHackOn = false;

		public static float BeamLengthColl;

		public static bool HasCollided;

		public static float Intersectdist = 1f;

		public static float MirrorRotation = 135f;

		public static Node DudExample = new Node(new Vector2(0.45f, 0.4f), 0f, NodeType.None);

		public static Node RotatingMirrorExample1 = new Node(new Vector2(0.5f, 0.1f), 0f, NodeType.Rotating_Mirror);

		public static Node RotatingMirrorExample2 = new Node(new Vector2(0.960983f, 0.5f), 135f, NodeType.Rotating_Mirror);

		public static Node RotatingMirrorExample3 = new Node(new Vector2(0.5f, 0.925f), 0f, NodeType.Rotating_Mirror);

		public static Node RotatingMirrorExample4 = new Node(new Vector2(0.1f, 0.1f), 0f, NodeType.Rotating_Mirror);

		public static Node StaticMirrorExample1 = new Node(new Vector2(0.95f, 0.8f), 315f, NodeType.Static_Mirror);

		public static Node StaticMirrorExample2 = new Node(new Vector2(0.95f, 0.2f), 45f, NodeType.Static_Mirror);

		public static Node StaticMirrorExample3 = new Node(new Vector2(0.65f, 0.8f), -135f, NodeType.Static_Mirror);

		public static Node StaticMirrorExample4 = new Node(new Vector2(0.65f, 0.15f), 135f, NodeType.Static_Mirror);

		public static Node PacketExample = new Node(new Vector2(0.75f, 0.4f), 0f, NodeType.Packet);

		public static Node FirewallExample = new Node(new Vector2(0.25f, 0.4f), 0f, NodeType.Firewall);

		public static Node PrismExample = new Node(new Vector2(0.25f, 0.65f), 0f, NodeType.Prism);

		public static List<Node> LevelNodes = new List<Node>();

		public static int BeamhackLives = 5;

		public static List<List<Vector2>> LevelWalls = new List<List<Vector2>>
		{
			new List<Vector2>
			{
				new Vector2(-0.1f, 0.845f),
				new Vector2(-0.1f, 0.17f),
				new Vector2(1.1f, 0.17f),
				new Vector2(1.1f, 0.845f)
			},
			new List<Vector2>
			{
				new Vector2(0.3f, 0.3f),
				new Vector2(0.35f, 0.3f),
				new Vector2(0.35f, 0.35f),
				new Vector2(0.3f, 0.35f)
			},
			new List<Vector2>
			{
				new Vector2(-0.1f, 0.845f),
				new Vector2(-0.1f, 0.62f),
				new Vector2(0.05f, 0.62f),
				new Vector2(0.05f, 0.845f)
			}
		};

		public static float fCursorMovement;

		public static int PrismChangeTime;

		public static int ciBH_MAX_BEAM_SEGMENTS = 30;

		public static int ciBEAMHACKBS_HITTING_PROXY = 0;

		public static int ciBH_PROXY_CANCEL_LOOP_COUNT = 5;

		public static int ciBH_INNER_WALL_POINTS = 4;

		public static int ciBH_FIREWALL_POINTS = 4;

		public static int ciBH_PACKET_POINTS = 4;

		public static int ciBH_MAX_OUTER_WALLS = 8;

		public static float cfBH_NODEVIBRADIUS = 0.019f;

		public static float cfBH_PACKET_SIZE = 0.085f;

		public static int StartTime;

		public static int tdHackTime;

		public static float cfBH_FLOAT_TOLERANCE = 0.001f;

		public static int MaxNodes = 15;

		public static int MaxRotatingNodes = 9;

		public static int MaxPackets = 10;

		public static int MaxFirewalls = 7;

		public static int MaxInnerWalls = 9;

		public static int MaxProxies = 1;

		public static bool EnableBeamHackDebug = false;

		public static int CurrentPackets;

		public static Vector2 vCursor = new Vector2(0.5f, 0.5f);

		public static int ciBH_BEAM_FX_MAX_FRAME = 4;

		public static float BH_BG_WIDTH = 1.2f;

		public static float BH_BG_HEIGHT = 0.675f;

		public static float BH_BG_XPOS = 0.5f;

		public static float BH_BG_YPOS = 0.5075f;

		public static int BeamLevel = 0;

		public static sbeamLevel beamLevel = new sbeamLevel();

		public static int BeamHacklevelAt = 0;

		public static bool HasSelectedANode;

		public static bool PlayedBounceSound;

		public static int BeamHackBackgroundLoopSoundId;

		public static int BeamHackBlueChargeLoopSoundId;

		public static int BeamHackCursorLoopSoundId;

		public static int BeamHackRotateMirrorLoopSoundId;

		public static int BeamHackRedChargeLoopSoundId;

		public static int BeamhackChangeMessageTime;

		public static int BeamhackMsg;

		public static int BeamhackResetIn;

		private static Scaleform trackify;

		private static bool isTrackifyVisible = false;

		private static int targetId = 1;

		private static float sweepTime = 0f;

		public static bool IsRadarOff;

		public int CellphoneHandle
		{
			get
			{
				int num = 0;
				switch (Game.Player.Character.Model.Hash)
				{
				case 225514697:
					num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_ifruit");
					while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
					{
						Script.Yield();
					}
					return num;
				case -1692214353:
					num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_badger");
					while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
					{
						Script.Yield();
					}
					return num;
				case -1686040670:
					num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_facade");
					while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
					{
						Script.Yield();
					}
					return num;
				default:
					num = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "cellphone_ifruit");
					while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, num))
					{
						Script.Yield();
					}
					return num;
				}
			}
		}

		public void CleanupHacks()
		{
			CircuitBreaker.CleanupCircuitMinigame();
			HackFinishedID = 0;
			CircuitHackOn = false;
			CircuitHackOn = false;
			HackingSuccess = false;
			UnloadLoadehacker();
			BruteForceHackOn = false;
			FIBHackOn = false;
			if (Game.Player.Character.CurrentVehicle != null)
			{
				Game.Player.Character.CurrentVehicle.IsPositionFrozen = false;
			}
			HackingSuccess = false;
			colOneY = 0.4f;
			colTwoY = 0.4f;
			colThreeY = 0.4f;
			colFourY = 0.4f;
			colFiveY = 0.74f;
			colSixY = 0.74f;
			colSevenY = 0.74f;
			colEightY = 0.74f;
			collumn1 = false;
			collumn2 = false;
			collumn3 = false;
			collumn4 = false;
			collumn5 = false;
			collumn6 = false;
			collumn7 = false;
			collumn8 = false;
			skip1 = false;
			skip2 = false;
			skip3 = false;
			loadingin = true;
			up = false;
			up1 = false;
			up2 = false;
			up3 = false;
			up4 = true;
			up5 = true;
			up6 = true;
			up7 = true;
			currentbar = 0;
			indexx = 20;
			DataCrackOn = false;
			Game.Player.CanControlCharacter = true;
			VideoPlaying = false;
			BINKMovie = -1;
			PlayingVoltageMG = false;
			Game.Player.CanControlCharacter = true;
			VideoPlaying = false;
			Game.Player.Character.IsPositionFrozen = false;
			CopyPrintHackOn = false;
			Game.Player.CanControlCharacter = true;
			LeftSelected = false;
			RightSelected = false;
			RightNode = 0;
			HackStage = 0;
			HotWireMinigameOn = false;
			Game.Player.CanControlCharacter = true;
			Game.Player.CanControlCharacter = true;
			StopBeamhackSounds();
			BeamHackOn = false;
			PCBHHackStage = 0;
			PCBHackOn = false;
			FiveLetterPasscodeStage = 0;
			FiveLetterPasscodeHack = false;
			PCBHHackStage = 0;
			PCBHackOn = false;
			FiveLetterPasscodeStage = 0;
			FiveLetterPasscodeHack = false;
			HackGUI = -1;
			Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
			Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
			HackFinishedID = 0;
			Script.Wait(1);
			HackStage = 0;
			FingerprintHackOn = false;
			KeypadHackOn = false;
			HackGUI = -1;
			Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
			Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
			HackFinishedID = 0;
			Script.Wait(1);
			HackStage = 0;
			KeypadHackOn = false;
			FingerprintHackOn = false;
		}

		public FingerprintHack()
		{
			_mScriptHash = Game.GenerateHash("cellphone_flashhand");
			appcontactsHash = Game.GenerateHash("appcontacts");
			appemailHash = Game.GenerateHash("appemail");
			appsettingsHash = Game.GenerateHash("appSettings");
			appCameraHash = Game.GenerateHash("appCamera");
			appInternetHash = Game.GenerateHash("appInternet");
			appTextsHash = Game.GenerateHash("apptextmessage");
			appCheckListHash = Game.GenerateHash("appchecklist");
			base.Tick += onTick;
			base.Aborted += onShutdown;
		}

		private void DisplayHelpTextThisFrame(string text)
		{
			InputArgument[] arguments = new InputArgument[1] { "STRING" };
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, arguments);
			InputArgument[] arguments2 = new InputArgument[1] { text };
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, arguments2);
			InputArgument[] arguments3 = new InputArgument[4] { 0, 0, 1, -1 };
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, arguments3);
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

		public int ReturnButtonID(int ID)
		{
			return ID;
		}

		public void TEXT(string text, PointF Pos, float Size, Color Colour, GTA.UI.Font font)
		{
			ScaledText scaledText = new ScaledText(Pos, "");
			scaledText.Text = text;
			scaledText.Color = Colour;
			scaledText.Font = font;
			scaledText.Scale = Size;
			scaledText.Alignment = Alignment.Center;
			scaledText.Draw();
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void drawTextWithFont(string text, float x, float y, float scale, int r, int g, int b, GTA.UI.Font F)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.SET_TEXT_FONT, (int)F);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static float GetRatio(float num)
		{
			return 0.5f - (0.5f - num) / screenaspectratio;
		}

		public static void drawSpriteScaled(string dict, string tex, float x, float y, float w, float h, int r, int g, int b, int a, float rot = 0f, float scale = 0.8f)
		{
			ScaleAroundCenter(ref x, ref y, ref w, ref h, scale);
			drawSprite3(dict, tex, x, y, w, h, r, g, b, a, rot);
		}

		public static void ScaleAroundCenter(ref float x, ref float y, ref float w, ref float h, float scale, float cx = 0.5f, float cy = 0.5f)
		{
			x = cx + (x - cx) * scale;
			y = cy + (y - cy) * scale;
			w *= scale;
			h *= scale;
		}

		public static void drawVoltageHackSpriteScaled(string dict, string tex, float x, float y, float w, float h, int r, int g, int b, int a, float rot = 0f, float scale = 0.8f, float addx = 0f, float addy = 0f)
		{
			ScaleAroundCenter(ref x, ref y, ref w, ref h, VoltageMG_Scale);
			drawSprite3(dict, tex, x + addx, y + addy, w, h, r, g, b, a, rot);
		}

		public static void drawVoltageHackLineLonger(string dict, string tex, float x, float y, float w, float h, int r, int g, int b, int a, float rot, float extraPixels)
		{
			if (Math.Abs(NormalizeDeg(rot) - 90f) < 0.01f)
			{
				w += extraPixels;
			}
			else
			{
				h += extraPixels;
			}
			drawVoltageHackSpriteScaled(dict, tex, x, y, w, h, r, g, b, a, rot);
		}

		private static float NormalizeDeg(float deg)
		{
			deg %= 360f;
			if (deg < 0f)
			{
				deg += 360f;
			}
			return deg;
		}

		public static void drawSprite3(string textureDict, string textureName, float screenX, float screenY, float width, float height, float Heading, int r, int g, int b, int alpha, int unk)
		{
			float num = width * 1920f;
			float num2 = height * 1920f;
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num3 = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetAspect(screenX), screenY, num / 1920f * num3, num2 / 1920f, Heading, r, g, b, alpha, 0);
			}
		}

		public static void drawSprite3(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1920f * num, height / 1920f, 0, r, g, b, alpha, 0);
			}
		}

		public static void drawSprite3(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, float Rot = 0f)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1920f * num, height / 1920f, Rot, r, g, b, alpha, 0);
			}
		}

		public static void DrawRect(float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.DRAW_RECT, GetRatio(screenX), screenY, width / 1920f * num, height / 1920f, r, g, b, alpha, 0);
		}

		public static float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		private void drawSprite_Mem(int ID, string textureDict, string textureName, float screenX, float screenY, float width, float height, float Heading, int r, int g, int b, int alpha, int unk)
		{
			float num = width * 1920f;
			float num2 = height * 1920f;
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num3 = (float)(1.778 / (double)screenaspectratio);
			Memory.DrawTexture(ID, 1, 1, DrawCallsPerFrame, num / 1920f * num3, num2 / 1920f, 0.5f, 0.5f, GetAspect(screenX), screenY, 0f, masterScaleRace, (float)r / 255f, (float)g / 255f, (float)b / 255f, (float)alpha / 255f);
		}

		public static int ReturnGameBuild()
		{
			return FileVersionInfo.GetVersionInfo("GTA5.exe").FileBuildPart;
		}

		public static int ReturnMemoryID(string textureDict, string textureName)
		{
			int result = 0;
			textureName.Equals("");
			if (textureName.Equals("pcb_hack_background"))
			{
				result = PCBHack_Foreground_ID;
			}
			if (textureName.Equals("pcb_hack_offlight"))
			{
				result = PCBHack_NoLight1_ID;
			}
			if (textureName.Equals("pcb_hack_greenlight"))
			{
				result = PCBHack_GreenLight1_ID;
			}
			if (textureName.Equals("pcb_hack_redlight"))
			{
				result = PCBHack_RedLight_ID;
			}
			if (textureName.Equals("pcb_hack_selection"))
			{
				result = PCBHack_Selection_ID;
			}
			if (textureName.Equals("pcb_hack_endsolder_bottom"))
			{
				result = PCBHack_EndSolderBottom_ID;
			}
			if (textureName.Equals("pcb_hack_endsolder_middle"))
			{
				result = PCBHack_EndSolderMiddle_ID;
			}
			if (textureName.Equals("pcb_hack_endsolder_top"))
			{
				result = PCBHack_EndSolderTop_ID;
			}
			if (textureName.Equals("pcb_hack_startsolder_bottom"))
			{
				result = PCBHack_StartSolderBottom_ID;
			}
			if (textureName.Equals("pcb_hack_startsolder_middle"))
			{
				result = PCBHack_StartSolderMiddle_ID;
			}
			if (textureName.Equals("pcb_hack_startsolder_top"))
			{
				result = PCBHack_StartSolderTop_ID;
			}
			if (textureName.Equals("pcb_hack_block1"))
			{
				result = PCBHack_Level0_Block1_ID;
			}
			if (textureName.Equals("pcb_hack_block2"))
			{
				result = PCBHack_Level0_Block2_ID;
			}
			if (textureName.Equals("pcb_hack_block3"))
			{
				result = PCBHack_Level0_Block3_ID;
			}
			if (textureName.Equals("pcb_hack_block4"))
			{
				result = PCBHack_Level0_Block4_ID;
			}
			if (textureName.Equals("pcb_hack_block5"))
			{
				result = PCBHack_Level0_Block5_ID;
			}
			if (textureName.Equals("pcb_hack_block6"))
			{
				result = PCBHack_Level0_Block6_ID;
			}
			if (textureName.Equals("pcb_hack_block7"))
			{
				result = PCBHack_Level0_Block7_ID;
			}
			if (textureName.Equals("pcb_hack_block8"))
			{
				result = PCBHack_Level0_Block8_ID;
			}
			if (textureName.Equals("pcb_hack1_block1"))
			{
				result = PCBHack_Level1_Block1_ID;
			}
			if (textureName.Equals("pcb_hack1_block2"))
			{
				result = PCBHack_Level1_Block2_ID;
			}
			if (textureName.Equals("pcb_hack1_block3"))
			{
				result = PCBHack_Level1_Block3_ID;
			}
			if (textureName.Equals("pcb_hack1_block4"))
			{
				result = PCBHack_Level1_Block4_ID;
			}
			if (textureName.Equals("pcb_hack1_block5"))
			{
				result = PCBHack_Level1_Block5_ID;
			}
			if (textureName.Equals("pcb_hack1_block6"))
			{
				result = PCBHack_Level1_Block6_ID;
			}
			if (textureName.Equals("pcb_hack1_block7"))
			{
				result = PCBHack_Level1_Block7_ID;
			}
			if (textureName.Equals("pcb_hack1_block8"))
			{
				result = PCBHack_Level1_Block8_ID;
			}
			if (textureName.Equals("pcb_hack2_block1"))
			{
				result = PCBHack_Level2_Block1_ID;
			}
			if (textureName.Equals("pcb_hack2_block2"))
			{
				result = PCBHack_Level2_Block2_ID;
			}
			if (textureName.Equals("pcb_hack2_block3"))
			{
				result = PCBHack_Level2_Block3_ID;
			}
			if (textureName.Equals("pcb_hack2_block4"))
			{
				result = PCBHack_Level2_Block4_ID;
			}
			if (textureName.Equals("pcb_hack2_block5"))
			{
				result = PCBHack_Level2_Block5_ID;
			}
			if (textureName.Equals("pcb_hack2_block6"))
			{
				result = PCBHack_Level2_Block6_ID;
			}
			if (textureName.Equals("pcb_hack2_block7"))
			{
				result = PCBHack_Level2_Block7_ID;
			}
			if (textureName.Equals("pcb_hack2_block8"))
			{
				result = PCBHack_Level2_Block8_ID;
			}
			if (textureName.Equals("pcb_hack3_block1"))
			{
				result = PCBHack_Level3_Block1_ID;
			}
			if (textureName.Equals("pcb_hack3_block2"))
			{
				result = PCBHack_Level3_Block2_ID;
			}
			if (textureName.Equals("pcb_hack3_block3"))
			{
				result = PCBHack_Level3_Block3_ID;
			}
			if (textureName.Equals("pcb_hack3_block4"))
			{
				result = PCBHack_Level3_Block4_ID;
			}
			if (textureName.Equals("pcb_hack3_block5"))
			{
				result = PCBHack_Level3_Block5_ID;
			}
			if (textureName.Equals("pcb_hack3_block6"))
			{
				result = PCBHack_Level3_Block6_ID;
			}
			if (textureName.Equals("pcb_hack3_block7"))
			{
				result = PCBHack_Level3_Block7_ID;
			}
			if (textureName.Equals("pcb_hack3_block8"))
			{
				result = PCBHack_Level3_Block8_ID;
			}
			if (textureName.Equals("pcb_hack4_block1"))
			{
				result = PCBHack_Level4_Block1_ID;
			}
			if (textureName.Equals("pcb_hack4_block2"))
			{
				result = PCBHack_Level4_Block2_ID;
			}
			if (textureName.Equals("pcb_hack4_block3"))
			{
				result = PCBHack_Level4_Block3_ID;
			}
			if (textureName.Equals("pcb_hack4_block4"))
			{
				result = PCBHack_Level4_Block4_ID;
			}
			if (textureName.Equals("pcb_hack4_block5"))
			{
				result = PCBHack_Level4_Block5_ID;
			}
			if (textureName.Equals("pcb_hack4_block6"))
			{
				result = PCBHack_Level4_Block6_ID;
			}
			if (textureName.Equals("pcb_hack4_block7"))
			{
				result = PCBHack_Level4_Block7_ID;
			}
			if (textureName.Equals("pcb_hack4_block8"))
			{
				result = PCBHack_Level4_Block8_ID;
			}
			if (textureName.Equals("pcb_hack5_block1"))
			{
				result = PCBHack_Level5_Block1_ID;
			}
			if (textureName.Equals("pcb_hack5_block2"))
			{
				result = PCBHack_Level5_Block2_ID;
			}
			if (textureName.Equals("pcb_hack5_block3"))
			{
				result = PCBHack_Level5_Block3_ID;
			}
			if (textureName.Equals("pcb_hack5_block4"))
			{
				result = PCBHack_Level5_Block4_ID;
			}
			if (textureName.Equals("pcb_hack5_block5"))
			{
				result = PCBHack_Level5_Block5_ID;
			}
			if (textureName.Equals("pcb_hack5_block6"))
			{
				result = PCBHack_Level5_Block6_ID;
			}
			if (textureName.Equals("pcb_hack5_block7"))
			{
				result = PCBHack_Level5_Block7_ID;
			}
			if (textureName.Equals("pcb_hack5_block8"))
			{
				result = PCBHack_Level5_Block8_ID;
			}
			if (textureName.Equals("pcb_hack6_block1"))
			{
				result = PCBHack_Level6_Block1_ID;
			}
			if (textureName.Equals("pcb_hack6_block2"))
			{
				result = PCBHack_Level6_Block2_ID;
			}
			if (textureName.Equals("pcb_hack6_block3"))
			{
				result = PCBHack_Level6_Block3_ID;
			}
			if (textureName.Equals("pcb_hack6_block4"))
			{
				result = PCBHack_Level6_Block4_ID;
			}
			if (textureName.Equals("pcb_hack6_block5"))
			{
				result = PCBHack_Level6_Block5_ID;
			}
			if (textureName.Equals("pcb_hack6_block6"))
			{
				result = PCBHack_Level6_Block6_ID;
			}
			if (textureName.Equals("pcb_hack6_block7"))
			{
				result = PCBHack_Level6_Block7_ID;
			}
			if (textureName.Equals("pcb_hack6_block8"))
			{
				result = PCBHack_Level6_Block8_ID;
			}
			if (textureName.Equals("pcb_hack7_block1"))
			{
				result = PCBHack_Level7_Block1_ID;
			}
			if (textureName.Equals("pcb_hack7_block2"))
			{
				result = PCBHack_Level7_Block2_ID;
			}
			if (textureName.Equals("pcb_hack7_block3"))
			{
				result = PCBHack_Level7_Block3_ID;
			}
			if (textureName.Equals("pcb_hack7_block4"))
			{
				result = PCBHack_Level7_Block4_ID;
			}
			if (textureName.Equals("pcb_hack7_block5"))
			{
				result = PCBHack_Level7_Block5_ID;
			}
			if (textureName.Equals("pcb_hack7_block6"))
			{
				result = PCBHack_Level7_Block6_ID;
			}
			if (textureName.Equals("pcb_hack7_block7"))
			{
				result = PCBHack_Level7_Block7_ID;
			}
			if (textureName.Equals("pcb_hack7_block8"))
			{
				result = PCBHack_Level7_Block8_ID;
			}
			if (textureName.Equals("pcb_hack8_block1"))
			{
				result = PCBHack_Level8_Block1_ID;
			}
			if (textureName.Equals("pcb_hack8_block2"))
			{
				result = PCBHack_Level8_Block2_ID;
			}
			if (textureName.Equals("pcb_hack8_block3"))
			{
				result = PCBHack_Level8_Block3_ID;
			}
			if (textureName.Equals("pcb_hack8_block4"))
			{
				result = PCBHack_Level8_Block4_ID;
			}
			if (textureName.Equals("pcb_hack8_block5"))
			{
				result = PCBHack_Level8_Block5_ID;
			}
			if (textureName.Equals("pcb_hack8_block6"))
			{
				result = PCBHack_Level8_Block6_ID;
			}
			if (textureName.Equals("pcb_hack8_block7"))
			{
				result = PCBHack_Level8_Block7_ID;
			}
			if (textureName.Equals("pcb_hack8_block8"))
			{
				result = PCBHack_Level8_Block8_ID;
			}
			if (textureName.Equals("pcb_hack9_block1"))
			{
				result = PCBHack_Level9_Block1_ID;
			}
			if (textureName.Equals("pcb_hack9_block2"))
			{
				result = PCBHack_Level9_Block2_ID;
			}
			if (textureName.Equals("pcb_hack9_block3"))
			{
				result = PCBHack_Level9_Block3_ID;
			}
			if (textureName.Equals("pcb_hack9_block4"))
			{
				result = PCBHack_Level9_Block4_ID;
			}
			if (textureName.Equals("pcb_hack9_block5"))
			{
				result = PCBHack_Level9_Block5_ID;
			}
			if (textureName.Equals("pcb_hack9_block6"))
			{
				result = PCBHack_Level9_Block6_ID;
			}
			if (textureName.Equals("pcb_hack9_block7"))
			{
				result = PCBHack_Level9_Block7_ID;
			}
			if (textureName.Equals("pcb_hack9_block8"))
			{
				result = PCBHack_Level9_Block8_ID;
			}
			if (textureName.Equals("wm_background"))
			{
				result = PasscodeHack_Background;
			}
			if (textureName.Equals("wm_popup"))
			{
				result = PasscodeHack_Popup;
			}
			if (textureName.Equals("wm_popup_complete"))
			{
				result = PasscodeHack_Popup_Correct;
			}
			if (textureName.Equals("wm_popup_incorrect"))
			{
				result = PasscodeHack_Popup_InCorrect;
			}
			if (textureName.Equals("wm_popup_locked"))
			{
				result = PasscodeHack_Popup_OutofTime;
			}
			if (textureName.Equals("wm_line_1"))
			{
				result = PasscodeHack_Line_1;
			}
			if (textureName.Equals("wm_line_2"))
			{
				result = PasscodeHack_Line_2;
			}
			if (textureName.Equals("wm_line_3"))
			{
				result = PasscodeHack_Line_3;
			}
			if (textureName.Equals("wm_line_4"))
			{
				result = PasscodeHack_Line_4;
			}
			if (textureName.Equals("wm_line_5"))
			{
				result = PasscodeHack_Line_5;
			}
			return result;
		}

		private void drawTextAdv(string text, float x, float y, float scale, int r, int g, int b)
		{
			if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
			{
				double num = 1.778 / (double)screenaspectratio;
				Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
				Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
				Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
				Function.Call(Hash.SET_TEXT_CENTRE, true);
				Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, GetRatio(x), y, 0.1);
			}
			else
			{
				Memory.DrawText(text, false, 1, GetRatio(x), y, GetRatio(x), y, 1f, (float)r / 255f, (float)g / 255f, (float)b / 255f, scale / 1.5f, true);
			}
		}

		private void drawSprite_NormalOrMemory(string textureDict, string textureName, float screenX, float screenY, float width, float height, float Heading, int r, int g, int b, int alpha, int unk)
		{
			if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
			{
				int num = ReturnMemoryID(textureDict, textureName);
				if (!DrawnIDsThisFrame.Contains(num))
				{
					DrawnIDsThisFrame.Add(num);
				}
				else
				{
					if (PCBHackOn)
					{
						num = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PCBHack//" + textureName + ".png")).ID;
					}
					if (FiveLetterPasscodeHack)
					{
						num = new TextureID(Memory.CreateTexture(Environment.CurrentDirectory + FilePath + "textures//PasscodeHack//" + textureName + ".png")).ID;
					}
				}
				drawSprite_Mem(num, textureDict, textureName, screenX, screenY, width, height, Heading, r, g, b, alpha, unk);
			}
			else
			{
				float num2 = width * 1920f;
				float num3 = height * 1920f;
				screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
				float num4 = (float)(1.778 / (double)screenaspectratio);
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
				if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
				{
					Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetAspect(screenX), screenY, num2 / 1920f * num4, num3 / 1920f, Heading, r, g, b, alpha, 0);
				}
			}
		}

		private void drawSprite_NormalOrMemory(int ID, string textureDict, string textureName, float screenX, float screenY, float width, float height, float Heading, int r, int g, int b, int alpha, int unk)
		{
			if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
			{
				drawSprite_Mem(ID, textureDict, textureName, screenX, screenY, width, height, Heading, r, g, b, alpha, unk);
				return;
			}
			float num = width * 1920f;
			float num2 = height * 1920f;
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num3 = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetAspect(screenX), screenY, num / 1920f * num3, num2 / 1920f, Heading, r, g, b, alpha, 0);
			}
		}

		public static float GetAspect(float fParam0)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			return 0.5f - (0.5f - fParam0) / screenaspectratio;
		}

		public void GetTilePositions(int iParam0, out float uParam1, out float uParam2)
		{
			uParam1 = 0.105f;
			uParam2 = 0.306f;
			switch (iParam0)
			{
			case 1:
				uParam1 = 0.105f;
				uParam2 = 0.306f;
				break;
			case 2:
				uParam1 = 0.239f;
				uParam2 = 0.306f;
				break;
			case 3:
				uParam1 = 0.105f;
				uParam2 = 0.439f;
				break;
			case 4:
				uParam1 = 0.239f;
				uParam2 = 0.439f;
				break;
			case 5:
				uParam1 = 0.105f;
				uParam2 = 0.572f;
				break;
			case 6:
				uParam1 = 0.239f;
				uParam2 = 0.572f;
				break;
			case 7:
				uParam1 = 0.105f;
				uParam2 = 0.706f;
				break;
			case 8:
				uParam1 = 0.239f;
				uParam2 = 0.706f;
				break;
			}
		}

		public int ReturnID(float uParam1, float uParam2)
		{
			int result = -1;
			if (uParam1 == 0.105f && uParam2 == 0.306f)
			{
				result = 1;
			}
			if (uParam1 == 0.239f && uParam2 == 0.306f)
			{
				result = 2;
			}
			if (uParam1 == 0.105f && uParam2 == 0.439f)
			{
				result = 3;
			}
			if (uParam1 == 0.239f && uParam2 == 0.439f)
			{
				result = 4;
			}
			if (uParam1 == 0.105f && uParam2 == 0.572f)
			{
				result = 5;
			}
			if (uParam1 == 0.239f && uParam2 == 0.572f)
			{
				result = 6;
			}
			if (uParam1 == 0.105f && uParam2 == 0.706f)
			{
				result = 7;
			}
			if (uParam1 == 0.239f && uParam2 == 0.706f)
			{
				result = 8;
			}
			return result;
		}

		public string ReturnNFCPH(char C)
		{
			string result = "";
			int num = int.Parse(C.ToString());
			if (num == 0)
			{
				result = "numbers_0";
			}
			if (num == 1)
			{
				result = "numbers_1";
			}
			if (num == 2)
			{
				result = "numbers_2";
			}
			if (num == 3)
			{
				result = "numbers_3";
			}
			if (num == 4)
			{
				result = "numbers_4";
			}
			if (num == 5)
			{
				result = "numbers_5";
			}
			if (num == 6)
			{
				result = "numbers_6";
			}
			if (num == 7)
			{
				result = "numbers_7";
			}
			if (num == 8)
			{
				result = "numbers_8";
			}
			if (num == 9)
			{
				result = "numbers_9";
			}
			return result;
		}

		public string ReturnNFCPH(int C)
		{
			string result = "";
			if (C == 0)
			{
				result = "numbers_0";
			}
			if (C == 1)
			{
				result = "numbers_1";
			}
			if (C == 2)
			{
				result = "numbers_2";
			}
			if (C == 3)
			{
				result = "numbers_3";
			}
			if (C == 4)
			{
				result = "numbers_4";
			}
			if (C == 5)
			{
				result = "numbers_5";
			}
			if (C == 6)
			{
				result = "numbers_6";
			}
			if (C == 7)
			{
				result = "numbers_7";
			}
			if (C == 8)
			{
				result = "numbers_8";
			}
			if (C == 9)
			{
				result = "numbers_9";
			}
			return result;
		}

		public int GetPrintId(int TileId)
		{
			int result = -1;
			for (int i = 0; i < PrintId.Count; i++)
			{
				if (PrintId[i].PrintID == TileId)
				{
					result = PrintId[i].TileID;
				}
			}
			return result;
		}

		public bool GetSelectionResult(int TileId)
		{
			bool result = false;
			if (TileId == 1 && SelectedTile1PrintID + SelectedTile2PrintID + SelectedTile3PrintID + SelectedTile4PrintID == 18)
			{
				result = true;
			}
			if (TileId == 2 && SelectedTile1PrintID + SelectedTile2PrintID + SelectedTile3PrintID + SelectedTile4PrintID == 10)
			{
				result = true;
			}
			if (TileId == 3 && SelectedTile1PrintID + SelectedTile2PrintID + SelectedTile3PrintID + SelectedTile4PrintID == 10)
			{
				result = true;
			}
			if (TileId == 4 && SelectedTile1PrintID + SelectedTile2PrintID + SelectedTile3PrintID + SelectedTile4PrintID == 10)
			{
				result = true;
			}
			return result;
		}

		public bool SelectedID(int ID)
		{
			bool result = false;
			if (SelectedTile1PrintID == ID)
			{
				result = true;
			}
			if (SelectedTile2PrintID == ID)
			{
				result = true;
			}
			if (SelectedTile3PrintID == ID)
			{
				result = true;
			}
			if (SelectedTile4PrintID == ID)
			{
				result = true;
			}
			return result;
		}

		public Vector2 GetDotPos(int PX, int PY)
		{
			float x = 0f;
			float y = 0f;
			if (PX == 1)
			{
				x = 0.0713f;
			}
			if (PX == 2)
			{
				x = 0.172f;
			}
			if (PX == 3)
			{
				x = 0.271f;
			}
			if (PX == 4)
			{
				x = 0.371f;
			}
			if (PX == 5)
			{
				x = 0.472f;
			}
			if (PX == 6)
			{
				x = 0.572f;
			}
			if (PY == 1)
			{
				y = 0.315f;
			}
			if (PY == 2)
			{
				y = 0.417f;
			}
			if (PY == 3)
			{
				y = 0.515f;
			}
			if (PY == 4)
			{
				y = 0.616f;
			}
			if (PY == 5)
			{
				y = 0.716f;
			}
			return (Vector3)new Vector2(x, y);
		}

		public static void RunFingerPrintHack()
		{
			HackGUI = -1;
			Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
			Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
			HackFinishedID = 0;
			Script.Wait(1);
			HackStage = 0;
			FingerprintHackOn = !FingerprintHackOn;
			KeypadHackOn = false;
		}

		public static void RunKeypadHack()
		{
			HackGUI = -1;
			Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
			Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
			HackFinishedID = 0;
			Script.Wait(1);
			HackStage = 0;
			KeypadHackOn = !KeypadHackOn;
			FingerprintHackOn = false;
		}

		public void DisplayHelpTextThisFrame(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 1, Shape);
		}

		public void DisplayHelpTextThisFrameNoSound(string helpText, int Shape)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_HELP, "CELL_EMAIL_BCON");
			int num = 99;
			for (int i = 0; i < helpText.Length; i += num)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, Math.Min(num, helpText.Length - i)));
			}
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_HELP, 0, 0, 0, Shape);
		}

		public static void drawSpriteRTA(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha, float Heading)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, GetRatio(screenX), screenY, width / 1280f * num, height / 1280f, Heading, r, g, b, alpha, 0);
			}
		}

		public void GET_HOTWIRE_WIRE_DRAW_INFO(int iStartNode, int iEndNode, ref float fXPos, ref float fYPos, ref float fXScale, ref float fYScale, ref float fRot, ref string tlSprite, ref float fXPos2, ref float fYPos2, ref float fRot2, ref bool bTwoSprites)
		{
			switch (iStartNode)
			{
			case 0:
				switch (iEndNode)
				{
				case 0:
					fXPos = 0.5f;
					fYPos = 0.29f;
					fXScale = 0.7f;
					fYScale = 0.004f;
					fRot = 0f;
					tlSprite = "line";
					bTwoSprites = false;
					break;
				case 1:
					fXPos = 0.402f - cfWireOffset;
					fYPos = 0.372f;
					fXScale = 0.35f;
					fYScale = 0.17f;
					fRot = 0f;
					tlSprite = "line1";
					fXPos2 = 0.599f + cfWireOffset;
					fYPos2 = 0.37f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				case 2:
					fXPos = 0.5f;
					fYPos = 0.36f;
					fXScale = 0.7f;
					fYScale = 0.17f;
					fRot = 0f;
					tlSprite = "line2";
					fXPos2 = 0.5f;
					fYPos2 = 0.528f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				case 3:
					fXPos = 0.5f;
					fYPos = 0.392f;
					fXScale = 0.7f;
					fYScale = 0.25f;
					fRot = 0f;
					tlSprite = "line3";
					fXPos2 = 0.5f;
					fYPos2 = 0.64f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				}
				break;
			case 1:
				switch (iEndNode)
				{
				case 0:
					fXPos = 0.402f - cfWireOffset;
					fYPos = 0.369f;
					fXScale = -0.35f;
					fYScale = 0.17f;
					fRot = 180f;
					tlSprite = "line1";
					fXPos2 = 0.599f + cfWireOffset;
					fYPos2 = 0.371f;
					fRot2 = 0f;
					bTwoSprites = true;
					break;
				case 1:
					fXPos = 0.5f;
					fYPos = 0.44f;
					fXScale = 0.7f;
					fYScale = 0.004f;
					fRot = 0f;
					tlSprite = "line";
					bTwoSprites = false;
					break;
				case 2:
					fXPos = 0.402f - cfWireOffset;
					fYPos = 0.522f;
					fXScale = 0.35f;
					fYScale = 0.17f;
					fRot = 0f;
					tlSprite = "line1";
					fXPos2 = 0.599f + cfWireOffset;
					fYPos2 = 0.52f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				case 3:
					fXPos = 0.5f;
					fYPos = 0.51f;
					fXScale = 0.7f;
					fYScale = 0.17f;
					fRot = 0f;
					tlSprite = "line2";
					fXPos2 = 0.5f;
					fYPos2 = 0.678f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				}
				break;
			case 2:
				switch (iEndNode)
				{
				case 0:
					fXPos = 0.5f;
					fYPos = 0.36f;
					fXScale = -0.7f;
					fYScale = 0.17f;
					fRot = 0f;
					tlSprite = "line2";
					fXPos2 = 0.5f;
					fYPos2 = 0.528f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				case 1:
					fXPos = 0.402f - cfWireOffset;
					fYPos = 0.519f;
					fXScale = -0.35f;
					fYScale = 0.17f;
					fRot = 180f;
					tlSprite = "line1";
					fXPos2 = 0.599f + cfWireOffset;
					fYPos2 = 0.521f;
					fRot2 = 0f;
					bTwoSprites = true;
					break;
				case 2:
					fXPos = 0.5f;
					fYPos = 0.59f;
					fXScale = 0.7f;
					fYScale = 0.004f;
					fRot = 0f;
					tlSprite = "line";
					bTwoSprites = false;
					break;
				case 3:
					fXPos = 0.402f - cfWireOffset;
					fYPos = 0.672f;
					fXScale = 0.35f;
					fYScale = 0.17f;
					fRot = 0f;
					tlSprite = "line1";
					fXPos2 = 0.599f + cfWireOffset;
					fYPos2 = 0.67f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				}
				break;
			case 3:
				switch (iEndNode)
				{
				case 0:
					fXPos = 0.5f;
					fYPos = 0.392f;
					fXScale = -0.7f;
					fYScale = 0.25f;
					fRot = 0f;
					tlSprite = "line3";
					fXPos2 = 0.5f;
					fYPos2 = 0.64f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				case 1:
					fXPos = 0.5f;
					fYPos = 0.51f;
					fXScale = -0.7f;
					fYScale = 0.17f;
					fRot = 0f;
					tlSprite = "line2";
					fXPos2 = 0.5f;
					fYPos2 = 0.678f;
					fRot2 = 180f;
					bTwoSprites = true;
					break;
				case 2:
					fXPos = 0.402f - cfWireOffset;
					fYPos = 0.669f;
					fXScale = -0.35f;
					fYScale = 0.17f;
					fRot = 180f;
					tlSprite = "line1";
					fXPos2 = 0.599f + cfWireOffset;
					fYPos2 = 0.671f;
					fRot2 = 0f;
					bTwoSprites = true;
					break;
				case 3:
					fXPos = 0.5f;
					fYPos = 0.74f;
					fXScale = 0.7f;
					fYScale = 0.004f;
					fRot = 0f;
					tlSprite = "line";
					bTwoSprites = false;
					break;
				}
				break;
			}
		}

		public float LERP_FLOAT(float start, float end, float amount)
		{
			return start + (end - start) * amount;
		}

		public static Vector2 GET_PORT_FROM_START_NODE(int iNode)
		{
			Vector2 result = GET_MIDPOINT_FROM_START_NODE(iNode);
			result.X += 0.081f;
			return result;
		}

		public static Vector2 GET_MIDPOINT_FROM_START_NODE(int iNode)
		{
			List<Vector2> list = new List<Vector2>();
			if (iNode == 1)
			{
				list.AddRange(LeftNode1);
			}
			if (iNode == 2)
			{
				list.AddRange(LeftNode2);
			}
			if (iNode == 3)
			{
				list.AddRange(LeftNode3);
			}
			if (iNode == 4)
			{
				list.AddRange(LeftNode4);
			}
			return DIVIDE_VECTOR_2D(ADD_VECTOR_2D(list[0], list[2]), 2f);
		}

		public static void Draw_Wire(Vector2 vStartPoint, Vector2 vEndPoint)
		{
			float num = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			Vector2 vector = DIVIDE_VECTOR_2D(ADD_VECTOR_2D(vStartPoint, vEndPoint), 2f);
			float num2 = VECTOR_2D_DIST(vStartPoint, vEndPoint);
			float num3 = Function.Call<float>(Hash.ATAN2, vStartPoint.Y - vEndPoint.Y, vStartPoint.X - vEndPoint.X);
			Function.Call(Hash.DRAW_SPRITE_ARX, "MPHotwire", "line", GetRatio(vector.X), vector.Y, num2 / num, cfWireHeight, num3, 255, 255, 255, 255);
		}

		public static float VECTOR_2D_DIST(Vector2 v1, Vector2 v2)
		{
			float num = v1.X - v2.X;
			float num2 = v1.Y - v2.Y;
			return (float)Math.Sqrt(num * num + num2 * num2);
		}

		public static Vector2 ADD_VECTOR_2D(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
		}

		public static Vector2 DIVIDE_VECTOR_2D(Vector2 v, float divisor)
		{
			return new Vector2(v.X / divisor, v.Y / divisor);
		}

		private float func_11081(float fParam0)
		{
			fLocal_564 = Function.Call<float>(Hash.GET_ASPECT_RATIO, false);
			if (Function.Call<bool>(Hash.IS_PC_VERSION) && fLocal_564 >= 4f)
			{
				fLocal_564 /= 3f;
			}
			fParam0 = 0.5f - (0.5f - fParam0) / fLocal_564;
			return fParam0;
		}

		private static Vector2 ToFinalGfxPos(Vector2 p)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call(Hash.GET_SCRIPT_GFX_ALIGN_POSITION, p.X, p.Y, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		private static void DrawPath(Vector2[] pts, float thicknessPx, RGB c)
		{
			for (int i = 0; i < pts.Length - 1; i++)
			{
				DrawLineAxisAligned(pts[i], pts[i + 1], thicknessPx, c);
			}
		}

		private static Vector2 ToSafezone(Vector2 p)
		{
			float num = Function.Call<float>(Hash.GET_SAFE_ZONE_SIZE);
			float num2 = (1f - num) * 0.5f;
			return new Vector2(num2 + p.X * num, num2 + p.Y * num);
		}

		private static Vector2 AspectCorrect(Vector2 p)
		{
			float num = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			return new Vector2((p.X - 0.5f) / num + 0.5f, p.Y);
		}

		private static void DrawLineAxisAligned(Vector2 a, Vector2 b, float thicknessPx, RGB c)
		{
			float value = b.X - a.X;
			float value2 = b.Y - a.Y;
			bool num = Math.Abs(value) >= Math.Abs(value2);
			float num2 = thicknessPx / 1280f;
			float num3 = thicknessPx / 720f;
			if (num)
			{
				float x = a.X;
				float x2 = b.X;
				float y = a.Y;
				float num4 = (x + x2) * 0.5f;
				float num5 = Math.Abs(x2 - x);
				Function.Call(Hash.DRAW_RECT, num4, y, num5, num3, c.R, c.G, c.B, c.A);
			}
			else
			{
				float x3 = a.X;
				float y2 = a.Y;
				float y3 = b.Y;
				float num6 = (y2 + y3) * 0.5f;
				float num7 = Math.Abs(y3 - y2);
				Function.Call(Hash.DRAW_RECT, x3, num6, num2, num7, c.R, c.G, c.B, c.A);
			}
		}

		private static void DrawHighlight(Vector2 p, RGB c)
		{
			drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", p.X, p.Y, 45f, 50f, c.R, c.G, c.B, c.A);
		}

		private static void DrawHighlights(Vector2[] pts, RGB c)
		{
			for (int i = 0; i < pts.Length; i++)
			{
				DrawHighlight(pts[i], c);
			}
		}

		private static Vector2[] GetRoutePoints(int left, int right)
		{
			if (left == 1 && right == 1)
			{
				return new Vector2[2] { NodeCoord_L1, NodeCoord_R1 };
			}
			if (left == 1 && right == 2)
			{
				return new Vector2[4] { NodeCoord_L1, P_2889_2381, P_4790_4993, NodeCoord_R2 };
			}
			if (left == 1 && right == 3)
			{
				return new Vector2[6] { NodeCoord_L1, P_2889_2381, P_2875_5000, P_4790_4993, P_4802_7611, R3_ALT };
			}
			if (left == 2 && right == 1)
			{
				return new Vector2[6] { NodeCoord_L2, P_4139_5000, P_4156_3688, P_6046_3680, P_6075_2340, NodeCoord_R1 };
			}
			if (left == 2 && right == 2)
			{
				return new Vector2[2] { NodeCoord_L2, NodeCoord_R2 };
			}
			if (left == 2 && right == 3)
			{
				return new Vector2[4] { NodeCoord_L2, P_3526_4986, P_3529_7597, NodeCoord_R3 };
			}
			if (left == 3 && right == 1)
			{
				return new Vector2[6] { NodeCoord_L3, P_5418_7604, P_5427_6313, P_7328_6313, P_7319_2381, NodeCoord_R1 };
			}
			if (left == 3 && right == 2)
			{
				return new Vector2[6] { NodeCoord_L3, P_5418_7604, P_5409_6965, P_7331_6944, P_7319_5000, NodeCoord_R2 };
			}
			if (left == 3 && right == 3)
			{
				return new Vector2[2] { NodeCoord_L3, NodeCoord_R3 };
			}
			return null;
		}

		private static Vector2[] GetHighlightPoints(int left, int right)
		{
			if (left == 1 && right == 1)
			{
				return new Vector2[2] { NodeCoord_L1, NodeCoord_R1 };
			}
			if (left == 1 && right == 2)
			{
				return new Vector2[4] { NodeCoord_L1, P_4790_4993, NodeCoord_R2, NodeCoord_R1 };
			}
			if (left == 1 && right == 3)
			{
				return new Vector2[6] { NodeCoord_L1, P_2889_2381, P_2875_5000, P_4790_4993, P_4802_7611, R3_ALT };
			}
			if (left == 2 && right == 1)
			{
				return new Vector2[6] { NodeCoord_L2, P_4139_5000, P_4156_3688, P_6046_3680, P_6075_2340, NodeCoord_R1 };
			}
			if (left == 2 && right == 2)
			{
				return new Vector2[2] { NodeCoord_L2, NodeCoord_R2 };
			}
			if (left == 2 && right == 3)
			{
				return new Vector2[4] { NodeCoord_L2, P_3526_4986, P_3529_7597, NodeCoord_R3 };
			}
			if (left == 3 && right == 1)
			{
				return new Vector2[6] { NodeCoord_L3, P_5418_7604, P_5427_6313, P_7328_6313, P_7319_2381, NodeCoord_R1 };
			}
			if (left == 3 && right == 2)
			{
				return new Vector2[6] { NodeCoord_L3, P_5418_7604, P_5409_6965, P_7331_6944, P_7319_5000, NodeCoord_R2 };
			}
			if (left == 3 && right == 3)
			{
				return new Vector2[2] { NodeCoord_L3, NodeCoord_R3 };
			}
			return null;
		}

		private static RGB GetColour(int left)
		{
			switch (left)
			{
			case 1:
				return YELLOW;
			case 2:
				return RED;
			default:
				return BLUE;
			}
		}

		private static void RenderNodeConnectSection(int LeftNodeOn, int RightNodeOn, int VTLnodeSelected, int VTRnodeSelected, float thickness = 45f)
		{
			thickness = 45f;
			for (int i = 1; i <= 3; i++)
			{
				for (int j = 1; j <= 3; j++)
				{
					if ((LeftNodeOn == i && RightNodeOn == j) || (VTLnodeSelected == i && VTRnodeSelected == j))
					{
						RGB colour = GetColour(i);
						Vector2[] routePoints = GetRoutePoints(i, j);
						if (routePoints != null)
						{
							GTA.UI.Screen.ShowSubtitle("route " + routePoints[0].ToString());
							DrawPath(routePoints, thickness, colour);
						}
						Vector2[] highlightPoints = GetHighlightPoints(i, j);
						if (highlightPoints != null)
						{
							DrawHighlights(highlightPoints, colour);
						}
						return;
					}
				}
			}
		}

		public static float GetPositionFromScale_VMG(float DefaultPos)
		{
			float result = DefaultPos;
			if (DefaultPos < 0.5f)
			{
				result = DefaultPos * 1f + (1f - VoltageMG_Scale);
			}
			if (DefaultPos > 0.5f)
			{
				result = DefaultPos * VoltageMG_Scale;
			}
			return result;
		}

		public void ShowInstructionalButtonsCFPH()
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Pattern Up");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Pattern Down");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Pattern Next");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Pattern Back");
			scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Check");
			scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public void ShowInstructionalButtonsVT()
		{
			Scaleform scaleform = new Scaleform("instructional_buttons");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			scaleform.CallFunction("CREATE_CONTAINER");
			scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 176, 0), "Select Node");
			scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 172, 0), "Selection Up");
			scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 173, 0), "Selection Down");
			scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
			scaleform.Render2D();
		}

		public string ReturnNFC_Retro(char C)
		{
			string result = "";
			int num = int.Parse(C.ToString());
			if (num == 0)
			{
				result = "0";
			}
			if (num == 1)
			{
				result = "1";
			}
			if (num == 2)
			{
				result = "2";
			}
			if (num == 3)
			{
				result = "3";
			}
			if (num == 4)
			{
				result = "4";
			}
			if (num == 5)
			{
				result = "5";
			}
			if (num == 6)
			{
				result = "6";
			}
			if (num == 7)
			{
				result = "7";
			}
			if (num == 8)
			{
				result = "8";
			}
			if (num == 9)
			{
				result = "9";
			}
			return result;
		}

		public string ReturnNFC_Retro(int C)
		{
			string result = "";
			if (C == 0)
			{
				result = "0";
			}
			if (C == 1)
			{
				result = "1";
			}
			if (C == 2)
			{
				result = "2";
			}
			if (C == 3)
			{
				result = "3";
			}
			if (C == 4)
			{
				result = "4";
			}
			if (C == 5)
			{
				result = "5";
			}
			if (C == 6)
			{
				result = "6";
			}
			if (C == 7)
			{
				result = "7";
			}
			if (C == 8)
			{
				result = "8";
			}
			if (C == 9)
			{
				result = "9";
			}
			return result;
		}

		public string ReturnNFCPH_Retro(char C)
		{
			string result = "";
			int num = int.Parse(C.ToString());
			if (num == 0)
			{
				result = "fingerprint_hacking_minigame_numbers_00";
			}
			if (num == 1)
			{
				result = "fingerprint_hacking_minigame_numbers_01";
			}
			if (num == 2)
			{
				result = "fingerprint_hacking_minigame_numbers_02";
			}
			if (num == 3)
			{
				result = "fingerprint_hacking_minigame_numbers_03";
			}
			if (num == 4)
			{
				result = "fingerprint_hacking_minigame_numbers_04";
			}
			if (num == 5)
			{
				result = "fingerprint_hacking_minigame_numbers_05";
			}
			if (num == 6)
			{
				result = "fingerprint_hacking_minigame_numbers_06";
			}
			if (num == 7)
			{
				result = "fingerprint_hacking_minigame_numbers_07";
			}
			if (num == 8)
			{
				result = "fingerprint_hacking_minigame_numbers_08";
			}
			if (num == 9)
			{
				result = "fingerprint_hacking_minigame_numbers_09";
			}
			return result;
		}

		public string ReturnNFCPH_Retro(int C)
		{
			string result = "";
			if (C == 0)
			{
				result = "fingerprint_hacking_minigame_numbers_00";
			}
			if (C == 1)
			{
				result = "fingerprint_hacking_minigame_numbers_01";
			}
			if (C == 2)
			{
				result = "fingerprint_hacking_minigame_numbers_02";
			}
			if (C == 3)
			{
				result = "fingerprint_hacking_minigame_numbers_03";
			}
			if (C == 4)
			{
				result = "fingerprint_hacking_minigame_numbers_04";
			}
			if (C == 5)
			{
				result = "fingerprint_hacking_minigame_numbers_05";
			}
			if (C == 6)
			{
				result = "fingerprint_hacking_minigame_numbers_06";
			}
			if (C == 7)
			{
				result = "fingerprint_hacking_minigame_numbers_07";
			}
			if (C == 8)
			{
				result = "fingerprint_hacking_minigame_numbers_08";
			}
			if (C == 9)
			{
				result = "fingerprint_hacking_minigame_numbers_09";
			}
			return result;
		}

		public static void Loadehacker(bool playBoth)
		{
			BruteForcehackDone = false;
			IPHackDone = false;
			PlayBoth = playBoth;
			IpTryFailedTimer = Game.GameTime;
			TRYDL = false;
			Hacking_Lives = 3f;
			IpTry = false;
			DrawHackScaleform = false;
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
			Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "HACK", 3);
			string scaleformID = "Hacking_PC";
			if (Function.Call<bool>(Hash.GET_IS_WIDESCREEN))
			{
				GUIscaleform = new Scaleform(scaleformID);
			}
			else
			{
				GUIscaleform = new Scaleform(scaleformID);
			}
			hackingScalwform = GUIscaleform.Handle;
			while (!Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, hackingScalwform))
			{
				Script.Wait(10);
				GUIscaleform = new Scaleform(scaleformID);
				hackingScalwform = GUIscaleform.Handle;
			}
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_CURSOR_VISIBILITY");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, false);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_0", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_0"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_1", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_1"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_2", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_2"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_3", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_3"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_4", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_4"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "HACKING_PC_desktop_5", 0);
			while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "HACKING_PC_desktop_5"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@keypad@");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@keypad@"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel@hacking");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel@hacking"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@ornate_bank@hack");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@ornate_bank@hack"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "anim@heists@keypad@");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@keypad@"))
			{
				Script.Wait(10);
			}
			Function.Call(Hash.REQUEST_ANIM_DICT, "missheist_jewel@hacking");
			while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "missheist_jewel@hacking"))
			{
				Script.Wait(10);
			}
			Hacking_Index = 0;
		}

		public unsafe static void UnloadLoadehacker()
		{
			BruteForceHackOn = false;
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
			{
				Function.Call(Hash.STOP_SOUND, IPSound);
				IPSound = -1;
			}
			if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
			{
				Function.Call(Hash.STOP_SOUND, MoveSound);
				MoveSound = -1;
			}
			int num = hackingScalwform;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &num);
			hackingScalwform = 0;
			Hacking_Index = -1;
			Game.Player.CanControlCharacter = true;
			Script.Wait(1000);
			Game.Player.Character.Task.Wait(0);
		}

		public int DrawFIBHack(string amt)
		{
			iLocal_8077 = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "MORGUE_LAPTOP");
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, iLocal_8077))
			{
				Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, amt, "SET_PROGRESS_PERCENT");
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				return 1;
			}
			return 0;
		}

		private void drawSprite2(string textureDict, string textureName, float screenX, float screenY, float width, float height, int r, int g, int b, int alpha)
		{
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE, textureDict, textureName, screenX, screenY, width, height, 0, r, g, b, alpha, 0);
			}
		}

		private int check(float name)
		{
			if (name >= fLocal_240 && name <= fLocal_239)
			{
				return 1;
			}
			return 0;
		}

		private int activatescale(string scale)
		{
			scaleformloading = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, scale);
			if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, scaleformloading))
			{
				return 1;
			}
			return 0;
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

		public static string GetSpriteFromMultiplicationRatio(int MP)
		{
			string result = "icons__x1";
			switch (MP)
			{
			case 1:
				result = "icons__x1";
				break;
			case 2:
				result = "icons__x2";
				break;
			case 3:
				result = "icons__x3";
				break;
			case 5:
				result = "icons__x5";
				break;
			case 10:
				result = "icons__x10";
				break;
			case 15:
				result = "icons__x15";
				break;
			}
			return result;
		}

		public static int GetRandomMultiplier()
		{
			List<int> list = new List<int> { 1, 2, 3, 5, 10, 15 };
			int num = list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
			while (MultiplicationsChoosen.Contains(num))
			{
				Script.Wait(1);
				num = list[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, list.Count)];
			}
			MultiplicationsChoosen.Add(num);
			return num;
		}

		public static void ScrableNumber()
		{
			MultiplicationsChoosen.Clear();
			Random random = new Random();
			LNo1 = random.Next(1, 9);
			LNo2 = random.Next(1, 9);
			LNo3 = random.Next(1, 9);
			RNo1 = GetRandomMultiplier();
			RNo2 = GetRandomMultiplier();
			RNo3 = GetRandomMultiplier();
			int volthackTargetNumber = 0;
			int num = random.Next(0, 90);
			if (num >= 0 && num < 10)
			{
				volthackTargetNumber = LNo1 * RNo1 + LNo2 * RNo2 + LNo3 * RNo3;
			}
			if (num >= 10 && num < 20)
			{
				volthackTargetNumber = LNo1 * RNo2 + LNo2 * RNo3 + LNo3 * RNo1;
			}
			if (num >= 20 && num < 30)
			{
				volthackTargetNumber = LNo1 * RNo3 + LNo2 * RNo2 + LNo3 * RNo1;
			}
			if (num >= 30 && num < 40)
			{
				volthackTargetNumber = LNo2 * RNo1 + LNo1 * RNo2 + LNo3 * RNo3;
			}
			if (num >= 40 && num < 50)
			{
				volthackTargetNumber = LNo2 * RNo3 + LNo3 * RNo2 + LNo1 * RNo1;
			}
			if (num >= 50 && num < 60)
			{
				volthackTargetNumber = LNo2 * RNo3 + LNo1 * RNo2 + LNo3 * RNo1;
			}
			if (num >= 60 && num < 70)
			{
				volthackTargetNumber = LNo3 * RNo1 + LNo2 * RNo2 + LNo1 * RNo3;
			}
			if (num >= 70 && num < 80)
			{
				volthackTargetNumber = LNo3 * RNo2 + LNo1 * RNo3 + LNo2 * RNo1;
			}
			if (num >= 80 && num < 90)
			{
				volthackTargetNumber = LNo2 * RNo2 + LNo1 * RNo3 + LNo3 * RNo1;
			}
			VolthackTargetNumber = volthackTargetNumber;
		}

		public void CallVoltageHack()
		{
			if (!PlayingVoltageMG)
			{
				Game.Player.CanControlCharacter = false;
				VideoPlaying = false;
				BINKMovie = -1;
				AwaitingDelay = false;
				RightNodeOn = 0;
				SelectedLN1 = false;
				SelectedRN1 = false;
				SelectedLN2 = false;
				SelectedRN2 = false;
				SelectedLN3 = false;
				SelectedRN3 = false;
				VTL1nodeSelected = 0;
				VTR1nodeSelected = 0;
				VTL2nodeSelected = 0;
				VTR2nodeSelected = 0;
				VTL3nodeSelected = 0;
				VTR3nodeSelected = 0;
				VTL1node = 0;
				VTR1node = 0;
				VTL2node = 0;
				VTR2node = 0;
				VTL3node = 0;
				VTR3node = 0;
				RightNodeOn = 0;
				LeftNodeOn = 1;
				ScrableNumberTimer = Game.GameTime;
				NodeOn = 0;
				NumberAtCheck = 0f;
				DisableControlls = false;
				BINKMovie = Function.Call<int>(Hash.SET_BINK_MOVIE, "voltage_intro");
				Function.Call(Hash.PLAY_BINK_MOVIE, BINKMovie);
				VideoPlaying = true;
				HackStage = 0;
				PlayingVoltageMG = true;
				ScrableNumberTimer = Game.GameTime + (ScrableNumberTime + ScrableNumberTime / 6);
			}
			else if (PlayingVoltageMG)
			{
				Game.Player.CanControlCharacter = true;
				VideoPlaying = false;
				BINKMovie = -1;
				BINKMovie = -1;
				PlayingVoltageMG = false;
			}
		}

		public static void RunVoltageHack()
		{
			VideoPlaying = false;
			Function.Call(Hash.STOP_BINK_MOVIE, BINKMovie);
			Function.Call(Hash.RELEASE_BINK_MOVIE, BINKMovie);
			VoltageHack = 1;
		}

		public void CallCopyPrintHack()
		{
			if (!CopyPrintHackOn)
			{
				Game.Player.CanControlCharacter = false;
				PHHackSuccess = false;
				PHTimeLeft = 340f;
				VideoPlaying = false;
				BINKMovie = -1;
				Random random = new Random();
				Print = random.Next(0, 800);
				if (Print > 0 && Print <= 100)
				{
					PrintNum = 1;
					PrintDict = "mpfclone_retro_printfull1";
				}
				if (Print > 100 && Print <= 200)
				{
					PrintNum = 2;
					PrintDict = "mpfclone_retro_printfull2";
				}
				if (Print > 200 && Print <= 300)
				{
					PrintNum = 3;
					PrintDict = "mpfclone_retro_printfull3";
				}
				if (Print > 300 && Print <= 400)
				{
					PrintNum = 4;
					PrintDict = "mpfclone_retro_printfull4";
				}
				if (Print > 400 && Print <= 500)
				{
					PrintNum = 5;
					PrintDict = "mpfclone_retro_printfull5";
				}
				if (Print > 500 && Print <= 600)
				{
					PrintNum = 6;
					PrintDict = "mpfclone_retro_printfull6";
				}
				if (Print > 600 && Print <= 700)
				{
					PrintNum = 7;
					PrintDict = "mpfclone_retro_printfull7";
				}
				if (Print > 700 && Print <= 800)
				{
					PrintNum = 8;
					PrintDict = "mpfclone_retro_printfull8";
				}
				if (Section1Print.Count > 0)
				{
					Section1Print.Clear();
				}
				if (Section2Print.Count > 0)
				{
					Section2Print.Clear();
				}
				if (Section3Print.Count > 0)
				{
					Section3Print.Clear();
				}
				if (Section4Print.Count > 0)
				{
					Section4Print.Clear();
				}
				if (Section5Print.Count > 0)
				{
					Section5Print.Clear();
				}
				if (Section6Print.Count > 0)
				{
					Section6Print.Clear();
				}
				if (Section7Print.Count > 0)
				{
					Section7Print.Clear();
				}
				if (Section8Print.Count > 0)
				{
					Section8Print.Clear();
				}
				int num = random.Next(0, 400);
				if (num > 0 && num <= 100)
				{
					HacksToComplete = 1;
				}
				if (num > 100 && num <= 200)
				{
					HacksToComplete = 2;
				}
				if (num > 200 && num <= 300)
				{
					HacksToComplete = 3;
				}
				if (num > 300 && num <= 400)
				{
					HacksToComplete = 4;
				}
				if (OverrideHacksToComplete != 0)
				{
					HacksToComplete = OverrideHacksToComplete;
					OverrideHacksToComplete = 0;
				}
				HacksCompleted = 0;
				Section1Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_01");
				Section2Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_02");
				Section3Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_03");
				Section4Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_04");
				Section5Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_05");
				Section6Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_06");
				Section7Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_07");
				Section8Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_08");
				for (int i = 0; i < 200; i++)
				{
					int index = random.Next(0, Prints.Count);
					if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_01") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_01") && !Section1Print.Contains(Prints[index]) && Section1Print.Count < 15)
					{
						Section1Print.Add(Prints[index]);
						index = random.Next(0, Prints.Count);
					}
					index = random.Next(0, Prints.Count);
					if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_02") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_02") && !Section2Print.Contains(Prints[index]) && Section2Print.Count < 15)
					{
						Section2Print.Add(Prints[index]);
						index = random.Next(0, Prints.Count);
					}
					index = random.Next(0, Prints.Count);
					if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_03") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_03") && !Section3Print.Contains(Prints[index]) && Section3Print.Count < 15)
					{
						Section3Print.Add(Prints[index]);
						index = random.Next(0, Prints.Count);
					}
					index = random.Next(0, Prints.Count);
					if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_04") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_04") && !Section4Print.Contains(Prints[index]) && Section4Print.Count < 15)
					{
						Section4Print.Add(Prints[index]);
						index = random.Next(0, Prints.Count);
					}
					index = random.Next(0, Prints.Count);
					if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_05") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_05") && !Section5Print.Contains(Prints[index]) && Section5Print.Count < 15)
					{
						Section5Print.Add(Prints[index]);
						index = random.Next(0, Prints.Count);
					}
					index = random.Next(0, Prints.Count);
					if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_06") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_06") && !Section6Print.Contains(Prints[index]) && Section6Print.Count < 15)
					{
						Section6Print.Add(Prints[index]);
						index = random.Next(0, Prints.Count);
					}
					index = random.Next(0, Prints.Count);
					if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_07") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_07") && !Section7Print.Contains(Prints[index]) && Section7Print.Count < 15)
					{
						Section7Print.Add(Prints[index]);
						index = random.Next(0, Prints.Count);
					}
					index = random.Next(0, Prints.Count);
					if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_08") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_08") && !Section8Print.Contains(Prints[index]) && Section8Print.Count < 15)
					{
						Section8Print.Add(Prints[index]);
						index = random.Next(0, Prints.Count);
					}
				}
				Selection1 = random.Next(0, Section1Print.Count);
				Selection2 = random.Next(0, Section2Print.Count);
				Selection3 = random.Next(0, Section3Print.Count);
				Selection4 = random.Next(0, Section4Print.Count);
				Selection5 = random.Next(0, Section5Print.Count);
				Selection6 = random.Next(0, Section6Print.Count);
				Selection7 = random.Next(0, Section7Print.Count);
				Selection8 = random.Next(0, Section8Print.Count);
				BINKMovie = Function.Call<int>(Hash.SET_BINK_MOVIE, "fingerprint_intro");
				Function.Call(Hash.PLAY_BINK_MOVIE, BINKMovie);
				VideoPlaying = true;
				HackStage = 0;
				Game.Player.Character.IsPositionFrozen = true;
				CopyPrintHackOn = true;
			}
			else if (CopyPrintHackOn)
			{
				Game.Player.CanControlCharacter = true;
				VideoPlaying = false;
				BINKMovie = -1;
				BINKMovie = -1;
				Game.Player.Character.IsPositionFrozen = false;
				CopyPrintHackOn = false;
			}
		}

		public static void RunCopyPrintHack()
		{
			VideoPlaying = false;
			Function.Call(Hash.STOP_BINK_MOVIE, BINKMovie);
			Function.Call(Hash.RELEASE_BINK_MOVIE, BINKMovie);
			CopyPrintHack = 1;
		}

		public void CallPadLockHack()
		{
			if (!RunningPadLockMinigame)
			{
				Game.Player.CanControlCharacter = false;
				while (!Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, "anim@heists@fleeca_bank@drilling") && !Function.Call<bool>(Hash.HAS_NAMED_PTFX_ASSET_LOADED, "fm_mission_controler") && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "SAFE_CRACK", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "HUD_MINI_GAME_SOUNDSET", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "MissionFailedSounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "dlc_heist_fleeca_bank_door_sounds", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "vault_door", 0, -1) && !Function.Call<bool>(Hash.REQUEST_SCRIPT_AUDIO_BANK, "Alarms", 0, -1) && !Function.Call<bool>(Hash.REQUEST_AMBIENT_AUDIO_BANK, "DLC_HEIST_FLEECA_SOUNDSET", 0, -1))
				{
					Script.Wait(100);
				}
				ActualRotation = 0f;
				Rotation = 0f;
				Random random = new Random();
				Number1 = random.Next(1, 98);
				GotNo1 = false;
				Script.Wait(1);
				Number2 = random.Next(1, 98);
				GotNo2 = false;
				for (int i = 0; i < 2000; i++)
				{
					if (Number2 != Number1)
					{
						break;
					}
					Number2 = random.Next(1, 98);
					GotNo2 = false;
				}
				Script.Wait(1);
				Number3 = random.Next(1, 98);
				GotNo3 = false;
				for (int j = 0; j < 2000; j++)
				{
					if (Number3 == Number1)
					{
						Script.Wait(1);
						Number1 = random.Next(1, 98);
						GotNo2 = false;
					}
					if (Number3 == Number2)
					{
						Script.Wait(1);
						Number3 = random.Next(1, 98);
						GotNo2 = false;
					}
					if (Number3 != Number1 && Number3 != Number2 && Number2 != Number1)
					{
						break;
					}
				}
				Script.Wait(1);
				HackStage = 0;
				RunningPadLockMinigame = true;
				PDLHackComplete = false;
			}
			else if (RunningPadLockMinigame)
			{
				Game.Player.CanControlCharacter = true;
				HackStage = 0;
				RunningPadLockMinigame = false;
				PDLHackComplete = false;
			}
		}

		public static void RunPadLockHack()
		{
			PadLockHack = 1;
		}

		public void CallHotwireHack()
		{
			if (!HotWireMinigameOn)
			{
				Game.Player.CanControlCharacter = false;
				SelectingNode = false;
				Node1Warnings = new List<float> { 255f, 255f, 255f, 255f };
				Node2Warnings = new List<float> { 255f, 255f, 255f, 255f };
				Node3Warnings = new List<float> { 255f, 255f, 255f, 255f };
				Node4Warnings = new List<float> { 255f, 255f, 255f, 255f };
				RightNode_1Lives = 4;
				RightNode_2Lives = 4;
				RightNode_3Lives = 4;
				RightNode_4Lives = 4;
				HotwireTimeLeft = Game.GameTime + 90000;
				SuccessHotwire = false;
				HotwireloadingDelayTimer = 0f;
				HotwireloadingDelay = 0f;
				TokensSelected = 0;
				SelectedLeftNode_1 = 0;
				SelectedRightNode_1 = 0;
				SelectedLeftNode_2 = 0;
				SelectedRightNode_2 = 0;
				SelectedLeftNode_3 = 0;
				SelectedRightNode_3 = 0;
				SelectedLeftNode_4 = 0;
				SelectedRightNode_4 = 0;
				Random random = new Random();
				_1ID = 0;
				_2ID = 0;
				_3ID = 0;
				_4ID = 0;
				LeftNode_1Locked = 0;
				LeftNode_2Locked = 0;
				LeftNode_3Locked = 0;
				LeftNode_4Locked = 0;
				Correct_1ID = -1;
				Correct_2ID = -1;
				Correct_3ID = -1;
				Correct_4ID = -1;
				int num = random.Next(0, 100);
				if (num < 10)
				{
					Correct_1ID = 1;
					Correct_2ID = 2;
					Correct_3ID = 3;
					Correct_4ID = 4;
				}
				if (num >= 10 && num < 20)
				{
					Correct_1ID = 4;
					Correct_2ID = 3;
					Correct_3ID = 2;
					Correct_4ID = 1;
				}
				if (num >= 20 && num < 30)
				{
					Correct_1ID = 2;
					Correct_2ID = 4;
					Correct_3ID = 1;
					Correct_4ID = 3;
				}
				if (num >= 30 && num < 40)
				{
					Correct_1ID = 3;
					Correct_2ID = 1;
					Correct_3ID = 4;
					Correct_4ID = 2;
				}
				if (num >= 40 && num < 50)
				{
					Correct_1ID = 3;
					Correct_2ID = 4;
					Correct_3ID = 1;
					Correct_4ID = 2;
				}
				if (num >= 50 && num < 60)
				{
					Correct_1ID = 2;
					Correct_2ID = 1;
					Correct_3ID = 4;
					Correct_4ID = 3;
				}
				if (num >= 60 && num < 70)
				{
					Correct_1ID = 4;
					Correct_2ID = 3;
					Correct_3ID = 1;
					Correct_4ID = 2;
				}
				if (num >= 60 && num < 70)
				{
					Correct_1ID = 3;
					Correct_2ID = 1;
					Correct_3ID = 4;
					Correct_4ID = 2;
				}
				if (num >= 70 && num < 80)
				{
					Correct_1ID = 4;
					Correct_2ID = 1;
					Correct_3ID = 2;
					Correct_4ID = 3;
				}
				if (num >= 80 && num < 90)
				{
					Correct_1ID = 3;
					Correct_2ID = 1;
					Correct_3ID = 2;
					Correct_4ID = 4;
				}
				if (num >= 90 && num < 100)
				{
					Correct_1ID = 1;
					Correct_2ID = 4;
					Correct_3ID = 3;
					Correct_4ID = 2;
				}
				LeftSelected = false;
				RightSelected = true;
				RightNode = 0;
				Notification.Show("Hot Wire Minigame On");
				HackStage = 0;
				HotWireMinigameOn = true;
				Game.Player.CanControlCharacter = false;
			}
			else if (HotWireMinigameOn)
			{
				Game.Player.CanControlCharacter = true;
				LeftSelected = false;
				RightSelected = false;
				RightNode = 0;
				HackStage = 0;
				Notification.Show("Hot Wire Minigame off");
				HotWireMinigameOn = false;
				Game.Player.CanControlCharacter = true;
			}
		}

		public static void RunHotwireHack()
		{
			HotwireHack = 1;
		}

		public static void RunBruteforceHack_Numbers()
		{
			HackingSuccess = true;
			ActiveHack = false;
			HackDone = false;
			UnloadLoadehacker();
			HackFinishedID = 0;
			BruteforceType = 1;
			BruteforceHack = 1;
		}

		public static void RunBruteforceHack_Password()
		{
			HackingSuccess = true;
			ActiveHack = false;
			HackDone = false;
			UnloadLoadehacker();
			BruteforceType = 2;
			BruteforceHack = 1;
		}

		public static void RunBruteforceHack_Both()
		{
			HackingSuccess = true;
			ActiveHack = false;
			HackDone = false;
			UnloadLoadehacker();
			BruteforceType = 3;
			BruteforceHack = 1;
		}

		public void CallBruteforceHack()
		{
			if (!BruteForceHackOn)
			{
				HackToComplete = BruteforceType;
				HackingSuccess = false;
				Loadehacker(true);
				BruteForceHackOn = true;
			}
			else if (BruteForceHackOn)
			{
				HackingSuccess = false;
				UnloadLoadehacker();
				BruteForceHackOn = false;
			}
		}

		public static void RunFIBHack()
		{
			FIBHack = 1;
		}

		public void CallFIBHack()
		{
			if (!FIBHackOn)
			{
				FIBHackOn = true;
			}
			else if (FIBHackOn)
			{
				FIBHackOn = false;
			}
		}

		public static void RunDataCrack()
		{
			DataHack = 1;
		}

		public void CallDataCrackHack()
		{
			if (!DataCrackOn)
			{
				Notification.Show("~y~ You can cancel The hack at any time by pressing E");
				HackingSuccess = false;
				if (Game.Player.Character.CurrentVehicle != null)
				{
					Game.Player.Character.CurrentVehicle.IsPositionFrozen = true;
				}
				colOneY = 0.4f;
				colTwoY = 0.4f;
				colThreeY = 0.4f;
				colFourY = 0.4f;
				colFiveY = 0.74f;
				colSixY = 0.74f;
				colSevenY = 0.74f;
				colEightY = 0.74f;
				collumn1 = false;
				collumn2 = false;
				collumn3 = false;
				collumn4 = false;
				collumn5 = false;
				collumn6 = false;
				collumn7 = false;
				collumn8 = false;
				skip1 = false;
				skip2 = false;
				skip3 = false;
				loadingin = true;
				up = false;
				up1 = false;
				up2 = false;
				up3 = false;
				up4 = true;
				up5 = true;
				up6 = true;
				up7 = true;
				currentbar = 0;
				indexx = 20;
				DataCrackOn = true;
			}
			else if (DataCrackOn)
			{
				if (Game.Player.Character.CurrentVehicle != null)
				{
					Game.Player.Character.CurrentVehicle.IsPositionFrozen = false;
				}
				HackingSuccess = false;
				Notification.Show("~y~ Lester ~w~ : The Hack has Reset");
				colOneY = 0.4f;
				colTwoY = 0.4f;
				colThreeY = 0.4f;
				colFourY = 0.4f;
				colFiveY = 0.74f;
				colSixY = 0.74f;
				colSevenY = 0.74f;
				colEightY = 0.74f;
				collumn1 = false;
				collumn2 = false;
				collumn3 = false;
				collumn4 = false;
				collumn5 = false;
				collumn6 = false;
				collumn7 = false;
				collumn8 = false;
				skip1 = false;
				skip2 = false;
				skip3 = false;
				loadingin = true;
				up = false;
				up1 = false;
				up2 = false;
				up3 = false;
				up4 = true;
				up5 = true;
				up6 = true;
				up7 = true;
				currentbar = 0;
				indexx = 20;
				DataCrackOn = false;
			}
		}

		public static void RunSecuroServPhoneHack()
		{
			VirusStage = 0;
			DELETE_VIRUS();
			SecuroServPhoneHackOn = true;
			SecuroServPhoneHack = 1;
		}

		public unsafe static void DELETE_VIRUS()
		{
			int uPLOADVIRUS = UPLOADVIRUS;
			Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &uPLOADVIRUS);
			UPLOADVIRUS = 0;
		}

		public unsafe static void DRAW_VIRUS()
		{
			int num = 0;
			Function.Call(Hash.GET_MOBILE_PHONE_RENDER_ID, &num);
			Function.Call(Hash.SET_TEXT_RENDER_ID, num);
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE, UPLOADVIRUS, 0.10017f, 0.18136f, 0.2f, 0.36f, 255, 255, 255, 255, 0);
			Function.Call(Hash.SET_TEXT_RENDER_ID, 1);
			Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
		}

		public void RunScaleformFunction(int Handle, string name, params object[] args)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, name);
			foreach (object obj in args)
			{
				if (obj.GetType() == typeof(int))
				{
					Function.Call<int>(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)obj);
				}
				else if (obj.GetType() == typeof(float))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)obj);
				}
				else if (obj.GetType() == typeof(double))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)(double)obj);
				}
				else if (obj.GetType() == typeof(bool))
				{
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)obj);
				}
				else if (obj.GetType() == typeof(string))
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (string)obj);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
				else if (obj.GetType() == typeof(char))
				{
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ((char)obj).ToString());
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void ClearAppTrackifyTargets()
		{
			TrackifyTargets_Vector3.Clear();
			TrackifyTargets_Enitity.Clear();
		}

		public static void AddTrackifyTarget_Vector3(Vector3 Tar)
		{
			if (!TrackifyTargets_Vector3.Contains(Tar))
			{
				TrackifyTargets_Vector3.Add(Tar);
			}
		}

		public static void RemoveTrackifyTarget_Vector3(Vector3 Tar)
		{
			if (TrackifyTargets_Vector3.Contains(Tar))
			{
				TrackifyTargets_Vector3.Remove(Tar);
			}
		}

		public static void AddTrackifyTarget_Entity(Entity Tar)
		{
			if (!TrackifyTargets_Enitity.Contains(Tar))
			{
				TrackifyTargets_Enitity.Add(Tar);
			}
		}

		public static void RemoveTrackifyTarget_Entity(Entity Tar)
		{
			if (TrackifyTargets_Enitity.Contains(Tar))
			{
				TrackifyTargets_Enitity.Remove(Tar);
			}
		}

		public static void RunAppTrackify()
		{
			AppTrackifyOn = true;
			AppTrackify = 1;
			TrackifySequence = -1;
			TrackifyOpenTime = Game.GameTime + 2000;
		}

		public static void EndAppTrackify()
		{
			AppTrackifyOn = false;
			AppTrackify = -1;
			AppTrackifyWasRunning = false;
			Delete_AppTrackify();
			if (AppTrackifyHandle != null)
			{
				AppTrackifyHandle.CallFunction("CLEAN_UP_DATA");
				AppTrackifyHandle.CallFunction("checkAndRemoveTween");
				AppTrackifyHandle.Dispose();
			}
			TrackifySequence = -1;
		}

		public static void Delete_AppTrackify()
		{
			if (AppTrackifyHandle != null)
			{
				AppTrackifyHandle.Dispose();
			}
			AppTrackifyHandle = null;
		}

		private void func_14(int iParam0, string sParam1, float fParam2, float fParam3, float fParam4, float fParam5, float fParam6)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, sParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam2);
			if (fParam3 != -1f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam3);
			}
			if (fParam4 != -1f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam4);
			}
			if (fParam5 != -1f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam5);
			}
			if (fParam6 != -1f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam6);
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_8(int iParam0, string sParam1, float fParam2, float fParam3, float fParam4, float fParam5, float fParam6, string sParam7, string sParam8, string sParam9, string sParam10, string sParam11)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iParam0, sParam1);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam2);
			if (fParam3 != -1f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam3);
			}
			if (fParam4 != -1f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam4);
			}
			if (fParam5 != -1f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam5);
			}
			if (fParam6 != -1f)
			{
				Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)fParam6);
			}
			if (!Function.Call<bool>(Hash.IS_STRING_NULL_OR_EMPTY, sParam7))
			{
				func_9(sParam7);
			}
			if (!Function.Call<bool>(Hash.IS_STRING_NULL_OR_EMPTY, sParam8))
			{
				func_9(sParam8);
			}
			if (!Function.Call<bool>(Hash.IS_STRING_NULL_OR_EMPTY, sParam9))
			{
				func_9(sParam9);
			}
			if (!Function.Call<bool>(Hash.IS_STRING_NULL_OR_EMPTY, sParam10))
			{
				func_9(sParam10);
			}
			if (!Function.Call<bool>(Hash.IS_STRING_NULL_OR_EMPTY, sParam11))
			{
				func_9(sParam11);
			}
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		private void func_9(string sParam0)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, sParam0);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
		}

		public void CallCircuitHack()
		{
			if (!CircuitHackOn)
			{
				HacksCompleted = 0;
				CurrentSnakeLines.Clear();
				SnakeLinesPositions.Clear();
				Direction = "Right";
				if (new Random().Next(0, 100) < 50)
				{
					LevelChosen = 0;
				}
				else
				{
					LevelChosen = 3;
				}
				HackStage = 0;
				CircuitHackOn = true;
			}
			else if (CircuitHackOn)
			{
				CircuitBreaker.CleanupCircuitMinigame();
				HackFinishedID = 0;
				CircuitHackOn = false;
				CircuitHackOn = false;
			}
		}

		public static void RunCircuitHack()
		{
			CircuitHack = 1;
		}

		public void BEGIN_SCALEFORM_MOVIE_METHOD(int id, string method)
		{
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, id, method);
		}

		public void END_SCALEFORM_MOVIE_METHOD()
		{
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public int REQUEST_SCALEFORM_MOVIE(string Movie)
		{
			return Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, Movie);
		}

		public void SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT(int value)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, value);
		}

		public void SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL(bool B)
		{
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, B);
		}

		public void END_TEXT_COMMAND_SCALEFORM_STRING()
		{
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
		}

		public void BEGIN_TEXT_COMMAND_SCALEFORM_STRING(string ID)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, ID);
		}

		public void func_138(string sParam0)
		{
			BEGIN_TEXT_COMMAND_SCALEFORM_STRING(sParam0);
			END_TEXT_COMMAND_SCALEFORM_STRING();
		}

		public void LoadIniFile(string iniName)
		{
			try
			{
				Config = ScriptSettings.Load(iniName);
			}
			catch (Exception)
			{
			}
		}

		public static void ToggleButtonPressMinigame()
		{
			ButtonPressMinigameCompleted = false;
			SetsComplete = 0;
			ButtonAt = 0;
			ButtonsPressed = 0;
			CurrentSet.Clear();
			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				ButtonPressMinigameSet buttonPressMinigameSet = new ButtonPressMinigameSet();
				buttonPressMinigameSet.ControlSet = new List<Control>();
				buttonPressMinigameSet.ControlSet.Add(ButtonPressMinigameAllowedKeys[random.Next(0, ButtonPressMinigameAllowedKeys.Count)]);
				Script.Wait(0);
				buttonPressMinigameSet.ControlSet.Add(ButtonPressMinigameAllowedKeys[random.Next(0, ButtonPressMinigameAllowedKeys.Count)]);
				Script.Wait(0);
				buttonPressMinigameSet.ControlSet.Add(ButtonPressMinigameAllowedKeys[random.Next(0, ButtonPressMinigameAllowedKeys.Count)]);
				Script.Wait(0);
				buttonPressMinigameSet.ControlSet.Add(ButtonPressMinigameAllowedKeys[random.Next(0, ButtonPressMinigameAllowedKeys.Count)]);
				Script.Wait(0);
				buttonPressMinigameSet.ControlSet.Add(ButtonPressMinigameAllowedKeys[random.Next(0, ButtonPressMinigameAllowedKeys.Count)]);
				Script.Wait(0);
				CurrentSet.Add(buttonPressMinigameSet);
			}
			PlayingButtonPressMinigame = !PlayingButtonPressMinigame;
			if (PlayingButtonPressMinigame)
			{
				Game.Player.IsInvincible = true;
				Game.Player.CanControlCharacter = false;
			}
			else
			{
				Game.Player.IsInvincible = false;
				Game.Player.CanControlCharacter = true;
			}
		}

		public void ShowButtonPressMinigame(List<ButtonPressMinigameSet> Buttons, int ButtonsToComplete)
		{
			Game.Player.IsInvincible = true;
			Game.Player.CanControlCharacter = false;
			Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match");
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match", false, -1);
			Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking");
			Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking", false, -1);
			Game.DisableAllControlsThisFrame();
			Game.DisableControlThisFrame(Control.Phone);
			Scaleform scaleform = new Scaleform("GENERIC_INSTRUCTIONAL_BUTTONS");
			scaleform.CallFunction("CLEAR_ALL");
			scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
			if (ButtonAt <= 0 && Buttons.Count >= 1)
			{
				int num = (int)Buttons[SetsComplete].ControlSet[0];
				scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, num, 0), "");
				if (ButtonAt == 0 && Game.IsControlJustPressed((Control)num))
				{
					Script.Wait(0);
					FlashesToComplete = 4;
					FlashesDone = 0;
					ChangeColorTimer = Game.GameTime + 25;
					JustClickedButton = true;
					ButtonsPressed++;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
					ButtonAt++;
				}
			}
			if (ButtonAt <= 1 && Buttons.Count >= 2)
			{
				int num2 = (int)Buttons[SetsComplete].ControlSet[1];
				scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, num2, 0), "");
				if (ButtonAt == 1 && Game.IsControlJustPressed((Control)num2))
				{
					Script.Wait(0);
					FlashesToComplete = 4;
					FlashesDone = 0;
					ChangeColorTimer = Game.GameTime + 25;
					JustClickedButton = true;
					ButtonsPressed++;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
					ButtonAt++;
				}
			}
			if (ButtonAt <= 2 && Buttons.Count >= 3)
			{
				int num3 = (int)Buttons[SetsComplete].ControlSet[2];
				scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, num3, 0), "");
				if (ButtonAt == 2 && Game.IsControlJustPressed((Control)num3))
				{
					Script.Wait(0);
					FlashesToComplete = 4;
					FlashesDone = 0;
					ChangeColorTimer = Game.GameTime + 25;
					JustClickedButton = true;
					ButtonsPressed++;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
					ButtonAt++;
				}
			}
			if (ButtonAt <= 3 && Buttons.Count >= 4)
			{
				int num4 = (int)Buttons[SetsComplete].ControlSet[3];
				scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, num4, 0), "");
				if (ButtonAt == 3 && Game.IsControlJustPressed((Control)num4))
				{
					Script.Wait(0);
					FlashesToComplete = 4;
					FlashesDone = 0;
					ChangeColorTimer = Game.GameTime + 25;
					JustClickedButton = true;
					ButtonsPressed++;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
					ButtonAt++;
				}
			}
			if (ButtonAt <= 4 && Buttons.Count >= 5)
			{
				int num5 = (int)Buttons[SetsComplete].ControlSet[4];
				scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, num5, 0), "");
				if (ButtonAt == 4 && Game.IsControlJustPressed((Control)num5))
				{
					Script.Wait(0);
					FlashesToComplete = 4;
					FlashesDone = 0;
					ChangeColorTimer = Game.GameTime + 25;
					JustClickedButton = true;
					ButtonsPressed++;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
					ButtonAt++;
				}
			}
			if (ButtonAt <= 5 && Buttons.Count >= 6)
			{
				int num6 = (int)Buttons[SetsComplete].ControlSet[5];
				scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, num6, 0), "");
				if (ButtonAt == 5 && Game.IsControlJustPressed((Control)num6))
				{
					Notification.Show("Index " + num6);
					Script.Wait(0);
					FlashesToComplete = 4;
					FlashesDone = 0;
					ChangeColorTimer = Game.GameTime + 25;
					JustClickedButton = true;
					ButtonsPressed++;
					Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
					ButtonAt++;
				}
			}
			if (ButtonAt >= 5)
			{
				SetsComplete++;
				ButtonAt = 0;
			}
			if (ButtonsPressed >= ButtonsToComplete)
			{
				ButtonPressMinigameCompleted = true;
				Game.Player.IsInvincible = false;
				Game.Player.CanControlCharacter = true;
				Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Target_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
				PlayingButtonPressMinigame = false;
				SetsComplete = 0;
				ButtonAt = 0;
				ButtonsPressed = 0;
				HackFinishedID = 1;
			}
			scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", 0);
			float num7 = 0.45f;
			float num8 = 1.77777767f / Function.Call<float>(Hash.GET_ASPECT_RATIO, false);
			if (num8 > 1f)
			{
				num7 += 0.06f * (1f - 1f / num8);
			}
			else if (num8 < 1f)
			{
				num7 -= 0.06f * (1f - num8);
				num7 *= 1f / num8;
			}
			float num11 = 1f;
			Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, 66, 66);
			Function.Call(Hash.SET_SCRIPT_GFX_ALIGN_PARAMS, 0f, 0f, 0f, 0f);
			scaleform.CallFunction("OVERRIDE_POSITION", 0.6f, 0.5f, false);
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 7);
			Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
			Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE, scaleform.Handle, 0.119f, 0.04f, 1f, 1f, 255, 255, 255, 200, 0);
			Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
			Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);
			float height = 0.018f;
			drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.43f, 0.165f, height, 0, 0, 0, 255);
			drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.435f, 0.165f, height, 0, 0, 0, 255);
			drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.44f, 0.165f, height, 0, 0, 0, 255);
			drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.445f, 0.165f, height, 0, 0, 0, 255);
			drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.45f, 0.165f, height, 0, 0, 0, 255);
			drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.455f, 0.165f, height, 0, 0, 0, 255);
			drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.46f, 0.165f, height, 0, 0, 0, 255);
			drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.465f, 0.165f, height, 0, 0, 0, 255);
			float num9 = (float)ButtonsPressed / (float)ButtonsToComplete;
			float width = 0.165f * num9;
			float num10 = 0.08f * num9;
			float screenX = 0.42f + num10;
			if (JustClickedButton)
			{
				if (Game.GameTime >= ChangeColorTimer)
				{
					FlashesDone++;
					ChangeColorTimer = Game.GameTime + 25;
				}
				if (FlashesDone == 0)
				{
					drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.47f, 0.165f, height, 60, 60, 60, 255);
					drawSprite2("timerbars", "damagebarfill_128", screenX, 0.47f, width, height, 100, 255, 100, 255);
				}
				if (FlashesDone == 1)
				{
					drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.47f, 0.165f, height, 60, 120, 60, 255);
					drawSprite2("timerbars", "damagebarfill_128", screenX, 0.47f, width, height, 180, 255, 180, 255);
				}
				if (FlashesDone == 2)
				{
					drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.47f, 0.165f, height, 60, 60, 60, 255);
					drawSprite2("timerbars", "damagebarfill_128", screenX, 0.47f, width, height, 100, 255, 100, 255);
				}
				if (FlashesDone == 3)
				{
					drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.47f, 0.165f, height, 60, 120, 60, 255);
					drawSprite2("timerbars", "damagebarfill_128", screenX, 0.47f, width, height, 180, 255, 180, 255);
				}
				if (FlashesDone == 4)
				{
					drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.47f, 0.165f, height, 60, 60, 60, 255);
					drawSprite2("timerbars", "damagebarfill_128", screenX, 0.47f, width, height, 100, 255, 100, 255);
				}
				if (FlashesDone == 5)
				{
					FlashesDone = 0;
					JustClickedButton = false;
				}
			}
			else
			{
				drawSprite2("timerbars", "damagebarfill_128", 0.5f, 0.47f, 0.165f, height, 60, 60, 60, 255);
				drawSprite2("timerbars", "damagebarfill_128", screenX, 0.47f, width, height, 225, 225, 225, 255);
			}
		}

		public static bool OnPhoneHomeScreen()
		{
			bool result = false;
			if (Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, _mScriptHash) > 0 && Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, appcontactsHash) == 0 && Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, appsettingsHash) == 0 && Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, appemailHash) == 0 && Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, appCameraHash) == 0 && Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, appInternetHash) == 0 && Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, appTextsHash) == 0 && Function.Call<int>(Hash.GET_NUMBER_OF_THREADS_RUNNING_THE_SCRIPT_WITH_THIS_HASH, appCheckListHash) == 0)
			{
				result = true;
			}
			return result;
		}

		private void func_5758()
		{
			fLocal_207 = Function.Call<float>(Hash.GET_ASPECT_RATIO, false);
			if (fLocal_207 >= 4f)
			{
				fLocal_207 /= 3f;
			}
		}

		private float func_5705(float fParam0)
		{
			fParam0 = 0.5f - (0.5f - fParam0) / fLocal_207;
			return fParam0;
		}

		public static float GetAlignment(int Align)
		{
			float result = 0.5f;
			if (Align == 0)
			{
				result = 0.44f;
			}
			if (Align == 1)
			{
				result = 0.5f;
			}
			if (Align == 2)
			{
				result = 0.56f;
			}
			return result;
		}

		public static float GetSelectionX(int Selection)
		{
			float result = 0.03f;
			if (Selection == 0)
			{
				result = 0.03f;
			}
			if (Selection == 1)
			{
				result = 0.165f;
			}
			if (Selection == 2)
			{
				result = 0.3f;
			}
			if (Selection == 3)
			{
				result = 0.435f;
			}
			if (Selection == 4)
			{
				result = 0.57f;
			}
			if (Selection == 5)
			{
				result = 0.705f;
			}
			if (Selection == 6)
			{
				result = 0.84f;
			}
			if (Selection == 7)
			{
				result = 0.975f;
			}
			return result;
		}

		public static float GetSelectionY(int Selection)
		{
			float result = 0f;
			if (Selection == 0)
			{
				result = GetAlignment(PCBHackBar1_Alignment);
			}
			if (Selection == 1)
			{
				result = GetAlignment(PCBHackBar2_Alignment);
			}
			if (Selection == 2)
			{
				result = GetAlignment(PCBHackBar3_Alignment);
			}
			if (Selection == 3)
			{
				result = GetAlignment(PCBHackBar4_Alignment);
			}
			if (Selection == 4)
			{
				result = GetAlignment(PCBHackBar5_Alignment);
			}
			if (Selection == 5)
			{
				result = GetAlignment(PCBHackBar6_Alignment);
			}
			if (Selection == 6)
			{
				result = GetAlignment(PCBHackBar7_Alignment);
			}
			if (Selection == 7)
			{
				result = GetAlignment(PCBHackBar8_Alignment);
			}
			return result;
		}

		public static string Shuffle(string str)
		{
			Random random = new Random();
			SortedList<int, char> sortedList = new SortedList<int, char>();
			foreach (char value in str)
			{
				sortedList.Add(random.Next(), value);
			}
			return new string(sortedList.Values.ToArray());
		}

		public static void RunPasscodeHack()
		{
			PCBHHackStage = 0;
			PCBHackOn = false;
			FiveLetterPasscodeStage = 0;
			FiveLetterPasscodeHack = !FiveLetterPasscodeHack;
		}

		public static void RunPCBHack()
		{
			PCBHHackStage = 0;
			PCBHackOn = !PCBHackOn;
			FiveLetterPasscodeStage = 0;
			FiveLetterPasscodeHack = false;
		}

		public static float APPLY_ASPECT_MOD_TO_X(float X)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			return X * num;
		}

		public static float APPLY_ASPECT_MOD_TO_X_2(float fX)
		{
			float num = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			fX = cfSCREEN_CENTER - (cfSCREEN_CENTER - fX) / num;
			return fX;
		}

		private void drawSpriteARX(string textureDict, string textureName, float screenX, float screenY, float width, float height, float rot, int r, int g, int b, int alpha)
		{
			screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
			float num = (float)(1.778 / (double)screenaspectratio);
			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureDict, 0);
			float num2 = width * 1920f;
			float num3 = height * 1920f;
			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureDict))
			{
				Function.Call(Hash.DRAW_SPRITE_ARX, textureDict, textureName, GetRatio(screenX), screenY, num2 / 1920f * num, num3 / 1920f, rot, r, g, b, alpha, 0, 0);
			}
		}

		public unsafe static float GET_CUSTOM_SCREEN_ASPECT_RATIO()
		{
			int value;
			int value2;
			if (GetIsHidef())
			{
				value = 1280;
				value2 = 720;
			}
			else
			{
				value = 720;
				value2 = 480;
			}
			Function.Call(Hash.GET_ACTUAL_SCREEN_RESOLUTION, &value, &value2);
			float num = ToFloat(value) / ToFloat(value2);
			if (IsPcVersion() && (double)num > 4.0)
			{
				num /= 3f;
			}
			return num;
		}

		public static float ToFloat(int value)
		{
			return Function.Call<float>(Hash.TO_FLOAT, value);
		}

		public static bool IsPcVersion()
		{
			return Function.Call<bool>(Hash.IS_PC_VERSION);
		}

		public static bool GetIsHidef()
		{
			return Function.Call<bool>(Hash.GET_IS_HIDEF);
		}

		public static void DrawSprite(string txtDict, string txtName, float screenX, float screenY, float width, float height, float rotation, int r, int g, int b, int a, bool doStereorize = false, bool useNearest = false)
		{
			Function.Call(Hash.DRAW_SPRITE, txtDict, txtName, screenX, screenY, width, height, rotation, r, g, b, a, doStereorize, useNearest);
		}

		public static float Vector2DDistSquared(Vector2 v0, Vector2 v1)
		{
			return Function.Call<float>(Hash.POW, v1.X - v0.X, 2.0) + Function.Call<float>(Hash.POW, v1.Y - v0.Y, 2.0);
		}

		public static float Vector2DDist(Vector2 v0, Vector2 v1)
		{
			return Function.Call<float>(Hash.SQRT, Function.Call<float>(Hash.POW, v1.X - v0.X, 2.0) + Function.Call<float>(Hash.POW, v1.Y - v0.Y, 2.0));
		}

		public static bool IsFloatInRange(float value, float min, float max)
		{
			if (value > min && value < max)
			{
				return true;
			}
			return false;
		}

		public static Vector2 GetLeftPerpendicularVector(Vector2 v)
		{
			Vector2 result = default(Vector2);
			result.X = v.Y;
			result.Y = 0f - v.X;
			return result;
		}

		public static float DotProductVector(Vector2 v, Vector2 w)
		{
			return v.X * w.X + v.Y * w.Y;
		}

		public static Vector2 SubtractVector2D(Vector2 v0, Vector2 v1)
		{
			Vector2 result = default(Vector2);
			result.X = v0.X - v1.X;
			result.Y = v0.Y - v1.Y;
			return result;
		}

		public static Vector2 AddVector2D(Vector2 v0, Vector2 v1)
		{
			Vector2 result = default(Vector2);
			result.X = v0.X + v1.X;
			result.Y = v0.Y + v1.Y;
			return result;
		}

		public static Vector2 MultiplyVector2D(Vector2 v, float fScaler)
		{
			Vector2 result = default(Vector2);
			result.X = v.X * fScaler;
			result.Y = v.Y * fScaler;
			return result;
		}

		public static Vector2 DivideVector2D(Vector2 v, float fScaler)
		{
			Vector2 result = default(Vector2);
			result.X = v.X / fScaler;
			result.Y = v.Y / fScaler;
			return result;
		}

		public static float DotProductVector2D(Vector2 v, Vector2 w)
		{
			return v.X * w.X + v.Y * w.Y;
		}

		public static Vector2 NormalizeVector2D(Vector2 v)
		{
			float num = Vector2DMagnitude(v);
			if (num != 0f)
			{
				float fScaler = 1f / num;
				return MultiplyVector2D(v, fScaler);
			}
			Vector2 result = default(Vector2);
			result.X = 0f;
			result.Y = 0f;
			return result;
		}

		public static Vector2 GetLeftPerpendicularVector2D(Vector2 v)
		{
			Vector2 result = default(Vector2);
			result.X = v.Y;
			result.Y = 0f - v.X;
			return result;
		}

		public static bool IsBeamIntersectingEdge(Vector2 beamOrigin, Vector2 beamDirection, Vector2 v0, Vector2 v1, ref float distance)
		{
			Vector2 leftPerpendicularVector = GetLeftPerpendicularVector(SubtractVector2D(v1, v0));
			float num = DotProductVector(beamDirection, leftPerpendicularVector);
			if (IsFloatInRange(num, 0f - FloatTolerance, FloatTolerance))
			{
				distance = 1.25f;
				return false;
			}
			Vector2 w = SubtractVector2D(v0, beamOrigin);
			distance = DotProductVector(leftPerpendicularVector, w) / num;
			float num2 = DotProductVector(GetLeftPerpendicularVector(beamDirection), w) / num;
			if (distance > SameWallHitTolerance && distance <= 1.25f && num2 >= 0f)
			{
				return num2 <= 1f;
			}
			return false;
		}

		public static bool Intersects(Vector2 currentLocation, Vector2 requestedLocation, Boundary boundary, ref Vector2 collisionVector)
		{
			float num = (currentLocation.Y - boundary.Start.Y) * (boundary.End.X - boundary.Start.X) - (currentLocation.X - boundary.Start.X) * (boundary.End.Y - boundary.Start.Y);
			float num2 = (requestedLocation.X - currentLocation.X) * (boundary.End.Y - boundary.Start.Y) - (requestedLocation.Y - currentLocation.Y) * (boundary.End.X - boundary.Start.X);
			if (num2 == 0f)
			{
				return false;
			}
			float num3 = num / num2;
			num = (currentLocation.Y - boundary.Start.Y) * (requestedLocation.X - currentLocation.X) - (currentLocation.X - boundary.Start.X) * (requestedLocation.Y - currentLocation.Y);
			float num4 = num / num2;
			if (num3 < 0f || num3 > 1f || num4 < 0f || num4 > 1f)
			{
				return false;
			}
			collisionVector.X = currentLocation.X + (float)(int)(0.5f + num3 * (requestedLocation.X - currentLocation.X));
			collisionVector.Y = currentLocation.Y + (float)(int)(0.5f + num3 * (requestedLocation.Y - currentLocation.Y));
			return true;
		}

		public static bool DO_RECTANGLES_OVERLAP(float f_CentreX1, float f_CentreY1, float f_Width1, float f_Height1, float f_CentreX2, float f_CentreY2, float f_Width2, float f_Height2)
		{
			if ((double)f_CentreX1 + (double)f_Width1 * 0.5 >= (double)f_CentreX2 - (double)f_Width2 * 0.5 && (double)f_CentreX2 + (double)f_Width2 * 0.5 >= (double)f_CentreX1 - (double)f_Width1 * 0.5)
			{
				if ((double)f_CentreY1 + (double)f_Height1 * 0.5 >= (double)f_CentreY2 - (double)f_Height2 * 0.5)
				{
					return (double)f_CentreY2 + (double)f_Height2 * 0.5 >= (double)f_CentreY1 - (double)f_Height1 * 0.5;
				}
				return false;
			}
			return false;
		}

		public static Vector2 RotateVector2DAroundVector2D(Vector2 vRotateAround, Vector2 vToRotate, float fAngle)
		{
			float num = Function.Call<float>(Hash.SIN, fAngle);
			float num2 = Function.Call<float>(Hash.COS, fAngle);
			Vector2 result = default(Vector2);
			result.X = vToRotate.X - vRotateAround.X;
			result.Y = vToRotate.Y - vRotateAround.Y;
			float num3 = result.X * num2 - result.Y * num;
			float num4 = result.X * num + result.Y * num2;
			result.X = num3 + vRotateAround.X;
			result.Y = num4 + vRotateAround.Y;
			return result;
		}

		public static Vector3 GetVector2Offset(Vector3 Coord, float Heading, Vector3 Offset)
		{
			Vector3 vector = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_COORD_AND_HEADING_IN_WORLD_COORDS, Coord.X, Coord.Y, Coord.Z, Heading, Offset.X, Offset.Y, Offset.Z);
			return new Vector2(vector.X, vector.Y);
		}

		public static float Vector2DMagnitude(Vector2 v)
		{
			return (float)Math.Sqrt(Math.Pow(v.X, 2.0) + Math.Pow(v.Y, 2.0));
		}

		public static Vector2 NormaliseVector2D(Vector2 v)
		{
			float num = Vector2DMagnitude(v);
			if (num != 0f)
			{
				float fScaler = 1f / num;
				return MultiplyVector2(v, fScaler);
			}
			Vector2 result = default(Vector2);
			result.X = 0f;
			result.Y = 0f;
			return result;
		}

		public static Vector2 MultiplyVector2(Vector2 v, float fScaler)
		{
			Vector2 result = default(Vector2);
			result.X = v.X * fScaler;
			result.Y = v.Y * fScaler;
			return result;
		}

		public static int GetTimeDifference(int TimeA, int TimeB)
		{
			return TimeB - TimeA;
		}

		public static void GetDiagonalEdgeToCheck(int quadrant, ref int point0, ref int point1)
		{
			point0 = 0;
			point1 = 0;
			switch (quadrant)
			{
			case 1:
			case 3:
				point0 = 1;
				point1 = 3;
				break;
			case 2:
			case 4:
				point0 = 0;
				point1 = 2;
				break;
			}
		}

		public static void GetEdgeToCheck(int edge, int quadrant, ref int point0, ref int point1, bool invert = false)
		{
			point0 = 0;
			point1 = 0;
			if (invert)
			{
				switch (quadrant)
				{
				case 1:
					quadrant = 3;
					break;
				case 2:
					quadrant = 4;
					break;
				case 3:
					quadrant = 1;
					break;
				case 4:
					quadrant = 2;
					break;
				}
			}
			switch (quadrant)
			{
			case 1:
				switch (edge)
				{
				case 0:
					point0 = 0;
					point1 = 1;
					break;
				case 1:
					point0 = 3;
					point1 = 0;
					break;
				}
				break;
			case 2:
				switch (edge)
				{
				case 0:
					point0 = 2;
					point1 = 3;
					break;
				case 1:
					point0 = 3;
					point1 = 0;
					break;
				}
				break;
			case 3:
				switch (edge)
				{
				case 0:
					point0 = 1;
					point1 = 2;
					break;
				case 1:
					point0 = 2;
					point1 = 3;
					break;
				}
				break;
			case 4:
				switch (edge)
				{
				case 0:
					point0 = 0;
					point1 = 1;
					break;
				case 1:
					point0 = 1;
					point1 = 2;
					break;
				}
				break;
			}
		}

		public static int GetQuadrantFromVector(Vector2 v)
		{
			if (v.X > 0f - cfBH_FLOAT_TOLERANCE && (double)v.Y <= 0.0)
			{
				return 1;
			}
			if (v.X <= 0f - cfBH_FLOAT_TOLERANCE && (double)v.Y <= 0.0)
			{
				return 2;
			}
			if (v.X <= 0f - cfBH_FLOAT_TOLERANCE && (double)v.Y > 0.0)
			{
				return 3;
			}
			return 4;
		}

		public static bool WorthCheckingEdge(Vector2 vBeamOrigin, Vector2 v0, Vector2 v1, int iDirQuadrant)
		{
			switch (iDirQuadrant)
			{
			case 1:
			case 2:
				if (v0.Y > vBeamOrigin.Y && v1.Y > vBeamOrigin.Y)
				{
					return false;
				}
				break;
			case 3:
			case 4:
				if (v0.Y < vBeamOrigin.Y && v1.Y < vBeamOrigin.Y)
				{
					return false;
				}
				break;
			}
			switch (iDirQuadrant)
			{
			case 1:
			case 4:
				if (v0.X < vBeamOrigin.X && v1.X < vBeamOrigin.X)
				{
					return false;
				}
				break;
			case 2:
			case 3:
				if (v0.X > vBeamOrigin.X && v1.X > vBeamOrigin.X)
				{
					return false;
				}
				break;
			}
			return true;
		}

		public int GetBeamHackTextureIndexFromLevelIndex(int level)
		{
			switch (level)
			{
			case 0:
				return 0;
			case 1:
				return 1;
			case 2:
				return 2;
			case 3:
				return 3;
			case 4:
				return 12;
			case 5:
				return 5;
			case 6:
				return 6;
			case 7:
				return 7;
			case 8:
				return 8;
			case 9:
				return 9;
			case 10:
				return 10;
			case 11:
				return 11;
			case 12:
				return 13;
			case 13:
				return 14;
			default:
				return 0;
			}
		}

		public string GetBeamHackTextureDictForLevel(int level)
		{
			return "MPBeamHack_lvl" + GetBeamHackTextureIndexFromLevelIndex(level);
		}

		public string GetBeamHackTextureBgForLevel(int level)
		{
			return string.Concat("lvl" + GetBeamHackTextureIndexFromLevelIndex(level), "_bg");
		}

		public string GetBeamHackTextureFgForLevel(int level)
		{
			return "lvl" + GetBeamHackTextureIndexFromLevelIndex(level);
		}

		public static void GetBeamHacklevelData(int iLevel)
		{
			switch (iLevel)
			{
			case 0:
				beamLevel.MaxNodes = 11;
				beamLevel.MaxRotatingNodes = 5;
				beamLevel.MaxPackets = 9;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 13;
				beamLevel.MaxProxies = 0;
				vBeam[0].Origin = new Vector2(0.725f, 0.7325f);
				vBeam[1].Origin = new Vector2(0.987499f, 0.7325f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(1.025f, 0.845f);
				beamLevel.InnerWalls[0][1] = new Vector2(1.025f, 0.17f);
				beamLevel.InnerWalls[0][2] = new Vector2(1.1f, 0.17f);
				beamLevel.InnerWalls[0][3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.65f, 0.845f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.65f, 0.77f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.95f, 0.77f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.95f, 0.845f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.5f, 0.77f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.5f, 0.695f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.725f, 0.695f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.725f, 0.77f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.875f, 0.695f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.875f, 0.245f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.95f, 0.245f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.95f, 0.695f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.425f, 0.32f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.425f, 0.17f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.5f, 0.17f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.5f, 0.32f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.575f, 0.32f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.575f, 0.245f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.875f, 0.245f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.875f, 0.32f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.575f, 0.62f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.575f, 0.395f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.725f, 0.395f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.725f, 0.62f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.8f, 0.695f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.8f, 0.32f);
				beamLevel.InnerWalls[7][2] = new Vector2(0.875f, 0.32f);
				beamLevel.InnerWalls[7][3] = new Vector2(0.875f, 0.695f);
				beamLevel.InnerWalls[8][0] = new Vector2(-0.1f, 0.545f);
				beamLevel.InnerWalls[8][1] = new Vector2(-0.1f, 0.395f);
				beamLevel.InnerWalls[8][2] = new Vector2(-0.025f, 0.395f);
				beamLevel.InnerWalls[8][3] = new Vector2(-0.025f, 0.545f);
				beamLevel.InnerWalls[9][0] = new Vector2(0.35f, 0.77f);
				beamLevel.InnerWalls[9][1] = new Vector2(0.35f, 0.395f);
				beamLevel.InnerWalls[9][2] = new Vector2(0.5f, 0.395f);
				beamLevel.InnerWalls[9][3] = new Vector2(0.5f, 0.77f);
				beamLevel.InnerWalls[10][0] = new Vector2(0.2f, 0.545f);
				beamLevel.InnerWalls[10][1] = new Vector2(0.2f, 0.47f);
				beamLevel.InnerWalls[10][2] = new Vector2(0.35f, 0.47f);
				beamLevel.InnerWalls[10][3] = new Vector2(0.35f, 0.545f);
				beamLevel.InnerWalls[11][0] = new Vector2(0.125f, 0.62f);
				beamLevel.InnerWalls[11][1] = new Vector2(0.125f, 0.395f);
				beamLevel.InnerWalls[11][2] = new Vector2(0.2f, 0.395f);
				beamLevel.InnerWalls[11][3] = new Vector2(0.2f, 0.62f);
				beamLevel.InnerWalls[12][0] = new Vector2(0.125f, 0.245f);
				beamLevel.InnerWalls[12][1] = new Vector2(0.125f, 0.17f);
				beamLevel.InnerWalls[12][2] = new Vector2(0.425f, 0.17f);
				beamLevel.InnerWalls[12][3] = new Vector2(0.425f, 0.245f);
				beamLevel.InnerWalls[13][0] = new Vector2(0.35f, 0.32f);
				beamLevel.InnerWalls[13][1] = new Vector2(0.35f, 0.245f);
				beamLevel.InnerWalls[13][2] = new Vector2(0.425f, 0.245f);
				beamLevel.InnerWalls[13][3] = new Vector2(0.425f, 0.32f);
				beamLevel.Nodes[0][0] = new Vector2(0.960983f, 0.705984f);
				beamLevel.Nodes[0][1] = new Vector2(1.014017f, 0.759017f);
				beamLevel.Nodes[1][0] = new Vector2(0.510984f, 0.180983f);
				beamLevel.Nodes[1][1] = new Vector2(0.564016f, 0.234018f);
				beamLevel.Nodes[2][0] = new Vector2(0.7625f, 0.32f);
				beamLevel.Nodes[2][1] = new Vector2(0.7625f, 0.395f);
				beamLevel.Nodes[3][0] = new Vector2(0.414017f, 0.384017f);
				beamLevel.Nodes[3][1] = new Vector2(0.360983f, 0.330984f);
				beamLevel.Nodes[4][0] = new Vector2(0.060983f, 0.834017f);
				beamLevel.Nodes[4][1] = new Vector2(0.114017f, 0.780984f);
				beamLevel.Nodes[5][0] = new Vector2(0.95f, 0.17f);
				beamLevel.Nodes[5][1] = new Vector2(1.025f, 0.245f);
				beamLevel.Nodes[6][0] = new Vector2(0.575f, 0.695f);
				beamLevel.Nodes[6][1] = new Vector2(0.5f, 0.62f);
				beamLevel.Nodes[7][0] = new Vector2(0.725f, 0.695f);
				beamLevel.Nodes[7][1] = new Vector2(0.8f, 0.62f);
				beamLevel.Nodes[8][0] = new Vector2(-0.025f, 0.17f);
				beamLevel.Nodes[8][1] = new Vector2(-0.1f, 0.245f);
				beamLevel.Nodes[9][0] = new Vector2(0.05f, 0.17f);
				beamLevel.Nodes[9][1] = new Vector2(0.125f, 0.245f);
				beamLevel.Nodes[10][0] = new Vector2(-0.025f, 0.395f);
				beamLevel.Nodes[10][1] = new Vector2(-0.1f, 0.32f);
				beamLevel.Packets[0][0] = new Vector2(0.960983f, 0.309017f);
				beamLevel.Packets[0][1] = new Vector2(0.960983f, 0.255983f);
				beamLevel.Packets[0][2] = new Vector2(1.014017f, 0.255983f);
				beamLevel.Packets[0][3] = new Vector2(1.014017f, 0.309017f);
				beamLevel.Packets[1][0] = new Vector2(0.660983f, 0.234017f);
				beamLevel.Packets[1][1] = new Vector2(0.660983f, 0.180983f);
				beamLevel.Packets[1][2] = new Vector2(0.714017f, 0.180983f);
				beamLevel.Packets[1][3] = new Vector2(0.714017f, 0.234017f);
				beamLevel.Packets[2][0] = new Vector2(0.735983f, 0.459017f);
				beamLevel.Packets[2][1] = new Vector2(0.735983f, 0.405983f);
				beamLevel.Packets[2][2] = new Vector2(0.789017f, 0.405983f);
				beamLevel.Packets[2][3] = new Vector2(0.789017f, 0.459017f);
				beamLevel.Packets[3][0] = new Vector2(0.623483f, 0.684017f);
				beamLevel.Packets[3][1] = new Vector2(0.623483f, 0.630983f);
				beamLevel.Packets[3][2] = new Vector2(0.676517f, 0.630983f);
				beamLevel.Packets[3][3] = new Vector2(0.676517f, 0.684017f);
				beamLevel.Packets[4][0] = new Vector2(0.510983f, 0.459017f);
				beamLevel.Packets[4][1] = new Vector2(0.510983f, 0.405983f);
				beamLevel.Packets[4][2] = new Vector2(0.564017f, 0.405983f);
				beamLevel.Packets[4][3] = new Vector2(0.564017f, 0.459017f);
				beamLevel.Packets[5][0] = new Vector2(0.285983f, 0.459017f);
				beamLevel.Packets[5][1] = new Vector2(0.285983f, 0.405983f);
				beamLevel.Packets[5][2] = new Vector2(0.339017f, 0.405983f);
				beamLevel.Packets[5][3] = new Vector2(0.339017f, 0.459017f);
				beamLevel.Packets[6][0] = new Vector2(0.585983f, 0.834017f);
				beamLevel.Packets[6][1] = new Vector2(0.585983f, 0.780983f);
				beamLevel.Packets[6][2] = new Vector2(0.639017f, 0.780983f);
				beamLevel.Packets[6][3] = new Vector2(0.639017f, 0.834017f);
				beamLevel.Packets[7][0] = new Vector2(0.210983f, 0.459017f);
				beamLevel.Packets[7][1] = new Vector2(0.210983f, 0.405983f);
				beamLevel.Packets[7][2] = new Vector2(0.264017f, 0.405983f);
				beamLevel.Packets[7][3] = new Vector2(0.264017f, 0.459017f);
				beamLevel.Packets[8][0] = new Vector2(-0.089017f, 0.609017f);
				beamLevel.Packets[8][1] = new Vector2(-0.089017f, 0.555983f);
				beamLevel.Packets[8][2] = new Vector2(-0.035983f, 0.555983f);
				beamLevel.Packets[8][3] = new Vector2(-0.035983f, 0.609017f);
				beamLevel.Firewalls[0][0] = new Vector2(0.285983f, 0.309017f);
				beamLevel.Firewalls[0][1] = new Vector2(0.285983f, 0.255983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.339017f, 0.255983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.339017f, 0.309017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.210983f, 0.309017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.210983f, 0.255983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.264017f, 0.255983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.264017f, 0.309017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.135983f, 0.684017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.135983f, 0.630983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.189017f, 0.630983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.189017f, 0.684017f);
				break;
			case 1:
				beamLevel.MaxNodes = 6;
				beamLevel.MaxRotatingNodes = 6;
				beamLevel.MaxPackets = 5;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 7;
				beamLevel.MaxProxies = 0;
				vBeam[0].Origin = new Vector2(0.875f, 0.2825f);
				vBeam[1].Origin = new Vector2(0.012501f, 0.2825f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(1.025f, 0.545f);
				beamLevel.InnerWalls[0][1] = new Vector2(1.025f, 0.17f);
				beamLevel.InnerWalls[0][2] = new Vector2(1.1f, 0.17f);
				beamLevel.InnerWalls[0][3] = new Vector2(1.1f, 0.545f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.875f, 0.47f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.875f, 0.17f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.95f, 0.17f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.95f, 0.47f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.65f, 0.845f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.65f, 0.695f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.725f, 0.695f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.725f, 0.845f);
				beamLevel.InnerWalls[3][0] = new Vector2(-0.1f, 0.545f);
				beamLevel.InnerWalls[3][1] = new Vector2(-0.1f, 0.47f);
				beamLevel.InnerWalls[3][2] = new Vector2(-0.025f, 0.47f);
				beamLevel.InnerWalls[3][3] = new Vector2(-0.025f, 0.545f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.125f, 0.62f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.125f, 0.395f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.2f, 0.395f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.2f, 0.62f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.425f, 0.245f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.425f, 0.17f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.5f, 0.17f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.5f, 0.245f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.575f, 0.845f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.575f, 0.77f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.65f, 0.77f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.65f, 0.845f);
				beamLevel.Nodes[0][0] = new Vector2(-0.014017f, 0.309017f);
				beamLevel.Nodes[0][1] = new Vector2(0.039017f, 0.255984f);
				beamLevel.Nodes[1][0] = new Vector2(-0.014017f, 0.834017f);
				beamLevel.Nodes[1][1] = new Vector2(0.039017f, 0.780984f);
				beamLevel.Nodes[2][0] = new Vector2(1.014017f, 0.834017f);
				beamLevel.Nodes[2][1] = new Vector2(0.960983f, 0.780984f);
				beamLevel.Nodes[3][0] = new Vector2(0.513383f, 0.836217f);
				beamLevel.Nodes[3][1] = new Vector2(0.561617f, 0.778784f);
				beamLevel.Nodes[4][0] = new Vector2(0.537502f, 0.470001f);
				beamLevel.Nodes[4][1] = new Vector2(0.537498f, 0.395f);
				beamLevel.Packets[0][0] = new Vector2(-0.089017f, 0.609017f);
				beamLevel.Packets[0][1] = new Vector2(-0.089017f, 0.555983f);
				beamLevel.Packets[0][2] = new Vector2(-0.035983f, 0.555983f);
				beamLevel.Packets[0][3] = new Vector2(-0.035983f, 0.609017f);
				beamLevel.Packets[1][0] = new Vector2(0.510983f, 0.234017f);
				beamLevel.Packets[1][1] = new Vector2(0.510983f, 0.180983f);
				beamLevel.Packets[1][2] = new Vector2(0.564017f, 0.180983f);
				beamLevel.Packets[1][3] = new Vector2(0.564017f, 0.234017f);
				beamLevel.Packets[2][0] = new Vector2(0.810983f, 0.384017f);
				beamLevel.Packets[2][1] = new Vector2(0.810983f, 0.330983f);
				beamLevel.Packets[2][2] = new Vector2(0.864017f, 0.330983f);
				beamLevel.Packets[2][3] = new Vector2(0.864017f, 0.384017f);
				beamLevel.Packets[3][0] = new Vector2(0.735983f, 0.834017f);
				beamLevel.Packets[3][1] = new Vector2(0.735983f, 0.780983f);
				beamLevel.Packets[3][2] = new Vector2(0.789017f, 0.780983f);
				beamLevel.Packets[3][3] = new Vector2(0.789017f, 0.834017f);
				beamLevel.Packets[4][0] = new Vector2(0.960983f, 0.234017f);
				beamLevel.Packets[4][1] = new Vector2(0.960983f, 0.180983f);
				beamLevel.Packets[4][2] = new Vector2(1.014017f, 0.180983f);
				beamLevel.Packets[4][3] = new Vector2(1.014017f, 0.234017f);
				beamLevel.Firewalls[0][0] = new Vector2(-0.089017f, 0.459017f);
				beamLevel.Firewalls[0][1] = new Vector2(-0.089017f, 0.405983f);
				beamLevel.Firewalls[0][2] = new Vector2(-0.035983f, 0.405983f);
				beamLevel.Firewalls[0][3] = new Vector2(-0.035983f, 0.459017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.135983f, 0.234017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.135983f, 0.180983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.189017f, 0.180983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.189017f, 0.234017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.135983f, 0.684017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.135983f, 0.630983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.189017f, 0.630983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.189017f, 0.684017f);
				break;
			case 2:
				beamLevel.MaxNodes = 12;
				beamLevel.MaxRotatingNodes = 9;
				beamLevel.MaxPackets = 8;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 4;
				beamLevel.MaxProxies = 1;
				vBeam[0].Origin = new Vector2(0.35f, 0.5075f);
				vBeam[1].Origin = new Vector2(0.6125f, 0.5075f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(0.275f, 0.77f);
				beamLevel.InnerWalls[0][1] = new Vector2(0.275f, 0.245f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.35f, 0.245f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.35f, 0.77f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.575f, 0.695f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.575f, 0.62f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.725f, 0.62f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.725f, 0.695f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.65f, 0.62f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.65f, 0.395f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.725f, 0.395f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.725f, 0.62f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.575f, 0.395f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.575f, 0.32f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.725f, 0.32f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.725f, 0.395f);
				beamLevel.Nodes[0][0] = new Vector2(0.584852f, 0.532835f);
				beamLevel.Nodes[0][1] = new Vector2(0.640148f, 0.482166f);
				beamLevel.Nodes[1][0] = new Vector2(-0.089017f, 0.234017f);
				beamLevel.Nodes[1][1] = new Vector2(-0.035983f, 0.180984f);
				beamLevel.Nodes[2][0] = new Vector2(0.359852f, 0.307835f);
				beamLevel.Nodes[2][1] = new Vector2(0.415148f, 0.257166f);
				beamLevel.Nodes[3][0] = new Vector2(0.364671f, 0.702749f);
				beamLevel.Nodes[3][1] = new Vector2(0.410329f, 0.762251f);
				beamLevel.Nodes[4][0] = new Vector2(-0.0625f, 0.695001f);
				beamLevel.Nodes[4][1] = new Vector2(-0.0625f, 0.62f);
				beamLevel.Nodes[5][0] = new Vector2(0.7625f, 0.545001f);
				beamLevel.Nodes[5][1] = new Vector2(0.762501f, 0.47f);
				beamLevel.Nodes[6][0] = new Vector2(1.035983f, 0.534017f);
				beamLevel.Nodes[6][1] = new Vector2(1.089017f, 0.480984f);
				beamLevel.Nodes[7][0] = new Vector2(0.888396f, 0.253774f);
				beamLevel.Nodes[7][1] = new Vector2(0.936604f, 0.311227f);
				beamLevel.Nodes[8][0] = new Vector2(0.885983f, 0.759017f);
				beamLevel.Nodes[8][1] = new Vector2(0.939017f, 0.705984f);
				beamLevel.Nodes[9][0] = new Vector2(-0.025f, 0.845f);
				beamLevel.Nodes[9][1] = new Vector2(-0.1f, 0.77f);
				beamLevel.Nodes[10][0] = new Vector2(1.025f, 0.845f);
				beamLevel.Nodes[10][1] = new Vector2(1.1f, 0.77f);
				beamLevel.Nodes[11][0] = new Vector2(1.025f, 0.17f);
				beamLevel.Nodes[11][1] = new Vector2(1.1f, 0.245f);
				beamLevel.Packets[0][0] = new Vector2(0.810983f, 0.609017f);
				beamLevel.Packets[0][1] = new Vector2(0.810983f, 0.555983f);
				beamLevel.Packets[0][2] = new Vector2(0.864017f, 0.555983f);
				beamLevel.Packets[0][3] = new Vector2(0.864017f, 0.609017f);
				beamLevel.Packets[1][0] = new Vector2(0.210983f, 0.309017f);
				beamLevel.Packets[1][1] = new Vector2(0.210983f, 0.255983f);
				beamLevel.Packets[1][2] = new Vector2(0.264017f, 0.255983f);
				beamLevel.Packets[1][3] = new Vector2(0.264017f, 0.309017f);
				beamLevel.Packets[2][0] = new Vector2(0.510983f, 0.834017f);
				beamLevel.Packets[2][1] = new Vector2(0.510983f, 0.780983f);
				beamLevel.Packets[2][2] = new Vector2(0.564017f, 0.780983f);
				beamLevel.Packets[2][3] = new Vector2(0.564017f, 0.834017f);
				beamLevel.Packets[3][0] = new Vector2(0.510983f, 0.234017f);
				beamLevel.Packets[3][1] = new Vector2(0.510983f, 0.180983f);
				beamLevel.Packets[3][2] = new Vector2(0.564017f, 0.180983f);
				beamLevel.Packets[3][3] = new Vector2(0.564017f, 0.234017f);
				beamLevel.Packets[4][0] = new Vector2(0.210983f, 0.684017f);
				beamLevel.Packets[4][1] = new Vector2(0.210983f, 0.630983f);
				beamLevel.Packets[4][2] = new Vector2(0.264017f, 0.630983f);
				beamLevel.Packets[4][3] = new Vector2(0.264017f, 0.684017f);
				beamLevel.Packets[5][0] = new Vector2(0.810983f, 0.459017f);
				beamLevel.Packets[5][1] = new Vector2(0.810983f, 0.405983f);
				beamLevel.Packets[5][2] = new Vector2(0.864017f, 0.405983f);
				beamLevel.Packets[5][3] = new Vector2(0.864017f, 0.459017f);
				beamLevel.Packets[6][0] = new Vector2(1.035983f, 0.759017f);
				beamLevel.Packets[6][1] = new Vector2(1.035983f, 0.705983f);
				beamLevel.Packets[6][2] = new Vector2(1.089017f, 0.705983f);
				beamLevel.Packets[6][3] = new Vector2(1.089017f, 0.759017f);
				beamLevel.Packets[7][0] = new Vector2(1.035983f, 0.309017f);
				beamLevel.Packets[7][1] = new Vector2(1.035983f, 0.255983f);
				beamLevel.Packets[7][2] = new Vector2(1.089017f, 0.255983f);
				beamLevel.Packets[7][3] = new Vector2(1.089017f, 0.309017f);
				beamLevel.Firewalls[0][0] = new Vector2(-0.089017f, 0.534017f);
				beamLevel.Firewalls[0][1] = new Vector2(-0.089017f, 0.480983f);
				beamLevel.Firewalls[0][2] = new Vector2(-0.035983f, 0.480983f);
				beamLevel.Firewalls[0][3] = new Vector2(-0.035983f, 0.534017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.960983f, 0.309017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.960983f, 0.255983f);
				beamLevel.Firewalls[1][2] = new Vector2(1.014017f, 0.255983f);
				beamLevel.Firewalls[1][3] = new Vector2(1.014017f, 0.309017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.960983f, 0.759017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.960983f, 0.705983f);
				beamLevel.Firewalls[2][2] = new Vector2(1.014017f, 0.705983f);
				beamLevel.Firewalls[2][3] = new Vector2(1.014017f, 0.759017f);
				beamLevel.Proxies[0][0] = new Vector2(0.135983f, 0.534017f);
				beamLevel.Proxies[0][1] = new Vector2(0.135983f, 0.480984f);
				beamLevel.Proxies[0][2] = new Vector2(0.189017f, 0.480984f);
				beamLevel.Proxies[0][3] = new Vector2(0.189017f, 0.534017f);
				break;
			}
			switch (iLevel)
			{
			case 3:
				beamLevel.MaxNodes = 14;
				beamLevel.MaxRotatingNodes = 9;
				beamLevel.MaxPackets = 9;
				beamLevel.MaxFirewalls = 7;
				beamLevel.MaxInnerWalls = 11;
				beamLevel.MaxProxies = 0;
				vBeam[0].Origin = new Vector2(-0.1f, 0.2075f);
				vBeam[1].Origin = new Vector2(0.3125f, 0.2075f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(-0.1f, 0.32f);
				beamLevel.InnerWalls[0][1] = new Vector2(-0.1f, 0.245f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.125f, 0.245f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.125f, 0.32f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.125f, 0.695f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.125f, 0.245f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.2f, 0.245f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.2f, 0.695f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.2f, 0.47f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.2f, 0.395f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.35f, 0.395f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.35f, 0.47f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.35f, 0.47f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.35f, 0.245f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.425f, 0.245f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.425f, 0.47f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.425f, 0.47f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.425f, 0.245f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.5f, 0.245f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.5f, 0.47f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.5f, 0.47f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.5f, 0.395f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.725f, 0.395f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.725f, 0.47f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.725f, 0.47f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.725f, 0.245f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.8f, 0.245f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.8f, 0.47f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.8f, 0.32f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.8f, 0.245f);
				beamLevel.InnerWalls[7][2] = new Vector2(1.025f, 0.245f);
				beamLevel.InnerWalls[7][3] = new Vector2(1.025f, 0.32f);
				beamLevel.InnerWalls[8][0] = new Vector2(0.5f, 0.62f);
				beamLevel.InnerWalls[8][1] = new Vector2(0.5f, 0.545f);
				beamLevel.InnerWalls[8][2] = new Vector2(0.875f, 0.545f);
				beamLevel.InnerWalls[8][3] = new Vector2(0.875f, 0.62f);
				beamLevel.InnerWalls[9][0] = new Vector2(0.5f, 0.77f);
				beamLevel.InnerWalls[9][1] = new Vector2(0.5f, 0.62f);
				beamLevel.InnerWalls[9][2] = new Vector2(0.575f, 0.62f);
				beamLevel.InnerWalls[9][3] = new Vector2(0.575f, 0.77f);
				beamLevel.InnerWalls[10][0] = new Vector2(0.8f, 0.77f);
				beamLevel.InnerWalls[10][1] = new Vector2(0.8f, 0.62f);
				beamLevel.InnerWalls[10][2] = new Vector2(0.875f, 0.62f);
				beamLevel.InnerWalls[10][3] = new Vector2(0.875f, 0.77f);
				beamLevel.Nodes[0][0] = new Vector2(0.319012f, 0.244431f);
				beamLevel.Nodes[0][1] = new Vector2(0.305988f, 0.17057f);
				beamLevel.Nodes[1][0] = new Vector2(0.719976f, 0.226251f);
				beamLevel.Nodes[1][1] = new Vector2(0.655024f, 0.18875f);
				beamLevel.Nodes[2][0] = new Vector2(0.6125f, 0.845f);
				beamLevel.Nodes[2][1] = new Vector2(0.6125f, 0.770001f);
				beamLevel.Nodes[3][0] = new Vector2(-0.0625f, 0.845001f);
				beamLevel.Nodes[3][1] = new Vector2(-0.0625f, 0.77f);
				beamLevel.Nodes[4][0] = new Vector2(-0.0625f, 0.545001f);
				beamLevel.Nodes[4][1] = new Vector2(-0.0625f, 0.47f);
				beamLevel.Nodes[5][0] = new Vector2(0.285983f, 0.534017f);
				beamLevel.Nodes[5][1] = new Vector2(0.339017f, 0.480984f);
				beamLevel.Nodes[6][0] = new Vector2(0.435983f, 0.759017f);
				beamLevel.Nodes[6][1] = new Vector2(0.489017f, 0.705984f);
				beamLevel.Nodes[7][0] = new Vector2(0.810983f, 0.534017f);
				beamLevel.Nodes[7][1] = new Vector2(0.864017f, 0.480984f);
				beamLevel.Nodes[8][0] = new Vector2(0.953513f, 0.716652f);
				beamLevel.Nodes[8][1] = new Vector2(1.021487f, 0.748349f);
				beamLevel.Nodes[9][0] = new Vector2(0.5f, 0.47f);
				beamLevel.Nodes[9][1] = new Vector2(0.425f, 0.545f);
				beamLevel.Nodes[10][0] = new Vector2(0.875f, 0.32f);
				beamLevel.Nodes[10][1] = new Vector2(0.8f, 0.395f);
				beamLevel.Nodes[11][0] = new Vector2(0.95f, 0.32f);
				beamLevel.Nodes[11][1] = new Vector2(1.025f, 0.395f);
				beamLevel.Nodes[12][0] = new Vector2(1.025f, 0.17f);
				beamLevel.Nodes[12][1] = new Vector2(1.1f, 0.245f);
				beamLevel.Nodes[13][0] = new Vector2(1.025f, 0.845f);
				beamLevel.Nodes[13][1] = new Vector2(1.1f, 0.77f);
				beamLevel.Packets[0][0] = new Vector2(0.210983f, 0.384017f);
				beamLevel.Packets[0][1] = new Vector2(0.210983f, 0.330983f);
				beamLevel.Packets[0][2] = new Vector2(0.264017f, 0.330983f);
				beamLevel.Packets[0][3] = new Vector2(0.264017f, 0.384017f);
				beamLevel.Packets[1][0] = new Vector2(0.510983f, 0.384017f);
				beamLevel.Packets[1][1] = new Vector2(0.510983f, 0.330983f);
				beamLevel.Packets[1][2] = new Vector2(0.564017f, 0.330983f);
				beamLevel.Packets[1][3] = new Vector2(0.564017f, 0.384017f);
				beamLevel.Packets[2][0] = new Vector2(0.885983f, 0.384017f);
				beamLevel.Packets[2][1] = new Vector2(0.885983f, 0.330983f);
				beamLevel.Packets[2][2] = new Vector2(0.939017f, 0.330983f);
				beamLevel.Packets[2][3] = new Vector2(0.939017f, 0.384017f);
				beamLevel.Packets[3][0] = new Vector2(0.585983f, 0.684017f);
				beamLevel.Packets[3][1] = new Vector2(0.585983f, 0.630983f);
				beamLevel.Packets[3][2] = new Vector2(0.639017f, 0.630983f);
				beamLevel.Packets[3][3] = new Vector2(0.639017f, 0.684017f);
				beamLevel.Packets[4][0] = new Vector2(0.735983f, 0.684017f);
				beamLevel.Packets[4][1] = new Vector2(0.735983f, 0.630983f);
				beamLevel.Packets[4][2] = new Vector2(0.789017f, 0.630983f);
				beamLevel.Packets[4][3] = new Vector2(0.789017f, 0.684017f);
				beamLevel.Packets[5][0] = new Vector2(0.060983f, 0.384017f);
				beamLevel.Packets[5][1] = new Vector2(0.060983f, 0.330983f);
				beamLevel.Packets[5][2] = new Vector2(0.114017f, 0.330983f);
				beamLevel.Packets[5][3] = new Vector2(0.114017f, 0.384017f);
				beamLevel.Packets[6][0] = new Vector2(0.210983f, 0.684017f);
				beamLevel.Packets[6][1] = new Vector2(0.210983f, 0.630983f);
				beamLevel.Packets[6][2] = new Vector2(0.264017f, 0.630983f);
				beamLevel.Packets[6][3] = new Vector2(0.264017f, 0.684017f);
				beamLevel.Packets[7][0] = new Vector2(0.885983f, 0.609017f);
				beamLevel.Packets[7][1] = new Vector2(0.885983f, 0.555983f);
				beamLevel.Packets[7][2] = new Vector2(0.939017f, 0.555983f);
				beamLevel.Packets[7][3] = new Vector2(0.939017f, 0.609017f);
				beamLevel.Packets[8][0] = new Vector2(0.885983f, 0.759017f);
				beamLevel.Packets[8][1] = new Vector2(0.885983f, 0.705983f);
				beamLevel.Packets[8][2] = new Vector2(0.939017f, 0.705983f);
				beamLevel.Packets[8][3] = new Vector2(0.939017f, 0.759017f);
				beamLevel.Firewalls[0][0] = new Vector2(-0.014017f, 0.384017f);
				beamLevel.Firewalls[0][1] = new Vector2(-0.014017f, 0.330983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.039017f, 0.330983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.039017f, 0.384017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.060983f, 0.684017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.060983f, 0.630983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.114017f, 0.630983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.114017f, 0.684017f);
				beamLevel.Firewalls[2][0] = new Vector2(-0.014017f, 0.684017f);
				beamLevel.Firewalls[2][1] = new Vector2(-0.014017f, 0.630983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.039017f, 0.630983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.039017f, 0.684017f);
				beamLevel.Firewalls[3][0] = new Vector2(0.285983f, 0.684017f);
				beamLevel.Firewalls[3][1] = new Vector2(0.285983f, 0.630983f);
				beamLevel.Firewalls[3][2] = new Vector2(0.339017f, 0.630983f);
				beamLevel.Firewalls[3][3] = new Vector2(0.339017f, 0.684017f);
				beamLevel.Firewalls[4][0] = new Vector2(0.660983f, 0.684017f);
				beamLevel.Firewalls[4][1] = new Vector2(0.660983f, 0.630983f);
				beamLevel.Firewalls[4][2] = new Vector2(0.714017f, 0.630983f);
				beamLevel.Firewalls[4][3] = new Vector2(0.714017f, 0.684017f);
				beamLevel.Firewalls[5][0] = new Vector2(0.885983f, 0.534017f);
				beamLevel.Firewalls[5][1] = new Vector2(0.885983f, 0.480983f);
				beamLevel.Firewalls[5][2] = new Vector2(0.939017f, 0.480983f);
				beamLevel.Firewalls[5][3] = new Vector2(0.939017f, 0.534017f);
				beamLevel.Firewalls[6][0] = new Vector2(0.660983f, 0.384017f);
				beamLevel.Firewalls[6][1] = new Vector2(0.660983f, 0.330983f);
				beamLevel.Firewalls[6][2] = new Vector2(0.714017f, 0.330983f);
				beamLevel.Firewalls[6][3] = new Vector2(0.714017f, 0.384017f);
				break;
			case 4:
				beamLevel.MaxNodes = 9;
				beamLevel.MaxRotatingNodes = 9;
				beamLevel.MaxPackets = 8;
				beamLevel.MaxFirewalls = 6;
				beamLevel.MaxInnerWalls = 8;
				beamLevel.MaxProxies = 0;
				vBeam[0].Origin = new Vector2(-0.1f, 0.8075f);
				vBeam[1].Origin = new Vector2(-0.025f, 0.8075f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(-0.1f, 0.545f);
				beamLevel.InnerWalls[0][1] = new Vector2(-0.1f, 0.47f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.35f, 0.47f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.35f, 0.545f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.275f, 0.62f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.275f, 0.545f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.35f, 0.545f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.35f, 0.62f);
				beamLevel.InnerWalls[2][0] = new Vector2(-0.025f, 0.395f);
				beamLevel.InnerWalls[2][1] = new Vector2(-0.025f, 0.245f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.125f, 0.245f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.125f, 0.395f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.425f, 0.845f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.425f, 0.395f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.5f, 0.395f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.5f, 0.845f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.725f, 0.545f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.725f, 0.32f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.8f, 0.32f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.8f, 0.545f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.125f, 0.395f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.125f, 0.32f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.2f, 0.32f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.2f, 0.395f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.5f, 0.845f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.5f, 0.77f);
				beamLevel.InnerWalls[6][2] = new Vector2(1.1f, 0.77f);
				beamLevel.InnerWalls[6][3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.725f, 0.62f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.725f, 0.545f);
				beamLevel.InnerWalls[7][2] = new Vector2(0.875f, 0.545f);
				beamLevel.InnerWalls[7][3] = new Vector2(0.875f, 0.62f);
				beamLevel.Nodes[0][0] = new Vector2(0.135983f, 0.834017f);
				beamLevel.Nodes[0][1] = new Vector2(0.189017f, 0.780984f);
				beamLevel.Nodes[1][0] = new Vector2(0.060983f, 0.609017f);
				beamLevel.Nodes[1][1] = new Vector2(0.114017f, 0.555984f);
				beamLevel.Nodes[2][0] = new Vector2(0.414017f, 0.780984f);
				beamLevel.Nodes[2][1] = new Vector2(0.360983f, 0.834017f);
				beamLevel.Nodes[3][0] = new Vector2(-0.089017f, 0.459017f);
				beamLevel.Nodes[3][1] = new Vector2(-0.035983f, 0.405984f);
				beamLevel.Nodes[4][0] = new Vector2(-0.089017f, 0.234017f);
				beamLevel.Nodes[4][1] = new Vector2(-0.035983f, 0.180984f);
				beamLevel.Nodes[5][0] = new Vector2(0.414033f, 0.181001f);
				beamLevel.Nodes[5][1] = new Vector2(0.360967f, 0.234f);
				beamLevel.Nodes[6][0] = new Vector2(0.510983f, 0.459017f);
				beamLevel.Nodes[6][1] = new Vector2(0.564017f, 0.405984f);
				beamLevel.Nodes[7][0] = new Vector2(0.735983f, 0.234017f);
				beamLevel.Nodes[7][1] = new Vector2(0.789017f, 0.180984f);
				beamLevel.Nodes[8][0] = new Vector2(1.035983f, 0.759017f);
				beamLevel.Nodes[8][1] = new Vector2(1.089017f, 0.705984f);
				beamLevel.Packets[0][0] = new Vector2(0.360983f, 0.609017f);
				beamLevel.Packets[0][1] = new Vector2(0.360983f, 0.555983f);
				beamLevel.Packets[0][2] = new Vector2(0.414017f, 0.555983f);
				beamLevel.Packets[0][3] = new Vector2(0.414017f, 0.609017f);
				beamLevel.Packets[1][0] = new Vector2(0.135983f, 0.234017f);
				beamLevel.Packets[1][1] = new Vector2(0.135983f, 0.180983f);
				beamLevel.Packets[1][2] = new Vector2(0.189017f, 0.180983f);
				beamLevel.Packets[1][3] = new Vector2(0.189017f, 0.234017f);
				beamLevel.Packets[2][0] = new Vector2(0.510983f, 0.759017f);
				beamLevel.Packets[2][1] = new Vector2(0.510983f, 0.705983f);
				beamLevel.Packets[2][2] = new Vector2(0.564017f, 0.705983f);
				beamLevel.Packets[2][3] = new Vector2(0.564017f, 0.759017f);
				beamLevel.Packets[3][0] = new Vector2(0.735983f, 0.684017f);
				beamLevel.Packets[3][1] = new Vector2(0.735983f, 0.630983f);
				beamLevel.Packets[3][2] = new Vector2(0.789017f, 0.630983f);
				beamLevel.Packets[3][3] = new Vector2(0.789017f, 0.684017f);
				beamLevel.Packets[4][0] = new Vector2(0.960983f, 0.384017f);
				beamLevel.Packets[4][1] = new Vector2(0.960983f, 0.330983f);
				beamLevel.Packets[4][2] = new Vector2(1.014017f, 0.330983f);
				beamLevel.Packets[4][3] = new Vector2(1.014017f, 0.384017f);
				beamLevel.Packets[5][0] = new Vector2(0.023483f, 0.459017f);
				beamLevel.Packets[5][1] = new Vector2(0.023483f, 0.405983f);
				beamLevel.Packets[5][2] = new Vector2(0.076517f, 0.405983f);
				beamLevel.Packets[5][3] = new Vector2(0.076517f, 0.459017f);
				beamLevel.Packets[6][0] = new Vector2(-0.089017f, 0.609017f);
				beamLevel.Packets[6][1] = new Vector2(-0.089017f, 0.555983f);
				beamLevel.Packets[6][2] = new Vector2(-0.035983f, 0.555983f);
				beamLevel.Packets[6][3] = new Vector2(-0.035983f, 0.609017f);
				beamLevel.Packets[7][0] = new Vector2(0.885983f, 0.609017f);
				beamLevel.Packets[7][1] = new Vector2(0.885983f, 0.555983f);
				beamLevel.Packets[7][2] = new Vector2(0.939017f, 0.555983f);
				beamLevel.Packets[7][3] = new Vector2(0.939017f, 0.609017f);
				beamLevel.Firewalls[0][0] = new Vector2(-0.014017f, 0.684017f);
				beamLevel.Firewalls[0][1] = new Vector2(-0.014017f, 0.630983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.039017f, 0.630983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.039017f, 0.684017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.135983f, 0.309017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.135983f, 0.255983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.189017f, 0.255983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.189017f, 0.309017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.510983f, 0.684017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.510983f, 0.630983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.564017f, 0.630983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.564017f, 0.684017f);
				beamLevel.Firewalls[3][0] = new Vector2(0.810983f, 0.684017f);
				beamLevel.Firewalls[3][1] = new Vector2(0.810983f, 0.630983f);
				beamLevel.Firewalls[3][2] = new Vector2(0.864017f, 0.630983f);
				beamLevel.Firewalls[3][3] = new Vector2(0.864017f, 0.684017f);
				beamLevel.Firewalls[4][0] = new Vector2(0.435983f, 0.384017f);
				beamLevel.Firewalls[4][1] = new Vector2(0.435983f, 0.330983f);
				beamLevel.Firewalls[4][2] = new Vector2(0.489017f, 0.330983f);
				beamLevel.Firewalls[4][3] = new Vector2(0.489017f, 0.384017f);
				beamLevel.Firewalls[5][0] = new Vector2(1.035983f, 0.234017f);
				beamLevel.Firewalls[5][1] = new Vector2(1.035983f, 0.180983f);
				beamLevel.Firewalls[5][2] = new Vector2(1.089017f, 0.180983f);
				beamLevel.Firewalls[5][3] = new Vector2(1.089017f, 0.234017f);
				break;
			case 5:
				beamLevel.MaxNodes = 15;
				beamLevel.MaxRotatingNodes = 5;
				beamLevel.MaxPackets = 9;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 16;
				beamLevel.MaxProxies = 0;
				vBeam[0].Origin = new Vector2(0.725f, 0.7325f);
				vBeam[1].Origin = new Vector2(0.987499f, 0.7325f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(1.025f, 0.845f);
				beamLevel.InnerWalls[0][1] = new Vector2(1.025f, 0.17f);
				beamLevel.InnerWalls[0][2] = new Vector2(1.1f, 0.17f);
				beamLevel.InnerWalls[0][3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.65f, 0.845f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.65f, 0.77f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.95f, 0.77f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.95f, 0.845f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.425f, 0.77f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.425f, 0.695f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.725f, 0.695f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.725f, 0.77f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.875f, 0.695f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.875f, 0.245f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.95f, 0.245f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.95f, 0.695f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.425f, 0.47f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.425f, 0.395f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.5f, 0.395f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.5f, 0.47f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.425f, 0.32f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.425f, 0.17f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.5f, 0.17f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.5f, 0.32f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.575f, 0.32f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.575f, 0.245f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.875f, 0.245f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.875f, 0.32f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.575f, 0.47f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.575f, 0.395f);
				beamLevel.InnerWalls[7][2] = new Vector2(0.725f, 0.395f);
				beamLevel.InnerWalls[7][3] = new Vector2(0.725f, 0.47f);
				beamLevel.InnerWalls[8][0] = new Vector2(0.8f, 0.47f);
				beamLevel.InnerWalls[8][1] = new Vector2(0.8f, 0.395f);
				beamLevel.InnerWalls[8][2] = new Vector2(0.875f, 0.395f);
				beamLevel.InnerWalls[8][3] = new Vector2(0.875f, 0.47f);
				beamLevel.InnerWalls[9][0] = new Vector2(0.8f, 0.695f);
				beamLevel.InnerWalls[9][1] = new Vector2(0.8f, 0.62f);
				beamLevel.InnerWalls[9][2] = new Vector2(0.875f, 0.62f);
				beamLevel.InnerWalls[9][3] = new Vector2(0.875f, 0.695f);
				beamLevel.InnerWalls[10][0] = new Vector2(-0.1f, 0.545f);
				beamLevel.InnerWalls[10][1] = new Vector2(-0.1f, 0.395f);
				beamLevel.InnerWalls[10][2] = new Vector2(-0.025f, 0.395f);
				beamLevel.InnerWalls[10][3] = new Vector2(-0.025f, 0.545f);
				beamLevel.InnerWalls[11][0] = new Vector2(0.35f, 0.77f);
				beamLevel.InnerWalls[11][1] = new Vector2(0.35f, 0.395f);
				beamLevel.InnerWalls[11][2] = new Vector2(0.425f, 0.395f);
				beamLevel.InnerWalls[11][3] = new Vector2(0.425f, 0.77f);
				beamLevel.InnerWalls[12][0] = new Vector2(0.2f, 0.545f);
				beamLevel.InnerWalls[12][1] = new Vector2(0.2f, 0.47f);
				beamLevel.InnerWalls[12][2] = new Vector2(0.35f, 0.47f);
				beamLevel.InnerWalls[12][3] = new Vector2(0.35f, 0.545f);
				beamLevel.InnerWalls[13][0] = new Vector2(0.125f, 0.62f);
				beamLevel.InnerWalls[13][1] = new Vector2(0.125f, 0.395f);
				beamLevel.InnerWalls[13][2] = new Vector2(0.2f, 0.395f);
				beamLevel.InnerWalls[13][3] = new Vector2(0.2f, 0.62f);
				beamLevel.InnerWalls[14][0] = new Vector2(0.125f, 0.245f);
				beamLevel.InnerWalls[14][1] = new Vector2(0.125f, 0.17f);
				beamLevel.InnerWalls[14][2] = new Vector2(0.425f, 0.17f);
				beamLevel.InnerWalls[14][3] = new Vector2(0.425f, 0.245f);
				beamLevel.InnerWalls[15][0] = new Vector2(0.35f, 0.32f);
				beamLevel.InnerWalls[15][1] = new Vector2(0.35f, 0.245f);
				beamLevel.InnerWalls[15][2] = new Vector2(0.425f, 0.245f);
				beamLevel.InnerWalls[15][3] = new Vector2(0.425f, 0.32f);
				beamLevel.Nodes[0][0] = new Vector2(1.014017f, 0.759017f);
				beamLevel.Nodes[0][1] = new Vector2(0.960983f, 0.705984f);
				beamLevel.Nodes[1][0] = new Vector2(0.510983f, 0.180984f);
				beamLevel.Nodes[1][1] = new Vector2(0.564017f, 0.234017f);
				beamLevel.Nodes[2][0] = new Vector2(0.7625f, 0.395f);
				beamLevel.Nodes[2][1] = new Vector2(0.7625f, 0.32f);
				beamLevel.Nodes[3][0] = new Vector2(0.414017f, 0.384017f);
				beamLevel.Nodes[3][1] = new Vector2(0.360983f, 0.330984f);
				beamLevel.Nodes[4][0] = new Vector2(0.060983f, 0.834017f);
				beamLevel.Nodes[4][1] = new Vector2(0.114017f, 0.780984f);
				beamLevel.Nodes[5][0] = new Vector2(0.95f, 0.17f);
				beamLevel.Nodes[5][1] = new Vector2(1.025f, 0.245f);
				beamLevel.Nodes[6][0] = new Vector2(0.575f, 0.62f);
				beamLevel.Nodes[6][1] = new Vector2(0.5f, 0.545f);
				beamLevel.Nodes[7][0] = new Vector2(0.5f, 0.695f);
				beamLevel.Nodes[7][1] = new Vector2(0.425f, 0.62f);
				beamLevel.Nodes[8][0] = new Vector2(0.725f, 0.695f);
				beamLevel.Nodes[8][1] = new Vector2(0.8f, 0.62f);
				beamLevel.Nodes[9][0] = new Vector2(0.8f, 0.62f);
				beamLevel.Nodes[9][1] = new Vector2(0.875f, 0.545f);
				beamLevel.Nodes[10][0] = new Vector2(0.8f, 0.47f);
				beamLevel.Nodes[10][1] = new Vector2(0.875f, 0.545f);
				beamLevel.Nodes[11][0] = new Vector2(0.5f, 0.47f);
				beamLevel.Nodes[11][1] = new Vector2(0.425f, 0.545f);
				beamLevel.Nodes[12][0] = new Vector2(-0.025f, 0.17f);
				beamLevel.Nodes[12][1] = new Vector2(-0.1f, 0.245f);
				beamLevel.Nodes[13][0] = new Vector2(0.05f, 0.17f);
				beamLevel.Nodes[13][1] = new Vector2(0.125f, 0.245f);
				beamLevel.Nodes[14][0] = new Vector2(-0.025f, 0.395f);
				beamLevel.Nodes[14][1] = new Vector2(-0.1f, 0.32f);
				beamLevel.Packets[0][0] = new Vector2(0.960983f, 0.309017f);
				beamLevel.Packets[0][1] = new Vector2(0.960983f, 0.255983f);
				beamLevel.Packets[0][2] = new Vector2(1.014017f, 0.255983f);
				beamLevel.Packets[0][3] = new Vector2(1.014017f, 0.309017f);
				beamLevel.Packets[1][0] = new Vector2(0.660983f, 0.234017f);
				beamLevel.Packets[1][1] = new Vector2(0.660983f, 0.180983f);
				beamLevel.Packets[1][2] = new Vector2(0.714017f, 0.180983f);
				beamLevel.Packets[1][3] = new Vector2(0.714017f, 0.234017f);
				beamLevel.Packets[2][0] = new Vector2(0.735983f, 0.609017f);
				beamLevel.Packets[2][1] = new Vector2(0.735983f, 0.555983f);
				beamLevel.Packets[2][2] = new Vector2(0.789017f, 0.555983f);
				beamLevel.Packets[2][3] = new Vector2(0.789017f, 0.609017f);
				beamLevel.Packets[3][0] = new Vector2(0.585983f, 0.684017f);
				beamLevel.Packets[3][1] = new Vector2(0.585983f, 0.630983f);
				beamLevel.Packets[3][2] = new Vector2(0.639017f, 0.630983f);
				beamLevel.Packets[3][3] = new Vector2(0.639017f, 0.684017f);
				beamLevel.Packets[4][0] = new Vector2(0.585983f, 0.534017f);
				beamLevel.Packets[4][1] = new Vector2(0.585983f, 0.480983f);
				beamLevel.Packets[4][2] = new Vector2(0.639017f, 0.480983f);
				beamLevel.Packets[4][3] = new Vector2(0.639017f, 0.534017f);
				beamLevel.Packets[5][0] = new Vector2(0.285983f, 0.459017f);
				beamLevel.Packets[5][1] = new Vector2(0.285983f, 0.405983f);
				beamLevel.Packets[5][2] = new Vector2(0.339017f, 0.405983f);
				beamLevel.Packets[5][3] = new Vector2(0.339017f, 0.459017f);
				beamLevel.Packets[6][0] = new Vector2(0.585983f, 0.834017f);
				beamLevel.Packets[6][1] = new Vector2(0.585983f, 0.780983f);
				beamLevel.Packets[6][2] = new Vector2(0.639017f, 0.780983f);
				beamLevel.Packets[6][3] = new Vector2(0.639017f, 0.834017f);
				beamLevel.Packets[7][0] = new Vector2(0.210983f, 0.459017f);
				beamLevel.Packets[7][1] = new Vector2(0.210983f, 0.405983f);
				beamLevel.Packets[7][2] = new Vector2(0.264017f, 0.405983f);
				beamLevel.Packets[7][3] = new Vector2(0.264017f, 0.459017f);
				beamLevel.Packets[8][0] = new Vector2(-0.089017f, 0.609017f);
				beamLevel.Packets[8][1] = new Vector2(-0.089017f, 0.555983f);
				beamLevel.Packets[8][2] = new Vector2(-0.035983f, 0.555983f);
				beamLevel.Packets[8][3] = new Vector2(-0.035983f, 0.609017f);
				beamLevel.Firewalls[0][0] = new Vector2(0.285983f, 0.309017f);
				beamLevel.Firewalls[0][1] = new Vector2(0.285983f, 0.255983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.339017f, 0.255983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.339017f, 0.309017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.210983f, 0.309017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.210983f, 0.255983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.264017f, 0.255983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.264017f, 0.309017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.135983f, 0.684017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.135983f, 0.630983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.189017f, 0.630983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.189017f, 0.684017f);
				break;
			case 6:
				beamLevel.MaxNodes = 12;
				beamLevel.MaxRotatingNodes = 4;
				beamLevel.MaxPackets = 8;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 6;
				beamLevel.MaxProxies = 1;
				vBeam[0].Origin = new Vector2(0.425f, 0.2075f);
				vBeam[1].Origin = new Vector2(0.6875f, 0.2075f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(0.725f, 0.62f);
				beamLevel.InnerWalls[0][1] = new Vector2(0.725f, 0.17f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.8f, 0.17f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.8f, 0.62f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.575f, 0.62f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.575f, 0.545f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.725f, 0.545f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.725f, 0.62f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.8f, 0.32f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.8f, 0.245f);
				beamLevel.InnerWalls[2][2] = new Vector2(1.025f, 0.245f);
				beamLevel.InnerWalls[2][3] = new Vector2(1.025f, 0.32f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.35f, 0.695f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.35f, 0.17f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.425f, 0.17f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.425f, 0.695f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.2f, 0.695f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.2f, 0.62f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.35f, 0.62f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.35f, 0.695f);
				beamLevel.InnerWalls[5][0] = new Vector2(-0.1f, 0.695f);
				beamLevel.InnerWalls[5][1] = new Vector2(-0.1f, 0.62f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.05f, 0.62f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.05f, 0.695f);
				beamLevel.Nodes[0][0] = new Vector2(0.694011f, 0.244431f);
				beamLevel.Nodes[0][1] = new Vector2(0.680989f, 0.17057f);
				beamLevel.Nodes[1][0] = new Vector2(0.853093f, 0.398396f);
				beamLevel.Nodes[1][1] = new Vector2(0.821907f, 0.466605f);
				beamLevel.Nodes[2][0] = new Vector2(0.113945f, 0.780913f);
				beamLevel.Nodes[2][1] = new Vector2(0.061055f, 0.834088f);
				beamLevel.Nodes[3][0] = new Vector2(1.036055f, 0.834088f);
				beamLevel.Nodes[3][1] = new Vector2(1.088945f, 0.780913f);
				beamLevel.Nodes[4][0] = new Vector2(0.65f, 0.545f);
				beamLevel.Nodes[4][1] = new Vector2(0.725f, 0.47f);
				beamLevel.Nodes[5][0] = new Vector2(0.5f, 0.47f);
				beamLevel.Nodes[5][1] = new Vector2(0.425f, 0.545f);
				beamLevel.Nodes[6][0] = new Vector2(0.5f, 0.695f);
				beamLevel.Nodes[6][1] = new Vector2(0.425f, 0.62f);
				beamLevel.Nodes[7][0] = new Vector2(0.8f, 0.695f);
				beamLevel.Nodes[7][1] = new Vector2(0.875f, 0.62f);
				beamLevel.Nodes[8][0] = new Vector2(1.025f, 0.47f);
				beamLevel.Nodes[8][1] = new Vector2(1.1f, 0.395f);
				beamLevel.Nodes[9][0] = new Vector2(1.025f, 0.17f);
				beamLevel.Nodes[9][1] = new Vector2(1.1f, 0.245f);
				beamLevel.Nodes[10][0] = new Vector2(0.275f, 0.47f);
				beamLevel.Nodes[10][1] = new Vector2(0.35f, 0.395f);
				beamLevel.Nodes[11][0] = new Vector2(0.05f, 0.47f);
				beamLevel.Nodes[11][1] = new Vector2(-0.025f, 0.395f);
				beamLevel.Nodes[12][0] = new Vector2(1.025f, 0.845f);
				beamLevel.Nodes[12][1] = new Vector2(1.1f, 0.77f);
				beamLevel.Packets[0][0] = new Vector2(-0.014017f, 0.609017f);
				beamLevel.Packets[0][1] = new Vector2(-0.014017f, 0.555983f);
				beamLevel.Packets[0][2] = new Vector2(0.039017f, 0.555983f);
				beamLevel.Packets[0][3] = new Vector2(0.039017f, 0.609017f);
				beamLevel.Packets[1][0] = new Vector2(-0.089017f, 0.234017f);
				beamLevel.Packets[1][1] = new Vector2(-0.089017f, 0.180983f);
				beamLevel.Packets[1][2] = new Vector2(-0.035983f, 0.180983f);
				beamLevel.Packets[1][3] = new Vector2(-0.035983f, 0.234017f);
				beamLevel.Packets[2][0] = new Vector2(0.285983f, 0.609017f);
				beamLevel.Packets[2][1] = new Vector2(0.285983f, 0.555983f);
				beamLevel.Packets[2][2] = new Vector2(0.339017f, 0.555983f);
				beamLevel.Packets[2][3] = new Vector2(0.339017f, 0.609017f);
				beamLevel.Packets[3][0] = new Vector2(0.135983f, 0.234017f);
				beamLevel.Packets[3][1] = new Vector2(0.135983f, 0.180983f);
				beamLevel.Packets[3][2] = new Vector2(0.189017f, 0.180983f);
				beamLevel.Packets[3][3] = new Vector2(0.189017f, 0.234017f);
				beamLevel.Packets[4][0] = new Vector2(0.435983f, 0.384017f);
				beamLevel.Packets[4][1] = new Vector2(0.435983f, 0.330983f);
				beamLevel.Packets[4][2] = new Vector2(0.489017f, 0.330983f);
				beamLevel.Packets[4][3] = new Vector2(0.489017f, 0.384017f);
				beamLevel.Packets[5][0] = new Vector2(0.660983f, 0.459017f);
				beamLevel.Packets[5][1] = new Vector2(0.660983f, 0.405983f);
				beamLevel.Packets[5][2] = new Vector2(0.714017f, 0.405983f);
				beamLevel.Packets[5][3] = new Vector2(0.714017f, 0.459017f);
				beamLevel.Packets[6][0] = new Vector2(1.035983f, 0.684017f);
				beamLevel.Packets[6][1] = new Vector2(1.035983f, 0.630983f);
				beamLevel.Packets[6][2] = new Vector2(1.089017f, 0.630983f);
				beamLevel.Packets[6][3] = new Vector2(1.089017f, 0.684017f);
				beamLevel.Packets[7][0] = new Vector2(0.885983f, 0.234017f);
				beamLevel.Packets[7][1] = new Vector2(0.885983f, 0.180983f);
				beamLevel.Packets[7][2] = new Vector2(0.939017f, 0.180983f);
				beamLevel.Packets[7][3] = new Vector2(0.939017f, 0.234017f);
				beamLevel.Firewalls[0][0] = new Vector2(0.810983f, 0.234017f);
				beamLevel.Firewalls[0][1] = new Vector2(0.810983f, 0.180983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.864017f, 0.180983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.864017f, 0.234017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.285983f, 0.309017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.285983f, 0.255983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.339017f, 0.255983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.339017f, 0.309017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.435983f, 0.459017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.435983f, 0.405983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.489017f, 0.405983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.489017f, 0.459017f);
				beamLevel.Proxies[0][0] = new Vector2(0.135983f, 0.459017f);
				beamLevel.Proxies[0][1] = new Vector2(0.135983f, 0.405984f);
				beamLevel.Proxies[0][2] = new Vector2(0.189017f, 0.405984f);
				beamLevel.Proxies[0][3] = new Vector2(0.189017f, 0.459017f);
				break;
			case 7:
				beamLevel.MaxNodes = 16;
				beamLevel.MaxRotatingNodes = 8;
				beamLevel.MaxPackets = 6;
				beamLevel.MaxFirewalls = 5;
				beamLevel.MaxInnerWalls = 10;
				beamLevel.MaxProxies = 0;
				vBeam[0].Origin = new Vector2(-0.1f, 0.2075f);
				vBeam[1].Origin = new Vector2(-0.025f, 0.2075f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(-0.025f, 0.47f);
				beamLevel.InnerWalls[0][1] = new Vector2(-0.025f, 0.395f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.125f, 0.395f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.125f, 0.47f);
				beamLevel.InnerWalls[1][0] = new Vector2(-0.1f, 0.62f);
				beamLevel.InnerWalls[1][1] = new Vector2(-0.1f, 0.545f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.125f, 0.545f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.125f, 0.62f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.2f, 0.62f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.2f, 0.545f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.425f, 0.545f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.425f, 0.62f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.2f, 0.47f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.2f, 0.245f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.275f, 0.245f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.275f, 0.47f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.35f, 0.545f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.35f, 0.17f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.425f, 0.17f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.425f, 0.545f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.35f, 0.845f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.35f, 0.77f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.575f, 0.77f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.575f, 0.845f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.425f, 0.545f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.425f, 0.47f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.5f, 0.47f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.5f, 0.545f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.65f, 0.695f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.65f, 0.32f);
				beamLevel.InnerWalls[7][2] = new Vector2(0.725f, 0.32f);
				beamLevel.InnerWalls[7][3] = new Vector2(0.725f, 0.695f);
				beamLevel.InnerWalls[8][0] = new Vector2(0.8f, 0.695f);
				beamLevel.InnerWalls[8][1] = new Vector2(0.8f, 0.62f);
				beamLevel.InnerWalls[8][2] = new Vector2(0.95f, 0.62f);
				beamLevel.InnerWalls[8][3] = new Vector2(0.95f, 0.695f);
				beamLevel.InnerWalls[9][0] = new Vector2(0.575f, 0.695f);
				beamLevel.InnerWalls[9][1] = new Vector2(0.575f, 0.545f);
				beamLevel.InnerWalls[9][2] = new Vector2(0.65f, 0.545f);
				beamLevel.InnerWalls[9][3] = new Vector2(0.65f, 0.695f);
				beamLevel.Nodes[0][0] = new Vector2(-0.035983f, 0.480984f);
				beamLevel.Nodes[0][1] = new Vector2(-0.089017f, 0.534017f);
				beamLevel.Nodes[1][0] = new Vector2(0.435983f, 0.630984f);
				beamLevel.Nodes[1][1] = new Vector2(0.489017f, 0.684017f);
				beamLevel.Nodes[2][0] = new Vector2(1.09999f, 0.283375f);
				beamLevel.Nodes[2][1] = new Vector2(1.02501f, 0.283375f);
				beamLevel.Nodes[3][0] = new Vector2(1.035983f, 0.759017f);
				beamLevel.Nodes[3][1] = new Vector2(1.089017f, 0.705984f);
				beamLevel.Nodes[4][0] = new Vector2(0.060983f, 0.759017f);
				beamLevel.Nodes[4][1] = new Vector2(0.114017f, 0.705984f);
				beamLevel.Nodes[5][0] = new Vector2(0.486722f, 0.253873f);
				beamLevel.Nodes[5][1] = new Vector2(0.438278f, 0.311128f);
				beamLevel.Nodes[6][0] = new Vector2(0.575173f, 0.436107f);
				beamLevel.Nodes[6][1] = new Vector2(0.649827f, 0.436107f);
				beamLevel.Nodes[7][0] = new Vector2(0.79078f, 0.182874f);
				beamLevel.Nodes[7][1] = new Vector2(0.73422f, 0.232127f);
				beamLevel.Nodes[8][0] = new Vector2(-0.025f, 0.245f);
				beamLevel.Nodes[8][1] = new Vector2(-0.1f, 0.32f);
				beamLevel.Nodes[9][0] = new Vector2(0.125f, 0.245f);
				beamLevel.Nodes[9][1] = new Vector2(0.2f, 0.32f);
				beamLevel.Nodes[10][0] = new Vector2(0.05f, 0.62f);
				beamLevel.Nodes[10][1] = new Vector2(0.125f, 0.695f);
				beamLevel.Nodes[11][0] = new Vector2(-0.025f, 0.845f);
				beamLevel.Nodes[11][1] = new Vector2(-0.1f, 0.77f);
				beamLevel.Nodes[12][0] = new Vector2(0.125f, 0.845f);
				beamLevel.Nodes[12][1] = new Vector2(0.2f, 0.77f);
				beamLevel.Nodes[13][0] = new Vector2(0.275f, 0.545f);
				beamLevel.Nodes[13][1] = new Vector2(0.35f, 0.47f);
				beamLevel.Nodes[14][0] = new Vector2(-0.025f, 0.62f);
				beamLevel.Nodes[14][1] = new Vector2(-0.1f, 0.695f);
				beamLevel.Nodes[15][0] = new Vector2(0.275f, 0.17f);
				beamLevel.Nodes[15][1] = new Vector2(0.35f, 0.245f);
				beamLevel.Packets[0][0] = new Vector2(0.023483f, 0.309017f);
				beamLevel.Packets[0][1] = new Vector2(0.023483f, 0.255983f);
				beamLevel.Packets[0][2] = new Vector2(0.076517f, 0.255983f);
				beamLevel.Packets[0][3] = new Vector2(0.076517f, 0.309017f);
				beamLevel.Packets[1][0] = new Vector2(0.435983f, 0.459017f);
				beamLevel.Packets[1][1] = new Vector2(0.435983f, 0.405983f);
				beamLevel.Packets[1][2] = new Vector2(0.489017f, 0.405983f);
				beamLevel.Packets[1][3] = new Vector2(0.489017f, 0.459017f);
				beamLevel.Packets[2][0] = new Vector2(1.035983f, 0.459017f);
				beamLevel.Packets[2][1] = new Vector2(1.035983f, 0.405983f);
				beamLevel.Packets[2][2] = new Vector2(1.089017f, 0.405983f);
				beamLevel.Packets[2][3] = new Vector2(1.089017f, 0.459017f);
				beamLevel.Packets[3][0] = new Vector2(0.735983f, 0.609017f);
				beamLevel.Packets[3][1] = new Vector2(0.735983f, 0.555983f);
				beamLevel.Packets[3][2] = new Vector2(0.789017f, 0.555983f);
				beamLevel.Packets[3][3] = new Vector2(0.789017f, 0.609017f);
				beamLevel.Packets[4][0] = new Vector2(0.735983f, 0.834017f);
				beamLevel.Packets[4][1] = new Vector2(0.735983f, 0.780983f);
				beamLevel.Packets[4][2] = new Vector2(0.789017f, 0.780983f);
				beamLevel.Packets[4][3] = new Vector2(0.789017f, 0.834017f);
				beamLevel.Packets[5][0] = new Vector2(0.585983f, 0.834017f);
				beamLevel.Packets[5][1] = new Vector2(0.585983f, 0.780983f);
				beamLevel.Packets[5][2] = new Vector2(0.639017f, 0.780983f);
				beamLevel.Packets[5][3] = new Vector2(0.639017f, 0.834017f);
				beamLevel.Firewalls[0][0] = new Vector2(0.285983f, 0.684017f);
				beamLevel.Firewalls[0][1] = new Vector2(0.285983f, 0.630983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.339017f, 0.630983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.339017f, 0.684017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.285983f, 0.834017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.285983f, 0.780983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.339017f, 0.780983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.339017f, 0.834017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.585983f, 0.384017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.585983f, 0.330983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.639017f, 0.330983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.639017f, 0.384017f);
				beamLevel.Firewalls[3][0] = new Vector2(0.660983f, 0.834017f);
				beamLevel.Firewalls[3][1] = new Vector2(0.660983f, 0.780983f);
				beamLevel.Firewalls[3][2] = new Vector2(0.714017f, 0.780983f);
				beamLevel.Firewalls[3][3] = new Vector2(0.714017f, 0.834017f);
				beamLevel.Firewalls[4][0] = new Vector2(0.960983f, 0.684017f);
				beamLevel.Firewalls[4][1] = new Vector2(0.960983f, 0.630983f);
				beamLevel.Firewalls[4][2] = new Vector2(1.014017f, 0.630983f);
				beamLevel.Firewalls[4][3] = new Vector2(1.014017f, 0.684017f);
				break;
			case 8:
				beamLevel.MaxNodes = 10;
				beamLevel.MaxRotatingNodes = 5;
				beamLevel.MaxPackets = 6;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 6;
				beamLevel.MaxProxies = 1;
				vBeam[0].Origin = new Vector2(0.0125f, 0.845f);
				vBeam[1].Origin = new Vector2(0.0125f, 0.4325f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(0.125f, 0.395f);
				beamLevel.InnerWalls[0][1] = new Vector2(0.125f, 0.17f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.2f, 0.17f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.2f, 0.395f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.35f, 0.845f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.35f, 0.545f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.425f, 0.545f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.425f, 0.845f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.5f, 0.845f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.5f, 0.62f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.575f, 0.62f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.575f, 0.845f);
				beamLevel.InnerWalls[3][0] = new Vector2(1.025f, 0.845f);
				beamLevel.InnerWalls[3][1] = new Vector2(1.025f, 0.62f);
				beamLevel.InnerWalls[3][2] = new Vector2(1.1f, 0.62f);
				beamLevel.InnerWalls[3][3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.95f, 0.545f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.95f, 0.17f);
				beamLevel.InnerWalls[4][2] = new Vector2(1.025f, 0.17f);
				beamLevel.InnerWalls[4][3] = new Vector2(1.025f, 0.545f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.575f, 0.845f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.575f, 0.77f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.875f, 0.77f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.875f, 0.845f);
				beamLevel.Nodes[0][0] = new Vector2(0.0125f, 0.470001f);
				beamLevel.Nodes[0][1] = new Vector2(0.0125f, 0.395001f);
				beamLevel.Nodes[1][0] = new Vector2(0.285983f, 0.309017f);
				beamLevel.Nodes[1][1] = new Vector2(0.339017f, 0.255984f);
				beamLevel.Nodes[2][0] = new Vector2(0.885983f, 0.309017f);
				beamLevel.Nodes[2][1] = new Vector2(0.939017f, 0.255984f);
				beamLevel.Nodes[3][0] = new Vector2(0.285983f, 0.834017f);
				beamLevel.Nodes[3][1] = new Vector2(0.339017f, 0.780984f);
				beamLevel.Nodes[4][0] = new Vector2(0.4625f, 0.620001f);
				beamLevel.Nodes[4][1] = new Vector2(0.4625f, 0.545f);
				beamLevel.Nodes[5][0] = new Vector2(1.025f, 0.62f);
				beamLevel.Nodes[5][1] = new Vector2(1.1f, 0.545f);
				beamLevel.Nodes[6][0] = new Vector2(0.5f, 0.17f);
				beamLevel.Nodes[6][1] = new Vector2(0.425f, 0.245f);
				beamLevel.Nodes[7][0] = new Vector2(0.95f, 0.845f);
				beamLevel.Nodes[7][1] = new Vector2(0.875f, 0.77f);
				beamLevel.Nodes[8][0] = new Vector2(0.95f, 0.845f);
				beamLevel.Nodes[8][1] = new Vector2(1.025f, 0.77f);
				beamLevel.Nodes[9][0] = new Vector2(0.95f, 0.845f);
				beamLevel.Nodes[9][1] = new Vector2(1.025f, 0.77f);
				beamLevel.Packets[0][0] = new Vector2(-0.014017f, 0.234017f);
				beamLevel.Packets[0][1] = new Vector2(-0.014017f, 0.180983f);
				beamLevel.Packets[0][2] = new Vector2(0.039017f, 0.180983f);
				beamLevel.Packets[0][3] = new Vector2(0.039017f, 0.234017f);
				beamLevel.Packets[1][0] = new Vector2(-0.089017f, 0.834017f);
				beamLevel.Packets[1][1] = new Vector2(-0.089017f, 0.780983f);
				beamLevel.Packets[1][2] = new Vector2(-0.035983f, 0.780983f);
				beamLevel.Packets[1][3] = new Vector2(-0.035983f, 0.834017f);
				beamLevel.Packets[2][0] = new Vector2(0.510983f, 0.234017f);
				beamLevel.Packets[2][1] = new Vector2(0.510983f, 0.180983f);
				beamLevel.Packets[2][2] = new Vector2(0.564017f, 0.180983f);
				beamLevel.Packets[2][3] = new Vector2(0.564017f, 0.234017f);
				beamLevel.Packets[3][0] = new Vector2(0.885983f, 0.234017f);
				beamLevel.Packets[3][1] = new Vector2(0.885983f, 0.180983f);
				beamLevel.Packets[3][2] = new Vector2(0.939017f, 0.180983f);
				beamLevel.Packets[3][3] = new Vector2(0.939017f, 0.234017f);
				beamLevel.Packets[4][0] = new Vector2(1.035983f, 0.234017f);
				beamLevel.Packets[4][1] = new Vector2(1.035983f, 0.180983f);
				beamLevel.Packets[4][2] = new Vector2(1.089017f, 0.180983f);
				beamLevel.Packets[4][3] = new Vector2(1.089017f, 0.234017f);
				beamLevel.Packets[5][0] = new Vector2(0.960983f, 0.684017f);
				beamLevel.Packets[5][1] = new Vector2(0.960983f, 0.630983f);
				beamLevel.Packets[5][2] = new Vector2(1.014017f, 0.630983f);
				beamLevel.Packets[5][3] = new Vector2(1.014017f, 0.684017f);
				beamLevel.Firewalls[0][0] = new Vector2(0.060983f, 0.234017f);
				beamLevel.Firewalls[0][1] = new Vector2(0.060983f, 0.180983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.114017f, 0.180983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.114017f, 0.234017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.585983f, 0.759017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.585983f, 0.705983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.639017f, 0.705983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.639017f, 0.759017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.435983f, 0.759017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.435983f, 0.705983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.489017f, 0.705983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.489017f, 0.759017f);
				beamLevel.Proxies[0][0] = new Vector2(0.435983f, 0.309017f);
				beamLevel.Proxies[0][1] = new Vector2(0.435983f, 0.255984f);
				beamLevel.Proxies[0][2] = new Vector2(0.489017f, 0.255984f);
				beamLevel.Proxies[0][3] = new Vector2(0.489017f, 0.309017f);
				break;
			}
			switch (iLevel)
			{
			case 9:
				beamLevel.MaxNodes = 11;
				beamLevel.MaxRotatingNodes = 6;
				beamLevel.MaxPackets = 6;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 8;
				beamLevel.MaxProxies = 2;
				vBeam[0].Origin = new Vector2(0.0875f, 0.545f);
				vBeam[1].Origin = new Vector2(0.0875f, 0.8075f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(0.275f, 0.845f);
				beamLevel.InnerWalls[0][1] = new Vector2(0.275f, 0.695f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.35f, 0.695f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.35f, 0.845f);
				beamLevel.InnerWalls[1][0] = new Vector2(-0.025f, 0.62f);
				beamLevel.InnerWalls[1][1] = new Vector2(-0.025f, 0.545f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.05f, 0.545f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.05f, 0.62f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.05f, 0.545f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.05f, 0.47f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.125f, 0.47f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.125f, 0.545f);
				beamLevel.InnerWalls[3][0] = new Vector2(-0.1f, 0.47f);
				beamLevel.InnerWalls[3][1] = new Vector2(-0.1f, 0.395f);
				beamLevel.InnerWalls[3][2] = new Vector2(-0.025f, 0.395f);
				beamLevel.InnerWalls[3][3] = new Vector2(-0.025f, 0.47f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.8f, 0.47f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.8f, 0.395f);
				beamLevel.InnerWalls[4][2] = new Vector2(1.025f, 0.395f);
				beamLevel.InnerWalls[4][3] = new Vector2(1.025f, 0.47f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.8f, 0.77f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.8f, 0.62f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.875f, 0.62f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.875f, 0.77f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.725f, 0.77f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.725f, 0.47f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.8f, 0.47f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.8f, 0.77f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.5f, 0.545f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.5f, 0.245f);
				beamLevel.InnerWalls[7][2] = new Vector2(0.65f, 0.245f);
				beamLevel.InnerWalls[7][3] = new Vector2(0.65f, 0.545f);
				beamLevel.Nodes[0][0] = new Vector2(0.060983f, 0.834017f);
				beamLevel.Nodes[0][1] = new Vector2(0.114017f, 0.780984f);
				beamLevel.Nodes[1][0] = new Vector2(0.285983f, 0.571517f);
				beamLevel.Nodes[1][1] = new Vector2(0.339017f, 0.518484f);
				beamLevel.Nodes[2][0] = new Vector2(0.060983f, 0.384017f);
				beamLevel.Nodes[2][1] = new Vector2(0.114017f, 0.330984f);
				beamLevel.Nodes[3][0] = new Vector2(1.035983f, 0.759017f);
				beamLevel.Nodes[3][1] = new Vector2(1.089017f, 0.705984f);
				beamLevel.Nodes[4][0] = new Vector2(0.135983f, 0.459017f);
				beamLevel.Nodes[4][1] = new Vector2(0.189017f, 0.405984f);
				beamLevel.Nodes[5][0] = new Vector2(0.210983f, 0.234017f);
				beamLevel.Nodes[5][1] = new Vector2(0.264017f, 0.180984f);
				beamLevel.Nodes[6][0] = new Vector2(-0.025f, 0.545f);
				beamLevel.Nodes[6][1] = new Vector2(0.05f, 0.47f);
				beamLevel.Nodes[7][0] = new Vector2(-0.025f, 0.845f);
				beamLevel.Nodes[7][1] = new Vector2(-0.1f, 0.77f);
				beamLevel.Nodes[8][0] = new Vector2(0.35f, 0.17f);
				beamLevel.Nodes[8][1] = new Vector2(0.275f, 0.245f);
				beamLevel.Nodes[9][0] = new Vector2(1.025f, 0.17f);
				beamLevel.Nodes[9][1] = new Vector2(1.1f, 0.245f);
				beamLevel.Nodes[10][0] = new Vector2(1.025f, 0.845f);
				beamLevel.Nodes[10][1] = new Vector2(1.1f, 0.77f);
				beamLevel.Packets[0][0] = new Vector2(0.210983f, 0.834017f);
				beamLevel.Packets[0][1] = new Vector2(0.210983f, 0.780983f);
				beamLevel.Packets[0][2] = new Vector2(0.264017f, 0.780983f);
				beamLevel.Packets[0][3] = new Vector2(0.264017f, 0.834017f);
				beamLevel.Packets[1][0] = new Vector2(0.060983f, 0.459017f);
				beamLevel.Packets[1][1] = new Vector2(0.060983f, 0.405983f);
				beamLevel.Packets[1][2] = new Vector2(0.114017f, 0.405983f);
				beamLevel.Packets[1][3] = new Vector2(0.114017f, 0.459017f);
				beamLevel.Packets[2][0] = new Vector2(-0.014017f, 0.234017f);
				beamLevel.Packets[2][1] = new Vector2(-0.014017f, 0.180983f);
				beamLevel.Packets[2][2] = new Vector2(0.039017f, 0.180983f);
				beamLevel.Packets[2][3] = new Vector2(0.039017f, 0.234017f);
				beamLevel.Packets[3][0] = new Vector2(0.885983f, 0.534017f);
				beamLevel.Packets[3][1] = new Vector2(0.885983f, 0.480983f);
				beamLevel.Packets[3][2] = new Vector2(0.939017f, 0.480983f);
				beamLevel.Packets[3][3] = new Vector2(0.939017f, 0.534017f);
				beamLevel.Packets[4][0] = new Vector2(1.035983f, 0.384017f);
				beamLevel.Packets[4][1] = new Vector2(1.035983f, 0.330983f);
				beamLevel.Packets[4][2] = new Vector2(1.089017f, 0.330983f);
				beamLevel.Packets[4][3] = new Vector2(1.089017f, 0.384017f);
				beamLevel.Packets[5][0] = new Vector2(0.885983f, 0.759017f);
				beamLevel.Packets[5][1] = new Vector2(0.885983f, 0.705983f);
				beamLevel.Packets[5][2] = new Vector2(0.939017f, 0.705983f);
				beamLevel.Packets[5][3] = new Vector2(0.939017f, 0.759017f);
				beamLevel.Firewalls[0][0] = new Vector2(-0.089017f, 0.384017f);
				beamLevel.Firewalls[0][1] = new Vector2(-0.089017f, 0.330983f);
				beamLevel.Firewalls[0][2] = new Vector2(-0.035983f, 0.330983f);
				beamLevel.Firewalls[0][3] = new Vector2(-0.035983f, 0.384017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.810983f, 0.534017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.810983f, 0.480983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.864017f, 0.480983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.864017f, 0.534017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.360983f, 0.834017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.360983f, 0.780983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.414017f, 0.780983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.414017f, 0.834017f);
				beamLevel.Proxies[0][0] = new Vector2(0.660983f, 0.534017f);
				beamLevel.Proxies[0][1] = new Vector2(0.660983f, 0.480984f);
				beamLevel.Proxies[0][2] = new Vector2(0.714017f, 0.480984f);
				beamLevel.Proxies[0][3] = new Vector2(0.714017f, 0.534017f);
				beamLevel.Proxies[1][0] = new Vector2(-0.089017f, 0.534017f);
				beamLevel.Proxies[1][1] = new Vector2(-0.089017f, 0.480984f);
				beamLevel.Proxies[1][2] = new Vector2(-0.035983f, 0.480984f);
				beamLevel.Proxies[1][3] = new Vector2(-0.035983f, 0.534017f);
				break;
			case 10:
				beamLevel.MaxNodes = 13;
				beamLevel.MaxRotatingNodes = 8;
				beamLevel.MaxPackets = 10;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 8;
				beamLevel.MaxProxies = 1;
				vBeam[0].Origin = new Vector2(0.0125f, 0.245f);
				vBeam[1].Origin = new Vector2(0.0125f, 0.8075f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(-0.025f, 0.245f);
				beamLevel.InnerWalls[0][1] = new Vector2(-0.025f, 0.17f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.05f, 0.17f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.05f, 0.245f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.125f, 0.47f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.125f, 0.17f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.2f, 0.17f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.2f, 0.47f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.275f, 0.395f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.275f, 0.17f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.35f, 0.17f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.35f, 0.395f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.5f, 0.32f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.5f, 0.245f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.725f, 0.245f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.725f, 0.32f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.65f, 0.245f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.65f, 0.17f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.725f, 0.17f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.725f, 0.245f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.8f, 0.395f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.8f, 0.245f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.875f, 0.245f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.875f, 0.395f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.2f, 0.845f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.2f, 0.77f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.425f, 0.77f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.425f, 0.845f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.35f, 0.77f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.35f, 0.695f);
				beamLevel.InnerWalls[7][2] = new Vector2(0.575f, 0.695f);
				beamLevel.InnerWalls[7][3] = new Vector2(0.575f, 0.77f);
				beamLevel.Nodes[0][0] = new Vector2(-0.025f, 0.807481f);
				beamLevel.Nodes[0][1] = new Vector2(0.05f, 0.80752f);
				beamLevel.Nodes[1][0] = new Vector2(0.275f, 0.5075f);
				beamLevel.Nodes[1][1] = new Vector2(0.35f, 0.507501f);
				beamLevel.Nodes[2][0] = new Vector2(0.575f, 0.5075f);
				beamLevel.Nodes[2][1] = new Vector2(0.65f, 0.5075f);
				beamLevel.Nodes[3][0] = new Vector2(0.425f, 0.5075f);
				beamLevel.Nodes[3][1] = new Vector2(0.5f, 0.507501f);
				beamLevel.Nodes[4][0] = new Vector2(0.086072f, 0.319973f);
				beamLevel.Nodes[4][1] = new Vector2(0.086072f, 0.245028f);
				beamLevel.Nodes[5][0] = new Vector2(1.035983f, 0.234017f);
				beamLevel.Nodes[5][1] = new Vector2(1.089017f, 0.180984f);
				beamLevel.Nodes[6][0] = new Vector2(0.210983f, 0.759017f);
				beamLevel.Nodes[6][1] = new Vector2(0.264017f, 0.705984f);
				beamLevel.Nodes[7][0] = new Vector2(0.725f, 0.5075f);
				beamLevel.Nodes[7][1] = new Vector2(0.8f, 0.507501f);
				beamLevel.Nodes[8][0] = new Vector2(0.125f, 0.845f);
				beamLevel.Nodes[8][1] = new Vector2(0.2f, 0.77f);
				beamLevel.Nodes[9][0] = new Vector2(0.2f, 0.47f);
				beamLevel.Nodes[9][1] = new Vector2(0.125f, 0.545f);
				beamLevel.Nodes[10][0] = new Vector2(0.425f, 0.17f);
				beamLevel.Nodes[10][1] = new Vector2(0.35f, 0.245f);
				beamLevel.Nodes[11][0] = new Vector2(1.025f, 0.845f);
				beamLevel.Nodes[11][1] = new Vector2(1.1f, 0.77f);
				beamLevel.Nodes[12][0] = new Vector2(-0.025f, 0.845f);
				beamLevel.Nodes[12][1] = new Vector2(-0.1f, 0.77f);
				beamLevel.Packets[0][0] = new Vector2(-0.089017f, 0.234017f);
				beamLevel.Packets[0][1] = new Vector2(-0.089017f, 0.180983f);
				beamLevel.Packets[0][2] = new Vector2(-0.035983f, 0.180983f);
				beamLevel.Packets[0][3] = new Vector2(-0.035983f, 0.234017f);
				beamLevel.Packets[1][0] = new Vector2(0.210983f, 0.384017f);
				beamLevel.Packets[1][1] = new Vector2(0.210983f, 0.330983f);
				beamLevel.Packets[1][2] = new Vector2(0.264017f, 0.330983f);
				beamLevel.Packets[1][3] = new Vector2(0.264017f, 0.384017f);
				beamLevel.Packets[2][0] = new Vector2(0.210983f, 0.234017f);
				beamLevel.Packets[2][1] = new Vector2(0.210983f, 0.180983f);
				beamLevel.Packets[2][2] = new Vector2(0.264017f, 0.180983f);
				beamLevel.Packets[2][3] = new Vector2(0.264017f, 0.234017f);
				beamLevel.Packets[3][0] = new Vector2(0.585983f, 0.234017f);
				beamLevel.Packets[3][1] = new Vector2(0.585983f, 0.180983f);
				beamLevel.Packets[3][2] = new Vector2(0.639017f, 0.180983f);
				beamLevel.Packets[3][3] = new Vector2(0.639017f, 0.234017f);
				beamLevel.Packets[4][0] = new Vector2(0.735983f, 0.234017f);
				beamLevel.Packets[4][1] = new Vector2(0.735983f, 0.180983f);
				beamLevel.Packets[4][2] = new Vector2(0.789017f, 0.180983f);
				beamLevel.Packets[4][3] = new Vector2(0.789017f, 0.234017f);
				beamLevel.Packets[5][0] = new Vector2(0.960983f, 0.684017f);
				beamLevel.Packets[5][1] = new Vector2(0.960983f, 0.630983f);
				beamLevel.Packets[5][2] = new Vector2(1.014017f, 0.630983f);
				beamLevel.Packets[5][3] = new Vector2(1.014017f, 0.684017f);
				beamLevel.Packets[6][0] = new Vector2(0.435983f, 0.834017f);
				beamLevel.Packets[6][1] = new Vector2(0.435983f, 0.780983f);
				beamLevel.Packets[6][2] = new Vector2(0.489017f, 0.780983f);
				beamLevel.Packets[6][3] = new Vector2(0.489017f, 0.834017f);
				beamLevel.Packets[7][0] = new Vector2(0.135983f, 0.759017f);
				beamLevel.Packets[7][1] = new Vector2(0.135983f, 0.705983f);
				beamLevel.Packets[7][2] = new Vector2(0.189017f, 0.705983f);
				beamLevel.Packets[7][3] = new Vector2(0.189017f, 0.759017f);
				beamLevel.Packets[8][0] = new Vector2(0.885983f, 0.459017f);
				beamLevel.Packets[8][1] = new Vector2(0.885983f, 0.405983f);
				beamLevel.Packets[8][2] = new Vector2(0.939017f, 0.405983f);
				beamLevel.Packets[8][3] = new Vector2(0.939017f, 0.459017f);
				beamLevel.Packets[9][0] = new Vector2(0.360983f, 0.684017f);
				beamLevel.Packets[9][1] = new Vector2(0.360983f, 0.630983f);
				beamLevel.Packets[9][2] = new Vector2(0.414017f, 0.630983f);
				beamLevel.Packets[9][3] = new Vector2(0.414017f, 0.684017f);
				beamLevel.Firewalls[0][0] = new Vector2(0.735983f, 0.309017f);
				beamLevel.Firewalls[0][1] = new Vector2(0.735983f, 0.255983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.789017f, 0.255983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.789017f, 0.309017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.060983f, 0.234017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.060983f, 0.180983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.114017f, 0.180983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.114017f, 0.234017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.960983f, 0.309017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.960983f, 0.255983f);
				beamLevel.Firewalls[2][2] = new Vector2(1.014017f, 0.255983f);
				beamLevel.Firewalls[2][3] = new Vector2(1.014017f, 0.309017f);
				beamLevel.Proxies[0][0] = new Vector2(1.035983f, 0.534017f);
				beamLevel.Proxies[0][1] = new Vector2(1.035983f, 0.480984f);
				beamLevel.Proxies[0][2] = new Vector2(1.089017f, 0.480984f);
				beamLevel.Proxies[0][3] = new Vector2(1.089017f, 0.534017f);
				break;
			case 11:
				beamLevel.MaxNodes = 14;
				beamLevel.MaxRotatingNodes = 7;
				beamLevel.MaxPackets = 8;
				beamLevel.MaxFirewalls = 3;
				beamLevel.MaxInnerWalls = 8;
				beamLevel.MaxProxies = 1;
				vBeam[0].Origin = new Vector2(0.4625f, 0.245f);
				vBeam[1].Origin = new Vector2(0.4625f, 2f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(0.275f, 0.845f);
				beamLevel.InnerWalls[0][1] = new Vector2(0.275f, 0.62f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.35f, 0.62f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.35f, 0.845f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.575f, 0.845f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.575f, 0.62f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.65f, 0.62f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.65f, 0.845f);
				beamLevel.InnerWalls[2][0] = new Vector2(1.025f, 0.845f);
				beamLevel.InnerWalls[2][1] = new Vector2(1.025f, 0.77f);
				beamLevel.InnerWalls[2][2] = new Vector2(1.1f, 0.77f);
				beamLevel.InnerWalls[2][3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.2f, 0.545f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.2f, 0.395f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.275f, 0.395f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.275f, 0.545f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.8f, 0.395f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.8f, 0.17f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.875f, 0.17f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.875f, 0.395f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.8f, 0.695f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.8f, 0.62f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.875f, 0.62f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.875f, 0.695f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.65f, 0.395f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.65f, 0.17f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.725f, 0.17f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.725f, 0.395f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.425f, 0.245f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.425f, 0.17f);
				beamLevel.InnerWalls[7][2] = new Vector2(0.5f, 0.17f);
				beamLevel.InnerWalls[7][3] = new Vector2(0.5f, 0.245f);
				beamLevel.Nodes[0][0] = new Vector2(0.435983f, 0.834017f);
				beamLevel.Nodes[0][1] = new Vector2(0.489017f, 0.780984f);
				beamLevel.Nodes[1][0] = new Vector2(0.210983f, 0.234017f);
				beamLevel.Nodes[1][1] = new Vector2(0.264017f, 0.180984f);
				beamLevel.Nodes[2][0] = new Vector2(0.585983f, 0.234017f);
				beamLevel.Nodes[2][1] = new Vector2(0.639017f, 0.180984f);
				beamLevel.Nodes[3][0] = new Vector2(0.7625f, 0.32f);
				beamLevel.Nodes[3][1] = new Vector2(0.7625f, 0.245001f);
				beamLevel.Nodes[4][0] = new Vector2(1.035983f, 0.609017f);
				beamLevel.Nodes[4][1] = new Vector2(1.089017f, 0.555984f);
				beamLevel.Nodes[5][0] = new Vector2(0.885983f, 0.684017f);
				beamLevel.Nodes[5][1] = new Vector2(0.939017f, 0.630984f);
				beamLevel.Nodes[6][0] = new Vector2(-0.035983f, 0.684017f);
				beamLevel.Nodes[6][1] = new Vector2(-0.089017f, 0.630984f);
				beamLevel.Nodes[7][0] = new Vector2(0.275f, 0.62f);
				beamLevel.Nodes[7][1] = new Vector2(0.35f, 0.545f);
				beamLevel.Nodes[8][0] = new Vector2(0.65f, 0.62f);
				beamLevel.Nodes[8][1] = new Vector2(0.575f, 0.545f);
				beamLevel.Nodes[9][0] = new Vector2(1.025f, 0.77f);
				beamLevel.Nodes[9][1] = new Vector2(1.1f, 0.695f);
				beamLevel.Nodes[10][0] = new Vector2(-0.025f, 0.845f);
				beamLevel.Nodes[10][1] = new Vector2(-0.1f, 0.77f);
				beamLevel.Nodes[11][0] = new Vector2(-0.025f, 0.17f);
				beamLevel.Nodes[11][1] = new Vector2(-0.1f, 0.245f);
				beamLevel.Nodes[12][0] = new Vector2(0.5f, 0.845f);
				beamLevel.Nodes[12][1] = new Vector2(0.575f, 0.77f);
				beamLevel.Nodes[13][0] = new Vector2(0.575f, 0.17f);
				beamLevel.Nodes[13][1] = new Vector2(0.5f, 0.245f);
				beamLevel.Packets[0][0] = new Vector2(0.210983f, 0.834017f);
				beamLevel.Packets[0][1] = new Vector2(0.210983f, 0.780983f);
				beamLevel.Packets[0][2] = new Vector2(0.264017f, 0.780983f);
				beamLevel.Packets[0][3] = new Vector2(0.264017f, 0.834017f);
				beamLevel.Packets[1][0] = new Vector2(0.210983f, 0.759017f);
				beamLevel.Packets[1][1] = new Vector2(0.210983f, 0.705983f);
				beamLevel.Packets[1][2] = new Vector2(0.264017f, 0.705983f);
				beamLevel.Packets[1][3] = new Vector2(0.264017f, 0.759017f);
				beamLevel.Packets[2][0] = new Vector2(0.135983f, 0.534017f);
				beamLevel.Packets[2][1] = new Vector2(0.135983f, 0.480983f);
				beamLevel.Packets[2][2] = new Vector2(0.189017f, 0.480983f);
				beamLevel.Packets[2][3] = new Vector2(0.189017f, 0.534017f);
				beamLevel.Packets[3][0] = new Vector2(-0.089017f, 0.459017f);
				beamLevel.Packets[3][1] = new Vector2(-0.089017f, 0.405983f);
				beamLevel.Packets[3][2] = new Vector2(-0.035983f, 0.405983f);
				beamLevel.Packets[3][3] = new Vector2(-0.035983f, 0.459017f);
				beamLevel.Packets[4][0] = new Vector2(-0.089017f, 0.309017f);
				beamLevel.Packets[4][1] = new Vector2(-0.089017f, 0.255983f);
				beamLevel.Packets[4][2] = new Vector2(-0.035983f, 0.255983f);
				beamLevel.Packets[4][3] = new Vector2(-0.035983f, 0.309017f);
				beamLevel.Packets[5][0] = new Vector2(0.660983f, 0.759017f);
				beamLevel.Packets[5][1] = new Vector2(0.660983f, 0.705983f);
				beamLevel.Packets[5][2] = new Vector2(0.714017f, 0.705983f);
				beamLevel.Packets[5][3] = new Vector2(0.714017f, 0.759017f);
				beamLevel.Packets[6][0] = new Vector2(0.660983f, 0.834017f);
				beamLevel.Packets[6][1] = new Vector2(0.660983f, 0.780983f);
				beamLevel.Packets[6][2] = new Vector2(0.714017f, 0.780983f);
				beamLevel.Packets[6][3] = new Vector2(0.714017f, 0.834017f);
				beamLevel.Packets[7][0] = new Vector2(0.885983f, 0.309017f);
				beamLevel.Packets[7][1] = new Vector2(0.885983f, 0.255983f);
				beamLevel.Packets[7][2] = new Vector2(0.939017f, 0.255983f);
				beamLevel.Packets[7][3] = new Vector2(0.939017f, 0.309017f);
				beamLevel.Firewalls[0][0] = new Vector2(0.210983f, 0.609017f);
				beamLevel.Firewalls[0][1] = new Vector2(0.210983f, 0.555983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.264017f, 0.555983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.264017f, 0.609017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.735983f, 0.234017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.735983f, 0.180983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.789017f, 0.180983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.789017f, 0.234017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.960983f, 0.309017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.960983f, 0.255983f);
				beamLevel.Firewalls[2][2] = new Vector2(1.014017f, 0.255983f);
				beamLevel.Firewalls[2][3] = new Vector2(1.014017f, 0.309017f);
				beamLevel.Proxies[0][0] = new Vector2(0.735983f, 0.759017f);
				beamLevel.Proxies[0][1] = new Vector2(0.735983f, 0.705984f);
				beamLevel.Proxies[0][2] = new Vector2(0.789017f, 0.705984f);
				beamLevel.Proxies[0][3] = new Vector2(0.789017f, 0.759017f);
				break;
			case 12:
				beamLevel.MaxNodes = 14;
				beamLevel.MaxRotatingNodes = 7;
				beamLevel.MaxPackets = 6;
				beamLevel.MaxFirewalls = 5;
				beamLevel.MaxInnerWalls = 11;
				beamLevel.MaxProxies = 1;
				vBeam[0].Origin = new Vector2(0.0875f, 0.32f);
				vBeam[1].Origin = new Vector2(0.0875f, 2f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(-0.025f, 0.77f);
				beamLevel.InnerWalls[0][1] = new Vector2(-0.025f, 0.62f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.05f, 0.62f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.05f, 0.77f);
				beamLevel.InnerWalls[1][0] = new Vector2(-0.025f, 0.545f);
				beamLevel.InnerWalls[1][1] = new Vector2(-0.025f, 0.395f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.05f, 0.395f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.05f, 0.545f);
				beamLevel.InnerWalls[2][0] = new Vector2(-0.025f, 0.32f);
				beamLevel.InnerWalls[2][1] = new Vector2(-0.025f, 0.17f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.2f, 0.17f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.2f, 0.32f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.275f, 0.32f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.275f, 0.17f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.35f, 0.17f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.35f, 0.32f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.35f, 0.245f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.35f, 0.17f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.575f, 0.17f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.575f, 0.245f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.65f, 0.47f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.65f, 0.245f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.875f, 0.245f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.875f, 0.47f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.35f, 0.845f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.35f, 0.77f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.5f, 0.77f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.5f, 0.845f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.95f, 0.845f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.95f, 0.77f);
				beamLevel.InnerWalls[7][2] = new Vector2(1.1f, 0.77f);
				beamLevel.InnerWalls[7][3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[8][0] = new Vector2(0.125f, 0.695f);
				beamLevel.InnerWalls[8][1] = new Vector2(0.125f, 0.47f);
				beamLevel.InnerWalls[8][2] = new Vector2(0.2f, 0.47f);
				beamLevel.InnerWalls[8][3] = new Vector2(0.2f, 0.695f);
				beamLevel.InnerWalls[9][0] = new Vector2(0.35f, 0.545f);
				beamLevel.InnerWalls[9][1] = new Vector2(0.35f, 0.47f);
				beamLevel.InnerWalls[9][2] = new Vector2(0.875f, 0.47f);
				beamLevel.InnerWalls[9][3] = new Vector2(0.875f, 0.545f);
				beamLevel.InnerWalls[10][0] = new Vector2(0.35f, 0.47f);
				beamLevel.InnerWalls[10][1] = new Vector2(0.35f, 0.395f);
				beamLevel.InnerWalls[10][2] = new Vector2(0.425f, 0.395f);
				beamLevel.InnerWalls[10][3] = new Vector2(0.425f, 0.47f);
				beamLevel.Nodes[0][0] = new Vector2(0.060983f, 0.759017f);
				beamLevel.Nodes[0][1] = new Vector2(0.114017f, 0.705984f);
				beamLevel.Nodes[1][0] = new Vector2(0.35f, 0.732501f);
				beamLevel.Nodes[1][1] = new Vector2(0.425f, 0.732501f);
				beamLevel.Nodes[2][0] = new Vector2(0.575f, 0.732501f);
				beamLevel.Nodes[2][1] = new Vector2(0.65f, 0.732501f);
				beamLevel.Nodes[3][0] = new Vector2(0.800004f, 0.732501f);
				beamLevel.Nodes[3][1] = new Vector2(0.874996f, 0.732501f);
				beamLevel.Nodes[4][0] = new Vector2(1.089017f, 0.534017f);
				beamLevel.Nodes[4][1] = new Vector2(1.035983f, 0.480984f);
				beamLevel.Nodes[5][0] = new Vector2(0.885983f, 0.234017f);
				beamLevel.Nodes[5][1] = new Vector2(0.939017f, 0.180984f);
				beamLevel.Nodes[6][0] = new Vector2(0.4625f, 0.395f);
				beamLevel.Nodes[6][1] = new Vector2(0.4625f, 0.32f);
				beamLevel.Nodes[7][0] = new Vector2(-0.025f, 0.845f);
				beamLevel.Nodes[7][1] = new Vector2(-0.1f, 0.77f);
				beamLevel.Nodes[8][0] = new Vector2(0.275f, 0.845f);
				beamLevel.Nodes[8][1] = new Vector2(0.35f, 0.77f);
				beamLevel.Nodes[9][0] = new Vector2(1.025f, 0.77f);
				beamLevel.Nodes[9][1] = new Vector2(1.1f, 0.695f);
				beamLevel.Nodes[10][0] = new Vector2(0.35f, 0.32f);
				beamLevel.Nodes[10][1] = new Vector2(0.275f, 0.395f);
				beamLevel.Nodes[11][0] = new Vector2(0.575f, 0.395f);
				beamLevel.Nodes[11][1] = new Vector2(0.65f, 0.32f);
				beamLevel.Nodes[12][0] = new Vector2(0.65f, 0.17f);
				beamLevel.Nodes[12][1] = new Vector2(0.575f, 0.245f);
				beamLevel.Nodes[13][0] = new Vector2(1.025f, 0.17f);
				beamLevel.Nodes[13][1] = new Vector2(1.1f, 0.245f);
				beamLevel.Packets[0][0] = new Vector2(-0.089017f, 0.234017f);
				beamLevel.Packets[0][1] = new Vector2(-0.089017f, 0.180983f);
				beamLevel.Packets[0][2] = new Vector2(-0.035983f, 0.180983f);
				beamLevel.Packets[0][3] = new Vector2(-0.035983f, 0.234017f);
				beamLevel.Packets[1][0] = new Vector2(0.360983f, 0.309017f);
				beamLevel.Packets[1][1] = new Vector2(0.360983f, 0.255983f);
				beamLevel.Packets[1][2] = new Vector2(0.414017f, 0.255983f);
				beamLevel.Packets[1][3] = new Vector2(0.414017f, 0.309017f);
				beamLevel.Packets[2][0] = new Vector2(0.510983f, 0.309017f);
				beamLevel.Packets[2][1] = new Vector2(0.510983f, 0.255983f);
				beamLevel.Packets[2][2] = new Vector2(0.564017f, 0.255983f);
				beamLevel.Packets[2][3] = new Vector2(0.564017f, 0.309017f);
				beamLevel.Packets[3][0] = new Vector2(0.660983f, 0.834017f);
				beamLevel.Packets[3][1] = new Vector2(0.660983f, 0.780983f);
				beamLevel.Packets[3][2] = new Vector2(0.714017f, 0.780983f);
				beamLevel.Packets[3][3] = new Vector2(0.714017f, 0.834017f);
				beamLevel.Packets[4][0] = new Vector2(1.035983f, 0.609017f);
				beamLevel.Packets[4][1] = new Vector2(1.035983f, 0.555983f);
				beamLevel.Packets[4][2] = new Vector2(1.089017f, 0.555983f);
				beamLevel.Packets[4][3] = new Vector2(1.089017f, 0.609017f);
				beamLevel.Packets[5][0] = new Vector2(1.035983f, 0.459017f);
				beamLevel.Packets[5][1] = new Vector2(1.035983f, 0.405983f);
				beamLevel.Packets[5][2] = new Vector2(1.089017f, 0.405983f);
				beamLevel.Packets[5][3] = new Vector2(1.089017f, 0.459017f);
				beamLevel.Firewalls[0][0] = new Vector2(-0.014017f, 0.609017f);
				beamLevel.Firewalls[0][1] = new Vector2(-0.014017f, 0.555983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.039017f, 0.555983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.039017f, 0.609017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.435983f, 0.309017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.435983f, 0.255983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.489017f, 0.255983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.489017f, 0.309017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.435983f, 0.459017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.435983f, 0.405983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.489017f, 0.405983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.489017f, 0.459017f);
				beamLevel.Firewalls[3][0] = new Vector2(0.510983f, 0.834017f);
				beamLevel.Firewalls[3][1] = new Vector2(0.510983f, 0.780983f);
				beamLevel.Firewalls[3][2] = new Vector2(0.564017f, 0.780983f);
				beamLevel.Firewalls[3][3] = new Vector2(0.564017f, 0.834017f);
				beamLevel.Firewalls[4][0] = new Vector2(0.885983f, 0.534017f);
				beamLevel.Firewalls[4][1] = new Vector2(0.885983f, 0.480983f);
				beamLevel.Firewalls[4][2] = new Vector2(0.939017f, 0.480983f);
				beamLevel.Firewalls[4][3] = new Vector2(0.939017f, 0.534017f);
				beamLevel.Proxies[0][0] = new Vector2(-0.089017f, 0.609017f);
				beamLevel.Proxies[0][1] = new Vector2(-0.089017f, 0.555984f);
				beamLevel.Proxies[0][2] = new Vector2(-0.035983f, 0.555984f);
				beamLevel.Proxies[0][3] = new Vector2(-0.035983f, 0.609017f);
				break;
			case 13:
				beamLevel.MaxNodes = 15;
				beamLevel.MaxRotatingNodes = 9;
				beamLevel.MaxPackets = 10;
				beamLevel.MaxFirewalls = 7;
				beamLevel.MaxInnerWalls = 9;
				beamLevel.MaxProxies = 1;
				vBeam[0].Origin = new Vector2(0.05f, 0.7325f);
				vBeam[1].Origin = new Vector2(0.125f, 0.7325f);
				beamLevel.OuterWalls[0] = new Vector2(-0.1f, 0.845f);
				beamLevel.OuterWalls[1] = new Vector2(-0.1f, 0.17f);
				beamLevel.OuterWalls[2] = new Vector2(1.1f, 0.17f);
				beamLevel.OuterWalls[3] = new Vector2(1.1f, 0.845f);
				beamLevel.InnerWalls[0][0] = new Vector2(-0.1f, 0.845f);
				beamLevel.InnerWalls[0][1] = new Vector2(-0.1f, 0.62f);
				beamLevel.InnerWalls[0][2] = new Vector2(0.05f, 0.62f);
				beamLevel.InnerWalls[0][3] = new Vector2(0.05f, 0.845f);
				beamLevel.InnerWalls[1][0] = new Vector2(0.05f, 0.395f);
				beamLevel.InnerWalls[1][1] = new Vector2(0.05f, 0.32f);
				beamLevel.InnerWalls[1][2] = new Vector2(0.125f, 0.32f);
				beamLevel.InnerWalls[1][3] = new Vector2(0.125f, 0.395f);
				beamLevel.InnerWalls[2][0] = new Vector2(0.125f, 0.32f);
				beamLevel.InnerWalls[2][1] = new Vector2(0.125f, 0.17f);
				beamLevel.InnerWalls[2][2] = new Vector2(0.275f, 0.17f);
				beamLevel.InnerWalls[2][3] = new Vector2(0.275f, 0.32f);
				beamLevel.InnerWalls[3][0] = new Vector2(0.35f, 0.545f);
				beamLevel.InnerWalls[3][1] = new Vector2(0.35f, 0.395f);
				beamLevel.InnerWalls[3][2] = new Vector2(0.425f, 0.395f);
				beamLevel.InnerWalls[3][3] = new Vector2(0.425f, 0.545f);
				beamLevel.InnerWalls[4][0] = new Vector2(0.5f, 0.62f);
				beamLevel.InnerWalls[4][1] = new Vector2(0.5f, 0.47f);
				beamLevel.InnerWalls[4][2] = new Vector2(0.65f, 0.47f);
				beamLevel.InnerWalls[4][3] = new Vector2(0.65f, 0.62f);
				beamLevel.InnerWalls[5][0] = new Vector2(0.725f, 0.62f);
				beamLevel.InnerWalls[5][1] = new Vector2(0.725f, 0.395f);
				beamLevel.InnerWalls[5][2] = new Vector2(0.8f, 0.395f);
				beamLevel.InnerWalls[5][3] = new Vector2(0.8f, 0.62f);
				beamLevel.InnerWalls[6][0] = new Vector2(0.875f, 0.695f);
				beamLevel.InnerWalls[6][1] = new Vector2(0.875f, 0.545f);
				beamLevel.InnerWalls[6][2] = new Vector2(0.95f, 0.545f);
				beamLevel.InnerWalls[6][3] = new Vector2(0.95f, 0.695f);
				beamLevel.InnerWalls[7][0] = new Vector2(0.875f, 0.545f);
				beamLevel.InnerWalls[7][1] = new Vector2(0.875f, 0.32f);
				beamLevel.InnerWalls[7][2] = new Vector2(1.025f, 0.32f);
				beamLevel.InnerWalls[7][3] = new Vector2(1.025f, 0.545f);
				beamLevel.InnerWalls[8][0] = new Vector2(0.875f, 0.845f);
				beamLevel.InnerWalls[8][1] = new Vector2(0.875f, 0.77f);
				beamLevel.InnerWalls[8][2] = new Vector2(1.025f, 0.77f);
				beamLevel.InnerWalls[8][3] = new Vector2(1.025f, 0.845f);
				beamLevel.Nodes[0][0] = new Vector2(1.035983f, 0.759017f);
				beamLevel.Nodes[0][1] = new Vector2(1.089017f, 0.705984f);
				beamLevel.Nodes[1][0] = new Vector2(0.339017f, 0.834017f);
				beamLevel.Nodes[1][1] = new Vector2(0.285983f, 0.780984f);
				beamLevel.Nodes[2][0] = new Vector2(-0.035983f, 0.609017f);
				beamLevel.Nodes[2][1] = new Vector2(-0.089017f, 0.555984f);
				beamLevel.Nodes[3][0] = new Vector2(0.275f, 0.5825f);
				beamLevel.Nodes[3][1] = new Vector2(0.35f, 0.582501f);
				beamLevel.Nodes[4][0] = new Vector2(0.6875f, 0.545001f);
				beamLevel.Nodes[4][1] = new Vector2(0.6875f, 0.470001f);
				beamLevel.Nodes[5][0] = new Vector2(0.8f, 0.5075f);
				beamLevel.Nodes[5][1] = new Vector2(0.875f, 0.507501f);
				beamLevel.Nodes[6][0] = new Vector2(0.264017f, 0.384017f);
				beamLevel.Nodes[6][1] = new Vector2(0.210983f, 0.330984f);
				beamLevel.Nodes[7][0] = new Vector2(0.65f, 0.2075f);
				beamLevel.Nodes[7][1] = new Vector2(0.725f, 0.207501f);
				beamLevel.Nodes[8][0] = new Vector2(0.5f, 0.8075f);
				beamLevel.Nodes[8][1] = new Vector2(0.575f, 0.807501f);
				beamLevel.Nodes[9][0] = new Vector2(0.35f, 0.17f);
				beamLevel.Nodes[9][1] = new Vector2(0.275f, 0.245f);
				beamLevel.Nodes[10][0] = new Vector2(0.8f, 0.845f);
				beamLevel.Nodes[10][1] = new Vector2(0.875f, 0.77f);
				beamLevel.Nodes[11][0] = new Vector2(1.025f, 0.17f);
				beamLevel.Nodes[11][1] = new Vector2(1.1f, 0.245f);
				beamLevel.Nodes[12][0] = new Vector2(0.05f, 0.32f);
				beamLevel.Nodes[12][1] = new Vector2(0.125f, 0.245f);
				beamLevel.Nodes[13][0] = new Vector2(-0.025f, 0.17f);
				beamLevel.Nodes[13][1] = new Vector2(-0.1f, 0.245f);
				beamLevel.Nodes[14][0] = new Vector2(-0.025f, 0.17f);
				beamLevel.Nodes[14][1] = new Vector2(0.05f, 0.245f);
				beamLevel.Packets[0][0] = new Vector2(0.060983f, 0.234017f);
				beamLevel.Packets[0][1] = new Vector2(0.060983f, 0.180983f);
				beamLevel.Packets[0][2] = new Vector2(0.114017f, 0.180983f);
				beamLevel.Packets[0][3] = new Vector2(0.114017f, 0.234017f);
				beamLevel.Packets[1][0] = new Vector2(-0.089017f, 0.309017f);
				beamLevel.Packets[1][1] = new Vector2(-0.089017f, 0.255983f);
				beamLevel.Packets[1][2] = new Vector2(-0.035983f, 0.255983f);
				beamLevel.Packets[1][3] = new Vector2(-0.035983f, 0.309017f);
				beamLevel.Packets[2][0] = new Vector2(0.060983f, 0.684017f);
				beamLevel.Packets[2][1] = new Vector2(0.060983f, 0.630983f);
				beamLevel.Packets[2][2] = new Vector2(0.114017f, 0.630983f);
				beamLevel.Packets[2][3] = new Vector2(0.114017f, 0.684017f);
				beamLevel.Packets[3][0] = new Vector2(0.060983f, 0.834017f);
				beamLevel.Packets[3][1] = new Vector2(0.060983f, 0.780983f);
				beamLevel.Packets[3][2] = new Vector2(0.114017f, 0.780983f);
				beamLevel.Packets[3][3] = new Vector2(0.114017f, 0.834017f);
				beamLevel.Packets[4][0] = new Vector2(0.135983f, 0.834017f);
				beamLevel.Packets[4][1] = new Vector2(0.135983f, 0.780983f);
				beamLevel.Packets[4][2] = new Vector2(0.189017f, 0.780983f);
				beamLevel.Packets[4][3] = new Vector2(0.189017f, 0.834017f);
				beamLevel.Packets[5][0] = new Vector2(0.510983f, 0.684017f);
				beamLevel.Packets[5][1] = new Vector2(0.510983f, 0.630983f);
				beamLevel.Packets[5][2] = new Vector2(0.564017f, 0.630983f);
				beamLevel.Packets[5][3] = new Vector2(0.564017f, 0.684017f);
				beamLevel.Packets[6][0] = new Vector2(0.435983f, 0.384017f);
				beamLevel.Packets[6][1] = new Vector2(0.435983f, 0.330983f);
				beamLevel.Packets[6][2] = new Vector2(0.489017f, 0.330983f);
				beamLevel.Packets[6][3] = new Vector2(0.489017f, 0.384017f);
				beamLevel.Packets[7][0] = new Vector2(0.660983f, 0.459017f);
				beamLevel.Packets[7][1] = new Vector2(0.660983f, 0.405983f);
				beamLevel.Packets[7][2] = new Vector2(0.714017f, 0.405983f);
				beamLevel.Packets[7][3] = new Vector2(0.714017f, 0.459017f);
				beamLevel.Packets[8][0] = new Vector2(0.810983f, 0.459017f);
				beamLevel.Packets[8][1] = new Vector2(0.810983f, 0.405983f);
				beamLevel.Packets[8][2] = new Vector2(0.864017f, 0.405983f);
				beamLevel.Packets[8][3] = new Vector2(0.864017f, 0.459017f);
				beamLevel.Packets[9][0] = new Vector2(1.035983f, 0.834017f);
				beamLevel.Packets[9][1] = new Vector2(1.035983f, 0.780983f);
				beamLevel.Packets[9][2] = new Vector2(1.089017f, 0.780983f);
				beamLevel.Packets[9][3] = new Vector2(1.089017f, 0.834017f);
				beamLevel.Firewalls[0][0] = new Vector2(0.135983f, 0.684017f);
				beamLevel.Firewalls[0][1] = new Vector2(0.135983f, 0.630983f);
				beamLevel.Firewalls[0][2] = new Vector2(0.189017f, 0.630983f);
				beamLevel.Firewalls[0][3] = new Vector2(0.189017f, 0.684017f);
				beamLevel.Firewalls[1][0] = new Vector2(0.435983f, 0.609017f);
				beamLevel.Firewalls[1][1] = new Vector2(0.435983f, 0.555983f);
				beamLevel.Firewalls[1][2] = new Vector2(0.489017f, 0.555983f);
				beamLevel.Firewalls[1][3] = new Vector2(0.489017f, 0.609017f);
				beamLevel.Firewalls[2][0] = new Vector2(0.660983f, 0.609017f);
				beamLevel.Firewalls[2][1] = new Vector2(0.660983f, 0.555983f);
				beamLevel.Firewalls[2][2] = new Vector2(0.714017f, 0.555983f);
				beamLevel.Firewalls[2][3] = new Vector2(0.714017f, 0.609017f);
				beamLevel.Firewalls[3][0] = new Vector2(0.735983f, 0.384017f);
				beamLevel.Firewalls[3][1] = new Vector2(0.735983f, 0.330983f);
				beamLevel.Firewalls[3][2] = new Vector2(0.789017f, 0.330983f);
				beamLevel.Firewalls[3][3] = new Vector2(0.789017f, 0.384017f);
				beamLevel.Firewalls[4][0] = new Vector2(-0.014017f, 0.459017f);
				beamLevel.Firewalls[4][1] = new Vector2(-0.014017f, 0.405983f);
				beamLevel.Firewalls[4][2] = new Vector2(0.039017f, 0.405983f);
				beamLevel.Firewalls[4][3] = new Vector2(0.039017f, 0.459017f);
				beamLevel.Firewalls[5][0] = new Vector2(-0.014017f, 0.534017f);
				beamLevel.Firewalls[5][1] = new Vector2(-0.014017f, 0.480983f);
				beamLevel.Firewalls[5][2] = new Vector2(0.039017f, 0.480983f);
				beamLevel.Firewalls[5][3] = new Vector2(0.039017f, 0.534017f);
				beamLevel.Firewalls[6][0] = new Vector2(0.585983f, 0.459017f);
				beamLevel.Firewalls[6][1] = new Vector2(0.585983f, 0.405983f);
				beamLevel.Firewalls[6][2] = new Vector2(0.639017f, 0.405983f);
				beamLevel.Firewalls[6][3] = new Vector2(0.639017f, 0.459017f);
				beamLevel.Proxies[0][0] = new Vector2(-0.014017f, 0.309017f);
				beamLevel.Proxies[0][1] = new Vector2(-0.014017f, 0.255984f);
				beamLevel.Proxies[0][2] = new Vector2(0.039017f, 0.255984f);
				beamLevel.Proxies[0][3] = new Vector2(0.039017f, 0.309017f);
				break;
			}
			LevelWalls.Add(new List<Vector2>
			{
				beamLevel.OuterWalls[0],
				beamLevel.OuterWalls[1],
				beamLevel.OuterWalls[2],
				beamLevel.OuterWalls[3]
			});
			for (int i = 0; i < beamLevel.InnerWalls.Length; i++)
			{
				LevelWalls.Add(new List<Vector2>
				{
					beamLevel.InnerWalls[i][0],
					beamLevel.InnerWalls[i][1],
					beamLevel.InnerWalls[i][2],
					beamLevel.InnerWalls[i][3]
				});
			}
			for (int j = 0; j < beamLevel.MaxPackets; j++)
			{
				Vector2 orig = (beamLevel.Packets[j][1] + beamLevel.Packets[j][3]) / 2f;
				LevelNodes.Add(new Node(orig, new List<Vector2>
				{
					beamLevel.Packets[j][0],
					beamLevel.Packets[j][1],
					beamLevel.Packets[j][2],
					beamLevel.Packets[j][3]
				}, 0f, NodeType.Packet));
			}
			for (int k = 0; k < beamLevel.MaxFirewalls; k++)
			{
				Vector2 orig2 = (beamLevel.Firewalls[k][1] + beamLevel.Firewalls[k][3]) / 2f;
				LevelNodes.Add(new Node(orig2, new List<Vector2>
				{
					beamLevel.Firewalls[k][0],
					beamLevel.Firewalls[k][1],
					beamLevel.Firewalls[k][2],
					beamLevel.Firewalls[k][3]
				}, 0f, NodeType.Firewall));
			}
			if (beamLevel.MaxProxies != 0)
			{
				for (int l = 0; l < beamLevel.MaxProxies; l++)
				{
					Vector2 orig3 = (beamLevel.Proxies[l][0] + beamLevel.Proxies[l][2]) / 2f;
					LevelNodes.Add(new Node(orig3, new List<Vector2>
					{
						beamLevel.Proxies[l][0],
						beamLevel.Proxies[l][1],
						beamLevel.Proxies[l][2],
						beamLevel.Proxies[l][3]
					}, 0f, NodeType.Prism));
				}
			}
			for (int m = 0; m < beamLevel.MaxRotatingNodes; m++)
			{
				Vector2 orig4 = (beamLevel.Nodes[m][0] + beamLevel.Nodes[m][1]) / 2f;
				float ang = Function.Call<float>(Hash.ATAN2, beamLevel.Nodes[m][1].Y - beamLevel.Nodes[m][0].Y, beamLevel.Nodes[m][1].X - beamLevel.Nodes[m][0].X);
				LevelNodes.Add(new Node(orig4, new List<Vector2>
				{
					beamLevel.Nodes[m][0],
					beamLevel.Nodes[m][1]
				}, ang, NodeType.Rotating_Mirror));
			}
			for (int n = beamLevel.MaxRotatingNodes; n < beamLevel.MaxNodes; n++)
			{
				Vector2 orig5 = (beamLevel.Nodes[n][0] + beamLevel.Nodes[n][1]) / 2f;
				float ang2 = Function.Call<float>(Hash.ATAN2, beamLevel.Nodes[n][1].Y - beamLevel.Nodes[n][0].Y, beamLevel.Nodes[n][1].X - beamLevel.Nodes[n][0].X);
				LevelNodes.Add(new Node(orig5, new List<Vector2>
				{
					beamLevel.Nodes[n][0],
					beamLevel.Nodes[n][1]
				}, ang2, NodeType.Static_Mirror));
			}
			CurrentPackets = beamLevel.MaxPackets;
		}

		public static int GetSoundId()
		{
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}

		public static bool HasSoundFinished(int ID)
		{
			return Function.Call<bool>(Hash.HAS_SOUND_FINISHED, ID);
		}

		public static void PlaySoundFrontend(int ID, string Audio, string AudioRef)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, ID, Audio, AudioRef, true);
		}

		public static bool IsAudioSceneActive(string Scene)
		{
			return Function.Call<bool>(Hash.IS_AUDIO_SCENE_ACTIVE, Scene);
		}

		public static void StartAudioScene(string Scene)
		{
			Function.Call(Hash.START_AUDIO_SCENE, Scene);
		}

		public static void SetTextScale(float scale, float size)
		{
			Function.Call(Hash.SET_TEXT_SCALE, scale, size);
		}

		public static void SetTextColour(int red, int green, int blue, int alpha)
		{
			Function.Call(Hash.SET_TEXT_COLOUR, red, green, blue, alpha);
		}

		public static void SetTextCentre(bool Align)
		{
			Function.Call(Hash.SET_TEXT_CENTRE, Align);
		}

		public static void BeginTextCommandDisplayText(string Text)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, Text);
		}

		public static void EndTextCommandDisplayText(float x, float y)
		{
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0);
		}

		public static void AddTextComponentInteger(int value)
		{
			Function.Call(Hash.ADD_TEXT_COMPONENT_INTEGER, value);
		}

		public static void ResetBeamHack()
		{
			StopBeamhackSounds();
			GUIPosX = 0.5f;
			GUIPosY = 0.5f;
			vCursor = new Vector2(0.5f, 0.5f);
			BeamhackResetIn = 0;
			BeamHackOn = false;
			BeamhackLives = 3;
			BeamRotation = 0f;
			BeamLength = 0.25f;
			BeamLengthInt = 1;
			vBeam.Clear();
			LevelWalls.Clear();
			for (int i = 1; i < ciBH_MAX_BEAM_SEGMENTS; i++)
			{
				vBeam.Add(new Beam(Vector3.Zero, false, 0f));
			}
			vBeam[0] = new Beam(new Vector2(0.5f, 0.5f), false, 0f);
			vBeam[1] = new Beam(new Vector2(0.65f, 0.5f), false, 0f);
			LevelNodes.Clear();
			GetBeamHacklevelData(BeamLevel);
			List<Node> list = new List<Node>();
			List<Node> list2 = new List<Node>();
			List<Node> list3 = new List<Node>();
			List<Node> list4 = new List<Node>();
			List<Node> list5 = new List<Node>();
			foreach (Node levelNode in LevelNodes)
			{
				if (levelNode.NodeType == NodeType.Packet)
				{
					list2.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Prism)
				{
					list4.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Firewall)
				{
					list3.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Rotating_Mirror)
				{
					list.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Static_Mirror)
				{
					list5.Add(levelNode);
				}
			}
			StartTime = Game.GameTime;
			BeamHackOn = true;
		}

		public static void BeamHackNewLevel(int Level)
		{
			StopBeamhackSounds();
			GUIPosX = 0.5f;
			GUIPosY = 0.5f;
			vCursor = new Vector2(0.5f, 0.5f);
			BeamLevel = Level;
			BeamhackResetIn = 0;
			BeamHackOn = false;
			BeamhackLives = 3;
			BeamRotation = 0f;
			BeamLength = 0.25f;
			BeamLengthInt = 1;
			vBeam.Clear();
			LevelWalls.Clear();
			for (int i = 1; i < ciBH_MAX_BEAM_SEGMENTS; i++)
			{
				vBeam.Add(new Beam(Vector3.Zero, false, 0f));
			}
			vBeam[0] = new Beam(new Vector2(0.5f, 0.5f), false, 0f);
			vBeam[1] = new Beam(new Vector2(0.65f, 0.5f), false, 0f);
			LevelNodes.Clear();
			GetBeamHacklevelData(BeamLevel);
			List<Node> list = new List<Node>();
			List<Node> list2 = new List<Node>();
			List<Node> list3 = new List<Node>();
			List<Node> list4 = new List<Node>();
			List<Node> list5 = new List<Node>();
			foreach (Node levelNode in LevelNodes)
			{
				if (levelNode.NodeType == NodeType.Packet)
				{
					list2.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Prism)
				{
					list4.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Firewall)
				{
					list3.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Rotating_Mirror)
				{
					list.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Static_Mirror)
				{
					list5.Add(levelNode);
				}
			}
			StartTime = Game.GameTime;
			BeamHackOn = true;
		}

		private void DRAW_BIG_HACKING_TEXT(string sParam0, int iParam1, int iParam2, int iParam3, int iParam4, float fParam5, float fParam6, float fParam7, float fParam8)
		{
			Function.Call(Hash.SET_TEXT_SCALE, fParam5, fParam6);
			float num = Function.Call<int>(Hash.GET_GAME_TIMER);
			num = Function.Call<float>(Hash.ABSF, Function.Call<float>(Hash.SIN, num * 0.35f));
			iParam4 = 255 - Function.Call<int>(Hash.ROUND, num) * 100;
			Function.Call(Hash.SET_TEXT_COLOUR, iParam1, iParam2, iParam3, iParam4);
			Function.Call(Hash.SET_TEXT_CENTRE, true);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_TEXT_LABEL, sParam0);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, GetRatio(fParam7), fParam8, 0);
		}

		public static void StopBeamhackSounds()
		{
			if (!HasSoundFinished(BeamHackBackgroundLoopSoundId))
			{
				Function.Call(Hash.STOP_SOUND, BeamHackBackgroundLoopSoundId);
				Function.Call(Hash.RELEASE_SOUND_ID, BeamHackBackgroundLoopSoundId);
			}
			if (!HasSoundFinished(BeamHackBlueChargeLoopSoundId))
			{
				Function.Call(Hash.STOP_SOUND, BeamHackBlueChargeLoopSoundId);
				Function.Call(Hash.RELEASE_SOUND_ID, BeamHackBlueChargeLoopSoundId);
			}
			if (!HasSoundFinished(BeamHackRedChargeLoopSoundId))
			{
				Function.Call(Hash.STOP_SOUND, BeamHackRedChargeLoopSoundId);
				Function.Call(Hash.RELEASE_SOUND_ID, BeamHackRedChargeLoopSoundId);
			}
			if (!HasSoundFinished(BeamHackCursorLoopSoundId))
			{
				Function.Call(Hash.STOP_SOUND, BeamHackCursorLoopSoundId);
				Function.Call(Hash.RELEASE_SOUND_ID, BeamHackCursorLoopSoundId);
			}
			if (!HasSoundFinished(BeamHackRotateMirrorLoopSoundId))
			{
				Function.Call(Hash.STOP_SOUND, BeamHackRotateMirrorLoopSoundId);
				Function.Call(Hash.RELEASE_SOUND_ID, BeamHackRotateMirrorLoopSoundId);
			}
			BeamHackBackgroundLoopSoundId = -1;
			BeamHackBackgroundLoopSoundId = -1;
			BeamHackBlueChargeLoopSoundId = -1;
			BeamHackRotateMirrorLoopSoundId = -1;
			BeamHackRedChargeLoopSoundId = -1;
			if (IsAudioSceneActive("DLC_XM17_Silo_Laser_Hack_Minigame_Scene"))
			{
				Function.Call(Hash.STOP_AUDIO_SCENE, "DLC_XM17_Silo_Laser_Hack_Minigame_Scene");
			}
		}

		public static void RunBeamHack()
		{
			BeamHacklevelAt = 1;
			StopBeamhackSounds();
			GUIPosX = 0.5f;
			GUIPosY = 0.5f;
			vCursor = new Vector2(0.5f, 0.5f);
			BeamLevel = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
			BeamhackResetIn = 0;
			BeamhackLives = 3;
			BeamRotation = 0f;
			BeamLength = 0.25f;
			BeamLengthInt = 1;
			vBeam.Clear();
			LevelWalls.Clear();
			for (int i = 1; i < ciBH_MAX_BEAM_SEGMENTS; i++)
			{
				vBeam.Add(new Beam(Vector3.Zero, false, 0f));
			}
			vBeam[0] = new Beam(new Vector2(0.5f, 0.5f), false, 0f);
			vBeam[1] = new Beam(new Vector2(0.65f, 0.5f), false, 0f);
			LevelNodes.Clear();
			GetBeamHacklevelData(BeamLevel);
			List<Node> list = new List<Node>();
			List<Node> list2 = new List<Node>();
			List<Node> list3 = new List<Node>();
			List<Node> list4 = new List<Node>();
			List<Node> list5 = new List<Node>();
			foreach (Node levelNode in LevelNodes)
			{
				if (levelNode.NodeType == NodeType.Packet)
				{
					list2.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Prism)
				{
					list4.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Firewall)
				{
					list3.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Rotating_Mirror)
				{
					list.Add(levelNode);
				}
				if (levelNode.NodeType == NodeType.Static_Mirror)
				{
					list5.Add(levelNode);
				}
			}
			StartTime = Game.GameTime;
			BeamHackOn = true;
		}

		public static void ToggleTrackify()
		{
			if (!isTrackifyVisible)
			{
				trackify = new Scaleform("APP_TRACKIFY");
				trackify.CallFunction("SET_HEADER", "Trackify");
				trackify.CallFunction("setupMainScreenSkipLoading");
				SetupTarget();
				isTrackifyVisible = true;
			}
			else
			{
				trackify.CallFunction("CLOSE_APP");
				trackify.Dispose();
				isTrackifyVisible = false;
			}
		}

		private static void SetupTarget()
		{
			int num = 10;
			Vector3 vector = new Vector3(170.0896f, 664.696f, 206.6928f);
			float distance = GetDistance(Game.Player.Character.Position, vector);
			float num2 = (vector - Game.Player.Character.Position).ToHeading() - Game.Player.Character.Heading;
			float num3 = num2;
			Vector3.Angle(vector, Game.Player.Character.Position);
			if (num2 <= -1f)
			{
				num3 = 360f + num2;
			}
			num3 = 360f - num3;
			int num4 = 0;
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, trackify.Handle, "setTargetByID");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num4);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)num3);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)distance);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			trackify.CallFunction("animateSweep");
		}

		public static void ShowTrackify()
		{
			if (isTrackifyVisible && trackify != null)
			{
				OutputArgument outputArgument = new OutputArgument();
				Function.Call(Hash.GET_MOBILE_PHONE_RENDER_ID, outputArgument);
				int result = outputArgument.GetResult<int>();
				Function.Call(Hash.SET_TEXT_RENDER_ID, result);
				trackify.Render2DScreenSpace(new PointF(0f, 0f), new PointF(260f, 231f));
				Function.Call(Hash.SET_TEXT_RENDER_ID, 1);
				int trackifySequence = TrackifySequence;
				if ((float)Game.GameTime > sweepTime)
				{
					trackify.CallFunction("animateSweep");
					sweepTime = Game.GameTime + 2000;
				}
			}
		}

		public void onTick(object sender, EventArgs e)
		{
			DrawnIDsThisFrame.Clear();
			if (Game.Player.Name.ToUpper().Equals(AdminName) && Game.IsControlPressed(Control.SelectWeapon))
			{
				drawText("Instances of Gun Locker " + InstanceGunlocker, 0.1f, 0.1f, 0.5f, 255, 255, 255);
				drawText("Instances of Save Car " + InstanceSaveCar, 0.1f, 0.15f, 0.5f, 255, 255, 255);
			}
			if (CleanupHacksID == 1)
			{
				CleanupHacks();
				CleanupHacksID = 0;
			}
			if (!BeamHackOn && !PCBHackOn && !FiveLetterPasscodeHack && CircuitHack != 1 && BruteforceHack != 1 && FIBHack != 1 && DataHack != 1 && VoltageHack != 1 && CopyPrintHack != 1 && PadLockHack != 1 && HotwireHack != 1 && !VideoPlaying && !GUIOn && !KeypadHackOn && !PlayingButtonPressMinigame && !FingerprintHackOn && !CopyPrintHackOn && !PlayingVoltageMG && !RunningPadLockMinigame && !HotWireMinigameOn && !DataCrackOn && !FIBHackOn && !BruteForceHackOn && !CircuitHackOn && !SecuroServPhoneHackOn && !AppTrackifyOn)
			{
				if (IsRadarOff)
				{
					Function.Call(Hash.DISPLAY_RADAR, true);
					IsRadarOff = false;
				}
				return;
			}
			if (BeamHackOn)
			{
				Game.Player.CanControlCharacter = false;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
				{
					if (Game.IsControlJustPressed(Control.Context))
					{
						HackFinishedID = 3;
						Game.Player.CanControlCharacter = true;
						StopBeamhackSounds();
						BeamHackOn = false;
						return;
					}
				}
				else if (Game.IsControlJustPressed(Control.Cover))
				{
					HackFinishedID = 3;
					Game.Player.CanControlCharacter = true;
					StopBeamhackSounds();
					BeamHackOn = false;
					return;
				}
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
				if (BeamHackBackgroundLoopSoundId == -1)
				{
					BeamHackBackgroundLoopSoundId = GetSoundId();
				}
				if (BeamHackCursorLoopSoundId == -1)
				{
					BeamHackCursorLoopSoundId = GetSoundId();
				}
				if (BeamHackBlueChargeLoopSoundId == -1)
				{
					BeamHackBlueChargeLoopSoundId = GetSoundId();
				}
				if (BeamHackRedChargeLoopSoundId == -1)
				{
					BeamHackRedChargeLoopSoundId = GetSoundId();
				}
				if (BeamHackRotateMirrorLoopSoundId == -1)
				{
					BeamHackRotateMirrorLoopSoundId = GetSoundId();
				}
				if (HasSoundFinished(BeamHackBackgroundLoopSoundId))
				{
					PlaySoundFrontend(BeamHackBackgroundLoopSoundId, "Background", "dlc_xm_silo_laser_hack_sounds");
				}
				if (HasSoundFinished(BeamHackBlueChargeLoopSoundId))
				{
					PlaySoundFrontend(BeamHackBlueChargeLoopSoundId, "Blue_Target_Charge", "dlc_xm_silo_laser_hack_sounds");
				}
				if (HasSoundFinished(BeamHackRedChargeLoopSoundId))
				{
					PlaySoundFrontend(BeamHackRedChargeLoopSoundId, "Red_Target_Charge", "dlc_xm_silo_laser_hack_sounds");
				}
				if (HasSoundFinished(BeamHackCursorLoopSoundId))
				{
					PlaySoundFrontend(BeamHackCursorLoopSoundId, "Cursor_Move", "dlc_xm_silo_laser_hack_sounds");
				}
				if (HasSoundFinished(BeamHackRotateMirrorLoopSoundId))
				{
					PlaySoundFrontend(BeamHackRotateMirrorLoopSoundId, "Rotate_Mirror", "dlc_xm_silo_laser_hack_sounds");
				}
				if (!IsAudioSceneActive("DLC_XM17_Silo_Laser_Hack_Minigame_Scene"))
				{
					StartAudioScene("DLC_XM17_Silo_Laser_Hack_Minigame_Scene");
				}
				while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "MPBeamHack"))
				{
					Script.Wait(10);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPBeamHack", 0);
				}
				while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "MPBeamHackFG"))
				{
					Script.Wait(10);
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPBeamHackFG", 0);
				}
				for (int i = 0; i < 15; i++)
				{
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "MPBeamHack_lvl" + i))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPBeamHack_lvl" + i, 0);
					}
				}
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CHRISTMAS2017/XM_Silo_Laser_Hack");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CHRISTMAS2017/XM_Silo_Laser_Hack", false, -1);
				Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_CHRISTMAS2017/XM_Silo_Laser_Hack");
				Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_CHRISTMAS2017/XM_Silo_Laser_Hack", false, -1);
				string beamHackTextureDictForLevel = GetBeamHackTextureDictForLevel(BeamLevel);
				string beamHackTextureBgForLevel = GetBeamHackTextureBgForLevel(BeamLevel);
				string beamHackTextureFgForLevel = GetBeamHackTextureFgForLevel(BeamLevel);
				drawSprite3(beamHackTextureDictForLevel, beamHackTextureBgForLevel, 0.5f, 0.5075f, 0.675f, 0.685f, 0f, 255, 255, 255, 255, 0);
				drawSprite3(beamHackTextureDictForLevel, beamHackTextureFgForLevel, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
				drawSprite3("MPBeamHack", "border", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
				int num = 171;
				int num2 = 252;
				int num3 = 255;
				int num4 = 171;
				int num5 = 252;
				int num6 = 255;
				int num7 = 0;
				int num8 = 255;
				int num9 = 127;
				float num10 = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
				Vector2 zero = Vector2.Zero;
				BeamFxFrame++;
				if (BeamFxFrame >= 5)
				{
					BeamFxFrame = 1;
				}
				Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
				Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
				int num11 = Game.GameTime - StartTime;
				float num12 = Function.Call<float>(Hash.COS, (double)Function.Call<float>(Hash.ABSI, (float)num11) * (Math.PI * 2.0)) * cfBH_NODEVIBRADIUS;
				Function.Call<float>(Hash.SIN, (double)Function.Call<float>(Hash.ABSI, (float)num11) * (Math.PI * 2.0));
				float cfBH_NODEVIBRADIU = cfBH_NODEVIBRADIUS;
				Function.Call<float>(Hash.SIN, Function.Call<float>(Hash.ABSI, (float)num11) * 0.5f);
				Intersectdist = 1f;
				List<Node> list = new List<Node>();
				List<Node> list2 = new List<Node>();
				List<Node> list3 = new List<Node>();
				List<Node> list4 = new List<Node>();
				List<Node> list5 = new List<Node>();
				List<Node> list6 = new List<Node>();
				for (int j = 1; j < LevelWalls.Count; j++)
				{
					list6.Add(new Node(LevelWalls[j], 0f, NodeType.None));
				}
				Node node = null;
				foreach (Node levelNode in LevelNodes)
				{
					if (levelNode.NodeType == NodeType.Packet)
					{
						list2.Add(levelNode);
					}
					if (levelNode.NodeType == NodeType.Prism)
					{
						list4.Add(levelNode);
					}
					if (levelNode.NodeType == NodeType.Firewall)
					{
						list3.Add(levelNode);
					}
					if (levelNode.NodeType == NodeType.Rotating_Mirror)
					{
						list.Add(levelNode);
					}
					if (levelNode.NodeType == NodeType.Static_Mirror)
					{
						list5.Add(levelNode);
					}
				}
				if (false)
				{
					foreach (List<Vector2> levelWall in LevelWalls)
					{
						Vector2 vector = Vector2.Divide(Vector2.Add(levelWall[levelWall.Count - 1], levelWall[0]), 2f);
						float num13 = Vector2.Distance(levelWall[levelWall.Count - 1], levelWall[0]);
						double num14 = Function.Call<float>(Hash.ATAN2, levelWall[0].Y - levelWall[levelWall.Count - 1].Y, levelWall[0].X - levelWall[levelWall.Count - 1].X);
						Function.Call(Hash.DRAW_SPRITE_ARX, "MPBeamHack", "beam_middle", GetRatio(vector.X), vector.Y, 0.064 / (double)num10, num13, num14 + 90.0, num, num2, num3, num8);
						for (int k = 1; k < levelWall.Count; k++)
						{
							vector = Vector2.Divide(Vector2.Add(levelWall[k - 1], levelWall[k]), 2f);
							num13 = Vector2.Distance(levelWall[k - 1], levelWall[k]);
							num14 = Function.Call<float>(Hash.ATAN2, levelWall[k].Y - levelWall[k - 1].Y, levelWall[k].X - levelWall[k - 1].X);
							Function.Call(Hash.DRAW_SPRITE_ARX, "MPBeamHack", "beam_middle", GetRatio(vector.X), vector.Y, 0.064 / (double)num10, num13, num14 + 90.0, num, num2, num3, num8);
						}
					}
				}
				ShapeTypes shapeTypes = ShapeTypes.ST_MAX_SHAPES;
				Node node2 = null;
				Node node3 = null;
				if (CurrentPackets != 0 && BeamhackLives != 0)
				{
					float num15 = 1.25f;
					BeamLengthInt = 0;
					int num16 = -1;
					int num17 = -1;
					bool flag = false;
					for (int l = 1; l < ciBH_MAX_BEAM_SEGMENTS - 1; l++)
					{
						float num18 = 1.5f;
						float distance = 1.5f;
						Vector2 vector2 = NormaliseVector2D(SubtractVector2D(vBeam[l].Origin, vBeam[l - 1].Origin));
						int quadrantFromVector = GetQuadrantFromVector(vector2);
						bool flag2 = false;
						int num19 = -1;
						int num20 = -1;
						int num21 = -1;
						int num22 = -1;
						int num23 = 0;
						int point = 0;
						int point2 = 0;
						Vector2 v = Vector2.Zero;
						Vector2 v2 = Vector2.Zero;
						for (num23 = 0; num23 < beamLevel.MaxRotatingNodes; num23++)
						{
							if (WorthCheckingEdge(vBeam[l - 1].Origin, list[num23].Collisions[0], list[num23].Collisions[1], quadrantFromVector) && IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list[num23].Collisions[0], list[num23].Collisions[1], ref distance))
							{
								if (EnableBeamHackDebug)
								{
									drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersecting node {num23} Distance: {distance} < {num18} " + (distance < num18), 0.75f, 0.27f, 0.3f, 255, 255, 255);
								}
								if (distance < num18 && distance <= num15)
								{
									num18 = distance;
									v = list[num23].Collisions[0];
									v2 = list[num23].Collisions[1];
									flag2 = true;
									flag = true;
									num22 = num23;
									shapeTypes = ShapeTypes.ST_ROTATING_NODE;
								}
							}
						}
						int num24 = beamLevel.MaxNodes - beamLevel.MaxRotatingNodes;
						for (num23 = 0; num23 < num24; num23++)
						{
							bool flag3 = false;
							bool flag4 = false;
							bool flag5 = false;
							if (WorthCheckingEdge(vBeam[l - 1].Origin, list5[num23].Collisions[0], list5[num23].Collisions[1], quadrantFromVector))
							{
								flag3 = true;
								if (IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list5[num23].Collisions[0], list5[num23].Collisions[1], ref distance))
								{
									flag4 = true;
									if (EnableBeamHackDebug)
									{
										drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersecting node {num23} Distance: {distance} < {num18} " + (distance < num18), 0.75f, 0.27f, 0.3f, 255, 255, 255);
									}
									if (distance < num18 && distance <= num15)
									{
										flag5 = true;
										num18 = distance;
										v = list5[num23].Collisions[0];
										v2 = list5[num23].Collisions[1];
										flag2 = true;
										flag = true;
										num22 = num23;
										shapeTypes = ShapeTypes.ST_ROTATING_NODE;
									}
								}
							}
							Vector2 vector3 = new Vector2(list5[num23].Collisions[1].X, list5[num23].Collisions[0].Y);
							if (WorthCheckingEdge(vBeam[l - 1].Origin, list5[num23].Collisions[0], vector3, quadrantFromVector))
							{
								flag3 = true;
								if (IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list5[num23].Collisions[0], vector3, ref distance))
								{
									flag4 = true;
									if (EnableBeamHackDebug)
									{
										drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersecting node wall (1) {num23} Distance: {distance} < {num18} < {num18}" + (distance < num18), 0.75f, 0.3f, 0.3f, 255, 255, 255);
									}
									if (distance < num18 && distance <= num15)
									{
										flag5 = true;
										num18 = distance;
										v = list5[num23].Collisions[0];
										v2 = vector3;
										flag2 = true;
										flag = false;
										num22 = num23;
										shapeTypes = ShapeTypes.ST_NODE;
									}
								}
							}
							if (WorthCheckingEdge(vBeam[l - 1].Origin, vector3, list5[num23].Collisions[1], quadrantFromVector))
							{
								flag3 = true;
								if (IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, vector3, list5[num23].Collisions[1], ref distance))
								{
									flag4 = true;
									if (EnableBeamHackDebug)
									{
										drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersecting node wall (2) {num23} Distance: {distance} < {num18} < {num18} " + (distance < num18), 0.75f, 0.33f, 0.3f, 255, 255, 255);
									}
									if (distance < num18 && distance <= num15)
									{
										flag5 = true;
										num18 = distance;
										v = vector3;
										v2 = list5[num23].Collisions[1];
										flag2 = true;
										flag = false;
										num22 = num23;
										shapeTypes = ShapeTypes.ST_NODE;
										if (vBeam[l - 1] == vBeam[BeamLengthInt] && !PlayedBounceSound)
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hit_Mirror", "dlc_xm_silo_laser_hack_sounds");
											PlayedBounceSound = true;
										}
									}
								}
							}
							if (EnableBeamHackDebug)
							{
								drawText("(Mirror)CheckEdge\n" + flag3 + "/" + flag4 + "/" + flag5, GetRatio(list5[num23].Origin.X), list5[num23].Origin.Y, 0.3f, 255, 255, 255);
							}
						}
						for (num23 = 0; num23 < beamLevel.MaxPackets; num23++)
						{
							if (list2[num23].isDestroyed)
							{
								continue;
							}
							GetDiagonalEdgeToCheck(quadrantFromVector, ref point, ref point2);
							if (!WorthCheckingEdge(vBeam[l - 1].Origin, list2[num23].Collisions[point], list2[num23].Collisions[point2], quadrantFromVector))
							{
								continue;
							}
							if (IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list2[num23].Collisions[point], list2[num23].Collisions[point2], ref distance))
							{
								if (EnableBeamHackDebug)
								{
									drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersecting Packet {num23} Distance: {distance} < {num18} < {num18} " + (distance < num18), 0.75f, 0.36f, 0.3f, 255, 255, 255);
								}
								if (distance < num18 && distance <= num15)
								{
									num18 = distance;
									flag2 = true;
									flag = false;
									num20 = num23;
									num22 = num23;
									shapeTypes = ShapeTypes.ST_PACKET;
								}
							}
							bool enableBeamHackDebug = EnableBeamHackDebug;
						}
						for (num23 = 0; num23 < beamLevel.MaxFirewalls; num23++)
						{
							if (list3[num23].isDestroyed)
							{
								continue;
							}
							GetDiagonalEdgeToCheck(quadrantFromVector, ref point, ref point2);
							if (WorthCheckingEdge(vBeam[l - 1].Origin, list3[num23].Collisions[point], list3[num23].Collisions[point2], quadrantFromVector) && IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list3[num23].Collisions[point], list3[num23].Collisions[point2], ref distance))
							{
								if (EnableBeamHackDebug)
								{
									drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersecting Firewall {num23} Distance: {distance} < {num18} < {num18} " + (distance < num18), 0.75f, 0.39f, 0.3f, 255, 255, 255);
								}
								if (distance < num18 && distance <= num15)
								{
									num18 = distance;
									flag2 = true;
									flag = false;
									num20 = -1;
									num21 = num23;
									num22 = num23;
									shapeTypes = ShapeTypes.ST_FIREWALL;
								}
							}
							bool enableBeamHackDebug2 = EnableBeamHackDebug;
						}
						for (num23 = 0; num23 < beamLevel.MaxInnerWalls; num23++)
						{
							GetDiagonalEdgeToCheck(quadrantFromVector, ref point, ref point2);
							if (WorthCheckingEdge(vBeam[l - 1].Origin, list6[num23].Collisions[point], list6[num23].Collisions[point2], quadrantFromVector) && IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list6[num23].Collisions[point], list6[num23].Collisions[point2], ref distance))
							{
								if (EnableBeamHackDebug)
								{
									drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersect between inner wall {num23} edge {point}, {point2} Distance: {distance} < {num18} < {num18} " + (distance < num18), 0.75f, 0.1f, 0.3f, 255, 255, 255);
								}
								if (distance < num18 && distance <= num15)
								{
									num18 = distance;
									flag2 = true;
									flag = false;
									num19 = -1;
									num20 = -1;
									num21 = -1;
									num22 = num23;
									shapeTypes = ShapeTypes.ST_INNER_WALL;
								}
							}
						}
						if (beamLevel.MaxProxies >= 1)
						{
							for (num23 = 0; num23 < beamLevel.MaxProxies; num23++)
							{
								GetDiagonalEdgeToCheck(quadrantFromVector, ref point, ref point2);
								if (WorthCheckingEdge(vBeam[l - 1].Origin, list4[num23].Collisions[point], list4[num23].Collisions[point2], quadrantFromVector) && IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list4[num23].Collisions[point], list4[num23].Collisions[point2], ref distance))
								{
									if (EnableBeamHackDebug)
									{
										drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersect between proxy {num23} edge {point}, {point2} Distance: {distance} < {num18} < {num18}  " + (distance < num18), 0.75f, 0.13f, 0.3f, 255, 255, 255);
									}
									if (distance < num18 && distance <= num15)
									{
										num18 = distance;
										flag2 = true;
										flag = false;
										num19 = num23;
										num20 = -1;
										num21 = -1;
										num22 = num23;
										shapeTypes = ShapeTypes.ST_PROXY;
									}
								}
							}
						}
						if (!flag2)
						{
							for (num23 = 0; num23 < 4; num23++)
							{
								GetEdgeToCheck(num23, quadrantFromVector, ref point, ref point2, true);
								if (IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, LevelWalls[0][point], LevelWalls[0][point2], ref distance))
								{
									if (EnableBeamHackDebug)
									{
										drawText($"[JS][BeamHack] - PROCESS_BEAM - Intersect between outer walls edge {point}, {point2} Distance: {distance} < {num18} < {num18} " + (distance < num18), 0.75f, 0.4f, 0.3f, 255, 255, 255);
									}
									if (distance < num18 && distance <= num15)
									{
										num18 = distance;
										flag2 = true;
										flag = false;
										num19 = -1;
										num20 = -1;
										num21 = -1;
										num22 = num23;
										shapeTypes = ShapeTypes.ST_MAX_SHAPES;
									}
								}
							}
						}
						if (num18 < num15)
						{
							vBeam[l].Origin = AddVector2D(vBeam[l - 1].Origin, MultiplyVector2D(vector2, num18));
						}
						BeamLengthInt++;
						if (EnableBeamHackDebug)
						{
							drawText("Data " + flag2 + "," + flag + "," + num20 + "," + num21 + "," + num19 + "," + num22 + "," + shapeTypes, 0.75f, 0.65f + 0.03f * (float)l, 0.3f, 255, 255, 255);
						}
						if (!flag && num19 == -1)
						{
							if (num22 <= -1)
							{
								break;
							}
							switch (shapeTypes)
							{
							case ShapeTypes.ST_INNER_WALL:
							{
								for (int n = 0; n <= ciBH_INNER_WALL_POINTS / 2; n++)
								{
									GetEdgeToCheck(n, quadrantFromVector, ref point, ref point2);
									if (WorthCheckingEdge(vBeam[l - 1].Origin, list6[num22].Collisions[point], list6[num22].Collisions[point2], quadrantFromVector) && IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list6[num22].Collisions[point], list6[num22].Collisions[point2], ref distance))
									{
										if (EnableBeamHackDebug)
										{
											drawText($"[JS][BeamHack] - PROCESS_BEAM - BETTER intersect between inner wall {num22} edge {point}, {point2} Distance: {distance} < {num18} " + (distance < num18), 0.75f, 0.16f, 0.3f, 255, 255, 255);
										}
										if (distance < num18 && distance <= num15)
										{
											num18 = distance;
											flag2 = true;
											flag = false;
											num19 = -1;
											num20 = -1;
											num21 = -1;
											num22 = num23;
											shapeTypes = ShapeTypes.ST_INNER_WALL;
										}
									}
								}
								if (num18 < num15)
								{
									vBeam[l].Origin = AddVector2D(vBeam[l - 1].Origin, MultiplyVector2D(vector2, num18));
								}
								break;
							}
							case ShapeTypes.ST_FIREWALL:
							{
								for (int num25 = 0; num25 <= ciBH_FIREWALL_POINTS / 2; num25++)
								{
									GetEdgeToCheck(num25, quadrantFromVector, ref point, ref point2);
									if (WorthCheckingEdge(vBeam[l - 1].Origin, list3[num22].Collisions[point], list3[num22].Collisions[point2], quadrantFromVector) && IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list3[num22].Collisions[point], list3[num22].Collisions[point2], ref distance))
									{
										if (EnableBeamHackDebug)
										{
											drawText($"[JS][BeamHack] - PROCESS_BEAM - BETTER intersect between Firewall {num22} Distance: {distance} < {num18} " + (distance < num18), 0.75f, 0.19f, 0.3f, 255, 255, 255);
										}
										if (distance < num18 && distance <= num15)
										{
											node3 = list3[num22];
											num18 = distance;
											flag2 = true;
											flag = false;
											num20 = -1;
											num21 = num22;
											num22 = num22;
											shapeTypes = ShapeTypes.ST_FIREWALL;
										}
									}
								}
								if (num18 < num15)
								{
									vBeam[l].Origin = AddVector2D(vBeam[l - 1].Origin, MultiplyVector2D(vector2, num18));
								}
								break;
							}
							case ShapeTypes.ST_PACKET:
							{
								for (int m = 0; m <= ciBH_PACKET_POINTS / 2; m++)
								{
									GetEdgeToCheck(m, quadrantFromVector, ref point, ref point2);
									if (WorthCheckingEdge(vBeam[l - 1].Origin, list2[num22].Collisions[point], list2[num22].Collisions[point2], quadrantFromVector) && IsBeamIntersectingEdge(vBeam[l - 1].Origin, vector2, list2[num22].Collisions[point], list2[num22].Collisions[point2], ref distance))
									{
										if (EnableBeamHackDebug)
										{
											drawText($"[JS][BeamHack] - PROCESS_BEAM - BETTER intersect between Packet {num22} Distance: {distance} < {num18}  " + (distance < num18), 0.75f, 0.21f, 0.3f, 255, 255, 255);
										}
										if (distance < num18 && distance <= num15)
										{
											node2 = list2[num22];
											num18 = distance;
											flag2 = true;
											flag = false;
											num20 = num22;
											num22 = num22;
											shapeTypes = ShapeTypes.ST_PACKET;
										}
									}
								}
								if (num18 < num15)
								{
									vBeam[l].Origin = AddVector2D(vBeam[l - 1].Origin, MultiplyVector2D(vector2, num18));
								}
								break;
							}
							}
							break;
						}
						if (l >= ciBH_MAX_BEAM_SEGMENTS - 1)
						{
							continue;
						}
						if (flag)
						{
							try
							{
								Vector2 vector4 = NormaliseVector2D(GetLeftPerpendicularVector2D(SubtractVector2D(v, v2)));
								vBeam[l + 1].Origin = SubtractVector2D(vector2, MultiplyVector2D(vector4, 2f * DotProductVector2D(vector2, vector4)));
								vBeam[l + 1].Origin = AddVector2D(vBeam[l].Origin, vBeam[l + 1].Origin);
							}
							catch
							{
								drawText("~r~Caught Error~w~ " + (l + 1) + vBeam.Count, 0.1f, 0.95f, 0.5f, 255, 255, 255);
							}
						}
						else
						{
							if (num19 == -1)
							{
								continue;
							}
							if (l + 1 >= ciBH_MAX_BEAM_SEGMENTS - 1)
							{
								break;
							}
							if (num16 == num19 && num17 == l - ciBH_PROXY_CANCEL_LOOP_COUNT)
							{
								if (EnableBeamHackDebug)
								{
									drawText("[JS][Beamhack] - PROCESS_BEAM - Infinite proxy loop - BAIL", 0.75f, 0.24f, 0.3f, 255, 255, 255);
								}
								break;
							}
							Vector2 vector5 = DivideVector2D(AddVector2D(list4[num19].Collisions[0], list4[num19].Collisions[2]), 2f);
							vBeam[l + 1].Origin = vector5;
							vBeam[l + 2].Origin = RotateVector2DAroundVector2D(vector5, new Vector2(vector5.X, 0f), list4[num19].Angle);
							num16 = num19;
							num17 = l;
							BeamLengthInt++;
							l++;
						}
					}
					if (flag)
					{
						if (!PlayedBounceSound)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hit_Mirror", "dlc_xm_silo_laser_hack_sounds");
							PlayedBounceSound = true;
						}
					}
					else
					{
						PlayedBounceSound = false;
					}
					PlayedBounceSound = false;
				}
				if (shapeTypes == ShapeTypes.ST_FIREWALL)
				{
					num4 = 225;
					num5 = 0;
					num6 = 0;
				}
				if (CurrentPackets != 0 && BeamhackLives != 0)
				{
					foreach (Node levelNode2 in LevelNodes)
					{
						new Vector2(levelNode2.Origin.X - 0.045f, levelNode2.Origin.Y - 0.04f);
						new Vector2(levelNode2.Origin.X - 0.045f, levelNode2.Origin.Y + 0.04f);
						new Vector2(levelNode2.Origin.X + 0.045f, levelNode2.Origin.Y - 0.04f);
						new Vector2(levelNode2.Origin.X + 0.045f, levelNode2.Origin.Y + 0.04f);
						if (levelNode2.NodeType == NodeType.Rotating_Mirror)
						{
							if (Vector2.Distance(new Vector2(vCursor.X, vCursor.Y), new Vector2(levelNode2.Origin.X, levelNode2.Origin.Y)) < 0.035f)
							{
								if (!HasSelectedANode)
								{
									HasSelectedANode = true;
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Node_Select", "dlc_xm_silo_laser_hack_sounds");
								}
								node = levelNode2;
							}
							if (levelNode2.Collisions.Count != 0)
							{
								levelNode2.Collisions[0] = GetVector2Offset(new Vector3(levelNode2.Origin.X, levelNode2.Origin.Y, 0f), levelNode2.Angle, new Vector3(-0.035f, 0f, 0f));
								levelNode2.Collisions[1] = GetVector2Offset(new Vector3(levelNode2.Origin.X, levelNode2.Origin.Y, 0f), levelNode2.Angle, new Vector3(0.035f, 0f, 0f));
								Vector2 vector19 = levelNode2.Collisions[0];
								Vector2 vector20 = levelNode2.Collisions[1];
							}
						}
						if (levelNode2.NodeType == NodeType.Static_Mirror && levelNode2.Collisions.Count != 0)
						{
							levelNode2.Collisions[0] = GetVector2Offset(new Vector3(levelNode2.Origin.X, levelNode2.Origin.Y, 0f), levelNode2.Angle - 135f, new Vector3(0.025f, -0.025f, 0f));
							levelNode2.Collisions[1] = GetVector2Offset(new Vector3(levelNode2.Origin.X, levelNode2.Origin.Y, 0f), levelNode2.Angle - 135f, new Vector3(-0.025f, 0.025f, 0f));
							Vector2 vector21 = levelNode2.Collisions[0];
							Vector2 vector22 = levelNode2.Collisions[1];
						}
						if (EnableBeamHackDebug)
						{
							for (int num26 = 1; num26 < levelNode2.Collisions.Count; num26++)
							{
								if (!levelNode2.isDestroyed && false)
								{
									Vector2 vector6 = Vector2.Divide(Vector2.Add(levelNode2.Collisions[num26 - 1], levelNode2.Collisions[num26]), 2f);
									float num27 = Vector2.Distance(levelNode2.Collisions[num26 - 1], levelNode2.Collisions[num26]);
									double num28 = Function.Call<float>(Hash.ATAN2, levelNode2.Collisions[num26].Y - levelNode2.Collisions[num26 - 1].Y, levelNode2.Collisions[num26].X - levelNode2.Collisions[num26 - 1].X);
									Function.Call(Hash.DRAW_SPRITE_ARX, "MPBeamHack", "beam_middle", GetRatio(vector6.X), vector6.Y, 0.064 / (double)num10, num27, num28 + 90.0, num, num2, num3, num8);
								}
							}
						}
						if (levelNode2.NodeType == NodeType.None)
						{
							drawSprite3("mpbeamhackfg", "cursor_0", levelNode2.Origin.X, levelNode2.Origin.Y, 0.05f, 0.065f, 0f, 255, 255, 255, 255, 0);
						}
						if (levelNode2.NodeType == NodeType.Rotating_Mirror)
						{
							drawSprite3("mpbeamhackfg", "mirror_base", levelNode2.Origin.X, levelNode2.Origin.Y, 0.06f, 0.075f, 0f, 255, 255, 255, 255, 0);
							drawSprite3("mpbeamhackfg", "mirror_highlight", levelNode2.Origin.X, levelNode2.Origin.Y, 0.05f, 0.075f, levelNode2.Angle, 255, 255, 255, 255, 0);
						}
						NodeType nodeType = levelNode2.NodeType;
						int num83 = 2;
						if (levelNode2.NodeType == NodeType.Packet && !levelNode2.isDestroyed)
						{
							if (levelNode2.Frame == 0)
							{
								levelNode2.Frame = 24;
							}
							float cfBH_PACKET_SIZE2 = cfBH_PACKET_SIZE;
							if (levelNode2.Frame >= 61 && levelNode2.FrameTime <= Game.GameTime)
							{
								levelNode2.FrameTime = Game.GameTime + 150;
								if (levelNode2.Frame == 61)
								{
									levelNode2.Frame = 62;
								}
								else if (levelNode2.Frame == 62)
								{
									levelNode2.Frame = 63;
								}
								else
								{
									levelNode2.Frame += 2;
								}
							}
							if (levelNode2.Frame <= 76)
							{
								if (levelNode2.Frame == 24)
								{
									drawSprite3("mpbeamhackfg", "node_shatter_fg_" + levelNode2.Frame, levelNode2.Origin.X, levelNode2.Origin.Y, 0.05f, 0.065f, 0f, 255, 255, 255, 255, 0);
								}
								else
								{
									Vector2 origin = levelNode2.Origin;
									float num29 = num12 * (0.65f * ((float)levelNode2.Frame / 60f) + 0.25f) / 10f;
									origin.X += num29;
									origin.Y += num29;
									drawSprite3("mpbeamhackfg", "node_shatter_fg_" + levelNode2.Frame, origin.X, origin.Y, 0.05f, 0.065f, 0f, 255, 255, 255, 255, 0);
								}
							}
						}
						if (levelNode2.NodeType == NodeType.Firewall && !levelNode2.isDestroyed)
						{
							if (levelNode2.Frame == 0)
							{
								levelNode2.Frame = 58;
							}
							if (levelNode2.Frame >= 61 && levelNode2.FrameTime <= Game.GameTime)
							{
								levelNode2.FrameTime = Game.GameTime + 500;
								if (levelNode2.Frame == 60)
								{
									if (!levelNode2.isDestroyed)
									{
										BeamhackLives--;
										levelNode2.isDestroyed = true;
									}
									levelNode2.Frame = 61;
								}
								else if (levelNode2.Frame == 61)
								{
									levelNode2.Frame = 62;
								}
								else if (levelNode2.Frame == 62)
								{
									levelNode2.Frame = 63;
								}
								else
								{
									levelNode2.Frame += 2;
								}
							}
							if (levelNode2.Frame <= 74)
							{
								if (levelNode2.Frame == 58)
								{
									drawSprite3("mpbeamhackfg", "firewall_shatter_fg_" + levelNode2.Frame, levelNode2.Origin.X, levelNode2.Origin.Y, 0.045f, 0.055f, 0f, 255, 255, 255, 255, 0);
								}
								else
								{
									Vector2 origin2 = levelNode2.Origin;
									float num30 = num12 * (0.65f * ((float)levelNode2.Frame / 60f) + 0.25f) / 10f;
									origin2.X += num30;
									origin2.Y += num30;
									drawSprite3("mpbeamhackfg", "firewall_shatter_fg_" + levelNode2.Frame, origin2.X, origin2.Y, 0.045f, 0.055f, 0f, 255, 255, 255, 255, 0);
								}
							}
						}
						if (levelNode2.NodeType == NodeType.Prism)
						{
							string text = "Prism_FG_";
							int num31 = 750;
							int num32 = 8;
							int gameTime = Game.GameTime;
							levelNode2.Angle = gameTime / num31 % num32 * -45;
							text += Math.Abs(levelNode2.Angle);
							drawSprite3("mpbeamhackfg", text, levelNode2.Origin.X, levelNode2.Origin.Y, 0.055f, 0.065f, 0f, 255, 255, 255, 255, 0);
						}
						if (shapeTypes == ShapeTypes.ST_PACKET && levelNode2.NodeType == NodeType.Packet && node2 == levelNode2 && levelNode2.FrameTime <= Game.GameTime)
						{
							levelNode2.FrameTime = Game.GameTime + 75;
							if (levelNode2.Frame < 60)
							{
								levelNode2.Frame += 2;
							}
							else if (!levelNode2.isDestroyed)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Blue_Target_Explode", "dlc_xm_silo_laser_hack_sounds");
								CurrentPackets--;
								levelNode2.isDestroyed = true;
							}
						}
						if (shapeTypes != ShapeTypes.ST_FIREWALL || levelNode2.NodeType != NodeType.Firewall || node3 != levelNode2 || levelNode2.FrameTime > Game.GameTime)
						{
							continue;
						}
						levelNode2.FrameTime = Game.GameTime + 2500;
						if (levelNode2.Frame < 61)
						{
							levelNode2.Frame++;
							if (levelNode2.Frame == 61 && !levelNode2.isDestroyed)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Red_Target_Explode", "dlc_xm_silo_laser_hack_sounds");
								BeamhackLives--;
								levelNode2.isDestroyed = true;
							}
						}
					}
				}
				if (node == null)
				{
					HasSelectedANode = false;
				}
				if (CurrentPackets == 0)
				{
					if (BeamhackResetIn == 0)
					{
						BeamhackResetIn = Game.GameTime + 5000;
					}
					if (BeamhackResetIn != 0 && BeamhackResetIn < Game.GameTime)
					{
						if (BeamHacklevelAt == 1)
						{
							BeamHackNewLevel(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 3, 8));
							BeamHacklevelAt = 2;
						}
						else if (BeamHacklevelAt == 2)
						{
							BeamHackNewLevel(Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 9, 13));
							BeamHacklevelAt = 3;
						}
						else if (BeamHacklevelAt == 3)
						{
							Game.Player.CanControlCharacter = true;
							Function.Call(Hash.DISPLAY_RADAR, true);
							StopBeamhackSounds();
							BeamHackOn = false;
							HackFinishedID = 1;
							return;
						}
					}
					drawSprite3("MPBeamHack", "pass", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
					DRAW_BIG_HACKING_TEXT("BEAM_P", 17, 165, 141, 255, 1.75f, 1.75f, 0.5f, 0.4f);
				}
				if (BeamhackLives == 0)
				{
					drawSprite3("MPBeamHack", "fail", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
					DRAW_BIG_HACKING_TEXT("BEAM_F", 157, 35, 20, 255, 1.75f, 1.75f, 0.5f, 0.4f);
					if (BeamhackResetIn == 0)
					{
						BeamhackResetIn = Game.GameTime + 5000;
					}
					if (BeamhackResetIn != 0 && BeamhackResetIn < Game.GameTime)
					{
						ResetBeamHack();
					}
				}
				if (300000 - num11 < 0)
				{
					drawSprite3("MPBeamHack", "fail", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
					DRAW_BIG_HACKING_TEXT("BEAM_F", 157, 35, 20, 255, 1.75f, 1.75f, 0.5f, 0.4f);
					if (BeamhackResetIn == 0)
					{
						BeamhackResetIn = Game.GameTime + 5000;
					}
					if (BeamhackResetIn != 0 && BeamhackResetIn < Game.GameTime)
					{
						ResetBeamHack();
					}
				}
				if (CurrentPackets != 0 && BeamhackLives != 0)
				{
					if (Game.GameTime >= PrismChangeTime)
					{
						PrismChangeTime = Game.GameTime + 2500;
						foreach (Node levelNode3 in LevelNodes)
						{
							if (levelNode3.NodeType == NodeType.Prism)
							{
								levelNode3.AngleChanged = true;
								levelNode3.Angle -= 30f;
								if (levelNode3.Angle < 0f)
								{
									levelNode3.Angle = 360f;
								}
							}
						}
					}
					for (int num33 = 1; num33 <= BeamLengthInt; num33++)
					{
						Vector2 vector7 = DivideVector2D(AddVector2D(vBeam[num33 - 1].Origin, vBeam[num33].Origin), 2f);
						float num34 = Vector2DDist(vBeam[num33 - 1].Origin, vBeam[num33].Origin);
						float num35 = Function.Call<float>(Hash.ATAN2, vBeam[num33].Origin.Y - vBeam[num33 - 1].Origin.Y, vBeam[num33].Origin.X - vBeam[num33 - 1].Origin.X);
						Function.Call(Hash.DRAW_SPRITE_ARX, "MPBeamHack", "Beam_Glow_Tapered", GetRatio(vector7.X), vector7.Y, 0.064 / (double)num10, num34, (double)num35 + 90.0, num4, num5, num6, num7);
						Function.Call(Hash.DRAW_SPRITE_ARX, "MPBeamHack", "beam_middle", GetRatio(vector7.X), vector7.Y, 0.064 / (double)num10, num34, (double)num35 + 90.0, num4, num5, num6, num8);
						Function.Call(Hash.DRAW_SPRITE_ARX, "MPBeamHack", "beam_top", GetRatio(vector7.X), vector7.Y, 0.064 / (double)num10, num34, (double)num35 + 90.0, num4, num5, num6, num9);
					}
					if (BeamLengthInt > 0)
					{
						Vector2 v3 = NormaliseVector2D(SubtractVector2D(vBeam[BeamLengthInt].Origin, vBeam[BeamLengthInt - 1].Origin));
						Vector2 v4 = AddVector2D(vBeam[BeamLengthInt].Origin, MultiplyVector2D(v3, -0.016f));
						Vector2 vector8 = DivideVector2D(AddVector2D(vBeam[BeamLengthInt].Origin, v4), 2f);
						float num36 = Function.Call<float>(Hash.ATAN2, vBeam[BeamLengthInt].Origin.Y - v4.Y, vBeam[BeamLengthInt].Origin.X - v4.X);
						drawSprite3("MPBeamHack", "beam_spark_" + BeamFxFrame, vector8.X, vector8.Y, 0.064f, 0.064f, num36 + 90f, num4, num5, num6, num9, 0);
					}
				}
				if (CurrentPackets != 0 && BeamhackLives != 0)
				{
					SetTextScale(0.401f, 0.401f);
					SetTextColour(211, 55, 229, 255);
					SetTextCentre(true);
					BeginTextCommandDisplayText("BEAM_TRT");
					EndTextCommandDisplayText(GetRatio(0.851f), 0.061f);
					int num37 = 0;
					int num38 = 300000 - num11;
					num37 = num38 / 60000;
					int num39 = num38 / 1000 % 60;
					SetTextScale(0.693f, 0.693f);
					SetTextColour(211, 55, 229, 255);
					SetTextCentre(true);
					string text2 = "BEAM_TR";
					if (num39 < 10)
					{
						text2 += "S";
					}
					BeginTextCommandDisplayText(text2);
					AddTextComponentInteger(num37);
					AddTextComponentInteger(num39);
					EndTextCommandDisplayText(GetRatio(0.852f), 0.086f);
					SetTextScale(0.401f, 0.401f);
					SetTextColour(211, 55, 229, 255);
					SetTextCentre(true);
					BeginTextCommandDisplayText("BEAM_LVT");
					EndTextCommandDisplayText(GetRatio(1.09f), 0.061f);
					SetTextScale(0.693f, 0.693f);
					SetTextColour(211, 55, 229, 255);
					SetTextCentre(true);
					BeginTextCommandDisplayText("BEAM_LV");
					AddTextComponentInteger(BeamhackLives);
					EndTextCommandDisplayText(GetRatio(1.09f), 0.086f);
					screenaspectratio = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						vCursor.X = (Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239) - 0.1f) * screenaspectratio;
						vCursor.Y = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					}
					else
					{
						float num40 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 218);
						float num41 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 219);
						if (num40 > 0.1f || num40 < -0.1f)
						{
							vCursor.X += num40 / 1.5f;
						}
						if (num41 > 0.1f || num41 < -0.1f)
						{
							vCursor.Y += num41 / 1.5f;
						}
						fCursorMovement = Function.Call<float>(Hash.ABSF, num40) + Function.Call<float>(Hash.ABSF, num41);
						if (fCursorMovement > 1f)
						{
							fCursorMovement = 1f;
						}
					}
					if (node == null)
					{
						drawSprite3("MPBeamHack", "cursor_0", vCursor.X, vCursor.Y, 0.05f, 0.065f, 0f, 150, 150, 150, 255, 0);
						if (Game.IsControlPressed(Control.PhoneLeft))
						{
							BeamLength = 0.025f;
							Intersectdist = 1f;
							BeamRotation -= 0.5f;
							if (BeamRotation < 0f)
							{
								BeamRotation = 360f;
							}
						}
						if (Game.IsControlPressed(Control.PhoneRight))
						{
							BeamLength = 0.025f;
							Intersectdist = 1f;
							BeamRotation += 0.5f;
							if (BeamRotation > 360f)
							{
								BeamRotation = 0f;
							}
						}
					}
					else
					{
						drawSprite3("MPBeamHack", "cursor_0", node.Origin.X, node.Origin.Y, 0.05f, 0.065f, 0f, 171, 252, 255, 255, 0);
						if (!Game.IsControlPressed(Control.PhoneLeft) && !Game.IsControlPressed(Control.PhoneRight))
						{
							RotationAmt = 0.01f;
						}
						if (Game.IsControlPressed(Control.PhoneLeft))
						{
							BeamLength = 0.025f;
							node.Angle -= RotationAmt;
							if (RotationAmt <= 0.35f)
							{
								RotationAmt += 0.0025f;
							}
							if (node.Angle < -180f)
							{
								node.Angle = 180f;
							}
						}
						if (Game.IsControlPressed(Control.PhoneRight))
						{
							BeamLength = 0.025f;
							node.Angle += RotationAmt;
							if ((double)RotationAmt <= 0.35)
							{
								RotationAmt += 0.0025f;
							}
							if (node.Angle > 180f)
							{
								node.Angle = -180f;
							}
						}
					}
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
					if (Game.GameTime >= BeamhackChangeMessageTime)
					{
						BeamhackChangeMessageTime = Game.GameTime + 8000;
						if (BeamhackMsg == 0)
						{
							BeamhackMsg = 1;
						}
						else if (BeamhackMsg == 1)
						{
							BeamhackMsg = 2;
						}
						else if (BeamhackMsg == 2)
						{
							BeamhackMsg = 3;
						}
						else if (BeamhackMsg == 3)
						{
							BeamhackMsg = 0;
						}
					}
					if (BeamhackMsg == 0)
					{
						Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "BEAMTUT_4", -1);
					}
					if (BeamhackMsg == 1)
					{
						Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "BEAM_HLPa", -1);
					}
					if (BeamhackMsg == 2)
					{
						Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "BEAM_HLPb", -1);
					}
					if (BeamhackMsg == 3)
					{
						Function.Call(Hash.DISPLAY_HELP_TEXT_THIS_FRAME, "BEAM_HLPc", -1);
					}
					this.scaleform = new Scaleform("instructional_buttons");
					this.scaleform.CallFunction("CLEAR_ALL");
					this.scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
					this.scaleform.CallFunction("CREATE_CONTAINER");
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						this.scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 239, 0), "Move Cursor");
					}
					else
					{
						this.scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 195, 0), "Move Cursor");
					}
					if (node != null)
					{
						this.scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 174, 0), "");
						this.scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 175, 0), "Rotate Node");
					}
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						this.scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
					}
					else
					{
						this.scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 44, 0), "Quit");
					}
					this.scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
					this.scaleform.Render2D();
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
				}
			}
			if (PCBHackOn)
			{
				if (Game.IsControlJustPressed(Control.Context))
				{
					HackFinishedID = 3;
					RunPCBHack();
					CannotCallHackToggleTimeout = Game.GameTime + 1000;
				}
				Scaleform scaleform = new Scaleform("instructional_buttons");
				scaleform.CallFunction("CLEAR_ALL");
				scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform.CallFunction("CREATE_CONTAINER");
				scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Navigation Up");
				scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Navigation Down");
				scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Navigation Next");
				scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Navigation Back");
				scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Choose Section");
				scaleform.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
				scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform.Render2D();
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				DrawnIDsThisFrame.Clear();
				switch (PCBHHackStage)
				{
				case 0:
				{
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_9"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_9", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_8"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_8", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_7"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_7", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_6"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_6", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_5"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_5", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_4"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_4", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_3"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_3", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_2"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_2", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks_1"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks_1", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_levelblocks"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_levelblocks", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_foreground"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_foreground", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "pcb_hack_assets_backgrounds"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "pcb_hack_assets_backgrounds", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "MPCircuitHack"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPCircuitHack", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "MPBeamHack"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPBeamHack", 0);
					}
					int num42 = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 10);
					if (num42 == 0)
					{
						PCBHHackCurrentLevel = "";
						PCBHHackCurrentLevelID = "";
					}
					else
					{
						if (num42 == 10)
						{
							num42 = 9;
						}
						PCBHHackCurrentLevel = "_" + num42;
						PCBHHackCurrentLevelID = num42.ToString();
					}
					if (num42 == 0)
					{
						PCBHackBar1_AlignmentCorrect = 1;
						PCBHackBar2_AlignmentCorrect = 1;
						PCBHackBar3_AlignmentCorrect = 2;
						PCBHackBar4_AlignmentCorrect = 1;
						PCBHackBar5_AlignmentCorrect = 1;
						PCBHackBar6_AlignmentCorrect = 1;
						PCBHackBar7_AlignmentCorrect = 2;
						PCBHackBar8_AlignmentCorrect = 1;
					}
					if (num42 == 1)
					{
						PCBHackBar1_AlignmentCorrect = 2;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 0;
						PCBHackBar4_AlignmentCorrect = 2;
						PCBHackBar5_AlignmentCorrect = 0;
						PCBHackBar6_AlignmentCorrect = 1;
						PCBHackBar7_AlignmentCorrect = 2;
						PCBHackBar8_AlignmentCorrect = 0;
					}
					if (num42 == 2)
					{
						PCBHackBar1_AlignmentCorrect = 0;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 2;
						PCBHackBar4_AlignmentCorrect = 2;
						PCBHackBar5_AlignmentCorrect = 0;
						PCBHackBar6_AlignmentCorrect = 2;
						PCBHackBar7_AlignmentCorrect = 1;
						PCBHackBar8_AlignmentCorrect = 0;
					}
					if (num42 == 3)
					{
						PCBHackBar1_AlignmentCorrect = 1;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 2;
						PCBHackBar4_AlignmentCorrect = 0;
						PCBHackBar5_AlignmentCorrect = 0;
						PCBHackBar6_AlignmentCorrect = 2;
						PCBHackBar7_AlignmentCorrect = 1;
						PCBHackBar8_AlignmentCorrect = 0;
					}
					if (num42 == 4)
					{
						PCBHackBar1_AlignmentCorrect = 1;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 1;
						PCBHackBar4_AlignmentCorrect = 1;
						PCBHackBar5_AlignmentCorrect = 1;
						PCBHackBar6_AlignmentCorrect = 0;
						PCBHackBar7_AlignmentCorrect = 0;
						PCBHackBar8_AlignmentCorrect = 1;
					}
					if (num42 == 5)
					{
						PCBHackBar1_AlignmentCorrect = 2;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 2;
						PCBHackBar4_AlignmentCorrect = 2;
						PCBHackBar5_AlignmentCorrect = 0;
						PCBHackBar6_AlignmentCorrect = 2;
						PCBHackBar7_AlignmentCorrect = 0;
						PCBHackBar8_AlignmentCorrect = 2;
					}
					if (num42 == 6)
					{
						PCBHackBar1_AlignmentCorrect = 2;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 2;
						PCBHackBar4_AlignmentCorrect = 0;
						PCBHackBar5_AlignmentCorrect = 0;
						PCBHackBar6_AlignmentCorrect = 1;
						PCBHackBar7_AlignmentCorrect = 0;
						PCBHackBar8_AlignmentCorrect = 2;
					}
					if (num42 == 7)
					{
						PCBHackBar1_AlignmentCorrect = 1;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 1;
						PCBHackBar4_AlignmentCorrect = 2;
						PCBHackBar5_AlignmentCorrect = 2;
						PCBHackBar6_AlignmentCorrect = 0;
						PCBHackBar7_AlignmentCorrect = 0;
						PCBHackBar8_AlignmentCorrect = 2;
					}
					if (num42 == 8)
					{
						PCBHackBar1_AlignmentCorrect = 0;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 2;
						PCBHackBar4_AlignmentCorrect = 2;
						PCBHackBar5_AlignmentCorrect = 2;
						PCBHackBar6_AlignmentCorrect = 0;
						PCBHackBar7_AlignmentCorrect = 0;
						PCBHackBar8_AlignmentCorrect = 2;
					}
					if (num42 == 9)
					{
						PCBHackBar1_AlignmentCorrect = 2;
						PCBHackBar2_AlignmentCorrect = 0;
						PCBHackBar3_AlignmentCorrect = 2;
						PCBHackBar4_AlignmentCorrect = 1;
						PCBHackBar5_AlignmentCorrect = 2;
						PCBHackBar6_AlignmentCorrect = 0;
						PCBHackBar7_AlignmentCorrect = 0;
						PCBHackBar8_AlignmentCorrect = 2;
					}
					PCBHackLevelId = num42;
					PCBHackBar1_Alignment = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
					PCBHackBar2_Alignment = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
					PCBHackBar3_Alignment = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
					PCBHackBar4_Alignment = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
					PCBHackBar5_Alignment = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
					PCBHackBar6_Alignment = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
					PCBHackBar7_Alignment = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
					PCBHackBar8_Alignment = Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, 2);
					PCBHHackStage = 1;
					AlphaStage = 0;
					PCBHackBarSelection = 0;
					PCBHackTime = Game.GameTime + 60000;
					break;
				}
				case 1:
				{
					if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
					{
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match", false, -1);
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking", false, -1);
					}
					else
					{
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_24-2/DLC_24-2_Circuit_Hack");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_24-2/DLC_24-2_Circuit_Hack", false, -1);
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_24-2/DLC_24-2_Circuit_Hack");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_24-2/DLC_24-2_Circuit_Hack", false, -1);
					}
					drawSprite_NormalOrMemory("pcb_hack_assets_backgrounds", "pcb_hack_background", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_levelblocks" + PCBHHackCurrentLevel, "pcb_hack" + PCBHHackCurrentLevelID + "_block1", 0.03f, GetAlignment(PCBHackBar1_Alignment), 0.08f, 0.45f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_levelblocks" + PCBHHackCurrentLevel, "pcb_hack" + PCBHHackCurrentLevelID + "_block2", 0.165f, GetAlignment(PCBHackBar2_Alignment), 0.08f, 0.45f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_levelblocks" + PCBHHackCurrentLevel, "pcb_hack" + PCBHHackCurrentLevelID + "_block3", 0.3f, GetAlignment(PCBHackBar3_Alignment), 0.08f, 0.45f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_levelblocks" + PCBHHackCurrentLevel, "pcb_hack" + PCBHHackCurrentLevelID + "_block4", 0.435f, GetAlignment(PCBHackBar4_Alignment), 0.08f, 0.45f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_levelblocks" + PCBHHackCurrentLevel, "pcb_hack" + PCBHHackCurrentLevelID + "_block5", 0.57f, GetAlignment(PCBHackBar5_Alignment), 0.08f, 0.45f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_levelblocks" + PCBHHackCurrentLevel, "pcb_hack" + PCBHHackCurrentLevelID + "_block6", 0.705f, GetAlignment(PCBHackBar6_Alignment), 0.08f, 0.45f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_levelblocks" + PCBHHackCurrentLevel, "pcb_hack" + PCBHHackCurrentLevelID + "_block7", 0.84f, GetAlignment(PCBHackBar7_Alignment), 0.08f, 0.45f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_levelblocks" + PCBHHackCurrentLevel, "pcb_hack" + PCBHHackCurrentLevelID + "_block8", 0.975f, GetAlignment(PCBHackBar8_Alignment), 0.08f, 0.45f, 0f, 255, 255, 255, 255, 0);
					if (PCBHackLevelId == 0)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.31f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.312f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 1)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.493f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.493f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 2)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.495f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.492f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 3)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.494f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.496f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 4)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.622f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.435f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 5)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.495f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.495f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 6)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_middle", -0.05f, 0.683f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.495f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 7)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.496f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.683f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 8)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.496f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.683f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					if (PCBHackLevelId == 9)
					{
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_top", -0.05f, 0.494f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_startsolder_bottom", 1.05f, 0.681f, 0.05f, 0.07f, 0f, 255, 255, 255, 255, 0);
					}
					Game.DisableControlThisFrame(Control.Phone);
					if (Game.IsControlJustPressed(Control.PhoneUp))
					{
						if (PCBHackBarSelection == 0 && PCBHackBar1_Alignment > 0)
						{
							PCBHackBar1_Alignment--;
						}
						if (PCBHackBarSelection == 1 && PCBHackBar2_Alignment > 0)
						{
							PCBHackBar2_Alignment--;
						}
						if (PCBHackBarSelection == 2 && PCBHackBar3_Alignment > 0)
						{
							PCBHackBar3_Alignment--;
						}
						if (PCBHackBarSelection == 3 && PCBHackBar4_Alignment > 0)
						{
							PCBHackBar4_Alignment--;
						}
						if (PCBHackBarSelection == 4 && PCBHackBar5_Alignment > 0)
						{
							PCBHackBar5_Alignment--;
						}
						if (PCBHackBarSelection == 5 && PCBHackBar6_Alignment > 0)
						{
							PCBHackBar6_Alignment--;
						}
						if (PCBHackBarSelection == 6 && PCBHackBar7_Alignment > 0)
						{
							PCBHackBar7_Alignment--;
						}
						if (PCBHackBarSelection == 7 && PCBHackBar8_Alignment > 0)
						{
							PCBHackBar8_Alignment--;
						}
						if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
						}
						else
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Move_Circuit_Up", "DLC_24-2_Hack_Circuit_Board", true);
						}
					}
					if (Game.IsControlJustPressed(Control.PhoneDown))
					{
						if (PCBHackBarSelection == 0 && PCBHackBar1_Alignment < 2)
						{
							PCBHackBar1_Alignment++;
						}
						if (PCBHackBarSelection == 1 && PCBHackBar2_Alignment < 2)
						{
							PCBHackBar2_Alignment++;
						}
						if (PCBHackBarSelection == 2 && PCBHackBar3_Alignment < 2)
						{
							PCBHackBar3_Alignment++;
						}
						if (PCBHackBarSelection == 3 && PCBHackBar4_Alignment < 2)
						{
							PCBHackBar4_Alignment++;
						}
						if (PCBHackBarSelection == 4 && PCBHackBar5_Alignment < 2)
						{
							PCBHackBar5_Alignment++;
						}
						if (PCBHackBarSelection == 5 && PCBHackBar6_Alignment < 2)
						{
							PCBHackBar6_Alignment++;
						}
						if (PCBHackBarSelection == 6 && PCBHackBar7_Alignment < 2)
						{
							PCBHackBar7_Alignment++;
						}
						if (PCBHackBarSelection == 7 && PCBHackBar8_Alignment < 2)
						{
							PCBHackBar8_Alignment++;
						}
						if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
						}
						else
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Move_Circuit_Down", "DLC_24-2_Hack_Circuit_Board", true);
						}
					}
					if (Game.IsControlJustPressed(Control.PhoneLeft) && PCBHackBarSelection > 0)
					{
						PCBHackBarSelection--;
						if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
						}
						else
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Nav", "DLC_24-2_Hack_Circuit_Board", true);
						}
					}
					if (Game.IsControlJustPressed(Control.PhoneRight) && PCBHackBarSelection < 7)
					{
						PCBHackBarSelection++;
						if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
						}
						else
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Nav", "DLC_24-2_Hack_Circuit_Board", true);
						}
					}
					if (Game.IsControlJustPressed(Control.FrontendAccept))
					{
						if (PCBHackBar1_Alignment == PCBHackBar1_AlignmentCorrect && PCBHackBar2_Alignment == PCBHackBar2_AlignmentCorrect && PCBHackBar3_Alignment == PCBHackBar3_AlignmentCorrect && PCBHackBar4_Alignment == PCBHackBar4_AlignmentCorrect && PCBHackBar5_Alignment == PCBHackBar5_AlignmentCorrect && PCBHackBar6_Alignment == PCBHackBar6_AlignmentCorrect && PCBHackBar7_Alignment == PCBHackBar7_AlignmentCorrect && PCBHackBar8_Alignment == PCBHackBar8_AlignmentCorrect)
						{
							if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Target_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							}
							else
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Success", "DLC_24-2_Hack_Circuit_Board", true);
							}
						}
						else if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "No_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
						}
						else
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "error", "DLC_24-2_Hack_Circuit_Board", true);
						}
						PCBHHackStage = 2;
						PCBHackCheckTime = Game.GameTime + 5000;
					}
					if (Game.GameTime >= NextAlphaChange)
					{
						NextAlphaChange = Game.GameTime + 50;
						if (AlphaStage == 0)
						{
							SelectionAlpha += 10;
							if (SelectionAlpha >= 256)
							{
								AlphaStage = 1;
								NextAlphaChange = Game.GameTime + 100;
								SelectionAlpha = 255;
							}
						}
						if (AlphaStage == 1)
						{
							SelectionAlpha -= 10;
							if (SelectionAlpha <= 0)
							{
								AlphaStage = 0;
								NextAlphaChange = Game.GameTime + 100;
								SelectionAlpha = 0;
							}
						}
					}
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					drawSprite_NormalOrMemory(PCBHack_NoLight1_ID, "pcb_hack_assets_foreground", "pcb_hack_offlight", -0.12f, 0.243f, 0.03f, 0.06f, 0f, 255, 255, 255, 255, 0);
					if (PCBHackBar1_Alignment == PCBHackBar1_AlignmentCorrect)
					{
						drawSprite_NormalOrMemory(PCBHack_GreenLight1_ID, "pcb_hack_assets_foreground", "pcb_hack_greenlight", -0.12f, 0.243f, 0.03f, 0.06f, 0f, 255, 255, 255, 255, 0);
					}
					drawSprite_NormalOrMemory(PCBHack_NoLight2_ID, "pcb_hack_assets_foreground", "pcb_hack_offlight", 1.12f, 0.243f, 0.03f, 0.06f, 0f, 255, 255, 255, 255, 0);
					if (PCBHackBar8_Alignment == PCBHackBar8_AlignmentCorrect)
					{
						drawSprite_NormalOrMemory(PCBHack_GreenLight2_ID, "pcb_hack_assets_foreground", "pcb_hack_greenlight", 1.12f, 0.243f, 0.03f, 0.06f, 0f, 255, 255, 255, 255, 0);
					}
					drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_selection", GetSelectionX(PCBHackBarSelection), GetSelectionY(PCBHackBarSelection), 0.075f, 0.44f, 0f, 255, 255, 255, SelectionAlpha, 0);
					drawSprite_NormalOrMemory("pcb_hack_assets_foreground", "pcb_hack_selection", GetSelectionX(PCBHackBarSelection), GetSelectionY(PCBHackBarSelection), 0.075f, 0.44f, 0f, 255, 255, 255, SelectionAlpha, 0);
					drawText("PCBHHackStage\n" + PCBHHackStage, 0.025f, 0.65f, 0.3f, 255, 255, 255);
					TimeSpan timeSpan = TimeSpan.FromMilliseconds(PCBHackTime - Game.GameTime);
					int seconds = timeSpan.Seconds;
					int milliseconds = timeSpan.Milliseconds;
					string text3 = timeSpan.Milliseconds.ToString();
					if (milliseconds >= 100)
					{
						text3 = timeSpan.Milliseconds.ToString().ToCharArray()[0].ToString() + timeSpan.Milliseconds.ToString().ToCharArray()[1];
					}
					string text4 = "00.";
					text4 = ((seconds >= 9) ? (text4 + seconds + ".") : (text4 + "0" + seconds + "."));
					text4 = ((milliseconds >= 9) ? (text4 + text3) : (text4 + "0" + milliseconds));
					if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
					{
						drawTextAdv(text4, 0.8235f, 0.1f, 0.75f, 255, 255, 255);
					}
					else
					{
						drawTextAdv(text4, 0.8235f, 0.119f, 0.125f, 255, 255, 255);
					}
					if (PCBHackTime - Game.GameTime <= 0)
					{
						if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "No_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
						}
						else
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "error", "DLC_24-2_Hack_Circuit_Board", true);
						}
						PCBHHackStage = 3;
						PCBHackCheckTime = Game.GameTime + 5000;
					}
					break;
				}
				case 5:
					drawText(string.Concat(str3: Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240).ToString(), str0: "X/Y \n", str1: Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239).ToString(), str2: "\n"), 0.1f, 0.65f, 0.3f, 255, 255, 255);
					drawSprite3("MPBeamHack", "pass", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
					drawText("SUCCESS", 0.426f, 0.406f, 1.45f, 18, 118, 95);
					break;
				case 2:
					if (Game.GameTime < PCBHackCheckTime)
					{
						if (PCBHackBar1_Alignment == PCBHackBar1_AlignmentCorrect && PCBHackBar2_Alignment == PCBHackBar2_AlignmentCorrect && PCBHackBar3_Alignment == PCBHackBar3_AlignmentCorrect && PCBHackBar4_Alignment == PCBHackBar4_AlignmentCorrect && PCBHackBar5_Alignment == PCBHackBar5_AlignmentCorrect && PCBHackBar6_Alignment == PCBHackBar6_AlignmentCorrect && PCBHackBar7_Alignment == PCBHackBar7_AlignmentCorrect && PCBHackBar8_Alignment == PCBHackBar8_AlignmentCorrect)
						{
							drawSprite3("MPBeamHack", "pass", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
							drawText("SUCCESS", 0.426f, 0.406f, 1.45f, 18, 118, 95);
						}
						else
						{
							drawSprite3("MPBeamHack", "fail", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
							drawText("ERROR", 0.436f, 0.406f, 1.45f, 148, 28, 16);
						}
					}
					else if (PCBHackBar1_Alignment == PCBHackBar1_AlignmentCorrect && PCBHackBar2_Alignment == PCBHackBar2_AlignmentCorrect && PCBHackBar3_Alignment == PCBHackBar3_AlignmentCorrect && PCBHackBar4_Alignment == PCBHackBar4_AlignmentCorrect && PCBHackBar5_Alignment == PCBHackBar5_AlignmentCorrect && PCBHackBar6_Alignment == PCBHackBar6_AlignmentCorrect && PCBHackBar7_Alignment == PCBHackBar7_AlignmentCorrect && PCBHackBar8_Alignment == PCBHackBar8_AlignmentCorrect)
					{
						PCBHHackStage = -1;
						PCBHackOn = false;
						HackFinishedID = 1;
						PCBHackCheckTime = -1;
					}
					else
					{
						PCBHHackStage = 1;
						PCBHackCheckTime = -1;
					}
					break;
				case 3:
					if (Game.GameTime < PCBHackCheckTime)
					{
						drawSprite3("MPBeamHack", "fail", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
						drawText("ERROR", 0.436f, 0.406f, 1.45f, 148, 28, 16);
					}
					else if (PCBHackBar1_Alignment == PCBHackBar1_AlignmentCorrect && PCBHackBar2_Alignment == PCBHackBar2_AlignmentCorrect && PCBHackBar3_Alignment == PCBHackBar3_AlignmentCorrect && PCBHackBar4_Alignment == PCBHackBar4_AlignmentCorrect && PCBHackBar5_Alignment == PCBHackBar5_AlignmentCorrect && PCBHackBar6_Alignment == PCBHackBar6_AlignmentCorrect && PCBHackBar7_Alignment == PCBHackBar7_AlignmentCorrect && PCBHackBar8_Alignment == PCBHackBar8_AlignmentCorrect)
					{
						PCBHHackStage = -1;
						PCBHackOn = false;
						HackFinishedID = 1;
						PCBHackCheckTime = -1;
					}
					else
					{
						PCBHHackStage = 0;
						PCBHackCheckTime = -1;
					}
					break;
				}
			}
			if (FiveLetterPasscodeHack)
			{
				if (Game.IsControlJustPressed(Control.Context))
				{
					HackFinishedID = 3;
					RunPasscodeHack();
					CannotCallHackToggleTimeout = Game.GameTime + 1000;
				}
				Scaleform scaleform2 = new Scaleform("instructional_buttons");
				scaleform2.CallFunction("CLEAR_ALL");
				scaleform2.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform2.CallFunction("CREATE_CONTAINER");
				scaleform2.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Navigation Up");
				scaleform2.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Navigation Down");
				scaleform2.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Navigation Next");
				scaleform2.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Navigation Back");
				scaleform2.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Choose Letter");
				scaleform2.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
				scaleform2.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform2.Render2D();
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				DrawnIDsThisFrame.Clear();
				switch (FiveLetterPasscodeStage)
				{
				case 0:
					HackCompleteType = -1;
					ResetHackIn = -1;
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpwordhack_sprites"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpwordhack_sprites", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpwordhack_background"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpwordhack_background", 0);
					}
					FiveLetterPasscodeStage = 1;
					Passcode = PassCodes[Function.Call<int>(Hash.GET_RANDOM_INT_IN_RANGE, 0, PassCodes.Count)];
					JumbledPasscode = Shuffle(Passcode);
					LetterSelection = 0;
					Letter1Input = "";
					Letter1CorrectInput = Passcode.ToCharArray()[0].ToString();
					Letter2Input = "";
					Letter2CorrectInput = Passcode.ToCharArray()[1].ToString();
					Letter3Input = "";
					Letter3CorrectInput = Passcode.ToCharArray()[2].ToString();
					Letter4Input = "";
					Letter4CorrectInput = Passcode.ToCharArray()[3].ToString();
					Letter5Input = "";
					Letter5CorrectInput = Passcode.ToCharArray()[4].ToString();
					AttemptsLeft = 5;
					LetterAt = 0;
					FiveLetterPasscodeHackTime = Game.GameTime + 60000;
					break;
				case 1:
				{
					Vector3 vector9 = new Vector3(84f, 84f, 83f);
					Vector3 vector10 = new Vector3(84f, 84f, 83f);
					Vector3 vector11 = new Vector3(84f, 84f, 83f);
					Vector3 vector12 = new Vector3(84f, 84f, 83f);
					Vector3 vector13 = new Vector3(84f, 84f, 83f);
					bool flag6 = false;
					bool flag7 = false;
					if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
					{
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match", false, -1);
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking", false, -1);
					}
					else
					{
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_24-2/DLC_24-2_Password");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_24-2/DLC_24-2_Password", false, -1);
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_24-2/DLC_24-2_Password");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_24-2/DLC_24-2_Password", false, -1);
					}
					Game.DisableControlThisFrame(Control.Phone);
					if (ResetHackIn == -1)
					{
						if (Game.IsControlJustPressed(Control.PhoneLeft))
						{
							if (LetterSelection > 0)
							{
								LetterSelection--;
							}
							if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							}
							else
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Nav", "DLC_24-2_Hack_Password", true);
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneRight))
						{
							if (LetterSelection < 4)
							{
								LetterSelection++;
							}
							if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							}
							else
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Nav", "DLC_24-2_Hack_Password", true);
							}
						}
					}
					drawSprite_NormalOrMemory(PasscodeHack_Background, "mpwordhack_background", "wm_background", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Popup, "mpwordhack_sprites", "wm_popup", 0.51f, 0.5f, 0.385f, 0.315f, 0f, 255, 255, 255, 255, 0);
					if (LetterAt != 5)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[0], "mpwordhack_sprites", "wm_empty_node", 0.212f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[1], "mpwordhack_sprites", "wm_empty_node", 0.36f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[2], "mpwordhack_sprites", "wm_empty_node", 0.509f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[3], "mpwordhack_sprites", "wm_empty_node", 0.656f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[4], "mpwordhack_sprites", "wm_empty_node", 0.805f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[5], "mpwordhack_sprites", "wm_empty_node", 0.509f, 0.332f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[6], "mpwordhack_sprites", "wm_empty_node", 0.2688f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[7], "mpwordhack_sprites", "wm_empty_node", 0.304f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[8], "mpwordhack_sprites", "wm_empty_node", 0.416f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[9], "mpwordhack_sprites", "wm_empty_node", 0.452f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[10], "mpwordhack_sprites", "wm_empty_node", 0.564f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[11], "mpwordhack_sprites", "wm_empty_node", 0.601f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[12], "mpwordhack_sprites", "wm_empty_node", 0.712f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Nodes_Normal[13], "mpwordhack_sprites", "wm_empty_node", 0.749f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
					}
					if (LetterAt != 5)
					{
						if (LetterAt >= 1 || Letter1Input == Letter1CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Line_1, "mpwordhack_sprites", "wm_line_1", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 255, 255, 255, 0);
						}
						if (LetterAt >= 2 || Letter2Input == Letter2CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Line_2, "mpwordhack_sprites", "wm_line_2", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 255, 255, 255, 0);
						}
						if (LetterAt >= 3 || Letter3Input == Letter3CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Line_3, "mpwordhack_sprites", "wm_line_3", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 255, 255, 255, 0);
						}
						if (LetterAt >= 4 || Letter4Input == Letter4CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Line_4, "mpwordhack_sprites", "wm_line_4", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 255, 255, 255, 0);
						}
						if (LetterAt >= 5 || Letter5Input == Letter5CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Line_5, "mpwordhack_sprites", "wm_line_5", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 255, 255, 255, 0);
						}
					}
					drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom[0], "mpwordhack_sprites", "wm_letter_tile", 0.215f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom[1], "mpwordhack_sprites", "wm_letter_tile", 0.36f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom[2], "mpwordhack_sprites", "wm_letter_tile", 0.51f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom[3], "mpwordhack_sprites", "wm_letter_tile", 0.6575f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom[4], "mpwordhack_sprites", "wm_letter_tile", 0.805f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					if (ResetHackIn == -1)
					{
						if (Game.IsControlJustPressed(Control.FrontendAccept))
						{
							if (LetterAt == 0)
							{
								if (Letter1Input == "")
								{
									Letter1Input = JumbledPasscode.ToCharArray()[LetterSelection].ToString();
									LetterAt++;
									if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
									}
									else
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Set_Char", "DLC_24-2_Hack_Password", true);
									}
								}
							}
							else if (LetterAt == 1)
							{
								if (Letter2Input == "")
								{
									Letter2Input = JumbledPasscode.ToCharArray()[LetterSelection].ToString();
									LetterAt++;
									if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
									}
									else
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Set_Char", "DLC_24-2_Hack_Password", true);
									}
								}
							}
							else if (LetterAt == 2)
							{
								if (Letter3Input == "")
								{
									Letter3Input = JumbledPasscode.ToCharArray()[LetterSelection].ToString();
									LetterAt++;
									if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
									}
									else
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Set_Char", "DLC_24-2_Hack_Password", true);
									}
								}
							}
							else if (LetterAt == 3)
							{
								if (Letter4Input == "")
								{
									Letter4Input = JumbledPasscode.ToCharArray()[LetterSelection].ToString();
									LetterAt++;
									if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
									}
									else
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Set_Char", "DLC_24-2_Hack_Password", true);
									}
								}
							}
							else if (LetterAt == 4 && Letter5Input == "")
							{
								Letter5Input = JumbledPasscode.ToCharArray()[LetterSelection].ToString();
								LetterAt++;
								ResetIn = Game.GameTime + 5000;
								if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Processing", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								}
								else if (Letter1Input != Letter1CorrectInput || Letter2Input != Letter2CorrectInput || Letter3Input != Letter3CorrectInput || Letter4Input != Letter4CorrectInput || Letter5Input != Letter5CorrectInput)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Bad_Guess", "DLC_24-2_Hack_Password", true);
								}
								else
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Success", "DLC_24-2_Hack_Password", true);
								}
							}
						}
						if (LetterAt == 0)
						{
							if (!Letter1Input.Equals(""))
							{
								LetterAt++;
							}
						}
						else if (LetterAt == 1)
						{
							if (!Letter2Input.Equals(""))
							{
								LetterAt++;
							}
						}
						else if (LetterAt == 2)
						{
							if (!Letter3Input.Equals(""))
							{
								LetterAt++;
							}
						}
						else if (LetterAt == 3)
						{
							if (!Letter4Input.Equals(""))
							{
								LetterAt++;
							}
						}
						else if (LetterAt == 4 && !Letter5Input.Equals(""))
						{
							LetterAt++;
							ResetIn = Game.GameTime + 5000;
						}
					}
					if (ResetHackIn != -1)
					{
						if (Game.GameTime < ResetHackIn)
						{
							if (HackCompleteType == 0)
							{
								drawSprite_NormalOrMemory(PasscodeHack_Popup_Correct, "mpwordhack_sprites", "wm_popup_complete", 0.51f, 0.5f, 0.385f, 0.315f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[5], "mpwordhack_sprites", "wm_green_node", 0.509f, 0.332f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								flag7 = true;
								drawSprite_NormalOrMemory(PasscodeHack_Line_1, "mpwordhack_sprites", "wm_line_1", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
								drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_selection_correct", 0.215f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter1Input, 0.367f + TextAdd, 0.16f, 1.65f, 124, 214, 138);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[0], "mpwordhack_sprites", "wm_green_node", 0.212f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[6], "mpwordhack_sprites", "wm_green_node", 0.2688f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Line_2, "mpwordhack_sprites", "wm_line_2", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[1], "mpwordhack_sprites", "wm_selection_correct", 0.36f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter2Input, 0.426f + TextAdd, 0.16f, 1.65f, 124, 214, 138);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[1], "mpwordhack_sprites", "wm_green_node", 0.36f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[7], "mpwordhack_sprites", "wm_green_node", 0.304f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[8], "mpwordhack_sprites", "wm_green_node", 0.416f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Line_3, "mpwordhack_sprites", "wm_line_3", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[2], "mpwordhack_sprites", "wm_selection_correct", 0.51f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter3Input, 0.488f + TextAdd, 0.16f, 1.65f, 124, 214, 138);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[2], "mpwordhack_sprites", "wm_green_node", 0.509f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[9], "mpwordhack_sprites", "wm_green_node", 0.452f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[10], "mpwordhack_sprites", "wm_green_node", 0.564f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Line_4, "mpwordhack_sprites", "wm_line_4", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[3], "mpwordhack_sprites", "wm_selection_correct", 0.6575f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter4Input, 0.548f + TextAdd, 0.16f, 1.65f, 124, 214, 138);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[3], "mpwordhack_sprites", "wm_green_node", 0.656f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[11], "mpwordhack_sprites", "wm_green_node", 0.601f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[12], "mpwordhack_sprites", "wm_green_node", 0.712f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Line_5, "mpwordhack_sprites", "wm_line_5", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[4], "mpwordhack_sprites", "wm_selection_correct", 0.805f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter5Input, 0.608f + TextAdd, 0.16f, 1.65f, 124, 214, 138);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[4], "mpwordhack_sprites", "wm_green_node", 0.805f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[13], "mpwordhack_sprites", "wm_green_node", 0.749f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							}
							if (HackCompleteType == 1)
							{
								drawSprite_NormalOrMemory(PasscodeHack_Popup_InCorrect, "mpwordhack_sprites", "wm_popup_locked", 0.51f, 0.5f, 0.385f, 0.315f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[5], "mpwordhack_sprites", "wm_red_node", 0.509f, 0.332f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								flag6 = true;
								drawSprite_NormalOrMemory(PasscodeHack_Line_1, "mpwordhack_sprites", "wm_line_1", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
								drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_selection_incorrect", 0.215f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter1Input, 0.367f + TextAdd, 0.16f, 1.65f, 180, 0, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[0], "mpwordhack_sprites", "wm_red_node", 0.212f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[6], "mpwordhack_sprites", "wm_red_node", 0.2688f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Line_2, "mpwordhack_sprites", "wm_line_2", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[1], "mpwordhack_sprites", "wm_selection_incorrect", 0.36f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter2Input, 0.426f + TextAdd, 0.16f, 1.65f, 180, 0, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[1], "mpwordhack_sprites", "wm_red_node", 0.36f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[7], "mpwordhack_sprites", "wm_red_node", 0.304f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[8], "mpwordhack_sprites", "wm_red_node", 0.416f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Line_3, "mpwordhack_sprites", "wm_line_3", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[2], "mpwordhack_sprites", "wm_selection_incorrect", 0.51f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter3Input, 0.488f + TextAdd, 0.16f, 1.65f, 180, 0, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[2], "mpwordhack_sprites", "wm_red_node", 0.509f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[9], "mpwordhack_sprites", "wm_red_node", 0.452f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[10], "mpwordhack_sprites", "wm_red_node", 0.564f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Line_4, "mpwordhack_sprites", "wm_line_4", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[3], "mpwordhack_sprites", "wm_selection_incorrect", 0.6575f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter4Input, 0.548f + TextAdd, 0.16f, 1.65f, 180, 0, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[3], "mpwordhack_sprites", "wm_red_node", 0.656f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[11], "mpwordhack_sprites", "wm_red_node", 0.601f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[12], "mpwordhack_sprites", "wm_red_node", 0.712f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Line_5, "mpwordhack_sprites", "wm_line_5", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[4], "mpwordhack_sprites", "wm_selection_incorrect", 0.805f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
								drawTextAdv(Letter5Input, 0.608f + TextAdd, 0.16f, 1.65f, 180, 0, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[4], "mpwordhack_sprites", "wm_red_node", 0.805f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
								drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[13], "mpwordhack_sprites", "wm_red_node", 0.749f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							}
						}
						else if (HackCompleteType == 0)
						{
							if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Target_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							}
							else
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Success", "DLC_24-2_Hack_Password", true);
							}
							HackFinishedID = 1;
							FiveLetterPasscodeHack = false;
							FiveLetterPasscodeStage = -1;
							ResetHackIn = -1;
							HackCompleteType = -1;
						}
						else
						{
							if (ReturnGameBuild() < GameVersiontoDrawWithMemory)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "No_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							}
							else
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Bad_Guess", "DLC_24-2_Hack_Password", true);
							}
							ResetHackIn = -1;
							FiveLetterPasscodeStage = 0;
							HackCompleteType = -1;
						}
					}
					if (LetterAt == 5)
					{
						Vector3 vector14 = new Vector3(124f, 214f, 138f);
						Vector3 vector15 = new Vector3(255f, 255f, 255f);
						Vector3 vector16 = new Vector3(255f, 0f, 0f);
						if (Letter1Input == Letter1CorrectInput && Letter2Input == Letter2CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_Green[0], "mpwordhack_sprites", "wm_node_line", 0.288f, 0.221f, 0.02f, 0.001f, 0f, (int)vector14.X, (int)vector14.Y, (int)vector14.Z, 255, 0);
						}
						else if (Letter1Input == Letter1CorrectInput || Letter2Input == Letter2CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_White[0], "mpwordhack_sprites", "wm_node_line", 0.288f, 0.221f, 0.02f, 0.001f, 0f, (int)vector15.X, (int)vector15.Y, (int)vector15.Z, 255, 0);
						}
						else if (Letter1Input != Letter1CorrectInput && Letter2Input != Letter2CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_Red[0], "mpwordhack_sprites", "wm_node_line", 0.288f, 0.221f, 0.02f, 0.001f, 0f, (int)vector16.X, (int)vector16.Y, (int)vector16.Z, 255, 0);
						}
						if (Letter2Input == Letter2CorrectInput && Letter3Input == Letter3CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_Green[1], "mpwordhack_sprites", "wm_node_line", 0.435f, 0.221f, 0.02f, 0.001f, 0f, (int)vector14.X, (int)vector14.Y, (int)vector14.Z, 255, 0);
						}
						else if (Letter2Input == Letter2CorrectInput || Letter3Input == Letter3CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_White[1], "mpwordhack_sprites", "wm_node_line", 0.435f, 0.221f, 0.02f, 0.001f, 0f, (int)vector15.X, (int)vector15.Y, (int)vector15.Z, 255, 0);
						}
						else if (Letter2Input != Letter2CorrectInput && Letter3Input != Letter3CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_Red[1], "mpwordhack_sprites", "wm_node_line", 0.435f, 0.221f, 0.02f, 0.001f, 0f, (int)vector16.X, (int)vector16.Y, (int)vector16.Z, 255, 0);
						}
						if (Letter3Input == Letter3CorrectInput && Letter4Input == Letter4CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_Green[2], "mpwordhack_sprites", "wm_node_line", 0.582f, 0.221f, 0.02f, 0.001f, 0f, (int)vector14.X, (int)vector14.Y, (int)vector14.Z, 255, 0);
						}
						else if (Letter3Input == Letter3CorrectInput || Letter4Input == Letter4CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_White[2], "mpwordhack_sprites", "wm_node_line", 0.582f, 0.221f, 0.02f, 0.001f, 0f, (int)vector15.X, (int)vector15.Y, (int)vector15.Z, 255, 0);
						}
						else if (Letter3Input != Letter3CorrectInput && Letter4Input != Letter4CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_Red[2], "mpwordhack_sprites", "wm_node_line", 0.582f, 0.221f, 0.02f, 0.001f, 0f, (int)vector16.X, (int)vector16.Y, (int)vector16.Z, 255, 0);
						}
						if (Letter4Input == Letter4CorrectInput && Letter5Input == Letter5CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_Green[3], "mpwordhack_sprites", "wm_node_line", 0.731f, 0.221f, 0.02f, 0.001f, 0f, (int)vector14.X, (int)vector14.Y, (int)vector14.Z, 255, 0);
						}
						else if (Letter4Input == Letter4CorrectInput || Letter5Input == Letter5CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_White[3], "mpwordhack_sprites", "wm_node_line", 0.731f, 0.221f, 0.02f, 0.001f, 0f, (int)vector15.X, (int)vector15.Y, (int)vector15.Z, 255, 0);
						}
						else if (Letter4Input != Letter4CorrectInput && Letter5Input != Letter5CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Lines_Red[3], "mpwordhack_sprites", "wm_node_line", 0.731f, 0.221f, 0.02f, 0.001f, 0f, (int)vector16.X, (int)vector16.Y, (int)vector16.Z, 255, 0);
						}
						if (Letter1Input != Letter1CorrectInput || Letter2Input != Letter2CorrectInput || Letter3Input != Letter3CorrectInput || Letter4Input != Letter4CorrectInput || Letter5Input != Letter5CorrectInput)
						{
							drawSprite_NormalOrMemory(PasscodeHack_Popup_InCorrect, "mpwordhack_sprites", "wm_popup_incorrect", 0.51f, 0.5f, 0.385f, 0.315f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[5], "mpwordhack_sprites", "wm_red_node", 0.509f, 0.332f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							flag6 = true;
						}
						else
						{
							drawSprite_NormalOrMemory(PasscodeHack_Popup_InCorrect, "mpwordhack_sprites", "wm_green_node", 0.509f, 0.332f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_popup_complete", 0.51f, 0.5f, 0.385f, 0.315f, 0f, 255, 255, 255, 255, 0);
							flag7 = true;
						}
						if (Letter1Input == Letter1CorrectInput)
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_1", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[0], "mpwordhack_sprites", "wm_selection_correct", 0.215f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector9 = new Vector3(124f, 214f, 138f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[0], "mpwordhack_sprites", "wm_green_node", 0.212f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[6], "mpwordhack_sprites", "wm_green_node", 0.2688f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						else
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_1", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[0], "mpwordhack_sprites", "wm_selection_incorrect", 0.215f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector9 = new Vector3(180f, 0f, 0f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[0], "mpwordhack_sprites", "wm_red_node", 0.212f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[6], "mpwordhack_sprites", "wm_red_node", 0.2688f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						if (Letter2Input == Letter2CorrectInput)
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_2", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[1], "mpwordhack_sprites", "wm_selection_correct", 0.36f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector10 = new Vector3(124f, 214f, 138f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[1], "mpwordhack_sprites", "wm_green_node", 0.36f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[7], "mpwordhack_sprites", "wm_green_node", 0.304f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[8], "mpwordhack_sprites", "wm_green_node", 0.416f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						else
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_2", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[1], "mpwordhack_sprites", "wm_selection_incorrect", 0.36f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector10 = new Vector3(180f, 0f, 0f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[1], "mpwordhack_sprites", "wm_red_node", 0.36f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[7], "mpwordhack_sprites", "wm_red_node", 0.304f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[8], "mpwordhack_sprites", "wm_red_node", 0.416f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						if (Letter3Input == Letter3CorrectInput)
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_3", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[2], "mpwordhack_sprites", "wm_selection_correct", 0.51f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector11 = new Vector3(124f, 214f, 138f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[2], "mpwordhack_sprites", "wm_green_node", 0.509f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[9], "mpwordhack_sprites", "wm_green_node", 0.452f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[10], "mpwordhack_sprites", "wm_green_node", 0.564f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						else
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_3", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[2], "mpwordhack_sprites", "wm_selection_incorrect", 0.51f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector11 = new Vector3(180f, 0f, 0f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[2], "mpwordhack_sprites", "wm_red_node", 0.509f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[9], "mpwordhack_sprites", "wm_red_node", 0.452f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[10], "mpwordhack_sprites", "wm_red_node", 0.564f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						if (Letter4Input == Letter4CorrectInput)
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_4", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[3], "mpwordhack_sprites", "wm_selection_correct", 0.6575f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector12 = new Vector3(124f, 214f, 138f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[3], "mpwordhack_sprites", "wm_green_node", 0.656f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[11], "mpwordhack_sprites", "wm_green_node", 0.601f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[12], "mpwordhack_sprites", "wm_green_node", 0.712f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						else
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_4", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[3], "mpwordhack_sprites", "wm_selection_incorrect", 0.6575f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector12 = new Vector3(180f, 0f, 0f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[3], "mpwordhack_sprites", "wm_red_node", 0.656f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[11], "mpwordhack_sprites", "wm_red_node", 0.601f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[12], "mpwordhack_sprites", "wm_red_node", 0.712f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						if (Letter5Input == Letter5CorrectInput)
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_5", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 124, 214, 138, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_Correct[4], "mpwordhack_sprites", "wm_selection_correct", 0.805f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector13 = new Vector3(124f, 214f, 138f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[4], "mpwordhack_sprites", "wm_green_node", 0.805f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Green[13], "mpwordhack_sprites", "wm_green_node", 0.749f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						else
						{
							drawSprite_NormalOrMemory("mpwordhack_sprites", "wm_line_5", 0.5084f, 0.304f, 0.335f, 0.06f, 0f, 255, 0, 0, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Letter_Top_InCorrect[4], "mpwordhack_sprites", "wm_selection_incorrect", 0.805f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
							vector13 = new Vector3(180f, 0f, 0f);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[4], "mpwordhack_sprites", "wm_red_node", 0.805f, 0.277f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
							drawSprite_NormalOrMemory(PasscodeHack_Nodes_Red[13], "mpwordhack_sprites", "wm_red_node", 0.749f, 0.221f, 0.00675f, 0.010125f, 0f, 255, 255, 255, 255, 0);
						}
						if (Game.GameTime >= ResetIn)
						{
							if (Letter1Input != Letter1CorrectInput || Letter2Input != Letter2CorrectInput || Letter3Input != Letter3CorrectInput || Letter4Input != Letter4CorrectInput || Letter5Input != Letter5CorrectInput)
							{
								AttemptsLeft--;
							}
							LetterAt = 0;
							if (Letter1Input != Letter1CorrectInput)
							{
								Letter1Input = "";
							}
							if (Letter2Input != Letter2CorrectInput)
							{
								Letter2Input = "";
							}
							if (Letter3Input != Letter3CorrectInput)
							{
								Letter3Input = "";
							}
							if (Letter4Input != Letter4CorrectInput)
							{
								Letter4Input = "";
							}
							if (Letter5Input != Letter5CorrectInput)
							{
								Letter5Input = "";
							}
							if (Letter1Input == Letter1CorrectInput && Letter2Input == Letter2CorrectInput && Letter3Input == Letter3CorrectInput && Letter4Input == Letter4CorrectInput && Letter5Input == Letter5CorrectInput)
							{
								HackCompleteType = 0;
								ResetHackIn = Game.GameTime + 1;
							}
							if (AttemptsLeft == 0)
							{
								HackCompleteType = 1;
								ResetHackIn = Game.GameTime + 5000;
							}
						}
					}
					if (LetterSelection == 0)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom_Background[0], "mpwordhack_sprites", "wm_selection_marker", 0.215f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					}
					if (LetterSelection == 1)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom_Background[1], "mpwordhack_sprites", "wm_selection_marker", 0.36f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					}
					if (LetterSelection == 2)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom_Background[2], "mpwordhack_sprites", "wm_selection_marker", 0.51f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					}
					if (LetterSelection == 3)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom_Background[3], "mpwordhack_sprites", "wm_selection_marker", 0.6575f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					}
					if (LetterSelection == 4)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Bottom_Background[4], "mpwordhack_sprites", "wm_selection_marker", 0.805f, 0.745f, 0.0525f, 0.094f, 0f, 255, 255, 255, 255, 0);
					}
					if (LetterAt != 5)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Top[0], "mpwordhack_sprites", "wm_letter_tile", 0.215f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Top[1], "mpwordhack_sprites", "wm_letter_tile", 0.36f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Top[2], "mpwordhack_sprites", "wm_letter_tile", 0.51f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Top[3], "mpwordhack_sprites", "wm_letter_tile", 0.6575f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
						drawSprite_NormalOrMemory(PasscodeHack_Letter_Top[4], "mpwordhack_sprites", "wm_letter_tile", 0.805f, 0.2205f, 0.053f, 0.094f, 0f, 255, 255, 255, 255, 0);
					}
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					drawSprite_NormalOrMemory(PasscodeHack_Life_Background[0], "mpwordhack_sprites", "wm_life_background", -0.121f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Life_Background[1], "mpwordhack_sprites", "wm_life_background", -0.0845f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Life_Background[2], "mpwordhack_sprites", "wm_life_background", -0.0478f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Life_Background[3], "mpwordhack_sprites", "wm_life_background", -0.0107f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					drawSprite_NormalOrMemory(PasscodeHack_Life_Background[4], "mpwordhack_sprites", "wm_life_background", 0.0267f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					if (AttemptsLeft >= 1)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Life[0], "mpwordhack_sprites", "wm_life", -0.121f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					}
					if (AttemptsLeft >= 2)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Life[1], "mpwordhack_sprites", "wm_life", -0.0845f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					}
					if (AttemptsLeft >= 3)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Life[2], "mpwordhack_sprites", "wm_life", -0.0478f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					}
					if (AttemptsLeft >= 4)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Life[3], "mpwordhack_sprites", "wm_life", -0.0107f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					}
					if (AttemptsLeft >= 5)
					{
						drawSprite_NormalOrMemory(PasscodeHack_Life[4], "mpwordhack_sprites", "wm_life", 0.0267f, 0.851f, 0.01f, 0.0175f, 0f, 255, 255, 255, 255, 0);
					}
					if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[0].ToString(), 0.214f, 0.684f, 1.65f, 84, 84, 83);
					}
					else
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[0].ToString(), 0.19f, 0.793f, 1f, 84, 84, 83);
					}
					if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[1].ToString(), 0.358f, 0.685f, 1.65f, 84, 84, 83);
					}
					else
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[1].ToString(), 0.347f, 0.793f, 1f, 84, 84, 83);
					}
					if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[2].ToString(), 0.506f, 0.685f, 1.65f, 84, 84, 83);
					}
					else
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[2].ToString(), 0.508f, 0.793f, 1f, 84, 84, 83);
					}
					if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[3].ToString(), 0.657f, 0.685f, 1.65f, 84, 84, 83);
					}
					else
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[3].ToString(), 0.6675f, 0.793f, 1f, 84, 84, 83);
					}
					if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[4].ToString(), 0.802f, 0.685f, 1.65f, 84, 84, 83);
					}
					else
					{
						drawTextAdv(JumbledPasscode.ToCharArray()[4].ToString(), 0.825f, 0.793f, 1f, 84, 84, 83);
					}
					if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
					{
						drawTextAdv("ATTEMPTS", -0.075f, 0.8f, 0.4f, 255, 255, 255);
					}
					else
					{
						drawTextAdv("ATTEMPTS", -0.045f, 0.822f, 0.15f, 255, 255, 255);
					}
					if (flag7)
					{
						if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
						{
							drawTextAdv("PASSWORD ACQUIRED", 0.492f + TextAdd, 0.567f, 0.5f, 124, 214, 138);
						}
						else
						{
							drawTextAdv("PASSWORD ACQUIRED", 0.492f + TextAdd, 0.567f, 0.13f, 124, 214, 138);
						}
					}
					if (flag6)
					{
						if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
						{
							drawTextAdv("INCORRECT PASSWORD", 0.492f + TextAdd, 0.567f, 0.5f, 170, 0, 0);
						}
						else
						{
							drawTextAdv("INCORRECT PASSWORD", 0.492f + TextAdd, 0.567f, 0.13f, 220, 0, 0);
						}
					}
					if (Letter1Input != "")
					{
						if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
						{
							drawTextAdv(Letter1Input, 0.214f, 0.16f, 1.65f, (int)vector9.X, (int)vector9.Y, (int)vector9.Z);
						}
						else
						{
							drawTextAdv(Letter1Input, 0.19f, 0.165f, 1f, (int)vector9.X, (int)vector9.Y, (int)vector9.Z);
						}
					}
					if (Letter2Input != "")
					{
						if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
						{
							drawTextAdv(Letter2Input, 0.358f, 0.16f, 1.65f, (int)vector10.X, (int)vector10.Y, (int)vector10.Z);
						}
						else
						{
							drawTextAdv(Letter2Input, 0.347f, 0.165f, 1f, (int)vector10.X, (int)vector10.Y, (int)vector10.Z);
						}
					}
					if (Letter3Input != "")
					{
						if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
						{
							drawTextAdv(Letter3Input, 0.506f, 0.16f, 1.65f, (int)vector11.X, (int)vector11.Y, (int)vector11.Z);
						}
						else
						{
							drawTextAdv(Letter3Input, 0.508f, 0.165f, 1f, (int)vector11.X, (int)vector11.Y, (int)vector11.Z);
						}
					}
					if (Letter4Input != "")
					{
						if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
						{
							drawTextAdv(Letter4Input, 0.657f, 0.16f, 1.65f, (int)vector12.X, (int)vector12.Y, (int)vector12.Z);
						}
						else
						{
							drawTextAdv(Letter4Input, 0.6675f, 0.165f, 1f, (int)vector12.X, (int)vector12.Y, (int)vector12.Z);
						}
					}
					if (Letter5Input != "")
					{
						if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
						{
							drawTextAdv(Letter5Input, 0.802f, 0.16f, 1.65f, (int)vector13.X, (int)vector13.Y, (int)vector13.Z);
						}
						else
						{
							drawTextAdv(Letter5Input, 0.825f, 0.165f, 1f, (int)vector13.X, (int)vector13.Y, (int)vector13.Z);
						}
					}
					TimeSpan timeSpan2 = TimeSpan.FromMilliseconds(FiveLetterPasscodeHackTime - Game.GameTime);
					int seconds2 = timeSpan2.Seconds;
					int milliseconds2 = timeSpan2.Milliseconds;
					string text5 = timeSpan2.Milliseconds.ToString();
					if (milliseconds2 >= 100)
					{
						text5 = timeSpan2.Milliseconds.ToString().ToCharArray()[0].ToString() + timeSpan2.Milliseconds.ToString().ToCharArray()[1];
					}
					string text6 = "00.";
					text6 = ((seconds2 >= 9) ? (text6 + seconds2 + ".") : (text6 + "0" + seconds2 + "."));
					text6 = ((milliseconds2 >= 9) ? (text6 + text5) : (text6 + "0" + milliseconds2));
					if (ReturnGameBuild() >= GameVersiontoDrawWithMemory)
					{
						drawTextAdv(text6, 1.095f, 0.15f, 0.4f, 255, 255, 255);
					}
					else
					{
						drawTextAdv(text6, 1.085f, 0.159f, 0.115f, 255, 255, 255);
					}
					if (FiveLetterPasscodeHackTime - Game.GameTime <= 0 && ResetHackIn == -1)
					{
						HackCompleteType = 1;
						ResetHackIn = Game.GameTime + 5000;
					}
					break;
				}
				}
			}
			if (PlayingButtonPressMinigame)
			{
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				ShowButtonPressMinigame(CurrentSet, CurrentSet.Count * 5);
			}
			GUIOn = false;
			if (CircuitHack == 1)
			{
				CircuitHack = 0;
				CallCircuitHack();
			}
			if (BruteforceHack == 1)
			{
				CallBruteforceHack();
				BruteforceHack = 0;
			}
			if (FIBHack == 1)
			{
				CallFIBHack();
				FIBHack = 0;
			}
			if (DataHack == 1)
			{
				CallDataCrackHack();
				DataHack = 0;
			}
			if (VoltageHack == 1)
			{
				CallVoltageHack();
				VoltageHack = 0;
			}
			if (CopyPrintHack == 1)
			{
				CallCopyPrintHack();
				CopyPrintHack = 0;
			}
			if (PadLockHack == 1)
			{
				CallPadLockHack();
				PadLockHack = 0;
			}
			if (HotwireHack == 1)
			{
				CallHotwireHack();
				HotwireHack = 0;
			}
			if (VideoPlaying)
			{
				Function.Call(Hash.DRAW_BINK_MOVIE, HackGUI, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
			}
			if (!AppTrackifyOn && OnPhoneHomeScreen() && AppTrackifyWasRunning)
			{
				AppTrackifyOn = false;
				AppTrackify = -1;
				AppTrackifyWasRunning = false;
				Delete_AppTrackify();
				EndAppTrackify();
				Notification.Show("STOP APP TRACKIFY");
			}
			if (AppTrackifyOn && OnPhoneHomeScreen())
			{
				drawText("OnPhoneHomeScreen (START)  " + OnPhoneHomeScreen() + "||" + TrackifySequence, 0.1f, 0.6f, 0.3f, 255, 255, 255);
				AppTrackifyWasRunning = true;
				base.Interval = 1;
				float absoluteX;
				float absoluteY;
				LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
				Convert.ToInt32(absoluteX / 2f);
				Convert.ToInt32(absoluteX / 2f);
				Convert.ToInt32(absoluteY / 2f);
				Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
				Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
				if (TrackifySequence == -1)
				{
					NextTrackifyCall = Game.GameTime + 2000;
					AppTrackifyHandle = new Scaleform("APP_TRACKIFY");
					TrackifySequence = 0;
				}
				while (!AppTrackifyHandle.IsLoaded)
				{
					AppTrackifyHandle = new Scaleform("APP_TRACKIFY");
					Script.Wait(0);
					drawText("Loading APP_TRACKIFY " + OnPhoneHomeScreen(), 0.1f, 0.55f, 0.3f, 255, 255, 255);
				}
				if (AppTrackifyHandle.IsLoaded)
				{
					drawText("OnPhoneHomeScreen (SC LOADED) " + OnPhoneHomeScreen() + " || " + TrackifySequence + " || " + AppTrackifyHandle.Handle, 0.1f, 0.65f, 0.3f, 255, 255, 255);
					if (TrackifySequence == -1)
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "APP_FUNCTION");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 75);
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 100);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						TrackifySequence = 1;
						AppTrackifyHandle.CallFunction("SET_HEADER", "Trackify");
						AppTrackifyHandle.CallFunction("setupMainScreenSkipLoading");
					}
					if (TrackifySequence == 0 && Game.GameTime >= TrackifyOpenTime)
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "APP_FUNCTION");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 1);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						TrackifySequence = 1;
					}
					if (TrackifySequence == 1)
					{
						for (int num43 = 0; num43 < 4; num43++)
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "SET_DATA_SLOT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 23);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num43);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, -99);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 90);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 10);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 1f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 1);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						func_14(AppTrackifyHandle.Handle, "DISPLAY_VIEW", 23f, 1f, -1.08213043E+09f, -1.08213043E+09f, -1.08213043E+09f);
						if (NextTrackifyCall < Game.GameTime)
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "animateSweep");
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "setupMainScreenSkipLoading");
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "flashOn");
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							TrackifySequence = 2;
						}
					}
					if (TrackifySequence == 2)
					{
						func_14(AppTrackifyHandle.Handle, "DISPLAY_VIEW", 23f, 1f, -1.08213043E+09f, -1.08213043E+09f, -1.08213043E+09f);
					}
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "animateSweep");
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					int num44 = 10;
					if (TrackifyTargets_Enitity.Count > 0)
					{
						for (int num45 = 0; num45 < TrackifyTargets_Enitity.Count; num45++)
						{
							Vector3 position = TrackifyTargets_Enitity[num45].Position;
							float distance2 = GetDistance(Game.Player.Character.Position, position);
							float num46 = (position - Game.Player.Character.Position).ToHeading() - Game.Player.Character.Heading;
							float num47 = num46;
							Vector3.Angle(position, Game.Player.Character.Position);
							if (num46 <= -1f)
							{
								num47 = 360f + num46;
							}
							num47 = 360f - num47;
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "setTargetByID");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num45);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)num47);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)distance2);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num44);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num44);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num44);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
					}
					if (TrackifyTargets_Vector3.Count > 0)
					{
						for (int num48 = 0; num48 < TrackifyTargets_Vector3.Count; num48++)
						{
							Vector3 vector17 = TrackifyTargets_Vector3[num48];
							float distance3 = GetDistance(Game.Player.Character.Position, vector17);
							float num49 = (vector17 - Game.Player.Character.Position).ToHeading() - Game.Player.Character.Heading;
							float num50 = num49;
							Vector3.Angle(vector17, Game.Player.Character.Position);
							if (num49 <= -1f)
							{
								num50 = 360f + num49;
							}
							num50 = 360f - num50;
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "setTargetByID");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num48);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)num50);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)distance3);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num44);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num44);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num44);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, true);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
					}
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "animateSweep");
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "setupMainScreenSkipLoading");
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "flashOn");
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "testAllTargets");
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "checkRangeForAllTargets");
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "updateTargetPositionNew");
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle.Handle, "DISPLAY_VIEW");
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 23);
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, Function.Call<float>(Hash.TO_FLOAT, 1));
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					if (AppTrackifyHandle != null)
					{
						OutputArgument outputArgument = new OutputArgument();
						Function.Call(Hash.GET_MOBILE_PHONE_RENDER_ID, outputArgument);
						int result = outputArgument.GetResult<int>();
						Function.Call(Hash.SET_TEXT_RENDER_ID, result);
						AppTrackifyHandle.Render2DScreenSpace(new PointF(0f, 0f), new PointF(260f, 231f));
						Function.Call(Hash.SET_TEXT_RENDER_ID, 1);
					}
					drawText("mobilePhoneRenderId " + mobilePhoneRenderId + "|" + AppTrackifyHandle?.ToString() + "|" + DrawPhoneUiPos.ToString(), 0.1f, 0.5f, 0.3f, 255, 255, 255);
				}
			}
			if (SecuroServPhoneHackOn)
			{
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				if (VirusStage == 0)
				{
					DELETE_VIRUS();
					UPLOADVIRUS = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE_WITH_IGNORE_SUPER_WIDESCREEN, "APP_SECUROSERV_HACKING");
					if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, UPLOADVIRUS))
					{
						VirusStage = 1;
						SecuroServ_UploadProgress = 0f;
						uploadtimer = Game.GameTime + 4800;
						uploadviruskeypad = 10;
					}
				}
				if (VirusStage == 1)
				{
					RunScaleformFunction(UPLOADVIRUS, "initProgress");
					DRAW_VIRUS();
					if (Game.GameTime > uploadtimer)
					{
						VirusStage = 2;
						hacksounds = Function.Call<int>(Hash.GET_SOUND_ID);
						uploadviruskeypad = 20;
					}
				}
				if (VirusStage == 2)
				{
					RunScaleformFunction(UPLOADVIRUS, "initHacking", SecuroServ_UploadProgress);
					DRAW_VIRUS();
					if (SecuroServ_UploadProgress >= 100f)
					{
						VirusStage = 3;
						Function.Call(Hash.STOP_SOUND, hacksounds);
						uploadtimer = 0;
						uploadtimer = Game.GameTime + 3000;
						uploadviruskeypad = 30;
					}
					else if (SecuroServ_UploadProgress_ViaHelper)
					{
						SecuroServ_UploadProgress += 0.04f;
					}
				}
				if (VirusStage == 3)
				{
					RunScaleformFunction(UPLOADVIRUS, "initComplete");
					DRAW_VIRUS();
					if (Game.GameTime > uploadtimer)
					{
						HackFinishedID = 1;
						SecuroServPhoneHack = -1;
						SecuroServPhoneHackOn = false;
						VirusStage = 0;
						DELETE_VIRUS();
						uploadcomplete += 1f;
						uploadviruskeypad = -1;
					}
				}
			}
			if (KeypadHackOn)
			{
				Scaleform scaleform3 = new Scaleform("instructional_buttons");
				scaleform3.CallFunction("CLEAR_ALL");
				scaleform3.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform3.CallFunction("CREATE_CONTAINER");
				scaleform3.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Navigation Up");
				scaleform3.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Navigation Down");
				scaleform3.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Navigation Next");
				scaleform3.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Navigation Back");
				scaleform3.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Choose Dot");
				scaleform3.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
				scaleform3.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform3.Render2D();
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				GUIOn = true;
				if (Game.IsControlJustPressed((Control)HackKey))
				{
					Game.Player.Character.IsPositionFrozen = false;
					HackGUI = -1;
					Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
					Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
					Script.Wait(1);
					HackStage = 0;
					KeypadHackOn = false;
					HackFinishedID = 3;
				}
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				Function.Call(Hash.DISABLE_FRONTEND_THIS_FRAME);
				Function.Call(Hash.SUPPRESS_FRONTEND_RENDERING_THIS_FRAME);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
				switch (HackStage)
				{
				case 0:
				{
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match");
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match", false, -1);
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking");
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking", false, -1);
					new Random();
					VideoPlaying = false;
					StagesToComplete = 4;
					StagesToComplete = HacksToComplete;
					Random random3 = new Random();
					ChoosenNumber1 = random3.Next(0, 9);
					ChoosenNumber2 = random3.Next(0, 9);
					ChoosenNumber3 = random3.Next(0, 9);
					ChoosenNumber4 = random3.Next(0, 9);
					ChoosenNumbers.Clear();
					int num52 = 0;
					while (num52 <= 4)
					{
						int num53 = random3.Next(1, 9);
						bool flag13 = false;
						if (ChoosenNumbers.Count == 0)
						{
							num52++;
							ChoosenNumbers.Add(num53);
							continue;
						}
						foreach (int choosenNumber7 in ChoosenNumbers)
						{
							if (choosenNumber7 == num53)
							{
								flag13 = true;
							}
						}
						if (!flag13)
						{
							num52++;
							ChoosenNumbers.Add(num53);
						}
					}
					ChoosenNumber1 = ChoosenNumbers[0];
					ChoosenNumber2 = ChoosenNumbers[1];
					ChoosenNumber3 = ChoosenNumbers[2];
					ChoosenNumber4 = ChoosenNumbers[3];
					PHTimeLeft = 440f;
					CurrentPrint = 1;
					Lives = 6;
					PrintId.Clear();
					RandomPrints.Clear();
					SelectedPrints.Clear();
					int num54 = 0;
					while (num54 < 4)
					{
						int num55 = random3.Next(1, 5);
						bool flag14 = false;
						if (SelectedPrints.Count == 0)
						{
							num54++;
							SelectedPrints.Add(num55);
							continue;
						}
						foreach (int selectedPrint in SelectedPrints)
						{
							if (selectedPrint == num55)
							{
								flag14 = true;
							}
						}
						if (!flag14)
						{
							num54++;
							SelectedPrints.Add(num55);
						}
					}
					int num56 = 1;
					while (num56 <= 8)
					{
						int num57 = random3.Next(1, 9);
						bool flag15 = false;
						if (RandomPrints.Count == 0)
						{
							num56++;
							RandomPrints.Add(num57);
							PrintId.Add(new PrintTileID(0, 0));
							continue;
						}
						foreach (int randomPrint in RandomPrints)
						{
							if (randomPrint == num57)
							{
								flag15 = true;
							}
						}
						if (!flag15)
						{
							num56++;
							RandomPrints.Add(num57);
							PrintId.Add(new PrintTileID(0, 0));
						}
					}
					HackStage = 100;
					break;
				}
				case 1:
				{
					DotHackFailed = 0;
					CurrentDot = 1;
					CheckingPrint = false;
					LoadingTimer = 0;
					BarOn = 0;
					Selections = 0;
					BlipTimer1 = 61;
					BlipTimer2 = 61;
					SelectedTile1ID = -1;
					SelectedTile2ID = -1;
					SelectedTile3ID = -1;
					SelectedTile4ID = -1;
					SelectedTile5ID = -1;
					SelectedTile6ID = -1;
					BlipTile1 = false;
					BlipTile2 = false;
					BlipTile3 = false;
					BlipTile4 = false;
					BlipTile5 = false;
					BlipTile6 = false;
					BlipRow2 = false;
					BlipRow3 = false;
					BlipRow4 = false;
					BlipRow5 = false;
					BlipRow6 = false;
					Random random = new Random();
					CreatedRandomPatterns = false;
					ChoosenRandomPattern = random.Next(2, 5);
					RandomPatternAt = 0;
					CheckingPrint = false;
					SelectedTile1PrintID = random.Next(1, 5);
					SelectedTile2PrintID = random.Next(1, 5);
					SelectedTile3PrintID = random.Next(1, 5);
					SelectedTile4PrintID = random.Next(1, 5);
					SelectedTile5PrintID = random.Next(1, 5);
					SelectedTile6PrintID = random.Next(1, 5);
					SelectionStage = 1;
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Common");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Grid");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Grid1");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Grid2");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_GridDetails");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Decor");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Decor1");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mporderunlock_decor1");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mporderunlock_decor");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mporderunlock");
					int num51 = 0;
					while (num51 <= 3)
					{
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Print" + num51);
						num51++;
						Script.Wait(0);
					}
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggameoverlay");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggameoverlay1");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggame");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggamebg");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggamewin");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggamewin3");
					HackStage = 2;
					break;
				}
				case 2:
				{
					if (Lives <= 0)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Failed", "DLC_H3_Cas_Door_Minigame_Sounds", true);
						HackGUI = -1;
						Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
						Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
						VideoPlaying = false;
						HackStage = 4;
					}
					float absoluteX2;
					float absoluteY2;
					LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX2, out absoluteY2);
					Convert.ToInt32(absoluteX2 / 2f);
					float screenY = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					float screenX = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					drawSprite3("mphackinggamebg", "bg", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("mphackinggame", "Black_BG", 0.5f, 0.5f, 1f, 1f, 0f, 5, 5, 5, 100, 0);
					drawSprite3("mporderunlock", "background_layout", 0.5f, 0.5f, 0.6583334f, 47f / 54f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("mphackinggameoverlay", "grid_rgb_pixels", 0.5f, 0.5f, 0.6583334f, 47f / 54f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("mphackinggameoverlay1", "ScreenGrid", 0.5f, 0.5f, 0.6583334f, 47f / 54f, 0f, 255, 255, 255, 255, 0);
					if (!CreatedRandomPatterns)
					{
						if (RandomPatternAt >= ChoosenRandomPattern)
						{
							CreatedRandomPatterns = true;
						}
						if (RandomPatternAt < ChoosenRandomPattern)
						{
							PatternTimer++;
							if (PatternTimer < 36)
							{
								if (PatternTimer == 18)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Dot_Sequence_Choose_Flash", "DLC_H3_Cas_Door_Minigame_Sounds", true);
								}
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(1, SelectedTile1PrintID).X, GetDotPos(1, SelectedTile1PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(2, SelectedTile2PrintID).X, GetDotPos(2, SelectedTile2PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(3, SelectedTile3PrintID).X, GetDotPos(3, SelectedTile3PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(4, SelectedTile4PrintID).X, GetDotPos(4, SelectedTile4PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(5, SelectedTile5PrintID).X, GetDotPos(5, SelectedTile5PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(6, SelectedTile6PrintID).X, GetDotPos(6, SelectedTile6PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (PatternTimer >= 72 && PatternTimer <= 108)
							{
								if (PatternTimer == 90)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Dot_Sequence_Choose_Flash", "DLC_H3_Cas_Door_Minigame_Sounds", true);
								}
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(1, SelectedTile1PrintID).X, GetDotPos(1, SelectedTile1PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(2, SelectedTile2PrintID).X, GetDotPos(2, SelectedTile2PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(3, SelectedTile3PrintID).X, GetDotPos(3, SelectedTile3PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(4, SelectedTile4PrintID).X, GetDotPos(4, SelectedTile4PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(5, SelectedTile5PrintID).X, GetDotPos(5, SelectedTile5PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(6, SelectedTile6PrintID).X, GetDotPos(6, SelectedTile6PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (PatternTimer >= 144 && PatternTimer <= 180)
							{
								if (PatternTimer == 162)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Dot_Sequence_Choose_Flash", "DLC_H3_Cas_Door_Minigame_Sounds", true);
								}
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(1, SelectedTile1PrintID).X, GetDotPos(1, SelectedTile1PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(2, SelectedTile2PrintID).X, GetDotPos(2, SelectedTile2PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(3, SelectedTile3PrintID).X, GetDotPos(3, SelectedTile3PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(4, SelectedTile4PrintID).X, GetDotPos(4, SelectedTile4PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(5, SelectedTile5PrintID).X, GetDotPos(5, SelectedTile5PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(6, SelectedTile6PrintID).X, GetDotPos(6, SelectedTile6PrintID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (PatternTimer >= 180)
							{
								Random random2 = new Random();
								RandomPatternAt++;
								if (RandomPatternAt >= ChoosenRandomPattern)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "New_Line_Flash", "DLC_H3_Cas_Door_Minigame_Sounds", true);
								}
								if (RandomPatternAt < ChoosenRandomPattern && DotHackFailed == 0)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Dot_Sequence_Change", "DLC_H3_Cas_Door_Minigame_Sounds", true);
									SelectedTile1PrintID = random2.Next(1, 5);
									SelectedTile2PrintID = random2.Next(1, 5);
									SelectedTile3PrintID = random2.Next(1, 5);
									SelectedTile4PrintID = random2.Next(1, 5);
									SelectedTile5PrintID = random2.Next(1, 5);
									SelectedTile6PrintID = random2.Next(1, 5);
								}
								PatternTimer = 0;
							}
						}
					}
					if (Lives >= 1)
					{
						drawSprite3("mphackinggame", "life", 0.588f, 0.143f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 2)
					{
						drawSprite3("mphackinggame", "life", 0.645f, 0.143f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 3)
					{
						drawSprite3("mphackinggame", "life", 0.699f, 0.143f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 4)
					{
						drawSprite3("mphackinggame", "life", 0.755f, 0.143f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 5)
					{
						drawSprite3("mphackinggame", "life", 0.811f, 0.143f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 6)
					{
						drawSprite3("mphackinggame", "life", 0.866f, 0.143f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (CreatedRandomPatterns)
					{
						if (Game.IsControlPressed(Control.MultiplayerInfo))
						{
							if (ResizeMode == 0)
							{
								SizeX -= 0.001f;
							}
							else if (ResizeMode == 1)
							{
								SizeY -= 0.001f;
							}
						}
						if (Game.IsControlPressed(Control.VehicleDuck))
						{
							if (ResizeMode == 0)
							{
								SizeX += 0.001f;
							}
							else if (ResizeMode == 1)
							{
								SizeY += 0.001f;
							}
						}
						if (Game.IsControlJustPressed(Control.SelectWeapon))
						{
							if (ResizeMode == 0)
							{
								ResizeMode = 1;
							}
							else if (ResizeMode == 1)
							{
								ResizeMode = 0;
							}
						}
					}
					if (CurrentDot >= 1)
					{
						drawSprite3("mporderunlock", "inner_circles", 0.0713f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						drawSprite3("mporderunlock", "inner_circles", 0.0713f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						drawSprite3("mporderunlock", "inner_circles", 0.0713f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						drawSprite3("mporderunlock", "inner_circles", 0.0713f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						drawSprite3("mporderunlock", "inner_circles", 0.0713f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
					}
					if (CurrentDot >= 2 && BlipTile1)
					{
						if (!BlipRow2)
						{
							bool flag8 = false;
							BlipTimer2++;
							if (BlipTimer2 < 12)
							{
								flag8 = true;
							}
							if (BlipTimer2 >= 24 && BlipTimer2 < 36)
							{
								flag8 = true;
							}
							if (BlipTimer2 >= 48 && BlipTimer2 < 60)
							{
								flag8 = true;
							}
							if (BlipTimer2 >= 60)
							{
								BlipRow2 = true;
							}
							if (flag8)
							{
								drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
						}
						else
						{
							drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.172f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (CurrentDot >= 3 && BlipTile2)
					{
						if (!BlipRow3)
						{
							bool flag9 = false;
							BlipTimer2++;
							if (BlipTimer2 < 12)
							{
								flag9 = true;
							}
							if (BlipTimer2 >= 24 && BlipTimer2 < 36)
							{
								flag9 = true;
							}
							if (BlipTimer2 >= 48 && BlipTimer2 < 60)
							{
								flag9 = true;
							}
							if (BlipTimer2 >= 60)
							{
								BlipRow3 = true;
							}
							if (flag9)
							{
								drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
						}
						else
						{
							drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.271f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (CurrentDot >= 4 && BlipTile3)
					{
						if (!BlipRow4)
						{
							bool flag10 = false;
							BlipTimer2++;
							if (BlipTimer2 < 12)
							{
								flag10 = true;
							}
							if (BlipTimer2 >= 24 && BlipTimer2 < 36)
							{
								flag10 = true;
							}
							if (BlipTimer2 >= 48 && BlipTimer2 < 60)
							{
								flag10 = true;
							}
							if (BlipTimer2 >= 60)
							{
								BlipRow4 = true;
							}
							if (flag10)
							{
								drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
						}
						else
						{
							drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.371f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (CurrentDot >= 5 && BlipTile4)
					{
						if (!BlipRow5)
						{
							bool flag11 = false;
							BlipTimer2++;
							if (BlipTimer2 < 12)
							{
								flag11 = true;
							}
							if (BlipTimer2 >= 24 && BlipTimer2 < 36)
							{
								flag11 = true;
							}
							if (BlipTimer2 >= 48 && BlipTimer2 < 60)
							{
								flag11 = true;
							}
							if (BlipTimer2 >= 60)
							{
								BlipRow5 = true;
							}
							if (flag11)
							{
								drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
						}
						else
						{
							drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.472f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (CurrentDot >= 6 && BlipTile5)
					{
						if (!BlipRow6)
						{
							bool flag12 = false;
							BlipTimer2++;
							if (BlipTimer2 < 12)
							{
								flag12 = true;
							}
							if (BlipTimer2 >= 24 && BlipTimer2 < 36)
							{
								flag12 = true;
							}
							if (BlipTimer2 >= 48 && BlipTimer2 < 60)
							{
								flag12 = true;
							}
							if (BlipTimer2 >= 60)
							{
								BlipRow6 = true;
							}
							if (flag12)
							{
								drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
								drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
						}
						else
						{
							drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.315f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.417f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.515f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.616f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							drawSprite3("mporderunlock", "inner_circles", 0.572f, 0.716f, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					drawSprite3("mporderunlock", "keypad_0", screenX, screenY, SizeX, SizeY, 0f, 255, 255, 255, 255, 0);
					if (StagesToComplete < 2)
					{
						drawSprite3("mporderunlock", "disabled_digit", 0.8442f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (StagesToComplete < 3)
					{
						drawSprite3("mporderunlock", "disabled_digit", 0.9114f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (StagesToComplete < 4)
					{
						drawSprite3("mporderunlock", "disabled_digit", 0.9775f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 1 && BlipTile6 && BlipRow6 && SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
					{
						int choosenNumber = ChoosenNumber1;
						if (choosenNumber == 0)
						{
							drawSprite3("mporderunlock", "keypad_0", 0.8774f, 0.6232f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 1)
						{
							drawSprite3("mporderunlock", "keypad_1", 0.7836f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 2)
						{
							drawSprite3("mporderunlock", "keypad_2", 0.8774f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 3)
						{
							drawSprite3("mporderunlock", "keypad_3", 0.9729f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 4)
						{
							drawSprite3("mporderunlock", "keypad_4", 0.7836f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 5)
						{
							drawSprite3("mporderunlock", "keypad_5", 0.8774f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 6)
						{
							drawSprite3("mporderunlock", "keypad_6", 0.9729f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 7)
						{
							drawSprite3("mporderunlock", "keypad_7", 0.7836f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 8)
						{
							drawSprite3("mporderunlock", "keypad_8", 0.8774f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber == 9)
						{
							drawSprite3("mporderunlock", "keypad_9", 0.9729f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						drawSprite3("mporderunlock", "keypad_feedback_" + ChoosenNumber1, 0.7781f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint >= 2)
					{
						int choosenNumber2 = ChoosenNumber2;
						if (choosenNumber2 == 0)
						{
							drawSprite3("mporderunlock", "keypad_0", 0.8774f, 0.6232f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 1)
						{
							drawSprite3("mporderunlock", "keypad_1", 0.7836f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 2)
						{
							drawSprite3("mporderunlock", "keypad_2", 0.8774f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 3)
						{
							drawSprite3("mporderunlock", "keypad_3", 0.9729f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 4)
						{
							drawSprite3("mporderunlock", "keypad_4", 0.7836f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 5)
						{
							drawSprite3("mporderunlock", "keypad_5", 0.8774f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 6)
						{
							drawSprite3("mporderunlock", "keypad_6", 0.9729f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 7)
						{
							drawSprite3("mporderunlock", "keypad_7", 0.7836f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 8)
						{
							drawSprite3("mporderunlock", "keypad_8", 0.8774f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber2 == 9)
						{
							drawSprite3("mporderunlock", "keypad_9", 0.9729f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						drawSprite3("mporderunlock", "keypad_feedback_" + ChoosenNumber2, 0.7781f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint >= 3)
					{
						int choosenNumber3 = ChoosenNumber2;
						if (choosenNumber3 == 0)
						{
							drawSprite3("mporderunlock", "keypad_0", 0.8774f, 0.6232f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 1)
						{
							drawSprite3("mporderunlock", "keypad_1", 0.7836f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 2)
						{
							drawSprite3("mporderunlock", "keypad_2", 0.8774f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 3)
						{
							drawSprite3("mporderunlock", "keypad_3", 0.9729f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 4)
						{
							drawSprite3("mporderunlock", "keypad_4", 0.7836f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 5)
						{
							drawSprite3("mporderunlock", "keypad_5", 0.8774f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 6)
						{
							drawSprite3("mporderunlock", "keypad_6", 0.9729f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 7)
						{
							drawSprite3("mporderunlock", "keypad_7", 0.7836f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 8)
						{
							drawSprite3("mporderunlock", "keypad_8", 0.8774f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber3 == 9)
						{
							drawSprite3("mporderunlock", "keypad_9", 0.9729f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						drawSprite3("mporderunlock", "keypad_feedback_" + ChoosenNumber2, 0.8442f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint >= 4)
					{
						int choosenNumber4 = ChoosenNumber3;
						if (choosenNumber4 == 0)
						{
							drawSprite3("mporderunlock", "keypad_0", 0.8774f, 0.6232f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 1)
						{
							drawSprite3("mporderunlock", "keypad_1", 0.7836f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 2)
						{
							drawSprite3("mporderunlock", "keypad_2", 0.8774f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 3)
						{
							drawSprite3("mporderunlock", "keypad_3", 0.9729f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 4)
						{
							drawSprite3("mporderunlock", "keypad_4", 0.7836f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 5)
						{
							drawSprite3("mporderunlock", "keypad_5", 0.8774f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 6)
						{
							drawSprite3("mporderunlock", "keypad_6", 0.9729f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 7)
						{
							drawSprite3("mporderunlock", "keypad_7", 0.7836f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 8)
						{
							drawSprite3("mporderunlock", "keypad_8", 0.8774f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber4 == 9)
						{
							drawSprite3("mporderunlock", "keypad_9", 0.9729f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						drawSprite3("mporderunlock", "keypad_feedback_" + ChoosenNumber3, 0.9114f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 4 && BlipTile6 && BlipRow6 && SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
					{
						int choosenNumber5 = ChoosenNumber4;
						if (choosenNumber5 == 0)
						{
							drawSprite3("mporderunlock", "keypad_0", 0.8774f, 0.6232f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 1)
						{
							drawSprite3("mporderunlock", "keypad_1", 0.7836f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 2)
						{
							drawSprite3("mporderunlock", "keypad_2", 0.8774f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 3)
						{
							drawSprite3("mporderunlock", "keypad_3", 0.9729f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 4)
						{
							drawSprite3("mporderunlock", "keypad_4", 0.7836f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 5)
						{
							drawSprite3("mporderunlock", "keypad_5", 0.8774f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 6)
						{
							drawSprite3("mporderunlock", "keypad_6", 0.9729f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 7)
						{
							drawSprite3("mporderunlock", "keypad_7", 0.7836f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 8)
						{
							drawSprite3("mporderunlock", "keypad_8", 0.8774f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber5 == 9)
						{
							drawSprite3("mporderunlock", "keypad_9", 0.9729f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						drawSprite3("mporderunlock", "keypad_feedback_" + ChoosenNumber4, 0.9775f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint >= 5)
					{
						int choosenNumber6 = ChoosenNumber4;
						if (choosenNumber6 == 0)
						{
							drawSprite3("mporderunlock", "keypad_0", 0.8774f, 0.6232f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 1)
						{
							drawSprite3("mporderunlock", "keypad_1", 0.7836f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 2)
						{
							drawSprite3("mporderunlock", "keypad_2", 0.8774f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 3)
						{
							drawSprite3("mporderunlock", "keypad_3", 0.9729f, 0.3392f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 4)
						{
							drawSprite3("mporderunlock", "keypad_4", 0.7836f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 5)
						{
							drawSprite3("mporderunlock", "keypad_5", 0.8774f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 6)
						{
							drawSprite3("mporderunlock", "keypad_6", 0.9729f, 0.4305f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 7)
						{
							drawSprite3("mporderunlock", "keypad_7", 0.7836f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 8)
						{
							drawSprite3("mporderunlock", "keypad_8", 0.8774f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						if (choosenNumber6 == 9)
						{
							drawSprite3("mporderunlock", "keypad_9", 0.9729f, 0.5276f, 0.05299f, 0.094f, 0f, 255, 255, 255, 255, 0);
						}
						drawSprite3("mporderunlock", "keypad_feedback_" + ChoosenNumber4, 0.9775f, 0.7733f, 0.035f, 0.059f, 0f, 255, 255, 255, 255, 0);
					}
					if (SelectedTile1ID != -1)
					{
						if (!BlipTile1)
						{
							BlipTimer1++;
							if (BlipTimer1 < 12)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(1, SelectedTile1ID).X, GetDotPos(1, SelectedTile1ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if ((BlipTimer1 >= 24) & (BlipTimer1 < 36))
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(1, SelectedTile1ID).X, GetDotPos(1, SelectedTile1ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 48 && BlipTimer1 < 60)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(1, SelectedTile1ID).X, GetDotPos(1, SelectedTile1ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 60)
							{
								BlipTile1 = true;
							}
						}
						if (BlipTile1)
						{
							drawSprite3("mporderunlock", "correct_circles", GetDotPos(1, SelectedTile1ID).X, GetDotPos(1, SelectedTile1ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (SelectedTile2ID != -1)
					{
						if (!BlipTile2)
						{
							BlipTimer1++;
							if (BlipTimer1 < 12)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(2, SelectedTile2ID).X, GetDotPos(2, SelectedTile2ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if ((BlipTimer1 >= 24) & (BlipTimer1 < 36))
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(2, SelectedTile2ID).X, GetDotPos(2, SelectedTile2ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 48 && BlipTimer1 < 60)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(2, SelectedTile2ID).X, GetDotPos(2, SelectedTile2ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 60)
							{
								BlipTile2 = true;
							}
						}
						if (BlipTile2)
						{
							drawSprite3("mporderunlock", "correct_circles", GetDotPos(2, SelectedTile2ID).X, GetDotPos(2, SelectedTile2ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (SelectedTile3ID != -1)
					{
						if (!BlipTile3)
						{
							BlipTimer1++;
							if (BlipTimer1 < 12)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(3, SelectedTile3ID).X, GetDotPos(3, SelectedTile3ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if ((BlipTimer1 >= 24) & (BlipTimer1 < 36))
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(3, SelectedTile3ID).X, GetDotPos(3, SelectedTile3ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 48 && BlipTimer1 < 60)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(3, SelectedTile3ID).X, GetDotPos(3, SelectedTile3ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 60)
							{
								BlipTile3 = true;
							}
						}
						if (BlipTile3)
						{
							drawSprite3("mporderunlock", "correct_circles", GetDotPos(3, SelectedTile3ID).X, GetDotPos(3, SelectedTile3ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (SelectedTile4ID != -1)
					{
						if (!BlipTile4)
						{
							BlipTimer1++;
							if (BlipTimer1 < 12)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(4, SelectedTile4ID).X, GetDotPos(4, SelectedTile4ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if ((BlipTimer1 >= 24) & (BlipTimer1 < 36))
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(4, SelectedTile4ID).X, GetDotPos(4, SelectedTile4ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 48 && BlipTimer1 < 60)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(4, SelectedTile4ID).X, GetDotPos(4, SelectedTile4ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 60)
							{
								BlipTile4 = true;
							}
						}
						if (BlipTile4)
						{
							drawSprite3("mporderunlock", "correct_circles", GetDotPos(4, SelectedTile4ID).X, GetDotPos(4, SelectedTile4ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (SelectedTile5ID != -1)
					{
						if (!BlipTile5)
						{
							BlipTimer1++;
							if (BlipTimer1 < 12)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(5, SelectedTile5ID).X, GetDotPos(5, SelectedTile5ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if ((BlipTimer1 >= 24) & (BlipTimer1 < 36))
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(5, SelectedTile5ID).X, GetDotPos(5, SelectedTile5ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 48 && BlipTimer1 < 60)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(5, SelectedTile5ID).X, GetDotPos(5, SelectedTile5ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 60)
							{
								BlipTile5 = true;
							}
						}
						if (BlipTile5)
						{
							drawSprite3("mporderunlock", "correct_circles", GetDotPos(5, SelectedTile5ID).X, GetDotPos(5, SelectedTile5ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (SelectedTile6ID != -1)
					{
						if (!BlipTile6)
						{
							BlipTimer1++;
							if (BlipTimer1 < 12)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(6, SelectedTile6ID).X, GetDotPos(6, SelectedTile6ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 24 && BlipTimer1 < 36)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(6, SelectedTile6ID).X, GetDotPos(6, SelectedTile6ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 48 && BlipTimer1 < 60)
							{
								drawSprite3("mporderunlock", "correct_circles", GetDotPos(6, SelectedTile6ID).X, GetDotPos(6, SelectedTile6ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
							}
							if (BlipTimer1 >= 60)
							{
								BlipTile6 = true;
							}
						}
						if (BlipTile6)
						{
							drawSprite3("mporderunlock", "correct_circles", GetDotPos(6, SelectedTile6ID).X, GetDotPos(6, SelectedTile6ID).Y, 0.0659f, 0.116f, 0f, 255, 255, 255, 225, 0);
						}
					}
					if (CurrentPrint == 1)
					{
						drawSprite3("mporderunlock", "keypad_feedback_box", 0.7781f, 0.7733f, 0.04099f, 0.07499f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 2)
					{
						drawSprite3("mporderunlock", "keypad_feedback_box", 0.8442f, 0.7733f, 0.04099f, 0.07499f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 3)
					{
						drawSprite3("mporderunlock", "keypad_feedback_box", 0.9114f, 0.7733f, 0.04099f, 0.07499f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 4)
					{
						drawSprite3("mporderunlock", "keypad_feedback_box", 0.9775f, 0.7733f, 0.04099f, 0.07499f, 0f, 255, 255, 255, 255, 0);
					}
					if (CreatedRandomPatterns)
					{
						if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
						{
							if (Game.IsControlJustPressed(Control.PhoneDown))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								SelectedTilePosY++;
								if (SelectedTilePosY >= 6)
								{
									SelectedTilePosY = 1;
								}
							}
							if (Game.IsControlJustPressed(Control.PhoneUp))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								SelectedTilePosY--;
								if (SelectedTilePosY <= 0)
								{
									SelectedTilePosY = 5;
								}
							}
						}
						else
						{
							if (Game.IsControlJustPressed(Control.MoveUpOnly))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								SelectedTilePosY++;
								if (SelectedTilePosY >= 6)
								{
									SelectedTilePosY = 1;
								}
							}
							if (Game.IsControlJustPressed(Control.MoveDownOnly))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								SelectedTilePosY--;
								if (SelectedTilePosY <= 0)
								{
									SelectedTilePosY = 5;
								}
							}
						}
						if (Game.IsControlJustPressed(Control.FrontendAccept))
						{
							if (CurrentDot == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								BlipTimer2 = 0;
								BlipTile1 = false;
								BlipTimer1 = 0;
								SelectedTile1ID = SelectedTilePosY;
							}
							else if (CurrentDot == 2)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								BlipTimer2 = 0;
								BlipTile2 = false;
								BlipTimer1 = 0;
								SelectedTile2ID = SelectedTilePosY;
							}
							else if (CurrentDot == 3)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								BlipTimer2 = 0;
								BlipTile3 = false;
								BlipTimer1 = 0;
								SelectedTile3ID = SelectedTilePosY;
							}
							else if (CurrentDot == 4)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								BlipTimer2 = 0;
								BlipTile4 = false;
								BlipTimer1 = 0;
								SelectedTile4ID = SelectedTilePosY;
							}
							else if (CurrentDot == 5)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								BlipTimer2 = 0;
								BlipTile5 = false;
								BlipTimer1 = 0;
								SelectedTile5ID = SelectedTilePosY;
							}
							else if (CurrentDot == 6)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								BlipTimer2 = 0;
								BlipTile6 = false;
								BlipTimer1 = 0;
								SelectedTile6ID = SelectedTilePosY;
							}
							int currentDot = CurrentDot;
							int num84 = 7;
							CurrentDot++;
						}
					}
					if (CreatedRandomPatterns)
					{
						if (CurrentDot >= 7 && BlipTile6 && BlipRow6)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Processing", "DLC_H3_Cas_Door_Minigame_Sounds", true);
							CheckingPrint = true;
						}
						if (CurrentDot >= 1 && CurrentDot <= 6 && BlipTimer2 >= 60 && BlipTimer1 >= 60)
						{
							drawSprite3("mporderunlock", "inner_circles", GetDotPos(CurrentDot, SelectedTilePosY).X, GetDotPos(CurrentDot, SelectedTilePosY).Y, 0.0759f, 0.128f, 0f, 255, 255, 255, 255, 0);
						}
					}
					TimeSpan timeSpan3 = TimeSpan.FromSeconds(PHTimeLeft);
					PHTimeLeft -= 0.015f;
					string text7 = $"{(int)timeSpan3.TotalMinutes:D2}:{timeSpan3.Seconds:D2}:{timeSpan3.Milliseconds:D2}";
					if (CheckingPrint)
					{
						if (LoadingTimer < 40)
						{
							if (LoadingTimer == 1)
							{
								if (SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Target_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								}
								else
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "No_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
								}
							}
							if (SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
							{
								drawSprite3("mphackinggame", "correct_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
							else
							{
								drawSprite3("mphackinggame", "incorrect_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
							LoadingTimer++;
						}
						if (LoadingTimer >= 40 && LoadingTimer < 80)
						{
							LoadingTimer++;
							if (SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
							{
								drawSprite3("mphackinggame", "correct_1", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
							else
							{
								drawSprite3("mphackinggame", "incorrect_1", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
						}
						if (LoadingTimer >= 80 && LoadingTimer < 120)
						{
							if (SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
							{
								drawSprite3("mphackinggame", "correct_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
							else
							{
								drawSprite3("mphackinggame", "incorrect_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
							LoadingTimer++;
						}
						if (LoadingTimer >= 120 && LoadingTimer < 160)
						{
							LoadingTimer++;
							if (SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
							{
								drawSprite3("mphackinggame", "correct_1", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
							else
							{
								drawSprite3("mphackinggame", "incorrect_1", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
						}
						if (LoadingTimer >= 160 && LoadingTimer < 200)
						{
							if (SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
							{
								drawSprite3("mphackinggame", "correct_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
							else
							{
								drawSprite3("mphackinggame", "incorrect_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
							}
							LoadingTimer++;
						}
						if (LoadingTimer >= 200)
						{
							CheckingPrint = false;
							if (SelectedTile1PrintID == SelectedTile1ID && SelectedTile2PrintID == SelectedTile2ID && SelectedTile3PrintID == SelectedTile3ID && SelectedTile4PrintID == SelectedTile4ID && SelectedTile5PrintID == SelectedTile5ID && SelectedTile6PrintID == SelectedTile6ID)
							{
								if (CurrentPrint >= StagesToComplete)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_H3_Cas_Door_Minigame_Sounds", true);
									HackGUI = -1;
									Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
									Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
									VideoPlaying = false;
									HackStage = 3;
								}
								else
								{
									DotHackFailed = 0;
									RandomPatternAt = 0;
									CreatedRandomPatterns = false;
									HackStage = 1;
									CurrentPrint++;
								}
							}
							else
							{
								DotHackFailed = 1;
								RandomPatternAt = 0;
								CreatedRandomPatterns = false;
								CurrentDot = 1;
								CheckingPrint = false;
								LoadingTimer = 0;
								BarOn = 0;
								Selections = 0;
								BlipTimer1 = 61;
								BlipTimer2 = 61;
								SelectedTile1ID = -1;
								SelectedTile2ID = -1;
								SelectedTile3ID = -1;
								SelectedTile4ID = -1;
								SelectedTile5ID = -1;
								SelectedTile6ID = -1;
								BlipTile1 = false;
								BlipTile2 = false;
								BlipTile3 = false;
								BlipTile4 = false;
								BlipTile5 = false;
								BlipTile6 = false;
								BlipRow2 = false;
								BlipRow3 = false;
								BlipRow4 = false;
								BlipRow5 = false;
								BlipRow6 = false;
								Lives--;
							}
							BarOn = 0;
							LoadingTimer = 0;
						}
					}
					if (PHTimeLeft > 0f)
					{
						char[] array = text7.ToString().ToCharArray();
						drawSprite3("mphackinggame", ReturnNFCPH(array[0]), 0.0834f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array[1]), 0.1174f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", "numbers_colon", 0.1479f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array[3]), 0.1752f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array[4]), 0.2078f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", "numbers_colon", 0.2359f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array[6]), 0.2616f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array[7]), 0.2959f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
					}
					break;
				}
				case 3:
					if (!VideoPlaying)
					{
						while (HackGUI <= 0)
						{
							HackGUI = Function.Call<int>(Hash.SET_BINK_MOVIE, "Success_FC");
							Script.Wait(0);
						}
						Function.Call(Hash.PLAY_BINK_MOVIE, HackGUI);
						if (HackGUI >= 1)
						{
							VideoPlaying = true;
						}
					}
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, HackGUI, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
						if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, HackGUI) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
						{
							HackGUI = -1;
							Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
							Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
							KeypadHackOn = false;
							HackFinishedID = 1;
							VideoPlaying = false;
							VideoPlaying = false;
						}
					}
					break;
				case 4:
					if (!VideoPlaying)
					{
						while (HackGUI <= 0)
						{
							HackGUI = Function.Call<int>(Hash.SET_BINK_MOVIE, "FAIL_FC");
							Script.Wait(0);
						}
						Function.Call(Hash.PLAY_BINK_MOVIE, HackGUI);
						if (HackGUI >= 1)
						{
							VideoPlaying = true;
						}
					}
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, HackGUI, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
						if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, HackGUI) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
						{
							HackGUI = -1;
							Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
							Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
							KeypadHackOn = false;
							HackFinishedID = 2;
							VideoPlaying = false;
						}
					}
					break;
				case 100:
					if (!VideoPlaying)
					{
						while (HackGUI <= 0)
						{
							HackGUI = Function.Call<int>(Hash.SET_BINK_MOVIE, "INTRO_FC");
							Script.Wait(1);
						}
						Function.Call(Hash.PLAY_BINK_MOVIE, HackGUI);
						if (HackGUI >= 1)
						{
							VideoPlaying = true;
						}
					}
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, HackGUI, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
						if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, HackGUI) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
						{
							HackGUI = -1;
							Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
							Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
							HackStage = 1;
							VideoPlaying = false;
						}
					}
					break;
				}
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				if (HackStage == 2)
				{
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						DisplayHelpTextThisFrame("Memorize the last Signal Patern and re-enter it to guess the keypad's code\n~INPUT_FRONTEND_ACCEPT~ Confirm Selected Node\nArrow Keys to Move Input Select\n" + ReturnButtonString(ExitKey) + "Quit Hack", 1);
					}
					if (Game.LastInputMethod == InputMethod.GamePad)
					{
						DisplayHelpTextThisFrame("Memorize the last Signal Patern and re-enter it to guess the keypad's code\n~INPUT_FRONTEND_ACCEPT~ Confirm Selected Node\nRight Joystick to Move Input Select\n" + ReturnButtonString(ExitKey) + "Quit Hack", 1);
					}
				}
			}
			if (FingerprintHackOn)
			{
				Scaleform scaleform4 = new Scaleform("instructional_buttons");
				scaleform4.CallFunction("CLEAR_ALL");
				scaleform4.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform4.CallFunction("CREATE_CONTAINER");
				scaleform4.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Navigation Up");
				scaleform4.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Navigation Down");
				scaleform4.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Navigation Next");
				scaleform4.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Navigation Back");
				scaleform4.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Choose Fingerprint");
				scaleform4.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
				scaleform4.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform4.Render2D();
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				GUIOn = true;
				if (Game.IsControlJustPressed((Control)ExitKey))
				{
					Game.Player.Character.IsPositionFrozen = false;
					HackGUI = -1;
					Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
					Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
					Script.Wait(1);
					HackStage = 0;
					FingerprintHackOn = false;
					HackFinishedID = 3;
				}
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				Function.Call(Hash.DISABLE_FRONTEND_THIS_FRAME);
				Function.Call(Hash.SUPPRESS_FRONTEND_RENDERING_THIS_FRAME);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
				switch (HackStage)
				{
				case 0:
				{
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match");
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Fingerprint_Match", false, -1);
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking");
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEIST3/Door_Hacking", false, -1);
					VideoPlaying = false;
					Random random4 = new Random();
					HacksToComplete = random4.Next(1, 4);
					if (OverrideHacksToComplete != 0)
					{
						HacksToComplete = OverrideHacksToComplete;
						OverrideHacksToComplete = 0;
					}
					StagesToComplete = HacksToComplete;
					PHTimeLeft = 440f;
					CurrentPrint = 0;
					Lives = random4.Next(4, 6);
					PrintId.Clear();
					RandomPrints.Clear();
					SelectedPrints.Clear();
					int num58 = 0;
					while (num58 < 4)
					{
						int num59 = new Random().Next(1, 5);
						bool flag16 = false;
						if (SelectedPrints.Count == 0)
						{
							num58++;
							SelectedPrints.Add(num59);
							continue;
						}
						foreach (int selectedPrint2 in SelectedPrints)
						{
							if (selectedPrint2 == num59)
							{
								flag16 = true;
							}
						}
						if (!flag16)
						{
							num58++;
							SelectedPrints.Add(num59);
						}
					}
					int num60 = 1;
					while (num60 <= 8)
					{
						int num61 = new Random().Next(1, 9);
						bool flag17 = false;
						if (RandomPrints.Count == 0)
						{
							num60++;
							RandomPrints.Add(num61);
							PrintId.Add(new PrintTileID(0, 0));
							continue;
						}
						foreach (int randomPrint2 in RandomPrints)
						{
							if (randomPrint2 == num61)
							{
								flag17 = true;
							}
						}
						if (!flag17)
						{
							num60++;
							RandomPrints.Add(num61);
							PrintId.Add(new PrintTileID(0, 0));
						}
					}
					HackStage = 100;
					break;
				}
				case 1:
				{
					LoadingTimer = 0;
					BarOn = 0;
					Selections = 0;
					SelectedTile1ID = -1;
					SelectedTile2ID = -1;
					SelectedTile3ID = -1;
					SelectedTile4ID = -1;
					SelectedTile1PrintID = -1;
					SelectedTile2PrintID = -1;
					SelectedTile3PrintID = -1;
					SelectedTile4PrintID = -1;
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Common");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Grid");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Grid1");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Grid2");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_GridDetails");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Decor");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Decor1");
					int num67 = 0;
					while (num67 <= 3)
					{
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "MPFClone_Print" + num67);
						num67++;
						Script.Wait(0);
					}
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggameoverlay");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggameoverlay1");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggame");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggamebg");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggamewin");
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggamewin3");
					HackStage = 2;
					break;
				}
				case 2:
				{
					if (Lives <= 0)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Failed", "DLC_H3_Cas_Door_Minigame_Sounds", true);
						HackGUI = -1;
						Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
						Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
						VideoPlaying = false;
						HackStage = 4;
					}
					float absoluteX3;
					float absoluteY3;
					LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX3, out absoluteY3);
					Convert.ToInt32(absoluteX3 / 2f);
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					drawSprite3("mphackinggamebg", "bg", 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("mphackinggame", "Black_BG", 0.5f, 0.5f, 1f, 1f, 0f, 5, 5, 5, 100, 0);
					drawSprite3("MPFClone_Common", "background_layout", 0.5f, 0.5f, 0.6583334f, 47f / 54f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("mphackinggameoverlay", "grid_rgb_pixels", 0.5f, 0.5f, 0.6583334f, 47f / 54f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("mphackinggameoverlay1", "ScreenGrid", 0.5f, 0.5f, 0.6583334f, 47f / 54f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("MPFClone_Decor", "techaration_" + 1, 0.5f, 0.5f, 0.6583334f, 47f / 54f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("MPFClone_GridDetails", "griddetails_" + 1, 0.439f, 0.379f, 1f / 120f, 0.4740741f, 0f, 255, 255, 255, 255, 0);
					drawSprite3("MPFClone_GridDetails", "griddetails_" + 1, 0.902f, 0.379f, 1f / 120f, 0.4740741f, 0f, 255, 255, 255, 255, 0);
					if (Lives >= 1)
					{
						drawSprite3("mphackinggame", "life", 0.983f, 0.242f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 2)
					{
						drawSprite3("mphackinggame", "life", 0.983f, 0.3f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 3)
					{
						drawSprite3("mphackinggame", "life", 0.983f, 0.352f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 4)
					{
						drawSprite3("mphackinggame", "life", 0.983f, 0.407f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 5)
					{
						drawSprite3("mphackinggame", "life", 0.983f, 0.465f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (Lives >= 6)
					{
						drawSprite3("mphackinggame", "life", 0.983f, 0.521f, 0.03333334f, 8f / 135f, 0f, 255, 255, 255, 255, 0);
					}
					if (StagesToComplete >= 1)
					{
						drawSprite3("MPFClone_Common", ("decypher_" + SelectedPrints[0]).ToString(), 0.538f, 0.834f, 0.066f, 0.1185f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 0)
					{
						drawSprite3("MPFClone_Common", "decyphered_selector", 0.538f, 0.834f, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (StagesToComplete >= 2)
					{
						drawSprite3("MPFClone_Common", ("decypher_" + SelectedPrints[1]).ToString(), 0.66f, 0.834f, 0.066f, 0.1185f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						drawSprite3("MPFClone_Common", "disabled_signal", 0.66f, 0.834f, 0.066f, 0.1185f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 1)
					{
						drawSprite3("MPFClone_Common", "decyphered_selector", 0.66f, 0.834f, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (StagesToComplete >= 3)
					{
						drawSprite3("MPFClone_Common", ("decypher_" + SelectedPrints[2]).ToString(), 0.781f, 0.834f, 0.066f, 0.1185f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						drawSprite3("MPFClone_Common", "disabled_signal", 0.781f, 0.834f, 0.066f, 0.1185f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 2)
					{
						drawSprite3("MPFClone_Common", "decyphered_selector", 0.781f, 0.834f, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (StagesToComplete >= 4)
					{
						drawSprite3("MPFClone_Common", ("decypher_" + SelectedPrints[3]).ToString(), 0.904f, 0.834f, 0.066f, 0.1185f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						drawSprite3("MPFClone_Common", "disabled_signal", 0.904f, 0.834f, 0.066f, 0.1185f, 0f, 255, 255, 255, 255, 0);
					}
					if (CurrentPrint == 3)
					{
						drawSprite3("MPFClone_Common", "decyphered_selector", 0.904f, 0.834f, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					DiscTimer++;
					if (DiscTimer <= 18)
					{
						drawSprite3("MPFClone_Common", "disc_a" + 0, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
						drawSprite3("MPFClone_Common", "disc_b" + 0, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
						drawSprite3("MPFClone_Common", "disc_c" + 0, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
					}
					if (DiscTimer > 18 && DiscTimer < 36)
					{
						drawSprite3("MPFClone_Common", "disc_a" + 1, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
						drawSprite3("MPFClone_Common", "disc_b" + 1, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
						drawSprite3("MPFClone_Common", "disc_c" + 1, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
					}
					if (DiscTimer >= 36)
					{
						drawSprite3("MPFClone_Common", "disc_a" + 2, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
						drawSprite3("MPFClone_Common", "disc_b" + 2, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
						drawSprite3("MPFClone_Common", "disc_c" + 2, 0.983f, 0.669f, 0.05208333f, 5f / 54f, 0f, 255, 255, 255, 255, 0);
					}
					if (DiscTimer >= 54)
					{
						DiscTimer = 0;
					}
					float uParam = 0f;
					float uParam2 = 0f;
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						if (Game.IsControlJustPressed(Control.PhoneDown))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							SelectedTilePosY++;
							if (SelectedTilePosY >= 5)
							{
								SelectedTilePosY = 1;
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneUp))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							SelectedTilePosY--;
							if (SelectedTilePosY <= 0)
							{
								SelectedTilePosY = 4;
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneLeft))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							SelectedTilePosX--;
							if (SelectedTilePosX <= 0)
							{
								SelectedTilePosX = 2;
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneRight))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							SelectedTilePosX++;
							if (SelectedTilePosX >= 3)
							{
								SelectedTilePosX = 1;
							}
						}
					}
					else
					{
						if (Game.IsControlJustPressed(Control.MoveDownOnly))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							SelectedTilePosY++;
							if (SelectedTilePosY >= 5)
							{
								SelectedTilePosY = 1;
							}
						}
						if (Game.IsControlJustPressed(Control.MoveUpOnly))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							SelectedTilePosY--;
							if (SelectedTilePosY <= 0)
							{
								SelectedTilePosY = 4;
							}
						}
						if (Game.IsControlJustPressed(Control.MoveLeftOnly))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							SelectedTilePosX--;
							if (SelectedTilePosX <= 0)
							{
								SelectedTilePosX = 2;
							}
						}
						if (Game.IsControlJustPressed(Control.MoveRightOnly))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							SelectedTilePosX++;
							if (SelectedTilePosX >= 3)
							{
								SelectedTilePosX = 1;
							}
						}
					}
					if (Game.IsControlJustPressed(Control.FrontendAccept))
					{
						if (SelectedTile4ID == CurrentSelectedTile)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Deselect_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							Selections--;
							SelectedTile4ID = -1;
						}
						else if (SelectedTile3ID == CurrentSelectedTile)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Deselect_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							Selections--;
							SelectedTile3ID = -1;
						}
						else if (SelectedTile2ID == CurrentSelectedTile)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Deselect_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							Selections--;
							SelectedTile2ID = -1;
						}
						else if (SelectedTile1ID == CurrentSelectedTile)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Deselect_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							Selections--;
							SelectedTile1ID = -1;
						}
						else if (SelectedTile1ID == -1)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							Selections++;
							SelectedTile1ID = CurrentSelectedTile;
						}
						else if (SelectedTile2ID == -1)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							Selections++;
							SelectedTile2ID = CurrentSelectedTile;
						}
						else if (SelectedTile3ID == -1)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							Selections++;
							SelectedTile3ID = CurrentSelectedTile;
						}
						else if (SelectedTile4ID == -1)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Select_Print_Tile", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
							Selections++;
							SelectedTile4ID = CurrentSelectedTile;
						}
					}
					if (Game.IsControlJustPressed(Control.SelectWeapon) && !CheckingPrint)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Processing", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
						CheckingPrint = true;
					}
					if (SelectedTile1ID != -1)
					{
						SelectedTile1PrintID = GetPrintId(SelectedTile1ID);
						GetTilePositions(SelectedTile1ID, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "decyphered_selector", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						SelectedTile1PrintID = -1;
					}
					if (SelectedTile2ID != -1)
					{
						SelectedTile2PrintID = GetPrintId(SelectedTile2ID);
						GetTilePositions(SelectedTile2ID, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "decyphered_selector", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						SelectedTile2PrintID = -1;
					}
					if (SelectedTile3ID != -1)
					{
						SelectedTile3PrintID = GetPrintId(SelectedTile3ID);
						GetTilePositions(SelectedTile3ID, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "decyphered_selector", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						SelectedTile3PrintID = -1;
					}
					if (SelectedTile4ID != -1)
					{
						SelectedTile4PrintID = GetPrintId(SelectedTile4ID);
						GetTilePositions(SelectedTile4ID, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "decyphered_selector", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						SelectedTile4PrintID = -1;
					}
					if (SelectedTilePosX == 1 && SelectedTilePosY == 1)
					{
						CurrentSelectedTile = 1;
						CurrentSelectedPrintID = RandomPrints[0];
						GetTilePositions(CurrentSelectedTile, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "selectorframe", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (SelectedTilePosX == 2 && SelectedTilePosY == 1)
					{
						CurrentSelectedTile = 2;
						CurrentSelectedPrintID = RandomPrints[1];
						GetTilePositions(CurrentSelectedTile, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "selectorframe", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (SelectedTilePosX == 1 && SelectedTilePosY == 2)
					{
						CurrentSelectedTile = 3;
						CurrentSelectedPrintID = RandomPrints[2];
						GetTilePositions(CurrentSelectedTile, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "selectorframe", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (SelectedTilePosX == 2 && SelectedTilePosY == 2)
					{
						CurrentSelectedTile = 4;
						CurrentSelectedPrintID = RandomPrints[3];
						GetTilePositions(CurrentSelectedTile, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "selectorframe", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (SelectedTilePosX == 1 && SelectedTilePosY == 3)
					{
						CurrentSelectedTile = 5;
						CurrentSelectedPrintID = RandomPrints[4];
						GetTilePositions(CurrentSelectedTile, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "selectorframe", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (SelectedTilePosX == 2 && SelectedTilePosY == 3)
					{
						CurrentSelectedTile = 6;
						CurrentSelectedPrintID = RandomPrints[5];
						GetTilePositions(CurrentSelectedTile, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "selectorframe", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (SelectedTilePosX == 1 && SelectedTilePosY == 4)
					{
						CurrentSelectedTile = 7;
						CurrentSelectedPrintID = RandomPrints[6];
						GetTilePositions(CurrentSelectedTile, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "selectorframe", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 255, 0);
					}
					if (SelectedTilePosX == 2 && SelectedTilePosY == 4)
					{
						CurrentSelectedTile = 8;
						CurrentSelectedPrintID = RandomPrints[7];
						GetTilePositions(CurrentSelectedTile, out uParam, out uParam2);
						drawSprite3("MPFClone_Common", "selectorframe", uParam, uParam2, 3f / 32f, 0.1666667f, 0f, 255, 255, 255, 120, 0);
					}
					int num62 = SelectedPrints[CurrentPrint] - 1;
					if (CheckingPrint)
					{
						PrintFlickerTimer++;
						new Random();
						if (PrintFlickerTimer >= 3)
						{
							HighLightPrint++;
							if (HighLightPrint >= 9)
							{
								HighLightPrint = 1;
							}
							PrintFlickerTimer = 0;
						}
						for (int num63 = 1; num63 <= 8; num63++)
						{
							if (num63 == HighLightPrint)
							{
								drawSprite3("MPFClone_Print" + num62, "fp" + SelectedPrints[CurrentPrint] + "_" + num63, 0.674f, 0.379f, 0.2083333f, 0.4740741f, 0f, 255, 255, 255, 255, 0);
							}
							else
							{
								drawSprite3("MPFClone_Print" + num62, "fp" + SelectedPrints[CurrentPrint] + "_" + num63, 0.674f, 0.379f, 0.2083333f, 0.4740741f, 0f, 255, 255, 255, 200, 0);
							}
						}
						if (CheckDelay >= 0f && CheckDelay < 20f)
						{
							CheckDelay += 1f;
							drawSprite3("mphackinggame", "loading_window_blank", 0.5f, 0.5f, 350f, 250f, 255, 255, 255, 255);
						}
						if (CheckDelay >= 20f && CheckDelay < 50f)
						{
							CheckDelay += 1f;
							drawSprite3("mphackinggame", "loading_window", 0.5f, 0.5f, 350f, 250f, 255, 255, 255, 255);
						}
						if (CheckDelay >= 50f && CheckDelay < 2000f)
						{
							drawSprite3("mphackinggame", "loading_window", 0.5f, 0.5f, 350f, 250f, 255, 255, 255, 255);
							if (BarOn < 21)
							{
								if (LoadingTimer != 10)
								{
									LoadingTimer++;
								}
								if (LoadingTimer == 10)
								{
									BarOn++;
									LoadingTimer = 0;
								}
							}
							if (LoadingBarPoints.Count > 0)
							{
								for (int num64 = 0; num64 < LoadingBarPoints.Count; num64++)
								{
									if (num64 < BarOn && num64 <= 12)
									{
										Vector2 vector23 = LoadingBarPoints[num64];
										drawSprite3("mphackinggame", "loading_bar_segment", 0.39f + 0.02f * (float)num64, 83f / 160f, 40f, 80f, 255, 255, 255, 255);
										CurrentLoadingSegment = num64;
									}
								}
								if (CurrentLoadingSegment >= 12)
								{
									if (LoadingTimer == 1)
									{
										if (GetSelectionResult(SelectedPrints[CurrentPrint]))
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Target_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
										}
										else
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "No_Match", "DLC_H3_Cas_Finger_Minigame_Sounds", true);
										}
									}
									if (LoadingTimer < 40)
									{
										if (GetSelectionResult(SelectedPrints[CurrentPrint]))
										{
											drawSprite3("mphackinggame", "correct_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										else
										{
											drawSprite3("mphackinggame", "incorrect_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										LoadingTimer++;
									}
									if (LoadingTimer >= 40 && LoadingTimer < 80)
									{
										LoadingTimer++;
										if (GetSelectionResult(SelectedPrints[CurrentPrint]))
										{
											drawSprite3("mphackinggame", "correct_1", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										else
										{
											drawSprite3("mphackinggame", "incorrect_1", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
									}
									if (LoadingTimer >= 80 && LoadingTimer < 120)
									{
										if (GetSelectionResult(SelectedPrints[CurrentPrint]))
										{
											drawSprite3("mphackinggame", "correct_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										else
										{
											drawSprite3("mphackinggame", "incorrect_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										LoadingTimer++;
									}
									if (LoadingTimer >= 120 && LoadingTimer < 160)
									{
										LoadingTimer++;
										if (GetSelectionResult(SelectedPrints[CurrentPrint]))
										{
											drawSprite3("mphackinggame", "correct_1", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										else
										{
											drawSprite3("mphackinggame", "incorrect_1", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
									}
									if (LoadingTimer >= 160 && LoadingTimer < 200)
									{
										if (GetSelectionResult(SelectedPrints[CurrentPrint]))
										{
											drawSprite3("mphackinggame", "correct_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										else
										{
											drawSprite3("mphackinggame", "incorrect_0", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										LoadingTimer++;
									}
									if (LoadingTimer >= 200)
									{
										CheckingPrint = false;
										if (GetSelectionResult(SelectedPrints[CurrentPrint]))
										{
											if (CurrentPrint == StagesToComplete - 1)
											{
												Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_H3_Cas_Door_Minigame_Sounds", true);
												HackGUI = -1;
												Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
												Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
												VideoPlaying = false;
												HackStage = 3;
											}
											else
											{
												HackStage = 1;
											}
											if (CurrentPrint < 3)
											{
												CurrentPrint++;
											}
										}
										else if (!GetSelectionResult(SelectedPrints[CurrentPrint]))
										{
											Lives--;
										}
										BarOn = 0;
										LoadingTimer = 0;
									}
								}
							}
						}
					}
					if (!CheckingPrint)
					{
						for (int num65 = 1; num65 <= 8; num65++)
						{
							drawSprite3("MPFClone_Print" + num62, "fp" + SelectedPrints[CurrentPrint] + "_" + num65, 0.674f, 0.379f, 0.2083333f, 0.4740741f, 0f, 255, 255, 255, 120, 0);
						}
					}
					for (int num66 = 0; num66 <= 7; num66++)
					{
						GetTilePositions(RandomPrints[num66], out uParam, out uParam2);
						string textureName = ("fp" + SelectedPrints[CurrentPrint] + "_comp_" + (num66 + 1)).ToString();
						if (!SelectedID(num66 + 1))
						{
							drawSprite3("MPFClone_Print" + num62, textureName, uParam, uParam2, 1f / 15f, 0.1185185f, 0f, 255, 255, 255, 120, 0);
						}
						else
						{
							drawSprite3("MPFClone_Print" + num62, textureName, uParam, uParam2, 1f / 15f, 0.1185185f, 0f, 255, 255, 255, 255, 0);
						}
						GetTilePositions(RandomPrints[num66], out uParam, out uParam2);
						int printID = ReturnID(uParam, uParam2);
						PrintId[num66].PrintID = printID;
						PrintId[num66].TileID = num66 + 1;
					}
					TimeSpan timeSpan4 = TimeSpan.FromSeconds(PHTimeLeft);
					PHTimeLeft -= 0.015f;
					string text8 = $"{(int)timeSpan4.TotalMinutes:D2}:{timeSpan4.Seconds:D2}:{timeSpan4.Milliseconds:D2}";
					if (PHTimeLeft > 0f)
					{
						char[] array2 = text8.ToString().ToCharArray();
						drawSprite3("mphackinggame", ReturnNFCPH(array2[0]), 0.0834f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array2[1]), 0.1174f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", "numbers_colon", 0.1479f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array2[3]), 0.1752f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array2[4]), 0.2078f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", "numbers_colon", 0.2359f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array2[6]), 0.2616f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mphackinggame", ReturnNFCPH(array2[7]), 0.2959f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
					}
					break;
				}
				case 3:
					if (!VideoPlaying)
					{
						while (HackGUI <= 0)
						{
							HackGUI = Function.Call<int>(Hash.SET_BINK_MOVIE, "Success_FC");
							Script.Wait(0);
						}
						Function.Call(Hash.PLAY_BINK_MOVIE, HackGUI);
						if (HackGUI >= 1)
						{
							VideoPlaying = true;
						}
					}
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, HackGUI, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
						if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, HackGUI) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
						{
							HackGUI = -1;
							Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
							Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
							VideoPlaying = false;
							FingerprintHackOn = false;
							HackFinishedID = 1;
						}
					}
					break;
				case 4:
					if (!VideoPlaying)
					{
						while (HackGUI <= 0)
						{
							HackGUI = Function.Call<int>(Hash.SET_BINK_MOVIE, "FAIL_FC");
							Script.Wait(0);
						}
						Function.Call(Hash.PLAY_BINK_MOVIE, HackGUI);
						if (HackGUI >= 1)
						{
							VideoPlaying = true;
						}
					}
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, HackGUI, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
						if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, HackGUI) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
						{
							HackGUI = -1;
							Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
							Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
							VideoPlaying = false;
							FingerprintHackOn = false;
							HackFinishedID = 2;
						}
					}
					break;
				case 100:
					if (!VideoPlaying)
					{
						while (HackGUI <= 0)
						{
							HackGUI = Function.Call<int>(Hash.SET_BINK_MOVIE, "INTRO_FC");
							Script.Wait(1);
						}
						Function.Call(Hash.PLAY_BINK_MOVIE, HackGUI);
						if (HackGUI >= 1)
						{
							VideoPlaying = true;
						}
					}
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, HackGUI, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
						if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, HackGUI) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
						{
							HackGUI = -1;
							Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
							Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
							HackStage = 1;
							VideoPlaying = false;
						}
					}
					break;
				}
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				if (HackStage == 2)
				{
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						DisplayHelpTextThisFrame("Select the correct prints from available options \n~INPUT_SELECT_WEAPON~ Check Input ~INPUT_FRONTEND_ACCEPT~ Select Print\nArrow Keys to Move Input Select\n" + ReturnButtonString(ExitKey) + " Quit Hack", 1);
					}
					if (Game.LastInputMethod == InputMethod.GamePad)
					{
						DisplayHelpTextThisFrame("Select the correct prints from available options \n~INPUT_SELECT_WEAPON~ Check Input ~INPUT_FRONTEND_ACCEPT~ Select Print\nRight Joystick to Move Input Select\n" + ReturnButtonString(ExitKey) + " Quit Hack", 1);
					}
				}
			}
			if (CopyPrintHackOn)
			{
				Scaleform scaleform5 = new Scaleform("instructional_buttons");
				scaleform5.CallFunction("CLEAR_ALL");
				scaleform5.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform5.CallFunction("CREATE_CONTAINER");
				scaleform5.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Navigation Up");
				scaleform5.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Navigation Down");
				scaleform5.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Navigation Next");
				scaleform5.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Navigation Back");
				scaleform5.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Choose Fingerprint");
				scaleform5.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
				scaleform5.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform5.Render2D();
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				GUIOn = true;
				if (Game.IsControlJustPressed((Control)ExitKey))
				{
					HackFinishedID = 3;
					Game.Player.CanControlCharacter = true;
					VideoPlaying = false;
					BINKMovie = -1;
					BINKMovie = -1;
					Game.Player.Character.IsPositionFrozen = false;
					CopyPrintHackOn = false;
				}
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				PlayingVoltageMG = false;
				if (HackStage == 3)
				{
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, BINKMovie, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
					}
					if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, BINKMovie) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
					{
						LoadDict("anim@scripted@heist@ig14_elevator_hack@male@");
						Game.Player.CanControlCharacter = true;
						VideoPlaying = false;
						Function.Call(Hash.STOP_BINK_MOVIE, BINKMovie);
						Function.Call(Hash.RELEASE_BINK_MOVIE, BINKMovie);
						BINKMovie = -1;
						Game.Player.Character.IsPositionFrozen = false;
						CopyPrintHackOn = false;
						PHHackSuccess = false;
					}
				}
				if (HackStage == 4)
				{
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, BINKMovie, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
					}
					if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, BINKMovie) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
					{
						LoadDict("anim@scripted@heist@ig14_elevator_hack@male@");
						Game.Player.CanControlCharacter = true;
						VideoPlaying = false;
						Function.Call(Hash.STOP_BINK_MOVIE, BINKMovie);
						Function.Call(Hash.RELEASE_BINK_MOVIE, BINKMovie);
						BINKMovie = -1;
						Game.Player.Character.IsPositionFrozen = false;
						CopyPrintHackOn = false;
						PHHackSuccess = true;
						Game.Player.Character.Task.ClearAllImmediately();
					}
				}
				if (HackStage == 0)
				{
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, BINKMovie, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
					}
					if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, BINKMovie) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
					{
						Function.Call(Hash.STOP_BINK_MOVIE, BINKMovie);
						Function.Call(Hash.RELEASE_BINK_MOVIE, BINKMovie);
						BINKMovie = -1;
						BarOn = 0;
						Checking = false;
						CheckDelay = 0f;
						VideoPlaying = false;
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_scrambler"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_scrambler", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_printfull8"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_printfull8", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_printfull7"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_printfull7", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_printfull6"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_printfull6", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_printfull5"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_printfull5", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_printfull4"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_printfull4", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_printfull3"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_printfull3", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_printfull2"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_printfull2", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_printfull1"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_printfull1", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_print7"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_print7", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_print6"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_print6", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_print5"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_print5", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_print4"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_print4", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_print3"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_print3", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_print2"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_print2", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_print1"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_print1", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_print0"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_print0", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_numbers"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_numbers", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_messages"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_messages", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_loading"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_loading", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_life"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_life", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_correct"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_correct", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_correct"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_correct", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro_background"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro_background", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_retro"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_retro", 0);
						}
						while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpfclone_common"))
						{
							Script.Wait(10);
							Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpfclone_common", 0);
						}
						Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLC_HEI4_FH_MG");
						Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLC_HEI4_FH_MG", false, -1);
						HackStage = 1;
					}
				}
				if (HackStage == 1)
				{
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
					ShowInstructionalButtonsCFPH();
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
					drawSprite3("mpfclone_retro_background", "fingerprint_hacking_minigame_background", 0.5f, 0.5f, 1920f, 1920f, 0, 173, 55, 255);
					drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum).ToString(), 0.729f, 0.5891f, 500f, 1100f, 0, 173, 55, 255);
					if (Game.IsControlJustPressed(Control.MoveLeftOnly))
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", uParam0f_741, true);
						if (SectionSelected == 1)
						{
							Selection1++;
							if (Selection1 < 0)
							{
								Selection1 = 13;
							}
							else if (Selection1 > 13)
							{
								Selection1 = 0;
							}
						}
						if (SectionSelected == 2)
						{
							Selection2++;
							if (Selection2 < 0)
							{
								Selection2 = 13;
							}
							else if (Selection2 > 9)
							{
								Selection2 = 0;
							}
						}
						if (SectionSelected == 3)
						{
							Selection3++;
							if (Selection3 < 0)
							{
								Selection3 = 13;
							}
							else if (Selection3 > 13)
							{
								Selection3 = 0;
							}
						}
						if (SectionSelected == 4)
						{
							Selection4++;
							if (Selection4 < 0)
							{
								Selection4 = 13;
							}
							else if (Selection4 > 13)
							{
								Selection4 = 0;
							}
						}
						if (SectionSelected == 5)
						{
							Selection5++;
							if (Selection5 < 0)
							{
								Selection5 = 13;
							}
							else if (Selection5 > 13)
							{
								Selection5 = 0;
							}
						}
						if (SectionSelected == 6)
						{
							Selection6++;
							if (Selection6 < 0)
							{
								Selection6 = 13;
							}
							else if (Selection6 > 13)
							{
								Selection6 = 0;
							}
						}
						if (SectionSelected == 7)
						{
							Selection7++;
							if (Selection7 < 0)
							{
								Selection7 = 13;
							}
							else if (Selection7 > 13)
							{
								Selection7 = 0;
							}
						}
						if (SectionSelected == 8)
						{
							Selection8++;
							if (Selection8 < 0)
							{
								Selection8 = 13;
							}
							else if (Selection8 > 13)
							{
								Selection8 = 0;
							}
						}
					}
					if (Game.IsControlJustPressed(Control.MoveRightOnly))
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", uParam0f_741, true);
						if (SectionSelected == 1)
						{
							Selection1--;
							if (Selection1 < 0)
							{
								Selection1 = 13;
							}
							else if (Selection1 > 13)
							{
								Selection1 = 0;
							}
						}
						if (SectionSelected == 2)
						{
							Selection2--;
							if (Selection2 < 0)
							{
								Selection2 = 13;
							}
							else if (Selection2 > 13)
							{
								Selection2 = 0;
							}
						}
						if (SectionSelected == 3)
						{
							Selection3--;
							if (Selection3 < 0)
							{
								Selection3 = 13;
							}
							else if (Selection3 > 13)
							{
								Selection3 = 0;
							}
						}
						if (SectionSelected == 4)
						{
							Selection4--;
							if (Selection4 < 0)
							{
								Selection4 = 13;
							}
							else if (Selection4 > 13)
							{
								Selection4 = 0;
							}
						}
						if (SectionSelected == 5)
						{
							Selection5--;
							if (Selection5 < 0)
							{
								Selection5 = 13;
							}
							else if (Selection5 > 13)
							{
								Selection5 = 0;
							}
						}
						if (SectionSelected == 6)
						{
							Selection6--;
							if (Selection6 < 0)
							{
								Selection6 = 13;
							}
							else if (Selection6 > 13)
							{
								Selection6 = 0;
							}
						}
						if (SectionSelected == 7)
						{
							Selection7--;
							if (Selection7 < 0)
							{
								Selection7 = 13;
							}
							else if (Selection7 > 13)
							{
								Selection7 = 0;
							}
						}
						if (SectionSelected == 8)
						{
							Selection8--;
							if (Selection8 < 0)
							{
								Selection8 = 13;
							}
							else if (Selection8 > 13)
							{
								Selection8 = 0;
							}
						}
					}
					if (Game.IsControlJustPressed(Control.MoveUpOnly))
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", uParam0f_741, true);
						SectionSelected--;
						if (SectionSelected == 0)
						{
							SectionSelected = 8;
						}
					}
					if (Game.IsControlJustPressed(Control.MoveDownOnly))
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", uParam0f_741, true);
						SectionSelected++;
						if (SectionSelected == 9)
						{
							SectionSelected = 1;
						}
					}
					if (SectionSelected == 1)
					{
						if (Selection1 == 0)
						{
							drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_01").ToString(), 0.1918f, 0.345f, 470f, 125f, 255, 255, 255, 255);
						}
						else
						{
							string[] array3 = Section1Print[Selection1].Split(new char[1] { ',' });
							string textureDict = array3[0];
							string textureName2 = array3[1];
							drawSprite3(textureDict, textureName2, 0.1918f, 0.345f, 470f, 125f, 255, 255, 255, 255);
						}
						drawSprite3("mpfclone_retro", "selectorframe", 0.1918f, 0.345f, 470f, 125f, 255, 255, 255, 255);
					}
					else if (Selection1 == 0)
					{
						drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_01").ToString(), 0.1918f, 0.345f, 470f, 125f, 0, 173, 55, 255);
					}
					else
					{
						string[] array4 = Section1Print[Selection1].Split(new char[1] { ',' });
						string textureDict2 = array4[0];
						string textureName3 = array4[1];
						drawSprite3(textureDict2, textureName3, 0.1918f, 0.345f, 470f, 125f, 0, 173, 55, 255);
					}
					if (SectionSelected == 2)
					{
						if (Selection2 == 0)
						{
							drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_02").ToString(), 0.1918f, 0.4138f, 470f, 125f, 255, 255, 255, 255);
						}
						else
						{
							string[] array5 = Section2Print[Selection2].Split(new char[1] { ',' });
							string textureDict3 = array5[0];
							string textureName4 = array5[1];
							drawSprite3(textureDict3, textureName4, 0.1918f, 0.4138f, 470f, 125f, 255, 255, 255, 255);
						}
						drawSprite3("mpfclone_retro", "selectorframe", 0.1918f, 0.4138f, 470f, 125f, 255, 255, 255, 255);
					}
					else if (Selection2 == 0)
					{
						drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_02").ToString(), 0.1918f, 0.4138f, 470f, 125f, 0, 173, 55, 255);
					}
					else
					{
						string[] array6 = Section2Print[Selection2].Split(new char[1] { ',' });
						string textureDict4 = array6[0];
						string textureName5 = array6[1];
						drawSprite3(textureDict4, textureName5, 0.1918f, 0.4138f, 470f, 125f, 0, 173, 55, 255);
					}
					if (SectionSelected == 3)
					{
						if (Selection3 == 0)
						{
							drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_03").ToString(), 0.1918f, 0.4833f, 470f, 125f, 255, 255, 255, 255);
						}
						else
						{
							string[] array7 = Section3Print[Selection3].Split(new char[1] { ',' });
							string textureDict5 = array7[0];
							string textureName6 = array7[1];
							drawSprite3(textureDict5, textureName6, 0.1918f, 0.4833f, 470f, 125f, 255, 255, 255, 255);
						}
						drawSprite3("mpfclone_retro", "selectorframe", 0.1918f, 0.4833f, 470f, 125f, 255, 255, 255, 255);
					}
					else if (Selection3 == 0)
					{
						drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_03").ToString(), 0.1918f, 0.4833f, 470f, 125f, 0, 173, 55, 255);
					}
					else
					{
						string[] array8 = Section3Print[Selection3].Split(new char[1] { ',' });
						string textureDict6 = array8[0];
						string textureName7 = array8[1];
						drawSprite3(textureDict6, textureName7, 0.1918f, 0.4833f, 470f, 125f, 0, 173, 55, 255);
					}
					if (SectionSelected == 4)
					{
						if (Selection4 == 0)
						{
							drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_04").ToString(), 0.1918f, 0.555f, 470f, 125f, 255, 255, 255, 255);
						}
						else
						{
							string[] array9 = Section4Print[Selection4].Split(new char[1] { ',' });
							string textureDict7 = array9[0];
							string textureName8 = array9[1];
							drawSprite3(textureDict7, textureName8, 0.1918f, 0.555f, 470f, 125f, 255, 255, 255, 255);
						}
						drawSprite3("mpfclone_retro", "selectorframe", 0.1918f, 0.555f, 470f, 125f, 255, 255, 255, 255);
					}
					else if (Selection4 == 0)
					{
						drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_04").ToString(), 0.1918f, 0.555f, 470f, 125f, 0, 173, 55, 255);
					}
					else
					{
						string[] array10 = Section4Print[Selection4].Split(new char[1] { ',' });
						string textureDict8 = array10[0];
						string textureName9 = array10[1];
						drawSprite3(textureDict8, textureName9, 0.1918f, 0.555f, 470f, 125f, 0, 173, 55, 255);
					}
					if (SectionSelected == 5)
					{
						if (Selection5 == 0)
						{
							drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_05").ToString(), 0.1918f, 0.627f, 470f, 125f, 255, 255, 255, 255);
						}
						else
						{
							string[] array11 = Section5Print[Selection5].Split(new char[1] { ',' });
							string textureDict9 = array11[0];
							string textureName10 = array11[1];
							drawSprite3(textureDict9, textureName10, 0.1918f, 0.627f, 470f, 125f, 255, 255, 255, 255);
						}
						drawSprite3("mpfclone_retro", "selectorframe", 0.1918f, 0.627f, 470f, 125f, 255, 255, 255, 255);
					}
					else if (Selection5 == 0)
					{
						drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_05").ToString(), 0.1918f, 0.627f, 470f, 125f, 0, 173, 55, 255);
					}
					else
					{
						string[] array12 = Section5Print[Selection5].Split(new char[1] { ',' });
						string textureDict10 = array12[0];
						string textureName11 = array12[1];
						drawSprite3(textureDict10, textureName11, 0.1918f, 0.627f, 470f, 125f, 0, 173, 55, 255);
					}
					if (SectionSelected == 6)
					{
						if (Selection6 == 0)
						{
							drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_06").ToString(), 0.1918f, 0.699f, 470f, 125f, 255, 255, 255, 255);
						}
						else
						{
							string[] array13 = Section6Print[Selection6].Split(new char[1] { ',' });
							string textureDict11 = array13[0];
							string textureName12 = array13[1];
							drawSprite3(textureDict11, textureName12, 0.1918f, 0.699f, 470f, 125f, 255, 255, 255, 255);
						}
						drawSprite3("mpfclone_retro", "selectorframe", 0.1918f, 0.699f, 470f, 125f, 255, 255, 255, 255);
					}
					else if (Selection6 == 0)
					{
						drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_06").ToString(), 0.1918f, 0.699f, 470f, 125f, 0, 173, 55, 255);
					}
					else
					{
						string[] array14 = Section6Print[Selection6].Split(new char[1] { ',' });
						string textureDict12 = array14[0];
						string textureName13 = array14[1];
						drawSprite3(textureDict12, textureName13, 0.1918f, 0.699f, 470f, 125f, 0, 173, 55, 255);
					}
					if (SectionSelected == 7)
					{
						if (Selection7 == 0)
						{
							drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_07").ToString(), 0.1918f, 0.7708f, 470f, 125f, 255, 255, 255, 255);
						}
						else
						{
							string[] array15 = Section7Print[Selection7].Split(new char[1] { ',' });
							string textureDict13 = array15[0];
							string textureName14 = array15[1];
							drawSprite3(textureDict13, textureName14, 0.1918f, 0.7708f, 470f, 125f, 255, 255, 255, 255);
						}
						drawSprite3("mpfclone_retro", "selectorframe", 0.1918f, 0.7708f, 470f, 125f, 255, 255, 255, 255);
					}
					else if (Selection7 == 0)
					{
						drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_07").ToString(), 0.1918f, 0.7708f, 470f, 125f, 0, 173, 55, 255);
					}
					else
					{
						string[] array16 = Section7Print[Selection7].Split(new char[1] { ',' });
						string textureDict14 = array16[0];
						string textureName15 = array16[1];
						drawSprite3(textureDict14, textureName15, 0.1918f, 0.7708f, 470f, 125f, 0, 173, 55, 255);
					}
					if (SectionSelected == 8)
					{
						if (Selection8 == 0)
						{
							drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_08").ToString(), 0.1918f, 0.844f, 470f, 125f, 255, 255, 255, 255);
						}
						else
						{
							string[] array17 = Section8Print[Selection8].Split(new char[1] { ',' });
							string textureDict15 = array17[0];
							string textureName16 = array17[1];
							drawSprite3(textureDict15, textureName16, 0.1918f, 0.844f, 470f, 125f, 255, 255, 255, 255);
						}
						drawSprite3("mpfclone_retro", "selectorframe", 0.1918f, 0.844f, 470f, 125f, 255, 255, 255, 255);
					}
					else if (Selection8 == 0)
					{
						drawSprite3(PrintDict, ("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_08").ToString(), 0.1918f, 0.844f, 470f, 125f, 0, 173, 55, 255);
					}
					else
					{
						string[] array18 = Section8Print[Selection8].Split(new char[1] { ',' });
						string textureDict16 = array18[0];
						string textureName17 = array18[1];
						drawSprite3(textureDict16, textureName17, 0.1918f, 0.844f, 470f, 125f, 0, 173, 55, 255);
					}
					drawSprite3("mpfclone_retro_scrambler", "fingerprint_hacking_minigame_scrambler_background", 0.2017f, 0.2361f, 470f, 55f, 0, 102, 32, 255);
					TimeSpan timeSpan5 = TimeSpan.FromSeconds(PHTimeLeft);
					PHTimeLeft -= 0.025f;
					string text9 = $"{(int)timeSpan5.TotalMinutes:D2}:{timeSpan5.Seconds:D2}:{timeSpan5.Milliseconds:D2}";
					if (PHTimeLeft > 0f)
					{
						char[] array19 = text9.ToString().ToCharArray();
						drawSprite3("mpfclone_retro_numbers", ReturnNFCPH_Retro(array19[0]), 0.0834f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mpfclone_retro_numbers", ReturnNFCPH_Retro(array19[1]), 0.1174f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mpfclone_retro_numbers", "fingerprint_hacking_minigame_numbers_colon", 0.1479f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mpfclone_retro_numbers", ReturnNFCPH_Retro(array19[3]), 0.1752f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mpfclone_retro_numbers", ReturnNFCPH_Retro(array19[4]), 0.2078f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mpfclone_retro_numbers", "fingerprint_hacking_minigame_numbers_colon", 0.2359f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mpfclone_retro_numbers", ReturnNFCPH_Retro(array19[6]), 0.2616f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
						drawSprite3("mpfclone_retro_numbers", ReturnNFCPH_Retro(array19[7]), 0.2959f, 0.1409f, 30f, 70f, 255, 255, 255, 255);
					}
					if (PHTimeLeft <= 0f && HacksToComplete != -1)
					{
						HacksToComplete = -1;
						BINKMovie = -1;
						BINKMovie = Function.Call<int>(Hash.SET_BINK_MOVIE, "fingerprint_fail");
						Function.Call(Hash.PLAY_BINK_MOVIE, BINKMovie);
						VideoPlaying = true;
						HackStage = 4;
						HackFinishedID = 2;
					}
					if (HacksToComplete >= 1)
					{
						drawSprite3("mpfclone_common", "decypher_1", 0.5503f, 0.1805f, 150f, 250f, 0, 173, 55, 255);
						if (HacksCompleted == 0)
						{
							drawSprite3("mpfclone_retro", "decyphered_selector", 0.5503f, 0.1805f, 150f, 250f, 255, 255, 255, 255);
						}
					}
					if (HacksToComplete >= 2)
					{
						drawSprite3("mpfclone_common", "decypher_2", 0.6805f, 0.1805f, 150f, 250f, 0, 173, 55, 255);
						if (HacksCompleted == 1)
						{
							drawSprite3("mpfclone_retro", "decyphered_selector", 0.6805f, 0.1805f, 150f, 250f, 255, 255, 255, 255);
						}
					}
					if (HacksToComplete >= 3)
					{
						drawSprite3("mpfclone_common", "decypher_3", 0.8087f, 0.1805f, 150f, 250f, 0, 173, 55, 255);
						if (HacksCompleted == 2)
						{
							drawSprite3("mpfclone_retro", "decyphered_selector", 0.8087f, 0.1805f, 150f, 250f, 255, 255, 255, 255);
						}
					}
					if (HacksToComplete >= 4)
					{
						drawSprite3("mpfclone_common", "decypher_4", 0.9349f, 0.1805f, 150f, 250f, 0, 173, 55, 255);
						if (HacksCompleted == 3)
						{
							drawSprite3("mpfclone_retro", "decyphered_selector", 0.9349f, 0.1805f, 150f, 250f, 255, 255, 255, 255);
						}
					}
					if (Section1Print[0].Equals(Section1Print[Selection1].ToString()) && Section2Print[0].Equals(Section2Print[Selection2].ToString()) && Section3Print[0].Equals(Section3Print[Selection3].ToString()) && Section4Print[0].Equals(Section4Print[Selection4].ToString()) && Section5Print[0].Equals(Section5Print[Selection5].ToString()) && Section6Print[0].Equals(Section6Print[Selection6].ToString()) && Section7Print[0].Equals(Section7Print[Selection7].ToString()) && Section8Print[0].Equals(Section8Print[Selection8].ToString()) && !Checking)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Processing", uParam0f_741, true);
						LoadingTimer = 0;
						CheckDelay = 0f;
						BarOn = 0;
						Checking = true;
					}
					if (Game.IsControlJustPressed(Control.SelectWeapon) && !Checking)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Processing", uParam0f_741, true);
						LoadingTimer = 0;
						CheckDelay = 0f;
						BarOn = 0;
						Checking = true;
					}
					if (Checking)
					{
						if (CheckDelay >= 0f && CheckDelay < 20f)
						{
							CheckDelay += 1f;
							drawSprite3("mpfclone_retro_loading", "fingerprint_hacking_minigame_loading_window_blank", 0.5f, 0.5f, 350f, 250f, 255, 255, 255, 255);
						}
						if (CheckDelay >= 20f && CheckDelay < 50f)
						{
							CheckDelay += 1f;
							drawSprite3("mpfclone_retro_loading", "fingerprint_hacking_minigame_loading_window", 0.5f, 0.5f, 350f, 250f, 255, 255, 255, 255);
						}
						if (CheckDelay >= 50f && CheckDelay < 2000f)
						{
							drawSprite3("mpfclone_retro_loading", "fingerprint_hacking_minigame_loading_window", 0.5f, 0.5f, 350f, 250f, 255, 255, 255, 255);
							Game.IsControlJustPressed(Control.Attack);
							if (BarOn < 21)
							{
								if (LoadingTimer != 10)
								{
									LoadingTimer++;
								}
								if (LoadingTimer == 10)
								{
									BarOn++;
									LoadingTimer = 0;
								}
							}
							if (LoadingBarPoints.Count > 0)
							{
								for (int num68 = 0; num68 < LoadingBarPoints.Count; num68++)
								{
									if (num68 < BarOn)
									{
										Vector2 vector18 = LoadingBarPoints[num68];
										drawSprite3("mpfclone_retro_loading", "fingerprint_hacking_minigame_loading_bar_segment", vector18.X, vector18.Y, 40f, 70f, 255, 255, 255, 255);
									}
								}
								if (BarOn >= LoadingBarPoints.Count - 2)
								{
									if (LoadingTimer < 20)
									{
										if (Section1Print[0].Equals(Section1Print[Selection1].ToString()) && Section2Print[0].Equals(Section2Print[Selection2].ToString()) && Section3Print[0].Equals(Section3Print[Selection3].ToString()) && Section4Print[0].Equals(Section4Print[Selection4].ToString()) && Section5Print[0].Equals(Section5Print[Selection5].ToString()) && Section6Print[0].Equals(Section6Print[Selection6].ToString()) && Section7Print[0].Equals(Section7Print[Selection7].ToString()) && Section8Print[0].Equals(Section8Print[Selection8].ToString()))
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Target_Match", uParam0f_741, true);
											drawSprite3("mpfclone_retro_correct", "fingerprint_hacking_minigame_messages_correct_01", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										else
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Print_Not_Match", uParam0f_741, true);
											drawSprite3("mpfclone_retro_messages", "fingerprint_hacking_minigame_messages_incorrect_01", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										LoadingTimer++;
									}
									if (LoadingTimer >= 20 && LoadingTimer < 40)
									{
										LoadingTimer++;
										if (Section1Print[0].Equals(Section1Print[Selection1].ToString()) && Section2Print[0].Equals(Section2Print[Selection2].ToString()) && Section3Print[0].Equals(Section3Print[Selection3].ToString()) && Section4Print[0].Equals(Section4Print[Selection4].ToString()) && Section5Print[0].Equals(Section5Print[Selection5].ToString()) && Section6Print[0].Equals(Section6Print[Selection6].ToString()) && Section7Print[0].Equals(Section7Print[Selection7].ToString()) && Section8Print[0].Equals(Section8Print[Selection8].ToString()))
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Target_Match", uParam0f_741, true);
											drawSprite3("mpfclone_retro_correct", "fingerprint_hacking_minigame_messages_correct_00", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
										else
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Print_Not_Match", uParam0f_741, true);
											drawSprite3("mpfclone_retro_messages", "fingerprint_hacking_minigame_messages_incorrect_00", 0.5f, 0.5f, 450f, 250f, 255, 255, 255, 255);
										}
									}
									if (LoadingTimer >= 40)
									{
										BarOn++;
										LoadingTimer = 0;
									}
									if (BarOn == 25)
									{
										if (Section1Print[0].Equals(Section1Print[Selection1].ToString()) && Section2Print[0].Equals(Section2Print[Selection2].ToString()) && Section3Print[0].Equals(Section3Print[Selection3].ToString()) && Section4Print[0].Equals(Section4Print[Selection4].ToString()) && Section5Print[0].Equals(Section5Print[Selection5].ToString()) && Section6Print[0].Equals(Section6Print[Selection6].ToString()) && Section7Print[0].Equals(Section7Print[Selection7].ToString()) && Section8Print[0].Equals(Section8Print[Selection8].ToString()))
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Target_Match", uParam0f_741, true);
											HacksCompleted++;
											if (HacksCompleted != HacksToComplete)
											{
												Random random5 = new Random();
												Print = random5.Next(0, 800);
												if (Print > 0 && Print <= 100)
												{
													PrintNum = 1;
													PrintDict = "mpfclone_retro_printfull1";
												}
												if (Print > 100 && Print <= 200)
												{
													PrintNum = 2;
													PrintDict = "mpfclone_retro_printfull2";
												}
												if (Print > 200 && Print <= 300)
												{
													PrintNum = 3;
													PrintDict = "mpfclone_retro_printfull3";
												}
												if (Print > 300 && Print <= 400)
												{
													PrintNum = 4;
													PrintDict = "mpfclone_retro_printfull4";
												}
												if (Print > 400 && Print <= 500)
												{
													PrintNum = 5;
													PrintDict = "mpfclone_retro_printfull5";
												}
												if (Print > 500 && Print <= 600)
												{
													PrintNum = 6;
													PrintDict = "mpfclone_retro_printfull6";
												}
												if (Print > 600 && Print <= 700)
												{
													PrintNum = 7;
													PrintDict = "mpfclone_retro_printfull7";
												}
												if (Print > 700 && Print <= 800)
												{
													PrintNum = 8;
													PrintDict = "mpfclone_retro_printfull8";
												}
												if (Section1Print.Count > 0)
												{
													Section1Print.Clear();
												}
												if (Section2Print.Count > 0)
												{
													Section2Print.Clear();
												}
												if (Section3Print.Count > 0)
												{
													Section3Print.Clear();
												}
												if (Section4Print.Count > 0)
												{
													Section4Print.Clear();
												}
												if (Section5Print.Count > 0)
												{
													Section5Print.Clear();
												}
												if (Section6Print.Count > 0)
												{
													Section6Print.Clear();
												}
												if (Section7Print.Count > 0)
												{
													Section7Print.Clear();
												}
												if (Section8Print.Count > 0)
												{
													Section8Print.Clear();
												}
												Section1Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_01");
												Section2Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_02");
												Section3Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_03");
												Section4Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_04");
												Section5Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_05");
												Section6Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_06");
												Section7Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_07");
												Section8Print.Add("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_08");
												for (int num69 = 0; num69 < 200; num69++)
												{
													int index = random5.Next(0, Prints.Count);
													if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_01") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_01") && !Section1Print.Contains(Prints[index]) && Section1Print.Count < 15)
													{
														Section1Print.Add(Prints[index]);
														index = random5.Next(0, Prints.Count);
													}
													index = random5.Next(0, Prints.Count);
													if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_02") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_02") && !Section2Print.Contains(Prints[index]) && Section2Print.Count < 15)
													{
														Section2Print.Add(Prints[index]);
														index = random5.Next(0, Prints.Count);
													}
													index = random5.Next(0, Prints.Count);
													if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_03") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_03") && !Section3Print.Contains(Prints[index]) && Section3Print.Count < 15)
													{
														Section3Print.Add(Prints[index]);
														index = random5.Next(0, Prints.Count);
													}
													index = random5.Next(0, Prints.Count);
													if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_04") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_04") && !Section4Print.Contains(Prints[index]) && Section4Print.Count < 15)
													{
														Section4Print.Add(Prints[index]);
														index = random5.Next(0, Prints.Count);
													}
													index = random5.Next(0, Prints.Count);
													if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_05") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_05") && !Section5Print.Contains(Prints[index]) && Section5Print.Count < 15)
													{
														Section5Print.Add(Prints[index]);
														index = random5.Next(0, Prints.Count);
													}
													index = random5.Next(0, Prints.Count);
													if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_06") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_06") && !Section6Print.Contains(Prints[index]) && Section6Print.Count < 15)
													{
														Section6Print.Add(Prints[index]);
														index = random5.Next(0, Prints.Count);
													}
													index = random5.Next(0, Prints.Count);
													if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_07") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_07") && !Section7Print.Contains(Prints[index]) && Section7Print.Count < 15)
													{
														Section7Print.Add(Prints[index]);
														index = random5.Next(0, Prints.Count);
													}
													index = random5.Next(0, Prints.Count);
													if (!Prints[index].Equals("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_08") && !Prints[index].Contains("fingerprint_hacking_minigame_fingerprints_0" + PrintNum + "_slice_08") && !Section8Print.Contains(Prints[index]) && Section8Print.Count < 15)
													{
														Section8Print.Add(Prints[index]);
														index = random5.Next(0, Prints.Count);
													}
												}
												Selection1 = random5.Next(0, Section1Print.Count);
												Selection2 = random5.Next(0, Section2Print.Count);
												Selection3 = random5.Next(0, Section3Print.Count);
												Selection4 = random5.Next(0, Section4Print.Count);
												Selection5 = random5.Next(0, Section5Print.Count);
												Selection6 = random5.Next(0, Section6Print.Count);
												Selection7 = random5.Next(0, Section7Print.Count);
												Selection8 = random5.Next(0, Section8Print.Count);
												BarOn = 0;
												Checking = false;
												CheckDelay = 0f;
												VideoPlaying = false;
												HackStage = 1;
												Game.Player.Character.IsPositionFrozen = true;
												CopyPrintHackOn = true;
											}
											if (HacksCompleted == HacksToComplete)
											{
												Checking = false;
												VideoPlaying = false;
												BINKMovie = -1;
												BINKMovie = Function.Call<int>(Hash.SET_BINK_MOVIE, "fingerprint_success");
												Function.Call(Hash.PLAY_BINK_MOVIE, BINKMovie);
												VideoPlaying = true;
												HackStage = 3;
												LoadingTimer = 0;
												CheckDelay = 0f;
												BarOn = 0;
												Checking = false;
												HackFinishedID = 1;
											}
										}
										else
										{
											Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Print_Not_Match", uParam0f_741, true);
											PHTimeLeft -= 30f;
											BINKMovie = -1;
											VideoPlaying = false;
											LoadingTimer = 0;
											CheckDelay = 0f;
											BarOn = 0;
											Checking = false;
										}
									}
								}
							}
						}
					}
				}
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				DisplayHelpTextThisFrame("Match up each component to the full fingerprint, WASD to move between each component, ~INPUT_FRONTEND_ACCEPT~ Select Print, ~INPUT_SELECT_WEAPON~ to check components, " + ReturnButtonString(ExitKey) + " to Exit", 1);
			}
			if (PlayingVoltageMG)
			{
				Scaleform scaleform6 = new Scaleform("instructional_buttons");
				scaleform6.CallFunction("CLEAR_ALL");
				scaleform6.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform6.CallFunction("CREATE_CONTAINER");
				scaleform6.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Navigation Up");
				scaleform6.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Navigation Down");
				scaleform6.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Navigation Next");
				scaleform6.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Navigation Back");
				scaleform6.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Choose Node");
				scaleform6.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
				scaleform6.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform6.Render2D();
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				GUIOn = true;
				DisplayHelpTextThisFrame("To bypass the system, link the numbers on the left to the modifiers on the right. You must link them in a specific way to achieve the target sum.\nPress " + ReturnButtonString(ExitKey) + " to Exit", 1);
				if (Game.IsControlJustPressed((Control)ExitKey))
				{
					HackFinishedID = 3;
					Game.Player.CanControlCharacter = true;
					VideoPlaying = false;
					BINKMovie = -1;
					BINKMovie = -1;
					PlayingVoltageMG = false;
				}
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				CopyPrintHackOn = false;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				if (VideoPlaying)
				{
					Function.Call(Hash.DRAW_BINK_MOVIE, BINKMovie, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
				}
				if (HackStage == 0 && (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, BINKMovie) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake)))
				{
					NumberAtCheck = 0f;
					RoundedNumber = 0f;
					new Random();
					ScrableNumber();
					Function.Call(Hash.STOP_BINK_MOVIE, BINKMovie);
					Function.Call(Hash.RELEASE_BINK_MOVIE, BINKMovie);
					BINKMovie = -1;
					VideoPlaying = false;
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpisland_voltage_bg"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpisland_voltage_bg", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpisland_voltage"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpisland_voltage", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "trafficcam"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "trafficcam", 0);
					}
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mphackinggame"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphackinggame", 0);
					}
					SelectedLN1 = false;
					SelectedRN1 = false;
					SelectedLN2 = false;
					SelectedRN2 = false;
					SelectedLN3 = false;
					SelectedRN3 = false;
					VTL1nodeSelected = 0;
					VTR1nodeSelected = 0;
					VTL2nodeSelected = 0;
					VTR2nodeSelected = 0;
					VTL3nodeSelected = 0;
					VTR3nodeSelected = 0;
					VTL1node = 0;
					VTR1node = 0;
					VTL2node = 0;
					VTR2node = 0;
					VTL3node = 0;
					VTR3node = 0;
					Delay = 0;
					AwaitingDelay = false;
					RightNodeOn = 0;
					LeftNodeOn = 1;
					HackStage = 1;
					NodeOn = 0;
					NumberAtCheck = 0f;
					ShownTargetNumber = VolthackTargetNumber;
					Function.Call(Hash.RELEASE_NAMED_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLC_HEI4_V_MG");
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_HEI4/DLC_HEI4_V_MG", false, -1);
				}
				if (HackStage == 1)
				{
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 0);
					Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
					ShowInstructionalButtonsVT();
					if (VTR1nodeSelected == 1)
					{
						int vTL1node = VTL1node;
						int vTR1node = VTR1node;
					}
					if (VTR1nodeSelected == 2)
					{
						int vTL1node2 = VTL1node;
						int vTR2node = VTR2node;
					}
					if (VTR1nodeSelected == 3)
					{
						int vTL1node3 = VTL1node;
						int vTR3node = VTR3node;
					}
					if (VTR2nodeSelected == 1)
					{
						int vTL2node = VTL2node;
						int vTR1node2 = VTR1node;
					}
					if (VTR2nodeSelected == 2)
					{
						int vTL2node2 = VTL2node;
						int vTR2node2 = VTR2node;
					}
					if (VTR2nodeSelected == 3)
					{
						int vTL2node3 = VTL2node;
						int vTR3node2 = VTR3node;
					}
					if (VTR3nodeSelected == 1)
					{
						int vTL3node = VTL3node;
						int vTR1node3 = VTR1node;
					}
					if (VTR3nodeSelected == 2)
					{
						int vTL3node2 = VTL3node;
						int vTR2node3 = VTR2node;
					}
					if (VTR3nodeSelected == 3)
					{
						int vTL3node3 = VTL3node;
						int vTR3node3 = VTR3node;
					}
					if (scaleform6 == null)
					{
						scaleform6 = new Scaleform("instructional_buttons");
					}
					scaleform6.Render2D();
					int volthackTargetNumber = VolthackTargetNumber;
					int num70 = VTL1node * VTR1node + VTL2node * VTR2node + VTL3node * VTR3node;
					drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5f, 0.5f, 8000f, 8000f, 0, 0, 0, 255);
					drawVoltageHackSpriteScaled("mpisland_voltage_bg", "phone_background", 0.5f, 0.5f, 2220f, 2320f, 255, 255, 255, 180);
					drawVoltageHackSpriteScaled("mpisland_voltage_bg", "voltage_pixel_grid", 0.5f, 0.5f, 2220f, 2320f, 255, 255, 255, 180);
					drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_bg", 0.5f, 0.5f, 1220f, 1920f, 255, 255, 255, 255);
					if (ShownTargetNumber < volthackTargetNumber)
					{
						ShownTargetNumber++;
					}
					if (ShownTargetNumber > volthackTargetNumber)
					{
						ShownTargetNumber--;
					}
					char[] array20 = ShownTargetNumber.ToString().ToCharArray();
					if (array20.Length == 1)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.3977f, 0.064f, 120f, 170f, 255, 36, 20, 255);
						drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.48933f, 0.064f, 120f, 170f, 255, 36, 20, 255);
						drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array20[0]), 0.5841f, 0.064f, 120f, 170f, 255, 36, 20, 255);
					}
					if (array20.Length == 2)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.3977f, 0.064f, 120f, 170f, 255, 36, 20, 255);
						drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array20[0]), 0.48933f, 0.064f, 120f, 170f, 255, 36, 20, 255);
						drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array20[1]), 0.5841f, 0.064f, 120f, 170f, 255, 36, 20, 255);
					}
					if (array20.Length == 3)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array20[0]), 0.3977f, 0.064f, 120f, 170f, 255, 36, 20, 255);
						drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array20[1]), 0.48933f, 0.064f, 120f, 170f, 255, 36, 20, 255);
						drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array20[2]), 0.5841f, 0.064f, 120f, 170f, 255, 36, 20, 255);
					}
					if (NumberAtCheck < (float)num70 && num70 >= 0)
					{
						NumberAtCheck += 1f;
					}
					if (NumberAtCheck > (float)num70 && num70 >= 0)
					{
						NumberAtCheck -= 1f;
					}
					char[] array21 = ((int)NumberAtCheck).ToString().ToCharArray();
					if (NumberAtCheck == (float)volthackTargetNumber)
					{
						if (array21.Length == 0)
						{
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.3977f, 0.934f, 100f, 170f, 88, 209, 102, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.48933f, 0.934f, 100f, 170f, 88, 209, 102, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.5841f, 0.934f, 100f, 170f, 88, 209, 102, 255);
						}
						if (array21.Length == 1)
						{
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.3977f, 0.934f, 100f, 170f, 88, 209, 102, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.48933f, 0.934f, 100f, 170f, 88, 209, 102, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[0]), 0.5841f, 0.934f, 100f, 170f, 88, 209, 102, 255);
						}
						if (array21.Length == 2)
						{
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.3977f, 0.934f, 100f, 170f, 88, 209, 102, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[0]), 0.48933f, 0.934f, 100f, 170f, 88, 209, 102, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[1]), 0.5841f, 0.934f, 100f, 170f, 88, 209, 102, 255);
						}
						if (array21.Length == 3)
						{
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[0]), 0.3977f, 0.934f, 100f, 170f, 88, 209, 102, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[1]), 0.48933f, 0.934f, 100f, 170f, 88, 209, 102, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[2]), 0.5841f, 0.934f, 100f, 170f, 88, 209, 102, 255);
						}
					}
					else
					{
						Color color = Color.FromArgb(255, 130, 130, 130);
						if (NumberAtCheck == (float)volthackTargetNumber)
						{
							color = Color.FromArgb(255, 88, 209, 102);
						}
						if (NumberAtCheck > (float)volthackTargetNumber)
						{
							color = Color.FromArgb(255, 184, 0, 0);
						}
						if (array21.Length == 0)
						{
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.3977f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.48933f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.5841f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
						}
						if (array21.Length == 1)
						{
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.3977f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.48933f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[0]), 0.5841f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
						}
						if (array21.Length == 2)
						{
							drawVoltageHackSpriteScaled("mpisland_voltage", "0", 0.3977f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[0]), 0.48933f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[1]), 0.5841f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
						}
						if (array21.Length == 3)
						{
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[0]), 0.3977f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[1]), 0.48933f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
							drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(array21[2]), 0.5841f, 0.934f, 100f, 170f, color.R, color.G, color.B, 255);
						}
					}
					drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(LNo1), 0.0232f, 0.2344f, 100f, 170f, 255, 255, 255, 255);
					drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(LNo2), 0.0232f, 0.502f, 100f, 170f, 255, 255, 255, 255);
					drawVoltageHackSpriteScaled("mpisland_voltage", ReturnNFC_Retro(LNo3), 0.0232f, 0.768f, 100f, 170f, 255, 255, 255, 255);
					drawVoltageHackSpriteScaled("mpisland_voltage", GetSpriteFromMultiplicationRatio(RNo1), 0.9476f, 0.2388f, 100f, 170f, 255, 255, 255, 255);
					drawVoltageHackSpriteScaled("mpisland_voltage", GetSpriteFromMultiplicationRatio(RNo2), 0.9476f, 0.50277f, 100f, 170f, 255, 255, 255, 255);
					drawVoltageHackSpriteScaled("mpisland_voltage", GetSpriteFromMultiplicationRatio(RNo3), 0.9476f, 123f / 160f, 100f, 170f, 255, 255, 255, 255);
					drawVoltageHackSpriteScaled("mpisland_voltage_bg", "result_text", 0.497f, 1f, 100f, 30f, 88, 209, 102, 180, 0f, VoltageMG_Scale, 0f, 0.0125f);
					drawVoltageHackSpriteScaled("mpisland_voltage_bg", "target_text", 0.4927f, 0f, 100f, 30f, 145, 0, 0, 180, 0f, VoltageMG_Scale, 0f, -0.0125f);
					if (LeftNodeOn == 1)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "number_node", 0.04418f, 0.23402f, 150f, 260f, 252, 140, 3, 255);
					}
					if (LeftNodeOn == 2)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "number_node", 0.04418f, 0.4986f, 150f, 260f, 145, 0, 0, 255);
					}
					if (LeftNodeOn == 3)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "number_node", 0.04418f, 0.76458f, 150f, 260f, 0, 0, 255, 255);
					}
					if (LeftNodeOn == 1 && RightNodeOn == 1)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.2333f, 190f, 330f, 252, 140, 3, 255);
					}
					if (LeftNodeOn == 2 && RightNodeOn == 2)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.4986f, 190f, 330f, 145, 0, 0, 255);
					}
					if (LeftNodeOn == 3 && RightNodeOn == 3)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.76527f, 190f, 330f, 0, 0, 255, 255);
					}
					if (LeftNodeOn == 1 && RightNodeOn == 2)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.4986f, 190f, 330f, 252, 140, 3, 255);
					}
					if (LeftNodeOn == 1 && RightNodeOn == 3)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.76527f, 190f, 330f, 252, 140, 3, 255);
					}
					if (LeftNodeOn == 2 && RightNodeOn == 1)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.2333f, 190f, 330f, 145, 0, 0, 255);
					}
					if (LeftNodeOn == 2 && RightNodeOn == 3)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.76527f, 190f, 330f, 145, 0, 0, 255);
					}
					if (LeftNodeOn == 3 && RightNodeOn == 1)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.2333f, 190f, 330f, 0, 0, 255, 255);
					}
					if (LeftNodeOn == 3 && RightNodeOn == 2)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "circle_node", 0.9313f, 0.4986f, 190f, 330f, 0, 0, 255, 255);
					}
					if (Game.GameTime >= ScrableNumberTimer)
					{
						DisableControlls = true;
						AwaitingDelay = false;
						RightNodeOn = 0;
						SelectedLN1 = false;
						SelectedRN1 = false;
						SelectedLN2 = false;
						SelectedRN2 = false;
						SelectedLN3 = false;
						SelectedRN3 = false;
						VTL1nodeSelected = 0;
						VTR1nodeSelected = 0;
						VTL2nodeSelected = 0;
						VTR2nodeSelected = 0;
						VTL3nodeSelected = 0;
						VTR3nodeSelected = 0;
						VTL1node = 0;
						VTR1node = 0;
						VTL2node = 0;
						VTR2node = 0;
						VTL3node = 0;
						VTR3node = 0;
						RightNodeOn = 0;
						LeftNodeOn = 1;
						ScrableNumberTimer = Game.GameTime;
						NodeOn = 0;
						NumberAtCheck = 0f;
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "All_Connected_Incorrect", uParam1f_741, true);
						ScrableCount++;
						if (ScrableCount < 5)
						{
							ScrableNumberTimer = Game.GameTime + 350;
							ScrableNumber();
						}
						else
						{
							DisableControlls = false;
							ScrableCount = 0;
							ScrableNumberTimer = Game.GameTime + (ScrableNumberTime + ScrableNumberTime / 6);
						}
					}
					int num71 = ScrableNumberTimer - Game.GameTime;
					if (num71 > ScrableNumberTime / 6)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.2425f, 0.9315f, 35f, 140f, 88, 209, 102, 255);
					}
					if (num71 > ScrableNumberTime / 6 * 2)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.217500016f, 0.9315f, 35f, 140f, 88, 209, 102, 255);
					}
					if (num71 > ScrableNumberTime / 6 * 3)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.19250001f, 0.9315f, 35f, 140f, 88, 209, 102, 255);
					}
					if (num71 > ScrableNumberTime / 6 * 4)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.167500019f, 0.9315f, 35f, 140f, 88, 209, 102, 255);
					}
					if (num71 > ScrableNumberTime / 6 * 5)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.142500013f, 0.9315f, 35f, 140f, 88, 209, 102, 255);
					}
					if (num71 > ScrableNumberTime / 6 * 6)
					{
						drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.117500007f, 0.9315f, 35f, 140f, 88, 209, 102, 255);
					}
					if (NodeOn == 0 && !DisableControlls)
					{
						if (Game.IsControlJustPressed(Control.FrontendRright))
						{
							if (SelectedLN3)
							{
								VTL3node = 0;
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
								SelectedLN3 = false;
								if (VTR3nodeSelected == 1)
								{
									SelectedRN1 = false;
								}
								if (VTR3nodeSelected == 2)
								{
									SelectedRN2 = false;
								}
								if (VTR3nodeSelected == 3)
								{
									SelectedRN3 = false;
								}
								VTL3nodeSelected = 0;
								VTR3nodeSelected = 0;
								VTR3node = 0;
								LeftNodeOn = 3;
								return;
							}
							if (SelectedLN2)
							{
								VTL2node = 0;
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
								SelectedLN2 = false;
								if (VTR2nodeSelected == 1)
								{
									SelectedRN1 = false;
								}
								if (VTR2nodeSelected == 2)
								{
									SelectedRN2 = false;
								}
								if (VTR2nodeSelected == 3)
								{
									SelectedRN3 = false;
								}
								VTL2nodeSelected = 0;
								VTR2nodeSelected = 0;
								VTR2node = 0;
								LeftNodeOn = 2;
								return;
							}
							if (SelectedLN1)
							{
								VTL1node = 0;
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
								if (VTR1nodeSelected == 1)
								{
									SelectedRN1 = false;
								}
								if (VTR1nodeSelected == 2)
								{
									SelectedRN2 = false;
								}
								if (VTR1nodeSelected == 3)
								{
									SelectedRN3 = false;
								}
								VTL1nodeSelected = 0;
								SelectedLN1 = false;
								VTR1nodeSelected = 0;
								VTR1node = 0;
								LeftNodeOn = 1;
								return;
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneSelect))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Cursor_Move", uParam1f_741, true);
							if (LeftNodeOn == 1)
							{
								if (SelectedLN1)
								{
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
									if (VTR1nodeSelected == 1)
									{
										SelectedRN1 = false;
									}
									if (VTR1nodeSelected == 2)
									{
										SelectedRN2 = false;
									}
									if (VTR1nodeSelected == 3)
									{
										SelectedRN3 = false;
									}
									VTR1nodeSelected = 0;
									VTR1node = 0;
									VTL1nodeSelected = 0;
									VTL1node = 0;
									LeftNodeOn = 1;
									SelectedLN1 = false;
								}
								if (!SelectedLN1)
								{
									LastLeftNodeSelected = 1;
									if (LeftNodeOn == 1)
									{
										VTL1nodeSelected = 1;
										VTL1node = LNo1;
									}
									NodeOn = 1;
									RightNodeOn = 1;
									return;
								}
							}
							if (LeftNodeOn == 2)
							{
								if (SelectedLN2)
								{
									if (VTR2nodeSelected == 1)
									{
										SelectedRN1 = false;
									}
									if (VTR2nodeSelected == 2)
									{
										SelectedRN2 = false;
									}
									if (VTR2nodeSelected == 3)
									{
										SelectedRN3 = false;
									}
									VTR2nodeSelected = 0;
									VTR2node = 0;
									VTL2nodeSelected = 0;
									VTL2node = 0;
									LeftNodeOn = 2;
									SelectedLN2 = false;
								}
								if (!SelectedLN2)
								{
									LastLeftNodeSelected = 2;
									if (LeftNodeOn == 2)
									{
										VTL2nodeSelected = 2;
										VTL2node = LNo2;
									}
									NodeOn = 1;
									RightNodeOn = 1;
									return;
								}
							}
							if (LeftNodeOn == 3)
							{
								if (SelectedLN3)
								{
									if (VTR3nodeSelected == 1)
									{
										SelectedRN1 = false;
									}
									if (VTR3nodeSelected == 2)
									{
										SelectedRN2 = false;
									}
									if (VTR3nodeSelected == 3)
									{
										SelectedRN3 = false;
									}
									VTR3nodeSelected = 0;
									VTR3node = 0;
									VTL3nodeSelected = 0;
									VTL3node = 0;
									LeftNodeOn = 3;
									SelectedLN3 = false;
								}
								if (!SelectedLN3)
								{
									LastLeftNodeSelected = 3;
									if (LeftNodeOn == 3)
									{
										VTL3nodeSelected = 3;
										VTL3node = LNo3;
									}
									NodeOn = 1;
									RightNodeOn = 1;
									return;
								}
							}
							LastLeftNodeSelected = 1;
							if (SelectedRN1)
							{
								LastLeftNodeSelected = 2;
							}
							if (SelectedRN1 && SelectedRN2)
							{
								LastLeftNodeSelected = 3;
							}
							if (LastLeftNodeSelected == 1)
							{
								VTR1nodeSelected = RightNodeOn;
								if (VTR1nodeSelected == 1)
								{
									VTR1node = RNo1;
								}
								if (VTR1nodeSelected == 2)
								{
									VTR1node = RNo2;
								}
								if (VTR1nodeSelected == 3)
								{
									VTR1node = RNo3;
								}
							}
							if (LastLeftNodeSelected == 2)
							{
								VTR2nodeSelected = RightNodeOn;
								if (VTR2nodeSelected == 1)
								{
									VTR1node = RNo1;
								}
								if (VTR2nodeSelected == 2)
								{
									VTR2node = RNo2;
								}
								if (VTR2nodeSelected == 3)
								{
									VTR2node = RNo3;
								}
							}
							if (LastLeftNodeSelected == 3)
							{
								VTR3nodeSelected = RightNodeOn;
								if (VTR3nodeSelected == 1)
								{
									VTR3node = RNo1;
								}
								if (VTR3nodeSelected == 2)
								{
									VTR3node = RNo2;
								}
								if (VTR3nodeSelected == 3)
								{
									VTR3node = RNo3;
								}
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneDown))
						{
							LeftNodeOn++;
							if (LeftNodeOn == 4)
							{
								LeftNodeOn = 1;
							}
							if (LeftNodeOn == 0)
							{
								LeftNodeOn = 3;
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneUp))
						{
							LeftNodeOn--;
							if (LeftNodeOn == 4)
							{
								LeftNodeOn = 1;
							}
							if (LeftNodeOn == 0)
							{
								LeftNodeOn = 3;
							}
						}
					}
					if (NodeOn == 1 && !DisableControlls)
					{
						if (LastLeftNodeSelected == 1)
						{
							VTR1nodeSelected = RightNodeOn;
							if (VTR1nodeSelected == 1)
							{
								VTR1node = RNo1;
							}
							if (VTR1nodeSelected == 2)
							{
								VTR1node = RNo2;
							}
							if (VTR1nodeSelected == 3)
							{
								VTR1node = RNo3;
							}
						}
						if (LastLeftNodeSelected == 2)
						{
							VTR2nodeSelected = RightNodeOn;
							if (VTR2nodeSelected == 1)
							{
								VTR1node = RNo1;
							}
							if (VTR2nodeSelected == 2)
							{
								VTR2node = RNo2;
							}
							if (VTR2nodeSelected == 3)
							{
								VTR2node = RNo3;
							}
						}
						if (LastLeftNodeSelected == 3)
						{
							VTR3nodeSelected = RightNodeOn;
							if (VTR3nodeSelected == 1)
							{
								VTR3node = RNo1;
							}
							if (VTR3nodeSelected == 2)
							{
								VTR3node = RNo2;
							}
							if (VTR3nodeSelected == 3)
							{
								VTR3node = RNo3;
							}
						}
						if (Game.IsControlJustPressed(Control.FrontendRright))
						{
							if (LastLeftNodeSelected == 1 && !SelectedLN1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
								VTR1nodeSelected = 0;
								VTR1node = 0;
								VTL1nodeSelected = 0;
								VTL1node = 0;
								LeftNodeOn = 1;
							}
							if (LastLeftNodeSelected == 2 && !SelectedLN2)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
								VTR2nodeSelected = 0;
								VTR2node = 0;
								VTL2nodeSelected = 0;
								VTL2node = 0;
								LeftNodeOn = 2;
							}
							if (LastLeftNodeSelected == 3 && !SelectedLN3)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
								VTR3nodeSelected = 0;
								VTR3node = 0;
								VTL3nodeSelected = 0;
								VTL3node = 0;
								LeftNodeOn = 3;
							}
							NodeOn = 0;
							RightNodeOn = 0;
						}
						if (Game.IsControlJustPressed(Control.PhoneSelect))
						{
							if (RightNodeOn == 1)
							{
								if (!SelectedRN1)
								{
									SelectedRN1 = true;
									if (LastLeftNodeSelected == 1)
									{
										SelectedLN1 = true;
									}
									if (LastLeftNodeSelected == 2)
									{
										SelectedLN2 = true;
									}
									if (LastLeftNodeSelected == 3)
									{
										SelectedLN3 = true;
									}
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
									if (LastLeftNodeSelected == 1)
									{
										VTR1nodeSelected = RightNodeOn;
										VTR1node = RNo1;
									}
									if (LastLeftNodeSelected == 2)
									{
										VTR2nodeSelected = RightNodeOn;
										VTR2node = RNo1;
									}
									if (LastLeftNodeSelected == 3)
									{
										VTR3nodeSelected = RightNodeOn;
										VTR3node = RNo1;
									}
									NodeOn = 0;
									RightNodeOn = 0;
									if (!SelectedRN2)
									{
										LeftNodeOn = 2;
									}
									else
									{
										LeftNodeOn = 3;
									}
									return;
								}
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "All_Connected_Incorrect", uParam1f_741, true);
							}
							else if (RightNodeOn == 2)
							{
								if (!SelectedRN2)
								{
									SelectedRN2 = true;
									if (LastLeftNodeSelected == 1)
									{
										SelectedLN1 = true;
									}
									if (LastLeftNodeSelected == 2)
									{
										SelectedLN2 = true;
									}
									if (LastLeftNodeSelected == 3)
									{
										SelectedLN3 = true;
									}
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
									if (LastLeftNodeSelected == 1)
									{
										VTR1nodeSelected = RightNodeOn;
										VTR1node = RNo2;
									}
									if (LastLeftNodeSelected == 2)
									{
										VTR2nodeSelected = RightNodeOn;
										VTR2node = RNo2;
									}
									if (LastLeftNodeSelected == 3)
									{
										VTR3nodeSelected = RightNodeOn;
										VTR3node = RNo2;
									}
									NodeOn = 0;
									RightNodeOn = 0;
									if (!SelectedRN1)
									{
										LeftNodeOn = 1;
									}
									else
									{
										LeftNodeOn = 3;
									}
									return;
								}
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "All_Connected_Incorrect", uParam1f_741, true);
							}
							else if (RightNodeOn == 3)
							{
								if (!SelectedRN3)
								{
									SelectedRN3 = true;
									if (LastLeftNodeSelected == 1)
									{
										SelectedLN1 = true;
									}
									if (LastLeftNodeSelected == 2)
									{
										SelectedLN2 = true;
									}
									if (LastLeftNodeSelected == 3)
									{
										SelectedLN3 = true;
									}
									Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Disconnect_Wire", uParam1f_741, true);
									if (LastLeftNodeSelected == 1)
									{
										VTR1nodeSelected = RightNodeOn;
										VTR1node = RNo3;
									}
									if (LastLeftNodeSelected == 2)
									{
										VTR2nodeSelected = RightNodeOn;
										VTR2node = RNo3;
									}
									if (LastLeftNodeSelected == 3)
									{
										VTR3nodeSelected = RightNodeOn;
										VTR3node = RNo3;
									}
									NodeOn = 0;
									RightNodeOn = 0;
									if (!SelectedRN1)
									{
										LeftNodeOn = 1;
									}
									else
									{
										LeftNodeOn = 2;
									}
									return;
								}
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "All_Connected_Incorrect", uParam1f_741, true);
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneDown))
						{
							RightNodeOn++;
							if (RightNodeOn == 4)
							{
								RightNodeOn = 1;
							}
							if (RightNodeOn == 0)
							{
								RightNodeOn = 3;
							}
							if (SelectedRN1 && RightNodeOn == 1)
							{
								RightNodeOn = 2;
							}
							if (SelectedRN2 && RightNodeOn == 2)
							{
								RightNodeOn = 3;
							}
							if (SelectedRN3 && RightNodeOn == 3)
							{
								RightNodeOn = 1;
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneUp))
						{
							RightNodeOn--;
							if (RightNodeOn == 4)
							{
								RightNodeOn = 1;
							}
							if (RightNodeOn == 0)
							{
								RightNodeOn = 3;
							}
							if (SelectedRN1 && RightNodeOn == 1)
							{
								RightNodeOn = 3;
							}
							if (SelectedRN2 && RightNodeOn == 2)
							{
								RightNodeOn = 1;
							}
							if (SelectedRN3 && RightNodeOn == 3)
							{
								RightNodeOn = 2;
							}
							if (LastLeftNodeSelected == 1)
							{
								VTR1nodeSelected = RightNodeOn;
								if (VTR1nodeSelected == 1)
								{
									VTR1node = RNo1;
								}
								if (VTR1nodeSelected == 2)
								{
									VTR1node = RNo2;
								}
								if (VTR1nodeSelected == 3)
								{
									VTR1node = RNo3;
								}
							}
							if (LastLeftNodeSelected == 2)
							{
								VTR2nodeSelected = RightNodeOn;
								if (VTR2nodeSelected == 1)
								{
									VTR1node = RNo1;
								}
								if (VTR2nodeSelected == 2)
								{
									VTR2node = RNo2;
								}
								if (VTR2nodeSelected == 3)
								{
									VTR2node = RNo3;
								}
							}
							if (LastLeftNodeSelected == 3)
							{
								VTR3nodeSelected = RightNodeOn;
								if (VTR3nodeSelected == 1)
								{
									VTR3node = RNo1;
								}
								if (VTR3nodeSelected == 2)
								{
									VTR3node = RNo2;
								}
								if (VTR3nodeSelected == 3)
								{
									VTR3node = RNo3;
								}
							}
						}
					}
					if (true)
					{
						if (true)
						{
							if ((LeftNodeOn == 1 && RightNodeOn == 1) || (VTL1nodeSelected == 1 && VTR1nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.2368f, 550f, 10f, 252, 140, 3, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 1 && RightNodeOn == 2) || (VTL1nodeSelected == 1 && VTR1nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2895f, 0.2375f, 270f, 10f, 252, 140, 3, 255, 90f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4802f, 0.3715f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6758f, 0.4972f, 270f, 10f, 252, 140, 3, 255, 90f, 150f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4779f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4787f, 0.4972f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 1 && RightNodeOn == 3) || (VTL1nodeSelected == 1 && VTR1nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.1985f, 0.2361f, 140f, 10f, 252, 140, 3, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2889f, 0.3729f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3851f, 0.4993f, 140f, 10f, 252, 140, 3, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4802f, 0.6354f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6726f, 0.7625f, 270f, 10f, 252, 140, 3, 255, 90f, 120f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.2889f, 0.2381f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.2875f, 0.5f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.479f, 0.4993f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4802f, 0.7611f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8566f, 0.7731f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 1) || (VTL1nodeSelected == 2 && VTR1nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7293f, 0.2368f, 170f, 10f, 145, 0, 0, 255, 90f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.259f, 0.4993f, 230f, 10f, 145, 0, 0, 255, 90f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4156f, 0.4361f, 10f, 180f, 145, 0, 0, 255, 0f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5171f, 0.368f, 130f, 10f, 145, 0, 0, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6072f, 0.3027f, 10f, 180f, 145, 0, 0, 255, 0f, 120f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4139f, 0.5f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4156f, 59f / 160f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.6046f, 0.368f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.6075f, 0.23402f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 2) || (VTL1nodeSelected == 2 && VTR1nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.4993f, 550f, 10f, 145, 0, 0, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 3) || (VTL1nodeSelected == 2 && VTR1nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2293f, 0.5f, 180f, 10f, 145, 0, 0, 255, 90f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3529f, 0.6284f, 10f, 330f, 145, 0, 0, 255, 0f, 135f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6061f, 0.7611f, 350f, 10f, 145, 0, 0, 255, 90f, 165f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.7631f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.3526f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.3529f, 0.7597f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 1) || (VTL1nodeSelected == 3 && VTR1nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3287f, 0.7611f, 310f, 10f, 0, 0, 255, 255, 90f, 140f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5418f, 0.6972f, 10f, 150f, 0, 0, 255, 255, 0f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.636f, 0.6305f, 140f, 10f, 0, 0, 255, 255, 90f, 85f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7331f, 0.4354f, 10f, 490f, 0, 0, 255, 255, 0f, 190f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7998f, 0.2368f, 100f, 10f, 0, 0, 255, 255, 90f, 80f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5418f, 0.7604f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5427f, 101f / 160f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7328f, 101f / 160f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7319f, 0.2381f, 45f, 50f, 0, 0, 255, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 2) || (VTL1nodeSelected == 3 && VTR1nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3287f, 0.7611f, 310f, 10f, 0, 0, 255, 255, 90f, 130f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5424f, 0.725f, 10f, 80f, 0, 0, 255, 255, 0f, 75f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6401f, 0.6965f, 140f, 10f, 0, 0, 255, 255, 90f, 85f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7328f, 0.5958f, 10f, 260f, 0, 0, 255, 255, 0f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.8011f, 0.4993f, 100f, 10f, 0, 0, 255, 255, 90f, 80f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5418f, 0.7604f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5409f, 0.6965f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7331f, 0.6944f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7319f, 0.5f, 45f, 50f, 0, 0, 255, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 3) || (VTL1nodeSelected == 3 && VTR1nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.7638f, 550f, 10f, 0, 0, 255, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
							}
						}
						if (true)
						{
							if ((LeftNodeOn == 1 && RightNodeOn == 1) || (VTL2nodeSelected == 1 && VTR2nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.2368f, 550f, 10f, 252, 140, 3, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 1 && RightNodeOn == 2) || (VTL2nodeSelected == 1 && VTR2nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2895f, 0.2375f, 270f, 10f, 252, 140, 3, 255, 90f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4802f, 0.3715f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6758f, 0.4972f, 270f, 10f, 252, 140, 3, 255, 90f, 150f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4779f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4787f, 0.4972f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 1 && RightNodeOn == 3) || (VTL2nodeSelected == 1 && VTR2nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.1985f, 0.2361f, 140f, 10f, 252, 140, 3, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2889f, 0.3729f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3851f, 0.4993f, 140f, 10f, 252, 140, 3, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4802f, 0.6354f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6726f, 0.7625f, 270f, 10f, 252, 140, 3, 255, 90f, 120f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.2889f, 0.2381f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.2875f, 0.5f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.479f, 0.4993f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4802f, 0.7611f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8566f, 0.7731f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 1) || (VTL2nodeSelected == 2 && VTR2nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7293f, 0.2368f, 170f, 10f, 145, 0, 0, 255, 90f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.259f, 0.4993f, 230f, 10f, 145, 0, 0, 255, 90f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4156f, 0.4361f, 10f, 180f, 145, 0, 0, 255, 0f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5171f, 0.368f, 130f, 10f, 145, 0, 0, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6072f, 0.3027f, 10f, 180f, 145, 0, 0, 255, 0f, 120f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4139f, 0.5f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4156f, 59f / 160f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.6046f, 0.368f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.6075f, 0.23402f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 2) || (VTL2nodeSelected == 2 && VTR2nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.4993f, 550f, 10f, 145, 0, 0, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 3) || (VTL2nodeSelected == 2 && VTR2nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2293f, 0.5f, 180f, 10f, 145, 0, 0, 255, 90f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3529f, 0.6284f, 10f, 330f, 145, 0, 0, 255, 0f, 135f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6061f, 0.7611f, 350f, 10f, 145, 0, 0, 255, 90f, 165f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.7631f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.3526f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.3529f, 0.7597f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 1) || (VTL2nodeSelected == 3 && VTR2nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3287f, 0.7611f, 310f, 10f, 0, 0, 255, 255, 90f, 140f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5418f, 0.6972f, 10f, 150f, 0, 0, 255, 255, 0f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.636f, 0.6305f, 140f, 10f, 0, 0, 255, 255, 90f, 85f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7331f, 0.4354f, 10f, 490f, 0, 0, 255, 255, 0f, 190f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7998f, 0.2368f, 100f, 10f, 0, 0, 255, 255, 90f, 80f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5418f, 0.7604f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5427f, 101f / 160f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7328f, 101f / 160f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7319f, 0.2381f, 45f, 50f, 0, 0, 255, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 2) || (VTL2nodeSelected == 3 && VTR2nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3287f, 0.7611f, 310f, 10f, 0, 0, 255, 255, 90f, 130f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5424f, 0.725f, 10f, 80f, 0, 0, 255, 255, 0f, 75f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6401f, 0.6965f, 140f, 10f, 0, 0, 255, 255, 90f, 85f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7328f, 0.5958f, 10f, 260f, 0, 0, 255, 255, 0f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.8011f, 0.4993f, 100f, 10f, 0, 0, 255, 255, 90f, 80f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5418f, 0.7604f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5409f, 0.6965f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7331f, 0.6944f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7319f, 0.5f, 45f, 50f, 0, 0, 255, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 3) || (VTL2nodeSelected == 3 && VTR2nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.7638f, 550f, 10f, 0, 0, 255, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
							}
						}
						if (true)
						{
							if ((LeftNodeOn == 1 && RightNodeOn == 1) || (VTL3nodeSelected == 1 && VTR3nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.2368f, 550f, 10f, 252, 140, 3, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 1 && RightNodeOn == 2) || (VTL3nodeSelected == 1 && VTR3nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2895f, 0.2375f, 270f, 10f, 252, 140, 3, 255, 90f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4802f, 0.3715f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6758f, 0.4972f, 270f, 10f, 252, 140, 3, 255, 90f, 150f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4779f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4787f, 0.4972f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 1 && RightNodeOn == 3) || (VTL3nodeSelected == 1 && VTR3nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.1985f, 0.2361f, 140f, 10f, 252, 140, 3, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2889f, 0.3729f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3851f, 0.4993f, 140f, 10f, 252, 140, 3, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4802f, 0.6354f, 10f, 330f, 252, 140, 3, 255, 0f, 150f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6726f, 0.7625f, 270f, 10f, 252, 140, 3, 255, 90f, 120f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.2361f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.2889f, 0.2381f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.2875f, 0.5f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.479f, 0.4993f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4802f, 0.7611f, 45f, 50f, 252, 140, 3, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8566f, 0.7731f, 45f, 50f, 252, 140, 3, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 1) || (VTL3nodeSelected == 2 && VTR3nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7293f, 0.2368f, 170f, 10f, 145, 0, 0, 255, 90f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.259f, 0.4993f, 230f, 10f, 145, 0, 0, 255, 90f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4156f, 0.4361f, 10f, 180f, 145, 0, 0, 255, 0f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5171f, 0.368f, 130f, 10f, 145, 0, 0, 255, 90f, 90f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6072f, 0.3027f, 10f, 180f, 145, 0, 0, 255, 0f, 120f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4139f, 0.5f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.4156f, 59f / 160f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.6046f, 0.368f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.6075f, 0.23402f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 2) || (VTL3nodeSelected == 2 && VTR3nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.4993f, 550f, 10f, 145, 0, 0, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 2 && RightNodeOn == 3) || (VTL3nodeSelected == 2 && VTR3nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.2293f, 0.5f, 180f, 10f, 145, 0, 0, 255, 90f, 110f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3529f, 0.6284f, 10f, 330f, 145, 0, 0, 255, 0f, 135f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6061f, 0.7611f, 350f, 10f, 145, 0, 0, 255, 90f, 165f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.7631f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.3526f, 0.4986f, 45f, 50f, 145, 0, 0, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.3529f, 0.7597f, 45f, 50f, 145, 0, 0, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 1) || (VTL3nodeSelected == 3 && VTR3nodeSelected == 1))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3287f, 0.7611f, 310f, 10f, 0, 0, 255, 255, 90f, 140f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5418f, 0.6972f, 10f, 150f, 0, 0, 255, 255, 0f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.636f, 0.6305f, 140f, 10f, 0, 0, 255, 255, 90f, 85f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7331f, 0.4354f, 10f, 490f, 0, 0, 255, 255, 0f, 190f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7998f, 0.2368f, 100f, 10f, 0, 0, 255, 255, 90f, 80f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.2361f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5418f, 0.7604f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5427f, 101f / 160f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7328f, 101f / 160f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7319f, 0.2381f, 45f, 50f, 0, 0, 255, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 2) || (VTL3nodeSelected == 3 && VTR3nodeSelected == 2))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.3287f, 0.7611f, 310f, 10f, 0, 0, 255, 255, 90f, 130f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.5424f, 0.725f, 10f, 80f, 0, 0, 255, 255, 0f, 75f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.6401f, 0.6965f, 140f, 10f, 0, 0, 255, 255, 90f, 85f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.7328f, 0.5958f, 10f, 260f, 0, 0, 255, 255, 0f, 120f);
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.8011f, 0.4993f, 100f, 10f, 0, 0, 255, 255, 90f, 80f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.4986f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5418f, 0.7604f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.5409f, 0.6965f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7331f, 0.6944f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.7319f, 0.5f, 45f, 50f, 0, 0, 255, 255);
							}
							if ((LeftNodeOn == 3 && RightNodeOn == 3) || (VTL3nodeSelected == 3 && VTR3nodeSelected == 3))
							{
								drawVoltageHackLineLonger("trafficcam", "traffic_junction_line_v", 0.4828f, 0.7638f, 550f, 10f, 0, 0, 255, 255, 90f, 240f);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.104f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
								drawVoltageHackSpriteScaled("mpisland_voltage", "maininterface_progress_highlight", 0.8564f, 0.7631f, 45f, 50f, 0, 0, 255, 255);
							}
						}
					}
					if (NumberAtCheck >= (float)num70 && SelectedRN3 && SelectedRN2 && SelectedRN1)
					{
						if (!AwaitingDelay)
						{
							if ((int)NumberAtCheck != volthackTargetNumber)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "All_Connected_Incorrect", uParam1f_741, true);
							}
							AwaitingDelay = true;
							Delay = Game.GameTime + 1000;
						}
						if (Game.GameTime > Delay)
						{
							if ((int)NumberAtCheck != volthackTargetNumber)
							{
								DisableControlls = false;
								AwaitingDelay = false;
								RightNodeOn = 0;
								SelectedLN1 = false;
								SelectedRN1 = false;
								SelectedLN2 = false;
								SelectedRN2 = false;
								SelectedLN3 = false;
								SelectedRN3 = false;
								VTL1nodeSelected = 0;
								VTR1nodeSelected = 0;
								VTL2nodeSelected = 0;
								VTR2nodeSelected = 0;
								VTL3nodeSelected = 0;
								VTR3nodeSelected = 0;
								VTL1node = 0;
								VTR1node = 0;
								VTL2node = 0;
								VTR2node = 0;
								VTL3node = 0;
								VTR3node = 0;
								RightNodeOn = 0;
								LeftNodeOn = 1;
								ScrableNumberTimer = Game.GameTime;
								NodeOn = 0;
								NumberAtCheck = 0f;
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "All_Connected_Incorrect", uParam1f_741, true);
							}
							if ((int)NumberAtCheck == volthackTargetNumber)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Minigame_Success", uParam1f_741, true);
								HackFinishedID = 1;
								BINKMovie = -1;
								VideoPlaying = true;
								BINKMovie = Function.Call<int>(Hash.SET_BINK_MOVIE, "voltage_success");
								Function.Call(Hash.PLAY_BINK_MOVIE, BINKMovie);
								HackStage = 2;
							}
						}
					}
				}
				if (HackStage == 2)
				{
					if (VideoPlaying)
					{
						Function.Call(Hash.DRAW_BINK_MOVIE, BINKMovie, 0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);
					}
					if (Function.Call<float>(Hash.GET_BINK_MOVIE_TIME, BINKMovie) >= 99.5f || Game.IsControlJustPressed(Control.VehicleHandbrake))
					{
						Game.Player.CanControlCharacter = true;
						Function.Call(Hash.STOP_BINK_MOVIE, BINKMovie);
						Function.Call(Hash.RELEASE_BINK_MOVIE, BINKMovie);
						VideoPlaying = false;
						BINKMovie = -1;
						PlayingVoltageMG = false;
					}
				}
			}
			if (RunningPadLockMinigame)
			{
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				GUIOn = true;
				if (Game.IsControlJustPressed((Control)ExitKey))
				{
					Game.Player.CanControlCharacter = true;
					HackStage = 0;
					RunningPadLockMinigame = false;
					PDLHackComplete = false;
					HackFinishedID = 3;
				}
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				if (HackStage == 0)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mpsafecracking"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mpsafecracking", 0);
					}
					HackStage = 1;
				}
				if (HackStage == 1)
				{
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						DisplayHelpTextThisFrame("Use ~INPUT_CELLPHONE_LEFT~/~INPUT_CELLPHONE_RIGHT~ to spin the dial Left/Right, stop on a number to see if it correct\nPress " + ReturnButtonString(ExitKey) + " to Quit ", 1);
					}
					if (Game.LastInputMethod == InputMethod.GamePad)
					{
						DisplayHelpTextThisFrame("Use ~INPUT_MOVE_LEFT_ONLY~/~INPUT_MOVE_RIGHT_ONLY~ to spin the dial Left/Right, stop on a number to see if it correct\nPress " + ReturnButtonString(ExitKey) + " to Quit ", 1);
					}
					drawSpriteRTA("mpsafecracking", "dial_bg", 0.51f, 0.5f, 800f, 1250f, 255, 255, 255, 255, 0f);
					drawSpriteRTA("mpsafecracking", "dial", 0.5f, 0.5f, 400f, 650f, 255, 255, 255, 255, Rotation);
					if (!GotNo1)
					{
						drawSprite3("mpsafecracking", "lock_closed", 0.4f, 0.8f, 100f, 150f, 255, 255, 255, 255);
					}
					if (GotNo1)
					{
						drawSprite3("mpsafecracking", "lock_open", 0.4f, 0.8f, 100f, 150f, 255, 255, 255, 255);
					}
					if (!GotNo2)
					{
						drawSprite3("mpsafecracking", "lock_closed", 0.5f, 0.8f, 100f, 150f, 255, 255, 255, 255);
					}
					if (GotNo2)
					{
						drawSprite3("mpsafecracking", "lock_open", 0.5f, 0.8f, 100f, 150f, 255, 255, 255, 255);
					}
					if (!GotNo3)
					{
						drawSprite3("mpsafecracking", "lock_closed", 0.6f, 0.8f, 100f, 150f, 255, 255, 255, 255);
					}
					if (GotNo3)
					{
						drawSprite3("mpsafecracking", "lock_open", 0.6f, 0.8f, 100f, 150f, 255, 255, 255, 255);
					}
					Control control = Control.PhoneLeft;
					Control control2 = Control.PhoneRight;
					if (Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						control = Control.PhoneLeft;
						control2 = Control.PhoneRight;
					}
					else
					{
						control = Control.MoveLeftOnly;
						control2 = Control.MoveRightOnly;
					}
					if (Game.IsControlPressed(control))
					{
						Rotation += 0.5f;
						ActualRotation -= 0.14f;
						LeftMP += 0.075f;
						if (ActualRotation < 0f)
						{
							ActualRotation = 100f;
						}
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_TURN", "SAFE_CRACK_SOUNDSET", 1);
					}
					if (Game.IsControlPressed(control2))
					{
						Rotation -= 0.5f;
						ActualRotation += 0.14f;
						RightMP += 0.075f;
						if (ActualRotation > 100f)
						{
							ActualRotation = 0f;
						}
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_TURN", "SAFE_CRACK_SOUNDSET", 1);
					}
					if (!Game.IsControlPressed(control2))
					{
						RightMP = 1.25f;
					}
					if (!Game.IsControlPressed(control))
					{
						LeftMP = 1.25f;
					}
					if ((double)Rotation > -0.75 && (double)Rotation < -0.25)
					{
						ActualRotation = 0f;
					}
					if ((double)Rotation > 0.25 && (double)Rotation < 0.75)
					{
						ActualRotation = 99f;
					}
					if (ActualRotation > 100f)
					{
						ActualRotation = 0f;
					}
					if ((int)ActualRotation == Number1 && !GotNo1)
					{
						if (!AllowUnlockonNonStop && !Game.IsControlPressed(Control.PhoneLeft) && !Game.IsControlPressed(Control.PhoneRight))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_RESET", "SAFE_CRACK_SOUNDSET", 1);
							GotNo1 = true;
						}
						if (AllowUnlockonNonStop)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_RESET", "SAFE_CRACK_SOUNDSET", 1);
							GotNo1 = true;
						}
					}
					if ((int)ActualRotation == Number2 && !GotNo2 && GotNo1)
					{
						if (!AllowUnlockonNonStop && !Game.IsControlPressed(Control.PhoneLeft) && !Game.IsControlPressed(Control.PhoneRight))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
							GotNo2 = true;
						}
						if (AllowUnlockonNonStop)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
							GotNo2 = true;
						}
					}
					if ((int)ActualRotation == Number3 && !GotNo3 && GotNo2 && GotNo1)
					{
						if (!AllowUnlockonNonStop && !Game.IsControlPressed(Control.PhoneLeft) && !Game.IsControlPressed(Control.PhoneRight))
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
							GotNo3 = true;
						}
						if (AllowUnlockonNonStop)
						{
							HackTimer = 0;
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL", "SAFE_CRACK_SOUNDSET", 1);
							GotNo3 = true;
						}
					}
					if (GotNo3 && GotNo2 && GotNo1)
					{
						if (HackTimer < 50)
						{
							drawSprite3("mpsafecracking", "lock_closed_dark", 0.5f, 0.5f, 100f, 150f, 255, 255, 255, 255);
						}
						if (HackTimer >= 50)
						{
							drawSprite3("mpsafecracking", "lock_open_dark", 0.5f, 0.5f, 100f, 150f, 255, 255, 255, 255);
						}
						if (HackTimer < 1000)
						{
							HackTimer++;
						}
						if (HackTimer >= 50 && HackTimer < 22)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "TUMBLER_PIN_FALL_FINAL", "SAFE_CRACK_SOUNDSET", 1);
						}
						if (HackTimer >= 100)
						{
							HackFinishedID = 1;
							Game.Player.CanControlCharacter = true;
							PDLHackComplete = true;
							RunningPadLockMinigame = false;
							HackStage = 0;
						}
					}
				}
			}
			if (HotWireMinigameOn)
			{
				Scaleform scaleform7 = new Scaleform("instructional_buttons");
				scaleform7.CallFunction("CLEAR_ALL");
				scaleform7.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform7.CallFunction("CREATE_CONTAINER");
				scaleform7.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Navigation Up");
				scaleform7.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Navigation Down");
				scaleform7.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Navigation Next");
				scaleform7.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Navigation Back");
				scaleform7.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Choose Node");
				scaleform7.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
				scaleform7.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform7.Render2D();
				GUIOn = true;
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
				if (Game.IsControlJustPressed((Control)ExitKey))
				{
					HackFinishedID = 3;
					Game.Player.CanControlCharacter = true;
					LeftSelected = false;
					RightSelected = false;
					RightNode = 0;
					HackStage = 0;
					HotWireMinigameOn = false;
					Game.Player.CanControlCharacter = true;
				}
				float num72 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
				float num73 = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
				drawText("TokensSelected " + TokensSelected, 0.1f, 0.28f, 0.3f, 255, 255, 255);
				drawText("SelectedLeftNode_1 " + SelectedLeftNode_1 + "/" + SelectedRightNode_1, 0.1f, 0.31f, 0.3f, 255, 255, 255);
				drawText("SelectedLeftNode_2 " + SelectedLeftNode_2 + "/" + SelectedRightNode_2, 0.1f, 0.34f, 0.3f, 255, 255, 255);
				drawText("SelectedLeftNode_3 " + SelectedLeftNode_3 + "/" + SelectedRightNode_3, 0.1f, 0.37f, 0.3f, 255, 255, 255);
				drawText("SelectedLeftNode_4 " + SelectedLeftNode_4 + "/" + SelectedRightNode_4, 0.1f, 0.4f, 0.3f, 255, 255, 255);
				drawText("Nodes  " + LeftSelected + "/" + RightSelected, 0.1f, 0.43f, 0.3f, 255, 255, 255);
				drawText("Node Selected  " + LeftNode + "/" + RightNode, 0.1f, 0.46f, 0.3f, 255, 255, 255);
				drawText("Nodes Locked  " + LeftNode_1Locked + "/" + LeftNode_2Locked + "/" + LeftNode_3Locked + "/" + LeftNode_4Locked, 0.1f, 0.49f, 0.3f, 255, 255, 255);
				drawText("HotwirePassState  " + HotwirePassState, 0.1f, 0.52f, 0.3f, 255, 255, 255);
				int num74 = (int)HotwireTimeLeft - Game.GameTime;
				TimeSpan timeSpan6 = TimeSpan.FromMilliseconds(num74);
				string text10 = $"{(int)timeSpan6.TotalMinutes:D2}:{timeSpan6.Seconds:D2}";
				float absoluteX4;
				float absoluteY4;
				LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX4, out absoluteY4);
				int x = Convert.ToInt32(absoluteX4 / 2f);
				if (HotwirePassState == 0)
				{
					TEXT(text10, new Point(x, 190), 0.35f, Color.FromArgb(255, 145, 255, 200), GTA.UI.Font.ChaletLondon);
				}
				if (scaleform7 == null)
				{
					scaleform7 = new Scaleform("instructional_buttons");
				}
				scaleform7.Render2D();
				if (HotwireTimeLeft >= 1f && HotwireTimeLeft <= 2f)
				{
					HotwireloadingDelayTimer = 0f;
					HotwireloadingDelay = 250f;
				}
				if (HotwirePassState == 0 && num74 <= 0)
				{
					if (HotwireloadingDelayTimer <= HotwireloadingDelay)
					{
						SuccessHotwire = true;
						TEXT("FAILED", new Point(x, 510), 0.25f, Color.Red, GTA.UI.Font.ChaletLondon);
						if (this.scaleform2 == null)
						{
							this.scaleform2 = new Scaleform("instructional_buttons");
						}
						this.scaleform2.Render2D();
						HotwireloadingDelayTimer += 1f;
						drawSprite3("mphotwire", "failed", 0.5f, 0.5f, 350f, 200f, 255, 255, 255, 255);
						HackFinishedID = 2;
					}
					if (HotwireloadingDelayTimer >= HotwireloadingDelay)
					{
						HackFinishedID = 2;
						SuccessHotwire = false;
						LeftSelected = true;
						RightSelected = false;
						TokensSelected = 0;
						HotWireMinigameOn = false;
						Game.Player.CanControlCharacter = true;
					}
				}
				if (HackStage == 0)
				{
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "DLC_Biker_Cracked_Sounds", false, -1);
					Function.Call(Hash.REQUEST_SCRIPT_AUDIO_BANK, "SAFE_CRACK", false, -1);
					while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "mphotwire"))
					{
						Script.Wait(10);
						Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "mphotwire", 0);
					}
					LeftSelected = true;
					RightSelected = false;
					DelayInputTime = Game.GameTime + 1500;
					HackStage = 1;
				}
				if (HackStage == 1)
				{
					scaleform7.Render2D();
					drawSprite3("mphotwire", "bg", 0.5f, 0.5f, 1920f, 1920f, 255, 255, 255, 255);
					if (!LeftSelected && RightSelected)
					{
						if (Game.IsControlJustPressed(Control.Aim))
						{
							LeftSelected = true;
							RightSelected = false;
							if (LeftNode == 1)
							{
								LeftNode_1Locked = 0;
								SelectedLeftNode_1 = 0;
								SelectedRightNode_1 = 0;
								TokensSelected--;
								return;
							}
							if (LeftNode == 2)
							{
								LeftNode_2Locked = 0;
								SelectedLeftNode_2 = 0;
								SelectedRightNode_2 = 0;
								TokensSelected--;
								return;
							}
							if (LeftNode == 3)
							{
								LeftNode_3Locked = 0;
								SelectedLeftNode_3 = 0;
								SelectedRightNode_3 = 0;
								TokensSelected--;
								return;
							}
							if (LeftNode == 4)
							{
								LeftNode_4Locked = 0;
								SelectedLeftNode_4 = 0;
								SelectedRightNode_4 = 0;
								TokensSelected--;
								return;
							}
						}
						Vector2 vEndPoint = new Vector2(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239), Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240));
						if (LeftNode == 1)
						{
							Draw_Wire(GET_PORT_FROM_START_NODE(1), vEndPoint);
						}
						if (LeftNode == 2)
						{
							Draw_Wire(GET_PORT_FROM_START_NODE(2), vEndPoint);
						}
						if (LeftNode == 3)
						{
							Draw_Wire(GET_PORT_FROM_START_NODE(3), vEndPoint);
						}
						if (LeftNode == 4)
						{
							Draw_Wire(GET_PORT_FROM_START_NODE(4), vEndPoint);
						}
					}
					if (TokensSelected == 5 && Game.GameTime >= CheckHotwireTime)
					{
						TokensSelected = 6;
						CheckHotwireTime = Game.GameTime + 5000;
					}
					if (Game.GameTime > DelayInputTime)
					{
						if (LeftSelected && !RightSelected && Game.IsControlJustPressed(Control.Aim))
						{
							if (SelectedLeftNode_4 != 0)
							{
								LeftNode_4Locked = 0;
								SelectedLeftNode_4 = 0;
								SelectedRightNode_4 = 0;
								TokensSelected--;
								return;
							}
							if (SelectedLeftNode_3 != 0)
							{
								LeftNode_3Locked = 0;
								SelectedLeftNode_3 = 0;
								SelectedRightNode_3 = 0;
								TokensSelected--;
								return;
							}
							if (SelectedLeftNode_2 != 0)
							{
								LeftNode_2Locked = 0;
								SelectedLeftNode_2 = 0;
								SelectedRightNode_2 = 0;
								TokensSelected--;
								return;
							}
							if (SelectedLeftNode_1 != 0)
							{
								LeftNode_1Locked = 0;
								SelectedLeftNode_1 = 0;
								SelectedRightNode_1 = 0;
								TokensSelected--;
								return;
							}
						}
						if (Game.IsControlJustPressed(Control.PhoneSelect))
						{
							if (LeftSelected && !RightSelected)
							{
								SelectingNode = true;
								if (LeftNode == 1 && SelectedLeftNode_1 == 0)
								{
									SelectedLeftNode_1 = LeftNode;
									LeftSelected = false;
									RightSelected = true;
									RightNode = 1;
								}
								if (LeftNode == 2 && SelectedLeftNode_2 == 0)
								{
									SelectedLeftNode_2 = LeftNode;
									LeftSelected = false;
									RightSelected = true;
									RightNode = 1;
								}
								if (LeftNode == 3 && SelectedLeftNode_3 == 0)
								{
									SelectedLeftNode_3 = LeftNode;
									LeftSelected = false;
									RightSelected = true;
									RightNode = 1;
								}
								if (LeftNode == 4 && SelectedLeftNode_4 == 0)
								{
									SelectedLeftNode_4 = LeftNode;
									LeftSelected = false;
									RightSelected = true;
									RightNode = 1;
								}
								if (TokensSelected == 5)
								{
									HotwireloadingDelayTimer = 0f;
									HotwireloadingDelay = 250f;
									TokensSelected = 6;
								}
							}
							else if (!LeftSelected && RightSelected)
							{
								if (Game.IsControlJustPressed(Control.Aim))
								{
									RightNode = 0;
									LeftSelected = true;
									RightSelected = false;
									LeftNode = 1;
									if (SelectedRightNode_1 == 0)
									{
										LeftNode = 1;
									}
									else if (SelectedRightNode_2 != 0)
									{
										LeftNode = 2;
									}
									else if (SelectedRightNode_3 != 0)
									{
										LeftNode = 3;
									}
									else if (SelectedRightNode_4 != 0)
									{
										LeftNode = 4;
									}
								}
								if (Game.IsControlJustPressed(Control.PhoneSelect))
								{
									if (LeftNode == 1)
									{
										LeftNode_1Locked = 1;
									}
									if (LeftNode == 2)
									{
										LeftNode_2Locked = 1;
									}
									if (LeftNode == 3)
									{
										LeftNode_3Locked = 1;
									}
									if (LeftNode == 4)
									{
										LeftNode_4Locked = 1;
									}
									SelectingNode = true;
									if (LeftNode == 1 && SelectedRightNode_1 == 0)
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
										TokensSelected++;
										SelectedRightNode_1 = RightNode;
										RightNode = 0;
										LeftSelected = true;
										RightSelected = false;
										LeftNode = 0;
									}
									if (LeftNode == 2 && SelectedRightNode_2 == 0)
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
										TokensSelected++;
										SelectedRightNode_2 = RightNode;
										RightNode = 0;
										LeftSelected = true;
										RightSelected = false;
										LeftNode = 0;
									}
									if (LeftNode == 3 && SelectedRightNode_3 == 0)
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
										TokensSelected++;
										SelectedRightNode_3 = RightNode;
										RightNode = 0;
										LeftSelected = true;
										RightSelected = false;
										LeftNode = 0;
									}
									if (LeftNode == 4 && SelectedRightNode_4 == 0)
									{
										Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
										TokensSelected++;
										SelectedRightNode_4 = RightNode;
										RightNode = 0;
										LeftSelected = true;
										RightSelected = false;
										LeftNode = 0;
									}
									if (TokensSelected == 4)
									{
										TokensSelected = 5;
										RightNode = 0;
										LeftSelected = true;
										RightSelected = false;
										CheckHotwireTime = Game.GameTime + 1500;
									}
								}
							}
						}
					}
					if (!SuccessHotwire)
					{
						Vector2 position2 = new Vector2(0.08f, 0.28f);
						Vector2 position3 = new Vector2(0.08f, 0.43f);
						Vector2 position4 = new Vector2(0.08f, 0.58f);
						Vector2 position5 = new Vector2(0.08f, 0.73f);
						Vector2 position6 = new Vector2(0.93f, 0.28f);
						Vector2 position7 = new Vector2(0.93f, 0.43f);
						Vector2 position8 = new Vector2(0.93f, 0.58f);
						Vector2 position9 = new Vector2(0.93f, 0.73f);
						Vector2 position10 = new Vector2(num73, num72);
						if (!RightSelected && LeftSelected)
						{
							if ((double)Vector2.Distance(position2, position10) < 0.09)
							{
								LastNodeX = position2.X;
								LastNodeY = position2.Y;
								LeftNode = 1;
							}
							if ((double)Vector2.Distance(position3, position10) < 0.09)
							{
								LastNodeX = position3.X;
								LastNodeY = position3.Y;
								LeftNode = 2;
							}
							if ((double)Vector2.Distance(position4, position10) < 0.09)
							{
								LastNodeX = position4.X;
								LastNodeY = position4.Y;
								LeftNode = 3;
							}
							if ((double)Vector2.Distance(position5, position10) < 0.09)
							{
								LastNodeX = position5.X;
								LastNodeY = position5.Y;
								LeftNode = 4;
							}
						}
						if (RightSelected && !LeftSelected)
						{
							bool selectingNode = SelectingNode;
							bool selectingNode2 = SelectingNode;
							if ((double)Vector2.Distance(position6, position10) < 0.09)
							{
								RightNode = 1;
							}
							if ((double)Vector2.Distance(position7, position10) < 0.09)
							{
								RightNode = 2;
							}
							if ((double)Vector2.Distance(position8, position10) < 0.09)
							{
								RightNode = 3;
							}
							if ((double)Vector2.Distance(position9, position10) < 0.09)
							{
								RightNode = 4;
							}
						}
						if (LeftNode == 1 && LeftNode_1Locked != 1)
						{
							drawSprite3("mphotwire", "leftnode_roll", 0.08f, 0.28f, 150f, 100f, 255, 255, 255, 255);
						}
						else
						{
							drawSprite3("mphotwire", "leftnode", 0.08f, 0.28f, 150f, 100f, 255, 255, 255, 255);
						}
						if (LeftNode == 2 && LeftNode_2Locked != 1)
						{
							drawSprite3("mphotwire", "leftnode_roll", 0.08f, 0.43f, 150f, 100f, 255, 255, 255, 255);
						}
						else
						{
							drawSprite3("mphotwire", "leftnode", 0.08f, 0.43f, 150f, 100f, 255, 255, 255, 255);
						}
						if (LeftNode == 3 && LeftNode_3Locked != 1)
						{
							drawSprite3("mphotwire", "leftnode_roll", 0.08f, 0.58f, 150f, 100f, 255, 255, 255, 255);
						}
						else
						{
							drawSprite3("mphotwire", "leftnode", 0.08f, 0.58f, 150f, 100f, 255, 255, 255, 255);
						}
						if (LeftNode == 4 && LeftNode_4Locked != 1)
						{
							drawSprite3("mphotwire", "leftnode_roll", 0.08f, 0.73f, 150f, 100f, 255, 255, 255, 255);
						}
						else
						{
							drawSprite3("mphotwire", "leftnode", 0.08f, 0.73f, 150f, 100f, 255, 255, 255, 255);
						}
						if (RightNode == 1)
						{
							drawSprite3("mphotwire", "rightnode_roll", 0.93f, 0.28f, 150f, 100f, 255, 255, 255, 255);
						}
						else
						{
							drawSprite3("mphotwire", "rightnode", 0.93f, 0.28f, 150f, 100f, 255, 255, 255, 255);
						}
						if (RightNode == 2)
						{
							drawSprite3("mphotwire", "rightnode_roll", 0.93f, 0.43f, 150f, 100f, 255, 255, 255, 255);
						}
						else
						{
							drawSprite3("mphotwire", "rightnode", 0.93f, 0.43f, 150f, 100f, 255, 255, 255, 255);
						}
						if (RightNode == 3)
						{
							drawSprite3("mphotwire", "rightnode_roll", 0.93f, 0.58f, 150f, 100f, 255, 255, 255, 255);
						}
						else
						{
							drawSprite3("mphotwire", "rightnode", 0.93f, 0.58f, 150f, 100f, 255, 255, 255, 255);
						}
						if (RightNode == 4)
						{
							drawSprite3("mphotwire", "rightnode_roll", 0.93f, 0.73f, 150f, 100f, 255, 255, 255, 255);
						}
						else
						{
							drawSprite3("mphotwire", "rightnode", 0.93f, 0.73f, 150f, 100f, 255, 255, 255, 255);
						}
						Random random6 = new Random();
						if (RightNode_1Lives != 4)
						{
							if (RightNode_1Lives <= 3)
							{
								drawSprite3("mphotwire", "lifelight", 0.985f, 0.285f, 15f, 30f, 255, 255, 255, (int)Node1Warnings[0]);
								Node1Warnings[0] -= (float)random6.Next(135, 999) / 100f;
								if (Node1Warnings[0] <= 0f)
								{
									Node1Warnings[0] = 255f;
								}
							}
							if (RightNode_1Lives <= 2)
							{
								drawSprite3("mphotwire", "lifelight", 0.955f, 0.285f, 15f, 30f, 255, 255, 255, (int)Node1Warnings[1]);
								Node1Warnings[1] -= (float)random6.Next(135, 999) / 100f;
								if (Node1Warnings[1] <= 0f)
								{
									Node1Warnings[1] = 255f;
								}
							}
							if (RightNode_1Lives <= 1)
							{
								drawSprite3("mphotwire", "lifelight", 0.925f, 0.285f, 15f, 30f, 255, 255, 255, (int)Node1Warnings[2]);
								Node1Warnings[2] -= (float)random6.Next(135, 999) / 100f;
								if (Node1Warnings[2] <= 0f)
								{
									Node1Warnings[2] = 255f;
								}
							}
							if (RightNode_1Lives <= 0)
							{
								drawSprite3("mphotwire", "lifelight", 0.895f, 0.285f, 15f, 30f, 255, 255, 255, (int)Node1Warnings[3]);
								Node1Warnings[3] -= (float)random6.Next(135, 999) / 100f;
								if (Node1Warnings[3] <= 0f)
								{
									Node1Warnings[3] = 255f;
								}
							}
							if (RightNode_1Lives == -1)
							{
								HotwireloadingDelayTimer = 0f;
								HotwireloadingDelay = 250f;
								RightNode_1Lives = -2;
							}
						}
						if (RightNode_2Lives != 4)
						{
							if (RightNode_2Lives <= 3)
							{
								drawSprite3("mphotwire", "lifelight", 0.985f, 0.435f, 15f, 30f, 255, 255, 255, (int)Node2Warnings[0]);
								Node2Warnings[0] -= (float)random6.Next(135, 999) / 100f;
								if (Node2Warnings[0] <= 0f)
								{
									Node2Warnings[0] = 255f;
								}
							}
							if (RightNode_2Lives <= 2)
							{
								drawSprite3("mphotwire", "lifelight", 0.955f, 0.435f, 15f, 30f, 255, 255, 255, (int)Node2Warnings[1]);
								Node2Warnings[1] -= (float)random6.Next(135, 999) / 100f;
								if (Node2Warnings[1] <= 0f)
								{
									Node2Warnings[1] = 255f;
								}
							}
							if (RightNode_2Lives <= 1)
							{
								drawSprite3("mphotwire", "lifelight", 0.925f, 0.435f, 15f, 30f, 255, 255, 255, (int)Node2Warnings[2]);
								Node2Warnings[2] -= (float)random6.Next(135, 999) / 100f;
								if (Node2Warnings[2] <= 0f)
								{
									Node2Warnings[2] = 255f;
								}
							}
							if (RightNode_2Lives <= 0)
							{
								drawSprite3("mphotwire", "lifelight", 0.895f, 0.435f, 15f, 30f, 255, 255, 255, (int)Node2Warnings[3]);
								Node2Warnings[3] -= (float)random6.Next(135, 999) / 100f;
								if (Node2Warnings[3] <= 0f)
								{
									Node2Warnings[3] = 255f;
								}
							}
							if (RightNode_2Lives == -1)
							{
								HotwireloadingDelayTimer = 0f;
								HotwireloadingDelay = 250f;
								RightNode_2Lives = -2;
							}
						}
						if (RightNode_3Lives != 4)
						{
							if (RightNode_3Lives <= 3)
							{
								drawSprite3("mphotwire", "lifelight", 0.985f, 0.585f, 15f, 30f, 255, 255, 255, (int)Node3Warnings[0]);
								Node3Warnings[0] -= (float)random6.Next(135, 999) / 100f;
								if (Node3Warnings[0] <= 0f)
								{
									Node3Warnings[0] = 255f;
								}
							}
							if (RightNode_3Lives <= 2)
							{
								drawSprite3("mphotwire", "lifelight", 0.955f, 0.585f, 15f, 30f, 255, 255, 255, (int)Node3Warnings[1]);
								Node3Warnings[1] -= (float)random6.Next(135, 999) / 100f;
								if (Node3Warnings[1] <= 0f)
								{
									Node3Warnings[1] = 255f;
								}
							}
							if (RightNode_3Lives <= 1)
							{
								drawSprite3("mphotwire", "lifelight", 0.925f, 0.585f, 15f, 30f, 255, 255, 255, (int)Node3Warnings[2]);
								Node3Warnings[2] -= (float)random6.Next(135, 999) / 100f;
								if (Node3Warnings[2] <= 0f)
								{
									Node3Warnings[2] = 255f;
								}
							}
							if (RightNode_3Lives <= 0)
							{
								drawSprite3("mphotwire", "lifelight", 0.895f, 0.585f, 15f, 30f, 255, 255, 255, (int)Node3Warnings[3]);
								Node3Warnings[3] -= (float)random6.Next(135, 999) / 100f;
								if (Node3Warnings[3] <= 0f)
								{
									Node3Warnings[3] = 255f;
								}
							}
							if (RightNode_3Lives == -1)
							{
								HotwireloadingDelayTimer = 0f;
								HotwireloadingDelay = 250f;
								RightNode_3Lives = -2;
							}
						}
						if (RightNode_4Lives != 4)
						{
							if (RightNode_4Lives <= 3)
							{
								drawSprite3("mphotwire", "lifelight", 0.985f, 0.735f, 15f, 30f, 255, 255, 255, (int)Node4Warnings[0]);
								Node4Warnings[0] -= (float)random6.Next(135, 999) / 100f;
								if (Node4Warnings[0] <= 0f)
								{
									Node4Warnings[0] = 255f;
								}
							}
							if (RightNode_4Lives <= 2)
							{
								drawSprite3("mphotwire", "lifelight", 0.955f, 0.735f, 15f, 30f, 255, 255, 255, (int)Node4Warnings[1]);
								Node4Warnings[1] -= (float)random6.Next(135, 999) / 100f;
								if (Node4Warnings[1] <= 0f)
								{
									Node4Warnings[1] = 255f;
								}
							}
							if (RightNode_4Lives <= 1)
							{
								drawSprite3("mphotwire", "lifelight", 0.925f, 0.735f, 15f, 30f, 255, 255, 255, (int)Node4Warnings[2]);
								Node4Warnings[2] -= (float)random6.Next(135, 999) / 100f;
								if (Node4Warnings[2] <= 0f)
								{
									Node4Warnings[2] = 255f;
								}
							}
							if (RightNode_4Lives <= 0)
							{
								drawSprite3("mphotwire", "lifelight", 0.895f, 0.735f, 15f, 30f, 255, 255, 255, (int)Node4Warnings[3]);
								Node4Warnings[3] -= (float)random6.Next(135, 999) / 100f;
								if (Node4Warnings[3] <= 0f)
								{
									Node4Warnings[3] = 255f;
								}
							}
							if (RightNode_4Lives == -1)
							{
								HotwireloadingDelayTimer = 0f;
								HotwireloadingDelay = 250f;
								RightNode_4Lives = -2;
							}
						}
					}
					if (SuccessHotwire)
					{
						drawSprite3("mphotwire", "bg", 0.5f, 0.5f, 1920f, 1920f, 255, 255, 255, 255);
					}
					if (!SuccessHotwire)
					{
						if (SelectedLeftNode_1 == 1 && SelectedRightNode_1 == 4)
						{
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.4f, 525f, 310f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line2", 0.505f, 0.63f, 520f, 310f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 4 && SelectedRightNode_1 == 1)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.4f, 525f, -310f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.505f, 0.63f, 520f, -310f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_1 == 2 && SelectedRightNode_1 == 3)
						{
							drawSpriteRTA("mphotwire", "line1", 0.33f, 0.515f, 270f, 270f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.68f, 0.5f, 270f, 270f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 3 && SelectedRightNode_1 == 2)
						{
							drawSpriteRTA("mphotwire", "line1", 0.33f, 0.5f, 270f, -270f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.68f, 0.515f, 270f, -270f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 2 && SelectedRightNode_1 == 1)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.354f, 259f, -250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.355f, 258f, -250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 1 && SelectedRightNode_1 == 2)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.354f, 259f, 250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.355f, 258f, 250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 4 && SelectedRightNode_1 == 3)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.65f, 259f, -250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.65f, 258f, -250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 3 && SelectedRightNode_1 == 4)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.65f, 259f, 250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.65f, 258f, 250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 1 && SelectedRightNode_1 == 1)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.28f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 2 && SelectedRightNode_1 == 2)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.43f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 3 && SelectedRightNode_1 == 3)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.58f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 4 && SelectedRightNode_1 == 4)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.73f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_1 == 4 && SelectedRightNode_1 == 2)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.5f, 520f, -220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.66f, 520f, -220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_1 == 2 && SelectedRightNode_1 == 4)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.66f, 520f, 220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.5f, 520f, 220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_1 == 3 && SelectedRightNode_1 == 1)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.35f, 520f, -220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.51f, 520f, -220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_1 == 1 && SelectedRightNode_1 == 3)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.51f, 520f, 220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.35f, 520f, 220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_2 == 1 && SelectedRightNode_2 == 4)
						{
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.4f, 525f, 310f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line2", 0.505f, 0.63f, 520f, 310f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 4 && SelectedRightNode_2 == 1)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.4f, 525f, -310f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.505f, 0.63f, 520f, -310f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_2 == 2 && SelectedRightNode_2 == 3)
						{
							drawSpriteRTA("mphotwire", "line1", 0.33f, 0.515f, 270f, 270f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.68f, 0.5f, 270f, 270f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 3 && SelectedRightNode_2 == 2)
						{
							drawSpriteRTA("mphotwire", "line1", 0.33f, 0.5f, 270f, -270f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.68f, 0.515f, 270f, -270f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 2 && SelectedRightNode_2 == 1)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.354f, 259f, -250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.355f, 258f, -250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 1 && SelectedRightNode_2 == 2)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.354f, 259f, 250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.355f, 258f, 250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 4 && SelectedRightNode_2 == 3)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.65f, 259f, -250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.65f, 258f, -250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 3 && SelectedRightNode_2 == 4)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.65f, 259f, 250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.65f, 258f, 250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 1 && SelectedRightNode_2 == 1)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.28f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 2 && SelectedRightNode_2 == 2)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.43f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 3 && SelectedRightNode_2 == 3)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.58f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 4 && SelectedRightNode_2 == 4)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.73f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_2 == 4 && SelectedRightNode_2 == 2)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.5f, 520f, -220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.66f, 520f, -220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_2 == 2 && SelectedRightNode_2 == 4)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.66f, 520f, 220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.5f, 520f, 220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_2 == 3 && SelectedRightNode_2 == 1)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.35f, 520f, -220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.51f, 520f, -220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_2 == 1 && SelectedRightNode_2 == 3)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.51f, 520f, 220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.35f, 520f, 220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_3 == 1 && SelectedRightNode_3 == 4)
						{
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.4f, 525f, 310f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line2", 0.505f, 0.63f, 520f, 310f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 4 && SelectedRightNode_3 == 1)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.4f, 525f, -310f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.505f, 0.63f, 520f, -310f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_3 == 2 && SelectedRightNode_3 == 3)
						{
							drawSpriteRTA("mphotwire", "line1", 0.33f, 0.515f, 270f, 270f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.68f, 0.5f, 270f, 270f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 3 && SelectedRightNode_3 == 2)
						{
							drawSpriteRTA("mphotwire", "line1", 0.33f, 0.5f, 270f, -270f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.68f, 0.515f, 270f, -270f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 2 && SelectedRightNode_3 == 1)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.354f, 259f, -250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.355f, 258f, -250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 1 && SelectedRightNode_3 == 2)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.354f, 259f, 250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.355f, 258f, 250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 4 && SelectedRightNode_3 == 3)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.65f, 259f, -250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.65f, 258f, -250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 3 && SelectedRightNode_3 == 4)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.65f, 259f, 250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.65f, 258f, 250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 1 && SelectedRightNode_3 == 1)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.28f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 2 && SelectedRightNode_3 == 2)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.43f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 3 && SelectedRightNode_3 == 3)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.58f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 4 && SelectedRightNode_3 == 4)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.73f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_3 == 4 && SelectedRightNode_3 == 2)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.5f, 520f, -220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.66f, 520f, -220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_3 == 2 && SelectedRightNode_3 == 4)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.66f, 520f, 220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.5f, 520f, 220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_3 == 3 && SelectedRightNode_3 == 1)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.35f, 520f, -220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.51f, 520f, -220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_3 == 1 && SelectedRightNode_3 == 3)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.51f, 520f, 220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.35f, 520f, 220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_4 == 1 && SelectedRightNode_4 == 4)
						{
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.4f, 525f, 310f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line2", 0.505f, 0.63f, 520f, 310f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 4 && SelectedRightNode_4 == 1)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.4f, 525f, -310f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.505f, 0.63f, 520f, -310f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_4 == 2 && SelectedRightNode_4 == 3)
						{
							drawSpriteRTA("mphotwire", "line1", 0.33f, 0.515f, 270f, 270f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.68f, 0.5f, 270f, 270f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 3 && SelectedRightNode_4 == 2)
						{
							drawSpriteRTA("mphotwire", "line1", 0.33f, 0.5f, 270f, -270f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.68f, 0.515f, 270f, -270f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 2 && SelectedRightNode_4 == 1)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.354f, 259f, -250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.355f, 258f, -250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 1 && SelectedRightNode_4 == 2)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.354f, 259f, 250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.355f, 258f, 250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 4 && SelectedRightNode_4 == 3)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.65f, 259f, -250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.65f, 258f, -250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 3 && SelectedRightNode_4 == 4)
						{
							drawSpriteRTA("mphotwire", "line1", 0.325f, 0.65f, 259f, 250f, 255, 255, 255, 255, 0f);
							drawSpriteRTA("mphotwire", "line1", 0.685f, 0.65f, 258f, 250f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 1 && SelectedRightNode_4 == 1)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.28f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 2 && SelectedRightNode_4 == 2)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.43f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 3 && SelectedRightNode_4 == 3)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.58f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 4 && SelectedRightNode_4 == 4)
						{
							drawSpriteRTA("mphotwire", "line", 0.505f, 0.73f, 515f, 5f, 255, 255, 255, 255, 180f);
						}
						if (SelectedLeftNode_4 == 4 && SelectedRightNode_4 == 2)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.5f, 520f, -220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.66f, 520f, -220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_4 == 2 && SelectedRightNode_4 == 4)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.66f, 520f, 220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.5f, 520f, 220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_4 == 3 && SelectedRightNode_4 == 1)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.35f, 520f, -220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.51f, 520f, -220f, 255, 255, 255, 255, 0f);
						}
						if (SelectedLeftNode_4 == 1 && SelectedRightNode_4 == 3)
						{
							drawSpriteRTA("mphotwire", "line2", 0.5f, 0.51f, 520f, 220f, 255, 255, 255, 255, 180f);
							drawSpriteRTA("mphotwire", "line2", 0.51f, 0.35f, 520f, 220f, 255, 255, 255, 255, 0f);
						}
					}
					if (TokensSelected == 7)
					{
						TEXT("SUCCESS", new Point(x, 510), 0.35f, Color.FromArgb((int)Math.Round(fSuccessWindowAlpha), 145, 255, 200), GTA.UI.Font.ChaletLondon);
						float num75 = 0.25f;
						float num76 = 0.511f;
						float num77 = 22.7f;
						float num78 = 0.344f;
						float num79 = 0.127f;
						float num80 = Function.Call<float>(Hash.GET_ASPECT_RATIO, true);
						Function.Call(Hash.DRAW_SPRITE, "MPHotwire", "passed", 0.5f, 0.5f, num78 / num80, num79, 0, 255, 255, 255, (int)Math.Round(fSuccessWindowAlpha));
						num75 = 0.03f;
						if (HotwirePassState == 1)
						{
							num75 = 0.011f;
							float start = 0.375f;
							float fX = LERP_FLOAT(start, 0.5f, fDownloadAmount);
							Function.Call(Hash.DRAW_SPRITE, "MPHotwire", "load_bar", APPLY_ASPECT_MOD_TO_X_2(fX), num76, fDownloadAmount * num75 * num77 / num80, num75, 90, 255, 255, 255, 255);
						}
						switch (HotwirePassState)
						{
						case 0:
							fSuccessWindowAlpha += 3.5f;
							if (fSuccessWindowAlpha >= 255f)
							{
								fSuccessWindowAlpha = 255f;
								HotwirePassState = 1;
							}
							break;
						case 1:
							if (fDownloadAmount < 1f)
							{
								fDownloadAmount += 0.01f;
								if (fDownloadAmount >= 1f)
								{
									fDownloadAmount = 1f;
								}
							}
							else
							{
								Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "unlocked_bleep", Game.Player.Character, "HACKING_DOOR_UNLOCK_SOUNDS", 0, 0);
								DelayInputTime = Game.GameTime + 1000;
								fDownloadAmount = 1f;
								HotwirePassState = 2;
							}
							break;
						case 2:
							if (Game.GameTime >= DelayInputTime)
							{
								HotwirePassState = 3;
								float num81 = 0.612f;
								DRAW_BIG_HACKING_TEXT("BEAM_P", 76, 190, 148, 255, num81, num81, 0.5f, 0.456f);
							}
							break;
						case 3:
							SuccessHotwire = true;
							HotwireloadingDelayTimer += 1f;
							HotwirePassState = 0;
							HackFinishedID = 1;
							SuccessHotwire = false;
							LeftSelected = false;
							LeftNode = 0;
							RightSelected = false;
							RightNode = 0;
							HackStage = 0;
							LeftNode_4Locked = 0;
							SelectedLeftNode_4 = 0;
							SelectedRightNode_4 = 0;
							TokensSelected--;
							LeftNode_3Locked = 0;
							SelectedLeftNode_3 = 0;
							SelectedRightNode_3 = 0;
							TokensSelected--;
							LeftNode_2Locked = 0;
							SelectedLeftNode_2 = 0;
							SelectedRightNode_2 = 0;
							TokensSelected--;
							LeftNode_1Locked = 0;
							SelectedLeftNode_1 = 0;
							SelectedRightNode_1 = 0;
							TokensSelected--;
							TokensSelected = 0;
							Notification.Show("Hot Wire Minigame Success");
							HotWireMinigameOn = false;
							Game.Player.CanControlCharacter = true;
							break;
						}
						int hotwirePassState = HotwirePassState;
						int num85 = 3;
					}
					if (TokensSelected == 6)
					{
						if (SelectedRightNode_1 == Correct_1ID && SelectedRightNode_2 == Correct_2ID && SelectedRightNode_3 == Correct_3ID && SelectedRightNode_4 == Correct_4ID)
						{
							fSuccessWindowAlpha = 0f;
							fDownloadAmount = 0f;
							HotwirePassState = 0;
							HotwireloadingDelay = 250f;
							TokensSelected = 7;
						}
						else
						{
							if (HotwireloadingDelayTimer >= 1f && HotwireloadingDelayTimer <= 2f)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
							}
							if (Game.GameTime < CheckHotwireTime)
							{
								SuccessHotwire = true;
								TEXT("FAILED", new Point(x, 510), 0.25f, Color.Red, GTA.UI.Font.ChaletLondon);
								if (this.scaleform2 == null)
								{
									this.scaleform2 = new Scaleform("instructional_buttons");
								}
								this.scaleform2.Render2D();
								HotwireloadingDelayTimer += 1f;
								drawSprite3("mphotwire", "failed", 0.5f, 0.5f, 350f, 200f, 255, 255, 255, 255);
							}
							if (Game.GameTime >= CheckHotwireTime)
							{
								SuccessHotwire = false;
								LeftNode = 1;
								LeftSelected = true;
								RightSelected = false;
								TokensSelected = 4;
								if (SelectedRightNode_4 != Correct_4ID)
								{
									LeftNode_4Locked = 0;
									SelectedLeftNode_4 = 0;
									SelectedRightNode_4 = 0;
									TokensSelected--;
								}
								if (SelectedRightNode_3 != Correct_3ID)
								{
									LeftNode_3Locked = 0;
									SelectedLeftNode_3 = 0;
									SelectedRightNode_3 = 0;
									TokensSelected--;
								}
								if (SelectedRightNode_2 != Correct_2ID)
								{
									LeftNode_2Locked = 0;
									SelectedLeftNode_2 = 0;
									SelectedRightNode_2 = 0;
									TokensSelected--;
								}
								if (SelectedRightNode_1 != Correct_1ID)
								{
									LeftNode_1Locked = 0;
									SelectedLeftNode_1 = 0;
									SelectedRightNode_1 = 0;
									TokensSelected--;
								}
							}
						}
					}
					if (RightNode_1Lives == -2)
					{
						if (HotwireloadingDelayTimer <= HotwireloadingDelay)
						{
							HackFinishedID = 2;
							SuccessHotwire = true;
							TEXT("FAILED", new Point(x, 510), 0.25f, Color.Red, GTA.UI.Font.ChaletLondon);
							if (this.scaleform2 == null)
							{
								this.scaleform2 = new Scaleform("instructional_buttons");
							}
							this.scaleform2.Render2D();
							HotwireloadingDelayTimer += 1f;
							drawSprite3("mphotwire", "failed", 0.5f, 0.5f, 350f, 200f, 255, 255, 255, 255);
						}
						if (HotwireloadingDelayTimer >= HotwireloadingDelay)
						{
							SuccessHotwire = false;
							LeftNode = 1;
							LeftSelected = true;
							RightSelected = false;
							TokensSelected = 0;
							HotWireMinigameOn = false;
						}
					}
					if (RightNode_2Lives == -2)
					{
						if (HotwireloadingDelayTimer <= HotwireloadingDelay)
						{
							HackFinishedID = 2;
							SuccessHotwire = true;
							TEXT("FAILED", new Point(x, 510), 0.25f, Color.Red, GTA.UI.Font.ChaletLondon);
							if (this.scaleform2 == null)
							{
								this.scaleform2 = new Scaleform("instructional_buttons");
							}
							this.scaleform2.Render2D();
							HotwireloadingDelayTimer += 1f;
							drawSprite3("mphotwire", "failed", 0.5f, 0.5f, 350f, 200f, 255, 255, 255, 255);
						}
						if (HotwireloadingDelayTimer >= HotwireloadingDelay)
						{
							SuccessHotwire = false;
							LeftNode = 1;
							LeftSelected = true;
							RightSelected = false;
							TokensSelected = 0;
							HotWireMinigameOn = false;
						}
					}
					if (RightNode_3Lives == -2)
					{
						if (HotwireloadingDelayTimer <= HotwireloadingDelay)
						{
							HackFinishedID = 2;
							SuccessHotwire = true;
							TEXT("FAILED", new Point(x, 510), 0.25f, Color.Red, GTA.UI.Font.ChaletLondon);
							if (this.scaleform2 == null)
							{
								this.scaleform2 = new Scaleform("instructional_buttons");
							}
							this.scaleform2.Render2D();
							HotwireloadingDelayTimer += 1f;
							drawSprite3("mphotwire", "failed", 0.5f, 0.5f, 350f, 200f, 255, 255, 255, 255);
						}
						if (HotwireloadingDelayTimer >= HotwireloadingDelay)
						{
							SuccessHotwire = false;
							LeftNode = 1;
							LeftSelected = true;
							RightSelected = false;
							TokensSelected = 0;
							HotWireMinigameOn = false;
						}
					}
					if (RightNode_4Lives == -2)
					{
						if (HotwireloadingDelayTimer <= HotwireloadingDelay)
						{
							HackFinishedID = 2;
							SuccessHotwire = true;
							TEXT("FAILED", new Point(x, 510), 0.25f, Color.Red, GTA.UI.Font.ChaletLondon);
							if (this.scaleform2 == null)
							{
								this.scaleform2 = new Scaleform("instructional_buttons");
							}
							this.scaleform2.Render2D();
							HotwireloadingDelayTimer += 1f;
							drawSprite3("mphotwire", "failed", 0.5f, 0.5f, 350f, 200f, 255, 255, 255, 255);
						}
						if (HotwireloadingDelayTimer >= HotwireloadingDelay)
						{
							SuccessHotwire = false;
							LeftNode = 1;
							LeftSelected = true;
							RightSelected = false;
							TokensSelected = 0;
							HotWireMinigameOn = false;
						}
					}
				}
				drawSprite3("mphotwire", "cursor_0", num73, num72, 110f, 130f, 255, 255, 255, 255);
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				DisplayHelpTextThisFrame("To bypass the system, link the nodes on the left to the nodes on the right. You must link them in a specific way to achieve rewire the circuit.\nPress " + ReturnButtonString(ExitKey) + " to Exit", 1);
			}
			if (DataCrackOn)
			{
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				GUIOn = true;
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
				if (Game.IsControlJustPressed((Control)ExitKey))
				{
					if (Game.Player.Character.CurrentVehicle != null)
					{
						Game.Player.Character.CurrentVehicle.IsPositionFrozen = false;
					}
					HackingSuccess = false;
					colOneY = 0.4f;
					colTwoY = 0.4f;
					colThreeY = 0.4f;
					colFourY = 0.4f;
					colFiveY = 0.74f;
					colSixY = 0.74f;
					colSevenY = 0.74f;
					colEightY = 0.74f;
					collumn1 = false;
					collumn2 = false;
					collumn3 = false;
					collumn4 = false;
					collumn5 = false;
					collumn6 = false;
					collumn7 = false;
					collumn8 = false;
					skip1 = false;
					skip2 = false;
					skip3 = false;
					loadingin = true;
					up = false;
					up1 = false;
					up2 = false;
					up3 = false;
					up4 = true;
					up5 = true;
					up6 = true;
					up7 = true;
					currentbar = 0;
					indexx = 20;
					DataCrackOn = false;
				}
				switch (indexx)
				{
				case 0:
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "hackingNG", 0);
					break;
				case 10:
					if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "hackingNG"))
					{
						indexx = 20;
					}
					break;
				case 20:
					activatescale("HACKING_PC");
					Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, scaleformloading, 255, 255, 255, 255, 0);
					drawSprite2("hackingNG", "DHMain", 0.5f, 0.5f, 0.731f, 1.306f, 255, 255, 255, 255);
					if (currentbar == 0)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.35f, colOneY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.35f, colOneY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 1)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.39f, colTwoY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.39f, colTwoY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 2)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.43f, colThreeY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.43f, colThreeY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 3)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.47f, colFourY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.47f, colFourY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 4)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.51f, colFiveY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.51f, colFiveY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 5)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.55f, colSixY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.55f, colSixY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 6)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.59f, colSevenY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.59f, colSevenY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (currentbar == 7)
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHCompHi", 0.63f, colEightY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					else
					{
						Function.Call(Hash.DRAW_SPRITE, "hackingNG", "DHComp", 0.63f, colEightY, 0.4f, 0.4f, 0f, 255, 255, 255, 255, 0);
					}
					if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 2, 201) && firstbounce)
					{
						if (currentbar == 0)
						{
							if (check(colOneY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn1 = true;
								colOneY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 1;
							}
							if (check(colOneY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
							}
						}
						if (currentbar == 1 && Game.GameTime > GameTimeReference1)
						{
							if (check(colTwoY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn2 = true;
								colTwoY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 2;
							}
							if (check(colTwoY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 0;
								collumn1 = false;
							}
						}
						if (currentbar == 2 && Game.GameTime > GameTimeReference1)
						{
							if (check(colThreeY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn3 = true;
								colThreeY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 3;
							}
							if (check(colThreeY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 1;
								collumn2 = false;
							}
						}
						if (currentbar == 3 && Game.GameTime > GameTimeReference1)
						{
							if (check(colFourY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn4 = true;
								colFourY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 4;
							}
							if (check(colFourY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 2;
								collumn3 = false;
							}
						}
						if (currentbar == 4 && Game.GameTime > GameTimeReference1)
						{
							if (check(colFiveY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn5 = true;
								colFiveY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 5;
							}
							if (check(colFiveY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 3;
								collumn4 = false;
							}
						}
						if (currentbar == 5 && Game.GameTime > GameTimeReference1)
						{
							if (check(colSixY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn6 = true;
								colSixY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 6;
							}
							if (check(colSixY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 4;
								collumn5 = false;
							}
						}
						if (currentbar == 6 && Game.GameTime > GameTimeReference1)
						{
							if (check(colSevenY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn7 = true;
								colSevenY = 0.575f;
								GameTimeReference1 = Game.GameTime + 40;
								currentbar = 7;
							}
							if (check(colSevenY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 5;
								collumn6 = false;
							}
						}
						if (currentbar == 7 && Game.GameTime > GameTimeReference1)
						{
							if (check(colEightY) == 1)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								collumn8 = true;
								colEightY = 0.575f;
							}
							if (check(colEightY) == 0)
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, 2, "Pin_Bad", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
								currentbar = 6;
								collumn7 = false;
							}
						}
					}
					if (collumn1 && collumn2 && collumn3 && collumn4 && collumn5 && collumn6 && collumn7 && collumn8)
					{
						Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
						active = false;
						HackFinishedID = 1;
						colOneY = 0.4f;
						colTwoY = 0.4f;
						colThreeY = 0.4f;
						colFourY = 0.4f;
						colFiveY = 0.74f;
						colSixY = 0.74f;
						colSevenY = 0.74f;
						colEightY = 0.74f;
						collumn1 = false;
						collumn2 = false;
						collumn3 = false;
						collumn4 = false;
						collumn5 = false;
						collumn6 = false;
						collumn7 = false;
						collumn8 = false;
						skip1 = false;
						skip2 = false;
						skip3 = false;
						loadingin = true;
						firstbounce = false;
						up = false;
						up1 = false;
						up2 = false;
						up3 = false;
						up4 = true;
						up5 = true;
						up6 = true;
						up7 = true;
						currentbar = 0;
						DataCrackOn = false;
					}
					if (up && !collumn1)
					{
						if (colOneY > 0.4f)
						{
							colOneY -= 0.003f;
						}
						if (colOneY <= 0.4f)
						{
							up = false;
						}
					}
					if (!up && !collumn1)
					{
						if (colOneY < 0.74f)
						{
							colOneY += 0.003f;
						}
						if (colOneY >= 0.74f)
						{
							up = true;
						}
					}
					if (loadingin)
					{
						if (colOneY < 0.52f && colOneY > 0.47f && !up)
						{
							skip1 = true;
						}
						if (colOneY < 0.64f && colOneY > 0.59f && !up)
						{
							skip2 = true;
						}
						if ((double)colOneY < 0.75 && colOneY > 0.7f && !up)
						{
							skip3 = true;
							firstbounce = true;
							loadingin = false;
						}
					}
					if (skip1)
					{
						if (up1 && !collumn2)
						{
							if (colTwoY > 0.4f)
							{
								colTwoY -= 0.003f;
							}
							if (colTwoY <= 0.4f)
							{
								up1 = false;
							}
						}
						if (!up1 && !collumn2)
						{
							if (colTwoY < 0.74f)
							{
								colTwoY += 0.003f;
							}
							if (colTwoY >= 0.74f)
							{
								up1 = true;
							}
						}
					}
					if (skip2)
					{
						if (up2 && !collumn3)
						{
							if (colThreeY > 0.4f)
							{
								colThreeY -= 0.003f;
							}
							if (colThreeY <= 0.4f)
							{
								up2 = false;
							}
						}
						if (!up2 && !collumn3)
						{
							if (colThreeY < 0.74f)
							{
								colThreeY += 0.003f;
							}
							if (colThreeY >= 0.74f)
							{
								up2 = true;
							}
						}
					}
					if (skip3)
					{
						if (up3 && !collumn4)
						{
							if (colFourY > 0.4f)
							{
								colFourY -= 0.003f;
							}
							if (colFourY <= 0.4f)
							{
								up3 = false;
							}
						}
						if (!up3 && !collumn4)
						{
							if (colFourY < 0.74f)
							{
								colFourY += 0.003f;
							}
							if (colFourY >= 0.74f)
							{
								up3 = true;
							}
						}
					}
					if (skip3)
					{
						if (up4 && !collumn5)
						{
							if (colFiveY > 0.4f)
							{
								colFiveY -= 0.003f;
							}
							if (colFiveY <= 0.4f)
							{
								up4 = false;
							}
						}
						if (!up4 && !collumn5)
						{
							if (colFiveY < 0.74f)
							{
								colFiveY += 0.003f;
							}
							if (colFiveY >= 0.74f)
							{
								up4 = true;
							}
						}
					}
					if (skip2)
					{
						if (up5 && !collumn6)
						{
							if (colSixY > 0.4f)
							{
								colSixY -= 0.003f;
							}
							if (colSixY <= 0.4f)
							{
								up5 = false;
							}
						}
						if (!up5 && !collumn6)
						{
							if (colSixY < 0.74f)
							{
								colSixY += 0.003f;
							}
							if (colSixY >= 0.74f)
							{
								up5 = true;
							}
						}
					}
					if (skip1)
					{
						if (up6 && !collumn7)
						{
							if (colSevenY > 0.4f)
							{
								colSevenY -= 0.003f;
							}
							if (colSevenY <= 0.4f)
							{
								up6 = false;
							}
						}
						if (!up6 && !collumn7)
						{
							if (colSevenY < 0.74f)
							{
								colSevenY += 0.003f;
							}
							if (colSevenY >= 0.74f)
							{
								up6 = true;
							}
						}
					}
					if (up7 && !collumn8)
					{
						if (colEightY > 0.4f)
						{
							colEightY -= 0.003f;
						}
						if (colEightY <= 0.4f)
						{
							up7 = false;
						}
					}
					if (!up7 && !collumn8)
					{
						if (colEightY < 0.74f)
						{
							colEightY += 0.003f;
						}
						if (colEightY >= 0.74f)
						{
							up7 = true;
						}
					}
					break;
				}
				Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
				DisplayHelpTextThisFrame("Press ~INPUT_FRONTEND_ACCEPT~ when the highlighted block is passing through the red line. Align all blocks to successfuly pass the hack.\nPress " + ReturnButtonString(ExitKey) + " to Exit", 1);
			}
			if (FIBHackOn)
			{
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				GUIOn = true;
				if (Game.IsControlJustPressed((Control)ExitKey))
				{
					FIBHackOn = false;
				}
				DrawFIBHack("MORGUE_LAPTOP");
				if (Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, iLocal_8077))
				{
					Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, iLocal_8077, "SET_PROGRESS_PERCENT");
					int fIBHackPercentAmt = FIBHackPercentAmt;
					if (fIBHackPercentAmt >= 100)
					{
						HackFinishedID = 1;
					}
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, fIBHackPercentAmt);
					Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
					Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, iLocal_8077, 255, 255, 255, 255, 0);
				}
			}
			if (BruteForceHackOn)
			{
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				GUIOn = true;
				if (Game.IsControlJustPressed((Control)ExitKey))
				{
					HackingSuccess = false;
					UnloadLoadehacker();
					BruteForceHackOn = false;
					IPHackDone = false;
				}
				if (hackingScalwform != 0)
				{
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
					Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 240);
					Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 239);
					if ((Hacking_Index == 100 || Hacking_Index == 500) && Game.LastInputMethod == InputMethod.MouseAndKeyboard)
					{
						GUIscaleform.CallFunction("SET_CURSOR", Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 239), Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 240));
					}
					if (MoveTimer < Game.GameTime)
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_MOUSE_INPUT");
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 239 }));
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 240 }));
						GUIscaleform.CallFunction("SET_MOUSE_INPUT", Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 239 }), Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 240 }));
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 32) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 172))
						{
							MoveTimer = Game.GameTime + 200;
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 8);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 33) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 173))
						{
							MoveTimer = Game.GameTime + 200;
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 9);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174))
						{
							MoveTimer = Game.GameTime + 200;
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 10);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175))
						{
							MoveTimer = Game.GameTime + 200;
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 11);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
					}
					if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237) || Game.IsControlJustPressed(Control.FrontendAccept))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT_SELECT");
						uLocal_70 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD_RETURN_VALUE, hackingScalwform);
					}
					else if (Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 201) || Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 2, 237) || Game.IsControlJustPressed(Control.FrontendAccept))
					{
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT_SELECT");
						uLocal_70 = Function.Call<int>(Hash.END_SCALEFORM_MOVIE_METHOD, hackingScalwform);
					}
					if (Function.Call<bool>(Hash.IS_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_READY, uLocal_70))
					{
						FeedBackNumber = Function.Call<int>(Hash.GET_SCALEFORM_MOVIE_METHOD_RETURN_VALUE_INT, uLocal_70);
					}
					if (LastFeedBackNumber != FeedBackNumber && FeedBackNumber > 0)
					{
						LastFeedBackNumber = FeedBackNumber;
					}
					if (HackToComplete == 1 && IPHackDone)
					{
						HackFinishedID = 1;
						Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "unlocked_bleep", Game.Player.Character, "HACKING_DOOR_UNLOCK_SOUNDS", 0, 0);
						HackingSuccess = true;
						ActiveHack = true;
						HackDone = true;
						UnloadLoadehacker();
						HackFinishedID = 1;
					}
					if (HackToComplete == 2 && BruteForcehackDone)
					{
						HackFinishedID = 1;
						Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "unlocked_bleep", Game.Player.Character, "HACKING_DOOR_UNLOCK_SOUNDS", 0, 0);
						HackingSuccess = true;
						ActiveHack = true;
						HackDone = true;
						UnloadLoadehacker();
						HackFinishedID = 1;
					}
					if (HackToComplete == 3 && IPHackDone)
					{
						bool bruteForcehackDone = BruteForcehackDone;
					}
					switch (Hacking_Index)
					{
					case 0:
						TRYDL = true;
						if (!BruteforceStartInstantly)
						{
							Anim_Timer = Game.GameTime + 3000;
						}
						if (BruteforceStartInstantly)
						{
							Anim_Timer = Game.GameTime + 1;
						}
						Hacking_Index = 10;
						break;
					case 10:
						Hacking_Index = 50;
						DrawHackScaleform = true;
						break;
					case 50:
					{
						if (Function.Call<bool>(Hash.GET_IS_WIDESCREEN))
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "ADD_PROGRAM", 7f, 4f, -1f, -1f, -1f, "MyComputer", 0, 0, 0, 0);
						}
						else
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "ADD_PROGRAM", 8f, 4f, -1f, -1f, -1f, "MyComputer", 0, 0, 0, 0);
						}
						if (BruteForceBackgroundID < 0)
						{
							BruteForceBackgroundID = 3;
						}
						if (BruteForceBackgroundID > 5)
						{
							BruteForceBackgroundID = 3;
						}
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_BACKGROUND", (float)BruteForceBackgroundID, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						new Random().Next(0, 14);
						int num86 = 1;
						int num87 = 2;
						int num88 = 3;
						int num89 = 4;
						int num90 = 5;
						int num91 = 6;
						int num92 = 7;
						int num93 = 8;
						int num94 = 9;
						int num95 = 10;
						int num96 = 11;
						int num97 = 12;
						int num98 = 13;
						int num99 = 14;
						Random random7 = new Random();
						IList<string> list7 = new List<string>();
						list7.Add("MRRYWTHR");
						list7.Add("INSURGNT");
						list7.Add("STOCKADE");
						list7.Add("VENTURAS");
						list7.Add("ALAMOSEA");
						list7.Add("MCHILIAD");
						list7.Add("BRGRSHOT");
						list7.Add("MEGLODON");
						list7.Add("ZENTORNO");
						list7.Add("BUZZARDS");
						list7.Add("REDDEADR");
						list7.Add("VANGELCO");
						list7.Add("BUGSTARS");
						list7.Add("VANIUNIC");
						list7.Add("ROCKSTAR");
						list7.Add("ROCKFORD");
						list7.Add("CLIFFORD");
						list7.Add("VINEWOOD");
						list7.Add("VICECITY");
						list7.Add("FLEECABK");
						list7.Add("CARGOBOB");
						list7.Add("NIKOBELI");
						list7.Add("VESPUCCI");
						list7.Add("SUBURBAN");
						list7.Add("PONSONBY");
						list7.Add("PROLAPSS");
						list7.Add("LOSANTOS");
						list7.Add("LOWRIDER");
						list7.Add("MICHAELT");
						list7.Add("THXSLICK");
						list7.Add("GOPOSTAL");
						list7.Add("THNKROOT");
						list7.Add("THXEDDLM");
						list7.Add("GTAONLNE");
						list7.Add("TOSECURE");
						list7.Add("SHARKCRD");
						list7.Add("PASSWORD");
						list7.Add("SECUSERV");
						list7.Add("SECURITY");
						list7.Add("TRUFFADE");
						list7.Add("GRUPPE_6");
						BruteforcePassword = list7[random7.Next(list7.Count)];
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_STRING, hackingScalwform, "SET_ROULETTE_WORD", BruteforcePassword, 0, 0, 0, 0);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_LABELS");
						Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_1");
						Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
						Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_2");
						Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
						Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_3");
						Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
						Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_4");
						Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
						Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_5");
						Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
						Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "H_ICON_6");
						Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
						Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_SPEED", 10f, -1082130432, -1082130432, -1082130432, -1082130432);
						float num82 = Function.Call<float>(Hash.GET_RANDOM_FLOAT_IN_RANGE, 80f, 100f);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 0f, num82, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 1f, num82, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 2f, num82, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 3f, num82, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 4f, num82, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 5f, num82, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 6f, num82, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COLUMN_SPEED", 7f, num82, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "RUN_PROGRAM", 6f, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 2f, -1082130432, -1082130432, -1082130432, -1082130432);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, 59f, 0f, -1082130432, -1082130432);
						if (PlayBoth)
						{
							TRYDL = true;
							Hacking_Index = 100;
							if (HackToComplete == 1)
							{
								new Scaleform("Hacking_PC").CallFunction("SET_ROULETTE_WORD", BruteforcePassword);
								Hacking_Index = 100;
							}
							if (HackToComplete == 2)
							{
								new Scaleform("Hacking_PC").CallFunction("SET_ROULETTE_WORD", BruteforcePassword);
								Hacking_Index = 500;
							}
							if (HackToComplete == 3)
							{
								new Scaleform("Hacking_PC").CallFunction("SET_ROULETTE_WORD", BruteforcePassword);
								Hacking_Index = 100;
							}
						}
						if (!PlayBoth)
						{
							Hacking_Index = 500;
						}
						break;
					}
					case 100:
						GUIscaleform.CallFunction("SET_MOUSE_INPUT", Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 239 }), Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 240 }));
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 32) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 172))
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 8);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 33) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 173))
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 9);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174))
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 10);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175))
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 11);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (FeedBackNumber == 82 && !TRYDL)
						{
							new Scaleform("Hacking_PC").CallFunction("SET_ROULETTE_WORD", BruteforcePassword);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 0f, -1082130432, -1082130432, -1082130432, -1082130432);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, 60f, 0f, -1082130432, -1082130432);
							MyHackingTimer = new VariableTimer(60000);
							MyHackingTimer.Start();
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
							IPSound = Function.Call<int>(Hash.GET_SOUND_ID);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, IPSound, "HACKING_COUNTDOWN_IP_FIND", 0, true);
							FeedBackNumber = -1;
							Hacking_Index = 200;
						}
						if (FeedBackNumber == 83 && !TRYDL)
						{
							ErrorTimer = Game.GameTime + 500;
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 1f, -1f, -1f, -1f, -1f, "TRYHACK", 0, 0, 0, 0);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
							TRYDL = true;
							FeedBackNumber = -1;
						}
						if (FeedBackNumber == 80 && !TRYDL)
						{
							ErrorTimer = Game.GameTime + 500;
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 1f, -1f, -1f, -1f, -1f, "ACCESSD", 0, 0, 0, 0);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
							TRYDL = true;
							FeedBackNumber = -1;
						}
						if (ErrorTimer < Game.GameTime && TRYDL)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 0f, -1f, -1f, -1f, -1f, "ACCESSD", 0, 0, 0, 0);
							TRYDL = false;
						}
						if (FeedBackNumber == 4)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
							FeedBackNumber = -1;
						}
						if (FeedBackNumber == 81)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
							FeedBackNumber = -1;
						}
						break;
					case 200:
					{
						if (FeedBackNumber == 85 && !IpTry)
						{
							IpTryTimer = Game.GameTime + 500;
							if (Hacking_Lives > 0f)
							{
								Hacking_Lives -= 1f;
							}
							if (Hacking_Lives > 0f)
							{
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
							}
							if (Hacking_Lives == 0f)
							{
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_IP_OUTCOME", 1f, -1f, -1f, -1f, -1f, "LOSTIP", 0, 0, 0, 0);
								IpTryFailedTimer = Game.GameTime + 1000;
								Hacking_Index = 300;
								if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
								{
									Function.Call(Hash.STOP_SOUND, IPSound);
									IPSound = -1;
								}
							}
							IpTry = true;
							FeedBackNumber = -1;
						}
						if (FeedBackNumber == 84)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_IP_OUTCOME", 1f, -1f, -1f, -1f, -1f, "WINIP", 0, 0, 0, 0);
							if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
							{
								Function.Call(Hash.STOP_SOUND, IPSound);
								IPSound = -1;
							}
							Hacking_Index = 400;
						}
						if (IpTryTimer < Game.GameTime && IpTry)
						{
							IpTry = false;
						}
						if (MoveSoundTimer < Game.GameTime)
						{
							MoveSoundTimer = Game.GameTime + 200;
							if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 32) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 172)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
							}
							if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 33) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 173)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
							}
							if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
							}
							if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
							}
						}
						MyHackingTimer.Update(1f);
						TimeSpan timeSpan8 = TimeSpan.FromMilliseconds(MyHackingTimer.Counter);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, (float)timeSpan8.Seconds, (float)timeSpan8.Milliseconds, -1082130432, -1082130432);
						if ((float)timeSpan8.Seconds < 1f && (float)timeSpan8.Milliseconds < 1f)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_IP_OUTCOME", 1f, -1f, -1f, -1f, -1f, "LOSTIP", 0, 0, 0, 0);
							IpTryFailedTimer = Game.GameTime + 1000;
							Hacking_Index = 300;
							if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
							{
								Function.Call(Hash.STOP_SOUND, IPSound);
								IPSound = -1;
							}
						}
						break;
					}
					case 300:
						new List<object>();
						foreach (Control value in Enum.GetValues(typeof(Control)))
						{
							if (Game.IsControlJustPressed(value))
							{
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 2f, -1082130432, -1082130432, -1082130432, -1082130432);
								Hacking_Index = 100;
								Hacking_Lives = 3f;
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							}
						}
						break;
					case 400:
						new List<object>();
						foreach (Control value2 in Enum.GetValues(typeof(Control)))
						{
							if (Game.IsControlJustPressed(value2))
							{
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 2f, -1082130432, -1082130432, -1082130432, -1082130432);
								IPHackDone = true;
								Hacking_Index = 500;
								Hacking_Lives = 3f;
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							}
						}
						break;
					case 500:
						GUIscaleform.CallFunction("SET_MOUSE_INPUT", Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 239 }), Function.Call<float>(Hash.GET_CONTROL_NORMAL, new InputArgument[2] { 0, 240 }));
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 32) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 172))
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 8);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 33) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 173))
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 9);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174))
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 10);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, -10.5f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175))
						{
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 11);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "MOVE_CURSOR");
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 10.5f);
							Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, 0f);
							Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
						}
						if (FeedBackNumber == 83)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 1f, -1082130432, -1082130432, -1082130432, -1082130432);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, 60f, 0f, -1082130432, -1082130432);
							MyHackingTimer = new VariableTimer(60000);
							MyHackingTimer.Start();
							IPSound = Function.Call<int>(Hash.GET_SOUND_ID);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, IPSound, "HACKING_COUNTDOWN_IP_FIND", 0, true);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
							FeedBackNumber = -1;
							Hacking_Index = 600;
							Hacking_Lives = 3f;
						}
						if (FeedBackNumber == 82 && !TRYDL)
						{
							ErrorTimer = Game.GameTime + 500;
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 1f, -1f, -1f, -1f, -1f, "TRYBRUTE", 0, 0, 0, 0);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
							TRYDL = true;
							FeedBackNumber = -1;
						}
						if (FeedBackNumber == 80 && !TRYDL)
						{
							ErrorTimer = Game.GameTime + 500;
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 1f, -1f, -1f, -1f, -1f, "ACCESSD", 0, 0, 0, 0);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
							TRYDL = true;
							FeedBackNumber = -1;
						}
						if (ErrorTimer < Game.GameTime && TRYDL)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "OPEN_ERROR_POPUP", 0f, -1f, -1f, -1f, -1f, "ACCESSD", 0, 0, 0, 0);
							TRYDL = false;
						}
						if (FeedBackNumber == 4)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
							FeedBackNumber = -1;
						}
						if (FeedBackNumber == 81)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
							FeedBackNumber = -1;
						}
						break;
					case 600:
					{
						if (FeedBackNumber == 87 && !IpTry)
						{
							if (Hacking_Lives > 0f)
							{
								Hacking_Lives -= 1f;
							}
							if (Hacking_Lives > 0f)
							{
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_BAD", 0, true);
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "RESET_ROULETTE");
							}
							if (Hacking_Lives == 0f)
							{
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
								Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_ROULETTE_OUTCOME", 1f, -1f, -1f, -1f, -1f, "LOSEBRUTE", 0, 0, 0, 0);
								IpTryFailedTimer = Game.GameTime + 1000;
								Hacking_Index = 700;
								if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
								{
									Function.Call(Hash.STOP_SOUND, IPSound);
									IPSound = -1;
								}
							}
							FeedBackNumber = -1;
						}
						if (FeedBackNumber == 86)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_ROULETTE_OUTCOME", 1f, -1f, -1f, -1f, -1f, "WINBRUTE", 0, 0, 0, 0);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Hack_Success", "DLC_GR_Steal_Railguns_Sounds", true);
							HackFinishedID = 1;
							Hacking_Index = 800;
							FeedBackNumber = -1;
						}
						if (FeedBackNumber == 92)
						{
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_CLICK_GOOD", 0, true);
							FeedBackNumber = -1;
						}
						if (MoveSoundTimer < Game.GameTime)
						{
							MoveSoundTimer = Game.GameTime + 200;
							if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 34) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 174)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
							}
							if ((Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 35) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, 2, 175)) && Function.Call<bool>(Hash.HAS_SOUND_FINISHED, MoveSound))
							{
								Function.Call(Hash.PLAY_SOUND_FRONTEND, MoveSound, "HACKING_MOVE_CURSOR", 0, true);
							}
						}
						MyHackingTimer.Update(1f);
						TimeSpan timeSpan7 = TimeSpan.FromMilliseconds(MyHackingTimer.Counter);
						Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_COUNTDOWN", 0f, (float)timeSpan7.Seconds, (float)timeSpan7.Milliseconds, -1082130432, -1082130432);
						if ((float)timeSpan7.Seconds < 1f && (float)timeSpan7.Milliseconds < 1f)
						{
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "HACKING_FAILURE", 0, true);
							Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER_AND_STRING, hackingScalwform, "SET_ROULETTE_OUTCOME", 1f, -1f, -1f, -1f, -1f, "LOSEBRUTE", 0, 0, 0, 0);
							IpTryFailedTimer = Game.GameTime + 1000;
							Hacking_Index = 700;
							if (!Function.Call<bool>(Hash.HAS_SOUND_FINISHED, IPSound))
							{
								Function.Call(Hash.STOP_SOUND, IPSound);
								IPSound = -1;
							}
						}
						break;
					}
					case 700:
						new List<object>();
						foreach (Control value3 in Enum.GetValues(typeof(Control)))
						{
							if (Game.IsControlJustPressed(value3))
							{
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD, hackingScalwform, "CLOSE_APP");
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "OPEN_APP", 1f, -1082130432, -1082130432, -1082130432, -1082130432);
								Hacking_Index = 600;
								Hacking_Lives = 3f;
								Function.Call(Hash.CALL_SCALEFORM_MOVIE_METHOD_WITH_NUMBER, hackingScalwform, "SET_LIVES", Hacking_Lives, -1082130432, -1082130432, -1082130432, -1082130432);
							}
						}
						break;
					case 800:
						new List<object>();
						foreach (Control value4 in Enum.GetValues(typeof(Control)))
						{
							if (Game.IsControlJustPressed(value4))
							{
								Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, "unlocked_bleep", Game.Player.Character, "HACKING_DOOR_UNLOCK_SOUNDS", 0, 0);
								HackingSuccess = true;
								ActiveHack = true;
								HackDone = true;
								UnloadLoadehacker();
								HackFinishedID = 1;
							}
						}
						break;
					}
					if (DrawHackScaleform)
					{
						Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 1);
						Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, hackingScalwform, 255, 255, 255, 255, 0);
						if (Hacking_Index == 100)
						{
							Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
							DisplayHelpTextThisFrame("to start the hack, select 'My Computer', 'External Device', HackConnect.exe\nPress " + ReturnButtonString(ExitKey) + " to Exit", 1);
						}
						if (Hacking_Index == 200)
						{
							Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
							DisplayHelpTextThisFrame("Select the correct IP from the changing list of numbers, ~INPUT_MOVE_UP_ONLY~ ~INPUT_MOVE_LEFT_ONLY~ ~INPUT_MOVE_DOWN_ONLY~ ~INPUT_MOVE_RIGHT_ONLY~ Move IP selection\nPress " + ReturnButtonString(ExitKey) + " to Exit", 1);
						}
						if (Hacking_Index == 400)
						{
							Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
							DisplayHelpTextThisFrame("to start/continue the hack, select 'My Computer', 'External Device', Bruteforce.exe\nPress " + ReturnButtonString(ExitKey) + " to Exit", 1);
						}
						if (Hacking_Index == 500)
						{
							Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
							DisplayHelpTextThisFrame("to start/continue the hack, select 'My Computer', 'External Device', Bruteforce.exe\nPress " + ReturnButtonString(ExitKey) + " to Exit", 1);
						}
						if (Hacking_Index == 600)
						{
							Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, 4);
							DisplayHelpTextThisFrame("Correctly freeze each column to reveal the password, use ~INPUT_MOVE_LEFT_ONLY~ ~INPUT_MOVE_RIGHT_ONLY~ to to change columns and press ~INPUT_FRONTEND_ACCEPT~ to select current colum.", 1);
						}
					}
					if (GUIscaleform != null)
					{
						if (Game.LastInputMethod == InputMethod.GamePad && GUIMT < Game.GameTime)
						{
							if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 32 }) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 172 }))
							{
								GUIMT = Game.GameTime + 2;
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_INPUT_EVENT");
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 8);
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, hackingScalwform, "SET_MOUSE_INPUT");
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, GUIPosX);
								if (GUIPosY > -10.5f)
								{
									GUIPosY -= 0.01f;
								}
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { GUIPosY });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							}
							if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 33 }) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 173 }))
							{
								GUIMT = Game.GameTime + 2;
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { hackingScalwform, "SET_INPUT_EVENT" });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 9 });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { hackingScalwform, "SET_MOUSE_INPUT" });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { GUIPosX });
								if (GUIPosY < 10.5f)
								{
									GUIPosY += 0.01f;
								}
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { GUIPosY });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							}
							if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 34 }) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 174 }))
							{
								GUIMT = Game.GameTime + 2;
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { hackingScalwform, "SET_INPUT_EVENT" });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 10 });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { hackingScalwform, "SET_MOUSE_INPUT" });
								if (GUIPosX > -10.5f)
								{
									GUIPosX -= 0.01f;
								}
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { GUIPosX });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { GUIPosY });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							}
							if (Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 35 }) || Function.Call<bool>(Hash.IS_CONTROL_PRESSED, new InputArgument[2] { 2, 175 }))
							{
								GUIMT = Game.GameTime + 2;
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { hackingScalwform, "SET_INPUT_EVENT" });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, new InputArgument[1] { 11 });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
								if (GUIPosX < 10.5f)
								{
									GUIPosX += 0.01f;
								}
								Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, new InputArgument[2] { hackingScalwform, "SET_MOUSE_INPUT" });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { GUIPosX });
								Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, new InputArgument[1] { GUIPosY });
								Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
							}
						}
						Function.Call(Hash.ENABLE_CONTROL_ACTION, new InputArgument[3] { 2, 201, 1 });
						Function.Call(Hash.ENABLE_CONTROL_ACTION, new InputArgument[3] { 2, 237, 1 });
					}
				}
			}
			if (CircuitHackOn)
			{
				if (Game.IsControlJustPressed(Control.Context))
				{
					HackFinishedID = 3;
					CircuitBreaker.CleanupCircuitMinigame();
					CircuitHackOn = false;
					CannotCallHackToggleTimeout = Game.GameTime + 1000;
				}
				Scaleform scaleform8 = new Scaleform("instructional_buttons");
				scaleform8.CallFunction("CLEAR_ALL");
				scaleform8.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
				scaleform8.CallFunction("CREATE_CONTAINER");
				scaleform8.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 32, 0), "Snake Up");
				scaleform8.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 33, 0), "Snake Down");
				scaleform8.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 35, 0), "Snake Next");
				scaleform8.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 34, 0), "Snake Back");
				if (!CircuitBreaker.BSC_MoveHead)
				{
					scaleform8.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 37, 0), "Start Snake");
				}
				scaleform8.CallFunction("SET_DATA_SLOT", 5, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 2, 51, 0), "Quit");
				scaleform8.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
				scaleform8.Render2D();
				Function.Call(Hash.DISPLAY_RADAR, false);
				IsRadarOff = true;
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 27, true);
				Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 19, true);
				CircuitBreaker.Update();
			}
		}

		public void onShutdown(object sender, EventArgs e)
		{
			StopBeamhackSounds();
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle, "CLEAN_UP_DATA");
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, AppTrackifyHandle, "CLOSE_APP");
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			AppTrackifyOn = false;
			AppTrackify = -1;
			AppTrackifyWasRunning = false;
			Delete_AppTrackify();
			DELETE_VIRUS();
			Function.Call(Hash.STOP_BINK_MOVIE, HackGUI);
			Function.Call(Hash.RELEASE_BINK_MOVIE, HackGUI);
		}
	}
}
