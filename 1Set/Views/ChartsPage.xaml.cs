using System;
using System.Collections.Generic;
using Set.ViewModels;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using Xamarin.Forms;

namespace Set
{
	public partial class ChartsPage : ContentPage
	{
		public ChartsPage ()
		{
			InitializeComponent ();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing ();
			var viewModel = new ChartsViewModel ();
			await viewModel.Load ();
			BindingContext = viewModel;
		}
	}
}

