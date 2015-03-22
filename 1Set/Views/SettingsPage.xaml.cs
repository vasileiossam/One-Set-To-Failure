using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set
{
	public partial class SettingsPage : ContentPage
	{
		protected SettingsViewModel _viewModel;
		public SettingsViewModel ViewModel
		{
			get
			{
				if (_viewModel == null)
				{
					_viewModel =  new SettingsViewModel(){Navigation = this.Navigation};
					_viewModel.Page = this;
				}
				return _viewModel;
			}
			set
			{
				_viewModel = value;
			}
		}

		public SettingsPage()
		{
			this.InitializeComponent ();
			this.BindingContext = ViewModel;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			settingsList.ItemsSource = ViewModel.Settings;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		public void OnSettingTapped(object sender, ItemTappedEventArgs e)
		{
			var preference = e.Item as Preference;
			preference.Clicked(preference, null);

			// deselect row
			((ListView)sender).SelectedItem = null;
		}

	}
}

