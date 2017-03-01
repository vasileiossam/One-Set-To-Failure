using OneSet.ViewModels;

namespace OneSet.Views
{
	public partial class ExerciseListPage : ExerciseListPageXaml
    {
        public ExerciseListPage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			BindingContext = ViewModel;
			exercisesList.ItemsSource = ViewModel.Exercises;
            exercisesList.SelectedItem = null;
        }
    }

    public class ExerciseListPageXaml : BasePage<ExerciseListViewModel>
    {
    }
}

