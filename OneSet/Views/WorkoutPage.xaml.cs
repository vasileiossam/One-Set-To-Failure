using OneSet.ViewModels;

namespace OneSet.Views
{
	public partial class WorkoutPage : WorkoutPageXaml
    {
		public WorkoutPage ()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = ViewModel;
            await ViewModel.OnLoad();
        }
	}

    public class WorkoutPageXaml : BasePage<WorkoutViewModel>
    {
    }
}

