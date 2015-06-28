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

		void OnTextChanged(object sender, TextChangedEventArgs  e)
		{
			var entry = sender as Entry;
			var value = entry.Text;

			// max length = 3
			if(value.Length > 3)
			{
				// Remove Last character 
				value = value.Remove(value.Length - 1);
				// Set the Old value
				entry.Text = value; 
			}
		}
	}
}

