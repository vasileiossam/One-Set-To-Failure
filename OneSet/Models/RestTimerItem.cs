using System;
using OneSet.Abstract;
using Xamarin.Forms;

namespace OneSet.Models
{
	public class RestTimerItem : ObservableObject
	{
        #region private variables
		private bool _terminated;
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

        public int SecondsLeft { get; set; }
        public bool IsRunning { get; set; }
        #endregion
        
        public RestTimerItem(ISoundService soundService)
        {
            _soundService = soundService;
            IsRunning = false;
            SecondsLeft = 0;
        }

	    public void Reset()
	    {
	        SecondsLeft = 0;
        }

        public void Stop()
        {
            _terminated = true;
        }

        public void Update()
		{
			_terminated = false;

			if (SecondsLeft > 0)
			{
				Icon = string.Empty;
				Text = SecondsLeft.ToString ();

				if (!IsRunning)
				{
					Device.StartTimer (TimeSpan.FromSeconds (1), () =>
					{	
						if (SecondsLeft - 1 <= 0)
						{
                            SecondsLeft = 0;
							Icon = "ic_action_alarms";
							Text = string.Empty;

							if (!_terminated)
							{
								if (App.Settings.RestTimerPlaySounds)
								{
									_soundService.Play ("Bleep");
								}
							}
                            IsRunning = false;
							return IsRunning;
						}

                        SecondsLeft = SecondsLeft - 1;
						Text = SecondsLeft.ToString ();

                        IsRunning = !_terminated;
						return IsRunning;
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

