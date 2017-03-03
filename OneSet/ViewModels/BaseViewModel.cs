using System.ComponentModel;

namespace OneSet.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}