using System.Threading.Tasks;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class ErrorPage : ContentPage
	{
		public ErrorViewModel ViewModel { get; set; }

		public ErrorPage ()
		{
			InitializeComponent ();
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

