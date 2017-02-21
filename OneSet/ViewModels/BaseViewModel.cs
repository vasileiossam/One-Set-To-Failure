using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using Xamarin.Forms;

namespace OneSet.ViewModels
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
                if (_title == value) return;
                _title = value;
                OnPropertyChanged("Title");
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

		protected virtual async Task OnSave () 
		{
			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}
    }
}

