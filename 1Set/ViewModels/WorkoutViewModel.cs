using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Collections.Generic;
using Set.Resx;
using Xamarin.Forms;
using Toasts.Forms.Plugin.Abstractions;
using Set.Localization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Set.ViewModels
{
    public class WorkoutViewModel : BaseViewModel
    {
		public Workout Workout {get; set; }

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

		public string PreviousWeightTitle { get { return AppResources.PreviousWeightTitle + " (" + L10n.GetWeightUnit() + ")"; }}
		public string TargetWeightTitle { get { return AppResources.TargetWeightTitle + " (" + L10n.GetWeightUnit() + ")"; }}

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

		public bool TargetRepsWeightVisible { get { return App.Settings.TargetRepsWeightVisible;} }
		public bool PreviousRepsWeightVisible { get { return App.Settings.PreviousRepsWeightVisible;} }

		#region commands
		public ICommand RepsUpCommand {get; set;}
		public ICommand RepsDownCommand {get; set;}
		public ICommand WeighUpCommand {get; set;}
		public ICommand WeighDownCommand {get; set;}
		#endregion

		public WorkoutViewModel () : base()
		{
			Title = AppResources.WorkoutTitle;

			RepsUpCommand = new Command (async() => { await OnRepsUp(); });
			RepsDownCommand = new Command (async() => { await OnRepsDown(); });
			WeighUpCommand = new Command (async() => { await OnWeighUp(); });
			WeighDownCommand = new Command (async() => { await OnWeighDown(); });
		}

		public async Task Load()
		{
			await Workout.Load ();
			Reps = Workout.Reps;
			Weight = Workout.Weight;
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
			Workout.Weight = Weight;

			if (Validate ())
			{
				await App.Database.WorkoutsRepository.SaveAsync(Workout);

				App.ShowToast (ToastNotificationType.Success, "Success", AppResources.WorkoutSaved);
			 
				await Navigation.PopAsync();
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
				Weight = Workout.TargetWeight;
			} else
			{			
				Weight = Weight + GetStep ();
			}

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		private async Task OnWeighDown () 
		{
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

		private float GetStep()
		{
			var step = Workout.Exercise.PlateWeight;
			if (step <= 0)
				step = 1;
			return step;
		}
		#endregion
    }
}

