using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Autofac;
using AutoMapper;
using OneSet.Abstract;
using OneSet.Views;

namespace OneSet.ViewModels
{
	public class WorkoutListViewModel : BaseViewModel
    {
        private string _trophies;
		public string Trophies
		{
			get
			{
				return _trophies;
			}
			set
			{
			    if (_trophies == value) return;
			    _trophies = value;
			    OnPropertyChanged("Trophies");
			}
		}

        private string _calendarNotes;
		public string CalendarNotes
		{
			get
			{
				return _calendarNotes;
			}
			set
			{
			    if (_calendarNotes == value) return;
			    _calendarNotes = value;
			    OnPropertyChanged("CalendarNotes");
			}
		}

        private bool _calendarNotesVisible;
		public bool CalendarNotesVisible 
		{ 
			get
			{
				return _calendarNotesVisible;
			}
			set
			{
			    if (_calendarNotesVisible == value) return;
			    _calendarNotesVisible = value;
			    OnPropertyChanged("CalendarNotesVisible");
			}
		}

        private bool _workoutsListVisible;
		public bool WorkoutsListVisible
		{
			get
			{
				return _workoutsListVisible;
			}
			set
			{
			    if (_workoutsListVisible == value) return;
			    _workoutsListVisible = value;
			    OnPropertyChanged("WorkoutsListVisible");
			}
		}

		private bool _noWorkoutDataVisible;
		public bool NoWorkoutDataVisible
		{
			get
			{
				return _noWorkoutDataVisible;
			}
			set
			{
			    if (_noWorkoutDataVisible == value) return;
			    _noWorkoutDataVisible = value;
			    OnPropertyChanged("NoWorkoutDataVisible");
			}
		}

		public RestTimerToolbarItem RestTimerToolbarItem { get; set; }

        public ICommand ChevronTapCommand { get; }
        public ICommand CalendarNotesCommand { get; }
        public ICommand AnalysisCommand { get; }
        public ICommand GotoDateCommand { get; }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
			get
			{
				return _currentDate;
			}
			set
            {
                if (_currentDate == value) return;
                _currentDate = value;
                App.CurrentDate = value;
                OnPropertyChanged("CurrentDate");
            }
        }

        private ObservableCollection<RoutineDayViewModel> _routineDays;
		public ObservableCollection<RoutineDayViewModel> RoutineDays
		{
			get
			{
				return _routineDays;
			}
			set
			{
			    if (_routineDays == value) return;
			    _routineDays = value;
			    OnPropertyChanged ("RoutineDays");
			}
		}

        private readonly IComponentContext _componentContext;
        private readonly INavigationService _navigationService;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IRoutineDaysRepository _routineDaysRepository;

        public WorkoutListViewModel (IComponentContext componentContext, INavigationService navigationService, 
            IWorkoutsRepository workoutsRepository, ICalendarRepository calendarRepository, IRoutineDaysRepository routineDaysRepository)
        {
            _navigationService = navigationService;
            _componentContext = componentContext;
            _workoutsRepository = workoutsRepository;
            _calendarRepository = calendarRepository;
            _routineDaysRepository = routineDaysRepository;
            Title = "One Set To Fatigue";

			ChevronTapCommand = new Command (async(s) => { await OnChevronTapCommand(s); });
			CalendarNotesCommand = new Command (async() => { await OnCalendarNotesCommand(); });
			AnalysisCommand = new Command (async() => { await OnAnalysisCommand(); });
			GotoDateCommand = new Command (async() => { await OnGotoDateCommand(); });
		}

		public override async Task OnLoad(object parameter = null)
		{
		    if (!(parameter is DateTime)) return;
            CurrentDate = (DateTime) parameter;

            RestTimerToolbarItem.Update();
			CalendarNotes = await _calendarRepository.GetCalendarNotes (_currentDate);
			if (CalendarNotes != null)
			{
				CalendarNotes = CalendarNotes.Trim ();

				// trim to two lines
				var countLines = CalendarNotes.ToCharArray ().Count (c => c == '\n');
				if (countLines > 2)
				{
					var lines = CalendarNotes.Split (new[]{ '\n' });
					if (lines.Count () >= 2)
					{
						CalendarNotes = $"{lines[0].Trim()}\n{lines[1].Trim()}...";
					}
				}
			}

			CalendarNotesVisible = !string.IsNullOrEmpty (CalendarNotes);

			var list = await _routineDaysRepository.GetRoutine (_currentDate);
            RoutineDays = Mapper.Map<ObservableCollection<RoutineDayViewModel>>(list);

			if (RoutineDays == null)
			{
				WorkoutsListVisible = false;
			}
			else
			{
				WorkoutsListVisible = RoutineDays.Count > 0;
			}
			NoWorkoutDataVisible = !WorkoutsListVisible;

			if (App.TotalTrophies == null)
			{
  				App.TotalTrophies = await _workoutsRepository.GetTotalTrophies ();
			}
			var dayTrophies = await _workoutsRepository.GetTrophies (CurrentDate);
		    if (App.TotalTrophies != null) Trophies = $"{dayTrophies} / {(int) App.TotalTrophies}";
		}

        public override async Task OnSave()
        {
            await Task.FromResult(0);
        }

        private async Task OnChevronTapCommand (object s) 
		{
			if ((string)s == "Left")
			{
				await OnLoad(CurrentDate.AddDays (-1));
				//Page.ChangeOrientation (false);
				//Page.Refresh ();
			} 
			else
			{
				await OnLoad(CurrentDate.AddDays (1));
				//Page.ChangeOrientation (false);
				//Page.Refresh ();
			}
		}

		private async Task OnCalendarNotesCommand()
		{
		    var page = _componentContext.Resolve<CalendarNotesPage>();
		    page.ViewModel.Date = CurrentDate;
			await _navigationService.PushAsync(page); 	
		}

		private async Task OnAnalysisCommand()
		{
			var page = _componentContext.Resolve<AnalysisPage>();
			await _navigationService.PushAsync(page); 	
		}

		private async Task OnGotoDateCommand()
		{
			DependencyService.Get<IDatePickerDialog>().Show(OnGetDate);
		}

		private async void OnGetDate(object sender, EventArgs args)
		{
		    if (!(sender is DateTime)) return;
		    await OnLoad ((DateTime) sender);

		    Device.BeginInvokeOnMainThread (() =>
		    {
		        //Page.ChangeOrientation (false);
		        //Page.Refresh ();
		    });
		}
    }
}

