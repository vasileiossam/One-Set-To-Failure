using System.Threading.Tasks;
using Xamarin.Forms;

namespace Set
{
	public partial class StatisticsPage : ContentPage
	{
        public StatisticsPage()
		{
			this.InitializeComponent ();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}
	}
}

