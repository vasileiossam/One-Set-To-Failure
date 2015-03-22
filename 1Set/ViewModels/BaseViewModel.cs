using System;
using SQLite.Net.Attributes;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;
using AutoMapper;

namespace Set.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		[IgnoreMap]
		public INavigation Navigation { get; set; }

		[IgnoreMap]
		public ICommand SaveCommand { get; set; }

        private string _title;

		[IgnoreMap]
        public string Title
        {
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
            get
            {
                return _title;
            }
        }

		public BaseViewModel()
        {
			SaveCommand = new Command (() => OnSave ());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

		protected virtual void OnSave () 
		{

		}
    }
}

