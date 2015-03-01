using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;
using Set.Resx;

namespace Set
{
	public partial class ExerciseListPage : ContentPage
	{
		private ExerciseListViewModel _viewModel;
		public ExerciseListViewModel ViewModel
		{
			get
			{
				if (_viewModel == null)
				{
					_viewModel =  new ExerciseListViewModel();
				}
				return _viewModel;
			}
			set
			{
				_viewModel = value;
			}
		}

		public ExerciseListPage ()
		{
			InitializeComponent ();
			this.BindingContext = ViewModel;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			exercisesList.ItemsSource = ViewModel.Exercises;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		public void OnAddExerciseButtonClicked(object sender, EventArgs args)
		{
			var exercisePage = new ExercisePage
			{
				ViewModel = new ExerciseViewModel()
				{
					Title = AppResources.AddExerciseTitle
				}
			};

			Navigation.PushAsync(exercisePage);
		}

		public void OnExerciseSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (((ListView)sender).SelectedItem == null) return;

			var viewModel = new ExerciseViewModel
			{
				Exercise = e.SelectedItem as Exercise,
				Title = AppResources.EditExerciseTitle
			};

			var page = new ExercisePage
			{
				ViewModel = viewModel
			};

			Navigation.PushAsync(page);

			// deselect row
			((ListView)sender).SelectedItem = null;
		}
	}
}

