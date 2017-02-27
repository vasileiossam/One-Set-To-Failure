using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Abstract;
using OneSet.Models;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
		public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }

        private string _title;
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

        protected BaseViewModel()
        {
			SaveCommand = new Command (async () => await OnSave());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual async Task OnSave()
        {
            await Task.FromResult(0);
        }
    }
}