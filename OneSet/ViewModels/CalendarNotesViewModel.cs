using System;
using System.Threading.Tasks;
using OneSet.Models;
using OneSet.Resx;
using OneSet.Abstract;

namespace OneSet.ViewModels
{
	public class CalendarNotesViewModel : BaseViewModel
	{
        public DateTime Date { get; set; }

        private Calendar _calendar { get; set; }
	    public Calendar Calendar
	    {
            get
            {
                if (_calendar == null)
                {
                    Task.Run(async () =>
                    {
                        _calendar = await _repo.FindAsync(Date) ?? new Calendar() {Date = Date};
                        OnPropertyChanged("Calendar");
                    }).Wait();
                }
                return _calendar;
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
    }
}

