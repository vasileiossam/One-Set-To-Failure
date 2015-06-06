using System;
using System.Threading.Tasks;
using Set.Resx;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public partial class ExerciseListPage : ContentPage
	{
		private ExerciseListViewModel _viewModel;
		public ExerciseListViewModel ViewModel
		{
			get { return _viewModel ?? (_viewModel = new ExerciseListViewModel() {Navigation = Navigation}); }
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
			var exercisePage = new ExerciseDetailsPage
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
			viewModel.Title = AppResources.EditExerciseTitle; 

			// its already loaded, no need to load again because it will duplicate the PlateWeight value
			//await viewModel.Load ();

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

