using System;
using System.Collections.Generic;
using GTA;
using GTA.Native;

namespace SinglePlayerHeists.UtilsMisc
{
	internal class ManageCellphoneCall : Script
	{
		public class MultiDialoguePedWithVoice
		{
			public Ped SpeakerPed { get; set; }

			public string Speaker { get; set; }

			public MultiDialoguePedWithVoice()
			{
			}

			public MultiDialoguePedWithVoice(Ped SP, string S)
			{
				Speaker = S;
				SpeakerPed = SP;
			}
		}

		public class MultiDialogueConversationLine
		{
			public int SpeakerID { get; set; }

			public Ped SpeakerPed { get; set; }

			public string Speaker { get; set; }

			public string Line { get; set; }

			public MultiDialogueConversationLine()
			{
			}

			public MultiDialogueConversationLine(int SID, Ped SP, string S, string L)
			{
				SpeakerID = SID;
				Speaker = S;
				Line = L;
				SpeakerPed = SP;
			}
		}

		public static bool ConvStarted;

		public static Ped ConvPed;

		public static bool DisplayPhoneCall;

		public static int PhoneCallTimer;

		public static bool VoiceStarted;

		public static string Speaker = "Lester";

		public static List<string> DialogueLines = new List<string>();

		public static string ContactName = "Lester";

		public static string ContactIcon = "Char_Lester";

		public static bool VoiceOverStarted;

		public static List<MultiDialogueConversationLine> MultiDialogueConversation = new List<MultiDialogueConversationLine>();

		public static List<Ped> ConvPeds = new List<Ped>();

		public int Handle
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

		public ManageCellphoneCall()
		{
			base.Tick += onTick;
		}

		public static Model RequestModel(string Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
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

		public static Model RequestModel(int Name)
		{
			Model result = new Model(Name);
			result.Request(2000);
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

		public static Model RequestModel(VehicleHash Name)
		{
			Model result = new Model(Name);
			result.Request(10000);
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
			result.Request(10000);
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

		public static int GetHandle()
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

		public static void DisplayCallUI(string contactName, string statusText = "CELL_211", string picName = "CELL_300")
		{
			string text = Function.Call<string>(Hash.GET_FILENAME_FOR_AUDIO_CONVERSATION, statusText);
			int handle = GetHandle();
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "SET_DATA_SLOT");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 4);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 0);
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 3);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, contactName, -1);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "CELL_2000");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, picName);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
			Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PHONE_NUMBER, text, -1);
			Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
			Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, "DISPLAY_VIEW");
			Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, 4);
			Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
		}

		public static void StartPhoneCall()
		{
			int num = 2;
			switch (Game.Player.Character.Model.Hash)
			{
			case 225514697:
				num = 0;
				break;
			case -1692214353:
				num = 2;
				break;
			case -1686040670:
				num = 1;
				break;
			default:
				num = 0;
				break;
			}
			Function.Call(Hash.CREATE_MOBILE_PHONE, num);
			PhoneCallTimer = Game.GameTime + 1000;
			Game.Player.Character.Task.UseMobilePhone();
			DisplayPhoneCall = true;
		}

		private int func_88(string Voiceline)
		{
			return func_86(Voiceline);
		}

		private int func_86(string sParam0)
		{
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "0"))
			{
				return 0;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "1"))
			{
				return 1;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "2"))
			{
				return 2;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "3"))
			{
				return 3;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "4"))
			{
				return 4;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "5"))
			{
				return 5;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "6"))
			{
				return 6;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "7"))
			{
				return 7;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "8"))
			{
				return 8;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "9"))
			{
				return 9;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "A"))
			{
				return 10;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "B"))
			{
				return 11;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "C"))
			{
				return 12;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "D"))
			{
				return 13;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "E"))
			{
				return 14;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "F"))
			{
				return 15;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "G"))
			{
				return 16;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "H"))
			{
				return 17;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "I"))
			{
				return 18;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "J"))
			{
				return 19;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "K"))
			{
				return 20;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "L"))
			{
				return 21;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "M"))
			{
				return 22;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "N"))
			{
				return 23;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "O"))
			{
				return 24;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "P"))
			{
				return 25;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "Q"))
			{
				return 26;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "R"))
			{
				return 27;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "S"))
			{
				return 28;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "T"))
			{
				return 29;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "U"))
			{
				return 30;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "V"))
			{
				return 31;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "W"))
			{
				return 32;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "X"))
			{
				return 33;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "Y"))
			{
				return 34;
			}
			if (Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "Z"))
			{
				return 35;
			}
			Function.Call<bool>(Hash.ARE_STRINGS_EQUAL, sParam0, "z");
			return -1;
		}

		public static void StartMultiConversation(List<MultiDialoguePedWithVoice> Speakers)
		{
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			for (int i = 0; i < Speakers.Count; i++)
			{
				MultiDialoguePedWithVoice multiDialoguePedWithVoice = Speakers[i];
				Function.Call(Hash.ADD_PED_TO_CONVERSATION, i, multiDialoguePedWithVoice.SpeakerPed, multiDialoguePedWithVoice.Speaker);
			}
			foreach (MultiDialogueConversationLine item in MultiDialogueConversation)
			{
				Function.Call(Hash.ADD_LINE_TO_CONVERSATION, item.SpeakerID, item.Line, item.Speaker, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			}
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
			VoiceOverStarted = true;
		}

		public static void StartMultiConversation_DeletePedsAfterConv(List<MultiDialoguePedWithVoice> Speakers)
		{
			foreach (Ped convPed in ConvPeds)
			{
				if (convPed != null)
				{
					convPed.Delete();
				}
			}
			ConvPeds.Clear();
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			for (int i = 0; i < Speakers.Count; i++)
			{
				MultiDialoguePedWithVoice multiDialoguePedWithVoice = Speakers[i];
				multiDialoguePedWithVoice.SpeakerPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
				multiDialoguePedWithVoice.SpeakerPed.Opacity = 0;
				multiDialoguePedWithVoice.SpeakerPed.IsInvincible = true;
				multiDialoguePedWithVoice.SpeakerPed.IsCollisionEnabled = false;
				multiDialoguePedWithVoice.SpeakerPed.AttachTo(Game.Player.Character);
				multiDialoguePedWithVoice.SpeakerPed.BlockPermanentEvents = true;
				Function.Call(Hash.ADD_PED_TO_CONVERSATION, i, multiDialoguePedWithVoice.SpeakerPed, multiDialoguePedWithVoice.Speaker);
				ConvPeds.Add(multiDialoguePedWithVoice.SpeakerPed);
			}
			foreach (MultiDialogueConversationLine item in MultiDialogueConversation)
			{
				Function.Call(Hash.ADD_LINE_TO_CONVERSATION, item.SpeakerID, item.Line, item.Speaker, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			}
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
			VoiceOverStarted = true;
		}

		public static void StartVoiceOver()
		{
			if (ConvPed != null)
			{
				ConvPed.Delete();
			}
			ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
			ConvPed.Opacity = 0;
			ConvPed.IsInvincible = true;
			ConvPed.IsCollisionEnabled = false;
			ConvPed.AttachTo(Game.Player.Character);
			ConvPed.BlockPermanentEvents = true;
			Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
			Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, Speaker);
			foreach (string dialogueLine in DialogueLines)
			{
				string localizedString = Game.GetLocalizedString(dialogueLine);
				Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, dialogueLine, Speaker, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
			}
			Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
			ConvStarted = true;
			VoiceOverStarted = true;
		}

		public void onTick(object sender, EventArgs e)
		{
			if (Game.IsControlJustPressed(Control.VehicleDuck))
			{
				return;
			}
			if (VoiceOverStarted && ConvStarted && !Function.Call<bool>(Hash.IS_SCRIPTED_CONVERSATION_ONGOING))
			{
				VoiceOverStarted = false;
				ConvStarted = false;
				if (ConvPed != null)
				{
					ConvPed.Delete();
				}
				foreach (Ped convPed in ConvPeds)
				{
					if (convPed != null)
					{
						convPed.Delete();
					}
				}
				ConvPeds.Clear();
			}
			if (Game.IsControlJustPressed(Control.Context))
			{
				return;
			}
			if (DisplayPhoneCall)
			{
				if (!VoiceStarted && Game.GameTime >= PhoneCallTimer)
				{
					if (ConvPed != null)
					{
						ConvPed.Delete();
					}
					VoiceStarted = true;
					ConvPed = GlobalHelper.CreatePed(RequestModel(PedHash.LesterCrest), Game.Player.Character.Position.Around(3f));
					ConvPed.Opacity = 0;
					ConvPed.IsInvincible = true;
					ConvPed.IsCollisionEnabled = false;
					ConvPed.AttachTo(Game.Player.Character);
					ConvPed.BlockPermanentEvents = true;
					Function.Call(Hash.CREATE_NEW_SCRIPTED_CONVERSATION);
					Function.Call(Hash.ADD_PED_TO_CONVERSATION, 0, ConvPed, Speaker);
					foreach (string dialogueLine in DialogueLines)
					{
						Function.Call(Hash.ADD_LINE_TO_CONVERSATION, 0, dialogueLine, Speaker, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1);
					}
					Function.Call(Hash.START_SCRIPT_CONVERSATION, 0, 0);
					ConvStarted = true;
				}
				DisplayCallUI(ContactName, "CELL_219", ContactIcon);
			}
			if (ConvStarted && !VoiceOverStarted && !Function.Call<bool>(Hash.IS_SCRIPTED_CONVERSATION_ONGOING))
			{
				if (DisplayPhoneCall)
				{
					VoiceStarted = false;
					DisplayPhoneCall = false;
					Function.Call(Hash.DESTROY_MOBILE_PHONE);
				}
				ConvStarted = false;
				if (ConvPed != null)
				{
					ConvPed.Delete();
				}
			}
		}
	}
}
