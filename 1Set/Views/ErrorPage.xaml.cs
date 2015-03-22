using System;
using System.Collections.Generic;
using Set.ViewModels;
using Xamarin.Forms;

namespace Set.Views
{
	public partial class ErrorPage : ContentPage
	{
		public ErrorViewModel ViewModel { get; set; }

		public ErrorPage ()
		{
			InitializeComponent ();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = ViewModel;
		}

	}
}

