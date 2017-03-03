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
            list.SelectedItem = null;
        }
    }

    public class ExerciseListPageXaml : BasePage<ExerciseListViewModel>
    {

    }
}

