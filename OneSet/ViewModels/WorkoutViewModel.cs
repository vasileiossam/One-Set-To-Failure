using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Abstract;
using OneSet.Localization;
using OneSet.Models;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
    public class WorkoutViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitsService _units;
        private readonly IWorkoutRules _workoutRules;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IExercisesRepository _exercisesRepository;

        public WorkoutViewModel(INavigationService navigationService, IUnitsService units, IWorkoutRules workoutRules, 
            IWorkoutsRepository workoutsRepository, IExercisesRepository exercisesRepository)
        {
            _navigationService = navigationService;
            _exercisesRepository = exercisesRepository;
            _workoutsRepository = workoutsRepository;
            _units = units;
            _workoutRules = workoutRules;
            Title = AppResources.WorkoutTitle;

            RepsUpCommand = new Command(async () => { await OnRepsUp(); });
            RepsDownCommand = new Command(async () => { await OnRepsDown(); });
            WeighUpCommand = new Command(async () => { await OnWeighUp(); });
            WeighDownCommand = new Command(async () => { await OnWeighDown(); });
            PreviousIconCommand = new Command(() => { OnPreviousIconCommand(); });
            TargetIconCommand = new Command(() => { OnTargetIconCommand(); });
        }
        
        private Workout _workout;
        public Workout Workout
        {
            get { return _workout ?? (_workout = new Workout()); }
            set { _workout = value; }
        }

        public Exercise Exercise { get; set; }
        public RestTimerToolbarItem RestTimerToolbarItem { get; set; }

		protected int _reps;
		public int Reps
		{
			get
			{
				return _reps;
			}
			set
			{
			    if (_reps == value) return;
			    _reps = value;
			    OnPropertyChanged("Reps");
			}
		}

		protected double _weight;
		public double Weight
		{
			get
			{
				return _weight;
			}
			set
			{
			    if (_weight == value) return;
			    _weight = value;
			    OnPropertyChanged("Weight");
			}
		}

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

        public override async Task OnLoad(object parameter = null)
        {
            RestTimerToolbarItem.Update();
            Exercise = await _exercisesRepository.FindAsync(Workout.ExerciseId);
        }

        public override async Task OnSave () 
		{
			Workout.Reps = Reps;

			if (App.Settings.IsMetric)
			{
				Workout.Weight = Weight;
			} else
			{
				// imperial to metric - always save in metric
				Workout.Weight = Weight / _units.ImperialMetricFactor;
			}

			if (await Validate ())
			{
				// invalidate TotalTrophies
				App.TotalTrophies = null;

				Workout.Trophies = _workoutRules.GetTrophies(Workout);

				var isPersisted = Workout.WorkoutId > 0;

				await _workoutsRepository.SaveAsync(Workout);
				await App.ShowSuccess(AppResources.WorkoutSaved);

				if (App.Settings.RestTimerAutoStart && !isPersisted)
				{
					await _navigationService.PopAsync();
					await RestTimerToolbarItem.AutoStart ();
				} else
				{
					await _navigationService.PopAsync();					
				}
			}

            await Task.FromResult(0);
        }

		#region commands
		private async Task OnRepsUp () 
		{
			if (Reps == 0 && Workout.TargetReps > 0)
			{
				Reps = Workout.TargetReps;
			} else
			{
				Reps = Reps + 1;
			}

			await Task.FromResult(0);
		}

		private async Task OnRepsDown () 
		{
            if ((Reps == 0) && (Workout.TargetReps > 0))
            {
                Reps = Workout.TargetReps - 1;
            }
            else
			if ((Reps - 1) < 0)
			{
				Reps = 0;
			} else
			{
				Reps = Reps - 1;
			}

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnWeighUp () 
		{
			if ((Weight == 0) && Workout.TargetWeight > 0)
			{
				Weight = ConvertedTargetWeight;
			} else
			{			
				Weight = Weight + GetStep ();
			}

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnWeighDown () 
		{
            if ((Weight == 0) && Workout.TargetWeight > 0)
            {
                Weight = ConvertedTargetWeight;
            }
            else
			if (Weight - GetStep () < 0)
			{
				Weight = 0;
			} else
			{
				Weight = Weight - GetStep ();
			}

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
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
            //if ((Reps == 0) && (Weight == 0))
            {
                if (Workout.PreviousReps <= 0) return;
                Reps = Workout.PreviousReps;
                Weight = ConvertedPreviousWeight;
            }
        }

        private void OnTargetIconCommand() 
		{
            //if ((Reps == 0) && (Weight == 0))
            {
                if (Workout.TargetReps <= 0) return;
                Reps = Workout.TargetReps;
                Weight = ConvertedTargetWeight;
            }
        }
        #endregion
    }
}

