using System;
using SQLite.Net.Attributes;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;

namespace Set.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

		public INavigation Navigation { get; set; }
		public ICommand SaveCommand { get; set; }

        private string _title;
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

		public BaseViewModel(INavigation navigation)
        {
			Navigation = navigation;
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

