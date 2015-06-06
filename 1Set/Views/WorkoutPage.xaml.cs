using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public partial class WorkoutPage : ContentPage
	{
		public WorkoutViewModel ViewModel { get; set; }

		public WorkoutPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
			await ViewModel.LoadAsync ();
			BindingContext = ViewModel;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

	}
}

