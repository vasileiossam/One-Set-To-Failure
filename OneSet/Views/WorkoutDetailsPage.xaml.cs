﻿using OneSet.ViewModels;

namespace OneSet.Views
{
	public partial class WorkoutDetailsPage : WorkoutPageXaml
    {
		public WorkoutDetailsPage()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
            ViewModel.RestTimerItem.Update();
        }
	}

    public class WorkoutPageXaml : BasePage<WorkoutDetailsViewModel>
    {
    }
}

