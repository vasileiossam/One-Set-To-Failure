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
        private Calendar _calendar;
        public Calendar Calendar
        {
            get { return _calendar; }
            set { SetProperty(ref _calendar, value); }
        }

        public ICommand SaveCommand { get; set; }
        #endregion

        #region private variables
        private readonly INavigationService _navigationService;
        private readonly IMessagingService _messagingService;
        private readonly ICalendarRepository _repo;
        #endregion

        public CalendarNotesViewModel(INavigationService navigationService, IMessagingService messagingService, ICalendarRepository repo)
        {
            _navigationService = navigationService;
            _messagingService = messagingService;
            _repo = repo;

            Title = AppResources.CommentTitle;

            SaveCommand = new Command(async () => await OnSave());
        }

        private  async Task OnSave() 
		{
		    await _repo.SaveAsync(Calendar);

            _messagingService.Send(this, Messages.ItemChanged);

            await _navigationService.PopAsync();
		}

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("CurrentDate"))
            {
                var date = (DateTime) parameters["CurrentDate"];
                Calendar = await _repo.FindAsync(date) ?? new Calendar() { Date = date };
            }
        }
        #endregion
    }
}

