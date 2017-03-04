using System;
using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
    public enum RestTimerStates
    {
        Paused,
        Running,
        Editing
    }

    public class RestTimerViewModel : BaseViewModel, INavigationAware
    {
        #region private variables
        private ISoundService _soundService;
        #endregion

        #region properties
        public ProgressBar ProgressBar { get; set; }
		protected double _progressStep;

		protected RestTimerStates _state;
		public RestTimerStates State
		{
			get
			{
				return _state;
			}
			set
			{
			    if (_state == value) return;
			    _state = value;
			    OnPropertyChanged ("State");
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
			    if (_totalSeconds == value) return;
			    _totalSeconds = value;
			    Save ();
			    OnPropertyChanged ("TotalSeconds");
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
			    if (_secondsLeft == value) return;
			    _secondsLeft = value;
			    OnPropertyChanged ("SecondsLeft");
			}
		}

		protected bool? _playSounds;
		public bool? PlaySounds
		{
			get { return _playSounds; }
			set
			{
                SetProperty(ref _playSounds, value);

                _playSoundsImage = _playSounds == true ? "sound" : "nosound";
			    OnPropertyChanged("PlaySoundsImage");
			}
		}

		protected string _playSoundsImage;
		public string PlaySoundsImage
        {
            get { return _playSoundsImage; }
            set { SetProperty(ref _playSoundsImage, value); }
        }

        protected bool _autoStart;
		public bool AutoStart
		{
			get { return _autoStart; }
			set
			{
                SetProperty(ref _autoStart, value);
                Save ();
			}
		}

		protected bool _canSave;

		public string MotivationalQuoteImageFile 
		{
			get
			{
			    switch (App.Settings.ImagePackId)
			    {
			        case 1:
			        {
			            var r = new Random ();
			            int i = r.Next (1, 25);
			            return $"QFitness_{i}.png";
			        }
			        case 2:
			        {
			            var r = new Random ();
			            int i = r.Next (1, 20);
			            return $"QInspirational_{i}.png";
			        }
			    }
			    return string.Empty;
			}
		}

		public bool MotivationalQuoteImageVisible => App.Settings.CanShowImagePackInRestTimer;
        #endregion

        #region commands
        public ICommand StartCommand { get; set; }
		public ICommand PauseCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand PlaySoundsCommand { get; set; }
        public ICommand EditingModeCommand { get; set; }
        #endregion

        public RestTimerViewModel(ISoundService soundService)
        {
            _soundService = soundService;

            Title = AppResources.RestTimerTitle;
		
			StartCommand = new Command (async() => { await OnStartCommand(); });
			PauseCommand = new Command (async() => { await OnPauseCommand(); });
			ResetCommand = new Command (async() => { await OnResetCommand(); });

            PlaySoundsCommand  = new Command (() => { 
				PlaySounds = !PlaySounds; 
				Save();
			});

			EditingModeCommand  = new Command (() => { 
				State = RestTimerStates.Editing;
			});
		}

        #region private methods
        private void Save()
		{
			if (!_canSave)
				return;
			App.Settings.RestTimerAutoStart = AutoStart;
			App.Settings.RestTimerPlaySounds = PlaySounds ?? false;
			App.Settings.RestTimerTotalSeconds = TotalSeconds;
			App.SaveSettings ();
		}

        private bool OnTimer()
		{
			if (State != RestTimerStates.Running)
				return false;
			
			if (SecondsLeft - 1 <= 0)
			{
				OnResetCommand ();

				if (PlaySounds == true)
				{
                    _soundService.Play ("Bleep");
				}

				App.RestTimerSecondsLeft = 0;
				return false;
			}

			SecondsLeft = SecondsLeft - 1;

			var progress = ProgressBar.Progress + _progressStep;
			ProgressBar.Progress = progress >= 1 ? 1 : progress;

			App.RestTimerSecondsLeft = SecondsLeft;
			return State == RestTimerStates.Running;	
		}

        private async Task OnStartCommand()
		{	
			if (State == RestTimerStates.Editing)
			{
				SecondsLeft = TotalSeconds;
				ProgressBar.Progress = 0;
				_progressStep = GetProgressStep ();
			}

			State = RestTimerStates.Running;
			Device.StartTimer(TimeSpan.FromSeconds(1), () => {	return OnTimer(); });

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnPauseCommand()
		{
			App.RestTimerSecondsLeft = 0;
			State = RestTimerStates.Paused;

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnResetCommand()
		{
			App.RestTimerSecondsLeft = 0;
			State = RestTimerStates.Paused;
			SecondsLeft = TotalSeconds;
			ProgressBar.Progress = 0;
			_progressStep = GetProgressStep ();

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

        private double GetProgressStep()
		{
			if (TotalSeconds == 0)
			{
				return 0;
			} else
			{
				return 1.0 / TotalSeconds;
			}			
		}
        #endregion

        public void StopTimer()
		{
			State = RestTimerStates.Paused;
		}

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            _canSave = false;
            AutoStart = App.Settings.RestTimerAutoStart;
            PlaySounds = App.Settings.RestTimerPlaySounds;
            TotalSeconds = App.Settings.RestTimerTotalSeconds;

            // rest timer already running
            if (App.RestTimerSecondsLeft > 0)
            {
                var seconds = App.RestTimerSecondsLeft;
                await OnResetCommand();
                SecondsLeft = seconds;
                await OnStartCommand();
            }
            else
            {
                await OnResetCommand();
            }

            _canSave = true;

            await OnStartCommand();
        }
        #endregion
    }
}

