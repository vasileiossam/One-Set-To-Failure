using System;
using System.Collections.Generic;
using Set.ViewModels;
using Set.Models;
using Xamarin.Forms;
using Set.Resx;
using System.Threading.Tasks;

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
					_viewModel =  new ExerciseListViewModel(){Navigation = this.Navigation};
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

			exercisesList.ItemSelected += async (sender, e) =>
			{
				await OnExerciseSelected (sender, e);
			};
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			this.BindingContext = ViewModel;
			await ViewModel.Load ();
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
					Title = AppResources.AddExerciseTitle,
					Navigation = this.Navigation
				}
			};

			Navigation.PushAsync(exercisePage);
		}

		public async Task OnExerciseSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (((ListView)sender).SelectedItem == null) return;

			var viewModel = e.SelectedItem as ExerciseViewModel;
			viewModel.Navigation = Navigation;
			ViewModel.Title = AppResources.EditExerciseTitle; 
			await viewModel.Load ();

			var page = new ExerciseDetailsPage //ExercisePage
			{
				ViewModel = viewModel
			};

			// TODO replace this with MessagingCenter
			// https://forums.xamarin.com/discussion/22499/looking-to-pop-up-an-alert-like-displayalert-but-from-the-view-model-xamarin-forms-labs
			viewModel.Page = page;

			await Navigation.PushAsync(page);

			// deselect row
			((ListView)sender).SelectedItem = null;
		}
	}
}

