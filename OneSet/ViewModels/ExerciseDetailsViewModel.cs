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
	public class ExerciseDetailsViewModel : BaseViewModel
	{
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IUnitsService _units;
        private readonly IExercisesRepository _exercisesRepository;
        private readonly IRoutineDaysRepository _routineDaysRepository;

        public ExerciseDetailsViewModel(
            INavigationService navigationService, IDialogService dialogService, IUnitsService units, 
            IExercisesRepository exercisesRepository, IRoutineDaysRepository routineDaysRepository)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _units = units;
            _exercisesRepository = exercisesRepository;
            _routineDaysRepository = routineDaysRepository;
            Title = AppResources.AddExerciseTitle;
        }

	    private Exercise _exercise;
	    public Exercise Exercise
	    {
	        get { return _exercise ?? (_exercise = new Exercise()); }
	        set { _exercise = value; }
	    }

	    public RepsIncrement RepsIncrement
        {
            get
            {
                var repsIncrement = App.Database.RepsIncrements.FirstOrDefault(x => x.RepsIncrementId == Exercise.RepsIncrementId);
                return repsIncrement;
            }
        }
        
        public bool NotesVisible => !string.IsNullOrEmpty (Exercise.Notes);

        public RoutineDay Mon { get; set; }
        public RoutineDay Tue { get; set; }
        public RoutineDay Wed { get; set; }
        public RoutineDay Thu { get; set; }
        public RoutineDay Fri { get; set; }
        public RoutineDay Sat { get; set; }
        public RoutineDay Sun { get; set; }
        
        public string TrainingDays 
        { 
            get
            {
				var list = new List<string>();
				var dayNames = AppResources.Culture.DateTimeFormat.AbbreviatedDayNames;

				if (Mon.IsActive == 1) { list.Add(dayNames[1]); }
                if (Tue.IsActive == 1) { list.Add(dayNames[2]); }
				if (Wed.IsActive == 1) { list.Add(dayNames[3]); }
                if (Thu.IsActive == 1) { list.Add(dayNames[4]); }
                if (Fri.IsActive == 1) { list.Add(dayNames[5]); }
                if (Sat.IsActive == 1) { list.Add(dayNames[6]); }
                if (Sun.IsActive == 1) { list.Add(dayNames[0]); }

                if (list.Count == 7)
                {
                    return AppResources.AllWeekTraining;
                }

                var s = string.Join(", ", list);

                // replace last , with 'and'
                if (s == string.Empty) return s;
                var place = s.LastIndexOf(",", StringComparison.Ordinal);
                if (place >= 0)
                {
                    s = s.Remove(place, 1).Insert(place, " " + AppResources.And);
                }
                
                return s;
            }
        }
		public bool TrainingDaysVisible => !string.IsNullOrEmpty (TrainingDays);

	    protected ICommand _deleteCommand;
		public ICommand DeleteCommand { 
			get { return _deleteCommand ?? (_deleteCommand = new Command(() => OnDelete())); }
		}

        private RoutineDay GetRoutineDay(List<RoutineDay> routine, int dayOfWeek)
        {
            return routine.FirstOrDefault(x => x.DayOfWeek == dayOfWeek) ?? new RoutineDay
            {
                DayOfWeek = dayOfWeek,
                ExerciseId = Exercise.ExerciseId,
                IsActive = 0,
                RowNumber = 1
            };
        }

        public override async Task OnLoad(object parameter = null)
		{
            Exercise.PlateWeight = _units.GetWeight(App.Settings.IsMetric, Exercise.PlateWeight);

            var routine = await _routineDaysRepository.GetRoutine(Exercise.ExerciseId);
            Mon = GetRoutineDay(routine, 1);
            Tue = GetRoutineDay(routine, 2);
            Wed = GetRoutineDay(routine, 3);
            Thu = GetRoutineDay(routine, 4);
            Fri = GetRoutineDay(routine, 5);
            Sat = GetRoutineDay(routine, 6);
            Sun = GetRoutineDay(routine, 7);
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
        
		private bool Validate ()
		{
			if (string.IsNullOrWhiteSpace (Exercise.Name))
			{
				App.ShowWarning(AppResources.ExerciseNameIsRequired);
				return false;
			}

			if (Exercise.PlateWeight < 0 || Exercise.PlateWeight > 999)
			{
				App.ShowWarning(AppResources.ExerciseInvalidPlateWeight);
                Exercise.PlateWeight = 0;
				return false;
			}

			return true;
		}

        public override async Task OnSave () 
		{
            if (Validate())
            {
                // imperial to metric - always save in metric
                if (!App.Settings.IsMetric)
                {
                    Exercise.PlateWeight = Exercise.PlateWeight / _units.ImperialMetricFactor;
                }

                Exercise.ExerciseId = await _exercisesRepository.SaveAsync(Exercise);
                await SaveRoutine();

                await App.ShowSuccess(AppResources.ExerciseSaved);
                await _navigationService.PopAsync();
            }
        }

		protected virtual async Task OnDelete () 
		{
            try
            {
                var answer = await _dialogService.DisplayAlert (AppResources.ExerciseDeleteQuestionTitle, AppResources.ExerciseDeleteQuestion, AppResources.Yes, AppResources.No);

                if (answer)
                {
                    await _exercisesRepository.DeleteAsync(Exercise.ExerciseId);
                    await _navigationService.PopAsync();
                }
            }
            catch (Exception ex)
            {
                App.ShowErrorPage(this, ex);
            }
        }
	}
}

