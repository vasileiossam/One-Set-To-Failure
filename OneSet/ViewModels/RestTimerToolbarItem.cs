using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using Xamarin.Forms;
using OneSet.Abstract;
using OneSet.Views;

namespace OneSet.ViewModels
{
	public class RestTimerToolbarItem : BaseViewModel
	{
        #region private variables
        private bool _isRunning;
		private bool _terminated;
        private readonly INavigationService _navigationService;
        private readonly ISoundService _soundService;
        #endregion

        #region properties
        private string _text;
		public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        private string _icon;
		public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        public ICommand Command { get; set; }
        #endregion
        
        public RestTimerToolbarItem(INavigationService navigationService, ISoundService soundService)
        {
            _navigationService = navigationService;
            _soundService = soundService;
            Command = new Command (async() => { await OnRestTimerCommand(); });
			_isRunning = false;
		}

		private async Task OnRestTimerCommand()
		{
			_terminated = true;
            await _navigationService.NavigateTo<RestTimerViewModel>(); 	
		}

		public async Task AutoStart()
		{
			if (!_isRunning)
			{
				_terminated = true;
			    await _navigationService.NavigateTo<RestTimerViewModel>();
            }
		}

		public void Update()
		{
			_terminated = false;

			if (App.RestTimerSecondsLeft > 0)
			{
				Icon = string.Empty;
				Text = App.RestTimerSecondsLeft.ToString ();

				if (!_isRunning)
				{
					Device.StartTimer (TimeSpan.FromSeconds (1), () =>
					{	
						if ((App.RestTimerSecondsLeft - 1) <= 0)
						{
							App.RestTimerSecondsLeft = 0;
							Icon = "ic_action_alarms";
							Text = string.Empty;

							if (!_terminated)
							{
								if (App.Settings.RestTimerPlaySounds)
								{
									_soundService.Play ("Bleep");
								}
							}
							_isRunning = false;
							return _isRunning;
						}

						App.RestTimerSecondsLeft = App.RestTimerSecondsLeft - 1;
						Text = App.RestTimerSecondsLeft.ToString ();

						_isRunning = !_terminated;
						return _isRunning;
					});
				}
			} else
			{
				Icon = "ic_action_alarms";
				Text = string.Empty;
			}
		}
	}
}

