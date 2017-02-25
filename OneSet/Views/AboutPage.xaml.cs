using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class AboutPage : ContentPage
    {
        public AboutPage()
		{
			InitializeComponent();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new AboutViewModel();
        }
    }
}

