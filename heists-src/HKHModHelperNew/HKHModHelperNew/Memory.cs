using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using GTA;
using GTA.UI;

namespace HKHModHelperNew
{
	public class Memory
	{
		public static class Time
		{
			public static float deltaTime;

			private static int length = 200;

			private static float[] buffer = new float[length + 1];

			public static float maxDeltaTime = 0f;

			public static void Update(float fps)
			{
				deltaTime = 1f / fps;
				for (int i = 0; i < length; i++)
				{
					if (i < length)
					{
						buffer[i] = buffer[i + 1];
					}
				}
				buffer[length] = deltaTime;
				maxDeltaTime = buffer.Max();
			}
		}

		public class MemoryPatcher
		{
			public string Name { get; set; }

			public int Status { get; set; }

			public ulong Address { get; set; }

			public ulong Length { get; set; }

			public byte[] Code { get; set; }

			public MemoryPatcher(string name, string pattern, string code, string mask = "")
			{
				Name = name;
				Length = (ulong)code.Length;
				Code = code.Select((char x) => (byte)x).ToArray();
				Status = 0;
				Address = FindPattern(pattern, mask);
				if (Address == 0L)
				{
					Status = -1;
				}
				else
				{
					Status = 1;
				}
			}

			public unsafe void Apply()
			{
				if (Status != 1)
				{
					return;
				}
				try
				{
					byte[] array = Code.Select((byte x) => x).ToArray();
					for (ulong num = 0uL; num < Length; num++)
					{
						array[num] = *(byte*)(num + Address);
						*(sbyte*)(num + Address) = (sbyte)Code[num];
					}
					Code = array;
					Status = 2;
				}
				catch (Exception)
				{
				}
			}

			public unsafe void Revert()
			{
				if (Status != 2)
				{
					return;
				}
				try
				{
					byte[] array = Code.Select((byte x) => x).ToArray();
					for (ulong num = 0uL; num < Length; num++)
					{
						array[num] = *(byte*)(num + Address);
						*(sbyte*)(num + Address) = (sbyte)Code[num];
					}
					Code = array;
					Status = 1;
				}
				catch (Exception)
				{
				}
			}
		}

		public static float FontSpacing = 0.75f;

		public static Dictionary<char, int> Chars = new Dictionary<char, int>();

		private static Process process;

		public static ulong baseAddress;

		private static ulong endAddress;

		private static Dictionary<int, int> instances = new Dictionary<int, int>();

		private static Dictionary<int, int> drawTimes = new Dictionary<int, int>();

		public static void DrawText(string text, bool fast, int level, float startX, float startY, float centerX, float centerY, float a, float r, float g, float b, float scale, bool center)
		{
			float num = (center ? (-0.5f * (float)(text.Length - 1) * FontSpacing) : 0f);
			foreach (char c in text)
			{
				if (c != ' ' && Chars.ContainsKey(c))
				{
					int num2 = Chars[c];
					if (num2 != 0)
					{
						DrawTexture(num2, fast, level, a, r, g, b, startX + num * 0.1f * scale, startY, 0.1f * scale, 0.1f * scale, 0f, centerX, centerY);
					}
				}
				num += FontSpacing;
			}
		}

		public static string PatternToString(string pattern, string mask = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < pattern.Length; i++)
			{
				if (mask.Length > 0 && mask[i] == '?')
				{
					stringBuilder.Append("?? ");
				}
				else
				{
					stringBuilder.Append(Convert.ToString((byte)pattern[i], 16).PadLeft(2, '0').ToUpper() + " ");
				}
			}
			return stringBuilder.ToString();
		}

		public unsafe static ulong FindPattern(string pattern, string mask = "")
		{
			if (mask.Length > 0 && pattern.Length != mask.Length)
			{
				return 0uL;
			}
			for (ulong num = baseAddress; num < endAddress; num++)
			{
				for (int i = 0; i < pattern.Length && (*(byte*)(num + (ulong)i) == pattern[i] || (mask.Length != 0 && mask[i] != 'x')); i++)
				{
					if (i == pattern.Length - 1)
					{
						return num;
					}
				}
			}
			return 0uL;
		}

		public static void Initialize()
		{
			process = Process.GetCurrentProcess();
			baseAddress = (ulong)(long)process.MainModule.BaseAddress;
			endAddress = (ulong)process.MainModule.ModuleMemorySize + baseAddress;
		}

		public static int CreateTextureDict(string fileName)
		{
			int num = CreateTexture(fileName);
			instances.Add(num, 0);
			drawTimes.Add(num, 0);
			return num;
		}

		[DllImport("ScriptHookV.dll", EntryPoint = "?createTexture@@YAHPEBD@Z")]
		public static extern int CreateTexture([MarshalAs(UnmanagedType.LPStr)] string filename);

		public static void DrawTexture(int id, bool fast, int level, float a, float r, float g, float b, float posX, float posY, float sizeX, float sizeY, float rotation, float centerX, float centerY)
		{
			int frameCount = Game.FrameCount;
			if (frameCount != drawTimes[id])
			{
				drawTimes[id] = frameCount;
				instances[id] = 0;
			}
			float num = Math.Max(0.1f, Game.TimeScale);
			int time = (fast ? ((int)(1000.0 * (double)Time.maxDeltaTime / (double)num + 12.0)) : ((int)(1000.0 * (double)Time.maxDeltaTime / (double)num + 100.0)));
			DrawTexture(id, instances[id]++, level, time, sizeX, sizeY, centerX, centerY, posX, posY, rotation, Screen.AspectRatio, r, g, b, a);
		}

		public static void CreateAndDrawTexture(string Tex, int id, int instance, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, float colorR, float colorG, float colorB, float colorA)
		{
			if (File.Exists(Environment.CurrentDirectory + Tex))
			{
				if (id == 0)
				{
					id = CreateTexture(Environment.CurrentDirectory + Tex);
				}
				DrawTexture(id, instance, level, time, sizeX, sizeY, centerX, centerY, posX, posY, rotation, scaleFactor, colorR, colorG, colorB, colorA);
			}
		}

		[DllImport("ScriptHookV.dll", EntryPoint = "?drawTexture@@YAXHHHHMMMMMMMMMMMM@Z")]
		public static extern void DrawTexture(int id, int instance, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, float colorR, float colorG, float colorB, float colorA);
	}
}
