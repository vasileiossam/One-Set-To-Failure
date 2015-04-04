using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;

namespace Set
{
	public partial class WorkoutListPage : ContentPage
	{
        private WorkoutListViewModel _viewModel;
        public WorkoutListViewModel ViewModel
        {
			get
            {
                if (_viewModel == null)
                {
					_viewModel =  new WorkoutListViewModel(){Navigation = this.Navigation};
					_viewModel.Page = this;
                }
				return _viewModel;
            }
			set
            {
				_viewModel = value;
            }
        }

        public WorkoutListPage()
		{
			this.InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();

			this.BindingContext = ViewModel;
			await ViewModel.Load (App.CurrentDate);
			workoutsList.ItemsSource = ViewModel.RoutineDays;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public void OnWorkoutSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((ListView)sender).SelectedItem == null) return;

			var viewModel = new WorkoutViewModel()
            {
                Workout = (e.SelectedItem as RoutineDay).Workout,
				Navigation = this.Navigation
            };

            var workoutPage = new WorkoutPage
            {
                ViewModel = viewModel
            };

            Navigation.PushAsync(workoutPage);

			// deselect row
			((ListView)sender).SelectedItem = null;
        }

		public void OnExercisesButtonClicked(object sender, EventArgs args)
		{
			Navigation.PushAsync( new ExerciseListPage());
		}

		public void OnSettingsButtonClicked(object sender, EventArgs args)
		{
			Navigation.PushAsync( new SettingsPage());
		}

		public void Refresh()
		{
			workoutsList.ItemsSource = null;
			workoutsList.ItemsSource = ViewModel.RoutineDays;
		}
	}
}

