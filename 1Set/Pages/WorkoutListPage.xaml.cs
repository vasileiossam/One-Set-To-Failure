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
                    _viewModel =  new WorkoutListViewModel();
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
            
            ViewModel.CurrentDate = DateTime.Today;
            this.BindingContext = ViewModel;
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
			workoutsList.ItemsSource = ViewModel.RoutineDays;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public void OnPreviousDateClicked(object sender, EventArgs args)
        {
            ViewModel.CurrentDate.AddDays(-1);
        }

        public void OnNextDateClicked(object sender, EventArgs args)
        {
            ViewModel.CurrentDate.AddDays(1);
        }

        public void OnWorkoutSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((ListView)sender).SelectedItem == null) return;

            var viewModel = new WorkoutViewModel()
            {
                Workout = (e.SelectedItem as RoutineDay).Workout
            };

            var workoutPage = new WorkoutPage
            {
                ViewModel = viewModel
            };

            Navigation.PushAsync(workoutPage);

			// deselect row
			((ListView)sender).SelectedItem = null;
        }

	}
}

