using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BusinessV.Models;
using GTA;
using GTA.Native;

namespace BusinessV.World
{
	internal sealed class BusinessInputController
	{
		private readonly BusinessVSettings settings;

		private readonly Action<BusinessInputAction> dispatch;

		private readonly Func<bool> capturesInput;

		private readonly BusinessInputGate gate = new BusinessInputGate();

		private readonly HashSet<Keys> heldKeys = new HashSet<Keys>();

		public BusinessInputController(BusinessVSettings settings, Action<BusinessInputAction> dispatch, Func<bool> capturesInput)
		{
			this.settings = settings;
			this.dispatch = dispatch;
			this.capturesInput = capturesInput;
		}

		public void Tick()
		{
			if (!Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE))
			{
				if (capturesInput())
				{
					DisableMenuControls();
				}
				if (JustPressed(GTA.Control.FrontendCancel))
				{
					Send(BusinessInputAction.Cancel);
				}
				else if (JustPressed(GTA.Control.FrontendUp))
				{
					Send(BusinessInputAction.Up);
				}
				else if (JustPressed(GTA.Control.FrontendDown))
				{
					Send(BusinessInputAction.Down);
				}
				else if (JustPressed(GTA.Control.FrontendLeft))
				{
					Send(BusinessInputAction.Left);
				}
				else if (JustPressed(GTA.Control.FrontendRight))
				{
					Send(BusinessInputAction.Right);
				}
				else if (JustPressed(GTA.Control.FrontendAccept))
				{
					Send(BusinessInputAction.Accept);
				}
				else if (JustPressed(GTA.Control.Context))
				{
					Send(BusinessInputAction.Activate);
				}
			}
		}

		public void KeyDown(Keys key)
		{
			if (Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE) || !heldKeys.Add(key))
			{
				return;
			}
			if (key == settings.ParsedInteractionKey)
			{
				Send(BusinessInputAction.Activate);
				return;
			}
			switch (key)
			{
			case Keys.Return:
				Send(BusinessInputAction.Accept);
				break;
			case Keys.Back:
			case Keys.Escape:
				Send(BusinessInputAction.Cancel);
				break;
			case Keys.Up:
				Send(BusinessInputAction.Up);
				break;
			case Keys.Down:
				Send(BusinessInputAction.Down);
				break;
			case Keys.Left:
				Send(BusinessInputAction.Left);
				break;
			case Keys.Right:
				Send(BusinessInputAction.Right);
				break;
			}
		}

		public void KeyUp(Keys key)
		{
			heldKeys.Remove(key);
		}

		private void Send(BusinessInputAction action)
		{
			if (gate.TryAccept(Game.GameTime))
			{
				dispatch(action);
			}
		}

		private static bool JustPressed(GTA.Control control)
		{
			if (!Game.IsControlJustPressed(control))
			{
				return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, (int)control);
			}
			return true;
		}

		private static void DisableMenuControls()
		{
			Game.DisableControlThisFrame(GTA.Control.Context);
			Game.DisableControlThisFrame(GTA.Control.FrontendAccept);
			Game.DisableControlThisFrame(GTA.Control.FrontendCancel);
			Game.DisableControlThisFrame(GTA.Control.FrontendUp);
			Game.DisableControlThisFrame(GTA.Control.FrontendDown);
			Game.DisableControlThisFrame(GTA.Control.FrontendLeft);
			Game.DisableControlThisFrame(GTA.Control.FrontendRight);
			Game.DisableControlThisFrame(GTA.Control.FrontendPause);
			Game.DisableControlThisFrame(GTA.Control.FrontendPauseAlternate);
			Game.DisableControlThisFrame(GTA.Control.Phone);
			Game.DisableControlThisFrame(GTA.Control.PhoneUp);
			Game.DisableControlThisFrame(GTA.Control.PhoneDown);
			Game.DisableControlThisFrame(GTA.Control.PhoneLeft);
			Game.DisableControlThisFrame(GTA.Control.PhoneRight);
			Game.DisableControlThisFrame(GTA.Control.PhoneSelect);
			Game.DisableControlThisFrame(GTA.Control.PhoneCancel);
		}
	}
}
