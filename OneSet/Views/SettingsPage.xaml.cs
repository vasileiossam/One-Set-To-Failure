using System.Threading.Tasks;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class SettingsPage : ContentPage
	{
		protected SettingsViewModel _viewModel;
		public SettingsViewModel ViewModel
		{
			get
			{
			    return _viewModel ?? (_viewModel = new SettingsViewModel
			    {
			        Navigation = this.Navigation,
			        Page = this
			    });
			}
			set
			{
				_viewModel = value;
			}
		}

		public SettingsPage()
		{
			InitializeComponent ();
			BindingContext = ViewModel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await ViewModel.Load();
			settingsList.ItemsSource = ViewModel.Settings;
		}

	    public void OnSettingTapped(object sender, ItemTappedEventArgs e)
		{
			var preference = e.Item as Preference;
		    preference?.Clicked(preference, null);

		    // deselect row
			((ListView)sender).SelectedItem = null;
		}

		public async Task Refresh()
		{
			await ViewModel.Load();
			settingsList.ItemsSource = null;
			settingsList.ItemsSource = ViewModel.Settings;
		}
	}
}

