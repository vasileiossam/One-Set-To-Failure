﻿using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Collections.Generic;
using Set.Resx;
using Xamarin.Forms;
using Toasts.Forms.Plugin.Abstractions;
using Set.Localization;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Set
{
	public enum RestTimerStates 
	{
		Paused,
		Running,
		Editing
	};
}

namespace Set.ViewModels
{
	public class RestTimerViewModel : BaseViewModel
	{
		protected RestTimerStates _state;
		public RestTimerStates State
		{
			get
			{
				return _state;
			}
			set
			{
				if (_state != value)
				{
					_state = value;
					OnPropertyChanged ("State");
				}
			}
		}

		protected int _totalSeconds;
		public int TotalSeconds
		{
			get
			{
				return _totalSeconds;
			}
			set
			{
				if (_totalSeconds != value)
				{
					_totalSeconds = value;
					Save ();
					OnPropertyChanged ("TotalSeconds");
				}
			}
		}

		protected int _secondsLeft;
		public int SecondsLeft
		{
			get
			{
				return _secondsLeft;
			}
			set
			{
				if (_secondsLeft != value)
				{
					_secondsLeft = value;
					OnPropertyChanged ("SecondsLeft");
				}
			}
		}

		protected bool? _playSounds;
		public bool? PlaySounds
		{
			get
			{
				return _playSounds;
			}
			set
			{
				if (_playSounds != value)
				{
					_playSounds = value;
					OnPropertyChanged("PlaySounds");

					if (_playSounds == true)
					{
						_playSoundsImage = "sound";
					}
					else
					{
						_playSoundsImage = "nosound";
					}
					OnPropertyChanged("PlaySoundsImage");
				}
			}
		}

		protected string _playSoundsImage;
		public string PlaySoundsImage
		{
			get
			{
				return _playSoundsImage;
			}
			set
			{
				if (_playSoundsImage != value)
				{
					_playSoundsImage = value;
					OnPropertyChanged("PlaySoundsImage");
				}
			}
		}

		protected bool _autoStart;
		public bool AutoStart
		{
			get
			{
				return _autoStart;
			}
			set
			{
				if (_autoStart != value)
				{
					_autoStart = value;
					Save ();
					OnPropertyChanged("AutoStart");
				}
			}
		}

		protected bool _canSave = false;

		#region commands
		protected ICommand _startCommand;
		public ICommand StartCommand
		{
			get
			{
				return _startCommand;
			}
		}

		protected ICommand _pauseCommand;
		public ICommand PauseCommand
		{
			get
			{
				return _pauseCommand;
			}
		}

		protected ICommand _resetCommand;
		public ICommand ResetCommand
		{
			get
			{
				return _resetCommand;
			}
		}

		protected ICommand _playSoundsCommand;
		public ICommand PlaySoundsCommand
		{
			get
			{
				return _playSoundsCommand;
			}
		}

		protected ICommand _editingModeCommand;
		public ICommand EditingModeCommand
		{
			get
			{
				return _editingModeCommand;
			}
		}

		#endregion

		public RestTimerViewModel()
			: base()
		{
			Title = AppResources.RestTimerTitle;
		
			_startCommand = new Command (async() => { await OnStartCommand(); });
			_pauseCommand = new Command (async() => { await OnPauseCommand(); });
			_resetCommand = new Command (async() => { await OnResetCommand(); });

			_playSoundsCommand  = new Command (() => { 
				PlaySounds = !PlaySounds; 
				Save();
			});

			_editingModeCommand  = new Command (() => { 
				State = RestTimerStates.Editing;
			});
		}

		public async Task Load()
		{
			_canSave = false;
			AutoStart = App.Settings.RestTimerAutoStart;
			PlaySounds = App.Settings.RestTimerPlaySounds;
			TotalSeconds = App.Settings.RestTimerTotalSeconds;

			await OnResetCommand ();

			_canSave = true;

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		protected void Save()
		{
			if (!_canSave)
				return;
			App.Settings.RestTimerAutoStart = AutoStart;
			App.Settings.RestTimerPlaySounds = PlaySounds ?? false;
			App.Settings.RestTimerTotalSeconds = TotalSeconds;
			App.SaveSettings ();
		}

		protected bool OnTimer()
		{
			if (State != RestTimerStates.Running)
				return false;
			
			if ((SecondsLeft - 1) < 0)
			{
				OnResetCommand ();
				return false;
			}

			SecondsLeft = SecondsLeft - 1;
			return State == RestTimerStates.Running;	
		}

		private async Task OnStartCommand()
		{	
			if (State == RestTimerStates.Editing)
			{
				SecondsLeft = TotalSeconds;
			}

			State = RestTimerStates.Running;
			Device.StartTimer(TimeSpan.FromSeconds(1), () => {	return OnTimer(); });

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnPauseCommand()
		{
			State = RestTimerStates.Paused;

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnResetCommand()
		{
			State = RestTimerStates.Paused;
			SecondsLeft = TotalSeconds;
			 
			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}
	}
}
