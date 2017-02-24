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
		private bool _isRunning;
		private bool _terminated;

		private string _text;
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
			    if (_text == value) return;
			    _text = value;
			    OnPropertyChanged("Text");
			}
		}

		private string _icon;
		public string Icon
		{
			get
			{
				return _icon;
			}
			set
			{
			    if (_icon == value) return;
			    _icon = value;
			    OnPropertyChanged("Icon");
			}
		}

		private readonly ICommand _command;
		public ICommand Command => _command;

        private readonly INavigationService _navigationService;
        private readonly IComponentContext _componentContext;

        public RestTimerToolbarItem(INavigationService navigationService, IComponentContext componentContext)
        {
            _navigationService = navigationService;
            _componentContext = componentContext;
            _command = new Command (async() => { await OnRestTimerCommand(); });
			_isRunning = false;
		}

		private async Task OnRestTimerCommand()
		{
			_terminated = true;

		    var page = _componentContext.Resolve<RestTimerPage>();
            page.ViewModel = _componentContext.Resolve<RestTimerViewModel>();
            await _navigationService.PushAsync(page); 	
		}

		public async Task AutoStart()
		{
			if (!_isRunning)
			{
				_terminated = true;
                var page = _componentContext.Resolve<RestTimerPage>();
                page.ViewModel = _componentContext.Resolve<RestTimerViewModel>();
				await _navigationService.PushAsync (page); 	
				await page.ViewModel.OnStartCommand ();
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
									var soundService = DependencyService.Get<ISoundService> ();
									soundService.Play ("Bleep");
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

        public override Task OnLoad(object parameter)
	    {
	        throw new NotImplementedException();
	    }

	    public override Task OnSave()
	    {
	        throw new NotImplementedException();
	    }
	}
}

