using System.Threading.Tasks;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class SettingsPage : SettingsPageXaml
    {
		public SettingsPage()
		{
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

            BindingContext = ViewModel;
            await ViewModel.OnLoad();

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
			await ViewModel.OnLoad();
			settingsList.ItemsSource = null;
			settingsList.ItemsSource = ViewModel.Settings;
		}
	}

    public class SettingsPageXaml : BasePage<SettingsViewModel>
    {
    }
}

