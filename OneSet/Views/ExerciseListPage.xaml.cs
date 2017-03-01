using System;
using System.Threading.Tasks;
using Autofac;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Resx;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class ExerciseListPage : ExerciseListPageXaml
    {
        private readonly INavigationService _navigationService;

        public ExerciseListPage (INavigationService navigationService)
		{
			InitializeComponent ();
            _navigationService = navigationService;

            exercisesList.ItemSelected += async (sender, e) =>
			{
				await OnExerciseSelected (sender, e);
			};
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			BindingContext = ViewModel;
			exercisesList.ItemsSource = ViewModel.Exercises;
		}

	    public async Task OnAddExerciseButtonClicked(object sender, EventArgs args)
	    {
            await _navigationService.NavigateTo<ExerciseDetailsViewModel>();
        }

		public async Task OnExerciseSelected(object sender, SelectedItemChangedEventArgs args)
		{
            var item = args.SelectedItem as ExerciseItemViewModel;
            if (item == null) return;

		    var parameters = new NavigationParameters()
		    {
		        {"Title", AppResources.EditExerciseTitle},
                {"Exercise", item.Exercise }
		    };
		    await _navigationService.NavigateTo<ExerciseDetailsViewModel>(parameters);

            // deselect row
            ((ListView)sender).SelectedItem = null;
		}
	}

    public class ExerciseListPageXaml : BasePage<ExerciseListViewModel>
    {
    }
}

