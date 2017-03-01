using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Abstract;
using OneSet.Localization;
using OneSet.Models;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
    public class WorkoutDetailsViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitsService _units;
        private readonly IWorkoutRules _workoutRules;
        private readonly IWorkoutsRepository _workoutsRepository;

        public WorkoutDetailsViewModel(INavigationService navigationService, IUnitsService units, IWorkoutRules workoutRules, 
            IWorkoutsRepository workoutsRepository)
        {
            _navigationService = navigationService;
            _workoutsRepository = workoutsRepository;
            _units = units;
            _workoutRules = workoutRules;
            Title = AppResources.WorkoutTitle;

            RepsUpCommand = new Command(OnRepsUp);
            RepsDownCommand = new Command(OnRepsDown);
            WeighUpCommand = new Command(OnWeighUp);
            WeighDownCommand = new Command(OnWeighDown);
            PreviousIconCommand = new Command(OnPreviousIconCommand);
            TargetIconCommand = new Command(OnTargetIconCommand);
        }
        
        private Workout _workout;
        public Workout Workout
        {
            get { return _workout ?? (_workout = new Workout()); }
            set { _workout = value; }
        }

        public Exercise Exercise { get; set; }
        public RestTimerToolbarItem RestTimerToolbarItem { get; set; }

	    public string PreviousTitle => AppResources.PreviousTitle + " (" + L10n.GetWeightUnit() + ")";
        public string TargetTitle => AppResources.TargetTitle + " (" + L10n.GetWeightUnit() + ")";
        public bool NotesVisible => !string.IsNullOrEmpty (Exercise?.Notes);
        public bool LevelUpVisible => Workout != null && Workout?.TargetWeight > Workout.PreviousWeight;
        public bool TargetRepsWeightVisible => App.Settings.TargetRepsWeightVisible;
        public bool PreviousRepsWeightVisible => App.Settings.PreviousRepsWeightVisible;
        public double ConvertedWeight => _units.GetWeight(App.Settings.IsMetric, Workout.Weight);
        public double ConvertedPreviousWeight => _units.GetWeight(App.Settings.IsMetric, Workout.PreviousWeight);
        public double ConvertedTargetWeight => _units.GetWeight(App.Settings.IsMetric, Workout.TargetWeight);

        public ICommand RepsUpCommand {get; set;}
		public ICommand RepsDownCommand {get; set;}
		public ICommand WeighUpCommand {get; set;}
		public ICommand WeighDownCommand {get; set;}
        public ICommand PreviousIconCommand { get; set; }
        public ICommand TargetIconCommand { get; set; }

		private async Task<bool> Validate ()
		{
			if (Workout.Reps == 0)
			{
				await App.ShowWarning(AppResources.WorkoutRepsIsRequired);
				return false;
			}
			if (Workout.Weight == 0)
			{
				await App.ShowWarning(AppResources.WorkoutWeightIsRequired);
				return false;
			}

			if (Workout.Reps <= 0 || Workout.Reps > 100)
			{
				await App.ShowWarning(AppResources.WorkoutInvalidReps);
				Workout.Reps = 0;
				return false;
			}
			if (Workout.Weight <= 0 || Workout.Weight > 1000)
			{
				await App.ShowWarning(AppResources.WorkoutInvalidWeight);
				Workout.Weight = 0;
				return false;
			}

			return true;
		}

        public override async Task OnSave () 
		{
			//if (App.Settings.IsMetric)
			//{
			//	Workout.Weight = Weight;
			//} else
			//{
			//	// imperial to metric - always save in metric
			//	Workout.Weight = Weight / _units.ImperialMetricFactor;
			//}

			if (await Validate ())
			{
				// invalidate TotalTrophies
				App.TotalTrophies = null;

				Workout.Trophies = _workoutRules.GetTrophies(Workout);
			    Workout.ExerciseId = Exercise.ExerciseId;
                var isPersisted = Workout.WorkoutId > 0;
				await _workoutsRepository.SaveAsync(Workout);
				await App.ShowSuccess(AppResources.WorkoutSaved);

				if (App.Settings.RestTimerAutoStart && !isPersisted)
				{
					await _navigationService.PopAsync();
					await RestTimerToolbarItem.AutoStart();
				} else
				{
					await _navigationService.PopAsync();					
				}
			}
        }

		#region commands
		private void OnRepsUp () 
		{
			if (Workout.Reps == 0 && Workout.TargetReps > 0)
			{
                Workout.Reps = Workout.TargetReps;
			} else
			{
                Workout.Reps = Workout.Reps + 1;
			}
            OnPropertyChanged("Workout");
		}

		private void OnRepsDown () 
		{
            if (Workout.Reps == 0 && Workout.TargetReps > 0)
            {
                Workout.Reps = Workout.TargetReps - 1;
            }
            else
			if (Workout.Reps - 1 < 0)
			{
                Workout.Reps = 0;
			} else
			{
                Workout.Reps = Workout.Reps - 1;
			}
            OnPropertyChanged("Workout");
        }

		private void OnWeighUp () 
		{
			if (Workout.Weight == 0 && Workout.TargetWeight > 0)
			{
                Workout.Weight = ConvertedTargetWeight;
			} else
			{
                Workout.Weight = Workout.Weight + GetStep ();
			}
            OnPropertyChanged("Workout");
        }

		private void OnWeighDown () 
		{
            if (Workout.Weight == 0 && Workout.TargetWeight > 0)
            {
                Workout.Weight = ConvertedTargetWeight;
            }
            else
			if (Workout.Weight - GetStep () < 0)
			{
                Workout.Weight = 0;
			} else
			{
                Workout.Weight = Workout.Weight - GetStep ();
			}
            OnPropertyChanged("Workout");
        }

		private double GetStep()
		{
            var step =_units.GetWeight(App.Settings.IsMetric, Exercise.PlateWeight);
            if (step <= 0)
				step = 1;
			return step;
		}

        private void  OnPreviousIconCommand() 
		{
            if (Workout.PreviousReps <= 0) return;
            Workout.Reps = Workout.PreviousReps;
            Workout.Weight = ConvertedPreviousWeight;
            OnPropertyChanged("Workout");
        }

        private void OnTargetIconCommand() 
		{
            if (Workout.TargetReps <= 0) return;
            Workout.Reps = Workout.TargetReps;
            Workout.Weight = ConvertedTargetWeight;
            OnPropertyChanged("Workout");
        }
        #endregion

        public async Task OnNavigatedFrom(NavigationParameters parameters)
        {
            await Task.FromResult(0);
        }

        public async Task OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Workout"))
            {
                Workout = parameters["Workout"] as Workout;
            }
            if (parameters.ContainsKey("Exercise"))
            {
                Exercise = parameters["Exercise"] as Exercise;
            }
            if (parameters.ContainsKey("RestTimerToolbarItem"))
            {
                RestTimerToolbarItem = parameters["RestTimerToolbarItem"] as RestTimerToolbarItem;
            }

            RestTimerToolbarItem?.Update();

            if (Workout != null)
            {
                Workout.ExerciseId = Exercise.ExerciseId;
                var previousWorkout = await _workoutsRepository.GetPreviousWorkout(Workout);
                if (previousWorkout != null)
                {
                    Workout.PreviousReps = previousWorkout.Reps;
                    Workout.PreviousWeight = previousWorkout.Weight;
                }

                var targetWorkout = await _workoutRules.GetTargetWorkout(Workout, Exercise, previousWorkout);
                Workout.TargetReps = targetWorkout.Key;
                Workout.TargetWeight = targetWorkout.Value;
            }
        }
    }
}

