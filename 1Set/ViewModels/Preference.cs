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

		public EventHandler Clicked {get; set;}
		public EventHandler OnSave {get; set;}

		public Preference ()
		{

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
		public string[] Options {get; set;}

		public ListPreference()
		{

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
		public string TostMessage { get; set; }
	}
}

