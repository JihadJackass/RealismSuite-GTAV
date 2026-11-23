using System.Drawing;
using GTA;
using GTA.Math;
using GTA.UI;
using LemonUI.Elements;

namespace SinglePlayerHeists
{
	public class CasinoPed : Script
	{
		public bool IsSittingComputerPed { get; set; }

		public bool IsSittingCouchPed { get; set; }

		public Ped Ai { get; set; }

		public Vector3 Position { get; set; }

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
			scaledTexture.Color = Color.Black;
			scaledTexture.Draw();
		}

		public void SetupPed(Ped AI, bool Chair, bool Computer, Vector3 Pos)
		{
			Ai = AI;
			IsSittingCouchPed = Chair;
			IsSittingComputerPed = Computer;
			Position = Pos;
		}
	}
}
