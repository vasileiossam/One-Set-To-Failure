using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class WorkoutPage : ContentPage
	{
		public WorkoutViewModel ViewModel { get; set; }

		public WorkoutPage ()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

			await ViewModel.LoadAsync ();
			BindingContext = ViewModel;
        }
	}
}

