using System.Threading.Tasks;
using System.Windows.Input;
using OneSet.Entities;
using OneSet.Localization;
using OneSet.Models;
using OneSet.Resx;
using Xamarin.Forms;
using Plugin.Toasts;

namespace OneSet.ViewModels
{
    public class WorkoutViewModel : BaseViewModel
    {
		public Workout Workout {get; set; }
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
				if (_reps != value)
				{
					_reps = value;
					OnPropertyChanged("Reps");
				}
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
				if (_weight != value)
				{
					_weight = value;
					OnPropertyChanged("Weight");
				}
			}
		}

		public string PreviousTitle { get { return AppResources.PreviousTitle + " (" + L10n.GetWeightUnit() + ")"; }}
		public string TargetTitle { get { return AppResources.TargetTitle + " (" + L10n.GetWeightUnit() + ")"; }}

		public bool NotesVisible
		{
			get
			{
				if (Workout == null)
					return false;
				if (Workout.Exercise == null)
					return false;

				return !string.IsNullOrEmpty (Workout.Exercise.Notes);
			}
		}

		public bool LevelUpVisible
		{
			get
			{
				if (Workout == null)
					return false;
				return Workout.TargetWeight > Workout.PreviousWeight;
			}
		}

		public bool TargetRepsWeightVisible { get { return App.Settings.TargetRepsWeightVisible;} }
		public bool PreviousRepsWeightVisible { get { return App.Settings.PreviousRepsWeightVisible;} }

        public double ConvertedWeight
        {
            get
            {
                return Units.GetWeight(Workout.Weight);
            }
        }

        public double ConvertedPreviousWeight
        {
            get
            {
                return Units.GetWeight(Workout.PreviousWeight);
            }
        }

        public double ConvertedTargetWeight
        {
            get
            {
                return Units.GetWeight(Workout.TargetWeight);
            }
        }

        #region commands
        public ICommand RepsUpCommand {get; set;}
		public ICommand RepsDownCommand {get; set;}
		public ICommand WeighUpCommand {get; set;}
		public ICommand WeighDownCommand {get; set;}
        public ICommand PreviousIconCommand { get; set; }
        public ICommand TargetIconCommand { get; set; }
		#endregion

		public WorkoutViewModel () : base()
		{
			Title = AppResources.WorkoutTitle;

			RepsUpCommand = new Command (async() => { await OnRepsUp(); });
			RepsDownCommand = new Command (async() => { await OnRepsDown(); });
			WeighUpCommand = new Command (async() => { await OnWeighUp(); });
			WeighDownCommand = new Command (async() => { await OnWeighDown(); });
            PreviousIconCommand = new Command(() => { OnPreviousIconCommand(); });
            TargetIconCommand = new Command(() => { OnTargetIconCommand(); });
		}

		public async Task LoadAsync()
		{
			RestTimerToolbarItem.Update();
		}

		private bool Validate ()
		{
			if (Workout.Reps == 0)
			{
				App.ShowToast (ToastNotificationType.Warning, "Warning", AppResources.WorkoutRepsIsRequired);
				return false;
			}
			if (Workout.Weight == 0)
			{
				App.ShowToast (ToastNotificationType.Warning, "Warning", AppResources.WorkoutWeightIsRequired);
				return false;
			}

			if ((Workout.Reps <= 0) || (Workout.Reps > 100))
			{
				App.ShowToast (ToastNotificationType.Warning, "Warning", AppResources.WorkoutInvalidReps);
				Workout.Reps = 0;
				return false;
			}
			if ((Workout.Weight <= 0) || (Workout.Weight > 1000))
			{
				App.ShowToast (ToastNotificationType.Warning, "Warning", AppResources.WorkoutInvalidWeight);
				Workout.Weight = 0;
				return false;
			}

			return true;
		}

		protected override async Task OnSave () 
		{
			Workout.Reps = Reps;

			if (App.Settings.IsMetric)
			{
				Workout.Weight = Weight;
			} else
			{
				// imperial to metric - always save in metric
				Workout.Weight = Weight / Units.ImperialMetricFactor;
			}

			if (Validate ())
			{
				// invalidate TotalTrophies
				App.TotalTrophies = null;

				Workout.Trophies = WorkoutRules.GetTrophies(Workout);

				var isPersisted = Workout.WorkoutId > 0;

				await App.Database.WorkoutsRepository.SaveAsync(Workout);
				App.ShowToast (ToastNotificationType.Success, "Success", AppResources.WorkoutSaved);

				if ((App.Settings.RestTimerAutoStart == true) && (!isPersisted))
				{
					await Navigation.PopAsync (false);
					await RestTimerToolbarItem.AutoStart ();
				} else
				{
					await Navigation.PopAsync();					
				}
			}
		}

		#region commands
		private async Task OnRepsUp () 
		{
			if ((Reps == 0) && (Workout.TargetReps > 0))
			{
				Reps = Workout.TargetReps;
			} else
			{
				Reps = Reps + 1;
			}

			// following statement will prevent a compiler warning about async method lacking await
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
			if ((Weight == 0) && (Workout.TargetWeight > 0))
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
            if ((Weight == 0) && (Workout.TargetWeight > 0))
            {
                Weight = ConvertedTargetWeight;
            }
            else
			if ((Weight - GetStep ()) < 0)
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
            var step = Units.GetWeight(Workout.Exercise.PlateWeight);
            if (step <= 0)
				step = 1;
			return step;
		}

        private void  OnPreviousIconCommand() 
		{
            //if ((Reps == 0) && (Weight == 0))
            {
                if (Workout.PreviousReps > 0)
                {
                    Reps = Workout.PreviousReps;
                    Weight = ConvertedPreviousWeight;
                }
            }
        }

        private void OnTargetIconCommand() 
		{
            //if ((Reps == 0) && (Weight == 0))
            {
                if (Workout.TargetReps > 0)
                {
                    Reps = Workout.TargetReps;
                    Weight = ConvertedTargetWeight;
                }
            }
        }
		#endregion
    }
}

