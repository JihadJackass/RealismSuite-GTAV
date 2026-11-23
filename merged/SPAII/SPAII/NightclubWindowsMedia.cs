using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace SPAII
{
	internal sealed class NightclubWindowsMedia : IDisposable, ILocalMusicPlayer
	{
		internal enum Playback
		{
			Opening,
			Playing,
			Ended,
			Failed
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__11_002D0
		{
			public int _0024VB_0024Local_volume;

			public float _0024VB_0024Local_pan;

			public NightclubWindowsMedia _0024VB_0024Me;

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				_0024VB_0024Me.player.Volume = (double)Math.Max(0, Math.Min(1000, _0024VB_0024Local_volume)) / 1000.0;
				_0024VB_0024Me.player.Balance = ((float.IsNaN(_0024VB_0024Local_pan) || float.IsInfinity(_0024VB_0024Local_pan)) ? 0f : Math.Max(-1f, Math.Min(1f, _0024VB_0024Local_pan)));
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__14_002D0
		{
			public Action _0024VB_0024Local_command;

			public NightclubWindowsMedia _0024VB_0024Me;

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				try
				{
					_0024VB_0024Local_command();
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					_0024VB_0024Me.Finish(Playback.Failed, ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__15_002D0
		{
			public bool _0024VB_0024Local_repeat;

			public NightclubWindowsMedia _0024VB_0024Me;

			[SpecialName]
			internal void _Lambda_0024__1(object sender, EventArgs args)
			{
				try
				{
					if (_0024VB_0024Local_repeat)
					{
						_0024VB_0024Me.player.Position = TimeSpan.Zero;
						_0024VB_0024Me.ApplyPauseRequest();
					}
					else
					{
						_0024VB_0024Me.Finish(Playback.Ended, null);
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					_0024VB_0024Me.Finish(Playback.Failed, ex2.Message);
					ProjectData.ClearProjectError();
				}
			}
		}

		[CompilerGenerated]
		internal sealed class _Closure_0024__8_002D0
		{
			public Uri _0024VB_0024Local_source;

			public bool _0024VB_0024Local_repeat;

			public NightclubWindowsMedia _0024VB_0024Me;

			[SpecialName]
			internal void _Lambda_0024__0()
			{
				_0024VB_0024Me.Run(_0024VB_0024Local_source, _0024VB_0024Local_repeat);
			}
		}

		private readonly object gate;

		private Dispatcher dispatcher;

		private bool closed;

		private bool pauseRequested;

		private Playback state;

		private string detail;

		private MediaPlayer player;

		internal NightclubWindowsMedia(string path, bool repeat)
		{
			_Closure_0024__8_002D0 closure_0024__8_002D = new _Closure_0024__8_002D0();
			closure_0024__8_002D._0024VB_0024Local_repeat = repeat;
			closure_0024__8_002D._0024VB_0024Me = this;
			gate = RuntimeHelpers.GetObjectValue(new object());
			closure_0024__8_002D._0024VB_0024Local_source = new Uri(path, UriKind.Absolute);
			Thread thread = new Thread(closure_0024__8_002D._Lambda_0024__0);
			thread.IsBackground = true;
			thread.Name = "SPA II local audio";
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
		}

		internal Playback GetPlayback(ref string failure)
		{
			object obj = gate;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				failure = detail;
				return state;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
		}

		private int ReadState(ref string failure)
		{
			return (int)GetPlayback(ref failure);
		}

		int ILocalMusicPlayer.ReadState(ref string failure)
		{
			//ILSpy generated this explicit interface implementation from .override directive in ReadState
			return this.ReadState(ref failure);
		}

		internal void SetLevels(int volume, float pan)
		{
			_Closure_0024__11_002D0 closure_0024__11_002D = new _Closure_0024__11_002D0();
			closure_0024__11_002D._0024VB_0024Me = this;
			closure_0024__11_002D._0024VB_0024Local_volume = volume;
			closure_0024__11_002D._0024VB_0024Local_pan = pan;
			Queue(closure_0024__11_002D._Lambda_0024__0);
		}

		void ILocalMusicPlayer.SetLevels(int volume, float pan)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetLevels
			this.SetLevels(volume, pan);
		}

		internal void SetPaused(bool value)
		{
			object obj = gate;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				if (closed || pauseRequested == value)
				{
					return;
				}
				pauseRequested = value;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
			Queue(ApplyPauseRequest);
		}

		void ILocalMusicPlayer.SetPaused(bool value)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetPaused
			this.SetPaused(value);
		}

		private void ApplyPauseRequest()
		{
			object obj = gate;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			bool flag;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				if (closed || state != Playback.Playing)
				{
					return;
				}
				flag = pauseRequested;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
			if (flag)
			{
				player.Pause();
			}
			else
			{
				player.Play();
			}
		}

		private void Queue(Action command)
		{
			_Closure_0024__14_002D0 closure_0024__14_002D = new _Closure_0024__14_002D0();
			closure_0024__14_002D._0024VB_0024Me = this;
			closure_0024__14_002D._0024VB_0024Local_command = command;
			object obj = gate;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				if (!closed && dispatcher != null && state == Playback.Playing)
				{
					dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(closure_0024__14_002D._Lambda_0024__0));
				}
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
		}

		private void Run(Uri source, bool repeat)
		{
			_Closure_0024__15_002D0 closure_0024__15_002D = new _Closure_0024__15_002D0();
			closure_0024__15_002D._0024VB_0024Me = this;
			closure_0024__15_002D._0024VB_0024Local_repeat = repeat;
			try
			{
				object obj = gate;
				ObjectFlowControl.CheckForSyncLockOnValueType(obj);
				bool lockTaken = false;
				try
				{
					Monitor.Enter(obj, ref lockTaken);
					if (closed)
					{
						return;
					}
					dispatcher = Dispatcher.CurrentDispatcher;
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(obj);
					}
				}
				player = new MediaPlayer
				{
					Volume = 0.0
				};
				player.MediaOpened += _Lambda_0024__15_002D0;
				player.MediaEnded += closure_0024__15_002D._Lambda_0024__1;
				player.MediaFailed += _Lambda_0024__15_002D2;
				player.Open(source);
				Dispatcher.Run();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Finish(Playback.Failed, ex2.Message);
				ProjectData.ClearProjectError();
			}
			finally
			{
				try
				{
					if (player != null)
					{
						player.Close();
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					Logger.Log("SPA II Windows media cleanup: " + ex4.Message);
					ProjectData.ClearProjectError();
				}
				finally
				{
					object obj2 = gate;
					ObjectFlowControl.CheckForSyncLockOnValueType(obj2);
					bool lockTaken2 = false;
					try
					{
						Monitor.Enter(obj2, ref lockTaken2);
						dispatcher = null;
					}
					finally
					{
						if (lockTaken2)
						{
							Monitor.Exit(obj2);
						}
					}
				}
			}
		}

		private void FinishOpening()
		{
			object obj = gate;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			bool flag;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				if (closed || state != 0)
				{
					return;
				}
				flag = pauseRequested;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
			if (!player.HasAudio)
			{
				Finish(Playback.Failed, "The file has no audio stream.");
				return;
			}
			if (!flag)
			{
				player.Play();
			}
			object obj2 = gate;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj2);
			bool lockTaken2 = false;
			bool flag2;
			try
			{
				Monitor.Enter(obj2, ref lockTaken2);
				if (closed || state != 0)
				{
					return;
				}
				state = Playback.Playing;
				flag2 = flag != pauseRequested;
			}
			finally
			{
				if (lockTaken2)
				{
					Monitor.Exit(obj2);
				}
			}
			if (flag2)
			{
				ApplyPauseRequest();
			}
		}

		private void Finish(Playback result, string message)
		{
			object obj = gate;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				state = result;
				detail = message;
				RequestShutdown();
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
		}

		public void Dispose()
		{
			object obj = gate;
			ObjectFlowControl.CheckForSyncLockOnValueType(obj);
			bool lockTaken = false;
			try
			{
				Monitor.Enter(obj, ref lockTaken);
				if (!closed)
				{
					closed = true;
					RequestShutdown();
				}
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(obj);
				}
			}
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		private void RequestShutdown()
		{
			if (dispatcher != null && !dispatcher.HasShutdownStarted)
			{
				dispatcher.BeginInvokeShutdown(DispatcherPriority.Send);
			}
		}

		[SpecialName]
		[CompilerGenerated]
		private void _Lambda_0024__15_002D0(object sender, EventArgs args)
		{
			try
			{
				FinishOpening();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Finish(Playback.Failed, ex2.Message);
				ProjectData.ClearProjectError();
			}
		}

		[SpecialName]
		[CompilerGenerated]
		private void _Lambda_0024__15_002D2(object sender, ExceptionEventArgs args)
		{
			Finish(Playback.Failed, args.ErrorException?.Message ?? "Windows could not decode the audio stream.");
		}
	}
}
