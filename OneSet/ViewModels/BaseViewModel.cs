using OneSet.Models;

namespace OneSet.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected BaseViewModel()
        {

        }
    }
}