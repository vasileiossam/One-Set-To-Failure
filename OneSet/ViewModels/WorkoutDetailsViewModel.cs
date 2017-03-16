using System;
using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Abstract;
using OneSet.Extensions;
using OneSet.Localization;
using OneSet.Models;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.ViewModels
{
    public class WorkoutDetailsViewModel : BaseViewModel, INavigationAware
    {
        #region properties

        public int WorkoutId { get; set; }
        public DateTime Created { get; set; }
        public string Notes { get; set; }

        private int _reps;
        public int Reps
        {
            get { return _reps; }
            set { SetProperty(ref _reps, value); }
        }

        private double _weight;
        public double Weight
        {
            get { return _weight; }
            set { SetProperty(ref _weight, value); }
        }

        private int _reviousReps;
        public int PreviousReps
        {
            get { return _reviousReps; }
            set { SetProperty(ref _reviousReps, value); }
        }

        private double _previousWeight;
        public double PreviousWeight
        {
            get { return _previousWeight; }
            set { SetProperty(ref _previousWeight, value); }
        }

        private int _targetReps;
        public int TargetReps
        {
            get { return _targetReps; }
            set { SetProperty(ref _targetReps, value); }
        }

        private double _targetWeight;
        public double TargetWeight
        {
            get { return _targetWeight; }
            set { SetProperty(ref _targetWeight, value); }
        }

        public Exercise Exercise { get; set; }
        public RestTimerItem RestTimerItem { get; set; }

        public string PreviousTitle => AppResources.PreviousTitle + " (" + L10n.GetWeightUnit() + ")";
        public string TargetTitle => AppResources.TargetTitle + " (" + L10n.GetWeightUnit() + ")";
        public bool NotesVisible => !string.IsNullOrEmpty(Exercise?.Notes);
        public bool TargetRepsWeightVisible => App.Settings.TargetRepsWeightVisible;
        public bool PreviousRepsWeightVisible => App.Settings.PreviousRepsWeightVisible;

        public ICommand RepsUpCommand { get; set; }
        public ICommand RepsDownCommand { get; set; }
        public ICommand WeighUpCommand { get; set; }
        public ICommand WeighDownCommand { get; set; }
        public ICommand PreviousIconCommand { get; set; }
        public ICommand TargetIconCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand TimerCommand { get; set; }
        #endregion

        #region private variables
        private readonly IMasterDetailNavigation _navigationService;
        private readonly IUnitsService _units;
        private readonly IWorkoutRules _workoutRules;
        private readonly IWorkoutsRepository _workoutsRepository;
        private readonly IMessagingService _messagingService;
        #endregion

        public WorkoutDetailsViewModel(IMasterDetailNavigation navigationService, IMessagingService messagingService, 
            IUnitsService units, IWorkoutRules workoutRules, IWorkoutsRepository workoutsRepository)
        {
            _navigationService = navigationService;
            _messagingService = messagingService;
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
            SaveCommand = new Command(async () => await OnSave());
            TimerCommand = new Command(async () =>
            {
                var parameters = new NavigationParameters { { "RestTimerItem", RestTimerItem } };
                await _navigationService.NavigateToHierarchical<RestTimerViewModel>(parameters);
            });
        }

        private async Task<bool> Validate ()
		{
			if (Reps == 0)
			{
				AppResources.WorkoutRepsIsRequired.ToToast(ToastNotificationType.Warning);
				return false;
			}
			if (Weight == 0)
			{
				AppResources.WorkoutWeightIsRequired.ToToast(ToastNotificationType.Warning);
                return false;
			}

			if (Reps <= 0 || Reps > 100)
			{
				AppResources.WorkoutInvalidReps.ToToast(ToastNotificationType.Warning);
                Reps = 0;
				return false;
			}
			if (Weight <= 0 || Weight > 1000)
			{
				AppResources.WorkoutInvalidWeight.ToToast(ToastNotificationType.Warning);
                Weight = 0;
				return false;
			}

			return true;
		}

        public async Task OnSave () 
		{
            if (await Validate ())
			{
				// invalidate TotalTrophies
				App.TotalTrophies = null;

                var workout = new Workout
                {
                    WorkoutId = this.WorkoutId,
                    ExerciseId = Exercise.ExerciseId,
                    Created = this.Created,
                    Notes = this.Notes,
                    Reps = this.Reps,
                    Weight = _units.GetMetric(App.Settings.IsMetric, this.Weight),
                    PreviousReps = this.PreviousReps,
                    PreviousWeight = _units.GetMetric(App.Settings.IsMetric, this.PreviousWeight),
                    TargetReps = this.TargetReps,
                    TargetWeight = _units.GetMetric(App.Settings.IsMetric, this.TargetWeight),
                };
                workout.Trophies = _workoutRules.GetTrophies(workout);

                var isPersisted = WorkoutId > 0;
                WorkoutId = await _workoutsRepository.SaveAsync(workout);

                _messagingService.Send(this, Messages.ItemChanged, workout);
                AppResources.WorkoutSaved.ToToast(ToastNotificationType.Success);

				if (App.Settings.RestTimerAutoStart && !isPersisted)
				{
					await _navigationService.PopAsync();

                    if (!RestTimerItem.IsRunning)
				    {
                        var parameters = new NavigationParameters { {"StartImmediately", true } };
                        await _navigationService.NavigateToHierarchical<RestTimerViewModel>(parameters);
				    }
				} else
				{
					await _navigationService.PopAsync();					
				}
			}
        }

		#region commands
		private void OnRepsUp () 
		{
			if (Reps == 0 && TargetReps > 0)
			{
                Reps = TargetReps;
			} else
			{
                Reps = Reps + 1;
			}
		}

		private void OnRepsDown () 
		{
            if (Reps == 0 && TargetReps > 0)
            {
                Reps = TargetReps - 1;
            }
            else
			if (Reps - 1 < 0)
			{
                Reps = 0;
			} else
			{
                Reps = Reps - 1;
			}
        }

		private void OnWeighUp () 
		{
			if (Weight == 0 && TargetWeight > 0)
			{
                Weight = TargetWeight;
			} else
			{
                Weight = Weight + GetStep ();
			}
        }

		private void OnWeighDown () 
		{
            if (Weight == 0 && TargetWeight > 0)
            {
                Weight = TargetWeight;
            }
            else
			if (Weight - GetStep () < 0)
			{
                Weight = 0;
			} else
			{
                Weight = Weight - GetStep ();
			}
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
            if (PreviousReps <= 0) return;
            Reps = PreviousReps;
            Weight = PreviousWeight;
        }

        private void OnTargetIconCommand() 
		{
            if (TargetReps <= 0) return;
            Reps = TargetReps;
            Weight = TargetWeight;
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
                Created = (DateTime) parameters["CurrentDate"];
            }
            if (parameters.ContainsKey("Exercise"))
            {
                Exercise = parameters["Exercise"] as Exercise;
            }
            if (parameters.ContainsKey("RestTimerItem"))
            {
                RestTimerItem = parameters["RestTimerItem"] as RestTimerItem;
            }

            var workout = new Workout();
            if (parameters.ContainsKey("Workout"))
            {
                workout = parameters["Workout"] as Workout;

                if (workout != null)
                {
                    WorkoutId = workout.WorkoutId;
                    Created = workout.Created;
                    Notes = workout.Notes;
                    Reps = workout.Reps;
                    Weight = _units.GetWeight(App.Settings.IsMetric, workout.Weight);
                }
            }

            var previousWorkout = await _workoutsRepository.GetPreviousWorkout(Exercise.ExerciseId, Created);
            if (previousWorkout != null)
            {
                PreviousReps = previousWorkout.Reps;
                PreviousWeight = _units.GetWeight(App.Settings.IsMetric, previousWorkout.Weight);
            }

            var targetWorkout = await _workoutRules.GetTargetWorkout(workout, Exercise, previousWorkout);
            TargetReps = targetWorkout.Key;
            TargetWeight = _units.GetWeight(App.Settings.IsMetric, targetWorkout.Value);
        }
        #endregion
    }
}

