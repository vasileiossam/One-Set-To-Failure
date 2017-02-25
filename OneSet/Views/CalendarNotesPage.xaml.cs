using OneSet.ViewModels;

namespace OneSet.Views
{
	public partial class CalendarNotesPage : CalendarNotesPageXaml
	{
	    public CalendarNotesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = ViewModel;
            CommentEditor.Focus ();
        }
	}

    public class CalendarNotesPageXaml : BasePage<CalendarNotesViewModel>
    {
    }
}

