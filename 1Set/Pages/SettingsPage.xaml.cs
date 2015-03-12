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
					_viewModel =  new SettingsViewModel(Navigation);
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
			settingsList.ItemsSource = ViewModel.SettingsItems;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		public void OnSettingSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (((ListView)sender).SelectedItem == null) return;

			var settingItem = e.SelectedItem as SettingItem;
			settingItem.Clicked (settingItem, e);

			// deselect row
			((ListView)sender).SelectedItem = null;
		}

	}
}

