using System;
using System.Drawing;
using GTA;
using GTA.Math;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;

namespace HKHModHelperNew
{
	public class MissionScreen : Script
	{
		public bool Fail;

		public bool Pass;

		public string m;

		public int p;

		public string r;

		public int Timer;

		public float GetDistance(Vector3 _1, Vector3 _2)
		{
			return _1.DistanceTo(_2);
		}

		public void MissionPassed(string MissionName, int Payout, string Reason)
		{
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("mission passed", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(x, 230), 0.5f, Color.White, GTA.UI.Font.ChaletComprimeCologne);
			TEXT("Payout : $" + Payout.ToString("N"), new Point(x, 300), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TEXT(Reason, new Point(x, 330), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			new Scaleform("instructional_buttons").Render2D();
		}

		public void MissionPassed(string MissionName, int Payout)
		{
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("mission passed", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(x, 230), 0.5f, Color.White, GTA.UI.Font.ChaletComprimeCologne);
			TEXT("Payout : $" + Payout.ToString("N"), new Point(x, 300), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			new Scaleform("instructional_buttons").Render2D();
		}

		public void MissionPassed2(string MissionName, int Payout, int Payout2, float loss)
		{
			Fail = false;
			Pass = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 10), new Size(Convert.ToInt32(absoluteX), 550), 0f, Color.FromArgb(200, 255, 255, 255));
			TEXT("mission passed", new Point(x, 100), 2.5f, Color.FromArgb(255, 199, 168, 87), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(x, 230), 0.5f, Color.White, GTA.UI.Font.ChaletComprimeCologne);
			TEXT("Payout : $" + Payout.ToString("N"), new Point(x, 300), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TEXT("Bonus : $" + Payout2.ToString("N"), new Point(x, 330), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			TEXT("Loss : " + loss + "%", new Point(x, 360), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			new Scaleform("instructional_buttons").Render2D();
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

		public void TEXTURE(string Textdict, string Textname, PointF Pos, SizeF Size, float Rot, Color Colour)
		{
			ScaledTexture scaledTexture = new ScaledTexture(Textdict, Textname);
			scaledTexture.Dictionary = Textdict;
			scaledTexture.Texture = Textname;
			scaledTexture.Position = Pos;
			scaledTexture.Size = Size;
			scaledTexture.Color = Colour;
			scaledTexture.Draw();
		}

		public void MissionFailed(string Reason, string MissionName)
		{
			Pass = false;
			Fail = true;
			float absoluteX;
			float absoluteY;
			LemonUI.Screen.ToAbsolute(1f, 1f, out absoluteX, out absoluteY);
			int x = Convert.ToInt32(absoluteX / 2f);
			TEXTURE("mpentry", "mp_modenotselected_gradient", new Point(0, 30), new Size(Convert.ToInt32(absoluteX), 450), 0f, Color.FromArgb(230, 255, 255, 255));
			TEXT("mission failed", new Point(x, 100), 2.5f, Color.FromArgb(255, 148, 27, 46), GTA.UI.Font.Pricedown);
			TEXT(MissionName, new Point(x, 230), 0.5f, Color.White, GTA.UI.Font.ChaletComprimeCologne);
			TEXT("Reason : " + Reason, new Point(x, 280), 0.5f, Color.White, GTA.UI.Font.ChaletLondon);
			new Scaleform("instructional_buttons").Render2D();
		}

		public void SetPass2(string MissionName, int Payout, string Reason)
		{
			Class1.LivesSystemRunning = false;
			Class1.TimeLeftSystemRunning = false;
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300;
			m = MissionName;
			p = Payout;
			r = Reason;
			Pass = true;
			for (int i = 0; i < 100; i++)
			{
				if (Fail)
				{
					MissionFailed(r, m);
				}
				if (Pass)
				{
					MissionPassed(m, p, r);
				}
				Script.Wait(1);
			}
		}

		public void SetFail(string MissionName, int Payout, string Reason)
		{
			Class1.LivesSystemRunning = false;
			Class1.TimeLeftSystemRunning = false;
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Bed", "WastedSounds");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300;
			m = MissionName;
			p = Payout;
			r = Reason;
			Fail = true;
			for (int i = 0; i < 100; i++)
			{
				if (Fail)
				{
					MissionFailed(r, m);
				}
				if (Pass)
				{
					MissionPassed(m, p);
				}
				Script.Wait(1);
			}
		}

		public void SetPass(string MissionName, string unk, int Payout, string Reason)
		{
			Class1.LivesSystemRunning = false;
			Class1.TimeLeftSystemRunning = false;
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300;
			m = MissionName;
			p = Payout;
			r = Reason;
			Pass = true;
			for (int i = 0; i < 100; i++)
			{
				if (Fail)
				{
					MissionFailed(r, m);
				}
				if (Pass)
				{
					MissionPassed(m, p);
				}
				Script.Wait(1);
			}
		}

		public void SetPass(string MissionName, int Payout, string Reason)
		{
			Class1.LivesSystemRunning = false;
			Class1.TimeLeftSystemRunning = false;
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300;
			m = MissionName;
			p = Payout;
			r = Reason;
			Pass = true;
			for (int i = 0; i < 100; i++)
			{
				if (Fail)
				{
					MissionFailed(r, m);
				}
				if (Pass)
				{
					MissionPassed(m, p);
				}
				Script.Wait(1);
			}
		}

		public void SetPass2(string MissionName, int Payout, int Pay2, float Loss, string Reason)
		{
			Class1.LivesSystemRunning = false;
			Class1.TimeLeftSystemRunning = false;
			Audio.SetAudioFlag(AudioFlags.LoadMPData, true);
			Audio.PlaySoundFrontend("Mission_Pass_Notify", "DLC_HEISTS_GENERAL_FRONTEND_SOUNDS");
			Audio.SetAudioFlag(AudioFlags.LoadMPData, false);
			Timer = 300;
			m = MissionName;
			p = Payout;
			r = Reason;
			Pass = true;
			for (int i = 0; i < 100; i++)
			{
				if (Fail)
				{
					MissionFailed(r, m);
				}
				if (Pass)
				{
					MissionPassed2(m, p, Pay2, Loss);
				}
				Script.Wait(1);
			}
		}
	}
}
