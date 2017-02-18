using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet
{
	public partial class ExercisePage : TabbedPage
	{
		private ExerciseViewModel _viewModel;
		public ExerciseViewModel ViewModel
		{
			get { return _viewModel ?? (_viewModel = new ExerciseViewModel() {Navigation = Navigation}); }
		    set
			{
				_viewModel = value;
			}
		}

		public ExercisePage ()
		{
			InitializeComponent ();
		}
	}
}

