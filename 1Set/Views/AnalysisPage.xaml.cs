using Set.ViewModels;
using Xamarin.Forms;

namespace Set
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

