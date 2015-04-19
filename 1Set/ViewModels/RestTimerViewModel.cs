using System;
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

namespace Set.ViewModels
{
	public class RestTimerViewModel : BaseViewModel
	{
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

		protected bool _playSounds;
		public bool PlaySounds
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
					OnPropertyChanged("AutoStart");
				}
			}
		}

		protected bool _isActive;
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				if (_isActive != value)
				{
					_isActive = value;

					StartCommandVisible = !_isActive;
					PauseCommandVisible = _isActive;
					ResetCommandVisible = _isActive;

					OnPropertyChanged("IsActive");
				}
			}
		}

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

		protected bool _startCommandVisible;
		public bool StartCommandVisible
		{
			get
			{
				return _startCommandVisible;
			}
			set
			{
				if (_startCommandVisible != value)
				{
					_startCommandVisible = value;
					OnPropertyChanged("StartCommandVisible");
				}
			}
		}

		protected bool _pauseCommandVisible;
		public bool PauseCommandVisible
		{
			get
			{
				return _pauseCommandVisible;
			}
			set
			{
				if (_pauseCommandVisible != value)
				{
					_pauseCommandVisible = value;
					OnPropertyChanged("PauseCommandVisible");
				}
			}
		}

		protected bool _resetCommandVisible;
		public bool ResetCommandVisible
		{
			get
			{
				return _resetCommandVisible;
			}
			set
			{
				if (_resetCommandVisible != value)
				{
					_resetCommandVisible = value;
					OnPropertyChanged("ResetCommandVisible");
				}
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
		}

		public async Task Load()
		{
			AutoStart = App.Settings.RestTimerAutoStart;
			PlaySounds = App.Settings.RestTimerPlaySounds;
			TotalSeconds = App.Settings.RestTimerTotalSeconds;

			await OnResetCommand ();

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private bool Validate ()
		{
			return true;
		}

		protected override async Task OnSave () 
		{
			if (Validate ())
			{
				App.Settings.RestTimerAutoStart = AutoStart;
				App.Settings.RestTimerPlaySounds = PlaySounds;
				App.Settings.RestTimerTotalSeconds = TotalSeconds;
				App.SaveSettings();

				IsActive = false;

				await Navigation.PopAsync();
			}
		}

		protected bool OnTimer()
		{
			if (!IsActive)
				return false;
			
			if ((SecondsLeft - 1) < 0)
			{
				OnResetCommand ();
				return false;
			}

			SecondsLeft = SecondsLeft - 1;
			return IsActive;	
		}

		private async Task OnStartCommand()
		{
			IsActive = true;
			Device.StartTimer(TimeSpan.FromSeconds(1), () => {	return OnTimer(); });

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnPauseCommand()
		{
			IsActive = false;

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnResetCommand()
		{
			IsActive = false;
			SecondsLeft = TotalSeconds;

			StartCommandVisible = !_isActive;
			PauseCommandVisible = _isActive;
			ResetCommandVisible = _isActive;
			 
			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}
	}
}

