using System;
using GTA;
using GTA.Native;

namespace VehicleMechanics.Modules
{
	public class FuelHudModule
	{
		private bool _dictRequested;

		public void Update(Vehicle v)
		{
			if (Config.FuelHudEnabled && !(v == null) && v.Exists())
			{
				if (!_dictRequested)
				{
					Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "commonmenu", true);
					_dictRequested = true;
				}
				bool dictLoaded = Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "commonmenu");
				float num = 0f;
				try
				{
					num = v.HandlingData.PetrolTankVolume;
				}
				catch
				{
					num = 65f;
				}
				if (num <= 0f)
				{
					num = 65f;
				}
				float num2 = Math.Max(0f, Math.Min(1f, v.FuelLevel / num));
				float fuelHudX = Config.FuelHudX;
				float fuelHudY = Config.FuelHudY;
				float fuelHudWidth = Config.FuelHudWidth;
				float fuelHudHeight = Config.FuelHudHeight;
				float fuelHudRotation = Config.FuelHudRotation;
				bool fuelHudHorizontal = Config.FuelHudHorizontal;
				DrawBox(fuelHudX, fuelHudY, fuelHudWidth, fuelHudHeight, fuelHudRotation, 0, 0, 0, 160, dictLoaded);
				float num3 = (fuelHudHorizontal ? fuelHudWidth : fuelHudHeight);
				float num4 = num3 * num2;
				float num5 = (num3 - num4) / 2f;
				double num6 = (double)fuelHudRotation * Math.PI / 180.0;
				float num7;
				float num8;
				if (fuelHudHorizontal)
				{
					num7 = (0f - (float)Math.Cos(num6)) * num5;
					num8 = (0f - (float)Math.Sin(num6)) * num5;
				}
				else
				{
					num7 = (float)Math.Sin(num6) * num5;
					num8 = (float)Math.Cos(num6) * num5;
				}
				float x = fuelHudX + num7;
				float y = fuelHudY + num8;
				float w = (fuelHudHorizontal ? num4 : fuelHudWidth);
				float h = (fuelHudHorizontal ? fuelHudHeight : num4);
				int r;
				int g;
				int b;
				if (num2 >= 0.35f)
				{
					r = 255;
					g = 200;
					b = 0;
				}
				else if (num2 >= 0.15f)
				{
					float num9 = (0.35f - num2) / 0.2f;
					r = 255;
					g = (int)(200f - 90f * num9);
					b = 0;
				}
				else
				{
					float num10 = (0.15f - num2) / 0.15f;
					r = 255;
					g = (int)(110f - 90f * num10);
					b = (int)(20f * num10);
				}
				DrawBox(x, y, w, h, fuelHudRotation, r, g, b, 235, dictLoaded);
			}
		}

		private void DrawBox(float x, float y, float w, float h, float rot, int r, int g, int b, int a, bool dictLoaded)
		{
			if (Math.Abs(rot) < 0.01f || !dictLoaded)
			{
				Function.Call(Hash.DRAW_RECT, x, y, w, h, r, g, b, a, 0);
			}
			else
			{
				Function.Call(Hash.DRAW_SPRITE, "commonmenu", "gradient_bgd", x, y, w, h, rot, r, g, b, a, 0);
			}
		}
	}
}
