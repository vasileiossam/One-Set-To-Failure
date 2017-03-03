using System;
using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Models;
using OneSet.Resx;
using OneSet.Abstract;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
	public class CalendarNotesViewModel : BaseViewModel, INavigationAware
    {
        #region properties
        private Calendar _calendar { get; set; }
        public Calendar Calendar
        {
            get
            {
                return _calendar;
            }
            set
            {
                if (_calendar == value) return;
                _calendar = value;
                OnPropertyChanged("Calendar");
            }
        }

        public ICommand SaveCommand { get; set; }
        #endregion

        #region private variables
        private readonly ICalendarRepository _repo;
        private readonly INavigationService _navigationService;
        #endregion

        public CalendarNotesViewModel(INavigationService navigationService, ICalendarRepository repo)
        {
            _navigationService = navigationService;
            _repo = repo;

            Title = AppResources.CommentTitle;

            SaveCommand = new Command(async () => await OnSave());
        }

        private  async Task OnSave() 
		{
		    await _repo.SaveAsync(Calendar);
		    await _navigationService.PopAsync();
		}

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Date"))
            {
                var date = (DateTime) parameters["Date"];
                Calendar = await _repo.FindAsync(date) ?? new Calendar() { Date = date };
            }
        }
        #endregion
    }
}

