using OneSet.ViewModels;

namespace OneSet.Views
{
	public partial class CalendarNotesPage : CalendarNotesPageXaml
	{
	    public CalendarNotesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = ViewModel;
            await ViewModel.OnLoad();

            CommentEditor.Focus ();
        }
	}

    public class CalendarNotesPageXaml : BasePage<CalendarNotesViewModel>
    {
    }
}

