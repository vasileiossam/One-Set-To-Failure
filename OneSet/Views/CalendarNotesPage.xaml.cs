using System.Threading.Tasks;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class CalendarNotesPage : ContentPage
	{
        public CalendarNotesViewModel ViewModel { get; set; }

		public CalendarNotesPage()
		{
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;

			CommentEditor.Focus ();

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
        }
	}
}

