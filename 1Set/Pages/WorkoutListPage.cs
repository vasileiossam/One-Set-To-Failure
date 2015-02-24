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
            set
            {
                if (_viewModel == null)
                {
                    _viewModel =  new WorkoutListViewModel();
                }
            }
            get
            {
                return _viewModel;
            }
        }

        public WorkoutListPage()
		{
		//	this.InitializeComponent ();
            
            ViewModel.CurrentDate = DateTime.Today;
            this.BindingContext = ViewModel;
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
          //  workoutsList.ItemsSource = ViewModel.Routines;
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
            var workout = e.SelectedItem as Workout;
            var workoutPage = new WorkoutPage
            {
            //    BindingContext = new WorkoutViewModel(workout)
            };

            Navigation.PushAsync(workoutPage);
        }

	}
}

