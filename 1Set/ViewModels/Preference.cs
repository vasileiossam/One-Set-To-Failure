using System;
using Set.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using Set.Resx;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Set.ViewModels
{
	public class Preference : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string Title { get; set; }
		public string Hint { get; set; }
		public INavigation Navigation { get; set; }

		protected object _value;
		public object Value 
		{ 
			get
			{
				return _value;
			}
			set
			{ 
				if (_value != value)
				{
					_value = value;
					OnPropertyChanged("Value");
				}
			}
		}

		public bool IsHintVisible
		{
			get
			{
				return !string.IsNullOrEmpty (Hint);
			}
		}
		public bool IsValueVisible { get; set; }

		public EventHandler Clicked {get; set;}
		public EventHandler OnSave {get; set;}

		public Preference ()
		{
			IsValueVisible = true;
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}

	public class ListPreference : Preference
	{
		public static string[] YesNoOptions = new string[2]{"No", "Yes"};

		public string[] Options {get; set;}

		public ListPreference()
		{

		}

		public bool GetValueAsBool()
		{
			if (Value == null)
			{
				return false;
			}

			var s = (string)Value;
			return String.Equals (s, AppResources.Yes, StringComparison.OrdinalIgnoreCase);
		}

		public static string GetBoolAsString(bool b)
		{
			if (b)
				return AppResources.Yes;
			return AppResources.No;
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

		public PagePreference():base()
		{
			Clicked += OnClicked;
			IsValueVisible = false;
		}

		public async void OnClicked(object sender, EventArgs args)
		{
			var page = Activator.CreateInstance (NavigateToPage);
			await Navigation.PushAsync (page as Page);
		}
	}
}

