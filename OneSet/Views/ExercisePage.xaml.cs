using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class ExercisePage : TabbedPage
	{
		public ExerciseDetailsViewModel ViewModel { get; set; }

		public ExercisePage ()
		{
			InitializeComponent ();
		}
	}
}