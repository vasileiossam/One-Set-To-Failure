using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Views;

namespace OneSet.ViewModels
{
	public class WorkoutListViewModel : BaseViewModel, INavigationAware
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
        private readonly IMessagingService _messagingService;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IRoutineDaysRepository _routineDaysRepository;

        public WorkoutListViewModel (IComponentContext componentContext, INavigationService navigationService, IMessagingService messagingService,
            IWorkoutsRepository workoutsRepository, IExercisesRepository exercisesRepository, ICalendarRepository calendarRepository, IRoutineDaysRepository routineDaysRepository)
        {
            _componentContext = componentContext;
            _navigationService = navigationService;
            _messagingService = messagingService;
            _workoutsRepository = workoutsRepository;
            _exercisesRepository = exercisesRepository;
            _calendarRepository = calendarRepository;
            _routineDaysRepository = routineDaysRepository;
            Title = "One Set To Fatigue";

			ChevronTapCommand = new Command (async(s) => { await OnChevronTapCommand(s); });
			CalendarNotesCommand = new Command (async() => { await OnCalendarNotesCommand(); });
			AnalysisCommand = new Command (async() => { await OnAnalysisCommand(); });
			GotoDateCommand = new Command (async() => { await OnGotoDateCommand(); });

            RestTimerToolbarItem = _componentContext.Resolve<RestTimerToolbarItem>();
        }

        private async Task<ObservableCollection<RoutineDayViewModel>> GetRoutine(DateTime date)
        {
            var collection = new ObservableCollection<RoutineDayViewModel>();
            var list = await _routineDaysRepository.GetRoutine(date);
            var exercises = await _exercisesRepository.AllAsync();
            var workouts = await _workoutsRepository.GetWorkouts(date);

            foreach (var day in list)
            {
                var vm = new RoutineDayViewModel
                {
                    RoutineDay =  day,
                    Exercise = exercises.FirstOrDefault(x => x.ExerciseId == day.ExerciseId),
                    Workout = workouts.FirstOrDefault(x => x.ExerciseId == day.ExerciseId),
                };
                collection.Add(vm);
            }

            return collection;
        }

        public override async Task OnSave()
        {
            await Task.FromResult(0);
        }

        private async Task OnChevronTapCommand (object s) 
		{
			if ((string)s == "Left")
			{
				await Load(CurrentDate.AddDays (-1));
                _messagingService.Send(this, Messages.WorkoutsReloaded);
			} 
			else
			{
				await Load(CurrentDate.AddDays (1));
                _messagingService.Send(this, Messages.WorkoutsReloaded);
            }
		}

        private async Task OnCalendarNotesCommand()
        {
            var parameters = new NavigationParameters() {{"CurrentDate", CurrentDate}};
            await _navigationService.NavigateTo<CalendarNotesViewModel>(parameters);
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
		    await Load((DateTime) sender);

		    Device.BeginInvokeOnMainThread (() =>
		    {
                _messagingService.Send(this, Messages.WorkoutsReloaded);
            });
		}

        public async Task Load(DateTime date)
        {
            CurrentDate = date;

            RestTimerToolbarItem.Update();
            CalendarNotes = await _calendarRepository.GetCalendarNotes(_currentDate);
            if (CalendarNotes != null)
            {
                CalendarNotes = CalendarNotes.Trim();

                // trim to two lines
                var countLines = CalendarNotes.ToCharArray().Count(c => c == '\n');
                if (countLines > 2)
                {
                    var lines = CalendarNotes.Split(new[] { '\n' });
                    if (lines.Count() >= 2)
                    {
                        CalendarNotes = $"{lines[0].Trim()}\n{lines[1].Trim()}...";
                    }
                }
            }

            RoutineDays = await GetRoutine(_currentDate);

            CalendarNotesVisible = !string.IsNullOrEmpty(CalendarNotes);
            WorkoutsListVisible = RoutineDays.Count > 0;
            NoWorkoutDataVisible = !WorkoutsListVisible;

            if (App.TotalTrophies == null)
            {
                App.TotalTrophies = await _workoutsRepository.GetTotalTrophies();
            }
            var dayTrophies = await _workoutsRepository.GetTrophies(CurrentDate);
            if (App.TotalTrophies != null) Trophies = $"{dayTrophies} / {(int)App.TotalTrophies}";
        }

        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("CurrentDate"))
            {
                CurrentDate = (DateTime) parameters["CurrentDate"];
            }
            else
            {
                CurrentDate = DateTime.Today;
            }

            await Load(CurrentDate);
        }
    }
}

