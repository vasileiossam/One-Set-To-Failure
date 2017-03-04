using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Resx;

namespace OneSet.ViewModels
{
	public class ExerciseDetailsViewModel : BaseViewModel, INavigationAware
    {
        #region properties

        public int ExerciseId { get; set; }
        public int? RepsIncrementId { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private double _plateWeight;
        public double PlateWeight
        {
            get { return _plateWeight; }
            set { SetProperty(ref _plateWeight, value); }
        }

        private string _notes;
        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public RepsIncrement RepsIncrement
        {
            get
            {
                var repsIncrement = App.Database.RepsIncrements.FirstOrDefault(x => x.RepsIncrementId == RepsIncrementId);
                return repsIncrement;
            }
        }

        public RoutineDay Mon { get; set; }
        public RoutineDay Tue { get; set; }
        public RoutineDay Wed { get; set; }
        public RoutineDay Thu { get; set; }
        public RoutineDay Fri { get; set; }
        public RoutineDay Sat { get; set; }
        public RoutineDay Sun { get; set; }

        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

        #region private variables
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IMessagingService _messagingService;
        private readonly IUnitsService _units;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IRoutineDaysRepository _routineDaysRepository;
        #endregion

        public ExerciseDetailsViewModel(
            INavigationService navigationService, IDialogService dialogService, IMessagingService messagingService, IUnitsService units, 
            IExercisesRepository exercisesRepository, IRoutineDaysRepository routineDaysRepository)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _messagingService = messagingService;
            _units = units;
            _exercisesRepository = exercisesRepository;
            _routineDaysRepository = routineDaysRepository;

            Title = AppResources.AddExerciseTitle;

            DeleteCommand = new Command(async () => { await OnDelete(); });
            SaveCommand = new Command(async () => await OnSave());
        }
        
        #region private methods
        private RoutineDay GetRoutineDay(List<RoutineDay> routine, int dayOfWeek)
        {
            return routine.FirstOrDefault(x => x.DayOfWeek == dayOfWeek) ?? new RoutineDay
            {
                DayOfWeek = dayOfWeek,
                ExerciseId = ExerciseId,
                IsActive = 0,
                RowNumber = 1
            };
        }

		private async Task SaveRoutine()
		{
            await _routineDaysRepository.SaveAsync(Mon);
            await _routineDaysRepository.SaveAsync(Tue);
            await _routineDaysRepository.SaveAsync(Wed);
            await _routineDaysRepository.SaveAsync(Thu);
            await _routineDaysRepository.SaveAsync(Fri);
            await _routineDaysRepository.SaveAsync(Sat);
            await _routineDaysRepository.SaveAsync(Sun);
        }
        
        private async Task OnDelete () 
		{
            try
            {
                var answer = await _dialogService.DisplayAlert (AppResources.ExerciseDeleteQuestionTitle, AppResources.ExerciseDeleteQuestion, AppResources.Yes, AppResources.No);

                if (answer)
                {
                    await _exercisesRepository.DeleteAsync(ExerciseId);

                    _messagingService.Send(this, Messages.ItemDeleted);

                    await _navigationService.PopAsync();
                }
            }
            catch (Exception ex)
            {
                App.ShowErrorPage(this, ex);
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                App.ShowWarning(AppResources.ExerciseNameIsRequired);
                return false;
            }

            if (PlateWeight < 0 || PlateWeight > 999)
            {
                App.ShowWarning(AppResources.ExerciseInvalidPlateWeight);
                PlateWeight = 0;
                return false;
            }

            return true;
        }

        private async Task OnSave()
        {
            if (Validate())
            {
                var exercise = new Exercise()
                {
                    ExerciseId = this.ExerciseId,
                    Name = this.Name,
                    PlateWeight = _units.GetMetric(App.Settings.IsMetric, this.PlateWeight),
                    Notes = this.Notes
                };

                var message = Messages.ItemAdded;
                if (exercise.ExerciseId > 0)
                {
                    message = Messages.ItemChanged;
                }

                ExerciseId = await _exercisesRepository.SaveAsync(exercise);
                await SaveRoutine();

                _messagingService.Send(this, message, exercise);
                await App.ShowSuccess(AppResources.ExerciseSaved);
                await _navigationService.PopAsync();
            }
        }
        #endregion

        #region INavigationAware
        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Title"))
            {
                Title = (string) parameters["Title"];
            }
            if (parameters.ContainsKey("Exercise"))
            {
                var exercise = parameters["Exercise"] as Exercise;

                if (exercise != null)
                {
                    ExerciseId = exercise.ExerciseId;
                    Name = exercise.Name;
                    Notes = exercise.Notes;
                    PlateWeight = _units.GetWeight(App.Settings.IsMetric, exercise.PlateWeight);
                    RepsIncrementId = exercise.RepsIncrementId;
                }
            }

            var routine = await _routineDaysRepository.GetRoutine(ExerciseId);
            Mon = GetRoutineDay(routine, 1);
            Tue = GetRoutineDay(routine, 2);
            Wed = GetRoutineDay(routine, 3);
            Thu = GetRoutineDay(routine, 4);
            Fri = GetRoutineDay(routine, 5);
            Sat = GetRoutineDay(routine, 6);
            Sun = GetRoutineDay(routine, 0);
        }
        #endregion
    }
}

