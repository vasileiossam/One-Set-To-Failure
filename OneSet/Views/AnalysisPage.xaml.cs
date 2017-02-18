using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet
{
	public partial class AnalysisPage : TabbedPage
	{
		public AnalysisViewModel ViewModel { get; set; }

		public AnalysisPage ()
		{
			InitializeComponent ();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			this.BindingContext = ViewModel;
		}
	}
}

