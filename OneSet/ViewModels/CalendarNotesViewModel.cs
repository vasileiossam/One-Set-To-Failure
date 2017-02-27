using System;
using System.Threading.Tasks;
using OneSet.Models;
using OneSet.Resx;
using OneSet.Abstract;

namespace OneSet.ViewModels
{
	public class CalendarNotesViewModel : BaseViewModel, INavigationAware
    {
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

        private readonly ICalendarRepository _repo;
        private readonly INavigationService _navigationService;

        public CalendarNotesViewModel(INavigationService navigationService, ICalendarRepository repo)
        {
            _navigationService = navigationService;
            _repo = repo;
            Title = AppResources.CommentTitle;
		}

        public override async Task OnSave () 
		{

		    await _repo.SaveAsync(Calendar);
		    await _navigationService.PopAsync();
		}

        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("date"))
            {
                var date = (DateTime) parameters["date"];
                Calendar = await _repo.FindAsync(date) ?? new Calendar() { Date = date };
            }
        }
    }
}

