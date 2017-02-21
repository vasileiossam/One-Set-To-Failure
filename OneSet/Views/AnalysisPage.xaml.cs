using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class AnalysisPage : TabbedPage
	{
		public AnalysisViewModel ViewModel { get; set; }

		public AnalysisPage ()
		{
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = ViewModel;
		}
	}
}

