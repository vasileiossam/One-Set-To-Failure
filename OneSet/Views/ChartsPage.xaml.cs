using System;
using Autofac;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Views
{
	public partial class ChartsPage : ChartsPageXaml
    {
		public ChartsPage ()
		{
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing ();

            BindingContext = ViewModel;
            ViewModel.OxyPlotsLayout = OxyPlotsLayout;

            ChartsPicker.SelectedIndex = 0;
		}

		private async void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
		    var picker = sender as Picker;
		    if (picker != null)
		    {
		        var selectedIndex = picker.SelectedIndex;
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

    public class ChartsPageXaml : BasePage<ChartsViewModel>
    {
    }
}

