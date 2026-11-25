using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAO_DiamondCasino
{
	public class Zones : Script
	{
		public class PolygonFence
		{
			public enum PointContainment
			{
				Coincident,
				Outside,
				Inside
			}

			public List<Point> PointList = new List<Point>();

			public static double Tolerance = 20.0;

			public PolygonFence()
			{
			}

			public PolygonFence(List<Point> points)
			{
				foreach (Point point in points)
				{
					PointList.Add(point);
				}
			}

			public int Count()
			{
				return PointList.Count;
			}

			public PointContainment WithBoundary(Vector3 X)
			{
				Point point = new Point((int)X.X, (int)X.Y);
				int num = 0;
				int count = PointList.Count;
				for (int i = 0; i < count; i++)
				{
					int num2 = i + 1;
					if (num2 == count)
					{
						num2 = 0;
					}
					Point point2 = PointList[i];
					Point point3 = PointList[num2];
					int y = point2.Y;
					int y2 = point3.Y;
					int x = point2.X;
					int x2 = point3.X;
					if ((double)Math.Abs(x - x2) < Tolerance && (double)Math.Abs(y - y2) < Tolerance)
					{
						continue;
					}
					int num3 = Math.Min(y, y2);
					int num4 = Math.Max(y, y2);
					if (point.Y < num3 || point.Y > num4)
					{
						continue;
					}
					if ((double)Math.Abs(num3 - num4) < Tolerance)
					{
						int num5 = Math.Min(x, x2);
						int num6 = Math.Max(x, x2);
						if (point.X >= num5 && point.X <= num6)
						{
							return PointContainment.Coincident;
						}
						if (point.X < num5)
						{
							num++;
						}
					}
					else
					{
						int num7 = (x2 - x) * (point.Y - y) / (y2 - y) + x;
						if ((double)Math.Abs(num7 - point.X) <= Tolerance)
						{
							return PointContainment.Coincident;
						}
						if (point.X < num7)
						{
							num++;
						}
					}
				}
				if (((uint)num & (true ? 1u : 0u)) != 0)
				{
					return PointContainment.Inside;
				}
				return PointContainment.Outside;
			}

			public PointContainment WithBoundary(Point ptr)
			{
				int num = 0;
				int count = PointList.Count;
				for (int i = 0; i < count; i++)
				{
					int num2 = i + 1;
					if (num2 == count)
					{
						num2 = 0;
					}
					Point point = PointList[i];
					Point point2 = PointList[num2];
					int y = point.Y;
					int y2 = point2.Y;
					int x = point.X;
					int x2 = point2.X;
					if ((double)Math.Abs(x - x2) < Tolerance && (double)Math.Abs(y - y2) < Tolerance)
					{
						continue;
					}
					int num3 = Math.Min(y, y2);
					int num4 = Math.Max(y, y2);
					if (ptr.Y < num3 || ptr.Y > num4)
					{
						continue;
					}
					if ((double)Math.Abs(num3 - num4) < Tolerance)
					{
						int num5 = Math.Min(x, x2);
						int num6 = Math.Max(x, x2);
						if (ptr.X >= num5 && ptr.X <= num6)
						{
							return PointContainment.Coincident;
						}
						if (ptr.X < num5)
						{
							num++;
						}
					}
					else
					{
						int num7 = (x2 - x) * (ptr.Y - y) / (y2 - y) + x;
						if ((double)Math.Abs(num7 - ptr.X) <= Tolerance)
						{
							return PointContainment.Coincident;
						}
						if (ptr.X < num7)
						{
							num++;
						}
					}
				}
				if (((uint)num & (true ? 1u : 0u)) != 0)
				{
					return PointContainment.Inside;
				}
				return PointContainment.Outside;
			}

			public PointContainment WithBoundary(List<Point> polygon, Point ptr)
			{
				int num = 0;
				int count = polygon.Count;
				for (int i = 0; i < count; i++)
				{
					int num2 = i + 1;
					if (num2 == count)
					{
						num2 = 0;
					}
					Point point = polygon[i];
					Point point2 = polygon[num2];
					int y = point.Y;
					int y2 = point2.Y;
					int x = point.X;
					int x2 = point2.X;
					if ((double)Math.Abs(x - x2) < Tolerance && (double)Math.Abs(y - y2) < Tolerance)
					{
						continue;
					}
					int num3 = Math.Min(y, y2);
					int num4 = Math.Max(y, y2);
					if (ptr.Y < num3 || ptr.Y > num4)
					{
						continue;
					}
					if ((double)Math.Abs(num3 - num4) < Tolerance)
					{
						int num5 = Math.Min(x, x2);
						int num6 = Math.Max(x, x2);
						if (ptr.X >= num5 && ptr.X <= num6)
						{
							return PointContainment.Coincident;
						}
						if (ptr.X < num5)
						{
							num++;
						}
					}
					else
					{
						int num7 = (x2 - x) * (ptr.Y - y) / (y2 - y) + x;
						if ((double)Math.Abs(num7 - ptr.X) <= Tolerance)
						{
							return PointContainment.Coincident;
						}
						if (ptr.X < num7)
						{
							num++;
						}
					}
				}
				if (((uint)num & (true ? 1u : 0u)) != 0)
				{
					return PointContainment.Inside;
				}
				return PointContainment.Outside;
			}

			public bool IsWithinFence(Point p)
			{
				int num = Count();
				int index = num - 1;
				bool flag = false;
				for (int i = 0; i < num; i++)
				{
					if (((PointList[i].Y < p.Y && PointList[index].Y >= p.Y) || (PointList[index].Y < p.Y && PointList[i].Y >= p.Y)) && PointList[i].X + (p.Y - PointList[i].Y) / (PointList[index].Y - PointList[i].Y) * (PointList[index].X - PointList[i].X) < p.X)
					{
						flag = !flag;
					}
					index = i;
				}
				return flag;
			}
		}

		public class AreaZone
		{
			public PolygonFence Zone { get; set; }

			public AreaZone()
			{
			}

			public AreaZone(PolygonFence Z)
			{
				Zone = Z;
			}
		}

		public enum ZoneIn
		{
			None,
			MainHall,
			HallA,
			HallB,
			GamingArea,
			Penthouse,
			PenthouseTerrace
		}

		public static List<Vector3> ZonePos = new List<Vector3>();

		public static AreaZone CasinoZones = new AreaZone(new PolygonFence(new List<Point>()));

		public static AreaZone HorsebettingArea = new AreaZone(new PolygonFence(new List<Point>
		{
			new Point(1101, 249),
			new Point(1103, 251),
			new Point(1105, 253),
			new Point(1107, 255),
			new Point(1107, 258),
			new Point(1108, 260),
			new Point(1107, 262),
			new Point(1104, 264),
			new Point(1103, 263),
			new Point(1103, 263),
			new Point(1101, 265),
			new Point(1100, 266),
			new Point(1098, 267),
			new Point(1096, 267),
			new Point(1093, 267),
			new Point(1091, 265),
			new Point(1090, 264),
			new Point(1089, 262),
			new Point(1089, 259),
			new Point(1090, 257),
			new Point(1090, 256),
			new Point(1092, 254),
			new Point(1093, 253),
			new Point(1093, 253),
			new Point(1091, 251),
			new Point(1094, 248),
			new Point(1097, 248),
			new Point(1099, 248)
		}));

		public static AreaZone PenthouseTerraceZone = new AreaZone(new PolygonFence(new List<Point>
		{
			new Point(972, 87),
			new Point(965, 78),
			new Point(960, 78),
			new Point(951, 83),
			new Point(948, 84),
			new Point(946, 81),
			new Point(941, 72),
			new Point(937, 68),
			new Point(934, 65),
			new Point(929, 63),
			new Point(924, 63),
			new Point(920, 64),
			new Point(913, 68),
			new Point(911, 68),
			new Point(899, 50),
			new Point(898, 46),
			new Point(897, 42),
			new Point(897, 38),
			new Point(898, 35),
			new Point(900, 32),
			new Point(906, 28),
			new Point(908, 26),
			new Point(909, 23),
			new Point(909, 21),
			new Point(908, 18),
			new Point(902, 7),
			new Point(901, 5),
			new Point(902, 3),
			new Point(902, 1),
			new Point(904, -1),
			new Point(920, -11),
			new Point(921, -11),
			new Point(923, -10),
			new Point(929, -4),
			new Point(931, -2),
			new Point(934, -2),
			new Point(936, -2),
			new Point(938, -2),
			new Point(951, -10),
			new Point(954, -10),
			new Point(956, -11),
			new Point(959, -10),
			new Point(963, -9),
			new Point(965, -7),
			new Point(1006, 57),
			new Point(1007, 59),
			new Point(1007, 62),
			new Point(1007, 64),
			new Point(1007, 67),
			new Point(1005, 70),
			new Point(1004, 72),
			new Point(1002, 73),
			new Point(986, 83),
			new Point(984, 82),
			new Point(982, 81)
		}));

		public static AreaZone PenthouseZone = new AreaZone(new PolygonFence(new List<Point>
		{
			new Point(962, 72),
			new Point(970, 66),
			new Point(965, 59),
			new Point(936, 12),
			new Point(934, 10),
			new Point(933, 8),
			new Point(933, 7),
			new Point(933, 5),
			new Point(934, 4),
			new Point(934, 3),
			new Point(935, 2),
			new Point(936, 1),
			new Point(940, -1),
			new Point(942, -2),
			new Point(944, -3),
			new Point(946, -3),
			new Point(947, -2),
			new Point(949, -1),
			new Point(950, 0),
			new Point(951, 1),
			new Point(995, 71),
			new Point(971, 86)
		}));

		public static int ZonesSaved = 0;

		public static Point PlayerPoint;

		public static ZoneIn CurrentZone = ZoneIn.None;

		public static bool RunMethodNow()
		{
			return true;
		}

		public Zones()
		{
			if (_Debug.ZonesO != -1)
			{
				base.Tick += OnTick;
			}
		}

		public static Vector2 World3DToScreen2d(Vector3 pos)
		{
			OutputArgument outputArgument = new OutputArgument();
			OutputArgument outputArgument2 = new OutputArgument();
			Function.Call<bool>(Hash.GET_SCREEN_COORD_FROM_WORLD_COORD, pos.X, pos.Y, pos.Z, outputArgument, outputArgument2);
			return new Vector2(outputArgument.GetResult<float>(), outputArgument2.GetResult<float>());
		}

		public static void drawText(string text, float x, float y, float scale, int r, int g, int b)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
			Function.Call(Hash.SET_TEXT_COLOUR, r, g, b, 255);
			Function.Call(Hash.SET_TEXT_SCALE, 0f, scale);
			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y, 0.1);
		}

		public static void DrawLine(Vector3 From, Vector3 To, Vector3 RGB)
		{
			Function.Call(Hash.DRAW_LINE, From.X, From.Y, From.Z, To.X, To.Y, To.Z, RGB.X, RGB.Y, RGB.Z, 255);
		}

		public static void OnTick(object sender, EventArgs e)
		{
			if (!Class1.IsinInterior && Class1.GetDistance_VDIST2(Game.Player.Character.Position, Class1.MarkerEnter, Class1.GetDistSign.Greater, 200f))
			{
				_Debug.ZonesO = 0;
				return;
			}
			_Debug.ZonesO = 1;
			PlayerPoint = new Point((int)Game.Player.Character.Position.X, (int)Game.Player.Character.Position.Y);
			Vector3 position = Game.Player.Character.Position;
		}
	}
}
