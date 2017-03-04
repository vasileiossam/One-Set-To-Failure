using System;
using System.ComponentModel;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
	public class Preference : ObservableObject
	{
		public string Title { get; set; }
		public string Hint { get; set; }

		protected object _value;
		public object Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public bool IsHintVisible => !string.IsNullOrEmpty (Hint);
	    public bool IsValueVisible { get; set; }

		public EventHandler Clicked {get; set;}
		public EventHandler OnSave {get; set;}

		public Preference ()
		{
			IsValueVisible = true;
		}
	}

	public class ListPreference : Preference
	{
		public static string[] YesNoOptions = {"No", "Yes"};

		public string[] Options {get; set;}

	    public bool GetValueAsBool()
		{
			if (Value == null)
			{
				return false;
			}

			var s = (string)Value;
			return string.Equals (s, AppResources.Yes, StringComparison.OrdinalIgnoreCase);
		}

		public static string GetBoolAsString(bool b)
		{
		    return b ? AppResources.Yes : AppResources.No;
		}

		public static string[] GetOptionsList(int startNum, int finishNum, bool includeZero)
		{
			var total = finishNum - startNum + 1; 
			var index = 0;

			if (includeZero)
				total++;

			var options = new string[total];

			if (includeZero)
			{
				options [0] = "0";
				index++;
			}

			for (var i = startNum; i <= finishNum; i++)
			{
				options[index++] = i.ToString();
			}

			return options;
		}
	}

	public class AlertPreference : Preference
	{
		public string PopupTitle { get; set; }
		public string PopupMessage {get; set;}
		public EventHandler OnExecute {get; set;}

		public AlertPreference():base()
		{
			IsValueVisible = false;
		}
	}

	public class PagePreference : Preference
	{
		public Type NavigateToPage { get; set; }
	    private readonly INavigationService _navigationService;

        public PagePreference(INavigationService navigationService) 
        {
            _navigationService = navigationService;
            Clicked += OnClicked;
			IsValueVisible = false;
		}

		public async void OnClicked(object sender, EventArgs args)
		{
			var page = Activator.CreateInstance (NavigateToPage);
			await _navigationService.PushAsync (page as Page);
		}
	}
}

