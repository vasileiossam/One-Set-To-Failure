using System.Threading.Tasks;
using OneSet.Abstract;
using OneSet.Models;
using OneSet.Services;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class SettingsPage : SettingsPageXaml
    {
        private readonly IMessagingService _messagingService;

        public SettingsPage(IMessagingService messagingService)
        {
            InitializeComponent();

            _messagingService = messagingService;
            _messagingService.Subscribe<SettingsViewModel>(this, Messages.SettingsReloaded, async sender =>
            {
                await Refresh();
            });
        }

        ~SettingsPage()
        {
            _messagingService.Unsubscribe<SettingsViewModel>(this, Messages.SettingsReloaded);
        }

        protected override async void OnAppearing()
		{
			base.OnAppearing();

            BindingContext = ViewModel;
            settingsList.ItemsSource = ViewModel.Settings;
		}

	    public void OnSettingTapped(object sender, ItemTappedEventArgs args)
		{
            var preference = args.Item as Preference;
		    preference?.Clicked(preference, null);

		    // deselect row
			((ListView)sender).SelectedItem = null;
		}

		public async Task Refresh()
		{
			//await ViewModel.OnLoad();
			settingsList.ItemsSource = null;
			settingsList.ItemsSource = ViewModel.Settings;
		}
	}

    public class SettingsPageXaml : BasePage<SettingsViewModel>
    {
    }
}

