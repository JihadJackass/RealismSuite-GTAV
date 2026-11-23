using System;
using GTA;
using GTA.Native;

namespace CayoPericoHeistInSP
{
	public abstract class BaseScaleform
	{
		public int Handle { get; protected set; }

		public string Name { get; protected set; } = string.Empty;


		public bool HasLoaded => Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Handle);

		public BaseScaleform(string name)
		{
			Name = name;
		}

		public void CallFunction(string function, params object[] args)
		{
			if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, Handle, function))
			{
				PushArgsInternal(args);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			}
		}

		public static bool CallFunctionFrontend(string function, params object[] args)
		{
			if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND, function))
			{
				PushArgsInternal(args);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				return true;
			}
			return false;
		}

		public static bool CallFunctionFrontendHeader(string function, params object[] args)
		{
			if (Function.Call<bool>(Hash.BEGIN_SCALEFORM_MOVIE_METHOD_ON_FRONTEND_HEADER, function))
			{
				PushArgsInternal(args);
				Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
				return true;
			}
			return false;
		}

		private static void PushArgsInternal(params object[] args)
		{
			foreach (object obj in args)
			{
				if (obj is int)
				{
					int num = (int)obj;
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, num);
					continue;
				}
				if (obj is float)
				{
					float num2 = (float)obj;
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, num2);
					continue;
				}
				if (obj is bool)
				{
					bool flag = (bool)obj;
					Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, flag);
					continue;
				}
				string text = obj as string;
				if (text != null)
				{
					if (text.Length > 99)
					{
						Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_LITERAL_STRING, text);
						continue;
					}
					Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
					Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
				}
			}
		}

		public void Load(int timeout = 2000)
		{
			int gameTime = Game.GameTime;
			Handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, Name);
			while (!HasLoaded)
			{
				if (Game.GameTime - gameTime > timeout)
				{
					throw new TimeoutException($"ERROR: Scaleform '{Name}' failed to load within {timeout}ms.");
				}
				Script.Wait(0);
			}
		}

		public unsafe void Dispose()
		{
			if (HasLoaded)
			{
				int handle = Handle;
				Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &handle);
				Handle = 0;
			}
		}

		public void DrawFullscreen()
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Handle, 255, 255, 255, 255, 0);
		}

		public void DrawFullscreenMasked(BaseScaleform scaleform)
		{
			Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, Handle, scaleform.Handle, 255, 255, 255, 255);
		}
	}
}
