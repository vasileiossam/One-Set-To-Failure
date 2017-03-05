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
        private readonly ISoundService _soundService;
        private double _progressStep;
        private bool _canSave;
        #endregion

        #region properties
        protected double _progress;
        public double Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }
        
        protected RestTimerStates _state;
		public RestTimerStates State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        protected int _totalSeconds;
		public int TotalSeconds
        {
            get { return _totalSeconds; }
            set
            {
                SetProperty(ref _totalSeconds, value);
                Save();
            }
        }

        protected int _secondsLeft;
		public int SecondsLeft
        {
            get { return _secondsLeft; }
            set { SetProperty(ref _secondsLeft, value); }
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
		
			StartCommand = new Command (OnStartCommand);
			PauseCommand = new Command (OnPauseCommand);
			ResetCommand = new Command (OnResetCommand);

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

				App.RestTimerItem.SecondsLeft = 0;
				return false;
			}

            SecondsLeft = SecondsLeft - 1;

			var progress = Progress + _progressStep;
		    Progress = progress >= 1 ? 1 : progress;

            App.RestTimerItem.SecondsLeft = SecondsLeft;
			return State == RestTimerStates.Running;	
		}

        private void OnStartCommand()
        {
            if (State == RestTimerStates.Editing)
            {
                SecondsLeft = TotalSeconds;
                Progress = 0;
                _progressStep = GetProgressStep();
            }

            State = RestTimerStates.Running;
            Device.StartTimer(TimeSpan.FromSeconds(1), OnTimer);
        }

        private void OnPauseCommand()
		{
            App.RestTimerItem.SecondsLeft = 0;
			State = RestTimerStates.Paused;
		}

		private void OnResetCommand()
		{
            App.RestTimerItem.SecondsLeft = 0;
			State = RestTimerStates.Paused;
			SecondsLeft = TotalSeconds;
			Progress = 0;
			_progressStep = GetProgressStep();
		}

        private double GetProgressStep()
        {
            if (TotalSeconds == 0)
			{
				return 0;
			}
            return 1.0 / TotalSeconds;
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

            if (App.RestTimerItem.IsRunning)
            {
                var seconds = App.RestTimerItem.SecondsLeft;
                App.RestTimerItem.Stop();

                OnResetCommand();

                SecondsLeft = seconds;
                OnStartCommand();
            }
            else
            {
                OnResetCommand();
            }

            _canSave = true;

            if (parameters.ContainsKey("StartImmediately"))
            {
                OnStartCommand();
            }

            await Task.FromResult(0);
        }
        #endregion
    }
}

