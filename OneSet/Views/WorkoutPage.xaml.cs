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

	    void OnTextChanged(object sender, TextChangedEventArgs  e)
		{
			// fires multiple times and can go to infinite loop
			// example: when in the workout list we have a workout with weight 500.1
			// when we will tap this workout app will crash

//			var entry = sender as Entry;
//			if (entry == null) return;
//			var value = entry.Text;
//
//			// max length = 4
//			if(value.Length > 4)
//			{
//				// Remove Last character 
//				value = value.Remove(value.Length - 1);
//
//				// https://forums.xamarin.com/discussion/44018/removing-and-adding-of-textchanged-event-handler
//				entry.TextChanged -= OnTextChanged;
//				entry.Text = value; 
//				Task.Yield();
//				entry.TextChanged += OnTextChanged;
//
//			}
		}
	}
}

