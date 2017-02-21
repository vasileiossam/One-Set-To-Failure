using System.Threading.Tasks;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class AboutPage : ContentPage
	{
        protected AboutViewModel _viewModel;
        public AboutViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = new AboutViewModel {Navigation = this.Navigation}); }
            set
            {
                _viewModel = value;
            }
        }

        public AboutPage()
		{
			this.InitializeComponent ();
			this.BindingContext = ViewModel;
		}

		protected override async void OnAppearing()
		{
            base.OnAppearing();
            BindingContext = ViewModel;

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}
	}
}

