using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using Xamarin.Forms;
using System.Threading.Tasks;
using OneSet.ViewModels;

namespace OneSet
{
	public partial class ChartsPage : ContentPage
	{
		private ChartsViewModel _viewModel;
		public ChartsViewModel ViewModel
		{
			get
			{
				if (_viewModel == null)
				{
					_viewModel = new ChartsViewModel
					{
						Navigation = Navigation,
					};
				}
				return _viewModel;
			}
			set
			{
				_viewModel = value;
			}
		}

		public ChartsPage ()
		{
			InitializeComponent ();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing ();

			ViewModel.OxyPlotsLayout = OxyPlotsLayout;
			await ViewModel.Load ();
			ChartsPicker.SelectedIndex = 0;
		}

		private async void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			var selectedIndex = (sender as Picker).SelectedIndex;
			if (selectedIndex > -1)
			{
				OxyPlotsLayout.Children.Clear ();
				BindingContext = null;
				await ViewModel.SelectChart (selectedIndex);
				BindingContext = ViewModel;
			}
		}
	}
}

