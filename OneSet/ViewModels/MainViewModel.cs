﻿using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Views;
using System;
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

        private bool _listVisible;
        public bool ListVisible
        {
            get
            {
                return _listVisible;
            }
            set
            {
                if (_listVisible == value) return;
                _listVisible = value;
                OnPropertyChanged("ListVisible");
            }
        }

        private bool _noDataVisible;
        public bool NoDataVisible
        {
            get
            {
                return _noDataVisible;
            }
            set
            {
                if (_noDataVisible == value) return;
                _noDataVisible = value;
                OnPropertyChanged("NoDataVisible");
            }
        }

        public RestTimerToolbarItem RestTimerToolbarItem { get; set; }

        public ICommand ChevronTapCommand { get; }
        public ICommand CalendarNotesCommand { get; }
        public ICommand AnalysisCommand { get; }
        public ICommand GotoDateCommand { get; }
        public ICommand SelectItemCommand { get; }
        public ICommand ExercisesCommand { get; }
        public ICommand SettingsCommand { get; }

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

        private ObservableCollection<RoutineItemViewModel> _routine;
        public ObservableCollection<RoutineItemViewModel> Routine
        {
            get
            {
                return _routine;
            }
            set
            {
                if (_routine == value) return;
                _routine = value;
                OnPropertyChanged("Routine");
            }
        }
        #endregion

        #region private variables
        private readonly IComponentContext _componentContext;
        private readonly INavigationService _navigationService;
        private readonly IMessagingService _messagingService;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IRoutineDaysRepository _routineDaysRepository;
        private readonly IDatePickerDialog _datePickerDialog;
        #endregion

        public MainViewModel(IComponentContext componentContext, INavigationService navigationService, 
            IMessagingService messagingService, IDatePickerDialog datePickerDialog,
            IWorkoutsRepository workoutsRepository, IExercisesRepository exercisesRepository, 
            ICalendarRepository calendarRepository, IRoutineDaysRepository routineDaysRepository)
        {
            _componentContext = componentContext;
            _navigationService = navigationService;
            _messagingService = messagingService;
            _datePickerDialog = datePickerDialog;
            _workoutsRepository = workoutsRepository;
            _exercisesRepository = exercisesRepository;
            _calendarRepository = calendarRepository;
            _routineDaysRepository = routineDaysRepository;
            Title = "One Set To Fatigue";

            ChevronTapCommand = new Command(async (s) => { await OnChevronTapCommand(s); });
            CalendarNotesCommand = new Command(async () => { await OnCalendarNotesCommand(); });
            AnalysisCommand = new Command(async () => { await OnAnalysisCommand(); });
            GotoDateCommand = new Command(OnGotoDateCommand);
            SelectItemCommand = new Command(async (item) => { await OnItemSelected(item); });
            ExercisesCommand = new Command(async () => { await _navigationService.NavigateTo<ExerciseListViewModel>(); });
            SettingsCommand = new Command(async () => { await _navigationService.NavigateTo<SettingsViewModel>(); });

            RestTimerToolbarItem = _componentContext.Resolve<RestTimerToolbarItem>();

            _messagingService.Subscribe<WorkoutDetailsViewModel, Workout>(this, Messages.ItemChanged, (sender, workout) =>
            {
                var item = Routine.FirstOrDefault(x => x.Exercise.ExerciseId == workout.ExerciseId);
                if (item == null) return;
                item.Workout = null;
                item.Workout = workout;
            });
        }

        ~MainViewModel()
        {
            _messagingService.Unsubscribe<WorkoutDetailsViewModel, Workout>(this, Messages.ItemChanged);
        }
        
        #region commands
        private async Task OnChevronTapCommand(object s)
        {
            if ((string)s == "Left")
            {
                await Load(CurrentDate.AddDays(-1));
                _messagingService.Send(this, Messages.WorkoutsReloaded);
            }
            else
            {
                await Load(CurrentDate.AddDays(1));
                _messagingService.Send(this, Messages.WorkoutsReloaded);
            }
        }

        private async Task OnCalendarNotesCommand()
        {
            var parameters = new NavigationParameters() { { "CurrentDate", CurrentDate } };
            await _navigationService.NavigateTo<CalendarNotesViewModel>(parameters);
        }

        private async Task OnAnalysisCommand()
        {
            var page = _componentContext.Resolve<AnalysisPage>();
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
            var item = selectedItem as RoutineItemViewModel;
            if (item == null) return;

            var parameters = new NavigationParameters
            {
                {"CurrentDate", CurrentDate },
                {"Workout", item.Workout},
                {"Exercise", item.Exercise},
                {"RestTimerToolbarItem", RestTimerToolbarItem}
            };

            await _navigationService.NavigateTo<WorkoutDetailsViewModel>(parameters);
        }
        #endregion

        #region private methods
        private async Task<ObservableCollection<RoutineItemViewModel>> GetRoutine(DateTime date)
        {
            var collection = new ObservableCollection<RoutineItemViewModel>();
            var list = await _routineDaysRepository.GetRoutine(date);
            var exercises = await _exercisesRepository.AllAsync();
            var workouts = await _workoutsRepository.GetWorkouts(date);

            foreach (var day in list)
            {
                var vm = new RoutineItemViewModel
                {
                    RoutineDay = day,
                    Exercise = exercises.FirstOrDefault(x => x.ExerciseId == day.ExerciseId),
                    Workout = workouts.FirstOrDefault(x => x.ExerciseId == day.ExerciseId),
                };
                collection.Add(vm);
            }

            return collection;
        }

        private async Task Load(DateTime date)
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

            Routine = await GetRoutine(_currentDate);

            CalendarNotesVisible = !string.IsNullOrEmpty(CalendarNotes);
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