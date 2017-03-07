using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
    public class MainViewModel : BaseViewModel, INavigationAware
    {
        #region properties
        private string _trophies;
        public string Trophies
        {
            get { return _trophies; }
            set { SetProperty(ref _trophies, value); }
        }

        private string _calendarNotes;
        public string CalendarNotes
        {
            get { return _calendarNotes; }
            set
            {
                SetProperty(ref _calendarNotes, value);
                OnPropertyChanged("CalendarNotesVisible");
            }
        }

        private bool _calendarNotesVisible;
        public bool CalendarNotesVisible
        {
            get { return _calendarNotesVisible; }
            set { SetProperty(ref _calendarNotesVisible, value); }
        }

        private bool _listVisible;
        public bool ListVisible
        {
            get { return _listVisible; }
            set { SetProperty(ref _listVisible, value); }
        }

        private bool _noDataVisible;
        public bool NoDataVisible
        {
            get { return _noDataVisible; }
            set { SetProperty(ref _noDataVisible, value); }
        }

        public RestTimerItem RestTimerItem { get; set; }

        public ICommand ChevronTapCommand { get; }
        public ICommand CalendarNotesCommand { get; }
        public ICommand AnalysisCommand { get; }
        public ICommand GotoDateCommand { get; }
        public ICommand SelectItemCommand { get; }
        public ICommand ExercisesCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand RestTimerCommand { get; }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get
            {
                return _currentDate;
            }
            set
            {
                SetProperty(ref _currentDate, value);
                App.CurrentDate = value;
            }
        }

        private ObservableCollection<RoutineItem> _routine;
        public ObservableCollection<RoutineItem> Routine
        {
            get { return _routine; }
            set { SetProperty(ref _routine, value); }
        }
        #endregion

        #region private variables
        private readonly IComponentContext _componentContext;
        private readonly INavigationService _navigationService;
        private readonly IMessagingService _messagingService;
        private readonly IDatePickerDialog _datePickerDialog;
        private readonly IWorkoutRules _workoutRules;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IRoutineDaysRepository _routineDaysRepository;
        #endregion

        public MainViewModel(IComponentContext componentContext, INavigationService navigationService, 
            IMessagingService messagingService, IDatePickerDialog datePickerDialog, IWorkoutRules workoutRules,
            IWorkoutsRepository workoutsRepository, IExercisesRepository exercisesRepository, 
            ICalendarRepository calendarRepository, IRoutineDaysRepository routineDaysRepository)
        {
            _componentContext = componentContext;
            _navigationService = navigationService;
            _messagingService = messagingService;
            _datePickerDialog = datePickerDialog;
            _workoutRules = workoutRules;
            _workoutsRepository = workoutsRepository;
            _exercisesRepository = exercisesRepository;
            _calendarRepository = calendarRepository;
            _routineDaysRepository = routineDaysRepository;

            Title = "One Set To Failure";

            ChevronTapCommand = new Command(async (s) => { await OnChevronTapCommand(s); });
            CalendarNotesCommand = new Command(async () => { await OnCalendarNotesCommand(); });
            AnalysisCommand = new Command(async () => { await OnAnalysisCommand(); });
            GotoDateCommand = new Command(OnGotoDateCommand);
            SelectItemCommand = new Command(async (item) => { await OnItemSelected(item); });
            ExercisesCommand = new Command(async () => { await _navigationService.NavigateTo<ExerciseListViewModel>(); });
            SettingsCommand = new Command(async () => { await _navigationService.NavigateTo<SettingsViewModel>(); });
            RestTimerCommand = new Command(async () => { await _navigationService.NavigateTo<RestTimerViewModel>(); });

            RestTimerItem = App.RestTimerItem;

            _messagingService.Subscribe<WorkoutDetailsViewModel, Workout>(this, Messages.ItemChanged, (sender, workout) =>
            {
                var item = Routine.FirstOrDefault(x => x.Exercise.ExerciseId == workout.ExerciseId);
                if (item == null) return;
                item.Workout = null;
                item.Workout = workout;
            });

            _messagingService.Subscribe<ExerciseDetailsViewModel, Exercise>(this, Messages.ItemAdded, async (sender, e) =>
            {
                await Reload();
            });
            _messagingService.Subscribe<ExerciseDetailsViewModel, Exercise>(this, Messages.ItemChanged, async (sender, e) =>
            {
                await Reload();
            });
            _messagingService.Subscribe<ExerciseDetailsViewModel>(this, Messages.ItemDeleted, async sender =>
            {
                await Reload();
            });

            _messagingService.Subscribe<CalendarNotesViewModel>(this, Messages.ItemChanged, async sender =>
            {
                await LoadNotes();
            });
        }

        ~MainViewModel()
        {
            _messagingService.Unsubscribe<WorkoutDetailsViewModel, Workout>(this, Messages.ItemChanged);
            _messagingService.Unsubscribe<ExerciseDetailsViewModel, Exercise>(this, Messages.ItemAdded);
            _messagingService.Unsubscribe<ExerciseDetailsViewModel, Exercise>(this, Messages.ItemChanged);
            _messagingService.Unsubscribe<ExerciseDetailsViewModel>(this, Messages.ItemDeleted);
            _messagingService.Unsubscribe<CalendarNotesViewModel>(this, Messages.ItemChanged);
        }
        
        #region commands
        private async Task OnChevronTapCommand(object s)
        {
            if ((string)s == "Left")
            {
                await Load(CurrentDate.AddDays(-1));
            }
            else
            {
                await Load(CurrentDate.AddDays(1));
            }

            _messagingService.Send(this, Messages.WorkoutsReloaded);
        }

        private async Task OnCalendarNotesCommand()
        {
            var parameters = new NavigationParameters { { "CurrentDate", CurrentDate } };
            await _navigationService.NavigateTo<CalendarNotesViewModel>(parameters);
        }

        private async Task OnAnalysisCommand()
        {
            var page = _componentContext.Resolve<AnalysisTabbedPage>();
            await _navigationService.PushAsync(page);
        }

        private void OnGotoDateCommand()
        {
            _datePickerDialog.Show(OnGetDate);
        }

        private async void OnGetDate(object sender, EventArgs args)
        {
            if (!(sender is DateTime)) return;
            await Load((DateTime)sender);

            Device.BeginInvokeOnMainThread(() =>
           {
               _messagingService.Send(this, Messages.WorkoutsReloaded);
           });
        }

        private async Task OnItemSelected(object selectedItem)
        {
            var item = selectedItem as RoutineItem;
            if (item == null) return;

            var parameters = new NavigationParameters
            {
                {"CurrentDate", CurrentDate },
                {"Workout", item.Workout},
                {"Exercise", item.Exercise}
            };

            await _navigationService.NavigateTo<WorkoutDetailsViewModel>(parameters);
        }

        private async Task Reload()
        {
            await Load(CurrentDate);
            _messagingService.Send(this, Messages.WorkoutsReloaded);
        }
        #endregion

        #region private methods
        private async Task<ObservableCollection<RoutineItem>> GetRoutine(DateTime date)
        {
            var collection = new ObservableCollection<RoutineItem>();
            var list = await _routineDaysRepository.GetRoutine(date);
            var exercises = await _exercisesRepository.AllAsync();
            var workouts = await _workoutsRepository.GetWorkouts(date);

            foreach (var day in list)
            {
                // we shouldn't have days without an Exercise but we might have them because of an older fixed bug
                if (day.ExerciseId == 0) continue;

                var vm = new RoutineItem
                {
                    RoutineDay = day,
                    Exercise = exercises.FirstOrDefault(x => x.ExerciseId == day.ExerciseId),
                    Workout = workouts.FirstOrDefault(x => x.ExerciseId == day.ExerciseId),
                };
                
                var previousWorkout = await _workoutsRepository.GetPreviousWorkout(vm.Exercise.ExerciseId, date);
                if (previousWorkout != null)
                {
                    vm.PreviousReps = previousWorkout.Reps;
                    vm.PreviousWeight = previousWorkout.Weight;
                }

                var targetWorkout = await _workoutRules.GetTargetWorkout(vm.Workout, vm.Exercise, previousWorkout);
                if (!targetWorkout.Equals(default(KeyValuePair<int, double>)))
                {
                    vm.TargetReps = targetWorkout.Key;
                    vm.TargetWeight = targetWorkout.Value;
                }

                collection.Add(vm);
            }

            return collection;
        }

        private async Task LoadNotes()
        {
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

            CalendarNotesVisible = !string.IsNullOrEmpty(CalendarNotes);
        }

        private async Task Load(DateTime date)
        {
            CurrentDate = date;

            await LoadNotes();

            Routine = await GetRoutine(_currentDate);

            ListVisible = Routine.Count > 0;
            NoDataVisible = !ListVisible;

            if (App.TotalTrophies == null)
            {
                App.TotalTrophies = await _workoutsRepository.GetTotalTrophies();
            }
            var dayTrophies = await _workoutsRepository.GetTrophies(CurrentDate);
            if (App.TotalTrophies != null) Trophies = $"{dayTrophies} / {(int)App.TotalTrophies}";
        }
        #endregion

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("CurrentDate"))
            {
                CurrentDate = (DateTime)parameters["CurrentDate"];
            }
            else
            {
                CurrentDate = DateTime.Today;
            }

            await Load(CurrentDate);
        }
        #endregion
    }
}