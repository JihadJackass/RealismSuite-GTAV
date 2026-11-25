using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAO_DiamondCasino
{
	internal class EntityFlickerEffect : Script
	{
		public class FadeEffect
		{
			public int FinalAlpha { get; set; }

			public bool DoNotDelete { get; set; }

			public int Timeout { get; set; }

			public string VehicleID { get; set; }

			public Entity Entity { get; set; }

			public int StagesCompleted { get; set; }

			public int FadeCycle { get; set; }

			public int NextFadeTime { get; set; }

			public int MarkForDeletion { get; set; }

			public FadeEffect()
			{
			}

			public FadeEffect(Entity V, int unk)
			{
				Entity = V;
				StagesCompleted = 0;
				FadeCycle = 0;
				NextFadeTime = Game.GameTime + 1;
				MarkForDeletion = 1;
			}

			public FadeEffect(Entity V, bool donotdelete, int FA)
			{
				FinalAlpha = FA;
				Entity = V;
				DoNotDelete = donotdelete;
				StagesCompleted = 0;
				FadeCycle = 0;
				NextFadeTime = Game.GameTime + 1;
				MarkForDeletion = 1;
			}

			public FadeEffect(Entity V, bool donotdelete)
			{
				Entity = V;
				DoNotDelete = donotdelete;
				StagesCompleted = 0;
				FadeCycle = 0;
				NextFadeTime = Game.GameTime + 1;
				MarkForDeletion = 1;
			}

			public FadeEffect(Entity V)
			{
				Entity = V;
				DoNotDelete = false;
				StagesCompleted = 0;
				FadeCycle = 0;
				NextFadeTime = Game.GameTime + 1;
				MarkForDeletion = 1;
			}

			public FadeEffect(Entity V, string ID)
			{
				Entity = V;
				VehicleID = ID;
				StagesCompleted = 0;
				FadeCycle = 0;
				NextFadeTime = Game.GameTime + 1;
			}
		}

		public static List<FadeEffect> VehicleFadeEffects = new List<FadeEffect>();

		public EntityFlickerEffect()
		{
			base.Tick += Ontick;
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

		public void Ontick(object sender, EventArgs e)
		{
			if (VehicleFadeEffects.Count == 0)
			{
				return;
			}
			for (int i = 0; i < VehicleFadeEffects.Count; i++)
			{
				FadeEffect fadeEffect = VehicleFadeEffects[i];
				if (fadeEffect.Entity == null)
				{
					VehicleFadeEffects.RemoveAt(i);
					fadeEffect.StagesCompleted = 10;
				}
				if (!(fadeEffect.Entity != null))
				{
					continue;
				}
				if (fadeEffect.StagesCompleted != 3)
				{
					if (fadeEffect.FadeCycle == 0 && Game.GameTime >= fadeEffect.NextFadeTime)
					{
						fadeEffect.NextFadeTime = Game.GameTime + 1;
						if (fadeEffect.Entity.Opacity - 60 < 0)
						{
							fadeEffect.Entity.Opacity = 0;
							fadeEffect.FadeCycle = 1;
						}
						else
						{
							fadeEffect.Entity.Opacity -= 60;
						}
					}
					if (fadeEffect.FadeCycle == 1 && Game.GameTime >= fadeEffect.NextFadeTime)
					{
						fadeEffect.NextFadeTime = Game.GameTime + 1;
						if (fadeEffect.Entity.Opacity + 60 > 255)
						{
							fadeEffect.StagesCompleted++;
							fadeEffect.Entity.Opacity = 255;
							fadeEffect.FadeCycle = 0;
						}
						else
						{
							fadeEffect.Entity.Opacity += 60;
						}
					}
				}
				else
				{
					fadeEffect.Entity.Opacity = 255;
					fadeEffect.StagesCompleted = 10;
					if (!fadeEffect.DoNotDelete)
					{
						fadeEffect.Entity.Delete();
					}
					else if (fadeEffect.FinalAlpha != -1)
					{
						fadeEffect.Entity.Opacity = fadeEffect.FinalAlpha;
					}
					else
					{
						fadeEffect.Entity.Opacity = 0;
					}
					VehicleFadeEffects.RemoveAt(i);
				}
			}
		}
	}
}
