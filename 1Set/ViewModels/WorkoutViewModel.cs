using System;
using SQLite.Net.Attributes;
using Set.Models;
using System.Collections.Generic;
using Set.Resx;
using Xamarin.Forms;
using Toasts.Forms.Plugin.Abstractions;
using Set.Localization;

namespace Set.ViewModels
{
    public class WorkoutViewModel : BaseViewModel
    {
		public Workout Workout {get; set; }

		public string PreviousWeightTitle { get { return AppResources.PreviousWeightTitle + " (" + L10n.GetWeightUnit() + ")"; }}
		public string TargetWeightTitle { get { return AppResources.TargetWeightTitle + " (" + L10n.GetWeightUnit() + ")"; }}

		public WorkoutViewModel () : base()
		{
			Title = AppResources.WorkoutTitle;
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

		protected override void OnSave () 
		{
			if (Validate ())
			{
				App.Database.WorkoutsRepository.Save(Workout);
				App.ShowToast (ToastNotificationType.Success, "Success", AppResources.WorkoutSaved);
			 
				Navigation.PopAsync();
			}
		}
    }
}

