using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GTA;

namespace SinglePlayerHeists.UtilsMisc
{
	public static class Memory
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

		private static ulong dmfvOffset;

		private static ulong gearRatiosOffset;

		public static ulong wheelsOffset;

		private static ulong wheelCountOffset;

		public static int wheelAngularVelocityOffset;

		public static int wheelAngleOffset;

		public static int wheelsPtrOffset = 0;

		public static int numWheelsOffset;

		public static int steeringAngleOffset;

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

		public unsafe static int GetNumWheels(Vehicle handle)
		{
			if (numWheelsOffset == 0)
			{
				return 0;
			}
			IntPtr memoryAddress = handle.MemoryAddress;
			if (memoryAddress == (IntPtr)0)
			{
				return 0;
			}
			return *(int*)(void*)(memoryAddress + numWheelsOffset);
		}

		public unsafe static float GetSteeringAngle(Vehicle handle)
		{
			if (steeringAngleOffset == 0)
			{
				return 0f;
			}
			IntPtr memoryAddress = handle.MemoryAddress;
			return *(float*)(void*)(memoryAddress + steeringAngleOffset);
		}

		public unsafe static void SetSteeringAngle(Vehicle handle, float value)
		{
			if (steeringAngleOffset != 0)
			{
				IntPtr memoryAddress = handle.MemoryAddress;
				*(float*)(void*)(memoryAddress + steeringAngleOffset) = value;
			}
		}

		public unsafe static void Initialize()
		{
			process = Process.GetCurrentProcess();
			baseAddress = (ulong)(long)process.MainModule.BaseAddress;
			endAddress = (ulong)process.MainModule.ModuleMemorySize + baseAddress;
			if (Process.GetCurrentProcess().ProcessName == "GTA5_Enhanced")
			{
				ulong num = FindPattern("E\u000fWÉó\u000f\u0011\u0083`\u0001\0\0ó\u000f\\", "xxx?xxx???xxxxx");
				wheelAngularVelocityOffset = ((num != 0L) ? (*(int*)(num + 8)) : 0);
				wheelAngleOffset = ((num != 0L) ? (*(int*)(num + 8) + 8) : 0);
				num = FindPattern("4c 8b 89 ? ? ? ? 45 31 d2 0f 1f");
				numWheelsOffset = *(int*)(num - 9);
				wheelsPtrOffset = numWheelsOffset - 8;
				num = FindPattern("0f 56 f9 f3 0f 11 be ? ? ? ? 48 8b 86");
				steeringAngleOffset = ((num != 0L) ? (*(int*)(num + 7) + 8) : 0);
			}
			else
			{
				ulong num2 = FindPattern("E\u000fWÉó\u000f\u0011\u0083`\u0001\0\0ó\u000f\\", "xxx?xxx???xxxxx");
				wheelAngularVelocityOffset = ((num2 != 0L) ? (*(int*)(num2 + 8)) : 0);
				wheelAngleOffset = ((num2 != 0L) ? (*(int*)(num2 + 8) + 8) : 0);
				num2 = FindPattern(";·H\v\0\0}\r", "xx????xx");
				wheelsPtrOffset = ((num2 != 0L) ? (*(int*)(num2 + 2) - 8) : 0);
				numWheelsOffset = ((num2 != 0L) ? (*(int*)(num2 + 2)) : 0);
				num2 = FindPattern("t\nó\u000f\u0011³\u001c\t\0\0ë%", "xxxxx?????xx");
				steeringAngleOffset = ((num2 != 0L) ? (*(int*)(num2 + 6) + 8) : 0);
			}
		}
	}
}
