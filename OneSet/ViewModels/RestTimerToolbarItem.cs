using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Models;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

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
				if (_text != value)
				{
					_text = value;
					OnPropertyChanged("Text");
				}
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
				if (_icon != value)
				{
					_icon = value;
					OnPropertyChanged("Icon");
				}
			}
		}

		private readonly ICommand _command;
		public ICommand Command
		{
			get
			{
				return _command;
			}
		}

		public RestTimerToolbarItem()
			: base()
		{
			_command = new Command (async() => { await OnRestTimerCommand(); });
			_isRunning = false;
		}

		private async Task OnRestTimerCommand()
		{
			_terminated = true;
			var viewModel = new RestTimerViewModel() {Navigation = Navigation};
			var page = new RestTimerPage () {ViewModel = viewModel};
			await Navigation.PushAsync(page); 	
		}

		public async Task AutoStart()
		{
			if (!_isRunning)
			{
				_terminated = true;
				var viewModel = new RestTimerViewModel () { Navigation = Navigation };
				var page = new RestTimerPage () { ViewModel = viewModel };
				await Navigation.PushAsync (page); 	
				await viewModel.OnStartCommand ();
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
	}
}

